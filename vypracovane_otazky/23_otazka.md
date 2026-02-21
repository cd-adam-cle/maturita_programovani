# 📚 Zápisky: Otázka č. 23 – Hledání minimální kostry grafu
**Datum:** 2025-02-20
**Status:** ✅ Hotovo

---

## ✅ Checklist bodů otázky
- [x] Bod 1: Definice kostry grafu
- [x] Bod 2: Definice minimální kostry grafu
- [x] Bod 3: Motivační příklad
- [x] Bod 4a: Kruskalův algoritmus (popis + kód + obrázek)
- [x] Bod 4b: Jarníkův (Primův) algoritmus (popis + kód + obrázek)
- [x] Porovnání Kruskal vs Jarník

---

## 🧠 Klíčové koncepty & Snippety

### Bod 1: Kostra grafu

**Teorie:**
Kostra grafu je podgraf, který:
- Obsahuje **všechny vrcholy** původního grafu
- Obsahuje **podmnožinu hran** → je **souvislý** a **neobsahuje cyklus** (= je to strom)
- Má vždy přesně **V - 1** hran (V = počet vrcholů)

Graf může mít **více různých koster**. Nesouvislý graf kostru nemá.

```
PŮVODNÍ GRAF:                  KOSTRA:
    A ---3--- B                    A ---3--- B
    |  \      |                      \
    4   2     5                       2
    |    \    |                        \
    C ---1--- D                    C ---1--- D
5 hran                          3 hrany (V-1 = 4-1)
```

---

### Bod 2: Minimální kostra grafu (MST)

**Teorie:**
Minimální kostra = kostra s **nejmenším součtem vah hran**. Existuje jen pro **souvislý ohodnocený graf**.

Pokud jsou všechny váhy unikátní → MST je **jednoznačná**.

```
Kostra 1: AD(2), CD(1), AB(3) → součet = 6  ← MST ✅
Kostra 2: AD(2), CD(1), BD(5) → součet = 8
Kostra 3: AB(3), AC(4), CD(1) → součet = 8
```

---

### Bod 3: Motivační příklad

**Propojení vesnic optickým kabelem** za minimální cenu:
- Máš N vesnic (vrcholy) a možná propojení s cenami (ohodnocené hrany)
- Chceš propojit VŠECHNY vesnice za nejmenší cenu
- Nepotřebuješ redundantní spoje (žádné cykly)

Další příklady: rozvodná síť elektřiny, počítačová síť, stavba silnic.

---

### Bod 4a: Kruskalův algoritmus

**Teorie:**
Myšlenka: Seber všechny hrany, **seřaď od nejlevnější**, přidávej jednu po druhé – ale jen pokud **nevytvoří cyklus**.

**Algoritmus:**
1. Seřaď všechny hrany podle váhy (od nejmenší)
2. Procházej hrany:
   - Pokud hrana nespojuje dva už propojené vrcholy → přidej do kostry
   - Pokud by vznikl cyklus → přeskoč
3. Konec, když máš V - 1 hran

**Detekce cyklu:** Struktura **Union-Find**
- `Find(x)` – do které skupiny patří vrchol x
- `Union(x, y)` – spoj skupiny
- Cyklus nastane, když `Find(u) == Find(v)` (oba ve stejné skupině)

```
KROK 0: Seřazené hrany: CD(1), AD(2), AB(3), AC(4), BD(5)
         Komponenty: {A} {B} {C} {D}

KROK 1: CD(1) → C,D nejsou propojené → PŘIDEJ ✅
         Komponenty: {A} {B} {C,D}

KROK 2: AD(2) → A,D nejsou propojené → PŘIDEJ ✅
         Komponenty: {B} {A,C,D}

KROK 3: AB(3) → A,B nejsou propojené → PŘIDEJ ✅
         Komponenty: {A,B,C,D}  ← vše propojeno, HOTOVO!

MST: CD(1) + AD(2) + AB(3) = 6
```

**Kód (Maturitní verze):**

```csharp
class Hrana : IComparable<Hrana>
{
    public int Odkud, Kam, Vaha;

    public Hrana(int odkud, int kam, int vaha)
    {
        Odkud = odkud;
        Kam = kam;
        Vaha = vaha;
    }

    public int CompareTo(Hrana other)
    {
        return Vaha.CompareTo(other.Vaha);
    }
}

class UnionFind
{
    int[] rodic;

    public UnionFind(int n)
    {
        rodic = new int[n];
        for (int i = 0; i < n; i++)
            rodic[i] = i;  // každý je sám sobě rodičem
    }

    public int Find(int x)
    {
        while (rodic[x] != x)
            x = rodic[x];
        return x;
    }

    public bool Union(int x, int y)
    {
        int rx = Find(x);
        int ry = Find(y);
        if (rx == ry) return false; // stejná skupina = cyklus!
        rodic[rx] = ry;
        return true;
    }
}

static List<Hrana> Kruskal(int pocetVrcholu, List<Hrana> hrany)
{
    hrany.Sort();
    UnionFind uf = new UnionFind(pocetVrcholu);
    List<Hrana> kostra = new List<Hrana>();

    foreach (Hrana h in hrany)
    {
        if (uf.Union(h.Odkud, h.Kam))
        {
            kostra.Add(h);
            if (kostra.Count == pocetVrcholu - 1)
                break;
        }
    }

    return kostra;
}
```

**Časová složitost:** O(E log E) – dominuje řazení hran
**Paměťová složitost:** O(V + E)

---

### Bod 4b: Jarníkův (Primův) algoritmus

**Teorie:**
Myšlenka: Začni z libovolného vrcholu a **rozrůstej strom** – vždy přidej nejlevnější hranu vedoucí z stromu do nového vrcholu.

**Algoritmus:**
1. Vyber startovní vrchol, přidej do stromu
2. Ze všech hran vedoucích z stromu ven vyber **nejlevnější**
3. Přidej hranu a nový vrchol do stromu
4. Opakuj, dokud nejsou všechny vrcholy ve stromu

```
KROK 0: Start z A
         Ve stromu: {A}
         Dostupné: AB(3), AC(4), AD(2)

KROK 1: Nejlevnější = AD(2) → přidej D
         Ve stromu: {A, D}
         Dostupné: AB(3), AC(4), DB(5), DC(1)

KROK 2: Nejlevnější = DC(1) → přidej C
         Ve stromu: {A, D, C}
         Dostupné: AB(3), DB(5)

KROK 3: Nejlevnější = AB(3) → přidej B
         Ve stromu: {A, D, C, B} ← HOTOVO!

MST: AD(2) + DC(1) + AB(3) = 6
```

**Kód (Maturitní verze):**

```csharp
static List<(int Odkud, int Kam, int Vaha)> Jarnik(
    List<(int soused, int vaha)>[] sousede, int pocetVrcholu)
{
    bool[] veStrome = new bool[pocetVrcholu];
    var kostra = new List<(int Odkud, int Kam, int Vaha)>();
    var kandidati = new List<(int odkud, int kam, int vaha)>();

    // Start z vrcholu 0
    veStrome[0] = true;
    foreach (var (soused, vaha) in sousede[0])
        kandidati.Add((0, soused, vaha));

    while (kostra.Count < pocetVrcholu - 1 && kandidati.Count > 0)
    {
        // Najdi nejlevnější hranu
        int minIndex = 0;
        for (int i = 1; i < kandidati.Count; i++)
        {
            if (kandidati[i].vaha < kandidati[minIndex].vaha)
                minIndex = i;
        }

        var nejlevnejsi = kandidati[minIndex];
        kandidati.RemoveAt(minIndex);

        if (veStrome[nejlevnejsi.kam])
            continue; // už je ve stromu

        veStrome[nejlevnejsi.kam] = true;
        kostra.Add((nejlevnejsi.odkud, nejlevnejsi.kam, nejlevnejsi.vaha));

        foreach (var (soused, vaha) in sousede[nejlevnejsi.kam])
        {
            if (!veStrome[soused])
                kandidati.Add((nejlevnejsi.kam, soused, vaha));
        }
    }

    return kostra;
}
```

**Časová složitost:**
- Maturitní verze (seznam): O(V × E)
- Senior verze (PriorityQueue): O(E log V)

**Paměťová složitost:** O(V + E)

---

### Porovnání Kruskal vs Jarník

```
╔══════════════════╦════════════════════╦════════════════════╗
║                  ║    KRUSKAL         ║   JARNÍK (PRIM)    ║
╠══════════════════╬════════════════════╬════════════════════╣
║ Přístup          ║ Globální (hrany)   ║ Lokální (rosteme)  ║
║ Začíná           ║ Řazením VŠECH hran ║ Z jednoho vrcholu  ║
║ Datová struktura ║ Union-Find         ║ Prioritní fronta   ║
║ Složitost        ║ O(E log E)         ║ O(E log V)         ║
║ Lepší pro        ║ Řídké grafy        ║ Husté grafy        ║
╚══════════════════╩════════════════════╩════════════════════╝
```

---

### Senior verze (Nice to Have)

**Union-Find s optimalizací (komprese cesty + union by rank):**

```csharp
class UnionFindOptimal
{
    int[] rodic, rank;

    public UnionFindOptimal(int n)
    {
        rodic = new int[n];
        rank = new int[n];
        for (int i = 0; i < n; i++)
            rodic[i] = i;
    }

    public int Find(int x)
    {
        if (rodic[x] != x)
            rodic[x] = Find(rodic[x]);  // komprese cesty
        return rodic[x];
    }

    public bool Union(int x, int y)
    {
        int rx = Find(x), ry = Find(y);
        if (rx == ry) return false;
        if (rank[rx] < rank[ry]) rodic[rx] = ry;
        else if (rank[rx] > rank[ry]) rodic[ry] = rx;
        else { rodic[ry] = rx; rank[rx]++; }
        return true;
    }
}
```

**Jarník s PriorityQueue (.NET 6+):**

```csharp
static List<(int, int, int)> JarnikSenior(
    List<(int soused, int vaha)>[] sousede, int V)
{
    bool[] veStrome = new bool[V];
    var kostra = new List<(int, int, int)>();
    var pq = new PriorityQueue<(int odkud, int kam), int>();

    veStrome[0] = true;
    foreach (var (s, v) in sousede[0])
        pq.Enqueue((0, s), v);

    while (kostra.Count < V - 1 && pq.Count > 0)
    {
        pq.TryDequeue(out var hrana, out int vaha);
        if (veStrome[hrana.kam]) continue;

        veStrome[hrana.kam] = true;
        kostra.Add((hrana.odkud, hrana.kam, vaha));

        foreach (var (s, v) in sousede[hrana.kam])
            if (!veStrome[s])
                pq.Enqueue((hrana.kam, s), v);
    }

    return kostra;
}
```

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

- **Kostra existuje jen pro souvislý graf** – nesouvislý graf nemá kostru
- **V-1 hran** – kostra s V vrcholy má VŽDY přesně V-1 hran
- **Kruskal: nezapomeň na Union-Find** – bez něj neumíš detekovat cykly efektivně
- **Jarník: kontroluj `veStrome`** – přeskakuj hrany vedoucí do už přidaných vrcholů
- **Unikátní váhy → jednoznačná MST** – opakující se váhy mohou dát více MST se stejným součtem
- **Na tabuli:** Kresli graf a postupně zvýrazňuj přidávané hrany, piš komponenty

---

## 🚀 Senior Tip

V praxi se Kruskal hodí, pokud máš hrany už načtené v seznamu (edge list). Jarník je lepší, pokud máš graf jako seznamy sousedů (adjacency list) a graf je hustý. Obě varianty se dají zrychlit na téměř lineární čas s pokročilými datovými strukturami (Fibonacci heap pro Jarníka → O(E + V log V)).

Zmínka o **Borůvkově algoritmu**: Pracuje paralelně – v každém kroku každá komponenta přidá svou nejlevnější hranu. Historicky zajímavý (český matematik, 1926!), ale na maturitě se neptá.

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 8** (Reprezentace grafu) – graf potřebuješ reprezentovat, aby algoritmy fungovaly
- **Otázka 9** (Stromy) – kostra JE strom, halda se používá v optimalizovaném Jarníkovi
- **Otázka 13** (Heap sort) – prioritní fronta / halda v Jarníkově algoritmu
- **Otázka 21** (Teorie grafů) – pojmy: souvislý graf, strom, cyklus, ohodnocený graf
- **Otázka 22** (DFS/BFS) – DFS se dá použít k ověření souvislosti grafu
- **Otázka 25** (Dijkstra) – velmi podobný Jarníkovi! Oba rostou z jednoho bodu s prioritní frontou
