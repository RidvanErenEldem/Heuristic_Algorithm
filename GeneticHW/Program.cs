using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticHW
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            int PopulationSize = 10;
            int ChromosomeX1Length = 12;
            int ChromosomeX2Length = 13;
            int ChromosomeLength = ChromosomeX1Length + ChromosomeX2Length;
            int ParentCount = 2;
            double CrossoverProbability = 0.25;
            double MutateProbability = 0.01;
            int IterationCount = 200;
            List<Chromosome> Population = GeneratePopulation(PopulationSize, ChromosomeLength);

            foreach (Chromosome chromosome in Population)
            {
                string info = chromosome.getInformation();
            }

            List<Fitness> FitnessValues = CalculateFitness(Population, ChromosomeX1Length, ChromosomeX2Length);
            double totalFitnessValue = 0;
            double totalPercentageValue = 0;
            foreach (Fitness fitness in FitnessValues)
            {
                totalFitnessValue += fitness.Value;
                totalPercentageValue += fitness.Percentage;
            }
            Console.WriteLine("0. Generation Total fitness value: "+ totalFitnessValue);
            double bestResult = FitnessValues.Max(value => value.Value);
            int bestResultIndex = 0;
            Console.WriteLine("0. Generation Value of Best Chromosome: "+ bestResult);
            for(int i = 0 ; i < IterationCount; i++)
            {
                List<Chromosome> SelectedParents = Selection(FitnessValues, totalPercentageValue, ParentCount, Population);
                List<Chromosome> ChildChromosomes = Crossover(SelectedParents, CrossoverProbability);

                Population = Merger(Population, ChildChromosomes, FitnessValues, ParentCount);
                Population = Mutate(Population, MutateProbability);

                FitnessValues = CalculateFitness(Population, ChromosomeX1Length, ChromosomeX2Length);
                totalFitnessValue = 0;
                totalPercentageValue = 0;
                foreach (Fitness fitness in FitnessValues)
                {
                    totalFitnessValue += fitness.Value;
                    totalPercentageValue += fitness.Percentage;
                }
                Console.WriteLine(i+". Generation Total fitness value: "+ totalFitnessValue);
                double maxValueofGen = FitnessValues.Max(value => value.Value);
                Console.WriteLine(i+". Generation Value of Best Chromosome: "+maxValueofGen);
                if(maxValueofGen > bestResult)
                {
                    bestResult = maxValueofGen;
                    bestResultIndex = i;
                }
                      
            }
            Console.WriteLine("\nThe Best Value is: " + bestResult);
            Console.WriteLine("Index of The Best Value is: "+ bestResultIndex);
        }

        static List<Chromosome> Merger(List<Chromosome> Population, List<Chromosome> ChildChromosomes, List<Fitness> FitnessValues, int ParentCount)
        {
            for(int i = 0; i < ParentCount; i++)
            {
                double Worst = FitnessValues.Min(value => value.Value);
                int j = 0;
                foreach (Fitness value in FitnessValues)
                {
                    if(Worst == value.Value)
                        break;
                    j++;
                }
                Population.RemoveAt(j);
                FitnessValues.RemoveAt(j);
            }
            for (int i = 0; i < ParentCount; i++)
            {
                Population.Add(ChildChromosomes[i]);
            }
            return Population;
        }

        static Chromosome GenerateRandomChromosome(int size)
        {
            string chromosomeInfo = "";
            
            for (int i = 0; i < size; i++)
            {
                
                int num = random.Next() %2;
                chromosomeInfo += num.ToString();
            }
            Chromosome chromosome = new Chromosome();
            chromosome.setInformation(chromosomeInfo);
            return chromosome;
        }
        static List<Chromosome> GeneratePopulation(int PopulationSize, int ChromosomeSize)
        {
            List<Chromosome> population = new List<Chromosome>();
            for (int i = 0; i < PopulationSize; i++)
            {
                Chromosome chromosome = new Chromosome();
                population.Add(GenerateRandomChromosome(ChromosomeSize));
            }

            return population;
        }

        static List<Fitness> CalculateFitness(List<Chromosome> population, int X1Size, int X2Size)
        {
            List<double> fitnessValues = new List<double>();
            List<Chromosome> X1 = new List<Chromosome>();
            List<Chromosome> X2 = new List<Chromosome>();
            const double X1StartingValue = -1.5;
            const double X2StartingValue = 0;
            
            foreach (Chromosome chromosome in population)
            {
                X1.Add(chromosome.getPartofChromosome(chromosome,0 ,X1Size));
                X2.Add(chromosome.getPartofChromosome(chromosome, X1Size,X2Size));
            }

            for (int i = 0; i < X1.Count; i++)
            {
                int X1Pos = Convert.ToInt32(X1[i].getInformation(),2);
                int X2Pos = Convert.ToInt32(X2[i].getInformation(),2);
                
                if(X1Pos > 4000)
                    X1Pos = X1Pos - 4000;
                if(X2Pos > 5000)
                    X2Pos = X2Pos - 5000;

                double rate = Convert.ToDouble(4m/4000m);
                
                double X1Value = ((rate)*X1Pos)+X1StartingValue;
                double X2Value = ((rate)*X2Pos)+X2StartingValue;

                double part1 = 40 - Convert.ToDouble(9m/2m)*X1Value;
                double part2 = (4*X2Value) - Math.Pow(X1Value,2);
                double part3 = (2*Math.Pow(X2Value,2))+ (2*X1Value*X2Value);
                double part4 = Math.Pow(X1Value,4) + (2*Math.Pow(X1Value,2)*X2Value);

                double fitnessValue = part1 + part2 - part3 - part4;

                fitnessValues.Add(fitnessValue);
            }
            double total = fitnessValues.Sum(Convert.ToDouble);
            List<double> fitnessPercentages = CalculateFitnessPercentage(fitnessValues, total);
            List<Fitness> StructuredFitnessValues = new List<Fitness>();

            for (int i = 0; i<fitnessPercentages.Count; i++)
            {
                StructuredFitnessValues.Add(new Fitness(fitnessValues[i], fitnessPercentages[i]));
            }

            return StructuredFitnessValues;
        }

        static List<double> CalculateFitnessPercentage(List<double> fitnessValues, double total)
        {
            List<double> fitnessPercente = new List<double>();
            foreach (double fitnessValue in fitnessValues)
            {
                fitnessPercente.Add((fitnessValue/total) * 100);   
            }
            return fitnessPercente;
        }

        static List<Chromosome> Selection (List<Fitness> fitnessValues, double totalPercentage, int ParentCount, List<Chromosome> Population)
        {
            List<int> selectedIndexes = new List<int>();
            for(int j = 0; j < ParentCount; j++)
            {
                double randomNumber = random.Next(0, Convert.ToInt32(totalPercentage));
                for (int i = 0; i < fitnessValues.Count; i++)
                {
                    if (randomNumber < fitnessValues[i].Percentage)
                    {
                        selectedIndexes.Add(i);
                        break;
                    }
                    randomNumber = randomNumber - fitnessValues[i].Percentage;
                }
            }
            for (int i = 0; i < selectedIndexes.Count; i++) 
            {
                if(i%2 == 0 && selectedIndexes[i] == selectedIndexes[i+1])
                {
                    selectedIndexes[i] = selectedIndexes[i] + 1;
                }
            }
            List<Chromosome> Parents = new List<Chromosome>();
            int PopulationCount = Population.Count;
            foreach(int selectedIndex in selectedIndexes)
            {
                if(selectedIndex >= PopulationCount)
                    Parents.Add(Population[selectedIndex-1]);
                else
                    Parents.Add(Population[selectedIndex]);
            }
            return Parents;
        }

        static List<Chromosome> Crossover (List<Chromosome> Parents, double CrossoverProbability)
        {
            List<Chromosome> childChromosomes = new List<Chromosome>();
            for(int i = 0; i < Parents.Count; i = i+2)
            {
                char[] p1 = Parents[i].getInformation().ToCharArray();
                char[] p2 = Parents[i+1].getInformation().ToCharArray();
                for(int j = 0; j < p1.Length; j++)
                {
                    double isCrossover = random.NextDouble();
                    if(isCrossover < CrossoverProbability)
                    {
                        char temp = p1[j];
                        p1[j] = p2[j];
                        p2[j] = temp;
                        temp = '\0';
                    }
                }
                childChromosomes.Add(new Chromosome(new string(p1)));
                childChromosomes.Add(new Chromosome(new string(p2)));
            }
            return childChromosomes;
        }
        static List<Chromosome> Mutate (List<Chromosome> Population, double MutateProbability)
        {
            foreach (Chromosome chromosome in Population)
            {
                char[] charChromosome = chromosome.getInformation().ToCharArray();
                for (int j = 0; j < charChromosome.Length; j++)
                {
                    double isMutate = random.NextDouble();
                    if(isMutate < MutateProbability)
                    {
                        if(charChromosome[j] == '0')
                            charChromosome[j] = '1';
                        else
                            charChromosome[j] = '0';
                    }
                }
                chromosome.setInformation(new string(charChromosome));
            }
            return Population;
        }
    }
}
