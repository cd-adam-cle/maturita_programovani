# Zápisky: Otázka č. 14 - Lineární a binární vyhledávání. Vyhledávací stromy.

---

## Obsah

1. [Vyhledávání jako algoritmický problém](#1-vyhledávání-jako-algoritmický-problém)
2. [Lineární vyhledávání](#2-lineární-vyhledávání)
3. [Binární vyhledávání](#3-binární-vyhledávání)
4. [Porovnání lineárního a binárního vyhledávání](#4-porovnání-lineárního-a-binárního-vyhledávání)
5. [Příklady ze života](#5-příklady-ze-života)
6. [Binární strom a binární vyhledávací strom (BVS)](#6-binární-strom-a-binární-vyhledávací-strom-bvs)
7. [Operace BVS](#7-operace-bvs)
8. [Složitost operací BVS](#8-složitost-operací-bvs)
9. [Vyváženost a samovyvažující stromy](#9-vyváženost-a-samovyvažující-stromy)
10. [AVL stromy](#10-avl-stromy)
11. [Red-Black stromy a B-stromy](#11-red-black-stromy-a-b-stromy)
12. [Alternativy: hashování a další](#12-alternativy-hashování-a-další)
13. [Maturitní chytáky](#13-maturitní-chytáky)
14. [Klíčové pojmy](#14-klíčové-pojmy)

---

## 1. Vyhledávání jako algoritmický problém

Vyhledávání patří k nejzákladnějším operacím v informatice. Formálně: máme **kolekci prvků** a **vyhledávací klíč** `k`, hledáme buď konkrétní prvek s daným klíčem, nebo všechny prvky splňující predikát.

### Klasifikace problémů vyhledávání

| Typ vyhledávání | Příklad | Typická složitost |
|----------------|---------|-------------------|
| Přesná shoda (exact match) | "Najdi prvek = 42" | O(log n) nebo O(1) |
| Rozsah (range query) | "Najdi prvky 10 ≤ x ≤ 50" | O(log n + k) |
| Nejbližší soused (nearest neighbor) | "Najdi prvek nejbližší k 42" | O(log n) ve stromě |
| Substring search | "Najdi 'abc' v textu" | O(n + m) (KMP, Rabin-Karp) |
| Vyhledávání podle množinového predikátu | "Najdi všechny větší než 50" | O(n) nebo O(log n + k) |

### Faktory ovlivňující volbu algoritmu

- **Velikost dat** - pro malá data je režie složitějších struktur kontraproduktivní.
- **Statická vs. dynamická data** - jestli se kolekce mění (přidávání/mazání).
- **Setřídění dat** - některé algoritmy vyžadují setříděný vstup.
- **Frekvence operací** - jednorázové vs. opakované vyhledávání.
- **Paměťová omezení** - in-memory vs. disk-based struktury.
- **Cache lokalita** - důležitá pro výkon na moderním hardwaru.

### Tři kanonické přístupy

1. **Lineární průchod** v nesetříděném poli - O(n).
2. **Binární vyhledávání** v setříděném poli - O(log n).
3. **Binární vyhledávací strom (BVS)** - O(log n) pro vyvážený strom, podporuje i dynamické vkládání/mazání.

---

## 2. Lineární vyhledávání

### Princip

Lineární vyhledávání (sekvenční vyhledávání) prochází kolekci **prvek po prvku** od začátku do konce a porovnává s hledaným klíčem. Funguje na **libovolné** kolekci - nevyžaduje setřídění.

### Pseudokód

```
LINEAR_SEARCH(pole, klic):
   for i = 0 to délka(pole) - 1:
      if pole[i] == klic:
         return i
   return -1
```

### Implementace v C#

```csharp
static int LinearniHledani(int[] pole, int hledany)
{
    for (int i = 0; i < pole.Length; i++)
    {
        if (pole[i] == hledany)
            return i;
    }
    return -1;
}
```

### Generická varianta

```csharp
static int LinearniHledani<T>(T[] pole, T hledany, IEqualityComparer<T> cmp = null)
{
    cmp ??= EqualityComparer<T>.Default;
    for (int i = 0; i < pole.Length; i++)
        if (cmp.Equals(pole[i], hledany))
            return i;
    return -1;
}
```

### LINQ varianta

```csharp
int index = Array.IndexOf(pole, hledany);          // built-in
int? prvek = pole.FirstOrDefault(x => x == hledany);
bool obsahuje = pole.Contains(hledany);
```

### Složitost

| Případ | Složitost | Vysvětlení |
|--------|-----------|------------|
| **Nejlepší** | O(1) | Klíč je první prvek |
| **Průměrný** | O(n) | Průměrně n/2 porovnání |
| **Nejhorší** | O(n) | Klíč je poslední nebo není přítomen |

### Sentinel optimalizace

Vyhneme se kontrole `i < n` v každé iteraci tím, že na konec pole umístíme sám hledaný prvek (sentinel):

```csharp
static int LinearWithSentinel(int[] pole, int hledany)
{
    int n = pole.Length;
    int posledni = pole[n - 1];
    pole[n - 1] = hledany;                          // sentinel

    int i = 0;
    while (pole[i] != hledany) i++;

    pole[n - 1] = posledni;                          // obnov původní

    if (i < n - 1 || posledni == hledany)
        return i;
    return -1;
}
```

V praxi se moc nepoužívá, ale akademicky zajímavé.

### Self-organizing search

Optimalizace pro opakovaná hledání: po každém úspěšném nalezení posuneme prvek **na začátek** (Move-To-Front) nebo dopředu o jednu pozici (Transpose). Často hledané prvky se sám sebe "vyzobou" na začátek a další hledání je rychlejší.

---

## 3. Binární vyhledávání

### Princip

Binární vyhledávání pracuje **pouze na setříděném poli**. Využívá strategie **Rozděl a panuj**:

1. Zjisti **prostřední prvek** pole.
2. Pokud je roven hledanému klíči - hotovo.
3. Pokud je hledaný klíč **menší** - hledej v **levé** polovině.
4. Pokud je hledaný klíč **větší** - hledej v **pravé** polovině.
5. Opakuj, dokud máš co prohledávat.

V každém kroku **vyloučíme polovinu** zbývajících prvků, takže po `log₂ n` krocích zbývá maximálně jeden prvek.

### Pseudokód (iterativní)

```
BINARY_SEARCH(pole, klic):
   left = 0
   right = délka(pole) - 1

   while left <= right:
      mid = left + (right - left) / 2
      if pole[mid] == klic:
         return mid
      else if pole[mid] < klic:
         left = mid + 1
      else:
         right = mid - 1

   return -1
```

### Implementace v C# (iterativní)

```csharp
static int BinarniHledani(int[] pole, int hledany)
{
    int levy = 0;
    int pravy = pole.Length - 1;

    while (levy <= pravy)
    {
        int stred = levy + (pravy - levy) / 2;     // bezpečné proti overflow

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

### Rekurzivní varianta

```csharp
static int BinarniHledaniRek(int[] pole, int hledany, int levy, int pravy)
{
    if (levy > pravy) return -1;

    int stred = levy + (pravy - levy) / 2;

    if (pole[stred] == hledany) return stred;
    if (pole[stred] < hledany)
        return BinarniHledaniRek(pole, hledany, stred + 1, pravy);
    else
        return BinarniHledaniRek(pole, hledany, levy, stred - 1);
}
```

### Vizualizace pro `[3, 7, 15, 28, 42, 66, 91]`, hledáme 15

```
Iterace 1: levy=0, pravy=6, stred=3
   pole[3] = 28, 28 > 15 → hledej vlevo
   levy=0, pravy=2

Iterace 2: levy=0, pravy=2, stred=1
   pole[1] = 7, 7 < 15 → hledej vpravo
   levy=2, pravy=2

Iterace 3: levy=2, pravy=2, stred=2
   pole[2] = 15 = hledany → NALEZENO!
   return 2
```

```
Indexy:  0   1    2    3    4    5    6
Pole:   [3,  7,  15,  28,  42,  66,  91]
                       ^ start mid

Iter 1:       [3, 7, 15] | (28 vyloučeno) | -
                  ^ mid

Iter 2:                  [15] | -
                          ^ mid = FOUND
```

### Variace binárního vyhledávání

#### Lower bound (najít první výskyt nebo pozici pro vložení)

Vrátí index **prvního** prvku ≥ klíč. Užitečné pro:
- Vkládání do setříděného pole.
- Hledání rozsahu duplicit (společně s upper bound).

```csharp
static int LowerBound(int[] pole, int klic)
{
    int levy = 0, pravy = pole.Length;             // pozor: pravy = n, ne n-1

    while (levy < pravy)
    {
        int stred = levy + (pravy - levy) / 2;
        if (pole[stred] < klic)
            levy = stred + 1;
        else
            pravy = stred;
    }
    return levy;
}
```

#### Upper bound (poslední výskyt + 1)

Vrátí index **prvního** prvku > klíč.

```csharp
static int UpperBound(int[] pole, int klic)
{
    int levy = 0, pravy = pole.Length;

    while (levy < pravy)
    {
        int stred = levy + (pravy - levy) / 2;
        if (pole[stred] <= klic)
            levy = stred + 1;
        else
            pravy = stred;
    }
    return levy;
}
```

#### Počet duplicit klíče

```csharp
int pocetDuplicit = UpperBound(pole, klic) - LowerBound(pole, klic);
```

### Built-in v .NET

```csharp
int index = Array.BinarySearch(pole, 42);
// Vrátí: index pokud najde
//        bitový doplněk pozice, kam patří (~index) pokud nenajde
//        např. -1 znamená, že patří na pozici 0

int kamPatri = index < 0 ? ~index : index;
```

`List<T>.BinarySearch()` funguje stejně.

### Bezpečný výpočet středu

```csharp
// PROBLEM: overflow pro velké hodnoty
int stred = (levy + pravy) / 2;                    // může přetéct int

// SPRÁVNĚ
int stred = levy + (pravy - levy) / 2;             // matematicky stejné, bez overflow
```

V Javě byla tato chyba v `java.util.Arrays.binarySearch` 9 let nezdetekována.

### Složitost binárního vyhledávání

| Případ | Složitost |
|--------|-----------|
| Nejlepší | O(1) - prvek je přímo ve středu |
| Průměrný | O(log n) |
| Nejhorší | O(log n) - prvek není přítomen |

Pro `n = 10⁹` (miliarda prvků) je počet kroků jen `⌈log₂(10⁹)⌉ ≈ 30`. Lineární vyhledávání by potřebovalo miliardu kroků.

### Interpolated search

Pro **rovnoměrně rozdělená data** (např. čísla od 1 do 1000) můžeme místo prostředku odhadnout pozici lineární interpolací:

```csharp
int stred = levy + ((klic - pole[levy]) * (pravy - levy)) / (pole[pravy] - pole[levy]);
```

Průměrná složitost: **O(log log n)** - extrémně rychlé. Worst case stále O(n).

### Exponential search (galloping search)

Pro **neomezené** nebo extrémně velké pole najdeme hrubě interval, kde klíč leží, zdvojnásobováním rozsahu:

```csharp
static int ExponentialSearch(int[] pole, int klic)
{
    if (pole[0] == klic) return 0;

    int i = 1;
    while (i < pole.Length && pole[i] <= klic) i *= 2;

    return BinarniHledani(pole, klic /* levy = i/2, pravy = Min(i, n-1) */);
}
```

Složitost O(log i), kde i je pozice klíče. Užitečné, když je klíč pravděpodobně blízko začátku.

---

## 4. Porovnání lineárního a binárního vyhledávání

### Tabulka složitostí

| Algoritmus | Best | Avg | Worst | Vyžaduje setřídění | Funguje na linked listu |
|-----------|------|-----|-------|---------------------|--------------------------|
| Lineární | O(1) | O(n) | O(n) | Ne | Ano |
| Binární | O(1) | O(log n) | O(log n) | Ano | Ne (vyžaduje random access) |

### Konkrétní čísla

| n | Lineární (worst) | Binární (worst) | Poměr |
|---|------------------|-----------------|-------|
| 10 | 10 | 4 | 2,5× |
| 100 | 100 | 7 | 14× |
| 1 000 | 1 000 | 10 | 100× |
| 10 000 | 10 000 | 14 | 700× |
| 1 000 000 | 1 000 000 | 20 | 50 000× |
| 1 000 000 000 | 1 000 000 000 | 30 | 33 000 000× |

### Kdy je lineární vyhledávání lepší

1. **Pole není setříděné** - binární nelze použít bez předchozího setřídění.
2. **Malé pole (< ~20 prvků)** - režie binárního (skoky, branch prediction) se nevyplatí; lineární vyhraje díky cache.
3. **Jednorázové vyhledávání v nesetříděných datech** - setřídění O(n log n) je dražší než jedno lineární O(n).
4. **Linked list nebo jiná struktura bez random access** - binární vyhledávání potřebuje O(1) přístup k libovolnému indexu.
5. **Hledání podle predikátu** (ne přesný klíč) - binární funguje jen pro porovnatelné klíče.

### Kdy je binární vyhledávání lepší

1. **Setříděné pole + opakovaná vyhledávání** - každé hledání O(log n).
2. **Velká data** - asymptotický rozdíl O(n) vs O(log n) je drtivý.
3. **Pole už setříděné z jiných důvodů** (např. časové řady, databázové indexy).

### Rozhodovací pravidlo (break-even point)

Pokud děláme `k` vyhledávání v nesetříděném poli o velikosti `n`:

```
Lineární: k · O(n) = O(k · n)
Binární:  setřídění O(n log n) + k · O(log n) = O((n + k) log n)

Break-even (kdy je binární výhodnější):
   k · n  >  (n + k) · log n
```

Pro velké `k` se vždy vyplatí setřídit a hledat binárně.

---

## 5. Příklady ze života

### Lineární vyhledávání

- **Klíče v kapse** - máš 5 klíčů, vytahuješ je postupně.
- **Auto na parkovišti** - chodíš řadu po řadě.
- **Ctrl+F v dokumentu** - editor čte text sekvenčně od začátku.
- **Sociální síť: najdi přítele Jana mezi 50 přáteli** - projdeš seznam.
- **Vyhledávání v `List<T>` nebo `LinkedList<T>` v C#**.

### Binární vyhledávání

- **Slovník** - otevřeš zhruba uprostřed, zúžíš podle abecedy.
- **Telefonní seznam** - hledáš podle příjmení.
- **Hra "hádej číslo 1-100"** - vždy tipuješ střed intervalu.
- **Git bisect** - hledání commitu, který způsobil bug.
- **Databáze (B-tree indexy)** - vyhledávání záznamu podle primárního klíče.
- **Logaritmická tabulka** - hledání hodnoty funkce.

### Binární vyhledávací stromy (BVS)

- **Dynamický slovník** - kontakty, kde přidáváš/mažeš čísla a hledáš.
- **Databázové indexy** (B-tree, varianta BVS).
- **Mapy a sety v C#**: `SortedDictionary<K,V>`, `SortedSet<T>` (interně Red-Black tree).
- **Souborové systémy** - hierarchická struktura adresářů.
- **Skladiště v Linuxu** - kernel scheduler CFS (Completely Fair Scheduler) používá red-black tree.

### Rozhodovací tabulka

| Situace | Použij | Proč |
|---------|--------|------|
| Málo dat (< 20 prvků) | Lineární | Jednoduché, cache-friendly |
| Setříděné statické pole, hodně hledání | Binární | O(log n) za každé hledání |
| Dynamická data, časté vkládání/mazání | BVS (Red-Black, AVL) | O(log n) pro všechny operace |
| Jen vyhledávání podle klíče (bez řazení) | Hash tabulka | O(1) průměrně |
| Klíče v rozsahu (range query) | BVS | Setříděný průchod přes rozsah |
| Disková data | B-tree | Optimalizováno pro disky |
| Substring v textu | KMP, Rabin-Karp | Specializované algoritmy |

---

## 6. Binární strom a binární vyhledávací strom (BVS)

### Binární strom

**Binární strom** je hierarchická struktura, kde každý uzel má **nejvýše 2 potomky** - obvykle označované jako **levý** a **pravý** syn. Speciálním případem prázdného binárního stromu je strom bez kořene.

```
        [A]          ← kořen (root)
       /   \
     [B]   [C]       ← vnitřní uzly
     / \     \
   [D] [E]   [F]     ← listy (nemají potomky)
```

**Klíčové pojmy:**
- **Kořen (root)** - vrchní uzel bez rodiče.
- **List (leaf)** - uzel bez potomků.
- **Vnitřní uzel** - uzel s alespoň jedním potomkem.
- **Hloubka uzlu** - vzdálenost od kořene (počet hran).
- **Výška stromu** - nejdelší cesta od kořene k listu.
- **Úroveň** - množina uzlů ve stejné hloubce.
- **Podstrom** - strom tvořený libovolným uzlem a všemi jeho potomky.
- **Stupeň uzlu** - počet jeho přímých potomků (v binárním stromu 0, 1, nebo 2).

### Binární vyhledávací strom (BVS)

**BVS** (Binary Search Tree) je binární strom splňující **invariant BVS**:

> Pro každý uzel `u` platí:
> - Všechny hodnoty v **levém podstromu** uzlu `u` jsou **menší** než hodnota uzlu `u`.
> - Všechny hodnoty v **pravém podstromu** uzlu `u` jsou **větší** než hodnota uzlu `u`.

```
         [10]
        /    \
      [5]   [15]
      / \    / \
    [3] [7] [12] [20]
```

**Důležité:** Pravidlo platí pro **CELÝ podstrom**, ne jen pro přímé potomky.

```
       [10]
       /  \
     [5]  [15]
     / \
   [3] [12]    ← NENÍ BVS! 12 > 10, ale je v levém podstromu kořene
```

### In-order průchod BVS dává setříděnou posloupnost

Klíčová vlastnost BVS: **in-order průchod** (levý → uzel → pravý) navštíví uzly v **rostoucím pořadí**.

```
BVS:           In-order:    3, 5, 7, 10, 12, 15, 20

       [10]
       /  \
     [5]  [15]
     / \   / \
   [3] [7] [12] [20]
```

To znamená, že BVS implicitně reprezentuje **setříděnou posloupnost**.

### Implementace uzlu

```csharp
class Node
{
    public int Key;
    public Node Left;
    public Node Right;

    public Node(int key)
    {
        Key = key;
    }
}
```

Generická verze:

```csharp
class Node<TKey, TValue> where TKey : IComparable<TKey>
{
    public TKey Key;
    public TValue Value;
    public Node<TKey, TValue> Left;
    public Node<TKey, TValue> Right;
}
```

### Varianty binárního stromu (rekapitulace z Ot. 9)

- **Plný binární strom (full)** - každý uzel má 0 nebo 2 potomky.
- **Úplný binární strom (complete)** - všechny úrovně plné kromě poslední, ta zleva doprava.
- **Perfektní binární strom (perfect)** - všechny vnitřní uzly mají 2 potomky a všechny listy jsou na stejné úrovni.
- **Vyvážený binární strom** - výška levého a pravého podstromu se liší max o 1 (pro všechny uzly).
- **Degenerovaný strom** - každý uzel má jen jednoho potomka (vypadá jako linked list).

---

## 7. Operace BVS

### Find (vyhledání klíče)

**Princip:** Začni v kořeni. Pokud klíč < uzel, jdi vlevo. Pokud klíč > uzel, jdi vpravo. Pokud klíč = uzel, vrať uzel. Pokud narazíš na null, klíč není v BVS.

#### Iterativní verze

```csharp
Node Find(Node root, int key)
{
    Node current = root;
    while (current != null)
    {
        if (key == current.Key) return current;
        current = key < current.Key ? current.Left : current.Right;
    }
    return null;
}
```

#### Rekurzivní verze

```csharp
Node Find(Node node, int key)
{
    if (node == null) return null;
    if (key == node.Key) return node;
    return key < node.Key
        ? Find(node.Left, key)
        : Find(node.Right, key);
}
```

### Min a Max

V BVS je **minimum** vlevo úplně dole, **maximum** vpravo úplně dole.

```csharp
Node FindMin(Node node)
{
    while (node.Left != null) node = node.Left;
    return node;
}

Node FindMax(Node node)
{
    while (node.Right != null) node = node.Right;
    return node;
}
```

### Successor a Predecessor

**In-order successor** uzlu `u` je nejmenší prvek větší než `u.Key`. Případy:
1. Pokud `u` má pravý podstrom → minimum pravého podstromu.
2. Pokud `u` nemá pravý podstrom → první předek, kde je `u` v levém podstromu.

```csharp
Node Successor(Node u)
{
    if (u.Right != null)
        return FindMin(u.Right);

    Node ancestor = root;
    Node successor = null;
    while (ancestor != u)
    {
        if (u.Key < ancestor.Key)
        {
            successor = ancestor;
            ancestor = ancestor.Left;
        }
        else
            ancestor = ancestor.Right;
    }
    return successor;
}
```

### Insert

**Princip:** Hledej jako Find. Až narazíš na `null`, vlož nový uzel na to místo.

```csharp
Node Insert(Node node, int key)
{
    if (node == null) return new Node(key);

    if (key < node.Key)
        node.Left = Insert(node.Left, key);
    else if (key > node.Key)
        node.Right = Insert(node.Right, key);
    // key == node.Key → ignoruj nebo nahraď podle politiky

    return node;
}

// Volání: root = Insert(root, 8);
```

Iterativní verze:

```csharp
void InsertIterative(int key)
{
    if (root == null) { root = new Node(key); return; }

    Node curr = root, parent = null;
    while (curr != null)
    {
        parent = curr;
        if (key == curr.Key) return;               // duplikát
        curr = key < curr.Key ? curr.Left : curr.Right;
    }

    if (key < parent.Key) parent.Left = new Node(key);
    else parent.Right = new Node(key);
}
```

### Delete - tři případy

Mazání je nejsložitější operace, protože musíme zachovat invariant BVS. Existují tři případy:

#### Případ 1: List (0 potomků)

Stačí ho odstranit.

```
Mažeme 3:
       [10]                 [10]
       /  \                 /  \
     [5]  [15]    →      [5]  [15]
     /                       \
   [3]                       (3 pryč)
```

#### Případ 2: Jeden potomek

Nahraď uzel jeho jediným potomkem.

```
Mažeme 5 (má jen pravého syna 7):
       [10]                 [10]
       /  \                 /  \
     [5]  [15]    →       [7]  [15]
       \
       [7]
```

#### Případ 3: Dva potomci

Nahraď uzel jeho **in-order successorem** (= minimum pravého podstromu) nebo **predecessorem** (= maximum levého podstromu). Successor/predecessor má max jeden potomek → použijeme případ 1 nebo 2.

```
Mažeme 10 (in-order successor = 12):
       [10]                 [12]
       /  \                 /  \
     [5]  [15]    →       [5]  [15]
     / \   /  \           / \    \
   [3] [7] [12] [20]    [3] [7]  [20]
```

#### Implementace

```csharp
Node Delete(Node node, int key)
{
    if (node == null) return null;

    if (key < node.Key)
        node.Left = Delete(node.Left, key);
    else if (key > node.Key)
        node.Right = Delete(node.Right, key);
    else
    {
        // našli jsme uzel k smazání

        if (node.Left == null) return node.Right;     // případ 1 nebo 2 (pouze pravý)
        if (node.Right == null) return node.Left;     // případ 2 (pouze levý)

        // případ 3: dva potomci
        Node successor = FindMin(node.Right);
        node.Key = successor.Key;                     // zkopíruj klíč
        node.Right = Delete(node.Right, successor.Key); // smaž successora
    }

    return node;
}
```

### Průchody BVS (traversals - viz Ot. 9)

- **In-order (LNR):** levý → uzel → pravý. Pro BVS dává **setříděnou** posloupnost.
- **Pre-order (NLR):** uzel → levý → pravý. Užitečné pro kopírování stromu.
- **Post-order (LRN):** levý → pravý → uzel. Užitečné pro mazání stromu (mažeme listy první).
- **Level-order (BFS):** po úrovních. Využívá frontu.

In-order průchod (rekurzivní):

```csharp
IEnumerable<int> InOrder(Node node)
{
    if (node == null) yield break;
    foreach (int x in InOrder(node.Left)) yield return x;
    yield return node.Key;
    foreach (int x in InOrder(node.Right)) yield return x;
}
```

### Kompletní třída BinarySearchTree

```csharp
public class BinarySearchTree<T> where T : IComparable<T>
{
    private Node<T> root;

    public bool Contains(T key) => Find(root, key) != null;
    public void Insert(T key)  { root = Insert(root, key); }
    public void Delete(T key)  { root = Delete(root, key); }
    public T Min()             => FindMin(root).Key;
    public T Max()             => FindMax(root).Key;
    public IEnumerable<T> InOrder() => InOrderRec(root);

    private Node<T> Find(Node<T> n, T key)
    {
        if (n == null) return null;
        int cmp = key.CompareTo(n.Key);
        if (cmp == 0) return n;
        return cmp < 0 ? Find(n.Left, key) : Find(n.Right, key);
    }

    private Node<T> Insert(Node<T> n, T key)
    {
        if (n == null) return new Node<T>(key);
        int cmp = key.CompareTo(n.Key);
        if (cmp < 0) n.Left  = Insert(n.Left, key);
        else if (cmp > 0) n.Right = Insert(n.Right, key);
        return n;
    }

    private Node<T> Delete(Node<T> n, T key)
    {
        if (n == null) return null;
        int cmp = key.CompareTo(n.Key);
        if (cmp < 0) n.Left  = Delete(n.Left, key);
        else if (cmp > 0) n.Right = Delete(n.Right, key);
        else
        {
            if (n.Left == null) return n.Right;
            if (n.Right == null) return n.Left;
            Node<T> succ = FindMin(n.Right);
            n.Key = succ.Key;
            n.Right = Delete(n.Right, succ.Key);
        }
        return n;
    }

    private Node<T> FindMin(Node<T> n)
    {
        while (n.Left != null) n = n.Left;
        return n;
    }

    private Node<T> FindMax(Node<T> n)
    {
        while (n.Right != null) n = n.Right;
        return n;
    }

    private IEnumerable<T> InOrderRec(Node<T> n)
    {
        if (n == null) yield break;
        foreach (T x in InOrderRec(n.Left)) yield return x;
        yield return n.Key;
        foreach (T x in InOrderRec(n.Right)) yield return x;
    }
}

class Node<T>
{
    public T Key;
    public Node<T> Left, Right;
    public Node(T key) { Key = key; }
}
```

---

## 8. Složitost operací BVS

Všechny základní operace (Find, Insert, Delete, Min, Max) procházejí strom od kořene dolů po jedné cestě. Jejich složitost je proto **O(h)**, kde `h` je **výška stromu**.

### Tabulka složitostí

| Operace | Vyvážený BVS (h = log n) | Degenerovaný BVS (h = n) |
|---------|--------------------------|--------------------------|
| Find | O(log n) | O(n) |
| Min / Max | O(log n) | O(n) |
| Insert | O(log n) | O(n) |
| Delete | O(log n) | O(n) |
| Successor / Predecessor | O(log n) | O(n) |
| In-order průchod | O(n) | O(n) |

### Paměťová složitost

**O(n)** - každý prvek = jeden uzel. Každý uzel zabírá:
- klíč (např. 4 B pro int),
- 2 pointery na potomky (např. 16 B na 64-bit systému),
- případně další metadata (rodič, barva u red-black, výška u AVL).

Pro `n = 10⁶` prvků: cca 24 MB jen na pointery (oproti 4 MB v poli).

### Závislost výšky na pořadí vkládání

| Pořadí vkládání | Výsledný strom | Výška | Find |
|----------------|----------------|-------|------|
| Náhodné | Skoro vyvážený | ~1,4 log n | O(log n) |
| Setříděné (1, 2, 3, ...) | Degenerovaný | n | O(n) |
| Zpětně setříděné | Degenerovaný | n | O(n) |
| "Pyramidové" (50, 25, 75, ...) | Vyvážený | log n | O(log n) |

### Konkrétní příklad

Pro n = 1000 prvků:

| Strom | Výška | Operace |
|-------|-------|---------|
| Vyvážený | ~10 | 10 porovnání |
| Degenerovaný | 999 | 999 porovnání |

Rozdíl je 100×. Pro n = 10⁶ to je 50 000×.

---

## 9. Vyváženost a samovyvažující stromy

### Co znamená "vyvážený"

Existuje několik definic, lišících se mírou striktnosti:

- **Výškově vyvážený (AVL):** Výška levého a pravého podstromu se liší max o 1.
- **Váhově vyvážený:** Počet uzlů v levém a pravém podstromu se liší nejvýše o konstantní faktor.
- **Red-Black property:** Cesty od kořene k listům se liší max 2× v délce.

### Balance factor

Pro každý uzel:

```
bf(uzel) = výška(levý podstrom) - výška(pravý podstrom)
```

- **bf = 0** - dokonale vyvážený.
- **bf ∈ {-1, 0, +1}** - akceptovatelně vyvážený (AVL).
- **|bf| ≥ 2** - nevyvážený, potřebuje rotaci.

### Proč BVS degenerují

Klasický BVS bez samoopravy degeneruje, když:
- Vkládáme prvky v setříděném pořadí (1, 2, 3, ...) → strom roste jen vpravo.
- Vkládáme prvky v opačném pořadí → strom roste jen vlevo.
- Po sérii smazání vznikne nevyvážený tvar.

V praxi se BVS proto **málokdy používá bez samovyvažování**. Standardní knihovny (např. .NET `SortedSet`, Java `TreeSet`, C++ `std::set`) používají Red-Black tree nebo AVL.

### Samovyvažující stromy

Samovyvažující strom automaticky udržuje výšku O(log n) po každé operaci pomocí **rotací** a/nebo **rebalanc**. Hlavní zástupci:

| Strom | Vyváženost | Použití |
|-------|-----------|---------|
| **AVL strom** | Striktně výškově (max 1) | Akademický, hodně rotací |
| **Red-Black tree** | Slabě (max 2× v délce) | Standardní v knihovnách (.NET, Java, C++ STL, Linux kernel) |
| **Splay tree** | Amortizovaně | Často přistupované prvky blízko kořene |
| **Treap** | Randomizovaný | Jednoduchá implementace |
| **B-tree** | M-cestný strom | Databáze a souborové systémy |
| **B+-tree** | Listy s daty | Databázové indexy |

### Rotace

Rotace je lokální transformace, která zachovává invariant BVS a mění výšku podstromu. Existují dva typy:

#### Pravá rotace

```
Před:                 Po:
       y                   x
      / \                 / \
     x   C      →        A   y
    / \                     / \
   A   B                   B   C
```

#### Levá rotace

```
Před:                 Po:
     x                       y
    / \                     / \
   A   y          →        x   C
      / \                 / \
     B   C               A   B
```

```csharp
Node RotateRight(Node y)
{
    Node x = y.Left;
    y.Left = x.Right;
    x.Right = y;
    return x;
}

Node RotateLeft(Node x)
{
    Node y = x.Right;
    x.Right = y.Left;
    y.Left = x;
    return y;
}
```

Rotace běží v **O(1)** - pouhé přepojení pár ukazatelů.

---

## 10. AVL stromy

### Historie a definice

**AVL strom** je první samovyvažující BVS, vymyšlený v roce 1962 sovětskými matematiky **Adelson-Velsky** a **Landis** (odtud zkratka). Invariant:

> Pro každý uzel `u` platí: |bf(u)| ≤ 1.

Tato přísná podmínka zaručuje výšku stromu **O(log n)** přesněji `≤ 1,44 · log₂(n + 2)`.

### Operace

**Insert** a **Delete** se provádí jako v klasickém BVS, ale **po každé operaci** se aktualizuje výška uzlů na cestě zpět ke kořeni a pokud se `|bf| > 1`, provede se **rotace**.

### Čtyři typy nevyvážení a jejich rotace

| Případ | Vzor | Rotace |
|--------|------|--------|
| **LL** (levý-levý) | Nový uzel v **levém podstromu levého potomka** | Jedna **pravá** rotace |
| **RR** (pravý-pravý) | Nový uzel v **pravém podstromu pravého potomka** | Jedna **levá** rotace |
| **LR** (levý-pravý) | Nový uzel v **pravém podstromu levého potomka** | **Levá** rotace na potomka, pak **pravá** na uzel |
| **RL** (pravý-levý) | Nový uzel v **levém podstromu pravého potomka** | **Pravá** rotace na potomka, pak **levá** na uzel |

### Příklad: LL rotace

```
Vložením 5, 4, 3 v tomto pořadí:

   [5]              [5]               [4]
                    /                /   \
                  [4]    bf=2    →  [3]  [5]
                  /
                [3]

(po vložení 3 je |bf(5)| = 2 → pravá rotace)
```

### Příklad: LR rotace (dvojitá)

```
Vložením 5, 3, 4:

   [5]              [5]            [5]                [4]
                    /              /                 /   \
                  [3]    →       [4]      →       [3]   [5]
                    \            /
                    [4]        [3]

Krok 1: levá rotace na [3] (přesun 4 nahoru)
Krok 2: pravá rotace na [5] (přesun 4 na kořen)
```

### Implementace AVL Insert

```csharp
class AvlNode<T> where T : IComparable<T>
{
    public T Key;
    public AvlNode<T> Left, Right;
    public int Height = 1;
    public AvlNode(T key) { Key = key; }
}

class AvlTree<T> where T : IComparable<T>
{
    private AvlNode<T> root;

    private int Height(AvlNode<T> n) => n?.Height ?? 0;
    private int BalanceFactor(AvlNode<T> n) => Height(n.Left) - Height(n.Right);
    private void UpdateHeight(AvlNode<T> n)
        => n.Height = 1 + Math.Max(Height(n.Left), Height(n.Right));

    private AvlNode<T> RotateRight(AvlNode<T> y)
    {
        AvlNode<T> x = y.Left;
        y.Left = x.Right;
        x.Right = y;
        UpdateHeight(y);
        UpdateHeight(x);
        return x;
    }

    private AvlNode<T> RotateLeft(AvlNode<T> x)
    {
        AvlNode<T> y = x.Right;
        x.Right = y.Left;
        y.Left = x;
        UpdateHeight(x);
        UpdateHeight(y);
        return y;
    }

    public void Insert(T key) => root = Insert(root, key);

    private AvlNode<T> Insert(AvlNode<T> n, T key)
    {
        if (n == null) return new AvlNode<T>(key);

        int cmp = key.CompareTo(n.Key);
        if (cmp < 0) n.Left  = Insert(n.Left, key);
        else if (cmp > 0) n.Right = Insert(n.Right, key);
        else return n;                              // duplikát

        UpdateHeight(n);
        int bf = BalanceFactor(n);

        // LL
        if (bf > 1 && key.CompareTo(n.Left.Key) < 0)
            return RotateRight(n);

        // RR
        if (bf < -1 && key.CompareTo(n.Right.Key) > 0)
            return RotateLeft(n);

        // LR
        if (bf > 1 && key.CompareTo(n.Left.Key) > 0)
        {
            n.Left = RotateLeft(n.Left);
            return RotateRight(n);
        }

        // RL
        if (bf < -1 && key.CompareTo(n.Right.Key) < 0)
        {
            n.Right = RotateRight(n.Right);
            return RotateLeft(n);
        }

        return n;
    }
}
```

### Složitost AVL

| Operace | Složitost |
|---------|-----------|
| Find | O(log n) |
| Insert | O(log n) (max 2 rotace) |
| Delete | O(log n) (max O(log n) rotací po cestě nahoru) |

Insert vyžaduje **maximálně 1 jednoduchou nebo 1 dvojitou rotaci** - po jedné rotaci je strom opět vyvážený. Delete může vyžadovat až O(log n) rotací (rotace se propaguje vzhůru).

---

## 11. Red-Black stromy a B-stromy

### Red-Black tree

**Red-Black tree** je samovyvažující BVS, kde každý uzel má **barvu** (červenou nebo černou). Invarianty:

1. Každý uzel je červený nebo černý.
2. Kořen je černý.
3. Listy (NIL) jsou černé.
4. Červený uzel nesmí mít červeného potomka (žádné dvě červené hrany za sebou).
5. Z každého uzlu vede stejný počet černých uzlů ke všem potomkům-listům.

Tyto invarianty zaručují, že nejdelší cesta od kořene k listu je nejvýše **2×** delší než nejkratší → výška je O(log n).

**Výhody oproti AVL:**
- **Méně rotací** při Insert/Delete (max 2 rotace pro Insert, max 3 pro Delete).
- **Rychlejší zápisové operace** - v praxi rychlejší než AVL pro frekventovaná Insert/Delete.

**Nevýhody:**
- **Méně vyvážený** než AVL → Find je v průměru o trochu pomalejší.

**Použití:**
- .NET: `SortedDictionary<K,V>`, `SortedSet<T>`.
- Java: `TreeMap`, `TreeSet`.
- C++ STL: `std::map`, `std::set`.
- Linux kernel: Completely Fair Scheduler (CFS), epoll.

### B-tree

**B-tree** je m-cestný strom (každý uzel může mít až `m` potomků), navržený speciálně pro **diskové úložiště** a velké datové sady. Charakteristiky:

- Každý uzel obsahuje **více klíčů** (typicky desítky až stovky) a **více ukazatelů**.
- Strom je velmi **mělký** - i pro miliardy záznamů má hloubku jen několik úrovní.
- Optimalizováno pro **block I/O** - jeden uzel se vejde do jednoho diskového bloku.

**Použití:**
- **Databázové indexy** - téměř všechny RDBMS (PostgreSQL, MySQL InnoDB, Oracle, SQL Server).
- **Souborové systémy** - NTFS, HFS+, ext4 (s HTree variantou), Btrfs.

**B+-tree** je varianta, kde data jsou jen v listech (vnitřní uzly mají jen klíče-ukazatele) a listy jsou propojené do linked listu - umožňuje rychlý **range query**.

### Trie (prefix tree)

Specializovaný strom pro vyhledávání **řetězců**:
- Každý uzel reprezentuje jeden znak.
- Cesta od kořene k listu je řetězec.
- Find má složitost O(|řetězec|), nezávisle na počtu uložených řetězců.

Použití: autocomplete, slovníky pro kontrolu pravopisu, IP routing.

---

## 12. Alternativy: hashování a další

### Hash tabulka

Pro **pouze vyhledávání podle klíče** (bez požadavku na řazení) je často nejlepší **hash tabulka**:

| Operace | Hash tabulka | BVS |
|---------|-------------|-----|
| Find | O(1) průměrně, O(n) worst | O(log n) |
| Insert | O(1) průměrně | O(log n) |
| Delete | O(1) průměrně | O(log n) |
| Range query | O(n) - musí projít vše | O(log n + k) |
| Setříděný průchod | O(n log n) - musí setřídit | O(n) |
| Min / Max | O(n) | O(log n) |
| Successor / Predecessor | Nepodporováno | O(log n) |

### V C#

```csharp
Dictionary<string, int> dict = new();              // hash tabulka, neseřazené
SortedDictionary<string, int> sorted = new();      // red-black tree, seřazené
HashSet<int> set = new();                          // hash množina
SortedSet<int> sortedSet = new();                  // red-black množina
```

### Skip list

Pravděpodobnostní datová struktura s úrovněmi linked listů. V průměru O(log n) pro všechny operace, jednodušší implementace než AVL/Red-Black. Použití: Redis (sorted sets), LevelDB.

### Bloom filter

Pravděpodobnostní struktura pro test příslušnosti: vrátí **buď "určitě ne", nebo "možná ano"**. Velmi paměťově úsporná, ale s false positives. Použití: detekce duplicit ve streamu, web cache, Bitcoin SPV klienti.

---

## 13. Maturitní chytáky

### Časté chyby

**Binární vyhledávání na nesetříděném poli:**

```csharp
int[] pole = { 5, 2, 8, 1, 9 };                    // NESETŘÍDĚNÉ!
Array.BinarySearch(pole, 8);                       // nedefinované chování
```

**Overflow při výpočtu středu:**

```csharp
// CHYBA pro velké levy + pravy
int stred = (levy + pravy) / 2;

// SPRÁVNĚ
int stred = levy + (pravy - levy) / 2;
```

**BVS invariant jen pro přímé potomky:**

```csharp
// CHYBA - kontrola jen rodič vs děti
bool IsBst(Node n) =>
    (n.Left == null || n.Left.Key < n.Key) &&
    (n.Right == null || n.Right.Key > n.Key);

// SPRÁVNĚ - kontrola s min/max bounds pro celý podstrom
bool IsBst(Node n, int? min, int? max)
{
    if (n == null) return true;
    if (min != null && n.Key <= min) return false;
    if (max != null && n.Key >= max) return false;
    return IsBst(n.Left, min, n.Key) && IsBst(n.Right, n.Key, max);
}
```

**Vkládání duplicitních klíčů:**

Klasický BVS neřeší, co se má stát při duplikátu. Tři politiky:
- Ignorovat (ne-insertnout).
- Přepsat hodnotu (pokud máme klíč → hodnota mapování).
- Povolit duplikáty (vkládat doleva nebo doprava).

**Návrat při Delete:**

```csharp
// node.Left = Delete(...) - důležitý mechanismus
// vrátíme upravený podstrom, rodič si ho přiřadí
```

### Typické otázky u ústní zkoušky

- **"Kdy je binární vyhledávání lepší než lineární?"**
  Pro setříděná pole, kde děláme více vyhledávání. Asymptotický rozdíl O(log n) vs O(n) je výrazný od cca 20 prvků.

- **"Proč binární vyhledávání nefunguje na linked listu?"**
  Linked list nemá O(1) přístup k libovolnému indexu (skok na střed by stál O(n/2)). Binární vyhledávání tedy degraduje na O(n · log n), což je horší než lineární.

- **"Jaký je rozdíl mezi binárním stromem a BVS?"**
  Binární strom je libovolná hierarchie s max 2 potomky. BVS navíc splňuje invariant: levý podstrom < uzel < pravý podstrom.

- **"Jaká je složitost BVS v nejhorším případě a proč?"**
  O(n), když je strom degenerovaný (lineární řetězec). To se stane při vkládání setříděných dat. Řešením jsou samovyvažující stromy.

- **"K čemu slouží AVL strom?"**
  Garantuje O(log n) pro všechny operace tím, že po každém Insertu/Deletu provede rotace, aby udržel |bf| ≤ 1.

- **"Jaký je rozdíl mezi AVL a Red-Black tree?"**
  AVL je striktnější (|bf| ≤ 1) → méně Find, ale více rotací. Red-Black je benevolentnější (nejdelší cesta max 2× nejkratší) → více Find, ale méně rotací. V praxi v knihovnách převažuje Red-Black.

- **"Co dělá In-order průchod BVS?"**
  Navštíví uzly v rostoucím pořadí - dává setříděnou posloupnost. Důsledek: BVS implicitně reprezentuje setříděnou kolekci.

- **"Jak smazat uzel se dvěma potomky z BVS?"**
  Najdi in-order successor (= minimum pravého podstromu), zkopíruj jeho klíč do mazaného uzlu, smaž successora (ten má max 1 potomka, takže to je případ 1 nebo 2).

- **"Co je rotace a kdy se provádí?"**
  Lokální transformace BVS, která zachovává invariant a mění výšku. Provádí se v samovyvažujících stromech (AVL, Red-Black) po Insert/Delete, pokud strom přestane být vyvážený.

### Kontrolní seznam při code review

- [ ] Binární vyhledávání: bezpečný výpočet středu `levy + (pravy - levy) / 2`
- [ ] Binární vyhledávání: hranice `while (levy <= pravy)` (ne `<`)
- [ ] BVS invariant: kontrola pro CELÝ podstrom, ne jen přímé potomky
- [ ] Delete: tři případy (0, 1, 2 potomci) a správné použití successora
- [ ] Insert: vrátit upravený podstrom, rodič si ho přiřadí
- [ ] AVL: aktualizace `Height` po každé operaci
- [ ] AVL: kontrola `|bf| > 1` a správný typ rotace (LL, RR, LR, RL)
- [ ] Generika a `IComparable<T>` místo hardcoded `int`

---

## 14. Klíčové pojmy

- **Vyhledávání** - operace nalezení prvku s daným klíčem v kolekci.
- **Lineární vyhledávání** - sekvenční průchod, O(n), nevyžaduje setřídění.
- **Binární vyhledávání** - půlení intervalu, O(log n), vyžaduje setříděné pole.
- **Sentinel** - umělý prvek na konci pole, který eliminuje kontrolu hranice.
- **Lower bound / Upper bound** - varianty binárního vyhledávání pro nalezení rozsahu duplicit.
- **Interpolated search** - odhadnutí pozice lineární interpolací, O(log log n) pro rovnoměrná data.
- **Exponential search** - zdvojnásobování rozsahu, vhodné pro neomezená pole.
- **Binární strom** - hierarchická struktura s max 2 potomky na uzel.
- **Binární vyhledávací strom (BVS)** - binární strom s invariantem levý < uzel < pravý.
- **In-order průchod** - navštíví uzly v rostoucím pořadí (pro BVS).
- **Pre-order / Post-order** - další způsoby průchodu stromem.
- **Successor / Predecessor** - in-order následník/předchůdce uzlu.
- **Insert v BVS** - vložení nového uzlu na pozici nalezenou jako Find.
- **Delete v BVS** - tři případy podle počtu potomků (0/1/2); pro 2 použít successor.
- **Výška stromu** - délka nejdelší cesty od kořene k listu.
- **Hloubka uzlu** - vzdálenost uzlu od kořene.
- **Balance factor (bf)** - rozdíl výšek levého a pravého podstromu.
- **Vyvážený strom** - výška podstromů se liší max o 1 pro každý uzel.
- **Degenerovaný strom** - lineární řetězec, výška = n.
- **Samovyvažující strom** - automaticky udržuje O(log n) výšku přes rotace.
- **Rotace** - lokální transformace stromu zachovávající BVS invariant.
- **AVL strom** - samovyvažující BVS s |bf| ≤ 1, max 1 rotace na Insert.
- **Red-Black tree** - samovyvažující BVS s barvami; standard v knihovnách.
- **B-tree / B+-tree** - m-cestný strom pro databáze a souborové systémy.
- **Trie (prefix tree)** - specializovaný strom pro vyhledávání řetězců.
- **Splay tree** - amortizovaně O(log n) s pohybem často přistupovaných uzlů ke kořeni.
- **Treap** - randomizovaná kombinace BVS a haldy.
- **Skip list** - pravděpodobnostní alternativa k BVS.
- **Hash tabulka** - O(1) průměrná složitost pro vyhledávání bez řazení.
- **Bloom filter** - pravděpodobnostní test příslušnosti s false positives.
- **Range query** - vyhledávání všech prvků v daném intervalu, BVS to umí v O(log n + k).
- **Array.BinarySearch** - .NET built-in binární vyhledávání, vrací bitový doplněk pozice při nenalezení.
- **SortedDictionary, SortedSet** - .NET kolekce postavené na Red-Black tree.

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| Ot. 2 - Spojové struktury | Linked list nemá random access → nelze binární vyhledávání |
| Ot. 5 - Rekurze | Insert/Delete/Find v BVS přirozeně rekurzivní |
| Ot. 7 - Složitost | Porovnání O(n), O(log n), O(n log n) |
| Ot. 9 - Stromy | Definice stromu, průchody, AVL, Red-Black |
| Ot. 11 - Merge Sort | Princip Divide & Conquer (binární vyhledávání ho také používá) |
| Ot. 12 - Quick Sort | Partition jako analogie binárního dělení |
| Ot. 13 - Heap Sort | Halda je další binární strom, ale ne BVS |
| Ot. 15 - Rozděl a panuj | Binární vyhledávání jako klasická D&C aplikace |
| Ot. 18 - Grafy | Strom jako acyklický souvislý graf |
