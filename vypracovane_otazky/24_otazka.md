# Zápisky: Otázka č. 24 – Topologické třídění a jeho využití
**Datum:** 2025-02-21
**Status:** Hotovo

---

## Checklist bodů otázky
- [x] Bod 1: Motivace
- [x] Bod 2: Pojmy – cyklus v grafu, DAG, detekce cyklů
- [x] Bod 3: Topologické uspořádání vrcholů (na obrázku)
- [x] Bod 4: Algoritmus topologického třídění
- [x] Bod 5: Proč tento algoritmus funguje
- [x] Bod 6: Využití DFS
- [x] Bod 7: Časová složitost
- [x] Bod 8: Využití topologického třídění

---

## Klíčové koncepty & Snippety

### Bod 1: Motivace

Topologické třídění řeší problém: **"V jakém pořadí dělat věci, které na sobě závisí?"**

Příklady:
- Prerekvizity předmětů na škole (Mat1 → Mat2 → Statistika)
- Build systém – pořadí kompilace souborů
- Instalace balíčků se závislostmi
- Oblékání (trenky → kalhoty → boty, ponožky → boty)

Topologické pořadí **nemusí být jednoznačné** – může existovat více platných pořadí.

---

### Bod 2: Pojmy

**Cyklus v orientovaném grafu:**
Cesta, která začíná a končí ve stejném vrcholu a sleduje směr šipek.

```
BEZ CYKLU:                    S CYKLEM:
  A → B → C                     A → B → C
      ↓                              ↓   ↑
      D                              D →──┘
```

Cyklus = problém pro topologické třídění (slepička a vajíčko – nelze určit, co je první).

**DAG – Directed Acyclic Graph:**
- **Directed** = orientovaný (hrany mají směr)
- **Acyclic** = acyklický (žádný cyklus)

**Topologické třídění existuje PRÁVĚ TEHDY, když je graf DAG.**

**Detekce cyklu pomocí DFS – tři barvy:**

```
BÍLÝ (0) = nenavštívený
ŠEDÝ (1) = právě zpracováváme (v rekurzi)
ČERNÝ (2) = hotovo, všichni potomci prozkoumáni

Pravidlo: Narazíš na ŠEDÝ vrchol → CYKLUS!
```

**Kód (Maturitní verze):**

```csharp
static bool MaCyklus(List<int>[] sousede, int pocetVrcholu)
{
    int[] stav = new int[pocetVrcholu]; // 0=bílý, 1=šedý, 2=černý

    for (int i = 0; i < pocetVrcholu; i++)
    {
        if (stav[i] == 0)
        {
            if (DFSCyklus(sousede, i, stav))
                return true;
        }
    }
    return false;
}

static bool DFSCyklus(List<int>[] sousede, int vrchol, int[] stav)
{
    stav[vrchol] = 1; // ŠEDÝ

    foreach (int soused in sousede[vrchol])
    {
        if (stav[soused] == 1) return true;  // CYKLUS!
        if (stav[soused] == 0)
        {
            if (DFSCyklus(sousede, soused, stav))
                return true;
        }
    }

    stav[vrchol] = 2; // ČERNÝ
    return false;
}
```

---

### Bod 3: Topologické uspořádání – na obrázku

Topologické uspořádání = seřazení vrcholů tak, že **pro každou hranu U → V platí, že U je PŘED V**.

```
DAG:                          Topologické pořadí:
    A → B → D                   A, C, E, B, D
    ↓       ↑
    C → E →─┘                 Ověření - všechny šipky ukazují DOPRAVA →
                              A→B   A→C   B→D   C→E   E→D
```

Jiné platné pořadí: A, B, C, E, D – taky OK!

---

### Bod 4: Algoritmus topologického třídění (DFS)

**Princip:** Udělej DFS. Když je vrchol HOTOVÝ (černý), přidej ho do výsledku. Na konci obrať výsledek.

**Algoritmus:**
1. Pro každý nenavštívený vrchol spusť DFS
2. V DFS: označ šedý, projdi sousedy
3. Když hotový (všichni sousedé prozkoumáni) → černý → přidej do výsledku
4. Na konci Reverse

```
DFS na grafu A→B→D, A→C→E→D:

  A=šedý → B=šedý → D=šedý → D=ČERNÝ [D]
           B=ČERNÝ [D,B]
           C=šedý → E=šedý → (D černý, přeskoč)
                    E=ČERNÝ [D,B,E]
           C=ČERNÝ [D,B,E,C]
  A=ČERNÝ [D,B,E,C,A]

  Reverse: A, C, E, B, D
```

**Kód (Maturitní verze):**

```csharp
static List<int> TopologickeTridi(List<int>[] sousede, int pocetVrcholu)
{
    int[] stav = new int[pocetVrcholu];
    List<int> vysledek = new List<int>();
    bool maCyklus = false;

    for (int i = 0; i < pocetVrcholu; i++)
    {
        if (stav[i] == 0)
            DFS(sousede, i, stav, vysledek, ref maCyklus);
    }

    if (maCyklus)
    {
        Console.WriteLine("Graf obsahuje cyklus! Nelze topologicky setřídit.");
        return null;
    }

    vysledek.Reverse();
    return vysledek;
}

static void DFS(List<int>[] sousede, int vrchol, int[] stav,
                List<int> vysledek, ref bool maCyklus)
{
    stav[vrchol] = 1; // ŠEDÝ

    foreach (int soused in sousede[vrchol])
    {
        if (stav[soused] == 1)
        {
            maCyklus = true;
            return;
        }
        if (stav[soused] == 0)
            DFS(sousede, soused, stav, vysledek, ref maCyklus);
    }

    stav[vrchol] = 2; // ČERNÝ
    vysledek.Add(vrchol);
}
```

**Použití:**

```csharp
int V = 5;
List<int>[] sousede = new List<int>[V];
for (int i = 0; i < V; i++)
    sousede[i] = new List<int>();

sousede[0].Add(1); // A→B
sousede[0].Add(2); // A→C
sousede[1].Add(3); // B→D
sousede[2].Add(4); // C→E
sousede[4].Add(3); // E→D

List<int> poradi = TopologickeTridi(sousede, V);
// Výstup: 0, 2, 4, 1, 3  (= A, C, E, B, D)
```

---

### Bod 5: Proč algoritmus funguje

Vrchol se stane ČERNÝM teprve, když **všichni jeho potomci jsou už černí** (= zpracovaní).

Proto po Reverse platí: pokud A → B, tak A je ve výsledku PŘED B.

```
Důkaz:
  - DFS z A nejdřív prozkoumá B (a vše za B)
  - B se stane černým DŘÍVE než A
  - B se přidá do seznamu DŘÍVE než A
  - Po Reverse: A je PŘED B
```

---

### Bod 6: Využití DFS

DFS dělá v topologickém třídění dvě věci najednou:
1. **Určení pořadí** – přirozeně prochází do hloubky, listové uzly skončí první
2. **Detekce cyklu** – tři barvy (šedý → šedý = cyklus)

= Jeden průchod, všechno vyřešeno za O(V + E).

---

### Bod 7: Časová složitost

```
Časová složitost:   O(V + E)
  - Každý vrchol navštívíme 1× (bílý → šedý → černý)
  - Každou hranu projdeme 1×
  - Reverse: O(V)

Paměťová složitost: O(V + E)
  - Pole stav[]: O(V)
  - Výsledek: O(V)
  - Graf: O(V + E)
  - Rekurzivní zásobník: O(V) nejhorší případ
```

---

### Bod 8: Využití topologického třídění

- **Plánování úloh / projektů** – "Co musím udělat dřív?"
- **Prerekvizity předmětů** – "V jakém pořadí zapsat?"
- **Build systémy (make, gradle, npm)** – pořadí kompilace
- **Instalace balíčků se závislostmi** – pip, npm
- **Vyhodnocení vzorců v tabulce (Excel)** – A1=B1+C1, B1=D1*2 → pořadí: D1, B1, C1, A1
- **Detekce cyklických závislostí** – A závisí na B, B závisí na A = CHYBA

---

## Senior verze (Nice to Have)

**Kahnův algoritmus (BFS přístup):**

```csharp
static List<int> KahnTopSort(List<int>[] sousede, int V)
{
    int[] vstupniStupen = new int[V];
    foreach (var seznam in sousede)
        foreach (int s in seznam)
            vstupniStupen[s]++;

    Queue<int> fronta = new Queue<int>();
    for (int i = 0; i < V; i++)
        if (vstupniStupen[i] == 0)
            fronta.Enqueue(i); // vrcholy bez závislostí

    List<int> vysledek = new List<int>();
    while (fronta.Count > 0)
    {
        int v = fronta.Dequeue();
        vysledek.Add(v);
        foreach (int soused in sousede[v])
        {
            vstupniStupen[soused]--;
            if (vstupniStupen[soused] == 0)
                fronta.Enqueue(soused);
        }
    }

    if (vysledek.Count != V) return null; // cyklus!
    return vysledek;
}
```

Výhoda: intuitivní ("odebírej vrcholy bez závislostí"), detekce cyklu zdarma.

---

## Na co si dát pozor (Maturitní "chytáky")

- **Topologické třídění POUZE pro DAG** – orientovaný + acyklický. Neorientovaný graf nelze topologicky třídit.
- **Pořadí nemusí být jednoznačné** – může existovat více platných pořadí
- **Nezapomeň na Reverse** – bez otočení máš opačné pořadí!
- **Tři barvy pro detekci cyklu** – nestačí jen "navštívený/nenavštívený", potřebuješ rozlišit šedý (v procesu) a černý (hotový)
- **`ref maCyklus`** – v C# musíš předat bool referencí, aby se změna propagovala z rekurze
- **Na tabuli:** kresli graf, piš barvy vrcholů, ukazuj jak se plní výsledkový seznam

---

## Senior Tip

Kahnův algoritmus (BFS přístup) je v praxi často preferovaný, protože je snáze paralelizovatelný – všechny vrcholy se vstupním stupněm 0 můžeš zpracovat najednou. Používá se v build systémech a CI/CD pipeline.

---

## Souvislosti s jinými otázkami

- **Otázka 3** (Fronta a zásobník) – fronta v Kahnově algoritmu, zásobník v DFS přístupu
- **Otázka 5** (Rekurze) – DFS je rekurzivní, hrozí StackOverflow na hlubokých grafech
- **Otázka 8** (Reprezentace grafu) – seznamy sousedů pro orientovaný graf
- **Otázka 21** (Teorie grafů) – orientovaný graf, cyklus, DAG
- **Otázka 22** (DFS/BFS) – topologické třídění je nadstavba DFS
- **Otázka 23** (Minimální kostra) – oba jsou grafové algoritmy, ale MST je pro neohodnocené/ohodnocené neorientované grafy
