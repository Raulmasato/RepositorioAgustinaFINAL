# Guía de Mantenimiento — Sistema de Venta de Autos (D03)

Dirigida al administrador/Ejecutivo responsable de mantener la base de datos y el sistema en
funcionamiento.

## Copias de seguridad (T07)

Desde el menú Ejecutivo → **Copias de seguridad**:

- **Generar backup**: crea un archivo `.bak` con una copia completa de `AutoVentasDB` mediante
  `BACKUP DATABASE`, y lo registra en el catálogo (tabla `Backups`).
- **Restaurar**: selecciona un backup del catálogo y ejecuta `RESTORE DATABASE ... WITH
  REPLACE`, reemplazando el estado actual de la base de datos por el del backup elegido.

**Recomendación:** generar un backup antes de cualquier operación masiva de datos o antes de
actualizar el sistema a una nueva versión.

## Verificación de integridad (T08)

Al iniciar la aplicación, antes de mostrar el login, el sistema recalcula los dígitos
verificadores horizontales (por fila) y verticales (por tabla) de las tablas sensibles
(`Usuarios`, `Clientes`, `Vehiculos`, `Mantenimientos`, `Presupuestos`, `Contratos`,
`Reservas`, `Pagos`, `Entregas`, `Reportes`) y los compara contra los valores guardados.

Si aparece un cartel de "Control de integridad" con anomalías, significa que los datos de esa
tabla fueron modificados **por fuera del sistema** (por ejemplo, editando directamente en SSMS)
o que se agregaron/eliminaron filas sin pasar por la aplicación. En ese caso:

1. Revisar qué se modificó fuera del sistema.
2. Si el cambio fue intencional y válido, dejar que el sistema "selle" de nuevo los dígitos: la
   forma más simple es hacer una modificación mínima (por ejemplo, reabrir y volver a guardar
   cada fila afectada) desde la aplicación, lo que recalcula automáticamente su dígito.
3. Si el cambio no fue autorizado, restaurar el último backup válido.

## Bitácora y control de cambios (T06)

- La **Bitácora** (menú Ejecutivo) muestra qué usuario hizo qué operación y cuándo. Es de solo
  lectura y no requiere mantenimiento, pero conviene revisarla periódicamente para auditoría.
- El **Control de cambios** (tabla `ControlCambios`, sin pantalla propia) guarda, campo por
  campo, el valor anterior y nuevo de cada modificación, para poder reconstruir el historial
  completo de una entidad si hiciera falta. Se consulta directamente por SQL cuando se necesita
  investigar un caso puntual:

  ```sql
  SELECT * FROM ControlCambios
  WHERE Tabla = 'Vehiculos' AND IdRegistro = 5
  ORDER BY FechaHora DESC;
  ```

## Permisos por rol (T04)

Desde el menú Ejecutivo → **Permisos**: elegir un rol en el combo, tildar/destildar los
permisos del árbol (tildar un permiso compuesto tilda automáticamente todos sus permisos hijos)
y presionar **Guardar**. Los cambios se aplican de inmediato.

## Registro de excepciones (gestión de excepciones)

Toda excepción no controlada que ocurra en la aplicación se guarda como un archivo XML
serializado en la carpeta `Logs/` (junto al ejecutable), además de intentar registrarse en la
Bitácora. Si un usuario reporta un comportamiento inesperado, revisar los archivos más
recientes de esa carpeta para ver el detalle técnico del error.

## Multi-idioma (T05)

Las traducciones viven en las tablas `Idiomas` y `Traducciones`, no en archivos del programa.
Para agregar un idioma nuevo o corregir una traducción, se puede hacer directamente por SQL:

```sql
-- Agregar un idioma nuevo
INSERT INTO Idiomas (Codigo, Nombre) VALUES ('it', 'Italiano');

-- Agregar/corregir una traducción puntual
DECLARE @idIdioma INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'it');
MERGE Traducciones AS destino
USING (SELECT @idIdioma AS IdIdioma, 'btn.guardar' AS Clave) AS origen
ON destino.IdIdioma = origen.IdIdioma AND destino.Clave = origen.Clave
WHEN MATCHED THEN UPDATE SET Valor = N'Salva'
WHEN NOT MATCHED THEN INSERT (IdIdioma, Clave, Valor) VALUES (@idIdioma, 'btn.guardar', N'Salva');
```

Los formularios que estén abiertos en ese momento no van a ver el idioma nuevo hasta que se
reinicie la aplicación (recién ahí `GestorIdioma` vuelve a leer la lista de idiomas
disponibles), pero las traducciones de un idioma ya existente se ven reflejadas apenas se
vuelve a elegir ese idioma desde el combo.
