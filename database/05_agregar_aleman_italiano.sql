/*
================================================================================
 Script incremental: agrega Aleman (de) e Italiano (it) a un sistema que
 YA tiene la base de datos creada con una version anterior (01+02+03, con o
 sin 04_actualizacion_permisos_idiomas.sql). Si vas a crear la base de datos
 desde cero, NO hace falta correr este archivo: ya esta incluido en 02_seed.sql.
 Es idempotente: se puede correr mas de una vez sin duplicar datos.
================================================================================
*/
USE AutoVentasDB;
GO

IF NOT EXISTS (SELECT 1 FROM Idiomas WHERE Codigo = 'de')
    INSERT INTO Idiomas (Codigo, Nombre) VALUES ('de', 'Deutsch');
IF NOT EXISTS (SELECT 1 FROM Idiomas WHERE Codigo = 'it')
    INSERT INTO Idiomas (Codigo, Nombre) VALUES ('it', 'Italiano');
GO

DECLARE @de INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'de');
DECLARE @it INT = (SELECT IdIdioma FROM Idiomas WHERE Codigo = 'it');

;WITH Textos AS (
    SELECT * FROM (VALUES
        ('lbl.usuario', N'Benutzer', N'Utente'),
        ('lbl.clave', N'Passwort', N'Password'),
        ('btn.ingresar', N'Anmelden', N'Accedi'),
        ('btn.registrarse', N'Registrieren', N'Registrati'),
        ('btn.cancelar', N'Abbrechen', N'Annulla'),
        ('btn.guardar', N'Speichern', N'Salva'),
        ('btn.nuevo', N'Neu', N'Nuovo'),
        ('btn.editar', N'Bearbeiten', N'Modifica'),
        ('btn.eliminar', N'Löschen', N'Elimina'),
        ('btn.refrescar', N'Aktualisieren', N'Aggiorna'),
        ('btn.cerrarsesion', N'Abmelden', N'Esci'),
        ('btn.iralmenu', N'Zu meinem Menü', N'Vai al mio menu'),
        ('frm.login', N'Anmeldung', N'Accesso'),
        ('frm.registro', N'Benutzerregistrierung', N'Registrazione utente'),
        ('frm.principal', N'Autoverkaufssystem', N'Sistema di Vendita Auto'),
        ('frm.menuejecutivo', N'Geschäftsführer-Menü', N'Menu Dirigente'),
        ('frm.menuvendedor', N'Verkäufer-Menü', N'Menu Venditore'),
        ('frm.menutecnico', N'Techniker-Menü', N'Menu Tecnico'),
        ('frm.menucliente', N'Kunden-Menü', N'Menu Cliente'),
        ('menu.contratos', N'Verträge', N'Contratti'),
        ('menu.presupuestos', N'Kostenvoranschläge', N'Preventivi'),
        ('menu.reservas', N'Reservierungen', N'Prenotazioni'),
        ('menu.pagos', N'Zahlungen', N'Pagamenti'),
        ('menu.entregas', N'Lieferungen', N'Consegne'),
        ('menu.reportes', N'Berichte', N'Rapporti'),
        ('menu.vehiculos', N'Fahrzeuge', N'Veicoli'),
        ('menu.clientes', N'Kunden', N'Clienti'),
        ('menu.mantenimientos', N'Wartungen', N'Manutenzioni'),
        ('menu.bitacora', N'Protokoll', N'Registro attività'),
        ('menu.permisos', N'Berechtigungen', N'Permessi'),
        ('menu.backup', N'Sicherungskopien', N'Backup'),
        ('menu.idioma', N'Sprache', N'Lingua'),
        ('menu.idiomas', N'Sprachen', N'Lingue'),
        ('menu.historialcambios', N'Änderungsverlauf', N'Cronologia modifiche'),
        ('btn.nuevoidioma', N'Neue Sprache', N'Nuova lingua'),
        ('msg.idiomaguardado', N'Die Übersetzungen wurden erfolgreich gespeichert.', N'Le traduzioni sono state salvate correttamente.'),
        ('lbl.tabla', N'Tabelle', N'Tabella'),
        ('btn.volver', N'Zurück', N'Indietro'),
        ('btn.buscar', N'Suchen', N'Cerca'),
        ('btn.reservar', N'Reservieren', N'Prenota'),
        ('btn.nuevareserva', N'Neue Reservierung', N'Nuova prenotazione'),
        ('btn.generarbackup', N'Sicherung erstellen', N'Genera backup'),
        ('btn.restaurar', N'Wiederherstellen', N'Ripristina'),
        ('lbl.confirmarclave', N'Passwort bestätigen', N'Conferma password'),
        ('lbl.rol', N'Rolle', N'Ruolo'),
        ('lbl.nombre', N'Vorname', N'Nome'),
        ('lbl.apellido', N'Nachname', N'Cognome'),
        ('lbl.dni', N'Ausweisnummer', N'Codice fiscale'),
        ('lbl.marca', N'Marke', N'Marca'),
        ('lbl.modelo', N'Modell', N'Modello'),
        ('lbl.color', N'Farbe', N'Colore'),
        ('lbl.anio', N'Jahr', N'Anno'),
        ('lbl.precio', N'Preis', N'Prezzo'),
        ('lbl.disponible', N'Verfügbar', N'Disponibile'),
        ('lbl.vehiculo', N'Fahrzeug', N'Veicolo'),
        ('lbl.cliente', N'Kunde', N'Cliente'),
        ('lbl.monto', N'Betrag', N'Importo'),
        ('lbl.estado', N'Status', N'Stato'),
        ('lbl.servicio', N'Dienstleistung', N'Servizio'),
        ('lbl.fecha', N'Datum', N'Data'),
        ('lbl.vencimiento', N'Fälligkeit', N'Scadenza'),
        ('lbl.metodopago', N'Zahlungsmethode', N'Metodo di pagamento'),
        ('lbl.lugar', N'Ort', N'Luogo'),
        ('lbl.titulo', N'Titel', N'Titolo'),
        ('lbl.tipo', N'Typ', N'Tipo'),
        ('lbl.desde', N'Von', N'Da'),
        ('lbl.hasta', N'Bis', N'A'),
        ('lbl.actividad', N'Aktivität', N'Attività'),
        ('msg.registroexitoso', N'Benutzer erfolgreich registriert.', N'Utente registrato correttamente.'),
        ('msg.clavesnocoinciden', N'Die Passwörter stimmen nicht überein.', N'Le password non coincidono.'),
        ('msg.confirmareliminar', N'Möchten Sie den ausgewählten Datensatz wirklich löschen?', N'Confermi di voler eliminare il record selezionato?'),
        ('msg.seleccionevehiculocliente', N'Sie müssen ein Fahrzeug und einen Kunden auswählen.', N'Devi selezionare un veicolo e un cliente.'),
        ('msg.completetodosloscampos', N'Sie müssen alle Felder ausfüllen.', N'Devi compilare tutti i campi.'),
        ('msg.permisosguardados', N'Die Berechtigungen der Rolle wurden erfolgreich gespeichert.', N'I permessi del ruolo sono stati salvati correttamente.'),
        ('msg.backupgenerado', N'Die Sicherung wurde erfolgreich erstellt.', N'Il backup è stato generato correttamente.'),
        ('msg.confirmarrestaurar', N'Möchten Sie dieses Backup wirklich wiederherstellen? Die aktuelle Datenbank wird ersetzt.', N'Confermi di voler ripristinare questo backup? Il database attuale verrà sostituito.'),
        ('msg.backuprestaurado', N'Die Sicherung wurde erfolgreich wiederhergestellt.', N'Il backup è stato ripristinato correttamente.'),
        ('msg.clientenoencontrado', N'Es wurde kein mit diesem Benutzer verknüpfter Kunde gefunden.', N'Non è stato trovato nessun cliente associato a questo utente.'),
        ('btn.traducir', N'Übersetzen', N'Traduci'),
        ('msg.seleccioneidioma', N'Sie müssen eine Sprache auswählen.', N'Devi selezionare una lingua.'),
        ('btn.exportarpdf', N'Als PDF exportieren', N'Esporta in PDF'),
        ('msg.seleccionereporte', N'Sie müssen einen Bericht aus der Liste auswählen.', N'Devi selezionare un rapporto dall''elenco.'),
        ('msg.pdfgenerado', N'Das PDF wurde erfolgreich erstellt.', N'Il PDF è stato generato correttamente.'),
        ('menu.ayuda', N'Hilfe', N'Aiuto')
    ) AS t(Clave, De, It)
)
MERGE Traducciones AS destino
USING (
    SELECT @de AS IdIdioma, Clave, De AS Valor FROM Textos
    UNION ALL SELECT @it, Clave, It FROM Textos
) AS origen
ON destino.IdIdioma = origen.IdIdioma AND destino.Clave = origen.Clave
WHEN MATCHED THEN UPDATE SET Valor = origen.Valor
WHEN NOT MATCHED THEN INSERT (IdIdioma, Clave, Valor) VALUES (origen.IdIdioma, origen.Clave, origen.Valor);
GO
