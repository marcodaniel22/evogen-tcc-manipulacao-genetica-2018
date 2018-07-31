using EvoGen.Domain.Collections;
using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Domain.GA.StructureGenerator
{
    public class StructureGenerator
    {
        private volatile List<SGChromosome> population, children;
        private volatile SGChromosome bestIndividual, worseIndividual;
        private volatile int generation;
        private int generations;
        private double mutationRate;
        private string target;

        public List<SGChromosome> Population
        {
            get { return this.population; }
        }

        public List<SGChromosome> Childrem
        {
            get { return this.children; }
        }

        public SGChromosome BestIndividual
        {
            get { return this.bestIndividual; }
        }

        public SGChromosome WorseIndividual
        {
            get { return this.worseIndividual; }
        }

        public int Generation
        {
            get { return this.generation; }
        }

        private static Random random = new Random(DateTime.Now.Millisecond);

        public StructureGenerator(string target, int populationSize, int generations, double mutationRate)
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
            population = new List<SGChromosome>(size);
            var atoms = MoleculeGraph.ExtractAtomsFromNomenclature(this.target);
            for (int i = 0; i < size; i++)
                population.Add(new SGChromosome(new MoleculeGraph(this.target, atoms)));
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
            children = new List<SGChromosome>(halfSize);

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
