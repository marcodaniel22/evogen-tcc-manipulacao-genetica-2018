using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Domain.Services.Reactions
{
    public class AddictionReactionService : IAddictionReactionService
    {
        private readonly IMoleculeService _moleculeService;
        private readonly ILinkService _linkService;

        public AddictionReactionService(IMoleculeService moleculeService, ILinkService linkService)
        {
            this._moleculeService = moleculeService;
            this._linkService = linkService;
        }
        
        public Molecule React(Molecule reagent, Molecule substract)
        {
            var reagentCycles = _moleculeService.GetMoleculeCycles(reagent);
            var substractCycles = _moleculeService.GetMoleculeCycles(substract);
            if (substractCycles.Count >= 0 && substractCycles.Count <= 2)
            {
                var substractOutCycleLinks = _linkService.GetOutCycleLinks(substract, substractCycles);
                var reagentOutCycleLinks = _linkService.GetOutCycleLinks(reagent, reagentCycles);

                var carbonLinks = GetSubstractLinks(substractOutCycleLinks);
                if (carbonLinks.Count >= 2)
                {
                    var visitedCarbonLink = new List<Link>();
                    foreach (var carbonLink in carbonLinks)
                    {
                        if (!visitedCarbonLink.Contains(carbonLink))
                        {
                            visitedCarbonLink.Add(carbonLink);
                            var sameCarbonLinks = carbonLinks.Where(x => x.From.AtomId == carbonLink.From.AtomId && x.To.AtomId == carbonLink.To.AtomId).ToList();

                            //TODO
                        }
                    }
                }
            }
            return substract;
        }
        
        public List<Link> GetReagentLinks(List<Link> reagentOutCycleLinks)
        {
            throw new System.NotImplementedException();
        }

        public List<Link> GetSubstractLinks(List<Link> substractOutCycleLinks)
        {
            return substractOutCycleLinks.Where(x => x.From.Symbol == "C" && x.To.Symbol == "C").ToList();
        }

        public Atom GetSubstractTargetAtom(Link link)
        {
            throw new System.NotImplementedException();
        }

        public Atom GetReagentTargetAtom(Link link)
        {
            throw new System.NotImplementedException();
        }
    }
}
