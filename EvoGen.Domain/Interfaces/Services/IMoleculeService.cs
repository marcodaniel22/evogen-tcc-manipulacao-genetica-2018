using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        int GetMoleculeCount();
        Molecule Create(MoleculeGraph molecule);
        Molecule Delete(Molecule molecule);
        Molecule GetCollectionFromGraph(MoleculeGraph molecule);
        Molecule GetByIdStructure(string nomenclature, string idStructure);
        int GetNotEmptyMoleculeCount(string nomenclature);
        List<Cycle> GetMoleculeCycles(Molecule molecule);
    }
}
