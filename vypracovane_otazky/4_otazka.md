# Zápisky: Otázka č. 4 - Algoritmus a jeho vlastnosti

**Téma:** Algoritmus, jeho vlastnosti, časová a prostorová složitost, větvení a cykly

---

## Checklist bodů otázky

- [x] Pojem algoritmus (definice, historie, zápis)
- [x] Vlastnost: elementárnost
- [x] Vlastnost: konečnost
- [x] Vlastnost: determinovanost
- [x] Vlastnost: obecnost (hromadnost)
- [x] Vlastnost: determinismus
- [x] Vlastnost: výstup (rezultativnost)
- [x] Časová a prostorová složitost
- [x] Asymptotická notace (O, Ω, Θ) a Best/Average/Worst case
- [x] Příklad O(1) - konstantní
- [x] Příklad O(log n) - logaritmický
- [x] Příklad O(n) - lineární
- [x] Příklad O(n log n) - lineárně-logaritmický
- [x] Příklad O(n²) - kvadratický
- [x] O(2ⁿ), O(n!) - exponenciální a faktoriální
- [x] Větvení (if, switch, pattern matching)
- [x] Cykly (for, while, do-while, foreach)
- [x] Vývojový diagram a pseudokód

---

## 1. POJEM ALGORITMUS

### Definice

> **Algoritmus** je konečná posloupnost přesně definovaných kroků, která pro každý přípustný vstup vede v konečném čase k požadovanému výstupu.

**Klíčová slova v definici:**
- **konečná** → musí skončit
- **přesně definovaných** → každý krok je jednoznačný
- **přípustný vstup** → definovaná doména vstupů
- **konečný čas** → ne nekonečno
- **požadovaný výstup** → musí něco vyprodukovat (správné řešení úlohy)

### Historie pojmu

Slovo "algoritmus" pochází ze jména perského matematika **Muhammada ibn Músá al-Chwárizmího** (~780-850 n. l.), který napsal díla o aritmetice a řešení rovnic. Latinský přepis jeho jména "Algorismus" se postupně stal obecným označením pro výpočetní postup.

**Moderní formalizace:**
- **Alan Turing** (1936) – Turingův stroj jako matematický model algoritmu
- **Alonzo Church** – lambda kalkul, ekvivalentní s Turingovým strojem
- **Church-Turingova teze**: vše, co lze "intuitivně" považovat za algoritmus, lze vypočítat Turingovým strojem

### Algoritmus vs program

```
ALGORITMUS                          PROGRAM
- Abstraktní postup                 - Konkrétní implementace
- Nezávislý na jazyce               - V konkrétním jazyce (C#, Python...)
- Zapsán pseudokódem/diagramem      - Spustitelný kód
- Popisuje CO a JAK                 - Realizuje algoritmus
```

Tentýž algoritmus (např. bubble sort) lze naprogramovat v jakémkoliv jazyce – algoritmus zůstává stejný, mění se pouze syntaxe.

### Zápis algoritmu

**1. Slovní popis (čeština/angličtina):**
"Projdi pole. Pokud najdeš sousední dvojici v nesprávném pořadí, prohoď ji. Opakuj, dokud nedojde k žádné výměně."

**2. Pseudokód:**
```
funkce BubbleSort(pole):
    n = délka(pole)
    pro i od 0 do n-1:
        pro j od 0 do n-i-2:
            pokud pole[j] > pole[j+1]:
                prohoď pole[j] a pole[j+1]
```

**3. Vývojový diagram (flowchart):**
Grafický zápis pomocí standardizovaných symbolů (viz dále).

**4. Strukturogram (Nassi-Shneiderman):**
Vnořené obdélníky reprezentující strukturu programu.

**5. Programovací jazyk:**
Spustitelný kód v C#, Pythonu, Javě atd.

### Vývojový diagram - symboly

```
┌───────────┐       ELIPSA / OVÁL
│  Start    │   →   Začátek nebo konec algoritmu
└───────────┘

┌───────────┐       OBDÉLNÍK
│ x = a + b │   →   Zpracování (přiřazení, výpočet)
└───────────┘

  ◇                 KOSOČTVEREC
 / \                Rozhodování (větvení)
< x>0 >    →        Otázka s ANO/NE odpověďmi
 \ /
  ◇

┌─/─────────/      ROVNOBĚŽNÍK
│  Čti X    │   →  Vstup nebo výstup dat
└─/─────────/

   │
   ▼               ŠIPKA
                   Tok řízení (kam pokračovat)
```

**Příklad – Najdi maximum:**

```
       ┌─────────┐
       │  Start  │
       └────┬────┘
            ▼
       ┌─────────────┐
       │ max = pole[0]│
       │ i = 1        │
       └────┬────────┘
            ▼
         ◇
        /  \
       < i<n >─NE─→ Konec (vrať max)
        \  /
         ◇
         │ ANO
         ▼
         ◇
        /  \
   < pole[i]>max >─NE─┐
        \  /          │
         ◇            │
         │ ANO        │
         ▼            │
    ┌──────────┐      │
    │ max=pole[i]│    │
    └────┬─────┘      │
         │            │
         ▼            ▼
       ┌────────┐
       │ i = i+1│←────┘
       └───┬────┘
           ▲
           └── (zpět k podmínce)
```

---

## 2. ŠEST VLASTNOSTÍ ALGORITMU

| # | Vlastnost | Co znamená | Příklad porušení |
|---|-----------|------------|------------------|
| 1 | **Elementárnost** | Kroky jsou jednoduché a přímo proveditelné | "Vyřeš rovnici" – příliš složitý krok |
| 2 | **Konečnost** | Skončí po konečném počtu kroků | `while(true) { }` – nekonečná smyčka |
| 3 | **Determinovanost** | V každém kroku je jednoznačně definováno, co se má dělat | "Vezmi nějaké číslo" – nejednoznačné |
| 4 | **Obecnost (hromadnost)** | Řeší celou třídu úloh, ne jen jeden konkrétní případ | `vrať 42` – řeší jeden případ |
| 5 | **Determinismus** | Stejný vstup vede vždy ke stejnému výstupu | Použití `Random` – nedeterministické |
| 6 | **Výstup (rezultativnost)** | Vrací aspoň jeden výsledek | Funkce co nic nedělá ani nevrací |

### 2.1 Elementárnost

Každý krok algoritmu musí být **dostatečně jednoduchý** na to, aby ho vykonavatel (počítač, člověk) mohl provést bez další interpretace. Co je elementární závisí na vykonavateli:

- Pro **procesor**: instrukce ADD, MOV, JMP
- Pro **programátora v C#**: `x = a + b`, `if`, `for`
- Pro **kuchaře**: "rozšlehej vejce", "přidej sůl"

**Porušení:**
```
1. Vyřeš úlohu obchodního cestujícího pro 100 měst.
   (Není elementární – sám o sobě je to celý algoritmus!)
```

### 2.2 Konečnost

Algoritmus musí **vždy skončit po konečném počtu kroků**, ať je vstup jakýkoliv. Toto je jedna z nejdůležitějších vlastností – odlišuje algoritmus od procesu, který může běžet věčně (operační systém, server).

**Porušení – nekonečná smyčka:**
```csharp
int n = 5;
while (n > 0) {
    Console.WriteLine(n);
    // CHYBA: chybí n--; → cyklus nikdy neskončí
}
```

**Halting problem (problém zastavení):** Alan Turing dokázal, že **neexistuje obecný algoritmus**, který by pro libovolný program rozhodl, zda program skončí, nebo poběží navždy. Toto je hluboký výsledek teorie algoritmů.

### 2.3 Determinovanost

V každém kroku musí být **jednoznačně určeno**, co se má udělat dál. Nesmí být dvojznačnost.

**Porušení:**
```
1. Vezmi některé číslo z pole. (Které?)
2. Pokud se ti to líbí, vrať ho. (Co znamená "líbí"?)
```

**Pozor:** Determinovanost ≠ determinismus. Determinovanost se týká **kroků**, determinismus **výsledků**.

### 2.4 Obecnost (hromadnost)

Algoritmus musí řešit **celou třídu úloh**, ne jen jeden konkrétní případ. Vstupní data jsou parametry, ne pevné hodnoty.

**Není algoritmus (řeší 1 případ):**
```csharp
int Soucet() { return 5 + 3; }   // Sečte jen 5 a 3
```

**Je algoritmus (řeší třídu úloh):**
```csharp
int Soucet(int a, int b) { return a + b; }   // Sečte libovolná dvě čísla
```

### 2.5 Determinismus

Pro **stejný vstup** musí algoritmus vždy vyprodukovat **stejný výstup**, bez ohledu na to, kdy nebo kde běží.

**Porušení – nedeterministické algoritmy:**
```csharp
Random rng = new Random();
int Hod() { return rng.Next(1, 7); }   // Pokaždé jiné!
```

Nedeterministické postupy mají své místo (Monte Carlo simulace, randomizované algoritmy), ale **klasický algoritmus** je deterministický.

### 2.6 Výstup (rezultativnost)

Algoritmus musí vyprodukovat **alespoň jeden výsledek** (návratovou hodnotu, výpis, změnu stavu). Algoritmus, který nic nedělá a nic nevrací, je k ničemu.

**Příklady výstupů:**
- Návratová hodnota funkce (`return`)
- Tisk na konzoli (`Console.WriteLine`)
- Zápis do souboru
- Změna parametru předaného referencí
- Změna stavu objektu (metoda voidu, která mění pole třídy)

### POZOR: Determinovanost vs determinismus

```
DETERMINOVANOST = jednoznačnost KROKŮ     (víš CO máš dělat)
DETERMINISMUS   = jednoznačnost VÝSLEDKŮ  (víš CO DOSTANEŠ ze stejného vstupu)

Algoritmus může být:
- determinovaný + deterministický → klasický algoritmus
- determinovaný + nedeterministický → randomizovaný (každý krok jasný,
  ale výsledek závisí na náhodě)
```

---

## 3. ČASOVÁ A PROSTOROVÁ SLOŽITOST

### Co to je?

| Typ | Měří | Otázka |
|-----|------|--------|
| **Časová složitost** | Počet elementárních operací | Kolik kroků algoritmus provede? |
| **Prostorová složitost** | Spotřebu paměti | Kolik paměti algoritmus potřebuje? |

### Proč neměříme v sekundách?

Doba běhu v sekundách závisí na:
- Rychlosti procesoru
- Množství RAM
- Programovacím jazyce
- Optimalizacích kompilátoru
- Vytížení systému

**Složitost** je **univerzální měřítko**, které popisuje, **jak rychle náročnost algoritmu roste s velikostí vstupu n**. Tím se dá srovnávat algoritmy nezávisle na hardwaru.

### Asymptotická analýza

Zajímá nás chování pro **velká n** (n → ∞). Konstanty a méně významné členy se ignorují:

```
T(n) = 3n² + 5n + 7   →  složitost je O(n²)
                          (kvadratický člen převládne pro velká n)

T(n) = 100n + 1       →  O(n)   (lineární)
T(n) = log(n) + 50    →  O(log n)
T(n) = 2ⁿ + n²        →  O(2ⁿ)  (exponenciála poráží polynom)
```

### Asymptotická notace - O, Ω, Θ

| Notace | Význam | Slovem |
|--------|--------|--------|
| **O(f(n))** | Horní mez | "Nejhůř roste jako f(n)" |
| **Ω(f(n))** | Dolní mez | "Nejlépe roste jako f(n)" |
| **Θ(f(n))** | Přesná mez | "Roste přesně jako f(n)" |

**Praxe:** většinou se mluví o **Big O** (nejhorší případ). Formálně se ale **Big Theta (Θ)** používá, když známe přesnou rychlost růstu.

```
Bubble sort:
- Best case  (už setříděné):    Ω(n)
- Worst case (obráceně):        O(n²)
- Average:                       Θ(n²)
```

### Best / Average / Worst case

| Případ | Význam | Příklad – lineární vyhledávání |
|--------|--------|--------------------------------|
| **Best case** | Nejlepší možný vstup | Hledaný prvek je hned první → O(1) |
| **Average case** | Průměrný vstup | Prvek někde uprostřed → O(n/2) = O(n) |
| **Worst case** | Nejhorší možný vstup | Prvek je poslední nebo není v poli → O(n) |

**V praxi se ptáme na worst case**, protože garantuje horní mez pro libovolný vstup.

### Tabulka růstu

| Složitost | n=10 | n=100 | n=1 000 | n=1 000 000 |
|-----------|------|-------|---------|-------------|
| O(1) | 1 | 1 | 1 | 1 |
| O(log n) | 3 | 7 | 10 | 20 |
| O(n) | 10 | 100 | 1 000 | 1 000 000 |
| O(n log n) | 33 | 700 | 10 000 | 20 mil. |
| O(n²) | 100 | 10 000 | 1 000 000 | 10¹² |
| O(2ⁿ) | 1024 | 10³⁰ | ∞ (prakticky) | ∞ |
| O(n!) | 3.6 mil. | 10¹⁵⁸ | ∞ | ∞ |

**Praktický dopad:** algoritmus O(n²) pro n=10⁶ udělá 10¹² operací – při miliardě operací za sekundu to trvá ~17 minut. O(n log n) zvládne to samé za ~0.02 s.

### Prostorová složitost - příklady

```csharp
// O(1) prostor - jen pár proměnných
int Soucet(int[] pole) {
    int s = 0;
    for (int i = 0; i < pole.Length; i++) s += pole[i];
    return s;
}

// O(n) prostor - vytvořím nové pole velikosti n
int[] Zdvojuj(int[] pole) {
    int[] novy = new int[pole.Length];
    for (int i = 0; i < pole.Length; i++) novy[i] = pole[i] * 2;
    return novy;
}

// O(n) prostor (skrytý) - rekurze používá Call Stack
int Faktorial(int n) {
    if (n <= 1) return 1;
    return n * Faktorial(n - 1);  // n stack frames!
}
```

**Pozor:** rekurze "stojí" paměť na Call Stacku – často přehlížený zdroj prostorové složitosti.

---

## 4. O(1) - KONSTANTNÍ SLOŽITOST

> Počet operací je **vždy stejný**, nezávisí na velikosti vstupu.

```csharp
// Přístup k prvku pole - O(1)
int prvek = pole[500];

// Operace se zásobníkem/frontou - O(1)
stack.Push(42);
int x = stack.Pop();
queue.Enqueue(42);
int y = queue.Dequeue();

// Přístup do Dictionary/HashSet - O(1) průměrně (hash)
int vek = slovnik["Petr"];
bool je = mnozina.Contains(42);

// Aritmetické operace - O(1)
int vysledek = a + b * c;

// Délka pole/listu - O(1)
int delka = pole.Length;
int pocet = list.Count;
```

**Charakteristika:** žádný cyklus závislý na n. Doba běhu je stejná pro 10 i 10 000 000 prvků.

---

## 5. O(log n) - LOGARITMICKÁ SLOŽITOST

> S každým krokem se **problém zmenší o konstantní podíl** (typicky na polovinu). Pro miliardu prvků stačí ~30 kroků!

```csharp
// Binární vyhledávání - O(log n)
// POZOR: Pole MUSÍ být setříděné!
int BinarniVyhledavani(int[] pole, int hledany) {
    int levy = 0;
    int pravy = pole.Length - 1;

    while (levy <= pravy) {
        int stred = (levy + pravy) / 2;

        if (pole[stred] == hledany)
            return stred;
        else if (pole[stred] < hledany)
            levy = stred + 1;
        else
            pravy = stred - 1;
    }
    return -1;
}
```

**Vizualizace pro pole [2, 5, 13, 27, 45, 67, 78, 91, 99], hledáme 67:**

```
Krok 1: [2, 5, 13, 27, 45, 67, 78, 91, 99]   střed=45, 67>45 → vpravo
Krok 2:                  [67, 78, 91, 99]    střed=78, 67<78 → vlevo
Krok 3:                  [67]                střed=67, NALEZENO

9 prvků, 3 kroky.   log₂(9) ≈ 3.17
```

**Pravidlo:** Půlení problému v každém kroku → O(log n).

**Další příklady O(log n):**
- Hledání v BST (binary search tree) – pokud je vyvážený
- Operace v haldě (binary heap): insert, extract-min
- Algoritmus exponenciace umocňováním (`x^n` v log n krocích)

---

## 6. O(n) - LINEÁRNÍ SLOŽITOST

> Počet operací roste **přímo úměrně** s velikostí vstupu.

```csharp
// Hledání prvku v poli - O(n) (lineární vyhledávání)
int NajdiIndex(int[] pole, int hledany) {
    for (int i = 0; i < pole.Length; i++) {
        if (pole[i] == hledany)
            return i;
    }
    return -1;
}

// Součet prvků - O(n)
int Soucet(int[] pole) {
    int suma = 0;
    foreach (int x in pole) suma += x;
    return suma;
}

// Hledání maxima - O(n)
int NajdiMax(int[] pole) {
    int max = pole[0];
    for (int i = 1; i < pole.Length; i++)
        if (pole[i] > max) max = pole[i];
    return max;
}

// Kopie pole - O(n)
int[] Kopie(int[] pole) {
    int[] novy = new int[pole.Length];
    for (int i = 0; i < pole.Length; i++) novy[i] = pole[i];
    return novy;
}
```

**Pravidlo:** Jeden cyklus přes n prvků = O(n).

**Pozor:** několik sekvenčních cyklů přes n prvků je stále O(n), ne O(n²):
```csharp
for (int i = 0; i < n; i++) /* ... */    // O(n)
for (int i = 0; i < n; i++) /* ... */    // O(n)
// Dohromady: O(n) + O(n) = O(2n) = O(n)
```

---

## 7. O(n log n) - LINEÁRNĚ-LOGARITMICKÁ SLOŽITOST

> Pro každý z n prvků provedeme log n operací. Typická složitost **rychlých třídicích algoritmů**.

**Klasické algoritmy O(n log n):**
- **Merge Sort** – rozdělit pole na poloviny (log n úrovní) a slévat (n operací na úroveň)
- **Heap Sort** – n× extract-min z haldy, každý v O(log n)
- **Quick Sort** – průměrně O(n log n), worst case O(n²)
- **Třídění obecných srovnáním** – dolní mez Ω(n log n) (nelze rychleji)

```csharp
// Merge Sort - O(n log n) čas, O(n) paměť
void MergeSort(int[] pole, int levy, int pravy) {
    if (levy >= pravy) return;
    int stred = (levy + pravy) / 2;

    MergeSort(pole, levy, stred);       // T(n/2)
    MergeSort(pole, stred + 1, pravy);  // T(n/2)
    Merge(pole, levy, stred, pravy);    // O(n) slití
}

// Rekurence: T(n) = 2·T(n/2) + O(n) → O(n log n)
```

**Vizualizace merge sortu pro n=8:**
```
[8,3,5,1,7,2,6,4]                       úroveň 0 (1× n práce)
       /        \
[8,3,5,1]    [7,2,6,4]                  úroveň 1 (2× n/2 práce = n)
   /  \         /  \
[8,3][5,1]  [7,2][6,4]                  úroveň 2 (4× n/4 = n)
 / \  / \    / \  / \
[8][3][5][1][7][2][6][4]                úroveň 3 (8× n/8 = n)

log₂(8) = 3 úrovní × n práce/úroveň = O(n log n)
```

---

## 8. O(n²) - KVADRATICKÁ SLOŽITOST

> Počet operací roste **se čtvercem** vstupu. 2× více dat → 4× déle, 10× více dat → 100× déle.

```csharp
// Bubble Sort - O(n²)
void BubbleSort(int[] pole) {
    int n = pole.Length;
    for (int i = 0; i < n - 1; i++) {           // n×
        for (int j = 0; j < n - i - 1; j++) {   // n×
            if (pole[j] > pole[j + 1]) {
                (pole[j], pole[j + 1]) = (pole[j + 1], pole[j]);
            }
        }
    }
}

// Selection Sort - O(n²)
void SelectionSort(int[] pole) {
    for (int i = 0; i < pole.Length - 1; i++) {
        int min = i;
        for (int j = i + 1; j < pole.Length; j++)
            if (pole[j] < pole[min]) min = j;
        (pole[i], pole[min]) = (pole[min], pole[i]);
    }
}

// Insert Sort - O(n²)
void InsertSort(int[] pole) {
    for (int i = 1; i < pole.Length; i++) {
        int klic = pole[i];
        int j = i - 1;
        while (j >= 0 && pole[j] > klic) {
            pole[j + 1] = pole[j];
            j--;
        }
        pole[j + 1] = klic;
    }
}

// Hledání duplikátů (porovnání každého s každým) - O(n²)
bool MaDuplicit(int[] pole) {
    for (int i = 0; i < pole.Length; i++)
        for (int j = i + 1; j < pole.Length; j++)
            if (pole[i] == pole[j]) return true;
    return false;
}
```

**Pravidlo:** Dva vnořené cykly, každý přes n prvků = O(n²).

**Pozor – vnořený cyklus nemusí být O(n²):**
```csharp
for (int i = 0; i < n; i++)
    for (int j = 0; j < 100; j++)   // konstantní počet iterací!
        /* ... */
// Výsledek: O(100n) = O(n)
```

---

## 9. O(2ⁿ) - EXPONENCIÁLNÍ SLOŽITOST

> Každé přidání jednoho prvku vstupu **zdvojnásobí** počet operací.

**Klasický příklad – naivní rekurzivní Fibonacci:**
```csharp
int Fib(int n) {
    if (n <= 1) return n;
    return Fib(n - 1) + Fib(n - 2);   // 2 rekurzivní volání!
}
```

Pro `Fib(40)` se funkce zavolá ~331 milionkrát! Strom volání má hloubku n a v každé úrovni se větve zdvojnásobují.

**Další exponenciální problémy:**
- Generování všech podmnožin množiny (2ⁿ podmnožin)
- Hledání všech kombinací bez paměti (backtracking bez prořezávání)
- Hanojské věže (přesun n disků trvá 2ⁿ−1 přesunů)

**O(n!) - faktoriální:**
- Permutace n prvků (n! permutací)
- Brute-force řešení obchodního cestujícího (zkus všechny trasy)
- Pro n=20 už n! ≈ 2.4 × 10¹⁸ – nepraktické

---

## 10. VĚTVENÍ (IF, SWITCH)

### IF - základní podmíněné větvení

```csharp
// Jednoduchý if
if (vek >= 18) {
    Console.WriteLine("Dospělý");
}

// If-else
if (cislo % 2 == 0)
    Console.WriteLine("Sudé");
else
    Console.WriteLine("Liché");

// If-else if-else (řetězec)
if (znamka == 1)
    Console.WriteLine("Výborně");
else if (znamka == 2)
    Console.WriteLine("Chvalitebně");
else if (znamka == 3)
    Console.WriteLine("Dobře");
else
    Console.WriteLine("Nedostatečně");

// Ternární operátor (zkrácený if-else)
string stav = vek >= 18 ? "Dospělý" : "Nezletilý";

// Logické operátory: && (AND), || (OR), ! (NOT)
if (vek >= 18 && jeStudent)
    Console.WriteLine("Dospělý student");
```

### SWITCH - přepínač podle hodnoty

```csharp
// Klasický switch
switch (den) {
    case 1:
        Console.WriteLine("Pondělí");
        break;
    case 2:
        Console.WriteLine("Úterý");
        break;
    case 6:
    case 7:
        Console.WriteLine("Víkend!");
        break;
    default:
        Console.WriteLine("Neplatný den");
        break;
}
```

**Switch expression (C# 8+) - moderní zápis:**
```csharp
string nazev = den switch {
    1 => "Pondělí",
    2 => "Úterý",
    3 or 4 or 5 => "Pracovní den",
    6 or 7 => "Víkend",
    _ => "Neplatný den"      // _ je default
};
```

**Pattern matching (C# 9+) - mocný switch:**
```csharp
string Popis(object o) => o switch {
    int n when n < 0 => "Záporné celé číslo",
    int 0 => "Nula",
    int n => $"Kladné: {n}",
    string s => $"Řetězec: {s}",
    null => "null",
    _ => "Neznámý typ"
};
```

### Kdy if a kdy switch?

| Situace | Doporučení |
|---------|------------|
| 2-3 podmínky | `if-else` |
| Mnoho hodnot jedné proměnné | `switch` |
| Komplexní podmínky (AND/OR) | `if-else` |
| Rozhodování dle typu | `switch` s pattern matchingem |
| Mapování hodnota → hodnota | `switch expression` |

---

## 11. CYKLY (FOR, WHILE, DO-WHILE, FOREACH)

### FOR - známý počet opakování

```csharp
// Základní for - 10× opakování
for (int i = 0; i < 10; i++) {
    Console.WriteLine(i);
}

// Procházení pole s indexem
for (int i = 0; i < pole.Length; i++) {
    Console.WriteLine($"pole[{i}] = {pole[i]}");
}

// Pozpátku
for (int i = pole.Length - 1; i >= 0; i--) {
    Console.WriteLine(pole[i]);
}

// Krok jiný než 1
for (int i = 0; i <= 100; i += 5) {
    Console.WriteLine(i);   // 0, 5, 10, ..., 100
}
```

**Anatomie for:**
```
for (INICIALIZACE; PODMÍNKA; KROK) {
    TĚLO
}

Posloupnost: INICIALIZACE → PODMÍNKA → TĚLO → KROK → PODMÍNKA → TĚLO → KROK → ...
```

### WHILE - opakuj dokud platí podmínka

```csharp
// Klasický while
string vstup = "";
while (vstup != "konec") {
    vstup = Console.ReadLine();
}

// Půlení čísla (typický O(log n) pattern!)
int n = 1024;
int kroky = 0;
while (n > 1) {
    n = n / 2;
    kroky++;
}
// kroky = 10 (log₂(1024) = 10)
```

### DO-WHILE - vždy alespoň jedna iterace

```csharp
int volba;
do {
    Console.WriteLine("1. Hra  2. Konec");
    volba = int.Parse(Console.ReadLine());
} while (volba != 2);
```

**Rozdíl while vs do-while:**
- `while` – podmínka se testuje **před** prvním provedením těla → tělo se nemusí provést ani jednou
- `do-while` – tělo se provede **vždy aspoň jednou**, podmínka se testuje **po**

### FOREACH - procházení kolekcí

```csharp
// Pole
foreach (int cislo in pole) {
    Console.WriteLine(cislo);
}

// List
foreach (string jmeno in seznam) {
    Console.WriteLine(jmeno);
}

// Dictionary
foreach (var zaznam in slovnik) {
    Console.WriteLine($"{zaznam.Key}: {zaznam.Value}");
}

// Vlastní třída musí implementovat IEnumerable<T>
foreach (Student s in studenti) { /* ... */ }
```

**Výhody foreach:**
- Stručnější syntaxe
- Žádné off-by-one chyby s indexy
- Pracuje s libovolnou `IEnumerable<T>`

**Nevýhody foreach:**
- Nemáš přístup k indexu (pokud nepoužiješ `i` zvlášť)
- Nesmíš modifikovat kolekci během průchodu

### BREAK a CONTINUE

```csharp
// break - okamžitě ukonči cyklus
for (int i = 0; i < 100; i++) {
    if (i == 5) break;
    Console.WriteLine(i);   // Vypíše 0,1,2,3,4
}

// continue - přeskoč na další iteraci
for (int i = 0; i < 10; i++) {
    if (i % 2 == 0) continue;
    Console.WriteLine(i);   // Vypíše 1,3,5,7,9
}
```

### Vnořené cykly

```csharp
// Tisk násobilky 1-5
for (int i = 1; i <= 5; i++) {
    for (int j = 1; j <= 5; j++) {
        Console.Write($"{i*j,3} ");
    }
    Console.WriteLine();
}
// Výstup:
//   1   2   3   4   5
//   2   4   6   8  10
//   ...
```

**Pozor:** každá další vnořená vrstva typicky násobí složitost faktorem n.

---

## 12. MATURITNÍ CHYTÁKY

### 1. Determinovanost ≠ determinismus
```
Determinovanost = jednoznačnost KROKŮ        (víš CO dělat)
Determinismus   = jednoznačnost VÝSLEDKŮ     (víš CO DOSTANEŠ)
```

### 2. List.Contains() není O(1)!
```csharp
list.Contains(x);            // O(n) - prochází celý seznam
slovnik.ContainsKey(x);      // O(1) - hashování
mnozina.Contains(x);         // O(1) - hashování (HashSet)
```

### 3. Binární vyhledávání vyžaduje SETŘÍDĚNÉ pole
```csharp
// CHYBA: nefunguje na nesetříděném poli
int idx = BinarniVyhledavani(nesetrideneSeznam, 42);

// SPRÁVNĚ: nejdřív setřídit (O(n log n)) nebo udržovat pole setříděné
Array.Sort(pole);
int idx = BinarniVyhledavani(pole, 42);
```

### 4. Časté chyby v cyklech
```csharp
// Nekonečný cyklus (chybí inkrement)
int i = 0;
while (i < 10) {
    Console.WriteLine(i);
    // CHYBA: chybí i++
}

// Off-by-one error
for (int i = 0; i <= pole.Length; i++)   // CHYBA: IndexOutOfRangeException!
    Console.WriteLine(pole[i]);

// Modifikace kolekce při foreach
foreach (var x in list) {
    list.Remove(x);   // CHYBA: InvalidOperationException
}
```

### 5. Přiřazení vs porovnání
```csharp
if (x = 5)    // CHYBA: přiřazení (v C# nezkompiluje pro int, v jiných jazycích past)
if (x == 5)   // SPRÁVNĚ: porovnání
```

### 6. switch bez break propadává (v C# se to ale hlídá)
```csharp
// V jazyce C/Java by toto propadlo, v C# musí být break nebo return
switch (x) {
    case 1:
        Console.WriteLine("Jedna");
        // V C# CHYBA bez break!
    case 2:
        Console.WriteLine("Dvě");
        break;
}
```

### 7. Složitost není o sekundách, ale o růstu
```
O(n²) algoritmus s rychlým procesorem může být na malých datech
rychlejší než O(n log n) na pomalém procesoru.
Ale s rostoucím n O(n log n) VŽDY vyhraje.
```

### 8. Big O ignoruje konstanty
```
T(n) = 1000n        → O(n)     (konstanta 1000 se vypouští)
T(n) = n + 5n²      → O(n²)    (lineární člen je zanedbatelný)
T(n) = log₂(n)      → O(log n) (základ logaritmu nezáleží)
```

### 9. Rekurze má skrytou prostorovou složitost
```csharp
int Faktorial(int n) {
    if (n <= 1) return 1;
    return n * Faktorial(n - 1);   // n stack framů na Call Stacku!
}
// Časová O(n), prostorová O(n) - kvůli zásobníku volání
```

### 10. n log n na maturitě – vědět příklady
```
Merge Sort, Heap Sort, Quick Sort (průměr)
Nejlepší možná složitost srovnávacího třídění = Ω(n log n)
```

---

## 13. SOUHRNNÁ TABULKA SLOŽITOSTÍ

| Složitost | Příklady algoritmů |
|-----------|---------------------|
| **O(1)** | Přístup do pole, push/pop, hashtable lookup |
| **O(log n)** | Binární vyhledávání, BST search, heap insert |
| **O(n)** | Lineární vyhledávání, součet, kopie pole, max/min |
| **O(n log n)** | Merge sort, heap sort, quick sort (průměr) |
| **O(n²)** | Bubble sort, selection sort, insert sort, nested loops |
| **O(n³)** | Maticové násobení (naivně), Floyd-Warshall |
| **O(2ⁿ)** | Naivní rekurzivní Fibonacci, podmnožiny, brute-force backtracking |
| **O(n!)** | Permutace, brute-force TSP |

---

## 14. KLÍČOVÉ POJMY K ZAPAMATOVÁNÍ

- **Algoritmus** = konečná posloupnost jednoznačně definovaných kroků vedoucích ke správnému výstupu
- **6 vlastností:** elementárnost, konečnost, determinovanost, obecnost, determinismus, výstup
- **Determinovanost** (jasné kroky) vs **determinismus** (stejný vstup → stejný výstup)
- **Časová složitost** popisuje růst počtu operací; **prostorová** růst paměti
- **Big O** = horní mez, **Ω** = dolní mez, **Θ** = přesná mez
- **Best/Average/Worst case** – obvykle nás zajímá worst case
- **Asymptotická analýza** ignoruje konstanty a méně významné členy
- **O(log n)** = půlení v každém kroku (binární vyhledávání)
- **O(n log n)** = nejlepší možná složitost srovnávacího třídění
- **O(n²)** = dva vnořené cykly přes n
- **Vývojový diagram** = grafický zápis algoritmu pomocí standardních symbolů
- **Pseudokód** = textový zápis algoritmu nezávislý na jazyce
- **if/switch** = větvení; **for/while/foreach** = cykly
