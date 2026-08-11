/*
================================================================================
 Datos iniciales: roles, permisos (arbol Composite), idiomas/traducciones
 y un usuario Ejecutivo por defecto para el primer ingreso.
================================================================================
*/
USE AutoVentasDB;
GO

-- ============================================================
-- Roles
-- ============================================================
INSERT INTO Roles (Nombre) VALUES
    ('Cliente'), ('Vendedor'), ('Tecnico'), ('Ejecutivo');
GO

-- ============================================================
-- T04. Permisos atomicos y compuestos (patron Composite)
-- ============================================================
INSERT INTO Permisos (Codigo, Nombre, IdPermisoPadre) VALUES
    -- Atomicos: Vehiculos
    ('VE001', 'Crear vehiculo', NULL),
    ('VE002', 'Modificar vehiculo', NULL),
    ('VE003', 'Eliminar vehiculo', NULL),
    ('VE004', 'Listar vehiculos', NULL),
    -- Atomicos: Clientes
    ('CL001', 'Crear cliente', NULL),
    ('CL002', 'Modificar cliente', NULL),
    ('CL003', 'Eliminar cliente', NULL),
    ('CL004', 'Listar clientes', NULL),
    -- Atomicos: Presupuestos
    ('PR001', 'Crear presupuesto', NULL),
    ('PR002', 'Modificar presupuesto', NULL),
    ('PR003', 'Eliminar presupuesto', NULL),
    ('PR004', 'Listar presupuestos', NULL),
    -- Atomicos: Contratos
    ('CO001', 'Crear contrato', NULL),
    ('CO002', 'Modificar contrato', NULL),
    ('CO003', 'Eliminar contrato', NULL),
    ('CO004', 'Listar contratos', NULL),
    -- Atomicos: Reservas
    ('RE001', 'Crear reserva', NULL),
    ('RE002', 'Modificar reserva', NULL),
    ('RE003', 'Eliminar reserva', NULL),
    ('RE004', 'Listar reservas', NULL),
    -- Atomicos: Pagos
    ('PA001', 'Crear pago', NULL),
    ('PA002', 'Modificar pago', NULL),
    ('PA003', 'Eliminar pago', NULL),
    ('PA004', 'Listar pagos', NULL),
    -- Atomicos: Entregas
    ('EN001', 'Crear entrega', NULL),
    ('EN002', 'Modificar entrega', NULL),
    ('EN003', 'Eliminar entrega', NULL),
    ('EN004', 'Listar entregas', NULL),
    -- Atomicos: Reportes
    ('RP001', 'Crear reporte', NULL),
    ('RP002', 'Modificar reporte', NULL),
    ('RP003', 'Eliminar reporte', NULL),
    ('RP004', 'Listar reportes', NULL),
    -- Atomicos: Mantenimientos
    ('MA001', 'Crear mantenimiento', NULL),
    ('MA002', 'Modificar mantenimiento', NULL),
    ('MA003', 'Eliminar mantenimiento', NULL),
    ('MA004', 'Listar mantenimientos', NULL),
    -- Atomicos: administracion
    ('AD001', 'Ver bitacora', NULL),
    ('AD002', 'Gestionar permisos', NULL),
    ('AD003', 'Gestionar backups', NULL);
GO

-- Compuestos de primer nivel (agrupan atomicos por gestion)
INSERT INTO Permisos (Codigo, Nombre, IdPermisoPadre) VALUES
    ('GE-VE', 'Gestion de vehiculos', NULL),
    ('GE-CL', 'Gestion de clientes', NULL),
    ('GE-PR', 'Gestion de presupuestos', NULL),
    ('GE-CO', 'Gestion de contratos', NULL),
    ('GE-RE', 'Gestion de reservas', NULL),
    ('GE-PA', 'Gestion de pagos', NULL),
    ('GE-EN', 'Gestion de entregas', NULL),
    ('GE-RP', 'Gestion de reportes', NULL),
    ('GE-MA', 'Gestion de mantenimientos', NULL),
    ('GE-AD', 'Gestion administrativa', NULL);
GO

UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-VE') WHERE Codigo IN ('VE001','VE002','VE003','VE004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-CL') WHERE Codigo IN ('CL001','CL002','CL003','CL004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-PR') WHERE Codigo IN ('PR001','PR002','PR003','PR004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-CO') WHERE Codigo IN ('CO001','CO002','CO003','CO004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-RE') WHERE Codigo IN ('RE001','RE002','RE003','RE004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-PA') WHERE Codigo IN ('PA001','PA002','PA003','PA004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-EN') WHERE Codigo IN ('EN001','EN002','EN003','EN004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-RP') WHERE Codigo IN ('RP001','RP002','RP003','RP004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-MA') WHERE Codigo IN ('MA001','MA002','MA003','MA004');
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-AD') WHERE Codigo IN ('AD001','AD002','AD003');
GO

-- Permiso raiz "Administrador" que agrupa toda la gestion administrativa + reportes
INSERT INTO Permisos (Codigo, Nombre, IdPermisoPadre) VALUES ('AA099', 'Administrador general', NULL);
GO

-- Asignacion Rol -> Permiso (perfil = permiso compuesto asociado al rol)
-- Ejecutivo: contratos, reservas, pagos, entregas, reportes + administracion
INSERT INTO RolPermisos (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso FROM Roles r, Permisos p
WHERE r.Nombre = 'Ejecutivo' AND p.Codigo IN ('GE-CO','GE-RE','GE-PA','GE-EN','GE-RP','GE-AD');
GO

-- Vendedor: presupuestos, vehiculos, clientes
INSERT INTO RolPermisos (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso FROM Roles r, Permisos p
WHERE r.Nombre = 'Vendedor' AND p.Codigo IN ('GE-PR','GE-VE','GE-CL');
GO

-- Tecnico: mantenimientos
INSERT INTO RolPermisos (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso FROM Roles r, Permisos p
WHERE r.Nombre = 'Tecnico' AND p.Codigo IN ('GE-MA');
GO

-- Cliente: reservas (crear/listar las propias) y consulta de vehiculos
INSERT INTO RolPermisos (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso FROM Roles r, Permisos p
WHERE r.Nombre = 'Cliente' AND p.Codigo IN ('RE001','RE004','VE004');
GO

-- ============================================================
-- T05. Idiomas y traducciones (sin resx estaticos, todo en BD)
-- ============================================================
INSERT INTO Idiomas (Codigo, Nombre) VALUES
    ('es', 'Español'), ('en', 'English'), ('pt', 'Português'), ('fr', 'Français');
GO

DECLARE @es INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'es');
DECLARE @en INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'en');
DECLARE @pt INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'pt');
DECLARE @fr INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'fr');

;WITH Textos AS (
    SELECT * FROM (VALUES
        ('lbl.usuario',       N'Usuario',                N'User',                 N'Usuário',              N'Utilisateur'),
        ('lbl.clave',         N'Contraseña',              N'Password',             N'Senha',                N'Mot de passe'),
        ('btn.ingresar',      N'Ingresar',                N'Log in',               N'Entrar',               N'Se connecter'),
        ('btn.registrarse',   N'Registrarse',             N'Register',             N'Registrar-se',         N'S''inscrire'),
        ('btn.cancelar',      N'Cancelar',                N'Cancel',               N'Cancelar',             N'Annuler'),
        ('btn.guardar',       N'Guardar',                 N'Save',                 N'Salvar',               N'Enregistrer'),
        ('btn.nuevo',         N'Nuevo',                   N'New',                  N'Novo',                 N'Nouveau'),
        ('btn.editar',        N'Editar',                  N'Edit',                 N'Editar',               N'Modifier'),
        ('btn.eliminar',      N'Eliminar',                N'Delete',               N'Excluir',              N'Supprimer'),
        ('btn.refrescar',     N'Refrescar',               N'Refresh',              N'Atualizar',            N'Actualiser'),
        ('btn.cerrarsesion',  N'Cerrar sesión',           N'Log out',              N'Sair',                 N'Se déconnecter'),
        ('btn.iralmenu',      N'Ir a mi menú',            N'Go to my menu',        N'Ir ao meu menu',       N'Aller à mon menu'),
        ('frm.login',         N'Inicio de sesión',        N'Login',                N'Login',                N'Connexion'),
        ('frm.registro',      N'Registro de usuario',     N'User registration',    N'Registro de usuário',  N'Inscription utilisateur'),
        ('frm.principal',     N'Sistema de Venta de Autos', N'Car Sales System',   N'Sistema de Venda de Carros', N'Système de Vente de Voitures'),
        ('frm.menuejecutivo', N'Menú Ejecutivo',          N'Executive Menu',       N'Menu Executivo',       N'Menu Exécutif'),
        ('frm.menuvendedor',  N'Menú Vendedor',           N'Salesperson Menu',     N'Menu Vendedor',        N'Menu Vendeur'),
        ('frm.menutecnico',   N'Menú Técnico',            N'Technician Menu',      N'Menu Técnico',         N'Menu Technicien'),
        ('frm.menucliente',   N'Menú Cliente',            N'Customer Menu',        N'Menu Cliente',         N'Menu Client'),
        ('menu.contratos',    N'Contratos',               N'Contracts',            N'Contratos',            N'Contrats'),
        ('menu.presupuestos', N'Presupuestos',            N'Quotes',               N'Orçamentos',           N'Devis'),
        ('menu.reservas',     N'Reservas',                N'Reservations',         N'Reservas',             N'Réservations'),
        ('menu.pagos',        N'Pagos',                   N'Payments',             N'Pagamentos',           N'Paiements'),
        ('menu.entregas',     N'Entregas',                N'Deliveries',           N'Entregas',             N'Livraisons'),
        ('menu.reportes',     N'Reportes',                N'Reports',              N'Relatórios',           N'Rapports'),
        ('menu.vehiculos',    N'Vehículos',               N'Vehicles',             N'Veículos',             N'Véhicules'),
        ('menu.clientes',     N'Clientes',                N'Customers',            N'Clientes',             N'Clients'),
        ('menu.mantenimientos', N'Mantenimientos',        N'Maintenance',          N'Manutenções',          N'Entretiens'),
        ('menu.bitacora',     N'Bitácora',                N'Audit log',            N'Log de auditoria',     N'Journal d''audit'),
        ('menu.permisos',     N'Permisos',                N'Permissions',          N'Permissões',           N'Autorisations'),
        ('menu.backup',       N'Copias de seguridad',     N'Backups',              N'Cópias de segurança',  N'Sauvegardes'),
        ('menu.idioma',       N'Idioma',                  N'Language',             N'Idioma',               N'Langue'),
        ('btn.volver',        N'Volver',                  N'Back',                 N'Voltar',               N'Retour'),
        ('btn.buscar',        N'Buscar',                  N'Search',               N'Buscar',               N'Rechercher'),
        ('btn.reservar',      N'Reservar',                N'Reserve',              N'Reservar',             N'Réserver'),
        ('btn.nuevareserva',  N'Nueva reserva',           N'New reservation',      N'Nova reserva',         N'Nouvelle réservation'),
        ('btn.generarbackup', N'Generar backup',          N'Generate backup',      N'Gerar backup',         N'Générer une sauvegarde'),
        ('btn.restaurar',     N'Restaurar',               N'Restore',              N'Restaurar',            N'Restaurer'),
        ('lbl.confirmarclave', N'Confirmar contraseña',   N'Confirm password',     N'Confirmar senha',      N'Confirmer le mot de passe'),
        ('lbl.rol',           N'Rol',                     N'Role',                 N'Função',               N'Rôle'),
        ('lbl.nombre',        N'Nombre',                  N'First name',           N'Nome',                 N'Prénom'),
        ('lbl.apellido',      N'Apellido',                N'Last name',            N'Sobrenome',            N'Nom'),
        ('lbl.dni',           N'DNI',                     N'National ID',          N'RG/CPF',               N'Pièce d''identité'),
        ('lbl.marca',         N'Marca',                   N'Brand',                N'Marca',                N'Marque'),
        ('lbl.modelo',        N'Modelo',                  N'Model',                N'Modelo',               N'Modèle'),
        ('lbl.color',         N'Color',                   N'Color',                N'Cor',                  N'Couleur'),
        ('lbl.anio',          N'Año',                     N'Year',                 N'Ano',                  N'Année'),
        ('lbl.precio',        N'Precio',                  N'Price',                N'Preço',                N'Prix'),
        ('lbl.disponible',    N'Disponible',              N'Available',            N'Disponível',           N'Disponible'),
        ('lbl.vehiculo',      N'Vehículo',                N'Vehicle',              N'Veículo',              N'Véhicule'),
        ('lbl.cliente',       N'Cliente',                 N'Customer',             N'Cliente',              N'Client'),
        ('lbl.monto',         N'Monto',                   N'Amount',               N'Valor',                N'Montant'),
        ('lbl.estado',        N'Estado',                  N'Status',               N'Situação',             N'État'),
        ('lbl.servicio',      N'Servicio',                N'Service',              N'Serviço',              N'Service'),
        ('lbl.fecha',         N'Fecha',                   N'Date',                 N'Data',                 N'Date'),
        ('lbl.vencimiento',   N'Vencimiento',             N'Expiration',           N'Vencimento',           N'Expiration'),
        ('lbl.metodopago',    N'Método de pago',          N'Payment method',       N'Forma de pagamento',   N'Mode de paiement'),
        ('lbl.lugar',         N'Lugar',                   N'Location',             N'Local',                N'Lieu'),
        ('lbl.titulo',        N'Título',                  N'Title',                N'Título',               N'Titre'),
        ('lbl.tipo',          N'Tipo',                    N'Type',                 N'Tipo',                 N'Type'),
        ('lbl.desde',         N'Desde',                   N'From',                 N'De',                   N'Depuis'),
        ('lbl.hasta',         N'Hasta',                   N'To',                   N'Até',                  N'Jusqu''à'),
        ('lbl.actividad',     N'Actividad',               N'Activity',             N'Atividade',            N'Activité'),
        ('msg.registroexitoso', N'Usuario registrado correctamente.', N'User registered successfully.', N'Usuário registrado com sucesso.', N'Utilisateur enregistré avec succès.'),
        ('msg.clavesnocoinciden', N'Las contraseñas no coinciden.', N'Passwords do not match.', N'As senhas não coincidem.', N'Les mots de passe ne correspondent pas.'),
        ('msg.confirmareliminar', N'¿Confirma que desea eliminar el registro seleccionado?', N'Confirm you want to delete the selected record?', N'Confirma que deseja excluir o registro selecionado?', N'Confirmez-vous la suppression de l''enregistrement sélectionné ?'),
        ('msg.seleccionevehiculocliente', N'Debe seleccionar un vehículo y un cliente.', N'You must select a vehicle and a customer.', N'Selecione um veículo e um cliente.', N'Vous devez sélectionner un véhicule et un client.'),
        ('msg.completetodosloscampos', N'Debe completar todos los campos.', N'You must complete all fields.', N'Preencha todos os campos.', N'Vous devez remplir tous les champs.'),
        ('msg.permisosguardados', N'Los permisos del rol se guardaron correctamente.', N'Role permissions were saved successfully.', N'As permissões da função foram salvas com sucesso.', N'Les autorisations du rôle ont été enregistrées avec succès.'),
        ('msg.backupgenerado', N'El backup se generó correctamente.', N'The backup was generated successfully.', N'O backup foi gerado com sucesso.', N'La sauvegarde a été générée avec succès.'),
        ('msg.confirmarrestaurar', N'¿Confirma que desea restaurar este backup? Se reemplazará la base de datos actual.', N'Confirm you want to restore this backup? The current database will be replaced.', N'Confirma que deseja restaurar este backup? O banco de dados atual será substituído.', N'Confirmez-vous la restauration de cette sauvegarde ? La base de données actuelle sera remplacée.'),
        ('msg.backuprestaurado', N'El backup se restauró correctamente.', N'The backup was restored successfully.', N'O backup foi restaurado com sucesso.', N'La sauvegarde a été restaurée avec succès.'),
        ('msg.clientenoencontrado', N'No se encontró un cliente asociado a este usuario.', N'No customer record was found for this user.', N'Nenhum cliente associado a este usuário foi encontrado.', N'Aucun client associé à cet utilisateur n''a été trouvé.'),
        ('btn.traducir', N'Traducir', N'Translate', N'Traduzir', N'Traduire'),
        ('msg.seleccioneidioma', N'Debe seleccionar un idioma.', N'You must select a language.', N'Selecione um idioma.', N'Vous devez sélectionner une langue.'),
        ('btn.exportarpdf', N'Exportar a PDF', N'Export to PDF', N'Exportar para PDF', N'Exporter en PDF'),
        ('msg.seleccionereporte', N'Debe seleccionar un reporte de la lista.', N'You must select a report from the list.', N'Selecione um relatório da lista.', N'Vous devez sélectionner un rapport dans la liste.'),
        ('msg.pdfgenerado', N'El PDF se generó correctamente.', N'The PDF was generated successfully.', N'O PDF foi gerado com sucesso.', N'Le PDF a été généré avec succès.'),
        ('menu.ayuda', N'Ayuda', N'Help', N'Ajuda', N'Aide')
    ) AS t(Clave, Es, En, Pt, Fr)
)
INSERT INTO Traducciones (IdIdioma, Clave, Valor)
SELECT @es, Clave, Es FROM Textos
UNION ALL SELECT @en, Clave, En FROM Textos
UNION ALL SELECT @pt, Clave, Pt FROM Textos
UNION ALL SELECT @fr, Clave, Fr FROM Textos;
GO

-- ============================================================
-- Usuario Ejecutivo inicial
-- Usuario: admin  /  Clave: Admin123!
-- Hash generado con el mismo algoritmo que AutoVentas.Services.ServicioCriptografia.HashClave:
-- PBKDF2-HMAC-SHA256, 100000 iteraciones, salt de 16 bytes, clave derivada de 32 bytes, Base64.
-- ============================================================
DECLARE @idRolEjecutivo INT = (SELECT IdRol FROM Roles WHERE Nombre = 'Ejecutivo');

INSERT INTO Usuarios (NombreUsuario, ClaveHash, ClaveSalt, IdRol, Activo)
VALUES (
    'admin',
    '/yn4usxa0ybNVl1+NYWTPXtyQFh1LQugtuuvjFzCbvg=',
    'm1Bc9h1gt7lbt9MXZGvJig==',
    @idRolEjecutivo,
    1
);
GO
