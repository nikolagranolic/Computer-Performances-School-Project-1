using ANOVAiKontrastiConsole;

Anova anova = new Anova();

Console.Write("Unesite vjerovatnocu za F raspodjelu (npr. 0.95): ");
double probability = Double.Parse(Console.ReadLine());
anova.SetProbability(probability);

string option = "";

do
{
    Console.Clear();
    Console.WriteLine("Unesite \'+\' za dodavanje nove alternative,\'r\' za prikaz rezultata tehnike ANOVA,\n\'k\' za poredjenje dvije alternative tehnikom kontrasta ili x za kraj:");
    option = Console.ReadLine();

    if (option == "+")
    {
        Console.WriteLine("Koliko mjerenja cete unijeti za ovu alternativu?");
        int numOfMeasurements = int.Parse(Console.ReadLine());
        double[] data = new double[numOfMeasurements];
        for (int i = 0; i < numOfMeasurements; i++)
        {
            Console.WriteLine("Unesite " + (i + 1) + ". mjerenje:");
            data[i] = double.Parse(Console.ReadLine());
        }

        Alternative temp = new Alternative(numOfMeasurements);
        temp.SetData(data);

        anova.AddAlternative(temp);

        Console.WriteLine("Alternativa uspjesno zabiljezena! Pritisnite dugme Enter za nastavak.");
        Console.ReadLine();
    }
    else if (option.ToLower() == "r")
    {
        anova.ComputeEverything();
        anova.GetResults();

        Console.WriteLine("Pritisnite dugme Enter za nastavak.");
        Console.ReadLine();
    }
    else if (option.ToLower() == "k")
    {
        if (anova.AlternativesCount() < 2)
        {
            Console.WriteLine("Potrebne su najmanje dvije alternative za tehniku kontrasta! Pritisnite dugme Enter pa pokusajte ponovo.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Unesite redni broj prve alternative koju zelite da poredite:");
            int indexFirst = int.Parse(Console.ReadLine());
            Console.WriteLine("Unesite redni broj druge alternative koju zelite da poredite:");
            int indexSecond = int.Parse(Console.ReadLine());
            Console.WriteLine("Unesite vjerovatnocu za tehniku kontrasta (npr. 0.9):");
            double cProbability = double.Parse(Console.ReadLine());

            anova.ComputeEverything();
            anova.Contrasts(indexFirst, indexSecond, cProbability);

            Console.WriteLine("Pritisnite dugme Enter za nastavak.");
            Console.ReadLine();
        }
    }
} while (option.ToLower() != "x");