# Manual de Instalación — Sistema de Venta de Autos (D01)

Este documento explica cómo instalar el sistema desde cero. Hay dos caminos: instalación
**automática** (script) o **manual** (paso a paso). Se recomienda el automático.

## Requisitos previos

- Windows 10/11.
- [.NET 8 SDK (x64)](https://dotnet.microsoft.com/download/dotnet/8.0) — necesario para compilar y ejecutar la aplicación.
- Un motor de **SQL Server** accesible: SQL Server Express, SQL Server Developer, LocalDB, o
  una instancia ya existente. Si no tenés ninguno, el instalador automático intenta instalar
  SQL Server Express LocalDB por vos (necesita conexión a internet y `winget`).
- Visual Studio 2022 (opcional, solo si vas a abrir/editar el código; no es necesario para
  correr la aplicación ya publicada).

## Opción A — Instalación automática (recomendada)

1. Descomprimí el proyecto en una carpeta de tu elección.
2. Abrí PowerShell **dentro de la carpeta `instalador`** del proyecto.
3. Si es la primera vez que ejecutás scripts de PowerShell en tu usuario, corré una sola vez:
   ```powershell
   Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
   ```
4. Ejecutá:
   ```powershell
   .\Instalar.ps1
   ```
5. El script, con mínima interacción de tu parte (solo te va a preguntar el nombre de tu
   instancia de SQL Server si no puede detectarla solo):
   - Verifica que tengas el .NET SDK instalado.
   - Detecta (o instala) un motor de SQL Server.
   - Crea la base de datos `AutoVentasDB`, sus tablas, y carga los datos iniciales (roles,
     permisos, idiomas, usuario administrador) y datos de prueba (vehículos de ejemplo).
   - Ajusta automáticamente la cadena de conexión de la aplicación (`App.config`).
   - Compila y publica la aplicación, resolviendo automáticamente (vía NuGet) todas las
     dependencias de librerías de terceros (Microsoft.Data.SqlClient, PDFsharp, etc.).
   - Crea un acceso directo en el Escritorio.
6. Al finalizar, iniciá la aplicación desde el acceso directo del Escritorio, o ejecutando
   `publicar\AutoVentas.exe`.

**Usuario inicial:** `admin` — **Contraseña inicial:** `Admin123!`

## Opción B — Instalación manual

### 1. Base de datos

Con SSMS (SQL Server Management Studio), Azure Data Studio, o el "Explorador de objetos de
SQL Server" de Visual Studio, conectate a tu servidor y ejecutá, **en este orden exacto**, los
archivos de la carpeta `database/`:

1. `01_schema.sql` — crea la base de datos y todas las tablas.
2. `02_seed.sql` — carga roles, permisos, idiomas/traducciones y el usuario `admin`.
3. `03_datos_prueba.sql` — carga vehículos de ejemplo (opcional pero recomendado).

### 2. Cadena de conexión

Editá `src/AutoVentas.UI/App.config` y ajustá el valor de `Server=` para que coincida con el
nombre real de tu instancia de SQL Server:

```xml
<connectionStrings>
  <add name="AutoVentasDB"
       connectionString="Server=TU_SERVIDOR;Database=AutoVentasDB;Trusted_Connection=True;TrustServerCertificate=True;" />
</connectionStrings>
```

Para saber el nombre de tu instancia: en Visual Studio, `Ver` → `Explorador de objetos de SQL
Server`, ahí aparece listado tu servidor.

### 3. Compilar y ejecutar

```
cd src
dotnet build AutoVentas.sln
dotnet run --project AutoVentas.UI
```

O bien, abrir `src/AutoVentas.sln` en Visual Studio, marcar `AutoVentas.UI` como proyecto de
inicio (clic derecho → "Establecer como proyecto de inicio") y presionar F5.

## Solución de problemas comunes

| Problema | Causa habitual | Solución |
|---|---|---|
| "Un proyecto con un tipo de salida de biblioteca de clases no se puede iniciar directamente" | El proyecto de inicio seleccionado no es `AutoVentas.UI` | En el desplegable de la barra de herramientas (al lado del botón ▶), elegí `AutoVentas.UI`. |
| El "diseñador" de Visual Studio no muestra nada, o tarda mucho y da timeout | Es una limitación conocida del diseñador visual de Windows Forms con proyectos .NET 8 y código escrito a mano | No afecta a la aplicación en ejecución. Usá F5 para correr el programa real; no hace falta el diseñador para nada. |
| `Cannot open database "AutoVentasDB" requested by the login` | La base de datos todavía no fue creada | Ejecutar los pasos de la sección "Base de datos" (arriba). |
| `You must install or update .NET to run this application` | Falta el .NET 8 Desktop Runtime (x64) | Instalar el SDK de https://dotnet.microsoft.com/download/dotnet/8.0 (versión x64, no x86/ARM64). |
| Al compilar aparece un error de tipo "X es un espacio de nombres pero se usa como tipo" | Colisión de nombres entre una carpeta/namespace y una clase | Ya corregido en la versión actual del proyecto; si aparece en código nuevo, renombrar el namespace en conflicto. |
