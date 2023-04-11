using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace ANOVAiKontrastiConsole
{
    public class Anova
    {
        private List<Alternative> data = new List<Alternative>();
        private double totalAverage;

        private double sse = 0;
        private int sseDegreesOfFreedom;
        private double ssa = 0;
        private int ssaDegreesOfFreedom;
        private double sst = 0;
        private int sstDegreesOfFreedom;

        private double varianceA;
        private double varianceE;

        private double fComputed;
        private double fTabulated;

        private double probability = 0.95;

        public int AlternativesCount()
        {
            return data.Count;
        }
        public void SetProbability(double probability)
        {
            this.probability = probability;
        }

        public void AddAlternative(Alternative alternative)
        {
            data.Add(alternative);
        }

        public void ComputeEverything()
        {
            ComputeTotalAverage();
            ComputeEffects();
            ComputeSumOfSquaresError();
            ComputeSumOfSquaresAlternatives();
            ComputeSumOfSquaresTotal();

            ssaDegreesOfFreedom = data.Count - 1;
            sseDegreesOfFreedom = 0;
            foreach (Alternative alternative in data)
            {
                sseDegreesOfFreedom += alternative.GetNumOfMeasurements() - 1;
            }
            sstDegreesOfFreedom = ssaDegreesOfFreedom + sseDegreesOfFreedom;

            varianceA = ssa / ssaDegreesOfFreedom;
            varianceE = sse / sseDegreesOfFreedom;

            fComputed = varianceA / varianceE;

            FisherSnedecor fDistribution = new FisherSnedecor(ssaDegreesOfFreedom, sseDegreesOfFreedom);

            fTabulated = fDistribution.InverseCumulativeDistribution(probability); ;
        }

        public void GetResults()
        {
            string results = "";
            results += Math.Round((ssa / sst) * 100, 1) + "% ukupne varijacije u mjerenjima je zbog razlika izmedju alternativa.";
            results += "\n";
            results += Math.Round((sse / sst) * 100, 1) + "% ukupne varijacije u mjerenjima je zbog gresaka u mjerenjima.";
            results += "\n";
            if (fComputed > fTabulated)
            {
                results += "Sa " + Math.Round(probability * 100, 0) + "%-tnim povjerenjem možemo reci da su razlike izmedju alternativa statisticki znacajne.";
            }
            else
            {
                results += "Razlike između alternativa nisu statistički značajne.";
            }

            Console.WriteLine(results);
        }

        private void ComputeTotalAverage()
        {
            double sum = 0;
            int i = 0;

            foreach (Alternative column in data)
            {
                i += column.GetNumOfMeasurements();
                foreach (double measurement in column.GetData())
                {
                    sum += measurement;
                }
            }

            totalAverage = sum / i;
        }

        private void ComputeEffects()
        {
            foreach (Alternative column in data)
            {
                column.SetEffect(column.GetMeanColumnValue() - totalAverage);
            }
        }

        private void ComputeSumOfSquaresError()
        {
            sse = 0;
            foreach (Alternative column in data)
            {
                foreach (double e in column.GetErrorsInMeasurements())
                {
                    sse += Math.Pow(e, 2);
                }
            }
        }

        private void ComputeSumOfSquaresAlternatives()
        {
            ssa = 0;
            foreach (Alternative column in data)
            {
                double alpha = column.GetEffect();
                ssa += column.GetNumOfMeasurements() * (alpha * alpha);
            }
        }

        private void ComputeSumOfSquaresTotal()
        {
            sst = 0;
            foreach (Alternative column in data)
            {
                foreach (double measurement in column.GetData())
                {
                    double t = measurement - totalAverage;
                    sst += t * t;
                }
            }
        }

        public void Contrasts(int numOffirstAlternative, int numOfSecondAlternative, double probability)
        {
            double summaryDeviation = 0;
            double c = data[numOffirstAlternative - 1].GetEffect() - data[numOfSecondAlternative - 1].GetEffect();

            Console.WriteLine("c: " + c);

            int i = 0;
            foreach (Alternative column in data)
            {
                i += column.GetNumOfMeasurements();
            }
            summaryDeviation = Math.Sqrt(varianceE) * Math.Sqrt(2.0 / i);
            Console.WriteLine("sc: " + summaryDeviation);

            StudentT tDistribution = new StudentT(0, 1, sseDegreesOfFreedom);
            double tValue = tDistribution.InverseCumulativeDistribution(1 - (1 - probability) / 2);

            Console.WriteLine("t: " + tValue);

            double c1 = Math.Round(c - summaryDeviation * tValue, 4), c2 = Math.Round(c + summaryDeviation * tValue, 4);

            Console.WriteLine(Math.Round(probability * 100, 1) + ": " + c1 + "  " + c2);

            if (c1 * c2 < 0)
            {
                Console.WriteLine("Nema statisticki znacajne razlike izmedju alternativa.");
            }
            else
            {
                Console.WriteLine("Razlika izmedju alternativa je statisticki znacajna.");
            }
        }
    }
}
