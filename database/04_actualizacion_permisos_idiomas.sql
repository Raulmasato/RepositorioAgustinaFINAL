/*
================================================================================
 Script de actualización incremental (solo para bases de datos que ya fueron
 creadas con una versión anterior de 02_seed.sql). Si vas a crear la base de
 datos desde cero, NO hace falta correr este archivo: ya está incluido en
 01_schema.sql + 02_seed.sql.

 Agrega:
   - Permisos AD004 (Gestionar idiomas) y AD005 (Ver historial de cambios),
     necesarios para que el Ejecutivo vea las nuevas pantallas de Idiomas e
     Historial de cambios (quedan incluidos automáticamente en el permiso
     compuesto GE-AD, ya asignado al rol Ejecutivo).
   - Las traducciones (es/en/pt/fr) de esas pantallas nuevas.
================================================================================
*/
USE AutoVentasDB;
GO

IF NOT EXISTS (SELECT 1 FROM Permisos WHERE Codigo = 'AD004')
BEGIN
    DECLARE @idPadreAd INT = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-AD');
    INSERT INTO Permisos (Codigo, Nombre, IdPermisoPadre) VALUES
        ('AD004', 'Gestionar idiomas', @idPadreAd),
        ('AD005', 'Ver historial de cambios', @idPadreAd);
END
GO

DECLARE @es INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'es');
DECLARE @en INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'en');
DECLARE @pt INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'pt');
DECLARE @fr INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'fr');

;WITH Textos AS (
    SELECT * FROM (VALUES
        ('menu.idiomas',          N'Idiomas',              N'Languages',       N'Idiomas',                  N'Langues'),
        ('menu.historialcambios', N'Historial de cambios', N'Change history',  N'Histórico de alterações',  N'Historique des modifications'),
        ('btn.nuevoidioma',       N'Nuevo idioma',         N'New language',    N'Novo idioma',               N'Nouvelle langue'),
        ('msg.idiomaguardado',    N'Las traducciones se guardaron correctamente.', N'The translations were saved successfully.', N'As traduções foram salvas com sucesso.', N'Les traductions ont été enregistrées avec succès.'),
        ('lbl.tabla',             N'Tabla',                N'Table',           N'Tabela',                   N'Table')
    ) AS t(Clave, Es, En, Pt, Fr)
)
MERGE Traducciones AS destino
USING (
    SELECT @es AS IdIdioma, Clave, Es AS Valor FROM Textos
    UNION ALL SELECT @en, Clave, En FROM Textos
    UNION ALL SELECT @pt, Clave, Pt FROM Textos
    UNION ALL SELECT @fr, Clave, Fr FROM Textos
) AS origen
ON destino.IdIdioma = origen.IdIdioma AND destino.Clave = origen.Clave
WHEN MATCHED THEN UPDATE SET Valor = origen.Valor
WHEN NOT MATCHED THEN INSERT (IdIdioma, Clave, Valor) VALUES (origen.IdIdioma, origen.Clave, origen.Valor);
GO
