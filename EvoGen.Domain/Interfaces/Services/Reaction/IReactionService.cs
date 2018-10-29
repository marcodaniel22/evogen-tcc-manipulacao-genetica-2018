using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IReactionService
    {
        Molecule React(Molecule reagent, Molecule substract);
        List<Link> GetSubstractLinks(List<Link> substractOutCycleLinks);
        List<Link> GetReagentLinks(List<Link> reagentOutCycleLinks);
        Atom GetSubstractTargetAtom(Link link);
        Atom GetReagentTargetAtom(Link link);
    }
}
