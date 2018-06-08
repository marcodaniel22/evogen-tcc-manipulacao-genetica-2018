using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvoGen.Helper;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Factory;
using EvoGen.Repository.Interfaces.Service;
using EvoGen.Repository.Repositories;

namespace EvoGen.Repository.Service
{
    public class MoleculeService : ServiceBase<Molecule>, IMoleculeService
    {
        private static readonly MoleculeRepository _moleculeRepository = new MoleculeRepository();
        private static readonly MoleculeFactory _moleculeFactory = new MoleculeFactory();
        
        public MoleculeService() : base(_moleculeRepository, _moleculeFactory)
        {
            
        }

        public long UpdateNomenclature()
        {
            long counter = 0;
            Regex regex = new Regex(@"[a-zA-Z]1[^0-9]|[a-zA-Z]1\b");
            var toUpdate = _moleculeRepository.GetAll().ToList()
                .Where(x => regex.Match(x.Nomenclature).Success).ToList();
            foreach (var item in toUpdate)
            {
                item.setNomenclature(item.Atoms);
                counter += _moleculeRepository.Update(item);
            }
            return counter;
        }
    }
}
