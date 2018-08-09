using EvoGen.Domain.Collections;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        int MoleculeCount();
        Molecule Create(MoleculeGraph molecule);
        Molecule GetCollectionFromGraph(MoleculeGraph molecule);
        Molecule GetByIdStructure(string nomenclature, string idStructure);
    }
}
