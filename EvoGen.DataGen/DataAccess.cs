using EvoGen.Repository.Repositories;
using EvoGen.Repository.Service;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvoGen.DataGen
{
    public class DataAccess
    {
        public void UpdateMolecules()
        {
            try
            {
                var serv = new MoleculeService();
                var count = serv.UpdateNomenclature();
                Console.WriteLine(string.Format("{0} molecules were updated!"), count);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
