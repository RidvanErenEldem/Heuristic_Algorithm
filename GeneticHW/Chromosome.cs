using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticHW
{
    public class Chromosome
    {
        private string information;

        public string getInformation()
        {
            return this.information;
        }

        public void setInformation(string information)
        {
            this.information = information;
        }

        public Chromosome(){}

        public Chromosome(string information)
        {
            this.information = information;
        }

        public Chromosome getPartofChromosome(Chromosome chromosome, int startSize, int endSize)
        {
            string info = chromosome.getInformation();
            info = info.Substring(startSize, endSize);
            Chromosome dummy = new Chromosome();
            dummy.information = info;
            return dummy;
        }

        public override string ToString()
        {
            return this.information;
        }
    }
}