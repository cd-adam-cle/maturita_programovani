# Zápisky: Otázka č. 16 - Aritmetické výrazy – reprezentace v grafu, vyhodnocení

**Datum:** 2026-05-10
**Status:** Hotovo

---

## Checklist bodů otázky

- [x] Bod 1: Různé reprezentace aritmetických výrazů (infix, postfix, prefix, binární strom)
- [x] Bod 2: Algoritmus vyhodnocení výrazu v infixu (Shunting-yard + zásobník)
- [x] Bod 3: Algoritmus vyhodnocení výrazu v postfixu (zásobník)
- [x] Bod 4: Algoritmus vyhodnocení výrazu v prefixu (zásobník/rekurze)
- [x] Bod 5: Algoritmus vyhodnocení výrazu v binárním stromě (post-order rekurze)
- [x] Bod 6: Převod binární strom → infix / prefix / postfix
- [x] Bod 7: Převod postfix → binární strom (zásobník)
- [x] Bod 8: Převod infix → postfix (Shunting-yard)

---

## Úvod a motivace

Aritmetický výraz je posloupnost **operandů** (čísla, proměnné) a **operátorů** (+, −, *, /, ^), která definuje výpočet. Problém zní triviálně, ale skrývá tři důležité momenty z teorie programovacích jazyků:

1. **Reprezentace** - jak výraz zapsat tak, aby ho mohl jednoznačně vyhodnotit stroj.
2. **Parsování** - jak ze vstupního řetězce (`"(3+4)*5"`) vytvořit interní reprezentaci (strom, postfix).
3. **Vyhodnocení (interpretace)** - jak z interní reprezentace získat výsledek.

Tato látka tvoří jádro **kompilátorů a interpretů**. Stejný princip se objevuje v:
- překladačích (C#, Java, Python) - výraz → AST (Abstract Syntax Tree)
- bajtkódových virtuálních strojích (JVM, CLR) - vykonávání postfixu nad zásobníkem
- vědeckých kalkulačkách (HP) - vstup přímo v RPN
- tabulkových procesorech (Excel) - parsování vzorců
- databázových engine (SQL where) - logické výrazy ve stromě
- regulárních výrazech - regex je vlastně výraz se speciální syntaxí.

**Centrální idea:** lidé píšou výrazy v **infixu** (`a + b`), protože je to čitelné, ale infix vyžaduje **priority**, **asociativitu** a **závorky**. Stroje preferují **bezzávorkové** reprezentace (postfix, prefix, strom), které jsou jednoznačné a vyhodnotitelné v jednom průchodu zásobníkem nebo rekurzí.

---

## Klíčové koncepty

---

### Bod 1: Reprezentace aritmetických výrazů

**Teorie:**

Aritmetický výraz lze zapsat různými způsoby. Liší se pozicí **operátoru** vůči **operandům**:

| Notace | Pozice operátoru | Příklad | Potřebuje závorky? |
|--------|------------------|---------|--------------------|
| **Infix** | Mezi operandy | `(3 + 4) * 5` | Ano (priorita) |
| **Prefix** (polská) | Před operandy | `* + 3 4 5` | Ne |
| **Postfix** (reverzní polská, RPN) | Za operandy | `3 4 + 5 *` | Ne |
| **Binární strom** | Vnitřní uzel | viz níže | Ne (struktura sama drží prioritu) |

**Pojmy:**
- **Operand** = hodnota / proměnná (listy stromu)
- **Operátor** = funkce nad operandy (`+`, `-`, `*`, `/`, `^`) (vnitřní uzly stromu)
- **Arita** = počet operandů (binární operátor = 2, unární = 1, ternární = 3 jako `a ? b : c`)
- **Priorita (precedence)** = pořadí, ve kterém se operátory vyhodnocují (`*` se vyhodnotí dřív než `+`)
- **Asociativita** = pravidlo pro řazení operátorů stejné priority (zleva, zprava)

**Historický kontext:**
- **Polská notace** (prefix) - polský matematik **Jan Łukasiewicz** (1920) navrhl zápis bez závorek pro logiku.
- **Reverzní polská notace** (postfix) - australský filozof **Charles Hamblin** (1962). Použita v překladačích a HP kalkulačkách (HP-35, 1972).
- **Shunting-yard** - **Edsger Dijkstra** (1961), pojmenoval algoritmus podle železničního "seřaďovacího nádraží".

---

**ASCII vizualizace - výraz `(3 + 4) * 5`:**

```
INFIX:                  ( 3  +  4 )  *  5

PREFIX:                  *  +  3  4  5

POSTFIX:                 3  4  +  5  *

BINÁRNÍ STROM:
                          *
                         / \
                        +   5
                       / \
                      3   4
```

**Klíčový poznatek:** prefix/postfix/strom **NEPOTŘEBUJÍ závorky**, protože jejich struktura jednoznačně určuje pořadí vyhodnocení. Infix je lidsky čitelný, ale stroj ho převádí na jednu z bezzávorkových reprezentací.

---

**Vlastnosti binárního stromu výrazu:**

| Pozice v stromu | Co tam je |
|-----------------|-----------|
| **Listy** | Operandy (čísla, proměnné) |
| **Vnitřní uzly** | Operátory |
| **Kořen** | Operátor s nejnižší prioritou (vyhodnotí se poslední) |

**Klíčový invariant:** strom výrazu je vždy **plný binární strom** - každý vnitřní uzel má právě 2 potomky (pro binární operátory), listy jsou operandy. Počet listů = počet operandů, počet vnitřních uzlů = počet operátorů.

---

**Vizualizace pro složitější výraz `3 + 4 * 5 - 6 / 2`:**

```
INFIX:    3 + 4 * 5 - 6 / 2

POSTFIX:  3 4 5 * + 6 2 / -

PREFIX:   - + 3 * 4 5 / 6 2

STROM:
                  -
                 / \
                +   /
               / \  /\
              3   * 6 2
                 / \
                4   5
```

**Pravidlo priorit při stavbě stromu:** operátor s **nejnižší prioritou** (vyhodnotí se naposledy) jde do **kořene**, operátory s vyšší prioritou se zanoří hlouběji.

**Proč to platí:** při post-order průchodu se kořen vyhodnotí jako poslední. Pokud má kořen nejnižší prioritu, pak operátory s vyšší prioritou (které mají být provedeny dřív) skončí níže ve stromě a jsou vyhodnoceny dřív.

---

### Bod 2: Vyhodnocení výrazu v INFIXU

**Teorie:**

Vyhodnocení infixu **přímo** je složité, protože musíme:
1. Sledovat **prioritu** operátorů (`*` > `+`)
2. Sledovat **asociativitu** (zleva doprava: `8 - 3 - 1 = 4`, ne `6`)
3. Respektovat **závorky**

**Standardní postup:** infix → převedu na postfix (Shunting-yard, viz Bod 8) → vyhodnotím postfix (Bod 3).

Alternativa: infix → strom (parser typu **recursive descent** nebo **Pratt parser**) → vyhodnotím rekurzí. Toto je přístup používaný v reálných kompilátorech.

**Priorita operátorů:**

| Priorita | Operátory | Asociativita |
|----------|-----------|--------------|
| Nejvyšší | `^` (mocnina) | Zprava doleva |
| Střední | `*`, `/`, `%` | Zleva doprava |
| Nejnižší | `+`, `-` | Zleva doprava |

V plnokrevných jazycích (C#, C++) existuje **15-20 úrovní priority** (logické, bitové, porovnávací, přiřazovací operátory). Pro maturitu stačí čtyři aritmetické.

---

**Recursive descent parser pro infix (idea):**

```
Vyraz()  = Term  { ('+' | '-') Term }       <- nejnižší priorita zde
Term()   = Faktor { ('*' | '/') Faktor }    <- střední priorita
Faktor() = Cislo | '(' Vyraz ')' | '-' Faktor
```

Každá úroveň priority je samostatná funkce. Vyšší priorita = hlouběji v rekurzi. Zaručuje, že `*` se vyhodnotí dřív než `+`, protože jeho uzel skončí hlouběji ve stromě.

**Pratt parser** je elegantnější varianta používající **binding power** (levou a pravou váhu operátoru). Používá ho např. interpret Pythonu.

---

### Bod 3: Vyhodnocení výrazu v POSTFIXU (RPN)

**Teorie:**

Postfix se vyhodnocuje pomocí **zásobníku** v jediném průchodu zleva doprava. Žádné priority, žádné závorky. Tato jednoduchost je důvod, proč ho používají virtuální stroje (JVM, CLR) a HP kalkulačky.

**Invariant zásobníku po každém kroku:** zásobník obsahuje všechny zatím nezpracované **mezivýsledky**. Po zpracování operátoru se dva vrcholy pop a nahradí jejich výsledkem.

**Algoritmus:**

```
Pro každý token (zleva doprava):
    JE-LI token OPERAND:
        Push na zásobník
    JE-LI token OPERÁTOR:
        b = Pop()      <- druhý operand (POZOR - pořadí!)
        a = Pop()      <- první operand
        výsledek = a OP b
        Push(výsledek)

Na konci: na zásobníku zůstane JEDEN prvek = výsledek výrazu.
```

**Vizualizace pro `3 4 + 5 *`:**

```
Token   Zásobník        Akce
---------------------------------------
 3      [3]             push
 4      [3, 4]          push
 +      [7]             pop 4, pop 3, push (3+4)
 5      [7, 5]          push
 *      [35]            pop 5, pop 7, push (7*5)
                        ^^^ konec -> výsledek = 35
```

**Vizualizace pro `3 4 5 * + 6 2 / -` (= 3 + 4*5 − 6/2 = 20):**

```
Token   Zásobník
-----------------
 3      [3]
 4      [3, 4]
 5      [3, 4, 5]
 *      [3, 20]        4*5
 +      [23]           3+20
 6      [23, 6]
 2      [23, 6, 2]
 /      [23, 3]        6/2
 -      [20]           23-3 -> VÝSLEDEK
```

**Pozor na pořadí operandů u nekomutativních operátorů (`-`, `/`):**
- `5 3 -` → pop b=3, pop a=5 → výsledek = a-b = **5-3 = 2**
- (Kdo prohodí, dostane −2 a má chybu.)

**Časová složitost:** O(n), kde n = počet tokenů.
**Paměťová složitost:** O(n) v nejhorším případě (zásobník) - např. `1 2 3 4 5 6 + + + + +` má hluboký zásobník před prvním operátorem.

**Kód - vyhodnocení postfixu:**

```csharp
static int VyhodnotPostfix(string vyraz)
{
    Stack<int> stack = new Stack<int>();
    string[] tokeny = vyraz.Split(' ');

    foreach (string token in tokeny)
    {
        if (int.TryParse(token, out int cislo))
        {
            stack.Push(cislo);
        }
        else
        {
            int b = stack.Pop();   // POZOR: druhý operand první!
            int a = stack.Pop();

            switch (token)
            {
                case "+": stack.Push(a + b); break;
                case "-": stack.Push(a - b); break;
                case "*": stack.Push(a * b); break;
                case "/": stack.Push(a / b); break;
            }
        }
    }
    return stack.Pop();   // jediný zbývající prvek = výsledek
}

// Použití:
// VyhodnotPostfix("3 4 + 5 *") -> 35
```

**Validace postfixu:** korektní postfix musí splnit:
- Na konci jediný prvek na zásobníku.
- Při každém operátoru musí na zásobníku být alespoň 2 prvky.
- Pro výraz s n operandy a k binárními operátory platí n = k + 1.

---

### Bod 4: Vyhodnocení výrazu v PREFIXU

**Teorie:**

Prefix se vyhodnocuje **zprava doleva** pomocí zásobníku (zrcadlově k postfixu). Alternativně jde **zleva doprava rekurzivně** - operátor "spotřebuje" další dvě hodnoty z proudu.

**Algoritmus (zprava doleva):**

```
Pro každý token ZPRAVA DOLEVA:
    JE-LI token OPERAND:
        Push
    JE-LI token OPERÁTOR:
        a = Pop()     <- v prefixu se OPERANDY čtou OPAČNĚ
        b = Pop()
        výsledek = a OP b
        Push(výsledek)
```

**Vizualizace pro `* + 3 4 5`:**

```
Čtu zprava doleva: 5, 4, 3, +, *

Token   Zásobník        Akce
-------------------------------------
 5      [5]             push
 4      [5, 4]          push
 3      [5, 4, 3]       push
 +      [5, 7]          pop a=3, pop b=4, push (3+4)
 *      [35]            pop a=7, pop b=5, push (7*5)
                        ^^^ konec -> výsledek = 35
```

**Alternativa - rekurzivně zleva doprava:**

```
Vyhodnot(tokens, ukazatel):
    token = tokens[ukazatel++]
    if token je OPERAND:
        return double.Parse(token)
    else (operátor):
        a = Vyhodnot(tokens, ukazatel)   <- rekurze pro levý podstrom
        b = Vyhodnot(tokens, ukazatel)   <- rekurze pro pravý podstrom
        return a OP b
```

```csharp
static int pos;
static double VyhodnotPrefix(string[] tokeny)
{
    string t = tokeny[pos++];
    if (double.TryParse(t, out double v))
        return v;
    double a = VyhodnotPrefix(tokeny);
    double b = VyhodnotPrefix(tokeny);
    return t switch
    {
        "+" => a + b,
        "-" => a - b,
        "*" => a * b,
        "/" => a / b,
        _   => throw new InvalidOperationException()
    };
}
```

Tato rekurzivní varianta vlastně **buduje a vyhodnocuje strom najednou**, aniž by ho explicitně držela v paměti.

**Časová složitost:** O(n).
**Paměťová složitost:** O(n) (zásobník nebo rekurze).

**Použití prefixu v praxi:** programovací jazyky rodiny **LISP** (Scheme, Clojure, Racket). `(+ 1 2 3)` je prefix se podporou variabilní arity. Důsledek: v LISPu se výrazy a kód píší ve stejné syntaxi - tomu se říká **homoiconicita** a umožňuje to mocná makra.

---

### Bod 5: Vyhodnocení výrazu v BINÁRNÍM STROMĚ

**Teorie:**

Strom se vyhodnocuje **rekurzivně post-order** (nejprve levý podstrom, pak pravý, pak operátor v kořeni). Listy = okamžitě vrátí svou hodnotu.

Toto je **standardní postup ve skutečných interpretech a kompilátorech**. AST (Abstract Syntax Tree) se prochází rekurzivně, listy vracejí konkrétní hodnoty a vnitřní uzly aplikují operátory.

**Algoritmus:**

```
Vyhodnot(uzel):
    if uzel je LIST:
        return uzel.Hodnota

    levy   = Vyhodnot(uzel.Levy)     <- rekurze
    pravy  = Vyhodnot(uzel.Pravy)    <- rekurze
    return aplikujOperator(uzel.Op, levy, pravy)
```

**Vizualizace pro strom `(3+4)*5`:**

```
              *                    1) Vyhodnot(*)
             / \                       potřebuje vyhodnotit levý a pravý
            +   5
           / \
          3   4

Krok 1: Vyhodnot(*)   -> potřebuju levý a pravý
Krok 2: Vyhodnot(+)   -> potřebuju levý a pravý
Krok 3: Vyhodnot(3)   -> return 3 (list)
Krok 4: Vyhodnot(4)   -> return 4 (list)
Krok 5: Návrat do (+) -> 3 + 4 = 7
Krok 6: Vyhodnot(5)   -> return 5 (list)
Krok 7: Návrat do (*) -> 7 * 5 = 35  <- VÝSLEDEK
```

**Časová složitost:** O(n) - navštívím každý uzel právě jednou.
**Paměťová složitost:** O(h), kde h = výška stromu (zásobník rekurze). Pro vyvážený strom O(log n), pro degradovaný O(n).

**Kód - třída uzlu + vyhodnocení:**

```csharp
public class Uzel
{
    public string Hodnota { get; set; }   // číslo NEBO operátor
    public Uzel Levy { get; set; }
    public Uzel Pravy { get; set; }
}

static double Vyhodnot(Uzel u)
{
    // BASE CASE: list = operand
    if (u.Levy == null && u.Pravy == null)
        return double.Parse(u.Hodnota);

    // REKURZE: vyhodnoť levý a pravý podstrom
    double levy = Vyhodnot(u.Levy);
    double pravy = Vyhodnot(u.Pravy);

    return u.Hodnota switch
    {
        "+" => levy + pravy,
        "-" => levy - pravy,
        "*" => levy * pravy,
        "/" => levy / pravy,
        _   => throw new InvalidOperationException("Neznámý operátor")
    };
}
```

**Iterativní vyhodnocení stromu** (bez rekurze): pomocí dvou zásobníků nebo Morrisova průchodu. V praxi se však používá rekurze, protože je čitelnější a hloubka stromu je obvykle malá.

**Optimalizace - constant folding:** pokud jsou oba podstromy listy s čísly, můžeme strom **přepsat** na list s předpočítanou hodnotou. Toto dělá kompilátor v době překladu (`3 + 4` v kódu se přeloží přímo jako `7`).

---

### Bod 6: Převod BINÁRNÍ STROM → infix / prefix / postfix

**Teorie:**

Tři způsoby průchodu = tři reprezentace. Stačí **rekurzivně procházet strom v jiném pořadí**.

| Notace | Průchod | Pořadí |
|--------|---------|--------|
| **Prefix** | Pre-order | KOŘEN, levý, pravý |
| **Infix** | In-order | levý, KOŘEN, pravý |
| **Postfix** | Post-order | levý, pravý, KOŘEN |

**Mnemo:** "pre" = "před" (kořen před dětmi), "in" = "uprostřed" (kořen mezi dětmi), "post" = "po" (kořen po dětech). Pozice slova vzhledem k pořadí návštěvy potomků.

**Algoritmus - pre-order (prefix):**
```
PreOrder(uzel):
    if uzel == null: return
    Vypis(uzel.Hodnota)         <- KOŘEN
    PreOrder(uzel.Levy)
    PreOrder(uzel.Pravy)
```

**Algoritmus - in-order (infix):**
```
InOrder(uzel):
    if uzel == null: return
    if uzel není LIST: Vypis("(")    <- závorky kolem podvýrazu
    InOrder(uzel.Levy)
    Vypis(uzel.Hodnota)              <- KOŘEN
    InOrder(uzel.Pravy)
    if uzel není LIST: Vypis(")")
```

**Pozor:** in-order MUSÍ tisknout závorky, jinak ztratíme prioritu! Bez závorek by `* + 3 4 5` skončilo jako `3 + 4 * 5` (nesprávně).

**Inteligentní závorkování:** můžeme **vynechat zbytečné závorky** porovnáním priority rodiče a potomka. Pokud má potomek vyšší prioritu než rodič, závorky nepotřebujeme. Tak se z `((3+4)*5)` stane úspornější `(3+4)*5`.

**Algoritmus - post-order (postfix):**
```
PostOrder(uzel):
    if uzel == null: return
    PostOrder(uzel.Levy)
    PostOrder(uzel.Pravy)
    Vypis(uzel.Hodnota)         <- KOŘEN
```

---

**Vizualizace pro strom `(3+4)*5`:**

```
Strom:
              *
             / \
            +   5
           / \
          3   4

Pre-order  (prefix):  *  +  3  4  5
In-order   (infix):   ((3 + 4) * 5)
Post-order (postfix): 3  4  +  5  *
```

**Časová složitost všech tří průchodů:** O(n).
**Paměťová složitost:** O(h) (rekurze, h = výška stromu).

**Důležité pozorování:** ze samotného **pre-order ani post-order** výpisu **nelze obecně rekonstruovat strom**, pokud nevíme, který uzel je list. U stromu výrazu to ale víme - operandy = listy, operátory = vnitřní uzly. **Z in-order také nelze rekonstruovat strom** bez znalosti priority (in-order pre-order kombinace už strom určuje jednoznačně).

---

### Bod 7: Převod POSTFIX → BINÁRNÍ STROM

**Teorie:**

Stejný princip jako vyhodnocení postfixu, ale místo čísel pushuju **uzly stromu**.

**Algoritmus:**

```
Pro každý token (zleva doprava):
    JE-LI OPERAND:
        Vytvoř LIST(token)
        Push uzel
    JE-LI OPERÁTOR:
        pravy = Pop()           <- druhý operand -> pravý podstrom
        levy  = Pop()           <- první operand -> levý podstrom
        novy  = Uzel(token, levy, pravy)
        Push(novy)

Na konci: na zásobníku jeden uzel = KOŘEN stromu.
```

**Vizualizace pro `3 4 + 5 *`:**

```
Token  Zásobník (uzly)                Strom na vrcholu zásobníku
-------------------------------------------------------------------
 3     [3]                            3
 4     [3, 4]                         4
 +     [(3+4)]                            +
                                         / \
                                        3   4
 5     [(3+4), 5]                      5
 *     [((3+4)*5)]                         *
                                          / \
                                         +   5
                                        / \
                                       3   4
```

**Časová složitost:** O(n).

**Kód:**

```csharp
static Uzel PostfixNaStrom(string[] tokeny)
{
    Stack<Uzel> stack = new Stack<Uzel>();
    foreach (var t in tokeny)
    {
        if (double.TryParse(t, out _))
            stack.Push(new Uzel { Hodnota = t });
        else
        {
            var pravy = stack.Pop();
            var levy  = stack.Pop();
            stack.Push(new Uzel { Hodnota = t, Levy = levy, Pravy = pravy });
        }
    }
    return stack.Pop();
}
```

**Pro převod INFIX → STROM:** nejprve infix → postfix (Shunting-yard, Bod 8), pak postfix → strom. Nebo přímo recursive descent parser, který vytváří strom rovnou při parsování.

---

### Bod 8: Převod INFIX → POSTFIX (Shunting-yard, Dijkstra)

**Teorie:**

Klasický algoritmus **Edsgera Dijkstry** (1961). Pojmenování pochází z analogie se železničním seřaďovacím nádražím - operátory jsou "vagóny", které se dočasně odstaví na boční kolej (zásobník), než dorazí na koncové stanoviště (výstup).

Používá **zásobník operátorů** a **výstupní frontu**.

**Algoritmus:**

```
Pro každý token zleva doprava:
    JE-LI ČÍSLO:
        Pošli do výstupu
    JE-LI OPERÁTOR:
        Dokud je na vrcholu zásobníku operátor s vyšší nebo stejnou
        prioritou (a NENÍ to "(") :
            Pop ze zásobníku -> pošli do výstupu
        Push aktuální operátor na zásobník
    JE-LI "(":
        Push na zásobník
    JE-LI ")":
        Dokud na vrcholu není "(":
            Pop ze zásobníku -> pošli do výstupu
        Pop "(" (zahoď)

Na konci: vše ze zásobníku přesuň do výstupu.
```

**Vizualizace pro `(3 + 4) * 5`:**

```
Token   Výstup            Zásobník       Komentář
----------------------------------------------------------
 (      [ ]               [(]            push
 3      [3]               [(]            číslo -> výstup
 +      [3]               [(, +]         push (po "(" se nevyhazuje)
 4      [3, 4]            [(, +]         číslo -> výstup
 )      [3, 4, +]         [ ]            pop až po "(", "(" zahodit
 *      [3, 4, +]         [*]            push
 5      [3, 4, +, 5]      [*]            číslo -> výstup
 KONEC  [3, 4, +, 5, *]   [ ]            přesypat zbytek

Výsledek (postfix): 3 4 + 5 *
```

**Vizualizace pro `3 + 4 * 5`:**

```
Token   Výstup            Zásobník       Komentář
----------------------------------------------------------
 3      [3]               []
 +      [3]               [+]            push (zásobník prázdný)
 4      [3, 4]            [+]
 *      [3, 4]            [+, *]         priorita * > +, push
 5      [3, 4, 5]         [+, *]
 KONEC  [3, 4, 5, *, +]   []             přesypat: nejprv *, pak +

Výsledek: 3 4 5 * +  (=> 3 + (4*5))
```

**Časová složitost:** O(n) - každý token vstoupí a vystoupí ze zásobníku právě jednou.

**Zpracování asociativity:**
- Pro **levoasociativní** operátory (`+`, `-`, `*`, `/`): vyhazuj operátory **stejné nebo vyšší** priority.
- Pro **pravoasociativní** operátory (`^`): vyhazuj operátory **přísně vyšší** priority.

To zajistí, že `2 ^ 3 ^ 2 = 2^(3^2) = 512`, ne `(2^3)^2 = 64`.

**Kód - Shunting-yard:**

```csharp
static int Priorita(string op) => op switch
{
    "+" or "-" => 1,
    "*" or "/" => 2,
    "^"        => 3,
    _          => 0
};

static bool JeLevoAsoc(string op) => op != "^";

static List<string> ShuntingYard(string[] tokeny)
{
    var vystup = new List<string>();
    var zasobnik = new Stack<string>();

    foreach (var t in tokeny)
    {
        if (double.TryParse(t, out _))
            vystup.Add(t);
        else if (t == "(")
            zasobnik.Push(t);
        else if (t == ")")
        {
            while (zasobnik.Peek() != "(")
                vystup.Add(zasobnik.Pop());
            zasobnik.Pop();  // zahoď "("
        }
        else  // operátor
        {
            while (zasobnik.Count > 0 && zasobnik.Peek() != "(" &&
                   (Priorita(zasobnik.Peek()) > Priorita(t) ||
                    (Priorita(zasobnik.Peek()) == Priorita(t) && JeLevoAsoc(t))))
                vystup.Add(zasobnik.Pop());
            zasobnik.Push(t);
        }
    }
    while (zasobnik.Count > 0)
        vystup.Add(zasobnik.Pop());
    return vystup;
}
```

**Validace vstupu:** Shunting-yard může detekovat chyby:
- Neuzavřená závorka - na konci zůstane "(" na zásobníku.
- Nadbytečná pravá závorka - hledání "(" dojde na prázdný zásobník.
- Chybějící operand - výraz `3 +` skončí s prázdným zásobníkem mezivýsledků.

---

## Srovnávací tabulka reprezentací

| Reprezentace | Lidsky čitelné | Závorky | Jednoznačné | Vyhodnocení | Použití |
|--------------|:--------------:|:-------:|:-----------:|-------------|---------|
| **Infix** | Ano | Nutné | Bez priority ne | Složité (Shunting-yard) | Lidé, matematika, většina jazyků |
| **Prefix** | Ne | Ne | Ano | Zásobník zprava | LISP, Scheme |
| **Postfix** | Ne | Ne | Ano | Zásobník zleva | HP kalkulačky, JVM, .NET CLR |
| **Binární strom** | Ne (datová struktura) | Ne | Ano | Post-order rekurze | Kompilátory (AST), interpretery |

---

## Na co si dát pozor (Maturitní chytáky)

1. **Pořadí operandů u `-` a `/` v postfixu:**
   Prvek na vrcholu zásobníku (Pop jako PRVNÍ) je **pravý operand**, ne levý!
   `5 3 -` → Pop b=3, Pop a=5 → výsledek a−b = **2**, ne −2.

2. **Závorky jen v infixu:**
   Postfix a prefix závorky **nepotřebují**. Strom je drží implicitně ve struktuře.

3. **In-order PROCHÁZENÍ vs in-order S ZÁVORKAMI:**
   Pokud při převodu strom → infix neuvedete závorky, ztratíte informaci o prioritě a výsledek bude chybný.

4. **Operátor v kořeni stromu má NEJNIŽŠÍ prioritu:**
   Vyhodnotí se totiž až jako úplně poslední. `+` v kořeni znamená, že se sčítají dvě (už hotová) podčísla.

5. **Asociativita:**
   `8 - 3 - 1` v infixu = `(8-3)-1 = 4`, postfix `8 3 - 1 -` = 4. Pozor u mocniny `^` - ta je obvykle pravoasociativní (`2^3^2 = 2^(3^2) = 512`).

6. **Shunting-yard po `(` nevyhazuje nic:**
   Když na vrcholu je `(`, žádné operátory nad ním se neodebírají, dokud nepřijde `)`.

7. **Vyhodnocení prefixu jde ZPRAVA DOLEVA**, postfixu ZLEVA DOPRAVA. Snadné si to splést.

8. **Unární minus** je extra problém - `-5 + 3` vs `5 - 3`. Tokenizer musí poznat, zda `-` je binární (mezi dvěma operandy) nebo unární (před operandem nebo po `(`).

9. **Dělení nulou** není detekovatelné parsováním - musí ho odhalit až vyhodnocení (a buď vyhodit výjimku, nebo vrátit NaN).

10. **Tokenizace vs vyhodnocení:** než spustíme jakýkoli algoritmus, musíme řetězec rozdělit na tokeny. `"12+34"` → `["12", "+", "34"]`, ne `["1", "2", "+", "3", "4"]`. Toto je úloha **lexeru**, který se obvykle implementuje pomocí stavového automatu nebo regex.

---

## Rozšíření

1. **AST (Abstract Syntax Tree)** v reálných kompilátorech = obecnější varianta binárního stromu výrazů. Listy nemusí být jen čísla, ale i volání funkce, identifikátory atd. Vnitřní uzly mohou mít proměnnou aritu (např. volání funkce s n argumenty).

2. **Postfix v JVM/CLR:** virtuální stroje Javy a .NETu vykonávají instrukce nad zásobníkem (`iload`, `iadd`...). To je v podstatě postfix.

3. **HP kalkulačky** používaly RPN, protože vyhodnocení je triviální (jeden zásobník) a uživatel nemusí psát závorky. Klávesa Enter "tlačí" číslo na zásobník.

4. **Unární minus** je extra problém - `-5 + 3` vs `5 - 3`. Řeší se buď tokenizerem (rozliší `-5` jako jedno číslo), nebo speciálním unárním operátorem v zásobníku (uses arita 1).

5. **Algoritmus pro převod prefix → strom:** stejný jako postfix → strom, ale **čteme zprava doleva**.

6. **Trojkový operátor `?:`** v C# / Java vyžaduje **ternární uzel** s arity 3. Strom přestane být striktně binární, ale postup vyhodnocení (rekurze post-order s lazy evaluation) zůstává stejný.

7. **Lazy evaluation:** v jazycích jako Haskell se podvýrazy nevyhodnocují, dokud nejsou skutečně potřeba. To znamená, že strom může reprezentovat výpočet, který se nikdy neprovede.

8. **JIT kompilace AST:** moderní interpretery (V8, .NET) AST nepřímo nevyhodnocují, ale **překládají do strojového kódu za běhu** pomocí JIT (Just-In-Time) kompilátoru.

9. **Symbolické výpočty (CAS):** Computer Algebra Systems (Mathematica, SymPy) reprezentují výraz jako strom a manipulují s ním algebraicky - derivace, integrace, zjednodušení.

10. **DAG místo stromu:** pokud výraz obsahuje opakovaně tentýž podvýraz (`(a+b)*(a+b)`), můžeme ho reprezentovat jako **DAG** (acyklický orientovaný graf) a sdílet uzly. Toto je optimalizace zvaná **common subexpression elimination** (CSE).

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 3** (Fronta a zásobník) | Klíčová struktura pro vyhodnocení postfixu i pro Shunting-yard |
| **Ot. 5** (Rekurze) | Vyhodnocení stromu = post-order rekurze |
| **Ot. 9** (Stromy) | Binární strom výrazu je speciální typ binárního stromu |
| **Ot. 14** (BVS) | Strom výrazu vs BVS - liší se invariantem (BVS má uspořádání hodnot) |
| **Ot. 15** (D&C, DP) | Vyhodnocení stromu = D&C - rozděl na podstromy, kombinuj výsledky |
| **Ot. 8** (Reprezentace grafu) | Strom je acyklický souvislý graf |

---

## Klíčová věta pro maturitu

> *"Aritmetický výraz lze reprezentovat čtyřmi způsoby: infixem (lidsky čitelný, ale potřebuje závorky a priority), prefixem a postfixem (bezzávorkové, vyhodnocují se pomocí zásobníku v O(n)) a binárním stromem (vnitřní uzly = operátory, listy = operandy, vyhodnocuje se rekurzivně post-order). Mezi reprezentacemi se převádí pomocí průchodů stromem (pre/in/post-order) nebo Shunting-yard algoritmem."*

---

## KLÍČOVÉ POJMY

1. **Aritmetický výraz** - posloupnost operandů a operátorů definující výpočet.
2. **Operand** - hodnota (číslo, proměnná), listy stromu.
3. **Operátor** - funkce nad operandy (+, −, *, /, ^), vnitřní uzly stromu.
4. **Arita** - počet operandů operátoru (unární 1, binární 2, ternární 3).
5. **Priorita (precedence)** - pořadí vyhodnocení operátorů různé úrovně.
6. **Asociativita** - pravidlo pro operátory stejné priority (levo/pravo).
7. **Infix** - operátor mezi operandy (`a + b`), potřebuje závorky a priority.
8. **Prefix** - operátor před operandy (`+ a b`), polská notace (Łukasiewicz 1920).
9. **Postfix (RPN)** - operátor za operandy (`a b +`), reverzní polská notace (Hamblin 1962).
10. **Binární strom výrazu** - vnitřní uzly operátory, listy operandy, kořen = nejnižší priorita.
11. **AST (Abstract Syntax Tree)** - obecnější strom v kompilátorech, listy mohou být i volání funkcí.
12. **Plný binární strom** - vlastnost stromu výrazu: každý vnitřní uzel má 2 potomky.
13. **Shunting-yard** - Dijkstrův algoritmus pro převod infix → postfix v O(n).
14. **Recursive descent parser** - rekurzivní parser, kde vyšší priorita = hlubší rekurze.
15. **Pratt parser** - elegantní parser s binding power (např. Python).
16. **Pre-order průchod** - kořen, levý, pravý → vytváří prefix.
17. **In-order průchod** - levý, kořen, pravý → vytváří infix (se závorkami).
18. **Post-order průchod** - levý, pravý, kořen → vytváří postfix.
19. **Vyhodnocení postfixu** - jeden průchod zleva doprava se zásobníkem, O(n).
20. **Vyhodnocení prefixu** - průchod zprava doleva se zásobníkem, nebo rekurzivně zleva.
21. **Vyhodnocení stromu** - post-order rekurze, O(n) čas, O(h) paměť.
22. **Constant folding** - kompilátorová optimalizace - statické předpočítání konstantních podvýrazů.
23. **Tokenizace (lexer)** - rozdělení vstupního řetězce na tokeny před parsováním.
24. **Unární minus** - speciální případ, který tokenizer/parser musí rozlišit od binárního.
25. **Homoiconicita** - vlastnost LISPu, kde kód i data mají stejnou syntaxi (prefix).
26. **JIT kompilace** - překlad AST do strojového kódu za běhu (V8, .NET CLR).
27. **DAG (acyklický graf)** - sdílení podvýrazů, optimalizace CSE.
28. **CSE (common subexpression elimination)** - eliminace opakovaných podvýrazů.
29. **Lazy evaluation** - vyhodnocení jen při potřebě (Haskell).
30. **Validace výrazu** - kontrola závorek, arity, sémantiky (dělení nulou).

---

*Vytvořeno: 2026-05-10 - Maturitní příprava PRG 2025/2026*
