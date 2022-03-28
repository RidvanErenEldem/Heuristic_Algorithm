using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticHW
{
    public class Fitness
    {
        public double Value;
        public double Percentage;

        public Fitness(double Value, double Percentage)
        {
            this.Value = Value;
            this.Percentage = Percentage;
        }
        public Fitness(double Value)
        {
            this.Value = Value;
        }
    }
}