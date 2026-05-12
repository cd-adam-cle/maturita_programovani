# Zápisky: Otázka č. 21 – Základy teorie grafů. Bipartitní graf.
**Datum:** 2026-03-01
**Status:** Hotovo

---

## Checklist bodů otázky
- [x] Bod 1: Definice pojmů (graf, ohodnocený, orientovaný, souvislý, cesta, sled, nejkratší cesta, cyklus, strom, komponenta souvislosti, úplný graf)
- [x] Bod 2: Bipartitní graf – definice, vlastnosti, využití
- [x] Bod 3: Problém největšího párování
- [x] Bod 4: Reprezentace grafu v počítači

---

## Klíčové koncepty & Snippety

### Bod 1: Definice pojmů

**Graf** = `G = (V, E)` kde V = vrcholy, E = hrany

```
Příklad:
   (1)---(2)
    |   / |
    |  /  |
   (3)---(4)    (5)

V = {1,2,3,4,5}
E = {(1,2),(1,3),(2,3),(2,4),(3,4)}
```

| Pojem | Definice | Příklad |
|-------|----------|---------|
| **Graf** | G = (V, E), vrcholy + hrany | viz výše |
| **Ohodnocený graf** | hrany mají váhu (číslo) | mapa měst s km |
| **Orientovaný graf** | hrany mají směr (šipky) | jednosměrky |
| **Souvislý graf** | z každého vrcholu se dostaneš kamkoliv | 1 komponenta |
| **Sled** | posloupnost vrcholů, vrcholy se **mohou** opakovat | 1→2→3→2 |
| **Cesta** | sled kde se **žádný vrchol neopakuje** | 1→2→4→3 |
| **Nejkratší cesta** | cesta s nejmenším součtem vah | Dijkstra |
| **Cyklus/Kružnice** | cesta začínající a končící ve stejném vrcholu | 1→2→4→3→1 |
| **Strom** | souvislý graf **bez cyklů**, platí \|E\| = \|V\|-1 | hierarchie |
| **Komponenta souvislosti** | maximální souvislá část grafu | {1,2,3,4} a {5} |
| **Úplný graf Kₙ** | každý vrchol spojen s každým, \|E\| = n*(n-1)/2 | K4 má 6 hran |

```
Orientovaný graf:        Strom:          Úplný graf K4:
  (1)──►(2)               (1)              (1)---(2)
   ▲      │              /   \              | \ / |
   │      ▼            (2)   (3)            |  X  |
  (3)◄──(4)            /                   | / \ |
                      (4)                 (3)---(4)
```

---

### Bod 2: Bipartitní graf

**Definice:** Graf jehož vrcholy lze rozdělit do **2 skupin (partit)** tak, že každá hrana vede **mezi skupinami** — nikdy uvnitř jedné skupiny.

```
   L          R
  (A)────── (X)
  (A)────── (Y)
  (B)────── (Y)
  (B)────── (Z)
  (C)────── (X)

 Žádná hrana A-B, X-Y apod.
```

**Zlaté pravidlo:** Graf je bipartitní ↔ **neobsahuje cyklus liché délky**

```
 Bipartitní (cyklus délky 4):     NENÍ bipartitní (trojúhelník = délka 3):
  (1)---(2)                              (1)
   |     |                              / \
  (3)---(4)                           (2)-(3)
```

**Detekce – BFS obarvení 2 barvami:**
1. Vezmi libovolný vrchol, obarvi ho červeně
2. Všechny sousedy obarvi modře
3. Jejich sousedy zase červeně...
4. Pokud soused = stejná barva jako ty → **NENÍ bipartitní**

```csharp
//  VERZE A – MATURITNÍ
bool JeBipartitni(List<int>[] sousedi, int n)
{
    int[] barva = new int[n]; // 0=neobarvený, 1=červená, -1=modrá

    for (int start = 0; start < n; start++)
    {
        if (barva[start] != 0) continue;

        Queue<int> fronta = new Queue<int>();
        fronta.Enqueue(start);
        barva[start] = 1;

        while (fronta.Count > 0)
        {
            int vrchol = fronta.Dequeue();
            foreach (int soused in sousedi[vrchol])
            {
                if (barva[soused] == 0)
                {
                    barva[soused] = -barva[vrchol]; // opačná barva
                    fronta.Enqueue(soused);
                }
                else if (barva[soused] == barva[vrchol]) // stejná barva = chyba!
                {
                    return false;
                }
            }
        }
    }
    return true;
}
```

**Využití bipartitních grafů:**

| Situace | Levá partita | Pravá partita | Hrana = |
|---------|-------------|--------------|---------|
| Pracovní agentúra | Uchazeči | Pracovní pozice | "umí tuto práci" |
| Rozvrh hodin | Učitelé | Předměty | "může učit" |
| Doporučovací systém | Uživatelé | Filmy/produkty | "líbí se mu" |
| Taxi (Uber) | Řidiči | Zákazníci | "může odvézt" |

---

### Bod 3: Problém největšího párování

**Párování** = výběr hran tak, že **každý vrchol je v nejvýše jedné hraně**.
**Největší párování** = chceme co nejvíc párů.

```
Uchazeči    Pozice
  (A)────── (X)
  (A)────── (Y)
  (B)────── (Y)
  (C)────── (Z)

Maximální párování: A=X, B=Y, C=Z  → 3 páry
```

**Augmentující cesta** = cesta střídající nespárované/spárované hrany, začíná i končí **volným vrcholem**. Přehozením získáš o 1 pár navíc.

```
PŘED:                        PO přehození:
Adam ─── Anna (nespárováno)  Adam ═══ Bára
Adam ═══ Bára (spárováno)    Bob  ═══ Bára...
Bob  ─── Bára (nespárováno)
→ Augmentující cesta: Anna ─ Adam ═ Bára ─ Bob
```

**Klíčová myšlenka:** Dokud existuje augmentující cesta → párování lze zvětšit. Když žádná neexistuje → máme **maximální párování**.

```csharp
//  VERZE A – MATURITNÍ (DFS hledání augmentující cesty)
int[] parovani; // parovani[v] = kdo z levé je spárován s v (-1 = nikdo)

bool NajdiAugmentujiciCestu(int u, List<int>[] adj, bool[] navstiveny)
{
    foreach (int v in adj[u])
    {
        if (!navstiveny[v])
        {
            navstiveny[v] = true;
            if (parovani[v] == -1 || NajdiAugmentujiciCestu(parovani[v], adj, navstiveny))
            {
                parovani[v] = u;
                return true;
            }
        }
    }
    return false;
}

int MaxParovani(List<int>[] adj, int n, int m)
{
    parovani = new int[m];
    for (int i = 0; i < m; i++) parovani[i] = -1;

    int vysledek = 0;
    for (int u = 0; u < n; u++)
    {
        bool[] navstiveny = new bool[m];
        if (NajdiAugmentujiciCestu(u, adj, navstiveny))
            vysledek++;
    }
    return vysledek;
}
```

**Časová složitost:** O(V · E)

---

### Bod 4: Reprezentace grafu v počítači

#### 1. Matice sousednosti
```
Graf:           Matice [V×V]:
  1---2           1  2  3  4
  | \ |         1[0, 1, 1, 1]
  4---3         2[1, 0, 0, 1]
                3[1, 0, 0, 1]
                4[1, 1, 1, 0]
```
- Paměť: **O(V²)**
- Hrana existuje?: **O(1)**
- Sousedé?: **O(V)**

#### 2. Matice incidence
- Řádky = vrcholy, sloupce = hrany
- Paměť: O(V·E) — **v praxi se nepoužívá**

#### 3. Seznamy sousedů ← nejpoužívanější
```csharp
List<int>[] sousedi = new List<int>[n];
for (int i = 0; i < n; i++)
    sousedi[i] = new List<int>();

sousedi[0].Add(1); // hrana 0→1
sousedi[1].Add(0); // neorientovaný → oběma směry
```
- Paměť: **O(V + E)**
- Hrana existuje?: O(stupeň)
- Sousedé?: **O(stupeň)**

#### Kdy co použít?
| | Matice sousednosti | Seznamy sousedů |
|---|---|---|
| Hustý graf | vhodné | plýtvá pamětí |
| Řídký graf | plýtvá pamětí | vhodné |
| "Existuje hrana X-Y?" | O(1) | O(stupeň) |
| DFS/BFS algoritmy | pomalé | rychlé |

---

## Na co si dát pozor (Maturitní "chytáky")

- **Sled vs Cesta** — ve sledu se vrcholy mohou opakovat, v cestě NE
- **Strom** — musí být **souvislý** A **bez cyklů** (obojí!)
- **Bipartitní ≠ jen 2 skupiny** — musí platit, že hrany vedou POUZE mezi skupinami
- **Cyklus liché délky** — trojúhelník (délka 3) je nejčastější důkaz, že graf NENÍ bipartitní
- **Inicializace seznamů sousedů** — `new List<int>()` pro KAŽDÝ prvek pole, jinak `NullReferenceException`
- **Neorientovaný graf** — při přidání hrany přidej OBĚMA směrům

---

## Senior Tip

Bipartitní grafy jsou základem **doporučovacích systémů** (Netflix, Spotify). V praxi se reprezentují jako řídké matice (sparse matrix) nebo slovníky `Dictionary<int, HashSet<int>>`, protože uživatelů jsou miliony ale každý má hodnocení jen pár set filmů — klasické seznamy sousedů by byly neefektivní.

---

## Souvislosti s jinými otázkami

- **Otázka 8** (Reprezentace grafu) — Bod 4 je přímé zopakování
- **Otázka 22** (DFS/BFS) — detekce bipartitnosti používá BFS
- **Otázka 24** (Topologické třídění) — orientovaný graf, DAG
- **Otázka 25** (Dijkstra) — ohodnocený graf, nejkratší cesta
- **Otázka 9** (Stromy) — strom je speciální případ grafu
