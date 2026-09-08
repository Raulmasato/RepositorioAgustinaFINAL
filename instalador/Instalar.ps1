<#
    ============================================================================
    A01. Instalador del Sistema de Venta de Autos
    ============================================================================
    Automatiza, con la mínima interacción posible del usuario:
      1) Verificar que esté instalado el .NET 8 SDK (motor de ejecución).
      2) Detectar (o instalar, si falta) un motor de SQL Server disponible.
      3) Crear la base de datos, las tablas y los datos iniciales (esquema + seed
         + datos de prueba), instalando así "el esquema de la base de datos con
         datos de prueba" pedido por la especificación.
      4) Configurar automáticamente la cadena de conexión de la aplicación.
      5) Compilar y publicar la aplicación (esto también resuelve/instala,
         mediante NuGet, todas las dependencias de librerías de terceros).
      6) Crear un acceso directo en el Escritorio.

    Uso: abrir PowerShell en esta carpeta y ejecutar:
        .\Instalar.ps1

    Si PowerShell bloquea la ejecución de scripts, ejecutar antes (una sola vez):
        Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
    ============================================================================
#>

$ErrorActionPreference = "Stop"
$raiz = Split-Path -Parent $PSScriptRoot
if (-not (Test-Path (Join-Path $raiz "src\AutoVentas.sln"))) {
    $raiz = Split-Path -Parent $MyInvocation.MyCommand.Path
    $raiz = Split-Path -Parent $raiz
}
$carpetaSrc = Join-Path $raiz "src"
$carpetaDatabase = Join-Path $raiz "database"
$carpetaPublicacion = Join-Path $raiz "publicar"

function Escribir-Titulo($texto) {
    Write-Host ""
    Write-Host "=== $texto ===" -ForegroundColor Cyan
}

function Escribir-Error($texto) {
    Write-Host $texto -ForegroundColor Red
}

# ----------------------------------------------------------------------------
# 1) Verificar .NET 8 SDK
# ----------------------------------------------------------------------------
Escribir-Titulo "Verificando .NET SDK"
$dotnetInstalado = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnetInstalado) {
    Escribir-Error "No se encontró el .NET SDK instalado."
    Write-Host "Descargalo desde: https://dotnet.microsoft.com/download/dotnet/8.0 (elegí 'SDK x64')."
    Write-Host "Después de instalarlo, volvé a ejecutar este script."
    exit 1
}
$versionDotnet = dotnet --version
Write-Host "Detectado .NET SDK $versionDotnet"

# ----------------------------------------------------------------------------
# 2) Detectar (o instalar) un motor de SQL Server
# ----------------------------------------------------------------------------
Escribir-Titulo "Configurando el motor de base de datos"

function Probar-Instancia($servidor) {
    try {
        $sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
        if ($sqlcmd) {
            & sqlcmd -S $servidor -E -Q "SELECT 1" -b *> $null
            return ($LASTEXITCODE -eq 0)
        }
        return $false
    } catch { return $false }
}

$instanciasCandidatas = @(".\SQLEXPRESS", "localhost\SQLEXPRESS", "(localdb)\MSSQLLocalDB", ".")
$servidorDetectado = $null
foreach ($candidata in $instanciasCandidatas) {
    Write-Host "Probando $candidata ..."
    if (Probar-Instancia $candidata) {
        $servidorDetectado = $candidata
        break
    }
}

if (-not $servidorDetectado) {
    Write-Host "No se detectó automáticamente ninguna instancia de SQL Server."
    $winget = Get-Command winget -ErrorAction SilentlyContinue
    if ($winget) {
        Write-Host "Se intentará instalar SQL Server Express LocalDB automáticamente (requiere conexión a internet)..."
        try {
            winget install --id Microsoft.SQLServer.2022.LocalDB -e --accept-source-agreements --accept-package-agreements
            Start-Sleep -Seconds 5
            if (Probar-Instancia "(localdb)\MSSQLLocalDB") {
                $servidorDetectado = "(localdb)\MSSQLLocalDB"
            }
        } catch {
            Write-Host "No se pudo instalar automáticamente." -ForegroundColor Yellow
        }
    }
}

if (-not $servidorDetectado) {
    # Mínima interacción: se le pide al usuario el nombre de su instancia solo si
    # no se pudo resolver de ninguna otra forma.
    $ingresado = Read-Host "Escribí el nombre de tu instancia de SQL Server (Enter para 'localhost\SQLEXPRESS')"
    $servidorDetectado = if ([string]::IsNullOrWhiteSpace($ingresado)) { "localhost\SQLEXPRESS" } else { $ingresado }
}

Write-Host "Usando servidor: $servidorDetectado" -ForegroundColor Green

# ----------------------------------------------------------------------------
# 3) Crear la base de datos, el esquema y los datos iniciales
# ----------------------------------------------------------------------------
Escribir-Titulo "Instalando la base de datos (esquema + datos iniciales + datos de prueba)"

$sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
if (-not $sqlcmd) {
    Escribir-Error "No se encontró 'sqlcmd' en el sistema (viene con SQL Server / SSMS / 'sqlcmd utility')."
    Write-Host "Instalalo desde: https://learn.microsoft.com/sql/tools/sqlcmd-utility"
    Write-Host "o abrí manualmente 01_schema.sql, 02_seed.sql y 03_datos_prueba.sql con SSMS, en ese orden."
    exit 1
}

$scripts = @("01_schema.sql", "02_seed.sql", "03_datos_prueba.sql")
foreach ($script in $scripts) {
    $ruta = Join-Path $carpetaDatabase $script
    Write-Host "Ejecutando $script ..."
    & sqlcmd -S $servidorDetectado -E -i $ruta -b
    if ($LASTEXITCODE -ne 0) {
        Escribir-Error "Falló la ejecución de $script. Revisá el mensaje de arriba."
        exit 1
    }
}
Write-Host "Base de datos instalada correctamente." -ForegroundColor Green

# ----------------------------------------------------------------------------
# 4) Configurar la cadena de conexión de la aplicación
# ----------------------------------------------------------------------------
Escribir-Titulo "Configurando la cadena de conexión"
$appConfig = Join-Path $carpetaSrc "AutoVentas.UI\App.config"
$cadenaNueva = "Server=$servidorDetectado;Database=AutoVentasDB;Trusted_Connection=True;TrustServerCertificate=True;"
$contenido = Get-Content $appConfig -Raw
$contenido = $contenido -replace 'connectionString="[^"]*"', "connectionString=`"$cadenaNueva`""
Set-Content -Path $appConfig -Value $contenido -Encoding UTF8
Write-Host "App.config actualizado con Server=$servidorDetectado"

# ----------------------------------------------------------------------------
# 5) Compilar y publicar la aplicación (resuelve dependencias NuGet de terceros)
# ----------------------------------------------------------------------------
Escribir-Titulo "Compilando y publicando la aplicación"
Push-Location $carpetaSrc
try {
    dotnet restore "AutoVentas.sln"
    dotnet publish "AutoVentas.UI\AutoVentas.UI.csproj" -c Release -o $carpetaPublicacion
} finally {
    Pop-Location
}
Write-Host "Publicado en: $carpetaPublicacion" -ForegroundColor Green

# ----------------------------------------------------------------------------
# 6) Acceso directo en el Escritorio
# ----------------------------------------------------------------------------
Escribir-Titulo "Creando acceso directo"
try {
    $wshShell = New-Object -ComObject WScript.Shell
    $escritorio = [Environment]::GetFolderPath("Desktop")
    $accesoDirecto = $wshShell.CreateShortcut((Join-Path $escritorio "AutoVentas.lnk"))
    $accesoDirecto.TargetPath = Join-Path $carpetaPublicacion "AutoVentas.exe"
    $accesoDirecto.WorkingDirectory = $carpetaPublicacion
    $accesoDirecto.Save()
    Write-Host "Acceso directo creado en el Escritorio."
} catch {
    Write-Host "No se pudo crear el acceso directo (no es crítico)." -ForegroundColor Yellow
}

Escribir-Titulo "Instalación finalizada"
Write-Host "Usuario inicial: admin"
Write-Host "Contraseña inicial: Admin123!"
Write-Host "Podés iniciar la aplicación desde el acceso directo del Escritorio,"
Write-Host "o ejecutando: $carpetaPublicacion\AutoVentas.exe"
