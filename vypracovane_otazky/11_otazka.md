# Zápisky: Otázka č. 11 - BUBBLE SORT. MERGE SORT.

---

## Obsah

1. [Motivace pro třídění](#1-motivace-pro-třídění)
2. [Bubble Sort - princip a popis](#2-bubble-sort---princip-a-popis)
3. [Bubble Sort - vizualizace](#3-bubble-sort---vizualizace)
4. [Bubble Sort - složitost a vlastnosti](#4-bubble-sort---složitost-a-vlastnosti)
5. [Optimalizace Bubble Sortu (Cocktail Sort)](#5-optimalizace-bubble-sortu-cocktail-sort)
6. [Merge Sort - princip a popis](#6-merge-sort---princip-a-popis)
7. [Merge Sort - vizualizace](#7-merge-sort---vizualizace)
8. [Merge Sort - složitost a vlastnosti](#8-merge-sort---složitost-a-vlastnosti)
9. [Princip Rozděl a panuj (Divide & Conquer)](#9-princip-rozděl-a-panuj-divide--conquer)
10. [Porovnání Bubble Sort vs Merge Sort](#10-porovnání-bubble-sort-vs-merge-sort)
11. [Aplikace v praxi (TimSort, externí třídění, paralelizace)](#11-aplikace-v-praxi)
12. [Maturitní chytáky](#12-maturitní-chytáky)
13. [Klíčové pojmy](#13-klíčové-pojmy)

---

## 1. Motivace pro třídění

Třídění (sorting) je proces uspořádání prvků posloupnosti podle nějakého **klíče** a relace **úplného uspořádání** (totální order). Klíč může být sám prvek (čísla, řetězce) nebo jeho atribut (jméno u objektu Osoba). Relace musí být:

- **reflexivní** - `a ≤ a`,
- **antisymetrická** - z `a ≤ b` a `b ≤ a` plyne `a = b`,
- **tranzitivní** - z `a ≤ b` a `b ≤ c` plyne `a ≤ c`,
- **totální** - pro každá dvě `a, b` platí `a ≤ b` nebo `b ≤ a`.

Pokud chybí totalita (např. částečné uspořádání u dělitelnosti), používá se **topologické třídění** (viz Ot. 18 - DAG).

### Proč se zabýváme třídícími algoritmy?

- **Vyhledávání** - v setříděném poli běží binární vyhledávání v O(log n) místo O(n).
- **Detekce duplicit** - duplicity jsou v setříděné posloupnosti vedle sebe, hledání je O(n) místo O(n²).
- **Group-by operace** - SQL `GROUP BY`, agregace, prefixové součty.
- **Slučování dat** - merge dvou setříděných polí je O(n), nesetříděných O(n·m).
- **Mediány, percentily, kvantily** - O(1) přístup k pozičním statistikám po setřídění.
- **Heuristiky a aproximační algoritmy** - greedy algoritmy (Kruskal, Huffman) potřebují setříděné vstupy.

### Klasifikace třídících algoritmů (rekapitulace z Ot. 10)

| Vlastnost | Význam |
|-----------|--------|
| **Stabilní** | Zachovává relativní pořadí prvků se stejným klíčem |
| **In-place** | Vyžaduje O(1) extra paměti (mimo vstup) |
| **Adaptivní** | Rychlejší na téměř setříděných datech |
| **Comparison-based** | Používá pouze porovnávací operace `<`, `>`, `=` |
| **Online** | Lze třídit data přicházející postupně |

**Bubble Sort:** stabilní, in-place, adaptivní (s optimalizací), comparison-based, offline.
**Merge Sort:** stabilní, NENÍ in-place, NENÍ adaptivní, comparison-based, offline (existuje i online varianta).

### Spodní mez pro comparison-based algoritmy

Dokáže se přes rozhodovací strom (decision tree), že žádný porovnávací algoritmus nemůže být rychlejší než **Ω(n log n)**. Bubble Sort tuto mez nedosahuje - patří mezi kvadratické algoritmy. Merge Sort jí naopak dosahuje optimálně. Nesrovnávací algoritmy (Counting Sort, Radix Sort, Bucket Sort) tuto mez obcházejí - viz Ot. 13.

---

## 2. Bubble Sort - princip a popis

### Idea

Bubble Sort (česky *bublinkové třídění*) funguje na principu **probublávání**. V každém průchodu polem porovnává sousední dvojice a prohazuje je, pokud jsou ve špatném pořadí. Po jednom úplném průchodu se největší prvek "probublá" na konec - jako bublina ve vodě stoupá k hladině.

### Invariant algoritmu

Po `i`-tém průchodu platí:

```
Posledních i prvků pole je již na své finální pozici (a jsou setříděné).
```

To znamená, že po `n-1` průchodech je celé pole setříděné. Vnitřní cyklus se v každé iteraci může zkrátit o jednu pozici (`n - 1 - i`), protože poslední prvky už jsou na svém místě.

### Algoritmus krok za krokem

```
1. Pro i od 0 do n-2:                  (vnější cyklus - průchody)
   a) Pro j od 0 do n-2-i:             (vnitřní cyklus - posun bubliny)
      - Pokud pole[j] > pole[j+1]:
          prohoď pole[j] a pole[j+1]
   b) Po průchodu: největší z prvních n-i prvků je na pozici n-1-i
2. Po n-1 průchodech je pole setříděné
```

### Kód (základní verze)

```csharp
static void BubbleSort(int[] pole)
{
    int n = pole.Length;

    for (int i = 0; i < n - 1; i++)              // n-1 průchodů
    {
        for (int j = 0; j < n - 1 - i; j++)       // zkracující se okno
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
```

### Optimalizovaná verze s `early exit`

Pokud v některém průchodu nedojde k žádné výměně, znamená to, že pole je setříděné a můžeme skončit. Tím dosáhneme nejlepšího případu O(n) pro již setříděné pole.

```csharp
static void BubbleSortOptimized(int[] pole)
{
    int n = pole.Length;
    bool swapped;

    for (int i = 0; i < n - 1; i++)
    {
        swapped = false;

        for (int j = 0; j < n - 1 - i; j++)
        {
            if (pole[j] > pole[j + 1])
            {
                (pole[j], pole[j + 1]) = (pole[j + 1], pole[j]);   // tuple swap
                swapped = true;
            }
        }

        if (!swapped)                              // už nebylo co prohazovat
            break;
    }
}
```

**Generická varianta** s `IComparable<T>`:

```csharp
static void BubbleSort<T>(T[] pole) where T : IComparable<T>
{
    int n = pole.Length;
    for (int i = 0; i < n - 1; i++)
    {
        bool swapped = false;
        for (int j = 0; j < n - 1 - i; j++)
        {
            if (pole[j].CompareTo(pole[j + 1]) > 0)
            {
                (pole[j], pole[j + 1]) = (pole[j + 1], pole[j]);
                swapped = true;
            }
        }
        if (!swapped) break;
    }
}
```

---

## 3. Bubble Sort - vizualizace

### Detailní průchod pro pole `[5, 1, 4, 2, 8]`

```
Počáteční stav: [5, 1, 4, 2, 8]
─────────────────────────────────────────────────────────────────
PRŮCHOD 1 (i = 0): největší prvek (8) probublá na konec
─────────────────────────────────────────────────────────────────
Krok 1.1: porovnej 5 vs 1   → 5 > 1   → SWAP
[5, 1, 4, 2, 8] → [1, 5, 4, 2, 8]
 ^  ^
Krok 1.2: porovnej 5 vs 4   → 5 > 4   → SWAP
[1, 5, 4, 2, 8] → [1, 4, 5, 2, 8]
    ^  ^
Krok 1.3: porovnej 5 vs 2   → 5 > 2   → SWAP
[1, 4, 5, 2, 8] → [1, 4, 2, 5, 8]
       ^  ^
Krok 1.4: porovnej 5 vs 8   → 5 < 8   → bez změny
[1, 4, 2, 5, 8] → [1, 4, 2, 5, 8]
          ^  ^

Stav: [1, 4, 2, 5, 8]   8 je definitivně na své pozici
                    └ setříděno

─────────────────────────────────────────────────────────────────
PRŮCHOD 2 (i = 1): druhý největší (5) probublá
─────────────────────────────────────────────────────────────────
Krok 2.1: 1 vs 4 → bez změny
Krok 2.2: 4 vs 2 → SWAP   →  [1, 2, 4, 5, 8]
Krok 2.3: 4 vs 5 → bez změny

Stav: [1, 2, 4, 5, 8]   5 je na své pozici

─────────────────────────────────────────────────────────────────
PRŮCHOD 3 (i = 2): kontrola
─────────────────────────────────────────────────────────────────
Krok 3.1: 1 vs 2 → bez změny
Krok 3.2: 2 vs 4 → bez změny

(s optimalizací: swapped = false → break)
Stav: [1, 2, 4, 5, 8]   HOTOVO
```

### Shrnutí průchodů

```
Průchod 1: [5, 1, 4, 2, 8] → [1, 4, 2, 5, 8]    ← 8 probublalo
Průchod 2: [1, 4, 2, 5, 8] → [1, 2, 4, 5, 8]    ← 5 probublalo
Průchod 3: [1, 2, 4, 5, 8] → [1, 2, 4, 5, 8]    ← žádný swap → konec
```

### Vizualizace "stoupající bubliny"

```
Iniciální:   5   1   4   2   8           (5 je největší na začátku)
             v   |   |   |   |
Po 1. iter.: 1   5   4   2   8           (5 se posunula)
             |   v   |   |   |
Po 2. iter.: 1   4   5   2   8           (5 stoupá dál)
             |   |   v   |   |
Po 3. iter.: 1   4   2   5   8           (5 narazila na 8, dál nestoupá)
                                            (ve výsledku ale 8 je nejvýše)
```

---

## 4. Bubble Sort - složitost a vlastnosti

### Časová složitost

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší (best case)** | O(n) | Pole je setříděné a používáme optimalizaci `early exit` |
| **Nejlepší (bez optimalizace)** | O(n²) | Vždy provede všech n-1 průchodů |
| **Průměrný (average)** | O(n²) | Náhodné pořadí, ~n²/4 swapů |
| **Nejhorší (worst case)** | O(n²) | Pole je setříděné opačně (každý prvek se musí přesunout) |

**Odvození O(n²):**

Vnější cyklus provede `n-1` iterací. Vnitřní cyklus v prvním průchodu provede `n-1`, v druhém `n-2`, ..., v posledním `1` porovnání. Celkový počet porovnání:

```
(n-1) + (n-2) + ... + 1 = n·(n-1)/2 ≈ n²/2 → O(n²)
```

Počet **swapů** v nejhorším případě je stejný `n·(n-1)/2`. To je důležité, protože swap je relativně drahá operace (3 přiřazení), takže Bubble Sort patří mezi nejpomalejší kvadratické algoritmy v praxi.

### Paměťová složitost

- **O(1)** - pouze pomocná proměnná `temp` pro swap a flag `swapped`.
- **In-place** - pracuje přímo nad vstupním polem.

### Vlastnosti

| Vlastnost | Hodnota | Vysvětlení |
|-----------|---------|------------|
| **Stabilní** | Ano | Prohazujeme jen pokud `pole[j] > pole[j+1]`, ne při rovnosti |
| **In-place** | Ano | Žádné pomocné pole |
| **Adaptivní** | Ano (s optim.) | Na téměř setříděném poli rychlé |
| **Comparison-based** | Ano | Používá pouze `>` |
| **Online** | Ne | Vyžaduje celý vstup |
| **Paralelizovatelný** | Špatně | Sousední swapy mají závislosti |

### Kdy Bubble Sort použít

- **Výuka** - nejjednodušší třídicí algoritmus, didakticky vhodný.
- **Velmi malá data** (< 10-20 prvků) - režie složitějších algoritmů se nevyplatí.
- **Téměř setříděná data** - s optimalizací běží v O(n) (například při údržbě setříděného pole po malé změně).
- **Embedded systémy** - krátký kód, nízká paměťová náročnost, žádná rekurze.

### Kdy Bubble Sort NEPOUŽÍT

- **Jakákoli produkční aplikace** s n > 100 prvků.
- **Velká data** - O(n²) je naprosto nepoužitelné pro n = 10⁶.

---

## 5. Optimalizace Bubble Sortu (Cocktail Sort)

### Cocktail Shaker Sort (Bidirectional Bubble Sort)

Klasický Bubble Sort vždy probublává směrem vpravo - velké prvky se posouvají rychle, ale malé prvky se posouvají vlevo jen po jedné pozici za průchod. **Cocktail Sort** probublává střídavě v obou směrech:

```csharp
static void CocktailSort(int[] pole)
{
    int left = 0;
    int right = pole.Length - 1;
    bool swapped = true;

    while (swapped)
    {
        swapped = false;

        for (int i = left; i < right; i++)        // doprava
            if (pole[i] > pole[i + 1])
            {
                (pole[i], pole[i + 1]) = (pole[i + 1], pole[i]);
                swapped = true;
            }

        if (!swapped) break;
        right--;

        swapped = false;

        for (int i = right; i > left; i--)        // doleva
            if (pole[i - 1] > pole[i])
            {
                (pole[i - 1], pole[i]) = (pole[i], pole[i - 1]);
                swapped = true;
            }

        left++;
    }
}
```

**Výhody:** rychlejší na polích s "tureckými prvky" (malý prvek u konce) - klasický Bubble Sort by je tahal pomalu.
**Stále O(n²)** - jen menší konstanta.

---

## 6. Merge Sort - princip a popis

### Idea

Merge Sort (česky *třídění slučováním*) je klasická aplikace paradigmatu **Rozděl a panuj** (Divide & Conquer). Vychází z pozorování, že sloučit dvě **již setříděná pole** lze v lineárním čase O(n). Pokud tedy umíme rekurzivně setřídit poloviny pole a poté je slít, dostaneme celkovou složitost O(n log n).

### Tři kroky paradigmatu

1. **Rozděl (Divide):** rozděl problém na menší podproblémy. Pole délky `n` rozděl na dvě poloviny délky `n/2`.
2. **Panuj (Conquer):** vyřeš podproblémy rekurzivně. Setřiď obě poloviny voláním sebe sama. Základní případ: pole délky 1 nebo 0 je již setříděné.
3. **Kombinuj (Combine):** zkombinuj řešení podproblémů. Slij obě setříděné poloviny do jednoho setříděného pole funkcí `Merge`.

### Operace MERGE (slévání)

`Merge(A, B)` slije dvě setříděná pole `A` a `B` do jednoho setříděného pole `C`:

```
1. Inicializuj indexy i = 0, j = 0, k = 0.
2. Dokud i < len(A) a j < len(B):
     pokud A[i] <= B[j]:
        C[k++] = A[i++]      ← <= zaručuje stabilitu (preferuje levou)
     jinak:
        C[k++] = B[j++]
3. Zkopíruj zbytek A (pokud zbyl).
4. Zkopíruj zbytek B (pokud zbyl).
```

### Pseudokód celého Merge Sortu

```
MERGE_SORT(pole, left, right):
   if left >= right: return                    // 1 nebo 0 prvků = setříděno
   mid = left + (right - left) / 2              // bezpečný výpočet středu
   MERGE_SORT(pole, left, mid)                  // setřiď levou půlku
   MERGE_SORT(pole, mid + 1, right)             // setřiď pravou půlku
   MERGE(pole, left, mid, right)                // slij obě půlky
```

### Kód

```csharp
static void MergeSort(int[] pole, int left, int right)
{
    if (left >= right) return;                   // základní případ

    int mid = left + (right - left) / 2;          // bezpečné proti overflow

    MergeSort(pole, left, mid);                   // rekurze - levá půlka
    MergeSort(pole, mid + 1, right);              // rekurze - pravá půlka
    Merge(pole, left, mid, right);                // sloučení
}

static void Merge(int[] pole, int left, int mid, int right)
{
    int n1 = mid - left + 1;
    int n2 = right - mid;

    int[] leva  = new int[n1];
    int[] prava = new int[n2];

    for (int x = 0; x < n1; x++) leva[x]  = pole[left + x];
    for (int x = 0; x < n2; x++) prava[x] = pole[mid + 1 + x];

    int i = 0, j = 0, k = left;

    while (i < n1 && j < n2)
    {
        if (leva[i] <= prava[j])                  // <= → stabilita
            pole[k++] = leva[i++];
        else
            pole[k++] = prava[j++];
    }

    while (i < n1) pole[k++] = leva[i++];          // zbytek levé části
    while (j < n2) pole[k++] = prava[j++];         // zbytek pravé části
}

// Volání: MergeSort(pole, 0, pole.Length - 1);
```

### Iterativní (bottom-up) varianta

Rekurzivní MergeSort lze přepsat na iterativní algoritmus, který postupně slévá podpole velikosti 1, 2, 4, 8, ... Výhoda: žádná hloubka zásobníku, vhodné pro velmi velká pole nebo embedded systémy bez podpory hluboké rekurze.

```csharp
static void MergeSortIterative(int[] pole)
{
    int n = pole.Length;

    for (int width = 1; width < n; width *= 2)    // velikost slévaných bloků
    {
        for (int left = 0; left < n; left += 2 * width)
        {
            int mid   = Math.Min(left + width - 1, n - 1);
            int right = Math.Min(left + 2 * width - 1, n - 1);
            Merge(pole, left, mid, right);
        }
    }
}
```

### Funkcionální (LINQ) varianta

```csharp
static int[] MergeSortLinq(int[] pole)
{
    if (pole.Length <= 1) return pole;

    int mid = pole.Length / 2;
    var leva  = MergeSortLinq(pole.Take(mid).ToArray());
    var prava = MergeSortLinq(pole.Skip(mid).ToArray());

    return MergeArrays(leva, prava);
}

static int[] MergeArrays(int[] a, int[] b)
{
    var result = new List<int>(a.Length + b.Length);
    int i = 0, j = 0;
    while (i < a.Length && j < b.Length)
        result.Add(a[i] <= b[j] ? a[i++] : b[j++]);
    result.AddRange(a.Skip(i));
    result.AddRange(b.Skip(j));
    return result.ToArray();
}
```

Tato verze je čistší, ale méně efektivní kvůli mnoha alokacím polí.

---

## 7. Merge Sort - vizualizace

### Strom rekurze pro pole `[38, 27, 43, 3, 9, 82, 10]`

```
                FÁZE ROZDĚLOVÁNÍ (top-down)
═══════════════════════════════════════════════════════════════

                [38, 27, 43, 3, 9, 82, 10]
                           |
              +------------+------------+
              |                         |
        [38, 27, 43, 3]          [9, 82, 10]
              |                         |
        +-----+-----+             +-----+-----+
        |           |             |           |
    [38, 27]    [43, 3]       [9, 82]      [10]
        |           |             |
     +--+--+     +--+--+      +--+--+
     |     |     |     |      |     |
   [38]  [27]  [43]  [3]    [9]   [82]

                FÁZE SLUČOVÁNÍ (bottom-up)
═══════════════════════════════════════════════════════════════

   [38]  [27]  [43]  [3]    [9]   [82]     [10]
     |     |     |     |      |     |        |
     +--+--+     +--+--+      +--+--+        |
        v           v            v           |
    [27, 38]    [3, 43]       [9, 82]      [10]
        |           |             |           |
        +-----+-----+             +-----+-----+
              v                         v
        [3, 27, 38, 43]          [9, 10, 82]
              |                         |
              +------------+------------+
                           v
                [3, 9, 10, 27, 38, 43, 82]
                       SETŘÍDĚNO
```

### Detail jednoho MERGE kroku

Sloučení `[27, 38]` a `[3, 43]`:

```
Levá:  [27, 38]     i = 0
Pravá: [ 3, 43]     j = 0
Výstup: [_, _, _, _]  k = 0

Krok 1: 27 vs 3   → 3 < 27   → ber 3  z pravé
        [3, _, _, _]                  j = 1

Krok 2: 27 vs 43  → 27 < 43  → ber 27 z levé
        [3, 27, _, _]                 i = 1

Krok 3: 38 vs 43  → 38 < 43  → ber 38 z levé
        [3, 27, 38, _]                i = 2 (levá vyčerpaná)

Krok 4: levá prázdná → dokopíruj zbytek pravé (43)
        [3, 27, 38, 43]               HOTOVO
```

---

## 8. Merge Sort - složitost a vlastnosti

### Časová složitost

| Případ | Složitost |
|--------|-----------|
| **Nejlepší** | O(n log n) |
| **Průměrný** | O(n log n) |
| **Nejhorší** | O(n log n) |

**Merge Sort má vždy stejnou složitost** - na rozdíl od Bubble Sortu, Insert Sortu nebo Quick Sortu nemá patologický nejhorší případ. Je to **deterministický** algoritmus z hlediska počtu operací.

### Odvození O(n log n) z rekurence

Rekurzivní rovnice:

```
T(n) = 2·T(n/2) + Θ(n)
       └─┬─┘    └─┬─┘
        │         │
        │         └ čas MERGE (lineární)
        └ dvě poloviční volání
```

Pomocí **Master Theorem** (a = 2, b = 2, f(n) = n, log_b(a) = 1) získáme `T(n) = Θ(n log n)`. Intuitivně: strom rekurze má `log₂ n` úrovní, na každé úrovni se zpracuje celkem `n` prvků v rámci operací MERGE.

```
Úroveň 0: 1 pole velikosti n          → práce: n
Úroveň 1: 2 pole velikosti n/2        → práce: 2·(n/2) = n
Úroveň 2: 4 pole velikosti n/4        → práce: 4·(n/4) = n
...
Úroveň log₂(n): n polí velikosti 1    → práce: n
─────────────────────────────────────
Celkem: n · log₂(n) = O(n log n)
```

### Paměťová složitost

- **O(n)** - pomocná pole `leva` a `prava` při operaci MERGE.
- **O(log n)** - zásobník rekurzivních volání.
- Celkem: **O(n)**.

Existují i in-place varianty Merge Sortu, ale jsou složitější a v praxi pomalejší.

### Vlastnosti

| Vlastnost | Hodnota | Vysvětlení |
|-----------|---------|------------|
| **Stabilní** | Ano | Operace MERGE používá `<=` (preferuje levou stranu při rovnosti) |
| **In-place** | Ne | Vyžaduje O(n) extra paměti |
| **Adaptivní** | Ne | Vždy dělá O(n log n) operací |
| **Comparison-based** | Ano | Používá pouze `<=` |
| **Online** | Ne (klasická), existují varianty | Streaming merge sort dokáže zpracovat data po částech |
| **Paralelizovatelný** | Výborně | Obě poloviny lze třídit nezávisle (viz Parallel.Invoke) |
| **Externí** | Ano | Ideální pro data nevejdoucí se do RAM |

### Kdy Merge Sort použít

- **Velká data** - garantovaný výkon O(n log n) bez patologických případů.
- **Stabilní třídění** - když záleží na zachování pořadí stejných klíčů (např. třídění tabulky podle více sloupců).
- **Spojové seznamy (linked lists)** - MergeSort lze implementovat pro linked listy s O(1) extra paměti, žádné kopírování.
- **Externí třídění** - třídění souborů, které se nevejdou do paměti.
- **Paralelní výpočty** - obě poloviny jsou nezávislé, ideální pro vícejádrové procesory.
- **Počítání inverzí** - klasická úloha řešená modifikací Merge Sortu.

### Kdy Merge Sort NEPOUŽÍT

- **Velmi malá pole** (< 10-20 prvků) - režie rekurze, hybridní algoritmy přepínají na Insert Sort.
- **Když je paměť kritická** - O(n) extra alokace může být překážka.
- **Když je QuickSort dostatečně dobrý** - v praxi má QuickSort menší konstantu (lepší lokalita cache).

---

## 9. Princip Rozděl a panuj (Divide & Conquer)

### Definice

**Divide & Conquer** je algoritmické paradigma, kde se problém řeší rekurzivně ve třech fázích:

1. **Divide** - rozděl problém na menší **nezávislé** podproblémy stejného typu.
2. **Conquer** - vyřeš podproblémy rekurzivně. Pokud jsou dostatečně malé, vyřeš je přímo (base case).
3. **Combine** - zkombinuj řešení podproblémů do řešení původního problému.

### Klíčový rozdíl od Dynamického programování (DP)

| Vlastnost | Divide & Conquer | Dynamické programování |
|-----------|------------------|------------------------|
| Podproblémy | Nezávislé | Překrývající se |
| Cache výsledků | Ne | Ano (memoizace) |
| Příklad | Merge Sort, QuickSort | Fibonacci, Knapsack |

### Známé algoritmy s D&C

| Algoritmus | Divide | Conquer | Combine |
|------------|--------|---------|---------|
| **Merge Sort** | Půl pole | Setřiď poloviny | MERGE |
| **Quick Sort** | Kolem pivota | Setřiď části | Nic (in-place) |
| **Binary Search** | Půl pole | Hledej v polovině | Vrať výsledek |
| **Strassen** (násobení matic) | Matice na 4 podmatice | 7× násobení | Sečti výsledky → O(n^2.81) |
| **FFT** | Sudé/liché koeficienty | Rekurzivní FFT | Butterfly → O(n log n) |
| **Karatsuba** (násobení čísel) | Půl bitů | 3× násobení | Sečti → O(n^1.58) |
| **Closest Pair of Points** | Půl roviny | Najdi v polovinách | Strip merge → O(n log n) |
| **Maximum Subarray** | Půl pole | Max v polovinách + přes střed | Max ze tří → O(n log n) |

### Výhody D&C

- **Snadná analýza** přes rekurenční rovnice a Master Theorem.
- **Paralelizace** - nezávislé podproblémy lze řešit paralelně.
- **Cache-friendly** - rekurzivní algoritmy často mají dobrou lokalitu paměti.
- **Často optimální** - mnoho problémů má dolní mez Ω(n log n), kterou D&C dosahuje.

### Nevýhody D&C

- **Režie rekurze** - každé volání má svou paměť na zásobníku.
- **Není always in-place** - rekombinace často vyžaduje pomocnou paměť.
- **Hluboká rekurze** - může způsobit StackOverflow při nevhodné implementaci.

---

## 10. Porovnání Bubble Sort vs Merge Sort

### Tabulka vlastností

| Vlastnost | Bubble Sort | Merge Sort |
|-----------|-------------|------------|
| **Časová - nejlepší** | O(n) (s optim.) | O(n log n) |
| **Časová - průměrná** | O(n²) | O(n log n) |
| **Časová - nejhorší** | O(n²) | O(n log n) |
| **Paměťová** | O(1) | O(n) |
| **Stabilní** | Ano | Ano |
| **In-place** | Ano | Ne |
| **Adaptivní** | Ano (s optim.) | Ne |
| **Paralelizace** | Špatná | Výborná |
| **Externí třídění** | Ne | Ano |
| **Snadnost implementace** | Triviální | Střední (rekurze) |
| **Použití v praxi** | Pouze výuka | Hojně (TimSort, externí třídění, linked lists) |

### Konkrétní časy pro různé velikosti vstupu

Pro velikost vstupu `n` při předpokládaných 10⁸ operací/sec:

| n | Bubble O(n²) operací | Bubble čas | Merge O(n log n) operací | Merge čas | Poměr |
|---|----------------------|------------|--------------------------|-----------|-------|
| 10 | 100 | < 1 µs | 33 | < 1 µs | 3× |
| 100 | 10 000 | 0,1 ms | 664 | < 0,01 ms | 15× |
| 1 000 | 1 000 000 | 10 ms | 9 966 | 0,1 ms | 100× |
| 10 000 | 100 000 000 | 1 s | 132 877 | 1,3 ms | 753× |
| 100 000 | 10¹⁰ | ~2 minuty | 1,66·10⁶ | 16 ms | ~7500× |
| 1 000 000 | 10¹² | ~3 hodiny | 2·10⁷ | 200 ms | ~50000× |

**Závěr:** pro produkční data jsou Bubble Sort a podobné kvadratické algoritmy zcela nepoužitelné.

---

## 11. Aplikace v praxi

### TimSort (Python, Java)

**TimSort** je hybridní stabilní algoritmus kombinující **Merge Sort** a **Insertion Sort**. Vytvořil ho Tim Peters v roce 2002 pro Python. Od té doby je defaultním algoritmem v:

- Python: `list.sort()` a `sorted()`
- Java: `Arrays.sort()` pro objekty (od JDK 7)
- Android, V8, Rust, Swift

**Princip:**
1. Identifikuj v poli již existující **runy** - po sobě jdoucí setříděné úseky (vzestupně/sestupně).
2. Krátké runy rozšiř pomocí Insertion Sortu na minimální délku (typicky 32 nebo 64).
3. Slévej runy stejně jako Merge Sort, ale chytře (galloping mode pro nestejné velikosti).

**Výhody:**
- O(n) na již setříděných datech (real-world data jsou často téměř setříděná).
- Stabilní.
- O(n log n) worst case.
- Dobře využívá cache.

### IntroSort (C++ STL, .NET)

**IntroSort** = QuickSort + HeapSort + InsertionSort. Používá ho:
- C++ STL: `std::sort()`
- .NET: `Array.Sort()` pro primitivní typy

Začíná QuickSortem, ale když hloubka rekurze přesáhne `2 · log₂(n)`, přepne na HeapSort (aby se vyhnul O(n²) worst-case QuickSortu). Pro malá podpole (< 16) přepne na Insertion Sort. Nestabilní.

### Externí třídění (External Sort)

Když data nevejdou do RAM, používá se **multiway external merge sort**:

1. **Rozděl** velký soubor na bloky vejdoucí se do paměti.
2. **Setřiď** každý blok v RAM (in-memory sort).
3. **Slij** všechny bloky pomocí k-way merge s prioritní frontou (heap).

Tento algoritmus používají databáze (PostgreSQL, MySQL, Oracle) pro `ORDER BY` velkých tabulek, MapReduce a Hadoop.

### Paralelizace Merge Sortu

Merge Sort se ideálně hodí pro paralelizaci - obě poloviny jsou nezávislé:

```csharp
static void ParallelMergeSort(int[] pole, int left, int right, int depth = 0)
{
    if (left >= right) return;

    int mid = left + (right - left) / 2;

    if (depth < 4 && (right - left) > 1024)        // paralelně jen do hloubky 4
    {
        Parallel.Invoke(
            () => ParallelMergeSort(pole, left, mid, depth + 1),
            () => ParallelMergeSort(pole, mid + 1, right, depth + 1)
        );
    }
    else
    {
        MergeSort(pole, left, mid);                 // sekvenčně
        MergeSort(pole, mid + 1, right);
    }

    Merge(pole, left, mid, right);
}
```

**Pozor:** omezujeme hloubku paralelizace (overhead vytváření vláken). Také paralelní MERGE je netriviální (existují algoritmy s O(log² n) hloubkou na PRAM).

### Počítání inverzí pomocí Merge Sortu

**Inverze** v poli je dvojice indexů `(i, j)` kde `i < j` ale `pole[i] > pole[j]`. Počet inverzí měří, jak "rozházené" je pole. Naivní algoritmus je O(n²), ale modifikovaný Merge Sort to umí v O(n log n):

```csharp
static long CountInversions(int[] pole, int left, int right)
{
    if (left >= right) return 0;

    int mid = left + (right - left) / 2;
    long count = 0;

    count += CountInversions(pole, left, mid);
    count += CountInversions(pole, mid + 1, right);
    count += MergeAndCount(pole, left, mid, right);

    return count;
}

static long MergeAndCount(int[] pole, int left, int mid, int right)
{
    int[] leva  = pole.Skip(left).Take(mid - left + 1).ToArray();
    int[] prava = pole.Skip(mid + 1).Take(right - mid).ToArray();

    int i = 0, j = 0, k = left;
    long invCount = 0;

    while (i < leva.Length && j < prava.Length)
    {
        if (leva[i] <= prava[j])
            pole[k++] = leva[i++];
        else
        {
            pole[k++] = prava[j++];
            invCount += leva.Length - i;            // klíčový krok!
        }
    }

    while (i < leva.Length) pole[k++] = leva[i++];
    while (j < prava.Length) pole[k++] = prava[j++];

    return invCount;
}
```

Tato úloha se objevuje v algoritmických soutěžích i v praxi (např. měření rozdílu mezi dvěma seznamy doporučení).

### Použití v .NET

- `Array.Sort(...)` - používá IntroSort pro primitivní typy.
- `List<T>.Sort()` - delegát na `Array.Sort`.
- `Enumerable.OrderBy()` (LINQ) - používá **stabilní** sort (původně QuickSort, později vylepšeno).
- `Span<T>.Sort()` - od .NET 5 dostupné pro spany.

```csharp
int[] pole = { 5, 2, 8, 1, 9 };
Array.Sort(pole);                              // IntroSort, in-place

var setridene = pole.OrderBy(x => x).ToList(); // LINQ, stabilní

Array.Sort(pole, (a, b) => b.CompareTo(a));    // sestupně přes Comparer
```

---

## 12. Maturitní chytáky

### Bubble Sort - časté chyby

**Špatné hranice vnitřního cyklu:**

```csharp
// CHYBA - vždy jde do konce (zbytečné iterace + pro j = n-1 přístup za pole)
for (int j = 0; j < n; j++)
    if (pole[j] > pole[j + 1]) ...                // IndexOutOfRange při j = n-1!

// SPRÁVNĚ
for (int j = 0; j < n - 1 - i; j++)
    if (pole[j] > pole[j + 1]) ...
```

**Zapomenutí optimalizace:**

Bez `early exit` má Bubble Sort vždy O(n²), i na setříděném poli. S optimalizací je nejlepší případ O(n).

**Porušení stability:**

```csharp
// CHYBA - prohazuje i při rovnosti, naruší stabilitu
if (pole[j] >= pole[j + 1]) swap(...);

// SPRÁVNĚ
if (pole[j] > pole[j + 1]) swap(...);
```

### Merge Sort - časté chyby

**Overflow při výpočtu středu:**

```csharp
// PROBLEM - pro velké left + right může přetéct int
int mid = (left + right) / 2;

// SPRÁVNĚ - matematicky ekvivalentní, bez overflow
int mid = left + (right - left) / 2;
```

V Javě tato chyba byla 9 let nezdetekována v `java.util.Arrays.binarySearch()` až do roku 2006.

**Zapomenutí dokopírovat zbytek:**

```csharp
// CHYBA - po hlavním while cyklu zůstanou nezpracované prvky
while (i < n1 && j < n2) { ... }
// kód končí - jenže zbytek z levé/pravé části chybí v původním poli!

// SPRÁVNĚ
while (i < n1) pole[k++] = leva[i++];
while (j < n2) pole[k++] = prava[j++];
```

**Porušení stability:**

```csharp
// CHYBA - při rovnosti bere pravou stranu, naruší stabilitu
if (leva[i] < prava[j])

// SPRÁVNĚ - <= zachová stabilitu (levá strana je v původním poli první)
if (leva[i] <= prava[j])
```

**Špatný základní případ:**

```csharp
// CHYBA - nikdy se nezastaví, nekonečná rekurze
if (left == right) return;                       // co když left > right (prázdné podpole)?

// SPRÁVNĚ - obecná podmínka pokrývající i prázdné podpole
if (left >= right) return;
```

### Typické otázky u ústní zkoušky

- **"Proč je Merge Sort vždy O(n log n)?"**
  Stromová struktura má log₂(n) úrovní, na každé je celkem O(n) práce při slévání. Master Theorem: T(n) = 2T(n/2) + Θ(n) = Θ(n log n).

- **"Je Bubble Sort někdy rychlejší než Merge Sort?"**
  Ano, pro velmi malá pole (< 10 prvků) kvůli režii rekurze a pro téměř setříděná pole s optimalizací (O(n)). V praxi se ale používá raději Insert Sort.

- **"Proč Merge Sort potřebuje O(n) extra paměti?"**
  Operace MERGE vytváří pomocná pole pro kopie obou polovin. In-place merge existuje, ale je složitý a v praxi pomalejší.

- **"Co je stabilní třídění a proč na něm záleží?"**
  Stabilní algoritmus zachovává relativní pořadí prvků se stejným klíčem. Důležité při třídění podle více kritérií (např. nejprve podle příjmení, pak podle věku - musí zůstat zachované předchozí pořadí).

- **"Proč Merge Sort není adaptivní?"**
  Bez ohledu na vstupní pořadí provádí vždy stejný počet rekurzivních volání a porovnání. TimSort tento problém řeší detekcí runů.

- **"Co je princip Rozděl a panuj a jaké algoritmy ho používají?"**
  Rozdělení problému na nezávislé podproblémy, jejich rekurzivní řešení a kombinace. Příklady: Merge Sort, Quick Sort, Binary Search, FFT, Strassen, Karatsuba.

- **"Jak se Merge Sort liší od Quick Sortu?"**
  Merge Sort je vždy O(n log n), stabilní, NENÍ in-place. Quick Sort je v průměru O(n log n), worst case O(n²), nestabilní, ALE in-place a v praxi rychlejší (lepší cache lokality).

### Kontrolní seznam při code review

- [ ] Bubble Sort: správná podmínka `j < n - 1 - i` ve vnitřním cyklu
- [ ] Bubble Sort: přítomnost `early exit` optimalizace
- [ ] Bubble Sort: `>` (ne `>=`) pro zachování stability
- [ ] Merge Sort: bezpečný výpočet středu `left + (right - left) / 2`
- [ ] Merge Sort: základní případ `if (left >= right) return;`
- [ ] Merge Sort: kopírování VŠECH zbývajících prvků po hlavní smyčce
- [ ] Merge Sort: `<=` (ne `<`) pro zachování stability
- [ ] Konzistentní indexování (inkluzivní `right` vs exkluzivní `right`)

---

## 13. Klíčové pojmy

- **Bubble Sort** - kvadratický porovnávací algoritmus založený na probublávání největších prvků na konec.
- **Probublávání** - princip, kdy se prvek pomocí sousedních výměn postupně přesouvá na své místo.
- **Cocktail Sort** - obousměrná varianta Bubble Sortu, střídá probublávání doprava a doleva.
- **Merge Sort** - stabilní porovnávací algoritmus s garantovanou složitostí O(n log n) postavený na paradigmatu Rozděl a panuj.
- **MERGE (slévání)** - lineární operace, která sloučí dvě setříděná pole do jednoho setříděného.
- **Divide & Conquer (Rozděl a panuj)** - paradigma rozdělení problému na nezávislé podproblémy, jejich rekurzivního řešení a kombinace výsledků.
- **Rekurence T(n) = 2T(n/2) + Θ(n)** - rekurzivní rovnice popisující složitost Merge Sortu, řešení Θ(n log n) přes Master Theorem.
- **Stabilita** - zachování relativního pořadí prvků se stejným klíčem.
- **In-place** - algoritmus pracující s O(1) extra pamětí.
- **Adaptivita** - schopnost algoritmu být rychlejší na téměř setříděných datech.
- **Early exit / swapped flag** - optimalizace ukončující Bubble Sort, pokud v průchodu nebyla žádná výměna.
- **Top-down rekurze** - klasická forma Merge Sortu, kde se nejprve dělí a pak slévá.
- **Bottom-up iterace** - iterativní varianta Merge Sortu slévající bloky velikosti 1, 2, 4, ... bez rekurze.
- **TimSort** - hybridní algoritmus (Merge Sort + Insertion Sort) s detekcí runů, defaultní v Pythonu a Javě.
- **IntroSort** - hybridní algoritmus (QuickSort + HeapSort + InsertionSort), defaultní v C++ STL a .NET pro primitivní typy.
- **Externí třídění (External Sort)** - třídění dat větších než RAM pomocí k-way merge.
- **Run** - po sobě jdoucí setříděný úsek v poli, využívaný v TimSortu.
- **Galloping mode** - optimalizace v TimSortu pro slévání run různých délek.
- **Master Theorem** - věta pro řešení rekurencí typu T(n) = a·T(n/b) + f(n).
- **Inverze** - dvojice indexů (i, j) s i < j a a[i] > a[j]; počet inverzí lze najít v O(n log n) modifikovaným Merge Sortem.
- **Comparison-based** - algoritmus pracující pouze přes porovnání prvků; spodní mez Ω(n log n).
- **Hluboká rekurze (deep recursion)** - riziko StackOverflow při nevhodné implementaci, řešení: iterativní varianta nebo tail-call optimalizace.
- **k-way merge** - slévání k setříděných sekvencí najednou pomocí prioritní fronty (heap).

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| Ot. 5 - Rekurze | Merge Sort jako klasický rekurzivní algoritmus |
| Ot. 7 - Složitost | Master Theorem, analýza O(n log n) |
| Ot. 9 - Stromy | Heap pro k-way merge, decision tree pro spodní mez |
| Ot. 10 - Insert Sort, Select Sort | Porovnání jednoduchých O(n²) algoritmů, hybridní algoritmy |
| Ot. 12 - Quick Sort | Další Divide & Conquer algoritmus |
| Ot. 13 - Counting/Radix Sort | Nesrovnávací algoritmy překonávající Ω(n log n) |
| Ot. 15 - Rozděl a panuj | Merge Sort jako ukázkový příklad paradigmatu |
| Ot. 18 - Grafové algoritmy | Topologické třídění jako rozšíření třídění na DAG |
