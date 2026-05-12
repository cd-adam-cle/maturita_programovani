# Zápisky: Otázka č. 13 - HEAP SORT

---

## Obsah

1. [Motivace pro třídění a místo Heap Sortu](#1-motivace-pro-třídění-a-místo-heap-sortu)
2. [Definice binární haldy](#2-definice-binární-haldy)
3. [Uložení haldy v poli](#3-uložení-haldy-v-poli)
4. [Operace SiftUp a SiftDown](#4-operace-siftup-a-siftdown)
5. [Insert a ExtractMax](#5-insert-a-extractmax)
6. [BuildHeap (Heapify)](#6-buildheap-heapify)
7. [Algoritmus Heap Sort krok za krokem](#7-algoritmus-heap-sort-krok-za-krokem)
8. [Časová a paměťová složitost](#8-časová-a-paměťová-složitost)
9. [Prioritní fronta (Priority Queue)](#9-prioritní-fronta-priority-queue)
10. [Použití haldy v praxi](#10-použití-haldy-v-praxi)
11. [Maturitní chytáky](#11-maturitní-chytáky)
12. [Klíčové pojmy](#12-klíčové-pojmy)

---

## 1. Motivace pro třídění a místo Heap Sortu

Heap Sort vyřešil v roce 1964 J. W. J. Williams a později ho zdokonalil R. W. Floyd. Cílem bylo navrhnout algoritmus, který kombinuje **garantovanou složitost O(n log n)** s **in-place** chováním (na rozdíl od Merge Sortu, který potřebuje O(n) extra paměti).

### Srovnání s ostatními algoritmy

| Algoritmus | Worst case | Paměť | Stabilní | In-place | Cache-friendly |
|------------|-----------|-------|----------|----------|----------------|
| Quick Sort | O(n²) | O(log n) | Ne | Ano | Ano |
| Merge Sort | O(n log n) | O(n) | Ano | Ne | Spíš ne |
| **Heap Sort** | **O(n log n)** | **O(1)** | **Ne** | **Ano** | **Ne** (skoky) |

Heap Sort je tedy jediný porovnávací algoritmus, který je **současně worst-case O(n log n) a in-place**. V praxi se přesto nepoužívá tak často jako Quick Sort kvůli horší **cache lokalitě** - SiftDown skáče po poli (rodič 0 → dítě 1 → vnoučata 3, 4 → ...), což zatěžuje paměťovou hierarchii. Hlavní využití haldy v praxi je proto spíš jako **prioritní fronta**, ne jako třídicí algoritmus.

### Halda - víc než jen pro Heap Sort

Halda (heap) je velmi univerzální datová struktura. Používá se v:

- **Priority queue** (např. v Dijkstrově algoritmu, A*, plánovači úloh OS).
- **Top-K problémech** - najít k největších/nejmenších prvků v O(n log k).
- **Streamovaný medián** - dvě haldy (min + max) udržují běžící medián.
- **Event simulation** - diskrétní simulace, kde halda obsahuje časované události.
- **Huffmanovo kódování** - opakovaně bere dva nejmenší stromy.
- **Heap Sort** - třídění.

---

## 2. Definice binární haldy

### Dvě klíčové vlastnosti

**Binární halda** (binary heap) je binární strom, který splňuje:

1. **Tvarová podmínka (shape property):** Strom je **úplný** - všechny úrovně jsou plně obsazené, kromě poslední, která se plní **zleva doprava**. Tato vlastnost umožňuje efektivní uložení v poli.

2. **Vlastnost haldy (heap property):** Mezi rodičem a jeho dětmi platí daný vztah - buď rodič ≥ děti (max-heap), nebo rodič ≤ děti (min-heap). Tato vlastnost platí lokálně mezi rodičem a jeho dvěma dětmi, nikoliv mezi sourozenci.

### Max-heap

Každý rodič je **větší nebo roven** svým dětem. Kořen obsahuje **maximum** celé haldy.

```
         90               ← kořen = maximum
        /  \
      70    80
     / \   /
    40 50  60
```

Vzestupně setříděné by bylo `[40, 50, 60, 70, 80, 90]`. Halda **není** plně setříděná - např. 70 a 80 nejsou ve "správném" pořadí mezi sebou. Garantována je jen relace rodič ≥ děti.

### Min-heap

Každý rodič je **menší nebo roven** svým dětem. Kořen obsahuje **minimum**.

```
         10               ← kořen = minimum
        /  \
      20    30
     / \   /
    40 50  60
```

### Halda vs. binární vyhledávací strom (BVS)

| Vlastnost | Halda | BVS |
|-----------|-------|-----|
| Vztah rodič-dítě | rodič ≥/≤ obě děti | levé < rodič < pravé |
| Sourozenci | Bez vztahu | Bez přímého vztahu, ale jsou v jiných podstromech |
| Najdi minimum | O(1) v min-heap | O(log n) |
| Najdi maximum | O(1) v max-heap | O(log n) |
| Vyhledání hodnoty | O(n) (lineární!) | O(log n) |
| Najdi předchůdce/následníka | Pomalé | O(log n) |
| Struktura | Úplný strom | Libovolný BVS |
| Uložení | Pole bez pointerů | Pointery (typicky) |

**Důležité:** Halda je **slabší** datová struktura než BVS pro obecné vyhledávání. Její síla je v **rychlém přístupu k extrému** a v **kompaktním uložení**.

### Výška haldy

Halda s `n` prvky má výšku `⌊log₂ n⌋`. Tím pádem operace SiftUp i SiftDown běží v O(log n).

---

## 3. Uložení haldy v poli

### Mapování stromu do pole

Díky tvarové podmínce (úplný strom) lze haldu uložit do pole **bez pointerů**. Indexy rodiče a dětí se vypočítají jednoduchou aritmetikou.

```
Strom:                          Pole (index od 0):
            90                  Index:    0   1   2   3   4   5
           /  \                 Hodnota:[90][70][80][40][50][60]
         70    80                        ^ kořen
        / \   /
      40  50 60
```

### Aritmetika indexů (indexování od 0)

```
Pro prvek na indexu i:
   rodič:       (i - 1) / 2        (celočíselné dělení)
   levé dítě:   2 · i + 1
   pravé dítě:  2 · i + 2

   je list:           2·i + 1 >= n
   první list:        n / 2
   poslední rodič:    n / 2 - 1   (= (n - 2) / 2)
```

### Aritmetika indexů (indexování od 1) - alternativa

V některých učebnicích se používá indexování od 1, kde aritmetika je ještě jednodušší:

```
Pro prvek na indexu i (od 1):
   rodič:       i / 2
   levé dítě:   2 · i
   pravé dítě:  2 · i + 1
```

Pole se pak ale plýtvá indexem 0. V C# se používá indexování od 0.

### Ověření na příkladu

```
Pole: [90, 70, 80, 40, 50, 60]   n = 6

Prvek 70 (index 1):
   rodič:       (1-1)/2 = 0   → 90
   levé dítě:   2·1+1 = 3     → 40
   pravé dítě:  2·1+2 = 4     → 50

Prvek 50 (index 4):
   rodič:       (4-1)/2 = 1   → 70
   levé dítě:   2·4+1 = 9     → mimo (n=6)
   → 50 je list

Poslední rodič: n/2 - 1 = 6/2 - 1 = 2  → prvek 80
```

### Parametr `n` v operacích

V Heap Sortu pole obsahuje současně **aktivní haldu** (prvních `n` prvků) a **už setříděnou část** (od `n` dál). Funkce typu `SiftDown` musí znát hranici `n`, aby nesáhla do setříděné oblasti:

```
[60, 50, 40 | 70, 80, 90]
 └─halda──┘  └─setříděno┘
   n = 3
```

### Výhody uložení v poli

- **Žádné pointery** - úspora paměti (cca polovina oproti stromu s ukazateli).
- **Sekvenční layout** - prvky vedle sebe (lépe pro cache).
- **O(1) navigace** - žádné dereferencování, pouze aritmetika.
- **Snadná serializace** - pole se snadno uloží do souboru.

### Nevýhody

- **Pevná velikost** - při překročení kapacity nutno realokovat (typicky `List<T>` to řeší automaticky).
- **Cache miss na hluboké úrovni** - dítě je 2·i+1 daleko, což pro hluboké úrovně neleží v cache.

---

## 4. Operace SiftUp a SiftDown

### SiftUp (probublání nahoru)

Používá se při **vkládání** nového prvku. Prvek umístíme na konec haldy a poté ho posunujeme nahoru, dokud splňuje heap property.

```
Vložení 95 do max-heap:

Krok 0: vlož na konec
        90              95 je na pozici 5 (poslední)
       /  \             95 > rodič 80? ANO → swap
     70    80
    / \   /
  40  50 95←nový

Krok 1: po swapu 95↔80
        90
       /  \             95 > rodič 90? ANO → swap
     70    95
    / \   /
  40  50 80

Krok 2: po swapu 95↔90
        95              95 je v kořeni, končíme
       /  \
     70    90
    / \   /
  40  50 80
```

Implementace:

```csharp
static void SiftUp(int[] pole, int i)
{
    while (i > 0)
    {
        int parent = (i - 1) / 2;
        if (pole[i] <= pole[parent]) break;        // heap property OK
        (pole[i], pole[parent]) = (pole[parent], pole[i]);
        i = parent;
    }
}
```

### SiftDown (propadnutí dolů)

Používá se při **odstranění kořene** a při **stavbě haldy**. Prvek na vrcholu propadá dolů - swapujeme ho s **větším** dětětem (v max-heap), dokud nesplňuje heap property.

```
Odebrání 95 z max-heap:

Krok 0: kořen 95 nahradíme posledním prvkem (80)
                                 a haldu zkrátíme o 1
        80              80 < max(70, 90) = 90 → swap s 90
       /  \
     70    90
    / \
  40  50

Krok 1: po swapu 80↔90
        90
       /  \             80 ≥ děti (40)? ANO, končíme (50 jen 1 dítě)
     70    80
    / \
  40  50
```

**Pravidlo:** Swapujeme s tím dítětem, které je **větší** (v max-heap) nebo **menší** (v min-heap). Jinak by se nový rodič mohl stát menším než druhé dítě a heap property by se porušila.

Rekurzivní implementace:

```csharp
static void SiftDown(int[] pole, int n, int i)
{
    int largest = i;
    int left  = 2 * i + 1;
    int right = 2 * i + 2;

    if (left  < n && pole[left]  > pole[largest]) largest = left;
    if (right < n && pole[right] > pole[largest]) largest = right;

    if (largest != i)
    {
        (pole[i], pole[largest]) = (pole[largest], pole[i]);
        SiftDown(pole, n, largest);                // pokračuj v podstromě
    }
}
```

Iterativní implementace (úsporná na zásobníku):

```csharp
static void SiftDownIter(int[] pole, int n, int i)
{
    while (true)
    {
        int largest = i;
        int l = 2 * i + 1, r = 2 * i + 2;

        if (l < n && pole[l] > pole[largest]) largest = l;
        if (r < n && pole[r] > pole[largest]) largest = r;

        if (largest == i) break;                   // heap property OK
        (pole[i], pole[largest]) = (pole[largest], pole[i]);
        i = largest;
    }
}
```

### Složitost

Obě operace běží v O(log n), protože výška haldy je `⌊log₂ n⌋`.

---

## 5. Insert a ExtractMax

### Insert (vložení nového prvku)

```
1. Přidej prvek na konec haldy (pozice n).
2. Zvětši n.
3. SiftUp(n - 1).
```

```csharp
static void Insert(int[] pole, ref int n, int hodnota)
{
    pole[n] = hodnota;
    n++;
    SiftUp(pole, n - 1);
}
```

Složitost: **O(log n)** - probublávání nahoru přes maximálně log n úrovní.

### ExtractMax (odebrání maxima z max-heap)

```
1. Maximum je na kořeni (index 0).
2. Přesuň poslední prvek do kořene (pole[0] = pole[n-1]).
3. Zmenši n.
4. SiftDown(0) na nově umístěném prvku.
5. Vrať uloženou hodnotu max.
```

```csharp
static int ExtractMax(int[] pole, ref int n)
{
    if (n == 0) throw new InvalidOperationException("Empty heap");

    int max = pole[0];
    pole[0] = pole[n - 1];
    n--;
    SiftDown(pole, n, 0);
    return max;
}
```

Složitost: **O(log n)** - SiftDown přes log n úrovní.

### Peek (přístup k maximu bez odebrání)

```csharp
static int Peek(int[] pole, int n)
{
    if (n == 0) throw new InvalidOperationException("Empty heap");
    return pole[0];
}
```

Složitost: **O(1)**.

---

## 6. BuildHeap (Heapify)

### Naivní přístup - O(n log n)

Stavbu haldy lze provést postupným vkládáním všech prvků:

```csharp
static void BuildHeapNaive(int[] pole)
{
    int n = 0;
    int[] heap = new int[pole.Length];
    foreach (int x in pole)
        Insert(heap, ref n, x);
}
```

Celkový čas: `n · O(log n) = O(n log n)`. Nepotřebujeme ale tolik práce.

### Floydův algoritmus - O(n)

Klíčové pozorování: pokud opravíme haldu **od listů ke kořeni**, můžeme začít až od **posledního rodiče** (index `n/2 - 1`). Listy už podmínku splňují (nemají děti).

```csharp
static void BuildHeap(int[] pole)
{
    int n = pole.Length;
    for (int i = n / 2 - 1; i >= 0; i--)
        SiftDown(pole, n, i);
}
```

### Proč je BuildHeap O(n), ne O(n log n)?

Naivní odhad: každý uzel může propadat až `log n` úrovní → n · log n. Skutečnost je ale **přísnější**, protože **většina uzlů je u listů** a propadají málo úrovní:

- Listy (n/2 uzlů): propadají 0 úrovní.
- Předposlední úroveň (n/4 uzlů): propadají max 1 úroveň.
- Další úroveň (n/8 uzlů): propadají max 2 úrovně.
- ...

Suma:

```
Σ (n / 2^(h+1)) · h   pro h = 0, 1, ..., log n
= n · Σ h / 2^(h+1)
= n · (konstanta) ≈ n · 1
= O(n)
```

Kde `Σ h / 2^h = 2` (konvergentní řada).

Toto je důležitý poznatek - často se objevuje u ústní zkoušky.

### Vizualizace BuildHeap

Vstup: `[40, 10, 80, 50, 90, 60]`

```
Strom před:           Strom po BuildHeap:
        40                    90
       /  \                  /  \
     10    80              50    80
    / \   /               / \   /
  50  90 60             40  10 60

Pole před: [40, 10, 80, 50, 90, 60]
Pole po:   [90, 50, 80, 40, 10, 60]
```

Krok za krokem (od posledního rodiče):

```
n = 6, poslední rodič = n/2 - 1 = 2

i = 2 (prvek 80):
   děti: 60 (index 5), žádné pravé
   80 > 60 → bez změny

i = 1 (prvek 10):
   děti: 50 (index 3), 90 (index 4)
   max(10, 50, 90) = 90 → swap 10↔90
   pole: [40, 90, 80, 50, 10, 60]
   nový rekurzivní SiftDown na index 4 (kde je nyní 10)
   - dítě (index 9, 10) mimo n → STOP

i = 0 (prvek 40):
   děti: 90 (index 1), 80 (index 2)
   max(40, 90, 80) = 90 → swap 40↔90
   pole: [90, 40, 80, 50, 10, 60]
   rekurzivní SiftDown na index 1 (kde je nyní 40)
   - děti: 50, 10. max(40, 50, 10) = 50 → swap 40↔50
   pole: [90, 50, 80, 40, 10, 60]
   rekurze na index 3 - mimo n
```

---

## 7. Algoritmus Heap Sort krok za krokem

### Idea

Heap Sort má dvě fáze:

1. **BuildHeap** - postavíme max-heap z neuspořádaného pole (O(n)).
2. **Extract loop** - opakovaně bereme maximum z haldy a ukládáme ho na konec pole. Halda se zmenšuje, setříděná část roste.

Klíčový trik: nemusíme přesouvat maximum do extra pole - místo toho ho **swapujeme s posledním prvkem haldy** a haldu zkrátíme. Tím vznikne setříděná část přímo v poli, vzestupně.

### Algoritmus

```
HEAP_SORT(pole):
   n = délka(pole)

   // Fáze 1: BuildHeap
   for i = n/2 - 1 down to 0:
      SIFT_DOWN(pole, n, i)

   // Fáze 2: Extract max repeatedly
   for i = n - 1 down to 1:
      swap(pole[0], pole[i])         // max na konec
      SIFT_DOWN(pole, i, 0)          // oprav haldu velikosti i
```

### Implementace

```csharp
static void HeapSort(int[] pole)
{
    int n = pole.Length;

    // Fáze 1: postav max-heap (O(n))
    for (int i = n / 2 - 1; i >= 0; i--)
        SiftDown(pole, n, i);

    // Fáze 2: opakovaně vytáhni maximum (O(n log n))
    for (int i = n - 1; i > 0; i--)
    {
        (pole[0], pole[i]) = (pole[i], pole[0]);   // max na pozici i
        SiftDown(pole, i, 0);                       // oprav zmenšenou haldu
    }
}

static void SiftDown(int[] pole, int n, int i)
{
    while (true)
    {
        int largest = i;
        int l = 2 * i + 1, r = 2 * i + 2;

        if (l < n && pole[l] > pole[largest]) largest = l;
        if (r < n && pole[r] > pole[largest]) largest = r;

        if (largest == i) break;
        (pole[i], pole[largest]) = (pole[largest], pole[i]);
        i = largest;
    }
}
```

### Trace pro `[40, 10, 80, 50, 90, 60]`

**Fáze 1 - BuildHeap:**
```
Start:     [40, 10, 80, 50, 90, 60]
Po SiftDown(i=2): [40, 10, 80, 50, 90, 60]    (žádná změna)
Po SiftDown(i=1): [40, 90, 80, 50, 10, 60]    (10↔90)
Po SiftDown(i=0): [90, 50, 80, 40, 10, 60]    (40→90, pak 40→50)

Max-heap: [90, 50, 80, 40, 10, 60]
```

**Fáze 2 - Extract:**
```
Start:    [90, 50, 80, 40, 10, 60]    n=6
swap(0,5): [60, 50, 80, 40, 10 | 90]   n=5
SiftDown:  [80, 50, 60, 40, 10 | 90]

swap(0,4): [10, 50, 60, 40 | 80, 90]   n=4
SiftDown:  [60, 50, 10, 40 | 80, 90]

swap(0,3): [40, 50, 10 | 60, 80, 90]   n=3
SiftDown:  [50, 40, 10 | 60, 80, 90]

swap(0,2): [10, 40 | 50, 60, 80, 90]   n=2
SiftDown:  [40, 10 | 50, 60, 80, 90]

swap(0,1): [10 | 40, 50, 60, 80, 90]   n=1
(halda velikosti 1, hotovo)

Výsledek: [10, 40, 50, 60, 80, 90]
```

### Vzestupně vs sestupně

- **Max-heap → vzestupně setříděné pole** (max jde na konec).
- **Min-heap → sestupně setříděné pole** (min jde na konec).

V praxi se obvykle používá max-heap pro vzestupné třídění.

---

## 8. Časová a paměťová složitost

### Časová složitost

| Fáze | Operace | Složitost |
|------|---------|-----------|
| **BuildHeap** | n/2 × SiftDown s amortizovanou analýzou | **O(n)** |
| **Extract loop** | (n-1) × (swap + SiftDown) = (n-1) · O(log n) | **O(n log n)** |
| **Celkem** | O(n) + O(n log n) | **O(n log n)** |

### Best / Average / Worst

Heap Sort **vždy** běží v O(n log n), bez ohledu na vstup. Není adaptivní - nezáleží, zda je pole setříděné, nesetříděné nebo částečně setříděné.

| Případ | Složitost |
|--------|-----------|
| Best case | O(n log n) |
| Average case | O(n log n) |
| Worst case | O(n log n) |

Pro pole z `n` stejných prvků existuje varianta haldy, která dosáhne O(n), ale klasický Heap Sort to nevyužije.

### Paměťová složitost

**O(1)** - skutečně in-place. Používá jen pár pomocných proměnných (i, largest, temp). Pokud používáme rekurzivní SiftDown, je tu navíc O(log n) na zásobníku - iterativní verze eliminuje i toto.

### Srovnání s ostatními algoritmy

```
Algoritmus    | Nejlepší   | Průměr     | Nejhorší   | Paměť    | Stabilní
──────────────|────────────|────────────|────────────|──────────|─────────
Insert Sort   | O(n)       | O(n²)      | O(n²)      | O(1)     | Ano
Select Sort   | O(n²)      | O(n²)      | O(n²)      | O(1)     | Ne
Bubble Sort   | O(n)       | O(n²)      | O(n²)      | O(1)     | Ano
Merge Sort    | O(n log n) | O(n log n) | O(n log n) | O(n)     | Ano
Quick Sort    | O(n log n) | O(n log n) | O(n²)      | O(log n) | Ne
Heap Sort     | O(n log n) | O(n log n) | O(n log n) | O(1)     | Ne
```

### Proč Heap Sort není v praxi tak rychlý jako Quick Sort

I když má **lepší worst case** než Quick Sort, v praxi je obvykle **2-3× pomalejší** kvůli:

1. **Cache locality** - SiftDown skáče po poli (rodič → dítě 2i+1 → vnoučata 4i+3, ...). Tyto skoky způsobují **cache miss** na hluboké úrovni.
2. **Více swapů** - Heap Sort typicky provede více swapů než Quick Sort.
3. **Větvení (branch prediction)** - SiftDown má dva navazující ify, které procesor špatně predikuje.

Quick Sort naopak pracuje sekvenčně přes pole (PARTITION), což je cache-friendly.

---

## 9. Prioritní fronta (Priority Queue)

### Co je prioritní fronta

**Prioritní fronta** je abstraktní datová struktura, kde každý prvek má **prioritu** a operace `Dequeue` vrací prvek s **nejvyšší** (resp. nejnižší) prioritou. Není to FIFO ani LIFO - pořadí určuje priorita.

### Operace prioritní fronty

| Operace | Popis | Složitost (heap) |
|---------|-------|------------------|
| `Enqueue(x, p)` | Vlož prvek x s prioritou p | O(log n) |
| `Dequeue()` | Odeber a vrať prvek s nejvyšší prioritou | O(log n) |
| `Peek()` | Vrať prvek s nejvyšší prioritou (bez odebrání) | O(1) |
| `Count` | Počet prvků | O(1) |

### Implementace prioritní fronty haldou

Halda je **kanonická** implementace prioritní fronty. Min-heap → fronta s minimální prioritou nahoře (typické), max-heap → fronta s maximální prioritou nahoře.

### PriorityQueue v .NET (od .NET 6)

```csharp
var pq = new PriorityQueue<string, int>();        // <element, priorita>

pq.Enqueue("Email send", 5);
pq.Enqueue("System crash", 1);                     // nižší = vyšší priorita
pq.Enqueue("Backup", 10);

while (pq.Count > 0)
    Console.WriteLine(pq.Dequeue());
// Pořadí: "System crash", "Email send", "Backup"
```

### Custom comparer (max-heap)

`PriorityQueue` defaultně používá min-heap. Pro max-heap stačí převrácený komparátor:

```csharp
var maxHeap = new PriorityQueue<int, int>(Comparer<int>.Create((a, b) => b - a));
maxHeap.Enqueue(5, 5);
maxHeap.Enqueue(10, 10);
maxHeap.Enqueue(1, 1);
Console.WriteLine(maxHeap.Dequeue());              // 10 (max)
```

### Vlastní implementace prioritní fronty

```csharp
public class PriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> heap = new();
    public int Count => heap.Count;
    public bool IsEmpty => heap.Count == 0;

    public void Enqueue(T item)
    {
        heap.Add(item);
        SiftUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (IsEmpty) throw new InvalidOperationException();
        T top = heap[0];
        int last = heap.Count - 1;
        heap[0] = heap[last];
        heap.RemoveAt(last);
        if (!IsEmpty) SiftDown(0);
        return top;
    }

    public T Peek() => IsEmpty ? throw new InvalidOperationException() : heap[0];

    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].CompareTo(heap[parent]) >= 0) break;
            (heap[i], heap[parent]) = (heap[parent], heap[i]);
            i = parent;
        }
    }

    private void SiftDown(int i)
    {
        int n = heap.Count;
        while (true)
        {
            int smallest = i;
            int l = 2 * i + 1, r = 2 * i + 2;
            if (l < n && heap[l].CompareTo(heap[smallest]) < 0) smallest = l;
            if (r < n && heap[r].CompareTo(heap[smallest]) < 0) smallest = r;
            if (smallest == i) break;
            (heap[i], heap[smallest]) = (heap[smallest], heap[i]);
            i = smallest;
        }
    }
}
```

Tato implementace je min-heap (vrací nejmenší prvek).

---

## 10. Použití haldy v praxi

### Dijkstrův algoritmus a A*

Hledání nejkratší cesty v grafu - opakovaně vybíráme uzel s **nejmenší tentativní vzdáleností**. Prioritní fronta dělá tento výběr v O(log V), díky čemuž má Dijkstra složitost O((V+E) log V) místo O(V²).

```csharp
var pq = new PriorityQueue<int, double>();         // uzel, vzdálenost
pq.Enqueue(start, 0);

while (pq.TryDequeue(out int u, out double d))
{
    if (d > dist[u]) continue;                     // zastaralý záznam
    foreach (var (v, w) in graf[u])
    {
        double nove = d + w;
        if (nove < dist[v])
        {
            dist[v] = nove;
            pq.Enqueue(v, nove);
        }
    }
}
```

### Huffmanovo kódování

Stavba Huffmanova stromu - opakovaně bere dva uzly s **nejnižší frekvencí** a slučuje je.

### Top-K elements (najít k největších/nejmenších)

Pro k << n je halda velikosti k efektivnější než třídění celého pole:

```csharp
// Najdi 10 největších čísel z velkého streamu
var minHeap = new PriorityQueue<int, int>();

foreach (int x in stream)
{
    if (minHeap.Count < 10)
        minHeap.Enqueue(x, x);
    else if (x > minHeap.Peek())
    {
        minHeap.Dequeue();
        minHeap.Enqueue(x, x);
    }
}
```

Složitost: O(n log k), paměť O(k). Pro k = 10, n = 10⁹ je to velmi rychlé.

### Streaming median (běžící medián)

Udržujeme **dvě haldy**:
- **Max-heap** pro spodní polovinu hodnot.
- **Min-heap** pro horní polovinu hodnot.

Medián je buď kořen jedné z hald, nebo průměr obou kořenů.

```csharp
class RunningMedian
{
    private readonly PriorityQueue<int, int> lower = new(Comparer<int>.Create((a, b) => b - a)); // max
    private readonly PriorityQueue<int, int> upper = new();                                       // min

    public void Add(int x)
    {
        if (lower.Count == 0 || x <= lower.Peek())
            lower.Enqueue(x, x);
        else
            upper.Enqueue(x, x);

        // Rebalance: rozdíl velikosti max 1
        if (lower.Count > upper.Count + 1)
        {
            int top = lower.Dequeue();
            upper.Enqueue(top, top);
        }
        else if (upper.Count > lower.Count)
        {
            int top = upper.Dequeue();
            lower.Enqueue(top, top);
        }
    }

    public double Median => lower.Count > upper.Count
        ? lower.Peek()
        : (lower.Peek() + upper.Peek()) / 2.0;
}
```

### Event simulation

Diskrétní simulace, kde halda obsahuje budoucí události seřazené podle času jejich nástupu. `Dequeue` vrátí nejbližší budoucí událost.

### Plánovač úloh OS

Operating system používá haldu (nebo její variantu - CFS v Linuxu používá red-black tree) pro výběr **další úlohy s nejvyšší prioritou**.

### d-ary haldy

Klasická binární halda má 2 děti. **d-ary heap** má `d` dětí na uzel. Pro `d = 4` (cache-friendly) bývá Heap Sort až 2× rychlejší v praxi, protože:
- Strom je mělčí (`log_d n` místo `log_2 n`).
- SiftDown porovnává s d dětmi najednou (jeden cache miss místo dvou).

### Pairing heap, Fibonacci heap

Pokročilejší variace haldy s lepší amortizovanou složitostí pro některé operace. Fibonacci heap má `Decrease-Key` v amortizovaném O(1), což zlepší Dijkstrův algoritmus na O(V log V + E). V praxi se ale málo používá kvůli velkým konstantám.

---

## 11. Maturitní chytáky

### Časté chyby

**Špatný směr swapu v SiftDown:**

```csharp
// CHYBA - swap s menším dítětem rozbije max-heap
if (pole[l] < pole[largest]) largest = l;

// SPRÁVNĚ pro max-heap - swap s VĚTŠÍM dítětem
if (pole[l] > pole[largest]) largest = l;
```

**Zapomenutí na parametr `n`:**

```csharp
// CHYBA - SiftDown použije celé pole, sáhne do setříděné části
SiftDown(pole, pole.Length, 0);    // při i = 5 a n měl být 5

// SPRÁVNĚ - v Heap Sortu se velikost haldy zmenšuje
SiftDown(pole, i, 0);              // i je aktuální velikost haldy
```

**Off-by-one v indexech:**

```csharp
// CHYBA
int left = 2 * i;                  // platí jen pro indexování od 1!

// SPRÁVNĚ (indexování od 0)
int left = 2 * i + 1;
int right = 2 * i + 2;
```

**Špatný startovní index BuildHeap:**

```csharp
// CHYBA - začíná od kořene, ale děti ještě nejsou haldy
for (int i = 0; i < n; i++) SiftDown(pole, n, i);

// SPRÁVNĚ - od posledního rodiče směrem ke kořeni
for (int i = n / 2 - 1; i >= 0; i--) SiftDown(pole, n, i);
```

**Heap Sort jako stabilní:**

```
"Je Heap Sort stabilní?"
NE - swap kořene s posledním prvkem může změnit relativní pořadí stejných hodnot.
```

### Typické otázky u ústní zkoušky

- **"Jaký je rozdíl mezi haldou a binárním vyhledávacím stromem?"**
  Halda garantuje jen vztah rodič-dítě (rodič ≥ děti nebo ≤). BVS garantuje levé < rodič < pravé. Halda má rychlý přístup k extrému (O(1)), BVS rychlé vyhledávání (O(log n)). Halda je úplný strom uložený v poli, BVS může být libovolný strom.

- **"Proč je BuildHeap O(n) a ne O(n log n)?"**
  Většina uzlů je u listů a propadají málo úrovní. Suma `Σ (n/2^(h+1)) · h = O(n)` přes všechny úrovně. Konvergentní geometrická řada.

- **"Proč se používají vzorce 2i+1 a 2i+2?"**
  Pro úplný strom uložený v poli s indexováním od 0. Z odvození: kořen je na indexu 0, jeho děti na 1, 2. Děti uzlu na pozici i jsou na 2i+1 a 2i+2.

- **"Proč není Heap Sort stabilní?"**
  Při swap kořene s posledním prvkem mohou prvky se stejnou hodnotou změnit pořadí. Příklad: `[5a, 5b]` jako max-heap může po extract dát `[5b, 5a]` nebo `[5a, 5b]` podle struktury haldy.

- **"Kdy použít Heap Sort místo Quick Sortu?"**
  Když potřebujeme garantovaný O(n log n) worst case (bezpečnostně kritické aplikace, real-time systémy). Quick Sort má v praxi menší konstantu, ale může degradovat na O(n²).

- **"Jaký je rozdíl mezi min-heap a max-heap?"**
  Vztah rodič-dítě je obrácený. Min-heap → kořen = minimum, max-heap → kořen = maximum. Pro Heap Sort vzestupně používáme max-heap.

- **"K čemu slouží prioritní fronta v Dijkstrově algoritmu?"**
  K rychlému výběru uzlu s nejmenší tentativní vzdáleností (`extract-min`). Bez haldy: O(V²), s haldou: O((V+E) log V).

### Kontrolní seznam při code review

- [ ] Indexy `2i+1` a `2i+2` (indexování od 0)
- [ ] Rodič `(i-1)/2`
- [ ] BuildHeap od `n/2 - 1` směrem k 0
- [ ] SiftDown swapuje s VĚTŠÍM dítětem (max-heap) / MENŠÍM (min-heap)
- [ ] Parametr `n` znamená velikost AKTIVNÍ haldy (ne délku pole)
- [ ] Kontrola `left < n` a `right < n` před přístupem
- [ ] Insert/Extract operace končí SiftUp/SiftDown
- [ ] Heap Sort: po BuildHeap následuje extract loop s klesajícím `i`

---

## 12. Klíčové pojmy

- **Halda (heap, binární halda)** - úplný binární strom splňující heap property.
- **Max-heap** - varianta haldy s rodič ≥ děti; kořen = maximum.
- **Min-heap** - varianta haldy s rodič ≤ děti; kořen = minimum.
- **Tvarová podmínka (shape property)** - strom je úplný, plní se zleva doprava.
- **Heap property** - vztah rodič-dítě (≥ nebo ≤) platí pro všechny uzly.
- **Úplný binární strom (complete binary tree)** - všechny úrovně plné kromě poslední, ta zleva doprava.
- **SiftUp (probublání nahoru)** - oprava heap property směrem ke kořeni; použito v Insert.
- **SiftDown (propadnutí dolů, heapify)** - oprava heap property směrem k listům; použito v Extract a BuildHeap.
- **BuildHeap (Heapify)** - sestavení haldy z neuspořádaného pole v O(n).
- **Insert** - přidání prvku do haldy v O(log n).
- **ExtractMax / ExtractMin** - odebrání kořene v O(log n).
- **Peek** - přístup k kořeni bez odebrání v O(1).
- **Heap Sort** - třídicí algoritmus využívající haldu, worst case O(n log n), in-place.
- **Floydův algoritmus** - efektivní stavba haldy v O(n) (BuildHeap od posledního rodiče).
- **Prioritní fronta (priority queue)** - ADT s prioritou; halda je její standardní implementace.
- **PriorityQueue<TElement, TPriority>** - .NET 6+ built-in prioritní fronta.
- **Mapování stromu do pole** - vzorce `2i+1`, `2i+2`, `(i-1)/2` pro navigaci.
- **Cache locality** - vlastnost algoritmu pracovat se sousedními buňkami paměti; Heap Sort má horší než Quick Sort kvůli skokům.
- **d-ary heap** - zobecnění s d dětmi na uzel; rychlejší v praxi pro d = 4.
- **Fibonacci heap** - pokročilá varianta s amortizovaným O(1) pro Decrease-Key.
- **Pairing heap** - další pokročilá varianta haldy.
- **Top-K problém** - úloha najít k největších/nejmenších prvků, řešená haldou velikosti k.
- **Streaming median** - online medián pomocí dvou hald (max + min).
- **Huffmanův strom** - kódování postavené opakovaným odebíráním dvou nejmenších uzlů z haldy.
- **Dijkstra / A*** - algoritmy nejkratších cest využívající prioritní frontu.
- **Stabilita** - vlastnost zachovávat pořadí stejných klíčů; Heap Sort NENÍ stabilní.

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| Ot. 5 - Rekurze | SiftDown jako klasická tail rekurze (lze přepsat na iteraci) |
| Ot. 7 - Složitost | BuildHeap O(n) jako příklad amortizované analýzy |
| Ot. 9 - Stromy | Halda jako speciální binární strom; uložení bez pointerů |
| Ot. 10 - Insert/Select Sort | Select Sort jako "naivní" verze Heap Sortu (O(n²)) |
| Ot. 11 - Bubble/Merge Sort | Srovnání algoritmů, stabilita, paměťová náročnost |
| Ot. 12 - Quick Sort | IntroSort přepíná na Heap Sort při hluboké rekurzi |
| Ot. 18 - Grafové algoritmy | Dijkstra, Prim, A* využívají prioritní frontu |
| Ot. 25 - Dijkstra | Konkrétní použití prioritní fronty (haldy) |
