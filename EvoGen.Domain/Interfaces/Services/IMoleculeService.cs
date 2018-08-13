using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        int MoleculeCount();
        Molecule Create(MoleculeGraph molecule);
        Molecule GetCollectionFromGraph(MoleculeGraph molecule);
        Molecule GetByIdStructure(string nomenclature, string idStructure);
        List<Cycle> GetMoleculeCycles(Molecule molecule);
    }
}
