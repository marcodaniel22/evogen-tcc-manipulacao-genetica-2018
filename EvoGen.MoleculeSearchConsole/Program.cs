using EvoGen.Domain.Collections;
using EvoGen.Domain.DataGen;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Repository.Repositories;
using Inject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.MoleculeSearchConsole
{
    public class Program
    {
        public static IMoleculeService _moleculeService { get; set; }
        public static ILinkService _linkService { get; set; }
        public static ILogService _logService { get; set; }

        public static void Main(string[] args)
        {
            GetServices();
            int counter = 0;
            Console.Write("Quantidade MÍNIMA de átomos na molécula: ");
            var min = Convert.ToInt32(Console.ReadLine());
            Console.Write("Quantidade MÁXIMA de átomos na molécula: ");
            var max = Convert.ToInt32(Console.ReadLine());
            Console.Write("\n");

            if (min >= 2 && min <= 50 && max >= 2 && max <= 50)
            {
                while (true)
                {
                    string formula = string.Empty;
                    int atomsCount = 0;
                    int diferentAtomsCount = 0;
                    int searchCounter = 0;
                    bool fromDataSet = false;
                    var resultCounter = 0;
                    var idStructure = string.Empty;
                    Molecule saved = null;
                    FormulaGenerator fg = new FormulaGenerator();
                    try
                    {
                        var moleculeAtoms = _moleculeService.GetRandomEmpty();
                        formula = moleculeAtoms.Nomenclature;
                        atomsCount = moleculeAtoms.AtomsCount;
                        diferentAtomsCount = moleculeAtoms.DiferentAtomsCount;
                        fromDataSet = moleculeAtoms.FromDataSet;
                        searchCounter = _logService.GetCounter(formula);


                        if (!string.IsNullOrEmpty(formula))
                        {
                            Console.WriteLine(string.Format("Iniciando busca para {0}", formula));

                            var ga = new StructureGenerator(
                                formula,
                                GetPopulationSize(atomsCount, diferentAtomsCount, searchCounter),
                                GetMaxGenerations(atomsCount, diferentAtomsCount, searchCounter),
                                GetMutationRate(atomsCount, diferentAtomsCount, searchCounter)
                            );
                            ga.FindSolutions();
                            if (ga.Finished)
                            {
                                while (ga.ResultList.Count > 0)
                                {
                                    var molecule = ga.ResultList.Dequeue();
                                    if (molecule != null)
                                    {
                                        molecule.ReorganizeLinks();
                                        molecule.SetEnergy();
                                        molecule.FromDataSet = fromDataSet;
                                        molecule.IdStructure = _linkService.GetIdStructure(molecule.LinkEdges);
                                        idStructure = molecule.IdStructure;

                                        if (!string.IsNullOrEmpty(idStructure) && _moleculeService.GetByIdStructure(molecule.Nomenclature, molecule.IdStructure) == null)
                                        {
                                            saved = _moleculeService.Create(molecule);

                                            if (saved != null)
                                            {
                                                counter++;
                                                var empty = _moleculeService.GetByIdStructure(saved.Nomenclature, string.Empty);
                                                if (empty != null)
                                                    _moleculeService.Delete(empty);

                                                resultCounter++;
                                                Console.WriteLine(string.Format("Encontrado {0}", saved.IdStructure));
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine(string.Format("Finalizado busca para {0}", formula));
                            }
                            _logService.NewSearch(formula);
                            Console.Write("\n");
                            Console.Write(string.Format("Você já encontrou {0} moléculas!", counter));
                            Console.Write("\n\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n" + ex.Message + "\n");
                    }
                }
            }
        }

        private static int GetPopulationSize(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            var result = 0;
            if (atomsCount < 5)
                result = 100;
            else if (atomsCount >= 5 && atomsCount < 7)
                result = 150;
            else
                result = 200;
            return result + Convert.ToInt32(result * (searchCounter + 1) * 0.1);
        }

        private static int GetMaxGenerations(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            var result = 0;
            if (atomsCount < 40)
                result = 2000;
            else
                result = 4000;
            return (result * (searchCounter + 1));
        }

        private static double GetMutationRate(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            if ((searchCounter % 2) == 0)
                return 0.20;
            return 0.15;
        }

        private static void GetServices()
        {
            var container = new InjectContainer();
            container.Register<IMoleculeService, MoleculeService>();
            container.Register<IMoleculeRepository, MoleculeRepository>();
            container.Register<ILogService, LogService>();
            container.Register<ILogRepository, LogRepository>();
            container.Register<IAtomService, AtomService>();
            container.Register<ILinkService, LinkService>();

            _moleculeService = container.Resolve<IMoleculeService>();
            _linkService = container.Resolve<ILinkService>();
            _logService = container.Resolve<ILogService>();
        }
    }
}
