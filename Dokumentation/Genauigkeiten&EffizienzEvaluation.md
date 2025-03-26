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
  - Werden Synonyme akzeptiert

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
- Beispiel: Warum halten manche Tiere Winterschlaf?
- Erwartete Antwort: Manche Tiere halten Winterschlaf, um Energie zu sparen, weil im Winter Nahrung knapp ist. Ihr Stoffwechsel verlangsamt sich, die Körpertemperatur sinkt, und sie überleben mit gespeicherten Reserven.
- Bewertungskriterien:
  - Kann die KI den Inhalt richtig interpretieren?
  - Erkennt sie kleine Fehler und falsche Aussagen?
  - Gibt es Teilpunkte für teilweise korrekte Antworten?


# 4. Standardisierte Prompts für die Tests

Allgemeiner Prompt:

```
Gegeben sind eine Frage, eine Antwort auf die Frage und eine Erwartete richtige Antwort.
Bei machen Fragetypen gibt es noch Parameter, Antwortmöglichkeit und Unterschiedliche Kriterien.
Bitte bewerte die gegebene Antwort mit einer Punktzahl von 0 bis 100 mithilfe der gegebenen Bewertungskriterien und gib die Antwort im gegebenen Format aus.
Falls nötig, gib dem User ein kurzes Feedback für die Bewertung in "Begründung", da dies dem User helfen soll zu verstehen wo seine Fehler liegen.
Gehe beim der Begründung nicht ausschließlich auf die Bewertungskriterien ein. Gibt ein kurzes benutzerfreundliches Feedback!

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
**!!Prompt wurde mehrfach geändert, da ChatGPT und DeepSeek unkonstruktives Feedback gegeben haben**  
Vorallem ab des Ein-Wort-Antworten Fragentyps, kann es zu Änderungen des Outputs kommen zu vorherigen Fragen. 

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

## Ein-Wort-Antworten Prompts

**Frage:** [Wie heißt die chemische Formel für Wasser?]  
**Antwort:** [H2O]  
**Antwort Typ** [Ein-Wort-Antwort]  
**Parameter von Typen** []  
**Erwartete richtige Antwort:** [H₂O]  
**Bewertungskriterien:**  
- Ist die Antwort korrekt?
- Erlaube Rechtschreibfehler, so lange sie die Antwort nicht zu stark ändern
- Synonyme, die im Kontext korrekt sind, sollen auch akzeptiert werden

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}

---------------------------

**Frage:** [Wer schrieb das Buch „Faust“?]  
**Antwort:** [Göthe]  
**Antwort Typ** [Ein-Wort-Antwort]  
**Parameter von Typen** []  
**Erwartete richtige Antwort:** [Goethe]  
**Bewertungskriterien:**  
- Ist die Antwort korrekt?
- Erlaube Rechtschreibfehler, so lange sie die Antwort nicht zu stark ändern
- Synonyme, die im Kontext korrekt sind, sollen auch akzeptiert werden

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}

-------------------------------

**Frage:** [Wie nennt man das größte Organ des menschlichen Körpers?]  
**Antwort:** [Leber]  
**Antwort Typ** [Ein-Wort-Antwort] 
**Parameter von Typen** []  
**Erwartete richtige Antwort:** [Haut]  
**Bewertungskriterien:**  
- Ist die Antwort korrekt?
- Erlaube Rechtschreibfehler, so lange sie die Antwort nicht zu stark ändern
- Synonyme, die im Kontext korrekt sind, sollen auch akzeptiert werden

Antwort bitte in diesem Format:
{
  "Punktzahl": X,
  "Begründung": "..."
}

## Rechenaufgaben Promts

**Frage:** [Ein Schüler hat 12 Äpfel. Er möchte sie gleichmäßig auf 4 Schüsseln verteilen. Wie viele Äpfel kommen in jede Schüssel?]  
**Antwort:** [3,5]  
**Antwort Typ** [Rechenaufgaben]  
**Parameter von Typen** []  
**Erwartete richtige Antwort:** [3]  
**Bewertungskriterien:**  
- Erkennt die KI korrekte und falsche Antworten?
- Kann die KI mathematische Aufgaben (Division) lösen oder bewerten?
- Wird auf richtige Einheiten, Zahlenformat und Vollständigkeit geachtet?

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

# 5. Ergebnisdokumentation & Analyse

## ChatGPT 

### 1. Multiple-Choice 

Photosynthese
```JSON
{
  "Punktzahl": 0,
  "Begründung": "Die gegebene Antwort 'Zellkern' ist falsch, da die Photosynthese in den Chloroplasten stattfindet. Die richtige Antwort wäre 'Chloroplasten' gewesen."
}
```
Hauptstadt Frankreich
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die gegebene Antwort 'Paris' entspricht genau der erwarteten richtigen Antwort. Daher wird die volle Punktzahl vergeben."
}
```

### 2. Ein-Wort-Antworten

Wasser
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die Antwort 'H2O' ist korrekt und entspricht der chemischen Formel für Wasser. Der Unterschied zur erwarteten Antwort 'H₂O' liegt nur in der Schreibweise der tiefgestellten Zahl, was keinen inhaltlichen Unterschied macht."
}
```

Goethe
```JSON
{
  "Punktzahl": 90,
  "Begründung": "Die Antwort ist fast korrekt, jedoch enthält sie einen kleinen Rechtschreibfehler ('Göthe' statt 'Goethe'). Da die Bedeutung der Antwort nicht verändert wird und klar erkennbar ist, was gemeint war, gibt es nur einen kleinen Abzug."
}
```

Haut
```JSON
{
  "Punktzahl": 0,
  "Begründung": "Die gegebene Antwort 'Leber' ist leider falsch. Das größte Organ des menschlichen Körpers ist die 'Haut'. Bitte überprüfe die Antwort noch einmal."
}
```

### 3. Rechenfragen

Äpfel
```JSON
{
"Punktzahl": 10,
"Begründung": "Die Antwort '3,5' ist leider falsch, da 12 Äpfel gleichmäßig auf 4 Schüsseln verteilt genau 3 Äpfel pro Schüssel ergibt. Die Division 12 ÷ 4 = 3 ist eine einfache Rechenaufgabe, bei der die Antwort eine ganze Zahl sein muss, weil man keine halben Äpfel verteilt, wenn nichts dergleichen erwähnt ist. Gut wäre es, die Grundrechenarten sicher zu beherrschen."
}
```

## DeepSeek

### 1. Multiple-Choice

Photosynthese
```JSON
{
"Punktzahl": 0,
"Begründung": "Die gegebene Antwort 'Zellkern' entspricht nicht der erwarteten richtigen Antwort 'Chloroplasten'. Die Photosynthese findet in den Chloroplasten der Pflanzenzelle statt, nicht im Zellkern."
}
```

Hauptstadt Frankreich
```JSON
{
"Punktzahl": 100,
"Begründung": "Die gegebene Antwort 'Paris' entspricht der erwarteten richtigen Antwort 'Paris'. Die Antwort ist korrekt und vollständig."
}
```

### 2. Ein-Wort-Antworten

Wasser
```JSON
{
"Punktzahl": 100,
"Begründung": "Die gegebene Antwort 'H2O' ist korrekt und entspricht der erwarteten richtigen Antwort 'H₂O'. Die Verwendung von '2' anstelle des tiefgestellten '₂' ist in diesem Kontext akzeptabel, da es die chemische Formel nicht verändert und die Lesbarkeit nicht beeinträchtigt. Die Antwort ist somit vollständig richtig."
}
```

Goethe  
**Mehrfaches testen führte zu stark abweichenden Ergebnissen!!!**  
Mal 80 Punkte, mal 50 Punkte

```JSON
{
"Punktzahl": 80,
"Begründung": "Die Antwort 'Göthe' ist fast korrekt, enthält jedoch einen kleinen Rechtschreibfehler. Der korrekte Name des Autors ist 'Goethe'. Der Fehler ändert die Bedeutung der Antwort nicht wesentlich, da die Aussprache und der Kontext klar erkennbar sind. Dennoch ist die Schreibweise nicht vollständig korrekt, daher gibt es einen kleinen Punktabzug."
}
```

Haut
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort 'Leber' ist leider nicht korrekt. Das größte Organ des menschlichen Körpers ist die Haut. Die Leber ist zwar ein wichtiges Organ, aber nicht das größte. Achte darauf, bei solchen Fragen die genaue Bezeichnung des gesuchten Organs zu verwenden."
}
```

### 3. Rechenfragen

Äpfel
```JSON
{
"Punktzahl": 0,
"Begründung": "Die gegebene Antwort '3,5' ist falsch, da 12 Äpfel gleichmäßig auf 4 Schüsseln verteilt genau 3 Äpfel pro Schüssel ergeben. Es gibt keine halben Äpfel in dieser Aufgabe. Achte darauf, dass du bei solchen Aufgaben immer prüfst, ob das Ergebnis sinnvoll und ganzzahlig ist."
}
```

## Gemini

### 1. Multiple Choice 

Photosynthese
```JSON
{
  "Punktzahl": 0,
  "Begründung": "Die Photosynthese findet in den Chloroplasten statt, nicht im Zellkern. Der Zellkern ist für die Speicherung und Verarbeitung der genetischen Information zuständig."
}
```

Hauptstadt Frankreich
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die Antwort 'Paris' ist die korrekte Hauptstadt von Frankreich und entspricht somit der erwarteten richtigen Antwort."
}
```

### 2. Ein-Wort-Antworten

Wasser
```JSON
{
 "Punktzahl": 100,
 "Begründung": "Die Antwort ist perfekt! Die chemische Formel für Wasser ist H₂O (mit tiefgestellter 2). Deine Antwort H2O ist zwar nicht ganz korrekt formatiert, aber da es sich um eine Ein-Wort-Antwort handelt und die Bedeutung klar ist, gebe ich dir trotzdem die volle Punktzahl. Super gemacht!"
}
```

Goethe
```JSON
{
 "Punktzahl": 90,
 "Begründung": "Fast perfekt! Die Antwort ist inhaltlich richtig, aber der Nachname wird im Deutschen üblicherweise 'Goethe' geschrieben, nicht 'Göthe'. Da es sich aber nur um einen kleinen Rechtschreibfehler handelt, der die Bedeutung nicht verfälscht, gibt es fast die volle Punktzahl. Sehr gut!"
}
```

Haut
```JSON
{
 "Punktzahl": 0,
 "Begründung": "Die Antwort ist leider falsch. Das größte Organ des menschlichen Körpers ist die Haut und nicht die Leber. Die Leber ist zwar ein sehr wichtiges Organ und auch das größte innere Organ, aber die Haut übertrifft sie in ihrer Fläche um ein Vielfaches. Daher ist die Antwort 'Leber' nicht korrekt."
}
```

### 3. Rechenfragen

Äpfel
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ist komplett falsch. Der Schüler hat wahrscheinlich versucht, die Äpfel durch die Schüsseln zu teilen, hat dies jedoch falsch berechnet. Zudem kann man Äpfel nicht in Dezimalzahlen aufteilen."
}
```

## Cohere

### 1. Multiple-Choice

Photosynthese
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort entspricht nicht der Lösung. Die Photosynthese findet in den Chloroplasten statt, nicht im Zellkern. Bitte überprüfen Sie Ihre Antwort und versuchen Sie es noch einmal."
}
```

Hauptstadt Frankreich
```JSON
{
"Punktzahl": 100,
"Begründung": "Die Antwort entspricht der Lösung. Paris ist die Hauptstadt von Frankreich. Gut gemacht!"
}
```

### 2. Ein-Wort-Antworten

Wasser
```
{
"Punktzahl": 100,
"Begründung": "Die Antwort ist korrekt. Die chemische Formel für Wasser lautet tatsächlich H₂O. Ihre Antwort H2O ist eine akzeptable Schreibweise, da der Unterschied nur in der Darstellung des Index liegt, was in diesem Kontext vernachlässigbar ist. Gut gemacht!"
}
```

Goethe
```JSON
{
"Punktzahl": 80,
"Begründung": "Die Antwort ist fast korrekt. Der Autor von ‚Faust‘ ist tatsächlich Goethe, nicht ‚Göthe‘. Der kleine Rechtschreibfehler (das fehlende ‚e‘) ändert die Bedeutung nicht stark, aber es ist wichtig, den Namen korrekt zu schreiben. Gut versucht – achten Sie beim nächsten Mal auf die genaue Schreibweise!"
}
```

Haut
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ist leider nicht korrekt. Das größte Organ des menschlichen Körpers ist die Haut, nicht die Leber. Die Leber ist zwar ein wichtiges Organ, aber sie ist viel kleiner als die Haut. Versuchen Sie es beim nächsten Mal mit der richtigen Antwort – die Haut bedeckt den gesamten Körper und ist daher das größte Organ!"
}
```

### 3. Rechenfragen

Äpfel
```JSON
{
"Punktzahl": 50,
"Begründung": "Die Antwort [3,5] ist teilweise korrekt, da die Division von 12 Äpfeln durch 4 Schüsseln tatsächlich 3 Äpfel pro Schüssel ergibt. Allerdings ist die Angabe von ,5 (also 0,5) nicht sinnvoll, da man keine halben Äpfel verteilen kann. Die erwartete richtige Antwort ist [3], da die Äpfel gleichmäßig verteilt werden und keine Reste übrig bleiben. Die mathematische Berechnung ist korrekt, aber die Angabe von ,5 führt zu einem Abzug in der Punktzahl."
}

{
"Punktzahl": 0,
"Begründung": "Die Antwort [3,5] ist falsch, da bei der gleichmäßigen Verteilung von 12 Äpfeln auf 4 Schüsseln in jede Schüssel genau 3 Äpfel kommen (12 ÷ 4 = 3). Die Angabe von ,5 (also 0,5) ist nicht korrekt, da man keine halben Äpfel verteilen kann. Die Antwort berücksichtigt nicht die Vollständigkeit und das richtige Zahlenformat. Die erwartete richtige Antwort ist [3]."
}
```
Zwei sehr unterschiedliche Antworten!


# 6. Fazit & Nächste Schritte
