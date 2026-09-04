# Sistema de Venta de Autos

Aplicación de escritorio en C# / Windows Forms (.NET 8) para la gestión de venta de
vehículos, con persistencia en SQL Server. Desarrollada en arquitectura de 4 capas
(Domain / DAL / BLL / Servicios) + UI, siguiendo los lineamientos técnicos de la
"Carpeta de Proyecto" (apartado T00 — Aspectos técnicos del sistema).

## Estructura del repositorio

```
database/
  01_schema.sql           Creación de la base de datos y todas las tablas
  02_seed.sql              Roles, permisos (árbol Composite), idiomas/traducciones, usuario admin
  03_datos_prueba.sql      Vehículos de ejemplo (opcional)
  04_actualizacion_permisos_idiomas.sql   Solo si la BD ya existía antes de agregar
                            las pantallas de Idiomas e Historial de cambios
  05_agregar_aleman_italiano.sql          Solo si la BD ya existía antes de agregar
                            alemán e italiano (idempotente)
  06_agregar_porcentajes_reportes.sql     Solo si la BD ya existía antes de agregar
                            las columnas de porcentaje a Reportes (idempotente)
  07_sincronizar_traducciones_y_permisos.sql   Reaplica TODO lo de 04+05+06 de una
                            sola vez (idiomas, permisos, columnas de Reportes y las
                            84 claves de traducción x 6 idiomas). Recomendado si no
                            estás seguro de cuáles de los scripts anteriores corriste.

instalador/
  Instalar.ps1             A01 — instalador automático (ver docs/Manual_Instalacion.md)

docs/
  Manual_Instalacion.md    D01
  Manual_Usuario.md        D03 — guía de uso por rol
  Guia_Mantenimiento.md    D03 — guía de operación/mantenimiento

src/
  AutoVentas.sln
  AutoVentas.Domain/       Entidades, excepciones de dominio, patrón Composite de permisos
  AutoVentas.DAL/          Acceso a datos ADO.NET puro (Microsoft.Data.SqlClient), sin ORM
  AutoVentas.BLL/          Reglas de negocio por gestión (Vehículos, Clientes, Contratos, etc.)
  AutoVentas.Services/     Servicios transversales: seguridad, idioma, bitácora, backup, integridad, ayuda, exportación PDF
  AutoVentas.UI/           Windows Forms (MDI por rol)
```

## Puesta en marcha

**La forma más rápida es el instalador automático** — ver `docs/Manual_Instalacion.md` o
correr `instalador/Instalar.ps1`. A continuación, la puesta en marcha manual:

### 1. Base de datos

Con SQL Server (o SQL Server Express) instalado, ejecutar en orden, con SQLCMD o SSMS:

```
database/01_schema.sql
database/02_seed.sql
database/03_datos_prueba.sql   (opcional, agrega vehículos de ejemplo)
```

Esto crea la base `AutoVentasDB`, sus tablas, los roles/permisos por defecto, las
traducciones (español/inglés/portugués/francés/alemán/italiano) y un usuario **Ejecutivo** inicial:

- Usuario: `admin`
- Contraseña: `Admin123!`

### 2. Cadena de conexión

Editar `src/AutoVentas.UI/App.config` si el servidor no es `localhost\SQLEXPRESS`:

```xml
<connectionStrings>
  <add name="AutoVentasDB"
       connectionString="Server=TU_SERVIDOR;Database=AutoVentasDB;Trusted_Connection=True;TrustServerCertificate=True;" />
</connectionStrings>
```

### 3. Compilar y ejecutar

Requiere Windows y .NET 8 SDK (con el workload de Windows Desktop):

```
cd src
dotnet build AutoVentas.sln
dotnet run --project AutoVentas.UI
```

> Nota: este proyecto usa Windows Forms, por lo que solo compila y corre en Windows.
> No pudo compilarse ni ejecutarse dentro de este entorno de desarrollo (Linux, sin
> SDK de .NET ni WinForms), por lo que el código fue escrito y revisado manualmente
> siguiendo las convenciones del lenguaje; se recomienda compilarlo en un entorno
> Windows antes de la entrega para corregir cualquier detalle de sintaxis.

## Roles y menús

| Rol        | Menú (formularios MDI)                                                        |
|------------|---------------------------------------------------------------------------------|
| Ejecutivo  | Contratos, Reservas, Pagos, Entregas, Reportes, Bitácora, Permisos, Backup, Idiomas, Historial de cambios |
| Vendedor   | Presupuestos, Vehículos, Clientes                                               |
| Técnico    | Mantenimientos                                                                  |
| Cliente    | Catálogo de vehículos (solo lectura + reservar), Mis reservas (crear/listar)     |

Todo usuario, sin importar su rol, ingresa primero a **Login** → **Formulario
principal** (con un botón "Ir a mi menú") → menú MDI correspondiente a su rol.
Desde Login también se puede acceder a **Registro** de un nuevo usuario.

## Mapeo de requisitos técnicos (T00 de la carpeta de proyecto)

| Ítem | Descripción | Dónde está implementado |
|------|-------------|--------------------------|
| T01 | Arquitectura de 4 capas + MDI | `AutoVentas.Domain/DAL/BLL/Services` + `FormMenuRolBase` (MDI) |
| T02 | Login/Logout — patrón Singleton. Arranque/login/apagado diferenciados y auditados | `Services/Seguridad/SesionActual.cs`, `Services/Seguridad/GestorAutenticacion.cs`, `Program.cs` (bitácora en arranque y en `Application.ApplicationExit`) |
| T03 | Encriptado (hash de claves + AES para datos sensibles) | `Services/Seguridad/ServicioCriptografia.cs` |
| T04 | Perfiles de usuario — patrón Composite + TreeView recursivo, **permisos aplicados realmente** (los ítems de menú se ocultan si el rol no tiene el permiso) | `Domain/Permisos/PermisoComponente.cs`, `Services/Permisos/ServicioPermisos.cs`, `UI/Formularios/Ejecutivo/FrmPermisos.cs`, `UI/Formularios/Comunes/FormMenuRolBase.AgregarOpcion(..., codigoPermiso)` |
| T05 | Múltiples idiomas — patrón Observer, sin .resx estáticos, **idiomas y leyendas administrables desde el propio sistema** | `Services/Idioma/GestorIdioma.cs`, tabla `Traducciones`, `UI/Formularios/Ejecutivo/FrmIdiomas.cs` |
| T06a | Bitácora | `Services/Bitacora/ServicioBitacora.cs`, `UI/Formularios/Ejecutivo/FrmBitacora.cs` |
| T06b | Control de cambios (auditoría), con **pantalla para reconstruir el historial de una entidad** | `Services/Bitacora/ServicioControlCambios.cs`, `UI/Formularios/Ejecutivo/FrmHistorialCambios.cs` |
| T07 | Backup | `Services/Backup/ServicioBackup.cs`, `UI/Formularios/Ejecutivo/FrmBackup.cs` |
| T08 | Dígitos verificadores horizontal/vertical | `Services/Integridad/ServicioDigitoVerificador.cs` (se ejecuta al arrancar, antes del login) |
| — | Gestión de excepciones | `Services/Excepciones/ServicioManejoExcepciones.cs` |
| A01 | Instalador | `instalador/Instalar.ps1` — ver `docs/Manual_Instalacion.md` |
| A02 | Informe y exportación en PDF (librería de terceros, sin impresora virtual) | `Services/Reportes/ServicioExportacionPdf.cs` (PDFsharp), botón "Exportar a PDF" en `FrmReportes` |
| A03 | Serialización | `ServicioManejoExcepciones` serializa cada excepción no controlada a XML en `Logs/` |
| D01 | Manual de instalación | `docs/Manual_Instalacion.md` |
| D02 | Ayuda en línea | `Services/Ayuda/ServicioAyuda.cs`, `UI/Formularios/Comunes/FrmAyuda.cs` (menú "Ayuda" en cada pantalla) |
| D03 | Guías de usuario/operación/mantenimiento | `docs/Manual_Usuario.md`, `docs/Guia_Mantenimiento.md` |

## Modelo de datos

Ver `database/01_schema.sql`. Tablas de negocio: `Usuarios`, `Roles`, `Clientes`,
`Vehiculos`, `Mantenimientos`, `Presupuestos`, `Contratos`, `Reservas`, `Pagos`,
`Entregas`, `Reportes`. Tablas técnicas: `Permisos`, `RolPermisos`, `Bitacora`,
`ControlCambios`, `Idiomas`, `Traducciones`, `Backups`, `DigitosVerticales`.

## Limitaciones conocidas / próximos pasos

- El código no pudo compilarse en este entorno (no hay Windows ni .NET SDK
  disponibles). Antes de la entrega final, abrir la solución en Visual Studio /
  `dotnet build` en Windows y corregir cualquier error de compilación remanente.
- La clave de encriptación simétrica (`ServicioCriptografia.ClaveAes`) usa un valor
  por defecto embebido en el código a modo demostrativo; en un despliegue real debería
  administrarse con un mecanismo externo (por ejemplo, DPAPI o un key vault).
- El instalador automático (`instalador/Instalar.ps1`) no pudo probarse en un entorno
  Windows real por la misma razón; revisar su ejecución antes de confiar en él para la
  entrega final, y usar la instalación manual como respaldo (ver
  `docs/Manual_Instalacion.md`).
