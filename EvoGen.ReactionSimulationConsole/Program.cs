using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.Services;
using EvoGen.Domain.Services.Reactions;
using EvoGen.Domain.ValueObjects.DNA;
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
        public static IReplacementReactionService _replacementReactionService { get; set; }

        public static void Main(string[] args)
        {
            GetServices();
            var cytosine = new Cytosine();
            var substract = cytosine.GetTestSubstract();
            var reagent = cytosine.GetTestReagent();
            var substractLinks = substract.Links.ToList();
            var result = _replacementReactionService.React(reagent, substract);

            Console.WriteLine("Substrato \t\t Reagente \t\t Resultado");
            Console.WriteLine("C4H3ON2Cl \t\t NH3 \t\t\t C4H5ON3\n");
            for (int i = 0; i < Math.Max(substractLinks.Count, result.Links.Count); i++)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3} {4}",
                    substractLinks.Count > i ? substractLinks[i].ToString() : "\t",
                    i == 6 ? "\t+\t" : "\t\t",
                    reagent.Links.Count > i ? reagent.Links[i].ToString() : "\t",
                    i == 6 ? "\t=\t" : "\t\t",
                    result.Links.Count > i ? result.Links[i].ToString() : "\t"
                ));
            }
            Console.ReadKey();

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
            
        }
    }
}
