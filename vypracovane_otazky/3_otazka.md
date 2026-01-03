# 📚 Maturitní Zápisky: Otázka č. 3 - Fronta a Zásobník

**Datum:** 3. ledna 2025  
**Status:** ✅ KOMPLETNÍ  
**Téma:** Fronta, Zásobník, Časová složitost, Algoritmy (DFS, BFS, ...), Rekurze, Call Stack

---

## 📋 OBSAH

1. [Checklist bodů otázky](#checklist)
2. [BOD 1: Fronta (Queue)](#bod-1-fronta)
3. [BOD 2: Zásobník (Stack)](#bod-2-zásobník)
4. [BOD 3: Časová složitost - DETAILNÍ](#bod-3-časová-složitost)
5. [BOD 4: Reprezentace v C#](#bod-4-reprezentace-v-c)
6. [BOD 5: Příklady algoritmů - HODNĚ!](#bod-5-příklady-algoritmů)
7. [BOD 6: Nahrazení rekurze zásobníkem](#bod-6-nahrazení-rekurze)
8. [BOD 7: Volací zásobník (Call Stack)](#bod-7-volací-zásobník)
9. [Maturitní chytáky](#chytáky)
10. [Senior tipy](#senior-tipy)

---

<a name="checklist"></a>
## ✅ CHECKLIST BODŮ OTÁZKY

- [x] **Bod 1:** Fronta - co to je (FIFO), demonstrace na obrázku, základní metody (Enqueue, Dequeue)
- [x] **Bod 2:** Zásobník - co to je (LIFO), demonstrace na obrázku, základní metody (Push, Pop)
- [x] **Bod 3:** Časová náročnost operací vzhledem k implementaci (pole, spojový seznam)
- [x] **Bod 4:** Reprezentace v jazyce C# (Queue<T>, Stack<T>)
- [x] **Bod 5:** Příklady algoritmů kde se používají (DFS, BFS, kontrola závorek, tiskárna, Undo/Redo, ...)
- [x] **Bod 6:** Nahrazení rekurze zásobníkem
- [x] **Bod 7:** Volací zásobník (Call Stack)

---

<a name="bod-1-fronta"></a>
## 📦 BOD 1: FRONTA (QUEUE)

### 🎯 Definice

**FIFO** = **F**irst **I**n, **F**irst **O**ut  
➡️ "První dovnitř, první ven"  
➡️ Jako fronta v obchodě - kdo přijde první, je první obsloužen

---

### 📊 Vizualizace

```
    ← DEQUEUE (ODEBÍRÁME ODSUD)    ENQUEUE (PŘIDÁVÁME SEM) →
    
    ┌─────┬─────┬─────┬─────┬─────┐
    │  1  │  2  │  3  │  4  │  5  │
    └─────┴─────┴─────┴─────┴─────┘
    ↑                             ↑
  FRONT                         BACK
  (začátek, hlava)             (konec, ocas)
```

**Kroky:**
1. `Enqueue(1)` → Fronta: [1]
2. `Enqueue(2)` → Fronta: [1, 2]
3. `Enqueue(3)` → Fronta: [1, 2, 3]
4. `Dequeue()` → Vrátí 1, Fronta: [2, 3]
5. `Dequeue()` → Vrátí 2, Fronta: [3]

---

### 🔧 Základní metody

| Metoda | Popis | Časová složitost |
|--------|-------|------------------|
| `Enqueue(T)` | Přidá prvek na konec | O(1)* |
| `Dequeue()` | Odebere prvek ze začátku | O(1) |
| `Peek()` | Podívá se na první prvek (neodebírá) | O(1) |
| `Count` | Počet prvků | O(1) |
| `Clear()` | Vymaže všechny prvky | O(1) |
| `Contains(T)` | Zda fronta obsahuje prvek | O(n) |

*O(1) amortizovaná - viz [BOD 3](#bod-3-časová-složitost)

---

### 💡 Příklady z reálného života

1. **Fronta v obchodě:**
   - První zákazník v řadě → První obsloužen
   - Poslední v řadě → Poslední obsloužen

2. **Tiskárna:**
   - První dokument poslán na tisk → První vytisknut
   - Dokumenty se tisknou v pořadí odeslání

3. **Call centrum:**
   - První volající → První spojen s operátorem
   - Další čekají v pořadí

4. **Provoz na dálnici:**
   - První auto vjede na dálnici → První vyjede
   - FIFO tok vozidel

---

<a name="bod-2-zásobník"></a>
## 📦 BOD 2: ZÁSOBNÍK (STACK)

### 🎯 Definice

**LIFO** = **L**ast **I**n, **F**irst **O**ut  
➡️ "Poslední dovnitř, první ven"  
➡️ Jako hromada talířů - poslední položený je první sebraný

---

### 📊 Vizualizace

```
         ↑ POP (ODEBÍRÁME ODSUD)
         ↓ PUSH (PŘIDÁVÁME SEM)
         
    ┌─────────┐
    │    5    │ ← VRCHOL (TOP) - poslední přidaný
    ├─────────┤
    │    4    │
    ├─────────┤
    │    3    │
    ├─────────┤
    │    2    │
    ├─────────┤
    │    1    │ ← DNO (BOTTOM) - první přidaný
    └─────────┘
```

**Kroky:**
1. `Push(1)` → Stack: [1]
2. `Push(2)` → Stack: [1, 2]
3. `Push(3)` → Stack: [1, 2, 3]
4. `Pop()` → Vrátí 3, Stack: [1, 2]
5. `Pop()` → Vrátí 2, Stack: [1]

---

### 🔧 Základní metody

| Metoda | Popis | Časová složitost |
|--------|-------|------------------|
| `Push(T)` | Přidá prvek na vrchol | O(1)* |
| `Pop()` | Odebere prvek z vrcholu | O(1) |
| `Peek()` | Podívá se na vrchol (neodebírá) | O(1) |
| `Count` | Počet prvků | O(1) |
| `Clear()` | Vymaže všechny prvky | O(1) |
| `Contains(T)` | Zda stack obsahuje prvek | O(n) |

*O(1) amortizovaná - viz [BOD 3](#bod-3-časová-složitost)

---

### 💡 Příklady z reálného života

1. **Hromada talířů:**
   - Poslední položený talíř → První sebraný
   - První položený talíř → Poslední sebraný

2. **Undo/Redo v editoru:**
   - Poslední akce → První vrácena pomocí Ctrl+Z
   - Stack akcí: [Napsal "A", Napsal "B", Smazal "B"] → Undo vrátí mazání

3. **Historie prohlížeče (tlačítko Zpět):**
   - Poslední navštívená stránka → První kam se vrátíš
   - Stack: [Google, YouTube, Wiki] → Zpět vrátí na YouTube

4. **Zásobník knih:**
   - Poslední kniha nahoře → První sebraná
   - První kniha dole → Poslední sebraná

---

<a name="bod-3-časová-složitost"></a>
## ⏱️ BOD 3: ČASOVÁ SLOŽITOST - DETAILNÍ VYSVĚTLENÍ

### 📊 PŘEHLEDOVÁ TABULKA

```
╔═══════════════════════════════════════════════════════════════╗
║  OPERACE      │  POLE           │  SPOJOVÝ SEZNAM             ║
╠═══════════════════════════════════════════════════════════════╣
║  Push/Pop     │  O(1)*          │  O(1)                       ║
║  Enqueue      │  O(1)*          │  O(1)                       ║
║  Dequeue      │  O(1)           │  O(1)                       ║
║  Peek         │  O(1)           │  O(1)                       ║
║  Contains     │  O(n)           │  O(n)                       ║
║  Count        │  O(1)           │  O(1)                       ║
╚═══════════════════════════════════════════════════════════════╝

* = Amortizovaná O(1) - většinou O(1), občas O(n) při resize
```

---

### 🎯 1. ZÁSOBNÍK - IMPLEMENTACE POLEM

#### **Struktura:**

```
Zásobník s kapacitou 5:
┌───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │   │   │  ← pole (array)
└───┴───┴───┴───┴───┘
  0   1   2   3   4    (indexy)
          ↑
       vrchol = 2
       (index posledního prvku)
```

---

#### **PUSH(5) - Přidání prvku:**

```csharp
class MujStack {
    private int[] pole;
    private int vrchol;      // index posledního prvku
    private int kapacita;
    
    public void Push(int hodnota) {
        // Krok 1: Kontrola kapacity
        if (vrchol + 1 >= kapacita) {
            Resize();  // ← TADY je O(n)!
        }
        
        // Krok 2: Přidáme prvek
        vrchol++;
        pole[vrchol] = hodnota;  // O(1) - přímý přístup do pole
    }
}
```

**VIZUALIZACE PUSH(5):**

```
PŘED Push(5):
┌───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │   │   │
└───┴───┴───┴───┴───┘
          ↑
       vrchol=2

Krok 1: vrchol+1 (3) < kapacita (5) ✅ OK, není potřeba resize
Krok 2: vrchol = 3
Krok 3: pole[3] = 5

PO Push(5):
┌───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │ 5 │   │
└───┴───┴───┴───┴───┘
              ↑
          vrchol=3

ČASOVÁ SLOŽITOST: O(1) ✅
```

---

#### **POP() - Odebrání prvku:**

```csharp
public int Pop() {
    // Krok 1: Kontrola, zda není prázdný
    if (vrchol < 0) {
        throw new InvalidOperationException("Stack je prázdný!");
    }
    
    // Krok 2: Odebereme prvek
    int hodnota = pole[vrchol];  // O(1) - přímý přístup
    vrchol--;                    // O(1) - jen snížíme index
    return hodnota;
}
```

**VIZUALIZACE POP():**

```
PŘED Pop():
┌───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │ 5 │   │
└───┴───┴───┴───┴───┘
              ↑
          vrchol=3

Krok 1: vrchol (3) >= 0 ✅ OK
Krok 2: return pole[3] = 5
Krok 3: vrchol = 2

PO Pop():
┌───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │ 5 │   │  (5 tam zůstává, ale je "neviditelné")
└───┴───┴───┴───┴───┘
          ↑
       vrchol=2

ČASOVÁ SLOŽITOST: O(1) ✅
```

---

### ⚠️ CO JE TO "AMORTIZOVANÁ O(1)"?

**Problém:** Co když je pole plné a chceme Push?

```
Plný zásobník (kapacita=3):
┌───┬───┬───┐
│ 3 │ 7 │ 9 │  ← PLNÝ!
└───┴───┴───┘
vrchol=2

Teď chci Push(5) → pole je plné! ❌
```

**Řešení: RESIZE (zvětšení pole)**

```csharp
private void Resize() {
    // Krok 1: Vytvoř nové pole 2× větší
    int novaKapacita = kapacita * 2;
    int[] novePole = new int[novaKapacita];  // O(1)
    
    // Krok 2: Zkopíruj VŠECHNY prvky
    for (int i = 0; i <= vrchol; i++) {      // O(n) ❌
        novePole[i] = pole[i];
    }
    
    // Krok 3: Nahraď staré pole novým
    pole = novePole;          // O(1)
    kapacita = novaKapacita;  // O(1)
}
```

**VIZUALIZACE RESIZE:**

```
PŘED Resize (kapacita=3, plný):
┌───┬───┬───┐
│ 3 │ 7 │ 9 │
└───┴───┴───┘

Krok 1: Vytvoř nové pole velikosti 2×3 = 6
┌───┬───┬───┬───┬───┬───┐
│   │   │   │   │   │   │
└───┴───┴───┴───┴───┴───┘

Krok 2: Zkopíruj všechny prvky (O(n) - musím projít n=3 prvky)
┌───┬───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │   │   │   │
└───┴───┴───┴───┴───┴───┘

PO Resize:
┌───┬───┬───┬───┬───┬───┐
│ 3 │ 7 │ 9 │   │   │   │  kapacita=6
└───┴───┴───┴───┴───┴───┘
          ↑
       vrchol=2

ČASOVÁ SLOŽITOST RESIZE: O(n) ❌
```

---

### 📊 AMORTIZOVANÁ ANALÝZA

**Co to znamená "amortizovaná O(1)"?**

Představ si, že děláš **100 operací Push**:

```
Push č. 1:      O(1)            ✅ OK
Push č. 2:      O(1)            ✅ OK
Push č. 3:      O(1)            ✅ OK
Push č. 4:      O(3) - RESIZE!  ❌ Zkopíruj 3 prvky
Push č. 5-7:    O(1) × 3        ✅ OK
Push č. 8:      O(7) - RESIZE!  ❌ Zkopíruj 7 prvků
Push č. 9-15:   O(1) × 7        ✅ OK
Push č. 16:     O(15) - RESIZE! ❌ Zkopíruj 15 prvků
...

Celkem operací: 100
Resize operací: ~7 (při kapacitách 4, 8, 16, 32, 64, 128)
Normální Push:  ~93

PRŮMĚR = (93×1 + 7×n) / 100 ≈ O(1)
```

**JEDNODUŠEJI:**

```
Z 1000 Push operací:
- 990× je O(1) ✅ (normální push)
- 10× je O(n) ❌ (resize)

Průměrně každá operace = O(1)
Proto říkáme "amortizovaná O(1)"
```

**ANALOGIE:**  
Je to jako platit nájem. Většinu dní nic neplatíš (O(1)), ale jednou za měsíc zaplatíš větší částku (O(n)). Průměrně ale platíš stejně každý den.

---

### 🎯 2. ZÁSOBNÍK - IMPLEMENTACE SPOJOVÝM SEZNAMEM

#### **Struktura:**

```
Vrchol
  ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 9 │───→│ 7 │───→│ 3 │───→│null
└───┘    └───┘    └───┘    └───┘
```

**Node třída:**
```csharp
class Node {
    public int Hodnota;
    public Node Dalsi;
}
```

---

#### **PUSH(5) - Přidání prvku:**

```csharp
class MujStack {
    private Node vrchol;
    
    public void Push(int hodnota) {
        // Krok 1: Vytvoř nový uzel
        Node novyNode = new Node {        // O(1)
            Hodnota = hodnota,
            Dalsi = vrchol  // ukazuje na starý vrchol
        };
        
        // Krok 2: Nastav nový vrchol
        vrchol = novyNode;                // O(1)
    }
}
```

**VIZUALIZACE PUSH(5):**

```
PŘED Push(5):
Vrchol
  ↓
┌───┐    ┌───┐    ┌───┐
│ 9 │───→│ 7 │───→│ 3 │───→null
└───┘    └───┘    └───┘

Krok 1: Vytvoř nový node s hodnotou 5
        ┌───┐
        │ 5 │
        └───┘

Krok 2: Nastav nový.Dalsi = starý vrchol
        ┌───┐
        │ 5 │───┐
        └───┘   │
                ↓
              ┌───┐    ┌───┐    ┌───┐
              │ 9 │───→│ 7 │───→│ 3 │───→null
              └───┘    └───┘    └───┘

Krok 3: vrchol = nový
Vrchol
  ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 5 │───→│ 9 │───→│ 7 │───→│ 3 │───→null
└───┘    └───┘    └───┘    └───┘

ČASOVÁ SLOŽITOST: O(1) ✅
ŽÁDNÝ RESIZE! ✅
```

---

#### **POP() - Odebrání prvku:**

```csharp
public int Pop() {
    // Krok 1: Kontrola
    if (vrchol == null) {
        throw new InvalidOperationException("Stack je prázdný!");
    }
    
    // Krok 2: Ulož hodnotu
    int hodnota = vrchol.Hodnota;  // O(1)
    
    // Krok 3: Posuň vrchol na další
    vrchol = vrchol.Dalsi;         // O(1)
    
    return hodnota;
}
```

**VIZUALIZACE POP():**

```
PŘED Pop():
Vrchol
  ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 5 │───→│ 9 │───→│ 7 │───→│ 3 │───→null
└───┘    └───┘    └───┘    └───┘

Krok 1: hodnota = vrchol.Hodnota = 5
Krok 2: vrchol = vrchol.Dalsi

PO Pop():
        Vrchol
          ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 5 │    │ 9 │───→│ 7 │───→│ 3 │───→null
└───┘    └───┘    └───┘    └───┘
  ↑
 Garbage Collector toto automaticky smaže

ČASOVÁ SLOŽITOST: O(1) ✅
```

---

### 🎯 3. FRONTA - IMPLEMENTACE POLEM (CIRCULAR BUFFER)

#### **Struktura:**

```
Fronta s kapacitou 6:
    FRONT                    BACK
      ↓                        ↓
┌───┬───┬───┬───┬───┬───┐
│   │ 3 │ 7 │ 9 │ 5 │   │
└───┴───┴───┴───┴───┴───┘
  0   1   2   3   4   5

front = 1 (index prvního prvku)
back = 5 (index pro další prvek)
```

---

#### **ENQUEUE(8) - Přidání prvku:**

```csharp
class MojaFronta {
    private int[] pole;
    private int front;
    private int back;
    private int pocet;
    private int kapacita;
    
    public void Enqueue(int hodnota) {
        // Krok 1: Kontrola kapacity
        if (pocet == kapacita) {
            Resize();  // ← O(n)
        }
        
        // Krok 2: Přidej prvek
        pole[back] = hodnota;           // O(1)
        back = (back + 1) % kapacita;   // O(1) - circular!
        pocet++;                        // O(1)
    }
}
```

**VIZUALIZACE ENQUEUE(8):**

```
PŘED Enqueue(8):
    FRONT                    BACK
      ↓                        ↓
┌───┬───┬───┬───┬───┬───┐
│   │ 3 │ 7 │ 9 │ 5 │   │
└───┴───┴───┴───┴───┴───┘
  0   1   2   3   4   5

Krok 1: pocet (4) < kapacita (6) ✅ OK
Krok 2: pole[5] = 8
Krok 3: back = (5 + 1) % 6 = 0  (vrátí se na začátek!)

PO Enqueue(8):
  BACK  FRONT
    ↓     ↓
┌───┬───┬───┬───┬───┬───┐
│   │ 3 │ 7 │ 9 │ 5 │ 8 │
└───┴───┴───┴───┴───┴───┘
  0   1   2   3   4   5

ČASOVÁ SLOŽITOST: O(1)* (amortizovaná)
```

---

#### **DEQUEUE() - Odebrání prvku:**

```csharp
public int Dequeue() {
    // Krok 1: Kontrola
    if (pocet == 0) {
        throw new InvalidOperationException("Fronta je prázdná!");
    }
    
    // Krok 2: Ulož hodnotu
    int hodnota = pole[front];          // O(1)
    
    // Krok 3: Posuň front
    front = (front + 1) % kapacita;     // O(1) - circular!
    pocet--;                            // O(1)
    
    return hodnota;
}
```

**VIZUALIZACE DEQUEUE():**

```
PŘED Dequeue():
  BACK  FRONT
    ↓     ↓
┌───┬───┬───┬───┬───┬───┐
│   │ 3 │ 7 │ 9 │ 5 │ 8 │
└───┴───┴───┴───┴───┴───┘
  0   1   2   3   4   5

Krok 1: hodnota = pole[1] = 3
Krok 2: front = (1 + 1) % 6 = 2
Krok 3: pocet = 3

PO Dequeue():
  BACK      FRONT
    ↓         ↓
┌───┬───┬───┬───┬───┬───┐
│   │ 3 │ 7 │ 9 │ 5 │ 8 │
└───┴───┴───┴───┴───┴───┘
  0   1   2   3   4   5

ČASOVÁ SLOŽITOST: O(1) ✅
```

**DŮLEŽITÉ:** Circular buffer (% operátor) zajišťuje, že Dequeue je O(1)!

---

### 🎯 4. FRONTA - IMPLEMENTACE SPOJOVÝM SEZNAMEM

#### **Struktura:**

```
Front                              Back
  ↓                                  ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 3 │───→│ 7 │───→│ 9 │───→│ 5 │───→null
└───┘    └───┘    └───┘    └───┘
```

---

#### **ENQUEUE(8) - Přidání prvku:**

```csharp
class MojaFronta {
    private Node front;
    private Node back;
    
    public void Enqueue(int hodnota) {
        // Krok 1: Vytvoř nový uzel
        Node novyNode = new Node {        // O(1)
            Hodnota = hodnota,
            Dalsi = null
        };
        
        // Krok 2: Připoj za back
        if (back != null) {
            back.Dalsi = novyNode;        // O(1)
        }
        back = novyNode;                  // O(1)
        
        // Krok 3: Pokud je fronta prázdná
        if (front == null) {
            front = back;                 // O(1)
        }
    }
}
```

**VIZUALIZACE ENQUEUE(8):**

```
PŘED Enqueue(8):
Front                              Back
  ↓                                  ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 3 │───→│ 7 │───→│ 9 │───→│ 5 │───→null
└───┘    └───┘    └───┘    └───┘

Krok 1: Vytvoř nový node
                              ┌───┐
                              │ 8 │───→null
                              └───┘

Krok 2: back.Dalsi = nový, back = nový

PO Enqueue(8):
Front                                   Back
  ↓                                       ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 3 │───→│ 7 │───→│ 9 │───→│ 5 │───→│ 8 │───→null
└───┘    └───┘    └───┘    └───┘    └───┘

ČASOVÁ SLOŽITOST: O(1) ✅
ŽÁDNÝ RESIZE! ✅
```

---

#### **DEQUEUE() - Odebrání prvku:**

```csharp
public int Dequeue() {
    // Krok 1: Kontrola
    if (front == null) {
        throw new InvalidOperationException("Fronta je prázdná!");
    }
    
    // Krok 2: Ulož hodnotu
    int hodnota = front.Hodnota;  // O(1)
    
    // Krok 3: Posuň front na další
    front = front.Dalsi;          // O(1)
    
    // Krok 4: Pokud je fronta teď prázdná
    if (front == null) {
        back = null;              // O(1)
    }
    
    return hodnota;
}
```

**VIZUALIZACE DEQUEUE():**

```
PŘED Dequeue():
Front                                   Back
  ↓                                       ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 3 │───→│ 7 │───→│ 9 │───→│ 5 │───→│ 8 │───→null
└───┘    └───┘    └───┘    └───┘    └───┘

Krok 1: hodnota = front.Hodnota = 3
Krok 2: front = front.Dalsi

PO Dequeue():
        Front                            Back
          ↓                                ↓
┌───┐    ┌───┐    ┌───┐    ┌───┐    ┌───┐
│ 3 │    │ 7 │───→│ 9 │───→│ 5 │───→│ 8 │───→null
└───┘    └───┘    └───┘    └───┘    └───┘
  ↑
Garbage Collector smaže

ČASOVÁ SLOŽITOST: O(1) ✅
```

---

### 📊 FINÁLNÍ SROVNÁNÍ IMPLEMENTACÍ

#### **ZÁSOBNÍK (STACK):**

```
╔═══════════════════════════════════════════════════╗
║  IMPLEMENTACE POLEM:                              ║
║  • Push:    O(1)* (amortizovaná, občas resize)    ║
║  • Pop:     O(1) ✅                               ║
║  • Peek:    O(1) ✅                               ║
║  • Výhoda:  Rychlý přístup přes index             ║
║  • Nevýhoda: Občas resize (O(n))                  ║
╠═══════════════════════════════════════════════════╣
║  IMPLEMENTACE SPOJOVÝM SEZNAMEM:                  ║
║  • Push:    O(1) ✅ (ŽÁDNÝ RESIZE!)               ║
║  • Pop:     O(1) ✅                               ║
║  • Peek:    O(1) ✅                               ║
║  • Výhoda:  Bez resize, stabilní O(1)             ║
║  • Nevýhoda: Pomalejší (ukazatele v paměti)       ║
╚═══════════════════════════════════════════════════╝
```

#### **FRONTA (QUEUE):**

```
╔═══════════════════════════════════════════════════╗
║  IMPLEMENTACE POLEM (circular buffer):            ║
║  • Enqueue: O(1)* (amortizovaná)                  ║
║  • Dequeue: O(1) ✅                               ║
║  • Peek:    O(1) ✅                               ║
║  • Výhoda:  Rychlý přístup                        ║
║  • Nevýhoda: Občas resize (O(n))                  ║
╠═══════════════════════════════════════════════════╣
║  IMPLEMENTACE SPOJOVÝM SEZNAMEM:                  ║
║  • Enqueue: O(1) ✅ (ŽÁDNÝ RESIZE!)               ║
║  • Dequeue: O(1) ✅                               ║
║  • Peek:    O(1) ✅                               ║
║  • Výhoda:  Bez resize, stabilní O(1)             ║
║  • Nevýhoda: Více paměti (ukazatele)              ║
╚═══════════════════════════════════════════════════╝
```

---

### 🎯 CO POUŽÍVÁ C#?

```csharp
// Stack<T> - implementace POLEM s resize
Stack<int> stack = new Stack<int>();
// Push/Pop: O(1)* amortizovaná

// Queue<T> - implementace CIRCULAR BUFFEREM
Queue<int> queue = new Queue<int>();
// Enqueue/Dequeue: O(1)* amortizovaná
```

**Obě mají všechny základní operace v O(1)*!** ✅

---

<a name="bod-4-reprezentace-v-c"></a>
## 💻 BOD 4: REPREZENTACE V C#

### 🔧 Queue<T> - Fronta

```csharp
// Vytvoření fronty
Queue<int> cisla = new Queue<int>();
Queue<string> jmena = new Queue<string>();

// Základní operace
cisla.Enqueue(5);           // Přidá 5 na konec
cisla.Enqueue(10);          // Přidá 10 na konec
cisla.Enqueue(15);          // Fronta: [5, 10, 15]

int prvni = cisla.Dequeue(); // Odebere 5 (první), Fronta: [10, 15]
int dalsi = cisla.Peek();    // Podívá se na 10 (neodebere)

int pocet = cisla.Count;     // Počet prvků = 2
bool jePrazdna = cisla.Count == 0;  // false

cisla.Clear();               // Vymaže všechny prvky
bool obsahuje = cisla.Contains(10);  // false (už je prázdná)
```

#### **Kompletní příklad - Tiskárna:**

```csharp
Queue<string> tiskoveUlohy = new Queue<string>();

// Přidáváme dokumenty do fronty
tiskoveUlohy.Enqueue("Dokument1.pdf");
tiskoveUlohy.Enqueue("Foto.jpg");
tiskoveUlohy.Enqueue("Esej.docx");

Console.WriteLine($"Počet úloh: {tiskoveUlohy.Count}");

// Tiskneme v pořadí přidání
while (tiskoveUlohy.Count > 0) {
    string dokument = tiskoveUlohy.Dequeue();
    Console.WriteLine($"Tisknu: {dokument}");
}

// Výstup:
// Tisknu: Dokument1.pdf
// Tisknu: Foto.jpg
// Tisknu: Esej.docx
```

---

### 🔧 Stack<T> - Zásobník

```csharp
// Vytvoření zásobníku
Stack<int> cisla = new Stack<int>();
Stack<string> slova = new Stack<string>();

// Základní operace
cisla.Push(5);              // Přidá 5 na vrchol
cisla.Push(10);             // Přidá 10 na vrchol
cisla.Push(15);             // Stack: [5, 10, 15] (15 nahoře)

int vrchol = cisla.Pop();   // Odebere 15 (poslední), Stack: [5, 10]
int dalsi = cisla.Peek();   // Podívá se na 10 (neodebere)

int pocet = cisla.Count;    // Počet prvků = 2
bool jePrazdny = cisla.Count == 0;  // false

cisla.Clear();              // Vymaže všechny prvky
bool obsahuje = cisla.Contains(10);  // false (už je prázdný)
```

#### **Kompletní příklad - Historie prohlížeče:**

```csharp
Stack<string> historie = new Stack<string>();

// Procházení webu
historie.Push("google.com");
historie.Push("youtube.com");
historie.Push("wikipedia.org");

Console.WriteLine($"Aktuální stránka: {historie.Peek()}");

// Tlačítko "Zpět"
string predchozi = historie.Pop();
Console.WriteLine($"Zpět z: {predchozi}");
Console.WriteLine($"Teď jsem na: {historie.Peek()}");

// Výstup:
// Aktuální stránka: wikipedia.org
// Zpět z: wikipedia.org
// Teď jsem na: youtube.com
```

---

### 🎯 GENERICKÝ TYP <T>

**Co to je?**  
`<T>` je **placeholder** pro jakýkoli datový typ.

```csharp
// T = int
Stack<int> cisla = new Stack<int>();

// T = string
Stack<string> slova = new Stack<string>();

// T = Student (vlastní třída)
Stack<Student> studenti = new Stack<Student>();
```

#### **Kdy používat <T>?**

**1. Při TVORBĚ vlastní třídy:**
```csharp
// ❌ ŠPATNĚ - funguje jen pro int
class Box {
    public int Hodnota;
}

// ✅ SPRÁVNĚ - funguje pro cokoliv!
class Box<T> {
    public T Hodnota;
    
    public Box(T hodnota) {
        Hodnota = hodnota;
    }
}

// Použití:
Box<int> cislo = new Box<int>(42);
Box<string> text = new Box<string>("Ahoj");
Box<double> desetinne = new Box<double>(3.14);
```

**2. Při použití hotové třídy (Stack, Queue) - UŽ KONKRÉTNÍ:**
```csharp
// ✅ SPRÁVNĚ - konkretizujeme typ
Stack<int> s = new Stack<int>();

// ❌ ŠPATNĚ - Stack musí vědět, co v něm je
Stack s = new Stack();  // Chyba!
```

---

### 🎁 BONUS: HashSet<T>

**Co to je?**  
Množina - kolekce **unikátních** prvků (bez duplikátů)

```csharp
HashSet<int> mnozina = new HashSet<int>();

mnozina.Add(5);      // true - přidáno ✅
mnozina.Add(10);     // true - přidáno ✅
mnozina.Add(5);      // false - duplikát, IGNOROVÁNO ❌

Console.WriteLine(mnozina.Count);  // 2 (5 a 10)

bool obsahuje = mnozina.Contains(5);  // true - O(1) rychlé! ⚡
```

#### **Proč je HashSet rychlejší než List?**

```
List<int>.Contains(x):    O(n) - musí projít všechny prvky
HashSet<int>.Contains(x): O(1) - hash tabulka, přímý přístup!

Pro 1 000 000 prvků:
List:    1 000 000 operací ❌
HashSet: 1 operace ✅
```

#### **Použití v algoritmech:**

```csharp
// DFS/BFS - sledování navštívených uzlů
HashSet<Node> navstivene = new HashSet<Node>();

if (navstivene.Contains(uzel)) {    // O(1) ⚡
    // Už jsme tu byli
}

navstivene.Add(uzel);  // O(1) ⚡
```

**Srovnání s List:**
```csharp
// ❌ POMALÉ
List<Node> navstivene = new List<Node>();
if (navstivene.Contains(uzel)) {    // O(n) ❌
    // Musí projít celý list
}

// ✅ RYCHLÉ
HashSet<Node> navstivene = new HashSet<Node>();
if (navstivene.Contains(uzel)) {    // O(1) ✅
    // Okamžité zjištění
}
```

---

<a name="bod-5-příklady-algoritmů"></a>
## 🎯 BOD 5: PŘÍKLADY ALGORITMŮ - HODNĚ!

---

### 🌲 1. DFS (DEPTH-FIRST SEARCH) - Prohledávání do hloubky

**Co to je?**  
Algoritmus pro procházení grafu/stromu, který jde vždy **co nejhlouběji**.

**Používá:** ZÁSOBNÍK (Stack) - LIFO = do hloubky

---

#### **Graf příklad:**

```
        A
       / \
      B   C
     / \   \
    D   E   F
```

**DFS pořadí:** A → C → F → B → E → D  
(Jde hluboce: A→C→F, pak zpět a A→B→E→D)

---

#### **Implementace:**

```csharp
class Node {
    public string Jmeno;
    public List<Node> Sousede;
    
    public Node(string jmeno) {
        Jmeno = jmeno;
        Sousede = new List<Node>();
    }
}

static void DFS(Node start) {
    Stack<Node> zasobnik = new Stack<Node>();      // ZÁSOBNÍK pro DFS
    HashSet<Node> navstivene = new HashSet<Node>();  // O(1) kontrola
    
    zasobnik.Push(start);
    
    while (zasobnik.Count > 0) {
        Node aktualni = zasobnik.Pop();  // LIFO - poslední dovnitř, první ven
        
        // Už jsme tu byli?
        if (navstivene.Contains(aktualni))  // O(1) díky HashSet!
            continue;
        
        // Navštívíme
        navstivene.Add(aktualni);
        Console.WriteLine($"Navštívím: {aktualni.Jmeno}");
        
        // Přidáme sousedy na stack
        foreach (Node soused in aktualni.Sousede) {
            if (!navstivene.Contains(soused)) {
                zasobnik.Push(soused);
            }
        }
    }
}
```

**Kroky pro graf:**

```
Krok 1: Push(A), Stack=[A]
Krok 2: Pop A, navštív A, Push(B,C), Stack=[B,C]
Krok 3: Pop C (LIFO!), navštív C, Push(F), Stack=[B,F]
Krok 4: Pop F, navštív F, Stack=[B]
Krok 5: Pop B, navštív B, Push(D,E), Stack=[D,E]
Krok 6: Pop E, navštív E, Stack=[D]
Krok 7: Pop D, navštív D, Stack=[]

Pořadí: A, C, F, B, E, D
```

**Časová složitost:** **O(V + E)**
- V = Vertices (vrcholy)
- E = Edges (hrany)
- Každý vrchol navštívíme max 1× + projdeme všechny hrany

---

### 🌊 2. BFS (BREADTH-FIRST SEARCH) - Prohledávání do šířky

**Co to je?**  
Algoritmus pro procházení grafu/stromu, který jde **po vrstvách**.

**Používá:** FRONTA (Queue) - FIFO = po vrstvách

---

#### **Graf příklad:**

```
        A          (vrstva 0)
       / \
      B   C        (vrstva 1)
     / \   \
    D   E   F      (vrstva 2)
```

**BFS pořadí:** A → B → C → D → E → F  
(Po vrstvách: vrstva 0: A, vrstva 1: B,C, vrstva 2: D,E,F)

---

#### **Implementace:**

```csharp
static void BFS(Node start) {
    Queue<Node> fronta = new Queue<Node>();          // FRONTA pro BFS
    HashSet<Node> navstivene = new HashSet<Node>();  // O(1) kontrola
    
    fronta.Enqueue(start);
    navstivene.Add(start);  // Přidáme hned, aby se neopakoval
    
    while (fronta.Count > 0) {
        Node aktualni = fronta.Dequeue();  // FIFO - první dovnitř, první ven
        
        Console.WriteLine($"Navštívím: {aktualni.Jmeno}");
        
        // Přidáme sousedy do fronty
        foreach (Node soused in aktualni.Sousede) {
            if (!navstivene.Contains(soused)) {  // O(1)
                navstivene.Add(soused);
                fronta.Enqueue(soused);
            }
        }
    }
}
```

**Kroky pro graf:**

```
Krok 1: Enqueue(A), Fronta=[A]
Krok 2: Dequeue A, navštív A, Enqueue(B,C), Fronta=[B,C]
Krok 3: Dequeue B (FIFO!), navštív B, Enqueue(D,E), Fronta=[C,D,E]
Krok 4: Dequeue C, navštív C, Enqueue(F), Fronta=[D,E,F]
Krok 5: Dequeue D, navštív D, Fronta=[E,F]
Krok 6: Dequeue E, navštív E, Fronta=[F]
Krok 7: Dequeue F, navštív F, Fronta=[]

Pořadí: A, B, C, D, E, F
```

**Časová složitost:** **O(V + E)**

---

### ⚖️ DFS vs BFS - Kdy použít?

```
╔═══════════════════════════════════════════════════╗
║  DFS (ZÁSOBNÍK):                                  ║
║  • Hledání cest                                   ║
║  • Detekce cyklů                                  ║
║  • Topologické třídění                            ║
║  • Labyrinty (prohledej každou cestu)             ║
║  • Backtracking (Sudoku, 8 dam)                   ║
╠═══════════════════════════════════════════════════╣
║  BFS (FRONTA):                                    ║
║  • Nejkratší cesta (neohodnocený graf)            ║
║  • Level-order průchod stromem                    ║
║  • Šíření (virus, požár)                          ║
║  • Sociální sítě (přátelé, přátelé přátel...)     ║
╚═══════════════════════════════════════════════════╝
```

---

### 🔗 3. KONTROLA SPRÁVNĚ UZÁVORKOVANÉHO VÝRAZU

**Problém:** Zjisti, zda jsou závorky správně spárované.

```
"(())"     → true  ✅
"(()"      → false ❌
"())"      → false ❌
"(a+b)*c"  → true  ✅
```

**Používá:** ZÁSOBNÍK (Stack)

---

#### **Algoritmus:**

1. Projdi výraz znak po znaku
2. Otevírací závorka `(` → Push na stack
3. Zavírací závorka `)` → Pop ze stacku (musí tam něco být!)
4. Na konci musí být stack PRÁZDNÝ

---

#### **Implementace:**

```csharp
static bool JsouZavorkySpravne(string vyraz) {
    Stack<char> zasobnik = new Stack<char>();
    
    foreach (char c in vyraz) {
        if (c == '(') {
            zasobnik.Push(c);  // Otevírací → Push
        }
        else if (c == ')') {
            // Zavírací
            if (zasobnik.Count == 0)
                return false;  // Zavírací bez otevírací ❌
            
            zasobnik.Pop();    // Zavírací → Pop
        }
        // Jiné znaky ignorujeme
    }
    
    // Musí být prázdný (všechny závorky spárované)
    return zasobnik.Count == 0;
}
```

**Příklady krok za krokem:**

```
Vstup: "(())"

Krok 1: '(' → Push, Stack=['(']
Krok 2: '(' → Push, Stack=['(','(']
Krok 3: ')' → Pop,  Stack=['(']
Krok 4: ')' → Pop,  Stack=[]
Výsledek: Stack prázdný → true ✅

---

Vstup: "(()"

Krok 1: '(' → Push, Stack=['(']
Krok 2: '(' → Push, Stack=['(','(']
Krok 3: ')' → Pop,  Stack=['(']
Výsledek: Stack není prázdný → false ❌

---

Vstup: "())"

Krok 1: '(' → Push, Stack=['(']
Krok 2: ')' → Pop,  Stack=[]
Krok 3: ')' → Stack je prázdný! → return false ❌
```

---

#### **Rozšíření - více typů závorek:**

```csharp
static bool JsouZavorkySpravne(string vyraz) {
    Stack<char> zasobnik = new Stack<char>();
    
    foreach (char c in vyraz) {
        // Otevírací závorky
        if (c == '(' || c == '[' || c == '{') {
            zasobnik.Push(c);
        }
        // Zavírací závorky
        else if (c == ')' || c == ']' || c == '}') {
            if (zasobnik.Count == 0)
                return false;
            
            char otev = zasobnik.Pop();
            
            // Kontrola správné dvojice
            if (c == ')' && otev != '(') return false;
            if (c == ']' && otev != '[') return false;
            if (c == '}' && otev != '{') return false;
        }
    }
    
    return zasobnik.Count == 0;
}
```

**Příklady:**
```
"{[()]}"    → true  ✅
"{[(])}"    → false ❌ (] a ( nesedí)
"[({})]"    → true  ✅
```

---

### 🧮 4. VYHODNOCENÍ POSTFIX VÝRAZU

**Co je postfix?**

```
Infix (běžný):    2 + 3 * 5        = 2 + 15 = 17
Postfix:          2 3 5 * +        = 17

Proč postfix?
✅ Žádné závorky
✅ Jednoznačné pořadí operací
✅ Snadné vyhodnocení pomocí stacku
```

**Používá:** ZÁSOBNÍK (Stack)

---

#### **Algoritmus:**

1. Projdi výraz zleva doprava
2. Číslo → Push na stack
3. Operátor (+, -, *, /) → Pop 2 čísla, spočítej, Push výsledek

---

#### **Implementace:**

```csharp
static int VyhodnotPostfix(string vyraz) {
    Stack<int> zasobnik = new Stack<int>();
    
    string[] casti = vyraz.Split(' ');  // "2 3 * 5 +" → ["2","3","*","5","+"]
    
    foreach (string cast in casti) {
        // Je to číslo?
        if (int.TryParse(cast, out int cislo)) {
            zasobnik.Push(cislo);
        }
        // Je to operátor?
        else {
            int b = zasobnik.Pop();  // Druhé číslo (vrchol)
            int a = zasobnik.Pop();  // První číslo
            
            int vysledek = 0;
            if (cast == "+") vysledek = a + b;
            if (cast == "-") vysledek = a - b;
            if (cast == "*") vysledek = a * b;
            if (cast == "/") vysledek = a / b;
            
            zasobnik.Push(vysledek);
        }
    }
    
    return zasobnik.Pop();  // Finální výsledek
}
```

**Příklad krok za krokem:**

```
Vstup: "2 3 * 5 +"

Krok 1: "2" → Push(2), Stack=[2]
Krok 2: "3" → Push(3), Stack=[2,3]
Krok 3: "*" → Pop(3), Pop(2), 2*3=6, Push(6), Stack=[6]
Krok 4: "5" → Push(5), Stack=[6,5]
Krok 5: "+" → Pop(5), Pop(6), 6+5=11, Push(11), Stack=[11]
Krok 6: Pop() → Výsledek=11

Odpověď: 11 ✅
```

**Další příklady:**

```
"5 1 2 + 4 * + 3 -"  = 5 + (1+2)*4 - 3 = 5 + 12 - 3 = 14

Krok za krokem:
"5"   → [5]
"1"   → [5,1]
"2"   → [5,1,2]
"+"   → Pop 2,1 → 1+2=3 → [5,3]
"4"   → [5,3,4]
"*"   → Pop 4,3 → 3*4=12 → [5,12]
"+"   → Pop 12,5 → 5+12=17 → [17]
"3"   → [17,3]
"-"   → Pop 3,17 → 17-3=14 → [14]
Pop() → 14
```

---

### 🖨️ 5. TISKÁRNA (FRONTA ÚLOH)

**Problém:** Dokumenty se tisknou v pořadí, jak byly poslány.

**Používá:** FRONTA (Queue) - FIFO

---

#### **Implementace:**

```csharp
class Tiskarna {
    private Queue<string> tiskoveFronta;
    
    public Tiskarna() {
        tiskoveFronta = new Queue<string>();
    }
    
    // Přidej dokument do fronty
    public void PridejDokument(string nazev) {
        tiskoveFronta.Enqueue(nazev);
        Console.WriteLine($"Přidán do fronty: {nazev}");
    }
    
    // Vytiskni další dokument
    public void Tiskni() {
        if (tiskoveFronta.Count == 0) {
            Console.WriteLine("Fronta je prázdná!");
            return;
        }
        
        string dokument = tiskoveFronta.Dequeue();  // FIFO
        Console.WriteLine($"Tisknu: {dokument}");
    }
    
    // Zobraz frontu
    public void ZobrazFrontu() {
        Console.WriteLine("Fronta:");
        foreach (string dok in tiskoveFronta) {
            Console.WriteLine($"  - {dok}");
        }
    }
}

// Použití:
Tiskarna t = new Tiskarna();
t.PridejDokument("Esej.docx");
t.PridejDokument("Foto.jpg");
t.PridejDokument("CV.pdf");

t.ZobrazFrontu();
// Fronta:
//   - Esej.docx
//   - Foto.jpg
//   - CV.pdf

t.Tiskni();  // Tisknu: Esej.docx (první přidaný)
t.Tiskni();  // Tisknu: Foto.jpg
t.Tiskni();  // Tisknu: CV.pdf
```

---

### ↩️ 6. UNDO/REDO V TEXTOVÉM EDITORU

**Problém:** Ctrl+Z vrací poslední akce, Ctrl+Y je vrací zpět.

**Používá:** 2× ZÁSOBNÍK (Stack)

---

#### **Implementace:**

```csharp
class TextEditor {
    private Stack<string> undoStack;
    private Stack<string> redoStack;
    private string aktualniText;
    
    public TextEditor() {
        undoStack = new Stack<string>();
        redoStack = new Stack<string>();
        aktualniText = "";
    }
    
    // Napis text
    public void Pis(string text) {
        undoStack.Push(aktualniText);  // Ulož starý stav
        aktualniText += text;
        redoStack.Clear();  // Redo se vymaže (nová větev)
        
        Console.WriteLine($"Text: {aktualniText}");
    }
    
    // Ctrl+Z
    public void Undo() {
        if (undoStack.Count == 0) {
            Console.WriteLine("Není co vrátit!");
            return;
        }
        
        redoStack.Push(aktualniText);       // Ulož aktuální pro Redo
        aktualniText = undoStack.Pop();     // Vrať se na předchozí
        Console.WriteLine($"Undo → Text: {aktualniText}");
    }
    
    // Ctrl+Y
    public void Redo() {
        if (redoStack.Count == 0) {
            Console.WriteLine("Není co opakovat!");
            return;
        }
        
        undoStack.Push(aktualniText);       // Ulož aktuální pro Undo
        aktualniText = redoStack.Pop();     // Vrať se dopředu
        Console.WriteLine($"Redo → Text: {aktualniText}");
    }
}

// Použití:
TextEditor editor = new TextEditor();
editor.Pis("A");      // Text: A
editor.Pis("B");      // Text: AB
editor.Pis("C");      // Text: ABC

editor.Undo();        // Undo → Text: AB
editor.Undo();        // Undo → Text: A

editor.Redo();        // Redo → Text: AB

editor.Pis("D");      // Text: ABD (nová větev, Redo se smaže)
editor.Redo();        // Není co opakovat!
```

---

### 🌐 7. HISTORIE PROHLÍŽEČE (TLAČÍTKO ZPĚT)

**Problém:** Zpět vrací poslední navštívené stránky.

**Používá:** ZÁSOBNÍK (Stack)

---

#### **Implementace:**

```csharp
class Prohlizec {
    private Stack<string> historie;
    private string aktualni;
    
    public Prohlizec() {
        historie = new Stack<string>();
        aktualni = "";
    }
    
    // Navštiv stránku
    public void NavstivStranku(string url) {
        if (aktualni != "") {
            historie.Push(aktualni);  // Ulož předchozí
        }
        aktualni = url;
        Console.WriteLine($"Navigace → {aktualni}");
    }
    
    // Tlačítko Zpět
    public void Zpet() {
        if (historie.Count == 0) {
            Console.WriteLine("Nelze jít zpět!");
            return;
        }
        
        aktualni = historie.Pop();  // LIFO
        Console.WriteLine($"Zpět → {aktualni}");
    }
}

// Použití:
Prohlizec p = new Prohlizec();
p.NavstivStranku("google.com");      // Navigace → google.com
p.NavstivStranku("youtube.com");     // Navigace → youtube.com
p.NavstivStranku("wikipedia.org");   // Navigace → wikipedia.org

p.Zpet();  // Zpět → youtube.com
p.Zpet();  // Zpět → google.com
p.Zpet();  // Nelze jít zpět!
```

---

### 🎮 8. SLIDING WINDOW MAXIMUM (Pokročilé)

**Problém:** Najdi maximum v každém "okně" velikosti k.

```
Pole: [1, 3, -1, -3, 5, 3, 6, 7], k=3
Okna:
[1, 3, -1] → max = 3
   [3, -1, -3] → max = 3
      [-1, -3, 5] → max = 5
         [-3, 5, 3] → max = 5
            [5, 3, 6] → max = 6
               [3, 6, 7] → max = 7

Výsledek: [3, 3, 5, 5, 6, 7]
```

**Používá:** DEQUE (Double-Ended Queue) - fronta s přístupem z obou stran

*Poznámka: Tohle je pokročilý příklad, na maturitě stačí znát předchozí příklady!*

---

### 🔄 9. FIBONACCI - ITERACE VS REKURZE (Náhled Bodu 6)

**Rekurze (exponenciální O(2^n)):**
```csharp
static int FibRekurze(int n) {
    if (n <= 1) return n;
    return FibRekurze(n-1) + FibRekurze(n-2);  // 2 volání → exponenciální!
}

// Fib(5) → 15 volání funkce!
```

**Iterace se zásobníkem (lineární O(n)):**
```csharp
static int FibZasobnik(int n) {
    if (n <= 1) return n;
    
    Stack<int> stack = new Stack<int>();
    stack.Push(0);  // Fib(0)
    stack.Push(1);  // Fib(1)
    
    for (int i = 2; i <= n; i++) {
        int a = stack.Pop();
        int b = stack.Pop();
        int fib = a + b;
        stack.Push(a);   // Vrátíme a
        stack.Push(fib); // Přidáme nové
    }
    
    return stack.Pop();
}

// Fib(5) → 5 iterací!
```

**Nejlepší - iterace bez stacku (lineární O(n)):**
```csharp
static int FibIterace(int n) {
    if (n <= 1) return n;
    int a = 0, b = 1;
    for (int i = 2; i <= n; i++) {
        int temp = a + b;
        a = b;
        b = temp;
    }
    return b;
}
```

*Detaily v [Bodu 6](#bod-6-nahrazení-rekurze)*

---

<a name="bod-6-nahrazení-rekurze"></a>
## 🔄 BOD 6: NAHRAZENÍ REKURZE ZÁSOBNÍKEM

### 🎯 Proč nahrazovat rekurzi?

1. **Riziko StackOverflow** - příliš hluboká rekurze vyčerpá Call Stack
2. **Větší kontrola** - můžeme kdykoliv zastavit/pokračovat
3. **Úspora paměti** - ruční stack může být menší
4. **Optimalizace** - některé kompilátory neoptimalizují rekurzi

---

### 📚 1. FAKTORIÁL

#### **Rekurze:**

```csharp
static int FaktorialRekurze(int n) {
    if (n <= 1)
        return 1;
    
    return n * FaktorialRekurze(n - 1);
}

// Faktorial(5) = 5 * Faktorial(4)
//              = 5 * 4 * Faktorial(3)
//              = 5 * 4 * 3 * Faktorial(2)
//              = 5 * 4 * 3 * 2 * Faktorial(1)
//              = 5 * 4 * 3 * 2 * 1
//              = 120
```

**Call Stack:**
```
Faktorial(5)
  └─ Faktorial(4)
      └─ Faktorial(3)
          └─ Faktorial(2)
              └─ Faktorial(1)  ← BASE CASE, vrací 1
                  ↑ vrací 2
              ↑ vrací 6
          ↑ vrací 24
      ↑ vrací 120
```

---

#### **Zásobník:**

```csharp
static int FaktorialZasobnik(int n) {
    Stack<int> zasobnik = new Stack<int>();
    
    // Naplň zásobník čísly n...1
    for (int i = n; i >= 1; i--) {
        zasobnik.Push(i);  // [5, 4, 3, 2, 1]
    }
    
    int vysledek = 1;
    while (zasobnik.Count > 0) {
        vysledek *= zasobnik.Pop();  // 1*5*4*3*2*1 = 120
    }
    
    return vysledek;
}
```

**Kroky:**

```
Push: 5, 4, 3, 2, 1 → Stack=[5,4,3,2,1]

Pop 1: vysledek = 1 * 1 = 1
Pop 2: vysledek = 1 * 2 = 2
Pop 3: vysledek = 2 * 3 = 6
Pop 4: vysledek = 6 * 4 = 24
Pop 5: vysledek = 24 * 5 = 120

Výsledek: 120
```

---

### 🌲 2. DFS - REKURZE VS ZÁSOBNÍK

#### **Rekurze:**

```csharp
static void DFS_Rekurze(Node node, HashSet<Node> navstivene) {
    if (navstivene.Contains(node))
        return;
    
    navstivene.Add(node);
    Console.WriteLine($"Navštívím: {node.Jmeno}");
    
    foreach (Node soused in node.Sousede) {
        DFS_Rekurze(soused, navstivene);  // Rekurzivní volání
    }
}

// Použití:
HashSet<Node> navstivene = new HashSet<Node>();
DFS_Rekurze(startNode, navstivene);
```

---

#### **Zásobník:**

```csharp
static void DFS_Zasobnik(Node start) {
    Stack<Node> zasobnik = new Stack<Node>();
    HashSet<Node> navstivene = new HashSet<Node>();
    
    zasobnik.Push(start);
    
    while (zasobnik.Count > 0) {
        Node aktualni = zasobnik.Pop();
        
        if (navstivene.Contains(aktualni))
            continue;
        
        navstivene.Add(aktualni);
        Console.WriteLine($"Navštívím: {aktualni.Jmeno}");
        
        foreach (Node soused in aktualni.Sousede) {
            if (!navstivene.Contains(soused)) {
                zasobnik.Push(soused);
            }
        }
    }
}
```

---

### 🔢 3. VÝPIS ČÍSEL OD N DO 1

#### **Rekurze:**

```csharp
static void VypisCisla_Rekurze(int n) {
    if (n <= 0)
        return;
    
    Console.WriteLine(n);
    VypisCisla_Rekurze(n - 1);
}

// VypisCisla_Rekurze(5):
// 5
// 4
// 3
// 2
// 1
```

---

#### **Zásobník:**

```csharp
static void VypisCisla_Zasobnik(int n) {
    Stack<int> zasobnik = new Stack<int>();
    
    for (int i = 1; i <= n; i++) {
        zasobnik.Push(i);  // [1, 2, 3, 4, 5]
    }
    
    while (zasobnik.Count > 0) {
        Console.WriteLine(zasobnik.Pop());  // LIFO: 5, 4, 3, 2, 1
    }
}
```

---

### 📊 SROVNÁNÍ PAMĚTI

**Faktoriál(5):**

```
REKURZE:
5 stack frames × ~100 bytů = ~500 bytů

ZÁSOBNÍK:
5 int × 4 byty = 20 bytů

ÚSPORA: 25× MÉNĚ PAMĚTI! ✅
```

---

### ✅ KDY POUŽÍT CO?

```
╔═══════════════════════════════════════════════════╗
║  REKURZE:                                         ║
║  ✅ Přirozeně rekurzivní problémy (stromy, DFS)   ║
║  ✅ Kód je jednodušší a čitelnější                ║
║  ✅ Hloubka volání je malá (<1000)                ║
║                                                   ║
║  ❌ Velká hloubka → StackOverflow                 ║
║  ❌ Víc paměti                                    ║
║  ❌ Pomalejší                                     ║
╠═══════════════════════════════════════════════════╣
║  ZÁSOBNÍK:                                        ║
║  ✅ Bezpečnější (bez StackOverflow)               ║
║  ✅ Méně paměti                                   ║
║  ✅ Větší kontrola (můžeš zastavit kdykoli)       ║
║                                                   ║
║  ❌ Delší kód                                     ║
║  ❌ Méně čitelný                                  ║
╚═══════════════════════════════════════════════════╝
```

---

<a name="bod-7-volací-zásobník"></a>
## 🎯 BOD 7: VOLACÍ ZÁSOBNÍK (CALL STACK)

### 📚 Co je Call Stack?

**Call Stack** = speciální zásobník, který **počítač AUTOMATICKY** používá pro správu volání funkcí

**Vlastnosti:**
- LIFO struktura
- Omezená velikost (~1 MB)
- Řídí tok programu
- Spravuje lokální proměnné

---

### 📦 Co obsahuje Stack Frame?

Každé volání funkce vytvoří **Stack Frame** obsahující:

1. **Parametry funkce**
2. **Lokální proměnné**
3. **Návratovou adresu** (kam se vrátit po return)
4. **Technické info** (registry CPU, atd.)

---

### 🎯 Příklad - Jednoduchá funkce

```csharp
static void Main() {
    int x = 5;
    int vysledek = Secti(x, 3);
    Console.WriteLine(vysledek);
}

static int Secti(int a, int b) {
    int suma = a + b;
    return suma;
}
```

**Call Stack kroky:**

```
1. Main() začíná
   ┌──────────────────┐
   │ Main()           │
   │ - x = 5          │
   │ - vysledek = ?   │
   └──────────────────┘

2. Volání Secti(5, 3) → PUSH
   ┌──────────────────┐
   │ Secti(5, 3)      │ ← VRCHOL
   │ - a = 5          │
   │ - b = 3          │
   │ - suma = ?       │
   │ - return adresa  │
   ├──────────────────┤
   │ Main()           │
   │ - x = 5          │
   │ - vysledek = ?   │
   └──────────────────┘

3. Secti počítá
   ┌──────────────────┐
   │ Secti(5, 3)      │
   │ - a = 5          │
   │ - b = 3          │
   │ - suma = 8       │ ← spočítáno
   │ - return adresa  │
   ├──────────────────┤
   │ Main()           │
   │ - x = 5          │
   │ - vysledek = ?   │
   └──────────────────┘

4. return suma → POP
   ┌──────────────────┐
   │ Main()           │
   │ - x = 5          │
   │ - vysledek = 8   │ ← vráceno
   └──────────────────┘

5. Main() končí → Stack prázdný
```

---

### 🔄 Příklad - Rekurze Faktoriál(4)

```csharp
static int Faktorial(int n) {
    if (n <= 1)
        return 1;
    return n * Faktorial(n - 1);
}

// Faktorial(4)
```

**Call Stack kroky:**

```
1. Faktorial(4) → PUSH
   ┌──────────────────┐
   │ Faktorial(4)     │ ← VRCHOL
   │ - n = 4          │
   └──────────────────┘

2. Volá Faktorial(3) → PUSH
   ┌──────────────────┐
   │ Faktorial(3)     │ ← VRCHOL
   │ - n = 3          │
   ├──────────────────┤
   │ Faktorial(4)     │
   │ - n = 4          │
   └──────────────────┘

3. Volá Faktorial(2) → PUSH
   ┌──────────────────┐
   │ Faktorial(2)     │ ← VRCHOL
   │ - n = 2          │
   ├──────────────────┤
   │ Faktorial(3)     │
   │ - n = 3          │
   ├──────────────────┤
   │ Faktorial(4)     │
   │ - n = 4          │
   └──────────────────┘

4. Volá Faktorial(1) → PUSH
   ┌──────────────────┐
   │ Faktorial(1)     │ ← VRCHOL
   │ - n = 1          │
   ├──────────────────┤
   │ Faktorial(2)     │
   │ - n = 2          │
   ├──────────────────┤
   │ Faktorial(3)     │
   │ - n = 3          │
   ├──────────────────┤
   │ Faktorial(4)     │
   │ - n = 4          │
   └──────────────────┘

5. n=1 → BASE CASE → return 1 → POP
   ┌──────────────────┐
   │ Faktorial(2)     │ ← VRCHOL
   │ - n = 2          │
   │ return 2*1 = 2   │
   ├──────────────────┤
   │ Faktorial(3)     │
   │ - n = 3          │
   ├──────────────────┤
   │ Faktorial(4)     │
   │ - n = 4          │
   └──────────────────┘

6. Faktorial(2) vrací 2 → POP
   ┌──────────────────┐
   │ Faktorial(3)     │ ← VRCHOL
   │ - n = 3          │
   │ return 3*2 = 6   │
   ├──────────────────┤
   │ Faktorial(4)     │
   │ - n = 4          │
   └──────────────────┘

7. Faktorial(3) vrací 6 → POP
   ┌──────────────────┐
   │ Faktorial(4)     │ ← VRCHOL
   │ - n = 4          │
   │ return 4*6 = 24  │
   └──────────────────┘

8. Faktorial(4) vrací 24 → POP
   Stack prázdný → Výsledek: 24
```

---

### ⚠️ StackOverflowException

**Co to je?**  
Exception, která nastane, když Call Stack je **plný** (vyčerpán limit ~1 MB)

**Příčiny:**

1. **Chybějící base case:**
```csharp
// ❌ ŠPATNĚ - nikdy neskončí!
static int Faktorial(int n) {
    return n * Faktorial(n - 1);  // Chybí if (n <= 1)
}
```

2. **Base case nikdy nenastane:**
```csharp
// ❌ ŠPATNĚ - n jde do minusu!
static int Faktorial(int n) {
    if (n == 0)  // Base case jen pro n=0
        return 1;
    return n * Faktorial(n - 1);  // n=5,4,3,2,1,0,-1,-2,-3...
}

// ✅ SPRÁVNĚ
static int Faktorial(int n) {
    if (n <= 1)  // Base case pro n≤1
        return 1;
    return n * Faktorial(n - 1);
}
```

3. **Příliš hluboká rekurze:**
```csharp
Faktorial(100000);  // ❌ CRASH! Příliš hluboké
```

**Limit:**  
Call Stack má limit ~**10 000 - 50 000** volání (závisí na systému)

---

### 🔍 Debugging - Call Stack okno

**Visual Studio:** View → Call Stack (Ctrl+Alt+C)

```
Program.exe!Faktorial(int n = 1)   ← AKTUÁLNÍ pozice
Program.exe!Faktorial(int n = 2)
Program.exe!Faktorial(int n = 3)
Program.exe!Faktorial(int n = 4)
Program.exe!Main()
[External Code]
```

**Čte se ODSPODA NAHORU:**
- Main() zavolal Faktorial(4)
- Faktorial(4) zavolal Faktorial(3)
- Faktorial(3) zavolal Faktorial(2)
- Faktorial(2) zavolal Faktorial(1) ← TADY JSME TEĎ

---

### 💾 Stack vs Heap - Rozdíly

```
╔═══════════════════════════════════════════════════╗
║  STACK (Call Stack):                              ║
║  • Automatický (počítač spravuje)                 ║
║  • LIFO struktura                                 ║
║  • Rychlý                                         ║
║  • Malý (~1 MB)                                   ║
║  • Ukládá: lokální proměnné, parametry, adresy    ║
║  • Value types: int, bool, double, struct...      ║
║  • Automaticky vyčištěn po return                 ║
╠═══════════════════════════════════════════════════╣
║  HEAP:                                            ║
║  • Ruční (programátor vytváří objekty pomocí new) ║
║  • Chaotický (žádná struktura)                    ║
║  • Pomalejší                                      ║
║  • Velký (GB)                                     ║
║  • Ukládá: objekty tříd                           ║
║  • Reference types: class, string, array...       ║
║  • Garbage Collector čistí                        ║
╚═══════════════════════════════════════════════════╝
```

**Příklad:**

```csharp
static void Main() {
    int x = 5;              // STACK (value type)
    Student s = new Student();  // s=STACK (reference), 
                                // objekt Student=HEAP
}

class Student {
    public string Jmeno;    // HEAP (součást objektu)
    public int Vek;         // HEAP (součást objektu)
}
```

**Vizualizace:**

```
STACK:
┌──────────────────┐
│ Main()           │
│ - x = 5          │ ← value type
│ - s = 0x1234     │ ← reference (adresa na heap)
└──────────────────┘

HEAP:
┌──────────────────┐
│ Adresa: 0x1234   │ ← objekt Student
│ - Jmeno = "Jan"  │
│ - Vek = 20       │
└──────────────────┘
```

---

<a name="chytáky"></a>
## ⚠️ MATURITNÍ CHYTÁKY - NA CO SI DÁT POZOR

### 1. **Fronta vs Zásobník - Zaměňování FIFO a LIFO**

```
❌ CHYBA: "Fronta je LIFO"
✅ SPRÁVNĚ:
   Fronta (Queue)  = FIFO (First In, First Out)
   Zásobník (Stack) = LIFO (Last In, First Out)
```

**Test:**
```
Přidám: 1, 2, 3, 4, 5

Fronta:  Dequeue() → 1 (první přidané)
Zásobník: Pop() → 5 (poslední přidané)
```

---

### 2. **Peek() neodebírá prvek!**

```csharp
Stack<int> s = new Stack<int>();
s.Push(5);
s.Push(10);

int a = s.Peek();  // a = 10, stack = [5, 10] ✅
int b = s.Pop();   // b = 10, stack = [5]     ✅

// ❌ CHYBA: Myslet si že Peek() odebírá
```

---

### 3. **Pop() / Dequeue() na prázdném → Exception!**

```csharp
Stack<int> s = new Stack<int>();
int x = s.Pop();  // ❌ InvalidOperationException!

// ✅ SPRÁVNĚ - vždy kontroluj Count
if (s.Count > 0) {
    int x = s.Pop();
}
```

---

### 4. **DFS používá Stack, BFS používá Queue**

```
❌ CHYBA: "DFS používá frontu"
✅ SPRÁVNĚ:
   DFS (do hloubky)  → Stack  (LIFO)
   BFS (do šířky)    → Queue  (FIFO)
```

---

### 5. **O(V+E) je SOUČET, ne SOUČIN**

```
Graf: 5 vrcholů, 7 hran

✅ SPRÁVNĚ: O(V+E) = O(5+7) = O(12)
❌ ŠPATNĚ:  O(V×E) = O(5×7) = O(35)

O(V+E) znamená:
- Projdeme každý vrchol 1× = V
- Projdeme každou hranu 1× = E
- Celkem = V + E operací
```

---

### 6. **Generický typ <T> - kdy použít**

```csharp
// ❌ ŠPATNĚ při TVORBĚ vlastní třídy:
class Box {
    public int Hodnota;  // Co když chci string?
}

// ✅ SPRÁVNĚ při tvorbě:
class Box<T> {
    public T Hodnota;
}

// ❌ ŠPATNĚ při použití:
Stack<T> s = new Stack<T>();  // Co je T?

// ✅ SPRÁVNĚ při použití:
Stack<int> s = new Stack<int>();  // Konkrétní typ!
```

---

### 7. **HashSet vs List - rychlost Contains()**

```
❌ CHYBA: Používat List pro kontrolu "už jsem viděl"

List<int>.Contains(x):    O(n) - musí projít všechny
HashSet<int>.Contains(x): O(1) - hash tabulka!

Pro 1 000 000 prvků:
List:    ~1 000 000 operací ❌
HashSet: ~1 operace ✅

✅ V DFS/BFS VŽDY používej HashSet pro navštívené uzly!
```

---

### 8. **Amortizovaná O(1) ≠ vždy O(1)**

```
Push do Stacku/Queue:
- 99% času: O(1) ✅
- 1% času (resize): O(n) ❌
- Průměrně: O(1) ✅

Proto "amortizovaná O(1)"
```

---

### 9. **Rekurze = automatický Call Stack**

```
❌ CHYBA: "Rekurze nepoužívá stack"

✅ SPRÁVNĚ:
Rekurze volá sama sebe
→ Počítač automaticky používá Call Stack
→ Můžeme to nahradit ručním Stack<T>
```

---

### 10. **StackOverflow = příliš hluboká rekurze**

```csharp
// ❌ CRASH!
Faktorial(100000);

// Proč?
Call Stack má limit ~10 000 volání
→ 100 000 volání = přetečení

✅ ŘEŠENÍ:
1. Oprav base case
2. Použij iteraci místo rekurze
3. Použij ruční Stack<T>
```

---

### 11. **Circular buffer (%) pro frontu**

```
❌ CHYBA: Dequeue() posunuje všechny prvky doleva → O(n)

✅ SPRÁVNĚ: Použij circular buffer:
back = (back + 1) % kapacita
front = (front + 1) % kapacita

→ Dequeue() je O(1) ✅
```

---

### 12. **Pořadí Pop v DFS ovlivňuje výsledek**

```
Graf:
    A
   / \
  B   C

// Pokud Push(B, C):
DFS: A → C → B  (C je nahoře, pop první)

// Pokud Push(C, B):
DFS: A → B → C  (B je nahoře, pop první)

Oba správně! Jen jiné pořadí.
```

---

<a name="senior-tipy"></a>
## 🚀 SENIOR TIPY - PRO PRAXI

### 💡 1. V praxi se Queue a Stack používají VŠUDE

**Real-world příklady:**

#### **Webový server - Request Queue:**
```
Příchozí požadavky → Fronta (FIFO)
První request → První zpracován

Queue<HttpRequest> requests = new Queue<HttpRequest>();
```

#### **Thread Pool - Task Queue:**
```
Úlohy čekají ve frontě
Vlákna je berou v pořadí přidání

Queue<Task> taskQueue = new Queue<Task>();
```

#### **Call Center - Waiting Queue:**
```
Volající čekají ve frontě
První volající → První spojen s operátorem
```

---

### ⚡ 2. Concurrent collections pro multithreading

```csharp
// ❌ NEBEZPEČNÉ v multithreadingu:
Queue<int> q = new Queue<int>();

// ✅ BEZPEČNÉ v multithreadingu:
using System.Collections.Concurrent;
ConcurrentQueue<int> q = new ConcurrentQueue<int>();
```

---

### 🎯 3. PriorityQueue pro Dijkstrův algoritmus

```csharp
// .NET 6+
PriorityQueue<Node, int> pq = new PriorityQueue<Node, int>();

pq.Enqueue(nodeA, priorita: 5);   // Nižší priorita = dříve
pq.Enqueue(nodeB, priorita: 2);
pq.Enqueue(nodeC, priorita: 8);

Node next = pq.Dequeue();  // nodeB (nejnižší priorita)
```

**Použití:**
- Dijkstra (nejkratší cesta)
- A* (pathfinding)
- Huffman coding
- Task scheduling

---

### 📊 4. LINQ - elegantní zpracování kolekcí

```csharp
Stack<int> cisla = new Stack<int>();
cisla.Push(1);
cisla.Push(2);
cisla.Push(3);

// Převod na List (zachová pořadí stacku)
List<int> list = cisla.ToList();  // [3, 2, 1]

// Převod na Array
int[] pole = cisla.ToArray();     // [3, 2, 1]

// Filtrování
var suda = cisla.Where(x => x % 2 == 0).ToList();  // [2]

// Suma
int suma = cisla.Sum();  // 6
```

---

### 🎮 5. Producer-Consumer pattern

```csharp
class ProducerConsumer {
    private Queue<int> fronta = new Queue<int>();
    private object zamek = new object();
    
    // Producer - přidává úlohy
    public void Produce(int data) {
        lock (zamek) {
            fronta.Enqueue(data);
        }
    }
    
    // Consumer - zpracovává úlohy
    public void Consume() {
        lock (zamek) {
            if (fronta.Count > 0) {
                int data = fronta.Dequeue();
                // Zpracuj data...
            }
        }
    }
}
```

---

### 🔥 6. Stack pro detekci cyklů v grafu

```csharp
static bool MaCyklus(Node start) {
    Stack<Node> zasobnik = new Stack<Node>();
    HashSet<Node> navstivene = new HashSet<Node>();
    HashSet<Node> aktualne = new HashSet<Node>();  // Na stacku
    
    zasobnik.Push(start);
    
    while (zasobnik.Count > 0) {
        Node node = zasobnik.Peek();  // Peek, ne Pop!
        
        if (!navstivene.Contains(node)) {
            navstivene.Add(node);
            aktualne.Add(node);
            
            foreach (Node soused in node.Sousede) {
                if (aktualne.Contains(soused))
                    return true;  // CYKLUS! ✅
                
                if (!navstivene.Contains(soused))
                    zasobnik.Push(soused);
            }
        } else {
            aktualne.Remove(node);
            zasobnik.Pop();
        }
    }
    
    return false;
}
```

---

### 💾 7. Paměťové srovnání

```
Pro 1 000 000 prvků:

List<int>:       4 MB (+ resize overhead)
HashSet<int>:    ~8 MB (hash tabulka)
Stack<int>:      4 MB (+ resize overhead)
Queue<int>:      4 MB (+ resize overhead)

LinkedList<int>: ~24 MB (8 byte reference × 2 + 4 byte int)

LinkedList zabírá 6× VÍCE paměti než Stack/Queue!
Ale nemá resize operace → stabilní O(1)
```

---

### 🎯 8. Kdy použít co - Quick reference

```
╔═══════════════════════════════════════════════════╗
║  Stack:                                           ║
║  • DFS (prohledávání grafu do hloubky)            ║
║  • Undo/Redo                                      ║
║  • Kontrola závorek                               ║
║  • Postfix vyhodnocení                            ║
║  • Historie prohlížeče (Zpět)                     ║
║  • Rekurze → iterace                              ║
╠═══════════════════════════════════════════════════╣
║  Queue:                                           ║
║  • BFS (prohledávání grafu do šířky)              ║
║  • Tiskárna (fronta úloh)                         ║
║  • Producer-Consumer                              ║
║  • Level-order průchod stromem                    ║
║  • Simulace front (obchod, call centrum)          ║
╠═══════════════════════════════════════════════════╣
║  HashSet:                                         ║
║  • Navštívené uzly v DFS/BFS                      ║
║  • Odstranění duplikátů                           ║
║  • Rychlá kontrola "už jsem viděl" (O(1))         ║
╠═══════════════════════════════════════════════════╣
║  PriorityQueue:                                   ║
║  • Dijkstra (nejkratší cesta)                     ║
║  • A* pathfinding                                 ║
║  • Task scheduling dle priorit                    ║
╚═══════════════════════════════════════════════════╝
```

---

### 🏆 9. Performance tipy

1. **Nastavení počáteční kapacity:**
```csharp
// ❌ Pomalé - resize víckrát
Stack<int> s = new Stack<int>();
for (int i = 0; i < 10000; i++) s.Push(i);

// ✅ Rychlé - resize jen jednou
Stack<int> s = new Stack<int>(10000);  // Počáteční kapacita
for (int i = 0; i < 10000; i++) s.Push(i);
```

2. **HashSet místo List pro Contains:**
```csharp
// ❌ POMALÉ O(n)
List<int> navstivene = new List<int>();
if (navstivene.Contains(x)) { ... }

// ✅ RYCHLÉ O(1)
HashSet<int> navstivene = new HashSet<int>();
if (navstivene.Contains(x)) { ... }
```

3. **StringBuilder pro string concatenation:**
```csharp
// ❌ POMALÉ O(n²)
string s = "";
for (int i = 0; i < 10000; i++) {
    s += i.ToString();  // Vytvoří nový string pokaždé!
}

// ✅ RYCHLÉ O(n)
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 10000; i++) {
    sb.Append(i);
}
string s = sb.ToString();
```

---

## 📚 ZÁVĚR

**Gratuluju! Zvládl jsi kompletně Otázku 3!** 🎉

**Co teď umíš:**
- ✅ Fronta (Queue) - FIFO
- ✅ Zásobník (Stack) - LIFO
- ✅ Časová složitost (O(1), O(n), amortizovaná)
- ✅ Implementace (pole vs spojový seznam)
- ✅ C# reprezentace (Queue<T>, Stack<T>, HashSet<T>)
- ✅ HODNĚ algoritmů (DFS, BFS, závorky, postfix, tiskárna, Undo/Redo, ...)
- ✅ Nahrazení rekurze zásobníkem
- ✅ Call Stack a StackOverflow

---

**📅 Další kroky:**
1. Opakuj si tyto zápisky před maturitou
2. Procvič praktické příklady
3. Zkus si napsat vlastní DFS/BFS
4. Simuluj maturitní úlohu

**🎯 Pro maturitu si zapamatuj:**
- DFS = Stack (LIFO), BFS = Queue (FIFO)
- O(V+E) je SOUČET, ne součin
- HashSet pro "už jsem viděl" → O(1)
- Base case v rekurzi!
- Amortizovaná O(1) = většinou O(1), občas O(n)

---

**💾 Ulož si tyto zápisky a hodně štěstí na maturitě!** 🍀

*Vytvořeno: 3. ledna 2025*  
*Verze: FINAL - Kompletní s příklady*
