# Zápisky: Otázka č. 25 - Hledání nejkratší cesty v grafu

**Datum:** 2025-02-17
**Status:** KOMPLETNÍ (7/7 bodů)
**Předmět:** Programování – Maturitní příprava

---

## Checklist bodů otázky

| # | Bod | Status |
|---|-----|--------|
| 1 | Definice pojmů (graf, ohodnocený graf, vzdálenost, cesta, nejkratší cesta, záporné hrany) | |
| 2 | Motivační příklad z praxe | |
| 3 | Příklady úloh vedoucích na hledání nejkratší cesty | |
| 4 | Využití BFS pro nejkratší cestu (neohodnocený graf) | |
| 5 | Využití DFS (a proč není ideální) | |
| 6 | Dijkstrův algoritmus s minimovou haldou | |
| 7 | Omezení Dijkstry (záporné hrany, alternativy) | |

---

# BOD 1: DEFINICE POJMŮ

## Graf
Dvojice G = (V, E), kde V = množina vrcholů, E = množina hran.

```
    A --- B
    |     |
    C --- D

V = {A, B, C, D}
E = {(A,B), (A,C), (B,D), (C,D)}
```

## Ohodnocený graf (weighted graph)
Graf, kde každá hrana má přiřazenou **váhu** (číslo) – vzdálenost, cena, čas.

```
    A --5-- B
    |       |
    2       3
    |       |
    C --1-- D
```

## Vzdálenost
Součet vah hran na cestě mezi dvěma vrcholy. V neohodnoceném grafu = počet hran.

## Cesta (path)
Posloupnost vrcholů v₁, v₂, ..., vₖ, kde každé dva po sobě jdoucí jsou spojeny hranou. Žádný vrchol se neopakuje.

## Nejkratší cesta
Cesta s **minimální celkovou vzdáleností** (minimální součet vah).

```
A→D přes graf výše:
  A → B → D = 5 + 3 = 8
  A → C → D = 2 + 1 = 3   (nejkratší!)
```

## Záporné hrany
Hrany se zápornou vahou. Mohou vytvořit **záporný cyklus** – cyklus, kde součet vah < 0. Pak nejkratší cesta **neexistuje** (→ minus nekonečno).

**Dijkstra záporné hrany NEUMÍ!** → Bellman-Ford.

---

# BOD 2: MOTIVAČNÍ PŘÍKLAD Z PRAXE

## Navigace (Google Maps, Mapy.cz)

```
         Praha
        /     \
      80km    120km
      /         \
   Plzeň --90km-- Brno
      \         /
      60km    40km
        \     /
        Olomouc
```

- **Vrcholy** = města
- **Hrany** = silnice
- **Váhy** = vzdálenost (nebo čas, spotřeba, mýtné)

Nejkratší Praha → Olomouc: Praha → Plzeň → Olomouc = 140 km

## Další příklady:
- **Síťový routing** – pakety hledají nejrychlejší cestu internetem
- **Logistika** – optimální rozvoz zásilek
- **Herní AI** – pathfinding (A* = rozšíření Dijkstry)
- **Sociální sítě** – stupně oddělení (BFS)
- **IDOS** – spoje s nejméně přestupy

**Klíčové:** Váha hrany nemusí být vzdálenost – může to být čas, cena, spotřeba.

---

# BOD 3: PŘÍKLADY ÚLOH

## Typ 1: Explicitní graf – města a silnice
Města, silnice s délkami → **Dijkstra**

## Typ 2: Neohodnocený graf – nejmenší počet kroků
Každá hrana = 1. Hledáme nejméně hran (přestupů, tahů) → **BFS stačí!**

Příklad: MHD s nejméně přestupy, šachový kůň (min. tahů)

## Typ 3: Mřížkový graf (grid) – bludiště

```
S . . #
# # . #
. . . .
# # . E

S = start, E = end, # = stěna, . = volno
→ Každé políčko = vrchol, sousední volná = hrany → BFS
```

## Typ 4: Stavový prostor
Stavy = vrcholy, přechody = hrany. Příklad: Rubikova kostka.

## Jak rozpoznat typ:

| Situace | Váhy | Algoritmus |
|---------|------|------------|
| Všechny hrany stejné (bez vah) | 1 | **BFS** |
| Různé kladné váhy | kladné | **Dijkstra** |
| Záporné váhy (bez záporného cyklu) | i záporné | **Bellman-Ford** |

---

# BOD 4: BFS PRO NEJKRATŠÍ CESTU

## Proč BFS funguje?
BFS prochází graf **po vrstvách** (vzdálenost 0, 1, 2...). FIFO fronta garantuje, že první nalezení cíle = nejkratší cesta.

## Klíč: Pole předchůdců (parent/previous)
U každého vrcholu si pamatujeme **odkud jsme přišli** → rekonstrukce cesty pozpátku.

## Implementace:

```csharp
static List<int> BFS_NejkratsiCesta(List<int>[] graf, int start, int cil)
{
    int n = graf.Length;
    bool[] navstiveno = new bool[n];
    int[] predchudce = new int[n];

    for (int i = 0; i < n; i++)
        predchudce[i] = -1;

    Queue<int> fronta = new Queue<int>();
    fronta.Enqueue(start);
    navstiveno[start] = true;

    while (fronta.Count > 0)
    {
        int aktualni = fronta.Dequeue();

        if (aktualni == cil)
            return RekonstruujCestu(predchudce, start, cil);

        foreach (int soused in graf[aktualni])
        {
            if (!navstiveno[soused])
            {
                navstiveno[soused] = true;
                predchudce[soused] = aktualni;
                fronta.Enqueue(soused);
            }
        }
    }

    return null;  // Cesta neexistuje
}

static List<int> RekonstruujCestu(int[] predchudce, int start, int cil)
{
    List<int> cesta = new List<int>();
    int aktualni = cil;

    while (aktualni != -1)
    {
        cesta.Add(aktualni);
        aktualni = predchudce[aktualni];
    }

    cesta.Reverse();
    return cesta;
}
```

## Vizualizace rekonstrukce:

```
Pole předchůdců po BFS (start=0, cíl=5):
Index:      0    1    2    3    4    5
Předchůdce: -1   0    0    1    2    3

Rekonstrukce od cíle zpět:
  5 → pred[5]=3 → pred[3]=1 → pred[1]=0 → STOP (-1)
Pozpátku: 5, 3, 1, 0
Reverse:  0 → 1 → 3 → 5
```

## Složitost:
- **Časová:** O(V + E)
- **Paměťová:** O(V) – pole navstiveno, predchudce, fronta

## Omezení BFS:
Funguje **POUZE** pro neohodnocený graf (nebo všechny hrany stejná váha). Pro různé váhy → Dijkstra.

---

# BOD 5: DFS A PROČ NENÍ IDEÁLNÍ

## DFS najde NĚJAKOU cestu, ale ne nutně nejkratší

```
    A --- B --- C --- E
    |                 |
    +---D---F---G---H-+

DFS (přes D): A → D → F → G → H → E  (5 hran)
BFS:          A → B → C → E            (3 hrany)
```

## Proč?
DFS jde **do hloubky** – najde první cestu, na kterou narazí. BFS díky vrstvám garantuje, že první nalezení = nejkratší.

## Dá se DFS "opravit"?
Ano, ale musí prozkoumat **VŠECHNY** cesty (backtracking) → až O(V!) = exponenciální. Neefektivní.

## Kde se DFS reálně používá:
- Zjistit **zda cesta existuje** (ano/ne)
- **Topologické třídění** (otázka 24)
- **Detekce cyklů**
- **Backtracking** (Sudoku, N dam)

## Co říct u maturity:
> „DFS negarantuje nejkratší cestu, protože prochází graf do hloubky a může najít dlouhou cestu dřív než krátkou. Pro nejkratší cestu v neohodnoceném grafu použijeme BFS."

---

# BOD 6: DIJKSTRŮV ALGORITMUS S MINIMOVOU HALDOU

## Co řeší?
Nejkratší cestu z jednoho startu do všech ostatních ve **váženém grafu s kladnými hranami**.

## Hlavní myšlenka:
**Greedy** – v každém kroku zpracuj vrchol s nejmenší známou vzdáleností. Jako rozlévající se voda – teče nejdřív tam, kam je to nejblíž.

## Algoritmus krok za krokem:

```
1. Nastav dist[start] = 0, ostatní = ∞
2. Vlož start do prioritní fronty (min-heap)
3. Opakuj dokud fronta není prázdná:
   a) Vyber vrchol s NEJMENŠÍ vzdáleností
   b) Pro každého souseda:
      - Spočítej: nová = dist[aktuální] + váha hrany
      - Pokud nová < dist[soused] → aktualizuj (RELAXACE)
      - Vlož souseda do fronty
```

## Vizualizace:

```
Graf:  A --4-- B
       |      /|
       2    1  6
       |  /    |
       C --3-- D

KROK 0: dist[A]=0, dist[B]=∞, dist[C]=∞, dist[D]=∞
KROK 1: Zpracuj A → dist[B]=4, dist[C]=2
KROK 2: Zpracuj C (dist=2, nejmenší!) → dist[B]=3 (lepší!), dist[D]=5
KROK 3: Zpracuj B (dist=3) → D: 3+6=9 > 5 → neaktualizuj
KROK 4: Zpracuj D (dist=5) → konec

Výsledek: A=0, B=3 (A→C→B), C=2 (A→C), D=5 (A→C→D)
```

## Co je RELAXACE?

```
Relaxace hrany (u → v) s vahou w:
if (dist[u] + w < dist[v])
{
    dist[v] = dist[u] + w;    // Lepší vzdálenost!
    pred[v] = u;               // Nový předchůdce
}
```

## Implementace:

```csharp
static (int[] dist, int[] pred) Dijkstra(List<(int soused, int vaha)>[] graf, int start)
{
    int n = graf.Length;
    int[] dist = new int[n];
    int[] pred = new int[n];
    bool[] hotovo = new bool[n];

    // 1. Inicializace
    for (int i = 0; i < n; i++)
    {
        dist[i] = int.MaxValue;
        pred[i] = -1;
    }
    dist[start] = 0;

    // 2. Prioritní fronta (min-heap)
    var pq = new PriorityQueue<int, int>();
    pq.Enqueue(start, 0);

    // 3. Hlavní smyčka
    while (pq.Count > 0)
    {
        int u = pq.Dequeue();

        if (hotovo[u]) continue;  // Přeskoč zastaralé záznamy
        hotovo[u] = true;

        // 4. Relaxace sousedů
        foreach (var (v, w) in graf[u])
        {
            if (!hotovo[v] && dist[u] + w < dist[v])
            {
                dist[v] = dist[u] + w;
                pred[v] = u;
                pq.Enqueue(v, dist[v]);
            }
        }
    }

    return (dist, pred);
}
```

## Použití + rekonstrukce cesty:

```csharp
static void Main()
{
    var graf = new List<(int, int)>[4];
    graf[0] = new List<(int, int)> { (1, 4), (2, 2) };
    graf[1] = new List<(int, int)> { (0, 4), (2, 1), (3, 6) };
    graf[2] = new List<(int, int)> { (0, 2), (1, 1), (3, 3) };
    graf[3] = new List<(int, int)> { (1, 6), (2, 3) };

    var (dist, pred) = Dijkstra(graf, 0);

    // Rekonstrukce cesty k cíli (např. D = index 3)
    List<int> cesta = new List<int>();
    int cur = 3;
    while (cur != -1)
    {
        cesta.Add(cur);
        cur = pred[cur];
    }
    cesta.Reverse();
    // Výsledek: 0 → 2 → 3 (A → C → D), vzdálenost = 5
}
```

## Proč minimová halda?
- **Bez haldy:** O(V²) – musíš projít všechny vrcholy pro minimum
- **S haldou:** O((V + E) · log V) – vytáhneš minimum v O(log V)

## Zastaralé záznamy v haldě:
PriorityQueue v C# neumí aktualizovat prioritu → přidáme nový záznam, starý přeskočíme pomocí `hotovo[]`.

---

# BOD 7: OMEZENÍ DIJKSTRY

## Omezení 1: Záporné hrany
Dijkstra je **greedy** – když zpracuje vrchol, předpokládá finální vzdálenost. Záporné hrany tento předpoklad porušují.

## Omezení 2: Záporné cykly
Dijkstra je nedetekuje – zacyklí se nebo dá špatný výsledek.

## Co použít místo Dijkstry:

| Situace | Algoritmus | Složitost |
|---------|-----------|-----------|
| Neohodnocený graf | BFS | O(V + E) |
| Kladné váhy | Dijkstra | O((V+E) log V) |
| Záporné váhy (bez cyklů) | Bellman-Ford | O(V · E) |
| Všechny dvojice vrcholů | Floyd-Warshall | O(V³) |
| Záporné cykly (detekce) | Bellman-Ford | O(V · E) |

## Bellman-Ford (princip):

```
1. dist[start] = 0, ostatní = ∞
2. Opakuj (V-1) krát:
   - Pro KAŽDOU hranu (u, v, w): relaxuj
3. Ještě jeden průchod:
   - Pokud se cokoliv zlepší → ZÁPORNÝ CYKLUS!

Proč V-1 opakování? Nejkratší cesta má max V-1 hran.
```

## Shrnutí Dijkstry:

```
 Kladné váhy
 Orientovaný i neorientovaný graf
 Řídký i hustý graf
 Záporné hrany
 Záporné cykly
 All-pairs (→ Floyd-Warshall)
```

---

## Na co si dát pozor (Maturitní "chytáky")

1. **BFS vs Dijkstra** – BFS POUZE pro neohodnocený graf! Pro váhy musíš Dijkstru.
2. **DFS nenajde nejkratší cestu** – jen "nějakou" cestu. Tohle se rádi ptají!
3. **Rekonstrukce cesty** – nezapomeň na pole `predchudce[]` a `Reverse()`.
4. **Záporné hrany** – Dijkstra je NEUMÍ. Zmíň Bellman-Ford jako alternativu.
5. **PriorityQueue zastaralé záznamy** – v C# nelze aktualizovat prioritu, řeší se `hotovo[]` polem.
6. **int.MaxValue overflow** – při `dist[u] + w` kde dist[u]=MaxValue může přetéct! Kontroluj `hotovo[u]` nebo `dist[u] != int.MaxValue`.
7. **Graf jako seznam sousedů** – pro Dijkstru potřebuješ `List<(int soused, int vaha)>[]`, ne matici sousednosti.
8. **Typická otázka u ústní:** „Proč Dijkstra nefunguje se zápornými hranami?" → Greedy předpoklad o finalitě vzdálenosti.

---

## Senior Tip

- **A\* algoritmus** = Dijkstra + heuristika (odhad vzdálenosti k cíli). Používá se v herním pathfindingu – prohledává méně vrcholů, protože "ví" kterým směrem je cíl.
- **PriorityQueue<T, TPriority>** je v .NET 6+ – na maturitě zmíň, že starší verze C# to nemají a musíš si haldu napsat nebo použít SortedSet.
- **Generická verze** s `Dictionary<T, List<T>>` místo pole – funguje s jakýmkoliv typem vrcholů.

---

## Souvislosti s jinými otázkami

- **Otázka 3** (Fronta a zásobník) – BFS = fronta, DFS = zásobník, PriorityQueue pro Dijkstru
- **Otázka 8** (Reprezentace grafu) – seznam sousedů s vahami pro Dijkstru
- **Otázka 9** (Stromy) – halda (min-heap) = základ prioritní fronty
- **Otázka 13** (Heap sort) – minimová halda, stejný princip jako v Dijkstrovi
- **Otázka 21** (Teorie grafů) – definice grafu, cesty, cyklu
- **Otázka 22** (DFS/BFS) – BFS pro nejkratší cestu, DFS pro existenci cesty
- **Otázka 23** (Minimální kostra) – Jarník/Prim je podobný Dijkstrovi (greedy + halda)
- **Otázka 24** (Topologické třídění) – DFS v orientovaném grafu

---

## Relevantní maturitní úlohy

| Úloha | Soubor | Téma | Souvisí s body |
|-------|--------|------|----------------|
| 352 | 33-69 | Existuje autobusové spojení? | 4 (BFS) |
| 355 | 33-69 | Cesta bludištěm | 4 (rekonstrukce) |
| 356 | 33-69 | Letiště s nejméně přestupy | 3, 4 (BFS) |
| 384 | 33-69 | Šachový kůň | 3, 4 (BFS grid) |
| 385 | 33-69 | Bludiště ve čtverečkové síti | 3, 4 (BFS grid) |
