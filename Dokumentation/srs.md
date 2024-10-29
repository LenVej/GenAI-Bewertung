# Software Requirements Specification

## 1. Einführung
### 1.1 Übersicht
> Das Ziel dieses Projekts ist die Entwicklung eines Minimum Viable Product (MVP), das den Einsatz von GenAI zur flexiblen und genauen Bewertung von Antworten in Lernsystemen ermöglicht.

### 1.2 Umfang
> Dieses Software Requirements Specification (SRS)-Dokument umfasst die funktionalen und nicht-funktionalen Anforderungen des Systems. Es bietet eine detaillierte Übersicht der Architektur, der Merkmale und der Einschränkungen der Plattform, um das Ziel des Projekts zu verdeutlichen.

*Das SRS umfasst:*

- Eine Systemübersicht und Anwendungsfälle.
- Detaillierte funktionale Anforderungen, wie Fragetypen und Bewertungsoptionen.
- Eine Liste der verwendeten Technologien und deren Auswahlgründe.


### 1.3 Definitionen, Akronyme und Abkürzungen
- **SRS**: Software Requirements Specification – dieses Dokument, das die Anforderungen beschreibt.
- **GenAI**: Generative AI-Technologien, die für die Bewertung der Antworten verwendet werden.
- **ASP.NET Core**: Backend-Framework für die Verarbeitung der Quiz-Logik.
- **Angular**: Framework für die Benutzeroberfläche.
- **REST-API**: Schnittstelle zur Kommunikation zwischen Frontend und Backend.
  
### 1.4 Referenzen
- **Angular-Dokumentation**: [Angular](https://angular.io/), Zugriff im Oktober 2023.
- **ASP.NET Core-Dokumentation**: [.NET Documentation](https://docs.microsoft.com/de-de/aspnet/core/), Zugriff im Oktober 2023.
- **OpenAI API**: [OpenAI API Docs](https://beta.openai.com/docs/), Zugriff im Oktober 2023.

## 2. Funktionale Anforderungen
### 2.1 Übersicht 
> Das System ermöglicht die Erstellung, Verwaltung und Bewertung von verschiedenen Fragetypen. Mithilfe der GenAI-Bewertung können Antworten flexibel und präzise ausgewertet werden.

### 2.2 Benutzeroberfläche und Einstellungen
> Benutzer können Fragen erstellen, Einstellungen anpassen und Bewertungsergebnisse einsehen.

#### Mockups


### 2.3 Fragenmanagement
- **Fragentypen**: Multiple-Choice, Ein-Wort-Antworten, Rechenfragen, Entweder/Oder-Fragen, Schätzfragen, Lückentext, Freitext.

> **Vorbedingungen**: Backend-Server muss aktiv sein.
> **Nachbedingungen**: Alle ausgehenden Routen der Funktionen müssen korrekt arbeiten.

### 2.4 Bewertung durch Generative AI
> GenAI wird zur flexiblen Bewertung eingesetzt, einschließlich Toleranz bei Tippfehlern, Groß-/Kleinschreibung und Synonym-Erkennung.

## 3. Zukünftige Erweiterungen (Ideen)
- **Adaptive Lernpfade**: Personalisierte Empfehlungen basierend auf den Bewertungen.
- **Mehrsprachigkeit**: Unterstützung für mehrere Sprachen, z.B. Deutsch und Englisch.
- **Integration von Sprach- und Bilderkennung**: Erweiterung um Sprach- und Bildanalyse-Optionen für Antworten.

## 4. Technologieübersicht

### Projektmanagement:
- **GitHub Kanban**: Projektmanagement und Aufgabenverfolgung.
  
### IDEs:
- **WebStorm** für Angular und Frontend-Entwicklung.
- **Rider** für Backend-Entwicklung

### Bibliotheken:
- **Angular**: Frontend-Framework.
- **ASP.NET Core**: Backend-Framework.

### Tool
- **Swagger**: API-Dokumentation und -Testwerkzeug für ASP.NET Core.
