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
**Antwort Typ** [Rechenaufgabe]  
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

-------------------------------

**Frage:** [Ein Rechteck hat eine Länge von 12 cm und eine Breite von 7 cm. Wie groß ist der Flächeninhalt?]  
**Antwort:** [96cm]  
**Antwort Typ** [Rechenaufgabe]  
**Parameter von Typen** []  
**Erwartete richtige Antwort:** [84cm²]  
**Bewertungskriterien:**  
- Wurde die Rechnung korrekt durchgeführt?
- Ist die Antwort korrekt, auch wenn keine Einheit angegeben wurde?
- Werden alternative Formate akzeptiert? (z. B. „84 cm“ vs. „84“)

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

## Entweder-Oder-Fragen Prompts

**Frage:** [Ist der Eiffelturm höher als der Kölner Dom?]  
**Antwort:** [Nein]  
**Antwort Typ:** [Entweder/Oder]  
**Parameter von Typen:** []  
**Erwartete richtige Antwort:** [Ja]  
**Bewertungskriterien:**  
- Ist die Antwort inhaltlich korrekt?  
- Werden alternative Schreibweisen berücksichtigt (z. B. „ja“, „Ja, natürlich“, „nein“)?  
- Sind kleinere sprachliche Abweichungen akzeptabel?

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

## Schätzfragen Prompts

**Frage:** [Wie hoch ist der Eiffelturm?]  
**Antwort:** [350m]  
**Antwort Typ:** [Schätzfrage]  
**Parameter von Typen:** [  
**Akzeptabler Bereich:** [±10%]  
]  
**Erwartete richtige Antwort:** [330m]  
**Bewertungskriterien:**  
- Wird eine Toleranz bei der Bewertung berücksichtigt?  
- Gibt die KI eine Begründung für die Bewertung?  
- Kann die KI die Abweichung berechnen oder muss der Bereich vorgegeben werden?

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

## Lückentextfragen Prompts

### Lückentextfrage mit mehreren Lücken: Bewertungs-Prompt

**Frage:** [Die Photosynthese findet in den ___ der Pflanzenzelle statt. Dabei wird mit Hilfe von ___ und Lichtenergie ___ in Glukose umgewandelt.]  
**Antwort:** [Chloroplasten, Wasser, Kohlendioxid]  
**Antwort Typ:** [Lückentextfrage]  
**Parameter von Typen:** [
**Antwortmöglichkeiten:**[Wasser, Sonne, Chloroplasten, Zellkern, Kohlendioxid, Sauerstoff]
]  
**Erwartete richtige Antwort:** [Chloroplasten, Wasser, Kohlendioxid]  
**Bewertungskriterien:**  
- Sind alle Lücken korrekt und fachlich richtig ausgefüllt?  
- Werden alternative Begriffe akzeptiert (z. B. „CO₂“ statt „Kohlendioxid“)?  
- Werden kleinere Schreibfehler toleriert?  
- Kann die KI teilweise korrekte Antworten differenziert bewerten?

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

-------------------------------

**Frage:** [Im Jahr ___ begann der Zweite Weltkrieg mit dem Überfall Deutschlands auf ___. Der Krieg endete im Jahr ___ mit der Kapitulation ___.]  
**Antwort:** [1936, Polen, 1945, Russlands]  
**Antwort Typ:** [Lückentextfrage]  
**Parameter von Typen:** [
**Antwortmöglichkeiten:**[1936, 1939, 1945, Polen, Russlands, Deutschlands, Tschechien]
]  
**Erwartete richtige Antwort:** [1939, Polen, 1945, Deutschlands]  
**Bewertungskriterien:**  
- Sind alle Lücken historisch korrekt ausgefüllt?  
- Werden alternative Schreibweisen akzeptiert (z. B. „BRD“ oder „NS-Deutschland“ für „Deutschland“)?  
- Können kleine Tippfehler oder Datumsformate (z. B. „'39“) toleriert werden?  
- Gibt es eine Teilbewertung bei teilweise richtigen Angaben?

Antwort bitte in diesem Format:  
{  
  "Punktzahl": X,  
  "Begründung": "..."  
}  

## Freitextantworten Prompts

**Frage:** [Erkläre, warum der Treibhauseffekt sowohl für das Leben auf der Erde notwendig als auch in seiner verstärkten Form problematisch ist.]  
**Antwort:** [Der Treibhauseffekt sorgt dafür, dass die Erde warm genug bleibt, damit Leben existieren kann. Wenn er aber zu stark wird, weil zu viele Gase wie CO₂ ausgestoßen werden, wird es zu heiß, was zum Klimawandel führt.]  
**Antwort Typ:** [Freitextantwort]  
**Parameter von Typen:** []  
**Erwartete richtige Antwort:** [Der natürliche Treibhauseffekt ist entscheidend für das Leben auf der Erde, da er die durchschnittliche Temperatur auf einem Niveau hält, das flüssiges Wasser und damit Leben ermöglicht. In seiner verstärkten Form – verursacht durch den Anstieg menschengemachter Treibhausgase wie CO₂ und Methan – führt er jedoch zur globalen Erwärmung. Diese bringt schwerwiegende Folgen mit sich, etwa häufigere Extremwetterereignisse, das Abschmelzen von Gletschern und den Anstieg des Meeresspiegels.]  
**Bewertungskriterien:**  
- Wird der Unterschied zwischen natürlichem und verstärktem Treibhauseffekt korrekt dargestellt?  
- Werden die positiven und negativen Aspekte nachvollziehbar erklärt?  
- Sind wissenschaftlich relevante Begriffe korrekt verwendet (z. B. „Treibhausgase“, „globale Erwärmung“)?  
- Ist die Antwort logisch aufgebaut und sprachlich klar formuliert?  
- Gibt es sachliche Fehler, Auslassungen oder vereinfachende Verkürzungen?  
- Wird bei teilrichtigen Antworten differenziert bewertet?

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

Fläche
```JSON
{
"Punktzahl": 10,
"Begründung": "Die Antwort ist leider falsch. Der Flächeninhalt eines Rechtecks berechnet sich aus Länge mal Breite, also 12 cm × 7 cm = 84 cm². Die Antwort 96 cm ist daher ein Rechenfehler. Außerdem fehlt die korrekte Einheit (cm²). Achte beim nächsten Mal darauf, die richtige Formel zu verwenden und das Ergebnis zu überprüfen."
}
```

### 4. Entwerder-oder-Fragen

Eiffelturm
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ist inhaltlich falsch – der Eiffelturm ist mit etwa 330 Metern deutlich höher als der Kölner Dom mit rund 157 Metern. Da es sich um eine Entweder/Oder-Frage handelt, kommt es allein auf die inhaltliche Richtigkeit an, alternative Formulierungen sind hier nicht relevant. Achte beim nächsten Mal einfach darauf, kurz zu prüfen, welche Struktur wirklich höher ist – das hilft schon enorm!"
}
```

### 5. Schätzfrage

Höhe
```JSON
{
"Punktzahl": 80,
"Begründung": "Deine Antwort liegt mit 350 m nur knapp außerhalb des akzeptablen Bereichs von ±10 % (297 m bis 363 m). Allerdings wäre der richtige Wert für den Eiffelturm etwa 330 m, was deine Schätzung ziemlich nah dran macht. Gute Arbeit – kleine Abweichung, aber insgesamt eine solide Schätzung!"
}
{
"Punktzahl": 91,
"Begründung": "Deine Schätzung von 350 m liegt nur 6,1 % über der erwarteten Höhe von 330 m und damit im akzeptablen Bereich von ±10 %. Die Antwort ist also sehr nah dran – gut geschätzt! Für eine perfekte Punktzahl hätte eine noch genauere Angabe (z. B. 330 m oder 331 m) gesorgt."
}
```
Zwei ambivalente Aussagen!!

### 6. Lückentextfragen

Zelle
```JSON
{
"Punktzahl": 100,
"Begründung": "Super gemacht! Alle Lücken wurden korrekt und fachlich richtig ausgefüllt: Die Photosynthese findet in den Chloroplasten statt, dabei wird Wasser zusammen mit Kohlendioxid und Lichtenergie in Glukose umgewandelt. Es gibt nichts zu beanstanden!"
}
```

WWII
```JSON
{
"Punktzahl": 50,
"Begründung": "Du hast zwei von vier Lücken korrekt ausgefüllt – 'Polen' und '1945' sind richtig. Der Beginn des Zweiten Weltkriegs war jedoch 1939, nicht 1936. Auch endete der Krieg mit der Kapitulation Deutschlands, nicht Russlands. Es ist gut, dass du Polen und das Endjahr korrekt erkannt hast, aber achte bitte auf die historischen Details, besonders bei solch zentralen Ereignissen."
}
```

### 7. Freitextfragen

Treibhauseffekt 
```JSON
{
"Punktzahl": 80,
"Begründung": "Die Antwort ist klar formuliert und trifft die grundlegende Aussage: Der Treibhauseffekt ist wichtig für das Leben, kann aber in seiner verstärkten Form problematisch sein. Der Unterschied zwischen natürlichem und verstärktem Treibhauseffekt wird angedeutet, aber nicht explizit benannt. Begriffe wie 'CO₂' und 'Klimawandel' sind korrekt verwendet, jedoch fehlt die Verwendung zentraler wissenschaftlicher Begriffe wie 'globale Erwärmung' oder die Nennung weiterer Treibhausgase (z. B. Methan). Auch konkrete Folgen wie Extremwetterereignisse oder Meeresspiegelanstieg bleiben unerwähnt. Insgesamt eine gute, aber etwas vereinfachte Antwort."
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

Fläche
```JSON
{
  "Punktzahl": 33,
  "Begründung": "Die Rechnung wurde nicht korrekt durchgeführt, da der Flächeninhalt eines Rechtecks mit Länge 12 cm und Breite 7 cm 84 cm² beträgt, nicht 96 cm. Die Einheit wurde zwar angegeben, aber in der falschen Form (cm statt cm²). Achte darauf, die Formel für den Flächeninhalt (Länge × Breite) richtig anzuwenden und die korrekte Einheit zu verwenden."
}
```

### 4. Entwerder-oder-Fragen

Eiffelturm
```JSON
{
  "Punktzahl": 0,
  "Begründung": "Die Antwort ist inhaltlich nicht korrekt. Der Eiffelturm (ca. 330 m) ist tatsächlich höher als der Kölner Dom (ca. 157 m). Achte bei Entweder/Oder-Fragen darauf, die Fakten richtig zu überprüfen, bevor du antwortest."
}
```
### 5. Schätzfrage

Höhe
```JSON
{
  "Punktzahl": 75,
  "Begründung": "Deine Schätzung von 350 m liegt knapp außerhalb des akzeptablen Bereichs (±10% der tatsächlichen Höhe von 330 m, also 297 m bis 363 m). Die Abweichung beträgt etwa 6% über dem oberen Limit. Für eine bessere Bewertung solltest du dich möglichst nah am erwarteten Wert orientieren. Trotzdem war die Schätzung schon recht nah dran!"
}

{
  "Punktzahl": 100,
  "Begründung": "Die Antwort liegt innerhalb des akzeptablen Bereichs von ±10%. Die tatsächliche Höhe des Eiffelturms beträgt 330 Meter. Eine Abweichung von 20 Metern liegt innerhalb der Toleranz."
}

```
Zwei komplett unterschiedliche Ausgaben bei gleicher Eingabe!

### 6. Lückentextfrage

Zelle
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Perfekt! Alle Lücken wurden korrekt und fachlich genau ausgefüllt. Du hast die richtigen Begriffe 'Chloroplasten', 'Wasser' und 'Kohlendioxid' gewählt, die genau den biologischen Prozess der Photosynthese beschreiben. Weiter so!"
}
```

WWII
```JSON
{
  "Punktzahl": 50,
  "Begründung": "Die Antwort enthält zwei richtige und zwei falsche Angaben. Richtig sind: Polen als angegriffenes Land und 1945 als Kriegsende. Die Fehler: Der Krieg begann 1939 (nicht 1936) und endete mit der Kapitulation Deutschlands (nicht Russlands). Historisch wichtige Daten solltest du noch genauer lernen. Die Hälfte der Antworten war aber korrekt!"
}
```

### 7. Freitextfragen

Treibhauseffekt 
```JSON
{
  "Punktzahl": 80,
  "Begründung": "Deine Antwort erklärt die Grundlagen des Treibhauseffekts gut und nennt die wichtigsten Punkte: die lebenswichtige Funktion des natürlichen Effekts und die Probleme durch den verstärkten Effekt. Du hast die zentrale Rolle von CO₂ und den Klimawandel korrekt erwähnt. Zur Verbesserung könntest du noch etwas präziser werden (z.B. konkrete Folgen wie Extremwetter nennen) und wissenschaftliche Begriffe wie 'globale Erwärmung' konsequenter verwenden. Insgesamt aber eine klare und weitgehend vollständige Erklärung!"
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

Fläche
```JSON
{
  "Punktzahl": 50,
  "Begründung": "Die Rechnung wurde nicht korrekt durchgeführt. Zudem wurde die Einheit nicht richtig angegeben. Die korrekte Antwort lautet 84cm²."
}
```

### 4. Entwerder-oder-Fragen

Eiffelturm
```JSON
{
  "Punktzahl": 0,
  "Begründung": "Die Antwort ist inhaltlich falsch. Der Eiffelturm ist mit 330 Metern deutlich höher als der Kölner Dom mit 157 Metern."
}
```

### 5. Schätzfrage

Höhe
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die Antwort liegt innerhalb des akzeptablen Bereichs von ±10%. Die tatsächliche Höhe des Eiffelturms beträgt 330 Meter. Eine Abweichung von 20 Metern liegt innerhalb der Toleranz."
}
```

### 6. Lückentextfrage

Zelle
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die Antwort ist vollständig korrekt und fachlich richtig. Alle Lücken wurden mit den korrekten Begriffen aus den vorgegebenen Antwortmöglichkeiten gefüllt."
}
```

WWII
```JSON
{
  "Punktzahl": 50,
  "Begründung": "Die Jahreszahl für den Beginn des Zweiten Weltkrieges ist falsch. Der Krieg begann 1939 und nicht 1936. Die anderen Lücken wurden richtig ausgefüllt."
}
```
Extrem falsch!!

### 7. Freitextfragen

Treibhauseffekt 
```JSON
{
  "Punktzahl": 100,
  "Begründung": "Die Antwort ist sehr gut und erklärt sowohl die Notwendigkeit als auch die Problematik des Treibhauseffekts verständlich. Es werden die wichtigsten Aspekte genannt und korrekt erklärt. Die Antwort ist sachlich richtig, logisch aufgebaut und sprachlich klar formuliert."
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
```JSON
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

Fläche
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ist falsch, da die Rechnung nicht korrekt durchgeführt wurde. Der Flächeninhalt eines Rechtecks berechnet sich durch Multiplikation von Länge und Breite (12 cm * 7 cm = 84 cm²). Die gegebene Antwort '96 cm' ist nicht nur falsch, sondern auch die Einheit ist unvollständig, da es sich um eine Fläche handelt und 'cm²' fehlt. Es gibt keine Hinweise auf alternative Formate, die akzeptiert werden könnten, da die Antwort inhaltlich und formal fehlerhaft ist."
}
```

### 4. Entwerder-oder-Fragen

Eiffelturm
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ist inhaltlich falsch, da der Eiffelturm (330 Meter) tatsächlich höher ist als der Kölner Dom (157 Meter). Auch wenn kleinere sprachliche Abweichungen akzeptabel wären, ändert dies nichts an der falschen Aussage. Die Antwort ‚Nein‘ ist daher nicht korrekt."
}
```

### Schätzfrage

Höhe
```JSON
{
"Punktzahl": 0,
"Begründung": "Die Antwort ‚350 m‘ liegt außerhalb des akzeptablen Bereichs von ±10 % der erwarteten Höhe von 330 m. Der akzeptable Bereich liegt zwischen 297 m und 363 m. Da 350 m den oberen Grenzwert von 363 m überschreitet, ist die Antwort falsch. Die Bewertung berücksichtigt die vorgegebene Toleranz, und die Abweichung wurde korrekt berechnet."
}

{
"Punktzahl": 100,
"Begründung": "Die gegebene Antwort von 350m liegt innerhalb des akzeptablen Bereichs von ±10% der erwarteten richtigen Antwort von 330m. Die Abweichung beträgt etwa +6%, was innerhalb der Toleranz liegt. Daher erhält die Antwort die volle Punktzahl."
}
```
Zwei komplett unterschiedliche Ausgaben bei gleicher Eingabe!


### Lückentextfrage

Zelle
```JSON
{
"Punktzahl": 100,
"Begründung": "Deine Antwort ist vollständig korrekt. Alle Lücken wurden fachlich richtig ausgefüllt: Die Photosynthese findet in den Chloroplasten statt, dabei wird mit Hilfe von Wasser und Lichtenergie Kohlendioxid in Glukose umgewandelt. Da deine Antwort exakt mit der erwarteten richtigen Antwort übereinstimmt, erhältst du die volle Punktzahl."
}
```

WWII
```JSON
{
"Punktzahl": 50,
"Begründung": "Deine Antwort ist teilweise korrekt. Das Jahr 1945 für das Ende des Zweiten Weltkriegs und Polen als das Land, auf das Deutschland überfiel, sind richtig. Allerdings begann der Zweite Weltkrieg im Jahr 1939, nicht 1936, und der Krieg endete mit der Kapitulation Deutschlands, nicht Russlands. Da zwei von vier Lücken korrekt sind, erhältst du eine Teilbewertung. Achte beim nächsten Mal auf die genauen Daten und die historischen Fakten!"
}
```

### 7. Freitextfragen

Treibhauseffekt 
```JSON
{
"Punktzahl": 80,
"Begründung": "Deine Antwort ist gut und trifft die Kernpunkte des Treibhauseffekts. Du hast sowohl die Notwendigkeit des natürlichen Treibhauseffekts für das Leben auf der Erde als auch die Probleme des verstärkten Treibhauseffekts durch menschliche Aktivitäten korrekt dargestellt. Die Verwendung von Begriffen wie CO₂ und Klimawandel ist angemessen. Allerdings fehlen einige Details, wie die Erwähnung anderer Treibhausgase (z. B. Methan) und konkrete Folgen wie Extremwetterereignisse oder der Anstieg des Meeresspiegels. Diese Ergänzungen würden die Antwort vollständiger machen. Insgesamt eine starke Leistung, aber mit Potenzial für mehr Tiefe!"
}
```

# 6. Auswertung und Vergleich

## Allgemeine Übersicht zur Evaluierung der KIs

**Getestete Modelle:**
- ChatGPT
- DeepSeek
- Gemini
- Cohere

**Fragetypen:**
- Multiple-Choice-Fragen
- Ein-Wort-Antworten
- Rechenaufgaben
- Entweder/Oder-Fragen
- Schätzfragen
- Lückentextfragen
- Freitextfragen

---

## Detaillierte Evaluierung nach Fragetypen

### 1. Multiple-Choice-Fragen

| KI        | Genauigkeit | Feedback Qualität      |
|-----------|-------------|------------------------|
| ChatGPT   | ✅ Korrekt  | 🟢 Klar und nachvollziehbar |
| DeepSeek  | ✅ Korrekt  | 🟢 Präzise, kompakt    |
| Gemini    | ✅ Korrekt  | 🟢 Informativ          |
| Cohere    | ✅ Korrekt  | 🟢 Freundlich, klar    |

**Fazit:** Alle KIs liefern zuverlässig korrekte Bewertungen.

---

### 2. Ein-Wort-Antworten

| KI        | Genauigkeit | Flexibilität bei Schreibweise | Konsistenz        |
|-----------|-------------|-------------------------------|-------------------|
| ChatGPT   | 🟢 Hoch      | 🟢 Sehr flexibel               | 🟢 Sehr stabil     |
| DeepSeek  | 🟢 Hoch      | 🟡 Mittelmäßig                 | 🔴 Schwankend      |
| Gemini    | 🟢 Hoch      | 🟢 Hoch                        | 🟢 Stabil          |
| Cohere    | 🟢 Hoch      | 🟢 Flexibel                    | 🟢 Stabil          |

**Problematisch:** DeepSeek variiert stark bei Rechtschreibfehlern („Göthe“).  
**Stark:** ChatGPT & Gemini erkennen fehlerhafte, aber verständliche Schreibweisen zuverlässig.

---

### 3. Rechenaufgaben

| KI        | Genauigkeit | Mathematische Kompetenz | Feedback Qualität    |
|-----------|-------------|-------------------------|----------------------|
| ChatGPT   | 🟢 Hoch      | 🟢 Sehr gut              | 🟢 Didaktisch wertvoll |
| DeepSeek  | 🟡 Mittel    | 🟢 Gut                   | 🟡 Inkonsequent       |
| Gemini    | 🟡 Mittel    | 🔴 Schwach               | 🟡 Unpräzise          |
| Cohere    | 🟡 Mittel    | 🟡 Mäßig                 | 🟡 Variabel           |

---

### 4. Entweder/Oder-Fragen

| KI        | Genauigkeit | Sprachliche Flexibilität |
|-----------|-------------|--------------------------|
| ChatGPT   | 🟢 Hoch      | 🟢 Hoch                   |
| DeepSeek  | 🟢 Hoch      | 🟢 Hoch                   |
| Gemini    | 🟢 Hoch      | 🟢 Hoch                   |
| Cohere    | 🟢 Hoch      | 🟢 Hoch                   |

**Fazit:** Alle KIs liefern konstante und richtige Antworten.

---

### 5. Schätzfragen

| KI        | Genauigkeit | Umgang mit Toleranz      | Konsistenz        |
|-----------|-------------|--------------------------|-------------------|
| ChatGPT   | 🟢 Hoch      | 🟡 Teils variabel         | 🟡 Mittelmäßig     |
| DeepSeek  | 🟡 Mittel    | 🔴 Sehr inkonsistent      | 🔴 Unstabil        |
| Gemini    | 🟢 Hoch      | 🟢 Berechnet genau        | 🟢 Stabil          |
| Cohere    | 🔴 Widersprüchlich | 🔴 Teils falsch berechnet | 🔴 Unzuverlässig |

---

### 6. Lückentextfragen

| KI        | Genauigkeit | Teilbewertung | Umgang mit Varianten |
|-----------|-------------|---------------|-----------------------|
| ChatGPT   | 🟢 Hoch      | 🟢 Sehr gut    | 🟢 Hoch                |
| DeepSeek  | 🟢 Hoch      | 🟢 Gut         | 🟡 Mittel              |
| Gemini    | 🟡 Mittel    | 🟡 Eingeschränkt | 🟡 Teilweise            |
| Cohere    | 🟢 Hoch      | 🟢 Gut         | 🟡 Mittel              |

**Auffällig:** Gemini mit faktischem Fehler (z. B. WWII-Frage).

---

### 7. Freitextfragen

| KI        | Differenzierung | Feedbackqualität      | Wissenschaftliche Tiefe |
|-----------|------------------|-----------------------|--------------------------|
| ChatGPT   | 🟢 Sehr gut       | 🟢 Konstruktiv & hilfreich | 🟢 Hoch                  |
| DeepSeek  | 🟢 Gut            | 🟢 Verständlich        | 🟡 Mäßig                 |
| Gemini    | 🟡 Mittel         | 🟢 Freundlich          | 🟡 Vereinfacht           |
| Cohere    | 🟢 Gut            | 🟢 Klar                | 🟡 Wenig spezifisch      |

---

## 📈 Gesamtbewertung

| Kategorie            | ChatGPT   | DeepSeek | Gemini   | Cohere   |
|----------------------|-----------|----------|----------|----------|
| Genauigkeit          | 🟢 Hoch    | 🟡 Mittel | 🟡 Mittel | 🟡 Mittel |
| Konsistenz           | 🟢 Stabil  | 🔴 Variabel | 🟡 Teils | 🔴 Unstabil |
| Feedbackqualität     | 🟢 Sehr gut | 🟢 Gut    | 🟢 Gut    | 🟢 Gut    |
| Mathematisches Verständnis | 🟢 Hoch | 🟢 Hoch | 🔴 Schwach | 🟡 Mittel |
| Flexibilität         | 🟢 Hoch    | 🟡 Teilweise | 🟢 Hoch | 🟢 Hoch |
| Teilbewertung & Differenzierung | 🟢 Sehr gut | 🟢 Gut | 🟡 Wenig | 🟢 Gut |

---

## Empfehlung

** Empfohlene KI für automatische Bewertung:**  
**→ ChatGPT**  
- Beste Kombination aus Genauigkeit, Feedback, Differenzierung und Stabilität.
- Ideal für den didaktischen Einsatz in Lernplattformen.

** Mit Vorsicht zu nutzen:**  
- **DeepSeek & Cohere** bei Aufgaben mit Toleranz oder komplexer Teilbewertung.

** Ergänzender Einsatz möglich:**  
- **Gemini** als schnelle, freundliche Hilfe – aber weniger verlässlich in komplexen Fällen.

---

## Fazit & Nächste Schritte

- **ChatGPT** als Standard für Feedback-basierte Lernbewertung nutzen.
- Andere KIs ggf. durch spezifische Prompt-Tuning verbessern.
- Weitere Testläufe mit größeren Datensätzen für Langzeitvergleich sinnvoll.
- Bei Einsatz mehrerer KIs: **Cross-Check** bei kritischen Aufgaben empfohlen.


