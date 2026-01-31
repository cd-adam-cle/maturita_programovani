# 📚 Zápisky: Otázka č. 18 - Dědičnost v OOP

**Datum:** 2025-01-25  
**Status:** ✅ Hotovo (9/9 bodů)  
**Předmět:** Programování - Maturitní příprava

---

## ✅ Checklist bodů otázky

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

## 🧠 Klíčové koncepty & Snippety

### Bod 1: Motivace dědičnosti

**Teorie:**
- Dědičnost umožňuje sdílet společný kód mezi třídami
- Zabraňuje duplicitě (DRY - Don't Repeat Yourself)
- Vytváří logickou hierarchii (Pes JE Zvíře - is-a vztah)
- Umožňuje polymorfismus (kolekce různých typů)

**Kód (Maturitní verze):**
```csharp
// Rodičovská třída (base class)
class Zvire
{
    public string Jmeno { get; set; }
    
    public void Jist()
    {
        Console.WriteLine($"{Jmeno} jí.");
    }
}

// Potomek (derived class) - dědí pomocí :
class Pes : Zvire
{
    public void Stekat()
    {
        Console.WriteLine("Haf!");
    }
}

// Použití
Pes rex = new Pes { Jmeno = "Rex" };
rex.Jist();      // metoda z Zvire
rex.Stekat();    // vlastní metoda

// Polymorfismus - kolekce různých typů
List<Zvire> zvirata = new List<Zvire>
{
    new Pes { Jmeno = "Rex" },
    new Kocka { Jmeno = "Micka" }
};
```

**ASCII hierarchie:**
```
         ┌─────────┐
         │  Zvire  │  ← RODIČ (base)
         │---------|
         │ Jmeno   │
         │ Jist()  │
         └────┬────┘
              │
      ┌───────┴───────┐
      │               │
┌─────▼─────┐   ┌─────▼─────┐
│    Pes    │   │   Kocka   │  ← POTOMCI (derived)
│-----------|   │-----------|
│ Stekat()  │   │ Mnoukat() │
└───────────┘   └───────────┘
```

**Výhody:**
- DRY (kód jen jednou)
- Jednodušší změny (změna na 1 místě)
- Polymorfismus (`List<Zvire>` obsahuje různé typy)
- Logická hierarchie

---

### Bod 2: Abstraktní třída (abstract class)

**Teorie:**
- Abstraktní třída = šablona pro potomky, která SAMA O SOBĚ NEMŮŽE EXISTOVAT
- Nelze vytvořit instanci (`new` nefunguje)
- Může obsahovat běžné i abstraktní metody
- Potomek musí implementovat všechny abstraktní metody

**Kód (Maturitní verze):**
```csharp
// Abstraktní třída - označená "abstract"
abstract class Zvire
{
    public string Jmeno { get; set; }
    
    // Běžná metoda - má implementaci
    public void Jist()
    {
        Console.WriteLine($"{Jmeno} jí.");
    }
    
    // Abstraktní metoda - BEZ implementace
    public abstract void VydejZvuk();  // ← potomek MUSÍ implementovat
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
// Zvire z = new Zvire();  // ❌ CHYBA! Cannot create instance
Zvire pes = new Pes { Jmeno = "Rex" };  // ✅ OK
pes.VydejZvuk();  // "Haf!"
```

**Kdy použít:**
- Máš skupinu podobných tříd se společným chováním
- Nechceš, aby existovala instance "obecné" třídy
- Chceš vynutit implementaci určitých metod v potomcích

**Příklad - geometrické tvary:**
```csharp
abstract class Tvar
{
    public string Barva { get; set; }
    
    // Abstraktní - každý tvar počítá jinak
    public abstract double VypoctiObsah();
    public abstract double VypoctiObvod();
}

class Kruh : Tvar
{
    public double Polomer { get; set; }
    
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

---

### Bod 3: Rozhraní (interface)

**Teorie:**
- Interface = SMLOUVA o tom, co třída umí (ne co je)
- Pouze signatury metod/vlastností, žádná implementace (před C# 8.0)
- Třída může implementovat VÍCE rozhraní najednou
- Jmenná konvence: začíná "I" (IBezec, IKresitelny)

**Kód (Maturitní verze):**
```csharp
// Definice rozhraní
interface IBezec
{
    void Behat();           // jen signatura
    int ZjistiRychlost();   // bez implementace
}

interface IPlavec
{
    void Plavat();
}

// Třída může implementovat více rozhraní
class Kachna : IBezec, IPlavec  // ← oddělené čárkami
{
    // MUSÍŠ implementovat VŠECHNY metody
    public void Behat()
    {
        Console.WriteLine("Kachna chodí.");
    }
    
    public int ZjistiRychlost()
    {
        return 5;
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
    new Auto()
};

foreach (IBezec b in bezci)
{
    b.Behat();  // každý se chová jinak
}
```

**Abstract class vs Interface:**

| Abstract class | Interface |
|---------------|-----------|
| `abstract class Zvire` | `interface IBezec` |
| Může mít implementaci | Jen signatury (tradičně) |
| Dědí se jen z 1 třídy | Může implementovat více |
| IS-A vztah (Pes JE Zvíře) | CAN-DO vztah (Pes UMÍ běhat) |

**Kdy použít interface:**
- Různé třídy mají společné chování (Pes, Auto umí běhat)
- Potřebuješ více "schopností" (Kachna: běhat, plavat, létat)
- Žádná hierarchie/IS-A vztah

---

### Bod 4: Abstraktní metoda

**Teorie:**
- Abstraktní metoda = metoda BEZ implementace v abstraktní třídě
- Potomek MUSÍ implementovat pomocí `override`
- Pouze v abstraktních třídách (ne v běžných)

**Kód (Maturitní verze):**
```csharp
abstract class Tvar
{
    // ABSTRAKTNÍ - BEZ implementace (jen hlavička)
    public abstract double VypoctiObsah();
    
    // ❌ Nemůže mít tělo:
    // public abstract double VypoctiObvod() { return 0; }  // CHYBA!
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

**Rozdíl interface vs abstract metoda:**
```csharp
// Interface - není override
interface IFoo
{
    void Method();
}

class Bar : IFoo
{
    public void Method() { }  // BEZ override
}

// Abstract - JE override
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
- Virtuální metoda = má implementaci, ale potomek ji MŮŽE přepsat
- Na rozdíl od abstraktní (MUSÍ přepsat)
- Používá se pro flexibilní chování s výchozím řešením

**Kód (Maturitní verze):**
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
    // MŮŽEŠ přepsat (ale nemusíš)
    public override void VydejZvuk()
    {
        Console.WriteLine("Haf!");
    }
}

class Kocka : Zvire
{
    // NEPŘEPISUJEŠ → použije se výchozí "Obecný zvuk"
}
```

**Abstract vs Virtual:**

| Abstract | Virtual |
|----------|---------|
| BEZ implementace | S implementací |
| Potomek MUSÍ přepsat | Potomek MŮŽE přepsat |
| `public abstract void M();` | `public virtual void M() { }` |

**Volání rodičovské metody - base:**
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
        base.Pracuj();  // ← zavolá rodičovskou metodu
        Console.WriteLine("+ řídím tým");
    }
}

// Výstup:
// Pracuji 8 hodin
// + řídím tým
```

**⚠️ Bez override nefunguje polymorfismus:**
```csharp
class Pes : Zvire
{
    public void VydejZvuk() { }  // ← chybí override!
}

Pes pes = new Pes();
pes.VydejZvuk();  // OK

Zvire zvire = new Pes();
zvire.VydejZvuk();  // ❌ volá Zvire.VydejZvuk(), ne Pes!
```

---

### Bod 6: Override

**Teorie:**
- `override` = klíčové slovo pro přepsání metody z rodiče
- Používá se POUZE u abstract a virtual metod
- NIKDY u interface metod

**Kód (Maturitní verze):**
```csharp
// KDY POUŽÍT override:

// ✅ Abstract metoda - POVINNÉ
abstract class A
{
    public abstract void M();
}
class B : A
{
    public override void M() { }  // ← override MUSÍ
}

// ✅ Virtual metoda - VOLITELNÉ
class C
{
    public virtual void M() { }
}
class D : C
{
    public override void M() { }  // ← override MŮŽE
}

// ❌ Interface - BEZ override
interface IFoo
{
    void M();
}
class E : IFoo
{
    public void M() { }  // ← BEZ override
}

// ❌ Běžná metoda - NELZE
class F
{
    public void M() { }  // bez virtual
}
class G : F
{
    // public override void M() { }  // ← CHYBA!
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
    // public override void VydejZvuk() { }  // ❌ CHYBA!
}
```

---

### Bod 7: Protected

**Teorie:**
- `protected` = viditelné v třídě + potomcích (ale ne veřejně)
- Kompromis mezi `private` (úplně skrytý) a `public` (veřejný)
- Používá se pro sdílení implementačních detailů

**Kód (Maturitní verze):**
```csharp
class BankovniUcet
{
    protected decimal zustatek;  // ← potomci vidí
    private string pin;          // ← jen BankovniUcet
    
    public void Vloz(decimal castka)
    {
        zustatek += castka;  // ✅ třída vidí
    }
}

class SporiciUcet : BankovniUcet
{
    public void PripoctiUrok()
    {
        zustatek *= 1.02m;  // ✅ potomek vidí protected
        // pin = "1234";    // ❌ nevidí private
    }
}

// Použití:
SporiciUcet ucet = new SporiciUcet();
ucet.Vloz(1000);
// ucet.zustatek = 0;  // ❌ protected není vidět zvenku
```

**Modifikátory přístupu - srovnání:**

| Modifikátor | Třída | Potomek | Zvenku |
|-------------|-------|---------|--------|
| `public` | ✅ | ✅ | ✅ |
| `protected` | ✅ | ✅ | ❌ |
| `private` | ✅ | ❌ | ❌ |
| `internal` | ✅ | ✅ (ve stejném projektu) | ✅ (ve stejném projektu) |

**Kdy použít:**
- Sdílení implementačních detailů s potomky
- Protected konstruktory (pro abstraktní třídy)
- Helper metody pro potomky

---

### Bod 8: Dědění konstruktorů

**Teorie:**
- Konstruktory se NEDĚDÍ!
- Potomek musí definovat vlastní konstruktor
- Potomek volá rodičovský konstruktor pomocí `: base(...)`
- Rodičovský konstruktor se volá vždy PRVNÍ

**Kód (Maturitní verze):**
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
    
    // : base(...) MUSÍ být, pokud rodič nemá bezparametrický
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

// SCÉNÁŘ 2: Rodič má bezparametrický
class C
{
    public C() { }
}
class D : C
{
    public D() { }  // NEMUSÍ base - zavolá se auto
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
```

---

### Bod 9: Polymorfismus

**Teorie:**
- Polymorfismus = "mnoho tvarů" = jeden objekt, více podob
- Proměnná typu rodič může obsahovat objekt typu potomek
- Při volání metod se použije implementace z potomka
- Umožňuje jednotné zpracování různých typů

**Kód (Maturitní verze):**
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
    z.VydejZvuk();  // Každý se chová jinak!
}

// Výstup:
// Rex: Haf!
// Micka: Mňau!
```

**Klíčové vlastnosti:**
```csharp
// 1. Proměnná typu rodič, objekt typu potomek
Zvire z = new Pes();  // ✅ Pes JE Zvíře
// Pes p = new Zvire();  // ❌ Zvíře NENÍ Pes

// 2. Volá se metoda potomka
Zvire z = new Pes();
z.VydejZvuk();  // "Haf!" (z Psa, ne ze Zvíře)

// 3. Vidíš jen rozhraní rodiče
class Pes : Zvire
{
    public override void VydejZvuk() { }
    public void Stekat() { }
}

Zvire z = new Pes();
z.VydejZvuk();  // ✅
// z.Stekat();  // ❌ není ve Zvíře

// Přetypování zpět:
if (z is Pes pes)
{
    pes.Stekat();  // ✅ bezpečné
}
```

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
    celkovyObsah += t.VypoctiObsah();
}
```

---

## 📋 Procvičené maturitní úlohy

**Poznámka:** V maturitním archivu nebyly nalezeny specifické úlohy zaměřené na dědičnost, abstraktní třídy a rozhraní. Pro procvičení doporučuji:

**Vlastní cvičení:**
1. **Geometrické tvary** - abstraktní třída Tvar, potomci Kruh, Obdelnik, Trojuhelnik
2. **Vozidla** - abstraktní třída Vozidlo, potomci Auto, Motorka, Kamion
3. **Zaměstnanci** - abstraktní třída Zamestnanec, potomci Programator, Manager, Asistent
4. **Zvířata** - rozhraní IBezec, IPlavec, ILetec; třídy Pes, Kachna, Ryba
5. **Evidence** - rozhraní IUlozitelny, ITiskovatelny; třídy Student, Kniha, Faktura

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Abstraktní třída nelze vytvořit**
   ```csharp
   abstract class Tvar { }
   Tvar t = new Tvar();  // ❌ CHYBA!
   ```

2. **Interface metody BEZ override**
   ```csharp
   interface IFoo { void M(); }
   class Bar : IFoo
   {
       public void M() { }  // ❌ BEZ override
   }
   ```

3. **Konstruktory se nedědí**
   ```csharp
   class A { public A(int x) { } }
   class B : A { }  // ❌ CHYBA! Chybí konstruktor
   
   // ✅ SPRÁVNĚ:
   class B : A
   {
       public B(int x) : base(x) { }
   }
   ```

4. **Zapomenutý override → polymorfismus nefunguje**
   ```csharp
   class Pes : Zvire
   {
       public void VydejZvuk() { }  // chybí override!
   }
   
   Zvire z = new Pes();
   z.VydejZvuk();  // ❌ volá Zvire.VydejZvuk()!
   ```

5. **Abstract metoda jen v abstract class**
   ```csharp
   class Trida  // není abstract
   {
       public abstract void M();  // ❌ CHYBA!
   }
   ```

6. **Třída dědí jen z 1 třídy, ale může mít více interface**
   ```csharp
   class A : B, C { }  // ❌ CHYBA (pokud B i C jsou třídy)
   class A : B, IC, ID { }  // ✅ OK (B třída, IC a ID interface)
   ```

7. **Protected není vidět zvenku**
   ```csharp
   class A { protected int x; }
   A a = new A();
   a.x = 5;  // ❌ CHYBA!
   ```

---

## 🚀 Senior Tip

**Prefer composition over inheritance** ("Upřednostňuj kompozici před dědičností")

V praxi se často setkáš se situací, kdy je lepší použít kompozici místo dědičnosti:

```csharp
// ❌ Problematická dědičnost
class Kachna : Ptak, Plavec { }  // CHYBA - jen 1 rodič!

// ✅ Kompozice + interface
interface IPlavec { void Plavat(); }
interface ILetec { void Letat(); }

class Kachna : IPlavec, ILetec
{
    private PlavaniSchopnost plavani = new PlavaniSchopnost();
    private LetaniSchopnost letani = new LetaniSchopnost();
    
    public void Plavat() => plavani.Plavat();
    public void Letat() => letani.Letat();
}
```

**Moderní C# features:**
- **C# 8.0+**: Interface může mít default implementaci
- **C# 9.0+**: Records (hodnotové třídy)
- **C# 11.0+**: Required members (povinné vlastnosti)

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 1**: Datové typy - hodnotové vs referenční (třída je referenční)
- **Otázka 17**: OOP základy - třída, instance, zapouzdření, polymorfismus
- **Otázka 20**: Programování řízené událostmi - dědičnost tříd v GUI

---

## 📝 Rychlý přehled - Co říct u tabule

**Abstraktní třída:**
> "Abstraktní třída je šablona pro potomky, která sama o sobě nemůže existovat. Může obsahovat běžné i abstraktní metody. Abstraktní metody nemají implementaci a potomek je MUSÍ přepsat pomocí override."

**Rozhraní:**
> "Rozhraní je smlouva o tom, co třída umí. Obsahuje jen signatury metod a vlastností bez implementace. Třída může implementovat více rozhraní najednou. Interface metody se implementují BEZ klíčového slova override."

**Virtual metoda:**
> "Virtuální metoda má výchozí implementaci, ale potomek ji MŮŽE přepsat pomocí override. Na rozdíl od abstraktní metody, kde přepsání je povinné."

**Protected:**
> "Protected umožňuje přístup k členům třídy jejím potomkům, ale nikoliv zvenku. Je to kompromis mezi private a public."

**Polymorfismus:**
> "Polymorfismus umožňuje pracovat s objekty různých potomků jednotným způsobem přes společné rozhraní rodiče. Proměnná typu rodič může obsahovat objekt typu potomk, a při volání metod se použije implementace z konkrétního potomka."

---

## 🎯 Maturitní simulace - Typické otázky

**U tabule (ústní):**
1. "Jaký je rozdíl mezi abstraktní třídou a rozhraním?"
2. "Kdy použijete virtual a kdy abstract?"
3. "Co znamená polymorfismus? Ukažte příklad."
4. "Proč se konstruktory nedědí?"
5. "Nakreslete hierarchii tříd pro geometrické tvary."

**U počítače (praktická):**
1. Vytvořte abstraktní třídu `Vozidlo` s metodou `Jed()` a potomky `Auto`, `Motorka`
2. Implementujte rozhraní `IKresitelny` pro třídy `Kruh`, `Obdelnik`
3. Napište polymorfní kód, který projde pole různých tvarů a spočítá celkový obsah

---

**Konec zápisků** ✅
