using EvoGen.Domain.Collections;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        int MoleculeCount();
    }
}
