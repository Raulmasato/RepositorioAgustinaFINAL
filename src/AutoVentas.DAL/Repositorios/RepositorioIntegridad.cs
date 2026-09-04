using AutoVentas.DAL.Conexion;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

/// <summary>Fila genérica de una tabla controlada por dígitos verificadores (T08):
/// su clave primaria, los valores de los atributos sensibles (en orden fijo) y el
/// dígito horizontal actualmente almacenado (puede ser null si nunca se calculó).</summary>
public record FilaControlada(int Id, string[] Valores, string? DigitoAlmacenado);

/// <summary>
/// Encapsula, para cada tabla sensible del sistema, cuáles son sus columnas de negocio
/// (usadas para calcular el dígito horizontal) y cómo leer/escribir su columna DigitoVerificador.
/// Mantener esto en un único lugar evita repetir SQL específico de integridad en cada repositorio.
/// </summary>
public class RepositorioIntegridad
{
    private static readonly Dictionary<string, (string Pk, string[] Columnas)> Tablas = new()
    {
        ["Usuarios"] = ("IdUsuario", new[] { "NombreUsuario", "ClaveHash", "ClaveSalt", "IdRol" }),
        ["Clientes"] = ("IdCliente", new[] { "Nombre", "Apellido", "DniEncriptado" }),
        ["Vehiculos"] = ("IdVehiculo", new[] { "Marca", "Modelo", "Color" }),
        ["Mantenimientos"] = ("IdMantenimiento", new[] { "IdVehiculo", "IdCliente", "Servicio", "FechaServicio" }),
        ["Presupuestos"] = ("IdPresupuesto", new[] { "IdVehiculo", "IdCliente", "Monto", "Estado" }),
        ["Contratos"] = ("IdContrato", new[] { "IdVehiculo", "IdCliente", "Precio", "Estado" }),
        ["Reservas"] = ("IdReserva", new[] { "IdVehiculo", "IdCliente", "FechaVencimiento", "Estado" }),
        ["Pagos"] = ("IdPago", new[] { "IdContrato", "Monto", "MetodoPago" }),
        ["Entregas"] = ("IdEntrega", new[] { "IdContrato", "LugarEntrega", "Estado" }),
        ["Reportes"] = ("IdReporte", new[] { "Titulo", "TipoReporte" }),
    };

    public static IReadOnlyCollection<string> ObtenerNombresTablas() => Tablas.Keys;

    public List<FilaControlada> ObtenerFilas(string tabla)
    {
        var (pk, columnas) = Tablas[tabla];
        var listaColumnas = string.Join(", ", columnas);
        var sql = $"SELECT {pk} AS Id, {listaColumnas}, DigitoVerificador FROM {tabla}";

        return SqlHelper.EjecutarConsulta(sql, r =>
        {
            var id = r.GetInt32(r.GetOrdinal("Id"));
            var valores = columnas.Select(c => r.IsDBNull(r.GetOrdinal(c)) ? string.Empty : r[c].ToString() ?? string.Empty).ToArray();
            var digitoOrd = r.GetOrdinal("DigitoVerificador");
            var digito = r.IsDBNull(digitoOrd) ? null : r.GetString(digitoOrd);
            return new FilaControlada(id, valores, digito);
        });
    }

    public void ActualizarDigito(string tabla, int id, string digito)
    {
        var (pk, _) = Tablas[tabla];
        SqlHelper.EjecutarNonQuery(
            $"UPDATE {tabla} SET DigitoVerificador = @digito WHERE {pk} = @id",
            SqlHelper.Param("@digito", digito), SqlHelper.Param("@id", id));
    }
}
