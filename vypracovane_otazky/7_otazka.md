# 📚 Zápisky: Otázka č. 7 - Časová a paměťová složitost

**Datum:** 2024-12-28  
**Status:** ✅ Hotovo

---

## ✅ Checklist bodů otázky

- [x] Bod 1: Vysvětlení pojmu – co, k čemu, proč
- [x] Bod 2: Nejhorší, nejlepší a průměrný případ
- [x] Bod 3: O-notace, horní a dolní odhad
- [x] Bod 4: Způsob určení časové a prostorové složitosti
- [x] Bod 5: Vzhledem k čemu časovou složitost určujeme
- [x] Bod 6: Příklad O(1) – konstantní čas
- [x] Bod 7: Příklad O(n) – lineární čas
- [x] Bod 8: Příklad O(n²) – kvadratický čas
- [x] Bod 9: Příklad O(log n) – logaritmický čas
- [x] Bod 10: Vylepšení exponenciální složitosti Fibonacciho

---

## 🧠 Klíčové koncepty

### 1. Co je složitost a proč ji měříme

| Typ | Co měří | Jednotka |
|-----|---------|----------|
| **Časová složitost** | Jak roste počet operací s velikostí vstupu | Počet kroků |
| **Paměťová složitost** | Kolik EXTRA paměti algoritmus potřebuje | Bajty / prvky |

**Proč je to důležité:**
- Porovnání algoritmů nezávisle na hardware
- Predikce výkonu pro velká data
- Optimalizace – víš, kde hledat problém

> 💡 **Složitost neměří sekundy, ale počet kroků. Sekundy závisí na počítači, kroky jsou univerzální.**

---

### 2. Nejhorší, nejlepší a průměrný případ

| Případ | Značení | Co znamená |
|--------|---------|------------|
| 🔴 **Nejhorší** | O(n) | Nikdy to nebude horší |
| 🟢 **Nejlepší** | Ω(n) | Nikdy to nebude lepší |
| 🟡 **Průměrný** | Θ(n) | Typický případ |

**Na maturitě téměř vždy uvádíme NEJHORŠÍ případ (Big O).**

Příklad – lineární vyhledávání v poli `[5, 8, 3, 9, 1]`:
- Nejlepší: hledáme `5` → 1 krok
- Průměrný: hledáme `3` → ~n/2 kroků  
- Nejhorší: hledáme `1` nebo `99` → n kroků

---

### 3. O-notace a pravidla zjednodušování

**Klíčová myšlenka:** Pro velká n nás zajímá jen **dominantní člen**.

| Původní výraz | O-notace | Pravidlo |
|---------------|----------|----------|
| `5n + 3` | O(n) | Konstanty vynecháváme |
| `n² + n` | O(n²) | Necháváme jen největší člen |
| `3n² + 2n + 1` | O(n²) | Kombinace obou pravidel |
| `log₂(n)` | O(log n) | Základ logaritmu nepíšeme |

**Typy odhadů:**
```
O(n)  → Horní odhad  – "Nikdy to nebude HORŠÍ"
Ω(n)  → Dolní odhad  – "Nikdy to nebude LEPŠÍ"
Θ(n)  → Těsný odhad  – "Je to PŘESNĚ toto"
```

---

### 4. Jak určit složitost

#### Časová složitost – postup:

1. **Najdi cykly** – hlavní viníci
2. **Spočítej vnoření** – cyklus v cyklu = násobení
3. **Sečti nezávislé části** – nechej jen největší

```csharp
// Jeden cyklus = O(n)
for (int i = 0; i < n; i++) { ... }

// Dva vnořené cykly = O(n²)
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++) { ... }

// Dva cykly za sebou = O(n) + O(n) = O(n)
for (int i = 0; i < n; i++) { ... }
for (int j = 0; j < n; j++) { ... }

// Půlení = O(log n)
while (n > 0) { n = n / 2; }
```

#### Paměťová složitost:

| Situace | Složitost |
|---------|-----------|
| Jen pár proměnných (`int i, j, temp`) | O(1) |
| Nové pole stejné velikosti jako vstup | O(n) |
| 2D matice n×n | O(n²) |
| Rekurze hloubky n (call stack) | O(n) |

**Rekurze zabírá místo na stacku!**
```csharp
int Faktorial(int n)  // Paměť: O(n) kvůli call stacku!
{
    if (n <= 1) return 1;
    return n * Faktorial(n - 1);
}
```

---

### 5. Vzhledem k čemu určujeme složitost

| Kontext | Co je "n" |
|---------|-----------|
| Pole/List | Počet prvků |
| Řetězec | Délka stringu |
| Matice | Rozměr (n×n) nebo počet buněk |
| Graf | Počet vrcholů (V) a hran (E) |
| Číslo | Hodnota nebo počet cifer |
| Strom | Počet uzlů nebo hloubka |

**U grafů máme DVA parametry:**
```csharp
// BFS: O(V + E)
// Dijkstra: O((V + E) log V)
```

**Vždy explicitně řekni:**
> "Složitost je O(n), kde **n je počet prvků v poli**."

---

## 💻 Příklady složitostí s kódem

### O(1) – Konstantní čas

```csharp
// Přístup k prvku pole
int prvek = pole[5];

// Stack/Queue operace
stack.Push(x);
int y = stack.Pop();

// Dictionary (průměrně)
slovnik["klic"] = 100;

// Swap
void Swap(int[] pole, int i, int j)
{
    int temp = pole[i];
    pole[i] = pole[j];
    pole[j] = temp;
}

// Délka pole
int delka = pole.Length;  // Metadata, nepočítá se
```

---

### O(n) – Lineární čas

```csharp
// Hledání prvku
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

// Hledání maxima
int Maximum(int[] pole)
{
    int max = pole[0];
    for (int i = 1; i < pole.Length; i++)
        if (pole[i] > max)
            max = pole[i];
    return max;
}
```

---

### O(n²) – Kvadratický čas

```csharp
// Bubble Sort
void BubbleSort(int[] pole)
{
    for (int i = 0; i < pole.Length; i++)
    {
        for (int j = 0; j < pole.Length - 1; j++)
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

// Všechny páry
void VsechnyPary(int[] pole)
{
    for (int i = 0; i < pole.Length; i++)
        for (int j = 0; j < pole.Length; j++)
            Console.WriteLine($"{pole[i]}, {pole[j]}");
}

// Kontrola duplicit (naivní)
bool MaDuplicity(int[] pole)
{
    for (int i = 0; i < pole.Length; i++)
        for (int j = i + 1; j < pole.Length; j++)
            if (pole[i] == pole[j])
                return true;
    return false;
}
```

---

### O(log n) – Logaritmický čas

```csharp
// Binární vyhledávání
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

// Půlení čísla
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

**Proč log n?** Kolikrát můžeš dělit 1 000 000 dvěma? → ~20 kroků!

---

### Fibonacci – vylepšení O(2ⁿ) → O(n)

#### ❌ Naivní rekurze – O(2ⁿ) čas, O(n) paměť
```csharp
int FibRekurze(int n)
{
    if (n <= 1) return n;
    return FibRekurze(n - 1) + FibRekurze(n - 2);
}
// PROBLÉM: Počítáme stejné hodnoty znovu a znovu!
```

#### ✅ Memoizace – O(n) čas, O(n) paměť
```csharp
int FibMemo(int n, int[] cache)
{
    if (n <= 1) return n;
    if (cache[n] != -1) return cache[n];
    
    cache[n] = FibMemo(n - 1, cache) + FibMemo(n - 2, cache);
    return cache[n];
}
```

#### ✅ Iterativní s polem – O(n) čas, O(n) paměť
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

#### 🏆 Optimální – O(n) čas, O(1) paměť
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
    
    return minuly;  // Vrací n-té Fibonacciho číslo
}
```

**Průběh pro n=5:**
| i | predminuly | minuly | soucasny | F(i) |
|---|------------|--------|----------|------|
| 2 | 0 | 1 | 1 | F(2)=1 |
| 3 | 1 | 1 | 2 | F(3)=2 |
| 4 | 1 | 2 | 3 | F(4)=3 |
| 5 | 2 | 3 | 5 | F(5)=5 ✓ |

---

## 📊 Přehledová tabulka složitostí

### Porovnání růstu:

| n | O(1) | O(log n) | O(n) | O(n log n) | O(n²) | O(2ⁿ) |
|---|------|----------|------|------------|-------|-------|
| 10 | 1 | 3 | 10 | 33 | 100 | 1024 |
| 100 | 1 | 7 | 100 | 664 | 10 000 | 10³⁰ |
| 1 000 | 1 | 10 | 1 000 | 10 000 | 1 000 000 | ∞ |
| 1 000 000 | 1 | 20 | 10⁶ | 2×10⁷ | 10¹² | ∞ |

### Typické složitosti algoritmů:

| Algoritmus | Časová | Paměťová |
|------------|--------|----------|
| Přístup k poli | O(1) | O(1) |
| Lineární vyhledávání | O(n) | O(1) |
| Binární vyhledávání | O(log n) | O(1) |
| Bubble/Selection/Insert Sort | O(n²) | O(1) |
| Merge Sort | O(n log n) | O(n) |
| Quick Sort | O(n log n) průměr | O(log n) |
| Heap Sort | O(n log n) | O(1) |
| DFS/BFS | O(V + E) | O(V) |
| Dijkstra (s haldou) | O((V+E) log V) | O(V) |

---

## ⚠️ Maturitní chytáky

### Časová složitost:

> ❓ "Je O(2n) to samé jako O(n)?"  
> ✅ **ANO** – konstanty zanedbáváme.

> ❓ "Jaká je složitost lineárního vyhledávání?"  
> ✅ **O(n)** v nejhorším případě, O(1) v nejlepším.

> ❓ "Lze najít maximum rychleji než O(n)?"  
> ✅ **NE** – musíš vidět každý prvek aspoň jednou.

> ❓ "Je binární vyhledávání vždy O(log n)?"  
> ✅ **ANO, ale pole musí být SEŘAZENÉ!**

> ❓ "Tohle je O(n²)?"
> ```csharp
> for (int i = 0; i < n; i++)
>     for (int j = 0; j < 10; j++)  // Konstanta!
> ```
> ✅ **NE!** Je to O(10n) = O(n).

> ❓ "Tohle je O(n²)?"
> ```csharp
> for (int i = 0; i < n; i++)
>     for (int j = i; j < n; j++)  // Začíná od i
> ```
> ✅ **ANO!** Je to n + (n-1) + ... + 1 = n(n+1)/2 = O(n²).

### Paměťová složitost:

> ❓ "Jaká je prostorová složitost Merge Sortu?"  
> ✅ **O(n)** – potřebuje pomocné pole pro slévání.

> ❓ "Jaká je prostorová složitost rekurzivního faktoriálu?"  
> ✅ **O(n)** – hloubka call stacku!

> ❓ "Co znamená in-place algoritmus?"  
> ✅ Paměťová složitost **O(1)** – nepotřebuje extra paměť.

### Fibonacci:

> ❓ "Proč je naivní rekurze Fibonacciho tak pomalá?"  
> ✅ Kvůli **překrývajícím se podproblémům** – stejné hodnoty počítáme mnohokrát.

---

## 🚀 Senior Tipy

1. **Trade-off čas vs. paměť:** Často můžeš vyměnit paměť za rychlost (memoizace, lookup tabulky).

2. **HashSet pro duplicity:** Místo O(n²) naivního řešení použij HashSet pro O(n):
   ```csharp
   bool MaDuplicity(int[] pole)
   {
       HashSet<int> videne = new HashSet<int>();
       foreach (int x in pole)
           if (!videne.Add(x)) return true;
       return false;
   }
   ```

3. **Amortizovaná složitost:** `List.Add()` je O(1) amortizovaně, i když občas je O(n) při realokaci.

4. **Logaritmus v praxi:** O(log n) pro miliardu prvků = ~30 kroků. Proto jsou stromy a binární vyhledávání tak užitečné!

5. **Kdy na složitosti záleží:** Pro 100 prvků je jedno, jestli je to O(n) nebo O(n²). Pro milion prvků je to rozdíl mezi sekundou a dnem.

---

## 🎯 Quick Reference pro maturitu

```
O(1)      → Konstantní   → přístup k poli, stack/queue operace
O(log n)  → Logaritmický → binární vyhledávání, vyvážené stromy
O(n)      → Lineární     → průchod polem, hledání maxima
O(n log n)→ Linearitmický→ efektivní třídění (Merge, Quick, Heap)
O(n²)     → Kvadratický  → vnořené cykly, Bubble/Selection sort
O(2ⁿ)     → Exponenciální→ naivní rekurze, všechny podmnožiny
```

**Vždy uveď:**
1. Jaká je složitost
2. Vzhledem k čemu (co je n)
3. Jestli je to nejhorší/průměrný případ

---

*Zpracováno: 28. prosince 2024*
