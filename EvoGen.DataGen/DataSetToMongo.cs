using EvoGen.Repository.Collection;
using EvoGen.Repository.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EvoGen.DataGen
{
    public class DataSetToMongo
    {
        private string jsonPath;
        private List<string> jsonFiles;

        public DataSetToMongo()
        {
            this.jsonPath = ConfigurationManager.AppSettings["ScriptPath"];
            this.jsonFiles = new List<string>
            {
                "pubChem_p_00000001_00025000",
                "pubChem_p_00025001_00050000",
                "pubChem_p_00050001_00075000",
                "pubChem_p_00075001_00100000",
                "pubChem_p_00100001_00125000",
                "pubChem_p_00125001_00150000",
                "pubChem_p_00150001_00175000",
                "pubChem_p_00175001_00200000",
                "pubChem_p_00200001_00225000",
                "pubChem_p_00225001_00250000"
            };
        }

        public string ConsumeJsonAndSave(string target)
        {
            switch (target)
            {
                case "Molecule": return MoleculeTarget();
                case "Element": return ElementTarget();
            }
            return "Undefined target";
        }

        private string MoleculeTarget()
        {
            int count = 0;
            var service = new MoleculeService();
            foreach (var jsonFile in jsonFiles)
            {
                using (StreamReader file = File.OpenText(jsonPath + jsonFile + ".json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    foreach (var molecule in JToken.ReadFrom(reader))
                    {
                        try
                        {
                            var mol = service.Create(molecule);
                            count++;

                        }
                        catch (Exception) { }
                    }
                }
            }
            return String.Format("{0} molecules saved!", count);
        }

        private string ElementTarget()
        {
            var list = new List<Element>();
            var service = new ElementService();
            var json = File.ReadAllText(jsonPath + "Elements.json");
            var jtoken = JsonConvert.DeserializeObject<JToken>(json);
            foreach (var element in jtoken)
            {
                try
                {
                    list.Add(service.Create(element));
                }
                catch (Exception) { }
            }
            return String.Format("{0} elements saved!", list.Count);
        }
    }
}
