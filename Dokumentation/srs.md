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


## Evaluation von AIs

### 1. AI APIs

#### OpenAI GPT-4 / ChatGPT API:
- Stärken: GPT-4 kann Antworten kontextbezogen bewerten, Synonyme erkennen, Tippfehler tolerieren und Freitext sowie multiple Fragentypen gut verarbeiten. Es bietet auch Flexibilität für Schätzfragen und Textanalysen in Freitextantworten.
- Einschränkungen: GPT-4 benötigt oft eine spezifische Eingabeanweisung, um auf bestimmte Fehler-Toleranzen (z. B. Synonyme) zu reagieren. Einige Bewertungsparameter (z. B. Toleranzlevel) müssen über das Backend gesteuert werden.

#### Google Cloud AI (BERT-basierte Modelle):
- Stärken: BERT und ähnliche Modelle können präzise Synonyme und ähnliche Begriffe erkennen und einfache Tippfehler tolerieren. BERT ist besonders gut darin, verschiedene Antwortformate zu bewerten, kann aber komplexere Bewertungen (wie Kontext oder tiefergehende Textanalyse) weniger flexibel bewältigen.
- Einschränkungen: Begrenzte Konfigurierbarkeit bei flexiblen Toleranz-Leveln. Eine genaue Bewertung von Rechen- und Schätzfragen erfordert zusätzliche Logik.
(300€ Guthaben für 90 Tage)

#### Anthropic Claude:
- Stärken: Claude bietet solide Flexibilität für Synonym- und Kontextanalyse, Fehler-Toleranz und Freitextbewertungen. Es kann „fuzzy“ Abgleiche durchführen, was bei variablen Antworten hilfreich ist.
- Einschränkungen: Anpassung der Toleranz-Level könnte im Vergleich zu anderen Modellen begrenzter sein, sodass die Backend-Logik hier mehr eingreifen müsste.

#### Microsoft Azure Cognitive Services:
- Stärken: Microsofts Modelle erkennen Tippfehler, Synonyme und einfache grammatische Fehler. Sie können gut bei kurzen, präzisen Antwortformaten helfen und lassen sich für grundlegende Synonymanalyse und Bewertung nutzen.
- Einschränkungen: Azure bietet weniger Flexibilität für komplexe Textanalysen und könnte Einschränkungen bei Freitext- und Schätzfragen haben, da es keine ausgeprägte generative Komponente hat.

#### Cohere API:
- Stärken: Cohere-Modelle bieten eine zuverlässige Erkennung von Synonymen, Wort-Varianten und können für flexible Antwortbewertung gut angepasst werden.
- Einschränkungen: Die Modelle haben in der Regel weniger tiefgehende Textanalysefähigkeiten und könnten bei Freitextbewertungen in größeren, variableren Kontexten -Schwierigkeiten haben.

#### Hugging Face API (z.B. T5, GPT-Neo):
- Stärken: Die Hugging Face-Modelle bieten eine gute Anpassungsfähigkeit und kostenfreundliche Optionen, die Synonyme und einfache Fehler erkennen können. Viele Modelle sind für einfache Bewertungsaufgaben ideal.
- Einschränkungen: Oft weniger ausgeprägte Genauigkeit bei komplexeren Analysen (wie Schätzfragen), sodass eine Kombination mit Backend-Logik für höhere Präzision nötig sein könnte.
