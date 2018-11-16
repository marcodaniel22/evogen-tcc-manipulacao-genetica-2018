using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        int GetMoleculeCount();
        List<Molecule> GetByNomenclature(string nomenclature);
        Molecule GetByIdStructure(string nomenclature, string idStructure);
        Molecule Create(MoleculeGraph molecule);
        Molecule Delete(Molecule molecule);
        int GetNotEmptyMoleculeCount(string nomenclature);
        Molecule GetRandomEmpty(int min, int max);
        Molecule GetRandomToSearch(int min, int max);
        List<Molecule> GetMoleculesByRange(int init, int last);

        Molecule GetCollectionFromGraph(MoleculeGraph molecule);
        List<Cycle> GetMoleculeCycles(Molecule molecule);
        Molecule CloneMolecule(Molecule molecule);
    }
}
