-- Solo hace falta correr este script si la base de datos ya existía antes de agregar el
-- cuadro de búsqueda por marca/modelo al catálogo de vehículos del Cliente. Es idempotente:
-- se puede ejecutar más de una vez sin error.
USE AutoVentasDB;
GO

DECLARE @es INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'es');
DECLARE @en INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'en');
DECLARE @pt INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'pt');
DECLARE @fr INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'fr');
DECLARE @de INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'de');
DECLARE @it INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'it');

;WITH Textos AS (
    SELECT * FROM (VALUES
        ('lbl.buscarvehiculo', N'Buscar (marca o modelo)', N'Search (brand or model)', N'Buscar (marca ou modelo)', N'Rechercher (marque ou modèle)', N'Suche (Marke oder Modell)', N'Cerca (marca o modello)')
    ) AS t(Clave, Es, En, Pt, Fr, De, It)
)
MERGE Traducciones AS destino
USING (
    SELECT @es AS IdIdioma, Clave, Es AS Valor FROM Textos
    UNION ALL SELECT @en, Clave, En FROM Textos
    UNION ALL SELECT @pt, Clave, Pt FROM Textos
    UNION ALL SELECT @fr, Clave, Fr FROM Textos
    UNION ALL SELECT @de, Clave, De FROM Textos
    UNION ALL SELECT @it, Clave, It FROM Textos
) AS origen
ON destino.IdIdioma = origen.IdIdioma AND destino.Clave = origen.Clave
WHEN MATCHED THEN UPDATE SET Valor = origen.Valor
WHEN NOT MATCHED THEN INSERT (IdIdioma, Clave, Valor) VALUES (origen.IdIdioma, origen.Clave, origen.Valor);
GO
