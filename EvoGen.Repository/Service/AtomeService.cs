using EvoGen.Repository.Collection;
using EvoGen.Repository.Factory;
using EvoGen.Repository.Interfaces.Service;
using EvoGen.Repository.Repositories;

namespace EvoGen.Repository.Service
{
    public class AtomService : ServiceBase<Atom>, IAtomService
    {
        private static readonly AtomRepository _atomRepository = new AtomRepository();
        private static readonly AtomFactory _atomFactory = new AtomFactory();
        
        public AtomService() : base(_atomRepository, _atomFactory)
        {
            
        } 
    }
}
