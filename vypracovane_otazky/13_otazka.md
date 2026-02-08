# 📚 Zápisky: Otázka č. 13 - HEAP SORT
**Datum:** 2025-02-08
**Status:** ✅ Hotovo (teorie)

---

## ✅ Checklist bodů otázky
- [x] Bod 1: Motivace pro třídění dat
- [x] Bod 2: Definice binární minimové/maximové haldy
- [x] Bod 3: Základní operace v haldě
- [x] Bod 4: Uložení haldy v poli
- [x] Bod 5: Algoritmus Heap Sort po jednotlivých krocích (+ obrázek)
- [x] Bod 6: Časová a paměťová složitost

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: Motivace pro třídění dat

Se setříděnými daty se pracuje efektivněji – binární vyhledávání (O(log n)), odstraňování duplicit, merge operace. Heap Sort konkrétně řeší slabiny ostatních algoritmů:

| Algoritmus | Nejhorší případ | In-place? | Stabilní? |
|-----------|----------------|-----------|-----------|
| Quick Sort | O(n²) ❌ | ✅ Ano | ❌ Ne |
| Merge Sort | O(n log n) ✅ | ❌ Ne (O(n) paměť) | ✅ Ano |
| **Heap Sort** | **O(n log n) ✅** | **✅ Ano** | **❌ Ne** |

**Heap Sort = garantovaně O(n log n) + in-place.** Kombinace, kterou nemá ani Quick Sort, ani Merge Sort.

---

### Bod 2: Definice binární haldy

Halda je **speciální binární strom** se dvěma vlastnostmi:

1. **Tvar (Shape property):** Strom je **úplný** (complete) – všechny úrovně plně obsazené, poslední se plní zleva doprava.
2. **Vlastnost haldy (Heap property):** Každý rodič je ve vztahu ke svým dětem.

**Maximová halda (Max-Heap):** Rodič >= děti. Kořen = maximum.
```
         90          ← největší nahoře
        /  \
      70    80
     / \   /
    40 50  60
```

**Minimová halda (Min-Heap):** Rodič <= děti. Kořen = minimum.
```
         10          ← nejmenší nahoře
        /  \
      20    30
     / \   /
    40 50  60
```

**Klíčové rozdíly od BVS:**
- Halda garantuje jen **vertikální** vztah (rodič vs dítě)
- BVS garantuje: levé dítě < rodič < pravé dítě
- Sourozenci v haldě **nemusí** být navzájem seřazení

---

### Bod 3: Základní operace v haldě

#### SiftUp (probublání nahoru) – pro Insert
Nový prvek vložíš na **konec**, pak ho probubláváš nahoru porovnáváním s rodičem.

```
Vložení 95:
     90              90              95
    /  \   →        /  \    →      /  \
  70    80        70    95       70    90
 / \   /         / \   /        / \   /
40 50 95←sem    40 50 80       40 50 80
```

#### SiftDown (propadnutí dolů) – pro ExtractMax
Kořen nahradíš posledním prvkem, pak ho necháš propadnout – swapuješ s **větším** dítětem (Max-Heap).

```
Odebrání 95:
     95             40              90
    /  \    →      /  \     →      /  \
  70    90       70    90        70    40
 / \            / \             / \
40 50          50  ×           50  ×
```

**Pravidlo:** Vždy swap s VĚTŠÍM dítětem (Max-Heap), protože nový rodič musí být >= obou dětí.

#### Implementace SiftDown:
```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)
// "n" = aktuální velikost haldy, "i" = index prvku k opravení
static void SiftDown(int[] pole, int n, int i)
{
    int largest = i;          // Předpokládáme, že rodič je největší
    int left = 2 * i + 1;    // Levé dítě
    int right = 2 * i + 2;   // Pravé dítě

    // Je levé dítě větší než rodič?
    if (left < n && pole[left] > pole[largest])
        largest = left;

    // Je pravé dítě větší než dosud největší?
    if (right < n && pole[right] > pole[largest])
        largest = right;

    // Pokud rodič NENÍ největší → swap a pokračuj dolů
    if (largest != i)
    {
        int temp = pole[i];
        pole[i] = pole[largest];
        pole[largest] = temp;

        SiftDown(pole, n, largest); // Rekurzivně oprav podstrom
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
// Iterativní verze - bez rekurze, šetří stack
static void SiftDown(int[] arr, int n, int i)
{
    while (true)
    {
        int largest = i;
        int l = 2 * i + 1, r = 2 * i + 2;

        if (l < n && arr[l] > arr[largest]) largest = l;
        if (r < n && arr[r] > arr[largest]) largest = r;

        if (largest == i) break;

        (arr[i], arr[largest]) = (arr[largest], arr[i]); // tuple swap
        i = largest;
    }
}
```

#### Přehled operací:
| Operace | Co dělá | Směr | Složitost |
|---------|---------|------|-----------|
| SiftUp | Probublání nahoru | ↑ | O(log n) |
| SiftDown | Propadnutí dolů | ↓ | O(log n) |
| Insert | Vlož na konec + SiftUp | ↑ | O(log n) |
| ExtractMax | Odeber kořen + SiftDown | ↓ | O(log n) |

---

### Bod 4: Uložení haldy v poli

Úplný binární strom lze mapovat do pole **bez pointerů** – stačí aritmetika indexů.

```
Strom:                          Pole:
            90                  Index:   0    1    2    3    4    5
           /  \                 Hodnota: [90] [70] [80] [40] [50] [60]
         70    80
        / \   /
      40  50 60
```

**Vzorce (index od 0):**
```
Pro prvek na indexu i:
  Rodič:           (i - 1) / 2     (celočíselné dělení)
  Levé dítě:       2 * i + 1
  Pravé dítě:      2 * i + 2
  Je list:         2 * i + 1 >= n
  Poslední rodič:  (n - 2) / 2  =  n/2 - 1
```

**Ověření na příkladu (n=6):**
```
Prvek 70 (i=1):
  Rodič:      (1-1)/2 = 0 → 90 ✅
  Levé dítě:  2*1+1 = 3  → 40 ✅
  Pravé dítě: 2*1+2 = 4  → 50 ✅
```

**Proč poslední rodič = (n-2)/2:**
Odvození: poslední prvek má index `n-1`, jeho rodič = `(n-1-1)/2 = (n-2)/2`.

**Parametr `n` v SiftDown:** Pole má vždy `pole.Length` prvků, ale halda zabírá jen prvních `n` prvků. Zbytek je již setříděná část. Podmínka `left < n` zabraňuje sahat do setříděné oblasti.

```
[60, 50, 40, | 70, 80, 90]
|__halda__|    |_setříděno_|
    n = 3       mimo haldu
```

**Výhody uložení v poli:**
- Žádné pointery → šetří paměť
- Cache-friendly → prvky za sebou v paměti
- O(1) navigace → jen aritmetika

---

### Bod 5: Algoritmus Heap Sort po krocích

#### FÁZE 1: BuildHeap – postav Max-Heap z pole

Od posledního rodiče voláš SiftDown směrem ke kořeni. Listy přeskakuješ.

```
Vstup: [40, 10, 80, 50, 90, 60]

Jako strom:            Po BuildHeap:
        40                     90
       /  \                   /  \
     10    80               50    80
    / \   /                / \   /
  50  90 60              40  10 60

Pole po BuildHeap: [90, 50, 80, 40, 10, 60]
```

#### FÁZE 2: Opakované ExtractMax

1. Swap kořen (max) ↔ poslední prvek haldy
2. Zmenši n o 1
3. SiftDown(0)

```
[90, 50, 80, 40, 10, 60]  n=6  → swap 90↔60, SiftDown
[80, 50, 60, 40, 10,|90]  n=5  → swap 80↔10, SiftDown
[60, 50, 10, 40,|80, 90]  n=4  → swap 60↔40, SiftDown
[50, 40, 10,|60, 80, 90]  n=3  → swap 50↔10, SiftDown
[40, 10,|50, 60, 80, 90]  n=2  → swap 40↔10
[10,|40, 50, 60, 80, 90]  n=1  → HOTOVO!

Výsledek: [10, 40, 50, 60, 80, 90] ✅
```

#### Kompletní implementace:

```csharp
// ✅ VERZE A - MATURITNÍ (Must Have)

static void HeapSort(int[] pole)
{
    int n = pole.Length;

    // FÁZE 1: BuildHeap - postav max-heap
    for (int i = n / 2 - 1; i >= 0; i--)
    {
        SiftDown(pole, n, i);
    }

    // FÁZE 2: Opakovaně vytahuj maximum
    for (int i = n - 1; i > 0; i--)
    {
        // Swap kořen (maximum) s posledním prvkem haldy
        int temp = pole[0];
        pole[0] = pole[i];
        pole[i] = temp;

        // Oprav haldu (zmenšenou o 1)
        SiftDown(pole, i, 0);  // "i" je nové n!
    }
}

static void SiftDown(int[] pole, int n, int i)
{
    int largest = i;
    int left = 2 * i + 1;
    int right = 2 * i + 2;

    if (left < n && pole[left] > pole[largest])
        largest = left;
    if (right < n && pole[right] > pole[largest])
        largest = right;

    if (largest != i)
    {
        int temp = pole[i];
        pole[i] = pole[largest];
        pole[largest] = temp;
        SiftDown(pole, n, largest);
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (Nice to Have)
static void HeapSort(int[] arr)
{
    int n = arr.Length;
    for (int i = n / 2 - 1; i >= 0; i--)
        SiftDown(arr, n, i);
    for (int i = n - 1; i > 0; i--)
    {
        (arr[0], arr[i]) = (arr[i], arr[0]);
        SiftDown(arr, i, 0);
    }
}
```

---

### Bod 6: Časová a paměťová složitost

**Časová složitost:**

| Fáze | Složitost | Proč |
|------|-----------|------|
| BuildHeap | O(n) | Většina prvků dole, propadá málo úrovní |
| ExtractMax (n-1×) | O(n log n) | Každý krok = SiftDown přes celou výšku |
| **Celkem** | **O(n log n)** | O(n) + O(n log n) = O(n log n) |

| Případ | Složitost |
|--------|-----------|
| Nejlepší | O(n log n) |
| Průměrný | O(n log n) |
| Nejhorší | O(n log n) |

**Paměťová složitost: O(1)** – in-place, jen pár pomocných proměnných.

**Srovnání všech třídění:**
```
Algoritmus    | Nejlepší   | Průměr     | Nejhorší   | Paměť   | Stabilní?
──────────────|────────────|────────────|────────────|─────────|──────────
Insert Sort   | O(n)       | O(n²)      | O(n²)      | O(1)    | ✅ Ano
Select Sort   | O(n²)      | O(n²)      | O(n²)      | O(1)    | ❌ Ne
Bubble Sort   | O(n)       | O(n²)      | O(n²)      | O(1)    | ✅ Ano
Merge Sort    | O(n log n) | O(n log n) | O(n log n) | O(n)    | ✅ Ano
Quick Sort    | O(n log n) | O(n log n) | O(n²)      | O(log n)| ❌ Ne
Heap Sort     | O(n log n) | O(n log n) | O(n log n) | O(1)    | ❌ Ne
```

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

- **Halda ≠ BVS** – v haldě nejsou sourozenci seřazení, jen rodič vs dítě
- **Max-Heap pro vzestupné třídění** – maximum dáváš na konec, proto výsledek roste
- **BuildHeap je O(n), ne O(n log n)** – častá otázka u ústní zkoušky
- **Parametr `n` v SiftDown** – označuje velikost haldy, ne délku pole
- **Poslední rodič = (n-2)/2** – odvození: rodič posledního prvku (n-1)
- **SiftDown swapuje s VĚTŠÍM dítětem** (Max-Heap) – nový rodič musí být >= obou dětí
- **Heap Sort NENÍ stabilní** – swap kořene s koncem mění pořadí rovných prvků
- **Proč se v praxi víc používá Quick Sort?** – cache-friendly (sekvenční přístup), Heap Sort skáče po poli

---

## 🚀 Senior Tip

V C# existuje `PriorityQueue<TElement, TPriority>` (od .NET 6), která interně používá haldu. V praxi nemusíš haldu psát ručně:

```csharp
var pq = new PriorityQueue<string, int>();
pq.Enqueue("Urgent", 1);
pq.Enqueue("Low", 10);
pq.Enqueue("Medium", 5);

while (pq.Count > 0)
    Console.WriteLine(pq.Dequeue()); // Urgent, Medium, Low
```

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 9 (Stromy)** – halda je speciální binární strom, zmínka o haldě je přímo v otázce 9
- **Otázka 7 (Složitost)** – analýza O(n) pro BuildHeap, amortizovaná složitost
- **Otázka 10-12 (Ostatní třídění)** – srovnání algoritmů, tabulka složitostí
- **Otázka 25 (Dijkstra)** – Dijkstrův algoritmus používá minimovou haldu (priority queue)
- **Otázka 5 (Rekurze)** – SiftDown lze napsat rekurzivně i iterativně
