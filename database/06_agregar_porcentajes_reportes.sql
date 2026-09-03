-- Solo hace falta correr este script si la base de datos ya existia antes de agregar las
-- columnas de porcentaje (cantidad/monto del periodo vs. total historico) a la tabla Reportes.
-- Es idempotente: se puede ejecutar mas de una vez sin error.
USE AutoVentasDB;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('Reportes') AND name = 'PorcentajeCantidad'
)
    ALTER TABLE Reportes ADD PorcentajeCantidad DECIMAL(5,2) NULL;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('Reportes') AND name = 'PorcentajeMonto'
)
    ALTER TABLE Reportes ADD PorcentajeMonto DECIMAL(5,2) NULL;
GO
