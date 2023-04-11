using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ANOVAiKontrastiConsole
{
    public class Alternative
    {
        private int numOfMeasurements;
        private double[] measurements;
        private double[] errorsInMeasurements;
        private double meanColumnValue;
        private double effect;

        public Alternative(int numOfMeasurements)
        {
            this.numOfMeasurements = numOfMeasurements;
            measurements = new double[numOfMeasurements];
            errorsInMeasurements = new double[numOfMeasurements];
        }

        public void SetData(double[] data)
        {
            measurements = data;
            meanColumnValue = measurements.Sum() / numOfMeasurements;
            for (int i = 0; i < measurements.Length; i++)
            {
                errorsInMeasurements[i] = measurements[i] - meanColumnValue;
            }
        }

        public double[] GetData()
        {
            return measurements;
        }
        public double[] GetErrorsInMeasurements()
        {
            return errorsInMeasurements;
        }

        public double GetMeanColumnValue()
        {
            return meanColumnValue;
        }

        public void SetEffect(double effect)
        {
            this.effect = effect;
        }

        public double GetEffect()
        {
            return effect;
        }

        public int GetNumOfMeasurements()
        {
            return numOfMeasurements;
        }
    }
}
