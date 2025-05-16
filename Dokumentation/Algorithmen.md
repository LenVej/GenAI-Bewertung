# Algorithmen zur Korrektur von Antworten

## Multiple Choice / Entweder/Oder-Fragen
Algorithmische Ansätze:
- Vergleichsbasierte Algorithmen: Direkter Vergleich der ausgewählten Antworten mit der gespeicherten richtigen Antwort (Vergleichsmatrix-Algorithmus)
- Gewichtete Punktesysteme: Möglichkeit, Teilpunkte für teilweise korrekte Antworten zu vergeben.
- Logische Entscheidungsbäume: Kann bei komplexeren Multiple-Choice-Fragen mit Abhängigkeiten zwischen Antworten verwendet werden.
- Regular Expressions: Für komplexere Muster, wie z.B. Datumsformate oder bestimmte Wortfolgen, können reguläre Ausdrücke verwendet werden.


## Ein-Wort-Antworten
Algorithmische Ansätze:
- String-Matching-Algorithmen
- Lexikalische Ähnlichkeit: Synonymwörterbücher oder semantische Netzwerke können genutzt werden, um die Ähnlichkeit zwischen der gegebenen Antwort und der korrekten Antwort zu bestimmen.
- Stemming und Lemmatisierung: Durch die Reduktion von Wörtern auf ihre Stammform (Stemming) oder ihre Grundform (Lemmatisierung) kann die Varianz von Wörtern reduziert werden.
- Phonetische Ähnlichkeit: Für Sprachen mit phonetischer Schreibweise können Algorithmen zur Berechnung der phonetischen Ähnlichkeit eingesetzt werden.

## Rechenfragen
- Numerische Berechnung: Die gegebene Antwort wird mit dem korrekten Ergebnis verglichen.

## Schätzfragen
- Toleranzbereiche: Wie bereits erwähnt, werden für Schätzfragen Toleranzbereiche definiert.

## Lückentextfragen
- Wortübereinstimmung: Die eingegebenen Wörter werden direkt mit den korrekten Wörtern verglichen.
- Semantische Ähnlichkeit: Für offene Lücken kann die semantische Ähnlichkeit zwischen dem eingegebenen Wort und dem Kontextwort analysiert werden.


## Freitextantworten
- Keyword-Matching: Wichtige Schlüsselwörter aus der korrekten Antwort werden in der gegebenen Antwort gesucht.
- Semantische Ähnlichkeit: Die semantische Ähnlichkeit zwischen der gesamten gegebenen Antwort und der korrekten Antwort wird berechnet.
- N-gram-Matching: Sequenzen von N Wörtern (N-grams) werden verglichen.


## Einschränkungen traditioneller Algorithmen:
- Kontextunabhängigkeit: Oftmals wird der Kontext der Frage nicht ausreichend berücksichtigt.
- Flexibilität: Die Algorithmen sind oft nicht flexibel genug, um mit unterschiedlichen Formulierungen und Sprachvariationen umzugehen.
- Subjektivität: Die Bewertung von Freitextantworten kann subjektiv sein und erfordert oft menschliche Beurteilung.

---------------------------


# Realistisch Umsetzbare Algorithmen:

## Multiple Choice / Entweder-Oder:
Vergleichsbasierte Algorithmen (Vergleichsmatrix): Sehr einfach zu implementieren.

Gewichtete Punktesysteme: Ebenfalls leicht mit normaler Logik (z. B. JSON-Strukturen) umzusetzen.

## Ein-Wort-Antworten:
String-Matching-Algorithmen: Leicht (z. B. lower().strip() + Vergleich).

Stemming und Lemmatisierung: Mit z. B. NLTK oder spaCy in Python gut machbar.

Phonetische Ähnlichkeit: Mit fuzzy oder Metaphone realisierbar – einfacher als semantische Methoden.

## Rechenfragen:
Numerische Berechnung: Simpler Vergleich mit Toleranzbereich – sehr einfach.

## Schätzfragen:
Toleranzbereiche: Leicht mit einfacher Differenzprüfung (z. B. abs(user - correct) < tol).

## Lückentext:
Wortübereinstimmung: Direkt umsetzbar über Vergleich pro Lücke.

## Freitext:
Keyword-Matching: Mit Set- oder Listenvergleichen einfach machbar.

N-gram-Matching: Einfach mit Tokenisierung und Sliding Window.



# Literaturen:
- https://pdfs.semanticscholar.org/0f7f/06879d26e3ba5b6fb7feeddc199f24dd4ff6.pdf
