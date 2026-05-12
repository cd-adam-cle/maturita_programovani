# Zápisky: Otázka č. 18 - Dědičnost v OOP

**Datum:** 2026-05-12
**Status:** Hotovo
**Předmět:** Programování - Maturitní příprava

---

## Checklist bodů otázky

- [x] Bod 1: Motivace dědičnosti v OOP
- [x] Bod 2: Abstraktní třída (abstract class)
- [x] Bod 3: Rozhraní (interface)
- [x] Bod 4: Abstraktní metoda/funkce
- [x] Bod 5: Virtuální metoda (virtual)
- [x] Bod 6: Override (přepsání metody)
- [x] Bod 7: Protected (chráněný přístup)
- [x] Bod 8: Dědění konstruktorů
- [x] Bod 9: Polymorfismus (v kontextu dědičnosti)

---

## Úvod a motivace

**Dědičnost** je jeden ze čtyř pilířů OOP. Umožňuje vytvořit novou třídu (**potomka, derived class**), která **přebírá** strukturu a chování existující třídy (**rodiče, base class**) a může je rozšířit nebo upravit.

**Centrální myšlenka:** modelujeme **hierarchii pojmů** ze skutečného světa. Pes je zvíře, manažer je zaměstnanec, kruh je geometrický tvar. Místo psaní stejných polí a metod ve více třídách je extrahujeme do společného předka.

**Tři klíčové cíle dědičnosti:**
1. **Code reuse** - sdílení společného kódu (DRY).
2. **Hierarchická organizace** - logická struktura kódu odrážející doménu.
3. **Polymorfismus** - jednotné zpracování různých typů přes společné rozhraní.

**Typy vztahů mezi třídami:**
- **Is-a** (je to) - dědičnost. Pes JE zvíře, manažer JE zaměstnanec.
- **Has-a** (má to) - kompozice. Auto MÁ motor, dům MÁ okna.
- **Can-do** (umí to) - rozhraní. Auto UMÍ jezdit, kachna UMÍ plavat.

**Historie:**
- Pojem dědičnosti zavedl jazyk **Simula 67** (1967).
- **Smalltalk** rozvinul model s jediným kořenem hierarchie (`Object`).
- **C++** zavedl **multiple inheritance** - třída může mít víc rodičů (vede k problémům jako **diamond problem**).
- **Java a C#** zvolily **single inheritance + multiple interface implementation** jako kompromis.

---

## Klíčové koncepty

---

### Bod 1: Motivace dědičnosti

**Teorie:**
- Dědičnost umožňuje sdílet společný kód mezi třídami (eliminace duplicity).
- DRY princip - "Don't Repeat Yourself".
- Vytváří logickou hierarchii (Pes JE Zvíře - is-a vztah).
- Umožňuje polymorfismus (kolekce různých typů).
- V C# se používá syntaxe `class Potomek : Rodic`.

**Bez dědičnosti - duplicita kódu:**
```csharp
class Pes
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }
    public void Jist() { Console.WriteLine($"{Jmeno} jí."); }
    public void Spat() { Console.WriteLine($"{Jmeno} spí."); }
    public void Stekat() { Console.WriteLine("Haf!"); }
}

class Kocka
{
    public string Jmeno { get; set; }   // duplicita
    public int Vek { get; set; }        // duplicita
    public void Jist() { Console.WriteLine($"{Jmeno} jí."); }   // duplicita
    public void Spat() { Console.WriteLine($"{Jmeno} spí."); }  // duplicita
    public void Mnoukat() { Console.WriteLine("Mňau!"); }
}
```

**S dědičností - sdílený kód:**
```csharp
// Rodičovská třída (base class)
class Zvire
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }

    public void Jist()
    {
        Console.WriteLine($"{Jmeno} jí.");
    }

    public void Spat()
    {
        Console.WriteLine($"{Jmeno} spí.");
    }
}

// Potomek (derived class) - dědí pomocí dvojtečky
class Pes : Zvire
{
    public void Stekat()
    {
        Console.WriteLine("Haf!");
    }
}

class Kocka : Zvire
{
    public void Mnoukat()
    {
        Console.WriteLine("Mňau!");
    }
}

// Použití
Pes rex = new Pes { Jmeno = "Rex", Vek = 5 };
rex.Jist();      // zděděno z Zvire
rex.Stekat();    // vlastní metoda
```

**ASCII hierarchie:**
```
         +---------+
         |  Zvire  |  <- RODIČ (base)
         |---------|
         | Jmeno   |
         | Vek     |
         | Jist()  |
         | Spat()  |
         +----+----+
              |
      +-------+-------+
      |               |
+-----v-----+   +-----v-----+
|    Pes    |   |   Kocka   |  <- POTOMCI (derived)
|-----------|   |-----------|
| Stekat()  |   | Mnoukat() |
+-----------+   +-----------+
```

**Co potomek dědí:**
- Všechna **public** a **protected** pole, vlastnosti, metody, události.
- NEDĚDÍ konstruktory ani **private** členy (ty existují, ale potomek na ně nevidí).

**Single Root Hierarchy:** v C# (jako v Javě, Smalltalku) má **každá třída implicitně rodiče - `object`**. To znamená, že každá třída zdědí `ToString()`, `Equals()`, `GetHashCode()`, `GetType()`. Tomu se říká **single root hierarchy** a usnadňuje to univerzální operace (např. `object[]` může obsahovat cokoliv).

```csharp
class Auto { }   // implicitně : object
Auto a = new Auto();
Console.WriteLine(a.ToString());  // zděděno z object
```

**Výhody dědičnosti:**
- DRY - kód jen jednou.
- Jednodušší změny - oprava v rodiči se promítne do všech potomků.
- Polymorfismus - `List<Zvire>` obsahuje různé typy.
- Logická hierarchie - kód odráží doménu.

**Nevýhody / problémy:**
- **Tight coupling** - potomek je pevně svázán s rodičem. Změna rodiče může rozbít potomky.
- **Fragile base class problem** - drobná změna rodiče má neočekávané důsledky.
- **Hloubka hierarchie** - víc než 3-4 úrovně dědičnosti je už nepřehledné.
- **Multiple inheritance** - C# zakazuje (kvůli diamond problem).

**Composition over inheritance:** moderní doporučení preferovat **kompozici** (objekt obsahuje jiné objekty) před dědičností. Místo `class Manazer : Zamestnanec` může být `class Manazer { Zamestnanec zamestnanec; }`. Dědičnost se hodí, když je vztah opravdu is-a; jinak často lépe kompozice.

---

### Bod 2: Abstraktní třída (abstract class)

**Teorie:**

**Abstraktní třída** je třída, která **nemůže být přímo instancována** (`new` nefunguje). Slouží jako **šablona** pro potomky. Může obsahovat:
- Běžné (konkrétní) metody s implementací.
- **Abstraktní metody** bez implementace (potomek je musí přepsat).
- Pole, vlastnosti, konstruktory.

**Motivace:** některé třídy v hierarchii nemají smysl jako konkrétní entita. Co je "obecné Zvíře" bez druhu? Co je "obecný Tvar" bez konkrétního typu (kruh, čtverec)? Abstraktní třída říká: "Toto je společný předek, ale instance dělejte jen z potomků."

```csharp
// Abstraktní třída - označená "abstract"
abstract class Zvire
{
    public string Jmeno { get; set; }

    // Běžná metoda - má implementaci, dědí se beze změny
    public void Jist()
    {
        Console.WriteLine($"{Jmeno} jí.");
    }

    // Abstraktní metoda - BEZ implementace
    public abstract void VydejZvuk();  // potomek MUSÍ implementovat
}

class Pes : Zvire
{
    // MUSÍŠ přepsat pomocí override
    public override void VydejZvuk()
    {
        Console.WriteLine("Haf!");
    }
}

// Použití:
// Zvire z = new Zvire();        // Chyba - Cannot create instance of abstract class
Zvire pes = new Pes { Jmeno = "Rex" };  // OK, polymorfismus
pes.VydejZvuk();  // "Haf!"
```

**Kdy použít abstraktní třídu:**
- Máš skupinu podobných tříd se společným chováním I společným stavem (pole, properties).
- Nechceš, aby existovala instance "obecné" třídy.
- Chceš vynutit implementaci určitých metod v potomcích.
- Chceš sdílet implementaci některých metod, ale jiné nechat na potomkovi.

**Příklad - geometrické tvary:**
```csharp
abstract class Tvar
{
    public string Barva { get; set; }

    public Tvar(string barva)
    {
        Barva = barva;
    }

    // Abstraktní - každý tvar počítá obsah jinak
    public abstract double VypoctiObsah();
    public abstract double VypoctiObvod();

    // Konkrétní metoda využívající abstraktní
    public void Vypis()
    {
        Console.WriteLine($"Tvar barvy {Barva}, obsah {VypoctiObsah()}, obvod {VypoctiObvod()}");
    }
}

class Kruh : Tvar
{
    public double Polomer { get; set; }

    public Kruh(string barva, double polomer) : base(barva)
    {
        Polomer = polomer;
    }

    public override double VypoctiObsah()
    {
        return Math.PI * Polomer * Polomer;
    }

    public override double VypoctiObvod()
    {
        return 2 * Math.PI * Polomer;
    }
}
```

**Pravidla:**
- Abstraktní třída může mít konstruktor (volá ho potomek přes `base()`).
- Abstraktní třída může mít pole, properties, eventy.
- Pokud má alespoň jednu **abstract metodu**, sama musí být `abstract`.
- Pokud potomek neimplementuje všechny abstract metody, sám se musí stát abstract.

---

### Bod 3: Rozhraní (interface)

**Teorie:**

**Rozhraní (interface)** je **smlouva** o tom, co třída **umí**. Definuje signatury metod, vlastností, eventů, ale **bez implementace** (tradičně).

**Konvence:** v C# se rozhraní jmenují s prefixem **I** (`IComparable`, `IDisposable`, `IEnumerable`).

```csharp
// Definice rozhraní
interface IBezec
{
    void Behat();           // jen signatura
    int Rychlost { get; }   // vlastnost jen pro čtení
}

interface IPlavec
{
    void Plavat();
}

// Třída může implementovat VÍCE rozhraní
class Kachna : IBezec, IPlavec
{
    public int Rychlost => 5;

    public void Behat()
    {
        Console.WriteLine("Kachna chodí.");
    }

    public void Plavat()
    {
        Console.WriteLine("Kachna plave.");
    }
}

// Použití - polymorfismus přes interface
List<IBezec> bezci = new List<IBezec>
{
    new Kachna(),
    new Pes(),
    new Auto()    // i auto může umět běhat (jezdit)
};

foreach (IBezec b in bezci)
{
    b.Behat();  // každý se chová jinak
}
```

**Vlastnosti interface v C#:**
- Třída může implementovat **více rozhraní** (oddělená čárkou).
- Třída může **dědit jednu třídu** + implementovat **mnoho rozhraní**:
  ```csharp
  class Kachna : Ptak, IBezec, IPlavec { }
  ```
- Rozhraní samo může dědit od jiných rozhraní:
  ```csharp
  interface IObojetne : IBezec, IPlavec { }
  ```
- Tradičně bez implementace, ale **C# 8.0+** umožňuje **default interface members** (defaultní implementace).
- Členy v interface jsou implicitně `public abstract` (nelze přidat modifikátor).

**Default interface members (C# 8+):**
```csharp
interface ILogger
{
    void Log(string msg);

    // Defaultní implementace
    void LogError(string msg) => Log("ERROR: " + msg);
}

class ConsoleLogger : ILogger
{
    public void Log(string msg) => Console.WriteLine(msg);
    // LogError zdědí default implementaci
}
```

**Důležitá rozhraní v .NET:**
- `IDisposable` - uvolnění zdrojů, použití s `using`.
- `IEnumerable<T>` / `IEnumerator<T>` - iterace v `foreach`.
- `IComparable<T>` - porovnání objektů (`Sort`).
- `IEquatable<T>` - test rovnosti.
- `INotifyPropertyChanged` - upozornění na změnu vlastnosti (MVVM v WPF).
- `ICollection<T>`, `IList<T>`, `IDictionary<K, V>` - kolekce.
- `ICloneable` - klonování (dnes nedoporučováno).

**Abstract class vs Interface:**

| Aspekt | Abstract class | Interface |
|--------|----------------|-----------|
| Syntaxe | `abstract class Zvire` | `interface IBezec` |
| Implementace metod | Ano (částečná) | Default (C# 8+), tradičně ne |
| Pole | Ano | Ne (jen properties) |
| Konstruktor | Ano | Ne |
| Modifikátory přístupu | Ano | Vše public |
| Vícenásobná dědičnost | Ne (jen 1 abstract rodič) | Ano (víc interface) |
| Statické členy | Ano | Ano (C# 8+) |
| Typ vztahu | IS-A (Pes JE Zvíře) | CAN-DO (Pes UMÍ běhat) |
| Sdílený stav | Ano (pole, properties) | Jen vlastnosti |

**Kdy použít interface vs abstract class:**

| Použít interface | Použít abstract class |
|------------------|----------------------|
| Různé typy mají společné chování (Auto, Pes umí běhat) | Třídy mají i společný stav (pole) |
| Potřebuješ více schopností (Kachna: běhat, plavat, létat) | Hierarchie typů (Zvíře -> Pes, Kočka) |
| Žádná hierarchie/IS-A vztah | Sdílíš implementaci některých metod |
| Plug-in architektura | Šablona algoritmu (Template Method pattern) |

---

### Bod 4: Abstraktní metoda

**Teorie:**

**Abstraktní metoda** je metoda **bez implementace** v abstraktní třídě. Pouze deklaruje signaturu (návratový typ, jméno, parametry). Potomek **musí** implementovat pomocí `override`.

```csharp
abstract class Tvar
{
    // ABSTRAKTNÍ - jen hlavička, BEZ těla
    public abstract double VypoctiObsah();

    // Nemůže mít tělo:
    // public abstract double VypoctiObvod() { return 0; }  // Chyba
}

class Kruh : Tvar
{
    public double Polomer { get; set; }

    // MUSÍŠ přepsat pomocí override
    public override double VypoctiObsah()
    {
        return Math.PI * Polomer * Polomer;
    }
}
```

**Pravidla:**
- Abstraktní metoda **musí** být v abstraktní třídě (ne v běžné).
- Abstraktní metoda **nemůže** být `private` (potomek by ji nemohl implementovat).
- Potomek implementuje pomocí `override`.
- Pokud potomek nepřepíše všechny abstract metody, sám musí být abstract.
- Abstraktní mohou být i **vlastnosti, indexery, eventy**.

**Abstraktní vs virtuální metoda:**

| Abstract | Virtual |
|----------|---------|
| BEZ implementace | S implementací (default) |
| Potomek MUSÍ přepsat | Potomek MŮŽE přepsat |
| Jen v abstract třídě | V kterékoli třídě |
| `public abstract void M();` | `public virtual void M() { }` |

**Rozdíl interface metoda vs abstract metoda:**
```csharp
// Interface - implementace BEZ override
interface IFoo
{
    void Method();
}

class Bar : IFoo
{
    public void Method() { }  // BEZ override
}

// Abstract - implementace S override
abstract class Foo
{
    public abstract void Method();
}

class Baz : Foo
{
    public override void Method() { }  // S override
}
```

---

### Bod 5: Virtuální metoda (virtual)

**Teorie:**

**Virtuální metoda** má **default implementaci**, ale potomek ji **může** (ne musí) přepsat. Na rozdíl od abstraktní (musí přepsat) nabízí výchozí chování.

Klíčové slovo `virtual` se používá v rodiči, `override` v potomkovi.

```csharp
class Zvire
{
    // VIRTUAL - má implementaci, MŮŽE se přepsat
    public virtual void VydejZvuk()
    {
        Console.WriteLine("Obecný zvuk");  // výchozí
    }
}

class Pes : Zvire
{
    // Přepisujeme
    public override void VydejZvuk()
    {
        Console.WriteLine("Haf!");
    }
}

class Kocka : Zvire
{
    // Nepřepisujeme -> použije se výchozí "Obecný zvuk"
}
```

**Volání rodičovské metody přes `base`:**
```csharp
class Zamestnanec
{
    public virtual void Pracuj()
    {
        Console.WriteLine("Pracuji 8 hodin");
    }
}

class Manager : Zamestnanec
{
    public override void Pracuj()
    {
        base.Pracuj();  // zavolá rodičovskou metodu
        Console.WriteLine("+ řídím tým");
    }
}

// Výstup:
// Pracuji 8 hodin
// + řídím tým
```

**Bez override polymorfismus nefunguje:**
```csharp
class Pes : Zvire
{
    public void VydejZvuk() { Console.WriteLine("Haf!"); }  // BEZ override - chyba (warning)!
}

Pes pes = new Pes();
pes.VydejZvuk();  // "Haf!" - volá Pes.VydejZvuk (statická vazba)

Zvire zvire = new Pes();
zvire.VydejZvuk();  // "Obecný zvuk" - volá Zvire.VydejZvuk (NE Pes!)
```

Bez `override` se metoda považuje za **novou metodu**, která pouze náhodou má stejný název jako v rodiči. Kompilátor vydá varování, doporučí buď `override`, nebo `new` (skrytí).

**Skrytí metody pomocí `new`:**
```csharp
class A { public virtual void M() => Console.WriteLine("A"); }
class B : A { public new void M() => Console.WriteLine("B"); }  // SKRYJE, ne přepíše

A a = new B();
a.M();  // "A" - new neumožní polymorfismus
```

V praxi se `new` modifikátor používá vzácně - je to obvykle známka špatného designu.

**Virtuální vlastnosti:**
```csharp
class Auto
{
    public virtual int MaxRychlost => 200;
}

class Ferrari : Auto
{
    public override int MaxRychlost => 350;
}
```

---

### Bod 6: Override

**Teorie:**

`override` je klíčové slovo pro **přepsání** metody z rodiče. Používá se **POUZE** u `abstract` nebo `virtual` metod.

**Kdy použít override:**

| Situace | override? |
|---------|:---------:|
| Abstract metoda v rodiči | Povinné |
| Virtual metoda v rodiči | Volitelné |
| Interface metoda | Ne (jen `public void M()`) |
| Běžná metoda v rodiči | Nelze (nepůjde to) |

```csharp
// Abstract metoda - POVINNÉ override
abstract class A
{
    public abstract void M();
}
class B : A
{
    public override void M() { }  // override MUSÍ
}

// Virtual metoda - VOLITELNÉ override
class C
{
    public virtual void M() { }
}
class D : C
{
    public override void M() { }  // override MŮŽE
}

// Interface - BEZ override
interface IFoo
{
    void M();
}
class E : IFoo
{
    public void M() { }  // BEZ override
}

// Běžná metoda - NELZE override
class F
{
    public void M() { }  // bez virtual
}
class G : F
{
    // public override void M() { }  // Chyba - F.M() není virtual
}
```

**Sealed - zákaz dalšího přepsání:**
```csharp
class Zvire
{
    public virtual void VydejZvuk() { }
}

class Pes : Zvire
{
    // sealed = další potomci NEMOHOU přepsat
    public sealed override void VydejZvuk()
    {
        Console.WriteLine("Haf!");
    }
}

class Ovcak : Pes
{
    // public override void VydejZvuk() { }  // Chyba - sealed
}
```

`sealed` lze použít:
- Na **třídě** - zakáže další dědění (`sealed class String`).
- Na **override metodě** - zakáže další přepsání v potomcích.

Smysl `sealed`:
- Bezpečnost - zabrání nečekanému přepsání.
- Optimalizace - JIT může inlinovat sealed metody.

**Pravidla override:**
- Stejné jméno, parametry a návratový typ jako přepisovaná metoda.
- Stejný (nebo širší) modifikátor přístupu.
- Lze rozšířit přístup z `protected` na `public`, ale nezúžit.

---

### Bod 7: Protected

**Teorie:**

`protected` je modifikátor přístupu - člen je viditelný **uvnitř třídy + ve všech potomcích**, ale **ne zvenku**.

Kompromis mezi `private` (úplně skrytý) a `public` (úplně otevřený). Používá se pro **sdílení implementačních detailů s potomky**, aniž by byly veřejně dostupné.

```csharp
class BankovniUcet
{
    protected decimal zustatek;  // potomci vidí
    private string pin;          // jen BankovniUcet

    public void Vloz(decimal castka)
    {
        zustatek += castka;
    }
}

class SporiciUcet : BankovniUcet
{
    public void PripoctiUrok()
    {
        zustatek *= 1.02m;  // potomek vidí protected
        // pin = "1234";    // nevidí private
    }
}

// Použití:
SporiciUcet ucet = new SporiciUcet();
ucet.Vloz(1000);
// ucet.zustatek = 0;  // Chyba - protected není vidět zvenku
```

**Modifikátory přístupu - srovnání:**

| Modifikátor | Třída | Potomek | Zvenku (stejná assembly) | Zvenku (jiná assembly) |
|-------------|:-----:|:-------:|:------------------------:|:----------------------:|
| `public` | Ano | Ano | Ano | Ano |
| `protected` | Ano | Ano | Ne | Ne (jen v potomku) |
| `private` | Ano | Ne | Ne | Ne |
| `internal` | Ano | Ano | Ano | Ne |
| `protected internal` | Ano | Ano | Ano | Ne (jen v potomku) |
| `private protected` | Ano | Ano (stejná assembly) | Ne | Ne |

**Kdy použít protected:**
- Sdílení implementačních detailů s potomky.
- Protected konstruktory (jen potomek může vytvořit instanci).
- Helper metody pro potomky (např. logování v base třídě).
- Hooks pattern - rodič volá protected virtual metodu, kterou potomek může přepsat.

**Protected konstruktor:**
```csharp
abstract class Tvar
{
    protected Tvar(string barva)  // jen potomek může volat
    {
        Barva = barva;
    }
}

class Kruh : Tvar
{
    public Kruh(string barva, double polomer) : base(barva) { }
}

// Tvar t = new Tvar("červená");  // Chyba - protected
Kruh k = new Kruh("modrá", 5);    // OK
```

**Template Method pattern s protected:**
```csharp
abstract class ReportGenerator
{
    // Veřejná metoda definuje kostru algoritmu
    public void Generate()
    {
        LoadData();
        Transform();      // potomek může přepsat
        Render();
    }

    protected abstract void LoadData();
    protected virtual void Transform() { /* default */ }
    protected abstract void Render();
}
```

---

### Bod 8: Dědění konstruktorů

**Teorie:**
- **Konstruktory se NEDĚDÍ.**
- Potomek musí definovat vlastní konstruktor (nebo použít implicitní bezparam.).
- Potomek volá rodičovský konstruktor pomocí `: base(...)`.
- Rodičovský konstruktor se volá **vždy první**.

**Důvod:** konstruktor inicializuje konkrétní typ. Potomek může mít navíc vlastní pole, která rodič nezná. Proto musí potomek explicitně rozhodnout, jak zavolat konstruktor rodiče (s jakými parametry) a jak inicializovat svá vlastní pole.

```csharp
class Zvire
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }

    public Zvire(string jmeno, int vek)
    {
        Console.WriteLine("1. Konstruktor Zvire");
        Jmeno = jmeno;
        Vek = vek;
    }
}

class Pes : Zvire
{
    public string Plemeno { get; set; }

    // : base(...) MUSÍ být, pokud rodič nemá bezparametrický konstruktor
    public Pes(string jmeno, int vek, string plemeno) : base(jmeno, vek)
    {
        Console.WriteLine("2. Konstruktor Pes");
        Plemeno = plemeno;
    }
}

// Použití:
Pes rex = new Pes("Rex", 5, "Ovčák");
// Výstup:
// 1. Konstruktor Zvire
// 2. Konstruktor Pes
```

**Pořadí volání konstruktorů:**
1. **Default hodnoty polí rodiče** (0, null, false).
2. **Inicializátory polí rodiče** (`int x = 5;`).
3. **Konstruktor rodiče** (`base(...)`).
4. **Default hodnoty polí potomka**.
5. **Inicializátory polí potomka**.
6. **Konstruktor potomka**.

**Různé scénáře:**

```csharp
// SCÉNÁŘ 1: Rodič má parametrický konstruktor
class A
{
    public A(int x) { }
}
class B : A
{
    public B(int x) : base(x) { }  // MUSÍ base
}
// class B2 : A { public B2() { } }  // Chyba - chybí base(x)

// SCÉNÁŘ 2: Rodič má bezparametrický konstruktor
class C
{
    public C() { }
}
class D : C
{
    public D() { }  // NEMUSÍ base - zavolá se automaticky
}

// SCÉNÁŘ 3: Potomek přidává parametry
class E
{
    public E(int x) { }
}
class F : E
{
    public int Y { get; set; }
    public F(int x, int y) : base(x)
    {
        Y = y;  // vlastní inicializace
    }
}

// SCÉNÁŘ 4: Rodič nemá konstruktor = má implicitní bezparametrický
class G { }   // implicitně G() { }
class H : G { public H() { } }   // base() je implicitní
```

**Konstruktor abstraktní třídy:**
- Abstraktní třída může mít konstruktor.
- Volá se z potomka přes `base(...)`.
- Nikdy se nevolá přímo (abstract = nelze instancovat).

---

### Bod 9: Polymorfismus

**Teorie:**

**Polymorfismus** = "mnoho tvarů". Proměnná typu rodič může obsahovat objekt typu potomek; při volání virtuálních/abstraktních metod se použije implementace **podle skutečného typu objektu**.

Polymorfismus funguje díky **pozdní vazbě (late binding / dynamic dispatch)** - výběr konkrétní metody se odkládá až na runtime.

```csharp
abstract class Zvire
{
    public string Jmeno { get; set; }
    public abstract void VydejZvuk();
}

class Pes : Zvire
{
    public override void VydejZvuk() { Console.WriteLine("Haf!"); }
}

class Kocka : Zvire
{
    public override void VydejZvuk() { Console.WriteLine("Mňau!"); }
}

// POLYMORFISMUS V AKCI
List<Zvire> zvirata = new List<Zvire>
{
    new Pes { Jmeno = "Rex" },
    new Kocka { Jmeno = "Micka" }
};

// Jednotný způsob práce s různými typy
foreach (Zvire z in zvirata)
{
    Console.Write($"{z.Jmeno}: ");
    z.VydejZvuk();  // Každý se chová jinak
}

// Výstup:
// Rex: Haf!
// Micka: Mňau!
```

**Klíčové vlastnosti:**

1. **Proměnná typu rodič, objekt typu potomek (upcasting):**
```csharp
Zvire z = new Pes();  // Pes JE Zvíře - OK
// Pes p = new Zvire();  // Zvíře NENÍ Pes - chyba
```

2. **Volá se metoda potomka (dynamic dispatch):**
```csharp
Zvire z = new Pes();
z.VydejZvuk();  // "Haf!" (z Psa, ne ze Zvíře)
```

3. **Vidíš jen rozhraní rodiče:**
```csharp
class Pes : Zvire
{
    public override void VydejZvuk() { }
    public void Stekat() { }  // jen v Pes
}

Zvire z = new Pes();
z.VydejZvuk();  // OK - je ve Zvire
// z.Stekat();  // Chyba - není ve Zvire
```

4. **Downcasting přes `is` a `as`:**
```csharp
Zvire z = new Pes();

// Bezpečné přetypování přes is
if (z is Pes pes)
{
    pes.Stekat();
}

// Nebo přes as (vrátí null pokud neuspěje)
Pes p = z as Pes;
if (p != null) p.Stekat();

// Tvrdý cast (vyhodí výjimku při neúspěchu)
Pes pesForce = (Pes)z;
```

**Pattern matching (C# 7+):**
```csharp
foreach (Zvire z in zvirata)
{
    switch (z)
    {
        case Pes p:
            p.Stekat();
            break;
        case Kocka k:
            k.Mnoukat();
            break;
        default:
            z.VydejZvuk();
            break;
    }
}
```

**Late binding (pozdní vazba):**
- Kompilátor neví, jaký typ bude v `Zvire z` za běhu.
- Při volání `z.VydejZvuk()` se za běhu:
  1. Najde **VMT (Virtual Method Table)** skutečného objektu.
  2. V VMT je ukazatel na konkrétní metodu (např. `Pes.VydejZvuk`).
  3. Volá se tato metoda.
- Drobné zpomalení (1 indirect call), ale extrémně efektivní v praxi.

**Praktický příklad - geometrické tvary:**
```csharp
abstract class Tvar
{
    public string Barva { get; set; }
    public abstract double VypoctiObsah();
}

class Kruh : Tvar
{
    public double Polomer { get; set; }
    public override double VypoctiObsah() => Math.PI * Polomer * Polomer;
}

class Obdelnik : Tvar
{
    public double Sirka { get; set; }
    public double Vyska { get; set; }
    public override double VypoctiObsah() => Sirka * Vyska;
}

// Polymorfismus
List<Tvar> obrazek = new List<Tvar>
{
    new Kruh { Polomer = 5, Barva = "Červená" },
    new Obdelnik { Sirka = 4, Vyska = 3, Barva = "Modrá" }
};

double celkovyObsah = 0;
foreach (Tvar t in obrazek)
{
    celkovyObsah += t.VypoctiObsah();  // polymorfní volání
}
```

**Liskov Substitution Principle (LSP):**

Jeden ze SOLID principů. Říká: **kdekoli se očekává rodič, musí být možné dosadit potomka, aniž by to porušilo správnost programu.**

Příklad porušení LSP:
```csharp
class Obdelnik
{
    public virtual int Sirka { get; set; }
    public virtual int Vyska { get; set; }
}

class Ctverec : Obdelnik  // Ctverec JE Obdelnik
{
    public override int Sirka
    {
        set { base.Sirka = value; base.Vyska = value; }  // udrží invariant
    }
    public override int Vyska
    {
        set { base.Sirka = value; base.Vyska = value; }
    }
}

void Test(Obdelnik o)
{
    o.Sirka = 5;
    o.Vyska = 3;
    Console.WriteLine(o.Sirka * o.Vyska);   // očekáváme 15
}

Test(new Obdelnik());   // 15
Test(new Ctverec());    // 9 (!) - porušilo to invariant
```

Ctverec není dobrý potomek Obdelnik - jeho přepsání setterů porušuje očekávání. LSP nám říká, že Ctverec není **is-a** Obdelnik, ale **má omezenou variantu**. Lepší by bylo Ctverec a Obdelnik dědit od společného předka.

---

## Multiple inheritance a diamond problem

**Multiple inheritance** = třída má více rodičovských tříd. C++ to umožňuje, C# a Java zakazují.

**Diamond problem:**
```
     A
    / \
   B   C    <- B i C dědí z A
    \ /
     D      <- D dědí z B i C
```

Pokud má A metodu `M()`, kterou B přepíše a C přepíše jinak, kterou verzi má D? Tomu se říká **diamond problem** (kosočtverec).

**Řešení v různých jazycích:**
- **C++**: explicitní řešení přes `virtual` dědičnost.
- **Java, C#**: single inheritance + multiple interface (interface nemá pole, takže diamond problem se eliminuje).
- **Python**: MRO (Method Resolution Order) - lineárně se rozhodne.
- **Scala**: traits s lineárním pořadím.

**C# 8+ default interface methods** přinesly mírnou variantu diamond problemu, ale jazyk vyžaduje, aby třída v případě konfliktu explicitně řekla, kterou implementaci chce.

---

## Composition over Inheritance

Moderní doporučení v OOP. Místo "Kachna **je** Pták a **je** Plavec" raději "Kachna **má** schopnost létat a **má** schopnost plavat".

```csharp
// Dědičnost (problematická - vícenásobná)
// class Kachna : Ptak, Plavec { }   // Chyba v C#

// Kompozice + interface
interface IPlavec { void Plavat(); }
interface ILetec { void Letat(); }

class PlavaniSchopnost : IPlavec
{
    public void Plavat() => Console.WriteLine("Plavu");
}

class LetaniSchopnost : ILetec
{
    public void Letat() => Console.WriteLine("Letím");
}

class Kachna : IPlavec, ILetec
{
    private PlavaniSchopnost plavani = new PlavaniSchopnost();
    private LetaniSchopnost letani = new LetaniSchopnost();

    public void Plavat() => plavani.Plavat();
    public void Letat() => letani.Letat();
}
```

**Výhody kompozice:**
- Flexibilita - schopnosti lze měnit za běhu (Strategy pattern).
- Žádný diamond problem.
- Slabší vazba mezi třídami.
- Snadnější testování (mockování závislostí).

**Kdy preferovat dědičnost:**
- Když je vztah opravdu **is-a** a hierarchie je stabilní.
- Když má rodič smysluplnou implementaci, kterou chce potomek rozšířit.
- Když Template Method pattern (rodič definuje kostru, potomci vyplní detaily).

---

## Na co si dát pozor (Maturitní chytáky)

1. **Abstraktní třídu nelze instancovat:**
   ```csharp
   abstract class Tvar { }
   Tvar t = new Tvar();  // Chyba
   Tvar t = new Kruh();  // OK
   ```

2. **Interface metody bez override:**
   ```csharp
   interface IFoo { void M(); }
   class Bar : IFoo
   {
       public void M() { }   // BEZ override
   }
   ```

3. **Konstruktory se nedědí:**
   ```csharp
   class A { public A(int x) { } }
   class B : A { }  // Chyba - chybí konstruktor

   class B2 : A
   {
       public B2(int x) : base(x) { }   // OK
   }
   ```

4. **Zapomenutý override -> polymorfismus nefunguje:**
   ```csharp
   class Pes : Zvire
   {
       public void VydejZvuk() { }  // chybí override - skryje
   }

   Zvire z = new Pes();
   z.VydejZvuk();  // volá Zvire.VydejZvuk, ne Pes!
   ```

5. **Abstract metoda jen v abstract class:**
   ```csharp
   class Trida   // není abstract
   {
       public abstract void M();  // Chyba
   }
   ```

6. **Třída dědí jen z 1 třídy, ale implementuje víc interface:**
   ```csharp
   class A : B, C { }       // Chyba pokud B i C jsou třídy
   class A : B, IC, ID { }  // OK (B třída, IC a ID interface)
   ```

7. **Protected není vidět zvenku:**
   ```csharp
   class A { protected int x; }
   A a = new A();
   a.x = 5;  // Chyba
   ```

8. **Hodnotový typ nelze dědit:**
   ```csharp
   struct S { }
   // class T : S { }  // Chyba - struct je sealed
   ```

9. **Sealed třída se nedá dědit:**
   ```csharp
   sealed class A { }
   // class B : A { }  // Chyba
   ```
   `string` je `sealed` v .NET.

10. **`base` jen v override:**
    ```csharp
    class B : A
    {
        public override void M()
        {
            base.M();  // OK - volá A.M()
        }

        public void Jine()
        {
            base.M();  // OK - lze volat i mimo override
        }
    }
    ```

11. **`base(...)` jen v konstruktoru:**
    ```csharp
    public B(int x) : base(x) { }   // OK
    public void M() { base(5); }    // Chyba
    ```

12. **Upcasting je implicitní, downcasting explicitní:**
    ```csharp
    Zvire z = new Pes();   // implicitní (vždy bezpečné)
    Pes p = (Pes)z;        // explicitní (může selhat)
    ```

13. **Při volání virtuální metody v konstruktoru POZOR:**
    Kompilátor zavolá metodu potomka, i když potomkův konstruktor ještě neproběhl. Pole potomka mohou být v default stavu.
    ```csharp
    class A
    {
        public A() { Init(); }
        public virtual void Init() { }
    }
    class B : A
    {
        private string s = "init";
        public override void Init()
        {
            Console.WriteLine(s);   // VYTISKNE null, ne "init"!
        }
    }
    new B();  // pole s ještě není inicializováno
    ```

---

## Souvislosti s jinými otázkami

- **Otázka 1**: Datové typy - hodnotové vs referenční (třída je referenční).
- **Otázka 17**: OOP základy - třída, instance, zapouzdření, polymorfismus.
- **Otázka 19**: Generika - omezení `where T : SomeClass` souvisí s dědičností.
- **Otázka 20**: Programování řízené událostmi - dědičnost tříd v GUI (Button : Control : ...).

---

## Rychlý přehled - Co říct u tabule

**Dědičnost:**
> *"Dědičnost umožňuje vytvořit novou třídu (potomka), která přebírá strukturu a chování existující třídy (rodiče) a může je rozšířit. V C# se používá syntaxe `class Potomek : Rodic`. Hlavní cíle jsou code reuse, hierarchie pojmů a polymorfismus."*

**Abstraktní třída:**
> *"Abstraktní třída je šablona pro potomky, která sama o sobě nemůže být instancována. Může obsahovat běžné metody s implementací i abstraktní metody bez implementace. Potomek musí implementovat všechny abstraktní metody pomocí override."*

**Rozhraní:**
> *"Rozhraní je smlouva o tom, co třída umí. Obsahuje jen signatury metod a vlastností bez implementace (před C# 8). Třída může implementovat více rozhraní najednou. Interface metody se implementují BEZ override."*

**Virtual metoda:**
> *"Virtuální metoda má výchozí implementaci, ale potomek ji MŮŽE přepsat pomocí override. Na rozdíl od abstraktní metody, kde přepsání je povinné."*

**Protected:**
> *"Protected umožňuje přístup k členům třídy jejím potomkům, ale ne zvenku. Je to kompromis mezi private (úplně skrytý) a public (úplně otevřený)."*

**Polymorfismus:**
> *"Polymorfismus umožňuje pracovat s objekty různých potomků jednotným způsobem přes společné rozhraní rodiče. Proměnná typu rodič může obsahovat objekt typu potomek, a při volání metod se použije implementace z konkrétního potomka díky pozdní vazbě."*

---

## Klíčová věta pro maturitu

> *"Dědičnost je mechanismus OOP, který umožňuje vytvořit novou třídu z existující. Potomek zdědí všechny public a protected členy rodiče a může je rozšířit nebo upravit. Abstraktní třída slouží jako šablona (nelze ji instancovat), interface jako smlouva o schopnostech. Virtuální metody umožňují potomkovi přepsat default chování, abstraktní metody přepsání vynutí. Polymorfismus díky pozdní vazbě dynamicky vybere správnou implementaci podle skutečného typu objektu za běhu."*

---

## KLÍČOVÉ POJMY

1. **Dědičnost (inheritance)** - nová třída přebírá vlastnosti existující.
2. **Rodič (base, super, parent class)** - třída, ze které se dědí.
3. **Potomek (derived, sub, child class)** - třída, která dědí.
4. **is-a vztah** - dědičnost (Pes je Zvíře).
5. **has-a vztah** - kompozice (Auto má motor).
6. **can-do vztah** - rozhraní (Pes umí běhat).
7. **Single inheritance** - třída má jen 1 rodiče (C#, Java).
8. **Multiple inheritance** - třída má víc rodičů (C++).
9. **Diamond problem** - konflikt při multiple inheritance.
10. **Single root hierarchy** - každá třída implicitně dědí z `object`.
11. **Abstract class** - šablona, nelze instancovat.
12. **Abstract method** - bez implementace, potomek musí přepsat.
13. **Virtual method** - s implementací, potomek může přepsat.
14. **Override** - klíčové slovo pro přepsání virtual/abstract metody.
15. **Sealed** - zákaz dalšího dědění / přepisování.
16. **Interface** - kontrakt, "co třída umí".
17. **Default interface methods** - implementace v interface (C# 8+).
18. **Protected** - viditelné v třídě a potomcích.
19. **Internal** - viditelné v rámci assembly.
20. **`base`** - reference / volání rodiče.
21. **`new` modifikátor** - skrytí metody bez polymorfismu.
22. **Upcasting** - potomek -> rodič (implicitní, bezpečné).
23. **Downcasting** - rodič -> potomek (explicitní, riskantní).
24. **`is`, `as`** - bezpečné testování / přetypování.
25. **Polymorfismus** - jedno rozhraní, různé implementace.
26. **Pozdní vazba (late binding)** - výběr metody za běhu.
27. **VMT (Virtual Method Table)** - tabulka virtuálních metod.
28. **Liskov Substitution Principle (LSP)** - potomek je zaměnitelný za rodiče.
29. **Open/Closed Principle (OCP)** - třída otevřená pro rozšíření, uzavřená pro modifikaci.
30. **Composition over Inheritance** - preferuj skládání před dědičností.
31. **Template Method pattern** - rodič definuje kostru, potomek doplňuje detaily.
32. **Strategy pattern** - výměnné algoritmy přes interface.
33. **Fragile base class problem** - problém s změnou rodiče.
34. **Constructor chaining** - `: base(...)` a `: this(...)`.

---

*Vytvořeno: 2026-05-12 - Maturitní příprava PRG 2025/2026*
