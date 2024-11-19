# GenAI-Bewertung

## Projektübersicht
Dieses Projekt bietet eine AI-basierte Bewertungsplattform zur automatischen Bewertung von Antworten in verschiedenen Fragetypen. Das System ist in ASP.NET Core aufgebaut und verwendet Angular im Frontend sowie PostgreSQL als Datenbank.

## Voraussetzungen
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js](https://nodejs.org/) (für das Angular-Frontend)
- [PostgreSQL](https://www.postgresql.org/download/)

## Einrichtung und Installation

### 1. Repository klonen
Klone das Projekt von GitHub:
```bash
git clone https://github.com/LenVej/GenAI-Bewertung.git
cd GenAI-Bewertung/GenAI-Bewertung
```

## 2. PostgreSQL-Datenbank einrichten

1. Erstelle eine neue PostgreSQL-Datenbank.
2. Aktualisiere die Verbindungszeichenfolge in der Datei `appsettings.json`:
 ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=DeinDatenbankname;Username=DeinBenutzername;Password=DeinPasswort"
    }
```

## 3. Datenbank-Migrationen anwenden
Führe die Migrationsbefehle aus, um die Datenbanktabellen zu erstellen:

```bash
dotnet ef database update
```

## 4. Abhängigkeiten installieren
Navigiere zum ClientApp-Verzeichnis und installieren Sie die Node.js-Abhängigkeiten:
```bash
cd ClientApp
npm install
```

## 5. Projekt starten
Das Projekt ist nun bereit. Starte es mit folgendem Befehl:

```bash
cd ..
dotnet run
```

##
Swagger: https://localhost:7087/swagger/index.html

## Projektstruktur
- ClientApp: Enthält das Angular-Frontend.
- Controllers: ASP.NET Core-Controller für die API.
- Data: Datenbankkontext und -zugriff.
- Migrations: Datenbankmigrationsdateien.
- Models: Datenmodelle und Business-Logik.
- Tests: Projekt für Unit-Tests (xUnit).

















