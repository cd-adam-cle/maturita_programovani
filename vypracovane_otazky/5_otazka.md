# Zápisky: Otázka č. 5 - Rekurze a její využití

**Téma:** Rekurze, její mechanika, využití, složitost, výhody/nevýhody a alternativy

---

## Checklist bodů otázky

- [x] Bod 1: Pojem rekurze – definice, mechanika, druhy
- [x] Bod 2: Příklady využití (faktoriál, Fibonacci, permutace, QuickSelect) + složitost
- [x] Bod 3: Výhody a nevýhody rekurze
- [x] Bod 4: Kde je (ne)efektivní rekurzi použít
- [x] Bod 5: Koncová rekurze (tail recursion)
- [x] Bod 6: Nahrazení rekurze zásobníkem (rozdíl Call Stack vs vlastní stack)
- [x] Bod 7: StackOverflowException
- [x] Bod 8: Navazující témata (DFS, Backtracking, MergeSort, QuickSort, BSearch)
- [x] Bod 9: Memoizace a souvislost s dynamickým programováním

---

## BOD 1: POJEM REKURZE

### Definice

**Rekurze** je situace, kdy funkce **volá sama sebe**, aby vyřešila menší instanci stejného problému. Z anglického *recursion*, z latinského *recurrere* = "vracet se".

### Dvě nutné části každé rekurzivní funkce

1. **Base case (ukončovací podmínka, kotva)** – případ, kdy už se rekurzivně nevolá. Bez něj by funkce volala sama sebe donekonečna → StackOverflowException.
2. **Recursive case (rekurzivní krok)** – funkce zavolá sama sebe s **menší / jednodušší** instancí problému, která se postupně blíží base case.

```csharp
static void Rekurze(int n) {
    // 1. BASE CASE - kdy přestat
    if (n <= 0) return;

    // 2. Užitečná práce
    Console.WriteLine(n);

    // 3. RECURSIVE CASE - zmenši problém a zavolej se znovu
    Rekurze(n - 1);
}
```

### Mechanika rekurze - Call Stack

Každé volání funkce vytvoří na Call Stacku nový **stack frame** s:
- Parametry volání
- Lokálními proměnnými
- Návratovou adresou (kam pokračovat po `return`)

```
Rekurze(3)  vytvoří frame, zavolá Rekurze(2)
Rekurze(2)  vytvoří frame, zavolá Rekurze(1)
Rekurze(1)  vytvoří frame, zavolá Rekurze(0)
Rekurze(0)  vrátí (base case) → frame se odebere
Rekurze(1)  pokračuje za voláním → vrátí → odebere
Rekurze(2)  pokračuje → vrátí → odebere
Rekurze(3)  pokračuje → vrátí → odebere

Call Stack během běhu:
┌────────────┐
│ Rekurze(0) │ ← vrchol (právě se vykonává)
├────────────┤
│ Rekurze(1) │
├────────────┤
│ Rekurze(2) │
├────────────┤
│ Rekurze(3) │
├────────────┤
│ Main()     │
└────────────┘
```

### Druhy rekurze

| Druh | Popis | Příklad |
|------|-------|---------|
| **Přímá** | Funkce volá sama sebe přímo | `Faktorial → Faktorial` |
| **Nepřímá (mutual)** | A volá B, B volá A | `JeSude → JeLiche → JeSude` |
| **Lineární** | Jediné rekurzivní volání v každém kroku | Faktoriál |
| **Stromová (tree)** | Více rekurzivních volání v jednom kroku | Naivní Fibonacci |
| **Koncová (tail)** | Rekurzivní volání je poslední operace | `return Helper(n-1, akku*n)` |

**Příklad nepřímé rekurze:**
```csharp
static bool JeSude(int n) {
    if (n == 0) return true;
    return JeLiche(n - 1);
}

static bool JeLiche(int n) {
    if (n == 0) return false;
    return JeSude(n - 1);
}
```

### Analogie

- **Ruské matrjošky** – uvnitř každé je menší, dokud nedojdeš k nejmenší
- **Zrcadlo proti zrcadlu** – obraz v obrazu v obrazu...
- **Mapa v mapě** – mapa Česka, na ní mapa Brna, na ní mapa centra Brna

---

## BOD 2: PŘÍKLADY VYUŽITÍ + SLOŽITOST

### 2.1 Faktoriál - klasický příklad lineární rekurze

```csharp
static long Faktorial(int n) {
    if (n <= 1) return 1;            // BASE CASE
    return n * Faktorial(n - 1);     // RECURSIVE CASE
}

// Faktorial(5) = 5 × 4 × 3 × 2 × 1 = 120
```

**Rozvinutí volání:**
```
Faktorial(5)
= 5 * Faktorial(4)
= 5 * (4 * Faktorial(3))
= 5 * (4 * (3 * Faktorial(2)))
= 5 * (4 * (3 * (2 * Faktorial(1))))
= 5 * (4 * (3 * (2 * 1)))
= 120
```

**Složitost:**
- Čas: O(n) – n rekurzivních volání
- Paměť: O(n) – n stack framů na Call Stacku

---

### 2.2 Fibonacci - varování před stromovou rekurzí

```csharp
// NAIVNÍ - exponenciální, NEPOUŽÍVAT!
static int FibNaivni(int n) {
    if (n <= 1) return n;
    return FibNaivni(n - 1) + FibNaivni(n - 2);   // 2 volání!
}
```

**Strom volání pro Fib(5):**
```
                    Fib(5)
                   /      \
              Fib(4)        Fib(3)
              /    \        /    \
          Fib(3)  Fib(2)  Fib(2)  Fib(1)
          /  \    /  \    /  \
       Fib(2) Fib(1) ... (Fib(2), Fib(3) se počítají VÍCKRÁT!)
```

**Problém:** stejné podproblémy se počítají opakovaně. Fib(40) udělá ~330 milionů volání!

**Složitost naivní verze:** O(2ⁿ) – exponenciální.

**Řešení 1 – memoizace (top-down DP):**
```csharp
static long FibMemo(int n, Dictionary<int, long> cache) {
    if (n <= 1) return n;
    if (cache.TryGetValue(n, out long v)) return v;

    long vysledek = FibMemo(n - 1, cache) + FibMemo(n - 2, cache);
    cache[n] = vysledek;
    return vysledek;
}
```
**Složitost:** O(n) čas, O(n) paměť.

**Řešení 2 – iterace (bottom-up):**
```csharp
static long FibIterativne(int n) {
    if (n <= 1) return n;
    long a = 0, b = 1;
    for (int i = 2; i <= n; i++) {
        (a, b) = (b, a + b);
    }
    return b;
}
```
**Složitost:** O(n) čas, O(1) paměť – nejlepší řešení.

---

### 2.3 Permutace - příklad backtrackingu

**Permutace** = všechny způsoby seřazení prvků. Pro pole `[1, 2, 3]` existuje **3! = 6** permutací:
```
[1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1]
```

```csharp
static void Permutace(int[] pole, int start) {
    if (start == pole.Length - 1) {
        Console.WriteLine(string.Join(", ", pole));
        return;
    }

    for (int i = start; i < pole.Length; i++) {
        Prohod(pole, start, i);          // 1. zkus prvek na pozici start
        Permutace(pole, start + 1);      // 2. rekurze pro zbytek
        Prohod(pole, start, i);          // 3. BACKTRACK - vrať zpět
    }
}

static void Prohod(int[] pole, int i, int j) {
    (pole[i], pole[j]) = (pole[j], pole[i]);
}
```

**Strom volání pro [1,2,3]:**
```
                  start=0
       ┌────────────┼────────────┐
   prohod 0↔0   prohod 0↔1    prohod 0↔2
    [1,2,3]      [2,1,3]       [3,2,1]
       │            │              │
    start=1      start=1        start=1
     ┌─┴─┐        ┌─┴─┐          ┌─┴─┐
  ...              ...             ...
```

**Složitost:** O(n!) – faktoriál (n! permutací × O(n) na výpis).

---

### 2.4 QuickSelect - hledání k-tého nejmenšího prvku

**Problém:** najdi k-tý nejmenší prvek BEZ úplného třídění pole.

```csharp
static int QuickSelect(int[] pole, int levy, int pravy, int k) {
    if (levy == pravy) return pole[levy];

    int pivotIndex = Partition(pole, levy, pravy);

    if (k == pivotIndex)
        return pole[k];                                    // NAŠLI
    else if (k < pivotIndex)
        return QuickSelect(pole, levy, pivotIndex - 1, k); // VLEVO
    else
        return QuickSelect(pole, pivotIndex + 1, pravy, k); // VPRAVO
}

static int Partition(int[] pole, int levy, int pravy) {
    int pivot = pole[pravy];
    int i = levy - 1;
    for (int j = levy; j < pravy; j++) {
        if (pole[j] <= pivot) {
            i++;
            (pole[i], pole[j]) = (pole[j], pole[i]);
        }
    }
    (pole[i + 1], pole[pravy]) = (pole[pravy], pole[i + 1]);
    return i + 1;
}
```

**Princip:** rozděl pole podle pivotu, vyber **jen tu polovinu**, kde leží k-tá pozice. Tím se vyhneš zbytečnému třídění druhé poloviny.

**Složitost:**
- Průměr: O(n) – v každém kroku zpracuješ polovinu, n + n/2 + n/4 + ... = 2n
- Nejhorší případ: O(n²) – pokud pivot stále dělí 1:n-1

---

### 2.5 Další klasické příklady rekurze

| Algoritmus | Princip rekurze | Složitost |
|------------|------------------|-----------|
| Součet číslic čísla | `f(n) = n%10 + f(n/10)` | O(log n) |
| Reverse stringu | `f(s) = f(s[1:]) + s[0]` | O(n²) bez akumulátoru |
| Mocnina | `x^n = x * x^(n-1)` nebo `x^n = (x^(n/2))²` | O(n) / O(log n) |
| Hanojské věže | `Move(n) = Move(n-1) + 1 + Move(n-1)` | O(2ⁿ) |
| GCD (Euklides) | `gcd(a, b) = gcd(b, a%b)` | O(log min(a,b)) |
| Strom – součet hodnot | `sum(node) = node.val + sum(left) + sum(right)` | O(n) |

---

## BOD 3: VÝHODY A NEVÝHODY REKURZE

| VÝHODY | NEVÝHODY |
|--------|----------|
| Čitelnější kód pro stromové a fraktální problémy | Vyšší paměťová náročnost (každé volání = stack frame) |
| Přirozený zápis pro "rozděl a panuj" | Riziko StackOverflowException při velké hloubce |
| Eleganší řešení pro problémy s rekurzivní strukturou | Pomalejší (overhead volání funkce vs cyklu) |
| Jednoduchý kód pro průchod stromy a grafy (DFS) | Hůře debuggovatelné (mnoho framů na stacku) |
| Přirozený backtracking (snadné "vrátit krok") | Stromová rekurze bez memoizace → exponenciální čas |

---

## BOD 4: KDE JE (NE)EFEKTIVNÍ POUŽÍT REKURZI

### Vhodné pro rekurzi (efektivní)

- **Stromové struktury** – průchod stromem (in-order, pre-order, post-order)
- **Grafy** – DFS, hledání komponent
- **Rozděl a panuj** – MergeSort, QuickSort, binární vyhledávání
- **Backtracking** – Sudoku, N dam, bludiště, kombinace, permutace
- **Fraktály a geometrické rekurze** – Kochova vločka, Sierpińského trojúhelník
- **Funkcionální problémy** – mapy, redukce, parsování gramatik

### Nevhodné pro rekurzi (neefektivní)

- **Lineární průchody** – součet pole, hledání max → lépe cyklus
- **Překrývající se podproblémy bez memoizace** – naivní Fibonacci → O(2ⁿ)
- **Příliš hluboká rekurze** – pro n=10⁶ vyhodí StackOverflow
- **Výkonově kritický kód** – overhead volání > tělo iterace
- **Tail rekurze v C#** – kompilátor ji neoptimalizuje na cyklus (na rozdíl od F#)

```
            POUŽÍT REKURZI?
                  │
       ┌──────────┴──────────┐
       │                     │
  Stromová /            Lineární průchod
  rozděl a panuj /      nebo velmi hluboký?
  backtracking?              │
       │                    ANO
      ANO                    │
       │                     ▼
       ▼               POUŽIJ CYKLUS
  POUŽIJ REKURZI       NEBO RUČNÍ STACK
```

---

## BOD 5: KONCOVÁ REKURZE (TAIL RECURSION)

### Definice

**Koncová rekurze** je taková, kde rekurzivní volání je **úplně poslední** operací funkce – po něm už se nic nepočítá.

```csharp
// NENÍ koncová - po Faktorial(n-1) se ještě násobí
static int Faktorial(int n) {
    if (n == 0) return 1;
    return n * Faktorial(n - 1);   // násobení JE poslední operace
}

// JE koncová - po FaktorialTail(n-1, ...) už se nic nedělá
static int FaktorialTail(int n, int akumulator = 1) {
    if (n == 0) return akumulator;
    return FaktorialTail(n - 1, n * akumulator);   // přímo vrátí výsledek volání
}
```

### Tail Call Optimization (TCO)

Některé kompilátory umí koncovou rekurzi **převést na cyklus** – místo přidání nového framu na stack se znovu použije ten stávající. Tím se ušetří paměť a předejde StackOverflow.

**Podpora TCO:**
- F#, Scala, Haskell, Scheme – ANO
- **C#** – ne (JIT může v některých případech, ale spolehnout se nedá)
- Python – NE

**Důsledek:** v C# nepřináší koncová rekurze přímý výkonový benefit, ale je dobrá praxe – ukazuje, že rekurzi lze snadno převést na iteraci.

---

## BOD 6: NAHRAZENÍ REKURZE ZÁSOBNÍKEM

### Proč nahrazovat?

- **Bez rizika StackOverflow** – ruční stack na **heapu** (~GB) má prakticky neomezenou velikost, oproti Call Stacku (~1 MB)
- **Lepší kontrola** – můžeš pozastavit, uložit stav, pokračovat
- **Předvídatelná paměť** – víš, kolik místa ukládáš
- **Bez režie volání funkce** – obvykle rychlejší

### Princip převodu

Každé rekurzivní volání odpovídá **push** na ruční stack. Návrat z rekurze odpovídá **pop**. Lokální proměnné, které potřebujeme po návratu z rekurze, musíme uložit do struktury, kterou pushneme.

### Příklad - DFS

```csharp
// REKURZIVNĚ
static void DFS_Rekurze(int vrchol, bool[] navstiveno, List<int>[] graf) {
    if (navstiveno[vrchol]) return;
    navstiveno[vrchol] = true;
    Console.WriteLine(vrchol);

    foreach (int soused in graf[vrchol]) {
        DFS_Rekurze(soused, navstiveno, graf);
    }
}

// ITERATIVNĚ S RUČNÍM ZÁSOBNÍKEM
static void DFS_Stack(int start, bool[] navstiveno, List<int>[] graf) {
    Stack<int> stack = new Stack<int>();
    stack.Push(start);

    while (stack.Count > 0) {
        int vrchol = stack.Pop();

        if (navstiveno[vrchol]) continue;
        navstiveno[vrchol] = true;
        Console.WriteLine(vrchol);

        foreach (int soused in graf[vrchol]) {
            if (!navstiveno[soused])
                stack.Push(soused);
        }
    }
}
```

### Srovnání Call Stack vs ruční Stack<T>

| Vlastnost | Call Stack (rekurze) | Stack<T> (iterace) |
|-----------|---------------------|---------------------|
| Umístění v paměti | Stack (~1 MB) | Heap (~GB) |
| Spravuje | Runtime/CPU automaticky | Programátor ručně |
| StackOverflow při hloubce | Ano (~10 000 volání) | Ne (omezeno jen heapem) |
| Kód | Stručný, elegantní | Delší, explicitní |
| Rychlost | Mírně pomalejší (overhead) | Rychlejší |
| Debugging | Stack trace je čitelný | Stav lze inspektovat ručně |

### Některé převody jsou snadné, jiné těžké

- **Snadné:** koncová rekurze → cyklus jedna ku jedné (kompilátor by to udělal sám)
- **Středně těžké:** lineární rekurze (faktoriál) – stačí akumulátor
- **Těžší:** stromová rekurze (Fibonacci, průchod stromem) – potřebuje strukturu na ukládání mezistavů
- **Nejtěžší:** nepřímá rekurze – nutno simulovat oba stavy

---

## BOD 7: STACKOVERFLOWEXCEPTION

### Co to je?

Výjimka `StackOverflowException` nastane, když **Call Stack přeteče** – obvykle při příliš hluboké rekurzi.

Velikost Call Stacku v .NET je typicky **~1 MB**, což odpovídá zhruba **10 000 - 50 000 vnořeným voláním** (záleží na velikosti framů).

### Příčiny

**1. Chybějící base case:**
```csharp
static void Nekonecna(int n) {
    Console.WriteLine(n);
    Nekonecna(n + 1);   // nikdy nepřestane!
}
```

**2. Base case se nikdy nedosáhne:**
```csharp
static int Faktorial(int n) {
    if (n == 0) return 1;
    return n * Faktorial(n - 1);   // pro n=-1 půjde do -∞
}

Faktorial(-5);   // přeteče stack!
```

**3. Příliš hluboká rekurze (správný kód, ale velký vstup):**
```csharp
static int Soucet(int n) {
    if (n == 0) return 0;
    return n + Soucet(n - 1);
}

Soucet(1_000_000);   // StackOverflow - příliš mnoho framů
```

### Důležitá vlastnost StackOverflowException

V .NET tuto výjimku **nelze chytit** pomocí `try-catch` (od .NET 2.0). Aplikace se prostě ukončí. Důvod: stav stacku je natolik poškozený, že další kód by mohl být nedefinovaný.

```csharp
try {
    Nekonecna(0);
} catch (StackOverflowException) {
    // NEVOLÁ SE! Program prostě umře.
}
```

### Řešení

1. **Opravit base case** – první co kontroluj
2. **Použít iteraci s cyklem** – pro lineární průchody
3. **Použít ruční Stack<T> na heapu** – pro stromové průchody s velkou hloubkou
4. **Memoizace** – sníží počet volání u stromové rekurze
5. **Tail-call optimalizace** – v jazycích, které ji podporují (ne C#)
6. **Zvětšit velikost stacku** – `new Thread(work, stackSize: 16_000_000)` – krajní řešení

---

## BOD 8: NAVAZUJÍCÍ TÉMATA

### 8.1 DFS (Depth-First Search)

```csharp
static void DFS(int vrchol, bool[] navstiveno, List<int>[] graf) {
    if (navstiveno[vrchol]) return;
    navstiveno[vrchol] = true;
    Console.WriteLine(vrchol);

    foreach (int soused in graf[vrchol])
        DFS(soused, navstiveno, graf);
}
```

**Složitost:** O(V + E), V = vrcholy, E = hrany.

**Použití:** detekce cyklů, topologické třídění, hledání komponent souvislosti, řešení bludišť.

---

### 8.2 Backtracking - "zkus, vrať se, zkus jinak"

**Princip:** zkus krok → pokud nevede k řešení, **vrať ho zpět** a zkus jiný.

```csharp
static bool Backtrack(Stav stav) {
    if (JeReseni(stav)) return true;

    foreach (var moznost in MozneKroky(stav)) {
        Aplikuj(stav, moznost);                // 1. udělej krok
        if (Backtrack(stav)) return true;      // 2. rekurze
        Zrus(stav, moznost);                   // 3. BACKTRACK
    }

    return false;
}
```

**Klasické backtracking problémy:**
- **N dam** – umísti N dam na šachovnici tak, aby se neohrožovaly
- **Sudoku** – doplň prázdná políčka
- **Bludiště** – najdi cestu od startu k cíli
- **Permutace, kombinace, podmnožiny**
- **Obarvení grafu**

**Příklad - hledání cesty v bludišti:**
```csharp
static bool NajdiCestu(int[,] bludiste, int r, int c, bool[,] navstiv) {
    if (r < 0 || c < 0 || r >= bludiste.GetLength(0) || c >= bludiste.GetLength(1))
        return false;
    if (bludiste[r, c] == 1 || navstiv[r, c]) return false;  // zeď nebo už navštíveno
    if (bludiste[r, c] == 9) return true;                    // cíl!

    navstiv[r, c] = true;

    if (NajdiCestu(bludiste, r - 1, c, navstiv)) return true;
    if (NajdiCestu(bludiste, r + 1, c, navstiv)) return true;
    if (NajdiCestu(bludiste, r, c - 1, navstiv)) return true;
    if (NajdiCestu(bludiste, r, c + 1, navstiv)) return true;

    navstiv[r, c] = false;   // BACKTRACK
    return false;
}
```

---

### 8.3 Rozděl a panuj (Divide & Conquer)

**Princip:** rozděl problém na menší podproblémy stejného typu → vyřeš každý zvlášť → spoj výsledky.

| Algoritmus | Podproblémy | Slévání | Složitost |
|------------|-------------|---------|-----------|
| **MergeSort** | 2× pole o n/2 | Merge v O(n) | O(n log n) |
| **QuickSort** | 2× pole (závisí na pivotu) | Žádné (in-place) | O(n log n) avg |
| **QuickSelect** | 1× pole (jen jedna polovina) | Žádné | O(n) avg |
| **Binární vyhledávání** | 1× pole o n/2 | Žádné | O(log n) |
| **Karatsuba** (násobení) | 3× čísla o n/2 cifer | O(n) | O(n^1.585) |

#### MergeSort

```csharp
static void MergeSort(int[] pole, int levy, int pravy) {
    if (levy >= pravy) return;

    int stred = (levy + pravy) / 2;
    MergeSort(pole, levy, stred);
    MergeSort(pole, stred + 1, pravy);
    Merge(pole, levy, stred, pravy);
}

static void Merge(int[] pole, int levy, int stred, int pravy) {
    int[] temp = new int[pravy - levy + 1];
    int i = levy, j = stred + 1, k = 0;

    while (i <= stred && j <= pravy) {
        if (pole[i] <= pole[j]) temp[k++] = pole[i++];
        else                    temp[k++] = pole[j++];
    }
    while (i <= stred)  temp[k++] = pole[i++];
    while (j <= pravy)  temp[k++] = pole[j++];

    for (int m = 0; m < temp.Length; m++)
        pole[levy + m] = temp[m];
}
```

**Vizualizace:**
```
       [38, 27, 43, 3]              ROZDĚL
              │
       ┌──────┴──────┐
   [38, 27]      [43, 3]            ROZDĚL
       │             │
    ┌──┴──┐       ┌──┴──┐
  [38] [27]    [43]  [3]            BASE CASE
    └──┬──┘       └──┬──┘
  [27, 38]       [3, 43]            SLÉVÁNÍ
       └──────┬──────┘
       [3, 27, 38, 43]              SLÉVÁNÍ
```

**Složitost:** O(n log n) vždy (i nejhorší případ), O(n) pomocné paměti. Stabilní.

#### QuickSort

```csharp
static void QuickSort(int[] pole, int levy, int pravy) {
    if (levy >= pravy) return;
    int pivot = Partition(pole, levy, pravy);
    QuickSort(pole, levy, pivot - 1);
    QuickSort(pole, pivot + 1, pravy);
}
```

**Složitost:** O(n log n) průměr, O(n²) nejhorší (při špatně voleném pivotu). In-place (O(log n) paměti na zásobníku).

---

### 8.4 Binární vyhledávání rekurzivně

```csharp
static int BSearch(int[] pole, int levy, int pravy, int hledany) {
    if (levy > pravy) return -1;
    int stred = (levy + pravy) / 2;

    if (pole[stred] == hledany) return stred;
    if (hledany < pole[stred])
        return BSearch(pole, levy, stred - 1, hledany);
    else
        return BSearch(pole, stred + 1, pravy, hledany);
}
```

**Složitost:** O(log n) – v každém kroku půlíme pole.

---

## BOD 9: MEMOIZACE A DYNAMICKÉ PROGRAMOVÁNÍ

### Memoizace = caching rekurzivních výsledků

```csharp
private static Dictionary<int, long> cache = new Dictionary<int, long>();

static long Fib(int n) {
    if (n <= 1) return n;
    if (cache.TryGetValue(n, out long v)) return v;

    long vysledek = Fib(n - 1) + Fib(n - 2);
    cache[n] = vysledek;
    return vysledek;
}
```

**Memoizace = "top-down dynamické programování"** – řešíš problém shora, výsledky podproblémů si pamatuješ.

**Tabulace ("bottom-up dynamické programování"):**
```csharp
static long Fib(int n) {
    if (n <= 1) return n;
    long[] tab = new long[n + 1];
    tab[0] = 0; tab[1] = 1;
    for (int i = 2; i <= n; i++)
        tab[i] = tab[i - 1] + tab[i - 2];
    return tab[n];
}
```

| Memoizace | Tabulace |
|-----------|----------|
| Rekurzivní (top-down) | Iterativní (bottom-up) |
| Počítá jen potřebné podproblémy | Počítá vždy všechny |
| Snadnější převod z rekurze | Stručnější kód |
| Stack overhead z rekurze | Bez rizika StackOverflow |

---

## MATURITNÍ CHYTÁKY

1. **Chybějící base case** → StackOverflowException – nejčastější chyba začátečníků
2. **Naivní Fibonacci je O(2ⁿ)** – znát memoizaci nebo iterativní řešení
3. **Backtrack = vrátit krok zpět** – pokud změníš stav před rekurzí, musíš ho po neúspěšné rekurzi vrátit
4. **Koncová vs běžná rekurze** – po koncovém volání už se nic nepočítá (jen vrací výsledek)
5. **DFS pořadí závisí na pořadí sousedů** – pro stejný graf různé pořadí výpisu
6. **QuickSort pivot** – špatný pivot (např. první prvek setříděného pole) → O(n²)
7. **MergeSort paměť** – potřebuje O(n) pomocné pole (na rozdíl od QuickSortu)
8. **Rekurze má skrytou paměť** – n volání = n stack framů = O(n) paměť
9. **StackOverflowException nelze chytit** v .NET pomocí try-catch
10. **C# neoptimalizuje tail-call** – psát koncovou rekurzi nepomůže výkonu
11. **Memoizace musí být na rekurzivních voláních**, ne až na vrchním (jinak nemá efekt)
12. **Hloubka ≠ počet volání** – pro stromovou rekurzi je počet volání exponenciální, hloubka jen lineární

---

## TABULKA SLOŽITOSTÍ

| Algoritmus | Čas | Paměť | Poznámka |
|------------|-----|-------|----------|
| Faktoriál | O(n) | O(n) | Lineární rekurze |
| Fibonacci naivně | O(2ⁿ) | O(n) | NEPOUŽÍVAT |
| Fibonacci s memoizací | O(n) | O(n) | Cache + stack |
| Fibonacci iterativně | O(n) | O(1) | Nejlepší |
| Permutace | O(n!) | O(n) | Hloubka rekurze = n |
| QuickSelect průměr | O(n) | O(log n) | Lepší než třídit |
| QuickSelect nejhorší | O(n²) | O(n) | Špatný pivot |
| DFS | O(V+E) | O(V) | Stack i navštívené |
| MergeSort | O(n log n) | O(n) | Stabilní |
| QuickSort průměr | O(n log n) | O(log n) | In-place |
| QuickSort nejhorší | O(n²) | O(n) | Špatný pivot |
| Binární vyhledávání | O(log n) | O(log n) | Rekurzivně |
| Binární vyhledávání iter. | O(log n) | O(1) | Bez stacku |
| Hanojské věže | O(2ⁿ) | O(n) | 2ⁿ−1 přesunů |
| GCD (Euklides) | O(log min(a,b)) | O(log) | |
| Mocnina rychle | O(log n) | O(log n) | Půlení |

---

## KLÍČOVÉ POJMY K ZAPAMATOVÁNÍ

- **Rekurze** = funkce volá sama sebe, musí mít base case + recursive case
- **Mechanika:** Call Stack ukládá frame na každé volání (parametry, lokální proměnné, návratová adresa)
- **Druhy:** přímá vs nepřímá, lineární vs stromová, běžná vs koncová
- **Výhody:** elegance pro stromové problémy, přirozený zápis D&C a backtrackingu
- **Nevýhody:** paměť (Call Stack), pomalost, StackOverflow při velké hloubce
- **Koncová rekurze:** rekurzivní volání je poslední operace; v C# ale není TCO
- **Náhrada rekurze:** ruční `Stack<T>` na heapu místo Call Stacku
- **StackOverflowException:** Call Stack ~1 MB, ~10 000 framů, nelze chytit v try-catch
- **Memoizace:** ukládání výsledků podproblémů – převede O(2ⁿ) Fibonacci na O(n)
- **Rozděl a panuj:** MergeSort O(n log n), QuickSort O(n log n) průměr, BSearch O(log n)
- **Backtracking:** zkus krok → rekurze → vrať krok zpět (N dam, Sudoku, bludiště)
