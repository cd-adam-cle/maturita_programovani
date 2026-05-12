# Zápisky: Otázka č. 8 - Reprezentace grafu v počítači

## Checklist bodů otázky

- [x] Bod 1: Definice grafu a základní pojmy
- [x] Bod 2: Matice sousednosti
- [x] Bod 3: Matice incidence
- [x] Bod 4: Seznamy sousedů
- [x] Bod 5: Seznam hran (edge list)
- [x] Bod 6: Časová náročnost základních metod
- [x] Bod 7: Která reprezentace pro jaký typ grafu
- [x] Bod 8: Reprezentace grafu pomocí OOP
- [x] Speciální typy grafů (úplný, bipartitní, řídký, hustý)
- [x] Ohodnocené a orientované grafy

---

## Klíčové koncepty & Snippety

### 1. Definice grafu a základní pojmy

**Graf** je matematická struktura `G = (V, E)`, kde:
- **V** = množina **vrcholů (vertices)**, někdy nazývaných uzly.
- **E** = množina **hran (edges)**, kde každá hrana spojuje dva vrcholy.

Graf je natolik obecná abstrakce, že popisuje neuvěřitelnou škálu reálných systémů: sociální sítě, silnice, internet, závislosti modulů, šachové tahy, chemické vzorce, stavový prostor hry, neuronové sítě.

**Typy grafů podle směru hran:**
- **Neorientovaný graf** – hrana `{u, v}` je neuspořádaná dvojice; lze přejít oběma směry. (Obousměrná ulice.)
- **Orientovaný graf (digraph)** – hrana `(u, v)` je uspořádaná dvojice; přechod jen z `u` do `v`. (Jednosměrka, link na webu.)

**Typy grafů podle vah:**
- **Neohodnocený graf** – hrana má jen identitu, žádnou číselnou hodnotu.
- **Ohodnocený (vážený) graf** – každé hraně přiřazena váha (cena, vzdálenost, propustnost). Pro nejkratší cestu (Dijkstra), minimální kostru (Kruskal/Jarník), tok v síti.

**Speciální vlastnosti:**
- **Prostý (jednoduchý) graf** – mezi dvojicí vrcholů max. 1 hrana, žádné smyčky.
- **Multigraf** – povoleny **paralelní hrany** (více hran mezi stejnou dvojicí).
- **Smyčka (loop)** – hrana `(v, v)` z vrcholu do sebe.
- **Úplný graf `Kₙ`** – každý vrchol je spojený s každým ostatním; má `n(n-1)/2` hran.
- **Bipartitní graf** – vrcholy lze rozdělit do dvou disjunktních množin tak, že každá hrana spojuje vrchol z jedné množiny s vrcholem z druhé. (Studenti vs. předměty.)
- **Řídký (sparse) graf** – `E = O(V)` nebo `O(V log V)`. (Silnice, sociální síť.)
- **Hustý (dense) graf** – `E = Θ(V²)`. (Turnaj, kompletní síť.)
- **Souvislý graf** – z každého vrcholu se dostanete do každého jiného.
- **Komponenta souvislosti** – maximální souvislý podgraf.
- **Strom** – souvislý graf bez cyklů; má přesně `V − 1` hran.
- **Les** – disjunktní sjednocení stromů (nesouvislý acyklický graf).
- **DAG (Directed Acyclic Graph)** – orientovaný graf bez cyklů; používá se pro závislosti, build systémy, topologické řazení.

**Pojmy o vrcholech a hranách:**
| Pojem | Význam |
|-------|--------|
| **Stupeň vrcholu `deg(v)`** | Počet hran incidentních s `v` (smyčka se počítá 2×). |
| **Vstupní stupeň `in-deg(v)`** | (Orientovaný graf) Počet hran vstupujících do `v`. |
| **Výstupní stupeň `out-deg(v)`** | (Orientovaný graf) Počet hran vystupujících z `v`. |
| **Sousedi (adjacency)** | Vrcholy spojené s `v` přímo hranou. |
| **Cesta (path)** | Posloupnost vrcholů spojených hranami, žádný se neopakuje. |
| **Sled (walk)** | Posloupnost vrcholů a hran (může se opakovat). |
| **Cyklus** | Cesta začínající a končící ve stejném vrcholu. |
| **Vzdálenost `d(u,v)`** | Délka nejkratší cesty mezi `u` a `v` (počet hran, nebo součet vah). |

**Věta o podání ruky (Handshake lemma):**
V neorientovaném grafu `Σ deg(v) = 2|E|`. Tedy součet stupňů všech vrcholů je sudý a roven dvojnásobku počtu hran. **Důsledek:** počet vrcholů s lichým stupněm je vždy sudý.

```
Příklad grafu (jednoduchý, neorientovaný):
    0 --- 1
    |   / |
    |  /  |
    2 --- 3

V = {0, 1, 2, 3}
E = {{0,1}, {0,2}, {1,2}, {1,3}, {2,3}}
deg(0)=2, deg(1)=3, deg(2)=3, deg(3)=2
Σ = 10 = 2 · 5 = 2|E|  ✓
```

---

### 2. Matice sousednosti (Adjacency Matrix)

**Princip:** 2D pole `M[V × V]`, kde `M[i,j] = 1` pokud existuje hrana z `i` do `j`, jinak `0`.

```
Matice sousednosti pro výše uvedený graf:
    0  1  2  3
0 [ 0, 1, 1, 0 ]
1 [ 1, 0, 1, 1 ]
2 [ 1, 1, 0, 1 ]
3 [ 0, 1, 1, 0 ]
```

**Vlastnosti:**
- Neorientovaný graf ⇒ matice je **symetrická** (`M[i,j] = M[j,i]`).
- Orientovaný graf ⇒ obecně nesymetrická.
- Ohodnocený graf ⇒ místo 0/1 ukládáme **váhu**; neexistující hrana = `int.MaxValue` nebo `nullable int`.
- Smyčky se objeví na diagonále.
- Multigraf nepokryje (jen 0/1 pro každou dvojici); pro multigraf můžete počítat hrany jako `int`.

**Paměť:** `O(V²)` – nezávisí na počtu hran. Pro `V = 10 000` to je 100 milionů buněk (400 MB pro `int`). Pro řídký graf je to **enormní plýtvání**.

**Časové složitosti operací:**
| Operace | Složitost |
|---------|-----------|
| `existuje_hrana(u,v)` | **O(1)** – přímý přístup |
| `pridej_hranu(u,v)` | O(1) |
| `odeber_hranu(u,v)` | O(1) |
| `sousedi(v)` | O(V) – projít celý řádek |
| `stupen(v)` | O(V) – sečíst řádek |
| `projdi_vse` (DFS/BFS) | O(V²) |

**Kód:**
```csharp
int pocetVrcholu = 4;
int[,] matice = new int[pocetVrcholu, pocetVrcholu];

void PridejHranu(int[,] m, int u, int v)
{
    m[u, v] = 1;
    m[v, u] = 1;   // neorientovaný → symetrie
}

bool ExistujeHrana(int[,] m, int u, int v)
{
    return m[u, v] == 1;
}

List<int> DejSousedy(int[,] m, int vrchol, int n)
{
    List<int> sousedi = new List<int>();
    for (int i = 0; i < n; i++)
    {
        if (m[vrchol, i] == 1)
            sousedi.Add(i);
    }
    return sousedi;
}
```

**Ohodnocený graf:**
```csharp
int[,] vahy = new int[n, n];
// inicializace na "neexistuje"
for (int i = 0; i < n; i++)
    for (int j = 0; j < n; j++)
        vahy[i, j] = int.MaxValue;

// hrana s vahou 5
vahy[0, 1] = 5;
vahy[1, 0] = 5;
```

**Kdy použít:**
- **Hustý graf**, kde E ≈ V².
- Malé grafy (do ~1 000 vrcholů).
- Často testujeme `existuje_hrana(u, v)` – potřebujeme O(1) lookup.
- Floyd-Warshall (vyžaduje matici).
- Maticové operace nad grafem (počet cest délky k = `M^k[i,j]`).

---

### 3. Matice incidence (Incidence Matrix)

**Princip:** 2D pole `I[V × E]`. Řádky = vrcholy, sloupce = hrany. `I[i, j] = 1` znamená, že vrchol `i` je incidentní s hranou `j`.

```
Matice incidence:
       e0  e1  e2  e3  e4
  0 [  1,  1,  0,  0,  0 ]
  1 [  1,  0,  1,  1,  0 ]
  2 [  0,  1,  1,  0,  1 ]
  3 [  0,  0,  0,  1,  1 ]
```

(Kde `e0 = {0,1}`, `e1 = {0,2}`, `e2 = {1,2}`, `e3 = {1,3}`, `e4 = {2,3}`.)

**Orientovaná verze:**
- `+1` na řádku zdrojového vrcholu (hrana vychází).
- `−1` na řádku cílového vrcholu (hrana vstupuje).
- `0` jinde.

**Paměť:** `O(V · E)` – mnohem větší než matice sousednosti pro běžné grafy!

**Časové složitosti:**
| Operace | Složitost |
|---------|-----------|
| `existuje_hrana(u,v)` | O(E) – projít všechny hrany |
| `sousedi(v)` | O(V·E) |
| `pridej_hranu(u,v)` | O(V) – nový sloupec |
| `projdi_vse` | O(V·E) |

**Kdy použít:** Téměř nikdy v programování! Je to **teoretický koncept** z matematické teorie grafů. Užitečný pro:
- Multigrafy (snadno rozliší paralelní hrany jako různé sloupce).
- Některé algebraické algoritmy (Tutteho matice).
- Akademické zápisy.

**Pro maturitu je třeba:** umět nakreslit, vysvětlit princip a říct, proč se nepoužívá v praxi (paměť i většina operací je horší než alternativy).

**Kód:**
```csharp
int[,] incidence = new int[pocetVrcholu, pocetHran];

void PridejHranu(int[,] m, int u, int v, int indexHrany)
{
    m[u, indexHrany] = 1;
    m[v, indexHrany] = 1;
}

bool ExistujeHrana(int[,] m, int u, int v, int pocetHran)
{
    for (int e = 0; e < pocetHran; e++)
    {
        if (m[u, e] == 1 && m[v, e] == 1)
            return true;
    }
    return false;
}
```

---

### 4. Seznamy sousedů (Adjacency List)

**Princip:** Pro každý vrchol uchováváme **seznam jeho sousedů**. Ukládáme pouze **existující** hrany – proto úspora pro řídké grafy.

```
Seznamy sousedů:
0 → [ 1, 2 ]
1 → [ 0, 2, 3 ]
2 → [ 0, 1, 3 ]
3 → [ 1, 2 ]
```

**Paměť:** `O(V + E)` – lineární vzhledem k velikosti grafu.

**Časové složitosti:**
| Operace | Složitost |
|---------|-----------|
| `existuje_hrana(u,v)` | **O(deg(u))** – musíme projít seznam |
| `pridej_hranu(u,v)` | O(1) – jen `Add` |
| `odeber_hranu(u,v)` | O(deg(u)) |
| `sousedi(v)` | **O(1)** – přímý přístup k seznamu |
| `stupen(v)` | O(1) (`sousedi[v].Count`) |
| `projdi_vse` (DFS/BFS) | **O(V + E)** – optimální |

**Kdy použít:**
- **Řídký graf** (E ≪ V²), což je drtivá většina reálných grafů.
- Iterace přes sousedy je častá (DFS, BFS, Dijkstra, A*).
- Síť silnic, sociální síť, web, závislosti modulů.

**Kód:**
```csharp
int pocetVrcholu = 4;
List<int>[] sousedi = new List<int>[pocetVrcholu];

// MUSÍŠ inicializovat každý prvek pole zvlášť!
for (int i = 0; i < pocetVrcholu; i++)
    sousedi[i] = new List<int>();

void PridejHranu(List<int>[] s, int u, int v)
{
    s[u].Add(v);
    s[v].Add(u);   // neorientovaný
}

bool ExistujeHrana(List<int>[] s, int u, int v)
{
    return s[u].Contains(v);
}

List<int> DejSousedy(List<int>[] s, int vrchol)
{
    return s[vrchol];
}

void VypisGraf(List<int>[] s, int n)
{
    for (int i = 0; i < n; i++)
    {
        Console.Write(i + " → [ ");
        Console.Write(string.Join(", ", s[i]));
        Console.WriteLine(" ]");
    }
}
```

**Ohodnocený graf přes tuple:**
```csharp
List<(int cil, int vaha)>[] sousedi = new List<(int, int)>[pocetVrcholu];

for (int i = 0; i < pocetVrcholu; i++)
    sousedi[i] = new List<(int, int)>();

sousedi[0].Add((1, 5));   // hrana z 0 do 1 s vahou 5
```

**Generická verze (vrcholy jako stringy, objekty):**
```csharp
Dictionary<string, List<(string cil, int vaha)>> mapa = new();

void Pridej(string od, string kam, int vaha)
{
    if (!mapa.ContainsKey(od)) mapa[od] = new();
    if (!mapa.ContainsKey(kam)) mapa[kam] = new();
    mapa[od].Add((kam, vaha));
    mapa[kam].Add((od, vaha));
}

Pridej("Praha", "Brno", 210);
Pridej("Brno", "Olomouc", 80);
```

**`HashSet<int>` místo `List<int>` pro rychlejší `ExistujeHrana`:**
Pokud často testujeme existenci hrany, použijeme HashSet (lookup O(1) místo O(deg)):
```csharp
HashSet<int>[] sousedi = new HashSet<int>[pocetVrcholu];
for (int i = 0; i < pocetVrcholu; i++)
    sousedi[i] = new HashSet<int>();
```
Trade-off: HashSet má vyšší konstantu paměti a horší cache lokalitu při iteraci. Pro malé `deg(v)` může být `List` rychlejší.

---

### 5. Seznam hran (Edge List)

**Princip:** Jeden seznam **všech hran** jako trojic `(u, v, váha)`. Vrcholy implicitně, hrany explicitně.

```csharp
List<(int od, int kam, int vaha)> hrany = new();
hrany.Add((0, 1, 5));
hrany.Add((1, 2, 3));
hrany.Add((2, 3, 7));
```

**Paměť:** `O(E)` – minimální.

**Časové složitosti:**
| Operace | Složitost |
|---------|-----------|
| `existuje_hrana(u,v)` | O(E) |
| `sousedi(v)` | O(E) |
| `pridej_hranu(u,v)` | O(1) |
| `iterace přes hrany` | **O(E)** – ideální |

**Kdy použít:**
- **Kruskalův algoritmus** pro minimální kostru – potřebuje hrany seřazené podle váhy.
- Vstupní/výstupní formát (textové soubory grafů, JSON).
- Bellman-Ford algoritmus.

Edge list je často **vstupní reprezentace**, kterou pak převedeme na seznamy sousedů pro práci.

---

### 6. Souhrnné srovnání

```
Operace            │ Mat. sousednosti │ Mat. incidence │ Seznamy sousedů │ Seznam hran
═══════════════════╪══════════════════╪════════════════╪═════════════════╪════════════
Existuje hrana?    │     O(1)         │    O(E)        │  O(deg)         │  O(E)
Sousedi vrcholu    │     O(V)         │    O(V·E)      │  O(1)           │  O(E)
Stupeň vrcholu     │     O(V)         │    O(E)        │  O(1)           │  O(E)
Přidej hranu       │     O(1)         │    O(V)        │  O(1)           │  O(1)
Odeber hranu       │     O(1)         │    O(E)        │  O(deg)         │  O(E)
Iterace všech hran │     O(V²)        │    O(V·E)      │  O(V+E)         │  O(E)
─────────────────────────────────────────────────────────────────────────────────────
Paměť              │     O(V²)        │    O(V·E)      │  O(V+E)         │  O(E)
```

---

### 7. Která reprezentace pro jaký graf

**Hustý graf** (E ≈ V², "skoro každý s každým") → **Matice sousednosti**
- Turnaj (každý hraje s každým).
- Floyd-Warshall (all-pairs shortest paths).
- Maticové operace (počet cest = `M^k`).
- Pro V do ~1 000 i u řídkých grafů použitelné (4 MB pro 1000×1000 byte).

**Řídký graf** (E ≪ V², `E = O(V)` nebo `O(V log V)`) → **Seznamy sousedů**
- Sociální sítě (každý má ~stovky přátel).
- Silniční mapy (každá křižovatka má pár sousedních).
- Internet, citation graphs.
- BFS, DFS, Dijkstra, A*.
- Drtivá většina reálných grafů.

**Hrany s váhou potřeba seřadit** → **Seznam hran**
- Kruskal pro minimální kostru.
- Bellman-Ford.

**Matice incidence** → **téměř nikdy** (jen teoretická matematika).

**Pravidlo palce:**
> Pokud nevíš, **použij seznamy sousedů**. Funguje dobře v 90 % případů.

---

### 8. OOP reprezentace grafu

**Princip:** Vrcholy a hrany jsou objekty, graf je objekt obsahující kolekci vrcholů. Výhody: čitelnost, zapouzdření, snadné rozšíření (přidat atributy vrcholu/hraně).

```csharp
class Vrchol
{
    public int Id { get; set; }
    public List<Hrana> Sousedi { get; set; }

    public Vrchol(int id)
    {
        Id = id;
        Sousedi = new List<Hrana>();
    }

    public void PridejHranu(Vrchol cil, int vaha = 1)
    {
        Sousedi.Add(new Hrana(cil, vaha));
    }
}

class Hrana
{
    public Vrchol Cil { get; set; }
    public int Vaha { get; set; }

    public Hrana(Vrchol cil, int vaha)
    {
        Cil = cil;
        Vaha = vaha;
    }
}

class Graf
{
    public List<Vrchol> Vrcholy { get; set; }

    public Graf()
    {
        Vrcholy = new List<Vrchol>();
    }

    public Vrchol PridejVrchol(int id)
    {
        Vrchol novy = new Vrchol(id);
        Vrcholy.Add(novy);
        return novy;
    }

    public Vrchol NajdiVrchol(int id)
    {
        for (int i = 0; i < Vrcholy.Count; i++)
        {
            if (Vrcholy[i].Id == id)
                return Vrcholy[i];
        }
        return null;
    }

    public void PridejHranu(int idOd, int idDo, int vaha = 1)
    {
        Vrchol od = NajdiVrchol(idOd);
        Vrchol cil = NajdiVrchol(idDo);
        if (od != null && cil != null)
        {
            od.PridejHranu(cil, vaha);
            cil.PridejHranu(od, vaha);   // neorientovaný
        }
    }

    public void Vypis()
    {
        for (int i = 0; i < Vrcholy.Count; i++)
        {
            Vrchol v = Vrcholy[i];
            Console.Write(v.Id + " → [ ");
            for (int j = 0; j < v.Sousedi.Count; j++)
            {
                Hrana h = v.Sousedi[j];
                Console.Write(h.Cil.Id + "(" + h.Vaha + ")");
                if (j < v.Sousedi.Count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine(" ]");
        }
    }
}

// Použití:
Graf g = new Graf();
g.PridejVrchol(0);
g.PridejVrchol(1);
g.PridejVrchol(2);
g.PridejVrchol(3);
g.PridejHranu(0, 1, 5);
g.PridejHranu(0, 2, 3);
g.PridejHranu(1, 3, 7);
g.Vypis();
```

**Výhody OOP přístupu:**
- Jasná **sémantika** – „vrchol" a „hrana" mají vlastní typy, ne anonymní indexy.
- **Rozšiřitelnost** – snadno přidám atribut do třídy (`Vrchol.Jmeno`, `Hrana.Typ`).
- **Zapouzdření** – metoda `PridejHranu` na třídě `Graf` automaticky udržuje symetrii.
- **Polymorfismus** – mohu mít specializované podtřídy (`OrientovanyGraf : Graf`).

**Nevýhody:**
- **Vyšší paměťová režie** – každý objekt má v .NET 16+ bajtů hlavičky (object header, syncblock).
- **Cache miss** – objekty rozházené po heapu, nikoli v souvislém poli (na rozdíl od `int[,]`).
- **Pomalejší pro velmi velké grafy** – pro miliony vrcholů preferuji `List<int>[]`.

**Generická verze:**
```csharp
class Graf<T> where T : notnull
{
    private Dictionary<T, List<(T cil, int vaha)>> _adj = new();

    public void PridejVrchol(T v)
    {
        if (!_adj.ContainsKey(v))
            _adj[v] = new List<(T, int)>();
    }

    public void PridejHranu(T od, T kam, int vaha = 1, bool orientovany = false)
    {
        PridejVrchol(od);
        PridejVrchol(kam);
        _adj[od].Add((kam, vaha));
        if (!orientovany)
            _adj[kam].Add((od, vaha));
    }

    public IEnumerable<(T cil, int vaha)> Sousedi(T v) => _adj[v];

    public int Stupen(T v) => _adj[v].Count;
}

// Vrcholy jsou stringy
var mesta = new Graf<string>();
mesta.PridejHranu("Praha", "Brno", 210);
mesta.PridejHranu("Brno", "Olomouc", 80);
```

---

## Speciální typy grafů

**Úplný graf `Kₙ`**
- Každý vrchol spojen s každým jiným.
- Počet hran: `n(n-1)/2` pro neorientovaný, `n(n-1)` pro orientovaný.
- Příklad: turnaj každý s každým.

**Bipartitní graf**
- Vrcholy lze rozdělit do dvou disjunktních množin tak, že hrany vedou jen mezi nimi.
- Příklad: studenti × předměty, lidé × pracovní pozice.
- Lze testovat 2-obarvením (BFS).

**k-regulární graf**
- Všechny vrcholy mají stejný stupeň k.
- Speciální případ: 3-regulární = kubický graf.

**Planární graf**
- Lze nakreslit v rovině bez křížení hran.
- Eulerův vztah: `V - E + F = 2`, kde F je počet stěn.
- `K₅` ani `K₃,₃` nejsou planární (Kuratowského věta).

**DAG (Directed Acyclic Graph)**
- Orientovaný graf bez cyklů.
- Lze provést **topologické řazení** vrcholů.
- Použití: závislosti modulů, build systémy, scheduling úloh.

---

## Maturitní úlohy k procvičení

| # | Úloha | Popis | Soubor | Obtížnost |
|---|-------|-------|--------|-----------|
| 1 | **352** | Existuje autobusové spojení mezi městy? | 33-69 | lehká |
| 2 | **353** | Do kterých měst se dostanu z výchozího? | 33-69 | lehká |
| 3 | **354** | Skupinky lidí – komponenty souvislosti | 33-69 | střední |
| 4 | **355** | Cesta bludištěm z X do Y | 33-69 | střední |
| 5 | **356** | Letiště – nejméně přestupů (BFS) | 33-69 | střední |
| 6 | **383** | Věž na šachovnici přes překážky | 33-69 | střední |
| 7 | **384** | Šachový kůň – min. počet tahů | 33-69 | vyšší |
| 8 | **385** | Bludiště ve čtverečkové síti | 33-69 | střední |

---

## Maturitní chytáky

1. **Zapomenutá inicializace `List<int>[]`** – pole listů vyžaduje `new List<int>()` pro každý prvek, jinak `NullReferenceException`.

2. **Symetrie u neorientovaného grafu** – vždy přidat hranu **oběma směry** (`m[u,v]` i `m[v,u]`, případně `sousedi[u].Add(v)` i `sousedi[v].Add(u)`).

3. **Neexistující hrana u ohodnoceného grafu** – nepoužívej 0, ale `int.MaxValue` nebo `int?` (nullable). Jinak nelze rozlišit "neexistuje" od "váha 0". Pozor také na **overflow** při sčítání `int.MaxValue + cokoliv` v algoritmech jako Floyd-Warshall.

4. **Matice incidence** – v praxi se nepoužívá, ale na maturitě tě na ni zkoušející může vyzkoušet. Umět nakreslit, vysvětlit princip a říct **proč** je neefektivní (O(V·E) paměti, O(E) lookup).

5. **Stupeň vrcholu** – vědět, co to je (počet sousedů), a proč ovlivňuje složitost u seznamů sousedů.

6. **Off-by-one chyba v číslování** – vrcholy se v algoritmech typicky číslují od 0, ale v zadání úloh často od 1. Pozor na konverzi `idVZadani - 1` ↔ `indexVPoli`.

7. **Orientovaný vs. neorientovaný** – v matici sousednosti je rozdíl v symetrii; v seznamech sousedů přidáváme jen jeden směr.

8. **Multigraf vs. prostý graf** – matice sousednosti reprezentuje jen 0/1, nikoliv "kolik hran"; pro multigraf je třeba `int` (počet hran) nebo přechod na seznam hran.

9. **Smyčka (loop)** – v matici sousednosti se objeví na diagonále; v seznamech sousedů je vrchol sám sebe ve svém seznamu.

10. **Volba reprezentace** – při maturitě se zeptají "kterou bys zvolil a proč". Klíč: hustý vs. řídký, jaké operace jsou časté, jaká je dostupná paměť.

---

## Souvislosti s jinými otázkami

- **Otázka 17 (OOP)** – OOP reprezentace grafu je přímá aplikace tříd, zapouzdření, konstruktorů.
- **Otázka 21 (Teorie grafů)** – definice pojmů, bipartitní graf, taky vyžaduje reprezentaci.
- **Otázka 22 (DFS/BFS)** – tyto algoritmy pracují nad reprezentací grafu (seznamy sousedů ideální).
- **Otázka 25 (Dijkstra)** – nejkratší cesta potřebuje ohodnocený graf (seznamy sousedů s váhami).
- **Otázka 23 (Minimální kostra)** – Kruskal/Jarník pracují s ohodnoceným grafem.
- **Otázka 9 (Stromy)** – strom je speciální graf (souvislý, bez cyklů) – podobná reprezentace.
- **Otázka 3 (Fronta/Zásobník)** – BFS používá frontu, DFS zásobník při průchodu grafem.

---

## Klíčové pojmy k zapamatování

- **Graf** `G = (V, E)` – množina vrcholů a hran.
- **Neorientovaný / orientovaný (digraph)** – směr hran.
- **Ohodnocený / vážený graf** – hrany mají číselnou hodnotu (vzdálenost, cena).
- **Multigraf** – povoluje paralelní hrany.
- **Smyčka (loop)** – hrana z vrcholu do sebe.
- **Stupeň vrcholu `deg(v)`** – počet hran u něj; smyčka 2×.
- **Vstupní / výstupní stupeň** – pro orientované grafy.
- **Souvislý graf** – mezi každými dvěma vrcholy existuje cesta.
- **Komponenta souvislosti** – maximální souvislý podgraf.
- **Strom** – souvislý acyklický graf; má `V − 1` hran.
- **DAG** – orientovaný acyklický graf; lze topologicky uspořádat.
- **Úplný graf `Kₙ`** – každý s každým, `n(n−1)/2` hran.
- **Bipartitní graf** – vrcholy ve dvou disjunktních množinách, hrany jen mezi nimi.
- **Řídký graf** – `E = O(V)` nebo `O(V log V)`.
- **Hustý graf** – `E = Θ(V²)`.
- **Cesta / sled / cyklus** – posloupnost vrcholů a hran s/bez opakování.
- **Vzdálenost** – délka nejkratší cesty.
- **Handshake lemma** – `Σ deg(v) = 2|E|`.
- **Matice sousednosti** – V×V, O(1) lookup, O(V²) paměť, vhodná pro husté grafy.
- **Matice incidence** – V×E, jen teoretická, O(V·E) paměť.
- **Seznam sousedů** – pro každý vrchol list jeho sousedů, O(V+E) paměť, vhodný pro řídké grafy.
- **Seznam hran** – jen list hran, pro Kruskal a vstupní reprezentaci.
- **OOP reprezentace** – třídy `Vrchol`, `Hrana`, `Graf`; čitelná, rozšiřitelná, ale s paměťovou režií.
- **Topologické řazení** – uspořádání vrcholů DAGu tak, aby všechny hrany šly "dopředu".
- **Planární graf** – nakreslitelný bez křížení hran.
