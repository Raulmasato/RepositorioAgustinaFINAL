using System.Text;
using AutoVentas.DAL.Repositorios;
using AutoVentas.Services.Seguridad;

namespace AutoVentas.Services.Integridad;

/// <summary>Resultado de una verificación de integridad de una tabla controlada.</summary>
public record ResultadoVerificacion(string Tabla, bool Integra, List<string> Anomalias);

/// <summary>
/// T08. Gestión de Dígitos Verificadores.
/// Dígito horizontal: uno por fila, calculado a partir del contenido de sus atributos
/// sensibles, incluyendo en el cálculo tanto el valor de cada carácter como su posición
/// dentro del atributo y la posición del atributo dentro de la entidad — así se detectan
/// tanto altas/bajas de datos por fuera del sistema como intercambios de posición.
/// Dígito vertical: uno por tabla, calculado a partir de todos los dígitos horizontales,
/// para detectar filas agregadas o eliminadas por fuera del sistema.
/// Se ejecuta al iniciar la aplicación, antes de mostrar el login (ver Program.cs).
/// </summary>
public class ServicioDigitoVerificador
{
    private readonly RepositorioIntegridad _repositorioIntegridad = new();
    private readonly RepositorioDigitosVerticales _repositorioVertical = new();

    /// <summary>Calcula el dígito horizontal de una fila incluyendo la posición de cada
    /// atributo y de cada carácter dentro de su valor.</summary>
    public string CalcularHorizontal(string[] valoresDeAtributos)
    {
        var constructor = new StringBuilder();
        for (var posicionAtributo = 0; posicionAtributo < valoresDeAtributos.Length; posicionAtributo++)
        {
            var valor = valoresDeAtributos[posicionAtributo] ?? string.Empty;
            for (var posicionCaracter = 0; posicionCaracter < valor.Length; posicionCaracter++)
            {
                constructor.Append(valor[posicionCaracter])
                           .Append(':').Append(posicionCaracter)
                           .Append(':').Append(posicionAtributo)
                           .Append('|');
            }
        }
        return ServicioCriptografia.CalcularSha256Hex(constructor.ToString());
    }

    public string CalcularVertical(IEnumerable<string> digitosHorizontalesOrdenados)
        => ServicioCriptografia.CalcularSha256Hex(string.Join('|', digitosHorizontalesOrdenados));

    /// <summary>Recalcula y persiste el dígito horizontal de cada fila y el vertical de la
    /// tabla completa. Se invoca luego de cada alta/baja/modificación desde el BLL.</summary>
    public void RecalcularYGuardar(string tabla)
    {
        var filas = _repositorioIntegridad.ObtenerFilas(tabla);
        var horizontales = new List<string>();

        foreach (var fila in filas.OrderBy(f => f.Id))
        {
            var digito = CalcularHorizontal(fila.Valores);
            if (digito != fila.DigitoAlmacenado)
            {
                _repositorioIntegridad.ActualizarDigito(tabla, fila.Id, digito);
            }
            horizontales.Add(digito);
        }

        _repositorioVertical.GuardarValor(tabla, CalcularVertical(horizontales));
    }

    /// <summary>Verifica la integridad de todas las tablas controladas. No lanza excepción:
    /// devuelve el detalle de anomalías para que la UI decida cómo informarlas al administrador.</summary>
    public List<ResultadoVerificacion> VerificarIntegridad()
    {
        var resultados = new List<ResultadoVerificacion>();

        foreach (var tabla in RepositorioIntegridad.ObtenerNombresTablas())
        {
            var anomalias = new List<string>();
            var filas = _repositorioIntegridad.ObtenerFilas(tabla).OrderBy(f => f.Id).ToList();
            var horizontalesRecalculados = new List<string>();

            foreach (var fila in filas)
            {
                var digitoRecalculado = CalcularHorizontal(fila.Valores);
                horizontalesRecalculados.Add(digitoRecalculado);

                // Una fila sin dígito almacenado todavía no fue "sellada" (dato recién cargado
                // por el seed inicial); no se considera una anomalía de integridad.
                if (fila.DigitoAlmacenado is not null && fila.DigitoAlmacenado != digitoRecalculado)
                {
                    anomalias.Add($"Fila {fila.Id}: el dígito horizontal no coincide (posible alteración de datos).");
                }
            }

            var verticalAlmacenado = _repositorioVertical.ObtenerValor(tabla);
            var verticalRecalculado = CalcularVertical(horizontalesRecalculados);
            if (verticalAlmacenado is not null && verticalAlmacenado != verticalRecalculado)
            {
                anomalias.Add("El dígito vertical de la tabla no coincide (posibles filas agregadas o eliminadas por fuera del sistema).");
            }

            resultados.Add(new ResultadoVerificacion(tabla, anomalias.Count == 0, anomalias));
        }

        return resultados;
    }

    /// <summary>Recalcula y "sella" (guarda) los dígitos de todas las tablas controladas.
    /// Útil para inicializar la base recién restaurada del seed de datos de ejemplo.</summary>
    public void SellarTodasLasTablas()
    {
        foreach (var tabla in RepositorioIntegridad.ObtenerNombresTablas())
        {
            RecalcularYGuardar(tabla);
        }
    }
}
