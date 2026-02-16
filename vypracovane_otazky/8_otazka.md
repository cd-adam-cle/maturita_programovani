# 📚 Zápisky: Otázka č. 8 - Reprezentace grafu v počítači
**Datum:** 2025-02-09
**Status:** ✅ Hotovo (teorie) | ⬜ Procvičení úloh

---

## ✅ Checklist bodů otázky
- [x] Bod 1: Definice grafu
- [x] Bod 2: Matice sousednosti
- [x] Bod 3: Matice incidence
- [x] Bod 4: Seznamy sousedů
- [x] Bod 5: Časová náročnost základních metod
- [x] Bod 6: Která reprezentace pro jaký typ grafu
- [x] Bod 7: Reprezentace grafu pomocí OOP

---

## 🧠 Klíčové koncepty & Snippety

### Bod 1: Definice grafu

**Teorie:**
- Graf G = (V, E) → V = množina vrcholů, E = množina hran
- **Neorientovaný** – hrany bez směru (silnice oběma směry)
- **Orientovaný (digraf)** – hrany mají směr (jednosměrka)
- **Ohodnocený** – hrany mají váhu (vzdálenost, cena)
- **Souvislý** – z každého vrcholu se dostaneš do každého jiného
- **Stupeň vrcholu** – počet hran vedoucích z/do vrcholu

```
Příklad grafu:
    0 --- 1
    |   / |
    |  /  |
    2 --- 3

V = {0, 1, 2, 3}
E = {{0,1}, {0,2}, {1,2}, {1,3}, {2,3}}
```

**Základní pojmy:**
| Pojem | Význam |
|-------|--------|
| Stupeň vrcholu | Počet hran vedoucích z/do vrcholu |
| Cesta | Posloupnost vrcholů spojených hranami |
| Cyklus | Cesta začínající a končící ve stejném vrcholu |
| Strom | Souvislý graf bez cyklů |
| Komponenta souvislosti | Maximální souvislý podgraf |

---

### Bod 2: Matice sousednosti

**Teorie:**
- 2D pole `[V × V]`, na pozici `[i,j]` je 1 pokud vede hrana, jinak 0
- Neorientovaný graf → matice je **symetrická**
- Ohodnocený graf → místo 0/1 píšeš váhu, neexistující hrana = `int.MaxValue`

```
Matice sousednosti:
    0  1  2  3
0 [ 0, 1, 1, 0 ]
1 [ 1, 0, 1, 1 ]
2 [ 1, 1, 0, 1 ]
3 [ 0, 1, 1, 0 ]
```

**Kód (Maturitní verze):**
```csharp
int pocetVrcholu = 4;
int[,] matice = new int[pocetVrcholu, pocetVrcholu];

void PridejHranu(int[,] m, int u, int v)
{
    m[u, v] = 1;
    m[v, u] = 1; // neorientovaný → symetrie
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

**Paměť:** O(V²)

**Složitosti:**
| Operace | Složitost |
|---------|-----------|
| Existuje hrana? | O(1) ✅ |
| Sousedi | O(V) |
| Přidej/Odeber hranu | O(1) |
| Projdi vše | O(V²) |

---

### Bod 3: Matice incidence

**Teorie:**
- 2D pole `[V × E]`, řádky = vrcholy, sloupce = hrany
- Na pozici `[i,j]` je 1 pokud vrchol i leží na hraně j
- Orientovaný graf: +1 = hrana vychází, -1 = hrana vstupuje
- V praxi se **téměř nepoužívá** – spíš teoretický koncept

```
Matice incidence:
       e0  e1  e2  e3  e4
  0 [  1,  1,  0,  0,  0 ]
  1 [  1,  0,  1,  1,  0 ]
  2 [  0,  1,  1,  0,  1 ]
  3 [  0,  0,  0,  1,  1 ]
```

**Kód (Maturitní verze):**
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

**Paměť:** O(V × E)

**Složitosti:**
| Operace | Složitost |
|---------|-----------|
| Existuje hrana? | O(E) |
| Sousedi | O(V × E) |
| Přidej hranu | O(V) |
| Projdi vše | O(V × E) |

---

### Bod 4: Seznamy sousedů

**Teorie:**
- Pro každý vrchol `List` jeho sousedů
- Ukládáš jen to, co existuje → žádné plýtvání
- **Nejpoužívanější** reprezentace v praxi

```
Seznamy sousedů:
0 → [ 1, 2 ]
1 → [ 0, 2, 3 ]
2 → [ 0, 1, 3 ]
3 → [ 1, 2 ]
```

**Kód (Maturitní verze):**
```csharp
int pocetVrcholu = 4;
List<int>[] sousedi = new List<int>[pocetVrcholu];

// NESMÍŠ ZAPOMENOUT inicializaci!
for (int i = 0; i < pocetVrcholu; i++)
    sousedi[i] = new List<int>();

void PridejHranu(List<int>[] s, int u, int v)
{
    s[u].Add(v);
    s[v].Add(u); // neorientovaný
}

bool ExistujeHrana(List<int>[] s, int u, int v)
{
    return s[u].Contains(v);
}

List<int> DejSousedy(List<int>[] s, int vrchol)
{
    return s[vrchol]; // hotové!
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

**Ohodnocený graf:**
```csharp
List<(int cil, int vaha)>[] sousedi = new List<(int, int)>[pocetVrcholu];

sousedi[0].Add((1, 5));  // z 0 do 1, váha 5
```

**Paměť:** O(V + E) ✅

**Složitosti:**
| Operace | Složitost |
|---------|-----------|
| Existuje hrana? | O(stupeň) |
| Sousedi | O(1) ✅ |
| Přidej hranu | O(1) ✅ |
| Projdi vše | O(V + E) ✅ |

---

### Bod 5: Souhrnné srovnání složitostí

```
Operace            │ Mat. sousednosti │ Mat. incidence │ Seznamy sousedů
═══════════════════╪══════════════════╪════════════════╪════════════════
Existuje hrana?    │     O(1)  ✅     │    O(E)        │  O(stupeň)
Sousedi vrcholu    │     O(V)         │    O(V×E)      │  O(stupeň) ✅
Přidej hranu       │     O(1)  ✅     │    O(V)        │  O(1)  ✅
Odeber hranu       │     O(1)  ✅     │    O(E)        │  O(stupeň)
Projdi vše         │     O(V²)        │    O(V×E)      │  O(V+E)  ✅
───────────────────┼──────────────────┼────────────────┼────────────────
Paměť              │     O(V²)        │    O(V×E)      │  O(V+E)  ✅
```

---

### Bod 6: Která reprezentace pro jaký graf

**Hustý graf** (hodně hran, E ≈ V²) → **Matice sousednosti**
- Turnaj (každý s každým)
- Malé grafy (do ~1000 vrcholů)
- Floyd-Warshall algoritmus

**Řídký graf** (málo hran, E << V²) → **Seznamy sousedů**
- Sociální sítě, silniční mapy, internet
- BFS/DFS prohledávání
- Dijkstra

**Matice incidence** → téměř nikdy (jen matematická teorie grafů)

**Ohodnocený graf:**
- Matice sousednosti: váha na `[i,j]`, neexistující = `int.MaxValue`
- Seznamy sousedů: `List<(int cil, int vaha)>[]`
- Pravidlo výběru se nemění (hustý → matice, řídký → seznamy)

---

### Bod 7: OOP reprezentace

**Teorie:**
- Třída `Vrchol` – id, seznam hran
- Třída `Hrana` – cílový vrchol, váha
- Třída `Graf` – seznam vrcholů, metody pro operace
- Výhody: čitelnost, rozšiřitelnost, zapouzdření

**Kód (Maturitní verze):**
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
        Vrchol doV = NajdiVrchol(idDo);
        if (od != null && doV != null)
        {
            od.PridejHranu(doV, vaha);
            doV.PridejHranu(od, vaha); // neorientovaný
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

**Senior verze (Nice to Have):**
```csharp
class Graf<T> where T : notnull
{
    private Dictionary<T, List<(T cil, int vaha)>> _adj = new();

    public void PridejVrchol(T v)
    {
        if (!_adj.ContainsKey(v)) _adj[v] = new();
    }

    public void PridejHranu(T od, T kam, int vaha = 1, bool orientovany = false)
    {
        PridejVrchol(od);
        PridejVrchol(kam);
        _adj[od].Add((kam, vaha));
        if (!orientovany) _adj[kam].Add((od, vaha));
    }

    public IEnumerable<(T cil, int vaha)> Sousedi(T v) => _adj[v];
}

// Generický → vrcholy mohou být cokoliv
var mesta = new Graf<string>();
mesta.PridejHranu("Praha", "Brno", 210);
```

---

## 📋 Maturitní úlohy k procvičení

| # | Úloha | Popis | Soubor | Obtížnost |
|---|-------|-------|--------|-----------|
| 1 | **352** | Existuje autobusové spojení mezi městy? | 33-69 | ⭐ |
| 2 | **353** | Do kterých měst se dostanu z výchozího? | 33-69 | ⭐ |
| 3 | **354** | Skupinky lidí – komponenty souvislosti | 33-69 | ⭐⭐ |
| 4 | **355** | Cesta bludištěm z X do Y | 33-69 | ⭐⭐ |
| 5 | **356** | Letiště – nejméně přestupů (BFS) | 33-69 | ⭐⭐ |
| 6 | **383** | Věž na šachovnici přes překážky | 33-69 | ⭐⭐ |
| 7 | **384** | Šachový kůň – min. počet tahů | 33-69 | ⭐⭐⭐ |
| 8 | **385** | Bludiště ve čtverečkové síti | 33-69 | ⭐⭐ |

**Status:** ⬜ Neprocvičeno

---

## ⚠️ Na co si dát pozor (Maturitní "chytáky")

1. **Zapomenutá inicializace `List<int>[]`** – pole listů vyžaduje `new List<int>()` pro KAŽDÝ prvek, jinak `NullReferenceException`
2. **Symetrie u neorientovaného grafu** – vždy přidej hranu OBĚMA směry (`m[u,v]` i `m[v,u]`)
3. **Neexistující hrana u ohodnoceného grafu** – nepoužívej 0, ale `int.MaxValue` nebo `int?` (nullable), jinak nerozlišíš "hrana neexistuje" od "hrana s váhou 0"
4. **Matice incidence** – zkoušející se rád zeptá, ale v praxi se nepoužívá. Umět nakreslit příklad a říct proč je neefektivní
5. **Stupeň vrcholu** – vědět co to je (počet sousedů) a proč ovlivňuje složitost u seznamů sousedů
6. **Off-by-one** – vrcholy se typicky číslují od 0, ale v zadání úloh často od 1

---

## 🚀 Senior Tip

V praxi se nejčastěji používají **seznamy sousedů** implementované přes `Dictionary<T, List<(T, int)>>`. Generický přístup umožňuje, že vrcholy nemusí být jen čísla – mohou být stringy (města), objekty (uzly sítě), cokoliv. Kombinace s OOP přístupem (vlastní třída `Graf<T>`) dává čistý, rozšiřitelný a testovatelný kód.

---

## 🔗 Souvislosti s jinými otázkami

- **Otázka 17 (OOP)** – OOP reprezentace grafu je přímá aplikace tříd, zapouzdření, konstruktorů
- **Otázka 21 (Teorie grafů)** – definice pojmů, bipartitní graf, taky vyžaduje reprezentaci
- **Otázka 22 (DFS/BFS)** – tyto algoritmy pracují NAD reprezentací grafu (seznamy sousedů ideální)
- **Otázka 25 (Dijkstra)** – nejkratší cesta potřebuje ohodnocený graf (seznamy sousedů s váhami)
- **Otázka 23 (Minimální kostra)** – Kruskal/Jarník pracuje s ohodnoceným grafem
- **Otázka 9 (Stromy)** – strom JE graf (souvislý, bez cyklů) – podobná reprezentace
- **Otázka 3 (Fronta/Zásobník)** – BFS používá frontu, DFS zásobník při průchodu grafem
