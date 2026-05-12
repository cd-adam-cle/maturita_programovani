# Zápisky: Otázka č. 17 - Objektově orientované programování

**Datum:** 2026-05-12
**Status:** Hotovo

---

## Checklist bodů otázky

- [x] Bod 1: Základní myšlenky procedurálního programování
- [x] Bod 2: Základní myšlenky OOP, motivace
- [x] Bod 3: Pojem třída
- [x] Bod 4: Pojem instance/objekt
- [x] Bod 5: Pojem referenční proměnná
- [x] Bod 6: Pojem konstruktor
- [x] Bod 7: Pojem zapouzdření
- [x] Bod 8: Pojem vlastnost (property)
- [x] Bod 9: Pojem funkce vs metoda
- [x] Bod 10: Pojem static
- [x] Bod 11: Pojem polymorfismus
- [x] BONUS: Konstruktory v dědičnosti, virtual/override/abstract

---

## Úvod a historický kontext

**Objektově orientované programování (OOP)** je programovací paradigma postavené na **objektech** - entitách spojujících data (stav) s chováním (metodami). Vzniklo v 60. letech jako reakce na rostoucí složitost procedurálních programů.

**Klíčové historické momenty:**
- **1962** - jazyk **Simula 67** (Ole-Johan Dahl, Kristen Nygaard, Norsko) zavádí pojmy třída, objekt, dědičnost. Vznikl pro simulace - odtud název.
- **1972** - **Smalltalk** (Alan Kay, Xerox PARC) - první ryze objektový jazyk. Kay vymyslel termín "object-oriented" a propagoval model **message passing** - objekty si posílají zprávy.
- **1983** - **C++** (Bjarne Stroustrup) - OOP rozšíření C, kompromis mezi výkonem a abstrakcí.
- **1995** - **Java** (James Gosling, Sun) - "Write once, run anywhere", silně typovaná, garbage collected, byte code na JVM.
- **2002** - **C#** (Anders Hejlsberg, Microsoft) - reakce na Javu, později přidáno mnoho moderních rysů (LINQ, async/await, nullable types, records).

Dnes je OOP **dominantní paradigma** v průmyslu (Java, C#, Python, C++, Swift, Kotlin), ale ne univerzálně přijaté. Funkcionální programování (Haskell, F#, Elixir) ho v některých oblastech doplňuje.

---

## Klíčové koncepty

---

## BOD 1: Procedurální programování

**Teorie:**

**Procedurální programování** je styl, kde program tvoří sekvence příkazů organizované do **procedur** (funkcí). Vychází ze strukturovaného programování (Dijkstra, 60. léta), které zavedlo if/else/while místo skoků goto.

**Klíčové rysy:**
- Kód = posloupnost instrukcí (shora dolů)
- Data a funkce jsou **oddělené**
- Globální stav - data jsou často sdílená přes celý program
- Programátor přemýšlí **top-down** (rozklad problému na podproblémy)

**Stavební kameny:**
- **Proměnné** - pojmenovaná místa v paměti pro data.
- **Funkce/procedury** - pojmenované bloky kódu s parametry a návratovou hodnotou.
- **Řídicí struktury** - sekvence, podmínka, cyklus (Böhm-Jacopiniho věta: tyto 3 stačí pro Turingovsky úplnost).
- **Globální/lokální stav** - data dostupná všude vs jen v rámci funkce.

**Příklady jazyků:** C, Pascal, Basic, Fortran, COBOL.

```csharp
string studentJmeno = "Pepa";
int studentVek = 20;
int[] studentZnamky = { 1, 2, 1, 3, 2 };

static double SpocitejPrumer(int[] znamky)
{
    int suma = 0;
    for (int i = 0; i < znamky.Length; i++)
        suma += znamky[i];
    return (double)suma / znamky.Length;
}

static void VypisStudenta(string jmeno, int vek, double prumer)
{
    Console.WriteLine($"Student: {jmeno}, Věk: {vek}, Průměr: {prumer:F2}");
}
```

**Problémy procedurálního přístupu:**

| Problém | Důsledek |
|---------|----------|
| Data a logika oddělené | Těžko se udržuje konzistence |
| Globální stav | Změna na jednom místě rozbije jiné |
| Slabá modularita | Kód se těžko znovupoužívá |
| Žádné typové vazby | Funkce nevěnuje souvislost s daty, na kterých pracuje |
| Škálování | Při tisících funkcí a struktur se stává nepřehledné |

**Procedurální programování není špatné** - pro malé skripty, výpočetní jádra, embedded systémy je často **lepší** než OOP (jednoduchost, predikovatelný výkon). Linuxové jádro je psané v C a má desítky milionů řádků.

---

## BOD 2: OOP - Motivace a 4 pilíře

**Teorie:**

**OOP** spojuje data a funkce do jednoho celku - **objektu**. Místo "co se má udělat" (procedurální) se ptáme "co tu máme za entity a jak spolu komunikují".

**Čtyři pilíře OOP:**

| Pilíř | Význam |
|-------|--------|
| **Zapouzdření (Encapsulation)** | Data + metody pohromadě, skrytí implementace |
| **Abstrakce (Abstraction)** | Zjednodušení - ukazujeme jen důležité, skrýváme detaily |
| **Dědičnost (Inheritance)** | Tvorba nových tříd z existujících |
| **Polymorfismus (Polymorphism)** | Stejné rozhraní, různé chování |

**Mnemo:** **A**bstrakce, **E**ncapsulace, **I**nheritance, **P**olymorfismus - "**A**lan **K**ay **I**n **P**aris" (v paměti pro maturitu jsou často 4 pilíře OOP žádanou znalostí).

**Vizualizace - objekty mají vlastní data, ale sdílejí metody (na úrovni třídy):**

```
   OBJEKT "Student"              OBJEKT "Student"
   +------------------+          +------------------+
   | DATA:            |          | DATA:            |
   |  jmeno = "Pepa"  |          |  jmeno = "Jana"  |
   |  vek = 20        |          |  vek = 22        |
   +------------------+          +------------------+
   | METODY:          |          | METODY:          |
   |  SpocitejPrumer  |          |  SpocitejPrumer  |
   |  Vypis()         |          |  Vypis()         |
   +------------------+          +------------------+
```

**Výhody OOP:**

| Výhoda | Vysvětlení |
|--------|------------|
| Modularita | Každý objekt je samostatný "modul" |
| Znovupoužitelnost | Třídu použiješ v jiném projektu |
| Údržba | Změna na jednom místě, zbytek funguje |
| Bezpečnost | Data jsou chráněna (zapouzdření) |
| Modelování reálného světa | Třídy odrážejí entity (Student, Auto, Kniha) |
| Týmová spolupráce | Jasné rozhraní = několik vývojářů může pracovat paralelně |

**Tabulka: Procedurální vs OOP:**

| Vlastnost | Procedurální | Objektové (OOP) |
|-----------|--------------|-----------------|
| Základní jednotka | Funkce / Procedura | Objekt (Data + Metody) |
| Přístup | Shora dolů (Top-down) | Zdola nahoru (Z komponent) |
| Data | Samostatná, často globální | Skrytá uvnitř objektu (zapouzdřená) |
| Změna kódu | Riziková (ovlivní zbytek) | Bezpečnější (lokální v třídě) |
| Příklady jazyků | C, Pascal, Basic | C#, Java, Python |

**Kritika OOP:** moderní kritici (např. Joe Armstrong, tvůrce Erlangu) namítají, že OOP vede k přílišnému provázání objektů, ztrácí výkon a komplikuje paralelizaci. **Alan Kay** sám později řekl, že "object-oriented" z jeho původní vize Smalltalku zdegenerovalo. Funkcionální paradigma se v posledních letech vrací do hlavního proudu (Rust, Scala, F#, moderní C# má record types, pattern matching).

---

## Konkrétní příklady: Procedurální vs OOP

### Příklad 1: Bankovní účet

**Procedurální:**

```csharp
// DATA - oddělená, globální, nechráněná
string[] jmena = new string[100];
decimal[] zustatky = new decimal[100];
int pocetUctu = 0;

static void Vloz(int index, decimal castka)
{
    zustatky[index] += castka;  // Žádná validace
}

// POUŽITÍ - nebezpečné
int ucet = 0;
Vloz(ucet, 500);
zustatky[ucet] = -99999;  // Může kdokoliv změnit
```

**OOP:**

```csharp
public class BankovniUcet
{
    public string Majitel { get; private set; }
    public decimal Zustatek { get; private set; }

    public BankovniUcet(string majitel, decimal vklad)
    {
        Majitel = majitel;
        Zustatek = vklad;
    }

    public void Vloz(decimal castka)
    {
        if (castka > 0)
            Zustatek += castka;
    }
}

// POUŽITÍ - bezpečné
BankovniUcet ucet = new BankovniUcet("Pepa", 1000);
ucet.Vloz(500);
// ucet.Zustatek = -99999;  // Nejde, private set
```

### Příklad 2: Herní postavy (polymorfismus)

**Procedurální (anti-pattern - nekonečný switch):**

```csharp
string[] typy = { "warrior", "mage" };
int[] sily = { 20, 5 };

static int Utok(int index)
{
    // Přidat nový typ = změnit VŠECHNY funkce
    if (typy[index] == "warrior") return sily[index];
    else if (typy[index] == "mage") return sily[index] * 2;
    return 0;
}
```

**OOP:**

```csharp
public abstract class Postava
{
    public string Jmeno { get; set; }
    public int Zivoty { get; set; }

    public abstract int Utok();
}

public class Warrior : Postava
{
    public override int Utok()
    {
        Console.WriteLine($"{Jmeno} sekne mečem!");
        return 20;
    }
}

public class Mage : Postava
{
    public override int Utok()
    {
        Console.WriteLine($"{Jmeno} sešle kouzlo!");
        return 40;
    }
}

// Polymorfismus - jeden cyklus, různé chování
Postava[] tym = { new Warrior { Jmeno = "Conan" },
                  new Mage { Jmeno = "Gandalf" } };

foreach (Postava p in tym)
    p.Utok();

// Přidat Archer = NOVÁ TŘÍDA, žádná změna existujícího kódu (Open/Closed Principle)
```

### Kdy použít OOP vs procedurální:

| Situace | Doporučení |
|---------|------------|
| Malý skript, jednorázový výpočet | Procedurální |
| Více objektů stejného typu | OOP |
| Různé typy se společným rozhraním | OOP + polymorfismus |
| Potřeba ochrany dat | OOP + zapouzdření |
| Rozšiřitelný systém | OOP + dědičnost / rozhraní |
| Výpočetně náročné jádro | Procedurální (lepší cache locality) |
| Vysoce paralelní | Funkcionální nebo Actor model |

---

## BOD 3: Třída (Class)

**Teorie:**

**Třída** je šablona / předpis pro vytváření objektů. Definuje **strukturu** (jaká data objekt obsahuje) a **chování** (jaké metody umí).

Analogie:
- Třída = stavební plán domu, formička na cukroví, kuchařský recept.
- Objekt (instance) = konkrétní postavený dům, konkrétní cukroví, konkrétní pečený dort.

**Třída sama o sobě nezabírá paměť pro data instancí** (kromě statických polí). Až vytvořením instance přes `new` se alokuje paměť na haldě (heap) a vyplní se podle plánu třídy.

```csharp
public class Student
{
    // 1. DATOVÉ POLOŽKY (fields) - obvykle privátní
    private string jmeno;
    private int vek;

    // 2. KONSTRUKTOR - volá se při "new"
    public Student(string jmeno, int vek)
    {
        this.jmeno = jmeno;
        this.vek = vek;
    }

    // 3. VLASTNOSTI (properties)
    public string Jmeno
    {
        get { return jmeno; }
        set { jmeno = value; }
    }

    public int Vek { get; set; }   // auto-property

    // 4. METODY
    public void Vypis()
    {
        Console.WriteLine($"Student: {jmeno}, věk: {vek}");
    }
}
```

**Anatomie třídy v C#:**
- **Pole (field)** - syrová proměnná uvnitř třídy, obvykle `private`.
- **Vlastnost (property)** - veřejná "obálka" nad polem s get/set.
- **Metoda** - funkce patřící třídě.
- **Konstruktor** - speciální metoda pro inicializaci nové instance.
- **Destruktor / Finalizer** - `~Student()` - volán garbage collectorem (v C# se používá vzácně).
- **Indexer** - `this[int i]` - umožňuje přistupovat k objektu jako k poli.
- **Operátor** - přetížení `+`, `-`, `==` atd. pomocí `public static T operator +(...)`.
- **Událost (event)** - mechanismus pro publish/subscribe pattern.
- **Vnořená třída** - třída uvnitř třídy.

**Modifikátory přístupu:**

| Modifikátor | Viditelnost |
|-------------|-------------|
| `public` | Odkudkoli |
| `private` | Jen ve své třídě (default pro členy třídy) |
| `protected` | Třída + potomci |
| `internal` | V rámci stejné assembly (projektu) |
| `protected internal` | Potomci NEBO stejná assembly |
| `private protected` | Potomci ve stejné assembly |

**Princip nejmenšího privilegia:** výchozí přístup je vždy ten nejméně otevřený (`private`). Postupně otvíráme jen to, co je opravdu potřeba zvenku. Tomu se říká **information hiding** - vnitřní implementaci skrýváme, abychom ji mohli změnit bez dopadu na uživatele třídy.

---

## BOD 4: Instance / Objekt

**Teorie:**

**Instance (objekt)** je konkrétní výskyt třídy v paměti, vytvořený pomocí `new`. Každá instance má **vlastní data**, ale **sdílí metody** s ostatními instancemi téže třídy (metody jsou definovány jednou v třídě).

```csharp
// VYTVOŘENÍ INSTANCÍ
Student pepa = new Student("Pepa", 20);    // 1. instance
Student jana = new Student("Jana", 22);    // 2. instance

// Každý objekt má VLASTNÍ data
Console.WriteLine(pepa.Jmeno);   // "Pepa"
Console.WriteLine(jana.Jmeno);   // "Jana"

// Změna jednoho NEOVLIVNÍ ostatní
pepa.Vek = 21;
Console.WriteLine(jana.Vek);     // 22 (nezměněno)
```

**Co dělá `new`:**

| Část | Co dělá |
|------|---------|
| `Student` | Typ proměnné |
| `pepa` | Název proměnné (reference) |
| `new` | Alokuje paměť na heapu pro objekt, inicializuje fields default hodnotami |
| `Student("Pepa", 20)` | Zavolá konstruktor |

**Bez `new` = chyba:**

```csharp
Student pepa;              // Jen deklarace (null)
pepa.Jmeno = "Pepa";       // NullReferenceException

Student pepa = new Student("Pepa", 20);  // OK
```

**Životní cyklus objektu:**
1. **Alokace** - `new` rezervuje paměť na heapu.
2. **Inicializace** - fields se nastaví na default (0, null, false).
3. **Konstrukce** - volá se konstruktor.
4. **Použití** - objekt žije, dokud na něj existuje reference.
5. **Garbage Collection** - po ztrátě poslední reference označí GC objekt jako mrtvý a uvolní paměť.
6. **Finalizace** - pokud má objekt finalizer (`~Class()`), zavolá se před uvolněním.

---

## BOD 5: Referenční proměnná

**Teorie:**

**Referenční proměnná** obsahuje **odkaz (adresu)** na místo v paměti, ne přímo data. Tím se liší od **hodnotové proměnné**, která obsahuje data přímo.

**Dva typy proměnných v C#:**

| Typ | Obsahuje | Uloženo na | Příklady |
|-----|----------|------------|----------|
| **Hodnotový** | Přímo hodnotu | STACK | `int`, `double`, `bool`, `struct`, `enum` |
| **Referenční** | Odkaz (adresu) | STACK (odkaz) + HEAP (data) | `class`, `string`, `array`, `interface`, delegate |

**Stack vs heap:**
- **Stack** - LIFO struktura pro lokální proměnné. Rychlá alokace/dealokace (jen posunutí ukazatele). Omezená velikost (typicky 1 MB).
- **Heap** - obecný "bazén" paměti pro dlouhožijící objekty. Pomalejší alokace, ale neomezená velikost (jen RAM). Spravován garbage collectorem.

**Vizualizace:**

```
HODNOTOVÝ TYP                    REFERENČNÍ TYP

int a = 42;                      Student s = new Student("Pepa", 20);

   STACK                            STACK              HEAP
+---------+                     +---------+      +--------------+
| a = 42  |                     | s = 0x1 | ---> | Jmeno="Pepa" |
+---------+                     +---------+      | Vek=20       |
                                                  +--------------+
```

**Klíčový rozdíl - kopírování:**

```csharp
// HODNOTOVÝ - kopíruje HODNOTU
int a = 10;
int b = a;
b = 99;
Console.WriteLine(a);  // 10 (nezměněno)

// REFERENČNÍ - kopíruje ODKAZ
Student s1 = new Student("Pepa", 20);
Student s2 = s1;       // Obě ukazují na STEJNÝ objekt
s2.Vek = 99;
Console.WriteLine(s1.Vek);  // 99
```

**Předávání parametrů:**

| Klíčové slovo | Význam |
|---------------|--------|
| (žádné) | Hodnotové předání (kopie) - hodnotové typy kopírují hodnotu, referenční kopírují odkaz |
| `ref` | Předání referencí - změna parametru se promítne do volajícího |
| `out` | Předání referencí, ale parametr nesmí být použit před přiřazením |
| `in` | Předání referencí jen pro čtení (read-only ref) |

```csharp
void Zvys(int x) { x++; }
void ZvysRef(ref int x) { x++; }

int a = 5;
Zvys(a);     // a zůstává 5
ZvysRef(ref a);  // a je teď 6
```

**Garbage Collector (GC):**

Co se stane s objektem, na který neukazuje žádná reference?

```csharp
void Metoda()
{
    Student s = new Student("Pepa", 20);
    // ... použití ...
}
// Po skončení metody:
// - proměnná "s" zmizí ze stacku
// - na objekt na heapu NIKDO neukazuje
// - Garbage Collector ho AUTOMATICKY smaže
```

**GC v .NET:**
- Generační GC: objekty se třídí do **generací 0, 1, 2** podle stáří. Mladé objekty (Gen 0) se sbírají často, staré (Gen 2) zřídka. Důvod: většina objektů umírá mladá ("generational hypothesis").
- **Mark and sweep** - algoritmus prochází živé objekty (od **root references**), označuje je, neoznačené uvolní.
- **Compaction** - po sběru se zbylé objekty přesunou k sobě, aby se zabránilo fragmentaci.
- Programátor v C# **nemusí** uvolňovat paměť ručně (na rozdíl od C++, kde se používá `delete`).
- Pro **neřízené prostředky** (soubory, sockety) existuje `IDisposable` a `using` (deterministické uvolnění).

**Hodnotové typy mohou skončit na heapu** - tomu se říká **boxing**:
```csharp
int x = 5;
object o = x;   // boxing - int se zabalí do objektu na heapu
int y = (int)o; // unboxing
```
Boxing je drahý, proto existují generické kolekce (`List<int>` místo `ArrayList`).

---

## BOD 6: Konstruktor

**Teorie:**

**Konstruktor** je speciální metoda volaná automaticky při vytvoření objektu (`new`). Jeho úlohou je **inicializovat objekt** - nastavit počáteční hodnoty fields a properties, alokovat zdroje, validovat parametry.

**Pravidla:**
- Jmenuje se **stejně jako třída**.
- **Nemá návratový typ** (ani void).
- Volá se automaticky při `new`.
- Může jich být **víc** (přetížení), liší se počtem nebo typem parametrů.

```csharp
public class Student
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }

    // 1. BEZPARAMETRICKÝ (default)
    public Student()
    {
        Jmeno = "Neznámý";
        Vek = 0;
    }

    // 2. PARAMETRICKÝ
    public Student(string jmeno, int vek)
    {
        Jmeno = jmeno;
        Vek = vek;
    }

    // 3. ŘETĚZENÍ konstruktorů
    public Student(string jmeno) : this(jmeno, 18)
    {
        // Tělo se vykoná PO this(jmeno, 18)
    }
}

// Použití:
Student s1 = new Student();                  // Bezparametrický
Student s2 = new Student("Pepa", 20);        // Parametrický
Student s3 = new Student("Jana");            // Řetězený -> věk 18
```

**Důležité pravidlo:** jakmile napíšeš **jakýkoli** konstruktor, bezparametrický se **nevytvoří automaticky**. Pokud ho potřebuješ, musíš ho explicitně napsat.

**Pořadí inicializace objektu:**
1. Default hodnoty fields (0, null, false).
2. Inicializátory polí (`int x = 5;`).
3. Konstruktor rodiče (`: base(...)`).
4. Tělo aktuálního konstruktoru.

**Statický konstruktor:**
```csharp
public class Konfigurace
{
    public static string AppName;

    static Konfigurace()    // bez parametrů, bez modifikátoru přístupu
    {
        AppName = "MyApp";
    }
}
```
Volá se **jednou** při prvním použití třídy, slouží pro inicializaci statických polí.

**Primary constructor (C# 12+):**
```csharp
public class Student(string jmeno, int vek)
{
    public string Jmeno { get; } = jmeno;
    public int Vek { get; set; } = vek;
}
```
Zkrácený zápis - parametry konstruktoru jsou přímo v deklaraci třídy.

**Record types (C# 9+):** zkratka pro immutable objekty:
```csharp
public record Bod(int X, int Y);
// automaticky: konstruktor, properties, Equals, GetHashCode, ToString
```

---

## BOD 7: Zapouzdření (Encapsulation)

**Teorie:**

**Zapouzdření** znamená **skrytí interních dat** a **kontrolovaný přístup** přes veřejné metody/vlastnosti. Jádro myšlenky: data jsou cenná, nesmí být přímo modifikovatelná zvenku, jinak hrozí, že se dostanou do nekonzistentního stavu.

**Dva aspekty zapouzdření:**
1. **Bundling** - data a metody spolu v jednom objektu.
2. **Information hiding** - vnitřní detaily jsou skryté, navenek je jen rozhraní.

**Důvody pro zapouzdření:**
- **Invariant** - třída si garantuje, že její stav je vždy platný (např. záporný zůstatek na účtě nedává smysl).
- **Změna implementace** - mohu změnit vnitřní strukturu, dokud zachovám veřejné rozhraní.
- **Bezpečnost** - cizí kód nemůže omylem rozbít stav.
- **Validace** - setter může odmítnout nevalidní hodnoty.

```csharp
public class BankovniUcet
{
    // PRIVATE setter = nelze měnit zvenku
    public decimal Zustatek { get; private set; }

    public BankovniUcet(decimal vklad)
    {
        if (vklad < 0)
            throw new ArgumentException("Počáteční vklad nesmí být záporný");
        Zustatek = vklad;
    }

    // Kontrolovaný přístup
    public void Vloz(decimal castka)
    {
        if (castka <= 0)
            throw new ArgumentException("Vklad musí být kladný");
        Zustatek += castka;
    }

    public bool Vyber(decimal castka)
    {
        if (castka > 0 && castka <= Zustatek)
        {
            Zustatek -= castka;
            return true;
        }
        return false;
    }
}

// Použití:
BankovniUcet ucet = new BankovniUcet(1000);
Console.WriteLine(ucet.Zustatek);  // Čtení OK
// ucet.Zustatek = 999999;         // Nejde, private set
ucet.Vloz(500);                    // Kontrolovaný vklad
```

**Anti-pattern - "Java Bean" s veřejnými gettery/settery na všechno:**
```csharp
public class Student
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }   // umožní Vek = -50, anti-pattern
}
```
Toto je **slabé zapouzdření** - třída neudržuje žádný invariant, je to v podstatě jen kontejner pro data. Pro takové případy lépe použít `record` nebo `struct`.

---

## BOD 8: Vlastnost (Property)

**Teorie:**

**Vlastnost (property)** je mechanismus pro kontrolovaný přístup k datům pomocí **getteru** a **setteru**. Navenek vypadá jako pole, ale uvnitř se jedná o pár metod.

- `get` - vrací hodnotu (čte).
- `set` - nastavuje hodnotu (zapisuje), používá implicitní parametr `value`.

**Vlastnosti vs pole:**
- **Pole** - syrová proměnná, žádná kontrola, žádná validace.
- **Vlastnost** - "obálka" s možností validace, logiky, výpočtu.

**Konvence:** v C# jsou vlastnosti **PascalCase** (`Jmeno`), soukromá pole **camelCase** (`jmeno` nebo `_jmeno`).

```csharp
public class Student
{
    // 1. AUTO-PROPERTY (kompilátor sám vygeneruje skryté pole)
    public string Jmeno { get; set; }

    // 2. S VÝCHOZÍ HODNOTOU
    public int Rocnik { get; set; } = 1;

    // 3. READ-ONLY (jen pro čtení, nastavitelná v konstruktoru)
    public DateTime Vytvoreno { get; } = DateTime.Now;

    // 4. PRIVATE SETTER (čte se zvenku, zapisuje jen uvnitř)
    public int PocetZkousek { get; private set; }

    // 5. S VALIDACÍ (plný zápis)
    private int vek;
    public int Vek
    {
        get { return vek; }
        set
        {
            if (value >= 0 && value <= 150)
                vek = value;
            else
                throw new ArgumentException("Neplatný věk");
        }
    }

    // 6. VYPOČÍTANÁ (expression-bodied)
    public bool JePlnolety => vek >= 18;

    // 7. INIT-ONLY SETTER (C# 9+)
    public string RodneCislo { get; init; }
    // Nastavitelné jen v object initializeru, jinak read-only
}

var s = new Student { RodneCislo = "1234567890" };
// s.RodneCislo = "X";  // Chyba, init-only
```

**Co je `value`?**

`value` je **kontextové klíčové slovo**, které představuje hodnotu přiřazovanou do setteru.

```csharp
student.Vek = 25;
//            ^
//      TOHLE je "value" (25) v setteru

public int Vek
{
    set { vek = value; }  // value = 25
}
```

**Přehled:**

| Typ | Syntaxe | Čtení | Zápis |
|-----|---------|:-----:|:-----:|
| Plný přístup | `{ get; set; }` | Ano | Ano |
| Read-only | `{ get; }` | Ano | Jen v konstruktoru |
| Private set | `{ get; private set; }` | Ano | Jen uvnitř třídy |
| Init-only | `{ get; init; }` | Ano | Jen v object initializeru |
| S validací | Plná verze s `if` | Ano | Ano + kontrola |
| Vypočítaná | `=> vyraz` | Ano | Ne (nelze přiřadit) |

**Klíčový rozdíl proti polím:**
- Vlastnost lze předefinovat v potomkovi (`virtual` + `override`).
- Vlastnost lze přidat do interface.
- Vlastnost má vlastní řádek v stack trace - lépe se debugguje.
- Vlastnost lze rozšířit o logiku bez změny rozhraní (zpětně kompatibilní).

---

## BOD 9: Funkce vs Metoda

**Teorie:**

V C# je **vše metoda** - i to co vrací hodnotu, i to co nevrací (`void`).

**Terminologie:**
- **Funkce** - obecný matematický pojem, který vrací hodnotu. V procedurálních jazycích (C, Pascal) je to základní stavební jednotka.
- **Metoda** - funkce, která **patří objektu nebo třídě** (v OOP).
- **Procedura** - v Pascalu funkce bez návratové hodnoty.

| Aspekt | Nevrací hodnotu | Vrací hodnotu |
|--------|-----------------|---------------|
| **Návratový typ** | `void` | `int`, `string`, `bool`, ... |
| **Return** | Nepovinný (lze ukončit metodu) | Povinný s hodnotou |
| **Účel** | Provede akci (side effect) | Vypočítá a vrátí hodnotu |
| **V C# se nazývá** | Metoda | Metoda |

```csharp
public class Kalkulacka
{
    // METODA - void, nic nevrací
    public void VypisPozdrav()
    {
        Console.WriteLine("Ahoj!");
    }

    // METODA - vrací hodnotu
    public int Secti(int a, int b)
    {
        return a + b;
    }

    public bool JeKladne(int cislo)
    {
        return cislo > 0;
    }
}

// Použití:
Kalkulacka k = new Kalkulacka();
k.VypisPozdrav();                    // Metoda (void)
int soucet = k.Secti(5, 3);          // Metoda (s návratovou hodnotou)
```

**Pure function vs metoda se side effecty:**
- **Čistá funkce** - vrací stejný výsledek pro stejné vstupy, nemá vedlejší účinky (`Math.Sqrt`).
- **Side effect** - mění stav (zapisuje do pole, do souboru, do konzole).

Čisté funkce jsou snadno testovatelné a paralelizovatelné. Funkcionální paradigma je preferuje.

**Speciální typy metod v C#:**
- **Konstruktor** - speciální metoda pro vytvoření instance.
- **Destruktor / finalizer** - `~Class()`, volán GC.
- **Indexer** - `public int this[int i] { get; set; }`.
- **Operátor** - `public static T operator +(...)`.
- **Konverzní operátor** - `public static implicit operator int(MyClass m)`.
- **Lambda výraz** - anonymní funkce `(x) => x * 2`.
- **Lokální funkce** - funkce uvnitř metody.
- **Rozšiřující metoda (extension method)** - `public static T ExtFn(this OtherClass o)`.
- **Asynchronní metoda** - `async Task<T> ...` s `await`.

**Pro maturitu:** "Metoda je funkce, která patří objektu nebo třídě. V C# je vše metoda - rozdíl je jen v tom, zda má návratový typ `void` nebo konkrétní typ."

---

## BOD 10: Static

**Teorie:**

**Static** označuje člen patřící **třídě**, ne konkrétní instanci. Statické pole existuje jen **jednou**, sdílené všemi instancemi.

| Aspekt | Instanční | Statický |
|--------|-----------|----------|
| Patří | Objektu | Třídě |
| Počet kopií | Každý objekt má svou | Jedna pro všechny |
| Volání | `objekt.Metoda()` | `Trida.Metoda()` |
| Přístup k `this` | Ano | Ne (žádný objekt) |
| Inicializace | V konstruktoru | Při prvním použití třídy (statický konstruktor) |

**Vizualizace:**

```
                    TŘÍDA Student
                +---------------------+
                | static PocetStudentu| = 3    <- JEDEN pro všechny
                +---------------------+
                          |
        +-----------------+-----------------+
        v                 v                 v
   +---------+       +---------+       +---------+
   | "Pepa"  |       | "Jana"  |       | "Karel" |
   +---------+       +---------+       +---------+
   KAŽDÝ MÁ SVOU      KAŽDÝ MÁ SVOU    KAŽDÝ MÁ SVOU
```

```csharp
public class Student
{
    // INSTANČNÍ
    public string Jmeno { get; set; }

    // STATICKÉ pole - počítadlo všech vytvořených studentů
    public static int PocetStudentu = 0;

    public Student(string jmeno)
    {
        Jmeno = jmeno;
        PocetStudentu++;
    }

    // STATICKÁ metoda
    public static int GetPocet()
    {
        return PocetStudentu;
    }
}

// Použití:
Student pepa = new Student("Pepa");

// INSTANČNÍ - na objektu
Console.WriteLine(pepa.Jmeno);

// STATICKÉ - na třídě
Console.WriteLine(Student.PocetStudentu);
Console.WriteLine(Student.GetPocet());
```

**Pravidlo přístupu:**

| Z... | K instančním | K statickým |
|------|:------------:|:-----------:|
| Statické metody | Ne | Ano |
| Instanční metody | Ano | Ano |

**Proč statická metoda nemůže k instančním?**
- Statická metoda **nemá `this`** - neví, kterého objektu by se to týkalo.
- Instančních členů může být mnoho (každý objekt má své).

```csharp
public class Ukazka
{
    public int hodnota = 10;            // instanční
    public static int staticka = 20;    // statická

    public static void StatickaMetoda()
    {
        Console.WriteLine(staticka);    // OK
        // Console.WriteLine(hodnota);  // Chyba - nemám this
    }

    public void InstancniMetoda()
    {
        Console.WriteLine(hodnota);     // OK
        Console.WriteLine(staticka);    // OK
    }
}
```

**Statická třída:**
```csharp
public static class Util
{
    public static int Zdvojnasob(int x) => x * 2;
}

// Nejde vytvořit instanci:
// new Util();   // Chyba
Util.Zdvojnasob(5);  // OK
```
- Všechny členy musí být statické.
- Nelze vytvořit instanci ani dědit od ní.
- Typické použití: pomocné funkce (`Math`, `Console`, `Path`).

**Extension methods** se píší jako statické metody ve statické třídě:
```csharp
public static class StringExt
{
    public static bool IsPalindrome(this string s)
    {
        return s.SequenceEqual(s.Reverse());
    }
}

"abba".IsPalindrome();  // true - volá se jako metoda na stringu
```

**Singleton pattern přes statiku:**
```csharp
public sealed class Logger
{
    private static Logger instance;
    public static Logger Instance => instance ??= new Logger();
    private Logger() { }
    public void Log(string msg) { /*...*/ }
}
```
- Garantuje **jednu instanci** pro celou aplikaci.
- Antipattern v moderním DI - lépe používat **dependency injection** se singleton scopem.

**Nebezpečí statiky:**
- **Globální stav** - skryté závislosti, špatná testovatelnost.
- **Thread safety** - statická pole sdílená všemi vlákny vyžadují synchronizaci.
- **Životnost** - statika žije po celou dobu běhu aplikace, nikdy ji GC neuvolní.

---

## BOD 11: Polymorfismus

**Teorie:**

**Polymorfismus** = "mnoho podob" (řecky "polys" = mnoho, "morphe" = podoba). Stejná metoda, různé chování podle typu objektu.

**Typy polymorfismu:**

1. **Ad-hoc polymorfismus** - **přetížení (overloading)**: stejný název metody, různé parametry.
   ```csharp
   int Secti(int a, int b);
   double Secti(double a, double b);
   string Secti(string a, string b);
   ```

2. **Parametrický polymorfismus** - **generika**: jedna metoda funguje pro různé typy.
   ```csharp
   T Max<T>(T a, T b) where T : IComparable<T>;
   ```

3. **Subtype polymorfismus** - **dědičnost + virtuální metody**: potomek mění chování metody rodiče.
   ```csharp
   class Zvire { public virtual void Mluv() {} }
   class Pes : Zvire { public override void Mluv() => Console.WriteLine("Haf"); }
   ```

4. **Coercion polymorfismus** - **implicit / explicit conversion**: automatický převod typu.
   ```csharp
   double d = 5;   // int -> double (implicit)
   int i = (int)d; // double -> int (explicit)
   ```

V maturitním kontextu se "polymorfismus" obvykle rozumí **subtype polymorfismus** (přepisování virtuálních metod).

```csharp
public class Zvire
{
    public string Jmeno { get; set; }

    public Zvire(string jmeno) { Jmeno = jmeno; }

    // VIRTUAL = lze přepsat
    public virtual void Mluv()
    {
        Console.WriteLine("Zvíře vydává zvuk");
    }
}

public class Pes : Zvire
{
    public Pes(string jmeno) : base(jmeno) { }

    // OVERRIDE = přepisuje
    public override void Mluv()
    {
        Console.WriteLine($"{Jmeno} říká: Haf!");
    }
}

public class Kocka : Zvire
{
    public Kocka(string jmeno) : base(jmeno) { }

    public override void Mluv()
    {
        Console.WriteLine($"{Jmeno} říká: Mňau!");
    }
}

// SÍLA POLYMORFISMU:
Zvire[] zvirata = { new Pes("Rex"), new Kocka("Micka") };

foreach (Zvire z in zvirata)
{
    z.Mluv();  // Každý mluví JINAK!
}
// Rex říká: Haf!
// Micka říká: Mňau!
```

**Klíčová slova:**

| Slovo | Kde | Co dělá |
|-------|-----|---------|
| `virtual` | Rodič | Povoluje přepsání |
| `override` | Potomek | Přepisuje metodu |
| `abstract` | Rodič | Musí být přepsána, rodič nemůže být instancován |
| `sealed` | Potomek | Zakazuje další přepisování v dalších potomcích |
| `new` (modifikátor) | Potomek | Skryje metodu rodiče bez polymorfismu (nedoporučováno) |
| `base` | Potomek | Volání metody rodiče (`base.Mluv()`) |

**Late Binding (pozdní vazba):**

Proč polymorfismus funguje?

```csharp
Zvire z = new Pes("Rex");  // Typ proměnné: Zvire, skutečný typ: Pes
z.Mluv();                  // Zavolá Pes.Mluv() -> "Haf!"
```

- **Pozdní vazba (Late Binding / Dynamic Dispatch)** - program až **za běhu (runtime)** zjišťuje skutečný typ objektu.
- Kompilátor neví, jaký typ bude v proměnné `z` - může tam být Pes, Kočka, cokoliv.
- Až při spuštění se vybere správná metoda podle skutečného typu.

**Implementace late binding:**
- Každá třída s virtuálními metodami má v paměti **VMT (Virtual Method Table)** - tabulku ukazatelů na metody.
- Při volání `z.Mluv()` se najde VMT skutečného objektu a v ní ukazatel na `Pes.Mluv`.
- Drobné zpomalení (jeden indirect call), ale extrémně výkonné v praxi (caches, branch prediction).

**Pravidlo přiřazování (rodič vs potomek):**

```csharp
class Kniha { }
class Detektivka : Kniha { }

// FUNGUJE - potomek do rodiče
Kniha k = new Detektivka();   // Detektivka MÁ vše co Kniha (LSP)

// NEJDE - rodič do potomka
Detektivka d = new Kniha();   // Kniha NEMÁ vše co Detektivka
```

**Liskov Substitution Principle (LSP):** kdekoli se očekává rodič, mohu dosadit potomka, aniž by to porušilo program. Toto je formální základ subtype polymorfismu.

**Pravidlo:**
- **Levá strana** (typ proměnné) = co **vidím** (jaké metody můžeš volat - statická kontrola kompilátorem).
- **Pravá strana** (`new ...`) = co tam **je** (jaký kód se vykoná - runtime).

```csharp
Kniha k = new Detektivka();
k.Nazev;       // Vidím - Kniha má Nazev
k.Detektiv;    // Nevidím - Kniha nemá Detektiv
k.Popis();     // Zavolá Detektivka.Popis() (polymorfismus)
```

**Pattern matching (C# 7+):**
```csharp
foreach (Zvire z in zvirata)
{
    switch (z)
    {
        case Pes p when p.Jmeno.StartsWith("R"):
            Console.WriteLine("Pes začínající na R");
            break;
        case Kocka k:
            Console.WriteLine("Kočka");
            break;
        default:
            Console.WriteLine("Neznámé zvíře");
            break;
    }
}
```

---

## BONUS: Konstruktory v dědičnosti

**Volání rodičovského konstruktoru:**

```csharp
public class Zvire
{
    public string Jmeno { get; set; }

    public Zvire(string jmeno)
    {
        Jmeno = jmeno;
    }
}

public class Pes : Zvire
{
    public string Plemeno { get; set; }

    // MUSÍM zavolat rodičovský konstruktor
    public Pes(string jmeno, string plemeno) : base(jmeno)
    {
        Plemeno = plemeno;
    }
}
```

**Pořadí volání:**
1. Pole rodiče se inicializují default hodnotami.
2. Inicializátory polí rodiče.
3. Konstruktor rodiče (`base`).
4. Pole potomka se inicializují default hodnotami.
5. Inicializátory polí potomka.
6. Konstruktor potomka.

**Abstract třída:**

```csharp
public abstract class Tvar
{
    public string Barva { get; set; }

    // Abstract třída MŮŽE mít konstruktor
    public Tvar(string barva) { Barva = barva; }

    // ABSTRACT metoda - potomek MUSÍ implementovat
    public abstract double VypocitejObsah();

    // Může mít i běžné metody
    public void VypisInfo() => Console.WriteLine($"Tvar {Barva}, obsah {VypocitejObsah()}");
}

public class Kruh : Tvar
{
    public double Polomer { get; set; }

    public Kruh(string barva, double polomer) : base(barva)
    {
        Polomer = polomer;
    }

    public override double VypocitejObsah()
    {
        return Math.PI * Polomer * Polomer;
    }
}

// Tvar t = new Tvar("červená");   // Chyba, abstraktní třída nelze instancovat
Tvar k = new Kruh("modrá", 5);     // OK, polymorfismus
```

**Rozdíl abstract vs interface:**

| Aspekt | Abstract class | Interface |
|--------|----------------|-----------|
| Více dědičnosti | Ne (jen 1 abstract rodič) | Ano (víc interface) |
| Implementace metod | Ano (částečná) | Default implementace (C# 8+) |
| Pole | Ano | Ne |
| Konstruktor | Ano | Ne |
| Modifikátory přístupu | Ano | Vše public |
| Účel | "Je to" (is-a) - hierarchie | "Umí to" (can-do) - kontrakt |

**Co MUSÍM v potomkovi:**

| Situace | Musím? |
|---------|:------:|
| Rodič má parametrický konstruktor (žádný bezparam.) | Definovat konstruktor + `: base(...)` |
| Rodič má bezparametrický konstruktor | Nemusím (volá se automaticky) |
| Rodič má `abstract` metodu | Implementovat s `override` |
| Rodič má `virtual` metodu | Nemusím přepisovat |
| Rodič má normální metodu | Nic (zdědí se) |
| Rodič má `sealed override` | Nelze dál přepsat |

---

## SOLID principy

Pět principů dobrého OOP designu (Robert C. Martin, "Uncle Bob"):

1. **S - Single Responsibility Principle** - třída má mít jen jeden důvod ke změně. Jedna třída = jedna odpovědnost.
2. **O - Open/Closed Principle** - třída má být **otevřená pro rozšíření**, **zavřená pro modifikaci**. Přidávám novou funkcionalitu novou třídou, ne změnou stávajících.
3. **L - Liskov Substitution Principle** - potomek musí být zaměnitelný za rodiče bez porušení správnosti programu.
4. **I - Interface Segregation Principle** - mnoho malých specializovaných interface je lepší než jedno velké. Klient by neměl být nucen implementovat metody, které nepoužívá.
5. **D - Dependency Inversion Principle** - závisíme na abstrakcích (interface), ne na konkrétních implementacích. Toto je základ **dependency injection**.

**Další principy:**
- **DRY** (Don't Repeat Yourself) - žádné duplicity.
- **KISS** (Keep It Simple, Stupid) - jednoduchost před komplexitou.
- **YAGNI** (You Aren't Gonna Need It) - nepřidávej rysy "do budoucna".
- **Demeter's Law** - "mluv jen se sousedy, ne se sousedy sousedů" - omezuje řetězené volání `a.b.c.d.e`.

---

## Návrhové vzory (Design Patterns)

Standardní řešení častých problémů (kniha "Gang of Four", 1994):

**Vytvářecí (Creational):**
- **Singleton** - jedna instance pro celou aplikaci.
- **Factory Method** - metoda vytváří objekty, volajícímu skryto, který typ se vrátí.
- **Builder** - stavění složitého objektu po krocích (`StringBuilder`).
- **Prototype** - klonování existující instance.

**Strukturální (Structural):**
- **Adapter** - převod rozhraní jedné třídy na rozhraní jiné.
- **Decorator** - obalování objektu pro přidání chování (`Stream` v .NET).
- **Facade** - jednoduché rozhraní nad složitým systémem.
- **Composite** - strom objektů, kde uzly i listy mají stejné rozhraní.

**Chovací (Behavioral):**
- **Strategy** - výměnné algoritmy zapouzdřené v třídách.
- **Observer** - publish/subscribe, posluchači se přihlásí k vydavateli (events v C#).
- **Iterator** - sekvenční přístup k prvkům kolekce (`IEnumerator`).
- **Command** - akce zapouzdřená jako objekt (undo/redo).
- **Template Method** - rodič definuje kostru algoritmu, potomek vyplní detaily.

---

## Na co si dát pozor (Maturitní chytáky)

1. **Zapomenutý `new`:**
   ```csharp
   Student s;
   s.Jmeno = "Test";  // NullReferenceException
   ```

2. **Kopírování referencí:**
   ```csharp
   Student s2 = s1;   // Obě ukazují na STEJNÝ objekt
   s2.Vek = 99;       // Změní i s1.Vek
   ```

3. **Statické vs instanční:**
   ```csharp
   Student.Jmeno;          // Nejde, instanční člen
   pepa.PocetStudentu;     // Lze, ale kompilátor varuje
   Student.PocetStudentu;  // Správně - statické přes třídu
   ```

4. **String je immutable:**
   ```csharp
   s.ToUpper();       // Vrátí novou hodnotu, originál nezmění
   s = s.ToUpper();   // Správně
   ```

5. **Konstruktor se nedědí:**
   ```csharp
   public class Potomek : Rodic
   {
       // MUSÍM definovat konstruktor a zavolat base(), pokud Rodic nemá bezparam.
   }
   ```

6. **Přiřazení rodič -> potomek nejde:**
   ```csharp
   Detektivka d = new Kniha();  // Chyba
   Kniha k = new Detektivka();  // OK (potomek do rodiče)
   ```

7. **Polymorfismus - typ proměnné omezuje viditelnost:**
   ```csharp
   Kniha k = new Detektivka();
   k.Detektiv;  // Nevidím, i když tam Detektivka je
   ((Detektivka)k).Detektiv;  // Lze přes cast
   ```

8. **Hodnotový typ ve struktuře přiřazení:** `struct` se **kopíruje**, `class` se **odkazuje**.

9. **`==` vs `.Equals()`:** `==` na referenčních typech porovnává **reference** (zda jde o stejný objekt), `.Equals()` může být přetížen pro hodnotové porovnání. U `string` je `==` přetížen na hodnotové porovnání.

10. **`override` vs `new`:**
    ```csharp
    class A { public virtual void M() => Console.WriteLine("A"); }
    class B : A { public override void M() => Console.WriteLine("B"); }  // polymorfní
    class C : A { public new void M() => Console.WriteLine("C"); }       // SKRYJE, ne přepíše

    A a = new B(); a.M();  // "B"
    A a2 = new C(); a2.M(); // "A" (!) - new neumožní polymorfismus
    ```

11. **Konstruktor v dědičnosti volá `base()` vždy:** pokud explicitně nezavolám, kompilátor vloží `: base()` (bezparam.). Pokud rodič nemá bezparam., kompilátor zahlásí chybu.

12. **Abstract metoda nemá tělo:**
    ```csharp
    public abstract void M();    // OK
    public abstract void M() {}  // Chyba - abstract nesmí mít implementaci
    ```

13. **`sealed` třída se nedá dědit:** `string` je sealed, proto z ní nemohu dědit.

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 1** | Datové typy - hodnotové vs referenční |
| **Ot. 4** | Pole, kolekce - vše jsou objekty |
| **Ot. 7** | Polymorfismus + dynamic dispatch = klíč k flexibilním programům |
| **Ot. 18** | Dědičnost, abstract, virtual, interface (navazuje) |
| **Ot. 19** | Generika - parametrický polymorfismus |
| **Ot. 20** | WPF - události, MVVM pattern, dependency injection |
| **Ot. 25** | Výjimky - try/catch, custom exception classes |

---

## Klíčová věta pro maturitu

> *"OOP je paradigma, které spojuje data a chování do objektů. Stojí na čtyřech pilířích: zapouzdření (skrytí dat za rozhraní), abstrakce (zjednodušení), dědičnost (reuse přes hierarchii) a polymorfismus (jedno rozhraní, různé implementace). Třída je předpis, instance je konkrétní objekt v paměti, vytvořený konstruktorem voláním `new`. Polymorfismus funguje díky pozdní vazbě - program za běhu zvolí správnou implementaci podle skutečného typu objektu."*

---

## KLÍČOVÉ POJMY

1. **OOP** - paradigma spojující data a chování do objektů.
2. **Procedurální programování** - data a funkce oddělené, top-down rozklad.
3. **Třída (class)** - šablona / předpis pro objekty.
4. **Instance (objekt)** - konkrétní výskyt třídy v paměti.
5. **Zapouzdření (encapsulation)** - skrytí dat, přístup přes rozhraní.
6. **Abstrakce** - skrytí složitosti, ukazujeme jen důležité.
7. **Dědičnost** - nová třída přebírá vlastnosti existující.
8. **Polymorfismus** - stejné rozhraní, různé chování.
9. **Konstruktor** - speciální metoda volaná při `new`.
10. **Destruktor / Finalizer** - `~Class()`, volán GC před uvolněním.
11. **Pole (field)** - proměnná uvnitř třídy.
12. **Vlastnost (property)** - obálka nad polem s get/set.
13. **Metoda** - funkce patřící třídě / objektu.
14. **Statický člen** - patří třídě, ne instanci, jedna kopie pro všechny.
15. **Statický konstruktor** - inicializace statických polí, volán jednou.
16. **Singleton** - třída s jedinou instancí.
17. **Reference** - adresa v paměti, ne přímo data.
18. **Hodnotový typ** - data přímo (stack), `struct`, primitivy.
19. **Referenční typ** - odkaz na heap, `class`, `string`, pole.
20. **Stack vs Heap** - lokální proměnné vs objekty.
21. **Garbage Collector (GC)** - automatické uvolňování paměti.
22. **Boxing / Unboxing** - převod hodnotového typu na object a zpět.
23. **Modifikátory přístupu** - public, private, protected, internal.
24. **`this`** - reference na aktuální instanci.
25. **`base`** - reference / volání rodiče.
26. **`virtual` / `override`** - polymorfní metoda.
27. **`abstract`** - povinná implementace v potomkovi, třída nelze instancovat.
28. **`sealed`** - zákaz dalšího dědění / přepisování.
29. **Pozdní vazba (late binding)** - výběr metody za běhu podle typu.
30. **VMT (Virtual Method Table)** - tabulka ukazatelů na virtuální metody.
31. **Interface** - kontrakt bez implementace ("umí to").
32. **Abstract class** - částečná implementace ("je to").
33. **Object initializer** - `new Class { Prop = val }`.
34. **Init-only setter** - vlastnost nastavitelná jen v initializeru.
35. **Record** - immutable třída s automatickým Equals/GetHashCode.
36. **Generika** - parametrický polymorfismus (`List<T>`).
37. **Extension method** - statická metoda jako instanční (`this` parametr).
38. **SOLID** - 5 principů OOP designu (SRP, OCP, LSP, ISP, DIP).
39. **DRY, KISS, YAGNI** - další design principy.
40. **Design patterns** - standardní řešení (Singleton, Factory, Strategy, Observer).
41. **Dependency Injection** - závislosti se vkládají zvenku, ne hard-coded.
42. **Liskov Substitution Principle** - potomek zaměnitelný za rodiče.
43. **Composition over Inheritance** - preferuj skládání objektů před dědičností.
44. **Information hiding** - skrytí implementačních detailů.
45. **Invariant třídy** - vlastnost, kterou si třída garantuje (kladný zůstatek).

---

*Vytvořeno: 2026-05-12 - Maturitní příprava PRG 2025/2026*
