# Plan zur Evaluierung von KI-Systemen für die Bewertung von Antworten

## 1. Zielsetzung
Das Ziel ist es, unterschiedliche generative KI-Modelle hinsichtlich ihrer Genauigkeit, 
Effizienz und Kosten bei der Bewertung von Antworten in Lernsystemen zu vergleichen. 
Dazu werden standardisierte Fragen verwendet, die in die KI-Modelle eingespeist und deren Antworten analysiert werden.
   
## 2. Teststrategie
Um eine objektive Bewertung der KIs zu ermöglichen, wird eine strukturierte Teststrategie entwickelt:

### 2.1 Erstellung eines standardisierten Testsets

- Definition einer Liste von Testfragen für verschiedene Fragetypen (Multiple-Choice, Lückentext, Freitext etc.).
- Festlegung von erwarteten Antworten inklusive akzeptabler Variationen (z. B. Synonyme, Tippfehler, alternative Schreibweisen).
- Berücksichtigung unterschiedlicher Bewertungsansätze (z. B. binary correct/incorrect, Punktesystem, teilweise Korrektheit).

### 2.2 Testdurchführung 

- Verwendung einer standardisierten Prompt-Vorlage, um die KIs einheitlich zu testen.
- Manuelles Kopieren und Einfügen der Prompts in verschiedene KI-Systeme (z. B. ChatGPT, Gemini, Claude, Cohere).
- Sammeln und Speichern der KI-Antworten für spätere Analyse.

### 2.3 Manuelle Bewertung der KI-Antworten

- Vergleich der Antworten mit dem Erwartungswert.
- Kategorisierung der KI-Antworten nach:
  - Genauigkeit: Wie gut stimmen die Antworten mit der erwarteten Bewertung überein?
  - Flexibilität: Kann die KI Tippfehler, Synonyme oder alternative Schreibweisen berücksichtigen?
  - Konsistenz: Gibt die KI bei wiederholten Tests ähnliche oder stark abweichende Bewertungen?

### 2.4 Messung der Effizienz

- Zeitmessung, wie lange es dauert, bis jede KI eine Bewertung zurückliefert.
- Subjektive Einschätzung der Benutzerfreundlichkeit (z. B. ob eine KI besonders gut für den späteren Einsatz geeignet ist).


# 3. Testfälle

Die Tests basieren auf realistischen Szenarien für verschiedene Fragentypen. Jede Frage wird mit unterschiedlichen erwarteten Antworten und Variationen getestet.

### Testfall 1: Multiple-Choice-Fragen
- Beispiel: Was ist die Hauptstadt von Deutschland?
- Richtige Antwort: Berlin
- Falsche Antworten: München, Hamburg
- Bewertungskriterien:
  - Erkennt die KI die richtige Antwort?
  - Gibt es eine Teilbewertung für fast richtige Antworten?
  - Erkennt die KI Tippfehler (z. B. berln statt Berlin)?

### Testfall 2: Ein-Wort-Antworten
- Beispiel: Wie heißt die chemische Formel für Wasser?
- Richtige Antwort: H₂O, H2O
- Falsche Antworten: HO, H2O2
- Bewertungskriterien:
  - Erkennt die KI Formatierungen (z. B. Tiefstellen von Zahlen)?
  - Werden Synonyme oder alternative Schreibweisen berücksichtigt?

### Testfall 3: Rechenaufgaben
- Beispiel: Was ist 12 × 8?
- Richtige Antwort: 96
- Falsche Antworten: 94, 98
- Bewertungskriterien:
  - Erkennt die KI korrekte und falsche Antworten?
  - Ist die KI in der Lage Mathematische Aufgaben zu lösen?

### Testfall 4: Entweder/Oder-Fragen
- Beispiel: Ist die Erde rund? (Ja/Nein)
- Richtige Antwort: Ja
- Bewertungskriterien:
  - Akzeptiert die KI „ja“ und „Ja“ gleichermaßen?
  - Werden alternative Formulierungen („Ja, natürlich“) akzeptiert?

### Testfall 5: Schätzfragen
- Beispiel: Wie hoch ist der Eiffelturm?
- Rivhtige Antwort: 312m 
- Akzeptabler Bereich: 10% Abweichung (~280-340)
- Bewertungskriterien:
  - Wird eine Toleranz bei der Bewertung berücksichtigt?
  - Gibt die KI eine Begründung für die Bewertung?
  - Kann die KI die Abweichung berechnen oder muss der Bereich vorgegeben werden?
 
### Testfall 6: Lückentextfragen
- Beispiel: Die Sonne ist ein ___ (Stern/Planet).
- Erwartete Antwort: Stern
- Bewertungskriterien:
  - Erkennt die KI alternative Antworten (Synonyme)?
  - Kann sie Tippfehler tolerieren?
 
### Testfall 7: Freitextantworten
Beispiel: Erkläre das Prinzip der Relativitätstheorie in zwei Sätzen.
Erwartete Antwort: Die Relativitätstheorie beschreibt die Beziehung zwischen Raum und Zeit. 
Sie zeigt, dass sich die Zeit in starken Gravitationsfeldern verlangsamt.
Bewertungskriterien:
Kann die KI den Inhalt richtig interpretieren?
Erkennt sie kleine Fehler und falsche Aussagen?
Gibt es Teilpunkte für teilweise korrekte Antworten?


# 4. Standardisierte Prompts für die Tests

Allgemeiner Prompt:

```
Gegeben sind eine Frage, eine Antwort auf die Frage und eine Erwartete richtige Antwort.
Bei machen Fragetypen gibt es noch Parameter, Antwortmöglichkeit und Unterschiedliche Kriterien.
Bitte bewerte die gegebene Antwort mit einer Punktzahl von 0 bis 100 mithilfe der gegebenen Bewertungskriterien und gib die Antwort im gegebenen Format aus.
Falls nötig, gib dem User ein kurzes Feedback für die Bewertung.

**Frage:** [Hier die Testfrage einfügen]  
**Antwort:** [Hier die Nutzerantwort einfügen]  
**Antwort Typ** [Hier Typ einfügen] 
**Parameter von Typen** [Hier Parameter einfügen]
**Erwartete richtige Antwort:** [Hier die korrekte Antwort einfügen]  
**Bewertungskriterien:**  
- Ist die Antwort inhaltlich korrekt?  
- Falls nicht ganz korrekt, welche Abweichungen sind akzeptabel?  
- Ist die Antwort grammatikalisch richtig?  
- Ist die Antwort vollständig?

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}
```

## Multiple-Choice Prompts

**Frage:** [In welchem Teil der Pflanzenzelle findet die Photosynthese statt?]  
**Antwort:** [Zellkern]  
**Antwort Typ:** [Multiple-Choice]  
**Parameter von Typen:** [  
**Antwortmöglichkeiten:**[Zellkern, Mitochondrien, Chloroplasten, Ribosomen]  
]   
**Erwartete richtige Antwort:** [Chloroplasten]  
**Bewertungskriterien:**  
- Entspricht die Antwort der Lösung?

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}

--------------------------------------------------

**Frage:** [Wie lautet die Hauptstadt von Frankreich?]  
**Antwort:** [Paris]  
**Antwort Typ:** [Multiple-Choice]  
**Parameter von Typen:** [  
**Antwortmöglichkeiten:**[Berlin, Paris, Madrid, Prag]  
]   
**Erwartete richtige Antwort:** [Paris]  
**Bewertungskriterien:**  
- Entspricht die Antwort der Lösung?  

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}

# 5. Ergebnisdokumentation & Analyse

## ChatGPT 

### Multiple-Choice 

Photosynthese
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die gegebene Antwort 'Paris' entspricht genau der erwarteten richtigen Antwort. Daher wird die volle Punktzahl vergeben."
}
```
Hauptstadt Frankreich
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die gegebene Antwort 'Paris' entspricht genau der erwarteten richtigen Antwort. Daher wird die volle Punktzahl vergeben."
}
```
# 6. Fazit & Nächste Schritte
