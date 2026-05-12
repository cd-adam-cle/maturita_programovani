# Zápisky: Otázka č. 6 - Práce s textovými soubory

## Checklist bodů otázky

- [x] Práce s textovými soubory v C# (úvod, motivace, perzistence)
- [x] Soubor jako proud bytů, textový vs. binární soubor
- [x] Třídy `File`, `StreamReader`, `StreamWriter`, `FileStream`
- [x] StreamReader – klíčové metody, principy čtení
- [x] StreamWriter – klíčové metody, buffering
- [x] Blok `using`, `IDisposable`, deterministická likvidace
- [x] Exceptions obecně – hierarchie, propagace, try/catch/finally
- [x] Časté výjimky specifické pro práci se soubory
- [x] Kódování (ASCII, ISO-8859, Windows-1250, Unicode, UTF-8, UTF-16, BOM)
- [x] Bílé znaky a jejich kategorie
- [x] Odřádkování (`\n`, `\r\n`, `Environment.NewLine`)
- [x] Cesty – absolutní vs. relativní, pracovní adresář, `Path` třída
- [x] Základní úloha: počet znaků
- [x] Základní úloha: počet slov
- [x] Základní úloha: frekvence slov
- [x] Výkon a streamování (lazy `ReadLines` vs. `ReadAllLines`)

---

## Klíčové koncepty & Snippety

### Úvod – Proč soubory a co to vlastně je

Když program běží, všechna jeho data jsou v **operační paměti (RAM)**. RAM je rychlá, ale **volatilní** – jakmile proces skončí nebo počítač vypneme, data zmizí. Aby informace přežila restart, musí se uložit na **trvalé úložiště** (disk, SSD, síť) – tomu se říká **perzistence**. Operační systém nám pro to nabízí abstrakci zvanou **soubor**.

**Soubor** je z pohledu OS pojmenovaná posloupnost bytů uložená v souborovém systému. Soubor sám o sobě **nemá pojem o "řádcích"** ani o "znacích" – to je pouze interpretace, kterou tomu dává aplikace nebo knihovna. Stejný binární obsah lze číst jako text (pokud kódování dává smysl) nebo jako binární data (obrázek, ZIP, EXE).

**Textový vs. binární soubor:**
- **Textový** – obsahuje sekvenci znaků, které jsou zakódované v nějakém znakovém kódování (ASCII, UTF-8 …). Lze ho otevřít editorem a "číst lidsky".
- **Binární** – obsahuje libovolné byty bez pevné textové interpretace (`.png`, `.exe`, `.zip`, `.docx`).

Rozdělení je pouze konvencí – fyzicky jsou oba jen sekvence bytů. Textové soubory se ve světě **.NET** čtou pomocí `StreamReader`/`StreamWriter` (které navíc řeší **dekódování bytů na `char`** a opačně), binární přes `BinaryReader`/`BinaryWriter` nebo přímo `FileStream`.

**Stream (proud) jako koncept:**
Práce se souborem je organizovaná kolem abstrakce **proudu (stream)**. Stream je obecné rozhraní pro **postupné čtení nebo zápis bytů**, typicky se sekvenčním ukazatelem polohy (position). Třída `Stream` je abstraktní bázová třída v `System.IO` – z ní dědí `FileStream`, `MemoryStream`, `NetworkStream` atd. Streamy poskytují jednotný způsob, jak číst/zapisovat data nezávisle na zdroji.

**Dva přístupy k souborům v C#:**

| Přístup | Třída | Použití |
|---------|-------|---------|
| Jednorázové operace (eager) | `File`, `File.ReadAllText`, `File.WriteAllLines` | Malé soubory, jednoduchý kód |
| Proudové (streamovací) | `StreamReader/Writer`, `FileStream` | Velké soubory, postupné zpracování |

**Pravidlo:** Pokud soubor není moc velký a operace je jednorázová, `File.ReadAllText/Lines` je nejjednodušší. Pokud soubor může mít stovky MB nebo nevíme dopředu, kolik dat dorazí, používáme proudy.

---

### Cesty k souborům

**Absolutní cesta** začíná od kořene systému:
```csharp
string win = @"C:\Users\Adik\Documents\data.txt";
string lin = "/home/adam/data.txt";
```

**Relativní cesta** se vyhodnocuje vzhledem k **aktuálnímu pracovnímu adresáři procesu** (`Environment.CurrentDirectory`). Pozor: při spuštění z Visual Studia je to typicky `bin/Debug/netX.Y/`, nikoli složka, kde leží zdrojový kód!

```csharp
string relativni = "data.txt";              // ./data.txt vůči pracovnímu adresáři
string podadresar = @"vstupy\soubor.txt";   // ./vstupy/soubor.txt
```

**Třída `Path` – bezpečné skládání cest:**
```csharp
string slozka = @"C:\Users\Adik";
string soubor = Path.Combine(slozka, "data.txt");   // C:\Users\Adik\data.txt

Path.GetFileName(soubor);             // "data.txt"
Path.GetFileNameWithoutExtension(soubor); // "data"
Path.GetExtension(soubor);            // ".txt"
Path.GetDirectoryName(soubor);        // "C:\Users\Adik"
Path.GetFullPath("data.txt");         // absolutní cesta z relativní
```

`Path.Combine` je platformně bezpečný – sám zvolí správný oddělovač (`\` na Windows, `/` na Unixu) a nezdvojí ho, pokud už v cestě je. **Ručně skládat cesty stringy je špatně.**

**Verbatim string `@"..."`:**
Před stringem napíše `@`, aby se zpětná lomítka braly jako lomítka, ne jako escape sekvence. Bez `@` by `"C:\nova\test.txt"` obsahovalo znak `\n` (newline)!

---

### `StreamReader` – Klíčové metody a principy

`StreamReader` čte **bytové data ze streamu a převádí je na znaky** podle daného kódování. Default je UTF-8 (s detekcí BOM).

```csharp
using System.IO;

StreamReader sr = new StreamReader("soubor.txt");
// nebo:
StreamReader sr2 = new StreamReader("soubor.txt", Encoding.UTF8);
```

| Metoda | Co dělá | Vrací |
|--------|---------|-------|
| `Read()` | Přečte 1 znak, posune kurzor | `int` (-1 = konec) |
| `Read(buffer, index, count)` | Přečte více znaků do pole | `int` (skutečně přečtených) |
| `ReadLine()` | Přečte řádek (do `\n`/`\r\n`), bez ukončovacího znaku | `string` (null = konec) |
| `ReadToEnd()` | Přečte celý zbytek souboru | `string` |
| `Peek()` | Vrátí další znak, **neposouvá** | `int` (-1 = konec) |
| `EndOfStream` | Jsme na konci streamu? | `bool` |
| `Close()` / `Dispose()` | Uvolní zdroje (zavře soubor) | `void` |

**Proč `Read()` vrací `int`, ne `char`?**
Protože `char` v C# je 16-bitová **nezáporná** hodnota a všechny její hodnoty jsou platné znaky. Potřebujeme nějak signalizovat "konec souboru" – proto se vrací `int` a hodnota `-1` znamená EOF. Před použitím jako `char` přetypujeme: `char c = (char)sr.Read();`.

**Tři typické vzory čtení:**

```csharp
// 1) Celý obsah najednou (malé soubory)
using StreamReader sr = new StreamReader("vstup.txt");
string obsah = sr.ReadToEnd();
```

```csharp
// 2) Po řádcích (idiomatické pro textové zpracování)
using StreamReader sr = new StreamReader("vstup.txt");
string radek;
while ((radek = sr.ReadLine()) != null)
{
    Console.WriteLine(radek);
}
// alternativa:
// while (!sr.EndOfStream) { string r = sr.ReadLine(); ... }
```

```csharp
// 3) Po znacích (nízkoúrovňové, např. lexer)
using StreamReader sr = new StreamReader("vstup.txt");
int znak;
while ((znak = sr.Read()) != -1)
{
    char c = (char)znak;
    Console.Write(c);
}
```

**Pozor – `ReadLine()` odstraňuje** ukončovací `\r`, `\n` nebo `\r\n`, ale **nezná**, jaký to byl typ. Pokud potřebujete přesný oddělovač zachovat (např. při kopírování souboru), čtěte přes `Read(buffer, ...)` nebo `ReadToEnd()`.

---

### `StreamWriter` – Klíčové metody a buffering

```csharp
StreamWriter sw = new StreamWriter("soubor.txt");
```

| Metoda | Co dělá |
|--------|---------|
| `Write(text)` | Zapíše text, neukončí řádek |
| `Write(format, args)` | Zápis s formátováním (jako `Console.Write`) |
| `WriteLine(text)` | Zapíše text + `Environment.NewLine` |
| `Flush()` | Vynutí zápis bufferu na disk |
| `Close()` / `Dispose()` | Flush + zavře soubor |

**Druhý parametr konstruktoru = append/overwrite:**
```csharp
new StreamWriter("soubor.txt", false)  // PŘEPÍŠE (default)
new StreamWriter("soubor.txt", true)   // PŘIPÍŠE na konec
```

**Buffering:**
`StreamWriter` interně udržuje **buffer** v paměti (cca 4 KB) – nezapisuje na disk každý znak hned, ale shromažďuje data a vypisuje až po naplnění bufferu, při `Flush()`, nebo při zavření. Tím se masivně zrychluje I/O (méně syscalů na OS). **Pokud program spadne před `Dispose()`/`Flush()`, ztratíme nezapsaná data.** Proto je `using` blok klíčový.

`Console.Out` je také `TextWriter` (jako `StreamWriter`), ale obvykle není bufferovaný stejně agresivně. Standardní výstup se chová podle terminálu.

---

### Třída `File` – jednorázové operace

`File` poskytuje statické metody nad celým souborem najednou. Vnitřně si otevře `StreamReader`/`StreamWriter`, udělá operaci a hned je zavře.

```csharp
// ČTENÍ
string obsah = File.ReadAllText("soubor.txt");
string[] radky = File.ReadAllLines("soubor.txt");
IEnumerable<string> lenive = File.ReadLines("soubor.txt"); // lazy!

// ZÁPIS
File.WriteAllText("soubor.txt", "obsah");         // přepíše
File.WriteAllLines("soubor.txt", new[] {"a","b"}); // přepíše, oddělí NewLine
File.AppendAllText("soubor.txt", "další");        // připíše

// MANIPULACE
File.Exists("soubor.txt");
File.Delete("soubor.txt");
File.Copy("zdroj.txt", "cil.txt", overwrite: false);
File.Move("stary.txt", "novy.txt");
```

**`ReadAllLines` vs. `ReadLines`:**
- `ReadAllLines` – načte **celý soubor do pole** v paměti. Rychlé na malé soubory, ale 5 GB soubor sežere 5 GB RAM.
- `ReadLines` – vrací `IEnumerable<string>` a čte **lazy** (postupně, řádek po řádku). Lze procházet `foreachem` s konstantní pamětí.

```csharp
// Spočítá řádky v ohromném logu bez načtení do RAM
int pocet = 0;
foreach (var r in File.ReadLines("velky.log"))
    pocet++;
```

---

### Blok `using` a `IDisposable`

Soubor zabírá **systémový handle** (OS resource). Pokud ho nezavřeme, dochází k:
- **Resource leak** – po čase OS odmítne otevřít další soubor.
- **File lock** – jiný proces nemůže k souboru přistoupit.
- **Ztrátě zapsaných dat** – buffer se nevyprázdní na disk.

V .NET je pro správné uvolnění tato disciplína:

```csharp
// ŠPATNĚ - když ReadToEnd vyhodí výjimku, Close() se nezavolá
StreamReader sr = new StreamReader("soubor.txt");
string text = sr.ReadToEnd();
sr.Close();
```

```csharp
// SPRÁVNĚ - using přeloží se na try/finally s Dispose()
using (StreamReader sr = new StreamReader("soubor.txt"))
{
    string text = sr.ReadToEnd();
} // tady se zavolá sr.Dispose() i v případě výjimky
```

```csharp
// C# 8+: using declaration (bez závorek, žije do konce bloku)
using StreamReader sr = new StreamReader("soubor.txt");
string text = sr.ReadToEnd();
// Dispose se zavolá při opuštění obklopujícího bloku
```

`using` funguje s každou třídou, která implementuje rozhraní **`IDisposable`** (`StreamReader`, `StreamWriter`, `FileStream`, `SqlConnection`, `HttpClient` …). `using` je **syntaktický cukr** pro `try { } finally { x.Dispose(); }`.

**Proč to nevyřeší garbage collector?**
Pokud bychom se spolehli na GC, soubor by se zavřel **nedeterministicky** – tedy možná až za sekundu, nebo také za minutu. To je pro souborové handle nepřijatelné. `IDisposable` zajišťuje **deterministické uvolnění** v okamžiku, kdy už zdroj nepotřebujeme.

---

### Výjimky (Exceptions) – obecně

**Výjimka** je objekt reprezentující chybový stav. Při chybě se "vyhodí" (throw) a propaguje se vzhůru zásobníkem volání, dokud ji někdo nezachytí (`catch`), nebo nezpůsobí pád programu.

**Anatomie `try/catch/finally`:**
```csharp
try
{
    // Kód, který může vyhodit výjimku
    string text = File.ReadAllText("soubor.txt");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine("Soubor nenalezen: " + ex.Message);
}
catch (IOException ex)            // obecnější – chytne víc typů
{
    Console.WriteLine("Chyba I/O: " + ex.Message);
}
catch (Exception ex)              // všechno ostatní (nedoporučeno samostatně)
{
    Console.WriteLine("Neznámá chyba: " + ex.Message);
}
finally
{
    // Vždy se spustí (i bez výjimky, i s ní)
    Console.WriteLine("Úklid hotov.");
}
```

**Pravidla pořadí `catch`:**
- Specifické typy musí jít **před** obecnější (`FileNotFoundException` před `IOException` před `Exception`), jinak je kompilátor odmítne.
- `catch` bez typu = chytá všechno (nedoporučeno – ztratíte info).

**Hierarchie výjimek v .NET (zjednodušená):**
```
Object
 └─ Exception
     ├─ SystemException
     │   ├─ ArgumentException
     │   │   └─ ArgumentNullException
     │   ├─ IOException
     │   │   ├─ FileNotFoundException
     │   │   ├─ DirectoryNotFoundException
     │   │   └─ PathTooLongException
     │   ├─ UnauthorizedAccessException
     │   ├─ NullReferenceException
     │   └─ FormatException
     └─ ApplicationException  (pro vlastní výjimky)
```

**Důležité vlastnosti `Exception`:**
| Vlastnost | Co obsahuje |
|-----------|-------------|
| `ex.Message` | Lidsky čitelný popis chyby |
| `ex.StackTrace` | Kde v kódu chyba nastala (řetěz volání) |
| `ex.InnerException` | Vnořená původní výjimka (pokud byla zabalena) |
| `ex.Source` | Název assembly/objektu, který výjimku vyhodil |

**Vyvolání vlastní výjimky:**
```csharp
if (vek < 0)
    throw new ArgumentException("Věk nemůže být záporný.", nameof(vek));
```

**`throw` vs. `throw ex`:**
```csharp
catch (Exception ex)
{
    throw;        // SPRÁVNĚ – zachová původní stack trace
    throw ex;     // ŠPATNĚ – přepíše stack trace, ztratíte info kde to vzniklo
}
```

---

### Časté výjimky při práci se soubory

| Výjimka | Kdy nastane |
|---------|-------------|
| `FileNotFoundException` | Soubor neexistuje (otevírání pro čtení) |
| `DirectoryNotFoundException` | Adresář na cestě neexistuje |
| `UnauthorizedAccessException` | Nedostatečná oprávnění (systémové složky, jen pro čtení) |
| `IOException` | Obecná I/O chyba – soubor zamčený jiným procesem, disk plný, síťový share nedostupný |
| `PathTooLongException` | Cesta překračuje OS limit (Windows historicky 260 znaků) |
| `ArgumentException` | Neplatná cesta (zakázané znaky `* ? < > |`) |
| `NotSupportedException` | Operace neplatná pro daný stream (např. Seek na NetworkStream) |

**Defenzivní vzor:**
```csharp
if (!File.Exists(cesta))
{
    Console.WriteLine("Soubor neexistuje.");
    return;
}
// pak teprve otevřít
```
Pozor: tohle je **TOCTOU race** – mezi `Exists` a otevřením může soubor zmizet. Pro robustní kód se spoléháme na `try/catch`, ne na předběžné dotazy.

---

### Kódování (Encoding)

**Znaková sada (charset)** je mapování čísel na znaky (např. 65 → 'A'). **Kódování (encoding)** je způsob, jak ta čísla uložit do bytů.

#### ASCII
- 7 bitů → 128 znaků (kódy 0–127).
- Pokrývá anglickou abecedu, číslice, základní interpunkci, řídicí znaky.
- 1 byte = 1 znak (8. bit je 0).
- **Neobsahuje** českou diakritiku, ani žádné jiné národní znaky.

```csharp
char c = 'A';
int kod = (int)c;     // 65
char z = (char)66;    // 'B'
```

#### ISO-8859 a Windows-1250 (historie)
- ISO-8859-x – řada 8-bitových rozšíření ASCII pro jednotlivé jazyky.
- Windows-1250 – středoevropské kódování od Microsoftu, obsahuje českou diakritiku.
- Problém: stejný byte znamená v různých kódováních různý znak → "kočičí písmena", "Å™eÅ™icha" místo "řeřicha".

#### Unicode
- Univerzální znaková sada – pokrývá **všechny** současné jazyky, historické písmo, symboly, emoji (~150 000 znaků).
- Každý znak má **code point** ve tvaru `U+XXXX` (např. `U+0159` = 'ř').
- Unicode definuje, **které znaky existují** – jak je uložit do bytů řeší konkrétní kódování (UTF).

#### UTF-8
- **Variabilní délka 1–4 byty na znak.**
- ASCII znaky (0–127) zaberou **1 byte** – UTF-8 je tak **zpětně kompatibilní s ASCII**.
- Diakritika typicky 2 byty, asijská písma 3 byty, vzácné znaky/emoji 4 byty.
- **Standardní kódování webu i moderních souborů.**

#### UTF-16
- 2 nebo 4 byty na znak.
- C# interně používá UTF-16 pro `string` a `char` (každý `char` je 16-bitový code unit).
- Pro znaky mimo BMP (Basic Multilingual Plane, např. emoji) potřebuje **surrogate pair** = dva `char`y na jeden code point.

```csharp
string s = "🙂";       // 1 znak (code point), ale s.Length == 2!
```

#### BOM (Byte Order Mark)
Speciální značka na začátku souboru, která signalizuje kódování:
- UTF-8 BOM: `EF BB BF`
- UTF-16 LE BOM: `FF FE`
- UTF-16 BE BOM: `FE FF`

`StreamReader` BOM **detekuje** automaticky a podle něj zvolí kódování. Některé nástroje (Linuxové) ale BOM nemají rády – C# defaultně píše UTF-8 **bez BOM**, pokud výslovně neřeknete jinak.

#### Encoding v C#
```csharp
using System.Text;

// Načtení s explicitním kódováním
using var sr = new StreamReader("starysoubor.txt",
                                Encoding.GetEncoding("windows-1250"));

// Zápis s konkrétním kódováním
using var sw = new StreamWriter("out.txt", false, Encoding.UTF8);

// Konzole - nutné pro českou diakritiku ve výpisu
Console.OutputEncoding = Encoding.UTF8;
```

**Kdy řešit kódování:**
| Situace | Řešení |
|---------|--------|
| Konzole + diakritika | `Console.OutputEncoding = Encoding.UTF8;` |
| Moderní textové soubory | Neřeš (UTF-8 default) |
| Starý soubor z Windows-CZ | `Encoding.GetEncoding("windows-1250")` |
| Práce s emoji v `string.Length` | Pozor na surrogate pairs |

---

### Bílé znaky (whitespace)

**Bílý znak** = znak, který se obvykle nezobrazuje, ale zabírá místo. Patří mezi ně:

| Znak | Název | ASCII | Význam |
|------|-------|-------|--------|
| `' '` | Space (mezera) | 32 | Klasická mezera |
| `'\t'` | Tab (tabulátor) | 9 | Horizontální tabulátor |
| `'\n'` | Line Feed | 10 | Konec řádku (Unix) |
| `'\r'` | Carriage Return | 13 | Návrat na začátek řádku |
| `'\v'` | Vertical Tab | 11 | Vertikální tabulátor (vzácné) |
| `'\f'` | Form Feed | 12 | Konec stránky (tisk) |
| `' '` | NBSP | 160 | Nezlomitelná mezera |

```csharp
Char.IsWhiteSpace(' ')   // true
Char.IsWhiteSpace('\t')  // true
Char.IsWhiteSpace('A')   // false

string s = "   Ahoj   ";
s.Trim();          // "Ahoj"
s.TrimStart();     // "Ahoj   "
s.TrimEnd();       // "   Ahoj"
```

`Char.IsWhiteSpace` zná i **Unicode whitespace** – tedy víc než jen ASCII (např. NBSP).

---

### Odřádkování

Historicky vznikla různá konvence pro "konec řádku":

| Systém | Sekvence | ASCII kódy |
|--------|----------|------------|
| Windows | `\r\n` | 13, 10 (CRLF) |
| Linux/macOS | `\n` | 10 (LF) |
| Klasický Mac (do OS 9) | `\r` | 13 (CR) |

Důvod: psací stroje měly dva pohyby – návrat válce na začátek (CR) a posun papíru o řádek (LF). Telegrafy a první terminály přidávaly **oba** znaky, Multics si zjednodušil na LF, CP/M a později DOS/Windows zachovaly CRLF.

**Praktické dopady:**
```csharp
// WriteLine() přidá Environment.NewLine (na Windows \r\n, na Linuxu \n)
sw.WriteLine("text");      // text\r\n na Windows

// Explicitní \n
sw.Write("text\n");        // pouze \n bez ohledu na OS

// Univerzálně přenositelné
sw.Write("text" + Environment.NewLine);
```

**`ReadLine()` rozpozná všechny tři varianty** (CRLF, LF, CR) a odstraní oddělovač. Výsledný string je bez ukončovacího znaku.

**Co je `\r` osamoceně?**
Carriage Return = "vrať se na začátek řádku". V konzoli může způsobit, že další výpis přepíše stávající – využívá se pro progress baru:
```csharp
for (int i = 0; i <= 100; i++)
{
    Console.Write($"\rProgres: {i}%");
    Thread.Sleep(20);
}
```

---

### Pomocné metody

**`Char` – statické metody:**
```csharp
Char.IsLetter(c)        // písmeno (Unicode)
Char.IsDigit(c)         // číslice '0'-'9'
Char.IsLetterOrDigit(c) // písmeno nebo číslice
Char.IsWhiteSpace(c)    // bílý znak
Char.IsPunctuation(c)   // interpunkce
Char.IsUpper(c)         // velké
Char.IsLower(c)         // malé
Char.ToLower(c)         // 'A' → 'a'
Char.ToUpper(c)         // 'a' → 'A'
```

**`String` – metody užitečné pro soubory:**
```csharp
str.Trim()                 // odstraní bílé znaky z krajů
str.ToLower() / ToUpper()  // kompletně malá/velká
str.Split(...)             // rozdělí na pole
str.Contains("x")          // obsahuje?
str.StartsWith("x")        // začíná?
str.EndsWith("x")          // končí?
str.IndexOf("x")           // pozice prvního výskytu (-1 = není)
str.Replace("a", "b")      // nahradí
str.Substring(start, len)  // výřez
str.Length                 // počet code units (pozor na surrogate pairs!)
```

**Konverze:**
```csharp
// int ↔ char
char c = (char)65;        // 'A'
int kod = (int)'A';       // 65

// cokoliv → string
string s = 42.ToString();     // "42"
string s2 = $"hodnota: {42}"; // string interpolation

// string → číslo
int x = int.Parse("42");
bool ok = int.TryParse("42", out int y);  // bez výjimky při chybě
```

---

### Základní úloha: Počet znaků

```csharp
string obsah = File.ReadAllText("soubor.txt");

// Všechny znaky včetně bílých
int vsechny = obsah.Length;

// Bez bílých znaků (cyklus)
int bezBilych = 0;
foreach (char c in obsah)
{
    if (!Char.IsWhiteSpace(c))
        bezBilych++;
}

// LINQ varianta
int bezBilychLinq = obsah.Count(c => !Char.IsWhiteSpace(c));

// Jen písmena
int pismen = obsah.Count(c => Char.IsLetter(c));
```

**Pozor:** `string.Length` vrací počet **UTF-16 code units**, ne grafémů. Emoji nebo znaky mimo BMP se počítají jako 2. Pro grafémové počítání:
```csharp
var enumerator = StringInfo.GetTextElementEnumerator(text);
int grafemy = 0;
while (enumerator.MoveNext()) grafemy++;
```

---

### Základní úloha: Počet slov

```csharp
string obsah = File.ReadAllText("soubor.txt");

// Split podle všech bílých znaků, prázdné se zahodí
string[] slova = obsah.Split(
    (char[])null,
    StringSplitOptions.RemoveEmptyEntries
);
int pocetSlov = slova.Length;
```

Vysvětlení parametrů:
- `(char[])null` říká `Splitu`: "rozděl podle **všech bílých znaků**" (mezera, tab, newline …).
- `StringSplitOptions.RemoveEmptyEntries` zahodí prázdné řetězce, které by vznikly při sousedních oddělovačích.

**Co je "slovo"?** Tahle definice je naivní – "Hello, world!" rozdělí na `["Hello,", "world!"]` s interpunkcí. Pro přesnější přístup:
```csharp
// Slovo = sekvence písmen/číslic, oddělená čímkoliv jiným
var slova = Regex.Matches(obsah, @"\w+").Select(m => m.Value).ToList();
```

`\w+` v regex značí jedna nebo více "word characters" (písmena, číslice, `_`). Pro češtinu funguje, protože regex je Unicode-aware.

---

### Základní úloha: Frekvence slov

```csharp
string obsah = File.ReadAllText("soubor.txt");
string[] slova = obsah.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

Dictionary<string, int> frekvence = new Dictionary<string, int>();

foreach (string slovo in slova)
{
    string klic = slovo.ToLower();   // "Ahoj" == "ahoj"

    if (frekvence.ContainsKey(klic))
        frekvence[klic]++;
    else
        frekvence[klic] = 1;
}

// Výpis seřazený podle klíče (A-Z)
foreach (var par in frekvence.OrderBy(kv => kv.Key))
{
    Console.WriteLine($"{par.Key}: {par.Value}");
}

// Top 10 nejčastějších
foreach (var par in frekvence.OrderByDescending(kv => kv.Value).Take(10))
{
    Console.WriteLine($"{par.Key}: {par.Value}");
}
```

**Optimalizace pomocí `TryGetValue` (1 lookup místo 2):**
```csharp
if (frekvence.TryGetValue(klic, out int aktualni))
    frekvence[klic] = aktualni + 1;
else
    frekvence[klic] = 1;
```

**LINQ varianta (deklarativní):**
```csharp
var frekvence = obsah
    .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
    .GroupBy(s => s.ToLower())
    .ToDictionary(g => g.Key, g => g.Count());
```

**Odstranění diakritiky pro robustní porovnání:**
```csharp
using System.Text;
using System.Globalization;

static string OdstranDiakritiku(string text)
{
    string norm = text.Normalize(NormalizationForm.FormD);
    StringBuilder sb = new StringBuilder();
    foreach (char c in norm)
    {
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            sb.Append(c);
    }
    return sb.ToString().Normalize(NormalizationForm.FormC);
}
// "řeřicha" → "rericha"
```

Princip: Unicode `FormD` rozloží znaky na "base + diacritic" (`ř` → `r` + ◌̌). Pak vyfiltrujeme znaky kategorie `NonSpacingMark` (samotnou diakritickou značku) a zbude jen base.

---

### Streamovací zpracování velkých souborů

Pro **velmi velké soubory** (gigabajty) nelze použít `ReadAllText/Lines`, protože by se celý obsah načetl do RAM. Místo toho:

```csharp
// Lazy iterace přes řádky (konstantní paměť)
using StreamReader sr = new StreamReader("velky.log");
string radek;
long pocet = 0;
while ((radek = sr.ReadLine()) != null)
{
    if (radek.Contains("ERROR"))
        pocet++;
}
Console.WriteLine($"Počet chyb: {pocet}");
```

**`File.ReadLines` jako lazy enumerable:**
```csharp
// Stejné jako výše, ale stručněji
long pocet = File.ReadLines("velky.log")
                 .Count(r => r.Contains("ERROR"));
```

**Pravidlo:** `File.ReadAllLines` ⇒ celý soubor do paměti, `File.ReadLines` ⇒ streamuje (lazy `IEnumerable`). Při velkých souborech vždy `ReadLines`.

**Kopírování souboru po blocích (pro úplnost):**
```csharp
using var src = new FileStream("zdroj.bin", FileMode.Open);
using var dst = new FileStream("cil.bin", FileMode.Create);
byte[] buffer = new byte[8192];
int prectenoBytes;
while ((prectenoBytes = src.Read(buffer, 0, buffer.Length)) > 0)
{
    dst.Write(buffer, 0, prectenoBytes);
}
// alternativně:
// src.CopyTo(dst);
```

---

### Quick Reference – Co použít kdy

| Chci... | Použij |
|---------|--------|
| Přečíst celý malý soubor jako jeden řetězec | `File.ReadAllText(cesta)` |
| Přečíst malý soubor po řádcích do pole | `File.ReadAllLines(cesta)` |
| Streamovat velký soubor lazy | `File.ReadLines(cesta)` nebo `StreamReader` + `ReadLine()` |
| Zapsat string do souboru | `File.WriteAllText(cesta, obsah)` |
| Zapsat řádky do souboru | `File.WriteAllLines(cesta, radky)` |
| Připsat na konec souboru | `File.AppendAllText` nebo `new StreamWriter(c, true)` |
| Kontrola existence | `File.Exists(cesta)` |
| Zjistit jestli je znak mezera/tab/\n | `Char.IsWhiteSpace(c)` |
| Rozdělit text na slova | `Split((char[])null, StringSplitOptions.RemoveEmptyEntries)` |
| Počítat výskyty | `Dictionary<string, int>` nebo `GroupBy(...).Count()` |
| Seřadit slovník | `.OrderBy(kv => kv.Key)` / `.OrderByDescending(kv => kv.Value)` |
| Bezpečně skládat cesty | `Path.Combine(...)` |
| Automaticky uzavřít zdroj | `using (var x = ...)` |

---

## Maturitní chytáky

1. **Relativní cesta** se vztahuje k pracovnímu adresáři procesu (typicky `bin/Debug/netX.Y/`), NE ke zdrojovému kódu! Pokud potřebujete spolehlivost, vyřešte cestu přes `Path.Combine(AppContext.BaseDirectory, "data.txt")`.

2. **`Read()` vrací `int`, ne `char`** – kvůli signalizaci konce souboru (-1). Při použití přetypovat: `(char)znak`.

3. **Vždycky zavřít soubor** – nejlépe `using` blokem. Bez `Dispose()` může zůstat handle otevřený a buffer nevyprázdněný.

4. **Windows vs. Linux odřádkování:**
   - `WriteLine()` → `\r\n` na Windows, `\n` na Linuxu.
   - `Write("\n")` → vždy `\n`.
   - `Environment.NewLine` → systémový default.

5. **`Split` bez `RemoveEmptyEntries`** vytváří prázdné stringy mezi sousedními oddělovači.

6. **Diakritika v konzoli** – nastav `Console.OutputEncoding = Encoding.UTF8;` ještě před prvním výpisem.

7. **`ToLower()` při frekvenci slov** – aby "Ahoj" a "ahoj" splynuly do jednoho klíče. Pro řazení v češtině pozor na `CultureInfo` (`OrderBy(s => s, StringComparer.Create(...))`).

8. **`throw ex;` vs. `throw;`** – `throw;` zachová původní stack trace, `throw ex;` ho přepíše. Při re-throwu používej `throw;`.

9. **`string.Length` není počet znaků** – je to počet UTF-16 code units. Emoji se počítají jako 2.

10. **Pořadí `catch` bloků** – od nejspecifičtějšího po nejobecnější. Jinak compiler chyba.

11. **Zápis bez `Flush()`/`Dispose()` = ztráta dat** – buffer se nedostane na disk při pádu procesu.

12. **`File.ReadAllLines` vs. `File.ReadLines`** – první načte vše do RAM, druhý streamuje. Pro gigabajty vždy `ReadLines`.

13. **TOCTOU race** – mezi `File.Exists` a `File.Open` může soubor zmizet. Spoléhej na `try/catch`, ne na předběžné dotazy, pokud na tom záleží.

14. **BOM u UTF-8** – `StreamReader` ho detekuje a tiše zahodí; `File.WriteAllText` ho defaultně **nepíše**. Některé editory (Notepad+) ho ale očekávají.

---

## Klíčové pojmy k zapamatování

- **Perzistence** – uložení dat tak, aby přežila restart programu/počítače.
- **Soubor** – pojmenovaná sekvence bytů v souborovém systému; OS abstrakce nad blokovým úložištěm.
- **Stream (proud)** – sekvenční abstrakce pro čtení/zápis dat; v .NET bázová třída `Stream`.
- **Textový vs. binární soubor** – konvence interpretace, fyzicky obojí jen byty.
- **Encoding (kódování)** – mapování `byte ↔ char`; ASCII (7 bit), Windows-1250, **UTF-8 (default)**, UTF-16.
- **BOM** – Byte Order Mark, signatura kódování na začátku souboru.
- **Buffering** – shromažďování dat v paměti před skutečným zápisem/čtením z disku.
- **`Flush()`** – vynucení vyprázdnění bufferu na podkladový stream.
- **`IDisposable`** – rozhraní pro deterministické uvolnění zdrojů přes `Dispose()`.
- **`using` blok / declaration** – syntaktický cukr nad `try/finally` s automatickým `Dispose()`.
- **Výjimka (Exception)** – objekt reprezentující chybu, propaguje se zásobníkem volání.
- **`try/catch/finally`** – blok pro zachycení a zpracování výjimek; `finally` vždy proběhne.
- **Hierarchie výjimek** – `Exception` → `IOException` → `FileNotFoundException` atd.
- **`File` třída** – statické "one-shot" metody (`ReadAllText`, `WriteAllLines`).
- **`StreamReader` / `StreamWriter`** – proudové čtení/zápis textu s automatickým dekódováním.
- **`File.ReadLines` vs. `ReadAllLines`** – lazy (streamuje) vs. eager (celý soubor do pole).
- **Bílé znaky** – mezera, tab, `\n`, `\r`, NBSP; rozpoznává `Char.IsWhiteSpace`.
- **CRLF vs. LF** – Windows `\r\n`, Unix `\n`; `Environment.NewLine` je platformní default.
- **Carriage Return (`\r`)** – návrat kurzoru na začátek řádku (historicky z psacích strojů).
- **Verbatim string `@"..."`** – řetězec bez interpretace escape sekvencí (`\` se bere doslovně).
- **`Path.Combine`** – platformně bezpečné skládání cest.
- **Relativní cesta** – vyhodnocena vůči `Environment.CurrentDirectory`; v IDE typicky `bin/Debug/...`.
- **TOCTOU** – Time-of-Check-to-Time-of-Use race; mezi `Exists` a `Open` se může soubor změnit.
- **TryParse vzor** – `int.TryParse(s, out int x)` neuhazuje výjimku při chybě.
