using EvoGen.Repository.Collection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoGen.Repository.Interfaces.Repositories
{
    public interface IMoleculeRepository : IRepositoryBase<Molecule>
    {
        List<Molecule> GetMoleculesUpToFiveXYZ();
    }
}
