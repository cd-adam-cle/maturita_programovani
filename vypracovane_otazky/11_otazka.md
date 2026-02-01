# 📚 Zápisky: Otázka č. 11 - BUBBLE SORT. MERGE SORT.

**Datum:** 2025-01-31  
**Status:** ✅ Hotovo  
**Priorita:** ⭐⭐⭐ Vysoká (Merge Sort častý na praktické maturitě!)

---

## ✅ Checklist bodů otázky

- [x] **Bod 1:** Motivace pro třídění dat
- [x] **Bod 2:** Bubble Sort - popis po jednotlivých krocích
- [x] **Bod 3:** Bubble Sort - znázornění na obrázku
- [x] **Bod 4:** Bubble Sort - časová a paměťová složitost
- [x] **Bod 5:** Merge Sort - popis po jednotlivých krocích
- [x] **Bod 6:** Merge Sort - znázornění na obrázku
- [x] **Bod 7:** Merge Sort - časová a paměťová složitost
- [x] **Bod 8:** Merge Sort - princip Rozděl a panuj (Divide & Conquer)

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: Motivace pro třídění dat

**Teorie:**

*(Stejná jako v Otázce 10 - zde pouze stručně)*

Třídění je jedním z nejzákladnějších úkonů v informatice. Setříděná data umožňují:
- **Rychlejší vyhledávání** - binární vyhledávání O(log n)
- **Snadnou detekci duplicit** - duplicity jsou vedle sebe
- **Efektivní slučování dat** - Merge Sort staví právě na tomto
- **Přehlednost pro uživatele** - abecední seznamy, ceníky

**Proč znát více algoritmů?**

| Algoritmus | Časová složitost | Kdy použít |
|------------|------------------|------------|
| Bubble Sort | O(n²) | Výuka, velmi malá data |
| Merge Sort | O(n log n) | Velká data, potřeba stability, externí třídění |
| Quick Sort | O(n log n) průměr | Obecné použití, in-place |
| Insert Sort | O(n²) | Téměř setříděná data |

---

### Bod 2: Bubble Sort - popis po jednotlivých krocích

**Teorie:**

Bubble Sort (bublinkové třídění) funguje na principu **probublávání největších prvků** na konec pole:
1. Procházíme pole od začátku
2. Porovnáváme sousední prvky
3. Pokud jsou ve špatném pořadí, prohodíme je
4. Největší prvek "probublá" na konec
5. Opakujeme pro zbytek pole

**Princip:**
- V každém průchodu "probublá" jeden největší prvek na své místo
- Po k průchodech je k největších prvků na konci setříděno
- Název pochází z toho, že velké prvky "stoupají" jako bubliny

**Algoritmus krok za krokem:**
```
1. Pro každý průchod i od 0 do n-2:
   a) Pro každou pozici j od 0 do n-2-i:
      - Porovnej pole[j] a pole[j+1]
      - Pokud pole[j] > pole[j+1], prohoď je
   b) Po průchodu je prvek na pozici n-1-i na svém místě
2. Opakuj, dokud není pole setříděné
```

**Kód (Maturitní verze):**

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// Bubble Sort - základní implementace
// Princip: Opakovaně procházíme pole a prohazujeme sousední prvky

static void BubbleSort(int[] pole)
{
    int n = pole.Length;
    
    // Vnější cyklus - počet průchodů
    for (int i = 0; i < n - 1; i++)
    {
        // Vnitřní cyklus - procházení nesetříděné části
        for (int j = 0; j < n - 1 - i; j++)
        {
            // Porovnání sousedních prvků
            if (pole[j] > pole[j + 1])
            {
                // Prohození (swap)
                int temp = pole[j];
                pole[j] = pole[j + 1];
                pole[j + 1] = temp;
            }
        }
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// Optimalizovaný Bubble Sort s early exit
// Pokud v průchodu nedojde k žádné výměně, pole je setříděné

static void BubbleSortOptimized(int[] pole)
{
    int n = pole.Length;
    bool swapped;  // Flag pro optimalizaci
    
    for (int i = 0; i < n - 1; i++)
    {
        swapped = false;
        
        for (int j = 0; j < n - 1 - i; j++)
        {
            if (pole[j] > pole[j + 1])
            {
                (pole[j], pole[j + 1]) = (pole[j + 1], pole[j]);  // Tuple swap
                swapped = true;
            }
        }
        
        // Pokud nedošlo k žádné výměně, pole je setříděné
        if (!swapped)
            break;
    }
}

// Výhoda: Na již setříděném poli běží v O(n) místo O(n²)!
```

---

### Bod 3: Bubble Sort - znázornění na obrázku

**ASCII vizualizace pro pole [5, 1, 4, 2, 8]:**

```
Počáteční stav: [5, 1, 4, 2, 8]

═══════════════════════════════════════════════════════════════
PRŮCHOD 1: Největší prvek (8) probublá na konec
═══════════════════════════════════════════════════════════════

Krok 1.1: Porovnej [5, 1]
[5, 1, 4, 2, 8]    5 > 1? ANO → swap
 ↑  ↑
[1, 5, 4, 2, 8]

Krok 1.2: Porovnej [5, 4]
[1, 5, 4, 2, 8]    5 > 4? ANO → swap
    ↑  ↑
[1, 4, 5, 2, 8]

Krok 1.3: Porovnej [5, 2]
[1, 4, 5, 2, 8]    5 > 2? ANO → swap
       ↑  ↑
[1, 4, 2, 5, 8]

Krok 1.4: Porovnej [5, 8]
[1, 4, 2, 5, 8]    5 > 8? NE → bez změny
          ↑  ↑
[1, 4, 2, 5, 8]    ✓ 8 je na svém místě!
             ↑
             setříděno

═══════════════════════════════════════════════════════════════
PRŮCHOD 2: Druhý největší (5) probublá na své místo
═══════════════════════════════════════════════════════════════

Krok 2.1: Porovnej [1, 4]
[1, 4, 2, 5, 8]    1 > 4? NE
 ↑  ↑
[1, 4, 2, 5, 8]

Krok 2.2: Porovnej [4, 2]
[1, 4, 2, 5, 8]    4 > 2? ANO → swap
    ↑  ↑
[1, 2, 4, 5, 8]

Krok 2.3: Porovnej [4, 5]
[1, 2, 4, 5, 8]    4 > 5? NE
       ↑  ↑
[1, 2, 4, 5, 8]    ✓ 5 je na svém místě!
          └──┘
          setříděno

═══════════════════════════════════════════════════════════════
PRŮCHOD 3: Kontrola zbytku
═══════════════════════════════════════════════════════════════

Krok 3.1: Porovnej [1, 2]
[1, 2, 4, 5, 8]    1 > 2? NE
 ↑  ↑

Krok 3.2: Porovnej [2, 4]
[1, 2, 4, 5, 8]    2 > 4? NE
    ↑  ↑

[1, 2, 4, 5, 8]    ✓ Žádný swap → HOTOVO!
 └────────────┘
 celé pole setříděno
```

**Schéma "probublávání":**

```
Průchod 1:  [5,1,4,2,8] → [1,4,2,5,8]  ← 8 probublalo na konec
                                   ↑
Průchod 2:  [1,4,2,5,8] → [1,2,4,5,8]  ← 5 probublalo
                                ↑
Průchod 3:  [1,2,4,5,8] → [1,2,4,5,8]  ← žádná změna = HOTOVO
```

---

### Bod 4: Bubble Sort - časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší** | O(n) | Pole je setříděné (s optimalizací!) |
| **Průměrný** | O(n²) | Náhodné pořadí |
| **Nejhorší** | O(n²) | Pole je setříděné opačně |

**Proč O(n²)?**
- Vnější cyklus: n-1 průchodů
- Vnitřní cyklus: průměrně n/2 porovnání
- Celkem: (n-1) + (n-2) + ... + 1 = n(n-1)/2 → **O(n²)**

**Paměťová složitost:**
- **O(1)** - konstantní, in-place algoritmus
- Pouze pomocná proměnná `temp` pro swap

**Vlastnosti Bubble Sortu:**

| Vlastnost | Hodnota |
|-----------|---------|
| ✅ **Stabilní** | Ano (prvky se stejnou hodnotou zachovávají pořadí) |
| ✅ **In-place** | Ano (nepotřebuje extra paměť) |
| ✅ **Adaptivní** | Ano (s optimalizací - rychlejší na částečně setříděných) |
| ❌ **Efektivní** | Ne (nejpomalejší z běžných algoritmů) |

**Kdy použít Bubble Sort?**
- 🎓 Výuka - snadný na pochopení
- 📊 Velmi malá data (do 20 prvků)
- ✅ Když potřebujeme stabilitu a jednoduchost
- ❌ NIKDY na velká data v produkci!

---

### Bod 5: Merge Sort - popis po jednotlivých krocích

**Teorie:**

Merge Sort (třídění slučováním) je založený na principu **Rozděl a panuj** (Divide & Conquer):
1. **Rozděl** (Divide): Rozděl pole na dvě poloviny
2. **Panuj** (Conquer): Rekurzivně setřiď obě poloviny
3. **Sluč** (Merge): Slij dvě setříděné poloviny do jedné

**Klíčová myšlenka:**
- Sloučit dvě **již setříděné** pole je snadné - stačí O(n)
- Rekurzí rozdělíme pole až na jednotlivé prvky (ty jsou triviálně setříděné)
- Pak slučujeme zpět nahoru

**Algoritmus krok za krokem:**
```
MERGE_SORT(pole, left, right):
1. Pokud left >= right, vrať se (základní případ - 1 nebo 0 prvků)
2. Vypočti střed: mid = (left + right) / 2
3. Rekurzivně zavolej MERGE_SORT(pole, left, mid)     // levá polovina
4. Rekurzivně zavolej MERGE_SORT(pole, mid+1, right)  // pravá polovina
5. Zavolej MERGE(pole, left, mid, right)              // sluč obě poloviny

MERGE(pole, left, mid, right):
1. Vytvoř pomocné pole pro výsledek
2. Použij dva ukazatele (i pro levou, j pro pravou část)
3. Porovnávej prvky a vkládej menší do výsledku
4. Zkopíruj zbývající prvky
5. Překopíruj výsledek zpět do původního pole
```

**Kód (Maturitní verze):**

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// Merge Sort - rekurzivní implementace
// Princip: Rozděl a panuj - rozděl, setřiď poloviny, sluč

static void MergeSort(int[] pole, int left, int right)
{
    // Základní případ: 1 nebo 0 prvků
    if (left >= right)
        return;
    
    // Rozděl: najdi střed
    int mid = (left + right) / 2;
    
    // Panuj: rekurzivně setřiď obě poloviny
    MergeSort(pole, left, mid);
    MergeSort(pole, mid + 1, right);
    
    // Sluč: spoj obě setříděné poloviny
    Merge(pole, left, mid, right);
}

static void Merge(int[] pole, int left, int mid, int right)
{
    // Velikosti obou polovin
    int n1 = mid - left + 1;
    int n2 = right - mid;
    
    // Pomocná pole pro kopie
    int[] levaCast = new int[n1];
    int[] pravaCast = new int[n2];
    
    // Zkopíruj data do pomocných polí
    for (int x = 0; x < n1; x++)
        levaCast[x] = pole[left + x];
    for (int x = 0; x < n2; x++)
        pravaCast[x] = pole[mid + 1 + x];
    
    // Slučování - dva ukazatele
    int i = 0, j = 0;
    int k = left;  // Index v původním poli
    
    while (i < n1 && j < n2)
    {
        if (levaCast[i] <= pravaCast[j])
        {
            pole[k] = levaCast[i];
            i++;
        }
        else
        {
            pole[k] = pravaCast[j];
            j++;
        }
        k++;
    }
    
    // Zkopíruj zbývající prvky z levé části
    while (i < n1)
    {
        pole[k] = levaCast[i];
        i++;
        k++;
    }
    
    // Zkopíruj zbývající prvky z pravé části
    while (j < n2)
    {
        pole[k] = pravaCast[j];
        j++;
        k++;
    }
}

// Volání: MergeSort(pole, 0, pole.Length - 1);
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// Jednodušší syntaxe s LINQ a Array.Copy

static int[] MergeSortFunctional(int[] pole)
{
    if (pole.Length <= 1)
        return pole;
    
    int mid = pole.Length / 2;
    
    // Rozděl pomocí LINQ
    var leva = MergeSortFunctional(pole.Take(mid).ToArray());
    var prava = MergeSortFunctional(pole.Skip(mid).ToArray());
    
    // Sluč
    return MergeFunctional(leva, prava);
}

static int[] MergeFunctional(int[] leva, int[] prava)
{
    var result = new List<int>();
    int i = 0, j = 0;
    
    while (i < leva.Length && j < prava.Length)
    {
        if (leva[i] <= prava[j])
            result.Add(leva[i++]);
        else
            result.Add(prava[j++]);
    }
    
    // Přidej zbývající prvky
    result.AddRange(leva.Skip(i));
    result.AddRange(prava.Skip(j));
    
    return result.ToArray();
}

// Poznámka: Čistější kód, ale méně efektivní kvůli alokacím
```

---

### Bod 6: Merge Sort - znázornění na obrázku

**ASCII vizualizace pro pole [38, 27, 43, 3, 9, 82, 10]:**

```
                    FÁZE ROZDĚLOVÁNÍ (shora dolů)
═══════════════════════════════════════════════════════════════

                [38, 27, 43, 3, 9, 82, 10]
                           │
              ┌────────────┴────────────┐
              │                         │
        [38, 27, 43, 3]          [9, 82, 10]
              │                         │
        ┌─────┴─────┐             ┌─────┴─────┐
        │           │             │           │
    [38, 27]    [43, 3]       [9, 82]      [10]
        │           │             │           │
     ┌──┴──┐     ┌──┴──┐      ┌──┴──┐        │
     │     │     │     │      │     │        │
   [38]  [27]  [43]  [3]    [9]   [82]     [10]
     │     │     │     │      │     │        │
     └──┬──┘     └──┬──┘      └──┬──┘        │
        ↓           ↓            ↓           │
                                             │
                    FÁZE SLUČOVÁNÍ (zdola nahoru)
═══════════════════════════════════════════════════════════════

   [38]  [27]  [43]  [3]    [9]   [82]     [10]
     │     │     │     │      │     │        │
     └──┬──┘     └──┬──┘      └──┬──┘        │
        ↓           ↓            ↓           │
    [27, 38]    [3, 43]       [9, 82]      [10]
        │           │             │           │
        └─────┬─────┘             └─────┬─────┘
              ↓                         ↓
        [3, 27, 38, 43]          [9, 10, 82]
              │                         │
              └────────────┬────────────┘
                           ↓
                [3, 9, 10, 27, 38, 43, 82]
                           ✓
                     SETŘÍDĚNO!
```

**Detailní ukázka operace MERGE:**

```
Slučování [27, 38] a [3, 43] → [3, 27, 38, 43]
═══════════════════════════════════════════════

Levá část:  [27, 38]     i = 0
Pravá část: [3, 43]      j = 0
Výsledek:   [_, _, _, _] k = 0

Krok 1: 27 vs 3 → 3 < 27 → bereme 3 z pravé
        Výsledek: [3, _, _, _]  j++

Krok 2: 27 vs 43 → 27 < 43 → bereme 27 z levé
        Výsledek: [3, 27, _, _]  i++

Krok 3: 38 vs 43 → 38 < 43 → bereme 38 z levé
        Výsledek: [3, 27, 38, _]  i++

Krok 4: levá prázdná → zkopíruj zbytek pravé (43)
        Výsledek: [3, 27, 38, 43]  ✓
```

---

### Bod 7: Merge Sort - časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Vysvětlení |
|--------|-----------|------------|
| **Nejlepší** | O(n log n) | VŽDY stejná |
| **Průměrný** | O(n log n) | VŽDY stejná |
| **Nejhorší** | O(n log n) | VŽDY stejná |

**Proč O(n log n)?**

```
Hloubka rekurze: log₂(n) úrovní
     │
     ▼
     [n prvků]                    ← úroveň 0: 1 pole, n prvků
        │
   ┌────┴────┐
[n/2]      [n/2]                  ← úroveň 1: 2 pole, n prvků celkem
   │          │
 ┌─┴─┐      ┌─┴─┐
[n/4][n/4] [n/4][n/4]             ← úroveň 2: 4 pole, n prvků celkem
  ...       ...
[1][1]...[1][1]                   ← úroveň log(n): n polí, n prvků

Na KAŽDÉ úrovni: O(n) práce (slučování)
Počet úrovní: log₂(n)
Celkem: O(n) × O(log n) = O(n log n)
```

**Paměťová složitost:**
- **O(n)** - potřebujeme pomocné pole pro slučování
- NENÍ in-place algoritmus!
- Plus O(log n) pro zásobník rekurze

**Vlastnosti Merge Sortu:**

| Vlastnost | Hodnota |
|-----------|---------|
| ✅ **Stabilní** | Ano (díky `<=` při slučování) |
| ❌ **In-place** | Ne (potřebuje O(n) extra paměti) |
| ❌ **Adaptivní** | Ne (vždy stejná složitost) |
| ✅ **Prediktabilní** | Ano (vždy O(n log n)) |

**Kdy použít Merge Sort?**
- 📊 Velká data - garantovaný O(n log n)
- 🔗 Spojové seznamy - ideální (merge je O(1) extra paměť)
- 💾 Externí třídění - třídění souborů větších než RAM
- ✅ Když potřebujeme stabilitu
- ✅ Když potřebujeme garantovaný výkon (žádný nejhorší případ)

---

### Bod 8: Merge Sort - princip Rozděl a panuj

**Paradigma Divide & Conquer:**

```
┌─────────────────────────────────────────────────────────┐
│                    ROZDĚL A PANUJ                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  1. ROZDĚL (Divide)                                     │
│     → Rozděl problém na menší podproblémy               │
│     → V Merge Sort: rozděl pole na poloviny             │
│                                                         │
│  2. PANUJ (Conquer)                                     │
│     → Vyřeš podproblémy rekurzivně                      │
│     → V Merge Sort: setřiď obě poloviny                 │
│                                                         │
│  3. KOMBINUJ (Combine)                                  │
│     → Zkombinuj řešení podproblémů                      │
│     → V Merge Sort: sluč setříděné poloviny             │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

**Další algoritmy používající Divide & Conquer:**

| Algoritmus | Rozděl | Panuj | Kombinuj |
|------------|--------|-------|----------|
| **Merge Sort** | Půl pole | Setřiď poloviny | Merge |
| **Quick Sort** | Kolem pivota | Setřiď části | Nic (in-place) |
| **Binary Search** | Půl pole | Hledej v polovině | Vrať výsledek |
| **Strassen** | Matice na 4 | Násobení | Sečti výsledky |

**Rekurzivní struktura Merge Sortu:**

```
MergeSort([38, 27, 43, 3])
│
├── MergeSort([38, 27])          ← ROZDĚL
│   ├── MergeSort([38])          ← základní případ
│   ├── MergeSort([27])          ← základní případ
│   └── Merge → [27, 38]         ← KOMBINUJ
│
├── MergeSort([43, 3])           ← ROZDĚL
│   ├── MergeSort([43])
│   ├── MergeSort([3])
│   └── Merge → [3, 43]          ← KOMBINUJ
│
└── Merge → [3, 27, 38, 43]      ← KOMBINUJ finální
```

---

## 📊 Porovnání Bubble Sort vs Merge Sort

| Vlastnost | Bubble Sort | Merge Sort |
|-----------|-------------|------------|
| **Časová složitost (nejhorší)** | O(n²) ❌ | O(n log n) ✅ |
| **Časová složitost (nejlepší)** | O(n) ✅ | O(n log n) |
| **Paměťová složitost** | O(1) ✅ | O(n) ❌ |
| **Stabilita** | ✅ Stabilní | ✅ Stabilní |
| **In-place** | ✅ Ano | ❌ Ne |
| **Adaptivita** | ✅ Ano (s opt.) | ❌ Ne |
| **Složitost implementace** | Velmi snadná | Střední |
| **Použití v praxi** | Pouze výuka | Ano (externí třídění) |

**Růst složitosti - konkrétní čísla:**

| n | Bubble O(n²) | Merge O(n log n) | Poměr |
|---|--------------|------------------|-------|
| 10 | 100 | 33 | 3× |
| 100 | 10 000 | 664 | 15× |
| 1 000 | 1 000 000 | 9 966 | 100× |
| 10 000 | 100 000 000 | 132 877 | 753× |

**Závěr:** Pro velká data je Merge Sort **stovky až tisíckrát rychlejší**!

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

### Časté chyby při implementaci:

1. **Bubble Sort - špatné hranice vnitřního cyklu:**
   ```csharp
   // ❌ ŠPATNĚ - zbytečné iterace, nebo přístup mimo pole
   for (int j = 0; j < n - 1; j++)
   
   // ✅ SPRÁVNĚ - zmenšujeme hranici s každým průchodem
   for (int j = 0; j < n - 1 - i; j++)
   ```

2. **Merge Sort - špatný výpočet středu:**
   ```csharp
   // ❌ ŠPATNĚ - může přetéct pro velká čísla
   int mid = (left + right) / 2;
   
   // ✅ SPRÁVNĚ - bezpečnější varianta
   int mid = left + (right - left) / 2;
   ```

3. **Merge Sort - zapomenutí na zbývající prvky:**
   ```csharp
   // ❌ ŠPATNĚ - chybí kopírování zbytku
   while (i < n1 && j < n2) { ... }
   // HOTOVO? NE!
   
   // ✅ SPRÁVNĚ - musíme zkopírovat zbytek
   while (i < n1) { pole[k++] = levaCast[i++]; }
   while (j < n2) { pole[k++] = pravaCast[j++]; }
   ```

4. **Merge Sort - nestabilní merge:**
   ```csharp
   // ❌ ŠPATNĚ - naruší stabilitu
   if (levaCast[i] < pravaCast[j])  // pouze <
   
   // ✅ SPRÁVNĚ - zachová stabilitu
   if (levaCast[i] <= pravaCast[j])  // <= preferuje levou část
   ```

### Typické otázky u ústní zkoušky:

- **"Proč je Merge Sort vždy O(n log n)?"**
  - Protože vždy rozdělí pole na poloviny (log n úrovní) a na každé úrovni udělá O(n) práce při slučování

- **"Je Bubble Sort někdy rychlejší než Merge Sort?"**
  - ANO, pro velmi malá pole (režie rekurze u Merge Sortu) nebo téměř setříděná data (s optimalizací)

- **"Proč Merge Sort potřebuje O(n) extra paměti?"**
  - Protože při slučování potřebujeme pomocné pole pro kopii dat (nelze slučovat in-place efektivně)

- **"Co je stabilita a proč je důležitá?"**
  - Stabilní algoritmus zachovává relativní pořadí prvků se stejným klíčem. Důležité např. při třídění podle více kritérií.

### Co kontrolovat při Code Review:

- [ ] Bubble Sort: Správná podmínka `n - 1 - i` ve vnitřním cyklu
- [ ] Merge Sort: Správné indexy `left`, `mid`, `right`
- [ ] Merge Sort: Kopírování VŠECH zbývajících prvků po merge
- [ ] Merge Sort: Základní případ `if (left >= right) return;`
- [ ] Stabilita: Použití `<=` místo `<` při porovnávání

---

## 🚀 Senior Tip

**Merge Sort je v praxi velmi důležitý algoritmus:**

1. **Externí třídění (External Sort):**
   - Třídění souborů větších než RAM
   - Rozdělíme soubor na části, setřídíme v paměti, slučujeme zpět
   - Používá se v databázích a velkých datových systémech

2. **TimSort (Python, Java):**
   - Hybridní algoritmus: Merge Sort + Insert Sort
   - Identifikuje již setříděné "runy" a slučuje je
   - Standardní algoritmus v mnoha jazycích

3. **Paralelizace:**
   - Merge Sort se skvěle paralelizuje (obě poloviny nezávisle)
   - Ideální pro vícejádrové procesory

```csharp
// Ukázka - paralelní Merge Sort (koncept)
static void ParallelMergeSort(int[] pole, int left, int right)
{
    if (right - left < THRESHOLD)
    {
        // Malé pole - sekvenčně
        MergeSort(pole, left, right);
        return;
    }
    
    int mid = (left + right) / 2;
    
    // Paralelní zpracování obou polovin
    Parallel.Invoke(
        () => ParallelMergeSort(pole, left, mid),
        () => ParallelMergeSort(pole, mid + 1, right)
    );
    
    Merge(pole, left, mid, right);
}
```

---

## 🔗 Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 5 - Rekurze** | Merge Sort je klasický příklad rekurzivního algoritmu |
| **Ot. 7 - Složitost** | Analýza O(n log n) vs O(n²) |
| **Ot. 10 - Insert/Select Sort** | Porovnání jednoduchých O(n²) algoritmů |
| **Ot. 12 - Quick Sort** | Další Divide & Conquer algoritmus |
| **Ot. 15 - Rozděl a panuj** | Merge Sort jako ukázkový příklad paradigmatu |

---

## 📋 Procvičovací úlohy

### Doporučené úlohy k procvičení:

1. **Základní implementace:**
   - Implementuj Bubble Sort s optimalizací (early exit)
   - Implementuj Merge Sort a sleduj průběh rekurze

2. **Porovnání výkonu:**
   - Změř čas obou algoritmů pro n = 1000, 5000, 10000
   - Vykresli graf závislosti času na velikosti vstupu

3. **Variace Merge Sortu:**
   - Iterativní (bottom-up) Merge Sort bez rekurze
   - Merge Sort pro spojový seznam

4. **Praktické úlohy:**
   - Počítání inverzí v poli pomocí Merge Sortu
   - Třídění souborů (externí merge sort)

---

## 🎯 Quick Reference Card (pro opakování)

```
╔══════════════════════════════════════════════════════════════╗
║              BUBBLE SORT vs MERGE SORT                       ║
╠══════════════════════════════════════════════════════════════╣
║  BUBBLE SORT                 │  MERGE SORT                   ║
║  "Probublávání"              │  "Rozděl a panuj"             ║
║                              │                               ║
║  1. Porovnej sousedy         │  1. Rozděl na poloviny        ║
║  2. Prohoď pokud třeba       │  2. Rekurzivně setřiď         ║
║  3. Největší probublá        │  3. Sluč setříděné části      ║
║                              │                               ║
║  O(n²) worst                 │  O(n log n) VŽDY              ║
║  O(n) best (s optim.)        │  O(n log n) VŽDY              ║
║  O(1) paměť                  │  O(n) paměť                   ║
║  STABILNÍ                    │  STABILNÍ                     ║
║  IN-PLACE                    │  NENÍ IN-PLACE                ║
║                              │                               ║
║  Použití: výuka, malá data   │  Použití: velká data,         ║
║                              │  externí třídění              ║
╚══════════════════════════════════════════════════════════════╝
```

---

## 🔗 Externí zdroje

- **[Interaktivní vizualizace třídících algoritmů](https://visualgo.net/en/sorting)** - VisuAlgo
- **[Vizualizace Bubble Sort a Merge Sort (Gemini)](https://gemini.google.com/share/bd2adfe8828b)** - vizuální demonstrace

---

*📅 Vytvořeno: 2025-01-31 | 🎓 Maturitní příprava PRG 2025/2026*
