# Evaluation der Kosten der AI APIs

## Fragen

### Hintergrund zur Token-Metrik

Ein Token kann ein Wort, ein Satzzeichen oder ein Teil eines Wortes sein. In der Praxis entsprechen:

- 1.000 Tokens etwa 750–800 Wörtern in englischem Text.
- Deutsche Texte benötigen tendenziell mehr Tokens, da deutsche Wörter oft länger sind.

### Schätzung für typische Anfragen

Im Kontext des Projekts umfasst die Kommunikation mit dem Modell in einer Anfrage mehrere Komponenten:

- **Fragetext**: Der eigentliche Inhalt der Frage (z. B. eine Multiple-Choice-Frage oder ein Freitext). Ein typischer Fragetext könnte zwischen 50–150 Tokens umfassen.
- **Kontext**: Zusätzliche Informationen, die das Modell benötigt, um die Frage zu bewerten, wie:
  - Vorherige Fragen (z. B. bei sequenziellen Aufgaben oder personalisierten Lernpfaden).
  - Bewertungsrichtlinien (z. B. Synonym-Erkennung oder Groß-/Kleinschreibung beachten).
  - Beispielantworten, um den Bewertungsprozess zu veranschaulichen.
  - Benutzerdefinierte Einstellungen (z. B. Toleranz für Tippfehler).
- **Token-Overhead durch APIs**: Der API-Aufruf und technische Zusatzinformationen (z. B. JSON-Formatierung, Tokenisierung der Anfrage) erhöhen die Token-Anzahl.

Für realistische Szenarien ergibt sich daher:

- **Einfache Anfragen** (z. B. Multiple-Choice): ~500–1.000 Tokens.
- **Komplexe Anfragen** (Freitext-Antworten mit Kontext): ~2.000–2.500 Tokens.

### Anpassung für unterschiedliche Fragetypen

| Fragetyp            | Geschätzte Tokens (Input) | Begründung                                                                                     |
|---------------------|---------------------------|------------------------------------------------------------------------------------------------|
| Multiple-Choice      | 500–800                  | Kurze Frage, wenige Optionen, minimaler Kontext.                                                |
| Ein-Wort-Antworten   | 500–1.000                | Fragetext + zusätzliche Hinweise, wie Synonyme oder Groß-/Kleinschreibung zu behandeln.         |
| Rechenfragen         | 700–1.500                | Komplexere Fragestellung + Erläuterung, wie numerische Toleranzen angewendet werden.             |
| Entweder/Oder        | 500–800                  | Einfacher Entscheidungsbaum, wenig Kontext erforderlich.                                        |
| Schätzfragen         | 1.000–1.500              | Frage + Definition der Toleranzspannen (z. B. +/-10%) + mögliche Beispielantworten.              |
| Lückentextfragen     | 1.500–2.000              | Fragetext + mehrere mögliche richtige Antworten für Lücken + Erläuterung für Synonyme und Tippfehler. |
| Freitextantworten    | 2.000–2.500              | Frage + umfangreicher Kontext + mögliche Zwischenschritte bei der Beurteilung.                   |

## Benutzer Antworten

### Typische Token-Anzahl für Antworten pro Fragetyp

Die verschiedenen Fragetypen haben unterschiedliche Antwortlängen:

| Fragetyp            | Geschätzte Tokens (Input)  | Begründung                                                                                     |
|---------------------|----------------------------|------------------------------------------------------------------------------------------------|
| Multiple-Choice      | 10–50                     | Die Antwort besteht in der Regel nur aus einer Auswahl (z. B. "A" oder "A, C"). Falls Begründungen benötigt werden, könnte es bis zu 50 Tokens umfassen. |
| Ein-Wort-Antworten   | 5–20                      | Einfache, kurze Antworten, z. B. "Haus" oder "42".                                              |
| Rechenfragen         | 50–150                    | Antwort enthält oft eine Zahl, aber je nach Einstellung könnte auch eine Schritt-für-Schritt-Lösung generiert werden (z. B. Rechenweg). |
| Entweder/Oder        | 10–50                     | "Ja" oder "Nein", evtl. mit kurzer Begründung (z. B. "Ja, weil die Aussage korrekt ist").        |
| Schätzfragen         | 50–100                    | Antwort liegt oft im Bereich eines numerischen Wertes mit einer kurzen Begründung zur Akzeptanz. |
| Lückentextfragen     | 50–150                    | Füllt die Lücken mit Wörtern und könnte mehrere Varianten oder Synonyme vorschlagen.            |
| Freitextantworten    | 200–1.000                 | Längere Freitext-Antworten können komplexe Erklärungen oder Interpretationen erfordern. Der Tokenverbrauch variiert je nach Fragestellung. |

## Bewertung der AI

| **Fragetyp**          | **Geschätzte Tokens (Output)** |
|-----------------------|--------------------------------|
| **Multiple-Choice**   | 15–30                          |
| **Ein-Wort-Antworten**| 10–20                          |
| **Rechenfragen**      | 80–150                         |
| **Entweder/Oder**     | 15–25                          |
| **Schätzfragen**      | 25–50                          |
| **Lückentextfragen**  | 150–250                        |
| **Freitextantworten** | 300–1.000                      |

- Einfache Bewertung: Die KI gibt nur eine Punktzahl oder eine kurze Bewertung aus (z. B. "Richtig", "Falsch", "Teilweise korrekt").
- Detaillierte Bewertung mit Feedback: Hier wird Feedback zur Begründung gegeben, mit Hinweisen auf Verbesserungspotenziale und ggf. alternative Lösungen.

### Token-Overhead durch die API

Neben der eigentlichen Antwort erzeugen APIs oft Zusatzinformationen (z. B. Metadaten, Antwortstruktur), die den Tokenverbrauch leicht erhöhen können.

## MVP

### Zielsetzung des MVPs

Das Ziel ist die Entwicklung eines Systems, das verschiedene Fragetypen bewertet und skalierbar ist. Ein MVP muss genügend Datenpunkte verarbeiten, um die Funktionalität unter realistischen Bedingungen zu testen.

### Typische Nutzerbasis im MVP

Ein MVP könnte mehrere Nutzergruppen simulieren, darunter Schüler, Lehrer oder andere Tester:

- **Anzahl der Nutzer**: Angenommen werden 100 Testpersonen, die das System nutzen.
- **Fragen pro Nutzer**: Jede Person beantwortet im Durchschnitt 100 Fragen, was typische Nutzungsszenarien abdeckt (z. B. Tests, Übungen).

### Verteilung auf Fragetypen

Die Fragetypen sind unterschiedlich komplex und haben variierende Token-Anforderungen. Für das MVP wird davon ausgegangen, dass die Fragetypen gleichmäßig getestet werden, um ihre Funktionalität zu validieren.

| Parameter              | Annahme                  |
|------------------------|--------------------------|
| Anzahl der Testpersonen | 100                      |
| Fragen pro Person       | 100                      |
| Gesamte Anzahl der Fragen | 100×100=10.000         |

### Flexibilität in der Schätzung

Die Anzahl von 10.000 Anfragen wurde bewusst großzügig gewählt, um eine ausreichende Datengrundlage für die Evaluation zu gewährleisten. Je nach realer Nutzerbeteiligung könnte die tatsächliche Anzahl variieren, aber sie bietet genug Spielraum für:

- Skalierungstests.
- Fehlertoleranzen (z. B. bei unerwartet hohen Tokenkosten).
- Variationen im Nutzerverhalten.

### Zusammenfassung der Annahmen

- **2.000 Tokens pro Frage und Antwort**: Durchschnitt basierend auf den Fragetypen und zusätzliche benötigter Informationen, die das Modell benötigt, ebenso wie die Antwort, welche zu bewerten ist.
- **500 Tokens pro Bewertung**: Durchschnitt basierend auf den Fragetypen und der erwarteten Bewertungskomplexität, inkl. Begründungen und Feedback.
- **10.000 Anfragen im MVP**: Realistischer Umfang für umfangreiche Tests, basierend auf einer Nutzerbasis von 100 Personen, die je 100 Fragen bearbeiten.

## Kostenschätzung

### Annahmen

Mit den zuvor gemachten Annahmen werden wir nun die Berechnungen für die durchschnittlichen Kosten der unterschiedlichen APIs ausführen:

1. **Input-Tokens pro Anfrage**: 2.000 (z.B. inklusive Frage, Antwort, Kontext, Benutzereingabe).
2. **Output-Tokens pro Bewertung**: 500 (z.B. KI-generierte Bewertung).
3. **Gesamtanzahl der Anfragen**: 10.000 Anfragen im MVP.

### Schritte

#### 1. Berechnung der gesamten Token-Menge

- **Input-Tokens Gesamt**:  
  10.000 Anfragen × 2.000 Tokens = 20.000.000 = 20 MTok
- **Output-Tokens Gesamt**:  
  10.000 Anfragen × 500 Tokens = 5.000.000 Tokens = 5 MTok

#### 2. Berechnung der Kosten pro API

Da das MVP kosteneffizient entwickelt werden soll, werden die Berechnungen nur für die Kosten der niedrigsten Versionen durchgeführt.

- **OpenAI (gpt-4o-mini)**
  - Input-Kosten: 20 MTok × 0,15 USD = 3,00 USD
  - Output-Kosten: 5 MTok × 0,60 USD = 3,00 USD
  - Gesamtkosten: 3,00 + 3,00 = 6,00 USD

- **Gemini Flash 1.5**
  - Input-Kosten: 20 MTok × 0,075 USD = 1,50 USD
  - Output-Kosten: 5 MTok × 0,30 USD = 1,50 USD
  - Gesamtkosten: 1,50 + 1,50 = 3,00 USD

- **Claude 3.5 Haiku**
  - Input-Kosten: 20 MTok × 1 USD = 20 USD
  - Output-Kosten: 5 MTok × 5 USD = 25 USD
  - Gesamtkosten: 20 + 25 = 45 USD

- **Azure (GPT-4o-mini)**
  - Input-Kosten: 20 MTok × 0,15 USD = 3,00 USD
  - Output-Kosten: 5 MTok × 0,61 USD = 3,05 USD
  - Gesamtkosten: 3,00 + 3,05 = 6,05 USD

- **Cohere (Command R)**
  - Input-Kosten: 20 MTok × 0,15 USD = 3,00 USD
  - Output-Kosten: 5 MTok × 0,60 USD = 3,00 USD
  - Gesamtkosten: 3,00 + 3,00 = 6,00 USD

### API-Kostenübersicht

| API                  | Gesamtkosten (USD) |
|----------------------|-------------------|
| Gemini Flash 1.5      | 3,00              |
| OpenAI gpt-4o mini    | 6,00              |
| Cohere Command R      | 6,00              |
| Azure GPT-4o mini     | 6,05              |
| Claude 3.5 Haiku      | 45,00             |
| OpenAI gpt-4o         | 100,00            |
| Cohere Command R+     | 100,00            |
| Azure GPT-4o          | 101,95            |
| OpenAI o1 mini        | 120,00            |
| OpenAI o1 preview     | 600,00            |
| Azure o1 preview      | 611,20            |

### Schlussfolgerungen

- **Günstigste Option**: Gemini Flash 1.5 mit 3,00 USD.  
  OpenAI gpt-4o mini, Cohere Command R und Azure GPT-4o mini sind ebenfalls in einem akzeptablen Preisbereich.
- **Teuerste Option**: Azure o1 preview mit 611,20 USD.
- **Kosten für populäre Modelle**: OpenAI gpt-4o liegt bei 100,00 USD, während Claude 3.5 bei 45,00 USD liegt.
- Gamini flash 1.5 bietet jedoch auch eine kostenlose API an mit begrenzter Nutzung zum Testen mit folgenden Begrenzungen:
  - 15 Anfragen pro Minute 
  - 1 Million TPM (Tokens pro Minute)
  - 1.500 Anfragen pro Tag

