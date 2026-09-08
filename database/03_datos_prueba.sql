/*
================================================================================
 Datos de prueba opcionales: algunos vehículos de ejemplo para poder operar el
 sistema (reservar, presupuestar, contratar) sin cargar todo a mano.
 Los dígitos verificadores (T08) se calculan y sellan la primera vez que la
 aplicación ejecuta cualquier alta/baja/modificación sobre estas tablas, o
 pueden sellarse manualmente llamando a ServicioDigitoVerificador.SellarTodasLasTablas().
================================================================================
*/
USE AutoVentasDB;
GO

INSERT INTO Vehiculos (Marca, Modelo, Color, Anio, Precio, Disponible) VALUES
    ('Ford', 'Fiesta', 'Rojo', 2020, 8500000, 1),
    ('Ford', 'Focus', 'Gris', 2021, 11200000, 1),
    ('Chevrolet', 'Onix', 'Blanco', 2022, 9800000, 1),
    ('Chevrolet', 'Cruze', 'Negro', 2021, 13500000, 1),
    ('Fiat', 'Cronos', 'Azul', 2023, 10500000, 1),
    ('Fiat', 'Argo', 'Blanco', 2022, 9200000, 1),
    ('Toyota', 'Corolla', 'Gris', 2023, 15800000, 1),
    ('Volkswagen', 'Gol', 'Rojo', 2020, 7600000, 1);
GO
