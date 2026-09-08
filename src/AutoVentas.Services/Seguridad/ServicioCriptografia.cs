using System.Security.Cryptography;
using System.Text;

namespace AutoVentas.Services.Seguridad;

/// <summary>
/// T03. Gestión de Encriptado.
/// - Hash de contraseñas: PBKDF2-HMAC-SHA256 con salt aleatorio (no reversible).
/// - Encriptado de datos sensibles (ej. DNI): AES simétrico (reversible), para poder
///   mostrarlos nuevamente en la UI cuando el usuario los necesita.
/// </summary>
public static class ServicioCriptografia
{
    private const int IteracionesPbkdf2 = 100_000;
    private const int LargoSaltBytes = 16;
    private const int LargoHashBytes = 32;

    // Clave/IV de AES para el cifrado simétrico de datos sensibles. En un despliegue productivo
    // esta clave debería administrarse con un proveedor externo (KeyVault, DPAPI, etc.); aquí se
    // inicializa desde configuración de la aplicación (ver ConfiguracionCriptografia).
    public static byte[] ClaveAes { get; set; } = SHA256.HashData(Encoding.UTF8.GetBytes("AutoVentas-Clave-Simetrica-Default"));
    public static byte[] IvAes { get; set; } = MD5.HashData(Encoding.UTF8.GetBytes("AutoVentas-IV-Default"));

    // ---------------------------------------------------------------------
    // Hash de contraseñas (irreversible)
    // ---------------------------------------------------------------------
    public static (string Hash, string Salt) HashClave(string claveEnClaro)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(LargoSaltBytes);
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(claveEnClaro, saltBytes, IteracionesPbkdf2, HashAlgorithmName.SHA256, LargoHashBytes);
        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public static bool VerificarClave(string claveEnClaro, string hashAlmacenado, string saltAlmacenado)
    {
        var saltBytes = Convert.FromBase64String(saltAlmacenado);
        var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(claveEnClaro, saltBytes, IteracionesPbkdf2, HashAlgorithmName.SHA256, LargoHashBytes);
        var hashEsperado = Convert.FromBase64String(hashAlmacenado);
        return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
    }

    // ---------------------------------------------------------------------
    // Encriptado simétrico (reversible) para datos sensibles como el DNI
    // ---------------------------------------------------------------------
    public static string Encriptar(string textoPlano)
    {
        if (string.IsNullOrEmpty(textoPlano)) return string.Empty;

        using var aes = Aes.Create();
        aes.Key = ClaveAes;
        aes.IV = IvAes;

        using var encriptador = aes.CreateEncryptor();
        var bytesPlano = Encoding.UTF8.GetBytes(textoPlano);
        var bytesCifrado = encriptador.TransformFinalBlock(bytesPlano, 0, bytesPlano.Length);
        return Convert.ToBase64String(bytesCifrado);
    }

    public static string Desencriptar(string textoCifrado)
    {
        if (string.IsNullOrEmpty(textoCifrado)) return string.Empty;

        using var aes = Aes.Create();
        aes.Key = ClaveAes;
        aes.IV = IvAes;

        using var desencriptador = aes.CreateDecryptor();
        var bytesCifrado = Convert.FromBase64String(textoCifrado);
        var bytesPlano = desencriptador.TransformFinalBlock(bytesCifrado, 0, bytesCifrado.Length);
        return Encoding.UTF8.GetString(bytesPlano);
    }

    // ---------------------------------------------------------------------
    // Hash genérico (usado también por el servicio de dígitos verificadores)
    // ---------------------------------------------------------------------
    public static string CalcularSha256Hex(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes);
    }
}
