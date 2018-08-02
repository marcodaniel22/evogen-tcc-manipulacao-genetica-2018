using System;
using System.Linq;
using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;

namespace EvoGen.Domain.Services
{
    public class MoleculeService : ServiceBase<Molecule>, IMoleculeService
    {
        private readonly IMoleculeRepository _moleculeRepository;

        public MoleculeService(IMoleculeRepository moleculeRepository)
            : base(moleculeRepository)
        {
            this._moleculeRepository = moleculeRepository;
        }

        public int MoleculeCount()
        {
            return _moleculeRepository.GetAll().Count();
        }
    }
}
