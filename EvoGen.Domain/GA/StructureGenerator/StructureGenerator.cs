using EvoGen.Domain.Collections;
using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Domain.GA.StructureGenerator
{
    public class StructureGenerator
    {
        public List<SGChromosome> Population { get; private set; }
        public List<SGChromosome> Children { get; private set; }
        public SGChromosome BestIndividual { get; private set; }
        public SGChromosome WorseIndividual { get; private set; }
        public int Generation { get; private set; }
        public string Target { get; private set; }
        public bool Searching { get; private set; }
        public bool Finished { get; private set; }
        public List<AtomNode> Atoms { get; private set; }

        public Queue<MoleculeGraph> ResultList { get; set; }

        private int _populationSize = 0;
        private int _maxGenerations = 0;
        private double _mutationRate = 0.00;

        private volatile static Random _random = new Random(DateTime.Now.Millisecond);

        public StructureGenerator(string target, int populationSize, int generations, double mutationRate)
        {
            Guard.IntLowerThanZero(populationSize, "PopulationSize");
            this._populationSize = populationSize;
            Guard.IntLowerThanZero(generations, "Generations");
            this._maxGenerations = generations;
            Guard.DoubleLowerThanZero(mutationRate, "Mutation Rate");
            this._mutationRate = mutationRate;
            Guard.StringNullOrEmpty(target, "Target");
            this.Target = target;
            this.Generation = 0;
            InitializePopulation();
        }

        public void InitializePopulation()
        {
            ResultList = new Queue<MoleculeGraph>();
            Population = new List<SGChromosome>(_populationSize);
            Atoms = MoleculeGraph.ExtractAtomsFromNomenclature(Target);
            if (Atoms.Count > 1)
                for (int i = 0; i < _populationSize; i++)
                    Population.Add(new SGChromosome(new MoleculeGraph(Target, Atoms)));
        }

        public MoleculeGraph FindSolution()
        {
            try
            {
                if (Atoms.Count > 1)
                {
                    Searching = true;
                    do
                    {
                        GenerateChildren();
                        Selection();
                        MutatePopulation();
                        Generation++;
                    } while (BestIndividual.Fitness > 0 && Generation < _maxGenerations);

                    Searching = false;
                    Finished = true;
                    return BestIndividual.Molecule;
                }
                return null;
            }
            catch (Exception) { return null; }
            finally
            {
                Searching = false;
                Finished = true;
            }

        }

        public void FindSolutions()
        {
            try
            {
                if (Atoms.Count > 1)
                {
                    Searching = true;
                    do
                    {
                        GenerateChildren();
                        Selection();
                        MutatePopulation();
                        Generation++;
                        if (BestIndividual.Fitness == 0)
                        {
                            var bestIndividuals = Population.Where(x => x.Fitness == 0).Select(x => x.Molecule);
                            if (bestIndividuals.Count() <= 100)
                            {
                                foreach (var item in bestIndividuals)
                                {
                                    ResultList.Enqueue(item);
                                }
                                Population.RemoveAll(x => x.Fitness == 0);
                                do
                                {
                                    Population.Add(new SGChromosome(new MoleculeGraph(Target, Atoms)));
                                } while (Population.Count < _populationSize);
                                GetBestIndividual();
                            }
                            else
                            {
                                bestIndividuals = bestIndividuals.Take(100);
                                foreach (var item in bestIndividuals)
                                {
                                    ResultList.Enqueue(item);
                                }
                                break;
                            }
                        }
                    } while (Generation < _maxGenerations && ResultList.Count < 1000);
                }
            }
            catch (Exception) { }
            finally
            {
                Searching = false;
                Finished = true;
            }
        }

        public void GenerateChildren()
        {
            int halfSize = Population.Count / 2;
            Children = new List<SGChromosome>(halfSize);

            for (int i = 0; i < halfSize; i++)
            {
                var parent1 = Population[i];
                var parent2 = Population[Population.Count - 1 - i];
                Children.Add(parent1.Crossover(parent2));
            }
            GetBestIndividual();
            GetWorseIndividual();
        }

        public void Selection()
        {
            if (BestIndividual.Fitness < WorseIndividual.Fitness)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    int x = 0;
                    do
                    {
                        x = _random.Next(Population.Count);
                        double fator = (Population[x].Fitness - BestIndividual.Fitness)
                            / (WorseIndividual.Fitness - BestIndividual.Fitness);
                        double r = _random.NextDouble();
                        if (r < fator || fator == 0) break;
                    } while (true);

                    if (Population[x].Fitness > BestIndividual.Fitness)
                        Population[x] = Children[i];
                }
                GetBestIndividual();
                GetWorseIndividual();
            }
        }

        public void MutatePopulation()
        {
            if (BestIndividual.Fitness > 0)
            {
                foreach (var individual in Population)
                {
                    individual.Mutate(_mutationRate);
                }
                GetBestIndividual();
                GetWorseIndividual();
            }
        }

        public void GetBestIndividual()
        {
            BestIndividual = Population.First(x => x.Fitness == Population.Min(y => y.Fitness));
        }

        public void GetWorseIndividual()
        {
            WorseIndividual = Population.First(x => x.Fitness == Population.Max(y => y.Fitness));
        }
    }
}
