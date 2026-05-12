# Zápisky: Otázka č. 2 - Spojové datové struktury. Pole.

**Datum:** 2026-01-05
**Status:** Hotovo

---

## Checklist bodů otázky

- [x] **Bod 1:** Spojový seznam - základy (co to je, paměť, typy)
- [x] **Bod 2:** Spojový seznam - operace (přidání, odebrání, průnik, sjednocení, max + časová složitost)
- [x] **Bod 3:** Pole - základy (co to je, předávání, kopírování, výpis)
- [x] **Bod 4:** Srovnání (výhody a nevýhody spojového seznamu vs pole)

---

## Klíčové koncepty & Implementace

---

## BOD 1: Spojový seznam - základy

### Co to je a k čemu to slouží?

**Spojový seznam** (linked list) je **dynamická datová struktura**, která se skládá z tzv. **uzlů** (nodes). Každý uzel obsahuje dvě hlavní části:

1. **Data** - samotná hodnota, kterou chceme uložit (např. číslo, text)
2. **Odkaz/ukazatel** (pointer/reference) - adresa v paměti, kde se nachází další uzel v seznamu

Představ si to jako **řetěz**, kde každé "oko" řetězu ukazuje na další oko. Nebo jako **lov pokladu**, kde na každém místě najdeš nápovědu, kam jít dál.

**Hlavní výhoda:** Nemusíš dopředu vědět, kolik prvků budeš potřebovat. Seznam se může dynamicky zvětšovat nebo zmenšovat podle potřeby.

**Hlavní nevýhoda:** Abys se dostal k prvku číslo 100, musíš projít prvních 99 uzlů jeden po druhém. Nemůžeš "skočit" přímo na daný prvek.

---

### Jak je spojový seznam uložen v paměti?

Na rozdíl od pole, kde jsou všechna data uložena **vedle sebe** v jednom souvislém bloku paměti, spojový seznam má své uzly **roztroušené** po různých místech v paměti. Každý uzel "ví", kde najít další uzel díky odkazu.

```
┌─────────────────────────────────────────────────┐
│  POLE (Array):                                  │
│  ┌───┬───┬───┬───┬───┐                         │
│  │ 5 │ 3 │ 8 │ 1 │ 7 │  ← Souvislý blok paměti │
│  └───┴───┴───┴───┴───┘                         │
│  Adresa: 1000, 1004, 1008, 1012, 1016          │
│                                                 │
│  • Všechna data vedle sebe                      │
│  • Rychlý přístup: array[3] = adresa 1012      │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│  SPOJOVÝ SEZNAM (Linked List):                 │
│                                                 │
│  ┌──────┬────┐     ┌──────┬────┐              │
│  │ Data │Next│ ──> │ Data │Next│ ──> ...      │
│  │  5   │ •──┼─┐   │  3   │ •──┼─┐            │
│  └──────┴────┘ │   └──────┴────┘ │            │
│  Adresa: 2000  │   Adresa: 3500  │            │
│                │                  │            │
│  Další uzel:   │   Další uzel:   │            │
│  Adresa: 3500 ←┘   Adresa: 7200 ←┘            │
│                                                 │
│  • Data roztroušená v paměti                    │
│  • Musíš následovat odkazy (Next)               │
│  • Žádný přímý přístup                          │
└─────────────────────────────────────────────────┘
```

**Proč je to důležité?**
- **Pole:** CPU může rychle načíst data, protože jsou blízko u sebe (cache-friendly)
- **Spojový seznam:** CPU musí "skákat" po paměti, což je pomalejší

---

### Typy spojových seznamů

#### 1⃣ Jednosměrný spojový seznam (Singly Linked List)

Každý uzel má pouze **jeden odkaz** - ukazuje na **následující** uzel. Můžeš procházet jen jedním směrem - od začátku ke konci.

```
HEAD → [5|•]──>[3|•]──>[8|•]──>[1|NULL]
        ↑                        ↑
     Začátek                   Konec
     (první)                 (poslední, Next = NULL)

• HEAD = referenční proměnná ukazující na první uzel
• NULL = konec seznamu (žádný další uzel)
```

**Vlastnosti:**
- Menší paměťová náročnost (jen jeden odkaz na uzel)
- Přidání na začátek: O(1) - rychlé
- Přidání na konec: O(n) - musíš projít celý seznam
- Nelze se vracet zpět (jen dopředu)

---

#### 2⃣ Obousměrný spojový seznam (Doubly Linked List)

Každý uzel má **dva odkazy** - ukazuje na **předchozí** i **následující** uzel. Můžeš se pohybovat oběma směry.

```
NULL←─[•|5|•]<──>[•|3|•]<──>[•|8|•]──>NULL
       ↑    ↑       ↑    ↑       ↑    ↑
     Prev Next    Prev Next    Prev Next

• Každý uzel zná svého souseda zprava i zleva
• Lze procházet oběma směry
```

**Vlastnosti:**
- Větší paměťová náročnost (dva odkazy na uzel = 2× více paměti)
- Přidání na začátek: O(1)
- Přidání na konec: O(1) - pokud máš odkaz na poslední uzel (tail)
- Můžeš se vracet zpět

**V C# je `LinkedList<T>` obousměrný!** To znamená, že má optimalizace jako rychlé přidání na konec.

---

#### 3⃣ Kruhový spojový seznam (Circular Linked List)

Poslední uzel neukazuje na NULL, ale zpět na **první uzel**. Seznam tvoří "kruh".

```
     ┌──────────────────────┐
     ↓                      │
  [5|•]──>[3|•]──>[8|•]─────┘

• Žádný konec (NULL)
• Lze procházet donekonečna
```

**Použití:**
- Round-robin scheduling (plánování úloh)
- Kruhové buffery
- Playlisty s opakováním

---

### Implementace v C#

#### Verze A: Maturitní (vlastní implementace)

Vytvoříme si vlastní třídu `Node`, která reprezentuje jeden uzel seznamu.

```csharp
// Třída reprezentující jeden uzel v seznamu
public class Node
{
    public int Data;       // Hodnota uložená v uzlu
    public Node Next;      // Odkaz na další uzel (nebo null)

    // Konstruktor - vytvoří nový uzel s danou hodnotou
    public Node(int data)
    {
        Data = data;
        Next = null;  // Zatím neukazuje nikam
    }
}
```

**Jak to funguje krok po kroku:**

1. **Vytvoření prvního uzlu:**
```csharp
Node head = new Node(5);
// head ukazuje na uzel: [5|null]
// head.Data = 5
// head.Next = null (zatím žádný další uzel)
```

2. **Přidání druhého uzlu:**
```csharp
head.Next = new Node(3);
// head.Next nyní ukazuje na: [3|null]
// Seznam: [5|•]──>[3|null]
```

3. **Přidání třetího uzlu:**
```csharp
head.Next.Next = new Node(8);
// head.Next.Next ukazuje na: [8|null]
// Seznam: [5|•]──>[3|•]──>[8|null]
```

4. **Procházení a výpis seznamu:**

Použijeme pomocnou proměnnou `current`, která postupně "skáče" z uzlu na uzel:

```csharp
Node current = head;  // Začni od začátku

while (current != null)  // Dokud nejsi na konci
{
    Console.Write(current.Data + " -> ");  // Vypiš hodnotu
    current = current.Next;  // Přeskoč na další uzel
}
Console.WriteLine("NULL");  // Konec seznamu

// Výstup: 5 -> 3 -> 8 -> NULL
```

**Jak while cyklus funguje:**
```
Krok 1: current = [5|•]     → Vypíše "5 -> ", current = current.Next
Krok 2: current = [3|•]     → Vypíše "3 -> ", current = current.Next
Krok 3: current = [8|null]  → Vypíše "8 -> ", current = current.Next
Krok 4: current = null      → Konec cyklu, vypíše "NULL"
```

---

#### Verze B: Senior (LinkedList<T> v C#)

V praxi nemusíš implementovat vlastní uzly. C# má vestavěnou třídu `LinkedList<T>`, která je:
- **Obousměrná** - každý uzel zná předchozí i následující
- **Typově bezpečná** - určíš typ dat při vytvoření
- **Optimalizovaná** - má odkaz na první i poslední prvek (rychlé operace)

```csharp
using System.Collections.Generic;

// Vytvoření prázdného obousměrného seznamu
LinkedList<int> list = new LinkedList<int>();

// Přidání prvků na konec - každé je O(1)!
list.AddLast(5);   // list: 5
list.AddLast(3);   // list: 5 <-> 3
list.AddLast(8);   // list: 5 <-> 3 <-> 8

// Výpis pomocí foreach
foreach (int value in list)
{
    Console.Write(value + " -> ");
}
// Výstup: 5 -> 3 -> 8 ->
```

**Proč je to lepší v praxi:**

1. **Méně chyb** - nemusíš řešit odkazy ručně
2. **Rychlejší vývoj** - hotové metody pro všechny operace
3. **Lepší výkon** - interní optimalizace
4. **Type-safe** - kompilátor hlídá typy

```csharp
//  LinkedList<int> - pouze čísla
LinkedList<int> cisla = new LinkedList<int>();
cisla.AddLast(5);
// cisla.AddLast("text");  //  Chyba při kompilaci!

// vs

//  Vlastní Node bez generik
Node node = new Node(5);
node.Next = new Node("text");  //  Projde, ale chyba za běhu!
```

---

### ⏱ Časová složitost základních operací

| Operace | Jednosměrný | Obousměrný (LinkedList<T>) | Vysvětlení |
|---------|-------------|---------------------------|------------|
| **Přístup k i-tému prvku** | O(n) | O(n) | Musíš projít všechny předchozí uzly |
| **Přidání na začátek** | O(1) | O(1) | Jen změníš head odkaz |
| **Přidání na konec** | O(n) | O(1) | Jednosměrný musí projít, obousměrný má tail |
| **Hledání prvku** | O(n) | O(n) | Musíš projít od začátku |

**Důležité pro maturitu:**
- Spojový seznam **nemá** přímý přístup jako pole!
- `list[5]` neexistuje u LinkedList<T> - nelze přistoupit přes index
- Vždy musíš procházet od začátku nebo konce

---

## BOD 2: Spojový seznam - operace

### Operace: Přidání prvku

#### 1. Přidání na začátek (AddFirst)

Toto je **nejrychlejší operace** u spojového seznamu - O(1).

**Postup:**
1. Vytvoř nový uzel
2. Nech ho ukazovat na současný první uzel
3. Nastav nový uzel jako hlavu (head)

**Vizualizace:**

```
PŘED:
HEAD → [5|•]──>[3|•]──>[8|NULL]

Chci přidat 9 na začátek:

Krok 1: Vytvoř nový uzel
        [9|null]

Krok 2: Nech ho ukazovat na současný head
        [9|•]──>[5|•]──>[3|•]──>[8|NULL]
                 ↑
              starý head

Krok 3: Nový uzel je teď head
HEAD → [9|•]──>[5|•]──>[3|•]──>[8|NULL]
```

**Kód - Maturitní verze:**

```csharp
// Funkce pro přidání prvku na začátek
public void AddFirst(int value)
{
    // 1. Vytvoř nový uzel s danou hodnotou
    Node newNode = new Node(value);

    // 2. Nový uzel ukazuje na současnou hlavu
    //    (i když je head null, funguje to - prázdný seznam)
    newNode.Next = head;

    // 3. Nový uzel se stává hlavou
    head = newNode;
}

// Příklad použití:
Node head = null;  // Prázdný seznam
AddFirst(5);       // Seznam: 5
AddFirst(3);       // Seznam: 3 -> 5
AddFirst(9);       // Seznam: 9 -> 3 -> 5
```

**Časová složitost:** **O(1)** - konstantní čas, nezáleží na velikosti seznamu. Měníš jen 2 odkazy.

---

**Kód - Senior verze:**

```csharp
LinkedList<int> list = new LinkedList<int>();

list.AddLast(5);   // list: 5
list.AddLast(3);   // list: 5 <-> 3
list.AddLast(8);   // list: 5 <-> 3 <-> 8

list.AddFirst(9);  // list: 9 <-> 5 <-> 3 <-> 8

// Interně dělá přesně to samé, ale už je to napsané
// Časová složitost: O(1)
```

---

#### 2. Přidání na konec (AddLast)

**Jednosměrný seznam:** Musíš projít celý seznam, abys našel poslední uzel → **O(n)**

**Obousměrný seznam (LinkedList<T>):** Má přímý odkaz na poslední prvek → **O(1)**

**Vizualizace pro jednosměrný:**

```
PŘED:
HEAD → [5|•]──>[3|•]──>[8|NULL]

Chci přidat 7 na konec:

Krok 1: Projdi celý seznam, dokud nenajdeš NULL
        current = head
        while (current.Next != null):
            current = current.Next

        HEAD → [5|•]──>[3|•]──>[8|NULL]
                                ↑
                             current

Krok 2: Vytvoř nový uzel
        [7|NULL]

Krok 3: Poslední uzel ukazuje na nový
        HEAD → [5|•]──>[3|•]──>[8|•]──>[7|NULL]
```

**Kód - Maturitní (jednosměrný):**

```csharp
public void AddLast(int value)
{
    // 1. Vytvoř nový uzel
    Node newNode = new Node(value);

    // 2. SPECIÁLNÍ PŘÍPAD: Seznam je prázdný
    if (head == null)
    {
        head = newNode;  // Nový uzel je zároveň první i poslední
        return;
    }

    // 3. Projdi seznam až na konec
    Node current = head;
    while (current.Next != null)  // Dokud není poslední
    {
        current = current.Next;  // Přeskoč na další
    }
    // Teď je current.Next == null, jsme na konci

    // 4. Poslední uzel ukazuje na nový
    current.Next = newNode;
}

// Časová složitost: O(n)
// - Musíme projít n uzlů
```

**Kód - Senior (LinkedList<T>):**

```csharp
list.AddLast(7);  // Přidá 7 na konec
// list = 5 <-> 3 <-> 8 <-> 7

// Časová složitost: O(1)
// - LinkedList<T> má interní referenci "tail" na poslední uzel
// - Žádné procházení není potřeba
```

---

#### 3. Přidání po konkrétním uzlu (AddAfter)

Pokud máš **referenci na konkrétní uzel**, můžeš za něj vložit nový prvek v O(1).

**Vizualizace:**

```
PŘED:
HEAD → [5|•]──>[3|•]──>[8|NULL]
                ↑
             Tento uzel

Chci přidat 99 ZA uzel s hodnotou 3:

Krok 1: Najdi uzel s hodnotou 3 - O(n)
        node = list.Find(3);

Krok 2: Vytvoř nový uzel [99|•]

Krok 3: Nový ukazuje tam, kam ukazoval nalezený
        [99|•]──>[8|NULL]

Krok 4: Nalezený ukazuje na nový
HEAD → [5|•]──>[3|•]──>[99|•]──>[8|NULL]
```

**Kód - Senior:**

```csharp
LinkedList<int> list = new LinkedList<int>();
list.AddLast(5);
list.AddLast(3);
list.AddLast(8);

// 1. Najdi uzel s hodnotou 3
LinkedListNode<int> node = list.Find(3);

// 2. Pokud existuje, přidej za něj 99
if (node != null)
{
    list.AddAfter(node, 99);
}
// list = 5 <-> 3 <-> 99 <-> 8

// Časová složitost:
// - Find(3): O(n) - musíme najít prvek
// - AddAfter: O(1) - jen změna odkazů
// Celkem: O(n)
```

**Poznámka:** `LinkedListNode<int>` je wrapper třída, která reprezentuje uzel v LinkedList<T>. Má vlastnosti:
- `Value` - hodnota uzlu
- `Next` - další uzel
- `Previous` - předchozí uzel
- `List` - odkaz na seznam, do kterého patří

---

### Operace: Odebrání prvku

#### 1. Odebrání ze začátku (RemoveFirst)

Nejjednodušší operace - jen změníme head.

**Vizualizace:**

```
PŘED:
HEAD → [9|•]──>[5|•]──>[3|•]──>[8|NULL]

Chci odebrat první (9):

Krok 1: Přesuň head na druhý uzel

        HEAD ─────────>[5|•]──>[3|•]──>[8|NULL]

        [9|•] (nyní nedostupný, garbage collector ho smaže)

PO:
HEAD → [5|•]──>[3|•]──>[8|NULL]
```

**Kód - Senior:**

```csharp
LinkedList<int> list = new LinkedList<int>();
list.AddLast(9);
list.AddLast(5);
list.AddLast(3);
// list: 9 <-> 5 <-> 3

list.RemoveFirst();  // Odebere 9
// list: 5 <-> 3

// Časová složitost: O(1)
```

---

#### 2. Odebrání z konce (RemoveLast)

**Jednosměrný:** Musíš najít předposlední uzel → O(n)
**Obousměrný (LinkedList<T>):** Přímý přístup → O(1)

**Kód - Senior:**

```csharp
list.RemoveLast();  // Odebere poslední prvek
// Časová složitost: O(1)
```

---

#### 3. Odebrání konkrétní hodnoty (Remove)

Najde **první výskyt** dané hodnoty a odebere ho.

**Vizualizace:**

```
PŘED:
HEAD → [5|•]──>[3|•]──>[8|•]──>[3|NULL]

Chci odebrat hodnotu 3:

Krok 1: Najdi první uzel s hodnotou 3
        HEAD → [5|•]──>[3|•]──>[8|•]──>[3|NULL]
                        ↑
                     Tento

Krok 2: Předchozí uzel přesměruj na následující
        HEAD → [5|•]─────────>[8|•]──>[3|NULL]

               [3|•] (nedostupný, bude smazán)

PO:
HEAD → [5|•]──>[8|•]──>[3|NULL]
```

**Kód - Senior:**

```csharp
list.Remove(3);  // Odebere první výskyt hodnoty 3
// Časová složitost: O(n)
// - Musíme projít seznam a najít prvek
```

---

### Operace: Hledání maxima

Musíme projít celý seznam a sledovat největší nalezenou hodnotu.

**Algoritmus krok po kroku:**

```
Seznam: 4 <-> 8 <-> 3 <-> 8 <-> 6

Krok 1: max = první prvek = 4
        current = 4

Krok 2: current = 8
        8 > 4? Ano → max = 8

Krok 3: current = 3
        3 > 8? Ne → max zůstává 8

Krok 4: current = 8
        8 > 8? Ne → max zůstává 8

Krok 5: current = 6
        6 > 8? Ne → max zůstává 8

Výsledek: max = 8
```

**Kód - Maturitní:**

```csharp
public int FindMax(LinkedList<int> list)
{
    // 1. Kontrola prázdného seznamu
    if (list.Count == 0)
        throw new Exception("Seznam je prázdný!");

    // 2. Začni s nejmenší možnou hodnotou
    int max = int.MinValue;  // -2,147,483,648

    // 3. Projdi všechny prvky
    foreach (int value in list)
    {
        // Pokud je aktuální hodnota větší než dosavadní max
        if (value > max)
        {
            max = value;  // Aktualizuj max
        }
    }

    // 4. Vrať nalezené maximum
    return max;
}

// Časová složitost: O(n)
// - Musíme zkontrolovat každý prvek
```

**Kód - Senior (s LINQ):**

```csharp
using System.Linq;

public int FindMax(LinkedList<int> list)
{
    return list.Max();  // LINQ metoda - kratší, stejně O(n)
}

// Interně dělá přesně to samé jako naše for cyklus
```

---

### Operace: Průnik seznamů

**Průnik** = prvky, které jsou **v obou seznamech**.

**Matematicky:** A ∩ B = {x | x ∈ A ∧ x ∈ B}

**Příklad:**

```
Seznam1: 1 <-> 3 <-> 5 <-> 6 <-> 8
Seznam2: 2 <-> 3 <-> 4 <-> 5 <-> 6

Průnik:  3 <-> 5 <-> 6
         (prvky, které jsou v obou)
```

**Algoritmus krok po kroku:**

```
1. Vytvoř prázdný výsledný seznam
   result = []

2. Pro každý prvek v list1:
   - Je v list2? Pokud ano:
     - Je už v result? Pokud ne:
       - Přidej ho do result

Projdeme list1:
  1 → není v list2 → přeskočit
  3 → je v list2 → přidat do result
  5 → je v list2 → přidat do result
  6 → je v list2 → přidat do result
  8 → není v list2 → přeskočit

Výsledek: [3, 5, 6]
```

**Kód - Maturitní:**

```csharp
public LinkedList<int> Prunik(LinkedList<int> list1, LinkedList<int> list2)
{
    // 1. Vytvoř prázdný výsledný seznam
    LinkedList<int> result = new LinkedList<int>();

    // 2. Pro každý prvek v list1
    foreach (int value in list1)
    {
        // 3. Zkontroluj, jestli je i v list2
        //    Contains prochází celý list2 → O(m)
        if (list2.Contains(value))
        {
            // 4. Přidej ho do výsledku (pouze pokud tam ještě není)
            //    Tím zajistíme, že máme pouze unikátní hodnoty
            if (!result.Contains(value))
            {
                result.AddLast(value);
            }
        }
    }

    // 5. Vrať výsledný seznam
    return result;
}

// Časová složitost: O(n × m)
// - Pro každý prvek v list1 (n prvků)
//   kontrolujeme celý list2 (m prvků)
// - n × m může být hodně pro velké seznamy!
```

**Kód - Senior (optimalizovaný s LINQ):**

```csharp
using System.Linq;

public LinkedList<int> Prunik(LinkedList<int> list1, LinkedList<int> list2)
{
    // LINQ metoda Intersect je optimalizovaná
    // Interně používá HashSet pro O(1) vyhledávání
    var result = list1.Intersect(list2);
    return new LinkedList<int>(result);
}

// Časová složitost: O(n + m)
// - Mnohem rychlejší pro velké seznamy!
// - Intersect vytvoří HashSet z list2 (O(m))
// - Pak prochází list1 a hledá v HashSetu (O(n))
```

**Proč je LINQ rychlejší:**

```
Naivní přístup (O(n×m)):
list1 má 1000 prvků, list2 má 1000 prvků
→ 1000 × 1000 = 1,000,000 operací

LINQ s HashSet (O(n+m)):
→ 1000 + 1000 = 2,000 operací
→ 500× rychlejší!
```

---

### Operace: Sjednocení seznamů

**Sjednocení** = všechny **unikátní prvky** z obou seznamů.

**Matematicky:** A ∪ B = {x | x ∈ A ∨ x ∈ B}

**Příklad:**

```
Seznam1: 1 <-> 3 <-> 5 <-> 6 <-> 8
Seznam2: 2 <-> 3 <-> 4 <-> 5 <-> 6

Sjednocení: 1 <-> 2 <-> 3 <-> 4 <-> 5 <-> 6 <-> 8
            (všechny prvky, každý maximálně jednou)
```

**Algoritmus krok po kroku:**

```
1. Vytvoř prázdný výsledný seznam
   result = []

2. Přidej všechny unikátní prvky z list1
   Pro každý prvek v list1:
     Pokud není v result:
       Přidej ho

3. Přidej všechny unikátní prvky z list2
   Pro každý prvek v list2:
     Pokud není v result:
       Přidej ho
```

**Kód - Maturitní:**

```csharp
public LinkedList<int> Sjednoceni(LinkedList<int> list1, LinkedList<int> list2)
{
    // 1. Vytvoř prázdný výsledný seznam
    LinkedList<int> result = new LinkedList<int>();

    // 2. Přidej všechny prvky z list1 (bez duplicit)
    foreach (int value in list1)
    {
        // Contains prochází celý result → O(n)
        if (!result.Contains(value))
        {
            result.AddLast(value);
        }
    }

    // 3. Přidej prvky z list2, které tam ještě nejsou
    foreach (int value in list2)
    {
        // Opět Contains → O(n)
        if (!result.Contains(value))
        {
            result.AddLast(value);
        }
    }

    // 4. Vrať výsledný seznam
    return result;
}

// Časová složitost: O(n² + m²)
// - Pro každý prvek voláme Contains, což je O(velikost result)
// - Result roste, takže průměrně O(n²)
```

**Kód - Senior (s LINQ):**

```csharp
using System.Linq;

public LinkedList<int> Sjednoceni(LinkedList<int> list1, LinkedList<int> list2)
{
    // Union automaticky odstraní duplicity
    var result = list1.Union(list2);
    return new LinkedList<int>(result);
}

// Časová složitost: O(n + m)
// - Interně používá HashSet
```

---

### Shrnutí časových složitostí operací

| Operace | LinkedList<T> (C#) | Vlastní jednosměrný | Vysvětlení |
|---------|-------------------|-------------------|------------|
| **AddFirst** | O(1) | O(1) | Jen změna head odkazu |
| **AddLast** | O(1) | O(n) | LinkedList má tail, jednosměrný musí projít |
| **AddAfter** | O(1) | O(1) | Pokud máš referenci na uzel |
| **RemoveFirst** | O(1) | O(1) | Jen změna head odkazu |
| **RemoveLast** | O(1) | O(n) | LinkedList má tail, jednosměrný musí najít předposlední |
| **Remove(value)** | O(n) | O(n) | Musíš projít a najít hodnotu |
| **Contains/Find** | O(n) | O(n) | Musíš projít všechny prvky |
| **Hledání maxima** | O(n) | O(n) | Musíš zkontrolovat všechny |
| **Průnik (naivně)** | O(n×m) | O(n×m) | Vnořené Contains |
| **Průnik (LINQ)** | O(n+m) | - | HashSet optimalizace |
| **Sjednocení (naivně)** | O(n²+m²) | O(n²+m²) | Rostoucí result + Contains |
| **Sjednocení (LINQ)** | O(n+m) | - | HashSet optimalizace |

**Poznámka k paměti:**
- Jednosměrný: 4 byty data + 8 bytů Next = 12 bytů/uzel
- Obousměrný: 4 byty data + 8 bytů Next + 8 bytů Prev = 20 bytů/uzel

---

## BOD 3: Pole (Arrays) - základy

### Co to je a k čemu to slouží?

**Pole** (array) je **statická datová struktura** s **pevnou velikostí**, kde jsou všechny prvky uloženy **vedle sebe** v jednom souvislém bloku paměti.

**Hlavní vlastnosti:**
- **Indexované** - každý prvek má své číslo (index) od 0
- **Rychlý přístup** - můžeš "skočit" přímo na prvek `array[42]` v konstantním čase O(1)
- **Fixní velikost** - po vytvoření nelze měnit velikost
- **Homogenní** - všechny prvky mají stejný typ

**Analogie:** Pole je jako řada **poštovních schránek** na ulici. Každá má své číslo a můžeš rovnou otevřít schránku číslo 5, aniž bys musel projít 1, 2, 3, 4.

---

### Jak je pole uloženo v paměti?

Pole je uloženo jako **jeden souvislý blok** v paměti. To znamená, že všechna data jsou "vedle sebe" a počítač může rychle vypočítat, kde se nachází jakýkoliv prvek.

```
┌──────────────────────────────────────────┐
│  POLE (Array):                           │
│                                          │
│  Index:    [0]  [1]  [2]  [3]  [4]      │
│  Data:     │ 5 │ 3 │ 8 │ 1 │ 7 │        │
│  Adresa:   1000 1004 1008 1012 1016     │
│            ↑    ↑                        │
│         začátek  +4 byty (int má 4 B)   │
│                                          │
│   Souvislý blok paměti                 │
│   Přímý výpočet adresy                 │
│                                          │
│  Výpočet adresy prvku:                   │
│  adresa[i] = začátek + (i × velikost)    │
│  adresa[2] = 1000 + (2 × 4) = 1008      │
└──────────────────────────────────────────┘
```

**Proč je to rychlé:**
- CPU nemusí "skákat" po paměti
- Jeden výpočet: `adresa = base + index × size`
- Cache procesoru může načíst více prvků najednou

**Srovnání s LinkedList:**

```
POLE:
  CPU: "Chci prvek [100]"
  → Vypočítám: adresa = 1000 + 100×4 = 1400
  → Přečtu z adresy 1400
  → Hotovo (1 operace)

LINKED LIST:
  CPU: "Chci prvek [100]"
  → Projdi od začátku: 1 → 2 → 3 → ... → 100
  → Hotovo (100 operací)
```

---

### Vytvoření a inicializace pole

#### Základní způsoby v C#:

```csharp
//  ZPŮSOB 1: Deklarace s velikostí
// Vytvoří pole pro 5 čísel, všechny inicializované na 0
int[] cisla = new int[5];

// Co je v paměti: [0, 0, 0, 0, 0]

// Přiřazení hodnot jednotlivě:
cisla[0] = 10;  // index 0
cisla[1] = 20;  // index 1
cisla[2] = 30;
cisla[3] = 40;
cisla[4] = 50;

// Nyní: [10, 20, 30, 40, 50]
```

```csharp
//  ZPŮSOB 2: Inicializace přímo s hodnotami
int[] cisla = { 10, 20, 30, 40, 50 };

// Kompilátor automaticky spočítá velikost (5)
// Kratší a čitelnější zápis
```

```csharp
//  ZPŮSOB 3: Explicitní s new
int[] cisla = new int[] { 10, 20, 30, 40, 50 };

// Stejné jako způsob 2, jen delší
```

```csharp
//  ZPŮSOB 4: Collection expression (C# 12+)
int[] cisla = [10, 20, 30, 40, 50];

// Nejmodernější a nejkratší syntaxe
```

```csharp
//  ZPŮSOB 5: Typové odvození
var cisla = new[] { 10, 20, 30, 40, 50 };

// Kompilátor odvodí typ z hodnot
```

**Vícerozměrné pole:**

```csharp
// Dvourozměrné pole (matice 3×3)
int[,] matice = new int[3, 3];
matice[0, 0] = 1;
matice[0, 1] = 2;
matice[0, 2] = 3;

// Nebo přímo:
int[,] matice2 = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Vizualizace:
//   [0,0] [0,1] [0,2]
//     1     2     3
//   [1,0] [1,1] [1,2]
//     4     5     6
//   [2,0] [2,1] [2,2]
//     7     8     9
```

---

### Předávání pole v parametru funkce

** KRITICKY DŮLEŽITÉ:** V C# se pole předává **referencí**, nikoliv hodnotou! To znamená, že funkce pracuje s **originálním polem**, ne s kopií.

**Co to znamená v praxi:**
- Změny uvnitř funkce **ovlivní původní pole**
- Nealokuje se nová paměť
- Efektivní pro velká pole

**Příklad:**

```csharp
// Funkce, která mění pole
static void ZmenPrvniPrvek(int[] pole)
{
    pole[0] = 999;  //  Změní původní pole!
}

static void Main()
{
    // Vytvoř pole
    int[] cisla = { 1, 2, 3, 4, 5 };

    Console.WriteLine("PŘED voláním:");
    Console.WriteLine(cisla[0]);  // Vypíše: 1

    // Zavolej funkci
    ZmenPrvniPrvek(cisla);

    Console.WriteLine("PO volání:");
    Console.WriteLine(cisla[0]);  // Vypíše: 999 ← Změnilo se!
}
```

**Vizualizace v paměti:**

```
┌──────────────────────────────────────┐
│  PŘED VOLÁNÍM:                       │
│                                      │
│  Stack (zásobník):                   │
│    Main: cisla = 0x1000 ─┐           │
│                          │           │
│  Heap (halda):           ↓           │
│    Adresa 0x1000: [1][2][3][4][5]   │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│  BĚHEM VOLÁNÍ ZmenPrvniPrvek():      │
│                                      │
│  Stack:                              │
│    Main:     cisla = 0x1000 ─┐       │
│    Funkce:   pole  = 0x1000 ─┤       │
│              (stejná adresa!) │       │
│                              │       │
│  Heap:                       ↓       │
│    Adresa 0x1000: [1][2][3][4][5]   │
│                   ↑                  │
│    pole[0] = 999 změní tuto hodnotu  │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│  PO VOLÁNÍ:                          │
│                                      │
│  Stack:                              │
│    Main: cisla = 0x1000 ─┐           │
│                          │           │
│  Heap:                   ↓           │
│    Adresa 0x1000: [999][2][3][4][5] │
│                   ↑                  │
│              změněno!                │
└──────────────────────────────────────┘
```

**Proč to tak je:**

Pole je **referenční typ** (jako třídy, na rozdíl od int, double, které jsou hodnotové typy). Když předáš pole do funkce, předáváš **adresu v paměti**, ne kopii dat.

**Výhody:**
- Rychlé - nekopíruje se celé pole
- Úsporné - neplýtvá pamětí

**Nevýhody:**
- Nečekané změny - musíš si dát pozor
- Těžší debugging - změna může přijít odkudkoliv

**Pokud chceš původní pole ochránit, musíš ho zkopírovat!** (viz další sekce)

---

### Kopírování pole

Pokud nechceš, aby funkce měnila originál, musíš vytvořit **kopii** pole.

#### Proč je to důležité:

```csharp
//  ŠPATNĚ: Pouze přiřazení reference
int[] original = { 1, 2, 3, 4, 5 };
int[] kopie = original;  // Toto NENÍ kopie!

kopie[0] = 999;

Console.WriteLine(original[0]);  // Vypíše 999 (oba ukazují na stejná data)
Console.WriteLine(kopie[0]);     // Vypíše 999
```

**Vizualizace:**

```
┌────────────────────────────────────┐
│  Po přiřazení: kopie = original    │
│                                    │
│  Stack:                            │
│    original = 0x1000 ─┐            │
│    kopie    = 0x1000 ─┤ (stejná!) │
│                       │            │
│  Heap:                ↓            │
│    Adresa 0x1000: [1][2][3][4][5] │
│                                    │
│  kopie[0] = 999 → mění jediná data │
└────────────────────────────────────┘
```

---

#### Způsoby kopírování:

**1⃣ Ruční kopírování (for cyklus):**

Nejzákladnější způsob - pochopíš, jak to funguje.

```csharp
int[] original = { 1, 2, 3, 4, 5 };

// 1. Vytvoř nové pole stejné velikosti
int[] kopie = new int[original.Length];

// 2. Zkopíruj prvek po prvku
for (int i = 0; i < original.Length; i++)
{
    kopie[i] = original[i];
}

// 3. Ověř, že jsou nezávislé
kopie[0] = 999;
Console.WriteLine(original[0]);  // Vypíše: 1 (nezměnilo se!)
Console.WriteLine(kopie[0]);     // Vypíše: 999
```

**Časová složitost:** O(n) - musíš zkopírovat každý prvek

---

**2⃣ Array.Copy (efektivnější):**

Vestavěná metoda - rychlejší než for cyklus.

```csharp
int[] original = { 1, 2, 3, 4, 5 };

// Vytvoř prázdné pole
int[] kopie = new int[original.Length];

// Zkopíruj všechny prvky
//           zdroj    cíl      kolik prvků
Array.Copy(original, kopie, original.Length);

// Nebo lze specifikovat start index:
// Array.Copy(original, 0, kopie, 0, original.Length);
//            zdroj    odkud  cíl  kam  kolik
```

**Výhody:**
- Rychlejší než for cyklus (nativní optimalizace)
- Může kopírovat jen část pole

---

**3⃣ Clone() metoda:**

Vytvoří mělkou kopii pole.

```csharp
int[] original = { 1, 2, 3, 4, 5 };

// Clone vrací object, musíš přetypovat
int[] kopie = (int[])original.Clone();

// Nebo s pattern matching (C# 7+)
if (original.Clone() is int[] kopie2)
{
    // Použij kopie2
}
```

** Pozor:** Clone dělá **mělkou kopii** (shallow copy). Pro pole objektů to znamená:
- Zkopírují se reference, ne samotné objekty
- Změna objektu ovlivní obě pole

```csharp
// Příklad problému s objekty:
class Osoba
{
    public string Jmeno;
}

Osoba[] original = { new Osoba { Jmeno = "Petr" } };
Osoba[] kopie = (Osoba[])original.Clone();

kopie[0].Jmeno = "Pavel";
Console.WriteLine(original[0].Jmeno);  // Vypíše: "Pavel" (změnilo se!)
```

---

**4⃣ ToArray() s LINQ:**

Moderní způsob pro jednoduchost.

```csharp
using System.Linq;

int[] original = { 1, 2, 3, 4, 5 };
int[] kopie = original.ToArray();

// Interně volá Array.Copy
```

---

**5⃣ Spread operator (C# 12+):**

Nejmodernější a nejelegantnější.

```csharp
int[] original = { 1, 2, 3, 4, 5 };
int[] kopie = [..original];

// Krátké, čitelné, moderní
```

---

#### Srovnání kopií vs reference:

```
┌────────────────────────────────────────────┐
│  REFERENCE (kopie = original):             │
│                                            │
│  Stack:                                    │
│    original ────┐                          │
│    kopie ───────┤ (ukazují na stejná data)│
│                 │                          │
│  Heap:          ↓                          │
│    [1][2][3][4][5]                         │
│                                            │
│  změna kopie = změna originalu           │
├────────────────────────────────────────────┤
│  KOPIE (Clone/ToArray/...):                │
│                                            │
│  Stack:                                    │
│    original ──> [1][2][3][4][5]            │
│    kopie ────> [1][2][3][4][5] (nová data)│
│                                            │
│  Heap: Dvě samostatná pole                 │
│                                            │
│  změna kopie ≠ originál                  │
└────────────────────────────────────────────┘
```

---

### Výpis prvků pole

Existuje několik způsobů, jak projít a vypsat pole:

#### 1⃣ Klasický for cyklus (s indexem):

Použij, když potřebuješ znát **index** prvku.

```csharp
int[] cisla = { 10, 20, 30, 40, 50 };

// Výpis s indexem
for (int i = 0; i < cisla.Length; i++)
{
    Console.WriteLine($"Index {i}: {cisla[i]}");
}

// Výstup:
// Index 0: 10
// Index 1: 20
// Index 2: 30
// Index 3: 40
// Index 4: 50
```

**Výhody:**
- Máš přístup k indexu
- Můžeš měnit prvky: `cisla[i] = novyHodnota;`
- Můžeš procházet pozpátku: `for (int i = cisla.Length - 1; i >= 0; i--)`

**Časová složitost:** O(n) - projdeš každý prvek

---

#### 2⃣ Foreach cyklus (bez indexu):

Použij, když index **nepotřebuješ** - jednodušší a čitelnější.

```csharp
int[] cisla = { 10, 20, 30, 40, 50 };

// Prostý výpis hodnot
foreach (int cislo in cisla)
{
    Console.WriteLine(cislo);
}

// Výstup:
// 10
// 20
// 30
// 40
// 50
```

**Výhody:**
- Kratší, čitelnější kód
- Nemůžeš udělat chybu s indexem (např. `i <= cisla.Length`)
- Funguje s jakoukoliv kolekcí (pole, list, dictionary...)

**Nevýhody:**
- Nemáš přístup k indexu
- Nemůžeš měnit prvky (readonly)

---

#### 3⃣ String.Join (pro debugging):

Vypíše celé pole na **jeden řádek**.

```csharp
int[] cisla = { 10, 20, 30, 40, 50 };

// Spoj prvky čárkou
string vysledek = string.Join(", ", cisla);
Console.WriteLine(vysledek);

// Výstup:
// 10, 20, 30, 40, 50
```

**Použití:**
- Debugging - rychlý náhled na obsah pole
- Logování
- Formátovaný výstup

---

#### 4⃣ Array.ForEach (funkcionální styl):

LINQ přístup - kratší pro jednoduché operace.

```csharp
int[] cisla = { 10, 20, 30, 40, 50 };

// Předej lambda funkci pro každý prvek
Array.ForEach(cisla, c => Console.WriteLine(c));

// Ekvivalentní s:
// foreach (int c in cisla)
//     Console.WriteLine(c);
```

---

#### 5⃣ Foreach s indexem (C# 8+):

Moderní způsob, pokud chceš **obojí** - hodnotu i index.

```csharp
using System.Linq;

int[] cisla = { 10, 20, 30, 40, 50 };

foreach (var (cislo, index) in cisla.Select((value, idx) => (value, idx)))
{
    Console.WriteLine($"Index {index}: {cislo}");
}

// Výstup:
// Index 0: 10
// Index 1: 20
// ...
```

**Jak to funguje:**
- `Select` projde pole a pro každý prvek vytvoří tuple `(hodnota, index)`
- `var (cislo, index)` rozbalí tuple do proměnných

---

### ⏱ Časové složitosti operací s polem

| Operace | Časová složitost | Vysvětlení |
|---------|------------------|------------|
| **Přístup k prvku** `array[i]` | **O(1)** | Přímý výpočet adresy: `base + i × size` |
| **Změna prvku** `array[i] = x` | **O(1)** | Přímý zápis na vypočítanou adresu |
| **Hledání hodnoty** | **O(n)** | Musíš projít všechny prvky (lineární) |
| **Hledání v setříděném poli** | **O(log n)** | Binární vyhledávání (polovení) |
| **Přidání na konec** (List<T>) | **O(1)** amortizovaně | Pokud je volné místo; jinak O(n) při resize |
| **Přidání uprostřed** | **O(n)** | Musíš posunout všechny prvky za ním |
| **Mazání** | **O(n)** | Musíš posunout prvky za vymazaným |
| **Kopírování** | **O(n)** | Musíš zkopírovat všechny prvky |

---

### Proč je přístup O(1)?

```
Mám pole: int[] cisla = { 10, 20, 30, 40, 50 };

Chci prvek na indexu 3:
  1. Zjisti začátek pole v paměti: base = 1000
  2. Zjisti velikost int: size = 4 byty
  3. Vypočítej adresu: adresa = base + (index × size)
                               = 1000 + (3 × 4)
                               = 1012
  4. Přečti z adresy 1012 → hodnota 40

Pouze aritmetická operace - konstantní čas!
Nezáleží, jestli pole má 10 nebo 10 000 000 prvků.
```

---

### Časté chyby při práci s polem

#### 1. IndexOutOfRangeException

Nejčastější chyba - přístup mimo meze pole.

```csharp
//  ŠPATNĚ:
int[] cisla = new int[5];  // Indexy 0-4
cisla[5] = 10;  // Chyba! Index 5 neexistuje

//  SPRÁVNĚ:
cisla[4] = 10;  // Poslední prvek má index Length-1
```

**Pravidlo:** Platné indexy jsou `0` až `pole.Length - 1`

---

#### 2. Přepsání reference místo kopie

```csharp
//  ŠPATNĚ:
int[] original = { 1, 2, 3 };
int[] kopie = original;  // Jen reference!
kopie[0] = 999;
Console.WriteLine(original[0]);  // Vypíše 999 (oba ukazují na stejná data)

//  SPRÁVNĚ:
int[] kopie = (int[])original.Clone();
kopie[0] = 999;
Console.WriteLine(original[0]);  // Vypíše 1 (nezměnilo se)
```

---

#### 3. Off-by-one chyba v cyklu

```csharp
//  ŠPATNĚ:
int[] cisla = { 10, 20, 30, 40, 50 };
for (int i = 0; i <= cisla.Length; i++)  // <= je chyba!
{
    Console.WriteLine(cisla[i]);  // Při i=5 → IndexOutOfRange
}

//  SPRÁVNĚ:
for (int i = 0; i < cisla.Length; i++)  // < je správně
{
    Console.WriteLine(cisla[i]);
}
```

---

#### 4. Zapomenutá inicializace

```csharp
//  ŠPATNĚ:
int[] cisla;
cisla[0] = 10;  // Chyba! Pole nebylo vytvořeno

//  SPRÁVNĚ:
int[] cisla = new int[5];  // Nebo { 0, 0, 0, 0, 0 }
cisla[0] = 10;
```

---

#### 5. NullReferenceException

```csharp
//  ŠPATNĚ:
int[] cisla = null;
Console.WriteLine(cisla.Length);  // Chyba! Nelze číst z null

//  SPRÁVNĚ:
int[] cisla = null;
if (cisla != null)
{
    Console.WriteLine(cisla.Length);
}

// Nebo C# 8+ nullable:
Console.WriteLine(cisla?.Length ?? 0);  // Vypíše 0 pokud null
```

---

### List<T> jako dynamické pole

V praxi často používáme `List<T>` místo klasického pole, protože nabízí **dynamickou velikost**.

```csharp
using System.Collections.Generic;

// Vytvoření prázdného listu
List<int> cisla = new List<int>();  // Začne s kapacitou 0

// Přidávání prvků - velikost se automaticky mění
cisla.Add(10);  // kapacita: 4,  count: 1
cisla.Add(20);  // kapacita: 4,  count: 2
cisla.Add(30);  // kapacita: 4,  count: 3
cisla.Add(40);  // kapacita: 4,  count: 4
cisla.Add(50);  // kapacita: 8,  count: 5 (zdvojnásobení!)

// Přístup jako u pole
Console.WriteLine(cisla[2]);  // Vypíše: 30 (O(1) přístup)

// Vlastnosti
Console.WriteLine(cisla.Count);     // 5 - počet prvků
Console.WriteLine(cisla.Capacity);  // 8 - alokovaná kapacita
```

**Jak List<T> funguje interně:**

```
┌────────────────────────────────────────────┐
│  List<T> je pole s "rezervou":            │
│                                            │
│  Kapacita: 4  → [10][20][30][40]           │
│  Count: 4         ↑   ↑   ↑   ↑           │
│                použito vše                 │
│                                            │
│  Add(50) → Není místo!                     │
│  → Vytvoř nové pole 2× větší              │
│  → Zkopíruj stará data                    │
│  → Přidej nový prvek                      │
│                                            │
│  Kapacita: 8  → [10][20][30][40][50][_][_][_] │
│  Count: 5         ↑                 ↑      │
│                použito           rezerva   │
└────────────────────────────────────────────┘
```

**Výhody List<T> oproti poli:**
- Dynamická velikost
- Stále O(1) přístup
- Užitečné metody: `Add`, `Remove`, `Insert`, `Sort`, ...
- LINQ podpora

**Nevýhody:**
- Trochu pomalejší než čisté pole (overhead)
- Vkládání uprostřed stále O(n)

---

## BOD 4: Srovnání pole vs spojový seznam

### Kompletní srovnávací tabulka

| Kritérium | **Pole (Array)** | **Spojový seznam (LinkedList)** |
|-----------|------------------|----------------------------------|
| **Struktura v paměti** | Souvislý blok | Roztroušené uzly |
| **Velikost** | Fixní (nebo drahé zvětšování u List<T>) | Dynamická |
| **Přístup k prvku `[i]`** | O(1) přímý přístup | O(n) musíš projít uzly |
| **Vložení na začátek** | O(n) posun všech prvků | O(1) změna odkazů |
| **Vložení na konec** | O(1) pokud je místo | O(1) obousměrný /  O(n) jednosměrný |
| **Vložení uprostřed** | O(n) posun prvků | O(n) musíš najít místo + O(1) vložení |
| **Mazání ze začátku** | O(n) posun všech | O(1) změna odkazů |
| **Mazání z konce** | O(1) jen sníž count | O(1) obousměrný /  O(n) jednosměrný |
| **Mazání uprostřed** | O(n) posun prvků | O(n) musíš najít + O(1) smazání |
| **Hledání prvku** | O(n) lineární / O(log n) setříděné | O(n) lineární |
| **Cache výkon** | Výborný (CPU prefetch) | Horší (random access) |
| **Paměťová režie** | Pouze data | Data + odkazy (16B/uzel navíc) |
| **Iterace (foreach)** | Velmi rychlá | Pomalejší (skákání po paměti) |
| **Použití** | Známá velikost, časté čtení | Časté vkládání/mazání, neznámá velikost |

---

### Výhody a nevýhody

#### **POLE:**

** Výhody:**

1. **Rychlý přímý přístup** - O(1)
   ```csharp
   int[] cisla = { 10, 20, 30, 40, 50 };
   int hodnota = cisla[3];  // Okamžitě 40
   ```

2. **Menší paměťová náročnost**
   - Pouze samotná data
   - Žádné odkazy na další prvky

3. **Cache-friendly výkon**
   - CPU načte více prvků najednou (spatial locality)
   - Rychlejší iterace (for, foreach)

4. **Jednodušší na použití**
   - Přímočará syntaxe
   - Snadné pochopení

** Nevýhody:**

1. **Fixní velikost**
   - Musíš předem vědět, kolik prvků budeš mít
   - Změna velikosti = vytvoř nové pole + zkopíruj data (O(n))

2. **Pomalé vkládání/mazání uprostřed** - O(n)
   ```
   PŘED:  [10][20][30][40][50]
   Vložit 25 na index 2:

   Krok 1: Posuň všechny prvky od indexu 2 doprava
   [10][20][  ][30][40][50]

   Krok 2: Vlož nový prvek
   [10][20][25][30][40][50]

   → Musíš posunout 3 prvky (O(n))
   ```

3. **Plýtvání pamětí**
   ```
   List<int> list = new List<int>(100);  // Kapacita 100
   list.Add(10);
   list.Add(20);
   // Používáš jen 2, ale alokováno 100 → plýtvání
   ```

---

#### **SPOJOVÝ SEZNAM:**

** Výhody:**

1. **Dynamická velikost**
   - Přidávej a odebírej, kolik chceš
   - Žádné plýtvání pamětí

2. **Rychlé vkládání/mazání na koncích** - O(1)
   ```
   PŘED:  5 <-> 3 <-> 8
   AddFirst(9):

   Krok 1: Vytvoř uzel [9|•]
   Krok 2: [9|•] ukazuje na 5
   Krok 3: head = [9|•]

   PO:    9 <-> 5 <-> 3 <-> 8

   → Pouze změna 2 odkazů (O(1))
   ```

3. **Žádné přesouvání dat**
   - Vkládání = změna odkazů
   - Není potřeba posunovat prvky

4. **Snadné reorganizace**
   - Můžeš přeřadit uzly bez kopírování dat

** Nevýhody:**

1. **Pomalý přístup k prvkům** - O(n)
   ```
   Chci prvek [100]:
   → Musím projít: 0 → 1 → 2 → ... → 100
   → 100 operací!
   ```

2. **Větší paměťová režie**
   ```
   Uložení 1000 čísel (int = 4 byty):

   POLE:
   1000 × 4 = 4,000 bytů

   LINKED LIST (obousměrný):
   1000 × (4 data + 16 odkazy) = 20,000 bytů

   Rozdíl: 5× více!
   ```

3. **Cache-unfriendly**
   - Data roztroušená → CPU musí často chodit do RAM
   - Pomalejší iterace

4. **Složitější implementace**
   - Musíš spravovat odkazy
   - Víc místa pro chyby (null pointers)

---

### Kdy použít co?

#### ** Použij POLE, když:**

**1. Znáš počet prvků dopředu**

```csharp
//  Výsledky testů 30 studentů
int[] vysledky = new int[30];

//  Herní mapa 100×100
int[,] mapa = new int[100, 100];

//  Dny v týdnu
string[] dny = { "Po", "Út", "St", "Čt", "Pá", "So", "Ne" };
```

**2. Potřebuješ rychlý přístup k libovolnému prvku**

```csharp
//  Tabulka ASCII kódů
char[] ascii = new char[128];
char znak = ascii[65];  // O(1) - okamžitě 'A'

//  Pixely obrázku
Color[,] obraz = new Color[1920, 1080];
Color pixel = obraz[500, 300];  // O(1)
```

**3. Málo vkládáš/mažeš, hlavně čteš**

```csharp
//  Tabulka matematických konstant
double[] konstanty = {
    3.14159,  // Pi
    2.71828,  // e
    1.41421,  // √2
    1.61803   // φ (zlatý řez)
};

// Pouze čteme, nikdy neměníme
```

**4. Záleží na výkonu (rychlá iterace)**

```csharp
//  Zpracování velkého množství dat
int[] data = new int[1_000_000];

// For cyklus je mnohem rychlejší na poli než na LinkedList
for (int i = 0; i < data.Length; i++)
{
    data[i] = Process(data[i]);
}
```

**5. Potřebuješ vícerozměrná data**

```csharp
//  Matice, tabulky, herní pole
int[,] sachovnice = new int[8, 8];
int[][] pascal = new int[10][];  // Pascalův trojúhelník
```

---

#### ** Použij SPOJOVÝ SEZNAM, když:**

**1. Nevíš, kolik prvků budeš mít**

```csharp
//  Fronta úkolů přicházejících z webu
LinkedList<Task> taskQueue = new LinkedList<Task>();

while (server.HasNewRequest())
{
    taskQueue.AddLast(server.GetRequest());
}
```

**2. Často vkládáš/mažeš na začátku nebo konci**

```csharp
//  Historie akcí (Undo/Redo)
LinkedList<Action> undoHistory = new LinkedList<Action>();

void PerformAction(Action action)
{
    undoHistory.AddFirst(action);  // O(1) - nová akce na začátek
}

void Undo()
{
    if (undoHistory.Count > 0)
    {
        Action last = undoHistory.First.Value;
        undoHistory.RemoveFirst();  // O(1)
        last.Revert();
    }
}
```

**3. Implementuješ frontu (Queue) nebo zásobník (Stack)**

```csharp
//  FIFO fronta
LinkedList<Customer> fronta = new LinkedList<Customer>();

void EnqueueCustomer(Customer c)
{
    fronta.AddLast(c);  // Přidej na konec - O(1)
}

Customer DequeueCustomer()
{
    Customer first = fronta.First.Value;
    fronta.RemoveFirst();  // Odeber ze začátku - O(1)
    return first;
}
```

**4. Potřebuješ vkládat mezi existující prvky**

```csharp
//  Playlist hudby - vložení písně za aktuální
LinkedList<Song> playlist = new LinkedList<Song>();

LinkedListNode<Song> currentSong = playlist.Find(nowPlaying);

if (currentSong != null)
{
    playlist.AddAfter(currentSong, newSong);  // O(1) pokud máš node
}
```

**5. Implementuješ LRU cache (Least Recently Used)**

```csharp
//  Cache s odstraněním nejméně používaných
LinkedList<CacheEntry> cache = new LinkedList<CacheEntry>();

void AccessItem(CacheEntry item)
{
    // Přesuň na začátek (byl nedávno použit)
    cache.Remove(item);       // O(1) pokud máš node
    cache.AddFirst(item);     // O(1)
}

void RemoveLRU()
{
    cache.RemoveLast();  // Poslední = nejméně nedávno použitý
}
```

---

### Paměťové nároky - konkrétní příklady

#### Příklad 1: 1000 celých čísel

```
┌──────────────────────────────────────┐
│  POLE (int[]):                       │
│  1000 × 4 byty = 4,000 bytů          │
│                                      │
│  LIST<T> (s rezervou):               │
│  Interní pole + overhead             │
│  ≈ 4,500 bytů (s rezervou)          │
│                                      │
│  LINKED LIST (obousměrný):           │
│  1000 × (4 data + 8 Next + 8 Prev)  │
│  = 1000 × 20 = 20,000 bytů           │
│                                      │
│  Poměr: LinkedList je 5× větší!    │
└──────────────────────────────────────┘
```

#### Příklad 2: 10 dlouhých textů

```
┌──────────────────────────────────────┐
│  POLE (string[]):                    │
│  10 × 8 bytů (reference) = 80 bytů   │
│  + samotné texty v heap              │
│                                      │
│  LINKED LIST:                        │
│  10 × (8 ref + 16 odkazy) = 240 bytů │
│  + samotné texty v heap              │
│                                      │
│  Rozdíl: 160 bytů overhead           │
└──────────────────────────────────────┘
```

**Pravidlo palce:**
- Pro **malé kolekce** (< 100 prvků): rozdíl zanedbatelný
- Pro **velké kolekce** (> 10,000 prvků): pole výrazně úspornější

---

### Praktické příklady ze života

| Scénář | Optimální struktura | Odůvodnění |
|--------|-------------------|------------|
| **Výsledky testů 30 studentů** | `int[30]` | Známý počet, častý přístup k výsledkům |
| **Historie prohlížeče (Back/Forward)** | `LinkedList<Url>` | Časté přidávání na začátek/konec |
| **Známky z předmětů** | `Dictionary<string, int>` | Rychlý přístup podle názvu předmětu |
| **Playlist hudby** | `LinkedList<Song>` | Vkládání písní mezi existující |
| **Obrázek (pixely)** | `Color[width, height]` | Rychlý přístup k pixelům |
| **Buffer dat z internetu** | `Queue<byte[]>` | FIFO princip (interně pole) |
| **Call stack** | `Stack<Function>` | LIFO princip (interně pole) |
| **Cache s LRU** | `LinkedList<Entry>` + `Dictionary` | Kombinace - rychlé hledání + řazení |
| **Graf (vrcholy a hrany)** | `Dictionary<int, List<int>>` | Matice sousednosti nebo seznamy |

---

### List<T> jako kompromis

**List<T>** kombinuje výhody obou:

```csharp
List<int> cisla = new List<int>();  // Začne s kapacitou 0

//  Dynamická velikost (jako LinkedList)
cisla.Add(10);
cisla.Add(20);
cisla.Add(30);

//  Rychlý přístup O(1) (jako pole)
int hodnota = cisla[1];  // 20

//  Ale: vkládání uprostřed stále O(n)
cisla.Insert(1, 15);  // Musí posunout prvky
```

**Jak List<T> funguje:**

```
┌────────────────────────────────────────┐
│  List<T> interně používá pole:        │
│                                        │
│  Kapacita: 4  → [10][20][30][40]       │
│  Count: 4                              │
│                                        │
│  Add(50) → Není místo!                 │
│                                        │
│  Krok 1: Vytvoř nové pole (2× větší)  │
│  Kapacita: 8  → [_][_][_][_][_][_][_][_]│
│                                        │
│  Krok 2: Zkopíruj stará data           │
│  Kapacita: 8  → [10][20][30][40][_][_][_][_]│
│                                        │
│  Krok 3: Přidej nový prvek             │
│  Kapacita: 8  → [10][20][30][40][50][_][_][_]│
│  Count: 5                              │
│                                        │
│  Amortizovaná složitost Add: O(1)      │
│  (občas O(n) při resize)               │
└────────────────────────────────────────┘
```

**Výhody List<T>:**
- Dynamická velikost
- O(1) přístup přes index
- Bohaté API (Add, Remove, Sort, Find, ...)
- LINQ podpora

**Kdy použít:**
- 90% případů v běžném programování
- Když potřebuješ dynamickou kolekci s rychlým přístupem
- Standardní volba pro většinu úloh

---

## Na co si dát pozor (Maturitní "chytáky")

### 1. LinkedList vs List<T>

**Chyták:** "Je LinkedList rychlejší než List?"

**Odpověď:** Záleží na operaci!
- AddFirst/RemoveFirst: LinkedList O(1) vs List O(n)
- Přístup [i]: LinkedList O(n) vs List O(1)
- Iterace: LinkedList pomalejší (cache)

**Pravidlo:** List<T> je výchozí volba. LinkedList jen když **opravdu** často vkládáš na začátek.

---

### 2. "Můžeš měnit velikost pole?"

**Chyták:** "Můžu zvětšit pole po vytvoření?"

**Odpověď:**
- Klasické pole (`int[]`) má fixní velikost
- `List<T>` ano, ale interně vytváří nové pole a kopíruje (O(n))
- `LinkedList<T>` ano, bez kopírování (O(1))

```csharp
//  Toto NEJDE:
int[] cisla = new int[5];
cisla.Length = 10;  // Chyba! Length je readonly

//  Toto JDE (ale vytvoří nové pole):
Array.Resize(ref cisla, 10);  // Interně: new + copy

//  List<T> to řeší automaticky:
List<int> list = new List<int>();
list.Add(x);  // Automaticky resize když je potřeba
```

---

### 3. "Je LinkedList vždy rychlejší na vkládání?"

**Chyták:** "LinkedList má O(1) vložení, takže je vždy rychlejší?"

**Odpověď:** NE! Záleží **kde** vkládáš:

```csharp
//  LinkedList rychlejší:
linkedList.AddFirst(x);   // O(1)
list.Insert(0, x);        // O(n) - musí posunout všechny

//  LinkedList NENÍ rychlejší:
linkedList.AddLast(x);    // O(1)
list.Add(x);              // O(1) amortizovaně - stejné!

//  LinkedList POMALEJŠÍ (musíš najít místo):
var node = linkedList.Find(target);  // O(n)
linkedList.AddAfter(node, x);        // O(1)
// Celkem: O(n) + O(1) = O(n)

list.Insert(index, x);    // O(n)
// Stejná složitost, ale list je rychlejší (cache)!
```

---

### 4. "Předávání pole do funkce"

**Chyták:** "Změní funkce původní pole?"

**Odpověď:** ANO! Pole se předává referencí.

```csharp
void Funkce(int[] pole)
{
    pole[0] = 999;  // Změní originál!
}

int[] cisla = { 1, 2, 3 };
Funkce(cisla);
Console.WriteLine(cisla[0]);  // 999 ← změnilo se!
```

**Řešení:** Pokud chceš ochránit originál, zkopíruj ho:
```csharp
Funkce((int[])cisla.Clone());  // Předáš kopii
```

---

### 5. "Je pole nebo LinkedList lepší pro hledání?"

**Chyták:** "Který je rychlejší pro hledání prvku?"

**Odpověď:** Záleží, jestli je **setříděné**:

| Struktura | Nesetříděné | Setříděné |
|-----------|-------------|-----------|
| **Pole** | O(n) lineární | O(log n) binární vyhledávání |
| **LinkedList** | O(n) lineární | O(n) binární vyhledávání NEFUNGUJE! |

**Proč LinkedList nemůže binární vyhledávání?**
- Binární vyhledávání potřebuje přístup ke středu → O(1)
- LinkedList přístup ke středu → O(n)
- Celkem: O(n log n) → horší než lineární O(n)!

---

### 6. Cache výkon

**Chyták:** "Proč je pole rychlejší při iteraci, když obě mají O(n)?"

**Odpověď:** **Cache locality!**

```
POLE:
[1][2][3][4][5]  ← Všechno vedle sebe
CPU načte: [1][2][3][4] najednou z cache
→ Rychlé!

LINKED LIST:
[1]→[2]→[3]→[4]→[5]  ← Roztroušené v paměti
CPU musí:
  Načti [1] z RAM → čekání
  Načti [2] z RAM → čekání
  Načti [3] z RAM → čekání
→ Pomalé!
```

**Benchmark (1 milion prvků):**
- Pole: ~2 ms
- LinkedList: ~15 ms (7× pomalejší!)

---

### 7. Null vs prázdné pole

**Chyták:** "Jaký je rozdíl mezi `null` a `new int[0]`?"

```csharp
//  Null - neexistuje žádné pole
int[] pole1 = null;
Console.WriteLine(pole1.Length);  // NullReferenceException!

//  Prázdné pole - existuje, ale má 0 prvků
int[] pole2 = new int[0];  // nebo Array.Empty<int>()
Console.WriteLine(pole2.Length);  // 0 (v pořádku)
```

**Best practice:** Raději prázdné pole než null (vyhneš se null checks).

---

## Senior Tipy

### 1. Preferuj List<T> před polem

```csharp
//  Méně flexibilní:
int[] cisla = new int[100];

//  Lépe:
List<int> cisla = new List<int>();

// Proč:
// - Dynamická velikost
// - Bohaté API
// - Stále O(1) přístup
```

---

### 2. Použij LINQ pro složité operace

```csharp
//  Ruční průnik (O(n×m)):
LinkedList<int> result = new LinkedList<int>();
foreach (int x in list1)
    if (list2.Contains(x) && !result.Contains(x))
        result.AddLast(x);

//  LINQ (O(n+m)):
var result = list1.Intersect(list2);
```

---

### 3. Array.Empty<T>() pro prázdná pole

```csharp
//  Alokuje paměť zbytečně:
int[] empty1 = new int[0];

//  Použije cached instanci (žádná alokace):
int[] empty2 = Array.Empty<int>();
```

---

### 4. Span<T> pro výkon

Pro high-performance scénáře:

```csharp
//  Žádná alokace, rychlé
Span<int> cisla = stackalloc int[100];
cisla[50] = 42;
```

---

### 5. Kdy opravdu použít LinkedList

```csharp
//  Opravdu má smysl:
// 1. LRU Cache
// 2. Undo/Redo historie
// 3. Playlist s častým vkládáním mezi písně
// 4. Priority Queue implementace

//  Zbytečné:
// 1. Běžná kolekce dat → použij List<T>
// 2. Setříděné hledání → použij pole nebo SortedSet<T>
// 3. Libovolný přístup → použij pole nebo List<T>
```

---

## Quick Reference (Rychlá nápověda)

### Pole (Array)
```csharp
// Vytvoření
int[] arr = { 1, 2, 3, 4, 5 };
int[] arr2 = new int[10];

// Přístup
int x = arr[2];  // O(1)

// Délka
int len = arr.Length;

// Kopie
int[] copy = (int[])arr.Clone();

// Výpis
foreach (int x in arr) { }
for (int i = 0; i < arr.Length; i++) { }
```

### LinkedList<T>
```csharp
// Vytvoření
LinkedList<int> list = new LinkedList<int>();

// Přidání
list.AddFirst(1);   // O(1)
list.AddLast(5);    // O(1)

// Hledání
var node = list.Find(3);  // O(n)
if (node != null)
    list.AddAfter(node, 4);  // O(1)

// Odebrání
list.RemoveFirst();  // O(1)
list.RemoveLast();   // O(1)
list.Remove(3);      // O(n)

// Výpis
foreach (int x in list) { }
```

### List<T>
```csharp
// Vytvoření
List<int> list = new List<int>();

// Přidání
list.Add(1);        // O(1) amortizovaně
list.Insert(0, 2);  // O(n)

// Přístup
int x = list[2];    // O(1)

// Hledání
bool exists = list.Contains(3);  // O(n)
int index = list.IndexOf(3);     // O(n)

// Odebrání
list.Remove(3);     // O(n)
list.RemoveAt(0);   // O(n)

// LINQ
var filtered = list.Where(x => x > 5).ToList();
```

---

## Souvislosti s jinými otázkami

- **Otázka 1:** Datové typy - pole a LinkedList jsou složené datové typy
- **Otázka 3:** Fronta a zásobník - implementovatelné pomocí pole i LinkedList
- **Otázka 4:** Vlastnosti algoritmů - časová složitost operací
- **Otázka 7:** Časová a paměťová složitost - analýza pole vs LinkedList
- **Otázka 9:** Stromy - stromy lze implementovat jako LinkedList uzlů
- **Otázky 10-13:** Třídění - třídí se pole (in-place) nebo LinkedList
- **Otázka 14:** Vyhledávání - binární vyhledávání v setříděném poli

---

** Otázka 2 kompletně hotova!**

*Toto je kompletní zápis pro maturitu. Obsahuje vše, co potřebuješ znát o spojových seznamech a polích - od základů přes implementace až po praktické použití a časté chyby.*
