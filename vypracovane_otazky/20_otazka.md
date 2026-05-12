# Zápisky: Otázka č. 20 - Programování řízené událostmi. Okenní aplikace.

**Datum:** 2026-05-10
**Status:** Hotovo

---

## Checklist bodů otázky

- [x] Bod 1: Základní princip fungování interaktivních aplikací, role událostí
- [x] Bod 2: Průběh zpracování události
- [x] Bod 3: Příklady základních událostí při vytváření okenní aplikace
- [x] Bod 4: Základní pravidla při vytváření a programování okenních aplikací
- [x] Bod 5: Responzivní vzhled aplikace
- [x] Bod 6: MVVM pattern

---

## Klíčové koncepty & Snippety

---

### Bod 1: Princip interaktivních aplikací, role událostí

**Teorie:**

**Programování řízené událostmi (event-driven programming)** je paradigma, ve kterém běh programu **není určen lineárním tokem instrukcí**, ale **událostmi**, které vznikají typicky:

- **Od uživatele** – stisk klávesy, kliknutí myší, dotyk na obrazovce
- **Od systému** – timer, příchozí síťová zpráva, vykreslení okna
- **Z jiných komponent** – jeden ovládací prvek vyvolá událost a druhý na ni reaguje

**Klíčový rozdíl oproti procedurálnímu / sekvenčnímu programu:**

| Procedurální (konzolová) aplikace | Událostmi řízená (okenní) aplikace |
|-----------------------------------|------------------------------------|
| Program běží od `Main()` po `return` | Program **čeká** v nekonečné smyčce |
| Vstup čte, kdy on chce (`Console.ReadLine()`) | Vstup **přichází** kdykoli (uživatel rozhodne) |
| Tok řízení = vývojář | Tok řízení = uživatel + framework |
| Lineární | Reaktivní |

---

**Hlavní smyčka událostí (Event Loop / Message Loop):**

```
Aplikace start
       |
       v
+---------------------+
|  Inicializace UI    |
+---------------------+
       |
       v
+---------------------+      <----- DOKUD aplikace běží
|  EVENT LOOP:        |
|  1) Vezmi další     |
|     událost z fronty|
|  2) Najdi handler   |
|  3) Zavolej handler |
|  4) Vrať se na 1)   |
+---------------------+
       |
       v (uživatel zavře okno)
+---------------------+
|  Cleanup, ukončení  |
+---------------------+
```

**Fronta událostí (Message Queue):** systém ukládá příchozí události do FIFO fronty. Aplikace je vybírá v pořadí, v jakém přišly.

---

**Role událostí v C#:**

C# má **vestavěný mechanismus událostí** založený na delegátech:

| Pojem | Význam |
|-------|--------|
| **Delegát** | "Ukazatel" na metodu (typ, který drží odkaz na metodu) |
| **Událost** (`event`) | Speciální delegát – publisher může jen vyvolat, ne přepsat |
| **Publisher** | Třída, která událost **vyvolává** (např. tlačítko) |
| **Subscriber** | Třída, která se k události **přihlásí** a reaguje (handler) |
| **Handler** | Metoda, která se zavolá při události |
| **EventArgs** | Objekt s daty o události (souřadnice myši, klávesa, ...) |

**Kód – vlastní událost (publisher–subscriber):**

```csharp
public class Tlacitko
{
    // 1) Definice události (delegát standardní podpisem)
    public event EventHandler Kliknuto;

    // 2) Metoda vyvolávající událost
    public void Klikni()
    {
        // ?.Invoke chrání proti případu, kdy nikdo není přihlášen
        Kliknuto?.Invoke(this, EventArgs.Empty);
    }
}

class Program
{
    static void Main()
    {
        Tlacitko t = new Tlacitko();

        // 3) Přihlášení handleru pomocí +=
        t.Kliknuto += Handler;

        t.Klikni();   // vyvolá událost -> spustí Handler
    }

    static void Handler(object sender, EventArgs e)
    {
        Console.WriteLine("Tlačítko kliknuto!");
    }
}
```

---

### Bod 2: Průběh zpracování události

**Teorie:**

Cesta od fyzické akce uživatele k vykonání kódu má několik fází:

```
1) FYZICKÁ AKCE
   Uživatel klikne myší na tlačítko.

2) HARDWARE INTERRUPT
   Myš pošle přes USB signál CPU.
   OS interpretuje hardware událost.

3) SYSTÉMOVÁ ZPRÁVA
   OS zjistí, které okno je pod kurzorem,
   vytvoří zprávu (např. WM_LBUTTONDOWN ve Windows)
   a zařadí ji do MESSAGE QUEUE té aplikace.

4) DISPATCH
   Hlavní vlákno aplikace (UI thread) zprávu vyzvedne
   a předá ji konkrétnímu ovládacímu prvku (Button).

5) ROUTING (WPF) / PROCESSING (WinForms)
   WPF: událost "bublá" hierarchií (routed events).
   WinForms: událost se zpracuje přímo na controlu.

6) HANDLER
   Framework zavolá všechny přihlášené handlery v pořadí
   přihlášení. Každý handler dostane:
     - sender   = kdo událost vyvolal
     - e        = EventArgs s daty (souřadnice, klávesa,...)

7) REFRESH UI
   Pokud handler změnil data, UI se překreslí.
```

---

**Vizualizace:**

```
Uživatel                                            UI vlákno aplikace
   |                                                       ^
   | klik                                                  |
   v                                                       |
+------+    +---------+    +-------------+    +-----------+
| Myš  |--->|   OS    |--->| Message Q   |--->| Event Loop |
+------+    +---------+    +-------------+    +-----------+
                                                     |
                                                     v
                                          +-------------------+
                                          | Najdi target ctrl |
                                          | Zavolej handler   |
                                          +-------------------+
                                                     |
                                                     v
                                          +-------------------+
                                          | Přepiš model,     |
                                          | aktualizuj UI     |
                                          +-------------------+
```

---

**Synchronní zpracování + důsledek:**

Handler běží **na UI vlákně**. Dokud handler neskončí, UI je **zamrzlé** – nereaguje na další události. Proto:

```csharp
// ŠPATNĚ - blokuje UI na 5 sekund
private void Tlacitko_Click(object sender, RoutedEventArgs e)
{
    Thread.Sleep(5000);     // UI je zamrzlá
    Status.Text = "Hotovo";
}

// SPRÁVNĚ - async/await uvolní UI vlákno
private async void Tlacitko_Click(object sender, RoutedEventArgs e)
{
    await Task.Delay(5000); // UI běží dál
    Status.Text = "Hotovo";
}
```

---

### Bod 3: Příklady základních událostí

**Teorie:**

Události lze rozdělit podle zdroje:

**Životní cyklus okna:**

| Událost | Kdy nastává |
|---------|-------------|
| `Loaded` | Okno se zobrazilo poprvé |
| `Initialized` | Komponenty byly inicializovány |
| `Activated` | Okno získalo fokus |
| `Deactivated` | Okno ztratilo fokus |
| `Closing` | Okno se chystá zavřít (lze zrušit `e.Cancel = true`) |
| `Closed` | Okno bylo zavřeno |

**Myš:**

| Událost | Kdy nastává |
|---------|-------------|
| `MouseDown` | Stisk tlačítka myši |
| `MouseUp` | Uvolnění tlačítka myši |
| `MouseMove` | Pohyb myši nad prvkem |
| `MouseEnter` / `MouseLeave` | Kurzor vstoupil / opustil prvek |
| `MouseWheel` | Otočení kolečka |
| `Click` (Button) | Klik – stisk + uvolnění nad stejným prvkem |

**Klávesnice:**

| Událost | Kdy nastává |
|---------|-------------|
| `KeyDown` | Stisk klávesy |
| `KeyUp` | Uvolnění klávesy |
| `TextInput` | Vložen znak (jen znakové klávesy, nikoli Shift) |
| `PreviewKeyDown` | "Tunneling" verze – probíhá dříve, lze událost zrušit |

**Ovládací prvky:**

| Událost | Kde |
|---------|-----|
| `Click` | Button, MenuItem |
| `TextChanged` | TextBox – obsah se změnil |
| `SelectionChanged` | ComboBox, ListBox – jiná položka vybrána |
| `Checked` / `Unchecked` | CheckBox, RadioButton |
| `ValueChanged` | Slider, ProgressBar |

**Časovač / systém:**

| Událost | Účel |
|---------|------|
| `Timer.Tick` | Pravidelná akce (animace, kontrola stavu) |
| `Application.DispatcherUnhandledException` | Globální chyba |

---

**Kód – přihlášení handleru (XAML + code-behind):**

```xml
<!-- MainWindow.xaml -->
<Window x:Class="App.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        Title="Demo" Height="200" Width="300"
        Loaded="Window_Loaded">
    <StackPanel>
        <Button Name="btnOk"
                Content="OK"
                Click="btnOk_Click"/>
        <TextBox Name="txtJmeno"
                 TextChanged="txtJmeno_TextChanged"/>
    </StackPanel>
</Window>
```

```csharp
// MainWindow.xaml.cs (code-behind)
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Inicializace po zobrazení okna
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show($"Ahoj, {txtJmeno.Text}!");
    }

    private void txtJmeno_TextChanged(object sender, TextChangedEventArgs e)
    {
        btnOk.IsEnabled = !string.IsNullOrEmpty(txtJmeno.Text);
    }
}
```

**Alternativa – přihlášení v C# kódu:**

```csharp
btnOk.Click += btnOk_Click;          // přihlásit
btnOk.Click -= btnOk_Click;          // odhlásit
```

---

### Bod 4: Základní pravidla při vytváření okenních aplikací

**Teorie:**

1. **Oddělení UI a logiky** – kód, který kreslí okno, by neměl počítat daně. Použij vrstvy (např. MVVM, viz Bod 6).

2. **Neblokuj UI vlákno** – všechno delší než ~50 ms patří na pozadí (`async/await`, `Task.Run`, `BackgroundWorker`).

3. **Validace vstupu** – uživatel zadá cokoli (prázdný text, nečísla, záporné hodnoty). Vždy validuj před použitím.

4. **Zpětná vazba** – uživatel musí vidět, že jeho akce má efekt (změna kurzoru, progres bar, hláška).

5. **Obnovitelnost stavu** – po pádu / restartu by se aplikace měla vrátit do rozumného stavu (uložit rozpracovanou práci).

6. **Lokalizace a přístupnost** – texty oddělit do resource souborů, ovládat klávesnicí (Tab, Enter), kontrastní barvy.

7. **Konzistentní vzhled** – stejné prvky (tlačítka, dialogy) ve všech oknech vypadají stejně. Používat styly / motivy.

8. **Pojmenování ovládacích prvků** – `btnUlozit`, `txtJmeno`, `lstZakaznici`. Jasné prefixy podle typu.

9. **Odhlašování událostí** – pokud objekt přežije déle než publisher (nebo naopak), může vzniknout **memory leak**. Při dispose odhlásit handlery.

10. **Thread safety pro UI** – z jiných vláken se na UI prvky **nesmí** sahat přímo. V WPF se používá `Dispatcher.Invoke`.

```csharp
// CHYBA - z background vlákna se nesmí měnit UI
Task.Run(() => txtStatus.Text = "Hotovo");  // hodí výjimku

// SPRÁVNĚ
Task.Run(() =>
{
    Application.Current.Dispatcher.Invoke(() =>
    {
        txtStatus.Text = "Hotovo";
    });
});
```

---

### Bod 5: Responzivní vzhled aplikace

**Teorie:**

**Responzivní vzhled** = aplikace vypadá dobře a je použitelná při různých velikostech okna, rozlišeních a poměrech stran.

**Klíčové principy:**

1. **Nepoužívat absolutní pozice (`Canvas`, `Margin` s pevnými čísly)** pro hlavní layout.
2. **Používat layout panely**, které samy přepočítávají rozměry potomků.
3. **Hodnoty jako "Auto" a "*"** místo pevných pixelů.
4. **MinWidth / MinHeight** – minimální rozměry, pod které se prvek nezmenší.
5. **ScrollViewer** – když se obsah nevejde, lze rolovat.

---

**Layout panely ve WPF:**

| Panel | Princip rozložení |
|-------|-------------------|
| **StackPanel** | Prvky pod sebou (vertical) nebo vedle sebe (horizontal) |
| **Grid** | Mřížka řádků a sloupců (jako tabulka) – nejflexibilnější |
| **DockPanel** | Prvky se "lepí" k okrajům (Top, Bottom, Left, Right, Fill) |
| **WrapPanel** | Prvky vedle sebe, automaticky zalomené na další řádek |
| **Canvas** | Absolutní pozice (jen pro grafiku, ne pro UI layout) |

**Kód – Grid s responzivními sloupci:**

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="200"/>     <!-- pevná -->
        <ColumnDefinition Width="*"/>       <!-- vyplní zbytek -->
        <ColumnDefinition Width="Auto"/>    <!-- podle obsahu -->
    </Grid.ColumnDefinitions>

    <ListBox Grid.Column="0"/>              <!-- vždy 200px -->
    <TextBox Grid.Column="1"/>              <!-- roste s oknem -->
    <Button  Grid.Column="2" Content="OK"/> <!-- jen na šířku textu -->
</Grid>
```

```
Velikost okna 600px:                       Velikost okna 1200px:
+------+------------------+----+           +------+----------------------------+----+
| 200  |       370        | 30 |           | 200  |            970             | 30 |
+------+------------------+----+           +------+----------------------------+----+
 pevné  roste              auto             pevné  roste                       auto
```

---

**HorizontalAlignment / VerticalAlignment:**

```xml
<Button HorizontalAlignment="Stretch"/>   <!-- vyplní šířku rodiče -->
<Button HorizontalAlignment="Right"/>     <!-- jen vpravo -->
<Button HorizontalAlignment="Center"/>    <!-- na střed -->
```

---

**Adaptivní design (rozdílný layout pro různé velikosti):**

V XAML se používají např. `VisualStateManager` nebo data triggery. Pro mobilní aplikace (UWP / MAUI) jsou speciální `AdaptiveTrigger` reagující na šířku okna.

---

### Bod 6: MVVM pattern (Model–View–ViewModel)

**Teorie:**

**MVVM** = návrhový vzor, který **odděluje UI (View) od logiky (ViewModel) a od dat (Model)**. Ideální pro WPF, UWP, MAUI, Xamarin – frameworky se silným data bindingem.

```
+-------+         +------------+         +-------+
| MODEL | <-----> | VIEWMODEL  | <-----> | VIEW  |
+-------+         +------------+         +-------+
  Data,           Stav UI +              UI (XAML)
  business        příkazy +              bez logiky,
  logika          INotify                jen binduje
                  PropertyChanged
```

---

**Tři vrstvy:**

| Vrstva | Co dělá | Co NEvidí |
|--------|---------|-----------|
| **Model** | Datové třídy, business logika, přístup k DB | Nic o UI ani VM |
| **ViewModel** | Drží stav pro View, vystavuje vlastnosti a příkazy (Commands), zpracovává akce | NEvidí konkrétní View |
| **View** | XAML – jen rozložení a binding na ViewModel | NEvolá Model přímo |

---

**Klíčové mechanismy:**

1. **Data binding** – V XAML se na ViewModel napojí jednotlivé prvky:
   ```xml
   <TextBox Text="{Binding Jmeno, Mode=TwoWay}"/>
   ```
   Když se změní `Jmeno` ve VM, TextBox se aktualizuje. A naopak.

2. **`INotifyPropertyChanged`** – ViewModel musí oznamovat změny, jinak View neví, že se data změnila.
   ```csharp
   public class MainViewModel : INotifyPropertyChanged
   {
       private string _jmeno;
       public string Jmeno
       {
           get => _jmeno;
           set
           {
               _jmeno = value;
               OnPropertyChanged();   // oznam UI změnu
           }
       }

       public event PropertyChangedEventHandler PropertyChanged;
       protected void OnPropertyChanged([CallerMemberName] string name = null)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
   }
   ```

3. **Commands (`ICommand`)** – akce z View (kliknutí) se neřeší přes `Click` handler, ale přes commandy:
   ```xml
   <Button Content="Uložit" Command="{Binding UlozCommand}"/>
   ```
   Příkaz drží VM, takže VM zpracuje akci bez znalosti View.

---

**Kód – kompletní mini příklad:**

Model:
```csharp
public class Uzivatel
{
    public string Jmeno { get; set; }
    public int Vek { get; set; }
}
```

ViewModel:
```csharp
public class MainViewModel : INotifyPropertyChanged
{
    private string _jmeno;
    public string Jmeno
    {
        get => _jmeno;
        set { _jmeno = value; OnPropertyChanged(); }
    }

    public ICommand UlozCommand { get; }

    public MainViewModel()
    {
        UlozCommand = new RelayCommand(Uloz);
    }

    private void Uloz()
    {
        // business logika - uložit do DB, atd.
        var u = new Uzivatel { Jmeno = Jmeno };
        // ...
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}
```

View (XAML):
```xml
<Window x:Class="App.MainWindow" ...>
    <Window.DataContext>
        <local:MainViewModel/>
    </Window.DataContext>

    <StackPanel>
        <TextBox Text="{Binding Jmeno, UpdateSourceTrigger=PropertyChanged}"/>
        <Button  Content="Uložit" Command="{Binding UlozCommand}"/>
    </StackPanel>
</Window>
```

**Důležité:** XAML je **bez code-behind** – žádné `Click` handlery, žádné `txtJmeno.Text`. Vše přes binding.

---

**Výhody MVVM:**

| Výhoda | Vysvětlení |
|--------|------------|
| **Testovatelnost** | ViewModel = obyčejná C# třída, lze unit testovat bez UI |
| **Oddělení odpovědností** | Designer dělá XAML, programátor VM |
| **Znovupoužitelnost** | Stejný VM pro WPF okno i pro mobilní stránku |
| **Synchronizace UI ↔ data** | Automaticky díky bindingu |
| **Žádný code-behind** | Méně chyb, čistší kód |

**Srovnání s code-behind:**

| | Code-behind | MVVM |
|---|---|---|
| Logika | V `*.xaml.cs` | Ve ViewModel |
| Přístup k controlům | `txtJmeno.Text` | binding `{Binding Jmeno}` |
| Reakce na akce | `Button_Click` | `Command` |
| Testovatelnost | Špatná (UI provázané) | Dobrá |
| Vhodné pro | Malé prototypy | Reálné aplikace |

---

## Na co si dát pozor (Maturitní "chytáky")

1. **UI vlákno je JEDNO** – z jiných vláken nelze přímo měnit UI. Použít `Dispatcher.Invoke` (WPF) nebo `Control.Invoke` (WinForms).

2. **Blokující operace v handleru** – `Thread.Sleep`, čtení souboru, HTTP request → zamrzne UI. Řešení: `async/await`.

3. **Memory leaks z událostí** – pokud subscriber nikdy neodhlásí handler, publisher ho drží naživu. Před dispose udělat `event -= handler`.

4. **`event` není totéž co `delegate`** – `event` přidává restrikci: zvenku lze jen `+=` a `-=`, ne přímé přepsání ani vyvolání.

5. **MVVM != prostě "přesunout kód jinam"** – klíč je `INotifyPropertyChanged` a binding. Bez toho je to jen rozdělení do souborů.

6. **`DataContext`** – Bez nastaveného `DataContext` binding nefunguje (warningy v Output okně, ale aplikace se nepřeruší).

7. **Routed events ve WPF** – událost může "bublat" od potomka k rodiči (`Bubbling`) nebo "tunelovat" zpět (`Tunneling`, `Preview...`). Toho lze využít k centrálnímu zpracování.

8. **Pevné pixely vs `*` v Gridu** – pro responzivitu používat `Auto` a `*`, ne pevné rozměry.

9. **Dvojklik vs jednoklik** – událost `Click` se vyvolá VŽDY (i u dvojkliku se vyvolá dvakrát). Pro `MouseDoubleClick` je samostatná událost.

10. **`async void` jen u event handlerů** – jinak vždy `async Task`. `async void` nelze čekat (`await`) ani odchytit výjimky standardně.

---

## Senior Tipy

1. **Reactive Extensions (Rx.NET)** – události jako observables, lze je filtrovat, kombinovat, throttlovat (skvělé pro vyhledávací políčko s debouncem).

2. **Code-behind vs MVVM hybrid** – pro malé controly je code-behind v pořádku. Dogmaticky odmítat ho je antipattern.

3. **CommunityToolkit.Mvvm** – knihovna, která generuje `INotifyPropertyChanged` a commandy přes atributy (`[ObservableProperty]`, `[RelayCommand]`). Šetří desítky řádků boilerplate.

4. **MVU / MVI** – modernější vzory (Elmish, Compose) – stav je jeden objekt, View je čistá funkce stavu. Vhodné pro React, Flutter, .NET MAUI.

5. **Event aggregator / Messenger** – pro komunikaci mezi VM bez vzájemné reference. Posílají se zprávy přes centrální sběrnici.

6. **Validační atributy** – `[Required]`, `[Range]`, `[StringLength]` na vlastnostech VM + `IDataErrorInfo` / `INotifyDataErrorInfo` pro automatické zobrazení chyb v UI.

---

## Souvislosti s jinými otázkami

| Otázka | Souvislost |
|--------|------------|
| **Ot. 17** (OOP) | Třídy, polymorfismus, zapouzdření – základ MVVM |
| **Ot. 18** (Dědičnost, interfaces) | `INotifyPropertyChanged`, `ICommand` jsou rozhraní |
| **Ot. 1** (Datové typy) | Hodnotové vs referenční – binding pracuje s referencemi |
| **Ot. 5** (Rekurze) – nepřímo | Routed events "bublání" je rekurzivní průchod stromem prvků |

---

## Klíčová věta pro maturitu

> *"Programování řízené událostmi je paradigma, kde tok programu řídí události od uživatele nebo systému, nikoli sekvence příkazů. Aplikace se zinicializuje, pak vstoupí do nekonečné smyčky událostí: vybere událost z fronty, najde přihlášený handler (delegát/event v C#) a zavolá ho. Při programování okenních aplikací musíme oddělit UI od logiky, neblokovat UI vlákno (async/await) a používat layout panely pro responzivní vzhled. Vzor MVVM rozděluje aplikaci na Model (data), View (XAML bez logiky) a ViewModel (stav a příkazy), které jsou propojeny data bindingem a INotifyPropertyChanged – díky tomu je kód testovatelný a UI se synchronizuje s daty automaticky."*

---

*Vytvořeno: 2026-05-10 - Maturitní příprava PRG 2025/2026*
