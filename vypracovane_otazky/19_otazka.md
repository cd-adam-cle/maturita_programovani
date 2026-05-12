# Zápisky: Otázka č. 19 - Srovnání jazyků Python a C#

**Datum:** 2026-05-10
**Status:** Hotovo

---

## Checklist bodů otázky

- [x] Bod 1: Interpretovaný vs kompilovaný jazyk
- [x] Bod 2: Statické vs dynamické typování
- [x] Bod 3: Programovací paradigmata (OOP, funkcionální, procedurální)
- [x] Bod 4: Syntaxe – deklarace a inicializace proměnné
- [x] Bod 5: Syntaxe – podmínky a větvení
- [x] Bod 6: Syntaxe – cykly
- [x] Bod 7: Syntaxe – funkce a metody
- [x] Bod 8: Datové struktury a jejich uložení v paměti
- [x] Bod 9: Souhrnné srovnání

---

## Klíčové koncepty & Snippety

---

### Bod 1: Interpretovaný vs kompilovaný jazyk

**Teorie:**

| Pojem | Význam |
|-------|--------|
| **Kompilovaný jazyk** | Zdrojový kód se před spuštěním převede překladačem (kompilátorem) na strojový kód / mezikód. Spouští se až výsledek překladu. |
| **Interpretovaný jazyk** | Zdrojový kód čte a vykonává **interpret** (řádek po řádku) za běhu. Žádný separátní krok kompilace. |

**V praxi je hranice neostrá** – moderní jazyky kombinují oba přístupy.

---

**C# – kompilovaný (s JIT):**

```
1) Zdrojový kód  (.cs)
        │  C# kompilátor (Roslyn / csc)
        ▼
2) IL kód        (.dll, .exe)   ← Common Intermediate Language
        │  CLR (Common Language Runtime) – JIT kompilátor
        ▼
3) Strojový kód  (běží na CPU)
```

- **AOT vs JIT:** standardně **JIT** (Just-In-Time) – IL se překládá do strojového kódu **až za běhu**, při prvním volání metody. Existuje také AOT (Ahead-Of-Time) kompilace pro zrychlení startu.
- **Runtime:** .NET (CLR) – stará se o paměť (GC), bezpečnost, výjimky.

---

**Python – interpretovaný (s předkompilací do bytecode):**

```
1) Zdrojový kód  (.py)
        │  Python kompilátor (interní)
        ▼
2) Bytecode      (.pyc – v __pycache__)
        │  CPython interpret (PVM – Python Virtual Machine)
        ▼
3) Vykonání      (interpret čte bytecode instrukci po instrukci)
```

- Python **NENÍ čistě interpretovaný** – nejprve se kompiluje do bytecodu, ten se pak interpretuje.
- Existují **různé implementace:** CPython (referenční), PyPy (s JIT, rychlejší), Jython (Java), IronPython (.NET).

---

**Důsledky pro vývoj:**

| Aspekt | C# | Python |
|--------|----|----|
| Rychlost spouštění | Pomalejší start (kompilace) | Rychlý start |
| Rychlost běhu | **Rychlejší** (strojový kód) | **Pomalejší** (interpret + dynamické typy) |
| Detekce chyb | Většina **při kompilaci** | Až **za běhu** (`NameError`, `TypeError`) |
| Distribuce | Binárky / DLL | Zdrojové kódy + interpret |
| REPL (interaktivní) | Omezeně (`dotnet-script`) | Nativně (`python` v terminálu) |

---

### Bod 2: Statické vs dynamické typování

**Teorie:**

| Typ | Význam | Kdy se kontroluje |
|-----|--------|-------------------|
| **Statické** | Typ proměnné je pevný – určen při deklaraci | **Při kompilaci** |
| **Dynamické** | Typ je vázán na **hodnotu**, ne na proměnnou | **Až za běhu** |

**Pozor – to NENÍ totéž jako:**
- **Silné typování** = jazyk neudělá implicitní konverzi mezi nesouvisejícími typy (`1 + "ahoj"` → chyba).
  *Oba* jazyky (C# i Python) jsou silně typované!
- **Slabé typování** = jazyk konverze dělá automaticky (např. JavaScript: `1 + "1" === "11"`).

---

**C# – staticky typovaný:**

```csharp
int vek = 20;          // typ "int" určen při deklaraci
vek = "ahoj";          //  CHYBA při kompilaci - nelze přiřadit string do int

string jmeno = "Pepa"; // typ "string"
jmeno = 42;            //  CHYBA při kompilaci
```

**Type inference (`var`):** typ se odvodí z hodnoty, ale je stále pevný:

```csharp
var x = 10;        // x JE typu int (pevně)
x = "ahoj";        //  CHYBA – x je int, ne string
```

**Dynamic (výjimečně):**

```csharp
dynamic d = 10;
d = "ahoj";        //  OK – typ se kontroluje až za běhu (jako Python)
d.Neexistuje();    // přeloží se, ale za běhu spadne
```

---

**Python – dynamicky typovaný:**

```python
vek = 20           # vek je int
vek = "ahoj"       #  OK – teď je vek string
vek = [1, 2, 3]    #  OK – teď je vek list

print(type(vek))   # <class 'list'>
```

**Type hints (od Pythonu 3.5):** jen **dokumentace pro vývojáře / IDE**, runtime je nehlídá:

```python
def secti(a: int, b: int) -> int:
    return a + b

secti("ahoj", "svet")    #  Spustí se! Vrátí "ahojsvet". Hint není kontrola.
```

(Externí nástroje jako `mypy` nebo `pyright` to umí staticky kontrolovat, ale Python sám ne.)

---

**Důsledky:**

| | C# (statické) | Python (dynamické) |
|---|---|---|
| Chyba typu | **Při kompilaci** | Až **za běhu** |
| IDE nápověda | Skvělá (zná typy) | Horší (musí hádat) |
| Refaktoring | Bezpečný | Riskantní |
| Flexibilita | Menší | **Větší** |
| Rychlost vývoje | Pomalejší | **Rychlejší** |
| Výkon | **Rychlejší** | Pomalejší |

---

### Bod 3: Programovací paradigmata

**Teorie:**

**Paradigma** = styl/přístup k psaní programů.

| Paradigma | Klíčová myšlenka |
|-----------|------------------|
| **Imperativní** | Říkám počítači KROK ZA KROKEM, co má dělat |
| **Procedurální** | Imperativní + organizace do procedur/funkcí |
| **Objektově orientované (OOP)** | Data + metody pohromadě v objektech, dědičnost, polymorfismus |
| **Funkcionální** | Výpočet jako vyhodnocování funkcí, neměnnost (immutability), funkce jako prvky |
| **Deklarativní** | Říkám CO chci, ne JAK to udělat (SQL, regex) |

---

**Oba jazyky jsou MULTIPARADIGMOVÉ** – podporují více stylů:

| Paradigma | C# | Python |
|-----------|:--:|:------:|
| Procedurální | Ano | Ano |
| OOP | Ano (silný důraz) | Ano |
| Funkcionální | Ano (LINQ, lambdy, delegáti) | Ano (lambdas, map/filter, generátory) |

---

**OOP v obou jazycích:**

C#:
```csharp
public class Pes
{
    public string Jmeno { get; set; }

    public Pes(string jmeno) { Jmeno = jmeno; }

    public void Stekat() => Console.WriteLine($"{Jmeno}: Haf!");
}

Pes rex = new Pes("Rex");
rex.Stekat();
```

Python:
```python
class Pes:
    def __init__(self, jmeno):
        self.jmeno = jmeno

    def stekat(self):
        print(f"{self.jmeno}: Haf!")

rex = Pes("Rex")
rex.stekat()
```

**Klíčové rozdíly v OOP:**

| Aspekt | C# | Python |
|--------|----|----|
| Klíčové slovo třídy | `class` | `class` |
| Konstruktor | Stejné jméno jako třída | `__init__(self, ...)` |
| Reference na instanci | implicitní `this` | explicitní `self` |
| Modifikátory přístupu | `public`, `private`, `protected` | konvence: `_` (private), `__` (very private, name mangling) |
| Vícenásobná dědičnost | Nelze (jen rozhraní) | Lze |
| Rozhraní | `interface` | "Duck typing" – pokud má objekt metody, je to OK |
| Abstraktní třída | `abstract class` | `from abc import ABC` |

---

**Funkcionální prvky:**

C#:
```csharp
List<int> cisla = new List<int> { 1, 2, 3, 4, 5 };

// Lambda + LINQ
var sudaCtverce = cisla
    .Where(n => n % 2 == 0)        // filter
    .Select(n => n * n)            // map
    .Sum();                        // reduce
// Výsledek: 4 + 16 = 20
```

Python:
```python
cisla = [1, 2, 3, 4, 5]

# Lambda + funcs
suda_ctverce = sum(map(
    lambda n: n * n,
    filter(lambda n: n % 2 == 0, cisla)
))
# Nebo idiomatičtěji – list comprehension:
suda_ctverce = sum(n*n for n in cisla if n % 2 == 0)
# Výsledek: 20
```

---

### Bod 4: Syntaxe – Deklarace a inicializace proměnné

**C#:**

```csharp
int vek = 20;                  // explicitní typ
double cena = 19.99;
string jmeno = "Pepa";
bool jeStudent = true;

// Type inference
var pocet = 42;                // var ≠ dynamic; typ se odvodí (int)

// Konstanta
const double PI = 3.14159;

// Středník UKONČUJE příkaz
```

**Python:**

```python
vek = 20                       # bez deklarace typu
cena = 19.99
jmeno = "Pepa"
je_student = True              # konvence: snake_case

# Konstanta - jen konvence (UPPERCASE)
PI = 3.14159

# Žádné středníky
```

**Klíčové rozdíly:**

| | C# | Python |
|---|---|---|
| Deklarace typu | Povinná (nebo `var`) | Žádná |
| Konec příkazu | `;` | Konec řádku (nebo `\` pro pokračování) |
| Konstanta | `const` (skutečná) | jen konvence (`UPPERCASE`) |
| Konvence | `camelCase` (lokální), `PascalCase` (public) | `snake_case` |

---

### Bod 5: Syntaxe – Podmínky

**C#:**

```csharp
if (vek >= 18)
{
    Console.WriteLine("Plnoletý");
}
else if (vek >= 15)
{
    Console.WriteLine("Mladistvý");
}
else
{
    Console.WriteLine("Dítě");
}

// Ternární operátor
string status = vek >= 18 ? "plnoletý" : "nezletilý";

// Switch (klasický)
switch (znamka)
{
    case 1: Console.WriteLine("Výborně"); break;
    case 2: Console.WriteLine("Chvalitebně"); break;
    default: Console.WriteLine("Něco jiného"); break;
}

// Switch expression (C# 8+)
string slovne = znamka switch
{
    1 => "Výborně",
    2 => "Chvalitebně",
    _ => "Něco jiného"
};
```

**Python:**

```python
if vek >= 18:
    print("Plnoletý")
elif vek >= 15:                # elif místo else if
    print("Mladistvý")
else:
    print("Dítě")

# Ternární (jiné pořadí!)
status = "plnoletý" if vek >= 18 else "nezletilý"

# Match (Python 3.10+, podobné switch)
match znamka:
    case 1:
        print("Výborně")
    case 2:
        print("Chvalitebně")
    case _:
        print("Něco jiného")
```

**Klíčové rozdíly:**

| | C# | Python |
|---|---|---|
| Závorky kolem podmínky | `if (...)` povinné | `if ...:` bez závorek |
| Bloky | `{ ... }` | **odsazení** (4 mezery) |
| Else if | `else if` | `elif` |
| Ternární | `cond ? a : b` | `a if cond else b` |
| Switch | `switch` / `switch expression` | `match` (od 3.10) |

---

### Bod 6: Syntaxe – Cykly

**C#:**

```csharp
// for (klasický)
for (int i = 0; i < 10; i++)
    Console.WriteLine(i);

// foreach (přes kolekci)
int[] cisla = { 1, 2, 3 };
foreach (int c in cisla)
    Console.WriteLine(c);

// while
int i = 0;
while (i < 10)
{
    Console.WriteLine(i);
    i++;
}

// do-while (provede se aspoň jednou)
do { ... } while (podminka);
```

**Python:**

```python
# for (přes kolekci) - HLAVNÍ způsob
cisla = [1, 2, 3]
for c in cisla:
    print(c)

# Klasický for? Použij range()
for i in range(10):                # 0..9
    print(i)

for i in range(2, 10, 2):          # 2, 4, 6, 8 (start, stop, step)
    print(i)

# while
i = 0
while i < 10:
    print(i)
    i += 1

# do-while NEEXISTUJE - simuluje se pomocí while True + break
while True:
    ...
    if podminka: break
```

**Klíčové rozdíly:**

| | C# | Python |
|---|---|---|
| Klasický `for(int i=0;...)` | Ano | Ne (musí přes `range`) |
| `foreach` | `foreach` | `for x in kolekce:` (jediný `for`) |
| `do-while` | Ano | Ne |
| `break`, `continue` | Ano | Ano |
| `for-else` | Ne | Ano (else běží, když se cyklus dokončí bez break) |

---

### Bod 7: Syntaxe – Funkce a metody

**C#:**

```csharp
// Funkce / metoda
public int Secti(int a, int b)
{
    return a + b;
}

// void metoda
public void Pozdrav(string jmeno)
{
    Console.WriteLine($"Ahoj {jmeno}!");
}

// Default parametry
public void Vypis(string text, bool velkePismena = false)
{
    Console.WriteLine(velkePismena ? text.ToUpper() : text);
}

// Lambda výraz
Func<int, int, int> nasob = (a, b) => a * b;
int vysl = nasob(3, 4);   // 12

// Pojmenované argumenty
Vypis(text: "ahoj", velkePismena: true);
```

**Python:**

```python
# Funkce
def secti(a, b):
    return a + b

# Bez návratové hodnoty - implicitně vrací None
def pozdrav(jmeno):
    print(f"Ahoj {jmeno}!")

# Default parametry
def vypis(text, velke_pismena=False):
    print(text.upper() if velke_pismena else text)

# Lambda
nasob = lambda a, b: a * b
vysl = nasob(3, 4)        # 12

# Pojmenované argumenty
vypis(text="ahoj", velke_pismena=True)

# *args, **kwargs (variadic)
def f(*args, **kwargs):
    pass
```

**Klíčové rozdíly:**

| | C# | Python |
|---|---|---|
| Klíčové slovo | `public/private` + návratový typ | `def` |
| Návratový typ | Povinný | Žádný (nebo type hint) |
| Typ parametrů | Povinný | Žádný (nebo type hint) |
| Závorky | `{ ... }` | odsazení po `:` |
| Přetěžování (overload) | Ano (stejné jméno, různé parametry) | Ne (druhé `def` přepíše první) |
| Default parametry | Ano | Ano |
| `*args` / `**kwargs` | `params int[]` (omezené) | Plně podporováno |

---

### Bod 8: Datové struktury a uložení v paměti

**Teorie:**

| Pojem | Význam |
|-------|--------|
| **Stack (zásobník)** | Rychlá paměť, automaticky uvolňována při skončení funkce. Hodnotové typy. |
| **Heap (halda)** | Větší, sdílená paměť. Referenční typy. Spravuje **Garbage Collector**. |
| **Hodnotový typ** | Kopíruje se HODNOTA (kopie). |
| **Referenční typ** | Kopíruje se ODKAZ (reference). |

---

**C# – rozlišuje hodnotové a referenční typy:**

```csharp
// HODNOTOVÉ TYPY (na zásobníku)
int x = 10;            // 4 bajty na stacku
double y = 3.14;       // 8 bajtů na stacku
bool b = true;         // 1 bajt na stacku
struct Bod { ... }     // struct = hodnotový typ

// REFERENČNÍ TYPY (data na heapu, reference na stacku)
string s = "ahoj";              // string je referenční (i když chová se "hodnotově" - immutable)
int[] pole = new int[5];        // pole = referenční
List<int> seznam = new List<int>();
Dictionary<string, int> slovnik = new Dictionary<string, int>();
class Pes { ... }                // class = referenční
```

```
ZÁSOBNÍK                HEAP
┌─────────┐             ┌────────────────────┐
│ x = 10  │             │ "ahoj"             │
│ y = 3.14│             │ [0,0,0,0,0]        │ (pole)
│ s ──────┼────────────►│                    │
│ pole ───┼─────────────│                    │
└─────────┘             └────────────────────┘
```

**Klíčové C# struktury:**

| Struktura | Typ | Velikost prvku | Použití |
|-----------|-----|----------------|---------|
| `int` | hodnotový | 4 B | Celé číslo, rozsah ±2 mld |
| `long` | hodnotový | 8 B | Velké celé číslo |
| `double` | hodnotový | 8 B | Desetinné |
| `bool` | hodnotový | 1 B | true/false |
| `char` | hodnotový | 2 B | Unicode znak |
| `struct` | hodnotový | dle obsahu | Malé celky (Point, Vector) |
| `class` | referenční | reference 8 B | Komplexní objekty |
| `string` | referenční (immutable) | reference | Text |
| `T[]` | referenční | reference | Pevná velikost |
| `List<T>` | referenční | reference | Dynamické pole |
| `Dictionary<K,V>` | referenční | reference | Hash mapa |

---

**Python – VŠECHNO je objekt na heapu:**

V Pythonu **neexistují hodnotové typy** v klasickém smyslu. **Všechno je objekt** na heapu, proměnná je jen **reference** (jméno, štítek).

```python
x = 10              # int objekt na heapu, x je reference
y = 3.14            # float objekt na heapu
s = "ahoj"          # str objekt
seznam = [1, 2, 3]  # list objekt
slovnik = {"a": 1}  # dict objekt
```

```
ZÁSOBNÍK (jména)        HEAP (objekty)
┌─────────┐             ┌────────────────────┐
│ x ──────┼────────────►│ int(10)            │
│ y ──────┼────────────►│ float(3.14)        │
│ s ──────┼────────────►│ str("ahoj")        │
│ seznam ─┼────────────►│ list([1,2,3])      │
└─────────┘             └────────────────────┘
```

**Důsledky:**

```python
# Python int má NEOMEZENÝ rozsah!
velke = 10 ** 100         #  funguje
print(velke)              # 100 nul, žádný overflow
```

V C# by `long.MaxValue + 1` přetékal (overflow). Python automaticky alokuje větší objekt.

---

**Klíčové Python struktury:**

| Struktura | Mutabilní? | Použití |
|-----------|:----------:|---------|
| `int` | objekt, prakticky immutable | Neomezený rozsah |
| `float` | immutable | 8 B (double) |
| `bool` | immutable | True/False |
| `str` | **immutable** | Text |
| `list` | Mutable | Dynamické pole jako `List<T>` |
| `tuple` | **immutable** | Neměnitelná n-tice `(1, 2, 3)` |
| `dict` | Mutable | Hash mapa jako `Dictionary` |
| `set` | Mutable | Množina (unikátní prvky) |

---

**Srovnání obdobných struktur:**

| Účel | C# | Python |
|------|----|----|
| Pevné pole | `int[] arr` | – (existuje `array` v knihovně) |
| Dynamické pole | `List<T>` | `list` |
| Hash mapa | `Dictionary<K,V>` | `dict` |
| Množina | `HashSet<T>` | `set` |
| Neměnitelná n-tice | `Tuple<...>` / `(int, int)` | `tuple` |
| Fronta | `Queue<T>` | `collections.deque` |
| Zásobník | `Stack<T>` | `list` (s `append`/`pop`) |

---

**Vizualizace – kopírování:**

C# (hodnotový typ):
```csharp
int a = 10;
int b = a;        // KOPIE hodnoty
b = 99;
// a == 10   (a je nezávislé)
```

C# (referenční typ):
```csharp
List<int> a = new List<int> { 1, 2 };
List<int> b = a;  // KOPIE reference (ne dat!)
b.Add(3);
// a == [1, 2, 3]   a se taky změnilo
```

Python (vždy reference):
```python
a = [1, 2]
b = a             # KOPIE reference
b.append(3)
# a == [1, 2, 3]   stejné chování jako referenční typ v C#

# Hlubokou kopii musíš udělat ručně:
import copy
b = copy.deepcopy(a)
```

---

### Bod 9: Souhrnné srovnání

| Vlastnost | C# | Python |
|-----------|----|----|
| **Překlad** | Kompilovaný (do IL → JIT) | Interpretovaný (přes bytecode) |
| **Typování** | Statické, silné | Dynamické, silné |
| **Paradigmata** | OOP (silný důraz), funkcionální, procedurální | OOP, funkcionální, procedurální |
| **Vstupní bariéra** | Vyšší (typy, syntaxe, projekty) | Nižší (přímočará syntaxe) |
| **Rychlost běhu** | Rychlý (JIT) | Pomalejší (interpret) |
| **Detekce chyb** | Při kompilaci | Až za běhu |
| **Konvence pojmenování** | PascalCase / camelCase | snake_case |
| **Vícenásobná dědičnost** | Ne (jen interfaces) | Ano |
| **Nullable** | `?` operátor, NRT (8.0+) | `None` (vždy) |
| **Garbage collector** | Generační | Reference counting + GC pro cykly |
| **Hlavní použití** | Enterprise aplikace, Unity hry, Web (ASP.NET) | Skripty, AI/ML, web (Django/Flask), data science |
| **Bloky kódu** | `{ ... }` | Odsazení |
| **Konec příkazu** | `;` | Konec řádku |
| **Klíčové slovo `this`** | `this` (implicitní u metod) | `self` (explicitní 1. parametr) |

---

## Na co si dát pozor (Maturitní "chytáky")

1. **Python NENÍ čistě interpretovaný** – nejprve se kompiluje do bytecodu (`.pyc`).

2. **C# `var` ≠ Python proměnná!**
   `var x = 10;` v C# je stále **statické typování** (typ je `int`, nelze přiřadit string).
   `x = 10` v Pythonu je **dynamické**.

3. **Silné vs slabé typování**:
   Oba jazyky jsou **silně typované** – `1 + "ahoj"` selže v obou. Slabé typování má JavaScript.

4. **Python `int` má neomezený rozsah** – C# `int` je 32-bit (max ≈ 2 mld), `long` je 64-bit. Python automaticky roste.

5. **Python: VŠE je referenční** – přiřazení `b = a` vždy kopíruje referenci, ne hodnotu. Pro hlubokou kopii `copy.deepcopy()`.

6. **`elif` ne `else if`** – Python má `elif` jako jedno klíčové slovo.

7. **Odsazení v Pythonu je SYNTAXE** – špatné odsazení = `IndentationError`. Standard: 4 mezery.

8. **Python nemá přetěžování metod** – druhé `def` se stejným jménem **přepíše** první. C# umí přetěžovat podle počtu/typu parametrů.

9. **`self` v Pythonu je EXPLICITNÍ** – musíš ho psát jako první parametr každé metody. C# `this` je implicitní.

10. **Python type hints nejsou kontrolovány za běhu** – jsou jen pro vývojáře/IDE. C# typy jsou vynucené kompilátorem.

---

## Senior Tipy

1. **Duck typing v Pythonu** – "Pokud to chodí jako kachna a kváká jako kachna, je to kachna." Není potřeba interface, stačí, aby objekt měl správné metody.

2. **GIL (Global Interpreter Lock)** – CPython má globální zámek, který znemožňuje skutečný paralelismus vláken (kvůli refcounting). C# vlákna běží paralelně bez problému.

3. **C# je "objektovější" než Python** – primitiva v C# (`int`) nejsou plnohodnotné objekty (jsou hodnotové, na stacku). V Pythonu je `5` plnohodnotný objekt s metodami: `(5).bit_length()`.

4. **C# 9+ records** = neměnné datové třídy s value-equality (podobné Python `@dataclass(frozen=True)`).

5. **Python's "batteries included"** – Python má obrovskou standardní knihovnu. C# má NuGet balíčky, ale méně v základu.

6. **Iterátory a generátory** – Python `yield` vytvoří generátor (líné vyhodnocení). C# má `yield return` v IEnumerable – velmi podobné.

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 1** (Datové typy) | Hodnotové vs referenční typy v C# |
| **Ot. 17** (OOP) | Třída, instance, polymorfismus – v obou jazycích jinak |
| **Ot. 18** (Dědičnost) | Multiple inheritance v Pythonu, jen single + interfaces v C# |
| **Ot. 7** (Časová složitost) | Stejné algoritmy jsou v Pythonu řádově pomalejší kvůli interpretu |

---

## Klíčová věta pro maturitu

> *"C# je staticky typovaný kompilovaný jazyk – kód se překládá do mezikódu IL, který za běhu JIT kompilátor převádí na strojový kód, takže chyby typů se odhalí už při kompilaci. Python je dynamicky typovaný interpretovaný jazyk – kompiluje se do bytecodu a interpretuje za běhu, kdy se teprve kontrolují typy. Oba jazyky jsou silně typované a multiparadigmové (OOP + funkcionální). C# rozlišuje hodnotové a referenční typy, zatímco v Pythonu je vše objekt na haldě. Syntakticky se liší hlavně v deklaraci proměnných (typ vs bez typu) a strukturování bloků (závorky vs odsazení)."*

---

* Vytvořeno: 2026-05-10 | Maturitní příprava PRG 2025/2026*
