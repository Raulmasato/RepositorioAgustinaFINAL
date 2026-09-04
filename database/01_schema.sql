/*
================================================================================
 Sistema de Venta de Autos - Script de creacion de base de datos (SQL Server)
 Arquitectura: Domain / DAL / BLL / Servicios
 Diseño normalizado (3FN) con integridad referencial.
================================================================================
*/
IF DB_ID('AutoVentasDB') IS NULL
BEGIN
    CREATE DATABASE AutoVentasDB;
END
GO

USE AutoVentasDB;
GO

-- ============================================================
-- T04. Gestion de Perfiles de Usuario (patron Composite)
-- ============================================================
CREATE TABLE Roles (
    IdRol           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre          NVARCHAR(30)  NOT NULL UNIQUE
);
GO

CREATE TABLE Permisos (
    IdPermiso       INT IDENTITY(1,1) PRIMARY KEY,
    Codigo          NVARCHAR(20)  NOT NULL UNIQUE,
    Nombre          NVARCHAR(120) NOT NULL,
    IdPermisoPadre  INT NULL REFERENCES Permisos(IdPermiso)
);
GO

CREATE TABLE RolPermisos (
    IdRol           INT NOT NULL REFERENCES Roles(IdRol),
    IdPermiso       INT NOT NULL REFERENCES Permisos(IdPermiso),
    CONSTRAINT PK_RolPermisos PRIMARY KEY (IdRol, IdPermiso)
);
GO

-- ============================================================
-- T02 / T03. Usuarios (login, hash de clave, encriptado)
-- ============================================================
CREATE TABLE Usuarios (
    IdUsuario           INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario        NVARCHAR(50)  NOT NULL UNIQUE,
    ClaveHash            NVARCHAR(200) NOT NULL,
    ClaveSalt            NVARCHAR(200) NOT NULL,
    IdRol                INT NOT NULL REFERENCES Roles(IdRol),
    Activo               BIT NOT NULL DEFAULT 1,
    FechaCreacion        DATETIME NOT NULL DEFAULT GETDATE(),
    DigitoVerificador    NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Clientes
-- ============================================================
CREATE TABLE Clientes (
    IdCliente           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre              NVARCHAR(60)  NOT NULL,
    Apellido            NVARCHAR(60)  NOT NULL,
    DniEncriptado       NVARCHAR(300) NOT NULL,
    IdUsuario           INT NULL REFERENCES Usuarios(IdUsuario),
    DigitoVerificador   NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Vehiculos
-- ============================================================
CREATE TABLE Vehiculos (
    IdVehiculo          INT IDENTITY(1,1) PRIMARY KEY,
    Marca               NVARCHAR(50) NOT NULL,
    Modelo              NVARCHAR(50) NOT NULL,
    Color               NVARCHAR(30) NOT NULL,
    Anio                INT NULL,
    Precio              DECIMAL(18,2) NULL,
    Disponible          BIT NOT NULL DEFAULT 1,
    DigitoVerificador   NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Mantenimientos (Tecnico)
-- ============================================================
CREATE TABLE Mantenimientos (
    IdMantenimiento     INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo          INT NOT NULL REFERENCES Vehiculos(IdVehiculo),
    IdCliente           INT NOT NULL REFERENCES Clientes(IdCliente),
    Servicio            NVARCHAR(200) NOT NULL,
    FechaServicio       DATETIME NOT NULL,
    DigitoVerificador   NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Presupuestos (Vendedor) -- Contratos <<include>> Presupuestos
-- ============================================================
CREATE TABLE Presupuestos (
    IdPresupuesto       INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo          INT NOT NULL REFERENCES Vehiculos(IdVehiculo),
    IdCliente           INT NOT NULL REFERENCES Clientes(IdCliente),
    IdUsuarioVendedor   INT NOT NULL REFERENCES Usuarios(IdUsuario),
    FechaPresupuesto    DATETIME NOT NULL DEFAULT GETDATE(),
    Monto               DECIMAL(18,2) NOT NULL,
    Estado              NVARCHAR(20) NOT NULL DEFAULT 'Pendiente', -- Pendiente/Aprobado/Rechazado
    DigitoVerificador   NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Contratos (Ejecutivo)
-- ============================================================
CREATE TABLE Contratos (
    IdContrato           INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo           INT NOT NULL REFERENCES Vehiculos(IdVehiculo),
    IdCliente            INT NOT NULL REFERENCES Clientes(IdCliente),
    IdUsuarioEjecutivo   INT NOT NULL REFERENCES Usuarios(IdUsuario),
    IdPresupuesto        INT NULL REFERENCES Presupuestos(IdPresupuesto),
    FechaContrato        DATETIME NOT NULL DEFAULT GETDATE(),
    Precio               DECIMAL(18,2) NOT NULL,
    Estado               NVARCHAR(20) NOT NULL DEFAULT 'Vigente', -- Vigente/Finalizado/Anulado
    DigitoVerificador    NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Reservas (Ejecutivo CRUD completo / Cliente crea y lista las propias)
-- ============================================================
CREATE TABLE Reservas (
    IdReserva            INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo           INT NOT NULL REFERENCES Vehiculos(IdVehiculo),
    IdCliente            INT NOT NULL REFERENCES Clientes(IdCliente),
    IdUsuarioEjecutivo   INT NULL REFERENCES Usuarios(IdUsuario),
    FechaReserva         DATETIME NOT NULL DEFAULT GETDATE(),
    FechaVencimiento     DATETIME NOT NULL,
    Estado               NVARCHAR(20) NOT NULL DEFAULT 'Pendiente', -- Pendiente/Confirmada/Cancelada
    DigitoVerificador    NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Pagos (Ejecutivo) -- Entregas <<include>> Pagos
-- ============================================================
CREATE TABLE Pagos (
    IdPago               INT IDENTITY(1,1) PRIMARY KEY,
    IdContrato           INT NOT NULL REFERENCES Contratos(IdContrato),
    IdUsuarioEjecutivo   INT NOT NULL REFERENCES Usuarios(IdUsuario),
    Monto                DECIMAL(18,2) NOT NULL,
    FechaPago            DATETIME NOT NULL DEFAULT GETDATE(),
    MetodoPago           NVARCHAR(30) NOT NULL,
    DigitoVerificador    NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Entregas (Ejecutivo)
-- ============================================================
CREATE TABLE Entregas (
    IdEntrega            INT IDENTITY(1,1) PRIMARY KEY,
    IdContrato           INT NOT NULL REFERENCES Contratos(IdContrato),
    IdUsuarioEjecutivo   INT NOT NULL REFERENCES Usuarios(IdUsuario),
    FechaEntrega         DATETIME NOT NULL,
    LugarEntrega         NVARCHAR(150) NOT NULL,
    Estado               NVARCHAR(20) NOT NULL DEFAULT 'Pendiente', -- Pendiente/Entregado/Cancelada
    DigitoVerificador    NVARCHAR(80) NULL
);
GO

-- ============================================================
-- Gestion de Reportes (Ejecutivo)
-- ============================================================
CREATE TABLE Reportes (
    IdReporte            INT IDENTITY(1,1) PRIMARY KEY,
    Titulo               NVARCHAR(150) NOT NULL,
    TipoReporte          NVARCHAR(30) NOT NULL, -- Ventas/Mantenimientos/Pagos/Reservas
    FechaDesde           DATETIME NOT NULL,
    FechaHasta           DATETIME NOT NULL,
    Contenido            NVARCHAR(MAX) NULL,
    IdUsuarioEjecutivo   INT NOT NULL REFERENCES Usuarios(IdUsuario),
    FechaGeneracion      DATETIME NOT NULL DEFAULT GETDATE(),
    DigitoVerificador    NVARCHAR(80) NULL,
    PorcentajeCantidad   DECIMAL(5,2) NULL, -- % que la cantidad de registros del periodo representa sobre el total historico
    PorcentajeMonto      DECIMAL(5,2) NULL  -- idem por monto (solo Ventas/Pagos; NULL en Mantenimientos/Reservas)
);
GO

-- ============================================================
-- T06a. Gestion de Bitacora
-- ============================================================
CREATE TABLE Bitacora (
    IdBitacora      BIGINT IDENTITY(1,1) PRIMARY KEY,
    FechaHora       DATETIME NOT NULL DEFAULT GETDATE(),
    IdUsuario       INT NULL REFERENCES Usuarios(IdUsuario),
    Actividad       NVARCHAR(100) NOT NULL,
    Informacion     NVARCHAR(1000) NULL
);
GO

-- ============================================================
-- T06b. Control de cambios (auditoria)
-- ============================================================
CREATE TABLE ControlCambios (
    IdControlCambio  BIGINT IDENTITY(1,1) PRIMARY KEY,
    Tabla            NVARCHAR(50) NOT NULL,
    IdRegistro       INT NOT NULL,
    Campo            NVARCHAR(50) NOT NULL,
    ValorAnterior    NVARCHAR(MAX) NULL,
    ValorNuevo       NVARCHAR(MAX) NULL,
    TipoOperacion    NVARCHAR(20) NOT NULL, -- INSERT/UPDATE/DELETE
    IdUsuario        INT NULL REFERENCES Usuarios(IdUsuario),
    FechaHora        DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- T05. Gestion de Multiples Idiomas (patron Observer, sin resx estaticos)
-- ============================================================
CREATE TABLE Idiomas (
    IdIdioma    INT IDENTITY(1,1) PRIMARY KEY,
    Codigo      NVARCHAR(10) NOT NULL UNIQUE,   -- es, en, pt, fr
    Nombre      NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Traducciones (
    IdTraduccion  INT IDENTITY(1,1) PRIMARY KEY,
    IdIdioma      INT NOT NULL REFERENCES Idiomas(IdIdioma),
    Clave         NVARCHAR(100) NOT NULL,
    Valor         NVARCHAR(400) NOT NULL,
    CONSTRAINT UQ_Traducciones UNIQUE (IdIdioma, Clave)
);
GO

-- ============================================================
-- T07. Gestion de Backup
-- ============================================================
CREATE TABLE Backups (
    IdBackup      INT IDENTITY(1,1) PRIMARY KEY,
    RutaArchivo   NVARCHAR(300) NOT NULL,
    FechaHora     DATETIME NOT NULL DEFAULT GETDATE(),
    IdUsuario     INT NULL REFERENCES Usuarios(IdUsuario),
    Resultado     NVARCHAR(20) NOT NULL, -- Exitoso/Error
    Detalle       NVARCHAR(500) NULL
);
GO

-- ============================================================
-- T08. Digitos verificadores (verticales, uno por tabla controlada)
-- Los horizontales viven en la columna DigitoVerificador de cada tabla.
-- ============================================================
CREATE TABLE DigitosVerticales (
    IdDigitoVertical  INT IDENTITY(1,1) PRIMARY KEY,
    Tabla             NVARCHAR(50) NOT NULL UNIQUE,
    Valor             NVARCHAR(80) NOT NULL,
    FechaCalculo      DATETIME NOT NULL DEFAULT GETDATE()
);
GO
