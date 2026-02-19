# 📚 Zápisky: Otázka č. 15 - Rozděl a panuj. Dynamické programování. Backtracking.

**Datum:** 2025-02-19  
**Status:** ✅ Hotovo

---

## ✅ Checklist bodů otázky

- [x] Bod 1: Klíčová myšlenka Rozděl a panuj + příklad + složitost
- [x] Bod 2: Hlavní myšlenka Dynamického programování + rozdíl oproti D&C + příklad + složitost
- [x] Bod 3: Hlavní myšlenka Backtrackingu + příklad + složitost
- [x] Bod 4: Pro jaké typy úloh použijeme který přístup
- [x] Bod 5: Časové složitosti jednotlivých příkladů

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: Rozděl a panuj (Divide & Conquer)

**Teorie:**

Strategie řešení problémů ve třech krocích:
1. **ROZDĚL** → Rozlož problém na menší podproblémy (stejného typu)
2. **VYŘEŠ** → Vyřeš podproblémy rekurzivně (až dojdeš k triviálním)
3. **SPOJ** → Zkombinuj řešení podproblémů do řešení původního problému

**Klíčový znak:** Podproblémy jsou **NEZÁVISLÉ** — řešení jednoho nepotřebuje výsledek druhého.

**ASCII vizualizace — Merge Sort:**

```
         [38, 27, 43, 3, 9, 82, 10]
                    |
            ROZDĚL (split na půlky)
           /                    \
    [38, 27, 43, 3]        [9, 82, 10]
       /        \            /       \
  [38, 27]  [43, 3]     [9, 82]   [10]
   /    \    /    \      /    \      |
 [38] [27] [43]  [3]  [9]  [82]  [10]  ← triviální (1 prvek)
   \    /    \    /      \    /      |
  [27,38]  [3,43]      [9,82]    [10]   ← SPOJ (merge)
       \     /            \       /
   [3, 27, 38, 43]     [9, 10, 82]
            \              /
      [3, 9, 10, 27, 38, 43, 82]         ← výsledek!
```

**Příklady algoritmů D&C:**
- **Merge Sort** — rozděl pole, setřiď poloviny, slij → O(n log n)
- **Quick Sort** — vyber pivot, rozděl, setřiď části → O(n log n) průměrně
- **Binární vyhledávání** — půl pole zahod, hledej v druhé půlce → O(log n)

**Složitost Merge Sortu:**

| Případ | Časová | Paměťová |
|--------|--------|----------|
| Nejlepší | O(n log n) | O(n) |
| Průměrný | O(n log n) | O(n) |
| Nejhorší | O(n log n) | O(n) |

**Proč O(n log n)?** Máš log n úrovní dělení × na každé úrovni projdeš n prvků při slévání.

---

### Bod 2: Dynamické programování (DP)

**Teorie:**

DP řeší problém tak, že ho rozloží na **překrývající se podproblémy**, vyřeší každý podproblém **jen jednou** a výsledek si **uloží do paměti** (tabulky/pole), aby ho nemusel počítat znovu.

Představ si to jako **chytrý student u tabule** — jakmile něco spočítá, zapíše si to na papír a příště se jen podívá.

**Klíčový rozdíl oproti Rozděl a panuj:**

```
ROZDĚL A PANUJ:                     DYNAMICKÉ PROGRAMOVÁNÍ:
- Podproblémy NEZÁVISLÉ             - Podproblémy se PŘEKRÝVAJÍ
- Každý podproblém řešíš JEDNOU     - Bez DP bys stejný řešil MNOHOKRÁT
- Příklad: Merge Sort               - DP si výsledek ZAPAMATUJE
```

**Vizualizace na Fibonacci:**

BEZ DP (naivní rekurze) — opakované výpočty:
```
                    fib(5)
                   /      \
              fib(4)      fib(3)      ← fib(3) se počítá 2×!
             /     \      /    \
         fib(3)  fib(2) fib(2) fib(1) ← fib(2) se počítá 3×!
         /   \
     fib(2) fib(1)

→ O(2^n) 💀 — exponenciální, duplikátní práce
```

S DP (tabulace) — každý podproblém JEN JEDNOU:
```
fib(0) = 0  ← spočítáš, uložíš
fib(1) = 1  ← spočítáš, uložíš
fib(2) = 1  ← koukneš na papír: fib(1)+fib(0) = 1, uložíš
fib(3) = 2  ← koukneš na papír: fib(2)+fib(1) = 2, uložíš
fib(4) = 3  ← koukneš na papír: fib(3)+fib(2) = 3, uložíš
fib(5) = 5  ← koukneš na papír: fib(4)+fib(3) = 5, uložíš

→ O(n) ✅ — lineární, žádné duplikáty
```

**Dva přístupy DP:**

| Přístup | Směr | Jak funguje |
|---------|------|-------------|
| **Memoizace** (top-down) | Shora dolů | Rekurze + cache (slovník/pole) |
| **Tabulace** (bottom-up) | Zdola nahoru | Iterativně plníš tabulku od nejmenších podproblémů |

**Kdy použít DP?** DP poznáš podle dvou vlastností:
1. **Optimální substruktura** — řešení se skládá z optimálních řešení podproblémů
2. **Překrývající se podproblémy** — stejné podproblémy se řeší opakovaně

**Kód — Fibonacci DP (tabulace):**

```csharp
// ✅ VERZE A - MATURITNÍ: Tabulace (bottom-up)
static long FibDP(int n)
{
    if (n <= 1) return n;
    
    long[] dp = new long[n + 1];  // "papír" pro výsledky
    dp[0] = 0;
    dp[1] = 1;
    
    for (int i = 2; i <= n; i++)
    {
        dp[i] = dp[i - 1] + dp[i - 2];  // kouknu na papír, dopočítám, zapíšu
    }
    
    return dp[n];
}
// Časová: O(n)  |  Paměťová: O(n)
```

```csharp
// 💡 VERZE B - SENIOR: Memoizace (top-down)
static Dictionary<int, long> memo = new Dictionary<int, long>();

static long FibMemo(int n)
{
    if (n <= 1) return n;
    if (memo.ContainsKey(n)) return memo[n];  // už jsem to počítal?
    
    memo[n] = FibMemo(n - 1) + FibMemo(n - 2);
    return memo[n];
}
// Stejná složitost O(n), ale rekurzivní přístup
```

**Kód — Coin Change (DP):**

```csharp
// ✅ VERZE A - MATURITNÍ
// Úloha: Najdi nejmenší počet mincí pro danou částku
static int CoinChange(int[] mince, int castka)
{
    int[] dp = new int[castka + 1];  // "papír" — dp[i] = min mincí pro částku i
    
    for (int i = 1; i <= castka; i++)
        dp[i] = int.MaxValue;  // zatím nevím řešení
    
    dp[0] = 0;  // pro částku 0 potřebuji 0 mincí
    
    for (int i = 1; i <= castka; i++)
    {
        foreach (int mince_hodnota in mince)
        {
            if (mince_hodnota <= i && dp[i - mince_hodnota] != int.MaxValue)
            {
                // dp[i - mince_hodnota] = kolik mincí na zbytek (kouknu na papír)
                // + 1 za minci, kterou právě používám
                dp[i] = Math.Min(dp[i], dp[i - mince_hodnota] + 1);
            }
        }
    }
    
    return dp[castka] == int.MaxValue ? -1 : dp[castka];
}

// Použití:
// int[] mince = { 1, 3, 5 };
// CoinChange(mince, 7) → 3 (5+1+1 nebo 3+3+1)
// CoinChange(mince, 11) → 3 (5+5+1)
```

**Krok za krokem Coin Change pro částku 7, mince [1,3,5]:**
```
dp[0] = 0
dp[1] = dp[0]+1 = 1       (mince 1)
dp[2] = dp[1]+1 = 2       (mince 1+1)
dp[3] = dp[0]+1 = 1       (mince 3)  ← lepší než dp[2]+1=3!
dp[4] = dp[3]+1 = 2       (mince 3+1)
dp[5] = dp[0]+1 = 1       (mince 5)
dp[6] = dp[5]+1 = 2       (mince 5+1)
dp[7] = dp[6]+1 = 3       (mince 5+1+1)
```

**Složitost Coin Change:** O(částka × počet_mincí) časová, O(částka) paměťová

**Složitost Fibonacci:**

| Přístup | Časová | Paměťová |
|---------|--------|----------|
| Naivní rekurze | O(2^n) 💀 | O(n) stack |
| DP tabulace | **O(n)** ✅ | O(n) |
| DP optimalizované | **O(n)** ✅ | O(1) — stačí 2 proměnné |

---

### Bod 3: Backtracking

**Teorie:**

Backtracking = **"zkus a vrať se"**. Systematicky zkoušíš všechny možnosti, ale jakmile zjistíš, že aktuální cesta nevede k řešení, **VRÁTÍŠ SE** (backtrack) a zkusíš jinou.

Jako bludiště — jdeš jedním směrem, narazíš do zdi, vrátíš se na křižovatku a zkusíš jinou cestu.

**Vzor backtrackingu (šablona):**

```
function Backtrack(stav):
    if stav je ŘEŠENÍ:
        zapiš/vypiš řešení
        return
    
    for každá MOŽNOST z aktuálního stavu:
        if MOŽNOST je validní:          ← "ořezávání" (pruning)
            UDĚLEJ krok (aplikuj možnost)
            Backtrack(nový stav)         ← rekurze hlouběji
            VRAŤ krok (undo)             ← BACKTRACK!
```

**Klíčový znak:** Ořezávání (pruning) — nezkouším VŠECHNY kombinace. Jakmile vím, že cesta je špatná, ZASTAVÍM a vrátím se.

**Rozdíl BFS/DFS vs Backtracking:**
- **BFS/DFS** = procházení grafu (navštívit uzly, najít cestu)
- **Backtracking** = hledání řešení ve stavovém prostoru (zkus → zkontroluji → vrať krok)
- Backtracking dělá **UNDO** (vrací krok), DFS jen značí visited

**Příklad: N-Queens (N dam na šachovnici):**

Úloha: Rozmísti N dam na šachovnici N×N tak, aby se žádné dvě neohrožovaly.

```
Řešení pro N=4:
  
  . Q . .      Q = dáma
  . . . Q      . = prázdné
  Q . . .
  . . Q .
```

**Kód:**

```csharp
// ✅ VERZE A - MATURITNÍ
static void NQueens(int n)
{
    int[] queens = new int[n];  // queens[řádek] = sloupec dámy
    Solve(queens, 0, n);
}

static void Solve(int[] queens, int radek, int n)
{
    // BASE CASE: všechny dámy umístěny = řešení!
    if (radek == n)
    {
        VypisReseni(queens, n);
        return;
    }
    
    // Zkus každý sloupec v tomto řádku
    for (int sloupec = 0; sloupec < n; sloupec++)
    {
        if (JeBezpecne(queens, radek, sloupec))  // ořezávání
        {
            queens[radek] = sloupec;        // UDĚLEJ krok
            Solve(queens, radek + 1, n);     // rekurze (další řádek)
            // queens[radek] se přepíše → automatický UNDO
        }
    }
}

static bool JeBezpecne(int[] queens, int radek, int sloupec)
{
    for (int i = 0; i < radek; i++)
    {
        // Stejný sloupec?
        if (queens[i] == sloupec) return false;
        // Stejná diagonála?
        if (Math.Abs(queens[i] - sloupec) == Math.Abs(i - radek)) return false;
    }
    return true;
}

static void VypisReseni(int[] queens, int n)
{
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
            Console.Write(queens[i] == j ? "Q " : ". ");
        Console.WriteLine();
    }
    Console.WriteLine("---");
}
```

**Vizualizace backtrackingu pro N=4:**
```
Řádek 0: zkus sloupec 0 → Q...
  Řádek 1: zkus sl. 0 → ✗ (stejný sloupec)
            zkus sl. 1 → ✗ (diagonála)
            zkus sl. 2 → ✓ → ..Q.
    Řádek 2: všechny sloupce ✗
              → BACKTRACK! ↩️
  Řádek 1: zkus sl. 3 → ✓ → ...Q
    Řádek 2: zkus sl. 1 → ✓ → .Q..
      Řádek 3: nic nefunguje → BACKTRACK! ↩️
    ... (pokračuje dokud nenajde řešení)
```

**Složitost N-Queens:** O(N!) v nejhorším případě, prakticky méně díky ořezávání

**Reálné využití backtrackingu:**
- Sudoku solvery
- Šachové enginy (prohledávání tahů)
- SAT solvery (verifikace hardware)
- Rozvrhy (rozmístění bez kolizí)
- Regulární výrazy (pattern matching)

---

### Bod 4: Pro jaké typy úloh použijeme který přístup

**Rozhodovací strom:**

```
Dostanu úlohu → ptám se:

1. "Dá se problém rozdělit na NEZÁVISLÉ poloviny?"
   → ANO → ROZDĚL A PANUJ
   → Příklad: "Setřiď pole"

2. "Hledám OPTIMUM (min/max) a podproblémy se OPAKUJÍ?"
   → ANO → DYNAMICKÉ PROGRAMOVÁNÍ
   → Příklad: "Nejmenší počet mincí", "Nejdelší cesta"

3. "Hledám VŠECHNA platná řešení nebo JEDNO co splňuje podmínky?"
   → ANO → BACKTRACKING
   → Příklad: "Rozmísti dámy", "Vyřeš sudoku"
```

**Typické úlohy — přehled:**

| Úloha | Přístup | Proč |
|-------|---------|------|
| Merge Sort | D&C | Nezávislé poloviny |
| Quick Sort | D&C | Nezávislé části kolem pivota |
| Binární vyhledávání | D&C | Půlíš prostor |
| Fibonacci | DP | Překrývající se podproblémy |
| Coin Change (mince) | DP | Hledám minimum, podproblémy se opakují |
| Knapsack (batoh) | DP | Optimalizace s podmínkami |
| N-Queens (dámy) | Backtracking | Hledám platné rozmístění |
| Sudoku solver | Backtracking | Zkouším čísla, vracím se |
| Generování permutací | Backtracking | Všechny kombinace |

---

### Bod 5: Časové složitosti — celkové shrnutí

| Algoritmus | Přístup | Časová složitost | Paměťová |
|------------|---------|-------------------|----------|
| Merge Sort | D&C | O(n log n) | O(n) |
| Quick Sort | D&C | O(n log n) prům. / O(n²) worst | O(log n) |
| Binární vyhledávání | D&C | O(log n) | O(1) |
| Fibonacci naivní | Rekurze | O(2^n) 💀 | O(n) |
| Fibonacci DP | DP | **O(n)** ✅ | O(n) |
| Coin Change | DP | O(částka × mince) | O(částka) |
| N-Queens | Backtracking | O(N!) | O(N) |
| Sudoku solver | Backtracking | O(9^81) worst | O(81) |

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **D&C vs DP** — klíčový rozdíl je v podproblémech: nezávislé (D&C) vs překrývající se (DP)
2. **DP index** — `dp[i - mince_hodnota]` znamená "kolik mincí na ZBYTEK", ne odečítání od počtu mincí
3. **Backtracking vs DFS** — backtracking dělá UNDO (vrací krok), DFS jen značí visited
4. **Backtracking ≠ BFS** — BFS hledá nejkratší cestu po úrovních, backtracking zkouší a vrací se
5. **DP inicializace** — nezapomeň na `int.MaxValue` nebo `-1` pro "ještě nevyřešeno"
6. **Backtracking šablona** — zapamatuj si: zkus → zkontroluji (ořezávání) → rekurze → undo

---

## 🚀 Senior Tipy

1. **DP optimalizace paměti** — Fibonacci nepotřebuje celé pole, stačí 2 proměnné → O(1) paměť
2. **Memoizace vs Tabulace** — memoizace je intuitivnější (rekurze), tabulace je rychlejší (žádný overhead zásobníku)
3. **Backtracking + pruning** — čím lepší ořezávání, tím rychlejší. V praxi se kombinuje s heuristikami
4. **C# Dictionary** — pro memoizaci se hodí `Dictionary<TKey, TValue>`, pro tabulaci stačí pole

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 5 (Rekurze)** — všechny 3 techniky využívají rekurzi
- **Otázka 7 (Složitost)** — porovnání O(n log n) vs O(2^n) vs O(n!)
- **Otázka 11 (Merge Sort)** — konkrétní příklad Rozděl a panuj
- **Otázka 12 (Quick Sort)** — další příklad D&C
- **Otázka 14 (Binární vyhledávání)** — příklad D&C
- **Otázka 22 (DFS/BFS)** — DFS je základ backtrackingu, ale BFS ≠ backtracking

---

## 📋 Procvičovací úlohy (Mini-Index)

1. **Merge Sort** — ukázka Divide & Conquer (souvisí s Bodem 1)
2. **Fibonacci DP** — memoizace vs tabulace (Bod 2)
3. **Coin Change** — nejmenší počet mincí, DP (Bod 2)
4. **Knapsack problem** — dynamické programování (Bod 2)
5. **N-Queens** — backtracking (Bod 3)
6. **Sudoku solver** — backtracking (Bod 3)

---

## 🎯 Klíčová věta pro maturitu

> *"Rozděl a panuj dělí problém na nezávislé části a kombinuje výsledky. Dynamické programování řeší překrývající se podproblémy a pamatuje si výsledky v tabulce, aby nepočítalo nic dvakrát. Backtracking systematicky prohledává všechny možnosti s ořezáváním neplatných cest — zkusí, zkontroluje, a pokud nefunguje, vrátí krok a zkusí jinou cestu."*

---

*📅 Vytvořeno: 2025-02-19 | 🎓 Maturitní příprava PRG 2025/2026*
