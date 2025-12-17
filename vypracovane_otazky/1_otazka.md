# 📚 ZÁPISKY: Otázka č. 1 - Datové typy v C#

**Datum zpracování:** 17. prosince 2024  
**Status:** ✅ KOMPLETNÍ (10/10 bodů)  
**Předmět:** Programování - Maturitní příprava

---

## ✅ CHECKLIST BODŮ OTÁZKY

| # | Bod | Status |
|---|-----|--------|
| 1 | Pojem proměnná | ✅ |
| 2 | Jednoduché typy (int, long, char, bool, double, float, decimal) | ✅ |
| 3 | Velikosti a rozsahy typů | ✅ |
| 4 | Signed vs unsigned | ✅ |
| 5 | Složené typy: pole | ✅ |
| 6 | Složené typy: List, Dictionary | ✅ |
| 7 | Složené typy: string | ✅ |
| 8 | Složené typy: struct vs class | ✅ |
| 8b | **BONUS: Detailní syntaxe CLASS** | ✅ |
| 9 | NULL a nullable typy | ✅ |
| 10 | Hodnotové vs referenční typy | ✅ |

---

# 📌 BOD 1: PROMĚNNÁ

## 🧠 Definice
**Proměnná** = pojmenované místo v paměti, které uchovává hodnotu určitého typu.

**Analogie:** Šuplík se jmenovkou – na jmenovce je název (`vek`) a uvnitř je obsah (`18`).

## 💻 Syntaxe

```csharp
// DEKLARACE - vytvoření proměnné (rezervace paměti)
int vek;

// INICIALIZACE - první přiřazení hodnoty
vek = 18;

// Nebo obojí najednou
int vek = 18;

// ZMĚNA HODNOTY
vek = 19;

// ČTENÍ HODNOTY
Console.WriteLine(vek);  // vypíše: 19
```

## 📋 Co musíš umět říct u tabule

Proměnná má:
1. **Název** (identifikátor) - např. `vek`
2. **Datový typ** - např. `int`
3. **Hodnotu** - např. `18`
4. **Adresu v paměti** - kde je uložena v RAM

## 🎨 Vizualizace

```
PAMĚŤ:
┌─────────────┬─────────┐
│ Název: vek  │ int     │
├─────────────┼─────────┤
│ Adresa:     │ 0x2A7F8 │
├─────────────┼─────────┤
│ Hodnota:    │   18    │
└─────────────┴─────────┘
```

## ⚠️ Chyták: Deklarace vs Inicializace

- **Deklarace** = vytvoření proměnné (`int cislo;`)
- **Inicializace** = první přiřazení hodnoty (`cislo = 10;`)

## 🚀 Senior tip

```csharp
var vek = 18;              // implicitně typovaná (kompilátor odvodí int)
const int MAX = 120;       // konstanta - nelze změnit
readonly int rok = 2007;   // pouze pro čtení
```

---

# 📌 BOD 2-4: JEDNODUCHÉ DATOVÉ TYPY

## 📊 KOMPLETNÍ TABULKA TYPŮ

### Celočíselné typy (SIGNED - se znaménkem)

| Typ | Velikost | Rozsah | Použití |
|-----|----------|--------|---------|
| `sbyte` | 8 bitů | -128 až 127 | Malá čísla se znaménkem |
| `short` | 16 bitů | -32 768 až 32 767 | Menší čísla |
| **`int`** | **32 bitů** | **-2 147 483 648 až 2 147 483 647** | **NEJČASTĚJŠÍ** |
| `long` | 64 bitů | -9 kvintilionů až +9 kvintilionů | Velká čísla (suffix `L`) |

### Celočíselné typy (UNSIGNED - bez znaménka)

| Typ | Velikost | Rozsah | Použití |
|-----|----------|--------|---------|
| `byte` | 8 bitů | 0 až 255 | Malá kladná čísla |
| `ushort` | 16 bitů | 0 až 65 535 | Menší kladná |
| `uint` | 32 bitů | 0 až 4 294 967 295 | Dvojnásobek int pro kladná |
| `ulong` | 64 bitů | 0 až 18 kvintilionů | Obrovská kladná |

### Desetinné typy

| Typ | Velikost | Přesnost | Rozsah | Použití |
|-----|----------|----------|--------|---------|
| `float` | 32 bitů | 7 číslic | ±3.4×10³⁸ | Grafika (suffix `f`) |
| **`double`** | **64 bitů** | **15-16 číslic** | **±1.7×10³⁰⁸** | **NEJČASTĚJŠÍ** |
| `decimal` | 128 bitů | 28-29 číslic | ±7.9×10²⁸ | **PENÍZE** (suffix `m`) |

### Ostatní typy

| Typ | Velikost | Rozsah | Použití |
|-----|----------|--------|---------|
| `char` | 16 bitů | 0 až 65 535 (Unicode) | Jeden znak |
| `bool` | 1 bit* | true / false | Logická hodnota |

*) bool zabírá 1 bajt kvůli zarovnání paměti

---

## 💻 INT (nejčastější)

```csharp
int pocetStudentu = 25;
int rok = 2025;
int teplota = -5;  // záporná čísla OK

// Aritmetické operace
int a = 10, b = 3;
int soucet = a + b;      // 13
int rozdil = a - b;      // 7
int soucin = a * b;      // 30
int podil = a / b;       // 3 (!) celočíselné dělení
int zbytek = a % b;      // 1 (modulo)
```

**U tabule řekni:** "Int je 32bitový signed celočíselný typ s rozsahem od minus 2 miliardy do plus 2 miliardy. Zabírá 4 bajty."

---

## 💻 LONG

```csharp
long velkeCislo = 9000000000000L;  // SUFFIX L povinný!
long obyvatel = 8000000000L;       // populace Země
```

**Kdy použít:** Když int nestačí (>2 miliardy).

---

## 💻 FLOAT, DOUBLE, DECIMAL

```csharp
// FLOAT - 7 číslic přesnosti
float rychlost = 9.8f;           // SUFFIX f povinný!

// DOUBLE - 15-16 číslic přesnosti (výchozí pro desetinná)
double pi = 3.14159265358979;    // žádný suffix

// DECIMAL - 28-29 číslic přesnosti (PRO PENÍZE!)
decimal cena = 19.99m;           // SUFFIX m povinný!
decimal plat = 45000.50m;
```

**Pravidlo pro maturitu:** Pro peníze VŽDY decimal!

---

## 💻 CHAR

```csharp
char pismeno = 'A';              // APOSTROFY!
char cislice = '5';              // i číslice je znak
char novyRadek = '\n';           // escape sekvence

// Char vs String
char c = 'A';                    // jeden znak - apostrofy
string s = "A";                  // text - uvozovky

// Char má číselnou hodnotu (Unicode)
int kod = (int)'A';              // 65
char dalsi = (char)('A' + 1);    // 'B'

// Převod číslice na číslo
char cif = '7';
int cislo = cif - '0';           // 7
```

**Důležité Unicode kódy:**
- `'0'` = 48, `'9'` = 57
- `'A'` = 65, `'Z'` = 90
- `'a'` = 97, `'z'` = 122

**Užitečné metody:**
```csharp
char.IsLetter('A');     // true
char.IsDigit('5');      // true
char.IsWhiteSpace(' '); // true
char.ToUpper('a');      // 'A'
char.ToLower('B');      // 'b'
```

---

## 💻 BOOL

```csharp
bool pravda = true;
bool nepravda = false;

// Logické operátory
bool a = true, b = false;
bool AND = a && b;       // false
bool OR = a || b;        // true
bool NOT = !a;           // false
```

---

## 📊 SIGNED vs UNSIGNED

**Klíčový rozdíl:** Stejná velikost, jiný rozsah!

| Typ | Rozsah |
|-----|--------|
| `int` (signed) | -2 147 483 648 až +2 147 483 647 |
| `uint` (unsigned) | 0 až 4 294 967 295 |

**Unsigned má dvojnásobek kladných čísel** (nepotřebuje bit na znaménko).

```csharp
int signed = -100;       // OK
uint unsigned = -100;    // ❌ CHYBA! uint nemůže být záporný
uint unsigned2 = 100;    // ✅ OK
```

---

## ⚠️ MATURITNÍ CHYŤÁKY - Jednoduché typy

### 1. Celočíselné dělení
```csharp
int vysledek = 10 / 3;   // 3, NE 3.333!
double vysledek2 = 10.0 / 3;  // 3.333... (alespoň jeden operand double)
```

### 2. Overflow (přetečení)
```csharp
int max = int.MaxValue;  // 2147483647
max = max + 1;           // -2147483648 (!) přeteče
```

### 3. Suffixes
```csharp
long l = 9000000000L;    // L povinné
float f = 3.14f;         // f povinné
decimal m = 19.99m;      // m povinné
```

### 4. Float porovnávání
```csharp
double a = 0.1 + 0.2;
a == 0.3;                // FALSE! (zaokrouhlovací chyby)

// Správně:
Math.Abs(a - 0.3) < 0.0001;  // TRUE
```

### 5. Char - apostrofy vs uvozovky
```csharp
char c = 'A';            // ✅ apostrofy
char c = "A";            // ❌ uvozovky jsou pro string!
```

---

# 📌 BOD 5: POLE (Array)

## 🧠 Definice
**Pole** = kolekce prvků stejného typu s pevnou velikostí, indexovaná od 0.

## 💻 Syntaxe

```csharp
// Vytvoření
int[] cisla = new int[5];              // [0,0,0,0,0]
int[] cisla2 = {10, 20, 30, 40, 50};   // s hodnotami
int[] cisla3 = new int[] {1, 2, 3};    // kombinace

// Přístup k prvkům
cisla[0] = 99;                         // první prvek
cisla[cisla.Length - 1] = 88;          // poslední prvek
int prvni = cisla[0];                  // čtení

// Procházení
for (int i = 0; i < cisla.Length; i++)
{
    Console.WriteLine(cisla[i]);
}

foreach (int c in cisla)
{
    Console.WriteLine(c);
}

// 2D pole
int[,] matice = new int[3, 4];         // 3 řádky, 4 sloupce
int[,] matice2 = {{1,2}, {3,4}, {5,6}};
matice2[0, 1] = 99;                    // řádek 0, sloupec 1
```

## 📋 Vlastnosti pole

- **Pevná velikost** - nelze měnit po vytvoření
- **Referenční typ** - předává se odkaz
- **Indexování od 0** - první prvek je `[0]`
- **Časová složitost:** přístup O(1), hledání O(n)

## 🛠️ Užitečné metody

```csharp
int[] pole = {5, 2, 8, 1, 9};

Array.Sort(pole);                    // {1, 2, 5, 8, 9}
Array.Reverse(pole);                 // {9, 8, 5, 2, 1}
int index = Array.IndexOf(pole, 8);  // najde index hodnoty
Array.Copy(zdroj, cil, delka);       // kopírování
Array.Resize(ref pole, 10);          // změna velikosti (vytvoří nové!)
```

## ⚠️ Chyťáky - Pole

```csharp
// 1. Index mimo rozsah
int[] pole = {1, 2, 3};
pole[3] = 10;                        // ❌ IndexOutOfRangeException!

// 2. Kopírování odkazu (ne dat!)
int[] a = {1, 2, 3};
int[] b = a;                         // b ukazuje na STEJNÁ data
b[0] = 999;
Console.WriteLine(a[0]);             // 999 (!) změnilo se i a

// 3. Správné kopírování dat
int[] c = (int[])a.Clone();
// nebo
int[] d = new int[a.Length];
Array.Copy(a, d, a.Length);
```

---

# 📌 BOD 6: LIST<T> A DICTIONARY<K,V>

## 💻 LIST<T>

**List** = dynamické pole s automatickou změnou velikosti.

```csharp
// Vytvoření
List<int> seznam = new List<int>();
List<int> s2 = new() {10, 20, 30};

// Přidávání
seznam.Add(10);                      // na konec
seznam.Insert(0, 5);                 // na index
seznam.AddRange(new[] {20, 30});     // více najednou

// Odebírání
seznam.Remove(10);                   // podle hodnoty
seznam.RemoveAt(0);                  // podle indexu
seznam.RemoveAll(x => x > 15);       // podmínka
seznam.Clear();                      // vše

// Přístup
int prvni = seznam[0];               // čtení
seznam[0] = 99;                      // zápis
int pocet = seznam.Count;            // počet prvků (NE Length!)

// Hledání
bool obsahuje = seznam.Contains(20); // true/false
int kde = seznam.IndexOf(20);        // index nebo -1
int prvek = seznam.Find(x => x > 25);// první vyhovující

// Třídění
seznam.Sort();
seznam.Reverse();

// Převody
int[] pole = seznam.ToArray();
List<int> zpet = pole.ToList();
```

### ⚠️ Chyťáky - List

```csharp
// 1. Count, ne Length!
seznam.Count;    // ✅
seznam.Length;   // ❌ neexistuje

// 2. Nelze měnit během foreach
foreach (int x in seznam)
{
    seznam.Remove(x);  // ❌ CHYBA!
}
// Řešení: for pozpátku nebo RemoveAll()
```

---

## 💻 DICTIONARY<TKey, TValue>

**Dictionary** = kolekce párů klíč-hodnota s rychlým vyhledáváním.

```csharp
// Vytvoření
Dictionary<string, int> skore = new();
Dictionary<string, int> s2 = new() {{"Adam", 100}, {"Bára", 150}};

// Přidávání
skore.Add("Adam", 100);              // ❌ chyba pokud klíč existuje
skore["Adam"] = 200;                 // ✅ přidá nebo přepíše
skore.TryAdd("Cyril", 120);          // vrací bool

// Přístup
int x = skore["Adam"];               // ❌ KeyNotFoundException pokud neexistuje!

// Bezpečný přístup
if (skore.TryGetValue("Eva", out int hodnota))
{
    Console.WriteLine(hodnota);
}

// Kontroly
skore.ContainsKey("Adam");           // O(1)
skore.ContainsValue(100);            // O(n)!

// Procházení
foreach (var (klic, hodnota) in skore)
{
    Console.WriteLine($"{klic}: {hodnota}");
}
```

### ⚠️ Chyťáky - Dictionary

```csharp
// 1. Neexistující klíč
skore["Neexistuje"];                 // ❌ KeyNotFoundException!
// Řešení: TryGetValue()

// 2. Duplicitní klíč v Add
skore.Add("Adam", 100);
skore.Add("Adam", 200);              // ❌ ArgumentException!
// Řešení: použij indexer skore["Adam"] = 200;

// 3. ContainsValue je O(n)!
skore.ContainsValue(100);            // pomalé - projde vše
```

---

# 📌 BOD 7: STRING

## 🧠 Definice
**String** = posloupnost znaků (char), referenční typ, ale **IMMUTABLE** (neměnný).

## 💻 Základní operace

```csharp
// Vytvoření
string text = "Ahoj";                // uvozovky!
string interpolace = $"Věk: {vek}";  // string interpolation
string verbatim = @"C:\Users\Adam"; // bez escape

// Délka
int delka = text.Length;             // 4

// Indexování (jako pole charů)
char c = text[0];                    // 'A'
text[0] = 'X';                       // ❌ CHYBA! immutable

// Spojování
string s = "Ahoj" + " " + "světe";
string s2 = $"Jmenuji se {jmeno}";   // doporučené
```

## 🛠️ Užitečné metody

```csharp
string text = "  Ahoj Světe!  ";

// Úpravy (vrací NOVÝ string!)
text.ToUpper();                      // "  AHOJ SVĚTE!  "
text.ToLower();                      // "  ahoj světe!  "
text.Trim();                         // "Ahoj Světe!"
text.Replace("Ahoj", "Čau");         // "  Čau Světe!  "
text.Substring(2, 4);                // "Ahoj"

// Hledání
text.Contains("Ahoj");               // true
text.StartsWith("  A");              // true
text.EndsWith("!  ");                // true
text.IndexOf("Světe");               // 7

// Rozdělení a spojení
string[] slova = text.Split(' ');    // {"", "", "Ahoj", "Světe!", "", ""}
string spojeno = string.Join("-", slova);

// Kontroly
string.IsNullOrEmpty(text);          // false
string.IsNullOrWhiteSpace("   ");    // true
```

## ⚠️ Chyťáky - String

```csharp
// 1. IMMUTABLE - musíš přiřadit zpět!
string s = "ahoj";
s.ToUpper();                         // ❌ nic se nestane!
s = s.ToUpper();                     // ✅ "AHOJ"

// 2. Apostrofy vs uvozovky
char c = 'A';                        // apostrofy = char
string s = "A";                      // uvozovky = string

// 3. null vs ""
string a = null;                     // žádný objekt
string b = "";                       // prázdný string (objekt existuje)
a.Length;                            // ❌ NullReferenceException!
b.Length;                            // ✅ 0

// 4. == porovnává OBSAH (speciální chování stringu)
string x = "test";
string y = "test";
x == y;                              // true (porovnává obsah)
```

## 🎨 Escape sekvence

| Sekvence | Význam |
|----------|--------|
| `\n` | Nový řádek |
| `\t` | Tabulátor |
| `\\` | Zpětné lomítko |
| `\"` | Uvozovky |
| `\'` | Apostrof |

---

# 📌 BOD 8: STRUCT vs CLASS

## 🎯 Klíčový rozdíl

| Vlastnost | STRUCT | CLASS |
|-----------|--------|-------|
| **Typ** | HODNOTOVÝ | REFERENČNÍ |
| **Uložení** | STACK | HEAP |
| **Kopírování** | Kopíruje HODNOTU | Kopíruje ODKAZ |
| **Dědičnost** | ❌ nelze | ✅ může |
| **Null** | ❌ (bez `?`) | ✅ může |

## 💻 Příklad

```csharp
// STRUCT - kopíruje hodnotu
struct BodStruct
{
    public int X, Y;
}

BodStruct a = new BodStruct { X = 10 };
BodStruct b = a;                     // KOPIE hodnoty
b.X = 99;
Console.WriteLine(a.X);              // 10 (nezměněno!)


// CLASS - kopíruje odkaz
class BodClass
{
    public int X, Y;
}

BodClass c = new BodClass { X = 10 };
BodClass d = c;                      // KOPIE odkazu
d.X = 99;
Console.WriteLine(c.X);              // 99 (!) změnilo se i c
```

## 📋 Kdy použít co?

**STRUCT:**
- Malá data (<16 bajtů)
- Logicky jedna hodnota
- Nepotřebuji dědičnost
- Příklady: Point, Color, DateTime

**CLASS:**
- Větší/složitější data
- Potřebuji dědičnost
- Objekt má "identitu"
- Příklady: Student, List, Dictionary

---

# 📌 BOD 8b: DETAILNÍ SYNTAXE TŘÍDY (CLASS)

## 🏗️ Základní struktura třídy

```csharp
// MODIFIKÁTOR PŘÍSTUPU + KLÍČOVÉ SLOVO + NÁZEV
public class Student
{
    // 1. DATOVÉ POLOŽKY (fields) - privátní!
    private string jmeno;
    private int vek;
    private static int pocetStudentu = 0;  // statická = sdílená všemi instancemi
    
    // 2. KONSTRUKTOR - volá se při "new"
    public Student(string jmeno, int vek)
    {
        this.jmeno = jmeno;   // "this" odkazuje na aktuální instanci
        this.vek = vek;
        pocetStudentu++;       // zvýšíme počítadlo
    }
    
    // 3. VLASTNOSTI (properties) - bezpečný přístup k datům
    public string Jmeno 
    { 
        get { return jmeno; }           // getter - čtení
        set { jmeno = value; }          // setter - zápis
    }
    
    // Zkrácená syntaxe (auto-property)
    public int Vek { get; set; }
    
    // Pouze pro čtení (read-only)
    public int RokNarozeni { get; }
    
    // 4. METODY - funkce třídy
    public void Predstav()
    {
        Console.WriteLine($"Jsem {jmeno}, je mi {vek} let.");
    }
    
    // Metoda s návratovou hodnotou
    public bool JePlnolety()
    {
        return vek >= 18;
    }
    
    // 5. STATICKÁ METODA - volá se na třídě, ne na instanci
    public static int GetPocetStudentu()
    {
        return pocetStudentu;
    }
    
    // 6. OVERRIDE ToString() - co se vypíše při Console.WriteLine(student)
    public override string ToString()
    {
        return $"Student: {jmeno} ({vek} let)";
    }
}
```

## 🔧 Použití třídy

```csharp
// Vytvoření instance (volá se konstruktor)
Student pepa = new Student("Pepa", 20);

// Přístup přes vlastnosti
Console.WriteLine(pepa.Jmeno);     // "Pepa"
pepa.Vek = 21;                     // změna věku

// Volání metody na instanci
pepa.Predstav();                   // "Jsem Pepa, je mi 21 let."

// Volání statické metody na TŘÍDĚ (ne na instanci!)
int pocet = Student.GetPocetStudentu();  // ✅
// int pocet = pepa.GetPocetStudentu();  // ❌ funguje, ale není správně

// Výpis (použije ToString())
Console.WriteLine(pepa);           // "Student: Pepa (21 let)"
```

## 🔐 Modifikátory přístupu

| Modifikátor | Viditelnost | Použití |
|-------------|-------------|---------|
| `public` | Odkudkoliv | Veřejné API třídy |
| `private` | Jen v třídě | Interní data (DEFAULT!) |
| `protected` | Třída + potomci | Pro dědičnost |
| `internal` | V rámci projektu | Mezi třídami projektu |

```csharp
class Ucet
{
    private decimal zustatek;     // ❌ zvenku nedostupné
    public decimal Zustatek       // ✅ bezpečný přístup
    {
        get { return zustatek; }
        private set { zustatek = value; }  // setter jen interně
    }
}
```

## 📐 Typy vlastností (Properties)

```csharp
class Ukazka
{
    // 1. Auto-property (nejčastější)
    public string Jmeno { get; set; }
    
    // 2. S výchozí hodnotou (C# 6+)
    public int Pocet { get; set; } = 0;
    
    // 3. Read-only (jen getter)
    public DateTime Vytvoreno { get; } = DateTime.Now;
    
    // 4. Computed property (vypočítaná)
    public string Pozdrav => $"Ahoj, {Jmeno}!";
    
    // 5. S validací v setteru
    private int vek;
    public int Vek
    {
        get { return vek; }
        set 
        { 
            if (value >= 0 && value <= 150)
                vek = value;
            else
                throw new ArgumentException("Neplatný věk!");
        }
    }
}
```

## 🏭 Konstruktory

```csharp
class Auto
{
    public string Znacka { get; set; }
    public string Model { get; set; }
    public int Rok { get; set; }
    
    // 1. Bezparametrický konstruktor
    public Auto()
    {
        Znacka = "Neznámá";
        Model = "Neznámý";
        Rok = 2000;
    }
    
    // 2. Parametrický konstruktor
    public Auto(string znacka, string model)
    {
        Znacka = znacka;
        Model = model;
        Rok = DateTime.Now.Year;
    }
    
    // 3. Plný konstruktor (volá jiný přes "this")
    public Auto(string znacka, string model, int rok) : this(znacka, model)
    {
        Rok = rok;
    }
}

// Různé způsoby vytvoření:
Auto a1 = new Auto();                           // bezparametrický
Auto a2 = new Auto("Škoda", "Octavia");         // 2 parametry
Auto a3 = new Auto("BMW", "M3", 2023);          // 3 parametry

// Object initializer (alternativa)
Auto a4 = new Auto { Znacka = "Audi", Model = "A4", Rok = 2022 };
```

## 🔄 Static vs Instance

```csharp
class Kalkulacka
{
    // STATICKÉ - patří TŘÍDĚ (sdílené)
    public static double PI = 3.14159;
    
    public static int Secti(int a, int b)
    {
        return a + b;
    }
    
    // INSTANČNÍ - patří konkrétnímu OBJEKTU
    public string Nazev { get; set; }
    
    public void Vypis()
    {
        Console.WriteLine($"Kalkulačka: {Nazev}");
    }
}

// Statické = volám na TŘÍDĚ
double pi = Kalkulacka.PI;
int soucet = Kalkulacka.Secti(5, 3);

// Instanční = volám na OBJEKTU
Kalkulacka k = new Kalkulacka();
k.Nazev = "Moje kalkulačka";
k.Vypis();
```

## 📝 Vizualizace: Co se děje v paměti

```
STACK                          HEAP
┌──────────────────┐          ┌─────────────────────────┐
│ pepa (reference) │────────► │ Student objekt          │
│ [0x1234]         │          │ ┌─────────────────────┐ │
└──────────────────┘          │ │ jmeno: "Pepa"       │ │
                              │ │ vek: 20             │ │
┌──────────────────┐          │ └─────────────────────┘ │
│ karel (reference)│────────► ├─────────────────────────┤
│ [0x5678]         │          │ Student objekt          │
└──────────────────┘          │ ┌─────────────────────┐ │
                              │ │ jmeno: "Karel"      │ │
                              │ │ vek: 22             │ │
                              │ └─────────────────────┘ │
                              └─────────────────────────┘

// Pozor na přiřazení!
Student kopie = pepa;   // NEKOPÍRUJE objekt!
                        // Obě proměnné ukazují na STEJNÝ objekt!
```

## ⚠️ Časté chyby u tříd

```csharp
// ❌ CHYBA 1: Zapomenutý "new"
Student s;
s.Jmeno = "Test";  // NullReferenceException!

// ✅ SPRÁVNĚ:
Student s = new Student();
s.Jmeno = "Test";

// ❌ CHYBA 2: Porovnávání referencí místo hodnot
Student a = new Student("Pepa", 20);
Student b = new Student("Pepa", 20);
if (a == b)  // FALSE! Různé objekty v paměti

// ✅ SPRÁVNĚ: Porovnat vlastnosti nebo override Equals()

// ❌ CHYBA 3: Změna přes "kopii"
Student original = new Student("Pepa", 20);
Student kopie = original;
kopie.Jmeno = "Karel";
// POZOR: original.Jmeno je teď taky "Karel"!
```

## 🎯 Pro maturitu: Co říct u tabule

> "Třída je šablona pro vytváření objektů. Obsahuje:
> - **Datové položky** (fields) - data objektu
> - **Vlastnosti** (properties) - bezpečný přístup k datům
> - **Konstruktor** - inicializace při vytvoření
> - **Metody** - chování objektu
> 
> Třída je **referenční typ** - na stacku je jen odkaz, samotná data jsou na heapu."

---

# 📌 BOD 9: NULL A NULLABLE TYPY

## 🧠 Co je NULL?

**NULL** = "nic" / "žádná hodnota" / "ukazatel nikam"

```csharp
string text = null;      // žádný objekt
string prazdny = "";     // prázdný string (objekt existuje!)
```

## 📊 Kdo může být NULL?

| Typ | Může být NULL? |
|-----|----------------|
| Referenční (string, class, array, List) | ✅ ANO |
| Hodnotové (int, double, bool, struct) | ❌ NE |
| Nullable hodnotové (int?, double?) | ✅ ANO |

## 💻 NULLABLE TYPY

```csharp
// Přidej ? za typ
int? cislo = null;                   // ✅ OK
int? vek = 18;                       // má hodnotu

// Kontrola hodnoty
if (cislo.HasValue)
{
    Console.WriteLine(cislo.Value);
}

// Nebo
if (cislo != null)
{
    Console.WriteLine(cislo);
}

// Získání hodnoty s výchozí
int vysledek = cislo ?? 0;           // pokud null, použij 0
int vysledek2 = cislo.GetValueOrDefault(99);
```

## 🎯 NULL OPERÁTORY

### `??` - Null-coalescing
```csharp
string jmeno = null;
string vysledek = jmeno ?? "Neznámý";  // "Neznámý"
```

### `??=` - Null-coalescing assignment
```csharp
string jmeno = null;
jmeno ??= "Výchozí";                 // přiřadí jen pokud je null
```

### `?.` - Null-conditional (bezpečný přístup)
```csharp
string text = null;
int? delka = text?.Length;           // null (ne exception!)

// Řetězení
string ulice = osoba?.Adresa?.Ulice ?? "Neznámá";
```

### `?[]` - Null-conditional index
```csharp
int[] pole = null;
int? prvni = pole?[0];               // null (ne exception!)
```

## ⚠️ Chyťáky - NULL

```csharp
// 1. NullReferenceException
string text = null;
text.Length;                         // ❌ NullReferenceException!

// Řešení:
text?.Length ?? 0;                   // ✅

// 2. Nullable → normální typ
int? nullable = 10;
int normalni = nullable;             // ❌ CHYBA!
int normalni = nullable ?? 0;        // ✅
int normalni = nullable.Value;       // ⚠️ může vyhodit výjimku

// 3. IsNullOrEmpty vs IsNullOrWhiteSpace
string.IsNullOrEmpty(null);          // true
string.IsNullOrEmpty("");            // true
string.IsNullOrEmpty("   ");         // false (!)

string.IsNullOrWhiteSpace("   ");    // true
```

---

# 📌 BOD 10: HODNOTOVÉ vs REFERENČNÍ TYPY

## 📊 PŘEHLED

### HODNOTOVÉ TYPY (Value Types) → STACK
```
int, long, short, byte, sbyte
uint, ulong, ushort
float, double, decimal
char, bool
struct, enum
DateTime, TimeSpan
int?, double? (nullable)
```

### REFERENČNÍ TYPY (Reference Types) → HEAP
```
string
class
array (int[], string[])
List<T>, Dictionary<K,V>, Queue<T>, Stack<T>
interface
delegate
```

## 🎨 VIZUALIZACE V PAMĚTI

```
STACK (rychlý, malý)          HEAP (pomalejší, velký)
┌─────────────────┐           ┌─────────────────────┐
│ int cislo = 42  │           │                     │
├─────────────────┤           │  ┌───────────────┐  │
│ Bod bod         │           │  │ "Ahoj"        │←─┼── string
│  X: 10          │           │  └───────────────┘  │
│  Y: 20          │           │                     │
├─────────────────┤           │  ┌───────────────┐  │
│ string text ════╪═══════════╪→ │ Osoba objekt  │  │
├─────────────────┤           │  └───────────────┘  │
│ Osoba osoba ════╪═══════════╪→                    │
└─────────────────┘           └─────────────────────┘
```

## 💻 CHOVÁNÍ PŘI KOPÍROVÁNÍ

```csharp
// HODNOTOVÝ TYP - kopíruje HODNOTU
int a = 10;
int b = a;
b = 99;
Console.WriteLine(a);                // 10 (nezměněno!)


// REFERENČNÍ TYP - kopíruje ODKAZ
class Osoba { public string Jmeno; }

Osoba x = new Osoba { Jmeno = "Adam" };
Osoba y = x;
y.Jmeno = "Bára";
Console.WriteLine(x.Jmeno);          // "Bára" (!) změnilo se i x
```

## 💻 PŘEDÁVÁNÍ DO FUNKCE

```csharp
// HODNOTOVÝ TYP - předává kopii
static void ZmenInt(int cislo)
{
    cislo = 999;                     // mění lokální kopii
}

int x = 10;
ZmenInt(x);
Console.WriteLine(x);                // 10 (nezměněno!)


// REFERENČNÍ TYP - předává odkaz
static void ZmenOsobu(Osoba o)
{
    o.Jmeno = "Změněno";             // mění originál!
}

Osoba adam = new Osoba { Jmeno = "Adam" };
ZmenOsobu(adam);
Console.WriteLine(adam.Jmeno);       // "Změněno" (!)
```

## 📋 SROVNÁVACÍ TABULKA

| Vlastnost | HODNOTOVÝ | REFERENČNÍ |
|-----------|-----------|------------|
| Uložení | Stack | Heap |
| Kopírování | Celá hodnota | Jen odkaz |
| Předání do funkce | Kopie | Odkaz |
| `==` porovnává | Hodnoty | Reference* |
| Může být null | NE (jen s `?`) | ANO |
| Výchozí hodnota | 0, false, '\0' | null |
| Garbage Collector | NE | ANO |

*) String je výjimka - porovnává obsah

## ⚠️ KLÍČOVÉ CHYŤÁKY

### 1. Pole JE referenční!
```csharp
int[] a = {1, 2, 3};
int[] b = a;
b[0] = 999;
Console.WriteLine(a[0]);             // 999 (!)
```

### 2. String je referenční, ale immutable
```csharp
string a = "Ahoj";
string b = a;
b = "Čau";                           // vytvoří NOVÝ string
Console.WriteLine(a);                // "Ahoj" (nezměněno)
```

### 3. Hodnotový typ uvnitř class jde na heap
```csharp
class Kontejner
{
    public int Cislo;                // int na HEAP (součást třídy)
}
```

---

# ⚠️ SOUHRN VŠECH CHYŤÁKŮ

## Jednoduché typy
1. **Suffixes:** `L` pro long, `f` pro float, `m` pro decimal
2. **Celočíselné dělení:** `10/3 = 3` (ne 3.333)
3. **Overflow:** `int.MaxValue + 1` přeteče
4. **Float porovnávání:** NIKDY `==`, vždy epsilon
5. **Char:** apostrofy `'A'`, ne uvozovky

## Složené typy
6. **String immutable:** musíš přiřadit zpět `s = s.ToUpper();`
7. **Pole index od 0:** poslední je `Length - 1`
8. **List:** `Count` (ne `Length`)
9. **Dictionary:** používej `TryGetValue()` (ne `[]`)

## Struct vs Class
10. **Struct kopíruje hodnotu**, class kopíruje odkaz

## NULL
11. **NullReferenceException:** vždy kontroluj před přístupem
12. **Nullable → normální:** musíš použít `??` nebo `.Value`

## Hodnotové vs Referenční
13. **Pole je referenční typ!**
14. **Předávání do funkce:** hodnotový = kopie, referenční = odkaz

---

# 🎯 CO ŘÍCT U TABULE (SHRNUTÍ)

## Proměnná
"Pojmenované místo v paměti s názvem, typem a hodnotou."

## Jednoduché typy
"Int je 32bitový signed typ, rozsah ±2 miliardy. Double je 64bitový pro desetinná čísla. Decimal pro peníze - 128 bitů, 28 číslic přesnost."

## Pole
"Kolekce stejného typu s pevnou velikostí, indexovaná od 0."

## List
"Dynamické pole - automaticky mění velikost. Add, Remove, Contains."

## Dictionary
"Kolekce párů klíč-hodnota. Rychlé vyhledávání O(1) podle klíče."

## String
"Referenční typ, ale immutable - každá změna vytvoří nový objekt."

## Struct vs Class
"Struct je hodnotový typ (stack, kopíruje hodnotu), class je referenční typ (heap, kopíruje odkaz)."

## NULL
"Referenční typy mohou být null. Hodnotové jen s otazníkem (int?)."

## Hodnotové vs Referenční
"Hodnotové na stacku, kopírují hodnotu. Referenční na heapu, kopírují odkaz."

---

# 🚀 SENIOR TIPY

```csharp
// Pattern matching
if (obj is string s) { }

// Null-forgiving operator
string text = moznaNull!;

// Record (C# 9+)
record Osoba(string Jmeno, int Vek);

// var pro jednoduchost
var seznam = new List<int>();

// StringBuilder pro mnoho spojování
var sb = new StringBuilder();
sb.Append("text");
```

---

*Zápisky vytvořeny: 17. prosince 2024*  