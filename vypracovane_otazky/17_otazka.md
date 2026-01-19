# 📚 Zápisky: Otázka č. 17 - Objektově orientované programování

**Datum:** 2025-01-19  
**Status:** ✅ Hotovo (ROZŠÍŘENO)

---

## ✅ Checklist bodů otázky

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

## 🧠 Klíčové koncepty & Snippety

---

# 📌 BOD 1: Procedurální programování

## Teorie

**Procedurální programování** = styl, kde program tvoří sekvence příkazů organizované do funkcí.

**Klíčové rysy:**
- Kód = posloupnost instrukcí (shora dolů)
- Data a funkce jsou **oddělené**
- Globální stav - data jsou sdílená přes celý program

**Příklady jazyků:** C, Pascal, Basic

## Kód

```csharp
// PROCEDURÁLNÍ PŘÍSTUP - data a funkce oddělené
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

## Problémy

| Problém | Důsledek |
|---------|----------|
| Data a logika oddělené | Těžko se udržuje konzistence |
| Globální stav | Změna na jednom místě rozbije jiné |
| Slabá modularita | Kód se těžko znovupoužívá |

---

# 📌 BOD 2: OOP - Motivace

## Teorie

**OOP** = "Spoj data a funkce do jednoho celku = OBJEKT"

**Čtyři pilíře OOP:**

| Pilíř | Význam |
|-------|--------|
| **Zapouzdření** | Data + metody pohromadě, skrytí implementace |
| **Abstrakce** | Zjednodušení - ukazujeme jen důležité |
| **Dědičnost** | Tvorba nových tříd z existujících |
| **Polymorfismus** | Stejné rozhraní, různé chování |

## Vizualizace

```
   OBJEKT "Student"              OBJEKT "Student"
   ┌──────────────────┐          ┌──────────────────┐
   │ 📦 DATA:         │          │ 📦 DATA:         │
   │   jmeno = "Pepa" │          │   jmeno = "Jana" │
   │   vek = 20       │          │   vek = 22       │
   ├──────────────────┤          ├──────────────────┤
   │ 🔧 METODY:       │          │ 🔧 METODY:       │
   │   SpocitejPrumer │          │   SpocitejPrumer │
   │   Vypis()        │          │   Vypis()        │
   └──────────────────┘          └──────────────────┘
```

## Výhody OOP

| Výhoda | Vysvětlení |
|--------|------------|
| Modularita | Každý objekt je samostatný "modul" |
| Znovupoužitelnost | Třídu použiješ v jiném projektu |
| Údržba | Změna na jednom místě, zbytek funguje |
| Bezpečnost | Data jsou chráněna (zapouzdření) |

## 📊 Tabulka: Procedurální vs OOP

| Vlastnost | Procedurální | Objektové (OOP) |
|-----------|--------------|-----------------|
| Základní jednotka | Funkce / Procedura | Objekt (Data + Metody) |
| Přístup | Shora dolů (Top-down) | Zdola nahoru (Z komponent) |
| Data | Samostatná, často globální | Skrytá uvnitř objektu (Zapouzdřená) |
| Změna kódu | Riziková (ovlivní zbytek) | Bezpečnější (lokální v třídě) |
| Příklady jazyků | C, Pascal, Basic | C#, Java, Python |

---

# 📌 KONKRÉTNÍ PŘÍKLADY: Procedurální vs OOP

## Příklad 1: Bankovní účet

### ❌ Procedurální:

```csharp
// DATA - oddělená, globální, nechráněná
string[] jmena = new string[100];
decimal[] zustatky = new decimal[100];
int pocetUctu = 0;

static void Vloz(int index, decimal castka)
{
    zustatky[index] += castka;  // Žádná validace!
}

// POUŽITÍ - nebezpečné
int ucet = 0;
Vloz(ucet, 500);
zustatky[ucet] = -99999;  // 😱 Může kdokoliv změnit!
```

### ✅ OOP:

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
// ucet.Zustatek = -99999;  // ❌ NEJDE! Private set
```

---

## Příklad 2: Herní postavy (POLYMORFISMUS)

### ❌ Procedurální:

```csharp
string[] typy = { "warrior", "mage" };
int[] sily = { 20, 5 };

static int Utok(int index)
{
    // Přidat nový typ = změnit VŠECHNY funkce!
    if (typy[index] == "warrior") return sily[index];
    else if (typy[index] == "mage") return sily[index] * 2;
    return 0;
}
```

### ✅ OOP:

```csharp
public abstract class Postava
{
    public string Jmeno { get; set; }
    public int Zivoty { get; set; }
    
    public abstract int Utok();  // Každý útočí JINAK
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

// POLYMORFISMUS - jeden cyklus, různé chování!
Postava[] tym = { new Warrior { Jmeno = "Conan" }, 
                  new Mage { Jmeno = "Gandalf" } };

foreach (Postava p in tym)
{
    int dmg = p.Utok();  // Každý útočí JINAK!
}

// ✅ Přidat Archer = NOVÁ TŘÍDA, žádná změna existujícího kódu!
```

---

## Kdy použít OOP vs Procedurální:

| Situace | Doporučení |
|---------|------------|
| Malý skript, jednorázový výpočet | Procedurální OK |
| Více objektů stejného typu | **OOP** |
| Různé typy se společným rozhraním | **OOP + Polymorfismus** |
| Potřeba ochrany dat | **OOP + Zapouzdření** |
| Rozšiřitelný systém | **OOP + Dědičnost** |

---

# 📌 BOD 3: Třída (Class)

## Teorie

**Třída** = šablona/předpis pro vytváření objektů.

```
TŘÍDA                           OBJEKT (instance)
─────                           ─────────────────
Stavební plán domu       →      Konkrétní postavený dům
Formička na cukroví      →      Konkrétní cukroví
class Student            →      Student pepa = new Student();
```

## Kód

```csharp
public class Student
{
    // 1. DATOVÉ POLOŽKY (fields) - privátní
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
    
    // Zkrácená verze (auto-property)
    public int Vek { get; set; }
    
    // 4. METODY
    public void Vypis()
    {
        Console.WriteLine($"Student: {jmeno}, věk: {vek}");
    }
}
```

## Modifikátory přístupu

| Modifikátor | Viditelnost |
|-------------|-------------|
| `public` | Odkudkoliv |
| `private` | Jen v třídě (DEFAULT!) |
| `protected` | Třída + potomci |
| `internal` | V rámci projektu |

---

# 📌 BOD 4: Instance / Objekt

## Teorie

**Instance (objekt)** = konkrétní výskyt třídy v paměti, vytvořený pomocí `new`.

## Kód

```csharp
// VYTVOŘENÍ INSTANCÍ
Student pepa = new Student("Pepa", 20);    // 1. instance
Student jana = new Student("Jana", 22);    // 2. instance

// Každý objekt má VLASTNÍ data
Console.WriteLine(pepa.Jmeno);   // "Pepa"
Console.WriteLine(jana.Jmeno);   // "Jana"

// Změna jednoho NEOVLIVNÍ ostatní
pepa.Vek = 21;
Console.WriteLine(jana.Vek);     // 22 (nezměněno!)
```

## Co dělá `new`

| Část | Co dělá |
|------|---------|
| `Student` | Typ proměnné |
| `pepa` | Název proměnné (reference) |
| `new` | Vytvoří objekt v paměti (HEAP) |
| `Student("Pepa", 20)` | Zavolá konstruktor |

## ⚠️ Bez `new` = chyba

```csharp
Student pepa;              // Jen deklarace (null)
pepa.Jmeno = "Pepa";       // ❌ NullReferenceException!

Student pepa = new Student("Pepa", 20);  // ✅ OK
```

---

# 📌 BOD 5: Referenční proměnná

## Teorie

**Referenční proměnná** = obsahuje odkaz (adresu) na místo v paměti, ne přímo data.

## Dva typy proměnných

| Typ | Obsahuje | Uloženo na | Příklady |
|-----|----------|------------|----------|
| **Hodnotový** | Přímo hodnotu | STACK | `int`, `double`, `bool`, `struct` |
| **Referenční** | Odkaz (adresu) | HEAP (data) | `class`, `string`, `array`, `List` |

## Vizualizace

```
HODNOTOVÝ TYP                    REFERENČNÍ TYP

int a = 42;                      Student s = new Student("Pepa", 20);

   STACK                            STACK              HEAP
┌─────────┐                     ┌─────────┐      ┌──────────────┐
│ a = 42  │ ← hodnota přímo     │ s = 0x1 │ ───► │ Jmeno="Pepa" │
└─────────┘                     └─────────┘      │ Vek=20       │
                                    ↑            └──────────────┘
                                  ODKAZ              DATA
```

## ⚠️ Klíčový rozdíl - kopírování

```csharp
// HODNOTOVÝ - kopíruje HODNOTU
int a = 10;
int b = a;
b = 99;
Console.WriteLine(a);  // 10 (nezměněno!)

// REFERENČNÍ - kopíruje ODKAZ
Student s1 = new Student("Pepa", 20);
Student s2 = s1;       // Obě ukazují na STEJNÝ objekt!
s2.Vek = 99;
Console.WriteLine(s1.Vek);  // 99 (!!!)
```

## 🗑️ Garbage Collector (GC)

**Co se stane s objektem, na který neukazuje žádná reference?**

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

**Garbage Collector:**
- Automaticky uvolňuje paměť objektů bez referencí
- V C# **nemusíme** uvolňovat paměť ručně (na rozdíl od C++)
- Běží na pozadí, když je potřeba

---

# 📌 BOD 6: Konstruktor

## Teorie

**Konstruktor** = speciální metoda volaná automaticky při vytvoření objektu (`new`).

**Pravidla:**
- Jmenuje se **stejně jako třída**
- **Nemá návratový typ** (ani void!)
- Volá se automaticky při `new`

## Kód

```csharp
public class Student
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }
    
    // 1. BEZPARAMETRICKÝ
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
    }
}

// Použití:
Student s1 = new Student();                  // Bezparametrický
Student s2 = new Student("Pepa", 20);        // Parametrický
Student s3 = new Student("Jana");            // Řetězený → věk 18
```

## ⚠️ Důležité

Jakmile napíšeš **jakýkoliv** konstruktor, bezparametrický se **nevytvoří automaticky**!

---

# 📌 BOD 7: Zapouzdření (Encapsulation)

## Teorie

**Zapouzdření** = skrytí interních dat + kontrolovaný přístup přes veřejné metody/vlastnosti.

## Kód

```csharp
public class BankovniUcet
{
    // PRIVATE = nelze měnit zvenku
    public decimal Zustatek { get; private set; }
    
    public BankovniUcet(decimal vklad)
    {
        Zustatek = vklad;
    }
    
    // Kontrolovaný přístup
    public void Vloz(decimal castka)
    {
        if (castka > 0)
            Zustatek += castka;
        else
            Console.WriteLine("Nelze vložit zápornou částku!");
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
Console.WriteLine(ucet.Zustatek);  // ✅ Čtení OK
// ucet.Zustatek = 999999;         // ❌ NEJDE! Private set
ucet.Vloz(500);                    // ✅ Kontrolovaný vklad
```

---

# 📌 BOD 8: Vlastnost (Property)

## Teorie

**Vlastnost** = mechanismus pro kontrolovaný přístup k datům pomocí **getteru** a **setteru**.

- `get` → vrací hodnotu
- `set` → nastavuje hodnotu (pomocí `value`)

## Typy vlastností

```csharp
public class Student
{
    // 1. AUTO-PROPERTY
    public string Jmeno { get; set; }
    
    // 2. S VÝCHOZÍ HODNOTOU
    public int Rocnik { get; set; } = 1;
    
    // 3. READ-ONLY
    public DateTime Vytvoreno { get; } = DateTime.Now;
    
    // 4. PRIVATE SETTER
    public int PocetZkousek { get; private set; }
    
    // 5. S VALIDACÍ
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
    
    // 6. VYPOČÍTANÁ
    public bool JePlnolety => vek >= 18;
}
```

## Co je `value`?

`value` = **kontextové klíčové slovo**, které představuje hodnotu přiřazovanou do setteru.

```csharp
student.Vek = 25;
//            ↑
//      TOHLE je "value" (25) v setteru

public int Vek
{
    set { vek = value; }  // value = 25
}
```

## Přehled

| Typ | Syntaxe | Čtení | Zápis |
|-----|---------|:-----:|:-----:|
| Plný přístup | `{ get; set; }` | ✅ | ✅ |
| Read-only | `{ get; }` | ✅ | ❌ |
| Private set | `{ get; private set; }` | ✅ | Jen uvnitř |
| S validací | Plná verze s `if` | ✅ | ✅ + kontrola |

---

# 📌 BOD 9: Funkce vs Metoda

## Teorie

**V C# je VŠECHNO METODA** - i to co vrací hodnotu, i to co nevrací (`void`).

| Aspekt | Nevrací hodnotu | Vrací hodnotu |
|--------|-----------------|---------------|
| **Návratový typ** | `void` | `int`, `string`, `bool`, ... |
| **Return** | Nepovinný | **Povinný** s hodnotou |
| **Účel** | Provede akci | Vypočítá a vrátí hodnotu |
| **V C# se nazývá** | Metoda | Metoda |

**Terminologie:**
- **Funkce** = pojem z procedurálních jazyků (C, Pascal) nebo pro delegáty (`Func<T>`)
- **Metoda** = funkce, která **patří objektu nebo třídě** (v OOP)

## Kód

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

> **🎯 Pro maturitu:** "Metoda je funkce, která patří objektu nebo třídě. V C# je všechno metoda - rozdíl je jen v tom, zda má návratový typ `void` nebo konkrétní typ."

---

# 📌 BOD 10: Static

## Teorie

**Static** = člen patřící **třídě**, ne konkrétní instanci.

| Aspekt | INSTANČNÍ | STATICKÝ |
|--------|-----------|----------|
| Patří | Objektu | Třídě |
| Kopií | Každý má svou | Jedna pro všechny |
| Volání | `objekt.Metoda()` | `Třída.Metoda()` |

## Vizualizace

```
                    TŘÍDA Student
                ┌─────────────────────┐
                │ static PocetStudentu│ = 3    ← JEDEN pro všechny
                └─────────────────────┘
                          │
        ┌─────────────────┼─────────────────┐
        ▼                 ▼                 ▼
   ┌─────────┐       ┌─────────┐       ┌─────────┐
   │ "Pepa"  │       │ "Jana"  │       │ "Karel" │
   └─────────┘       └─────────┘       └─────────┘
   KAŽDÝ MÁ SVOU      KAŽDÝ MÁ SVOU    KAŽDÝ MÁ SVOU
```

## Kód

```csharp
public class Student
{
    // INSTANČNÍ
    public string Jmeno { get; set; }
    
    // STATICKÉ
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

## Pravidlo přístupu

| Z... | K instančním | K statickým |
|------|:------------:|:-----------:|
| Statické metody | ❌ | ✅ |
| Instanční metody | ✅ | ✅ |

**Proč statická metoda nemůže k instančním?**
- Statická metoda **nemá `this`** - neví, kterého objektu by se to týkalo
- Instančních členů může být mnoho (každý objekt má své)

```csharp
public class Ukazka
{
    public int hodnota = 10;            // instanční
    public static int staticka = 20;    // statická
    
    public static void StatickaMetoda()
    {
        Console.WriteLine(staticka);    // ✅ OK
        // Console.WriteLine(hodnota);  // ❌ CHYBA! Nemám this
        // Console.WriteLine(this);     // ❌ CHYBA! this neexistuje
    }
    
    public void InstancniMetoda()
    {
        Console.WriteLine(hodnota);     // ✅ OK
        Console.WriteLine(staticka);    // ✅ OK
    }
}
```

---

# 📌 BOD 11: Polymorfismus

## Teorie

**Polymorfismus** = "mnoho podob" - stejná metoda, různé chování podle typu objektu.

## Kód

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

## Klíčová slova

| Slovo | Kde | Co dělá |
|-------|-----|---------|
| `virtual` | Rodič | Povoluje přepsání |
| `override` | Potomek | Přepisuje metodu |
| `abstract` | Rodič | **Musí** být přepsána |
| `sealed` | Potomek | Zakazuje další přepisování |

## 🔗 Late Binding (Pozdní vazba)

**Proč polymorfismus funguje?**

```csharp
Zvire z = new Pes("Rex");  // Typ proměnné: Zvire, Skutečný typ: Pes
z.Mluv();                   // Zavolá Pes.Mluv() → "Haf!"
```

- **Pozdní vazba (Late Binding)** = program až **za běhu (runtime)** zjišťuje skutečný typ objektu
- Kompilátor neví, jaký typ bude v proměnné `z` - může tam být Pes, Kočka, cokoliv
- Až při spuštění se vybere správná metoda podle skutečného typu

> **🎯 Pro maturitu:** "Polymorfismus funguje díky pozdní vazbě - program za běhu zjistí skutečný typ objektu a zavolá odpovídající verzi metody."

## 📐 Pravidlo přiřazování (Rodič vs Potomek)

```csharp
class Kniha { }
class Detektivka : Kniha { }

// ✅ FUNGUJE - potomek do rodiče
Kniha k = new Detektivka();   // Detektivka MÁ vše co Kniha

// ❌ NEJDE - rodič do potomka
Detektivka d = new Kniha();   // Kniha NEMÁ vše co Detektivka
```

**Pravidlo:**
- **LEVÁ strana** (typ proměnné) = co **VIDÍŠ** (jaké metody můžeš volat)
- **PRAVÁ strana** (new ...) = co tam **JE** (jaký kód se vykoná)

```csharp
Kniha k = new Detektivka();
k.Nazev;       // ✅ Vidím - Kniha má Nazev
k.Detektiv;    // ❌ Nevidím - Kniha nemá Detektiv
k.Popis();     // Zavolá Detektivka.Popis() (polymorfismus)
```

---

# 📌 BONUS: Konstruktory v dědičnosti

## Volání rodičovského konstruktoru

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

## Abstract třída

```csharp
public abstract class Tvar
{
    public string Barva { get; set; }
    
    // Abstract třída MŮŽE mít konstruktor
    public Tvar(string barva) { Barva = barva; }
    
    // ABSTRACT metoda - potomek MUSÍ implementovat
    public abstract double VypocitejObsah();
}

public class Kruh : Tvar
{
    public double Polomer { get; set; }
    
    public Kruh(string barva, double polomer) : base(barva)
    {
        Polomer = polomer;
    }
    
    // MUSÍM implementovat!
    public override double VypocitejObsah()
    {
        return Math.PI * Polomer * Polomer;
    }
}
```

## Co MUSÍM v potomkovi

| Situace | Musím? |
|---------|:------:|
| Rodič má parametrický konstruktor | ✅ Definovat konstruktor + `: base(...)` |
| Rodič má bezparametrický konstruktor | ❌ Nemusím |
| Rodič má `abstract` metodu | ✅ Implementovat s `override` |
| Rodič má `virtual` metodu | ❌ Nemusím přepisovat |
| Rodič má normální metodu | ❌ Nic (zdědí se) |

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Zapomenutý `new`:**
   ```csharp
   Student s;
   s.Jmeno = "Test";  // ❌ NullReferenceException!
   ```

2. **Kopírování referencí:**
   ```csharp
   Student s2 = s1;   // Obě ukazují na STEJNÝ objekt!
   s2.Vek = 99;       // Změní i s1.Vek!
   ```

3. **Statické vs instanční:**
   ```csharp
   Student.Jmeno;          // ❌ Nejde! Instanční člen
   pepa.PocetStudentu;     // ❌ Nejde! Kompilátor nedovolí
   Student.PocetStudentu;  // ✅ Správně - statické přes třídu
   ```

4. **String je immutable:**
   ```csharp
   s.ToUpper();       // ❌ Nic se nestane!
   s = s.ToUpper();   // ✅ Správně
   ```

5. **Konstruktor se nedědí:**
   ```csharp
   public class Potomek : Rodic
   {
       // MUSÍM definovat konstruktor a zavolat base()
   }
   ```

6. **Přiřazení rodič → potomek NEJDE:**
   ```csharp
   Detektivka d = new Kniha();  // ❌ CHYBA!
   Kniha k = new Detektivka();  // ✅ OK (potomek do rodiče)
   ```

7. **Polymorfismus - typ proměnné omezuje viditelnost:**
   ```csharp
   Kniha k = new Detektivka();
   k.Detektiv;  // ❌ Nevidím! I když tam Detektivka JE
   ```

---

## 🚀 Senior Tip

- V praxi preferuj **auto-properties** (`{ get; set; }`) - kratší a přehlednější
- Používej **private set** místo plné verze, když nepotřebuješ validaci
- Pamatuj na **SOLID principy** - jedna třída = jedna odpovědnost
- Pro složitější validace používej **FluentValidation** knihovnu

---

## 🔗 Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 1** | Datové typy - hodnotové vs referenční |
| **Ot. 18** | Dědičnost, abstract, virtual, interface (navazuje!) |
| **Ot. 20** | WPF - události, MVVM pattern |

---

*Poslední aktualizace: 18. ledna 2025*
