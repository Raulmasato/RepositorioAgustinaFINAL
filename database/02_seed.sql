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
    ('AD003', 'Gestionar backups', NULL),
    ('AD004', 'Gestionar idiomas', NULL),
    ('AD005', 'Ver historial de cambios', NULL);
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
UPDATE Permisos SET IdPermisoPadre = (SELECT IdPermiso FROM Permisos WHERE Codigo = 'GE-AD') WHERE Codigo IN ('AD001','AD002','AD003','AD004','AD005');
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
    ('es', 'Español'), ('en', 'English'), ('pt', 'Português'), ('fr', 'Français'),
    ('de', 'Deutsch'), ('it', 'Italiano');
GO

DECLARE @es INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'es');
DECLARE @en INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'en');
DECLARE @pt INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'pt');
DECLARE @fr INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'fr');
DECLARE @de INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'de');
DECLARE @it INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'it');

;WITH Textos AS (
    SELECT * FROM (VALUES
        ('lbl.usuario', N'Usuario',                N'User',                 N'Usuário',              N'Utilisateur', N'Benutzer', N'Utente'),
        ('lbl.clave', N'Contraseña',              N'Password',             N'Senha',                N'Mot de passe', N'Passwort', N'Password'),
        ('btn.ingresar', N'Ingresar',                N'Log in',               N'Entrar',               N'Se connecter', N'Anmelden', N'Accedi'),
        ('btn.registrarse', N'Registrarse',             N'Register',             N'Registrar-se',         N'S''inscrire', N'Registrieren', N'Registrati'),
        ('btn.cancelar', N'Cancelar',                N'Cancel',               N'Cancelar',             N'Annuler', N'Abbrechen', N'Annulla'),
        ('btn.guardar', N'Guardar',                 N'Save',                 N'Salvar',               N'Enregistrer', N'Speichern', N'Salva'),
        ('btn.nuevo', N'Nuevo',                   N'New',                  N'Novo',                 N'Nouveau', N'Neu', N'Nuovo'),
        ('btn.editar', N'Editar',                  N'Edit',                 N'Editar',               N'Modifier', N'Bearbeiten', N'Modifica'),
        ('btn.eliminar', N'Eliminar',                N'Delete',               N'Excluir',              N'Supprimer', N'Löschen', N'Elimina'),
        ('btn.refrescar', N'Refrescar',               N'Refresh',              N'Atualizar',            N'Actualiser', N'Aktualisieren', N'Aggiorna'),
        ('btn.cerrarsesion', N'Cerrar sesión',           N'Log out',              N'Sair',                 N'Se déconnecter', N'Abmelden', N'Esci'),
        ('btn.iralmenu', N'Ir a mi menú',            N'Go to my menu',        N'Ir ao meu menu',       N'Aller à mon menu', N'Zu meinem Menü', N'Vai al mio menu'),
        ('frm.login', N'Inicio de sesión',        N'Login',                N'Login',                N'Connexion', N'Anmeldung', N'Accesso'),
        ('frm.registro', N'Registro de usuario',     N'User registration',    N'Registro de usuário',  N'Inscription utilisateur', N'Benutzerregistrierung', N'Registrazione utente'),
        ('frm.principal', N'Sistema de Venta de Autos', N'Car Sales System',   N'Sistema de Venda de Carros', N'Système de Vente de Voitures', N'Autoverkaufssystem', N'Sistema di Vendita Auto'),
        ('frm.menuejecutivo', N'Menú Ejecutivo',          N'Executive Menu',       N'Menu Executivo',       N'Menu Exécutif', N'Geschäftsführer-Menü', N'Menu Dirigente'),
        ('frm.menuvendedor', N'Menú Vendedor',           N'Salesperson Menu',     N'Menu Vendedor',        N'Menu Vendeur', N'Verkäufer-Menü', N'Menu Venditore'),
        ('frm.menutecnico', N'Menú Técnico',            N'Technician Menu',      N'Menu Técnico',         N'Menu Technicien', N'Techniker-Menü', N'Menu Tecnico'),
        ('frm.menucliente', N'Menú Cliente',            N'Customer Menu',        N'Menu Cliente',         N'Menu Client', N'Kunden-Menü', N'Menu Cliente'),
        ('menu.contratos', N'Contratos',               N'Contracts',            N'Contratos',            N'Contrats', N'Verträge', N'Contratti'),
        ('menu.presupuestos', N'Presupuestos',            N'Quotes',               N'Orçamentos',           N'Devis', N'Kostenvoranschläge', N'Preventivi'),
        ('menu.reservas', N'Reservas',                N'Reservations',         N'Reservas',             N'Réservations', N'Reservierungen', N'Prenotazioni'),
        ('menu.pagos', N'Pagos',                   N'Payments',             N'Pagamentos',           N'Paiements', N'Zahlungen', N'Pagamenti'),
        ('menu.entregas', N'Entregas',                N'Deliveries',           N'Entregas',             N'Livraisons', N'Lieferungen', N'Consegne'),
        ('menu.reportes', N'Reportes',                N'Reports',              N'Relatórios',           N'Rapports', N'Berichte', N'Rapporti'),
        ('menu.vehiculos', N'Vehículos',               N'Vehicles',             N'Veículos',             N'Véhicules', N'Fahrzeuge', N'Veicoli'),
        ('menu.clientes', N'Clientes',                N'Customers',            N'Clientes',             N'Clients', N'Kunden', N'Clienti'),
        ('menu.mantenimientos', N'Mantenimientos',        N'Maintenance',          N'Manutenções',          N'Entretiens', N'Wartungen', N'Manutenzioni'),
        ('menu.bitacora', N'Bitácora',                N'Audit log',            N'Log de auditoria',     N'Journal d''audit', N'Protokoll', N'Registro attività'),
        ('menu.permisos', N'Permisos',                N'Permissions',          N'Permissões',           N'Autorisations', N'Berechtigungen', N'Permessi'),
        ('menu.backup', N'Copias de seguridad',     N'Backups',              N'Cópias de segurança',  N'Sauvegardes', N'Sicherungskopien', N'Backup'),
        ('menu.idioma', N'Idioma',                  N'Language',             N'Idioma',               N'Langue', N'Sprache', N'Lingua'),
        ('menu.idiomas', N'Idiomas',                 N'Languages',            N'Idiomas',              N'Langues', N'Sprachen', N'Lingue'),
        ('menu.historialcambios', N'Historial de cambios', N'Change history',      N'Histórico de alterações', N'Historique des modifications', N'Änderungsverlauf', N'Cronologia modifiche'),
        ('btn.nuevoidioma', N'Nuevo idioma',            N'New language',         N'Novo idioma',          N'Nouvelle langue', N'Neue Sprache', N'Nuova lingua'),
        ('msg.idiomaguardado', N'Las traducciones se guardaron correctamente.', N'The translations were saved successfully.', N'As traduções foram salvas com sucesso.', N'Les traductions ont été enregistrées avec succès.', N'Die Übersetzungen wurden erfolgreich gespeichert.', N'Le traduzioni sono state salvate correttamente.'),
        ('lbl.tabla', N'Tabla',                   N'Table',                N'Tabela',               N'Table', N'Tabelle', N'Tabella'),
        ('btn.volver', N'Volver',                  N'Back',                 N'Voltar',               N'Retour', N'Zurück', N'Indietro'),
        ('btn.buscar', N'Buscar',                  N'Search',               N'Buscar',               N'Rechercher', N'Suchen', N'Cerca'),
        ('btn.reservar', N'Reservar',                N'Reserve',              N'Reservar',             N'Réserver', N'Reservieren', N'Prenota'),
        ('btn.nuevareserva', N'Nueva reserva',           N'New reservation',      N'Nova reserva',         N'Nouvelle réservation', N'Neue Reservierung', N'Nuova prenotazione'),
        ('btn.generarbackup', N'Generar backup',          N'Generate backup',      N'Gerar backup',         N'Générer une sauvegarde', N'Sicherung erstellen', N'Genera backup'),
        ('btn.restaurar', N'Restaurar',               N'Restore',              N'Restaurar',            N'Restaurer', N'Wiederherstellen', N'Ripristina'),
        ('lbl.confirmarclave', N'Confirmar contraseña',   N'Confirm password',     N'Confirmar senha',      N'Confirmer le mot de passe', N'Passwort bestätigen', N'Conferma password'),
        ('lbl.rol', N'Rol',                     N'Role',                 N'Função',               N'Rôle', N'Rolle', N'Ruolo'),
        ('lbl.nombre', N'Nombre',                  N'First name',           N'Nome',                 N'Prénom', N'Vorname', N'Nome'),
        ('lbl.apellido', N'Apellido',                N'Last name',            N'Sobrenome',            N'Nom', N'Nachname', N'Cognome'),
        ('lbl.dni', N'DNI',                     N'National ID',          N'RG/CPF',               N'Pièce d''identité', N'Ausweisnummer', N'Codice fiscale'),
        ('lbl.marca', N'Marca',                   N'Brand',                N'Marca',                N'Marque', N'Marke', N'Marca'),
        ('lbl.modelo', N'Modelo',                  N'Model',                N'Modelo',               N'Modèle', N'Modell', N'Modello'),
        ('lbl.color', N'Color',                   N'Color',                N'Cor',                  N'Couleur', N'Farbe', N'Colore'),
        ('lbl.anio', N'Año',                     N'Year',                 N'Ano',                  N'Année', N'Jahr', N'Anno'),
        ('lbl.precio', N'Precio',                  N'Price',                N'Preço',                N'Prix', N'Preis', N'Prezzo'),
        ('lbl.disponible', N'Disponible',              N'Available',            N'Disponível',           N'Disponible', N'Verfügbar', N'Disponibile'),
        ('lbl.vehiculo', N'Vehículo',                N'Vehicle',              N'Veículo',              N'Véhicule', N'Fahrzeug', N'Veicolo'),
        ('lbl.cliente', N'Cliente',                 N'Customer',             N'Cliente',              N'Client', N'Kunde', N'Cliente'),
        ('lbl.monto', N'Monto',                   N'Amount',               N'Valor',                N'Montant', N'Betrag', N'Importo'),
        ('lbl.estado', N'Estado',                  N'Status',               N'Situação',             N'État', N'Status', N'Stato'),
        ('lbl.servicio', N'Servicio',                N'Service',              N'Serviço',              N'Service', N'Dienstleistung', N'Servizio'),
        ('lbl.fecha', N'Fecha',                   N'Date',                 N'Data',                 N'Date', N'Datum', N'Data'),
        ('lbl.vencimiento', N'Vencimiento',             N'Expiration',           N'Vencimento',           N'Expiration', N'Fälligkeit', N'Scadenza'),
        ('lbl.metodopago', N'Método de pago',          N'Payment method',       N'Forma de pagamento',   N'Mode de paiement', N'Zahlungsmethode', N'Metodo di pagamento'),
        ('lbl.lugar', N'Lugar',                   N'Location',             N'Local',                N'Lieu', N'Ort', N'Luogo'),
        ('lbl.titulo', N'Título',                  N'Title',                N'Título',               N'Titre', N'Titel', N'Titolo'),
        ('lbl.tipo', N'Tipo',                    N'Type',                 N'Tipo',                 N'Type', N'Typ', N'Tipo'),
        ('lbl.desde', N'Desde',                   N'From',                 N'De',                   N'Depuis', N'Von', N'Da'),
        ('lbl.hasta', N'Hasta',                   N'To',                   N'Até',                  N'Jusqu''à', N'Bis', N'A'),
        ('lbl.actividad', N'Actividad',               N'Activity',             N'Atividade',            N'Activité', N'Aktivität', N'Attività'),
        ('msg.registroexitoso', N'Usuario registrado correctamente.', N'User registered successfully.', N'Usuário registrado com sucesso.', N'Utilisateur enregistré avec succès.', N'Benutzer erfolgreich registriert.', N'Utente registrato correttamente.'),
        ('msg.clavesnocoinciden', N'Las contraseñas no coinciden.', N'Passwords do not match.', N'As senhas não coincidem.', N'Les mots de passe ne correspondent pas.', N'Die Passwörter stimmen nicht überein.', N'Le password non coincidono.'),
        ('msg.confirmareliminar', N'¿Confirma que desea eliminar el registro seleccionado?', N'Confirm you want to delete the selected record?', N'Confirma que deseja excluir o registro selecionado?', N'Confirmez-vous la suppression de l''enregistrement sélectionné ?', N'Möchten Sie den ausgewählten Datensatz wirklich löschen?', N'Confermi di voler eliminare il record selezionato?'),
        ('msg.seleccionevehiculocliente', N'Debe seleccionar un vehículo y un cliente.', N'You must select a vehicle and a customer.', N'Selecione um veículo e um cliente.', N'Vous devez sélectionner un véhicule et un client.', N'Sie müssen ein Fahrzeug und einen Kunden auswählen.', N'Devi selezionare un veicolo e un cliente.'),
        ('msg.completetodosloscampos', N'Debe completar todos los campos.', N'You must complete all fields.', N'Preencha todos os campos.', N'Vous devez remplir tous les champs.', N'Sie müssen alle Felder ausfüllen.', N'Devi compilare tutti i campi.'),
        ('msg.permisosguardados', N'Los permisos del rol se guardaron correctamente.', N'Role permissions were saved successfully.', N'As permissões da função foram salvas com sucesso.', N'Les autorisations du rôle ont été enregistrées avec succès.', N'Die Berechtigungen der Rolle wurden erfolgreich gespeichert.', N'I permessi del ruolo sono stati salvati correttamente.'),
        ('msg.backupgenerado', N'El backup se generó correctamente.', N'The backup was generated successfully.', N'O backup foi gerado com sucesso.', N'La sauvegarde a été générée avec succès.', N'Die Sicherung wurde erfolgreich erstellt.', N'Il backup è stato generato correttamente.'),
        ('msg.confirmarrestaurar', N'¿Confirma que desea restaurar este backup? Se reemplazará la base de datos actual.', N'Confirm you want to restore this backup? The current database will be replaced.', N'Confirma que deseja restaurar este backup? O banco de dados atual será substituído.', N'Confirmez-vous la restauration de cette sauvegarde ? La base de données actuelle sera remplacée.', N'Möchten Sie dieses Backup wirklich wiederherstellen? Die aktuelle Datenbank wird ersetzt.', N'Confermi di voler ripristinare questo backup? Il database attuale verrà sostituito.'),
        ('msg.backuprestaurado', N'El backup se restauró correctamente.', N'The backup was restored successfully.', N'O backup foi restaurado com sucesso.', N'La sauvegarde a été restaurée avec succès.', N'Die Sicherung wurde erfolgreich wiederhergestellt.', N'Il backup è stato ripristinato correttamente.'),
        ('msg.clientenoencontrado', N'No se encontró un cliente asociado a este usuario.', N'No customer record was found for this user.', N'Nenhum cliente associado a este usuário foi encontrado.', N'Aucun client associé à cet utilisateur n''a été trouvé.', N'Es wurde kein mit diesem Benutzer verknüpfter Kunde gefunden.', N'Non è stato trovato nessun cliente associato a questo utente.'),
        ('btn.traducir', N'Traducir', N'Translate', N'Traduzir', N'Traduire', N'Übersetzen', N'Traduci'),
        ('msg.seleccioneidioma', N'Debe seleccionar un idioma.', N'You must select a language.', N'Selecione um idioma.', N'Vous devez sélectionner une langue.', N'Sie müssen eine Sprache auswählen.', N'Devi selezionare una lingua.'),
        ('btn.exportarpdf', N'Exportar a PDF', N'Export to PDF', N'Exportar para PDF', N'Exporter en PDF', N'Als PDF exportieren', N'Esporta in PDF'),
        ('msg.seleccionereporte', N'Debe seleccionar un reporte de la lista.', N'You must select a report from the list.', N'Selecione um relatório da lista.', N'Vous devez sélectionner un rapport dans la liste.', N'Sie müssen einen Bericht aus der Liste auswählen.', N'Devi selezionare un rapporto dall''elenco.'),
        ('msg.pdfgenerado', N'El PDF se generó correctamente.', N'The PDF was generated successfully.', N'O PDF foi gerado com sucesso.', N'Le PDF a été généré avec succès.', N'Das PDF wurde erfolgreich erstellt.', N'Il PDF è stato generato correttamente.'),
        ('menu.ayuda', N'Ayuda', N'Help', N'Ajuda', N'Aide', N'Hilfe', N'Aiuto')
    ) AS t(Clave, Es, En, Pt, Fr, De, It)
)
INSERT INTO Traducciones (IdIdioma, Clave, Valor)
SELECT @es, Clave, Es FROM Textos
UNION ALL SELECT @en, Clave, En FROM Textos
UNION ALL SELECT @pt, Clave, Pt FROM Textos
UNION ALL SELECT @fr, Clave, Fr FROM Textos
UNION ALL SELECT @de, Clave, De FROM Textos
UNION ALL SELECT @it, Clave, It FROM Textos;
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
