using AutoVentas.DAL.Conexion;
using Microsoft.Data.SqlClient;

namespace AutoVentas.DAL.Repositorios;

/// <summary>T08. Persistencia del dígito verificador vertical de cada tabla controlada.</summary>
public class RepositorioDigitosVerticales
{
    public string? ObtenerValor(string tabla) => SqlHelper.EjecutarConsultaUno(
        "SELECT Valor FROM DigitosVerticales WHERE Tabla = @tabla",
        r => r.GetString(r.GetOrdinal("Valor")),
        SqlHelper.Param("@tabla", tabla));

    public void GuardarValor(string tabla, string valor) => SqlHelper.EjecutarNonQuery(
        @"MERGE DigitosVerticales AS destino
          USING (SELECT @tabla AS Tabla) AS origen
          ON destino.Tabla = origen.Tabla
          WHEN MATCHED THEN UPDATE SET Valor = @valor, FechaCalculo = GETDATE()
          WHEN NOT MATCHED THEN INSERT (Tabla, Valor) VALUES (@tabla, @valor);",
        SqlHelper.Param("@tabla", tabla),
        SqlHelper.Param("@valor", valor));
}
