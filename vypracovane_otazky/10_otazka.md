# 📚 Zápisky: Otázka č. 10 - INSERT SORT. SELECT SORT.

**Datum:** 2025-01-31  
**Status:** ✅ Hotovo  
**Priorita:** ⭐⭐⭐ Vysoká (základní třídící algoritmy)

---

## ✅ Checklist bodů otázky

- [x] **Bod 1:** Motivace pro třídění dat
- [x] **Bod 2:** Insert Sort - popis po jednotlivých krocích
- [x] **Bod 3:** Insert Sort - znázornění na obrázku
- [x] **Bod 4:** Insert Sort - časová a paměťová složitost
- [x] **Bod 5:** Select Sort - popis po jednotlivých krocích
- [x] **Bod 6:** Select Sort - znázornění na obrázku
- [x] **Bod 7:** Select Sort - časová a paměťová složitost

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: Motivace pro třídění dat

**Teorie:**

Třídění (sorting) je jeden z nejzákladnějších a nejčastějších úkonů v informatice. Máme kolekci dat a chceme ji uspořádat podle nějakého kritéria (vzestupně/sestupně).

**Proč třídíme?**
- **Rychlejší vyhledávání** - v setříděném poli můžeme použít binární vyhledávání O(log n) místo lineárního O(n)
- **Detekce duplicit** - duplicitní prvky jsou vedle sebe
- **Přehlednost dat** - pro uživatele (abecední seznamy, ceníky...)
- **Prerekvizita pro jiné algoritmy** - merge, median, statistiky

**Příklady z praxe:**
- Seřazení e-mailů podle data
- Produkty v e-shopu podle ceny
- Studenti podle průměru známek
- Kontakty v telefonu podle abecedy

---

### Bod 2: Insert Sort - popis po jednotlivých krocích

**Teorie:**

Insert Sort (řazení vkládáním) funguje jako **skládání karet v ruce**:
1. Vezmeme kartu z nesetříděné části
2. Najdeme její správnou pozici v setříděné části
3. Vložíme ji tam (ostatní posuneme)

**Princip:**
- Pole rozdělíme myšlenkově na **setříděnou** (vlevo) a **nesetříděnou** (vpravo) část
- Na začátku je setříděná část jen první prvek (jeden prvek je vždy setříděný)
- V každém kroku vezmeme první prvek z nesetříděné části a zařadíme ho na správné místo do setříděné části

**Algoritmus krok za krokem:**
```
1. Pro každý prvek od indexu 1 do konce pole:
   a) Uložíme si aktuální prvek do pomocné proměnné (key)
   b) Porovnáváme key s prvky vlevo (setříděná část)
   c) Dokud jsou prvky vlevo větší než key, posouváme je doprava
   d) Vložíme key na uvolněné místo
```

**Kód (Maturitní verze):**

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// Insert Sort - řazení vkládáním
// Princip: Postupně bereme prvky a zařazujeme je na správné místo

static void InsertSort(int[] pole)
{
    // Procházíme od druhého prvku (první je "setříděný")
    for (int i = 1; i < pole.Length; i++)
    {
        int key = pole[i];     // Prvek k zařazení
        int j = i - 1;         // Index posledního setříděného prvku
        
        // Posouváme větší prvky doprava
        while (j >= 0 && pole[j] > key)
        {
            pole[j + 1] = pole[j];  // Posun doprava
            j--;
        }
        
        // Vložíme key na správné místo
        pole[j + 1] = key;
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// Generická verze s IComparable

static void InsertSort<T>(T[] pole) where T : IComparable<T>
{
    for (int i = 1; i < pole.Length; i++)
    {
        T key = pole[i];
        int j = i - 1;
        
        while (j >= 0 && pole[j].CompareTo(key) > 0)
        {
            pole[j + 1] = pole[j];
            j--;
        }
        pole[j + 1] = key;
    }
}

// Použití: InsertSort(studenti); // kde Student : IComparable<Student>
```

---

### Bod 3: Insert Sort - znázornění na obrázku

**ASCII vizualizace pro pole [5, 2, 4, 6, 1, 3]:**

```
Počáteční stav: [5, 2, 4, 6, 1, 3]
                 ↑
                 setříděná část (1 prvek)

═══════════════════════════════════════════════════════════════
KROK 1: Zařazujeme 2
═══════════════════════════════════════════════════════════════
[5, 2, 4, 6, 1, 3]    key = 2
 ↑  ↑
 │  └── prvek k zařazení
 └── porovnáváme: 5 > 2? ANO → posuneme 5 doprava

[5, 5, 4, 6, 1, 3]    5 posunuta doprava
 ↑
 └── sem vložíme 2

[2, 5, 4, 6, 1, 3]    ✓ 2 zařazena
 └──┘
 setříděná část

═══════════════════════════════════════════════════════════════
KROK 2: Zařazujeme 4
═══════════════════════════════════════════════════════════════
[2, 5, 4, 6, 1, 3]    key = 4
 └──┘  ↑
   │   └── prvek k zařazení
   └── setříděná část

Porovnání: 5 > 4? ANO → posuneme 5
           2 > 4? NE  → stop

[2, 4, 5, 6, 1, 3]    ✓ 4 zařazena
 └─────┘
 setříděná část

═══════════════════════════════════════════════════════════════
KROK 3: Zařazujeme 6
═══════════════════════════════════════════════════════════════
[2, 4, 5, 6, 1, 3]    key = 6
 └─────┘  ↑
         └── 5 > 6? NE → 6 už je na správném místě!

[2, 4, 5, 6, 1, 3]    ✓ 6 zůstává (nejlepší případ)
 └────────┘
 setříděná část

═══════════════════════════════════════════════════════════════
KROK 4: Zařazujeme 1
═══════════════════════════════════════════════════════════════
[2, 4, 5, 6, 1, 3]    key = 1
 └────────┘  ↑
             └── prvek k zařazení

Porovnání: 6 > 1? ANO → posun
           5 > 1? ANO → posun
           4 > 1? ANO → posun
           2 > 1? ANO → posun
           (j = -1, konec)

[1, 2, 4, 5, 6, 3]    ✓ 1 zařazena na začátek (nejhorší případ)
 └───────────┘
 setříděná část

═══════════════════════════════════════════════════════════════
KROK 5: Zařazujeme 3
═══════════════════════════════════════════════════════════════
[1, 2, 4, 5, 6, 3]    key = 3

Porovnání: 6 > 3? ANO → posun
           5 > 3? ANO → posun
           4 > 3? ANO → posun
           2 > 3? NE  → stop

[1, 2, 3, 4, 5, 6]    ✓ HOTOVO!
 └──────────────┘
 celé pole setříděno
```

---

### Bod 4: Insert Sort - časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší** | O(n) | Pole je již setříděné (každý prvek porovnáme jen jednou) |
| **Průměrný** | O(n²) | Náhodné pořadí prvků |
| **Nejhorší** | O(n²) | Pole je setříděné opačně (každý prvek musíme posunout na začátek) |

**Proč O(n²)?**
- Vnější cyklus: n-1 iterací
- Vnitřní cyklus: průměrně n/2 porovnání a posunů
- Celkem: (n-1) × n/2 ≈ n²/2 → **O(n²)**

**Paměťová složitost:**
- **O(1)** - konstantní, in-place algoritmus
- Používáme pouze pomocnou proměnnou `key` a indexy
- Netvoříme žádné nové pole

**Vlastnosti Insert Sortu:**
- ✅ **Stabilní** - zachovává pořadí prvků se stejnou hodnotou
- ✅ **In-place** - nepotřebuje extra paměť
- ✅ **Adaptivní** - rychlejší na částečně setříděných datech
- ✅ **Online** - může třídit data, jak přicházejí
- ❌ Pomalý na velkých nesetříděných datech

**Kdy použít Insert Sort?**
- Malá pole (do ~50 prvků)
- Téměř setříděná data
- Když potřebujeme stabilní třídění
- Jako součást hybridních algoritmů (např. TimSort)

---

### Bod 5: Select Sort - popis po jednotlivých krocích

**Teorie:**

Select Sort (řazení výběrem) funguje na principu **hledání minima**:
1. Najdeme nejmenší prvek v nesetříděné části
2. Prohodíme ho s prvním prvkem nesetříděné části
3. Posuneme hranici setříděné části

**Princip:**
- V každém kroku **vybereme** (select) minimum z nesetříděné části
- Dáme ho na konec setříděné části (prohodíme)
- Opakujeme, dokud není vše setříděné

**Algoritmus krok za krokem:**
```
1. Pro každou pozici i od 0 do n-2:
   a) Najdeme index nejmenšího prvku v části [i, n-1]
   b) Prohodíme prvky na pozicích i a minIndex
   c) Prvek na pozici i je nyní na svém finálním místě
```

**Kód (Maturitní verze):**

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// Select Sort - řazení výběrem
// Princip: Opakovaně najdeme minimum a dáme ho na správné místo

static void SelectSort(int[] pole)
{
    for (int i = 0; i < pole.Length - 1; i++)
    {
        // Najdeme index minima v nesetříděné části
        int minIndex = i;
        
        for (int j = i + 1; j < pole.Length; j++)
        {
            if (pole[j] < pole[minIndex])
            {
                minIndex = j;
            }
        }
        
        // Prohodíme minimum s prvkem na pozici i
        if (minIndex != i)
        {
            int temp = pole[i];
            pole[i] = pole[minIndex];
            pole[minIndex] = temp;
        }
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// S pomocnou metodou Swap a tuple syntax (C# 7+)

static void SelectSort<T>(T[] pole) where T : IComparable<T>
{
    for (int i = 0; i < pole.Length - 1; i++)
    {
        int minIndex = i;
        
        for (int j = i + 1; j < pole.Length; j++)
        {
            if (pole[j].CompareTo(pole[minIndex]) < 0)
                minIndex = j;
        }
        
        if (minIndex != i)
            (pole[i], pole[minIndex]) = (pole[minIndex], pole[i]);  // Tuple swap
    }
}
```

---

### Bod 6: Select Sort - znázornění na obrázku

**ASCII vizualizace pro pole [64, 25, 12, 22, 11]:**

```
Počáteční stav: [64, 25, 12, 22, 11]
                 ↑
                 začátek nesetříděné části

═══════════════════════════════════════════════════════════════
KROK 1: Hledáme minimum v celém poli
═══════════════════════════════════════════════════════════════
[64, 25, 12, 22, 11]
  ↑              ↑
  i=0            minimum = 11 (index 4)

Prohodíme pole[0] a pole[4]:
[11, 25, 12, 22, 64]
 ↑
 ✓ 11 je na finální pozici

═══════════════════════════════════════════════════════════════
KROK 2: Hledáme minimum v [25, 12, 22, 64]
═══════════════════════════════════════════════════════════════
[11, 25, 12, 22, 64]
 ✓    ↑   ↑
      i=1 minimum = 12 (index 2)

Prohodíme pole[1] a pole[2]:
[11, 12, 25, 22, 64]
 ✓   ↑
     ✓ 12 je na finální pozici

═══════════════════════════════════════════════════════════════
KROK 3: Hledáme minimum v [25, 22, 64]
═══════════════════════════════════════════════════════════════
[11, 12, 25, 22, 64]
 ✓   ✓   ↑   ↑
         i=2 minimum = 22 (index 3)

Prohodíme pole[2] a pole[3]:
[11, 12, 22, 25, 64]
 ✓   ✓   ↑
         ✓ 22 je na finální pozici

═══════════════════════════════════════════════════════════════
KROK 4: Hledáme minimum v [25, 64]
═══════════════════════════════════════════════════════════════
[11, 12, 22, 25, 64]
 ✓   ✓   ✓   ↑   
             i=3, minimum = 25 (index 3)

25 už je na správném místě → žádná výměna!
[11, 12, 22, 25, 64]
 ✓   ✓   ✓   ✓   ✓   HOTOVO!
```

**Shrnutí průběhu:**

```
Krok 1: [64, 25, 12, 22, 11] → min=11 → swap(0,4) → [11, 25, 12, 22, 64]
Krok 2: [11, 25, 12, 22, 64] → min=12 → swap(1,2) → [11, 12, 25, 22, 64]
Krok 3: [11, 12, 25, 22, 64] → min=22 → swap(2,3) → [11, 12, 22, 25, 64]
Krok 4: [11, 12, 22, 25, 64] → min=25 → no swap  → [11, 12, 22, 25, 64] ✓
```

---

### Bod 7: Select Sort - časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Vysvětlení |
|--------|-----------|------------|
| **Nejlepší** | O(n²) | VŽDY musíme projít celou nesetříděnou část |
| **Průměrný** | O(n²) | Stejné jako nejlepší |
| **Nejhorší** | O(n²) | Stejné jako nejlepší |

**Proč VŽDY O(n²)?**
- I když je pole setříděné, musíme projít všechny prvky, abychom **ověřili**, že máme minimum
- Vnější cyklus: n-1 iterací
- Vnitřní cyklus: (n-1) + (n-2) + ... + 1 = n(n-1)/2 porovnání
- Celkem: **O(n²)** bez ohledu na vstup

**Paměťová složitost:**
- **O(1)** - konstantní, in-place algoritmus
- Pouze pomocné proměnné (temp, minIndex)

**Vlastnosti Select Sortu:**
- ❌ **Nestabilní** - může změnit pořadí prvků se stejnou hodnotou
- ✅ **In-place** - nepotřebuje extra paměť
- ❌ **Neadaptivní** - nepřizpůsobí se již setříděným datům
- ✅ **Minimální počet swapů** - max n-1 výměn (výhoda při drahých přesunech)

**Kdy použít Select Sort?**
- Když jsou výměny prvků drahé (velké objekty)
- Pro jednoduchost implementace
- Když nepotřebujeme stabilitu
- Na velmi malých polích

---

## 📊 Porovnání Insert Sort vs Select Sort

| Vlastnost | Insert Sort | Select Sort |
|-----------|-------------|-------------|
| **Časová složitost (nejhorší)** | O(n²) | O(n²) |
| **Časová složitost (nejlepší)** | O(n) ✅ | O(n²) |
| **Paměťová složitost** | O(1) | O(1) |
| **Stabilita** | ✅ Stabilní | ❌ Nestabilní |
| **Adaptivita** | ✅ Adaptivní | ❌ Neadaptivní |
| **Počet porovnání** | ~n²/2 (průměr) | ~n²/2 (vždy) |
| **Počet přesunů/swapů** | ~n²/4 (průměr) | ~n (max) ✅ |
| **Online třídění** | ✅ Ano | ❌ Ne |

**Závěr:**
- **Insert Sort** je lepší pro téměř setříděná data a když potřebujeme stabilitu
- **Select Sort** je lepší když jsou výměny drahé (minimální počet swapů)
- Oba jsou vhodné pouze pro **malá pole** (do ~100 prvků)

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

### Časté chyby při implementaci:

1. **Insert Sort - špatné hranice cyklu:**
   ```csharp
   // ❌ ŠPATNĚ - začínáme od 0
   for (int i = 0; i < pole.Length; i++)
   
   // ✅ SPRÁVNĚ - začínáme od 1 (první prvek je "setříděný")
   for (int i = 1; i < pole.Length; i++)
   ```

2. **Select Sort - zapomenutí na podmínku při swapu:**
   ```csharp
   // ❌ ŠPATNĚ - zbytečný swap sám se sebou
   Swap(pole[i], pole[minIndex]);
   
   // ✅ SPRÁVNĚ - swap jen když je třeba
   if (minIndex != i)
       Swap(pole[i], pole[minIndex]);
   ```

3. **Select Sort - špatná inicializace minIndex:**
   ```csharp
   // ❌ ŠPATNĚ - minIndex vždy na 0
   int minIndex = 0;
   
   // ✅ SPRÁVNĚ - minIndex začíná na aktuální pozici i
   int minIndex = i;
   ```

### Typické otázky u ústní zkoušky:

- **"Který algoritmus je stabilní a proč?"**
  - Insert Sort je stabilní, protože při vkládání prvku zastavíme, když najdeme první menší/rovný prvek (nikdy nepřeskočíme rovný)
  - Select Sort není stabilní, protože při prohození může přeskočit prvek se stejnou hodnotou

- **"Kdy by Select Sort byl rychlejší než Insert Sort?"**
  - Když jsou výměny velmi drahé (velké objekty), protože Select Sort má max. n-1 swapů

- **"Proč Insert Sort funguje v O(n) na setříděném poli?"**
  - Protože vnitřní while cyklus se nikdy nespustí (pole[j] nikdy není > key)

- **"Je možné zrychlit Select Sort na setříděném poli?"**
  - Ne, protože VŽDY musíme projít celou nesetříděnou část, abychom našli minimum

### Co kontrolovat při Code Review:

- [ ] Správné indexy - outer loop od 1 (Insert) nebo do n-1 (Select)
- [ ] Správný směr porovnání (> vs <)
- [ ] Uložení `key` PŘED posouváním (Insert Sort)
- [ ] Inicializace `minIndex = i` (Select Sort)
- [ ] Podmínka `j >= 0` ve while cyklu (Insert Sort)

---

## 🚀 Senior Tip

**V praxi se Insert Sort a Select Sort téměř nepoužívají samostatně**, ale jsou důležité jako:

1. **Součást hybridních algoritmů:**
   - **TimSort** (Python, Java) používá Insert Sort pro malé části pole
   - **IntroSort** (C++ STL) přepne na Insert Sort pro pole < 16 prvků

2. **Optimalizace:**
   ```csharp
   // Hybrid: QuickSort + InsertSort pro malé části
   static void HybridSort(int[] pole, int left, int right)
   {
       if (right - left < 10)  // Malé pole
       {
           InsertSort(pole, left, right);  // Insert Sort je rychlejší
       }
       else
       {
           QuickSort(pole, left, right);
       }
   }
   ```

3. **Pro praktickou maturitu:**
   - Znalost Insert/Select Sort je základ pro pochopení složitějších algoritmů
   - Na zkoušce můžeš použít `Array.Sort()`, ale musíš umět vysvětlit jak funguje

---

## 🔗 Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 4 - Algoritmus a jeho vlastnosti** | Vlastnosti: konečnost, determinismus, obecnost |
| **Ot. 7 - Časová a paměťová složitost** | O-notace, nejhorší/nejlepší/průměrný případ |
| **Ot. 11 - Bubble Sort, Merge Sort** | Další třídící algoritmy, porovnání O(n²) vs O(n log n) |
| **Ot. 12 - Quick Sort** | Pokročilejší třídění, Divide & Conquer |
| **Ot. 14 - Vyhledávání** | Třídění jako prerekvizita pro binární vyhledávání |

---

## 📋 Procvičovací úlohy

### Doporučené úlohy k procvičení:

1. **Základní implementace:**
   - Implementuj Insert Sort a Select Sort
   - Přidej výpis pole po každém kroku (pro vizualizaci)

2. **Třídění objektů:**
   - Vytvoř třídu `Student` s vlastnostmi `Jmeno`, `Prumer`
   - Setřiď studenty podle průměru pomocí obou algoritmů

3. **Měření výkonu:**
   - Změř čas třídění pro různé velikosti pole (100, 1000, 10000)
   - Porovnej výkon na náhodných vs. již setříděných datech

4. **Stabilita:**
   - Vytvoř data s duplicitními klíči
   - Ověř, který algoritmus zachovává původní pořadí

---

## 🎯 Quick Reference Card (pro opakování)

```
╔══════════════════════════════════════════════════════════════╗
║              INSERT SORT vs SELECT SORT                      ║
╠══════════════════════════════════════════════════════════════╣
║  INSERT SORT                 │  SELECT SORT                  ║
║  "Skládání karet"            │  "Hledání minima"             ║
║                              │                               ║
║  1. Vezmi prvek              │  1. Najdi minimum             ║
║  2. Posuň větší doprava      │  2. Prohoď s první pozicí     ║
║  3. Vlož na místo            │  3. Opakuj pro zbytek         ║
║                              │                               ║
║  O(n) best / O(n²) worst     │  O(n²) VŽDY                   ║
║  STABILNÍ                    │  NESTABILNÍ                   ║
║  ADAPTIVNÍ                   │  NEADAPTIVNÍ                  ║
║  Hodně přesunů               │  Málo swapů (max n-1)         ║
╚══════════════════════════════════════════════════════════════╝
```

---

---

## 🔗 Externí zdroje

- **[Interaktivní vizualizace třídících algoritmů (Gemini)](https://gemini.google.com/share/83da0d650089)** - vizuální demonstrace Insert Sort a Select Sort

---

*📅 Vytvořeno: 2025-01-31 | 🎓 Maturitní příprava PRG 2025/2026*
