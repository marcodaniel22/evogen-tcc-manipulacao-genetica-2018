using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using static EvoGen.GA_MoleculeValidation.Chromosome;

namespace EvoGen.GA_MoleculeValidation
{
    public class GA
    {
        private volatile List<Chromosome> population, children;
        private volatile Chromosome bestIndividual, worseIndividual;
        private volatile int generation;
        private int generations;
        private double mutationRate;
        private string target;

        public List<Chromosome> Population
        {
            get { return this.population; }
        }

        public List<Chromosome> Childrem
        {
            get { return this.children; }
        }

        public Chromosome BestIndividual
        {
            get { return this.bestIndividual; }
        }

        public Chromosome WorseIndividual
        {
            get { return this.worseIndividual; }
        }

        public int Generation
        {
            get { return this.generation; }
        }

        private static Random random = new Random(DateTime.Now.Millisecond);

        public GA(string target, int populationSize, int generations, double mutationRate)
        {
            GuardProperties(target, populationSize, generations, mutationRate);
            this.generations = generations;
            this.mutationRate = mutationRate;
            this.generation = 0;
            this.target = target;
            InitializePopulation(populationSize);
        }

        private void GuardProperties(string target, int populationSize, int generations, double mutation_rate)
        {
            Guard.StringNullOrEmpty(target, "Target");
            Guard.IntLowerThanZero(populationSize, "PopulationSize");
            Guard.IntLowerThanZero(generations, "Generations");
            Guard.DoubleLowerThanZero(mutation_rate, "Mutation Rate");
        }

        public void InitializePopulation(int size)
        {
            population = new List<Chromosome>(size);
            var atoms = MoleculeGraph.ExtractAtomsFromNomenclature(this.target);
            for (int i = 0; i < size; i++)
                population.Add(new Chromosome(new MoleculeGraph(this.target, atoms)));
        }

        public MoleculeGraph FindSolution()
        {
            do
            {
                GenerateChildren();
                Selection();
                MutatePopulation();
                generation++;
                GetBestIndividual();
            } while (bestIndividual.Fitness > 0 && generation < generations);

            return bestIndividual.Molecule;
        }

        public void GenerateChildren()
        {
            int halfSize = population.Count / 2;
            children = new List<Chromosome>(halfSize);

            for (int i = 0; i < halfSize; i++)
            {
                var parent1 = population[i];
                var parent2 = population[population.Count - 1 - i];
                children.Add(parent1.Crossover(parent2));
            }
            GetBestIndividual();
            GetWorseIndividual();
        }

        public void Selection()
        {
            if (bestIndividual.Fitness < worseIndividual.Fitness)
            {
                for (int i = 0; i < population.Count / 2; i++)
                {
                    int x = 0;
                    do
                    {
                        x = random.Next(population.Count);
                        double fator = (population[x].Fitness - bestIndividual.Fitness)
                            / (worseIndividual.Fitness - bestIndividual.Fitness);
                        double r = random.NextDouble();
                        if (r < fator || fator == 0) break;
                    } while (true);

                    if (population[x].Fitness > this.bestIndividual.Fitness)
                        population[x] = children[i];
                }
            }
        }

        public void MutatePopulation()
        {
            for (int i = 0; i < population.Count; i++)
                population[i].Mutate(mutationRate);
        }

        public void GetBestIndividual()
        {
            bestIndividual = population.OrderBy(x => x.Fitness).FirstOrDefault();
        }

        public void GetWorseIndividual()
        {
            worseIndividual = population.OrderByDescending(x => x.Fitness).FirstOrDefault();
        }
    }
}
