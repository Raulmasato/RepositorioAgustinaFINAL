using AutoVentas.DAL.Conexion;
using AutoVentas.Domain.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

public class RepositorioIdiomas
{
    public List<Idioma> ObtenerTodos() => SqlHelper.EjecutarConsulta(
        "SELECT IdIdioma, Codigo, Nombre FROM Idiomas ORDER BY Nombre",
        r => new Idioma
        {
            IdIdioma = r.GetInt32(r.GetOrdinal("IdIdioma")),
            Codigo = r.GetString(r.GetOrdinal("Codigo")),
            Nombre = r.GetString(r.GetOrdinal("Nombre"))
        });

    /// <summary>Todas las traducciones (clave -> valor) de un idioma, para cargar en memoria.</summary>
    public Dictionary<string, string> ObtenerTraducciones(string codigoIdioma)
    {
        var filas = SqlHelper.EjecutarConsulta(
            @"SELECT t.Clave, t.Valor FROM Traducciones t
              INNER JOIN Idiomas i ON i.IdIdioma = t.IdIdioma
              WHERE i.Codigo = @codigo",
            r => (Clave: r.GetString(r.GetOrdinal("Clave")), Valor: r.GetString(r.GetOrdinal("Valor"))),
            SqlHelper.Param("@codigo", codigoIdioma));

        return filas.ToDictionary(f => f.Clave, f => f.Valor);
    }

    /// <summary>Da de alta un idioma nuevo y devuelve su Id (T05: incorporar idiomas desde el sistema).</summary>
    public int AgregarIdioma(string codigo, string nombre) => SqlHelper.EjecutarInsertYObtenerId(
        "INSERT INTO Idiomas (Codigo, Nombre) VALUES (@codigo, @nombre)",
        SqlHelper.Param("@codigo", codigo), SqlHelper.Param("@nombre", nombre));

    /// <summary>Todas las claves de traducción conocidas por el sistema (unión de todos los
    /// idiomas), para poder mostrarlas como referencia al cargar un idioma nuevo.</summary>
    public List<string> ObtenerClavesConocidas() => SqlHelper.EjecutarConsulta(
        "SELECT DISTINCT Clave FROM Traducciones ORDER BY Clave",
        r => r.GetString(r.GetOrdinal("Clave")));

    public void GuardarTraduccion(int idIdioma, string clave, string valor) => SqlHelper.EjecutarNonQuery(
        @"MERGE Traducciones AS destino
          USING (SELECT @idIdioma AS IdIdioma, @clave AS Clave) AS origen
          ON destino.IdIdioma = origen.IdIdioma AND destino.Clave = origen.Clave
          WHEN MATCHED THEN UPDATE SET Valor = @valor
          WHEN NOT MATCHED THEN INSERT (IdIdioma, Clave, Valor) VALUES (@idIdioma, @clave, @valor);",
        SqlHelper.Param("@idIdioma", idIdioma),
        SqlHelper.Param("@clave", clave),
        SqlHelper.Param("@valor", valor));
}
