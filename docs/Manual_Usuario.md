# Manual de Usuario — Sistema de Venta de Autos (D03)

## Ingreso al sistema

1. Al abrir la aplicación aparece la pantalla de **Login**.
2. Si ya tenés una cuenta, ingresá tu **usuario** y **contraseña** y presioná **Ingresar**.
3. Si no tenés cuenta, presioná **Registrarse**, completá tus datos y elegí tu rol
   (Cliente, Vendedor, Técnico o Ejecutivo). Si elegís Cliente, además vas a completar tu
   nombre, apellido y DNI.
4. Podés cambiar el idioma de toda la aplicación con el combo y el botón **Traducir**.
5. Tras un login exitoso, se abre el **Menú Principal**, con un botón **"Ir a mi menú"** que te
   lleva a las opciones correspondientes a tu rol.

## Menú del Ejecutivo

Accedé a **Ir a mi menú** para ver, en la barra de menú superior, las siguientes opciones:

- **Contratos**: alta, edición, eliminación y listado de contratos de venta. Un contrato puede
  vincularse (opcionalmente) a un presupuesto ya aprobado.
- **Reservas**: CRUD completo sobre las reservas de vehículos de todos los clientes.
- **Pagos**: registro de pagos asociados a un contrato.
- **Entregas**: coordinación de la entrega física del vehículo de un contrato.
- **Reportes**: generación de reportes (Ventas/Mantenimientos/Pagos/Reservas) por rango de
  fechas, con exportación a PDF.
- **Bitácora**: consulta del historial de actividad de los usuarios del sistema.
- **Permisos**: asignación de permisos a cada rol.
- **Copias de seguridad**: generación y restauración de backups de la base de datos.
- **Idiomas**: alta de idiomas nuevos y edición de las leyendas de la aplicación, sin
  necesidad de tocar la base de datos directamente.
- **Historial de cambios**: elegir una tabla y un Id de registro para ver quién modificó qué
  campo y cuándo.
- **Ayuda**: ayuda contextual de cada funcionalidad.

Nota: cada una de estas opciones solo aparece en el menú si el rol del usuario logueado tiene
el permiso correspondiente asignado (ver "Permisos" más arriba y la Guía de Mantenimiento).

Para dar de alta o editar un registro: seleccioná **Nuevo** o **Editar** (con una fila
seleccionada en la grilla), completá el formulario y presioná **Guardar**. Para eliminar,
seleccioná una fila y presioná **Eliminar** (se pide confirmación).

## Menú del Vendedor

- **Presupuestos**: generación de presupuestos para un cliente sobre un vehículo.
- **Vehículos**: alta y mantenimiento del inventario de vehículos (marca, modelo, color, año,
  precio, disponibilidad).
- **Clientes**: alta y mantenimiento de la base de clientes.

## Menú del Técnico

- **Mantenimientos**: registro de los servicios realizados a los vehículos (service, cambio de
  aceite, arreglo de golpes, etc.), asociados a un vehículo y a un cliente, con fecha.

## Menú del Cliente

- **Vehículos** (catálogo): listado de solo lectura de los vehículos disponibles, con un botón
  **Reservar** para iniciar una reserva sobre el vehículo seleccionado.
- **Reservas** ("Mis reservas"): listado de las reservas propias, con un botón **Nueva
  reserva** para crear una reserva adicional. A diferencia del Ejecutivo, el Cliente solo ve y
  crea sus propias reservas, no las de otros clientes.

## Cambiar de idioma en cualquier momento

Todas las pantallas principales tienen un combo de idioma. Al elegir un idioma distinto, el
cambio se aplica de inmediato a todas las ventanas que tengas abiertas en ese momento.

## Cerrar sesión

Desde el Menú Principal, botón **Cerrar sesión**. Esto vuelve a la pantalla de Login.
