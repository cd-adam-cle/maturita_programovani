# 📚 Zápisky: Otázka č. 22 - Prohledávání do hloubky a do šířky
**Datum:** 2026-02-17
**Status:** ✅ Hotovo (teorie) | ⬜ Procvičení úloh

---

## ✅ Checklist bodů otázky
- [x] Bod 1: DFS – jednotlivé kroky, zásobník, rekurzivní i iterativní verze
- [x] Bod 2: BFS – jednotlivé kroky, fronta (Queue)
- [x] Bod 3: Časová a prostorová složitost
- [x] Bod 4: Příklady úloh vedoucí na použití DFS/BFS
- [x] Bod 5: Souvislost s hledáním nejkratší cesty

---

## 🧠 Klíčové koncepty & Snippety

---

### Bod 1: DFS (Depth-First Search) – Prohledávání do hloubky

**Teorie:**
- Algoritmus prochází graf **co nejhlouběji** – jde rovně dokud může, pak se vrátí na poslední křižovatku
- Používá **zásobník (Stack)** – LIFO (Last In, First Out)
- Dvě varianty: **iterativní** (explicitní Stack) a **rekurzivní** (využívá call stack)
- Rekurzivní verze hrozí **StackOverflowException** u velkých grafů

**Vizualizace:**
```
        A
       / \
      B   C
     / \   \
    D   E   F

DFS pořadí: A → C → F → B → E → D
(Jde do hloubky: A→C→F, pak zpět A→B→E→D)

Krok 1: Stack=[A]       → Pop A, navštív A, Push(B,C)
Krok 2: Stack=[B,C]     → Pop C (LIFO!), navštív C, Push(F)
Krok 3: Stack=[B,F]     → Pop F, navštív F
Krok 4: Stack=[B]       → Pop B, navštív B, Push(D,E)
Krok 5: Stack=[D,E]     → Pop E, navštív E
Krok 6: Stack=[D]       → Pop D, navštív D
Krok 7: Stack=[]        → KONEC
```

**Kód – Iterativní (Maturitní verze):**
```csharp
class Node
{
    public string Jmeno;
    public List<Node> Sousede = new List<Node>();
    public Node(string jmeno) { Jmeno = jmeno; }
}

static void DFS(Node start)
{
    Stack<Node> zasobnik = new Stack<Node>();
    HashSet<Node> navstivene = new HashSet<Node>();

    zasobnik.Push(start);

    while (zasobnik.Count > 0)
    {
        Node aktualni = zasobnik.Pop();

        if (navstivene.Contains(aktualni))
            continue;

        navstivene.Add(aktualni);
        Console.WriteLine(aktualni.Jmeno);

        foreach (Node soused in aktualni.Sousede)
        {
            if (!navstivene.Contains(soused))
                zasobnik.Push(soused);
        }
    }
}
```

**Kód – Rekurzivní (Maturitní verze):**
```csharp
static void DFSRekurze(Node aktualni, HashSet<Node> navstivene)
{
    navstivene.Add(aktualni);
    Console.WriteLine(aktualni.Jmeno);

    foreach (Node soused in aktualni.Sousede)
    {
        if (!navstivene.Contains(soused))
            DFSRekurze(soused, navstivene);
    }
}

// Volání:
// HashSet<Node> navstivene = new HashSet<Node>();
// DFSRekurze(startNode, navstivene);
```

**Senior verze (Dictionary přístup):**
```csharp
static void DFS(Dictionary<string, List<string>> graf, string start)
{
    var zasobnik = new Stack<string>();
    var navstivene = new HashSet<string>();

    zasobnik.Push(start);

    while (zasobnik.Count > 0)
    {
        var aktualni = zasobnik.Pop();
        if (!navstivene.Add(aktualni)) continue;

        Console.WriteLine(aktualni);

        foreach (var soused in graf[aktualni])
            if (!navstivene.Contains(soused))
                zasobnik.Push(soused);
    }
}
```

---

### Bod 2: BFS (Breadth-First Search) – Prohledávání do šířky

**Teorie:**
- Algoritmus prochází graf **po vrstvách** – nejdřív všichni ve vzdálenosti 1, pak 2, pak 3...
- Používá **frontu (Queue)** – FIFO (First In, First Out)
- FIFO zajistí zpracování po vrstvách → proto BFS najde nejkratší cestu v neohodnoceném grafu
- BFS existuje POUZE v iterativní verzi

**⚠️ Klíčový rozdíl oproti DFS:**
- DFS: Označíme jako navštívené AŽ PŘI ZPRACOVÁNÍ (Pop)
- BFS: Označíme jako navštívené UŽ PŘI PŘIDÁNÍ DO FRONTY (Enqueue)

**Vizualizace:**
```
        A            ← vrstva 0 (vzdálenost 0)
       / \
      B   C          ← vrstva 1 (vzdálenost 1)
     / \   \
    D   E   F        ← vrstva 2 (vzdálenost 2)

BFS pořadí: A → B → C → D → E → F
(Po vrstvách: vrstva 0, pak 1, pak 2)

Krok 1: Fronta=[A]       → Dequeue A, navštív A, Enqueue(B,C)
Krok 2: Fronta=[B,C]     → Dequeue B (FIFO!), navštív B, Enqueue(D,E)
Krok 3: Fronta=[C,D,E]   → Dequeue C, navštív C, Enqueue(F)
Krok 4: Fronta=[D,E,F]   → Dequeue D, navštív D
Krok 5: Fronta=[E,F]     → Dequeue E, navštív E
Krok 6: Fronta=[F]       → Dequeue F, navštív F
Krok 7: Fronta=[]        → KONEC
```

**Kód (Maturitní verze):**
```csharp
static void BFS(Node start)
{
    Queue<Node> fronta = new Queue<Node>();
    HashSet<Node> navstivene = new HashSet<Node>();

    fronta.Enqueue(start);
    navstivene.Add(start);  // ⚠️ Označíme HNED při přidání!

    while (fronta.Count > 0)
    {
        Node aktualni = fronta.Dequeue();
        Console.WriteLine(aktualni.Jmeno);

        foreach (Node soused in aktualni.Sousede)
        {
            if (!navstivene.Contains(soused))
            {
                navstivene.Add(soused);
                fronta.Enqueue(soused);
            }
        }
    }
}
```

**Porovnání kódu DFS vs BFS:**
```
╔══════════════════════════════════════════════════════════╗
║  DFS                          │  BFS                    ║
╠══════════════════════════════════════════════════════════╣
║  Stack<Node> zasobnik         │  Queue<Node> fronta     ║
║  zasobnik.Push(x)             │  fronta.Enqueue(x)      ║
║  zasobnik.Pop()               │  fronta.Dequeue()       ║
║  zasobnik.Count > 0           │  fronta.Count > 0       ║
║  Označit při Pop              │  Označit při Enqueue    ║
║  LIFO → do hloubky            │  FIFO → po vrstvách     ║
╚══════════════════════════════════════════════════════════╝
→ Celá logika je stejná, jen vyměníš Stack za Queue!
```

**Senior verze (s trackováním vzdáleností):**
```csharp
static Dictionary<string, int> BFSsVzdalenosti(
    Dictionary<string, List<string>> graf, string start)
{
    var fronta = new Queue<string>();
    var vzdalenost = new Dictionary<string, int>();

    fronta.Enqueue(start);
    vzdalenost[start] = 0;

    while (fronta.Count > 0)
    {
        var aktualni = fronta.Dequeue();

        foreach (var soused in graf[aktualni])
        {
            if (!vzdalenost.ContainsKey(soused))
            {
                vzdalenost[soused] = vzdalenost[aktualni] + 1;
                fronta.Enqueue(soused);
            }
        }
    }

    return vzdalenost;
}
```

---

### Bod 3: Časová a prostorová složitost

**Časová složitost: O(V + E)** (se seznamem sousedů)
- V = počet vrcholů (Vertices) – každý navštívíme max 1×
- E = počet hran (Edges) – každou hranu "projdeme" 2× (z obou stran)
- Platí pro DFS i BFS

**⚠️ Záleží na reprezentaci grafu:**
```
╔═══════════════════════════════════════════════════════════╗
║  Reprezentace          │  Časová složitost DFS/BFS       ║
╠═══════════════════════════════════════════════════════════╣
║  Seznam sousedů        │  O(V + E)  ← optimální         ║
║  Matice sousednosti    │  O(V²)    ← musíš projet celý  ║
║                        │            řádek pro každý V    ║
╚═══════════════════════════════════════════════════════════╝
```

**Prostorová složitost: O(V)**
- HashSet navstivene → max V prvků
- Stack / Queue → max V prvků
- Rekurzivní DFS: O(V) kvůli call stacku

**Souhrn:**
```
╔════════════════════════════════════════════════════╗
║              │  DFS          │  BFS               ║
╠════════════════════════════════════════════════════╣
║  Čas (seznam)│  O(V + E)     │  O(V + E)          ║
║  Čas (matice)│  O(V²)        │  O(V²)             ║
║  Paměť       │  O(V)         │  O(V)              ║
║  Struktura   │  Stack/rekurze│  Queue              ║
╚════════════════════════════════════════════════════╝
```

---

### Bod 4: Příklady úloh pro DFS a BFS

**DFS se hodí na:**
- Detekce cyklu v grafu
- Topologické třídění (Otázka 24)
- Hledání všech cest mezi dvěma vrcholy
- Backtracking (Sudoku, N dam, labyrint)
- Komponenty souvislosti (skupiny propojených vrcholů)

**BFS se hodí na:**
- Nejkratší cesta v neohodnoceném grafu (NEJDŮLEŽITĚJŠÍ!)
- Bludiště – nejkratší cesta v mřížce
- Šíření – virus, požár, vlna (po vrstvách = časové kroky)
- Level-order průchod stromem (po patrech)
- Min. počet tahů (šachový kůň, věž)

**Rozhodovací tabulka:**
```
╔═══════════════════════════════════════════════════════════════╗
║  Problém                        │  Algoritmus  │  Proč       ║
╠═══════════════════════════════════════════════════════════════╣
║  Nejkratší cesta (neohod.)      │  BFS         │  Vrstvy =   ║
║                                 │              │  vzdálenosti ║
║  Existuje cesta A→B?            │  DFS i BFS   │  Oba OK     ║
║  Detekce cyklu                  │  DFS         │  Backtrack   ║
║  Topologické třídění            │  DFS         │  Post-order  ║
║  Komponenty souvislosti         │  DFS i BFS   │  Oba OK     ║
║  Backtracking (Sudoku, N dam)   │  DFS         │  Zkoušej +  ║
║                                 │              │  vracej se   ║
║  Šíření (požár, virus)          │  BFS         │  Po vrstvách ║
║  Min. počet tahů (šachy)        │  BFS         │  Nejkratší   ║
╚═══════════════════════════════════════════════════════════════╝
```

**Klíčové pravidlo:**
- Potřebuješ nejkratší cestu? → **BFS**
- Potřebuješ prozkoumat všechny možnosti / backtracking? → **DFS**
- Je ti to jedno (jen existuje cesta)? → **Oba fungují**

**Komponenta souvislosti** = skupina vrcholů, které jsou navzájem dosažitelné po hranách. Mezi komponentami žádná cesta neexistuje.
```
Komponenta 1:  A---B    Komponenta 2:  E---F    Komponenta 3:  H
               |                       |
               C---D                   G
```
Najdeš je opakovaným spouštěním DFS/BFS z nenavštívených vrcholů.

---

### Bod 5: Souvislost s hledáním nejkratší cesty

**BFS = nejkratší cesta v neohodnoceném grafu**
- BFS prochází po vrstvách = po vzdálenostech
- Když poprvé dorazíš do cíle → zaručeně nejkratší cesta (v počtu hran)
- DFS NEGARANTUJE nejkratší cestu

**Implementace BFS s rekonstrukcí cesty:**
```csharp
static List<string> NejkratsiCesta(
    Dictionary<string, List<string>> graf,
    string start,
    string cil)
{
    Queue<string> fronta = new Queue<string>();
    HashSet<string> navstivene = new HashSet<string>();
    Dictionary<string, string> predchudce = new Dictionary<string, string>();

    fronta.Enqueue(start);
    navstivene.Add(start);
    predchudce[start] = null;

    while (fronta.Count > 0)
    {
        string aktualni = fronta.Dequeue();

        if (aktualni == cil)
        {
            // Rekonstrukce cesty pozpátku
            List<string> cesta = new List<string>();
            string vrchol = cil;
            while (vrchol != null)
            {
                cesta.Add(vrchol);
                vrchol = predchudce[vrchol];
            }
            cesta.Reverse();
            return cesta;
        }

        foreach (string soused in graf[aktualni])
        {
            if (!navstivene.Contains(soused))
            {
                navstivene.Add(soused);
                predchudce[soused] = aktualni;
                fronta.Enqueue(soused);
            }
        }
    }

    return null;  // Cesta neexistuje
}
```

**Kde BFS nestačí:**
```
╔══════════════════════════════════════════════════════════╗
║  Typ grafu              │  Algoritmus  │  Najde nejkr.? ║
╠══════════════════════════════════════════════════════════╣
║  Neohodnocený           │  BFS         │  ✅ ANO        ║
║  Ohodnocený (kladné)    │  Dijkstra    │  ✅ ANO        ║
║  Ohodnocený (záporné)   │  Bellman-Ford│  ✅ ANO        ║
║  DFS (jakýkoliv graf)   │  DFS         │  ❌ NE         ║
╚══════════════════════════════════════════════════════════╝
```

Pro ohodnocené grafy → Dijkstrův algoritmus (Otázka 25).

---

## 📋 Relevantní procvičovací úlohy
- ⬜ **Úloha 352** (⭐) – Existuje autobusové spojení? – DFS/BFS dosažitelnost
- ⬜ **Úloha 353** (⭐) – Dostupná města – všechny dosažitelné vrcholy
- ⬜ **Úloha 354** (⭐⭐) – Skupinky lidí – komponenty souvislosti
- ⬜ **Úloha 355** (⭐⭐) – Cesta bludištěm – rekonstrukce cesty
- ⬜ **Úloha 356** (⭐⭐) – Letiště s nejméně přestupy – BFS nejkratší cesta
- ⬜ **Úloha 383** (⭐⭐) – Věž na šachovnici – implicitní graf
- ⬜ **Úloha 385** (⭐⭐) – Bludiště ve čtverečkové síti – mřížkový BFS
- ⬜ **Úloha 384** (⭐⭐⭐) – Šachový kůň – BFS min. počet tahů

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **DFS vs BFS označování navštívených:**
   - DFS iterativní: označit při Pop (nebo check po Pop)
   - BFS: označit při Enqueue (ne při Dequeue!)
   - Pokud v BFS označíš až při Dequeue, vrchol se může dostat do fronty vícekrát

2. **DFS rekurzivní vs iterativní:**
   - Mohou dát jiné pořadí návštěv (záleží na pořadí sousedů)
   - Obě jsou korektní DFS
   - Rekurzivní hrozí StackOverflowException

3. **Složitost závisí na reprezentaci:**
   - Seznam sousedů → O(V + E)
   - Matice sousednosti → O(V²)
   - Na maturitě vždy řekni, jakou reprezentaci používáš!

4. **BFS najde nejkratší cestu JEN v neohodnoceném grafu**
   - Pro ohodnocený graf potřebuješ Dijkstru
   - DFS NIKDY negarantuje nejkratší cestu

5. **HashSet vs List pro navštívené:**
   - HashSet.Contains() = O(1) ✅
   - List.Contains() = O(n) ❌

---

## 🚀 Senior Tip
- V praxi se graf nejčastěji reprezentuje jako `Dictionary<string, List<string>>`
- `navstivene.Add()` v HashSet vrací `bool` → ušetříš volání Contains()
- Pro BFS s vzdálenostmi stačí `Dictionary<string, int>` – slouží jako navštíveno I vzdálenost

---

## 🔗 Souvislosti s jinými otázkami
- **Otázka 3** (Fronta a zásobník) – BFS používá Queue, DFS používá Stack
- **Otázka 5** (Rekurze) – DFS rekurzivní = přirozené využití call stacku
- **Otázka 8** (Reprezentace grafu) – Složitost závisí na reprezentaci
- **Otázka 9** (Stromy) – DFS/BFS průchod stromem = speciální případ grafu
- **Otázka 21** (Teorie grafů) – Pojmy: souvislost, komponenty, cesty
- **Otázka 24** (Topologické třídění) – Využívá DFS
- **Otázka 25** (Nejkratší cesta) – BFS pro neohodnocený, Dijkstra pro ohodnocený
