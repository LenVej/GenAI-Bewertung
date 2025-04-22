# Software Requirements Specification

## 1. Einführung
### 1.1 Übersicht
> Das Ziel dieses Projekts "Einsatz von GenAI-Technologien zur Bewertung von Antworten" ist die Entwicklung eines Minimum Viable Product (MVP), das den Einsatz von GenAI zur flexiblen und genauen Bewertung von Antworten in Lernsystemen ermöglicht.

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
- **Angular-Dokumentation**: [Angular](https://angular.io/), Zugriff im Oktober 2024.
- **ASP.NET Core-Dokumentation**: [.NET Documentation](https://docs.microsoft.com/de-de/aspnet/core/), Zugriff im Oktober 2024.
- **OpenAI API**: [OpenAI API Docs](https://beta.openai.com/docs/), Zugriff im Oktober 2024.

## 2. Technologieübersicht

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

### Projekttree

```
∨GenAI-GenAI_Bewertung  
   ∨GenAI-GenAI_Bewertung  
     >Dependencies  
     >Properties  
     >wwwroot  
     ∨ClientApp  
       >angular  
       ∨src  
         >app
         >assets
         >environments
         index.html
         main.ts
         polyfill.ts
         styles.css
         test.ts
     >Controllers
     >Data
     >Entities
     >Migrations
     >Pages
     >Reposiotries
     >Services
     gitignore
     appsettings.json
     appsettings.Development.json
     Program.cs
   ∨GenAI-GenAI_Bewertung.Tests
     >Dependencies
     ∨Controller
       >QuestionsControllerTests.cs
```


## 3. Funktionale Anforderungen
### 3.1 Übersicht 
> Das System ermöglicht die Erstellung, Verwaltung und Bewertung von verschiedenen Fragetypen. Mithilfe der GenAI-Bewertung können Antworten flexibel und präzise ausgewertet werden.

### 3.2 Benutzeroberfläche und Einstellungen
> Benutzer können Fragen erstellen, Einstellungen anpassen und Bewertungsergebnisse einsehen.

#### Mockups:

Futur Pictures:


### 3.3 Fragenmanagement
- **Fragentypen**: Multiple-Choice, Ein-Wort-Antworten, Rechenfragen, Entweder/Oder-Fragen, Schätzfragen, Lückentext, Freitext.

### 3.4 Bewertung durch Generative AI
> GenAI wird zur flexiblen Bewertung eingesetzt, einschließlich Toleranz bei Tippfehlern, Groß-/Kleinschreibung und Synonym-Erkennung.


## 4. Nicht-funktionale Anforderungen

### 4.1 Leistungsanforderungen
- Reaktionszeit: Die Anwendung muss in der Lage sein, Benutzeranfragen innerhalb weniger Sekunden zu verarbeiten und Ergebnisse zurückzugeben, um eine nahtlose Benutzererfahrung zu gewährleisten.
- Verfügbarkeit: Die Webanwendung soll eine Verfügbarkeit von 99% im Jahresdurchschnitt aufweisen, um sicherzustellen, dass Benutzer jederzeit auf die Plattform zugreifen können.
- Skalierbarkeit: Das System muss so entworfen werden, dass es bei steigender Benutzerzahl (z.B. während Tests oder Prüfungen) die Leistung ohne signifikante Verzögerungen oder Ausfälle aufrechterhalten kann.
- Ressourcennutzung: Die Anwendung soll die Serverressourcen (CPU, RAM) effizient nutzen, um die Betriebskosten zu minimieren und gleichzeitig eine hohe Leistung zu gewährleisten.

### 4.2 Sicherheitsanforderungen
- Benutzerdaten-Schutz: Alle Benutzerdaten, einschließlich Passwörter und persönliche Informationen, müssen verschlüsselt gespeichert und über sichere Verbindungen (HTTPS) übertragen werden.
- Zugriffskontrolle: Das System muss ein Rollensystem implementieren, das sicherstellt, dass nur autorisierte Benutzer Zugriff auf bestimmte Funktionen und Daten haben. Administratoren müssen die Möglichkeit haben, Benutzerrechte einfach zu verwalten.
- Eingabe Validierung: Alle Benutzereingaben müssen validiert werden, um SQL-Injection und andere Angriffe zu verhindern. Das System sollte sicherstellen, dass nur gültige Daten in die Datenbank gelangen.
- (Let's Encrypt)

### 4.3 Wartbarkeit
- Modularität: Der Code sollte in Module und Komponenten unterteilt werden, um die Wartung und Erweiterung des Systems zu erleichtern. Jedes Modul sollte eine klare Verantwortung haben, um die Verständlichkeit zu erhöhen.
- Dokumentation: Um die Wartbarkeit zu gewährleisten, muss der Code gut dokumentiert sein. Dies umfasst sowohl Inline-Kommentare als auch externe Dokumentation, die die Architektur, API-Spezifikationen und die Verwendung der Module beschreibt.
- Testbarkeit: Das System sollte so gestaltet sein, dass Unit-Tests und Integrationstests leicht implementiert werden können. Es sollen entsprechende Teststrategien und -tools bereitgestellt werden, um die Qualität des Codes zu gewährleisten.
- Update-Management: Das System muss Mechanismen unterstützen, die eine einfache Aktualisierung von Komponenten und Bibliotheken ermöglichen, ohne dass dabei die Funktionalität oder die Benutzererfahrung beeinträchtigt wird.

### 4.4 Usability
- Benutzerfreundlichkeit: Die Benutzeroberfläche muss intuitiv und benutzerfreundlich gestaltet sein, sodass Benutzer ohne umfangreiche Schulung effektiv mit dem System interagieren können.
- Barrierefreiheit: Die Anwendung sollte die WCAG-Richtlinien (Web Content Accessibility Guidelines) einhalten, um sicherzustellen, dass sie für alle Benutzer, einschließlich Menschen mit Behinderungen, zugänglich ist.

### 4.5 Architektur
Das System basiert auf einer Schichtenarchitektur, die mehrere klar definierte Schichten umfasst:

- Präsentationsschicht (Frontend): Implementiert mit Angular, ermöglicht die Benutzeroberfläche, in der Nutzer ihre Antworten eingeben und Bewertungsergebnisse anzeigen können.

- Anwendungsschicht: Diese Schicht enthält die Geschäftslogik und die Steuerung der Anwendung, die in den Controllers und Services organisiert ist. Hier wird die Logik zur Verarbeitung der Benutzeranfragen und -antworten implementiert.

- Datenschicht: Verantwortlich für den Zugriff auf die PostgreSQL-Datenbank, erfolgt über Repositories, die den Datenzugriff abstrahieren und Datenoperationen kapseln.

Zusätzlich wird eine service-orientierte Architektur (SOA) verwendet, die eine Kommunikation zwischen dem Frontend und dem Backend über REST-APIs ermöglicht. Dies fördert die Modularität und ermöglicht eine unabhängige Entwicklung und Bereitstellung von Anwendungsteilen.

Das Projekt berücksichtigt testgetriebene Entwicklung (TDD), indem eine Teststruktur (GenAI-GenAI_Bewertung.Tests) implementiert wird, die sowohl Unit-Tests mit xUnit als auch End-to-End-Tests mit Cypress umfasst. Diese Tests stellen sicher, dass die Anwendung qualitativ hochwertig und stabil ist.

## 5. Datenbankstruktur
### 5.1 Übersicht

Die Datenbank speichert Informationen zu Fragetypen, Nutzerantworten, Bewertungsergebnissen und Benutzereinstellungen. Die Struktur ist so gestaltet, dass sie eine effiziente Verwaltung und Abfrage der Daten ermöglicht.

### 5.2 Datenbanktabellen

### **Users**
UserID (Primary Key): Eindeutige Identifikation des Benutzers.

Username: Name des Benutzers.

PasswordHash: Hash des Passworts.

Email: E-Mail-Adresse des Benutzers.

CreatedAt: Zeitpunkt der Erstellung des Benutzerkontos.

### **Questions**
QuestionID (Primary Key): Eindeutige Identifikation der Frage.

QuestionText: Text der Frage.

QuestionType: Typ der Frage (z.B. Multiple-Choice, Freitext).

CreatedBy (Foreign Key): ID des Benutzers, der die Frage erstellt hat.

CreatedAt: Zeitpunkt der Erstellung der Frage.

### **Answers**
AnswerID (Primary Key): Eindeutige Identifikation der Antwort.

UserID (Foreign Key): ID des Benutzers, der die Antwort gegeben hat.

QuestionID (Foreign Key): ID der Frage, auf die die Antwort gegeben wurde.

AnswerText: Text der gegebenen Antwort.

SubmittedAt: Zeitpunkt der Abgabe der Antwort.

### **Evaluations**
EvaluationID (Primary Key): Eindeutige Identifikation der Bewertung.

AnswerID (Foreign Key): ID der bewerteten Antwort.

IsCorrect: Gibt Korrektheit der Antwort an.

Score: Punktzahl, die der Antwort zugewiesen wurde.

Feedback: Textuelles Feedback zur Antwort.

EvaluatedAt: Zeitpunkt der Bewertung.

### **Settings**
SettingID (Primary Key): Eindeutige Identifikation der Benutzereinstellungen.

UserID (Foreign Key): ID des Benutzers, zu dem die Einstellungen gehören.

ToleranceLevel: Einstellungswert für Tippfehler.

CaseSensitivity: Einstellung, ob Groß-/Kleinschreibung berücksichtigt werden soll.

AccuracyThreshold: Genauigkeitsgrenze für Schätzfragen.

### 5.3 ER-Diagramm

![Alternativer Text](/Dokumentation/Database_Structure.png)

How it's implemented so far (22.04.2025)
![er_diagram](https://github.com/user-attachments/assets/ca7566ca-51a2-4610-9144-198153973919)


## 6. Zukünftige Erweiterungen (Ideen)
- **Adaptive Lernpfade**: Personalisierte Empfehlungen basierend auf den Bewertungen.
- **Mehrsprachigkeit**: Unterstützung für mehrere Sprachen, z.B. Deutsch und Englisch.
- **Integration von Sprach- und Bilderkennung**: Erweiterung um Sprach- und Bildanalyse-Optionen für Antworten.


## 7. Evaluation von AIs

### 1. AI APIs

#### OpenAI GPT-4 / ChatGPT API:
- Stärken: GPT-4 kann Antworten kontextbezogen bewerten, Synonyme erkennen, Tippfehler tolerieren und Freitext sowie multiple Fragentypen gut verarbeiten. Es bietet auch Flexibilität für Schätzfragen und Textanalysen in Freitextantworten.
- Einschränkungen: GPT-4 benötigt oft eine spezifische Eingabeanweisung, um auf bestimmte Fehler-Toleranzen (z. B. Synonyme) zu reagieren. Einige Bewertungsparameter (z. B. Toleranzlevel) müssen über das Backend gesteuert werden.
>Not free at all

#### Gemini API:
- Stärken: Unterstützt multimodale Verarbeitung (Text und Bilder), wodurch zukünftige Erweiterungen wie Bildanalyse oder Sprachverarbeitung nahtlos integriert werden können. 
Großes Kontextfenster von bis zu 1 Million Tokens ermöglicht präzise Analysen langer Texte oder umfangreicher Antworten.
Flexible NLP-Fähigkeiten für Synonym-Erkennung, Tippfehler-Toleranz und Abweichungen in Antwortformaten, besonders für Freitextanalysen geeignet.
- Schwächen: Einige Funktionen wie strukturiertes Setzen von Bewertungstoleranzen (z. B. Tippfehlergrenzen) könnten mehr Backend-Logik erfordern.
Generative Textverarbeitung im Vergleich zu OpenAI möglicherweise weniger ausgereift bei komplexen Freitextanalysen.
Bestimmte Features, wie der OpenAI-kompatible Endpoint, befinden sich noch in der Beta-Phase und sind nicht vollständig stabil.

#### Anthropic Claude:
- Stärken: Claude bietet solide Flexibilität für Synonym- und Kontextanalyse, Fehler-Toleranz und Freitextbewertungen. Es kann „fuzzy“ Abgleiche durchführen, was bei variablen Antworten hilfreich ist.
- Einschränkungen: Anpassung der Toleranz-Level könnte im Vergleich zu anderen Modellen begrenzter sein, sodass die Backend-Logik hier mehr eingreifen müsste.

#### Microsoft Azure Cognitive Services:
- Stärken: Microsofts Modelle erkennen Tippfehler, Synonyme und einfache grammatische Fehler. Sie können gut bei kurzen, präzisen Antwortformaten helfen und lassen sich für grundlegende Synonymanalyse und Bewertung nutzen.
- Einschränkungen: Azure bietet weniger Flexibilität für komplexe Textanalysen und könnte Einschränkungen bei Freitext- und Schätzfragen haben, da es keine ausgeprägte generative Komponente hat.

#### Cohere API:
- Stärken: Cohere-Modelle bieten eine zuverlässige Erkennung von Synonymen, Wort-Varianten und können für flexible Antwortbewertung gut angepasst werden.
- Einschränkungen: Die Modelle haben in der Regel weniger tiefgehende Textanalysefähigkeiten und könnten bei Freitextbewertungen in größeren, variableren Kontexten -Schwierigkeiten haben.

### 2. Vergleich der Kosten pro Millionen Tokens

| API                         | Input ($) | Cached Input ($) | Output ($) | Caching Write ($) | Caching Read ($) |
|-----------------------------|-----------|------------------|------------|-------------------|------------------|
| OpenAI (gpt-4o)             | 2.50      | 1.25             | 10.00      | -                 | -                |
| OpenAI (gpt-4o-mini)        | 0.15      | 0.075            | 0.60       | -                 | -                |
| OpenAI (o1-preview)         | 15.00     | 7.50             | 60.00      | -                 | -                |
| OpenAI (o1-mini)            | 3.00      | 1.50             | 12.00      | -                 | -                |
| Gemini (Flash 1.5)          | 0.075     | 0.01875          | 0.30       | -                 | -                |
| Claude 3.5 Haiku            | 1.00      | 1.25             | 5.00       | 1.25              | 0.10             |
| Azure (GPT-4o)              | 2.55      | 1.27             | 10.19      | -                 | -                |
| Azure (GPT-4o-mini)         | 0.15      | 0.077            | 0.61       | -                 | -                |
| Azure (o1-preview)          | 15.28     | 7.64             | 61.12      | -                 | -                |
| Cohere (Command R+)         | 2.50      | -                | 10.00      | -                 | -                |
| Cohere (Command R)          | 0.15      | -                | 0.60       | -                 | -                |
| DeepSeek (deepseek-chat)    | 0.27      | 0.07             | 1.10       | -                 | -                |
| DeepSeek (deepseek-reasoner)| 0.55      | 0.14             | 2.19       | -                 | -                |
