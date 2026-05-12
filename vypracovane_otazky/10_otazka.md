# Zápisky: Otázka č. 10 - INSERT SORT. SELECT SORT.

## Checklist bodů otázky

- [x] Bod 1: Motivace pro třídění dat
- [x] Bod 2: Klasifikace třídicích algoritmů (stabilní, in-place, adaptivní…)
- [x] Bod 3: Insert Sort – popis a algoritmus
- [x] Bod 4: Insert Sort – znázornění na příkladu
- [x] Bod 5: Insert Sort – časová a paměťová složitost
- [x] Bod 6: Select Sort – popis a algoritmus
- [x] Bod 7: Select Sort – znázornění na příkladu
- [x] Bod 8: Select Sort – časová a paměťová složitost
- [x] Bod 9: Porovnání Insert Sort vs. Select Sort
- [x] Bod 10: Praktické tipy a hybridní algoritmy

---

## Klíčové koncepty & Snippety

### 1. Motivace pro třídění dat

**Třídění (sorting)** = uspořádání kolekce prvků podle daného **uspořádání (order)** – typicky vzestupně podle nějakého klíče (numerického, lexikografického, vlastního porovnávače).

**Proč třídíme?**
- **Rychlejší vyhledávání** – v seřazeném poli funguje binární vyhledávání O(log n) místo lineárního O(n).
- **Detekce duplicit** – stejné prvky jsou vedle sebe; stačí lineární průchod.
- **Sjednocení a průniky množin** – `merge` na seřazených datech je O(n).
- **Statistiky a kvantily** – medián, percentily, kvartily.
- **Algoritmy nad seřazenými daty** – Kruskal (hrany), sweep-line algoritmy, scheduling.
- **Přehlednost pro uživatele** – abecední seznamy, ceníky.

**Klíč (key)** je hodnota, podle které třídíme. **Komparátor (comparator)** je funkce, která vrátí záporné/nulu/kladné podle vztahu dvou prvků. V .NET je to `IComparer<T>` nebo `Comparison<T>` delegate.

**Příklady z praxe:**
- Emaily podle data.
- Produkty v e-shopu podle ceny.
- Studenti podle průměru.
- Kontakty v telefonu podle abecedy.
- Logy podle časové značky.

---

### 2. Klasifikace třídicích algoritmů

Třídicí algoritmy se dají dělit podle několika **nezávislých** kritérií:

**Stabilita (stability):**
- **Stabilní algoritmus** zachovává **relativní pořadí** prvků se stejným klíčem.
  Příklad: máme zaměstnance `[(Alice, 30), (Bob, 25), (Cyril, 30)]` setříděné podle věku. Stabilní třídění zachová `(Alice, 30)` před `(Cyril, 30)`; nestabilní je může prohodit.
- **Nestabilní algoritmus** může pořadí stejných klíčů narušit.
- Stabilita je důležitá pro **vícestupňové třídění** (nejprve podle příjmení, pak podle věku – jen stabilní třídění zachová sekundární řazení).

**In-place vs. out-of-place:**
- **In-place** – pracuje v původním poli s O(1) extra pamětí. Insert Sort, Select Sort, Quick Sort.
- **Out-of-place** – potřebuje O(n) pomocné pole. Merge Sort.

**Adaptivnost (adaptivity):**
- **Adaptivní** – rychlejší na téměř setříděných datech. Insert Sort.
- **Neadaptivní** – běh nezávisí na vstupu. Select Sort.

**Porovnávací vs. neporovnávací (comparison-based):**
- **Porovnávací (comparison sort)** – používá `<`, `>`, `==`; **dolní mez složitosti je Ω(n log n)**.
- **Neporovnávací (non-comparison sort)** – využívá strukturu klíčů (čísla, znaky). Counting Sort, Radix Sort, Bucket Sort – mohou být O(n).

**Online vs. offline:**
- **Online** – umí třídit, jak data přicházejí (Insert Sort).
- **Offline** – vyžaduje celou kolekci najednou (Merge Sort).

**Lower bound pro porovnávací třídění:**
Lze dokázat, že libovolný algoritmus třídicí porovnáváním potřebuje aspoň `Ω(n log n)` porovnání v nejhorším případě. Důvod: existuje `n!` možných permutací a každé porovnání rozhoduje binární otázku, takže rozhodovací strom má hloubku `≥ log₂(n!) = Θ(n log n)`. Žádný porovnávací algoritmus tuto hranici neporazí.

**Insert Sort a Select Sort jsou kvadratické (O(n²))** – jsou pro malá nebo téměř seřazená pole, ne pro velká data.

---

### 3. Insert Sort – popis a algoritmus

**Insert Sort (řazení vkládáním)** funguje jako **skládání karet do ruky**:
1. Vezmeme novou kartu (z nesetříděné části).
2. Najdeme její správnou pozici mezi již setříděnými kartami.
3. Vložíme ji – ostatní karty posuneme.

**Pole se myšlenkově dělí na dvě části:**
- **Setříděná** (vlevo) – na začátku jen `pole[0]`.
- **Nesetříděná** (vpravo) – na začátku `pole[1..n-1]`.

V každém kroku přesuneme **první prvek z nesetříděné** části na správné místo v **setříděné** části. Po `n-1` krocích je celé pole setříděné.

**Algoritmus krok za krokem:**
```
1. Pro každý index i od 1 do n-1:
   a) key = pole[i]                      (prvek k zařazení)
   b) j = i - 1                          (poslední index setříděné části)
   c) Dokud j >= 0 a pole[j] > key:
      - pole[j+1] = pole[j]              (posun většího prvku doprava)
      - j--
   d) pole[j+1] = key                    (vlož na uvolněné místo)
```

**Kód:**
```csharp
static void InsertSort(int[] pole)
{
    for (int i = 1; i < pole.Length; i++)
    {
        int key = pole[i];
        int j = i - 1;

        while (j >= 0 && pole[j] > key)
        {
            pole[j + 1] = pole[j];
            j--;
        }

        pole[j + 1] = key;
    }
}
```

**Generická verze (libovolný porovnatelný typ):**
```csharp
static void InsertSort<T>(T[] pole) where T : IComparable<T>
{
    for (int i = 1; i < pole.Length; i++)
    {
        T key = pole[i];
        int j = i - 1;

        while (j >= 0 && pole[j].CompareTo(key) > 0)
        {
            pole[j + 1] = pole[j];
            j--;
        }
        pole[j + 1] = key;
    }
}

// Pro vlastní pořadí lze předat IComparer<T> nebo Comparison<T>:
static void InsertSort<T>(T[] pole, IComparer<T> cmp)
{
    for (int i = 1; i < pole.Length; i++)
    {
        T key = pole[i];
        int j = i - 1;
        while (j >= 0 && cmp.Compare(pole[j], key) > 0)
        {
            pole[j + 1] = pole[j];
            j--;
        }
        pole[j + 1] = key;
    }
}
```

**Klíčový detail – proč přesouváme, místo abychom prohazovali?**
Klasická "naivní" implementace by každý větší prvek prohodila s `key`. To znamená 2 zápisy (`temp = a; a = b; b = temp;`) místo 1 (`a = b;`). Náš algoritmus využívá toho, že `key` máme uloženo v pomocné proměnné, a stačí jeden zápis na posunutí. **~3× méně zápisů** než naivní swap-verze.

---

### 4. Insert Sort – znázornění na příkladu

**ASCII vizualizace pro `[5, 2, 4, 6, 1, 3]`:**

```
Počáteční stav: [5, 2, 4, 6, 1, 3]
                 ↑ setříděná část (1 prvek)
```

**Krok 1: zařazujeme `2`**
```
[5, 2, 4, 6, 1, 3]   key = 2, j = 0
Porovnání: 5 > 2 ANO → posun 5 doprava
[5, 5, 4, 6, 1, 3]   j = -1, konec
Vlož key na pozici 0:
[2, 5, 4, 6, 1, 3]   setříděná část: [2, 5]
```

**Krok 2: zařazujeme `4`**
```
[2, 5, 4, 6, 1, 3]   key = 4, j = 1
Porovnání: 5 > 4 ANO → posun
           2 > 4 NE  → stop
[2, 4, 5, 6, 1, 3]   setříděná část: [2, 4, 5]
```

**Krok 3: zařazujeme `6`**
```
[2, 4, 5, 6, 1, 3]   key = 6, j = 2
Porovnání: 5 > 6 NE  → 6 už je na správném místě (nejlepší případ, 1 porovnání)
[2, 4, 5, 6, 1, 3]   setříděná část: [2, 4, 5, 6]
```

**Krok 4: zařazujeme `1`**
```
[2, 4, 5, 6, 1, 3]   key = 1, j = 3
Porovnání: 6 > 1 ANO → posun
           5 > 1 ANO → posun
           4 > 1 ANO → posun
           2 > 1 ANO → posun
           j = -1, konec
[1, 2, 4, 5, 6, 3]   (nejhorší případ, 4 porovnání + 4 posuny)
```

**Krok 5: zařazujeme `3`**
```
[1, 2, 4, 5, 6, 3]   key = 3, j = 4
Porovnání: 6 > 3 ANO → posun
           5 > 3 ANO → posun
           4 > 3 ANO → posun
           2 > 3 NE  → stop
[1, 2, 3, 4, 5, 6]   HOTOVO
```

---

### 5. Insert Sort – časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Kdy nastává |
|--------|-----------|-------------|
| **Nejlepší** | **O(n)** | Pole je již seřazené – vnitřní `while` se v každé iteraci nespustí. |
| **Průměrný** | **O(n²)** | Náhodné pořadí prvků – očekávaný počet posunů na prvek `~i/2`. |
| **Nejhorší** | **O(n²)** | Pole je seřazené opačně – každý prvek musíme posunout až úplně doleva. |

**Proč O(n²)?**
- Vnější cyklus: `n − 1` iterací.
- Vnitřní cyklus: v nejhorším případě posuneme `i` prvků pro `i`-tý vnější krok.
- Celkem porovnání: `1 + 2 + ... + (n−1) = n(n−1)/2 ≈ n²/2 = O(n²)`.

**Paměťová složitost:** **O(1)** – pouze pomocná proměnná `key` a index `j`. In-place.

**Vlastnosti Insert Sortu:**
- **Stabilní** – zachovává pořadí stejných klíčů (díky `pole[j] > key`, ne `>=`).
- **In-place** – O(1) extra paměti.
- **Adaptivní** – pro téměř seřazená data běží téměř O(n).
- **Online** – umí přijímat data postupně a udržovat setříděnou strukturu.
- Pomalý na velkých datech – kvadratický.

**Kdy Insert Sort použít:**
- Malá pole (do ~50 prvků).
- Téměř seřazená data (např. po malé změně v seřazeném seznamu).
- Když potřebujeme stabilitu.
- Jako součást **hybridních algoritmů** (TimSort, IntroSort).

**Online třídění – ukázka:**
```csharp
List<int> seznam = new List<int>();
foreach (int novy in vstup)
{
    int i = seznam.Count - 1;
    seznam.Add(novy);
    while (i >= 0 && seznam[i] > novy)
    {
        seznam[i + 1] = seznam[i];
        i--;
    }
    seznam[i + 1] = novy;
}
// po každém vložení je seznam stále seřazený
```

---

### 6. Select Sort – popis a algoritmus

**Select Sort (řazení výběrem)** funguje opačně než Insert Sort – místo aby vkládal každý prvek na správné místo, postupně **vybírá** minimum a dává ho na konec setříděné části.

**Princip:**
1. V nesetříděné části najdi **index minima**.
2. Prohoď minimum s **prvním prvkem nesetříděné části**.
3. Posuň hranici setříděné části doprava.
4. Opakuj, dokud zbývá víc než 1 prvek.

**Algoritmus krok za krokem:**
```
1. Pro každou pozici i od 0 do n-2:
   a) minIndex = i
   b) Pro každé j od i+1 do n-1:
      Pokud pole[j] < pole[minIndex]:
        minIndex = j
   c) Pokud minIndex != i:
      Prohoď pole[i] a pole[minIndex]
```

**Kód:**
```csharp
static void SelectSort(int[] pole)
{
    for (int i = 0; i < pole.Length - 1; i++)
    {
        int minIndex = i;
        for (int j = i + 1; j < pole.Length; j++)
        {
            if (pole[j] < pole[minIndex])
                minIndex = j;
        }

        if (minIndex != i)
        {
            int temp = pole[i];
            pole[i] = pole[minIndex];
            pole[minIndex] = temp;
        }
    }
}
```

**Generická verze s tuple-swap (C# 7+):**
```csharp
static void SelectSort<T>(T[] pole) where T : IComparable<T>
{
    for (int i = 0; i < pole.Length - 1; i++)
    {
        int minIndex = i;
        for (int j = i + 1; j < pole.Length; j++)
        {
            if (pole[j].CompareTo(pole[minIndex]) < 0)
                minIndex = j;
        }
        if (minIndex != i)
            (pole[i], pole[minIndex]) = (pole[minIndex], pole[i]);
    }
}
```

**Klíčová charakteristika Select Sortu:**
- **Vždy** dělá max `n − 1` prohození. To je výhoda, pokud je `swap` drahý (např. velké objekty, externí I/O).
- **Vždy** dělá `Θ(n²)` porovnání – nepřizpůsobí se vstupu.

---

### 7. Select Sort – znázornění na příkladu

**ASCII vizualizace pro `[64, 25, 12, 22, 11]`:**

**Krok 1: hledáme minimum v celém poli**
```
[64, 25, 12, 22, 11]
  i=0 ............ min=11 (index 4)
Prohod pole[0] ↔ pole[4]:
[11, 25, 12, 22, 64]
```

**Krok 2: hledáme minimum v `[25, 12, 22, 64]`**
```
[11, 25, 12, 22, 64]
     i=1 ........ min=12 (index 2)
Prohod pole[1] ↔ pole[2]:
[11, 12, 25, 22, 64]
```

**Krok 3: hledáme minimum v `[25, 22, 64]`**
```
[11, 12, 25, 22, 64]
         i=2 ... min=22 (index 3)
Prohod pole[2] ↔ pole[3]:
[11, 12, 22, 25, 64]
```

**Krok 4: hledáme minimum v `[25, 64]`**
```
[11, 12, 22, 25, 64]
             i=3, min=25 (index 3)
Žádný swap (minimum už je na správné pozici).
HOTOVO: [11, 12, 22, 25, 64]
```

**Shrnutí průběhu:**
```
Krok 1: [64, 25, 12, 22, 11] → swap(0,4) → [11, 25, 12, 22, 64]
Krok 2: [11, 25, 12, 22, 64] → swap(1,2) → [11, 12, 25, 22, 64]
Krok 3: [11, 12, 25, 22, 64] → swap(2,3) → [11, 12, 22, 25, 64]
Krok 4: [11, 12, 22, 25, 64] → no swap   → [11, 12, 22, 25, 64]
```

---

### 8. Select Sort – časová a paměťová složitost

**Časová složitost:**

| Případ | Složitost | Vysvětlení |
|--------|-----------|------------|
| **Nejlepší** | **O(n²)** | I seřazené pole musí algoritmus projít celé, aby ověřil minimum. |
| **Průměrný** | **O(n²)** | Stejné jako nejlepší. |
| **Nejhorší** | **O(n²)** | Stejné jako nejlepší. |

**Proč VŽDY O(n²)?**
- Vnější cyklus: `n − 1` iterací.
- Vnitřní cyklus: `(n−1) + (n−2) + ... + 1 = n(n−1)/2 ≈ n²/2` porovnání.
- **Bez ohledu na vstup.**

**Počet swapů: max `n − 1`** – pro každou pozici nejvýše jedna výměna. To je **lineárně** v počtu prvků, což je hlavní praktická výhoda.

**Paměťová složitost:** **O(1)** – pouze indexy a `temp` pro swap. In-place.

**Vlastnosti Select Sortu:**
- **Nestabilní** – swap může přeskočit prvek se stejnou hodnotou.
- **In-place** – O(1) extra paměti.
- **Neadaptivní** – běh nezávisí na vstupu.
- **Minimální swapy** – max `n − 1` výměn (výhoda při drahých přesunech).

**Příklad nestability:**
```
Pole: [(3, A), (5, B), (3, C)]
Krok 1: min=3 na pozici 0 → swap(0, 0) → žádná změna
Krok 2: min=3 na pozici 2 → swap(1, 2) → [(3, A), (3, C), (5, B)]
Výsledek: pořadí (3, A), (3, C) – ale ve vstupu bylo (3, A), (3, C) v pořadí A→C s prvkem (5,B) mezi nimi.
```
Tedy stejné klíče `3, 3` mohou v Select Sortu prohodit pořadí svých původních záznamů (závisí na situaci).

**Kdy Select Sort použít:**
- Když jsou výměny prvků drahé (velké objekty, externí I/O).
- Pro jednoduchost implementace.
- Když nepotřebujeme stabilitu.
- Velmi malá pole.

---

### 9. Porovnání Insert Sort vs. Select Sort

| Vlastnost | Insert Sort | Select Sort |
|-----------|-------------|-------------|
| **Časová složitost (nejhorší)** | O(n²) | O(n²) |
| **Časová složitost (průměr)** | O(n²) | O(n²) |
| **Časová složitost (nejlepší)** | **O(n)** | O(n²) |
| **Paměťová složitost** | O(1) | O(1) |
| **Stabilita** | **Stabilní** | Nestabilní |
| **Adaptivita** | **Adaptivní** | Neadaptivní |
| **Počet porovnání** | `~n²/4` průměr | **vždy `n(n−1)/2`** |
| **Počet přesunů/swapů** | `~n²/4` průměr | **max `n − 1`** |
| **Online třídění** | Ano | Ne |

**Závěr:**
- **Insert Sort** je výhodnější pro téměř setříděná data (O(n) v nejlepším případě) a když potřebujeme stabilitu.
- **Select Sort** je výhodnější, když jsou výměny drahé (lineární počet swapů).
- Oba jsou vhodné pouze pro **malá pole** (do ~100 prvků). Pro větší data jsou O(n²) prakticky nepoužitelné – pro 1 milion prvků by trvaly hodiny, zatímco O(n log n) algoritmy sekundy.

---

### 10. Praktické tipy a hybridní algoritmy

**V praxi se Insert Sort a Select Sort téměř nepoužívají samostatně**, ale jsou důležité jako součást hybridních algoritmů a jako výchozí teoretický základ.

**Hybridní algoritmy:**
- **TimSort** (Python `sorted`, Java `Arrays.sort` pro objekty) – Merge Sort kombinovaný s Insert Sortem na malé bloky. Detekuje "runs" (již seřazené úseky) a slévá je. Výchozí třídění v moderních jazycích.
- **IntroSort** (C++ STL `std::sort`, .NET `Array.Sort`) – Quick Sort, který přepíná na Heap Sort při velké rekurzi a na Insert Sort pro malá pole (< 16 prvků).
- **Block Sort** – využívá Insert Sort uvnitř bloků.

**Proč hybridní přístup?** Pro **malá pole** mají algoritmy O(n²) **menší konstantu** než O(n log n). Insert Sort pro 16 prvků je rychlejší než Merge Sort kvůli režii rekurze, alokace pomocného pole, cache miss atd. Přepnutí na Insert Sort pro malé úseky tedy zrychluje i jinak rychlejší algoritmy.

**Praktický příklad hybridu:**
```csharp
static void HybridSort(int[] pole, int left, int right)
{
    if (right - left < 16)
    {
        InsertSort(pole, left, right);
    }
    else
    {
        QuickSort(pole, left, right);
    }
}
```

**Měření výkonu v praxi:**
```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
InsertSort(data);
sw.Stop();
Console.WriteLine($"Čas: {sw.ElapsedMilliseconds} ms");
```

**Pro velká data v .NET:**
```csharp
Array.Sort(data);   // IntroSort, O(n log n)
List<int> list = data.ToList();
list.Sort();        // Stejný IntroSort
```

---

## Maturitní chytáky

### Časté chyby při implementaci

1. **Insert Sort – špatná hranice vnějšího cyklu:**
   ```csharp
   // ŠPATNĚ - začíná od 0
   for (int i = 0; i < pole.Length; i++)
   // SPRÁVNĚ - první prvek je už "setříděný"
   for (int i = 1; i < pole.Length; i++)
   ```

2. **Insert Sort – ztráta `key` při posuvu:**
   ```csharp
   // ŠPATNĚ - zapsali jsme přes původní pole[i]
   while (j >= 0 && pole[j] > pole[i])
   {
       pole[j + 1] = pole[j];
       j--;
   }
   // SPRÁVNĚ - uložit klíč PŘED posouváním
   int key = pole[i];
   while (j >= 0 && pole[j] > key) ...
   ```

3. **Select Sort – špatná inicializace `minIndex`:**
   ```csharp
   // ŠPATNĚ - minIndex vždy 0
   int minIndex = 0;
   // SPRÁVNĚ
   int minIndex = i;
   ```

4. **Select Sort – zbytečný swap sám se sebou:**
   ```csharp
   // SPRÁVNĚ s podmínkou (drobná optimalizace)
   if (minIndex != i)
       Swap(pole, i, minIndex);
   ```

5. **Insert Sort – chybějící podmínka `j >= 0`:**
   ```csharp
   // ŠPATNĚ - po posunu na pozici 0 ti j = -1, IndexOutOfRange
   while (pole[j] > key) ...
   // SPRÁVNĚ
   while (j >= 0 && pole[j] > key) ...
   ```

### Typické otázky u ústní zkoušky

> **Který algoritmus je stabilní a proč?**
> Insert Sort je stabilní, protože při hledání pozice posouváme prvky doprava jen pokud jsou **přísně větší** než `key` (`>`), nikoli `>=`. Tedy stejnou hodnotu nepřeskočíme. Select Sort není stabilní, protože swap může přeskočit stejnou hodnotu ležící mezi `i` a `minIndex`.

> **Kdy by Select Sort byl rychlejší než Insert Sort?**
> Když jsou výměny drahé (velké objekty, kopírování stovek bytů na prvek). Select Sort má max `n − 1` swapů, Insert Sort až `~n²/4` posunů.

> **Proč Insert Sort funguje v O(n) na setříděném poli?**
> Protože vnitřní `while` se nikdy nespustí – první porovnání `pole[j] > key` vrátí `false` a cyklus okamžitě skončí.

> **Lze zrychlit Select Sort na setříděném poli?**
> Ne – stále musíme projít celou nesetříděnou část pro nalezení minima. Žádná zkratka.

> **Proč nepoužíváme Insert/Select Sort na milionu prvků?**
> Pro `n = 10⁶` je `n² = 10¹²` operací. Při 10⁹ operací/s to je ~17 minut. Merge Sort s O(n log n) ≈ 2·10⁷ operací = 0.02 sekundy.

> **Jaký je dolní limit složitosti porovnávacího třídění?**
> `Ω(n log n)`. Existuje `n!` permutací a každé porovnání rozhoduje binární otázku, takže rozhodovací strom má hloubku `≥ log₂(n!) = Θ(n log n)`.

### Code review checklist

- [ ] Vnější cyklus: od 1 (Insert) nebo do `n−1` (Select).
- [ ] Správný směr porovnání (`>` vs `<`).
- [ ] Uložení `key` PŘED posouváním (Insert).
- [ ] `minIndex = i` na začátku každé iterace (Select).
- [ ] Podmínka `j >= 0` ve while (Insert).
- [ ] Swap s podmínkou `minIndex != i` (Select – mikrooptimalizace).

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 4 – Algoritmus a jeho vlastnosti** | Konečnost, determinismus, obecnost; Insert/Select jsou klasické příklady |
| **Ot. 7 – Časová a paměťová složitost** | O-notace, nejhorší/nejlepší/průměrný případ |
| **Ot. 11 – Bubble Sort, Merge Sort** | Další třídicí algoritmy, porovnání O(n²) vs. O(n log n) |
| **Ot. 12 – Quick Sort** | Pokročilejší třídění, Divide & Conquer, hybridní s Insert Sortem |
| **Ot. 13 – Heap Sort** | Další O(n log n) algoritmus, halda |
| **Ot. 14 – Vyhledávání** | Třídění jako prerekvizita pro binární vyhledávání |

---

## Quick Reference Card

```
╔══════════════════════════════════════════════════════════════╗
║              INSERT SORT vs SELECT SORT                      ║
╠══════════════════════════════════════════════════════════════╣
║  INSERT SORT                 │  SELECT SORT                  ║
║  "Skládání karet"            │  "Hledání minima"             ║
║                              │                               ║
║  1. Vezmi prvek              │  1. Najdi minimum             ║
║  2. Posuň větší doprava      │  2. Prohoď s první pozicí     ║
║  3. Vlož na místo            │  3. Opakuj pro zbytek         ║
║                              │                               ║
║  O(n) best / O(n²) worst     │  O(n²) VŽDY                   ║
║  STABILNÍ                    │  NESTABILNÍ                   ║
║  ADAPTIVNÍ                   │  NEADAPTIVNÍ                  ║
║  Hodně přesunů (~n²/4)       │  Málo swapů (max n−1)         ║
║  In-place, O(1) paměť        │  In-place, O(1) paměť         ║
║  Online                      │  Offline                      ║
╚══════════════════════════════════════════════════════════════╝
```

---

## Klíčové pojmy k zapamatování

- **Třídění (sorting)** – uspořádání prvků podle klíče a komparátoru.
- **Klíč (key)** – hodnota, podle které třídíme.
- **Komparátor (comparator)** – funkce vracející `<0`, `0`, `>0`.
- **Stabilní algoritmus** – zachovává relativní pořadí prvků se stejným klíčem.
- **In-place algoritmus** – O(1) extra paměti.
- **Adaptivní algoritmus** – rychlejší na téměř setříděných datech.
- **Online algoritmus** – umí přijímat data postupně.
- **Porovnávací třídění** – používá pouze porovnání; dolní mez `Ω(n log n)`.
- **Neporovnávací třídění** – využívá strukturu klíčů (Counting Sort, Radix Sort), může být O(n).
- **Insert Sort** – "skládání karet", `O(n)` best, `O(n²)` average/worst, stabilní, adaptivní.
- **Select Sort** – "hledání minima", `O(n²)` always, nestabilní, neadaptivní, ale jen `n−1` swapů.
- **Lower bound `Ω(n log n)`** – nejmenší možná složitost porovnávacího třídění.
- **TimSort** – hybridní algoritmus z Pythonu/Javy; Merge Sort + Insert Sort.
- **IntroSort** – hybridní algoritmus z .NET/C++; Quick Sort + Heap Sort + Insert Sort.
- **`Array.Sort()` / `List.Sort()` v .NET** – IntroSort.
- **Hybridní třídění** – přepíná na Insert Sort pro malé úseky kvůli nižší konstantě.
