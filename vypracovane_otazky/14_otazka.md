# 📚 Zápisky: Otázka č. 14 - Lineární a binární vyhledávání. Vyhledávací stromy.

**Datum:** 2026-02-17  
**Status:** ✅ Hotovo (teorie) | 🔜 Procvičení

---

## ✅ Checklist bodů otázky

- [x] Bod 1: Srovnání vyhledávání v nesetříděném poli, setříděném poli, BVS
- [x] Bod 2: Srovnání lineárního a binárního vyhledávání (časová složitost)
- [x] Bod 3: Příklady ze života, kdy které používáme
- [x] Bod 4: Definice binárního stromu a BVS
- [x] Bod 5: Základní operace – hledání, minimum, vkládání, mazání
- [x] Bod 6: Časová složitost operací
- [x] Bod 7: Vyváženost stromu
- [x] Bod 8: Zmínka o AVL stromech

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: Srovnání vyhledávání (nesetříděné pole, setříděné pole, BVS)

**Teorie:**

Tři hlavní způsoby jak hledat prvek:

| Struktura | Metoda | Složitost hledání | Přidání prvku |
|-----------|--------|-------------------|---------------|
| **Nesetříděné pole** | Lineární (od začátku po jednom) | O(n) | O(1) – na konec |
| **Setříděné pole** | Binární (půlení) | O(log n) | O(n) – musíš posunout prvky |
| **BVS (vyvážený)** | Porovnávání a odbočení vlevo/vpravo | O(log n) | O(log n) |

**Klíčový insight:** Setříděné pole a BVS mají stejnou rychlost hledání, ALE BVS je lepší pro dynamická data (časté přidávání/mazání), protože nemusíš posouvat prvky.

**Kód – Lineární vyhledávání:**

```csharp
// ✅ VERZE A - MATURITNÍ
int LinearniHledani(int[] pole, int hledany)
{
    for (int i = 0; i < pole.Length; i++)
    {
        if (pole[i] == hledany)
            return i;  // Vrátí index
    }
    return -1;  // Nenalezeno
}
```

**Kód – Binární vyhledávání:**

```csharp
// ✅ VERZE A - MATURITNÍ (iterativní)
int BinarniHledani(int[] pole, int hledany)
{
    int levy = 0;
    int pravy = pole.Length - 1;
    
    while (levy <= pravy)
    {
        int stred = (levy + pravy) / 2;
        
        if (pole[stred] == hledany)
            return stred;           // Našli!
        else if (pole[stred] < hledany)
            levy = stred + 1;       // Hledej vpravo
        else
            pravy = stred - 1;      // Hledej vlevo
    }
    return -1;  // Nenalezeno
}
```

**ASCII vizualizace binárního vyhledávání:**

```
Pole: [3, 7, 15, 28, 42, 66, 91]
Hledáme 15:
  Střed = 28 → 15 < 28 → hledej VLEVO [3, 7, 15]
  Střed = 7  → 15 > 7  → hledej VPRAVO [15]
  Střed = 15 → NAŠLI! ✅
```

---

### Bod 2: Srovnání lineárního a binárního vyhledávání (složitost)

**Lineární vyhledávání – O(n):**

| Případ | Složitost | Kdy |
|--------|-----------|-----|
| Nejlepší | O(1) | Prvek je hned první |
| Průměrný | O(n/2) = O(n) | Prvek je někde uprostřed |
| Nejhorší | O(n) | Prvek je poslední nebo tam není |

**Binární vyhledávání – O(log n):**

| Případ | Složitost | Kdy |
|--------|-----------|-----|
| Nejlepší | O(1) | Prvek je přesně uprostřed |
| Průměrný | O(log n) | Typický případ |
| Nejhorší | O(log n) | Prvek tam není |

**Konkrétní čísla:**

| Počet prvků (n) | Lineární (kroky) | Binární (kroky) |
|-----------------|-------------------|-----------------|
| 10 | 10 | 4 |
| 100 | 100 | 7 |
| 1 000 | 1 000 | 10 |
| 1 000 000 | 1 000 000 | **20** |
| 1 000 000 000 | 1 000 000 000 | **30** |

**Kdy je lineární LEPŠÍ?**
1. Pole není setříděné → binární nelze použít
2. Malé pole (do ~10 prvků) → režie binárního se nevyplatí
3. Jedno hledání → setřídění O(n log n) je dražší než lineární O(n)

**Rozhodovací pravidlo:**
- Jedno hledání → Lineární O(n)
- K hledání (K velké) → Setřiď + binární O(n log n + K × log n)

---

### Bod 3: Příklady ze života

**Lineární vyhledávání (neuspořádaná data, málo prvků):**
- Hledáš klíče v kapse (5 klíčů, zkusíš jeden po druhém)
- Ctrl+F v dokumentu (editor čte od začátku)
- Hledáš auto na parkovišti (chodíš řada po řadě)
- V programování: hledání v malém List<T>, v LinkedList

**Binární vyhledávání (seřazená data):**
- Slovník / encyklopedie (otevřeš uprostřed, zužuješ)
- Telefonní seznam (hledáš podle písmena)
- Hra "hádej číslo 1-100" (tipuješ prostředek)
- V programování: Array.BinarySearch(), Git bisect

**BVS (dynamická data, časté přidávání/mazání + hledání):**
- Databázový index (záznamy se mění, ale potřebuješ rychle hledat)
- Kontakty v telefonu (přidáváš/mažeš, ale hledáš rychle)
- V programování: SortedDictionary<K,V>, SortedSet<T>

**Rozhodovací tabulka:**

| Situace | Použij | Proč |
|---------|--------|------|
| Málo dat (< 20) | Lineární | Jednoduché, režie nepřeváží |
| Nesetříděná data, jedno hledání | Lineární | Třídění by bylo dražší |
| Setříděná data, hodně hledání | Binární | O(log n) za každé hledání |
| Často se mění + často hledáš | BVS | O(log n) přidání i hledání |
| Statická data, extrémně velká | Binární v poli | Pole je paměťově efektivnější |

---

### Bod 4: Definice binárního stromu a BVS

**Binární strom:**
Stromová datová struktura, kde každý uzel má **nejvýše 2 potomky** (levý a pravý syn). Žádné pravidlo pro uspořádání hodnot.

```
        [A]          ← Kořen (root)
       /   \
     [B]   [C]       ← Vnitřní uzly
     / \     \
   [D] [E]   [F]     ← Listy (nemají potomky)
```

**Klíčové pojmy:**
- **Kořen** – vrchní uzel, nemá rodiče
- **List** – uzel bez potomků
- **Vnitřní uzel** – má alespoň jednoho potomka
- **Výška stromu** – nejdelší cesta od kořene k listu
- **Hloubka uzlu** – vzdálenost od kořene

**Binární vyhledávací strom (BVS):**
Binární strom s pravidlem: Pro KAŽDÝ uzel platí:
- Všechny hodnoty v **levém** podstromu jsou **menší**
- Všechny hodnoty v **pravém** podstromu jsou **větší**

```
        [10]
       /    \
     [5]    [15]       ← 5 < 10 < 15 ✅
     / \    /  \
   [3] [7][12] [20]    ← 3 < 5, 7 > 5, 12 < 15, 20 > 15 ✅
```

**⚠️ CHYTÁK: Pravidlo platí pro CELÝ podstrom, ne jen přímé potomky!**

```
        [10]
       /    \
     [5]    [15]
     / \
   [3] [12]   ❌ NENÍ BVS! 12 > 10, ale je v levém podstromu kořene!
```

**Implementace uzlu:**

```csharp
// ✅ VERZE A - MATURITNÍ
class Node
{
    public int Key;
    public Node Left;
    public Node Right;

    public Node(int key)
    {
        Key = key;
        Left = null;
        Right = null;
    }
}
```

```csharp
// 💡 VERZE B - SENIOR (s generiky)
class Node<T>
{
    public int Key { get; set; }
    public T Value { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }

    public Node(int key, T value)
    {
        Key = key;
        Value = value;
    }
}
```

---

### Bod 5: Základní operace BVS

#### 5.1) Hledání klíče (Find)

**Princip:** Začni u kořene. Menší → vlevo. Větší → vpravo. Opakuj.

```csharp
// ✅ VERZE A - MATURITNÍ (iterativní)
Node Find(Node root, int key)
{
    Node current = root;
    
    while (current != null)
    {
        if (key == current.Key)
            return current;
        else if (key < current.Key)
            current = current.Left;
        else
            current = current.Right;
    }
    
    return null;  // Nenalezeno
}
```

```csharp
// 💡 VERZE B - SENIOR (rekurzivní)
Node Find(Node node, int key)
{
    if (node == null) return null;
    if (key == node.Key) return node;
    
    return key < node.Key 
        ? Find(node.Left, key) 
        : Find(node.Right, key);
}
```

#### 5.2) Hledání minima a maxima

**Minimum** = jdi pořád doleva. **Maximum** = jdi pořád doprava.

```csharp
// ✅ VERZE A - MATURITNÍ
Node FindMin(Node node)
{
    Node current = node;
    while (current.Left != null)
        current = current.Left;
    return current;
}

Node FindMax(Node node)
{
    Node current = node;
    while (current.Right != null)
        current = current.Right;
    return current;
}
```

#### 5.3) Vkládání (Insert)

**Princip:** Hledej jako Find. Až narazíš na null, tam vytvoř nový uzel.

```
Vkládáme 8:
  10 → 8<10 vlevo → 5 → 8>5 vpravo → 7 → 8>7 vpravo → null → VLOŽ

        [10]                    [10]
       /    \                  /    \
     [5]    [15]    →       [5]    [15]
     / \    /  \            / \    /  \
   [3] [7][12] [20]      [3] [7][12] [20]
                                \
                                [8] ← NOVÝ!
```

```csharp
// ✅ VERZE A - MATURITNÍ (rekurzivní)
Node Insert(Node node, int key)
{
    if (node == null)
        return new Node(key);
    
    if (key < node.Key)
        node.Left = Insert(node.Left, key);
    else if (key > node.Key)
        node.Right = Insert(node.Right, key);
    
    return node;
}

// Volání: root = Insert(root, 8);
```

#### 5.4) Mazání (Delete) – 3 případy

**Případ 1: List (0 potomků)** → prostě odstraň

```
Mažeme 3:
        [10]                [10]
       /    \              /    \
     [5]    [15]    →    [5]    [15]
     / \    /  \           \    /  \
   [3] [7][12] [20]      [7][12] [20]
```

**Případ 2: Jeden potomek** → nahraď potomkem

```
Mažeme 5 (má jen pravého syna 7):
        [10]                [10]
       /    \              /    \
     [5]    [15]    →    [7]    [15]
       \    /  \               /  \
      [7] [12] [20]         [12] [20]
```

**Případ 3: Dva potomci** → trik s následníkem
1. Najdi **následníka** = minimum v pravém podstromu
2. **Zkopíruj** jeho klíč do mazaného uzlu
3. **Smaž** následníka (má max 1 potomka → případ 1 nebo 2)

(Alternativně: najdi **předchůdce** = maximum v levém podstromu – funguje taky!)

```
Mažeme 10:
  Následník = min v pravém podstromu = 12

        [10]                [12]
       /    \              /    \
     [5]    [15]    →    [5]    [15]
     / \    /  \         / \      \
   [3] [7][12] [20]   [3] [7]   [20]
```

```csharp
// ✅ VERZE A - MATURITNÍ
Node Delete(Node node, int key)
{
    if (node == null)
        return null;
    
    // Hledáme správný uzel
    if (key < node.Key)
        node.Left = Delete(node.Left, key);
    else if (key > node.Key)
        node.Right = Delete(node.Right, key);
    else
    {
        // NAŠLI! Řešíme případy:
        
        // Případ 1 & 2: Žádný nebo jeden potomek
        if (node.Left == null)
            return node.Right;
        if (node.Right == null)
            return node.Left;
        
        // Případ 3: Dva potomci
        Node naslednik = FindMin(node.Right);
        node.Key = naslednik.Key;
        node.Right = Delete(node.Right, naslednik.Key);
    }
    
    return node;
}

// Volání: root = Delete(root, 10);
```

**Jak si zapamatovat mazání u tabule:**

```
MAZÁNÍ Z BVS:
1. Najdi uzel (jako Find)
2. Kolik má potomků?
   0 potomků → smaž (vrať null)
   1 potomek → nahraď tím potomkem
   2 potomci → najdi NÁSLEDNÍKA (min v pravém podstromu)
              → zkopíruj jeho klíč
              → smaž následníka
```

**Kompletní třída (public rozhraní):**

```csharp
class BinarySearchTree
{
    private Node root;
    
    public void Insert(int key)  { root = Insert(root, key); }
    public void Delete(int key)  { root = Delete(root, key); }
    public bool Contains(int key) { return Find(root, key) != null; }
    
    // ... privátní rekurzivní metody výše
}

// Volání:
BinarySearchTree tree = new BinarySearchTree();
tree.Insert(10);
tree.Insert(5);
tree.Insert(15);
tree.Delete(10);
```

---

### Bod 6: Časová složitost operací BVS

Všechny operace jdou od kořene dolů po jedné cestě → složitost = **O(h)**, kde h je výška stromu.

| Operace | Vyvážený strom | Nevyvážený (degenerovaný) |
|---------|---------------|---------------------------|
| Hledání (Find) | O(log n) | O(n) |
| Minimum / Maximum | O(log n) | O(n) |
| Vkládání (Insert) | O(log n) | O(n) |
| Mazání (Delete) | O(log n) | O(n) |

**Paměťová složitost:** O(n) – každý prvek = jeden uzel.

| Uzlů (n) | Výška vyváženého | Výška nejhoršího |
|-----------|------------------|------------------|
| 7 | 2 | 6 |
| 1 000 | ~10 | 999 |
| 1 000 000 | ~20 | 999 999 |

**Co říct u maturity:**
> "Všechny základní operace BVS mají složitost O(h), kde h je výška stromu. U vyváženého stromu h = log n → O(log n). U nevyváženého h = n → O(n). Proto je důležité udržovat strom vyvážený – k tomu slouží AVL stromy."

---

### Bod 7: Vyváženost stromu

**Strom je vyvážený**, když pro každý uzel platí, že výška levého a pravého podstromu se liší max o 1.

**Balance factor:**

```
balance factor = výška(levý podstrom) - výška(pravý podstrom)

Vyvážený: bf ∈ {-1, 0, +1}
Nevyvážený: |bf| ≥ 2
```

```
VYVÁŽENÝ ✅                    NEVYVÁŽENÝ ❌

        [10] bf=0                   [10] bf=-2
       /    \                           \
     [5]   [15]                        [15] bf=-1
     / \    /  \                           \
   [3][7][12] [20]                        [20]
```

**Jak vznikne nevyvážený strom?**
Vkládáš seřazená data: 1, 2, 3, 4... → strom degeneruje na spojový seznam.

**Řešení:** Samovyvažující stromy (AVL, Red-Black) – po každé operaci se automaticky opraví pomocí rotací.

---

### Bod 8: AVL stromy

**AVL strom** = samovyvažující BVS, kde pro každý uzel platí |balance factor| ≤ 1.

Po každém insertu/deletu se zkontroluje bf a pokud je |bf| > 1, provede se **rotace**.

**4 typy rotací:**

| Nevyváženost | Směr | Rotace |
|-------------|------|--------|
| LL (vlevo-vlevo) | Přetížený vlevo | Jedna PRAVÁ rotace |
| RR (vpravo-vpravo) | Přetížený vpravo | Jedna LEVÁ rotace |
| LR (vlevo-vpravo) | "Koleno" vlevo | LEVÁ + PRAVÁ (dvojitá) |
| RL (vpravo-vlevo) | "Koleno" vpravo | PRAVÁ + LEVÁ (dvojitá) |

**Příklad pravé rotace (LL případ):**

```
PŘED:              PO:
      [30]              [20]
      /                /    \
    [20]     →      [10]   [30]
    /
  [10]
```

**Příklad dvojité rotace (LR případ):**

```
PŘED:              Krok 1 (levá):     Krok 2 (pravá):
      [30]              [30]                [20]
      /                 /                  /    \
    [10]     →       [20]       →      [10]   [30]
       \             /
       [20]        [10]
```

**Kód rotací:**

```csharp
Node RotateRight(Node node)
{
    Node newRoot = node.Left;
    node.Left = newRoot.Right;      // Přepoj "sirotka"
    newRoot.Right = node;
    return newRoot;
}

Node RotateLeft(Node node)
{
    Node newRoot = node.Right;
    node.Right = newRoot.Left;      // Přepoj "sirotka"
    newRoot.Left = node;
    return newRoot;
}
```

**Srovnání BVS vs AVL:**

| Vlastnost | BVS | AVL |
|-----------|-----|-----|
| Hledání (nejhorší) | O(n) | **O(log n)** |
| Insert (nejhorší) | O(n) | **O(log n)** |
| Delete (nejhorší) | O(n) | **O(log n)** |
| Složitost implementace | Jednoduchá | Složitější (rotace) |

**Další samovyvažující stromy:**
- **Red-Black strom** – C# interně v SortedDictionary<K,V> a SortedSet<T>
- **B-strom** – databázové indexy

---

## 📋 Procvičené maturitní úlohy

*(Zatím neprocvičeno – úlohy připraveny k procvičení)*

- ⬜ **Úloha 275** – Vytvoření BVS ze souboru
- ⬜ **Úloha 276** – BVS ze souboru bez dvou největších
- ⬜ **Úloha 277** – Mazání uzlu z BVS
- ⬜ **Úloha 278** – Mazání uzlů větších než C
- ⬜ **Úloha 279** – Výpis BVS v rostoucím pořadí (inorder)
- ⬜ **Úloha 280** – Výpis BVS bez rekurze
- ⬜ **Úloha 281** – Vytvoření vyváženého BVS
- ⬜ **BONUS** – Kompletní AVL strom s rotacemi

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Binární vyhledávání vyžaduje SETŘÍDĚNÉ pole!** – nejčastější chyba
2. **BVS pravidlo platí pro CELÝ podstrom**, ne jen přímé potomky
3. **Složitost BVS závisí na vyváženosti** – O(log n) jen pro vyvážený, O(n) pro degenerovaný
4. **Mazání se 2 potomky** – nezapomeň na trik s následníkem (min v pravém podstromu)
5. **Binární vyhledávání nefunguje na LinkedList** – nemáš random access ke středu
6. **Lineární vyhledávání může být lepší** – pro malá pole nebo jedno hledání v nesetříděných datech
7. **`return node.Right`** v Delete nevrací "ven" – vrací rodiči, který si to přiřadí jako potomka
8. **AVL rotace** – jednoduchá jde OPAČNÝM směrem než nevyváženost, dvojitá nejdřív narovná "koleno"

---

## 🚀 Senior Tipy

1. **C# má Array.BinarySearch()** – nemusíš psát vlastní, ale u maturity musíš umět napsat ručně
2. **SortedDictionary<K,V>** interně používá Red-Black strom (varianta samovyvažujícího stromu)
3. **Dictionary<K,V>** používá hash tabulku → O(1) průměrně, ale nemáš seřazená data
4. **V praxi** se často používá Dictionary pro hledání (O(1)) místo BVS (O(log n)), pokud nepotřebuješ řazení

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 7 (Složitost)** – O-notace, porovnání O(n) vs O(log n) vs O(n²)
- **Otázka 9 (Stromy)** – definice stromu, BVS, průchody (inorder, preorder, postorder)
- **Otázka 5 (Rekurze)** – Insert a Delete v BVS jsou rekurzivní algoritmy
- **Otázka 2 (Spojové struktury)** – BVS je dynamická struktura jako LinkedList, ale s lepší složitostí hledání
- **Otázka 3 (Zásobník)** – iterativní průchod stromem bez rekurze vyžaduje zásobník
- **Otázka 13 (Heap Sort)** – halda je speciální typ binárního stromu (ale NENÍ BVS!)
- **Otázka 15 (Rozděl a panuj)** – binární vyhledávání je příklad přístupu rozděl a panuj
