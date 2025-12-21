# 📚 Zápisky: Otázka č. 4 - Algoritmus a jeho vlastnosti

**Datum:** 2024-12-18  
**Obtížnost:** ⭐⭐ (Střední - hodně teorie k zapamatování)

---

## ✅ Checklist bodů otázky

- [x] Pojem algoritmus
- [x] Vlastnost: elementárnost
- [x] Vlastnost: konečnost
- [x] Vlastnost: determinovanost
- [x] Vlastnost: obecnost
- [x] Vlastnost: determinismus
- [x] Vlastnost: výstup
- [x] Časová a prostorová složitost (úvod)
- [x] Příklad O(1) - konstantní
- [x] Příklad O(n) - lineární
- [x] Příklad O(n²) - kvadratický
- [x] Příklad O(log n) - logaritmický
- [x] Větvení (if, switch)
- [x] Cykly (for, while, foreach)

---


### 1. Pojem algoritmus

> **Algoritmus** je konečná posloupnost jednoznačně definovaných kroků, která pro každý přípustný vstup vede v konečném čase k požadovanému výstupu.

**Příklady algoritmů:**
- Recept na vaření (kroky vedoucí k jídlu)
- Třídění pole (Bubble sort, Quick sort)
- Hledání cesty v mapě (Dijkstra)

---

### 2. Šest vlastností algoritmu

| # | Vlastnost | Význam | Příklad porušení |
|---|-----------|--------|------------------|
| 1 | **Elementárnost** | Kroky jsou jednoduché a přímo proveditelné | "Vyřeš rovnici" - není elementární |
| 2 | **Konečnost** | Skončí po konečném počtu kroků | `while(true)` - nekonečná smyčka |
| 3 | **Determinovanost** | V každém kroku víme přesně co dělat | "Vezmi nějaké číslo" - nejednoznačné |
| 4 | **Obecnost** | Řeší celou třídu úloh | `suma = 1+2+3` - jen jeden případ |
| 5 | **Determinismus** | Stejný vstup → stejný výstup | `Random.Next()` - nedeterministické |
| 6 | **Výstup** | Má alespoň jeden výsledek | Funkce co nic nevrací a nic nedělá |

#### ⚠️ POZOR: Determinovanost vs Determinismus!

```
DETERMINOVANOST = jednoznačnost KROKŮ (víš co dělat)
DETERMINISMUS   = opakovatelnost VÝSLEDKŮ (stejný vstup = stejný výstup)
```

---

### 3. Časová a prostorová složitost

#### Co to je?

| Typ | Měří | Otázka |
|-----|------|--------|
| **Časová složitost** | Počet operací | Kolik kroků algoritmus udělá? |
| **Prostorová složitost** | Spotřeba paměti | Kolik paměti algoritmus potřebuje? |

#### Proč ne sekundy?
Sekundy závisí na hardwaru. Složitost je **univerzální měřítko** - popisuje, jak roste náročnost s velikostí vstupu **n**.

#### Big O notace - přehled

| Složitost | Název | n=10 | n=1000 | n=1000000 |
|-----------|-------|------|--------|-----------|
| O(1) | Konstantní | 1 | 1 | 1 |
| O(log n) | Logaritmická | 3 | 10 | 20 |
| O(n) | Lineární | 10 | 1000 | 1000000 |
| O(n log n) | Lineárně-log. | 33 | 10000 | 20000000 |
| O(n²) | Kvadratická | 100 | 1000000 | 10¹² 💀 |
| O(2ⁿ) | Exponenciální | 1024 | ∞ | ∞ |

---

### 4. O(1) - Konstantní složitost

> Počet operací je **vždy stejný**, nezáleží na velikosti vstupu.

```csharp
// Přístup k prvku pole - O(1)
int prvek = pole[500];

// Operace se zásobníkem - O(1)
stack.Push(42);
int x = stack.Pop();

// Operace s frontou - O(1)
queue.Enqueue(42);
int y = queue.Dequeue();

// Přístup do Dictionary - O(1)
int vek = slovnik["Petr"];

// Aritmetické operace - O(1)
int vysledek = a + b * c;

// Délka pole/listu - O(1)
int delka = pole.Length;
int pocet = list.Count;
```

---

### 5. O(n) - Lineární složitost

> Počet operací roste **přímo úměrně** s velikostí vstupu.

```csharp
// Hledání prvku v poli - O(n)
int NajdiIndex(int[] pole, int hledany)
{
    for (int i = 0; i < pole.Length; i++)
    {
        if (pole[i] == hledany)
            return i;
    }
    return -1;
}

// Součet prvků - O(n)
int Soucet(int[] pole)
{
    int suma = 0;
    for (int i = 0; i < pole.Length; i++)
    {
        suma += pole[i];
    }
    return suma;
}

// Hledání maxima - O(n)
int NajdiMax(int[] pole)
{
    int max = pole[0];
    foreach (int prvek in pole)
    {
        if (prvek > max)
            max = prvek;
    }
    return max;
}
```

**Pravidlo:** Jeden cyklus přes n prvků = O(n)

---

### 6. O(n²) - Kvadratická složitost

> Počet operací roste **s druhou mocninou** vstupu. 2× více dat = 4× déle!

```csharp
// Bubble Sort - O(n²)
void BubbleSort(int[] pole)
{
    int n = pole.Length;
    for (int i = 0; i < n - 1; i++)           // n×
    {
        for (int j = 0; j < n - i - 1; j++)   // n×
        {
            if (pole[j] > pole[j + 1])
            {
                int temp = pole[j];
                pole[j] = pole[j + 1];
                pole[j + 1] = temp;
            }
        }
    }
}

// Porovnání každého s každým - O(n²)
int PocetDuplicit(int[] pole)
{
    int pocet = 0;
    for (int i = 0; i < pole.Length; i++)
    {
        for (int j = i + 1; j < pole.Length; j++)
        {
            if (pole[i] == pole[j])
                pocet++;
        }
    }
    return pocet;
}
```

**Pravidlo:** Dva vnořené cykly přes n prvků = O(n²)

**Další O(n²) algoritmy:** Selection Sort, Insert Sort

---

### 7. O(log n) - Logaritmická složitost

> S každým krokem se problém **zmenší na polovinu**. Pro miliardu prvků stačí ~30 kroků!

```csharp
// Binární vyhledávání - O(log n)
// POZOR: Pole MUSÍ být setříděné!
int BinarniVyhledavani(int[] pole, int hledany)
{
    int levy = 0;
    int pravy = pole.Length - 1;
    
    while (levy <= pravy)
    {
        int stred = (levy + pravy) / 2;
        
        if (pole[stred] == hledany)
            return stred;
        else if (pole[stred] < hledany)
            levy = stred + 1;    // Pravá polovina
        else
            pravy = stred - 1;   // Levá polovina
    }
    return -1;
}
```

**Jak to funguje:**
```
Hledáme 67 v [2, 5, 13, 27, 45, 67, 78, 91, 99]

Krok 1: střed=45 → 67>45 → hledej VPRAVO
Krok 2: střed=78 → 67<78 → hledej VLEVO  
Krok 3: střed=67 → NALEZENO! ✓

9 prvků, jen 3 kroky!
```

**Pravidlo:** Půlení problému v každém kroku = O(log n)

---

### 8. Větvení (if, switch)

#### IF - základní větvení

```csharp
// Jednoduchý if
if (vek >= 18)
{
    Console.WriteLine("Dospělý");
}

// If-else
if (cislo % 2 == 0)
{
    Console.WriteLine("Sudé");
}
else
{
    Console.WriteLine("Liché");
}

// If-else if-else
if (znamka == 1)
    Console.WriteLine("Výborně");
else if (znamka == 2)
    Console.WriteLine("Chvalitebně");
else if (znamka == 3)
    Console.WriteLine("Dobře");
else
    Console.WriteLine("Nedostatečně");
```

#### SWITCH - přepínač

```csharp
// Klasický switch
switch (den)
{
    case 1:
        Console.WriteLine("Pondělí");
        break;
    case 2:
        Console.WriteLine("Úterý");
        break;
    case 6:
    case 7:
        Console.WriteLine("Víkend!");
        break;
    default:
        Console.WriteLine("Neplatný den");
        break;
}

// Switch expression (C# 8+)
string nazev = den switch
{
    1 => "Pondělí",
    2 => "Úterý",
    6 or 7 => "Víkend!",
    _ => "Neplatný den"
};
```

---

### 9. Cykly (for, while, foreach)

#### FOR - známý počet opakování

```csharp
// Základní for
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

// Procházení pole s indexem
for (int i = 0; i < pole.Length; i++)
{
    Console.WriteLine($"pole[{i}] = {pole[i]}");
}

// Pozpátku
for (int i = pole.Length - 1; i >= 0; i--)
{
    Console.WriteLine(pole[i]);
}
```

#### WHILE - opakuj dokud platí podmínka

```csharp
// Čti vstup dokud uživatel nezadá "konec"
string vstup = "";
while (vstup != "konec")
{
    vstup = Console.ReadLine();
}

// Půlení čísla (O(log n) pattern!)
while (n > 1)
{
    n = n / 2;
}
```

#### DO-WHILE - vždy alespoň jednou

```csharp
int volba;
do
{
    Console.WriteLine("1. Hra  2. Konec");
    volba = int.Parse(Console.ReadLine());
} while (volba != 2);
```

#### FOREACH - procházení kolekcí

```csharp
// Pole
foreach (int cislo in pole)
{
    Console.WriteLine(cislo);
}

// List
foreach (string jmeno in seznam)
{
    Console.WriteLine(jmeno);
}

// Dictionary
foreach (var zaznam in slovnik)
{
    Console.WriteLine($"{zaznam.Key}: {zaznam.Value}");
}
```

#### Break a Continue

```csharp
// break - okamžitě ukonči cyklus
for (int i = 0; i < 100; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);  // Vypíše 0,1,2,3,4
}

// continue - přeskoč na další iteraci
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0) continue;
    Console.WriteLine(i);  // Vypíše 1,3,5,7,9
}
```

---

## ⚠️ Na co si dát pozor (Maturitní chytáky)

### 1. Determinovanost ≠ Determinismus
```
Determinovanost = víš CO dělat (jednoznačnost kroků)
Determinismus = víš CO DOSTANEŠ (stejný vstup → stejný výstup)
```

### 2. List.Contains() není O(1)!
```csharp
list.Contains(x);    // O(n) - musí projít celý list!
slovnik.ContainsKey(x);  // O(1) - hashování
```

### 3. Binární vyhledávání vyžaduje SETŘÍDĚNÉ pole
```csharp
// ❌ Nefunguje na nesetříděném poli!
BinarniVyhledavani(nesetridenePole, x);
```

### 4. Časté chyby v cyklech
```csharp
// ❌ Nekonečný cyklus
while (i < 10) { /* chybí i++ */ }

// ❌ Off-by-one error
for (int i = 0; i <= pole.Length; i++)  // IndexOutOfRange!

// ❌ Modifikace kolekce ve foreach
foreach (var x in list) { list.Remove(x); }  // Exception!
```

### 5. Přiřazení vs porovnání
```csharp
if (x = 5)   // ❌ Přiřazení!
if (x == 5)  // ✅ Porovnání
```

---

## 🚀 Senior Tipy

### 1. Jak rychle určit složitost
```
Žádný cyklus           → O(1)
1 cyklus přes n        → O(n)
2 vnořené cykly        → O(n²)
Půlení v každém kroku  → O(log n)
1 cyklus + půlení      → O(n log n)
```

### 2. Switch expression je elegantnější
```csharp
// Místo dlouhého switch použij:
string vysledek = hodnota switch
{
    1 => "Jedna",
    2 => "Dva",
    _ => "Jiné"
};
```

### 3. LINQ pro kratší kód
```csharp
// Místo ručního hledání maxima:
int max = pole.Max();

// Místo ručního filtrování:
var suda = pole.Where(x => x % 2 == 0).ToList();
```

