# 📚 Zápisky: Otázka č. 9 - Stromy a jejich využití. Průchod stromem
**Datum:** 2025-02-16  
**Status:** 🚧 Rozpracováno (Body 1-8 hotovo, 9-10 zbývá)

---

## ✅ Checklist bodů otázky
- [x] Bod 1: Definice stromu
- [x] Bod 2: Definice binárního stromu
- [x] Bod 3: Definice binárního vyhledávacího stromu (BVS)
- [x] Bod 4: Algoritmus procházení libovolného stromu
- [x] Bod 5: Algoritmus hledání prvku v BVS
- [x] Bod 6: Průchod stromem do hloubky (DFS) a do šířky (BFS)
- [x] Bod 7: Co může být ve stromu uloženo
- [x] Bod 8: Co je halda a k čemu slouží
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

### Bod 6: Průchod stromem do hloubky (DFS) a do šířky (BFS)

**Teorie:**
- Existují **2 základní strategie** procházení stromů
- **DFS (Depth-First Search)** - Procházení do hloubky
  - Jdeme co nejhlouběji, pak se vracíme
  - Používá **zásobník** (Stack) nebo rekurzi
- **BFS (Breadth-First Search)** - Procházení do šířky
  - Procházíme po úrovních (po "patrech")
  - Používá **frontu** (Queue)

**Příklad stromu:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]
```

**DFS pořadí:** 1 → 2 → 4 → 5 → 3 → 6 → 7 (jdeme dolů, pak se vracíme)  
**BFS pořadí:** 1 → 2 → 3 → 4 → 5 → 6 → 7 (úroveň po úrovni)

---

#### 🌊 BFS - Procházení do šířky

**Princip:**
- Projdeme všechny uzly na úrovni N dříve, než přejdeme na úroveň N+1
- Používáme **frontu (Queue)** - FIFO (First In, First Out)

**Algoritmus:**
1. Vlož kořen do fronty
2. Dokud fronta není prázdná:
   - Vyndej uzel z fronty
   - Zpracuj ho (vypiš)
   - Přidej všechny jeho potomky do fronty

**Kód (Maturitní verze):**
```csharp
// ✅ VERZE A - MATURITNÍ
// BFS - procházení do šířky

void BFS(Node root)
{
    if (root == null)
        return;
    
    // Vytvoříme frontu a vložíme kořen
    Queue<Node> queue = new Queue<Node>();
    queue.Enqueue(root);
    
    // Dokud je ve frontě něco
    while (queue.Count > 0)
    {
        // Vybereme uzel z fronty
        Node current = queue.Dequeue();
        
        // Zpracujeme ho (vypiseme)
        Console.Write(current.Data + " ");
        
        // Přidáme jeho potomky do fronty
        if (current.Left != null)
            queue.Enqueue(current.Left);
        
        if (current.Right != null)
            queue.Enqueue(current.Right);
    }
}
```

**Simulace BFS krok po kroku:**
```
Strom:
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]

Krok 1: Fronta = [1]
        Dequeue → zpracuj 1 → Enqueue [2, 3]
        Výpis: 1

Krok 2: Fronta = [2, 3]
        Dequeue → zpracuj 2 → Enqueue [4, 5]
        Výpis: 1 2

Krok 3: Fronta = [3, 4, 5]
        Dequeue → zpracuj 3 → Enqueue [6, 7]
        Výpis: 1 2 3

Krok 4: Fronta = [4, 5, 6, 7]
        Dequeue → zpracuj 4 (nemá potomky)
        Výpis: 1 2 3 4

Krok 5: Fronta = [5, 6, 7]
        Dequeue → zpracuj 5
        Výpis: 1 2 3 4 5

Krok 6: Fronta = [6, 7]
        Dequeue → zpracuj 6
        Výpis: 1 2 3 4 5 6

Krok 7: Fronta = [7]
        Dequeue → zpracuj 7
        Výpis: 1 2 3 4 5 6 7

Konec - fronta je prázdná!
```

**BFS s výpisem po úrovních:**
```csharp
void BFSLevels(Node root)
{
    if (root == null)
        return;
    
    Queue<Node> queue = new Queue<Node>();
    queue.Enqueue(root);
    
    while (queue.Count > 0)
    {
        // Zjistíme, kolik uzlů je na aktuální úrovni
        int levelSize = queue.Count;
        
        // Projdeme všechny uzly na této úrovni
        for (int i = 0; i < levelSize; i++)
        {
            Node current = queue.Dequeue();
            Console.Write(current.Data + " ");
            
            if (current.Left != null)
                queue.Enqueue(current.Left);
            
            if (current.Right != null)
                queue.Enqueue(current.Right);
        }
        
        // Konec úrovně - nový řádek
        Console.WriteLine();
    }
}

// Výstup:
// 1 
// 2 3 
// 4 5 6 7
```

---

#### 🏔️ DFS - Procházení do hloubky

**Princip:**
- Jdeme co nejhlouběji v jednom směru, pak se vracíme
- Používáme **zásobník (Stack)** nebo **rekurzi**
- Pro binární strom máme **3 varianty**:
  1. **Pre-order** (N-L-R) - Uzel, pak potomci
  2. **In-order** (L-N-R) - Levý, uzel, pravý
  3. **Post-order** (L-R-N) - Potomci, pak uzel

**Klíč k pochopení:**
- **Rekurze má 3 fáze:** Sestup dolů → Zpracování → Návrat zpět
- **Pořadí těchto fází určuje typ průchodu!**

---

##### Pre-order DFS (N-L-R)

**Zpracuj uzel PŘED potomky**

**Kód:**
```csharp
// ✅ VERZE A - MATURITNÍ
// Pre-order: N-L-R

void PreOrder(Node node)
{
    if (node == null)
        return;
    
    // 1. Zpracuj aktuální uzel
    Console.Write(node.Data + " ");
    
    // 2. Projdi levý podstrom
    PreOrder(node.Left);
    
    // 3. Projdi pravý podstrom
    PreOrder(node.Right);
}
```

**Příklad:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]

Pre-order: 1 2 4 5 3 6 7

Postup:
1. Vypíšu 1 → jdu do 2
2. Vypíšu 2 → jdu do 4
3. Vypíšu 4 → návrat
4. Jdu do 5 → vypíšu 5 → návrat
5. Návrat do 1 → jdu do 3
6. Vypíšu 3 → jdu do 6
7. Vypíšu 6 → návrat
8. Jdu do 7 → vypíšu 7
```

**Použití:** Kopírování stromu, prefix zápis výrazu

---

##### In-order DFS (L-N-R)

**Zpracuj uzel MEZI potomky**

**Kód:**
```csharp
// ✅ VERZE A - MATURITNÍ
// In-order: L-N-R

void InOrder(Node node)
{
    if (node == null)
        return;
    
    // 1. Projdi levý podstrom
    InOrder(node.Left);
    
    // 2. Zpracuj aktuální uzel
    Console.Write(node.Data + " ");
    
    // 3. Projdi pravý podstrom
    InOrder(node.Right);
}
```

**Příklad:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]

In-order: 4 2 5 1 6 3 7

Postup:
1. Jdu do 2 (NEVYPISUJI 1!)
2. Jdu do 4 (NEVYPISUJI 2!)
3. Levý null → vypíšu 4 → návrat
4. Zpět v 2 → vypíšu 2
5. Jdu do 5 → vypíšu 5 → návrat
6. Zpět v 1 → vypíšu 1
7. Jdu do 3 → jdu do 6 → vypíšu 6
8. Zpět v 3 → vypíšu 3
9. Jdu do 7 → vypíšu 7
```

**✨ KLÍČOVÉ pro BVS:**
```
BVS:
        [10]
       /    \
     [5]    [15]
     / \      \
   [3] [7]   [20]

In-order výpis: 3 5 7 10 15 20  ← SEŘAZENÉ! ✅
```

**Proč?** BVS má pravidlo: levý < uzel < pravý  
→ In-order (L-N-R) vypíše přirozeně **seřazené hodnoty**!

**Použití:** Získání seřazených hodnot z BVS (nejdůležitější použití!)

---

##### Post-order DFS (L-R-N)

**Zpracuj uzel PO potomcích**

**Kód:**
```csharp
// ✅ VERZE A - MATURITNÍ
// Post-order: L-R-N

void PostOrder(Node node)
{
    if (node == null)
        return;
    
    // 1. Projdi levý podstrom
    PostOrder(node.Left);
    
    // 2. Projdi pravý podstrom
    PostOrder(node.Right);
    
    // 3. Zpracuj aktuální uzel
    Console.Write(node.Data + " ");
}
```

**Příklad:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]

Post-order: 4 5 2 6 7 3 1

Postup:
1. Jdu do 2 → jdu do 4
2. Obě děti 4 null → vypíšu 4
3. Zpět v 2 → jdu do 5 → vypíšu 5
4. Zpět v 2 → OBĚ děti hotové → vypíšu 2
5. Zpět v 1 → jdu do 3 → jdu do 6 → vypíšu 6
6. Zpět v 3 → jdu do 7 → vypíšu 7
7. Zpět v 3 → OBĚ děti hotové → vypíšu 3
8. Zpět v 1 → OBĚ děti hotové → vypíšu 1
```

**Použití:** Mazání stromu (smažeme děti před rodičem), postfix zápis výrazu, výpočet výšky stromu

---

#### 📊 Porovnání všech průchodů

**Stejný strom:**
```
        [1]
       /   \
     [2]   [3]
     / \   / \
   [4][5][6][7]
```

| Průchod | Pořadí | Vzorec | Kdy použít |
|---------|--------|--------|------------|
| **Pre-order** | 1 2 4 5 3 6 7 | N-L-R | Kopírování stromu, prefix výrazy |
| **In-order** | 4 2 5 1 6 3 7 | L-N-R | **Seřazený výpis BVS!** |
| **Post-order** | 4 5 2 6 7 3 1 | L-R-N | Mazání stromu, postfix výrazy |
| **BFS** | 1 2 3 4 5 6 7 | Po úrovních | Nejkratší cesta, úrovně stromu |

---

#### 🔄 DFS iterativní (se zásobníkem)

```csharp
// ✅ VERZE A - MATURITNÍ
// DFS iterativní verze (Pre-order)

void DFSIterative(Node root)
{
    if (root == null)
        return;
    
    Stack<Node> stack = new Stack<Node>();
    stack.Push(root);
    
    while (stack.Count > 0)
    {
        Node current = stack.Pop();
        Console.Write(current.Data + " ");
        
        // POZOR: Pravý PŘED levým (stack je LIFO)
        if (current.Right != null)
            stack.Push(current.Right);
        
        if (current.Left != null)
            stack.Push(current.Left);
    }
}
```

**Proč pravý před levým?**
- Stack je **LIFO** (Last In, First Out)
- Chceme zpracovat levý PŘED pravým
- Musíme pravý vložit PŘED levým, aby levý byl navrchu!

---

#### ⏱️ Časová a paměťová složitost

| Průchod | Časová složitost | Paměťová složitost | Vysvětlení |
|---------|------------------|-------------------|------------|
| **BFS** | O(n) | O(w) | w = šířka stromu (max. počet uzlů na úrovni) |
| **DFS (rekurzivní)** | O(n) | O(h) | h = výška stromu (hloubka call stacku) |
| **DFS (iterativní)** | O(n) | O(h) | h = výška stromu (velikost stacku) |

**Pro vyvážený strom:**
- h ≈ log n (výška)
- w ≈ n/2 (max. šířka na poslední úrovni)

**Pro nevyvážený strom (spojový seznam):**
- h = n (výška)
- w = 1 (šířka)

---

### Bod 7: Co může být ve stromu uloženo

**Teorie:**
- Ve stromu může být uložen **JAKÝKOLI datový typ**!
- Jednoduché typy: int, string, double, char
- Složené typy: vlastní třídy, struktury
- **Podmínka pro BVS:** Data musí být **porovnatelná** (aby fungoval Insert/Search)

---

#### 1️⃣ Čísla (int, double)

```csharp
// ✅ Nejjednodušší
class Node
{
    public int Data;
    public Node Left;
    public Node Right;
}
```

**Použití:** Matematické výrazy, seřazování čísel, priority queue

---

#### 2️⃣ Textové řetězce (string)

```csharp
// ✅ VERZE A - MATURITNÍ
// BVS pro slova

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

// Insert - porovnávání stringů
StringNode InsertRecursive(StringNode current, string word)
{
    if (current == null)
        return new StringNode(word);
    
    int comparison = string.Compare(word, current.Data);
    
    if (comparison < 0)
        current.Left = InsertRecursive(current.Left, word);
    else if (comparison > 0)
        current.Right = InsertRecursive(current.Right, word);
    
    return current;
}
```

**Příklad BVS slov:**
```
        [Dog]
       /     \
   [Cat]     [Zebra]
   /   \
[Ant] [Cow]

In-order výpis: Ant Cat Cow Dog Zebra (abecedně!)
```

**Použití:** Slovníky, vyhledávání v textu, autocomplete

---

#### 3️⃣ Vlastní třídy/objekty

```csharp
// ✅ VERZE A - MATURITNÍ
// Třída Student implementující IComparable

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
    
    // Určíme, podle čeho porovnávat (např. jméno)
    public int CompareTo(Student other)
    {
        return string.Compare(this.Name, other.Name);
    }
}

// Uzel stromu
class StudentNode
{
    public Student Data;
    public StudentNode Left;
    public StudentNode Right;
    
    public StudentNode(Student data)
    {
        Data = data;
    }
}
```

**Strom studentů:**
```
        [Student: "David", 20, 3.5]
       /                           \
[Student: "Alice", 19, 3.8]    [Student: "Martin", 21, 3.2]
```

**Použití:** Evidence studentů, databázové indexy, třídění záznamů

---

#### 4️⃣ Souborový systém (obecný strom)

```csharp
// ✅ VERZE A - MATURITNÍ
// Obecný strom (ne BVS!)

class FileNode
{
    public string Name;
    public bool IsDirectory;
    public List<FileNode> Children;  // N potomků!
    
    public FileNode(string name, bool isDirectory)
    {
        Name = name;
        IsDirectory = isDirectory;
        Children = new List<FileNode>();
    }
}
```

**Příklad:**
```
        [C:\]
       /  |  \
      /   |   \
[Users][Windows][Program Files]
   |
[Documents]
   |
[foto.jpg]
```

**Použití:** Souborový systém (Explorer), organizace složek

---

#### 5️⃣ Aritmetické výrazy (Expression Tree)

```csharp
// ✅ VERZE A - MATURITNÍ
// Strom pro matematický výraz

class ExpressionNode
{
    public string Value;  // Číslo nebo operátor (+, -, *, /)
    public ExpressionNode Left;
    public ExpressionNode Right;
    
    public ExpressionNode(string value)
    {
        Value = value;
    }
}
```

**Příklad: (3 + 5) * 2**
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
    if (node == null)
        return 0;
    
    // List (číslo) → vrátíme hodnotu
    if (node.Left == null && node.Right == null)
        return int.Parse(node.Value);
    
    // Rekurzivně vypočítáme levý a pravý
    int leftValue = Evaluate(node.Left);
    int rightValue = Evaluate(node.Right);
    
    // Aplikujeme operátor
    switch (node.Value)
    {
        case "+": return leftValue + rightValue;
        case "-": return leftValue - rightValue;
        case "*": return leftValue * rightValue;
        case "/": return leftValue / rightValue;
        default: return 0;
    }
}

// Evaluate(root) → 3 + 5 = 8, 8 * 2 = 16
```

**Použití:** Kalkulačky, překladače (parsování výrazů), vyhodnocování formulí

---

#### 🎯 Klíčové principy

**Pro BVS:**
- Musí existovat pravidlo porovnání!
- Jednoduché typy (int, string) fungují automaticky
- Vlastní třídy: implementuj `IComparable<T>`

**Pro obecný strom:**
- Žádné omezení - cokoli!
- Nepotřebuješ porovnávání
- Příklady: souborový systém, DOM strom, organizační struktura

---

### Bod 8: Co je halda a k čemu slouží

**Teorie:**
- **Halda (Heap)** = speciální typ **binárního stromu**
- **2 klíčové vlastnosti:**
  1. **Úplný binární strom** - všechny úrovně plně zaplněné kromě poslední (ta zleva)
  2. **Heap property** - pravidlo uspořádání:
     - **Min-heap:** Rodič ≤ všechny potomky (minimum v kořeni)
     - **Max-heap:** Rodič ≥ všechny potomky (maximum v kořeni)

---

#### 📊 Vizualizace Min-Heap

```
        [1]         ← Nejmenší (kořen)
       /   \
     [3]   [2]      ← Rodiče menší než děti ✅
     / \   / \
   [7][5][8][6]     ← 3≤7, 3≤5, 2≤8, 2≤6 ✅
```

**Kontrola pravidla:**
- Rodič `1`: děti {3, 2} → 1 ≤ 3 ✅, 1 ≤ 2 ✅
- Rodič `3`: děti {7, 5} → 3 ≤ 7 ✅, 3 ≤ 5 ✅
- Rodič `2`: děti {8, 6} → 2 ≤ 8 ✅, 2 ≤ 6 ✅

---

#### 📊 Vizualizace Max-Heap

```
        [10]        ← Největší (kořen)
       /    \
     [8]    [9]     ← Rodiče větší než děti ✅
     / \    / \
   [3][5] [6][7]    ← 8≥3, 8≥5, 9≥6, 9≥7 ✅
```

---

#### ⚠️ KRITICKÝ ROZDÍL: Halda vs BVS

**POZOR! Halda NENÍ BVS!**

| Vlastnost | BVS | Halda |
|-----------|-----|-------|
| **Pravidlo** | Levý < uzel < pravý | Rodič ≤/≥ děti |
| **Struktura** | Může být nevyvážená | Vždy úplný bin. strom |
| **Minimum** | Zcela vlevo | V kořeni! ✅ |
| **In-order** | Seřazené ✅ | NESEŘAZENÉ ❌ |
| **Účel** | Seřazená data, vyhledávání | Priority queue, rychlý přístup k min/max |

**Příklad rozdílu:**
```
BVS:                    Min-Heap:
    [5]                     [1]
   /   \                   /   \
 [3]   [7]               [3]   [2]
                        /  \
                      [7]  [5]

In-order BVS: 3 5 7 (seřazené ✅)
In-order Heap: 7 3 1 5 2 (NESEŘAZENÉ ❌)
```

**Klíčová pointa:**
- **BVS** → pro seřazený výpis, vyhledávání
- **Halda** → pro rychlý přístup k minimu/maximu, NE pro seřazený výpis!

---

#### 💾 Uložení haldy V POLI

**🎯 KLÍČOVÉ:** Halda se NEUKLÁDÁ pomocí uzlů s odkazy, ale **V POLI!**

```csharp
// ✅ VERZE A - MATURITNÍ
// Halda jako pole

int[] heap = {1, 3, 2, 7, 5, 8, 6};
```

**Vizualizace:**
```
Pole:  [1, 3, 2, 7, 5, 8, 6]
Index:  0  1  2  3  4  5  6

Strom:
        [1]         Index 0
       /   \
     [3]   [2]      Index 1, 2
     / \   / \
   [7][5][8][6]     Index 3, 4, 5, 6
```

**Proč pole?**
- ✅ Úspora paměti (žádné odkazy Left/Right)
- ✅ Rychlejší přístup (cache-friendly)
- ✅ Jednodušší implementace operací

---

#### 🧮 Vzorce pro navigaci

**Pro uzel na indexu `i`:**

```csharp
// Levý potomek
int leftChild = 2 * i + 1;

// Pravý potomek
int rightChild = 2 * i + 2;

// Rodič
int parent = (i - 1) / 2;
```

**Příklad:**
```
Uzel na indexu 1 (hodnota 3):
- Levý: 2*1+1 = 3 → heap[3] = 7 ✅
- Pravý: 2*1+2 = 4 → heap[4] = 5 ✅
- Rodič: (1-1)/2 = 0 → heap[0] = 1 ✅
```

---

#### 🔧 Základní operace v Min-Heap

##### 1️⃣ GetMin() - Získání minima

```csharp
// ✅ VERZE A - MATURITNÍ
// Vrátí minimum (kořen)

public int GetMin()
{
    if (heap.Count == 0)
        throw new Exception("Halda je prázdná");
    
    return heap[0]; // Minimum je vždy v kořeni!
}
```

**Časová složitost:** O(1) - konstantní čas! ⚡

---

##### 2️⃣ Insert() - Přidání prvku

**Algoritmus:**
1. Přidej prvek na **konec pole** (poslední list)
2. **Bubble Up** (probublej nahoru):
   - Porovnávej s rodičem
   - Pokud je menší než rodič → prohoď
   - Opakuj, dokud není na správném místě

```csharp
// ✅ VERZE A - MATURITNÍ
// Přidání prvku do min-heap

public void Insert(int value)
{
    // 1. Přidej na konec
    heap.Add(value);
    
    // 2. Bubble Up
    int index = heap.Count - 1;
    
    while (index > 0)
    {
        int parentIndex = (index - 1) / 2;
        
        // Je na správném místě?
        if (heap[index] >= heap[parentIndex])
            break;
        
        // Prohoď s rodičem
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

**Příklad Insert(0) krok po kroku:**
```
Původní halda:
        [1]
       /   \
     [3]   [2]
     / \
   [7][5]

Pole: [1, 3, 2, 7, 5]

Krok 1: Přidej 0 na konec
Pole: [1, 3, 2, 7, 5, 0]
        [1]
       /   \
     [3]   [2]
     / \   /
   [7][5][0]

Krok 2: Bubble Up - 0 < 2 → prohoď
Pole: [1, 3, 0, 7, 5, 2]
        [1]
       /   \
     [3]   [0]  ← prohozeno
     / \   /
   [7][5][2]

Krok 3: Bubble Up - 0 < 1 → prohoď
Pole: [0, 3, 1, 7, 5, 2]
        [0]         ← nový kořen!
       /   \
     [3]   [1]
     / \   /
   [7][5][2]

Hotovo! 0 je nové minimum.
```

**Časová složitost:** O(log n) - max. výška stromu

---

##### 3️⃣ ExtractMin() - Odebrání minima

**Algoritmus:**
1. Ulož minimum (kořen) do proměnné
2. Přesuň **poslední prvek** na místo kořene
3. **Bubble Down** (probublej dolů):
   - Porovnej s menším z dětí
   - Pokud je větší → prohoď
   - Opakuj

```csharp
// ✅ VERZE A - MATURITNÍ
// Odebrání minima z min-heap

public int ExtractMin()
{
    if (heap.Count == 0)
        throw new Exception("Halda je prázdná");
    
    // 1. Ulož minimum
    int min = heap[0];
    
    // 2. Poslední prvek na místo kořene
    heap[0] = heap[heap.Count - 1];
    heap.RemoveAt(heap.Count - 1);
    
    // 3. Bubble Down
    int index = 0;
    
    while (true)
    {
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;
        int smallest = index;
        
        // Najdi nejmenší z: rodič, levý, pravý
        if (leftChild < heap.Count && 
            heap[leftChild] < heap[smallest])
            smallest = leftChild;
        
        if (rightChild < heap.Count && 
            heap[rightChild] < heap[smallest])
            smallest = rightChild;
        
        // Je na správném místě?
        if (smallest == index)
            break;
        
        // Prohoď s menším dítětem
        Swap(index, smallest);
        index = smallest;
    }
    
    return min;
}
```

**Příklad ExtractMin() krok po kroku:**
```
Původní halda:
        [1]
       /   \
     [3]   [2]
     / \   /
   [7][5][6]

Pole: [1, 3, 2, 7, 5, 6]

Krok 1: Ulož min=1, přesuň poslední (6) na kořen
Pole: [6, 3, 2, 7, 5]
        [6]
       /   \
     [3]   [2]
     / \
   [7][5]

Krok 2: Bubble Down - 6 > min(3,2) → prohoď s 2
Pole: [2, 3, 6, 7, 5]
        [2]
       /   \
     [3]   [6]
     / \
   [7][5]

Krok 3: Bubble Down - 6 na správném místě (děti 7,5 jsou větší)
Hotovo!

Vrátíme: min = 1
```

**Časová složitost:** O(log n)

---

#### 🎯 K čemu se halda používá

##### 1️⃣ Priority Queue (Fronta s prioritou)

```csharp
// Příklad: Nemocniční systém
MinHeap urgentQueue = new MinHeap();
urgentQueue.Insert(3); // Střední urgence
urgentQueue.Insert(1); // Kritický! ← Vždy jako první
urgentQueue.Insert(5); // Lehký případ

int next = urgentQueue.ExtractMin(); // Vrátí 1 (kritický)
```

**Reálné použití:**
- CPU scheduling (operační systémy)
- Síťové routery (prioritní pakety)
- Nemocnice (urgentní případy)
- Event handling (události podle času)

---

##### 2️⃣ Heap Sort (Třídění pomocí haldy)

**Algoritmus:**
1. Vytvoř max-heap z pole
2. Postupně odebírej maximum → seřazené pole

**Časová složitost:** O(n log n) ✅  
**Paměťová složitost:** O(1) - třídí in-place ✅

---

##### 3️⃣ K největších/nejmenších prvků

```csharp
// Najdi 3 nejmenší čísla z velkého datasetu
// Použij min-heap a třikrát zavolej ExtractMin()
```

---

##### 4️⃣ Dijkstrův algoritmus

Hledání nejkratší cesty v grafu používá min-heap pro výběr nejbližšího vrcholu.

---

#### ⏱️ Časová složitost operací haldy

| Operace | Složitost | Vysvětlení |
|---------|-----------|------------|
| **GetMin/Max** | O(1) | Minimum/maximum je v kořeni |
| **Insert** | O(log n) | Bubble Up - max výška stromu |
| **ExtractMin/Max** | O(log n) | Bubble Down - max výška |
| **Build Heap** | O(n) | Vytvoření haldy z pole |

---

#### 📋 Shrnutí haldy

| Aspekt | Hodnota |
|--------|---------|
| **Typ** | Speciální binární strom |
| **Struktura** | Úplný binární strom |
| **Pravidlo** | Rodič ≤/≥ všechny potomky |
| **Uložení** | **V poli!** (ne uzly s odkazy) |
| **Hlavní operace** | Insert O(log n), ExtractMin O(log n), GetMin O(1) |
| **Použití** | Priority queue, Heap sort, K-largest/smallest, Dijkstra |
| **Hlavní rozdíl od BVS** | Rychlý přístup k min/max, NE pro seřazený výpis |

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

### Při definicích:
- **Strom vs graf:** Strom NEOBSAHUJE cykly, graf může
- **Binární strom:** Max 2 potomci, pořadí záleží (levý ≠ pravý)
- **Obecný strom:** Libovolný počet potomků, libovolná hloubka!
- **BVS pravidlo:** VŠECHNY hodnoty v levém podstromu < uzel < VŠECHNY v pravém
- **Halda pravidlo:** Rodič ≤/≥ děti (JINÉ než BVS!)

### Při implementaci:
- **Null kontrola:** Vždy kontroluj, jestli uzel není null!
- **Rekurze:** Nezapomeň na základní případ (null → zastavení)
- **BVS Insert:** Použij rekurzi nebo while cyklus, ne ruční `root.Left = ...`
- **Duplicity:** Rozhodněte se, jestli je přidáváte (typicky NE)
- **Halda v poli:** Používá vzorce pro navigaci, NE odkazy!

### U časové složitosti:
- **Vyvážený vs nevyvážený:** O(log n) vs O(n)
- **Procházení:** Vždy O(n) - musíme navštívit všechny uzly
- **Hledání v BVS:** O(log n) POUZE pokud je strom vyvážený!
- **Halda operace:** GetMin O(1), Insert/Extract O(log n)

### Průchody stromem:
- **Pre-order (N-L-R):** Zpracuj uzel PŘED potomky
- **In-order (L-N-R):** Zpracuj uzel MEZI potomky → **BVS seřazené!**
- **Post-order (L-R-N):** Zpracuj uzel PO potomcích
- **BFS:** Používá **frontu**, DFS používá **zásobník** (nebo rekurzi)
- **In-order na haldě:** NESEŘAZENÉ! (to je častá chyba)

### Halda vs BVS:
- **Halda NENÍ BVS!** Jiné pravidlo, jiný účel
- **Halda:** Rychlý přístup k min/max (priority queue)
- **BVS:** Seřazený výpis, vyhledávání
- **Halda v poli:** Ne uzly s odkazy!
- **Bubble Up/Down:** Nezapomeň porovnávat s rodičem/dětmi správně

### U ústní zkoušky:
- Umět nakreslit příklad stromu na tabuli
- Vysvětlit průchod krok po kroku s ukazováním
- Ukázat, jak Insert() automaticky najde místo v BVS
- Porovnat BVS hledání s lineárním hledáním
- Umět vysvětlit rozdíl mezi DFS a BFS
- Nakreslit, jak funguje Bubble Up/Down v haldě
- Vysvětlit, proč In-order vypíše BVS seřazené
- Ukázat vzorce pro navigaci v haldě (2*i+1, 2*i+2, (i-1)/2)

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

---

### Kdy použít který průchod?

| Scénář | Průchod | Proč? |
|--------|---------|-------|
| **Seřazený výpis BVS** | In-order | Vypíše hodnoty vzestupně! |
| **Kopírování stromu** | Pre-order | Vytvoříme rodiče před dětmi |
| **Mazání stromu** | Post-order | Smažeme děti před rodičem |
| **Nejkratší cesta** | BFS | Prochází po úrovních |
| **Hledání cesty** | DFS | Jde hlouběji, méně paměti |
| **Vyhodnocení výrazu** | Post-order | Vypočítáme operandy před operátorem |

---

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

---

### Generické stromy v praxi

```csharp
// 💡 VERZE B - SENIOR
// Generický BVS pro jakýkoli typ

class BinarySearchTree<T> where T : IComparable<T>
{
    class Node
    {
        public T Data { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        
        public Node(T data) => Data = data;
    }
    
    private Node root;
    
    public void Insert(T value)
    {
        root = InsertRecursive(root, value);
    }
    
    private Node InsertRecursive(Node current, T value)
    {
        if (current == null)
            return new Node(value);
        
        int comparison = value.CompareTo(current.Data);
        
        if (comparison < 0)
            current.Left = InsertRecursive(current.Left, value);
        else if (comparison > 0)
            current.Right = InsertRecursive(current.Right, value);
        
        return current;
    }
}

// Použití:
var intTree = new BinarySearchTree<int>();
var stringTree = new BinarySearchTree<string>();
var studentTree = new BinarySearchTree<Student>();
```

**Proč je to lepší:**
- ✅ Jeden kód pro všechny typy
- ✅ Type-safe (kompilátor kontroluje typy)
- ✅ Znovupoužitelné

---

### V praxi:
- C# má `SortedSet<T>` (implementuje balancovaný BST)
- C# má `PriorityQueue<T>` (implementuje haldu) - od .NET 6
- Nepiš vlastní BVS/Halda do produkce, použij knihovní implementaci
- BVS je základ pro pokročilejší struktury (B-stromy v databázích)
- Halda je ideální pro Dijkstrův algoritmus, A* pathfinding

---

### Halda - praktické tipy
- **Proč pole místo uzlů?** Cache-friendly, úspora paměti, rychlejší
- **Build Heap:** Rychlejší vytvořit haldu najednou (O(n)) než postupně vkládat (O(n log n))
- **Max-heap vs Min-heap:** Stačí změnit porovnání (< na >)
- **K největších prvků:** Použij min-heap velikosti K (ne max-heap!)

```csharp
// Najdi 3 největší čísla z velkého datasetu
MinHeap topK = new MinHeap(maxSize: 3);
foreach (int num in dataset)
{
    if (topK.Count < 3)
        topK.Insert(num);
    else if (num > topK.GetMin())
    {
        topK.ExtractMin();
        topK.Insert(num);
    }
}
// topK obsahuje 3 největší čísla
```

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 2 (Spojové struktury):** Strom je také spojová struktura (uzly propojené odkazy), halda ale používá pole
- **Otázka 3 (Fronta a zásobník):** BFS používá frontu, DFS používá zásobník (nebo rekurzi = automatický zásobník)
- **Otázka 5 (Rekurze):** Procházení stromu je klasický příklad rekurze, Post-order průchod je rekurzivní
- **Otázka 6 (Práce se soubory):** Souborový systém je stromová struktura
- **Otázka 7 (Časová složitost):** O(log n) vs O(n) v závislosti na vyvážení, halda má O(log n) pro Insert/Extract
- **Otázka 10-13 (Třídění):** Heap Sort používá haldu pro O(n log n) třídění
- **Otázka 13 (Heap Sort):** Halda je přímo použita pro třídění! Důležitá souvislost
- **Otázka 14 (Vyhledávání):** BVS kombinuje rychlost bin. vyhledávání s flexibilitou, O(log n) jako binární vyhledávání
- **Otázka 15 (Rozděl a panuj):** Pre-order průchod odpovídá rozdělení problému na podproblémy
- **Otázka 16 (Aritmetické výrazy):** Expression tree - strom pro výrazy, Post-order pro vyhodnocení
- **Otázka 17-18 (OOP):** Implementace stromu pomocí tříd (Node, Tree), dědičnost pro různé typy stromů
- **Otázka 20 (Událostmi řízené):** Event handling používá priority queue (haldu)
- **Otázka 21 (Teorie grafů):** Strom je speciální typ grafu (souvislý acyklický graf)
- **Otázka 22 (DFS/BFS):** Průchody stromem jsou základ pro grafové algoritmy! DFS a BFS na stromech je jednodušší než na grafech
- **Otázka 25 (Dijkstra):** Dijkstrův algoritmus používá min-heap pro výběr nejbližšího vrcholu

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

**Hotové body (1-8):**
- ✅ Bod 1: Definice stromu
- ✅ Bod 2: Definice binárního stromu
- ✅ Bod 3: Definice BVS (s automatickým Insert!)
- ✅ Bod 4: Algoritmus procházení obecného stromu
- ✅ Bod 5: Algoritmus hledání prvku v BVS
- ✅ Bod 6: Průchod stromem DFS (Pre/In/Post-order) a BFS
- ✅ Bod 7: Co může být ve stromu uloženo (int, string, třídy, výrazy, soubory)
- ✅ Bod 8: Halda (heap) - struktura, operace, použití

**Zbývající body k procvičení:**
- [ ] Bod 9: Praktické příklady využití stromů v reálném světě
- [ ] Bod 10: Možný způsob implementace (OOP přístup, kompletní třída)

**Až dokončíme všechny body, přejdeme na fázi praktického procvičení na maturitních úlohách!**

---

## 🎓 Mini-Index relevantních úloh (pro budoucí procvičení)

**Plánované úlohy k procvičení (po dokončení všech bodů):**
1. **BST Implementation** - Kompletní implementace BVS (insert, find, delete, průchody)
2. **Tree Traversal** - Procvičení všech průchodů (Pre/In/Post-order, BFS)
3. **BFS vs DFS** - Porovnání obou přístupů na konkrétních úlohách
4. **Min/Max Heap** - Implementace haldové struktury, Insert, ExtractMin
5. **Heap Sort** - Třídění pomocí haldy
6. **Priority Queue** - Použití haldy pro frontu s prioritou
7. **Expression Tree** - Vyhodnocení aritmetického výrazu pomocí stromu
8. **File System Tree** - Modelování souborového systému

---

**Konec zápisu - Aktualizováno: 2025-02-16**  
**Status:** Body 1-8 kompletní s detailními vysvětleními, příklady a kódem
