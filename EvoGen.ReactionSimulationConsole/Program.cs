using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.Services;
using EvoGen.Domain.Services.Reactions;
using EvoGen.Domain.ValueObjects.DNA;
using EvoGen.Domain.ValueObjects.MutatedDNA;
using EvoGen.Repository.Repositories;
using Inject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.ReactionSimulationConsole
{
    public class Program
    {

        private static IReplacementReactionService _replacementReactionService;
        private static IMoleculeService _moleculeService;
        private static ILinkService _linksService;

        public static void Main(string[] args)
        {
            GetServices();
            Molecule substract;
            Molecule reagent;
            Molecule target;
            Molecule result;
            string targetId;
            int initAtomsCount;
            bool found;

            while (true)
            {
                Console.Clear();
                substract = null;
                reagent = null;
                target = null;
                result = null;
                targetId = string.Empty;
                initAtomsCount = 2;
                found = false;
                
                Console.WriteLine("Escolha qual molécula danificada do DNA deseja restaurar:");
                Console.WriteLine("1 - Mutação na Adenina");
                Console.WriteLine("2 - Mutação na Citosina");
                Console.WriteLine("3 - Mutação na Guanina");
                Console.WriteLine("4 - Mutação na Timina");
                Console.Write("Opção: ");
                var input = Console.ReadKey();
                Console.WriteLine("\n");

                if (input.KeyChar == '1')
                {
                    target = new Adenine();
                    targetId = _linksService.GetIdStructure(target.Links);
                    substract = new MutatedAdenine1();
                }
                else if (input.KeyChar == '2')
                {
                    target = new Cytosine();
                    targetId = _linksService.GetIdStructure(target.Links);
                    substract = new MutatedCytosine1();
                }
                else if (input.KeyChar == '3')
                {
                    target = new Guanine();
                    targetId = _linksService.GetIdStructure(target.Links);
                    substract = new MutatedGuanine1();
                }
                else if (input.KeyChar == '4')
                {
                    target = new Thymine();
                    targetId = _linksService.GetIdStructure(target.Links);
                    substract = new MutatedThymine1();
                }

                if (target != null)
                {
                    while (initAtomsCount <= 20 && !found)
                    {
                        Console.WriteLine("Separando moléculas para teste de reagente...");
                        var testMolecules = _moleculeService.GetMoleculesByRange(initAtomsCount, initAtomsCount + 2);
                        foreach (var molecule in testMolecules)
                        {
                            reagent = molecule;
                            Console.Write(string.Format("Teste com reagente {0}", reagent.Nomenclature));
                            result = _replacementReactionService.React(reagent, substract);
                            if (result != null)
                            {
                                var resultId = _linksService.GetIdStructure(result.Links);
                                if (resultId.Equals(targetId))
                                {
                                    FoundResult(substract, reagent, result);
                                    found = true;
                                    break;
                                }
                            }
                            Console.Write(" - Falha\n");
                        }
                        initAtomsCount += 2;
                    }
                    if (!found)
                        Console.WriteLine("\nNão foi possível restaurar a molécula danificada.");
                    Console.ReadKey();
                }
            }
        }

        private static void FoundResult(Molecule substract, Molecule reagent, Molecule result)
        {
            Console.WriteLine(string.Format("\n\nReagente {0} conseguiu reparar a molécula danificada!", reagent.Nomenclature));
            Console.WriteLine("Substrato \t\t Reagente \t\t Resultado");
            Console.WriteLine(string.Format("{0} \t\t {1} \t\t\t {2}\n", substract.Nomenclature, reagent.Nomenclature, result.Nomenclature));
            for (int i = 0; i < Math.Max(substract.Links.Count, result.Links.Count); i++)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3} {4}",
                    substract.Links.Count > i ? substract.Links[i].ToString() : "\t",
                    i == 6 ? "\t+\t" : "\t\t",
                    reagent.Links.Count > i ? reagent.Links[i].ToString() : "\t",
                    i == 6 ? "\t=\t" : "\t\t",
                    result.Links.Count > i ? result.Links[i].ToString() : "\t"
                ));
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
            container.Register<IReplacementReactionService, ReplacementReactionService>();
            container.Register<IAddictionReactionService, AddictionReactionService>();

            _replacementReactionService = container.Resolve<IReplacementReactionService>();
            _moleculeService = container.Resolve<IMoleculeService>();
            _linksService = container.Resolve<ILinkService>();
        }
    }
}
