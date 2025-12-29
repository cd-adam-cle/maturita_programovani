# 📚 Zápisky: Otázka č. 6 - Práce s textovými soubory
**Datum:** 2024-12-29  
**Status:** Hotovo ✅

---

## ✅ Checklist bodů otázky

- [x] Práce s textovými soubory v C# (úvod, motivace)
- [x] StreamReader – klíčové funkce a metody
- [x] StreamWriter – klíčové metody
- [x] Blok `using`
- [x] Exceptions obecně
- [x] Časté výjimky specifické pro práci se soubory
- [x] Kódování (ASCII, Unicode)
- [x] Bílé znaky
- [x] Odřádkování (`\n`, `\r\n`)
- [x] Základní úloha: počet znaků
- [x] Základní úloha: počet slov
- [x] Základní úloha: frekvence slov

---

## 🧠 Klíčové koncepty & Snippety

### 1️⃣ Úvod - Proč soubory?

Program běží v **RAM** → data zmizí po vypnutí. Soubory = **perzistentní úložiště**.

**Dva přístupy:**
| Přístup | Třída | Použití |
|---------|-------|---------|
| Jednorázové operace | `File` | Malé soubory, rychlé operace |
| Proudové čtení/zápis | `StreamReader/Writer` | Velké soubory, po částech |

**Cesty k souborům:**
```csharp
// RELATIVNÍ (vzhledem k EXE v bin/Debug/netX.Y/)
string relativni = "data.txt";
string relativniSlozka = @"vstupy\soubor.txt";

// ABSOLUTNÍ
string absolutni = @"C:\Users\Adik\Documents\data.txt";
```

---

### 2️⃣ StreamReader - Klíčové metody

```csharp
using System.IO;

StreamReader sr = new StreamReader("soubor.txt");
```

| Metoda | Co dělá | Vrací |
|--------|---------|-------|
| `Read()` | Přečte 1 znak | `int` (-1 = konec) |
| `ReadLine()` | Přečte 1 řádek | `string` (null = konec) |
| `ReadToEnd()` | Přečte vše do konce | `string` |
| `Peek()` | Podívá se na další znak, neposune | `int` |
| `EndOfStream` | Jsme na konci? | `bool` |
| `Close()` | Zavře soubor | `void` |

**Čtení celého souboru:**
```csharp
StreamReader sr = new StreamReader("vstup.txt");
string obsah = sr.ReadToEnd();
sr.Close();
```

**Čtení po řádcích:**
```csharp
StreamReader sr = new StreamReader("vstup.txt");
while (!sr.EndOfStream)
{
    string radek = sr.ReadLine();
    Console.WriteLine(radek);
}
sr.Close();
```

**Čtení po znacích:**
```csharp
StreamReader sr = new StreamReader("vstup.txt");
int znak;
while ((znak = sr.Read()) != -1)
{
    char c = (char)znak;  // Přetypování int → char
    Console.Write(c);
}
sr.Close();
```

> ⚠️ `Read()` vrací `int` (ne `char`), protože -1 signalizuje konec souboru!

---

### 3️⃣ StreamWriter - Klíčové metody

```csharp
StreamWriter sw = new StreamWriter("soubor.txt");
```

| Metoda | Co dělá |
|--------|---------|
| `Write(text)` | Zapíše text, zůstane na řádku |
| `WriteLine(text)` | Zapíše text + odřádkování |
| `Flush()` | Vynutí zápis bufferu na disk |
| `Close()` | Zavře soubor |

**Druhý parametr = append:**
```csharp
new StreamWriter("soubor.txt", false)  // PŘEPÍŠE (default)
new StreamWriter("soubor.txt", true)   // PŘIPÍŠE na konec
```

---

### 4️⃣ Blok `using`

Zajistí **automatické zavření** souboru, i při výjimce:

```csharp
// ❌ ŠPATNĚ - můžeš zapomenout zavřít
StreamReader sr = new StreamReader("soubor.txt");
string text = sr.ReadToEnd();
sr.Close();  // Co když před tímto nastane chyba?

// ✅ SPRÁVNĚ - using zajistí zavření VŽDY
using (StreamReader sr = new StreamReader("soubor.txt"))
{
    string text = sr.ReadToEnd();
}  // Automaticky se zavolá Close()
```

---

### 5️⃣ Exceptions - Obecně

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
catch (IOException ex)
{
    Console.WriteLine("Chyba I/O: " + ex.Message);
}
```

**Vlastnosti výjimky (`ex`):**
| Vlastnost | Co obsahuje |
|-----------|-------------|
| `ex.Message` | Lidsky čitelný popis chyby |
| `ex.StackTrace` | Kde v kódu chyba nastala |

---

### 6️⃣ Časté výjimky pro soubory

| Výjimka | Kdy nastane |
|---------|-------------|
| `FileNotFoundException` | Soubor neexistuje |
| `DirectoryNotFoundException` | Složka neexistuje |
| `UnauthorizedAccessException` | Nemáš práva (systémové složky) |
| `IOException` | Soubor zamčený jiným procesem |

---

### 7️⃣ Kódování (ASCII, Unicode)

**ASCII:**
- 7 bitů = 128 znaků (0-127)
- 1 byte na znak
- ❌ Nemá českou diakritiku!

```csharp
char c = 'A';
int asciiKod = (int)c;  // 65

int kod = 66;
char znak = (char)kod;  // 'B'
```

**Unicode + UTF-8:**
- 150 000+ znaků (všechny jazyky + emoji)
- UTF-8: 1-4 byty na znak (variabilní)
- UTF-16: 2-4 byty (C# interně)

```csharp
// Soubory - UTF-8 je default, nemusíš řešit
File.WriteAllText("soubor.txt", "Řeřicha");

// Konzole - MUSÍŠ nastavit pro diakritiku!
Console.OutputEncoding = Encoding.UTF8;
```

**Kdy řešit kódování:**
| Situace | Řešení |
|---------|--------|
| Konzole + diakritika | `Console.OutputEncoding = Encoding.UTF8;` |
| Soubory | Neřeš (UTF-8 default) |
| Starý soubor z Windows | `Encoding.GetEncoding("windows-1250")` |

---

### 8️⃣ Bílé znaky

| Znak | Název | ASCII |
|------|-------|-------|
| `' '` | Mezera | 32 |
| `'\t'` | Tabulátor | 9 |
| `'\n'` | Line Feed | 10 |
| `'\r'` | Carriage Return | 13 |

**Detekce:**
```csharp
Char.IsWhiteSpace(' ')   // true
Char.IsWhiteSpace('\t')  // true
Char.IsWhiteSpace('A')   // false
```

**Odstranění z krajů:**
```csharp
string text = "   Ahoj   ";
string cisty = text.Trim();  // "Ahoj"
```

---

### 9️⃣ Odřádkování

| Systém | Sekvence | ASCII kódy |
|--------|----------|------------|
| Windows | `\r\n` | 13, 10 (CRLF) |
| Linux/Mac | `\n` | 10 (LF) |

```csharp
// WriteLine() na Windows přidá \r\n
sw.WriteLine("text");  // → text\r\n

// Explicitní \n v kódu
sw.Write("text\n");    // → text\n

// Univerzální řešení
sw.Write("text" + Environment.NewLine);
```

**Co je `\r`?**  
Carriage Return = vrátí kurzor na začátek řádku (z psacích strojů).

---

### 🔟 Třída File - Jednorázové operace

```csharp
// ČTENÍ
string obsah = File.ReadAllText("soubor.txt");
string[] radky = File.ReadAllLines("soubor.txt");

// ZÁPIS
File.WriteAllText("soubor.txt", "obsah");     // Přepíše
File.AppendAllText("soubor.txt", "další");    // Připíše
```

---

### 1️⃣1️⃣ Základní úloha: Počet znaků

```csharp
// Všechny znaky
string obsah = File.ReadAllText("soubor.txt");
int vsechny = obsah.Length;

// Bez bílých znaků
int bezBilych = 0;
foreach (char c in obsah)
{
    if (!Char.IsWhiteSpace(c))
        bezBilych++;
}

// LINQ varianta
int bezBilych = obsah.Count(c => !Char.IsWhiteSpace(c));
```

---

### 1️⃣2️⃣ Základní úloha: Počet slov

```csharp
string obsah = File.ReadAllText("soubor.txt");

// ✅ Správně - Split s null = všechny bílé znaky
string[] slova = obsah.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
int pocetSlov = slova.Length;
```

> ⚠️ `(char[])null` říká Splitu: "rozděl podle VŠECH bílých znaků"  
> ⚠️ `StringSplitOptions.RemoveEmptyEntries` odstraní prázdné stringy

---

### 1️⃣3️⃣ Základní úloha: Frekvence slov

```csharp
string obsah = File.ReadAllText("soubor.txt");
string[] slova = obsah.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

Dictionary<string, int> frekvence = new Dictionary<string, int>();

foreach (string slovo in slova)
{
    string upravene = slovo.ToLower();  // Aby "Ahoj" == "ahoj"
    
    if (frekvence.ContainsKey(upravene))
        frekvence[upravene]++;
    else
        frekvence[upravene] = 1;
}

// Výpis seřazený podle klíče (A-Z)
foreach (var par in frekvence.OrderBy(kv => kv.Key))
{
    Console.WriteLine($"{par.Key}: {par.Value}");
}
```

---

## 📦 Pomocné metody

### Char - statické metody
```csharp
Char.IsLetter(c)      // Je písmeno?
Char.IsDigit(c)       // Je číslice?
Char.IsWhiteSpace(c)  // Je bílý znak?
Char.ToLower(c)       // Malé písmeno
Char.ToUpper(c)       // Velké písmeno
```

### String - metody
```csharp
str.Trim()            // Odstraní bílé znaky z krajů
str.ToLower()         // Vše malé
str.Split(...)        // Rozdělí na pole
str.Contains("x")     // Obsahuje?
str.Length            // Délka
```

### Přetypování
```csharp
// int ↔ char (závorka nutná)
char c = (char)65;    // 'A'
int kod = (int)'A';   // 65

// cokoliv → string
int x = 42;
string s = x.ToString();  // "42"

// string → číslo
string s = "42";
int x = int.Parse(s);     // 42
```

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Relativní cesta** se vztahuje k `bin/Debug/netX.Y/`, NE ke zdrojovému kódu!

2. **Read() vrací int, ne char** - musíš přetypovat: `(char)znak`

3. **Nezapomeň zavřít soubor** - použij `using` blok

4. **Windows vs Linux odřádkování:**
   - `WriteLine()` → `\r\n` (2 znaky)
   - `Write("\n")` → `\n` (1 znak)

5. **Split bez parametrů** vytváří prázdné stringy - použij `RemoveEmptyEntries`

6. **Diakritika v konzoli** - nastav `Console.OutputEncoding = Encoding.UTF8`

7. **ToLower() při frekvenci** - aby "Ahoj" a "ahoj" bylo stejné slovo

---

## 🚀 Senior Tip

**LINQ zjednodušuje práci:**
```csharp
// Počet znaků bez bílých (1 řádek místo 5)
int pocet = obsah.Count(c => !Char.IsWhiteSpace(c));

// Seřazení slovníku
var serazene = frekvence.OrderByDescending(kv => kv.Value);  // Podle četnosti
```

**Odstranění diakritiky:**
```csharp
using System.Text;
using System.Globalization;

static string RemoveDiacritics(string text)
{
    var normalized = text.Normalize(NormalizationForm.FormD);
    var sb = new StringBuilder();
    
    foreach (var c in normalized)
    {
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            sb.Append(c);
    }
    
    return sb.ToString().Normalize(NormalizationForm.FormC);
}
// "řeřicha" → "rericha"
```

---

## 🎯 Quick Reference - Co použít kdy

| Chci... | Použij |
|---------|--------|
| Přečíst celý malý soubor | `File.ReadAllText()` |
| Přečíst velký soubor po částech | `StreamReader` + `ReadLine()` |
| Zapsat do souboru | `File.WriteAllText()` nebo `StreamWriter` |
| Zjistit jestli je znak mezera/tab/\n | `Char.IsWhiteSpace(c)` |
| Rozdělit text na slova | `Split((char[])null, RemoveEmptyEntries)` |
| Počítat výskyty | `Dictionary<string, int>` |
| Seřadit slovník | `.OrderBy(kv => kv.Key)` |
