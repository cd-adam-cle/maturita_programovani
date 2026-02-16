# 📚 Zápisky: Otázka č. 9 - Stromy a jejich využití. Průchod stromem
**Datum:** 2025-02-16  
**Status:** 🚧 Rozpracováno (Body 1-5 hotovo, 6-10 zbývá)

---

## ✅ Checklist bodů otázky
- [x] Bod 1: Definice stromu
- [x] Bod 2: Definice binárního stromu
- [x] Bod 3: Definice binárního vyhledávacího stromu (BVS)
- [x] Bod 4: Algoritmus procházení libovolného stromu
- [x] Bod 5: Algoritmus hledání prvku v BVS
- [ ] Bod 6: Průchod stromem do hloubky a do šířky
- [ ] Bod 7: Co může být ve stromu uloženo
- [ ] Bod 8: Co je halda a k čemu slouží
- [ ] Bod 9: Příklady využití stromů
- [ ] Bod 10: Možný způsob implementace

---

## 🧠 Klíčové koncepty & Snippety

### Bod 1: Definice stromu

**Teorie:**
- Strom je **hierarchická datová struktura** složená z uzlů a hran
- Má jeden **kořen (root)** - vrchní uzel
- Každý uzel (kromě kořene) má **právě jednoho rodiče**
- Uzly mohou mít **libovolný počet potomků**
- **Neobsahuje cykly** - není možné se vrátit ke stejnému uzlu

**ASCII Vizualizace:**
```
        [A]  ← Kořen (root)
       / | \
      /  |  \
    [B] [C] [D]  ← Potomci kořene (děti)
    / \      |
   /   \     |
 [E]   [F]  [G]  ← Listy (nemají potomky)
```

**Terminologie:**
- **Kořen (Root):** Nejvyšší uzel (A)
- **Rodiče (Parent):** Uzel s potomky
- **Potomci (Children):** Uzly pod rodičem
- **Listy (Leaves):** Uzly bez potomků (E, F, G)
- **Vnitřní uzly:** Uzly s alespoň jedním potomkem
- **Hloubka uzlu:** Počet hran od kořene
- **Výška stromu:** Maximální hloubka listu

**Rozdíl strom vs graf:**
```
Strom:
- ❌ Neobsahuje cykly
- ✅ Má jeden kořen
- ✅ Hierarchická struktura
- Každý uzel má max 1 rodiče

Graf:
- ✅ Může obsahovat cykly
- ❌ Nemá kořen
- ❌ Obecná struktura
- Uzel může mít více "rodičů"
```

---

### Bod 2: Definice binárního stromu

**Teorie:**
- Binární strom = každý uzel má **maximálně 2 potomky**
- Rozlišujeme **levého** a **pravého** potomka
- Pořadí potomků **záleží** (levý ≠ pravý)

**ASCII Vizualizace:**
```
        [10]        ← Kořen
        /  \
       /    \
     [5]    [15]    ← Levý a pravý potomek
     / \      \
    /   \      \
  [3]   [7]   [20]  ← Listy
```

**Kód (Maturitní verze):**
```csharp
// ✅ VERZE A - MATURITNÍ
// Struktura uzlu binárního stromu

class Node
{
    public int Data;           // Hodnota uzlu
    public Node Left;          // Levý potomek (může být null)
    public Node Right;         // Pravý potomek (může být null)
    
    public Node(int data)
    {
        Data = data;
        Left = null;
        Right = null;
    }
}

// Příklad vytvoření stromu
Node root = new Node(10);
root.Left = new Node(5);
root.Right = new Node(15);
root.Left.Left = new Node(3);
root.Left.Right = new Node(7);
root.Right.Right = new Node(20);
```

**Typy binárních stromů:**
1. **Plný (Full):** Každý uzel má 0 nebo 2 potomky
2. **Úplný (Complete):** Všechny úrovně zaplněné, kromě poslední (zleva)
3. **Perfektní (Perfect):** Všechny listy na stejné úrovni

**Vlastnosti:**
- Maximální počet uzlů na úrovni h: **2^h**
- Maximální počet uzlů ve stromu výšky h: **2^(h+1) - 1**
- Minimální výška pro n uzlů: **log₂(n)**

---

### Bod 3: Definice binárního vyhledávacího stromu (BVS)

**Teorie:**
- BVS = binární strom s pravidlem uspořádání
- **ZLATÉ PRAVIDLO:** Pro každý uzel platí:
  - Všechny hodnoty v **levém** podstromu < hodnota uzlu
  - Všechny hodnoty v **pravém** podstromu > hodnota uzlu

**ASCII Vizualizace:**
```
        [10]        ← Kořen
        /  \
       /    \
     [5]    [15]    ← 5 < 10 < 15 ✅
     / \      \
    /   \      \
  [3]   [7]   [20]  ← 3 < 5 < 7  a  15 < 20 ✅
```

**Kód (Maturitní verze):**
```csharp
// ✅ VERZE A - MATURITNÍ
// BVS s automatickým Insert()

class BinarySearchTree
{
    class Node
    {
        public int Data;
        public Node Left;
        public Node Right;
        
        public Node(int data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
    
    private Node root;
    
    public BinarySearchTree()
    {
        root = null;
    }
    
    // ✨ Automatické přidání prvku
    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }
    
    private Node InsertRecursive(Node current, int value)
    {
        // Prázdné místo → vytvoříme uzel
        if (current == null)
            return new Node(value);
        
        // Porovnáme a jdeme vlevo/vpravo
        if (value < current.Data)
            current.Left = InsertRecursive(current.Left, value);
        else if (value > current.Data)
            current.Right = InsertRecursive(current.Right, value);
        // else: duplicita, nepřidáváme
        
        return current;
    }
}
```

**Použití:**
```csharp
BinarySearchTree bst = new BinarySearchTree();
bst.Insert(10);
bst.Insert(5);
bst.Insert(15);
bst.Insert(3);
bst.Insert(7);
bst.Insert(20);
// Strom je automaticky správně uspořádaný!
```

**Časová složitost Insert():**
- **Vyvážený strom:** O(log n)
- **Nevyvážený strom:** O(n) (degeneruje na spojový seznam)

---

### Bod 4: Algoritmus procházení libovolného stromu

**Teorie:**
- Procházení = navštívit **každý uzel právě jednou**
- Obecný strom může mít **libovolný počet potomků**
- Používá se **rekurze** (přirozené pro stromovou strukturu)

**Struktura obecného stromu:**
```csharp
// ✅ VERZE A - MATURITNÍ
// Uzel obecného (N-árního) stromu

class TreeNode
{
    public int Data;
    public List<TreeNode> Children;  // Seznam všech potomků
    
    public TreeNode(int data)
    {
        Data = data;
        Children = new List<TreeNode>();
    }
    
    public void AddChild(TreeNode child)
    {
        Children.Add(child);
    }
}
```

**Kód (Maturitní verze):**
```csharp
// ✅ VERZE A - MATURITNÍ
// Procházení do hloubky (DFS)

void TraverseTree(TreeNode node)
{
    // Základní případ: prázdný uzel
    if (node == null)
        return;
    
    // 1. Zpracuj aktuální uzel
    Console.WriteLine(node.Data);
    
    // 2. Rekurzivně projdi všechny potomky
    foreach (TreeNode child in node.Children)
    {
        TraverseTree(child);
    }
}
```

**Příklad stromu:**
```
        [A]
       / | \
     [B][C][D]
     / \    |
   [E] [F] [G]
```

**Pořadí výpisu:** A B E F C D G

**Další operace při procházení:**
```csharp
// Sečtení všech hodnot
int Sum(TreeNode node)
{
    if (node == null) return 0;
    
    int sum = node.Data;
    foreach (TreeNode child in node.Children)
        sum += Sum(child);
    return sum;
}

// Počet uzlů
int CountNodes(TreeNode node)
{
    if (node == null) return 0;
    
    int count = 1;
    foreach (TreeNode child in node.Children)
        count += CountNodes(child);
    return count;
}

// Hloubka stromu
int GetDepth(TreeNode node)
{
    if (node == null) return 0;
    
    int maxChildDepth = 0;
    foreach (TreeNode child in node.Children)
    {
        int childDepth = GetDepth(child);
        if (childDepth > maxChildDepth)
            maxChildDepth = childDepth;
    }
    return 1 + maxChildDepth;
}
```

**Časová složitost:** O(n) - navštívíme každý uzel právě jednou

---

### Bod 5: Algoritmus hledání prvku v BVS

**Teorie:**
- Využití BVS pravidla → nemusíme procházet celý strom
- V každém kroku **eliminujeme polovinu** stromu
- Procházíme jen **jednu cestu** od kořene k listu

**Algoritmus:**
1. Začni v kořeni
2. Porovnej hledanou hodnotu s aktuálním uzlem:
   - **Našli jsme** → vrátíme true
   - **Hledané < aktuální** → jdi **vlevo**
   - **Hledané > aktuální** → jdi **vpravo**
3. Opakuj, dokud nenajdeš NEBO nedojdeš k null

**Vizualizace hledání 7:**
```
        [10]  ← 7 < 10 → jdi VLEVO
        /  \
       ↓    
     [5]    [15]  ← 7 > 5 → jdi VPRAVO
     / \      \
        ↓
  [3]   [7]  [20]  ← 7 == 7 → NAŠLI! ✅
```

**Kód (Maturitní verze - Rekurzivní):**
```csharp
// ✅ VERZE A - MATURITNÍ
// Hledání v BVS - rekurzivně

public bool Search(int value)
{
    return SearchRecursive(root, value);
}

private bool SearchRecursive(Node current, int value)
{
    // Prázdný uzel → nenašli
    if (current == null)
        return false;
    
    // Našli jsme!
    if (value == current.Data)
        return true;
    
    // Jdi vlevo nebo vpravo
    if (value < current.Data)
        return SearchRecursive(current.Left, value);
    else
        return SearchRecursive(current.Right, value);
}
```

**Kód (Maturitní verze - Iterativní):**
```csharp
// ✅ VERZE A - MATURITNÍ
// Hledání v BVS - iterativně (efektivnější!)

public bool SearchIterative(int value)
{
    Node current = root;
    
    while (current != null)
    {
        // Našli jsme!
        if (value == current.Data)
            return true;
        
        // Jdi vlevo nebo vpravo
        if (value < current.Data)
            current = current.Left;
        else
            current = current.Right;
    }
    
    return false; // Nenašli
}
```

**Varianta - vrátit celý uzel:**
```csharp
public Node Find(int value)
{
    Node current = root;
    
    while (current != null)
    {
        if (value == current.Data)
            return current;
        
        if (value < current.Data)
            current = current.Left;
        else
            current = current.Right;
    }
    
    return null;
}
```

**Časová složitost:**
- **Vyvážený BVS:** O(log n) - procházíme jen jednu cestu
- **Nevyvážený BVS:** O(n) - v nejhorším případě je to spojový seznam

**Porovnání s jinými metodami:**
```
Pro 1000 prvků:
- BVS hledání (vyvážený): ≈ 10 kroků
- Lineární hledání: ≈ 500 kroků (průměr)
- Binární hledání v poli: ≈ 10 kroků
```

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

### Při definicích:
- **Strom vs graf:** Strom NEOBSAHUJE cykly, graf může
- **Binární strom:** Max 2 potomci, pořadí záleží (levý ≠ pravý)
- **BVS pravidlo:** VŠECHNY hodnoty v levém podstromu < uzel < VŠECHNY v pravém

### Při implementaci:
- **Null kontrola:** Vždy kontroluj, jestli uzel není null!
- **Rekurze:** Nezapomeň na základní případ (null → zastavení)
- **BVS Insert:** Použij rekurzi nebo while cyklus, ne ruční `root.Left = ...`
- **Duplicity:** Rozhodněte se, jestli je přidáváte (typicky NE)

### U časové složitosti:
- **Vyvážený vs nevyvážený:** O(log n) vs O(n)
- **Procházení:** Vždy O(n) - musíme navštívit všechny uzly
- **Hledání v BVS:** O(log n) POUZE pokud je strom vyvážený!

### U ústní zkoušky:
- Umět nakreslit příklad stromu na tabuli
- Vysvětlit průchod krok po kroku s ukazováním
- Ukázat, jak Insert() automaticky najde místo
- Porovnat BVS hledání s lineárním hledáním

---

## 🚀 Senior Tip

### Iterativní vs Rekurzivní
**Rekurzivní verze:**
- ✅ Elegantnější, kratší kód
- ✅ Přirozenější pro stromové struktury
- ❌ Spotřebovává paměť (call stack)
- ❌ Může způsobit StackOverflow u velkých stromů

**Iterativní verze:**
- ✅ Šetří paměť
- ✅ Rychlejší (bez rekurzivního overhead)
- ✅ Vhodnější pro velké stromy
- ❌ Delší kód

**Pro maturitu:** Nauč se obě verze! Rekurzivní je jednodušší na vysvětlení, iterativní ukazuje hlubší pochopení.

### Nevyvážený BVS problém
```
Vkládání v pořadí: 1, 2, 3, 4, 5

Výsledek:
[1]
  \
  [2]
    \
    [3]
      \
      [4]
        \
        [5]  → Spojový seznam! O(n) operace!
```

**Řešení:** AVL stromy, Red-Black stromy (automatické vyvažování) - nemusíš implementovat, stačí zmínit existenci.

### V praxi:
- C# má `SortedSet<T>` (implementuje balancovaný BST)
- Nepiš vlastní BVS do produkce, použij knihovní implementaci
- BVS je základ pro pokročilejší struktury (B-stromy v databázích)

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 2 (Spojové struktury):** Strom je také spojová struktura (uzly propojené odkazy)
- **Otázka 5 (Rekurze):** Procházení stromu je klasický příklad rekurze
- **Otázka 7 (Časová složitost):** O(log n) vs O(n) v závislosti na vyvážení
- **Otázka 13 (Heap Sort):** Halda je speciální typ stromu
- **Otázka 14 (Vyhledávání):** BVS kombinuje rychlost bin. vyhledávání s flexibilitou
- **Otázka 16 (Aritmetické výrazy):** Expression tree - strom pro výrazy
- **Otázka 22 (DFS/BFS):** Průchody stromem jsou základ pro grafové algoritmy

---

## 📋 Procvičené maturitní úlohy

**Status:** ⬜ Zatím žádné (úlohy procvičíme po dokončení všech bodů)

**Plánované úlohy (z Mini-Indexu):**
1. **BST Implementation** - Implementace insert, find, delete
2. **Tree Traversal** - In-order, pre-order, post-order průchody
3. **BFS a DFS na stromu** - Oba způsoby procházení
4. **Min/Max Heap** - Implementace haldové struktury
5. **Huffman Coding** - Komprese pomocí stromu
6. **Expression Tree** - Vyhodnocení aritmetického výrazu

---

## 📝 Poznámky k dalšímu pokračování

**Zbývající body k procvičení:**
- [ ] Bod 6: Průchod stromem do hloubky (DFS) a do šířky (BFS)
- [ ] Bod 7: Co může být ve stromu uloženo
- [ ] Bod 8: Halda (heap) a její využití
- [ ] Bod 9: Praktické příklady využití stromů
- [ ] Bod 10: Implementace (OOP přístup)

**Až dokončíme všechny body, přejdeme na fázi praktického procvičení!**

---

**Konec zápisu - Aktualizováno: 2025-02-16**
