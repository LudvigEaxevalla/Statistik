using System.Text.Json;

int variationsBredd;
float stegTvå;
float stegTre;
float stegFyra = 0;
float stegFem;
float standardAvvikelse;
float total = 0;
float intervallL = 0;
float intervallH = 0;

var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

Console.WriteLine("Vilken fil? 1 = midterms.json, 2 = nhl.json");
string? val = Console.ReadLine();

List<(string Etikett, int Värde)> värden = new();

if (val == "2")
{
    string json = File.ReadAllText("nhl.json");
    var nhl = JsonSerializer.Deserialize<Dictionary<string, List<NhlEntry>>>(json, options) ?? new();

    // Lista spelarna och låt användaren välja en
    List<string> spelare = nhl.Keys.ToList();
    Console.WriteLine("Vilken spelare?");
    for (int i = 0; i < spelare.Count; i++)
    {
        Console.WriteLine($"{i + 1} = {spelare[i]}");
    }

    int spelarVal;
    while (!int.TryParse(Console.ReadLine(), out spelarVal) || spelarVal < 1 || spelarVal > spelare.Count)
    {
        Console.WriteLine($"Skriv ett nummer mellan 1 och {spelare.Count}");
    }

    string valdSpelare = spelare[spelarVal - 1];
    Console.WriteLine();
    Console.WriteLine(valdSpelare);

    foreach (NhlEntry s in nhl[valdSpelare])
    {
        värden.Add(($"{s.Year}", s.ExpectedPoints)); 
    }
}
else
{
    string json = File.ReadAllText("midterms.json");
    MidtermData? data = JsonSerializer.Deserialize<MidtermData>(json, options);
    foreach (Entry e in data?.Elections ?? new List<Entry>())
    {
        värden.Add(($"{e.Year}  {e.President,-12}", e.Change));
    }
}

int lägstaNummer = värden[0].Värde;
int högstaNummer = värden[0].Värde;
System.Console.WriteLine("LISTA:");
System.Console.WriteLine("------------");
foreach (var v in värden)
{
    Console.WriteLine($"{v.Etikett,-22} {v.Värde,4}");
    total += v.Värde;

    if (v.Värde < lägstaNummer) lägstaNummer = v.Värde;
    if (v.Värde > högstaNummer) högstaNummer = v.Värde;
}
System.Console.WriteLine("------------");

int antalEntries = värden.Count;
float medelVärde = total/antalEntries;
variationsBredd = högstaNummer - lägstaNummer;
System.Console.WriteLine();
Console.WriteLine("Lägsta värde: " + lägstaNummer + ", Högsta värde: " + högstaNummer);
Console.WriteLine("Variationsbredd: " + variationsBredd);

//Steg 1
System.Console.WriteLine("Steg 1 - Räkna ut medelvärdet");
System.Console.WriteLine(total + " / " + antalEntries + " = " + medelVärde);
System.Console.WriteLine();
Console.WriteLine("Medelvärde: " + medelVärde);
Console.ReadLine();


//Steg 2
Console.WriteLine("Steg 2 - Ta varje värde minus medelvärdet:");
foreach (var v in värden)
{
    stegTvå = v.Värde - medelVärde;
    Console.WriteLine(v.Värde + " - (" + medelVärde + ") = " + stegTvå.ToString("0.##"));
}
Console.ReadLine();

//Steg 3
Console.WriteLine("Steg 3 - Kvadera varje svar från steg 2:");

foreach (var v in värden)
{
    stegTvå = v.Värde - medelVärde;
    stegTre = stegTvå * stegTvå;
    stegFyra += stegTre;
    Console.WriteLine(stegTvå.ToString("0.##") + "²" + " = " + stegTre.ToString("0.##"));
}

//Steg 4
Console.WriteLine("Steg 4 - Lägg ihop alla kvadrater:");
Console.WriteLine(stegFyra.ToString("0.##"));
System.Console.WriteLine();
Console.ReadLine();

//Steg 5
Console.WriteLine("Steg 5 - Dela summan med n - 1:");
stegFem = stegFyra/(värden.Count-1);
Console.WriteLine(stegFyra.ToString("0.##") + " / " + (värden.Count-1) + " = " + stegFem);
System.Console.WriteLine();
Console.ReadLine();

//Steg 6
Console.WriteLine("Steg 6 - Dra roten ur");
standardAvvikelse = MathF.Sqrt(stegFem);
Console.WriteLine("√" + stegFem.ToString("0.##") + " = " + standardAvvikelse.ToString("0.##"));

Console.WriteLine("Standardavvikelsen s = " + standardAvvikelse.ToString("0.##"));
Console.ReadLine();

//Intervall

intervallL = medelVärde - standardAvvikelse;
intervallH = medelVärde + standardAvvikelse;

Console.WriteLine("Intervallen ligger mellan " + intervallL.ToString("0.##") + " och " + intervallH.ToString("0.##"));
Console.ReadLine();

public class MidtermData
{
    public List<Entry> Elections { get; set; } = new List<Entry>();
}

public class Entry
{
    public int Year { get; set; }
    public string President { get; set; } = "";
    public int Change { get; set; }
}

public class NhlEntry
{
    public int Year { get; set; }
    public int GP { get; set; }
    public int Points { get; set; }
    public int ExpectedPoints { get; set; }
    public int Change { get; set; }
}
