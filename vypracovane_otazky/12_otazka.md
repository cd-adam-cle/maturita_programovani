# Zápisky: Otázka č. 12 - Quick Sort

---

## Obsah

1. [Motivace pro třídění](#1-motivace-pro-třídění)
2. [Princip a popis algoritmu](#2-princip-a-popis-algoritmu)
3. [Operace Partition](#3-operace-partition)
4. [Volba pivota](#4-volba-pivota)
5. [Vizualizace](#5-vizualizace)
6. [Časová složitost](#6-časová-složitost)
7. [Paměťová složitost](#7-paměťová-složitost)
8. [Optimalizace Quick Sortu](#8-optimalizace-quick-sortu)
9. [QuickSelect - hledání k-tého prvku](#9-quickselect---hledání-k-tého-prvku)
10. [Quick Sort v praxi (IntroSort)](#10-quick-sort-v-praxi-introsort)
11. [Porovnání s ostatními algoritmy](#11-porovnání-s-ostatními-algoritmy)
12. [Maturitní chytáky](#12-maturitní-chytáky)
13. [Klíčové pojmy](#13-klíčové-pojmy)

---

## 1. Motivace pro třídění

### Proč třídíme

Třídění je jedna z nejzákladnějších operací v informatice. Setříděná data umožňují rychlejší vyhledávání (binární vyhledávání O(log n)), efektivní detekci duplicit, group-by operace, agregace, slévání datových toků a další. Mnoho algoritmů vyžaduje setříděný vstup jako prerekvizitu (např. Kruskalův MST, řadicí binární strom, k-way merge).

### Proč Quick Sort

Quick Sort je v praxi **nejpoužívanější** porovnávací třídicí algoritmus. Vymyslel ho **Tony Hoare** v roce 1961. Jeho přednost:

- **Průměrná složitost O(n log n)** - stejně jako Merge Sort nebo Heap Sort.
- **In-place** - na rozdíl od Merge Sortu nepotřebuje O(n) extra paměť.
- **Velmi malá konstanta** - díky výborné **cache lokality** je v praxi rychlejší než Merge Sort i Heap Sort.
- **Snadná implementace** - jednodušší než Merge Sort, nepotřebuje pomocné pole.
- **Hojné využití v knihovnách** - `Array.Sort()` v .NET (přes IntroSort), `std::sort()` v C++, `qsort()` v C, Java `Arrays.sort()` pro primitivní typy.

### Stinné stránky

- **Worst case O(n²)** - patologický případ při špatném výběru pivota (např. setříděné pole + první/poslední pivot).
- **Není stabilní** - prvky se stejným klíčem mohou změnit relativní pořadí.
- **Rekurze** - hluboký zásobník v nejhorším případě (až O(n)).

---

## 2. Princip a popis algoritmu

### Paradigma Divide & Conquer

Quick Sort patří mezi algoritmy typu **Rozděl a panuj** (viz Ot. 11 a Ot. 15). Jeho kroky:

1. **Vyber pivot** - jeden prvek z aktuálního úseku pole, který bude tvořit "rozdělovací hranici".
2. **Partition** - přeskupit prvky tak, aby:
   - vlevo od pivota byly všechny prvky `≤ pivot`,
   - vpravo od pivota byly všechny prvky `> pivot`,
   - pivot byl na své **finální** pozici v setříděném poli.
3. **Rekurze** - aplikuj algoritmus rekurzivně na levou a pravou část (bez pivota).

### Klíčový rozdíl od Merge Sortu

| Aspekt | Merge Sort | Quick Sort |
|--------|-----------|-----------|
| Hlavní práce probíhá v... | **Kombinaci** (MERGE) | **Rozdělení** (PARTITION) |
| Předtřídění | Rekurze nejprve, MERGE až poté | PARTITION nejprve, rekurze poté |
| Pomocná paměť | O(n) - pomocná pole | O(log n) - zásobník |
| Stabilita | Ano | Ne |
| Worst case | Stále O(n log n) | O(n²) |

### Pseudokód

```
QUICK_SORT(pole, left, right):
   if left >= right: return                       // 0 nebo 1 prvek
   p = PARTITION(pole, left, right)               // p = pozice pivota
   QUICK_SORT(pole, left, p - 1)                  // setřiď levou část
   QUICK_SORT(pole, p + 1, right)                 // setřiď pravou část
```

Všimněte si, že po PARTITION je pivot na pozici `p` na svém **finálním** místě - není potřeba ho zahrnout do rekurzivních volání.

### Základní implementace

```csharp
static void QuickSort(int[] pole, int left, int right)
{
    if (left >= right) return;                     // základní případ

    int pivotIndex = Partition(pole, left, right); // rozdělení

    QuickSort(pole, left, pivotIndex - 1);          // rekurze - levá část
    QuickSort(pole, pivotIndex + 1, right);         // rekurze - pravá část
}

// Volání: QuickSort(pole, 0, pole.Length - 1);
```

### Generická varianta

```csharp
static void QuickSort<T>(T[] pole, int left, int right, IComparer<T> cmp = null)
    where T : IComparable<T>
{
    cmp ??= Comparer<T>.Default;
    if (left >= right) return;
    int p = Partition(pole, left, right, cmp);
    QuickSort(pole, left, p - 1, cmp);
    QuickSort(pole, p + 1, right, cmp);
}
```

---

## 3. Operace Partition

PARTITION je srdcem Quick Sortu. Existují dvě klasické varianty: **Lomuto** (jednodušší) a **Hoare** (původní, efektivnější).

### Lomutův partition (didakticky vhodnější)

```csharp
static int Partition(int[] pole, int left, int right)
{
    int pivot = pole[right];                       // pivot = poslední prvek
    int i = left - 1;                              // hranice "menší zóny"

    for (int j = left; j < right; j++)
    {
        if (pole[j] < pivot)                       // patří do levé části
        {
            i++;
            (pole[i], pole[j]) = (pole[j], pole[i]);
        }
    }

    (pole[i + 1], pole[right]) = (pole[right], pole[i + 1]);   // pivot na místo
    return i + 1;
}
```

**Invariant:**

```
+----------+----------+----------+----------+
|  < pivot |  ≥ pivot |  ? ? ? ? |  pivot   |
+----------+----------+----------+----------+
^          ^          ^                     ^
left       i+1        j                     right
```

- Prvky v `pole[left..i]` jsou `< pivot`.
- Prvky v `pole[i+1..j-1]` jsou `≥ pivot`.
- Prvky v `pole[j..right-1]` ještě nejsou zpracované.
- `pole[right]` je pivot.

Po dokončení cyklu jsou všechny prvky zpracované a poslední swap dává pivot na pozici `i+1`.

### Hoareův partition (původní algoritmus)

```csharp
static int PartitionHoare(int[] pole, int left, int right)
{
    int pivot = pole[left + (right - left) / 2];   // pivot = prostřední
    int i = left - 1;
    int j = right + 1;

    while (true)
    {
        do { i++; } while (pole[i] < pivot);
        do { j--; } while (pole[j] > pivot);
        if (i >= j) return j;
        (pole[i], pole[j]) = (pole[j], pole[i]);
    }
}
```

**Pozor:** Hoareův partition vrací hranici mezi částmi, ne pozici pivota. Rekurzivní volání pak je:

```csharp
QuickSortHoare(pole, left, p);                     // pozor: p, ne p-1
QuickSortHoare(pole, p + 1, right);
```

**Výhody Hoarea:**
- Méně swapů v průměru (cca 3× méně).
- Méně náchylný na patologické případy.

**Nevýhody Hoarea:**
- Pivot není garantovaně na své finální pozici.
- Méně intuitivní.

V praxi se používá obvykle Lomuto nebo modifikované verze (3-way partition pro duplicity).

### Krok-za-krokem trace Lomuto pro `[3, 7, 2, 9, 1, 5]`, pivot = 5

```
Inicializace: i = -1, pivot = 5

j=0: pole[0]=3, 3<5 → i=0, swap(0,0)
     [3, 7, 2, 9, 1, 5]  i=0
      ^

j=1: pole[1]=7, 7<5? NE
     [3, 7, 2, 9, 1, 5]  i=0

j=2: pole[2]=2, 2<5 → i=1, swap(1,2)
     [3, 2, 7, 9, 1, 5]  i=1
         ^

j=3: pole[3]=9, 9<5? NE
     [3, 2, 7, 9, 1, 5]  i=1

j=4: pole[4]=1, 1<5 → i=2, swap(2,4)
     [3, 2, 1, 9, 7, 5]  i=2
            ^

Konec cyklu. Pivot na pozici i+1 = 3:
swap(3, 5): [3, 2, 1, 5, 7, 9]
                     ^
            pivot na finální pozici

Vrátí: 3
```

Výsledek: `[3, 2, 1] < 5 < [7, 9]`. Pivot (5) je trvale na indexu 3.

---

## 4. Volba pivota

### Proč na volbě pivota záleží

Volba pivota určuje, jak rovnoměrně se pole rozdělí - a tím přímo ovlivňuje hloubku rekurze a celkovou složitost.

| Volba | Rozdělení | Hloubka rekurze | Složitost |
|-------|-----------|-----------------|-----------|
| **Ideální** (medián) | n/2 + n/2 | log n | O(n log n) |
| **Dobrá** | ~n/4 + ~3n/4 | log n (s vyšší konst.) | O(n log n) |
| **Špatná** (min/max) | 0 + (n-1) | n | O(n²) |

### Strategie volby pivota

#### 1. První / poslední prvek

```csharp
int pivot = pole[left];                            // první prvek
int pivot = pole[right];                           // poslední prvek
```

**Problém:** Setříděné nebo zpětně setříděné pole způsobí worst case O(n²). V praxi je to extrémně častý patologický případ (např. dotaz na databázi, který už vrátil setříděná data).

#### 2. Prostřední prvek

```csharp
int pivot = pole[left + (right - left) / 2];
```

**Výhoda:** Pro setříděná pole funguje výborně.
**Nevýhoda:** Existují (uměle vytvořené) sekvence, které tuto strategii zlomí na O(n²).

#### 3. Náhodný pivot (Randomized Quick Sort)

```csharp
Random rng = new Random();
int pivotIndex = rng.Next(left, right + 1);
(pole[pivotIndex], pole[right]) = (pole[right], pole[pivotIndex]);
int pivot = pole[right];
```

**Výhoda:** Žádný konkrétní vstup nemůže být patologický (útočník neumí předpovědět pivot).
**Garance:** Očekávaná složitost O(n log n), bez ohledu na vstup.

#### 4. Median-of-three

Vyber medián z prvního, prostředního a posledního prvku:

```csharp
static int MedianOfThree(int[] pole, int low, int high)
{
    int mid = low + (high - low) / 2;

    if (pole[low] > pole[mid])  (pole[low], pole[mid])  = (pole[mid], pole[low]);
    if (pole[low] > pole[high]) (pole[low], pole[high]) = (pole[high], pole[low]);
    if (pole[mid] > pole[high]) (pole[mid], pole[high]) = (pole[high], pole[mid]);

    (pole[mid], pole[high - 1]) = (pole[high - 1], pole[mid]);  // pivot na předposlední
    return pole[high - 1];
}
```

**Výhody:**
- Eliminuje worst case pro setříděné a zpětně setříděné pole.
- Pivot je s velkou pravděpodobností "rozumný" prostřední prvek.
- Žádné náhodné číslo - deterministický.

#### 5. Median-of-medians (BFPRT)

Algoritmus zaručující skutečný medián v lineárním čase O(n). Použití pro Quick Sort by zaručilo worst case O(n log n), ale konstanta je obrovská, takže v praxi se nepoužívá. Důležitý pro QuickSelect s garantovaným O(n).

---

## 5. Vizualizace

### Kompletní průchod pro `[6, 3, 8, 5, 2, 7, 4, 1]`

```
Vstup: [6, 3, 8, 5, 2, 7, 4, 1]

ÚROVEŇ 0: celé pole, pivot = 1 (poslední)
─────────────────────────────────────────────────
Po partition: [1] | 1 nemá menší než sebe | [6, 3, 8, 5, 2, 7, 4]
             pozice 0 (pivot)

ÚROVEŇ 1: pravá část [6, 3, 8, 5, 2, 7, 4], pivot = 4
─────────────────────────────────────────────────
Po partition: [3, 2] | [4] | [6, 8, 5, 7]
                       pozice 3 (pivot)

ÚROVEŇ 2a: levá [3, 2], pivot = 2
─────────────────────────────────────────────────
Po partition: [] | [2] | [3]

ÚROVEŇ 2b: pravá [6, 8, 5, 7], pivot = 7
─────────────────────────────────────────────────
Po partition: [6, 5] | [7] | [8]

ÚROVEŇ 3: [6, 5], pivot = 5
─────────────────────────────────────────────────
Po partition: [] | [5] | [6]

VÝSLEDEK: [1, 2, 3, 4, 5, 6, 7, 8]
```

### Rekurzivní strom

```
              QuickSort([6,3,8,5,2,7,4,1])
                        |
              +---------+---------+
              |                   |
            QS([])         QS([6,3,8,5,2,7,4])
            prazdne                |
                          +--------+--------+
                          |                 |
                       QS([3,2])      QS([6,8,5,7])
                          |                 |
                       +--+--+           +--+--+
                       |     |           |     |
                     QS([]) QS([3])   QS([6,5]) QS([8])
                                         |
                                      +--+--+
                                      |     |
                                    QS([]) QS([6])
```

### Best vs worst case strom

```
BEST CASE (pivot vždy medián):              WORST CASE (pivot vždy min):

       [8]                                  [8]
      /   \                                /   \
   [4]     [4]                          [1]    [7]
   / \     / \                                 / \
 [2] [2] [2] [2]                            [1]   [6]
                                                  / \
hloubka log n,                                 [1]   [5]
celkem n log n                                       / \
                                                  ...
                                                  hloubka n,
                                                  celkem n²
```

---

## 6. Časová složitost

### Přehled

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší** | O(n log n) | Pivot vždy medián (dělí přesně na poloviny) |
| **Průměrný** | O(n log n) | Náhodná data; očekávaná hodnota |
| **Nejhorší** | O(n²) | Pivot vždy extrém (min nebo max) |

### Odvození průměrné složitosti

Rekurence pro průměrný případ (předpokládáme, že pozice pivota je rovnoměrně rozdělená v [0, n-1]):

```
T(n) = (1/n) · Σ (T(k) + T(n-1-k)) + Θ(n)
       └───────────┬───────────┘     └─┬─┘
        průměr přes všechny pozice    PARTITION

Řešením je T(n) = Θ(n log n).
```

Pro **best case** (pivot dělí přesně na poloviny) máme rekurenci:

```
T(n) = 2 · T(n/2) + Θ(n)     →     T(n) = Θ(n log n)
```

(Stejná jako Merge Sort, viz Master Theorem.)

### Odvození worst case

Pro setříděné pole a pivot = poslední prvek:

```
T(n) = T(n-1) + T(0) + Θ(n)
     = T(n-1) + Θ(n)
     = Θ(n) + Θ(n-1) + ... + Θ(1)
     = Θ(n²)
```

Patologické vstupy pro různé strategie:

| Strategie | Patologický vstup |
|-----------|-------------------|
| `pole[right]` | Setříděné pole |
| `pole[left]` | Setříděné pole |
| `pole[mid]` | Speciálně sestavená sekvence (existuje, neintuitivní) |
| `median-of-3` | Speciálně sestavená sekvence (těžko sestavit) |
| Random | Žádný (s vysokou pravděpodobností) |

### Proč v praxi rychlejší než Merge Sort?

I když mají stejnou asymptotickou složitost O(n log n), **Quick Sort má menší konstantu**:

1. **Cache lokalita** - PARTITION pracuje sekvenčně, nevyžaduje skoky v paměti.
2. **In-place** - žádné alokace pomocných polí (Merge Sort alokuje O(n) pomocné paměti).
3. **Méně paměťových přístupů** - každý prvek se v průměru přesune jen 1-2×.

Benchmarky ukazují, že Quick Sort je v praxi 2-3× rychlejší než Merge Sort pro běžná data v RAM.

---

## 7. Paměťová složitost

### In-place algoritmus

Quick Sort **nepotřebuje žádné pomocné pole**. Třídění probíhá výhradně přes swapy v původním poli.

### Paměť na zásobníku rekurze

| Případ | Hloubka rekurze | Paměť |
|--------|-----------------|-------|
| Best / Average | O(log n) | O(log n) |
| Worst | O(n) | O(n) - **StackOverflow** pro velká n! |

Pro `n = 10⁶` v nejhorším případě je hloubka rekurze 1 milion volání - to typicky překročí limit zásobníku (~1 MB) a způsobí pád programu.

### Tail call optimization

Standardní rekurze rekurzivně volá obě části:

```csharp
QuickSort(pole, left, pivot - 1);
QuickSort(pole, pivot + 1, right);                  // tail position
```

Druhé volání je v **tail position** - po něm už nic není. Můžeme ho převést na iteraci pomocí smyčky `while`. To samo ale nestačí - musíme rekurzivně volat vždy na **menší** část, abychom zaručili O(log n) hloubku rekurze:

```csharp
static void QuickSortOptimized(int[] pole, int low, int high)
{
    while (low < high)
    {
        int pivot = Partition(pole, low, high);

        if (pivot - low < high - pivot)             // menší část = levá
        {
            QuickSortOptimized(pole, low, pivot - 1);
            low = pivot + 1;                         // iteruj na pravou
        }
        else                                          // menší část = pravá
        {
            QuickSortOptimized(pole, pivot + 1, high);
            high = pivot - 1;                        // iteruj na levou
        }
    }
}
```

**Důsledek:** garantovaná hloubka rekurze O(log n) i v nejhorším případě. Worst case času zůstává O(n²), ale eliminujeme riziko StackOverflow.

---

## 8. Optimalizace Quick Sortu

### 1. Median-of-three pivot (viz Bod 4)

### 2. Tail call optimization (viz Bod 7)

### 3. 3-Way Partition (Dutch National Flag)

Klasický Lomuto má problém s **mnoha duplicitními hodnotami** - prvky rovné pivotu se nepřesouvají efektivně, vzniká nevyvážené rozdělení. **3-way partition** rozdělí pole na tři části:

```
+------------+------------+------------+
|  < pivot   |  = pivot   |  > pivot   |
+------------+------------+------------+
```

Implementace (Dijkstra):

```csharp
static (int lt, int gt) Partition3Way(int[] pole, int left, int right)
{
    int pivot = pole[left];
    int lt = left, i = left + 1, gt = right;

    while (i <= gt)
    {
        if (pole[i] < pivot)
        {
            (pole[lt], pole[i]) = (pole[i], pole[lt]);
            lt++; i++;
        }
        else if (pole[i] > pivot)
        {
            (pole[i], pole[gt]) = (pole[gt], pole[i]);
            gt--;
        }
        else i++;
    }

    return (lt, gt);
}

static void QuickSort3Way(int[] pole, int left, int right)
{
    if (left >= right) return;
    var (lt, gt) = Partition3Way(pole, left, right);
    QuickSort3Way(pole, left, lt - 1);
    QuickSort3Way(pole, gt + 1, right);
}
```

**Výhoda:** Pro pole s `k` distinct hodnotami běží v O(n log k) místo O(n log n). Pro pole pouze s několika unikátními hodnotami (např. true/false) běží v O(n).

### 4. Insertion sort pro malá podpole

Pro malá pole (cca `< 16` prvků) má Insertion Sort menší konstantu kvůli režii rekurze a vyhodnocování pivota. Hybridní algoritmy přepínají:

```csharp
const int INSERTION_THRESHOLD = 16;

static void HybridQuickSort(int[] pole, int low, int high)
{
    while (low < high)
    {
        if (high - low + 1 < INSERTION_THRESHOLD)
        {
            InsertionSort(pole, low, high);          // pro malé úseky
            return;
        }

        int p = Partition(pole, low, high);

        if (p - low < high - p)
        {
            HybridQuickSort(pole, low, p - 1);
            low = p + 1;
        }
        else
        {
            HybridQuickSort(pole, p + 1, high);
            high = p - 1;
        }
    }
}
```

### 5. Paralelní Quick Sort

Stejně jako Merge Sort se Quick Sort dá výborně paralelizovat - obě části po partition jsou nezávislé:

```csharp
static void ParallelQuickSort(int[] pole, int low, int high, int depth = 0)
{
    if (low >= high) return;
    int p = Partition(pole, low, high);

    if (depth < 4 && (high - low) > 4096)
    {
        Parallel.Invoke(
            () => ParallelQuickSort(pole, low, p - 1, depth + 1),
            () => ParallelQuickSort(pole, p + 1, high, depth + 1)
        );
    }
    else
    {
        ParallelQuickSort(pole, low, p - 1, depth);
        ParallelQuickSort(pole, p + 1, high, depth);
    }
}
```

---

## 9. QuickSelect - hledání k-tého prvku

### Úloha

Najdi `k`-tý nejmenší prvek v nesetříděném poli. Příklady:

- **Medián** - k = n/2.
- **Top-K** - prvních k největších (k-tý + všechno větší).
- **Percentily** - 95. percentil dotazování na výkon.

### Naivní řešení

1. Setřiď pole → O(n log n).
2. Vrať `pole[k-1]` → O(1).

Celkem O(n log n), zbytečně setřídíme celé pole.

### QuickSelect (Hoare)

QuickSelect je modifikace Quick Sortu: místo abychom rekurzivně třídili obě části, **rekurzujeme jen do té části, kde je hledaný k-tý prvek**.

```csharp
static int QuickSelect(int[] pole, int left, int right, int k)
{
    if (left == right) return pole[left];

    int pivotIndex = Partition(pole, left, right);
    int rank = pivotIndex - left + 1;              // pořadí pivota v aktuálním úseku

    if (k == rank)
        return pole[pivotIndex];                    // našli jsme
    else if (k < rank)
        return QuickSelect(pole, left, pivotIndex - 1, k);
    else
        return QuickSelect(pole, pivotIndex + 1, right, k - rank);
}
```

### Analýza složitosti

V průměrném případě:

```
T(n) = T(n/2) + Θ(n)     (jen JEDNA rekurze místo dvou)
     = Θ(n) + Θ(n/2) + Θ(n/4) + ... + Θ(1)
     = Θ(2n) = Θ(n)
```

Geometrická řada dává **lineární čas O(n)**. To je překvapivý výsledek - umíme najít k-tý nejmenší prvek v lineárním čase, aniž bychom celé pole třídili.

Worst case zůstává O(n²) (stejné patologické případy jako Quick Sort). Pomocí **Median-of-medians** lze garantovat worst case O(n) - viz BFPRT algoritmus.

### Příklad

```
Pole: [3, 7, 2, 9, 1, 5]    hledáme 3. nejmenší (k = 3)

Krok 1: Partition s pivot = 5
   Výsledek: [3, 2, 1, 5, 7, 9]
            pivot na indexu 3, rank = 4
   k = 3 < rank = 4 → rekurze na levou část [3, 2, 1]

Krok 2: Partition na [3, 2, 1] s pivot = 1
   Výsledek: [1, 3, 2]
            pivot na indexu 0, rank = 1
   k = 3 > rank = 1 → rekurze na pravou s k' = 3 - 1 = 2

Krok 3: Partition na [3, 2] s pivot = 2
   Výsledek: [2, 3]
            pivot na indexu 0, rank = 1
   k = 2 > rank = 1 → rekurze na pravou s k' = 1

Krok 4: Jediný prvek [3] → vrať 3

Odpověď: 3 (3. nejmenší)
```

### Použití v .NET

C# nemá built-in QuickSelect, ale lze implementovat snadno nebo použít `OrderBy().Skip(k-1).First()` (pomalejší O(n log n)).

---

## 10. Quick Sort v praxi (IntroSort)

### IntroSort = QuickSort + HeapSort + InsertionSort

Moderní implementace v knihovnách (C++ `std::sort`, .NET `Array.Sort`) používají **IntroSort** (Introspective Sort), který kombinuje tři algoritmy:

1. **Začíná QuickSortem** - nejrychlejší v průměru.
2. **Sleduje hloubku rekurze.** Pokud překročí `2 · log₂(n)`, **přepne na HeapSort** - to zaručuje worst case O(n log n) a eliminuje patologické případy.
3. **Pro malé úseky (< 16 prvků)** používá **Insertion Sort** - menší konstanta než rekurze.

```
n = 1000:
- threshold hloubky = 2 · log₂(1000) ≈ 20

QuickSort se spustí, pokud hloubka <= 20: pokračuj QuickSortem
                                  > 20:  přepni na HeapSort

Pro úseky < 16 prvků: InsertionSort
```

### Algoritmus IntroSort (zjednodušený)

```csharp
const int INSERTION_THRESHOLD = 16;

static void IntroSort(int[] pole, int low, int high, int depthLimit)
{
    while (high - low > INSERTION_THRESHOLD)
    {
        if (depthLimit == 0)
        {
            HeapSort(pole, low, high);              // fallback proti O(n²)
            return;
        }

        depthLimit--;
        int p = Partition(pole, low, high);
        IntroSort(pole, p + 1, high, depthLimit);   // rekurze na pravou
        high = p - 1;                                // iterativně na levou
    }

    InsertionSort(pole, low, high);                  // dokončení malých úseků
}

static void IntroSort(int[] pole)
{
    int depthLimit = 2 * (int)Math.Log2(pole.Length);
    IntroSort(pole, 0, pole.Length - 1, depthLimit);
}
```

### Použití v jazycích

| Jazyk / knihovna | Algoritmus |
|------------------|-----------|
| C++ STL `std::sort` | IntroSort |
| .NET `Array.Sort` (primitive) | IntroSort |
| .NET `Array.Sort` (objekty, .NET Core 3+) | Tim-like sort |
| Java `Arrays.sort(int[])` | Dual-Pivot QuickSort |
| Java `Arrays.sort(Object[])` | TimSort |
| Python `list.sort()` | TimSort |
| Rust `sort_unstable` | PDQSort (Pattern-Defeating QuickSort) |

### Dual-Pivot QuickSort

Java používá variantu se **dvěma pivoty** - vybere dva pivoty `p < q` a rozdělí pole na tři části:

```
+----------+----------+----------+
| < p      | p ≤ x ≤ q| > q      |
+----------+----------+----------+
```

V praxi výrazně rychlejší než klasický Quick Sort pro velká pole.

---

## 11. Porovnání s ostatními algoritmy

### Třídicí algoritmy - shrnutí

| Algoritmus | Best | Avg | Worst | Paměť | Stabilní | In-place |
|------------|------|-----|-------|-------|----------|----------|
| **Quick Sort** | O(n log n) | O(n log n) | O(n²) | O(log n) | Ne | Ano |
| **Merge Sort** | O(n log n) | O(n log n) | O(n log n) | O(n) | Ano | Ne |
| **Heap Sort** | O(n log n) | O(n log n) | O(n log n) | O(1) | Ne | Ano |
| **Insert Sort** | O(n) | O(n²) | O(n²) | O(1) | Ano | Ano |
| **Bubble Sort** | O(n) | O(n²) | O(n²) | O(1) | Ano | Ano |
| **Select Sort** | O(n²) | O(n²) | O(n²) | O(1) | Ne | Ano |
| **TimSort** | O(n) | O(n log n) | O(n log n) | O(n) | Ano | Ne |
| **IntroSort** | O(n log n) | O(n log n) | O(n log n) | O(log n) | Ne | Ano |

### Když použít co

| Situace | Doporučený algoritmus |
|---------|----------------------|
| Obecné třídění čísel | Quick Sort / IntroSort |
| Stabilní třídění objektů | Merge Sort / TimSort |
| Garantovaný worst case | Heap Sort / Merge Sort |
| Téměř setříděná data | TimSort / Insertion Sort |
| Externí třídění (soubory) | Merge Sort (k-way) |
| Malá pole (< 30 prvků) | Insertion Sort |
| Třídění s mnoha duplicitami | 3-way Quick Sort |
| Hledání k-tého prvku | QuickSelect |
| Streamingové třídění | Heap (PriorityQueue) |

---

## 12. Maturitní chytáky

### Časté implementační chyby

**Chybějící základní případ:**

```csharp
// CHYBA - nikdy se nezastaví
static void QuickSort(int[] pole, int low, int high)
{
    int p = Partition(pole, low, high);            // CRASH pro prázdné pole
    QuickSort(pole, low, p - 1);
    QuickSort(pole, p + 1, high);
}

// SPRÁVNĚ
static void QuickSort(int[] pole, int low, int high)
{
    if (low >= high) return;                       // základní případ
    int p = Partition(pole, low, high);
    QuickSort(pole, low, p - 1);
    QuickSort(pole, p + 1, high);
}
```

**Špatná inicializace `i`:**

```csharp
// CHYBA - přeskočí první prvek
int i = low;

// SPRÁVNĚ - hranice "menší zóny" je prázdná
int i = low - 1;
```

**Chybný cyklus `j`:**

```csharp
// CHYBA - zahrne pivot do partition
for (int j = low; j <= high; j++)

// SPRÁVNĚ - pivot je na indexu high
for (int j = low; j < high; j++)
```

**Overflow při výpočtu středu (median-of-three):**

```csharp
// PROBLEM - může přetéct
int mid = (low + high) / 2;

// SPRÁVNĚ - bezpečně
int mid = low + (high - low) / 2;
```

**Zapomenutí na duplicity:**

Klasický Lomuto neefektivně řeší pole s mnoha duplicitami. Pro takové vstupy použijte 3-way partition.

### Typické otázky u ústní zkoušky

- **"Proč je Quick Sort v praxi rychlejší než Merge Sort?"**
  Cache lokalita (sekvenční přístup), in-place (žádné alokace), menší konstanta. I když mají stejnou asymptotickou složitost, Quick Sort vyhrává v reálných benchmarcích.

- **"Kdy nastává worst case Quick Sortu?"**
  Když pivot vždy padne na extrém (min nebo max), takže jedna část je prázdná a druhá má n-1 prvků. Typicky pro setříděné pole + pivot = první/poslední prvek.

- **"Jak se dá worst case obejít?"**
  Median-of-three pivot, náhodný pivot, IntroSort (přepnutí na HeapSort při hluboké rekurzi), 3-way partition pro duplicity.

- **"Proč Quick Sort není stabilní?"**
  Při swapování v partition se mohou prvky se stejnou hodnotou přehodit přes sebe. Příklad: `[5a, 3, 5b, 1]` s pivot = 1 - swapy poruší pořadí `5a, 5b`.

- **"Vysvětli princip Lomuto partition."**
  Pivot = poslední prvek. Index `i` značí konec "menší zóny" (na začátku `i = low - 1`). Procházíme `j` přes celý úsek, a když najdeme prvek `< pivot`, zvětšíme `i` a swapneme `pole[i]` s `pole[j]`. Na konci swapneme pivot na pozici `i+1`.

- **"Co je QuickSelect a jaký má rozdíl od Quick Sortu?"**
  QuickSelect najde k-tý nejmenší prvek v O(n) průměrně. Rozdíl: po partition rekurzuje jen do JEDNÉ části (té, kde je k-tý prvek).

- **"Co je IntroSort?"**
  Hybridní algoritmus: začíná QuickSortem, při překročení hloubky `2·log(n)` přepne na HeapSort (garantuje O(n log n)) a pro malá podpole (< 16) přepne na Insertion Sort.

- **"Jaký je rozdíl mezi Lomuto a Hoare partition?"**
  Lomuto: pivot = poslední, jednoduchý, pivot je na finální pozici. Hoare: dva ukazatele se pohybují proti sobě, méně swapů v průměru, pivot není na finální pozici.

### Kontrolní seznam při code review

- [ ] Základní případ `if (low >= high) return;`
- [ ] Inicializace `i = low - 1`
- [ ] Cyklus `j < high` (ne `<=`)
- [ ] Závěrečný swap `pole[i+1] ↔ pole[high]`
- [ ] Rekurzivní volání: `(low, p-1)` a `(p+1, high)` - bez pivota
- [ ] Bezpečný výpočet `mid = low + (high - low) / 2`
- [ ] Tail call optimization pro garantovanou hloubku O(log n)
- [ ] Median-of-three nebo randomizace proti patologickým vstupům

---

## 13. Klíčové pojmy

- **Quick Sort** - rekurzivní in-place porovnávací třídicí algoritmus typu Divide & Conquer s průměrnou složitostí O(n log n).
- **Pivot** - prvek, který slouží jako dělicí hranice mezi menší a větší částí pole.
- **Partition** - operace, která přeskupí pole tak, aby pivot byl na své finální pozici a kolem něj byly menší/větší prvky.
- **Lomutův partition** - jednodušší schéma s pivotem na konci, jednou hranicí `i`.
- **Hoareův partition** - původní schéma s dvěma ukazateli pohybujícími se proti sobě.
- **3-Way Partition (Dutch National Flag)** - rozdělení na tři části `< = >` pivot, efektivní pro pole s duplicitami.
- **Median-of-three** - strategie volby pivota jako mediánu z prvního, prostředního a posledního prvku.
- **Median-of-medians (BFPRT)** - algoritmus pro nalezení skutečného mediánu v O(n).
- **Randomized Quick Sort** - varianta s náhodným pivotem, garantující očekávané O(n log n).
- **Cache lokalita** - vlastnost algoritmu pracovat se sousedními pamětními buňkami, využívající procesorovou cache.
- **In-place algoritmus** - pracuje s O(1) extra pamětí mimo vstup.
- **Stabilita** - vlastnost zachovávat relativní pořadí prvků se stejným klíčem; Quick Sort stabilní NENÍ.
- **QuickSelect** - lineární algoritmus pro hledání k-tého nejmenšího prvku, modifikace Quick Sortu.
- **IntroSort (Introspective Sort)** - hybridní algoritmus kombinující QuickSort, HeapSort a InsertionSort.
- **Dual-Pivot QuickSort** - varianta se dvěma pivoty rozdělující pole na tři části, používaná v Javě.
- **Pattern-Defeating QuickSort (PDQSort)** - moderní varianta v Rustu, detekuje patologické vzory a přepíná strategii.
- **Tail call optimization** - převedení rekurzivního volání v tail position na iteraci, eliminuje hloubku zásobníku.
- **Hloubka rekurze** - počet aktivních volání na zásobníku; pro Quick Sort O(log n) průměrně, O(n) worst case.
- **StackOverflow** - chyba způsobená přetečením zásobníku rekurze, hrozí při worst-case Quick Sortu bez optimalizace.
- **Patologický vstup** - vstup způsobující worst-case chování algoritmu (např. setříděné pole pro Quick Sort s prvním pivotem).
- **Comparison-based sort** - algoritmus pracující pouze přes porovnání prvků; spodní mez Ω(n log n).

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| Ot. 5 - Rekurze | Quick Sort jako klasický rekurzivní algoritmus |
| Ot. 7 - Složitost | Analýza O(n log n) vs O(n²), Master Theorem |
| Ot. 9 - Stromy | Heap Sort jako fallback v IntroSortu, prioritní fronta |
| Ot. 10 - Insert/Select Sort | Insertion Sort pro malé úseky v IntroSortu |
| Ot. 11 - Bubble/Merge Sort | Porovnání s Merge Sortem - oba D&C, rozdíly |
| Ot. 13 - Heap/Radix Sort | Heap Sort jako alternativa s garantovaným O(n log n) |
| Ot. 15 - Rozděl a panuj | Quick Sort jako ukázkový příklad paradigmatu |
| Ot. 16 - Algoritmické techniky | Randomizace, hybridní algoritmy |
