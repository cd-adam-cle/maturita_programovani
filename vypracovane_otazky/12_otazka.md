# 📚 Zápisky: Otázka č. 12 - Quick Sort

**Datum:** 2025-02-01  
**Status:** ✅ Hotovo  
**Obtížnost:** ⭐⭐⭐ (Vysoká - klíčový algoritmus!)

---

## ✅ Checklist bodů otázky

- [x] Bod 1: Motivace pro třídění dat
- [x] Bod 2: Popis algoritmu po jednotlivých krocích
- [x] Bod 3: Volba "dobrého" pivota
- [x] Bod 4: Využití v praxi
- [x] Bod 5: Znázornění na obrázku
- [x] Bod 6: Časová složitost
- [x] Bod 7: Paměťová složitost
- [x] Bod 8: Podobnost s algoritmem QuickSelect

---

## 🧠 Klíčové koncepty & Snippety

---

### BOD 1: Motivace pro třídění dat

**Proč třídíme?**
- **Rychlejší vyhledávání** - binární hledání O(log n) vs lineární O(n)
- **Efektivnější operace** - duplicity, průniky, sjednocení množin
- **Čitelnost** - setříděný výstup pro uživatele
- **Prerekvizita** - mnoho algoritmů vyžaduje setříděný vstup

**Proč Quick Sort?**
- V praxi **nejpoužívanější** třídící algoritmus
- `Array.Sort()` v C# ho interně používá (Introspective Sort)
- Průměrně **O(n log n)** jako Merge Sort, ale:
  - Třídí **in-place** (nepotřebuje extra paměť O(n))
  - Lepší **cache locality** → rychlejší v praxi

---

### BOD 2: Popis algoritmu po jednotlivých krocích

#### Hlavní myšlenka (Divide & Conquer)

```
1. VYBER PIVOT - jeden prvek z pole
2. PARTITION   - přesuň prvky:
                 • menší než pivot → VLEVO
                 • větší než pivot → VPRAVO
                 • pivot je na FINÁLNÍ pozici
3. REKURZE    - aplikuj na levou a pravou část
```

#### ASCII vizualizace principu

```
Pole: [3, 7, 2, 9, 1, 5, 4, 8, 6]
                    ↓
            Vyber pivot (např. 5)
                    ↓
    ┌───────────────┼───────────────┐
    ▼               ▼               ▼
[3, 2, 1, 4]       [5]       [7, 9, 8, 6]
  menší než 5    pivot=5     větší než 5
    ↓                               ↓
 (rekurze)                      (rekurze)
    ↓                               ↓
[1, 2, 3, 4]                  [6, 7, 8, 9]
                    ↓
        SPOJENO: [1, 2, 3, 4, 5, 6, 7, 8, 9]
```

#### Partition - Lomutův algoritmus (krok po kroku)

```
Pole: [3, 7, 2, 9, 1, 5]    Pivot = 5 (poslední prvek)

i = index pro "menší prvky" (začíná na -1, před polem)
j = aktuální procházený prvek

Krok 1: j=0, pole[0]=3, 3<5? ANO → i++, swap(i,j)
        i=0     → [3, 7, 2, 9, 1, 5]
                    ↑i

Krok 2: j=1, pole[1]=7, 7<5? NE → nic
        i=0     → [3, 7, 2, 9, 1, 5]
                    ↑i

Krok 3: j=2, pole[2]=2, 2<5? ANO → i++, swap(i,j)
        i=1     → [3, 2, 7, 9, 1, 5]  (swap 7↔2)
                       ↑i

Krok 4: j=3, pole[3]=9, 9<5? NE → nic
        i=1     → [3, 2, 7, 9, 1, 5]
                       ↑i

Krok 5: j=4, pole[4]=1, 1<5? ANO → i++, swap(i,j)
        i=2     → [3, 2, 1, 9, 7, 5]  (swap 7↔1)
                          ↑i

FINÁLE: swap pivot na pozici i+1
        → [3, 2, 1, 5, 7, 9]  (swap 9↔5)
                    ↑
               pivot na místě!
        
Výsledek: [3, 2, 1] < 5 < [7, 9]
          Vrátíme index 3 (pozice pivota)
```

#### Proč `i = low - 1`?

```
i = hranice mezi "menší" a "větší" prvky
Na začátku NEMÁME žádné menší prvky → hranice je PŘED polem

Index:   -1    0    1    2    3    4    5
              [3,   7,   2,   9,   1,   5]
          ↑i                             ↑pivot
          
"Zóna menších" je prázdná

Pokaždé když najdeme menší prvek:
→ i++ (rozšíříme zónu)
→ swap (dáme menší prvek do zóny)
```

#### Proč `Swap(i+1, high)` na konci?

```
Po for cyklu:        i               high
                     ↓                 ↓
Pole: [3, 2, 1,     9, 7,             5]
       \_____/      \___/             ↑
       menší        větší           PIVOT
       
i = 2 (poslední menší na indexu 2)
i+1 = 3 (sem patří pivot!)
high = 5 (pivot je zatím tady)

Swap(3, 5) → [3, 2, 1, 5, 7, 9]
                       ↑
                  pivot na místě!
```

---

### BOD 3: Volba "dobrého" pivota

#### Proč na volbě pivota záleží?

| Volba pivota | Výsledek | Složitost |
|--------------|----------|-----------|
| **Ideální** (medián) | Půlí pole na 2 stejné části | O(n log n) |
| **Špatná** (min/max) | Jedna část prázdná, druhá n-1 | O(n²) |

#### Strategie volby pivota

| Strategie | Popis | Složitost implementace |
|-----------|-------|------------------------|
| **Poslední prvek** | `pivot = pole[high]` | Jednoduchá ⭐ |
| **První prvek** | `pivot = pole[low]` | Jednoduchá ⭐ |
| **Prostřední prvek** | `pivot = pole[(low+high)/2]` | Jednoduchá ⭐ |
| **Median-of-three** | Medián z prvního, prostředního, posledního | Střední ⭐⭐ |
| **Random** | Náhodný prvek | Střední ⭐⭐ |

#### ⚠️ Problém s jednoduchými strategiemi

```
Už setříděné pole: [1, 2, 3, 4, 5, 6, 7, 8, 9]

Pivot = poslední = 9
Partition: [1,2,3,4,5,6,7,8] | 9 | []
                              ↑
                         všechno vlevo!

→ Místo log n úrovní rekurze máme n úrovní → O(n²)
```

#### Median-of-three (doporučená strategie)

```csharp
// Vyber medián z prvního, prostředního a posledního prvku
static int MedianOfThree(int[] pole, int low, int high)
{
    int mid = low + (high - low) / 2;
    
    // Seřadíme trojici: pole[low] <= pole[mid] <= pole[high]
    if (pole[low] > pole[mid])
        Swap(pole, low, mid);
    if (pole[low] > pole[high])
        Swap(pole, low, high);
    if (pole[mid] > pole[high])
        Swap(pole, mid, high);
    
    // Medián je uprostřed, přesuneme ho na konec pro partition
    Swap(pole, mid, high - 1);
    return pole[high - 1];
}
```

**Příklad:**
```
Pole: [8, 3, 1, 7, 0, 10, 2]
       ↑     ↑           ↑
      low   mid        high
      
Trojice: 8, 7, 2 → seřazeno: 2, 7, 8 → medián = 7
```

---

### BOD 4: Využití v praxi

#### Kde se Quick Sort používá?

| Použití | Příklad |
|---------|---------|
| **Standardní knihovny** | C# `Array.Sort()`, Java `Arrays.sort()`, C++ `std::sort()` |
| **Databáze** | Třídění výsledků dotazů (ORDER BY) |
| **Souborové systémy** | Třídění souborů podle jména/data |
| **Grafické aplikace** | Třídění objektů podle Z-indexu |

#### Introspective Sort (IntroSort)

Moderní implementace (včetně C#) kombinují:
```
IntroSort = QuickSort + HeapSort + InsertionSort

1. Začni QuickSortem
2. Pokud rekurze jde příliš hluboko → přepni na HeapSort (garantuje O(n log n))
3. Pro malé úseky (< 16 prvků) → použij InsertionSort (rychlejší pro malá pole)
```

#### Kdy Quick Sort NEPOUŽÍVAT?

| Situace | Proč | Alternativa |
|---------|------|-------------|
| Skoro setříděná data | Degraduje na O(n²) | InsertionSort O(n) |
| Potřeba stability | Quick Sort není stabilní | Merge Sort |
| Garantovaný O(n log n) | Nejhorší případ O(n²) | Heap Sort |
| Velmi malá pole | Overhead rekurze | InsertionSort |

---

### BOD 5: Znázornění na obrázku

#### Kompletní průběh Quick Sort

```
VSTUP: [6, 3, 8, 5, 2, 7, 4, 1]

═══════════════════════════════════════════════════════
ÚROVEŇ 0: Celé pole
═══════════════════════════════════════════════════════

[6, 3, 8, 5, 2, 7, 4, 1]    pivot = 1
         ↓ partition
[1] [6, 3, 8, 5, 2, 7, 4]   (1 je na místě)
 ↑
hotovo

═══════════════════════════════════════════════════════
ÚROVEŇ 1: Pravá část [6, 3, 8, 5, 2, 7, 4]
═══════════════════════════════════════════════════════

[6, 3, 8, 5, 2, 7, 4]       pivot = 4
         ↓ partition
[3, 2] [4] [6, 8, 5, 7]     (4 je na místě)
        ↑
      hotovo

═══════════════════════════════════════════════════════
ÚROVEŇ 2: Levá [3, 2] a Pravá [6, 8, 5, 7]
═══════════════════════════════════════════════════════

[3, 2]  pivot = 2           [6, 8, 5, 7]  pivot = 7
   ↓ partition                    ↓ partition
[2] [3]                     [6, 5] [7] [8]
 ↑   ↑                             ↑   ↑
hotovo                           hotovo hotovo

═══════════════════════════════════════════════════════
ÚROVEŇ 3: [6, 5]
═══════════════════════════════════════════════════════

[6, 5]  pivot = 5
   ↓ partition
[5] [6]
 ↑   ↑
hotovo

═══════════════════════════════════════════════════════
VÝSLEDEK: [1, 2, 3, 4, 5, 6, 7, 8]
═══════════════════════════════════════════════════════
```

#### Rekurzivní strom volání

```
                    QuickSort([6,3,8,5,2,7,4,1])
                              │
                    ┌─────────┴─────────┐
                    │                   │
              QS([])              QS([6,3,8,5,2,7,4])
              (prázdné)                 │
                              ┌─────────┴─────────┐
                              │                   │
                        QS([3,2])           QS([6,8,5,7])
                           │                      │
                      ┌────┴────┐            ┌────┴────┐
                      │         │            │         │
                   QS([])    QS([3])    QS([6,5])   QS([8])
                                              │
                                         ┌────┴────┐
                                         │         │
                                      QS([])    QS([6])
```

---

### BOD 6: Časová složitost

#### Přehled složitostí

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší** | O(n log n) | Pivot vždy medián → půlí pole |
| **Průměrný** | O(n log n) | Náhodná data |
| **Nejhorší** | O(n²) | Pivot vždy min/max (setříděné pole) |

#### Proč O(n log n) v průměru?

```
Partition = O(n) ... projdeme všechny prvky jednou

Pokud pivot půlí pole:
- Úroveň 0: 1× partition na n prvcích     = n
- Úroveň 1: 2× partition na n/2 prvcích   = n
- Úroveň 2: 4× partition na n/4 prvcích   = n
- ...
- Úroveň k: 2^k × partition na n/2^k      = n

Počet úrovní = log₂(n)

Celkem: n × log(n) = O(n log n)
```

```
      [████████████████]           n prvků
             ↓
    [████████] [████████]          n/2 + n/2 = n
         ↓          ↓
    [████] [████] [████] [████]    n/4 × 4 = n
       ↓              ↓
      ...            ...           log(n) úrovní
       ↓              ↓
      [█]            [█]           jednotlivé prvky
      
Každá úroveň = O(n) práce
Počet úrovní = O(log n)
CELKEM = O(n × log n)
```

#### Proč O(n²) v nejhorším případě?

```
Setříděné pole: [1, 2, 3, 4, 5]  pivot = poslední

Partition 1: [] [1] [2,3,4,5]     práce: 5
Partition 2: [] [2] [3,4,5]       práce: 4
Partition 3: [] [3] [4,5]         práce: 3
Partition 4: [] [4] [5]           práce: 2
Partition 5: [] [5] []            práce: 1

Celkem: 5+4+3+2+1 = n(n+1)/2 = O(n²)
```

---

### BOD 7: Paměťová složitost

#### Quick Sort třídí IN-PLACE

```
Nepotřebuje pomocné pole jako Merge Sort!
Pouze prohazuje prvky v původním poli.
```

#### Ale... rekurze zabírá paměť na CALL STACKU

| Případ | Hloubka rekurze | Paměť stacku |
|--------|-----------------|--------------|
| **Nejlepší/Průměrný** | O(log n) | O(log n) |
| **Nejhorší** | O(n) | O(n) |

```
Průměrný případ (pivot půlí pole):
- Hloubka rekurze = log(n)
- Stack: log(n) rámců

Nejhorší případ (pivot = min/max):
- Hloubka rekurze = n
- Stack: n rámců → možný STACK OVERFLOW!
```

#### Optimalizace - Tail Call Elimination

```csharp
// Optimalizovaná verze - vždy rekurze na MENŠÍ část
static void QuickSortOptimized(int[] pole, int low, int high)
{
    while (low < high)
    {
        int pivot = Partition(pole, low, high);
        
        // Rekurze na menší část, iterace na větší
        if (pivot - low < high - pivot)
        {
            QuickSortOptimized(pole, low, pivot - 1);
            low = pivot + 1;  // "Tail call" jako iterace
        }
        else
        {
            QuickSortOptimized(pole, pivot + 1, high);
            high = pivot - 1;
        }
    }
}
// Garantuje max O(log n) hloubku stacku!
```

#### Srovnání paměťové složitosti

| Algoritmus | Paměťová složitost | Poznámka |
|------------|-------------------|----------|
| Quick Sort | O(log n) průměr | In-place, jen stack |
| Merge Sort | O(n) | Potřebuje pomocné pole |
| Heap Sort | O(1) | Skutečně in-place |
| Bubble Sort | O(1) | In-place |

---

### BOD 8: Podobnost s algoritmem QuickSelect

#### Co je QuickSelect?

**Úloha:** Najdi **k-tý nejmenší** prvek v nesetříděném poli.

```
Pole: [3, 7, 2, 9, 1, 5]
Najdi 3. nejmenší prvek (k=3)

Odpověď: 3 (setříděně by bylo [1, 2, 3, 5, 7, 9])
```

#### Podobnost s Quick Sort

| | Quick Sort | QuickSelect |
|-|------------|-------------|
| **Cíl** | Setřídit celé pole | Najít k-tý prvek |
| **Partition** | ✅ Stejná | ✅ Stejná |
| **Rekurze** | Na OBĚ části | Jen na JEDNU část |
| **Složitost** | O(n log n) | O(n) průměr |

#### Jak QuickSelect funguje?

```
Pole: [3, 7, 2, 9, 1, 5]    Hledáme k=3 (3. nejmenší)

1. Partition s pivot=5:
   [3, 2, 1] [5] [7, 9]
   pozice:  0,1,2  3   4,5
   
   Pivot je na pozici 3 (je to 4. nejmenší)
   Hledáme 3. nejmenší → musí být VLEVO

2. Rekurze jen na [3, 2, 1], pivot=1:
   [] [1] [3, 2]
   
   Pivot na pozici 0 (je to 1. nejmenší)
   Hledáme 3. → musí být VPRAVO

3. Rekurze na [3, 2], pivot=2:
   [] [2] [3]
   
   Pivot na pozici 1 (je to 2. nejmenší)
   Hledáme 3. → musí být VPRAVO
   
4. "Rekurze" na [3]:
   Jediný prvek → to je náš 3. nejmenší!

ODPOVĚĎ: 3
```

#### Implementace QuickSelect

```csharp
// ✅ VERZE A - MATURITNÍ
// QuickSelect - najde k-tý nejmenší prvek

static int QuickSelect(int[] pole, int left, int right, int k)
{
    // Jeden prvek = odpověď
    if (left == right)
        return pole[left];
    
    // Partition - stejná jako u QuickSort!
    int pivotIndex = Partition(pole, left, right);
    
    // Kolikátý nejmenší je pivot?
    int pivotRank = pivotIndex - left + 1;
    
    if (k == pivotRank)
    {
        // Našli jsme!
        return pole[pivotIndex];
    }
    else if (k < pivotRank)
    {
        // Hledáme VLEVO
        return QuickSelect(pole, left, pivotIndex - 1, k);
    }
    else
    {
        // Hledáme VPRAVO (upravíme k)
        return QuickSelect(pole, pivotIndex + 1, right, k - pivotRank);
    }
}

// Použití:
int[] cisla = { 3, 7, 2, 9, 1, 5 };
int tretiNejmensi = QuickSelect(cisla, 0, cisla.Length - 1, 3);
// Výsledek: 3
```

#### Proč je QuickSelect O(n)?

```
Quick Sort: rekurze na obě části
  n + n/2 + n/2 + n/4 + n/4 + n/4 + n/4 + ... = n × log(n)

QuickSelect: rekurze jen na JEDNU část
  n + n/2 + n/4 + n/8 + ... = 2n = O(n)
  
(geometrická řada se součtem 2n)
```

---

## 💻 Kompletní implementace

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// Jednoduchý QuickSort s Lomutovým partition

static void QuickSort(int[] pole, int leva, int prava)
{
    // Základní podmínka: pokud má úsek 0 nebo 1 prvek, konec
    if (leva >= prava)
        return;
    
    // Partition - rozděl pole a získej pozici pivota
    int pivotIndex = Partition(pole, leva, prava);
    
    // Rekurzivně setřiď levou a pravou část
    QuickSort(pole, leva, pivotIndex - 1);   // levá část
    QuickSort(pole, pivotIndex + 1, prava);  // pravá část
}

static int Partition(int[] pole, int leva, int prava)
{
    // Pivot = poslední prvek (jednoduchá volba)
    int pivot = pole[prava];
    
    // i = hranice mezi "menší" a "větší" prvky
    // Začínáme před polem (žádný menší prvek zatím)
    int i = leva - 1;
    
    // Projdi všechny prvky (kromě pivota)
    for (int j = leva; j < prava; j++)
    {
        // Pokud je prvek menší než pivot
        if (pole[j] < pivot)
        {
            i++;  // Posuň hranici
            // Prohoď prvky
            int temp = pole[i];
            pole[i] = pole[j];
            pole[j] = temp;
        }
    }
    
    // Dej pivot na správné místo (za všechny menší)
    int tmp = pole[i + 1];
    pole[i + 1] = pole[prava];
    pole[prava] = tmp;
    
    // Vrať pozici pivota
    return i + 1;
}

// Hlavní program
static void Main()
{
    int[] cisla = { 6, 3, 8, 5, 2, 7, 4, 1 };
    
    Console.WriteLine("Před tříděním: " + string.Join(", ", cisla));
    
    QuickSort(cisla, 0, cisla.Length - 1);
    
    Console.WriteLine("Po třídění:    " + string.Join(", ", cisla));
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// Čistší kód + Median-of-three + Swap helper

static void QuickSortSenior(int[] pole) 
    => QuickSortRecursive(pole, 0, pole.Length - 1);

static void QuickSortRecursive(int[] pole, int low, int high)
{
    if (low < high)
    {
        int pivotIndex = PartitionMedian(pole, low, high);
        QuickSortRecursive(pole, low, pivotIndex - 1);
        QuickSortRecursive(pole, pivotIndex + 1, high);
    }
}

static int PartitionMedian(int[] pole, int low, int high)
{
    // Median-of-three pro lepší volbu pivota
    int mid = low + (high - low) / 2;
    if (pole[low] > pole[mid]) Swap(pole, low, mid);
    if (pole[low] > pole[high]) Swap(pole, low, high);
    if (pole[mid] > pole[high]) Swap(pole, mid, high);
    Swap(pole, mid, high);  // Pivot na konec
    
    int pivot = pole[high];
    int i = low - 1;
    
    for (int j = low; j < high; j++)
    {
        if (pole[j] < pivot)
        {
            i++;
            Swap(pole, i, j);
        }
    }
    
    Swap(pole, i + 1, high);
    return i + 1;
}

static void Swap(int[] pole, int a, int b) 
    => (pole[a], pole[b]) = (pole[b], pole[a]);
```

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

### 1. Hranice rekurze
```csharp
// ❌ ŠPATNĚ - chybí základní podmínka
static void QuickSort(int[] pole, int l, int r)
{
    int p = Partition(pole, l, r);  // CRASH pro prázdné pole!
    QuickSort(pole, l, p - 1);
    QuickSort(pole, p + 1, r);
}

// ✅ SPRÁVNĚ
static void QuickSort(int[] pole, int l, int r)
{
    if (l >= r) return;  // Základní podmínka!
    int p = Partition(pole, l, r);
    QuickSort(pole, l, p - 1);
    QuickSort(pole, p + 1, r);
}
```

### 2. Index `i` začíná na `low - 1`, ne na `low`
```csharp
// ❌ ŠPATNĚ
int i = low;  // Přeskočí první prvek!

// ✅ SPRÁVNĚ
int i = low - 1;  // Zóna menších je na začátku prázdná
```

### 3. Partition cyklus jde do `j < high`, ne `j <= high`
```csharp
// ❌ ŠPATNĚ - zahrnuje pivot do cyklu
for (int j = low; j <= high; j++)

// ✅ SPRÁVNĚ - pivot je na indexu high, nezahrnujeme ho
for (int j = low; j < high; j++)
```

### 4. Nejhorší případ pro setříděná data
```
❓ "Jaká je složitost Quick Sortu pro setříděné pole?"
✅ O(n²) - pivot je vždy min/max!

Řešení: Median-of-three nebo náhodný pivot
```

### 5. Quick Sort NENÍ stabilní
```
❓ "Je Quick Sort stabilní?"
✅ NE - stejné prvky mohou změnit pořadí

Stabilní = prvky se stejnou hodnotou zůstanou ve stejném pořadí
```

---

## 🚀 Senior Tipy

### 1. V praxi se používá IntroSort
```
IntroSort = QuickSort + HeapSort + InsertionSort
- Začne QuickSortem
- Hloubka > 2×log(n) → přepne na HeapSort
- Malé úseky (< 16) → InsertionSort
```

### 2. 3-Way Partition pro duplicity
```csharp
// Když máš hodně duplicitních hodnot:
// [ < pivot | == pivot | > pivot ]
// Sníží složitost pro pole s duplicitami
```

### 3. Tail Call Optimization
```csharp
// Rekurze vždy na menší část → max O(log n) stack
while (low < high)
{
    int p = Partition(pole, low, high);
    if (p - low < high - p)
    {
        QuickSort(pole, low, p - 1);
        low = p + 1;
    }
    else
    {
        QuickSort(pole, p + 1, high);
        high = p - 1;
    }
}
```

### 4. QuickSelect pro k-tý prvek
```
Potřebuješ najít medián nebo k-tý nejmenší?
→ QuickSelect v O(n), ne O(n log n)!
```

---

## 🔗 Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 5 - Rekurze** | Quick Sort je rekurzivní algoritmus |
| **Ot. 7 - Složitost** | Analýza O(n log n) vs O(n²) |
| **Ot. 10-11 - Třídění** | Srovnání s jinými algoritmy |
| **Ot. 13 - Heap Sort** | Alternativa s garantovaným O(n log n) |
| **Ot. 15 - Divide & Conquer** | Quick Sort je ukázka D&C |

---

## 📊 Srovnání třídících algoritmů

| Algoritmus | Průměr | Nejhorší | Paměť | Stabilní |
|------------|--------|----------|-------|----------|
| **Quick Sort** | O(n log n) | O(n²) | O(log n) | ❌ |
| Merge Sort | O(n log n) | O(n log n) | O(n) | ✅ |
| Heap Sort | O(n log n) | O(n log n) | O(1) | ❌ |
| Bubble Sort | O(n²) | O(n²) | O(1) | ✅ |
| Insert Sort | O(n²) | O(n²) | O(1) | ✅ |

**Quick Sort je nejrychlejší v praxi** díky cache locality, i když teoreticky má horší worst-case než Merge/Heap Sort.

---

## 🎯 Quick Reference pro maturitu

```
QUICK SORT = Divide & Conquer třídící algoritmus

POSTUP:
1. Vyber PIVOT
2. PARTITION: menší vlevo, větší vpravo
3. REKURZE na obě části

PARTITION (Lomuto):
- i = low - 1 (hranice menších, začíná prázdná)
- Pro každý prvek < pivot: i++, swap(i, j)
- Na konci: swap(i+1, high) - pivot na místo

SLOŽITOST:
- Čas: O(n log n) průměr, O(n²) worst
- Paměť: O(log n) - stack rekurze

PIVOT STRATEGIE:
- Jednoduchá: poslední prvek
- Lepší: median-of-three

QUICKSELECT:
- Najde k-tý nejmenší v O(n)
- Jako QuickSort, ale rekurze jen na jednu stranu
```

---

*Zpracováno: 1. února 2025*
