using System.Collections.Generic;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Interfaces.Repositories;
using System.Linq;
using System;

namespace EvoGen.Repository.Repositories
{
    public class MoleculeRepository : RepositoryBase<Molecule>, IMoleculeRepository
    {
        public List<Molecule> GetMoleculesUpToFiveXYZ()
        {
            return GetAll().ToList().Where(m => m.Atoms.All(a =>
                Math.Abs(a.X) < 5 && Math.Abs(a.Y) < 5 && Math.Abs(a.Z) < 5)).ToList();
        }
    }
}
