# 📚 Zápisky: Otázka č. 5 - Rekurze a její využití

**Datum:** 2025-01-08  
**Status:** ✅ Hotovo

---

## ✅ Checklist bodů otázky

- [x] Bod 1: Pojem rekurze
- [x] Bod 2: Příklady využití (Fibonacci, permutace, faktoriál, QuickSelect) + složitost
- [x] Bod 3: Výhody a nevýhody rekurze
- [x] Bod 4: Kde je (ne)efektivní rekurzi použít
- [x] Bod 5: Koncová rekurze (tail recursion)
- [x] Bod 6: Nahrazení rekurze zásobníkem
- [x] Bod 7: StackOverflow exception
- [x] Bod 8: Navazující témata (DFS, Backtracking, MergeSort, QuickSort)

---

## 🧠 Klíčové koncepty & Snippety

---

### BOD 1: Pojem rekurze

**Teorie:**

Rekurze = funkce, která **volá sama sebe**.

Každá rekurzivní funkce musí mít:
1. **Base case** (ukončovací podmínka) – kdy přestat
2. **Recursive case** – volání sama sebe se ZMENŠENÝM problémem

**Analogie:** Ruské matrjošky 🪆 – otevřeš jednu, uvnitř je menší, až dojdeš k nejmenší.

```csharp
// ✅ MATURITNÍ VERZE - Struktura rekurze
static void Rekurze(int n)
{
    // 1. BASE CASE - kdy skončit
    if (n <= 0)
        return;
    
    // 2. Něco udělej
    Console.WriteLine(n);
    
    // 3. RECURSIVE CASE - zavolej sám sebe s menším problémem
    Rekurze(n - 1);
}
```

---

### BOD 2: Příklady využití + složitost

#### Faktoriál

```csharp
// ✅ MATURITNÍ VERZE
static int Faktorial(int n)
{
    if (n <= 1)          // Base case
        return 1;
    
    return n * Faktorial(n - 1);  // n! = n × (n-1)!
}

// Faktorial(5) = 5 × 4 × 3 × 2 × 1 = 120
```

**Časová složitost:** O(n)  
**Paměťová složitost:** O(n) – kvůli zásobníku

---

#### Fibonacci

```csharp
// ❌ NAIVNÍ VERZE - O(2^n) - NEPOUŽÍVAT!
static int FibonacciNaivni(int n)
{
    if (n <= 1) return n;
    return FibonacciNaivni(n - 1) + FibonacciNaivni(n - 2);
}

// ✅ MATURITNÍ VERZE - S memoizací O(n)
static long FibonacciMemo(int n, Dictionary<int, long> cache)
{
    if (n <= 1) return n;
    
    if (cache.ContainsKey(n))
        return cache[n];
    
    cache[n] = FibonacciMemo(n - 1, cache) + FibonacciMemo(n - 2, cache);
    return cache[n];
}

// 💡 SENIOR VERZE - Iterativně O(n), O(1) paměť
static long FibonacciIterace(int n)
{
    if (n <= 1) return n;
    long a = 0, b = 1;
    for (int i = 2; i <= n; i++)
    {
        long temp = a + b;
        a = b;
        b = temp;
    }
    return b;
}
```

**Naivní:** O(2^n) - exponenciální, ŠPATNÉ!  
**S memoizací:** O(n) - lineární ✅

---

#### Permutace

**Co je permutace?** Všechny možné způsoby, jak seřadit prvky.

Pro `[1, 2, 3]` existuje **3! = 6** permutací:
```
[1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1]
```

```csharp
// ✅ MATURITNÍ VERZE - Všechny permutace pole
static void Permutace(int[] pole, int start)
{
    // Base case - jsme na poslední pozici → TISKNI!
    if (start == pole.Length - 1)
    {
        Console.WriteLine(string.Join(", ", pole));
        return;
    }
    
    // Zkus každý prvek na pozici start
    for (int i = start; i < pole.Length; i++)
    {
        Prohod(pole, start, i);       // 1. Udělej krok
        Permutace(pole, start + 1);   // 2. Rekurze
        Prohod(pole, start, i);       // 3. BACKTRACK - vrať zpět!
    }
}

static void Prohod(int[] pole, int i, int j)
{
    int temp = pole[i];
    pole[i] = pole[j];
    pole[j] = temp;
}
```

**Vizualizace:**
```
start=0: "Kdo na 1. místo?" → zkus 1, 2, 3
start=1: "Kdo na 2. místo?" → zkus zbylé
start=2: "Kdo na 3. místo?" → poslední = BASE CASE → TISKNI!
```

**Časová složitost:** O(n!) - faktoriál

---

#### QuickSelect (k-tý nejmenší prvek)

**Co to je?** Najdi k-tý nejmenší prvek BEZ třídění celého pole.

**Myšlenka:**
1. Vyber pivot
2. Partition: `[menší] [PIVOT] [větší]`
3. Je pivot na pozici k? → Našli! Jinak hledej v příslušné polovině.

```csharp
// ✅ MATURITNÍ VERZE
static int QuickSelect(int[] pole, int levy, int pravy, int k)
{
    if (levy == pravy)
        return pole[levy];
    
    int pivotIndex = Partition(pole, levy, pravy);
    
    if (k == pivotIndex)
        return pole[k];                                    // NAŠLI!
    else if (k < pivotIndex)
        return QuickSelect(pole, levy, pivotIndex - 1, k); // VLEVO
    else
        return QuickSelect(pole, pivotIndex + 1, pravy, k); // VPRAVO
}

static int Partition(int[] pole, int levy, int pravy)
{
    int pivot = pole[pravy];
    int i = levy - 1;
    
    for (int j = levy; j < pravy; j++)
    {
        if (pole[j] <= pivot)
        {
            i++;
            Prohod(pole, i, j);
        }
    }
    
    Prohod(pole, i + 1, pravy);
    return i + 1;
}
```

**Průměrná složitost:** O(n)  
**Nejhorší případ:** O(n²)

---

### BOD 3: Výhody a nevýhody rekurze

| ✅ VÝHODY | ❌ NEVÝHODY |
|-----------|-------------|
| Čitelnější kód pro stromové problémy | Paměťová náročnost (zásobník) |
| Přirozené pro "rozděl a panuj" | Riziko StackOverflow |
| Jednodušší pro stromy a grafy | Může být pomalejší (režie volání) |
| Elegantní řešení | Těžší debugování |

---

### BOD 4: Kde je (ne)efektivní použít rekurzi

#### ✅ EFEKTIVNÍ:
- Stromové struktury (průchod stromem)
- Grafy (DFS)
- Algoritmy "rozděl a panuj" (QuickSort, MergeSort)
- Backtracking (Sudoku, bludiště, N-queens)

#### ❌ NEEFEKTIVNÍ:
- Překrývající se podproblémy BEZ memoizace (naivní Fibonacci)
- Lineární průchody (součet pole → použij cyklus)
- Příliš hluboká rekurze (miliony volání → StackOverflow)

```
                    POUŽÍT REKURZI?
                          │
            ┌─────────────┴─────────────┐
            │                           │
     Je problém stromový          Je to lineární
     nebo "rozděl a panuj"?        průchod?
            │                           │
           ANO                         ANO
            │                           │
            ▼                           ▼
    ✅ POUŽIJ REKURZI           ❌ POUŽIJ CYKLUS
```

---

### BOD 5: Koncová rekurze (Tail Recursion)

**Koncová rekurze** = rekurzivní volání je POSLEDNÍ operací funkce.

```csharp
// ❌ BĚŽNÁ rekurze (NENÍ koncová)
static int Faktorial(int n)
{
    if (n == 0) return 1;
    return n * Faktorial(n - 1);  // ← Po návratu se ještě NÁSOBÍ!
}

// ✅ KONCOVÁ rekurze (tail recursion)
static int FaktorialTail(int n, int akumulator = 1)
{
    if (n == 0) return akumulator;
    return FaktorialTail(n - 1, n * akumulator);  // ← Nic po návratu!
}
```

**Výhoda:** Některé kompilátory optimalizují koncovou rekurzi na cyklus (C# zatím ne, ale F# ano).

---

### BOD 6: Nahrazení rekurze zásobníkem

**Proč?** Rekurze = automatický zásobník. Můžeme to udělat ručně.

```csharp
// ✅ DFS REKURZIVNĚ
static void DFS_Rekurze(int vrchol, bool[] navstiveno, List<int>[] graf)
{
    navstiveno[vrchol] = true;
    Console.WriteLine(vrchol);
    
    foreach (int soused in graf[vrchol])
    {
        if (!navstiveno[soused])
            DFS_Rekurze(soused, navstiveno, graf);
    }
}

// ✅ DFS SE ZÁSOBNÍKEM (bez rekurze)
static void DFS_Zasobnik(int start, bool[] navstiveno, List<int>[] graf)
{
    Stack<int> zasobnik = new Stack<int>();
    zasobnik.Push(start);
    
    while (zasobnik.Count > 0)
    {
        int vrchol = zasobnik.Pop();
        
        if (navstiveno[vrchol])
            continue;
        
        navstiveno[vrchol] = true;
        Console.WriteLine(vrchol);
        
        foreach (int soused in graf[vrchol])
        {
            if (!navstiveno[soused])
                zasobnik.Push(soused);
        }
    }
}
```

| Rekurze | Zásobník |
|---------|----------|
| Jednodušší kód | Bez rizika StackOverflow |
| Automatický stack | Větší kontrola |
| Přirozenější | Méně paměti |

---

### BOD 7: StackOverflow Exception

**Co to je?** Zásobník volání (call stack) má omezenou velikost (~1 MB). Příliš mnoho rekurzivních volání ho přeplní.

```csharp
// ❌ ZPŮSOBÍ STACKOVERFLOW!
static void Nekonecna(int n)
{
    Console.WriteLine(n);
    Nekonecna(n + 1);  // Žádný base case → nikdy nekončí!
}

// ❌ ZPŮSOBÍ STACKOVERFLOW PRO VELKÉ n
static void PrilisHluboka(int n)
{
    if (n == 0) return;
    PrilisHluboka(n - 1);  // Pro n = 1_000_000 → stack overflow
}
```

**Řešení:**
1. Vždy mít správný BASE CASE
2. Pro velká n použít iteraci nebo zásobník
3. Optimalizovat na koncovou rekurzi

---

### BOD 8: Navazující témata

#### DFS (Depth-First Search)

```csharp
static void DFS(int vrchol, bool[] navstiveno, List<int>[] graf)
{
    navstiveno[vrchol] = true;
    Console.WriteLine(vrchol);
    
    foreach (int soused in graf[vrchol])
    {
        if (!navstiveno[soused])
            DFS(soused, navstiveno, graf);
    }
}
```

```
Graf:  1 -- 2 -- 5
       |    |
       3 -- 4

DFS z vrcholu 1:  1 → 2 → 5 → 4 → 3
(záleží na pořadí sousedů!)
```

---

#### Backtracking

**Backtracking** = zkus krok, pokud nevede k řešení, VRAŤ HO ZPĚT a zkus jiný.

```csharp
// ✅ KOSTRA BACKTRACKINGU
static bool Backtrack(Stav stav)
{
    // Base case - máme řešení?
    if (JeReseni(stav))
        return true;
    
    foreach (var moznost in MozneKroky(stav))
    {
        Aplikuj(stav, moznost);       // Udělej krok
        
        if (Backtrack(stav))          // Rekurze
            return true;
        
        Zrus(stav, moznost);          // ← BACKTRACK! Vrať krok zpět
    }
    
    return false;  // Žádná možnost nevyšla
}
```

**Příklad - Bludiště:**

```csharp
static bool NajdiCestu(int[,] bludiste, int r, int c, bool[,] navstiveno)
{
    // Base cases
    if (r < 0 || c < 0 || r >= bludiste.GetLength(0) || c >= bludiste.GetLength(1))
        return false;
    if (bludiste[r, c] == 1 || navstiveno[r, c])
        return false;
    if (bludiste[r, c] == 9)  // Cíl!
        return true;
    
    navstiveno[r, c] = true;  // Označ jako navštívené
    
    // Zkus všechny směry
    if (NajdiCestu(bludiste, r - 1, c, navstiveno)) return true;  // Nahoru
    if (NajdiCestu(bludiste, r + 1, c, navstiveno)) return true;  // Dolů
    if (NajdiCestu(bludiste, r, c - 1, navstiveno)) return true;  // Vlevo
    if (NajdiCestu(bludiste, r, c + 1, navstiveno)) return true;  // Vpravo
    
    navstiveno[r, c] = false;  // ← BACKTRACK!
    return false;
}
```

---

#### Rozděl a panuj (Divide and Conquer)

**Princip:** Rozděl problém na menší části → vyřeš každou zvlášť → spoj výsledky.

| Algoritmus | Co dělá | Složitost |
|------------|---------|-----------|
| **MergeSort** | Třídění pole | O(n log n) |
| **QuickSort** | Třídění pole | O(n log n) průměr |
| **QuickSelect** | Najdi k-tý nejmenší | O(n) průměr |
| **Binární vyhledávání** | Najdi prvek v setříděném poli | O(log n) |

---

#### MergeSort

```csharp
static void MergeSort(int[] pole, int levy, int pravy)
{
    if (levy >= pravy)
        return;
    
    int stred = (levy + pravy) / 2;
    
    MergeSort(pole, levy, stred);       // Setřiď levou polovinu
    MergeSort(pole, stred + 1, pravy);  // Setřiď pravou polovinu
    Merge(pole, levy, stred, pravy);    // Slij dohromady
}

static void Merge(int[] pole, int levy, int stred, int pravy)
{
    int[] temp = new int[pravy - levy + 1];
    int i = levy, j = stred + 1, k = 0;
    
    while (i <= stred && j <= pravy)
    {
        if (pole[i] <= pole[j])
            temp[k++] = pole[i++];
        else
            temp[k++] = pole[j++];
    }
    
    while (i <= stred)
        temp[k++] = pole[i++];
    while (j <= pravy)
        temp[k++] = pole[j++];
    
    for (int m = 0; m < temp.Length; m++)
        pole[levy + m] = temp[m];
}
```

**Vizualizace:**
```
         [38, 27, 43, 3]          ← ROZDĚL
               │
        ┌──────┴──────┐
    [38, 27]      [43, 3]         ← ROZDĚL
        │             │
    ┌───┴───┐     ┌───┴───┐
  [38]    [27]  [43]    [3]       ← Base case
    └───┬───┘     └───┬───┘
    [27, 38]      [3, 43]         ← SPOJ
        └──────┬──────┘
         [3, 27, 38, 43]          ← SPOJ
```

**Složitost:** O(n log n) – vždy!

---

#### QuickSort

```csharp
static void QuickSort(int[] pole, int levy, int pravy)
{
    if (levy >= pravy)
        return;
    
    int pivotIndex = Partition(pole, levy, pravy);
    
    QuickSort(pole, levy, pivotIndex - 1);
    QuickSort(pole, pivotIndex + 1, pravy);
}
```

**Průměrná složitost:** O(n log n)  
**Nejhorší případ:** O(n²) – při špatném pivotu

---

#### Binární vyhledávání

```csharp
static int BinarniVyhledavani(int[] pole, int levy, int pravy, int hledany)
{
    if (levy > pravy)
        return -1;  // Nenašli
    
    int stred = (levy + pravy) / 2;
    
    if (pole[stred] == hledany)
        return stred;                                              // NAŠLI!
    else if (hledany < pole[stred])
        return BinarniVyhledavani(pole, levy, stred - 1, hledany); // VLEVO
    else
        return BinarniVyhledavani(pole, stred + 1, pravy, hledany); // VPRAVO
}
```

**Složitost:** O(log n)

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Chybějící base case** → StackOverflow
2. **Naivní Fibonacci** → O(2^n), musíš použít memoizaci nebo iteraci
3. **Backtrack = VRÁTIT KROK** → nezapomeň zrušit změnu stavu
4. **Koncová vs běžná rekurze** → po koncovém volání se nic nepočítá
5. **DFS pořadí** → záleží na pořadí sousedů v seznamu!
6. **QuickSort pivot** → špatný pivot = O(n²)
7. **`i++` vs `++i`** → `i++` použij a pak zvyš, `++i` zvyš a pak použij
8. **MergeSort paměť** → potřebuje O(n) pomocné paměti

---

## 🚀 Senior Tip

Memoizace pomocí Dictionary je základní technika dynamického programování:

```csharp
private static Dictionary<int, long> cache = new Dictionary<int, long>();

static long Fibonacci(int n)
{
    if (n <= 1) return n;
    if (cache.TryGetValue(n, out long result))
        return result;
    
    return cache[n] = Fibonacci(n - 1) + Fibonacci(n - 2);
}
```

---

## 📊 Tabulka složitostí

| Algoritmus | Čas | Paměť | Poznámka |
|------------|-----|-------|----------|
| Faktoriál | O(n) | O(n) | Zásobník |
| Fibonacci naivní | O(2^n) | O(n) | ❌ NEPOUŽÍVAT |
| Fibonacci memo | O(n) | O(n) | ✅ |
| Fibonacci iterace | O(n) | O(1) | ✅ Nejlepší |
| Permutace | O(n!) | O(n) | |
| QuickSelect avg | O(n) | O(log n) | |
| QuickSelect worst | O(n²) | O(n) | Špatný pivot |
| DFS | O(V+E) | O(V) | V=vrcholy, E=hrany |
| MergeSort | O(n log n) | O(n) | Stabilní |
| QuickSort avg | O(n log n) | O(log n) | In-place |
| QuickSort worst | O(n²) | O(n) | Špatný pivot |
| Binární vyhledávání | O(log n) | O(log n) | Rekurzivně |

---

## 🔗 Souvislosti s jinými otázkami

| Otázka | Téma | Souvislost |
|--------|------|------------|
| **Ot. 3** | Zásobník | Nahrazení rekurze, volací zásobník |
| **Ot. 7** | Časová složitost | Složitost rekurzivních algoritmů |
| **Ot. 9** | Stromy | Průchod stromem rekurzivně |
| **Ot. 11** | MergeSort | Rozděl a panuj |
| **Ot. 12** | QuickSort | Rozděl a panuj, pivot |
| **Ot. 15** | Backtracking | Rozšíření rekurze |
| **Ot. 22** | DFS/BFS | Prohledávání grafů |

---

## 📋 Maturitní úlohy k procvičení

| Úloha | Soubor | Téma |
|-------|--------|------|
| 101 | 0-32 | Aproximace e (faktoriál) |
| 158 | 0-32 | Rozklady čísla na součty |
| 163-164 | 0-32 | Permutace |

---

*Zápisky vytvořeny: 2025-01-08*
*Aktualizováno: 2025-01-31*
