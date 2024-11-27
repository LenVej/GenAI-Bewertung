# Anforderungskatalog

## Ziel

Das Ziel dieses Projekts ist die Entwicklung eines Minimum Viable Product (MVP), das den Einsatz von generativen KI-Technologien zur flexiblen und genauen Bewertung von Antworten in Lernsystemen ermöglicht. Dabei soll eine automatisierte Lösung geschaffen werden, die verschiedene Fragentypen unterstützt und in der Lage ist, Antworten präziser zu bewerten, indem sie Abweichungen berücksichtigt. Die Anwendung wird als Webanwendung implementiert, die eine benutzerfreundliche Oberfläche in Angular bietet, während das Backend auf ASP.NET Core basiert.

Ein zentraler Fokus liegt auf der Verbesserung der Bewertungsergebnisse durch den Einsatz von GenAI, um die aktuellen Herausforderungen bestehender Systeme zu überwinden, die oft korrekte Antworten fälschlicherweise als falsch bewerten. Durch eine umfassende Analyse und den Vergleich von GenAI-Technologien und traditionellen Algorithmen soll sichergestellt werden, dass Effizienz, Genauigkeit und Kosten optimal berücksichtigt werden. Langfristig strebt das Projekt an, adaptive Lernpfade und personalisierte Lernvorschläge für Schüler zu integrieren, um deren individuellen Lernbedürfnissen gerecht zu werden.

---

## 0. AI und Algorithmen vergleichen

Die Technologien sollen hinsichtlich ihrer Genauigkeit, Effizienz und Kosten evaluiert werden. Die folgenden Kriterien werden dabei berücksichtigt:

- **Genauigkeit**: Überprüfung, wie präzise die Antworten erkannt und bewertet werden, einschließlich der Berücksichtigung von Synonymen, Tippfehlern und flexiblen Antwortformaten.
- **Effizienz**: Analyse der Verarbeitungszeit und des Ressourcenverbrauchs, um zu gewährleisten, dass Antworten zeitnah und mit minimalem Ressourcenaufwand bewertet werden können.
- **Kosten**: Evaluierung der Betriebskosten für die Implementierung und den Einsatz der GenAI-Lösungen im Vergleich zu klassischen Algorithmen, unter Berücksichtigung möglicher Lizenzgebühren und anderer finanzieller Aufwendungen.


## 1. Funktionale Anforderungen

### Fragentypen

- **Ankreuzfragen (Multiple-Choice)**: Unterstützung für Fragen mit mehreren vorgegebenen Antwortmöglichkeiten, bei denen eine oder mehrere Antworten richtig sein können.
- **Ein-Wort-Antworten**: Bewertung von Fragen, bei denen nur ein einzelnes Wort erwartet wird (z.B. Vokabeln, Definitionen).
- **Rechenfragen**: Unterstützung für mathematische Fragen, bei denen der Nutzer eine Berechnung durchführen muss.
- **Entweder/Oder-Fragen**: Unterstützung für Fragen, bei denen zwei mögliche Antworten zur Auswahl stehen (z.B. "Ja" oder "Nein").
- **Schätzfragen**: Fragen, bei denen die Antwort geschätzt wird, und eine akzeptable Toleranzspanne definiert wird.
- **Lückentextfragen**: Möglichkeit, Fragen mit Lücken zu erstellen, in die Nutzer passende Wörter einsetzen müssen.
- **Freitextantworten**: Analyse von längeren Freitextantworten mit GenAI, um eine präzisere Auswertung und Interpretation von Texten zu ermöglichen.


## 2. Generative AI-basierte Bewertung

### Flexibles Antwortmatching

Implementierung einer GenAI-Technologie, die flexible Abweichungen akzeptiert, wie z.B.:

- **Toleranz bei Tippfehlern**: Erkennung und Korrektur von Rechtschreibfehlern (z.B. "haUs" statt "Haus").
- **Groß-/Kleinschreibung**: Akzeptanz von Antworten unabhängig von Groß- und Kleinschreibung (z.B. "Haus" = "haus").
- **Synonyme und ähnliche Begriffe**: Die AI sollte in der Lage sein, gängige Synonyme und ähnliche Begriffe als korrekte Antworten zu akzeptieren.
- **Toleranz bei Schätzfragen**: Möglichkeit, eine Toleranzspanne für die Bewertung von Schätzfragen zu definieren (z.B. eine Antwort innerhalb von +/- 10% wird als richtig gewertet).


## 3. Usereinstellungen

### Bewertungsoptionen

Nutzer sollen verschiedene Bewertungseinstellungen festlegen können, z.B.:

- **Toleranzlevel für Tippfehler**: Einstellbar, wie viele und welche Art von Fehlern in der Antwort erlaubt sind.
- **Genauigkeit für Schätzfragen**: Anpassbare Toleranzgrenzen für Schätzfragen.
- **Groß-/Kleinschreibung berücksichtigen**: Auswahloption, ob Groß-/Kleinschreibung berücksichtigt werden soll.


## 4. Technologien

### Frontend

- **Angular**: Benutzeroberfläche, die es den Nutzern ermöglicht, Antworten einzugeben und Bewertungsergebnisse anzuzeigen.

### Backend

- **ASP.NET Core**: Verarbeitung und Bewertung der Antworten.
- **GenAI-Integration**: Einbindung einer GenAI-Plattform (z.B. OpenAI, GPT-basierte Lösungen) zur Bewertung der Antworten.
- **Swagger**: Einfaches testen der API Endpunkte

### Datenbank

- **PostgreSQL**: Speicherung der Fragentypen, Nutzerantworten, Bewertungsergebnisse und Benutzereinstellungen.

### REST-APIs

- APIs zur Kommunikation zwischen Frontend und Backend, um Antworten zu übermitteln und Bewertungsergebnisse abzurufen.

### Tests

- **Unit Tests**: Mit xUnit werden isolierte Tests auf der Backend-Ebene implementiert.
- **End-to-End (E2E) Tests**: Einsatz von Cypress zur Simulation von Benutzerinteraktionen im Browser.


## 5. Zukünftige Erweiterungen (Ideen)

- **Adaptive Lernpfade**: Personalisierte Lernvorschläge basierend auf den Bewertungen.
- **Feedback-Mechanismen**: Spezifisches Feedback zu teilrichtigen oder falschen Antworten.
- **Lernfortschrittsüberwachung**: Verfolgung des Lernfortschritts der Schüler über die Webanwendung.
- **Mehrsprachigkeit**: Unterstützung für mehrere Sprachen, um internationale Nutzbarkeit zu gewährleisten.
- **Integration von Sprach- und Bilderkennung**: Erweiterung des Prototyps für Sprachantworten und Bildanalysen (z.B. handschriftliche Rechenaufgaben).
