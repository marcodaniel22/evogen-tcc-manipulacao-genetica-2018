using EvoGen.Repository.Collection;

namespace EvoGen.Repository.Interfaces.Service
{
    public interface IMoleculeService : IServiceBase<Molecule>
    {
        long UpdateNomenclature();
    }
}
