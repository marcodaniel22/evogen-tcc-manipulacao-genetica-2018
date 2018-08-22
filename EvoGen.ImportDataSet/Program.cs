using EvoGen.Domain.Collections;
using EvoGen.Domain.DataGen;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Repository.Repositories;
using Inject;
using System;
using System.Linq;
using System.Threading;

namespace EvoGen.ImportDataSet
{
    public class Program
    {
        private static IMoleculeService moleculeService;

        public static void Main(string[] args)
        {
            Console.WriteLine("Importar moléculas de arquivos JSON?");
            Console.Write("(S)Sim (N)Não: ");
            var key = Console.ReadKey();
            if (key.KeyChar.ToString().ToUpper() == "S")
            {
                try
                {
                    GetServices();
                    var fg = new FormulaGenerator();
                    var JsonLoader = new JsonDataSet();
                    new Thread(() =>
                    {
                        JsonLoader.ConsumeJsonFiles();
                    }).Start();

                    var counter = 1;
                    while (JsonLoader.FormulasQueue.Count > 0 || !JsonLoader.Finished)
                    {
                        if (JsonLoader.FormulasQueue.Count > 0)
                        {
                            var molecule = JsonLoader.GetMolecule();
                            var formula = fg.GetFormulaFromMolecule(molecule);
                            if (moleculeService.GetByNomenclature(formula).Count == 0 
                                && (molecule.Sum(x => x.Value) >= 2 && molecule.Sum(x => x.Value) <= 50))
                            {
                                Console.WriteLine(string.Format("{0} - Salvando molécula {1}", counter++, formula));
                                moleculeService.Create(new Molecule()
                                {
                                    AtomsCount = molecule.Sum(x => x.Value),
                                    DiferentAtomsCount = molecule.Count,
                                    Nomenclature = formula,
                                    Searched = false,
                                    FromDataSet = true
                                });
                            }
                        }
                        else Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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

            moleculeService = container.Resolve<IMoleculeService>();
        }
    }
}
