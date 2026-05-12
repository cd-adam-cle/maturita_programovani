# Zápisky: Otázka č. 9 - Stromy a jejich využití. Průchod stromem

## Checklist bodů otázky

- [x] Bod 1: Definice stromu
- [x] Bod 2: Definice binárního stromu
- [x] Bod 3: Definice binárního vyhledávacího stromu (BVS)
- [x] Bod 4: Algoritmus procházení libovolného stromu
- [x] Bod 5: Algoritmus hledání prvku v BVS
- [x] Bod 6: Průchod stromem do hloubky (DFS) a do šířky (BFS)
- [x] Bod 7: Co může být ve stromu uloženo
- [x] Bod 8: Co je halda a k čemu slouží
- [x] Bod 9: Příklady využití stromů
- [x] Bod 10: Možný způsob implementace
- [x] Vyvážené stromy (AVL, Red-Black) a problém degenerace BVS
- [x] Souvislosti s frontou, zásobníkem, rekurzí, grafy

---

## Klíčové koncepty & Snippety

### Bod 1: Definice stromu

**Strom** je hierarchická datová struktura tvořená **uzly (nodes)** a **hranami (edges)**. Z matematického hlediska je to **souvislý acyklický graf** – speciální typ grafu, který:
- je **souvislý** (mezi každými dvěma uzly existuje cesta),
- **neobsahuje cykly** (nelze se vrátit do stejného uzlu, aniž bys prošel hranu zpět).

**Klíčové vlastnosti:**
- Má **právě jeden kořen (root)** – výchozí uzel struktury.
- Každý uzel kromě kořene má **právě jednoho rodiče (parent)**.
- Uzel může mít **libovolný počet potomků (children)** – pro obecný strom.
- Strom s `n` uzly má vždy **přesně `n − 1` hran**.
- Mezi libovolnými dvěma uzly existuje **právě jedna cesta**.

**ASCII vizualizace:**
```
        [A]  ← kořen (root)
       / | \
      /  |  \
    [B] [C] [D]  ← potomci kořene (children, levels)
    / \      |
   /   \     |
 [E]   [F]  [G]  ← listy (leaves) – nemají potomky
```

**Terminologie:**
| Pojem | Význam |
|-------|--------|
| **Kořen (Root)** | Nejvyšší uzel; jediný bez rodiče. |
| **Rodič (Parent)** | Uzel, který má potomky. |
| **Potomek (Child)** | Uzel, který má rodiče. |
| **Sourozenci (Siblings)** | Uzly se stejným rodičem. |
| **Předek (Ancestor)** | Uzel na cestě od kořene k danému uzlu. |
| **Potomek (Descendant)** | Uzel v podstromu daného uzlu. |
| **List (Leaf)** | Uzel bez potomků (vnější uzel). |
| **Vnitřní uzel (Internal node)** | Uzel s alespoň jedním potomkem. |
| **Podstrom (Subtree)** | Strom tvořený uzlem a všemi jeho potomky. |
| **Hloubka (Depth)** | Počet hran od kořene k danému uzlu (kořen má hloubku 0). |
| **Výška (Height)** | Hloubka nejhlubšího listu; výška stromu = výška kořene. |
| **Úroveň (Level)** | Množina uzlů ve stejné hloubce. |
| **Stupeň uzlu** | Počet jeho potomků (v některých textech celkový počet sousedů). |

**Rozdíl strom vs. graf:**

| Vlastnost | Strom | Graf |
|-----------|-------|------|
| Cykly | NE | Může |
| Kořen | ANO (jeden) | NE |
| Hran | `V − 1` | obecně |
| Cesta mezi dvěma uzly | Právě jedna | Libovolný počet |
| Hierarchie | Ano | Ne nutně |

**Speciální typy stromů:**
- **Obecný (n-ární) strom** – uzly mají libovolný počet potomků.
- **Binární strom** – maximálně 2 potomci.
- **Vyhledávací strom (BST/BVS)** – binární strom s pravidlem uspořádání.
- **Vyvážený strom (AVL, Red-Black)** – BVS s mechanismem udržujícím malou výšku.
- **B-strom, B+ strom** – m-ární strom optimalizovaný pro databáze a disky.
- **Halda (Heap)** – úplný binární strom s heap property.
- **Trie (prefixový strom)** – strom pro řetězce, větvení podle znaků.
- **Quad-tree / Octree** – prostorové členění (2D/3D).

---

### Bod 2: Definice binárního stromu

**Binární strom** je strom, kde každý uzel má **maximálně 2 potomky** – pojmenované **levý** a **pravý** podstrom. **Pořadí potomků záleží** – `(L, R) ≠ (R, L)`.

```
        [10]        ← kořen
        /  \
       /    \
     [5]    [15]    ← levý a pravý potomek
     / \      \
    /   \      \
  [3]   [7]   [20]  ← listy
```

**Důležité vlastnosti:**
- Maximální počet uzlů na úrovni `d`: **2^d**.
- Maximální počet uzlů ve stromu výšky `h`: **2^(h+1) − 1**.
- Minimální výška pro `n` uzlů: **⌈log₂(n+1)⌉ − 1**, tedy přibližně **log₂ n**.
- Vyvážený binární strom má výšku `O(log n)`, degenerovaný `O(n)`.

**Typy binárních stromů:**
1. **Plný binární strom (full)** – každý uzel má buď 0 nebo 2 potomky.
2. **Úplný binární strom (complete)** – všechny úrovně plně obsazené, kromě poslední, která je zaplněna **zleva**. (Halda je úplný binární strom.)
3. **Perfektní binární strom (perfect)** – všechny vnitřní uzly mají 2 potomky a všechny listy jsou na stejné úrovni.
4. **Degenerovaný strom** – každý uzel má max 1 potomek → vlastně **spojový seznam**.
5. **Vyvážený binární strom** – výška obou podstromů každého uzlu se liší max o 1 (AVL).

**Implementace uzlu:**
```csharp
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

// Příklad vytvoření stromu
Node root = new Node(10);
root.Left = new Node(5);
root.Right = new Node(15);
root.Left.Left = new Node(3);
root.Left.Right = new Node(7);
root.Right.Right = new Node(20);
```

**Alternativně přes pole** (jak ukládá halda):
```csharp
int[] strom = new int[15];
// Index 0 = kořen
// Levý potomek uzlu i: 2*i + 1
// Pravý potomek uzlu i: 2*i + 2
// Rodič uzlu i: (i - 1) / 2
```
Tento přístup je úsporný pro úplné stromy (žádné odkazy), ale plýtvá pamětí pro řídké/nepravidelné stromy.

---

### Bod 3: Definice binárního vyhledávacího stromu (BVS)

**Binární vyhledávací strom (BVS, Binary Search Tree)** je binární strom s pravidlem uspořádání:

**ZLATÉ PRAVIDLO BVS:**
- Pro každý uzel platí:
  - **Všechny hodnoty v levém podstromu < hodnota uzlu**
  - **Všechny hodnoty v pravém podstromu > hodnota uzlu**

Pravidlo se vztahuje na **celé podstromy**, ne jen na bezprostřední děti.

```
        [10]
        /  \
       /    \
     [5]    [15]
     / \      \
    /   \      \
  [3]   [7]   [20]
```

Ověření: V levém podstromu kořene `[10]` jsou `{5, 3, 7}` – všechny menší než 10. V pravém podstromu `{15, 20}` – všechny větší.

**Klíčové důsledky BVS:**
- Vyhledávání trvá v průměru **O(log n)** v dobře vyváženém stromu.
- **In-order průchod** vypíše hodnoty **seřazené**.
- Minimum je v **nejlevějším** uzlu, maximum v **nejpravějším**.

**Implementace s automatickým Insert:**
```csharp
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

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    private Node InsertRecursive(Node current, int value)
    {
        if (current == null)
            return new Node(value);

        if (value < current.Data)
            current.Left = InsertRecursive(current.Left, value);
        else if (value > current.Data)
            current.Right = InsertRecursive(current.Right, value);
        // else: duplicita - obvykle nepřidáváme

        return current;
    }
}

// Použití:
BinarySearchTree bst = new BinarySearchTree();
bst.Insert(10);
bst.Insert(5);
bst.Insert(15);
bst.Insert(3);
bst.Insert(7);
bst.Insert(20);
```

**Časová složitost Insert/Search/Delete:**
- **Vyvážený BVS:** O(log n).
- **Degenerovaný BVS:** O(n) – strom zdegeneroval na spojový seznam.

**Problém degenerace:**
Při vkládání **seřazených** dat (1, 2, 3, 4, 5, …) BVS degeneruje:
```
[1]
  \
  [2]
    \
    [3]
      \
      [4]
        \
        [5]   → spojový seznam, hledání O(n)
```

Řešením jsou **samovyvažující stromy**:
- **AVL strom** – udržuje výškový rozdíl podstromů max 1.
- **Red-Black strom** – kompromis s méně rotacemi (použit v `SortedSet<T>` v .NET, v `std::map` v C++, v Linux scheduleru).
- **B-strom, B+ strom** – pro databáze a disky.

Tyto struktury garantují O(log n) i v nejhorším případě, ale vyžadují **rotace** po každém vložení/odstranění pro udržení rovnováhy.

---

### Bod 4: Algoritmus procházení libovolného (obecného) stromu

**Procházení (traversal)** = navštívit každý uzel **právě jednou**. Pro obecný strom (libovolný počet potomků) je nejpřirozenější **rekurzivní DFS**.

**Struktura uzlu obecného stromu:**
```csharp
class TreeNode
{
    public int Data;
    public List<TreeNode> Children;

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

**Procházení do hloubky (DFS) – rekurzivně:**
```csharp
void TraverseTree(TreeNode node)
{
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

**Příklad:**
```
        [A]
       / | \
     [B][C][D]
     / \    |
   [E] [F] [G]
```
Pořadí výpisu: **A B E F C D G** (pre-order na obecném stromu).

**Užitečné operace při průchodu:**
```csharp
// Součet hodnot
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

// Hloubka (výška) stromu
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

**Časová složitost:** **O(n)** – musíme navštívit každý uzel právě jednou. Nemůže to být rychlejší.
**Paměťová složitost:** **O(h)** kvůli rekurzi (h = výška stromu).

---

### Bod 5: Algoritmus hledání prvku v BVS

BVS umožňuje **rychlé vyhledávání** – v každém kroku **eliminujeme polovinu** zbývajícího stromu.

**Algoritmus:**
1. Začni v kořeni.
2. Porovnej hledanou hodnotu s aktuálním uzlem:
   - Pokud rovná – našli jsme.
   - Pokud menší – jdi **vlevo**.
   - Pokud větší – jdi **vpravo**.
3. Opakuj, dokud nenajdeš nebo nedojdeš na `null`.

**Vizualizace hledání 7:**
```
        [10]  ← 7 < 10 → jdi VLEVO
        /  \
       ↓
     [5]    [15]  ← 7 > 5 → jdi VPRAVO
     / \      \
        ↓
  [3]   [7]  [20]  ← 7 == 7 → NAŠLI
```

**Rekurzivní verze:**
```csharp
public bool Search(int value)
{
    return SearchRecursive(root, value);
}

private bool SearchRecursive(Node current, int value)
{
    if (current == null)
        return false;

    if (value == current.Data)
        return true;

    if (value < current.Data)
        return SearchRecursive(current.Left, value);
    else
        return SearchRecursive(current.Right, value);
}
```

**Iterativní verze (efektivnější, bez call-stack overhead):**
```csharp
public bool SearchIterative(int value)
{
    Node current = root;
    while (current != null)
    {
        if (value == current.Data)
            return true;

        if (value < current.Data)
            current = current.Left;
        else
            current = current.Right;
    }
    return false;
}
```

**Vrácení celého uzlu:**
```csharp
public Node Find(int value)
{
    Node current = root;
    while (current != null)
    {
        if (value == current.Data) return current;
        current = (value < current.Data) ? current.Left : current.Right;
    }
    return null;
}
```

**Časová složitost:**
- **Vyvážený BVS:** **O(log n)**.
- **Nevyvážený BVS:** O(n).

**Porovnání pro 1 000 prvků:**
- BVS (vyvážený): ~10 kroků
- Lineární hledání: ~500 kroků průměrně
- Binární vyhledávání v poli: ~10 kroků (ale pole musí být seřazené a vyhledávání pouze, ne snadná modifikace).

**Výhoda BVS oproti seřazenému poli:** Vložení/odstranění je O(log n), v poli O(n) (kvůli posunu).

---

### Bod 6: Průchod stromem do hloubky (DFS) a do šířky (BFS)

Dvě základní strategie procházení:

| Strategie | Princip | Datová struktura | Pořadí |
|-----------|---------|------------------|--------|
| **BFS** (Breadth-First Search) | Po úrovních ("po patrech") | **Fronta (FIFO)** | 1 → 2 → 3 → 4 → 5 |
| **DFS** (Depth-First Search) | Co nejhlouběji, pak návrat | **Zásobník (LIFO)** nebo rekurze | 1 → 2 → 4 → 5 → 3 |

**Příklad pro porovnání:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]
```
- **BFS:** 1 → 2 → 3 → 4 → 5 → 6 → 7 (po úrovních).
- **DFS pre-order:** 1 → 2 → 4 → 5 → 3 → 6 → 7 (do hloubky).

---

#### BFS – procházení do šířky

**Princip:** Projít všechny uzly na úrovni N dříve než přejdeme na úroveň N+1. Fronta funguje jako FIFO – první přidaný se vybere první.

**Algoritmus:**
1. Vlož kořen do fronty.
2. Dokud fronta není prázdná:
   - Vyber uzel z fronty.
   - Zpracuj ho.
   - Přidej všechny jeho potomky do fronty.

**Kód:**
```csharp
void BFS(Node root)
{
    if (root == null) return;

    Queue<Node> queue = new Queue<Node>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        Node current = queue.Dequeue();
        Console.Write(current.Data + " ");

        if (current.Left != null) queue.Enqueue(current.Left);
        if (current.Right != null) queue.Enqueue(current.Right);
    }
}
```

**Simulace krok po kroku pro výše uvedený strom:**
```
Krok 1: Fronta = [1]
  Dequeue → 1 → Enqueue [2, 3]
Krok 2: Fronta = [2, 3]
  Dequeue → 2 → Enqueue [4, 5]
Krok 3: Fronta = [3, 4, 5]
  Dequeue → 3 → Enqueue [6, 7]
Krok 4-7: Postupně 4, 5, 6, 7 (žádní potomci)
Výpis: 1 2 3 4 5 6 7
```

**BFS po úrovních (uvidíme strukturu stromu):**
```csharp
void BFSLevels(Node root)
{
    if (root == null) return;

    Queue<Node> queue = new Queue<Node>();
    queue.Enqueue(root);

    while (queue.Count > 0)
    {
        int levelSize = queue.Count;
        for (int i = 0; i < levelSize; i++)
        {
            Node current = queue.Dequeue();
            Console.Write(current.Data + " ");
            if (current.Left != null) queue.Enqueue(current.Left);
            if (current.Right != null) queue.Enqueue(current.Right);
        }
        Console.WriteLine();
    }
}

// Výstup:
// 1
// 2 3
// 4 5 6 7
```

**Použití BFS:**
- **Nejkratší cesta v neohodnoceném grafu** (počet hran, ne suma vah).
- Výpis úrovní stromu.
- Level-order serializace.
- Hledání nejbližšího řešení v nelineárním stavovém prostoru.

---

#### DFS – procházení do hloubky

**Princip:** Jdeme co nejhlouběji v jednom směru, pak se vracíme. Implicitní zásobník = call stack rekurze; explicitní zásobník pro iterativní verzi.

**Pro binární strom existují 3 varianty DFS podle pořadí zpracování uzlu:**
1. **Pre-order (N-L-R)** – Uzel **před** potomky.
2. **In-order (L-N-R)** – Uzel **mezi** levým a pravým podstromem.
3. **Post-order (L-R-N)** – Uzel **po** obou potomcích.

Klíč k pochopení: každé rekurzivní volání má 3 fáze (sestup, zpracování, návrat). Pořadí, kdy v rámci kroku "zpracujeme" uzel, určuje typ průchodu.

---

##### Pre-order (N-L-R)

```csharp
void PreOrder(Node node)
{
    if (node == null) return;

    Console.Write(node.Data + " ");   // 1. Zpracuj
    PreOrder(node.Left);              // 2. Levý
    PreOrder(node.Right);             // 3. Pravý
}
```

**Pořadí pro náš strom:** `1 2 4 5 3 6 7`.

**Použití:** kopírování stromu (kořen prve, pak děti), serializace, prefixový (polský) zápis výrazu.

---

##### In-order (L-N-R)

```csharp
void InOrder(Node node)
{
    if (node == null) return;

    InOrder(node.Left);                // 1. Levý
    Console.Write(node.Data + " ");    // 2. Zpracuj
    InOrder(node.Right);               // 3. Pravý
}
```

**Pořadí pro náš strom:** `4 2 5 1 6 3 7`.

**KLÍČOVÉ PRO BVS:** In-order výpis BVS dává **seřazené hodnoty**.

```
BVS:           [10]
              /    \
            [5]    [15]
            / \      \
          [3] [7]   [20]

In-order: 3 5 7 10 15 20  → SEŘAZENÉ
```

Proč? V BVS platí: pro každý uzel je celý jeho levý podstrom menší, pravý větší. In-order navštíví nejprve celý levý, pak kořen, pak celý pravý – přesně v pořadí hodnot.

**Použití:** seřazený výpis BVS, kontrola, zda binární strom je BVS, debug výpis.

---

##### Post-order (L-R-N)

```csharp
void PostOrder(Node node)
{
    if (node == null) return;

    PostOrder(node.Left);             // 1. Levý
    PostOrder(node.Right);            // 2. Pravý
    Console.Write(node.Data + " ");   // 3. Zpracuj
}
```

**Pořadí pro náš strom:** `4 5 2 6 7 3 1`.

**Použití:** mazání stromu (smazat děti **před** rodičem), výpočet velikosti/výšky stromu, postfixový (reverzní polský) zápis výrazu, vyhodnocení expression tree.

---

##### DFS iterativní (se zásobníkem)

```csharp
void DFSIterative(Node root)
{
    if (root == null) return;

    Stack<Node> stack = new Stack<Node>();
    stack.Push(root);

    while (stack.Count > 0)
    {
        Node current = stack.Pop();
        Console.Write(current.Data + " ");

        // POZOR: pravý PŘED levým (stack je LIFO)
        if (current.Right != null) stack.Push(current.Right);
        if (current.Left != null) stack.Push(current.Left);
    }
}
```

**Proč pravý před levým?** Zásobník je LIFO. Pokud chceme zpracovat levý před pravým (pre-order), musíme pravý strčit první, aby levý zůstal nahoře a vyběhl dříve.

---

#### Porovnání průchodů

| Průchod | Pořadí pro vzorový strom | Vzorec | Hlavní použití |
|---------|--------------------------|--------|----------------|
| **Pre-order** | 1 2 4 5 3 6 7 | N-L-R | Kopírování stromu, prefix výrazy, serializace |
| **In-order** | 4 2 5 1 6 3 7 | L-N-R | Seřazený výpis BVS |
| **Post-order** | 4 5 2 6 7 3 1 | L-R-N | Mazání stromu, postfix výrazy, výška stromu |
| **BFS** | 1 2 3 4 5 6 7 | Po úrovních | Nejkratší cesta, výpis po patrech |

#### Časová a paměťová složitost průchodů

| Průchod | Časová | Paměťová | Vysvětlení |
|---------|--------|----------|------------|
| **BFS** | O(n) | O(w) | w = max. šířka stromu (na poslední úrovni může být ~n/2) |
| **DFS rekurzivní** | O(n) | O(h) | h = výška stromu (hloubka call stacku) |
| **DFS iterativní** | O(n) | O(h) | h = výška stromu (velikost zásobníku) |

Pro **vyvážený strom** je h ≈ log n a w ≈ n/2 → DFS je paměťově efektivnější (O(log n) vs O(n)).
Pro **degenerovaný strom** (spojový seznam) je h = n a w = 1 → BFS je paměťově efektivnější.

---

### Bod 7: Co může být ve stromu uloženo

Strom je **generická struktura** – v uzlech může být **libovolný datový typ**. Pro BVS je jediná podmínka: data musí být **porovnatelná** (implementovat `IComparable<T>` nebo mít komparátor).

#### Čísla (int, double)
Klasický příklad. BVS čísel je výchozí učební model.
```csharp
class Node
{
    public int Data;
    public Node Left;
    public Node Right;
}
```
**Použití:** matematické výrazy, seřazení čísel, priority queue, indexy.

#### Textové řetězce (string)
Strings se porovnávají lexikograficky.
```csharp
class StringNode
{
    public string Data;
    public StringNode Left;
    public StringNode Right;

    public StringNode(string data)
    {
        Data = data;
    }
}

StringNode InsertRecursive(StringNode current, string word)
{
    if (current == null)
        return new StringNode(word);

    int cmp = string.Compare(word, current.Data);
    if (cmp < 0)
        current.Left = InsertRecursive(current.Left, word);
    else if (cmp > 0)
        current.Right = InsertRecursive(current.Right, word);

    return current;
}
```
**Příklad:**
```
        [Dog]
       /     \
   [Cat]    [Zebra]
   /   \
[Ant] [Cow]

In-order: Ant Cat Cow Dog Zebra (abecedně)
```
**Použití:** slovníky, autocomplete, vyhledávání ve slovech.

#### Vlastní třídy (objekty)
Pro vlastní typy implementujeme `IComparable<T>` nebo předáme `IComparer<T>`.
```csharp
class Student : IComparable<Student>
{
    public string Name;
    public int Age;
    public double GPA;

    public Student(string name, int age, double gpa)
    {
        Name = name;
        Age = age;
        GPA = gpa;
    }

    public int CompareTo(Student other)
    {
        return string.Compare(this.Name, other.Name);
    }
}
```
**Použití:** evidence studentů, zaměstnanců, produktů; databázové indexy.

#### Souborový systém (obecný strom)
```csharp
class FileNode
{
    public string Name;
    public bool IsDirectory;
    public List<FileNode> Children;

    public FileNode(string name, bool isDirectory)
    {
        Name = name;
        IsDirectory = isDirectory;
        Children = new List<FileNode>();
    }
}
```
**Struktura:**
```
        [C:\]
       /  |  \
[Users][Windows][Program Files]
   |
[Documents]
   |
[foto.jpg]
```
**Použití:** Explorer, file managery, virtuální FS.

#### Aritmetické výrazy (Expression Tree)
Strom, kde **listy jsou operandy** (čísla, proměnné) a **vnitřní uzly operátory** (+, −, *, /).
```csharp
class ExpressionNode
{
    public string Value;
    public ExpressionNode Left;
    public ExpressionNode Right;

    public ExpressionNode(string value)
    {
        Value = value;
    }
}
```

**Výraz `(3 + 5) * 2`:**
```
        [*]
       /   \
     [+]   [2]
     / \
   [3] [5]
```

**Vyhodnocení (Post-order):**
```csharp
int Evaluate(ExpressionNode node)
{
    if (node == null) return 0;

    if (node.Left == null && node.Right == null)
        return int.Parse(node.Value);

    int left = Evaluate(node.Left);
    int right = Evaluate(node.Right);

    switch (node.Value)
    {
        case "+": return left + right;
        case "-": return left - right;
        case "*": return left * right;
        case "/": return left / right;
        default:  return 0;
    }
}
```
**Použití:** kalkulačky, překladače (AST = Abstract Syntax Tree), formule v Excelu.

#### Klíčové principy
- **BVS** vyžaduje porovnatelnost. Primitivní typy a string fungují automaticky; pro vlastní třídy implementuj `IComparable<T>`.
- **Obecný strom** nemá omezení – cokoliv, žádné porovnávání není třeba.

---

### Bod 8: Co je halda a k čemu slouží

**Halda (Heap)** je speciální typ **úplného binárního stromu** s vlastností uspořádání **heap property**:
- **Min-heap:** každý rodič ≤ všech svých potomků → **minimum je v kořeni**.
- **Max-heap:** každý rodič ≥ všech svých potomků → **maximum je v kořeni**.

#### Vizualizace Min-heap a Max-heap

```
Min-heap:           Max-heap:
    [1]                [10]
   /   \              /    \
 [3]   [2]          [8]    [9]
 / \   / \          / \    / \
[7][5][8][6]      [3][5][6][7]
```

Ověření Min-heap: rodič `1` ≤ děti {3, 2}; rodič `3` ≤ {7, 5}; rodič `2` ≤ {8, 6}.

#### KRITICKÝ ROZDÍL: halda vs. BVS

**Halda NENÍ BVS!** Pravidla jsou jiná, účel je jiný.

| Vlastnost | BVS | Halda |
|-----------|-----|-------|
| **Pravidlo** | Levý < uzel < pravý (řazení) | Rodič ≤/≥ děti (heap property) |
| **Struktura** | Může být nevyvážená | Vždy úplný binární strom |
| **Min/max** | Min nejvíc vlevo, max vpravo | V kořeni |
| **In-order výpis** | Seřazený | NESEŘAZENÝ |
| **Hledání obecného prvku** | O(log n) | O(n) |
| **Účel** | Seřazení, vyhledávání | Rychlý přístup k min/max |

**Tedy: pokud chceš seřazený výpis, použij BVS, ne haldu. Halda umí jen rychle dát extrém.**

#### Uložení haldy v poli

**Halda se obvykle neukládá jako uzly s odkazy, ale v jednom poli!** Díky úplnému uspořádání lze hierarchii rekonstruovat z indexů.

```
Pole:  [1, 3, 2, 7, 5, 8, 6]
Index:  0  1  2  3  4  5  6

Strom:
        [1]         index 0
       /   \
     [3]   [2]      index 1, 2
     / \   / \
   [7][5][8][6]     index 3, 4, 5, 6
```

**Vzorce pro navigaci (uzel na indexu `i`):**
```csharp
int leftChild  = 2 * i + 1;
int rightChild = 2 * i + 2;
int parent     = (i - 1) / 2;
```

**Proč pole?**
- Žádné odkazy = úspora paměti.
- Souvislý blok = lepší cache lokalita.
- Jednoduchá implementace operací.
- Heap sort je in-place.

#### Operace v Min-heap

##### `GetMin()` – O(1)
Minimum je vždy v kořeni:
```csharp
public int GetMin()
{
    if (heap.Count == 0) throw new Exception("Halda je prázdná");
    return heap[0];
}
```

##### `Insert(value)` – O(log n)

**Algoritmus:**
1. Přidej prvek na **konec pole** (poslední list úplného stromu).
2. **Bubble Up** (probublej nahoru): pokud je menší než rodič, prohoď s ním a opakuj.

```csharp
public void Insert(int value)
{
    heap.Add(value);
    int index = heap.Count - 1;

    while (index > 0)
    {
        int parentIndex = (index - 1) / 2;
        if (heap[index] >= heap[parentIndex]) break;
        Swap(index, parentIndex);
        index = parentIndex;
    }
}

void Swap(int i, int j)
{
    int temp = heap[i];
    heap[i] = heap[j];
    heap[j] = temp;
}
```

**Příklad Insert(0):**
```
Před:      Po přidání na konec:    Po Bubble Up:
   [1]            [1]                    [0]
  /   \          /   \                  /   \
 [3]  [2]      [3]   [2]              [3]   [1]
 / \           / \   /                / \   /
[7][5]       [7][5][0]              [7][5][2]

(0 prohozeno s 2, pak s 1 → 0 je nový kořen)
```

##### `ExtractMin()` – O(log n)

**Algoritmus:**
1. Zapamatuj kořen (minimum).
2. Přesuň **poslední prvek** na pozici kořene.
3. **Bubble Down**: porovnej s menším z dětí; pokud je větší, prohoď a opakuj.
4. Vrať uložené minimum.

```csharp
public int ExtractMin()
{
    if (heap.Count == 0) throw new Exception("Halda je prázdná");

    int min = heap[0];
    heap[0] = heap[heap.Count - 1];
    heap.RemoveAt(heap.Count - 1);

    int index = 0;
    while (true)
    {
        int left = 2 * index + 1;
        int right = 2 * index + 2;
        int smallest = index;

        if (left < heap.Count && heap[left] < heap[smallest])
            smallest = left;
        if (right < heap.Count && heap[right] < heap[smallest])
            smallest = right;

        if (smallest == index) break;

        Swap(index, smallest);
        index = smallest;
    }
    return min;
}
```

##### `BuildHeap` – O(n)
Vytvořit haldu z neuspořádaného pole **najednou** lze v lineárním čase (heapify zdola). Postupné vkládání by trvalo O(n log n).

#### Časové složitosti operací haldy

| Operace | Složitost | Vysvětlení |
|---------|-----------|------------|
| `GetMin/Max` | O(1) | Kořen |
| `Insert` | O(log n) | Bubble Up max h kroků |
| `ExtractMin/Max` | O(log n) | Bubble Down max h kroků |
| `BuildHeap` | O(n) | Heapify zdola |
| Heap Sort | O(n log n) | n × ExtractMin |
| Hledání obecného prvku | O(n) | Nemá uspořádání pro lookup |

#### K čemu se halda používá

**1) Priority Queue (Fronta s prioritou)** – nejtypičtější použití. Vyber vždy prvek s nejvyšší prioritou.
- CPU scheduling (OS).
- Síťové routery (prioritní pakety).
- Nemocnice (triage podle urgence).
- Event handling (události podle času).

**2) Heap Sort** – O(n log n) třídění in-place, v praxi méně používané kvůli horší cache lokalitě než QuickSort/TimSort.

**3) Top-K problém** – najít K nejmenších/největších prvků z velkého datasetu.
```csharp
// K největších přes Min-heap velikosti K
MinHeap topK = new MinHeap(maxSize: K);
foreach (int num in data)
{
    if (topK.Count < K)
        topK.Insert(num);
    else if (num > topK.GetMin())
    {
        topK.ExtractMin();
        topK.Insert(num);
    }
}
```
Pro K největších použij **Min-heap velikosti K** (kořen je nejmenší z těch K – pokud nové číslo je větší, vyhoď kořen).

**4) Dijkstrův algoritmus** – výběr nejbližšího nezpracovaného vrcholu pomocí min-heap.

**5) A\* pathfinding** – min-heap podle f-skóre.

**6) Huffmanovo kódování** – sloučení dvou nejmenších stromů pomocí min-heap.

V .NET je `PriorityQueue<TElement, TPriority>` (od .NET 6) přesně tato struktura.

---

### Bod 9: Příklady využití stromů

**Databáze** – B-strom, B+ strom jsou základ databázových indexů (MySQL InnoDB, PostgreSQL, SQLite). Umožňují O(log n) vyhledávání i pro tabulky s miliardami záznamů, kde data nejsou celá v paměti.

**Souborový systém** – Adresářová struktura je obecný strom. NTFS, ext4, btrfs (B-tree FS) uvnitř používají B-stromy pro indexy souborů.

**HTML DOM** – Document Object Model je strom HTML elementů. Browser engine prochází DOM stromem pro renderování. JavaScript DOM API operuje nad tímto stromem.

**Kompilátory a parsery** – AST (Abstract Syntax Tree) reprezentuje strukturu programu. `if-then-else`, výrazy, definice funkcí – vše jsou uzly AST. Compiler nad AST provádí optimalizace, generuje kód.

**Game AI – Decision Trees** – stromová struktura rozhodování. Behavior trees v herním AI, Minimax strom pro šachy s alpha-beta ořezáním.

**Huffmanovo kódování** – komprese textu. Frekvence znaků → strom → kratší kód pro častější znaky. Použito v ZIP, JPEG, MP3.

**Routing v sítích** – BGP, OSPF používají stromové struktury pro směrovací tabulky. STP (Spanning Tree Protocol) vytváří strom z grafu propojení switchů.

**3D rendering** – BVH (Bounding Volume Hierarchy), KD-tree, Octree pro prostorové členění scény. Akcelerace ray tracingu.

**Strojové učení** – Decision Tree algoritmus (CART, ID3, C4.5). Random Forest = lesa rozhodovacích stromů. Gradient Boosting (XGBoost, LightGBM).

**Verzování (Git)** – commit graf je DAG (téměř strom). Merkle tree pro hashování obsahu, blockchain.

**Autocomplete a fulltext search** – Trie (prefixový strom) pro slovníky a autocomplete. Suffix tree pro fulltextové vyhledávání.

**Priority queues v OS** – process scheduling, I/O scheduling, network packet queueing – vše typicky implementováno přes haldy.

**Expression evaluation** – Excel formule, kalkulačky, scriptovací jazyky – AST nad výrazem, vyhodnocení Post-order průchodem.

**Genealogie** – rodokmen je v zásadě obrácený binární strom (každý člověk má 2 rodiče, kteří mají také 2 rodiče, atd.).

---

### Bod 10: Možný způsob implementace

Implementace generického BVS s plnou sadou operací:

```csharp
class BinarySearchTree<T> where T : IComparable<T>
{
    private class Node
    {
        public T Data;
        public Node Left;
        public Node Right;

        public Node(T data)
        {
            Data = data;
        }
    }

    private Node root;
    public int Count { get; private set; }

    public void Insert(T value)
    {
        root = InsertRec(root, value);
    }

    private Node InsertRec(Node current, T value)
    {
        if (current == null)
        {
            Count++;
            return new Node(value);
        }

        int cmp = value.CompareTo(current.Data);
        if (cmp < 0)
            current.Left = InsertRec(current.Left, value);
        else if (cmp > 0)
            current.Right = InsertRec(current.Right, value);
        // cmp == 0 → duplicita, ignoruj

        return current;
    }

    public bool Contains(T value)
    {
        Node current = root;
        while (current != null)
        {
            int cmp = value.CompareTo(current.Data);
            if (cmp == 0) return true;
            current = (cmp < 0) ? current.Left : current.Right;
        }
        return false;
    }

    public T FindMin()
    {
        if (root == null) throw new InvalidOperationException("Strom je prázdný");
        Node current = root;
        while (current.Left != null) current = current.Left;
        return current.Data;
    }

    public T FindMax()
    {
        if (root == null) throw new InvalidOperationException("Strom je prázdný");
        Node current = root;
        while (current.Right != null) current = current.Right;
        return current.Data;
    }

    public void Remove(T value)
    {
        root = RemoveRec(root, value);
    }

    private Node RemoveRec(Node current, T value)
    {
        if (current == null) return null;

        int cmp = value.CompareTo(current.Data);
        if (cmp < 0)
            current.Left = RemoveRec(current.Left, value);
        else if (cmp > 0)
            current.Right = RemoveRec(current.Right, value);
        else
        {
            // Nalezeno - 3 případy mazání:
            // 1) Žádný potomek → odstraň
            if (current.Left == null && current.Right == null)
            {
                Count--;
                return null;
            }
            // 2) Jeden potomek → nahraď tím potomkem
            if (current.Left == null) { Count--; return current.Right; }
            if (current.Right == null) { Count--; return current.Left; }

            // 3) Dva potomci → najdi následníka (min v pravém podstromu),
            //    nahraď, smaž následníka
            Node successor = current.Right;
            while (successor.Left != null) successor = successor.Left;
            current.Data = successor.Data;
            current.Right = RemoveRec(current.Right, successor.Data);
        }
        return current;
    }

    public IEnumerable<T> InOrder()
    {
        return InOrderRec(root);
    }

    private IEnumerable<T> InOrderRec(Node node)
    {
        if (node == null) yield break;
        foreach (T x in InOrderRec(node.Left)) yield return x;
        yield return node.Data;
        foreach (T x in InOrderRec(node.Right)) yield return x;
    }

    public int Height()
    {
        return HeightRec(root);
    }

    private int HeightRec(Node node)
    {
        if (node == null) return -1;   // prázdný strom má výšku -1, list 0
        return 1 + Math.Max(HeightRec(node.Left), HeightRec(node.Right));
    }
}

// Použití
var bst = new BinarySearchTree<int>();
bst.Insert(10);
bst.Insert(5);
bst.Insert(15);
foreach (int x in bst.InOrder())
    Console.WriteLine(x);
```

**Mazání uzlu** je nejnetriviálnější operace – tři případy:
1. **Žádný potomek (list)** – prostě odstraň.
2. **Jeden potomek** – nahraď uzel jeho potomkem.
3. **Dva potomci** – najdi **in-order následníka** (minimum v pravém podstromu) nebo **předchůdce** (maximum v levém), nahraď jeho hodnotou a smaž následníka.

**Vyvážené stromy (AVL, Red-Black)** přidávají po každé modifikaci **rotace** pro udržení O(log n) výšky.

**V .NET nepiš vlastní BVS do produkce:**
- `SortedSet<T>` – Red-Black tree.
- `SortedDictionary<TKey, TValue>` – Red-Black tree.
- `PriorityQueue<TElement, TPriority>` – Min-heap (.NET 6+).

Pro maturitu je třeba **umět implementovat vlastní BVS** s Insert/Search/Traversal, znát haldu a její vzorce navigace v poli, a popsat operace.

---

## Maturitní chytáky

### Při definicích
- **Strom vs. graf:** strom NEOBSAHUJE cykly, je souvislý acyklický graf.
- **Binární strom:** max 2 potomci, pořadí záleží (levý ≠ pravý).
- **Obecný strom:** libovolný počet potomků.
- **BVS pravidlo:** **všechny** hodnoty v levém podstromu < uzel < **všechny** v pravém. Neplatí jen pro bezprostřední děti!
- **Halda pravidlo:** rodič ≤/≥ děti (jiné než BVS!).
- **Strom má `V − 1` hran**, halda je úplný binární strom.

### Při implementaci
- **Null kontrola:** vždy testuj, jestli uzel není null, jinak NullReferenceException.
- **Rekurze:** základní případ = uzel null → return.
- **BVS Insert:** rekurzivně nebo while-cyklem; neukládat ručně `root.Left = ...` zvenku.
- **Duplicity:** rozhodnout, zda je povolíš (typicky ne).
- **Halda v poli:** používej vzorce `2i+1`, `2i+2`, `(i-1)/2`, NE odkazy.

### Časová složitost
- **Vyvážený vs. nevyvážený:** O(log n) vs. O(n).
- **Procházení:** vždy O(n) – musíme navštívit všechny uzly.
- **Hledání v BVS:** O(log n) **pouze** pokud je strom vyvážený.
- **Halda:** GetMin O(1), Insert/Extract O(log n), BuildHeap O(n).

### Průchody
- **Pre-order (N-L-R):** uzel před potomky.
- **In-order (L-N-R):** uzel mezi – **BVS seřazené!**
- **Post-order (L-R-N):** uzel po potomcích.
- **BFS:** fronta, po úrovních.
- **DFS:** zásobník nebo rekurze, do hloubky.
- **In-order na haldě:** NESEŘAZENÉ (častá chyba).

### Halda vs. BVS
- **Halda NENÍ BVS!** Pravidla i účel jsou jiné.
- **Halda:** rychlý přístup k min/max (priority queue).
- **BVS:** seřazený výpis, vyhledávání.
- **Halda v poli:** ne uzly s odkazy.
- **Bubble Up/Down:** porovnávat s rodičem/dětmi správným směrem.

### Při ústní zkoušce
- Umět nakreslit příklad stromu na tabuli.
- Vysvětlit průchod krok po kroku s ukazováním.
- Ukázat, jak Insert v BVS najde místo automaticky.
- Porovnat BVS hledání s lineárním a binárním vyhledáváním.
- Vysvětlit rozdíl DFS vs. BFS.
- Nakreslit Bubble Up/Down v haldě.
- Vysvětlit, proč In-order vypíše BVS seřazené.
- Ukázat vzorce navigace v haldě (`2i+1`, `2i+2`, `(i-1)/2`).
- Diskutovat **problém degenerace BVS** a zmínit AVL/Red-Black jako řešení.

---

## Praktické tipy a poznámky

### Iterativní vs. rekurzivní
**Rekurzivní:** elegantní, kratší, přirozené pro stromové struktury; spotřebovává call stack, riziko StackOverflow u velkých stromů.

**Iterativní:** šetří paměť, rychlejší (bez rekurzivní režie); delší kód.

Pro maturitu znát obě – rekurzivní pro vysvětlení, iterativní pro hlubší pochopení.

### Kdy použít který průchod

| Scénář | Průchod | Proč |
|--------|---------|------|
| Seřazený výpis BVS | In-order | Vypíše vzestupně |
| Kopírování stromu | Pre-order | Rodič před dětmi |
| Mazání stromu | Post-order | Děti před rodičem |
| Nejkratší cesta v neohodnoceném grafu | BFS | Po úrovních |
| Hledání cesty / existence | DFS | Méně paměti |
| Vyhodnocení výrazu | Post-order | Operandy před operátorem |

### Nevyvážený BVS problém
```
Vložení v pořadí: 1, 2, 3, 4, 5
[1] → [1]→[2] → [1]→[2]→[3] → spojový seznam, O(n)
```
**Řešení:** AVL (přísně vyvážený, časté rotace), Red-Black (volnější, méně rotací).

### Halda – praktické tipy
- **Build Heap O(n)** je rychlejší než postupné vkládání O(n log n).
- **Max-heap vs. Min-heap:** stačí změnit porovnání (`<` vs `>`).
- **K největších:** použij **Min-heap velikosti K** (NE Max-heap!). Kořen = "hranice" pro vyhazování.
- **PriorityQueue v .NET** podporuje `(element, priority)` páry – přesně to, co potřebuješ pro Dijkstru.

---

## Souvislosti s jinými otázkami

- **Otázka 2 (Spojové struktury):** strom je spojová struktura (uzly s odkazy), halda používá pole.
- **Otázka 3 (Fronta a zásobník):** BFS používá frontu, DFS používá zásobník (nebo rekurzi = implicitní zásobník).
- **Otázka 5 (Rekurze):** procházení stromu je klasický příklad rekurze; Post-order pro výpočet vlastností.
- **Otázka 6 (Práce se soubory):** souborový systém je strom.
- **Otázka 7 (Časová složitost):** O(log n) vs. O(n) podle vyváženosti; halda O(log n) operace.
- **Otázka 10–13 (Třídění):** Heap Sort, Tree Sort.
- **Otázka 14 (Vyhledávání):** BVS kombinuje rychlost binárního vyhledávání s flexibilitou dynamického vkládání.
- **Otázka 15 (Rozděl a panuj):** divide-and-conquer často kreslí rekurzivní strom volání.
- **Otázka 16 (Aritmetické výrazy):** Expression Tree, Post-order vyhodnocení.
- **Otázka 17–18 (OOP):** implementace přes třídy Node/Tree, generika, dědičnost.
- **Otázka 20 (Událostmi řízené):** event scheduling = priority queue (halda).
- **Otázka 21 (Teorie grafů):** strom = souvislý acyklický graf.
- **Otázka 22 (DFS/BFS):** stromy jsou nejjednodušší případ DFS/BFS – na grafu jde o totéž s doplněním visited setu.
- **Otázka 25 (Dijkstra):** min-heap pro výběr nejbližšího vrcholu.

---

## Klíčové pojmy k zapamatování

- **Strom** – souvislý acyklický graf; hierarchická struktura.
- **Kořen, list, vnitřní uzel, hloubka, výška** – základní pojmy.
- **`V − 1` hran** ve stromu s V uzly; mezi dvěma uzly **právě jedna cesta**.
- **Binární strom** – max 2 potomci, pořadí (levý/pravý) záleží.
- **Úplný binární strom** – plné úrovně kromě poslední (zaplněna zleva).
- **Plný binární strom** – uzel má 0 nebo 2 potomky.
- **Perfektní binární strom** – plný + všechny listy na stejné úrovni.
- **Degenerovaný strom** – výška n, defakto spojový seznam.
- **BVS** – binární strom s pravidlem "levý podstrom < uzel < pravý podstrom".
- **In-order výpis BVS** – seřazené hodnoty (vzestupně).
- **Rotace** – základní operace samovyvažujících stromů.
- **AVL strom, Red-Black strom** – samovyvažující BVS, garantují O(log n).
- **B-strom, B+ strom** – m-ární stromy pro databáze a souborové systémy.
- **DFS (Depth-First Search)** – do hloubky; zásobník/rekurze.
- **BFS (Breadth-First Search)** – do šířky; fronta.
- **Pre-order (N-L-R)** – uzel před potomky.
- **In-order (L-N-R)** – uzel mezi potomky.
- **Post-order (L-R-N)** – uzel po potomcích.
- **Halda (Heap)** – úplný binární strom s heap property; min-heap nebo max-heap.
- **Heap property** – rodič ≤/≥ všech potomků (NE celé podstromy jako u BVS).
- **Halda v poli** – navigace přes `2i+1`, `2i+2`, `(i-1)/2`.
- **Bubble Up / Bubble Down** – udržení heap property po Insert / ExtractMin.
- **Priority Queue** – ADT s prioritním výběrem; implementuje se haldou.
- **Heap Sort** – O(n log n) třídění in-place pomocí max-heap.
- **Expression Tree (AST)** – strom aritmetického výrazu; vyhodnocení Post-order.
- **Trie (prefixový strom)** – strom pro řetězce, větvení podle znaků.
- **Huffmanův strom** – komprese textu, binární strom kódů.
- **`SortedSet<T>`, `SortedDictionary<TK,TV>`** – Red-Black implementace v .NET.
- **`PriorityQueue<T,P>`** – min-heap implementace v .NET 6+.
