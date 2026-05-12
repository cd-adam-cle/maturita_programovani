# Zápisky: Otázka č. 15 - Rozděl a panuj. Dynamické programování. Backtracking.

---

## Obsah

1. [Algoritmická paradigmata - úvod](#1-algoritmická-paradigmata---úvod)
2. [Rozděl a panuj (Divide & Conquer)](#2-rozděl-a-panuj-divide--conquer)
3. [Příklady D&C algoritmů](#3-příklady-dc-algoritmů)
4. [Master Theorem](#4-master-theorem)
5. [Dynamické programování (DP)](#5-dynamické-programování-dp)
6. [Optimální substruktura a překrývající se podproblémy](#6-optimální-substruktura-a-překrývající-se-podproblémy)
7. [Memoizace vs tabulace](#7-memoizace-vs-tabulace)
8. [Klasické DP úlohy](#8-klasické-dp-úlohy)
9. [Backtracking](#9-backtracking)
10. [Pruning a heuristiky](#10-pruning-a-heuristiky)
11. [Klasické backtracking úlohy](#11-klasické-backtracking-úlohy)
12. [Porovnání tří paradigmat](#12-porovnání-tří-paradigmat)
13. [Maturitní chytáky](#13-maturitní-chytáky)
14. [Klíčové pojmy](#14-klíčové-pojmy)

---

## 1. Algoritmická paradigmata - úvod

**Algoritmické paradigma** (technika návrhu algoritmů) je obecná strategie, kterou lze aplikovat na celé třídy problémů. Tři nejdůležitější patří **Rozděl a panuj**, **Dynamické programování** a **Backtracking**. Všechny tři využívají **rekurzi**, ale liší se způsobem, jak rozkládají problém na podproblémy.

### Přehled paradigmat

| Paradigma | Klíčový rys | Typická úloha | Typická složitost |
|-----------|-------------|---------------|-------------------|
| **Rozděl a panuj (D&C)** | Nezávislé podproblémy | Třídění, vyhledávání | O(n log n), O(log n) |
| **Dynamické programování (DP)** | Překrývající se podproblémy, optimum | Coin Change, Knapsack | O(n·m), polynomiální |
| **Backtracking** | Stavový prostor, ořezávání | N-Queens, Sudoku | O(b^d), exponenciální |
| **Greedy (hladový)** | Lokálně optimální volba | Huffman, Dijkstra | O(n log n) |
| **Branch & Bound** | Jako backtracking + horní/dolní mez | TSP, ILP | Exponenciální, ale často rychlé |

### Další techniky

- **Greedy (hladové algoritmy)** - v každém kroku zvolí lokálně nejlepší možnost; funguje pro problémy s vlastností "matroid" (Kruskal, Huffman).
- **Branch & Bound** - rozšíření backtrackingu o spodní/horní meze, používané pro optimalizační úlohy.
- **Randomizace** - využití náhody (Quick Sort, Las Vegas a Monte Carlo algoritmy).
- **Aproximace** - pro NP-těžké problémy hledáme řešení blízké optimu v polynomiálním čase.
- **Lineární programování (LP)** - formulace problému jako soustavy lineárních nerovnic.

---

## 2. Rozděl a panuj (Divide & Conquer)

### Definice paradigmatu

Rozděl a panuj je strategie ve **třech krocích**:

1. **Divide (rozděl)** - rozděl problém na **menší nezávislé** podproblémy stejného typu.
2. **Conquer (panuj)** - vyřeš podproblémy rekurzivně. Pokud jsou dostatečně malé, vyřeš je přímo (**base case**).
3. **Combine (zkombinuj)** - zkombinuj řešení podproblémů do řešení původního problému.

### Klíčový znak: nezávislé podproblémy

Podproblémy v D&C jsou **nezávislé** - řešení jednoho nepotřebuje výsledek druhého. To je hlavní rozdíl oproti **dynamickému programování**, kde se podproblémy překrývají.

### Pseudokód obecné šablony

```
DIVIDE_AND_CONQUER(problém):
   if problém je dostatečně malý:
      return řešPřímo(problém)              // base case

   rozděl problém na podproblémy P₁, P₂, ..., Pₖ

   for i = 1 to k:
      řešeníᵢ = DIVIDE_AND_CONQUER(Pᵢ)       // rekurze

   return KOMBINUJ(řešení₁, řešení₂, ..., řešeníₖ)
```

### Kdy paradigma použít

- Problém lze rozdělit na **menší instance stejného typu**.
- Řešení podproblémů lze **efektivně zkombinovat** (typicky v lineárním čase).
- Podproblémy **se nepřekrývají** (jinak DP).

---

## 3. Příklady D&C algoritmů

### Merge Sort

Klasický příklad. Rozdělí pole na poloviny, rekurzivně setřídí každou, pak slévá.

```
            [38, 27, 43, 3, 9, 82, 10]
                       |
              ROZDĚL na poloviny
              /                  \
       [38, 27, 43, 3]       [9, 82, 10]
            |                       |
       (rekurze)                (rekurze)
            |                       |
       [3, 27, 38, 43]         [9, 10, 82]
              \                    /
               KOMBINUJ (merge)
                      |
            [3, 9, 10, 27, 38, 43, 82]
```

Rekurence: `T(n) = 2·T(n/2) + Θ(n) = Θ(n log n)`.

### Quick Sort

Pivot rozdělí pole na menší a větší část, rekurze na obě části.

```
QuickSort([6, 3, 8, 5, 2, 7, 4, 1]) s pivot=4:

PARTITION:    [3, 2, 1] | [4] | [6, 8, 5, 7]
                 |       fixed       |
              QuickSort           QuickSort
              (rekurze)           (rekurze)
```

Rekurence (avg case): `T(n) = 2·T(n/2) + Θ(n) = Θ(n log n)`.

### Binární vyhledávání

Najít prvek v setříděném poli rozpůlením rozsahu v každém kroku.

```
Hledáme 15 v [3, 7, 15, 28, 42, 66, 91]:
   stred = 28 > 15 → hledej v [3, 7, 15]
   stred = 7 < 15  → hledej v [15]
   stred = 15 = 15 → NALEZENO
```

Rekurence: `T(n) = T(n/2) + Θ(1) = Θ(log n)`.

### Karatsubova metoda násobení

Násobení velkých čísel v `O(n^1.585)` místo naivního `O(n²)`. Rozdělí čísla na poloviny:

```
x = a · 10^(n/2) + b
y = c · 10^(n/2) + d

x · y = ac · 10^n + (ad + bc) · 10^(n/2) + bd
      = ac · 10^n + ((a+b)·(c+d) - ac - bd) · 10^(n/2) + bd
              └─ 3 násobení místo 4! ─┘
```

Rekurence: `T(n) = 3·T(n/2) + Θ(n) = Θ(n^log₂3) = Θ(n^1.585)`.

### Strassenovo násobení matic

Násobení matic n×n v `O(n^2.807)` místo naivního `O(n³)`. Rozdělí matice na 4 podmatice, použije 7 násobení místo 8.

### FFT (Fast Fourier Transform)

Diskrétní Fourierova transformace v `O(n log n)` místo naivního `O(n²)`. Rozdělí signál na sudé a liché indexy.

### Closest Pair of Points

Nejbližší dvojice bodů v rovině v `O(n log n)` místo naivního `O(n²)`. Rozdělí body na poloviny vertikální čarou, rekurze na obě, pak zkontroluje úzký pás kolem dělící čáry.

### Maximum Subarray (Kadaneho prefix forma)

Existuje D&C řešení v `O(n log n)`. Klasický **Kadaneho algoritmus** to ale řeší v O(n) (DP přístup).

---

## 4. Master Theorem

Pro analýzu D&C algoritmů s rekurencí tvaru:

```
T(n) = a · T(n/b) + f(n)
```

kde `a ≥ 1`, `b > 1`, `f(n)` je asymptoticky pozitivní funkce, platí **Master Theorem**:

Označme **kritický exponent** `c = log_b(a)`. Pak:

| Případ | Podmínka | Výsledek |
|--------|----------|----------|
| **1** | `f(n) = O(n^(c-ε))` pro nějaké `ε > 0` | `T(n) = Θ(n^c)` |
| **2** | `f(n) = Θ(n^c · log^k n)` | `T(n) = Θ(n^c · log^(k+1) n)` |
| **3** | `f(n) = Ω(n^(c+ε))` a regularita | `T(n) = Θ(f(n))` |

### Aplikace na známé algoritmy

| Algoritmus | a | b | f(n) | c = log_b(a) | Výsledek |
|------------|---|---|------|--------------|----------|
| Merge Sort | 2 | 2 | n | 1 | Θ(n log n) (případ 2) |
| Binární hledání | 1 | 2 | 1 | 0 | Θ(log n) (případ 2) |
| Karatsuba | 3 | 2 | n | 1,585 | Θ(n^1.585) (případ 1) |
| Strassen | 7 | 2 | n² | 2,807 | Θ(n^2.807) (případ 1) |
| Quick Sort (avg) | 2 | 2 | n | 1 | Θ(n log n) |

### Příklad výpočtu (Merge Sort)

```
T(n) = 2·T(n/2) + Θ(n)
a = 2, b = 2, f(n) = n
c = log₂(2) = 1
n^c = n^1 = n

f(n) = Θ(n^c · log^0 n)  → případ 2 s k = 0
T(n) = Θ(n^c · log^(0+1) n) = Θ(n log n)
```

---

## 5. Dynamické programování (DP)

### Definice paradigmatu

**Dynamické programování** je technika, která rozkládá problém na **překrývající se podproblémy**, řeší každý podproblém **jen jednou** a výsledek **uloží do paměti** (cache, tabulka) pro opakované použití.

> Termín "dynamic programming" pochází z Richarda Bellmana (1950) - "programming" zde znamená "plánování" (jako v "linear programming"), ne "psaní kódu".

### Klíčový rozdíl od D&C

```
ROZDĚL A PANUJ:                  DYNAMICKÉ PROGRAMOVÁNÍ:
- Podproblémy NEZÁVISLÉ         - Podproblémy se PŘEKRÝVAJÍ
- Každý řešíš jednou (přirozeně) - Bez cache bys řešil tentýž N-krát
- Příklad: Merge Sort            - DP cache eliminuje opakovanou práci
                                  - Příklad: Fibonacci, Knapsack
```

### Motivační příklad: Fibonacci

**Naivní rekurze:**

```csharp
static long Fib(int n)
{
    if (n <= 1) return n;
    return Fib(n - 1) + Fib(n - 2);
}
```

Rekurzivní strom pro `Fib(5)`:

```
              fib(5)
             /      \
         fib(4)      fib(3)         ← fib(3) je zde dvakrát
        /     \      /    \
     fib(3)  fib(2) fib(2) fib(1)   ← fib(2) je 3×
     /   \
  fib(2) fib(1)
```

Počet volání `Fib(n)` je v řádu `Θ(2^n)` - exponenciální! Pro `n = 50` to je `~10¹⁵` volání, několik hodin výpočtu.

**DP s memoizací (top-down):**

```csharp
static Dictionary<int, long> memo = new();

static long FibMemo(int n)
{
    if (n <= 1) return n;
    if (memo.TryGetValue(n, out long val)) return val;

    long result = FibMemo(n - 1) + FibMemo(n - 2);
    memo[n] = result;
    return result;
}
```

Každý `Fib(k)` se spočítá jen jednou. Složitost `Θ(n)`.

**DP s tabulací (bottom-up):**

```csharp
static long FibTab(int n)
{
    if (n <= 1) return n;

    long[] dp = new long[n + 1];
    dp[0] = 0; dp[1] = 1;

    for (int i = 2; i <= n; i++)
        dp[i] = dp[i - 1] + dp[i - 2];

    return dp[n];
}
```

Plní tabulku iterativně od `dp[2]` po `dp[n]`. Složitost `Θ(n)`, paměť `Θ(n)`.

**Optimalizace paměti** (potřebujeme jen poslední dvě hodnoty):

```csharp
static long FibOpt(int n)
{
    if (n <= 1) return n;

    long prev = 0, curr = 1;
    for (int i = 2; i <= n; i++)
    {
        long next = prev + curr;
        prev = curr;
        curr = next;
    }
    return curr;
}
```

Složitost `Θ(n)` čas, **`Θ(1)` paměť**.

### Srovnání všech přístupů k Fibonacci

| Přístup | Čas | Paměť | Pro n = 50 |
|---------|-----|-------|------------|
| Naivní rekurze | O(2^n) | O(n) zásobník | hodiny |
| DP memoizace | O(n) | O(n) | < 1 ms |
| DP tabulace | O(n) | O(n) | < 1 ms |
| DP optimalizovaná paměť | O(n) | O(1) | < 1 ms |
| Matice + rychlé umocnění | O(log n) | O(log n) | < 0,01 ms |

---

## 6. Optimální substruktura a překrývající se podproblémy

DP je vhodné pro problémy s **dvěma vlastnostmi**:

### 1. Optimální substruktura

Optimální řešení problému lze sestavit z optimálních řešení jeho podproblémů.

**Příklad (nejkratší cesta):** Pokud nejkratší cesta z A do C vede přes B, pak podčást této cesty z A do B musí být sama nejkratší cestou z A do B. (Pokud by byla kratší jiná cesta z A do B, mohli bychom ji použít a získat kratší cestu z A do C - spor.)

**Kontra-příklad (nejdelší jednoduchá cesta):** Tato vlastnost neplatí pro nejdelší jednoduché cesty (bez opakování uzlů) - subcesta nemusí být sama nejdelší. Proto **NP-těžké**.

### 2. Překrývající se podproblémy

Stejné podproblémy se objevují opakovaně. Bez memoizace by se počítaly mnohokrát.

**Příklad (Fibonacci):** `Fib(5)` volá `Fib(3)` dvakrát - jednou přímo z `Fib(5)`, jednou přes `Fib(4)`.

### Když chybí překrytí - není to DP

Merge Sort má optimální substrukturu (setříděné poloviny dají setříděný celek), ale podproblémy se **nepřekrývají** - takže to není DP, ale jen D&C.

---

## 7. Memoizace vs tabulace

### Memoizace (top-down DP)

- Píšeme rekurzivně, ale výsledky kešujeme.
- Hodí se, když nepotřebujeme všechny podproblémy.
- Přirozenější k implementaci pro složitější problémy.
- Trochu vyšší konstanta kvůli režii rekurze a hash mapy.

```csharp
static Dictionary<(int, int), int> memo = new();

static int Solve(int a, int b)
{
    if (a == 0) return 0;                          // base case
    if (memo.TryGetValue((a, b), out int cached)) return cached;

    int result = /* rekurzivní výpočet */;
    memo[(a, b)] = result;
    return result;
}
```

### Tabulace (bottom-up DP)

- Iterujeme od nejmenších podproblémů po největší.
- Vyžaduje, abychom uměli předem určit pořadí výpočtu.
- Žádná režie rekurze - často rychlejší.
- Vyžaduje, abychom vypočítali všechny podproblémy (i ty zbytečné).

```csharp
int[,] dp = new int[n + 1, m + 1];

for (int i = 0; i <= n; i++)
    for (int j = 0; j <= m; j++)
    {
        if (base case) dp[i, j] = ...;
        else dp[i, j] = /* funkce dp[i-1, j], dp[i, j-1], ... */;
    }

return dp[n, m];
```

### Porovnání

| Aspekt | Memoizace | Tabulace |
|--------|-----------|----------|
| Směr | Top-down | Bottom-up |
| Implementace | Rekurze + cache | Iterace + pole |
| Paměť | Hash map (slovník) | Pole |
| Výpočet | Pouze potřebné podproblémy | Všechny podproblémy |
| Režie | Funkce + hash | Žádná |
| Riziko | Stack overflow pro velké n | Žádné |
| Snadnost | Snadná modifikace rekurze | Vyžaduje znát pořadí |

### Optimalizace paměti

Pokud aktuální stav závisí jen na několika předchozích, můžeme **uvolnit starší řádky**. Příklad Fibonacci: stačí 2 proměnné místo celého pole. Příklad 2D DP: často stačí 2 řádky místo celé matice.

---

## 8. Klasické DP úlohy

### Coin Change (Mince)

**Úloha:** Najdi nejmenší počet mincí daných nominálů pro vyplacení částky.

```
Mince: {1, 3, 5}, částka: 11
Optimum: 3 mince (5 + 5 + 1)
```

**DP formulace:** `dp[i]` = nejmenší počet mincí pro částku `i`.

**Rekurence:** `dp[i] = min(dp[i - c] + 1)` pro každý nominál `c ≤ i`.

```csharp
static int CoinChange(int[] mince, int castka)
{
    int[] dp = new int[castka + 1];
    Array.Fill(dp, int.MaxValue);
    dp[0] = 0;

    for (int i = 1; i <= castka; i++)
        foreach (int c in mince)
            if (c <= i && dp[i - c] != int.MaxValue)
                dp[i] = Math.Min(dp[i], dp[i - c] + 1);

    return dp[castka] == int.MaxValue ? -1 : dp[castka];
}
```

**Složitost:** `O(částka · |mince|)` čas, `O(částka)` paměť.

### 0/1 Knapsack (Batoh)

**Úloha:** Máme batoh s kapacitou `W` a `n` předmětů (každý s váhou `wᵢ` a hodnotou `vᵢ`). Vyber podmnožinu předmětů maximalizující celkovou hodnotu, aniž bys překročil kapacitu. Každý předmět vezmeš nejvýše jednou.

**DP formulace:** `dp[i, w]` = max hodnota pomocí prvních `i` předmětů s váhou ≤ `w`.

**Rekurence:**

```
dp[i, w] = max(
    dp[i-1, w],                              // nebereme i-tý
    dp[i-1, w - wᵢ] + vᵢ   pokud wᵢ ≤ w      // bereme i-tý
)
```

```csharp
static int Knapsack(int[] wt, int[] val, int W)
{
    int n = wt.Length;
    int[,] dp = new int[n + 1, W + 1];

    for (int i = 1; i <= n; i++)
        for (int w = 0; w <= W; w++)
        {
            dp[i, w] = dp[i - 1, w];               // nebereme i-tý
            if (wt[i - 1] <= w)
                dp[i, w] = Math.Max(dp[i, w], dp[i - 1, w - wt[i - 1]] + val[i - 1]);
        }

    return dp[n, W];
}
```

**Složitost:** `O(n · W)` - pseudopolynomiální (závisí na hodnotě W, ne na log W).

### Nejdelší společná podsekvence (LCS)

**Úloha:** Najdi nejdelší společnou podsekvenci dvou řetězců (znaky musí být ve stejném pořadí, nemusí být souvislé).

```
S1 = "ABCBDAB"
S2 = "BDCABA"
LCS = "BCBA" (délka 4)
```

**DP formulace:** `dp[i, j]` = délka LCS prefixů `S1[0..i)` a `S2[0..j)`.

**Rekurence:**

```
dp[i, j] = dp[i-1, j-1] + 1                       pokud S1[i-1] == S2[j-1]
         = max(dp[i-1, j], dp[i, j-1])            jinak
```

```csharp
static int LCS(string s1, string s2)
{
    int n = s1.Length, m = s2.Length;
    int[,] dp = new int[n + 1, m + 1];

    for (int i = 1; i <= n; i++)
        for (int j = 1; j <= m; j++)
        {
            if (s1[i - 1] == s2[j - 1])
                dp[i, j] = dp[i - 1, j - 1] + 1;
            else
                dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
        }

    return dp[n, m];
}
```

**Složitost:** `O(n · m)` čas i paměť.

**Aplikace:** Diff utility (Git, version control), bioinformatika (porovnání DNA sekvencí), spell checkers.

### Edit Distance (Levenshteinova vzdálenost)

**Úloha:** Najdi minimální počet operací (vložit, smazat, nahradit znak) potřebných k transformaci řetězce `S1` na `S2`.

```
S1 = "kitten", S2 = "sitting"
Editace: k→s, e→i, +g → vzdálenost 3
```

**Rekurence:**

```
dp[i, j] = dp[i-1, j-1]                                   pokud S1[i-1] == S2[j-1]
         = 1 + min(dp[i-1, j],         // smaž
                   dp[i, j-1],          // vlož
                   dp[i-1, j-1])        // nahraď      jinak
```

Aplikace: spell checkers, autocomplete, fuzzy search, OCR.

### Nejdelší rostoucí podsekvence (LIS)

**Úloha:** Najdi nejdelší rostoucí podsekvenci v poli.

```
[10, 9, 2, 5, 3, 7, 101, 18]
LIS = [2, 3, 7, 101] nebo [2, 5, 7, 101] - délka 4
```

**Naivní DP:** `dp[i]` = délka LIS končící na pozici `i`. Složitost O(n²).

**Optimalizace s binárním vyhledáváním:** O(n log n) - udržujeme pole `tail`, kde `tail[k]` je nejmenší možný konec LIS délky `k+1`.

### Floyd-Warshall (všechny nejkratší cesty v grafu)

DP nad grafy. `dp[k][i][j]` = nejkratší cesta z `i` do `j` použitím pouze prvních `k` mezivrcholů.

```
for k = 0 to V-1:
   for i = 0 to V-1:
      for j = 0 to V-1:
         dp[i][j] = min(dp[i][j], dp[i][k] + dp[k][j])
```

Složitost: `O(V³)`.

### Matrix Chain Multiplication

Optimální uzávorkování pro násobení sledu matic. `dp[i, j]` = minimální počet operací pro násobení matic A_i ... A_j. Klasický DP problém.

### Rod cutting

Maximální výnos z rozřezání tyče s daným ceníkem.

### TSP (Bitmask DP)

Pro TSP existuje DP řešení v `O(n² · 2^n)` (Held-Karp). Stav je `(mask, last)` - množina navštívených uzlů a poslední uzel.

---

## 9. Backtracking

### Definice paradigmatu

**Backtracking** je **systematické prohledávání stavového prostoru** s "vrácením kroku" (backtrack), když aktuální cesta nevede k řešení. Postup:

1. **Postavme částečné řešení** (např. první dáma na pozici).
2. Pokud je řešení **úplné** - zaznamenej/vypiš.
3. Pokud lze řešení rozšířit:
   a) Zkus každou možnou rozšířenou variantu.
   b) Rekurzivně pokračuj.
   c) Pokud zjistíš, že varianta nemůže vést k řešení (porušení omezení), **vrať se** a zkus jinou.

### Šablona

```
BACKTRACK(stav):
   if stav je úplné řešení:
      zaznamenej(stav)
      return

   for každá možná volba v aktuálním stavu:
      if volba je validní:                  // pruning
         aplikuj(volba)                     // UDĚLEJ krok
         BACKTRACK(stav po aplikaci)        // rekurze
         vrať(volba)                        // UNDO (backtrack)
```

### Stavový prostor jako strom

Backtracking si lze představit jako **DFS prohledávání stromu možností**:

```
                       []                         ← prázdné řešení
                    /  |  \
                  [1] [2] [3]                     ← volby na 1. úrovni
                  /|\
              [1,1][1,2][1,3]                     ← rozšíření [1]
              ...
                                                  ← úplné řešení (list)
```

**Pruning** = odřezání podstromů, které nemohou vést k řešení.

### Rozdíl od DFS

| Aspekt | DFS | Backtracking |
|--------|-----|--------------|
| Cíl | Projít všechny uzly grafu | Najít řešení/všechna řešení |
| Stavy | Pevný graf | Generovaný "za běhu" |
| Undo | Žádné (visited flag) | Explicitní undo po rekurzi |
| Pruning | Bez kontroly cíle | Aktivní pruning |
| Příklad | Procházení komponenty | N-Queens, Sudoku |

DFS je obecnější algoritmus, backtracking je specifická aplikace DFS na strom možností s pruningem a undo operací.

---

## 10. Pruning a heuristiky

**Pruning** (ořezávání) je klíčem k efektivitě backtrackingu. Naivní procházení všech možností by mělo složitost `O(b^d)` (kde `b` = větvení, `d` = hloubka). Inteligentní pruning to může zrychlit o **mnoho řádů**.

### Typy pruningu

1. **Constraint propagation** - pokud volba porušuje omezení, okamžitě se vrátíme. Příklad: v Sudoku zkontrolujeme řádek/sloupec/box před vložením čísla.

2. **Bound pruning (Branch & Bound)** - pokud aktuální parciální řešení má horší skóre než nejlepší dosud nalezené, přestaneme.

3. **Symmetry breaking** - pro symetrické problémy (např. N-Queens) generujeme jen "kanonická" řešení a ostatní odvodíme symetrií.

4. **Forward checking** - po aplikaci volby zkontrolujeme, zda zbývají validní volby pro nepřiřazené proměnné. Pokud ne, vrátíme se.

5. **MRV (Minimum Remaining Values)** - heuristika výběru proměnné - vyber tu, která má nejméně validních hodnot (rychleji se selže).

### Heuristiky

**Heuristika** je strategie volby, která **typicky zrychluje** běh, ale nezaručuje optimum:

- **Most Constrained Variable** (MRV) - viz výše.
- **Least Constraining Value (LCV)** - vyber hodnotu, která eliminuje nejméně možností pro ostatní proměnné.
- **Domain ordering** - zkus hodnoty v pořadí pravděpodobnosti úspěchu.

---

## 11. Klasické backtracking úlohy

### N-Queens (N dam na šachovnici)

**Úloha:** Rozmísti N dam na šachovnici N×N tak, aby se žádné dvě navzájem neohrožovaly (sdílely řádek, sloupec, nebo diagonálu).

Pro N = 8 existuje **92 řešení** (12 unikátních po symetrii).

```csharp
static List<int[]> NQueens(int n)
{
    var solutions = new List<int[]>();
    int[] queens = new int[n];                     // queens[row] = column
    Solve(0, queens, n, solutions);
    return solutions;
}

static void Solve(int row, int[] queens, int n, List<int[]> solutions)
{
    if (row == n)
    {
        solutions.Add((int[])queens.Clone());
        return;
    }

    for (int col = 0; col < n; col++)
    {
        if (IsSafe(queens, row, col))               // pruning
        {
            queens[row] = col;                       // udělej krok
            Solve(row + 1, queens, n, solutions);    // rekurze
            // undo nepotřebujeme - další iterace přepíše queens[row]
        }
    }
}

static bool IsSafe(int[] queens, int row, int col)
{
    for (int i = 0; i < row; i++)
    {
        if (queens[i] == col) return false;                              // stejný sloupec
        if (Math.Abs(queens[i] - col) == row - i) return false;          // diagonála
    }
    return true;
}
```

**Složitost:** `O(N!)` v nejhorším případě, prakticky mnohem méně díky pruningu.

### Sudoku solver

**Úloha:** Vyplň prázdná políčka v 9×9 mřížce čísly 1-9 tak, aby každý řádek, sloupec a 3×3 box obsahoval všechna čísla 1-9.

```csharp
static bool SolveSudoku(int[,] grid)
{
    for (int r = 0; r < 9; r++)
        for (int c = 0; c < 9; c++)
            if (grid[r, c] == 0)
            {
                for (int num = 1; num <= 9; num++)
                {
                    if (IsValid(grid, r, c, num))
                    {
                        grid[r, c] = num;
                        if (SolveSudoku(grid)) return true;
                        grid[r, c] = 0;             // UNDO
                    }
                }
                return false;                       // žádná hodnota nepasuje
            }
    return true;                                    // všechny políčka vyplněna
}
```

### Permutace a kombinace

**Permutace:** Vygeneruj všechna uspořádání n prvků.

```csharp
static void Permute(int[] arr, int start, List<int[]> result)
{
    if (start == arr.Length)
    {
        result.Add((int[])arr.Clone());
        return;
    }

    for (int i = start; i < arr.Length; i++)
    {
        (arr[start], arr[i]) = (arr[i], arr[start]); // swap
        Permute(arr, start + 1, result);
        (arr[start], arr[i]) = (arr[i], arr[start]); // UNDO
    }
}
```

Složitost: `O(n · n!)`.

**Kombinace:** Vygeneruj všechny podmnožiny velikosti k.

```csharp
static void Combine(int[] arr, int start, int k, List<int> current, List<List<int>> result)
{
    if (current.Count == k)
    {
        result.Add(new List<int>(current));
        return;
    }

    for (int i = start; i < arr.Length; i++)
    {
        current.Add(arr[i]);
        Combine(arr, i + 1, k, current, result);
        current.RemoveAt(current.Count - 1);         // UNDO
    }
}
```

Složitost: `O(C(n, k) · k)`.

### Hamiltonovská cesta v grafu

Najdi cestu, která navštíví každý uzel právě jednou.

```
NP-úplný problém, řeší se backtrackingem:
- Začni v uzlu 0.
- Zkus přidat každý neviděný sousední uzel.
- Když máš všechny uzly + návrat do startu = Hamiltonovský cyklus.
```

### Knight's Tour

Cesta jezdce na šachovnici, která navštíví každé políčko právě jednou.

**Warnsdorffova heuristika** - vždy se přesuň na pole s nejméně možnými pokračováními. Tato heuristika přemění exponenciální problém na téměř lineární.

### Word search v mřížce

Najdi slovo v mřížce písmen, kde se písmena dají skládat horizontálně, vertikálně nebo diagonálně.

### SAT solver

Splnitelnost boolovské formule. NP-úplný problém. Moderní SAT solvery (DPLL, CDCL) využívají backtracking + sofistikované učení z konfliktů.

---

## 12. Porovnání tří paradigmat

### Rozhodovací strom

```
Mám problém. Co použít?

Můžu rozdělit problém na NEZÁVISLÉ podproblémy?
   ANO → ROZDĚL A PANUJ
        Příklady: Merge Sort, Quick Sort, binární vyhledávání

Mám OPTIMUM (min/max) a podproblémy se PŘEKRÝVAJÍ?
   ANO → DYNAMICKÉ PROGRAMOVÁNÍ
        Příklady: Fibonacci, Coin Change, Knapsack, LCS, Edit Distance

Hledám VŠECHNA platná řešení / JEDNO řešení splňující omezení?
   ANO → BACKTRACKING
        Příklady: N-Queens, Sudoku, permutace, Hamiltonovská cesta
```

### Vlastnosti

| Vlastnost | D&C | DP | Backtracking |
|-----------|-----|-----|--------------|
| Rozkládá problém | Ano | Ano | Ne (rozšiřuje řešení) |
| Podproblémy | Nezávislé | Překrývající se | N/A |
| Cache | Ne | Ano | Ne |
| Optimální substruktura | Často ano | Vždy ano | N/A |
| Vrací krok (undo) | Ne | Ne | Ano |
| Pruning | Ne | Ne | Klíčový |
| Typická složitost | O(n log n) | Polynomiální | Exponenciální |

### Společné rysy

- **Všechny tři používají rekurzi** (i když DP častěji v iterativní formě - tabulace).
- **Všechny vyžadují identifikaci podproblémů / stavu**.
- **Všechny mají "base case"** - triviální případ řešený přímo.

### Hybridní přístupy

- **Memoizace + backtracking** - některé úlohy lze řešit kombinací (např. počet validních konfigurací).
- **DP + greedy** - některé DP úlohy mají strukturu, kde greedy postačí (např. Activity selection).
- **D&C + DP** - existují problémy, kde D&C kostra obsahuje DP "uvnitř" (např. Closest Pair s mřížkou).

---

## 13. Maturitní chytáky

### Časté chyby

**Záměna D&C a DP:**

```
"Je Merge Sort dynamické programování?"
NE - jeho podproblémy jsou nezávislé (poloviny pole). DP vyžaduje překrývání.
```

**DP bez báze:**

```csharp
// CHYBA - nikdy se nezastaví
static int Solve(int n) => Solve(n - 1) + Solve(n - 2);

// SPRÁVNĚ - base case
static int Solve(int n)
{
    if (n <= 1) return n;
    return Solve(n - 1) + Solve(n - 2);
}
```

**Backtracking bez undo:**

```csharp
// CHYBA - mění globální stav bez návratu
board[r, c] = num;
if (Solve(board)) return true;
// chybí: board[r, c] = 0;

// SPRÁVNĚ - undo po rekurzi
board[r, c] = num;
if (Solve(board)) return true;
board[r, c] = 0;
```

**Memoizace pro neopakující se stavy:**

Pokud podproblémy nejsou jednoznačně identifikované (např. stav obsahuje pointery na měnitelné objekty), memoizace nemusí fungovat. Klíče v cache musí být **immutable**.

**Tabulace ve špatném pořadí:**

```csharp
// CHYBA - dp[i] používá dp[i+1], ale ten ještě neexistuje
for (int i = 0; i <= n; i++)
    dp[i] = dp[i + 1] + ...;

// Buď změň pořadí (od n dolů), nebo přepiš rekurenci
```

### Typické otázky u ústní zkoušky

- **"Jaký je rozdíl mezi Divide & Conquer a dynamickým programováním?"**
  D&C má nezávislé podproblémy, řešené rekurzivně bez sdílení výsledků. DP má překrývající se podproblémy, výsledky se kešují, aby se nepočítaly opakovaně.

- **"Vysvětli optimální substrukturu na příkladu."**
  Optimální řešení problému lze sestavit z optimálních řešení podproblémů. Příklad: nejkratší cesta - subcesta nejkratší cesty je sama nejkratší cestou mezi svými konci.

- **"Co je rozdíl mezi memoizací a tabulací?"**
  Memoizace je top-down rekurze s cache (řeší podproblémy podle potřeby). Tabulace je bottom-up iterace plnící celou tabulku v určeném pořadí.

- **"Kdy použít backtracking?"**
  Když hledáme řešení v diskrétním stavovém prostoru s omezeními a chceme všechna řešení (nebo jedno) - typicky NP-těžké problémy: N-Queens, Sudoku, SAT, permutace.

- **"Jaký je rozdíl mezi DFS a backtrackingem?"**
  DFS prochází existující graf. Backtracking generuje strom možností "za běhu" a aktivně provádí undo po rekurzi (vrací stav). Backtracking je specializovaný DFS s pruningem.

- **"Proč naivní Fibonacci běží v O(2^n)?"**
  Rekurzivní strom má větvení 2 a hloubku n - počet listů cca 2^n. Každý `Fib(k)` se počítá vícekrát (např. `Fib(3)` 3x při výpočtu `Fib(6)`).

- **"Co je Master Theorem?"**
  Věta umožňující odvodit asymptotickou složitost rekurence tvaru `T(n) = a·T(n/b) + f(n)`. Tři případy podle vztahu `f(n)` k `n^log_b(a)`.

- **"Jak řešit problém batohu (0/1 Knapsack)?"**
  DP s tabulkou `dp[i, w]` = max hodnota s prvními `i` předměty a kapacitou `w`. Rekurence: `dp[i, w] = max(dp[i-1, w], dp[i-1, w-wᵢ] + vᵢ)`. Složitost O(n·W) - pseudopolynomiální.

### Kontrolní seznam

- [ ] D&C: nezávislé podproblémy, žádná cache
- [ ] DP: identifikace stavu a rekurence, base case, správné pořadí výpočtu
- [ ] Backtracking: validace volby (pruning), undo po rekurzi
- [ ] Optimalizace paměti pro DP (poslední řádky)
- [ ] Heuristiky a constraint propagation pro backtracking
- [ ] Konkrétní složitosti známých algoritmů (Merge Sort O(n log n), Knapsack O(n·W), N-Queens O(N!))

---

## 14. Klíčové pojmy

- **Algoritmické paradigma** - obecná strategie návrhu algoritmů.
- **Rozděl a panuj (Divide & Conquer)** - rozdělení problému na nezávislé podproblémy, jejich rekurzivní řešení a kombinace.
- **Dynamické programování (DP)** - řešení překrývajících se podproblémů s kešováním výsledků.
- **Backtracking** - systematické prohledávání stavového prostoru s ořezáváním a vrácením kroku.
- **Greedy (hladový algoritmus)** - volba lokálně nejlepší možnosti v každém kroku.
- **Branch & Bound** - backtracking s horními/dolními mezemi pro optimalizační úlohy.
- **Master Theorem** - věta pro řešení rekurencí typu T(n) = a·T(n/b) + f(n).
- **Optimální substruktura** - vlastnost, že optimum problému lze sestavit z optim podproblémů.
- **Překrývající se podproblémy** - vlastnost, že stejné podproblémy se objevují opakovaně.
- **Memoizace (top-down DP)** - rekurze s kešováním výsledků.
- **Tabulace (bottom-up DP)** - iterativní plnění tabulky od nejmenších podproblémů.
- **Báze rekurze (base case)** - triviální případ řešený přímo, ukončuje rekurzi.
- **Rekurenční rovnice** - matematický zápis časové složitosti rekurzivního algoritmu.
- **Pruning (ořezávání)** - eliminace větví stavového prostoru, které nemohou vést k řešení.
- **Constraint propagation** - šíření omezení po výběru hodnoty.
- **Forward checking** - kontrola validních hodnot pro budoucí proměnné.
- **MRV (Minimum Remaining Values)** - heuristika výběru proměnné s nejméně volnými hodnotami.
- **Symmetry breaking** - generování jen kanonických řešení pro symetrické problémy.
- **Stavový prostor** - množina všech možných stavů problému.
- **Pseudopolynomiální složitost** - závislá na hodnotě vstupu (např. W u Knapsacku), ne na velikosti zápisu.
- **NP-úplný problém** - problém z třídy NP, na který lze redukovat všechny ostatní NP problémy.
- **N-Queens** - klasický backtracking problém umístění dám.
- **Sudoku** - klasický CSP (Constraint Satisfaction Problem) řešený backtrackingem.
- **Coin Change** - klasická DP úloha minimálního počtu mincí.
- **Knapsack** - klasická DP úloha optimalizace nákladu batohu.
- **LCS (Longest Common Subsequence)** - nejdelší společná podsekvence dvou řetězců, řešená DP.
- **Edit Distance (Levenshtein)** - minimální počet editačních operací mezi dvěma řetězci.
- **LIS (Longest Increasing Subsequence)** - nejdelší rostoucí podsekvence.
- **Floyd-Warshall** - DP algoritmus pro všechny nejkratší cesty v grafu.
- **Held-Karp** - DP algoritmus pro TSP v O(n²·2^n) místo O(n!).
- **Karatsuba** - D&C násobení čísel v O(n^1.585).
- **Strassen** - D&C násobení matic v O(n^2.807).

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| Ot. 5 - Rekurze | Všechna paradigmata využívají rekurzi |
| Ot. 7 - Složitost | Master Theorem, polynomiální vs exponenciální |
| Ot. 11 - Merge Sort | Klasický D&C algoritmus |
| Ot. 12 - Quick Sort | D&C s randomizací |
| Ot. 14 - Binární vyhledávání | D&C v O(log n) |
| Ot. 16 - Algoritmické techniky | Greedy, randomizace, aproximace |
| Ot. 18 - Grafy | Floyd-Warshall (DP), Hamiltonovská cesta (backtracking) |
| Ot. 22 - DFS/BFS | Backtracking je specializovaný DFS |
| Ot. 25 - Dijkstra | Greedy algoritmus, alternativa k Floyd-Warshall |
