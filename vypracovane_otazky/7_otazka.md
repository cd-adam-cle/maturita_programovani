# Zápisky: Otázka č. 7 - Časová a paměťová složitost

## Checklist bodů otázky

- [x] Bod 1: Vysvětlení pojmu – co, k čemu, proč
- [x] Bod 2: Nejhorší, nejlepší a průměrný případ
- [x] Bod 3: O-notace, horní a dolní odhad (Big O, Big Omega, Big Theta)
- [x] Bod 4: Způsob určení časové a prostorové složitosti
- [x] Bod 5: Vzhledem k čemu časovou složitost určujeme
- [x] Bod 6: Příklad O(1) – konstantní čas
- [x] Bod 7: Příklad O(n) – lineární čas
- [x] Bod 8: Příklad O(n²) – kvadratický čas
- [x] Bod 9: Příklad O(log n) – logaritmický čas
- [x] Bod 10: Vylepšení exponenciální složitosti Fibonacciho
- [x] Amortizovaná složitost
- [x] Rekurzivní rovnice složitosti, Master Theorem (přehled)
- [x] Třídy P, NP a NP-úplnost (úvod)

---

## Klíčové koncepty

### 1. Co je složitost a proč ji měříme

**Časová složitost** popisuje, **jak rychle roste počet elementárních operací** algoritmu s rostoucí velikostí vstupu `n`. **Paměťová (prostorová) složitost** popisuje, **kolik extra paměti** (nad rámec vstupu) algoritmus potřebuje.

| Typ | Co měří | Jednotka |
|-----|---------|----------|
| **Časová složitost** | Jak roste počet operací s velikostí vstupu | Počet kroků (asymptoticky) |
| **Paměťová složitost** | Kolik extra paměti algoritmus potřebuje | Bajty / prvky / hloubka stacku |

**Proč to měříme abstraktně, ne v sekundách:**
- Sekundy závisí na **konkrétním hardwaru**, jazyku, kompilátoru, zatížení systému – nejsou přenositelné.
- Počet kroků je **univerzální** – stejný algoritmus má stejnou složitost na PC, mobilu i serveru.
- Umožňuje **predikci pro velká data**: pokud algoritmus zvládne 1 000 prvků za sekundu a je O(n²), pak 10 000 prvků za ~100 sekund.
- Umožňuje **porovnání algoritmů** ještě před implementací.

**Klíčové slovo: asymptotická složitost.** Zajímá nás chování pro **velká n**, ne přesné konstanty. Algoritmus s časem `5n + 100` a algoritmus s časem `n + 1000` budou pro velká n téměř totožné – oba jsou **lineární**.

**Model výpočtu (RAM model):**
Klasická analýza předpokládá tzv. **RAM (Random Access Machine)** model – každá základní operace (přiřazení, aritmetika, porovnání, přístup k poli přes index) trvá **jednotkový čas O(1)**. To je idealizace – ve skutečnosti se přístup do cache liší od přístupu do RAM o ~100× a do SSD o ~10 000×. Pro teoretickou analýzu to ale ignorujeme.

---

### 2. Nejhorší, nejlepší a průměrný případ

Algoritmus se může chovat **různě** podle konkrétního vstupu, i když je velikost stejná. Proto rozlišujeme:

| Případ | Notace | Co znamená |
|--------|--------|------------|
| **Nejhorší (worst-case)** | typicky **O(f(n))** | "Nikdy to nebude trvat víc než f(n)." |
| **Nejlepší (best-case)** | typicky **Ω(f(n))** | "Nikdy to nebude trvat méně než f(n)." |
| **Průměrný (average-case)** | často **Θ(f(n))** | Typická hodnota přes všechny vstupy. |

**Příklad – lineární vyhledávání v `[5, 8, 3, 9, 1]`:**
- **Nejlepší případ:** hledáme `5` → najdeme hned, 1 krok ⇒ Ω(1).
- **Průměrný případ:** hledaný prvek je někde uprostřed → ~n/2 kroků ⇒ Θ(n).
- **Nejhorší případ:** hledáme `1` nebo prvek, který tam není → n kroků ⇒ O(n).

**V praxi a na maturitě téměř vždy uvádíme nejhorší případ (Big O).** Důvod: poskytuje horní záruku ("rychlejší než toto to vždy bude"). U Quick Sortu se ale tradičně uvádí **průměrný případ** O(n log n), protože nejhorší případ O(n²) je vzácný a nereálný při randomizaci pivota.

**Amortizovaná složitost** – průměrná složitost přes **sekvenci operací**, ne přes vstupy. Liší se od průměrného případu! Více v sekci níže.

---

### 3. O-notace, dolní a těsný odhad

O-notace je formální matematický aparát pro asymptotické chování funkcí. Tři klíčové notace:

**Big O (horní odhad):**
`f(n) = O(g(n))` znamená, že existují konstanty `c > 0` a `n₀` takové, že pro všechna `n ≥ n₀` platí `f(n) ≤ c · g(n)`.

Slovně: "f roste **nanejvýš tak rychle** jako g, až na konstantu."

**Big Omega (dolní odhad):**
`f(n) = Ω(g(n))` znamená, že existují konstanty `c > 0` a `n₀` takové, že pro všechna `n ≥ n₀` platí `f(n) ≥ c · g(n)`.

Slovně: "f roste **alespoň tak rychle** jako g, až na konstantu."

**Big Theta (těsný odhad):**
`f(n) = Θ(g(n))` znamená současně `f = O(g)` **a** `f = Ω(g)`.

Slovně: "f a g rostou **přesně stejně**, až na konstantu."

**Vztah:**
```
Θ(g)  =  O(g) ∩ Ω(g)
```

**Pravidla zjednodušování:**

| Původní výraz | Zjednodušené | Pravidlo |
|---------------|--------------|----------|
| `5n + 3` | O(n) | Konstanty před proměnnou vypouštíme |
| `n² + n + 100` | O(n²) | Necháváme jen nejvyšší řád |
| `3n² + 2n + 1` | O(n²) | Kombinace obou |
| `log₂ n` | O(log n) | Základ logaritmu vypouštíme |
| `n + log n` | O(n) | n dominuje log n |
| `2ⁿ + n¹⁰⁰` | O(2ⁿ) | Exponenciála dominuje polynom |

**Proč nezáleží na základu logaritmu?** Změna základu je jen násobení konstantou: `logₐ n = log_b n / log_b a`. Protože konstanty vypouštíme, je jedno, jestli píšeme `log₂ n` nebo `log₁₀ n` – obojí je O(log n).

**Pořadí růstu funkcí (od nejpomalejšího k nejrychlejšímu):**
```
1  <<  log n  <<  √n  <<  n  <<  n log n  <<  n²  <<  n³  <<  2ⁿ  <<  n!  <<  nⁿ
```

(Symbol `<<` čteme "roste pomaleji než".)

---

### 4. Jak určit složitost

#### Časová složitost – postup

1. **Najdi cykly** – hlavní zdroj kroků.
2. **Spočítej vnoření** – cyklus uvnitř cyklu znamená násobení iterací.
3. **Sečti nezávislé části, vezmi maximum** – dva cykly za sebou se sčítají, ale dominuje větší.
4. **Vyhodnoť tělo cyklu** – pokud uvnitř není jen O(1), složitost se násobí.
5. **U rekurze** – sestav rekurzivní rovnici (viz Master Theorem níže).

```csharp
// Jeden cyklus = O(n)
for (int i = 0; i < n; i++) { ... }

// Dva vnořené cykly = O(n²)
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++) { ... }

// Vnořené cykly, vnitřní jde do i = O(n²) (n(n+1)/2)
for (int i = 0; i < n; i++)
    for (int j = 0; j < i; j++) { ... }

// Dva cykly za sebou = O(n) + O(n) = O(n)
for (int i = 0; i < n; i++) { ... }
for (int j = 0; j < n; j++) { ... }

// Půlení = O(log n)
while (n > 0) { n = n / 2; }

// Externí n, vnitřní log n => O(n log n)
for (int i = 0; i < n; i++)
{
    int x = n;
    while (x > 0) { x = x / 2; }
}
```

#### Paměťová složitost – postup

Vyhodnoť **kolik extra paměti** kromě vstupu algoritmus alokuje (proměnné, pomocná pole, call stack u rekurze).

| Situace | Složitost |
|---------|-----------|
| Jen pár proměnných (`int i, j, temp`) | O(1) |
| Pomocné pole velikosti vstupu | O(n) |
| 2D matice `n × n` | O(n²) |
| Rekurze hloubky n (call stack) | O(n) |
| Rekurze hloubky log n (binární vyhledávání) | O(log n) |
| HashSet/Dictionary nad všemi prvky | O(n) |

**Rekurze zabírá místo na zásobníku volání.** Každé volání = nový stack frame s lokálními proměnnými a return address.

```csharp
int Faktorial(int n)   // Paměť: O(n) kvůli call stacku
{
    if (n <= 1) return 1;
    return n * Faktorial(n - 1);
}
```

**In-place algoritmus** – algoritmus s O(1) extra pamětí (nepočítáme vstup). Příkladem je QuickSort nebo HeapSort. MergeSort není in-place, protože potřebuje O(n) pomocné pole.

---

#### Rekurzivní rovnice složitosti

Pro rekurzivní algoritmus zapíšeme **rekurentní rovnici** popisující práci na vstupu velikosti n přes práci na menších problémech:

```
T(n) = a · T(n/b) + f(n)
```
kde:
- `a` = počet rekurzivních volání,
- `n/b` = velikost podproblému,
- `f(n)` = práce mimo rekurzi (rozdělení + slévání).

**Příklady:**
- **MergeSort:** `T(n) = 2T(n/2) + O(n)` ⇒ O(n log n)
- **Binární vyhledávání:** `T(n) = T(n/2) + O(1)` ⇒ O(log n)
- **Naivní Fibonacci:** `T(n) = T(n-1) + T(n-2) + O(1)` ⇒ O(2ⁿ)

**Master Theorem (zjednodušený, přehled):**

Pro `T(n) = a · T(n/b) + O(n^d)`:
- Pokud `d > logₐ b` ⇒ T(n) = O(n^d)
- Pokud `d = logₐ b` ⇒ T(n) = O(n^d · log n)
- Pokud `d < logₐ b` ⇒ T(n) = O(n^(logₐ b))

(Pro maturitu nemusíš znát detailně, ale dobré vědět, že **existuje formule pro odhad** divide-and-conquer algoritmů.)

---

### 5. Vzhledem k čemu určujeme složitost

Vždy je třeba **explicitně říct, co je n**. Stejný algoritmus může mít různé složitosti podle toho, co považujeme za "velikost vstupu".

| Kontext | Co je obvykle "n" |
|---------|-------------------|
| Pole / List | Počet prvků |
| Řetězec (string) | Délka stringu |
| Matice | Rozměr (n×n) nebo počet buněk n² |
| Graf | Počet vrcholů V a hran E |
| Číslo | Hodnota (numeric) nebo počet cifer (bit-length) |
| Strom | Počet uzlů nebo hloubka |

**Pozor – paradox primality:** Test prvočíselnosti zkušebním dělením má složitost O(√n), kde n je **hodnota** čísla. Ale pokud n vyjádříme přes počet jeho cifer/bitů `k = log n`, pak √n = 2^(k/2) – exponenciální vůči délce vstupu! Proto se tomu říká **pseudopolynomiální algoritmus**.

**U grafů máme dva parametry V (vertices) a E (edges):**
```
BFS / DFS:        O(V + E)
Dijkstra (s haldou): O((V + E) log V)
Floyd-Warshall:    O(V³)
```

**Vždy explicitně formuluj:**
> "Složitost je **O(n)**, kde **n je počet prvků v poli**."
> nebo
> "Složitost je **O(V + E)**, kde V je počet vrcholů a E hran."

---

## Příklady složitostí s kódem

### O(1) – Konstantní čas

Operace, jejichž doba **nezávisí na velikosti vstupu**.

```csharp
// Přístup k prvku pole (array indexing)
int prvek = pole[5];

// Stack/Queue operace
stack.Push(x);
int y = stack.Pop();

// Dictionary lookup (průměrně, díky hashování)
slovnik["klic"] = 100;

// Aritmetické operace na pevné velikosti čísel
int x = a + b * c;

// Swap dvou prvků
void Swap(int[] pole, int i, int j)
{
    int temp = pole[i];
    pole[i] = pole[j];
    pole[j] = temp;
}

// Délka pole - uložená v metadatech
int delka = pole.Length;
```

**Pozor – `Dictionary` je O(1) jen průměrně.** V nejhorším případě (špatná hashovací funkce, kolize) může degradovat až na O(n). V .NET je hashing dostatečně robustní, takže v praxi O(1) předpokládáme.

---

### O(n) – Lineární čas

Algoritmus se musí podívat na každý prvek (nebo většinu) **konstantněkrát**.

```csharp
// Hledání prvku v neseřazeném poli
int NajdiPrvek(int[] pole, int hledany)
{
    for (int i = 0; i < pole.Length; i++)
        if (pole[i] == hledany)
            return i;
    return -1;
}

// Součet prvků
int Soucet(int[] pole)
{
    int suma = 0;
    for (int i = 0; i < pole.Length; i++)
        suma += pole[i];
    return suma;
}

// Hledání maxima (nelze rychleji! - musíš vidět každý prvek)
int Maximum(int[] pole)
{
    int max = pole[0];
    for (int i = 1; i < pole.Length; i++)
        if (pole[i] > max)
            max = pole[i];
    return max;
}

// Kopírování pole
int[] Kopie(int[] pole)
{
    int[] novy = new int[pole.Length];
    for (int i = 0; i < pole.Length; i++)
        novy[i] = pole[i];
    return novy;
}
```

---

### O(n²) – Kvadratický čas

Typicky **dva vnořené cykly** přes n prvků.

```csharp
// Bubble Sort - klasický kvadratický třídicí algoritmus
void BubbleSort(int[] pole)
{
    for (int i = 0; i < pole.Length - 1; i++)
    {
        for (int j = 0; j < pole.Length - 1 - i; j++)
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

// Všechny páry prvků
void VsechnyPary(int[] pole)
{
    for (int i = 0; i < pole.Length; i++)
        for (int j = 0; j < pole.Length; j++)
            Console.WriteLine($"{pole[i]}, {pole[j]}");
}

// Naivní kontrola duplicit - O(n²)
bool MaDuplicity(int[] pole)
{
    for (int i = 0; i < pole.Length; i++)
        for (int j = i + 1; j < pole.Length; j++)
            if (pole[i] == pole[j])
                return true;
    return false;
}

// Lepší řešení duplicit přes HashSet - O(n)
bool MaDuplicityRychle(int[] pole)
{
    HashSet<int> videne = new HashSet<int>();
    foreach (int x in pole)
        if (!videne.Add(x))
            return true;
    return false;
}
```

**Trojúhelníkový součet:** Když vnitřní cyklus probíhá od `i` (`for (j = i; j < n; j++)`), celkový počet iterací je `n + (n-1) + ... + 1 = n(n+1)/2 ≈ n²/2`. **Pořád O(n²)**, jen s nižší konstantou.

---

### O(log n) – Logaritmický čas

Vzniká tam, kde algoritmus **v každém kroku zmenší problém o konstantní zlomek** (typicky polovinu).

```csharp
// Binární vyhledávání v SEŘAZENÉM poli
int BinarniVyhledavani(int[] serazenePole, int hledany)
{
    int levy = 0;
    int pravy = serazenePole.Length - 1;

    while (levy <= pravy)
    {
        int stred = (levy + pravy) / 2;

        if (serazenePole[stred] == hledany)
            return stred;
        else if (serazenePole[stred] < hledany)
            levy = stred + 1;
        else
            pravy = stred - 1;
    }
    return -1;
}

// Půlení čísla - kolikrát musíme dělit dvěma?
int PocetPuleni(int n)
{
    int kroky = 0;
    while (n > 1)
    {
        n = n / 2;
        kroky++;
    }
    return kroky;
}
```

**Proč log n?** Pokud v každém kroku problém zmenšíme na polovinu, po k krocích je velikost `n / 2^k`. Když `n / 2^k = 1`, pak `k = log₂ n`. Pro milion prvků je log₂(10⁶) ≈ 20. Pro miliardu cca 30. Proto jsou stromy a binární vyhledávání tak rychlé.

**Klíčový předpoklad: pole musí být seřazené!** Bez toho binární vyhledávání nefunguje.

---

### O(n log n) – Lineárně-logaritmický čas

Optimální čas pro **porovnávací třídicí algoritmy** (lower bound). Vzniká například při divide-and-conquer (rozděl a panuj) – n úrovní rekurze, na každé úrovni O(n) práce.

```csharp
// Merge Sort - O(n log n)
void MergeSort(int[] pole, int levy, int pravy)
{
    if (levy < pravy)
    {
        int stred = (levy + pravy) / 2;
        MergeSort(pole, levy, stred);
        MergeSort(pole, stred + 1, pravy);
        Merge(pole, levy, stred, pravy);   // O(n) merge
    }
}
```

Další algoritmy s touto složitostí: **QuickSort (průměrně)**, **HeapSort**, **TimSort** (default v .NET, Python, Java).

---

### Fibonacciho čísla – studie optimalizace

#### Naivní rekurze – O(2ⁿ) čas, O(n) paměť
```csharp
int FibRekurze(int n)
{
    if (n <= 1) return n;
    return FibRekurze(n - 1) + FibRekurze(n - 2);
}
```
**Problém: překrývající se podproblémy.** F(5) volá F(4) a F(3); F(4) volá F(3) a F(2). F(3) se počítá dvakrát, F(2) třikrát, F(1) pětkrát. Strom volání má `~φⁿ ≈ 1.618ⁿ` uzlů, což je exponenciální.

Pro n=50 je to ~12 miliard volání – několik minut běhu. Pro n=100 už déle než věk vesmíru.

#### Memoizace (top-down DP) – O(n) čas, O(n) paměť
```csharp
int FibMemo(int n, int[] cache)
{
    if (n <= 1) return n;
    if (cache[n] != 0) return cache[n];

    cache[n] = FibMemo(n - 1, cache) + FibMemo(n - 2, cache);
    return cache[n];
}
```
**Princip:** Pokud už víme F(k), nepočítáme ho znovu – uložíme do cache. Z exponenciálního stromu se stane lineární průchod.

#### Iterativní s polem (bottom-up DP) – O(n) čas, O(n) paměť
```csharp
int FibPole(int n)
{
    if (n <= 1) return n;

    int[] fib = new int[n + 1];
    fib[0] = 0;
    fib[1] = 1;

    for (int i = 2; i <= n; i++)
        fib[i] = fib[i - 1] + fib[i - 2];

    return fib[n];
}
```

#### Optimální iterativní – O(n) čas, O(1) paměť
```csharp
int FibOptimal(int n)
{
    if (n <= 1) return n;

    int predminuly = 0;
    int minuly = 1;

    for (int i = 2; i <= n; i++)
    {
        int soucasny = predminuly + minuly;
        predminuly = minuly;
        minuly = soucasny;
    }

    return minuly;
}
```
**Klíč:** Potřebujeme jen **dvě poslední** hodnoty, ne celé pole. Z O(n) paměti se stane O(1).

**Průběh pro n=5:**
| i | predminuly | minuly | soucasny | F(i) |
|---|------------|--------|----------|------|
| 2 | 0 | 1 | 1 | F(2) = 1 |
| 3 | 1 | 1 | 2 | F(3) = 2 |
| 4 | 1 | 2 | 3 | F(4) = 3 |
| 5 | 2 | 3 | 5 | F(5) = 5 |

#### Matrix exponentiation – O(log n) čas
Existuje i řešení v O(log n) přes umocňování matice `[[1,1],[1,0]]^n`. Pro maturitu zbytečné, ale dobré vědět, že limit je dál.

**Shrnutí Fibonacciho:**
| Verze | Čas | Paměť |
|-------|-----|-------|
| Naivní rekurze | O(2ⁿ) | O(n) |
| Memoizace | O(n) | O(n) |
| Iterativní s polem | O(n) | O(n) |
| Iterativní 2 proměnné | O(n) | O(1) |
| Matrix exp. | O(log n) | O(1) |

---

## Amortizovaná složitost

**Amortizovaná složitost** = průměrná složitost operace přes **dlouhou sekvenci operací**, ne přes různé vstupy. Je užitečná, když některé operace jsou občas drahé, ale **celkově se to vyrovná**.

**Klasický příklad: `List<T>.Add` (dynamické pole).**
- `List<T>` má interní pole pevné kapacity.
- Když je plné a chceme přidat, vytvoří se **nové pole 2× větší** a starý obsah se zkopíruje (O(n)).
- Občasná operace je tedy O(n), ale **mezi nimi** je n levných O(1) operací.
- **Amortizovaně O(1) na operaci.**

Důkaz (přes účtovací metodu): Při každém Add zaplatíme 3 "kredity" – 1 za vlastní zápis, 2 odložíme. Když nastane realokace n prvků, máme naspořeno 2n kreditů, které stačí na kopii. Celková práce přes m operací = O(m).

**Pozor – amortizovaná ≠ průměrná:** Průměrná je přes různé vstupy, amortizovaná je přes sekvenci na **jednom** vstupu. `List.Add` má amortizovaně O(1) i v nejhorším případě.

Další amortizované algoritmy:
- **HashSet/Dictionary** – Add/Lookup průměrně O(1), amortizovaně O(1).
- **Union-Find** s path compression – amortizovaně O(α(n)) (inverzní Ackermannova funkce, prakticky konstantní).

---

## Třídy P, NP, NP-úplnost (úvod)

**P (polynomial time):** problémy, které lze vyřešit v polynomiálním čase O(n^k) pro nějakou konstantu k. "Prakticky řešitelné". Třídění, vyhledávání, nejkratší cesta – vše P.

**NP (nondeterministic polynomial):** problémy, kde **lze řešení rychle ověřit** v polynomiálním čase, ale možná těžké najít. Příklad: Sudoku – ověření vyplněné mřížky je triviální, ale vyplnění může být složité.

**NP-úplné (NP-complete):** "nejtěžší" problémy v NP. Pokud najdeš polynomiální algoritmus pro **jeden NP-úplný problém**, vyřešils všechny (zhroutíš P = NP).

Klasické NP-úplné: SAT, problém obchodního cestujícího (TSP), batoh (knapsack), 3-barvení grafu.

**Otevřená otázka P vs NP** – stále se neví, zda P = NP. Jeden z **7 Milénia problémů** (1 milion dolarů odměna).

Pro maturitu: stačí vědět, že **některé problémy nemáme jak řešit rychle** a používáme aproximační/heuristické algoritmy (genetické, simulated annealing).

---

## Přehledová tabulka složitostí

### Porovnání růstu pro různá n:

| n | O(1) | O(log n) | O(√n) | O(n) | O(n log n) | O(n²) | O(2ⁿ) | O(n!) |
|---|------|----------|-------|------|------------|-------|-------|-------|
| 10 | 1 | 3 | 3 | 10 | 33 | 100 | 1 024 | 3.6×10⁶ |
| 100 | 1 | 7 | 10 | 100 | 664 | 10 000 | 10³⁰ | 10¹⁵⁸ |
| 1 000 | 1 | 10 | 32 | 1 000 | 10 000 | 1 000 000 | ∞ | ∞ |
| 1 000 000 | 1 | 20 | 1 000 | 10⁶ | 2×10⁷ | 10¹² | ∞ | ∞ |

### Typické složitosti algoritmů:

| Algoritmus | Časová | Paměťová |
|------------|--------|----------|
| Přístup k poli | O(1) | O(1) |
| Lineární vyhledávání | O(n) | O(1) |
| Binární vyhledávání | O(log n) | O(1) iter. / O(log n) rek. |
| Bubble / Selection / Insertion Sort | O(n²) | O(1) |
| Merge Sort | O(n log n) | O(n) |
| Quick Sort (průměr) | O(n log n) | O(log n) |
| Quick Sort (nejhorší) | O(n²) | O(n) |
| Heap Sort | O(n log n) | O(1) |
| Counting / Radix Sort | O(n + k) | O(n + k) |
| DFS / BFS na grafu | O(V + E) | O(V) |
| Dijkstra (s binární haldou) | O((V+E) log V) | O(V) |
| Floyd-Warshall (all pairs) | O(V³) | O(V²) |
| Naivní Fibonacci | O(2ⁿ) | O(n) |
| Memoizovaný Fibonacci | O(n) | O(n) |

---

## Maturitní chytáky

### Časová složitost:

> "Je O(2n) totéž co O(n)?"
> **ANO** – konstantní násobitele zanedbáváme. Big O říká **třídu růstu**, ne přesný počet kroků.

> "Jaká je složitost lineárního vyhledávání?"
> **O(n)** v nejhorším případě, Ω(1) v nejlepším.

> "Lze najít maximum rychleji než O(n)?"
> **NE** – musíš se podívat na každý prvek aspoň jednou, jinak bys mohl maximum minout.

> "Je binární vyhledávání vždy O(log n)?"
> **ANO, ale jen na seřazeném poli.** Bez řazení nelze.

> "Tohle je O(n²)?"
> ```csharp
> for (int i = 0; i < n; i++)
>     for (int j = 0; j < 10; j++) { ... }
> ```
> **NE!** Vnitřní cyklus má **konstantní** počet iterací → O(10·n) = O(n).

> "Tohle je O(n²)?"
> ```csharp
> for (int i = 0; i < n; i++)
>     for (int j = i; j < n; j++) { ... }
> ```
> **ANO!** Celkem `n + (n-1) + ... + 1 = n(n+1)/2 = O(n²)`.

> "Tohle je O(n²) nebo O(n)?"
> ```csharp
> for (int i = 1; i < n; i *= 2)
>     for (int j = 0; j < n; j++) { ... }
> ```
> **O(n log n)** – vnější cyklus půlí (log n iterací), vnitřní lineární.

### Paměťová složitost:

> "Jaká je prostorová složitost MergeSortu?"
> **O(n)** – potřebuje pomocné pole pro slévání.

> "Jaká je prostorová složitost rekurzivního faktoriálu?"
> **O(n)** – hloubka call stacku.

> "Co znamená in-place algoritmus?"
> Algoritmus s O(1) extra pamětí – pracuje "na místě" ve vstupním poli.

### Fibonacci:

> "Proč je naivní rekurzivní Fibonacci tak pomalý?"
> Kvůli **překrývajícím se podproblémům** – F(3), F(2), F(1) se počítají mnohokrát. Strom volání má `Θ(φⁿ)` uzlů.

> "Jak ho zrychlit?"
> **Memoizací** (top-down) nebo **iterací** (bottom-up). Stačí si pamatovat poslední 2 hodnoty → O(n) čas, O(1) paměť.

### Obecné triky:

> "Trade-off čas vs. paměť"
> Často lze vyměnit paměť za rychlost (memoizace, lookup tabulky, prepočet). Naopak také – pokud nemáme paměť, někdy musíme přepočítávat.

> "HashSet pro detekci duplicit"
> Místo O(n²) naivního dvojcyklu použijeme HashSet pro O(n) průchod (viz kód výše).

> "Logaritmus v praxi"
> O(log n) pro miliardu prvků = ~30 kroků. Proto jsou vyvážené stromy (AVL, Red-Black) a binární vyhledávání tak užitečné.

> "Kdy na složitosti záleží"
> Pro 100 prvků je jedno, jestli O(n) nebo O(n²). Pro milion prvků je to **rozdíl mezi sekundou a dnem**. Pro miliardu rozdíl mezi minutou a desetiletími.

> "Měření v praxi"
> `System.Diagnostics.Stopwatch` pro hrubá měření, `BenchmarkDotNet` pro mikrobenchmarky (zohledňuje JIT warmup, GC, statistickou významnost).

---

## Quick Reference pro maturitu

```
O(1)       → Konstantní     → přístup k poli, stack/queue ops, hash lookup
O(log n)   → Logaritmický   → binární vyhledávání, vyvážené stromy
O(√n)      → Odmocninový    → naivní test prvočísla
O(n)       → Lineární       → průchod polem, hledání maxima
O(n log n) → Linearitmický  → efektivní třídění (Merge, Quick, Heap)
O(n²)      → Kvadratický    → vnořené cykly, Bubble/Selection sort
O(n³)      → Kubický        → Floyd-Warshall, naivní násobení matic
O(2ⁿ)      → Exponenciální  → všechny podmnožiny, naivní Fibonacci
O(n!)      → Faktoriální    → všechny permutace, naivní TSP
```

**U maturitní odpovědi vždy uveď:**
1. **Jakou složitost** algoritmus má (čas i paměť).
2. **Vzhledem k čemu** – co je n (počet prvků, znaků, hran...).
3. **Jestli je to** nejhorší / průměrný / nejlepší případ.
4. **Proč** – krátká úvaha (vnořené cykly, půlení, rekurze...).

---

## Klíčové pojmy k zapamatování

- **Časová složitost** – počet elementárních operací jako funkce velikosti vstupu.
- **Paměťová složitost** – extra paměť mimo vstup jako funkce velikosti vstupu.
- **Asymptotická složitost** – chování pro `n → ∞`, ignoruje konstanty.
- **RAM model** – idealizace, kde každá elementární operace trvá O(1).
- **Big O (`O(g)`)** – horní odhad, "nikdy ne víc než g, až na konstantu".
- **Big Omega (`Ω(g)`)** – dolní odhad, "nikdy ne méně než g, až na konstantu".
- **Big Theta (`Θ(g)`)** – těsný odhad, `O(g) ∩ Ω(g)`.
- **Worst / Average / Best case** – nejhorší / průměrný / nejlepší vstup dané velikosti.
- **Amortizovaná složitost** – průměr přes sekvenci operací, ne přes vstupy.
- **In-place algoritmus** – O(1) extra paměti.
- **Stabilní algoritmus** – zachovává relativní pořadí stejných prvků (relevantní hlavně pro třídění).
- **Dominantní člen** – nejrychleji rostoucí část součtu; jen ten v Big O zůstává.
- **Logaritmus** – exponent, na který musíme základ umocnit; `log₂ n` ≈ "kolikrát půlíme".
- **Rekurzivní rovnice** – `T(n) = a · T(n/b) + f(n)`; popis složitosti rekurzivního algoritmu.
- **Master Theorem** – formule pro odhad rekurentních rovnic divide-and-conquer.
- **Memoizace** – ukládání mezivýsledků pro úsporu času (top-down DP).
- **Dynamické programování (DP)** – řešení překrývajících se podproblémů; top-down (memoizace) nebo bottom-up (tabulace).
- **Překrývající se podproblémy** – stejný podproblém vzniká vícekrát v rekurzi.
- **Trade-off čas/paměť** – obvykle můžeme jedno zaplatit druhým.
- **P** – třída problémů řešitelných polynomiálním algoritmem.
- **NP** – třída problémů, kde lze řešení **ověřit** polynomiálně.
- **NP-úplný** – nejtěžší problém v NP; řešení jednoho by vyřešilo všechny.
- **Pseudopolynomiální** – polynomiální vzhledem k hodnotě vstupu, exponenciální vzhledem k délce zápisu (např. test prvočíselnosti dělením).
