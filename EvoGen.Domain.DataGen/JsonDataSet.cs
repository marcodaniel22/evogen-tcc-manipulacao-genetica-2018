using EvoGen.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace EvoGen.Domain.DataGen
{
    public class JsonDataSet
    {
        public Queue<Dictionary<string, int>> FormulasQueue { get; private set; }
        private Object queueLock = new object();

        public JsonDataSet()
        {
            var jsonPath = ConfigurationManager.AppSettings["ScriptPath"];
            var jsonFiles = new List<string>
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

            this.FormulasQueue = new Queue<Dictionary<string, int>> ();
            this.ConsumeJsonFiles(jsonPath, jsonFiles);
        }

        private void ConsumeJsonFiles(string jsonPath, List<string> jsonFiles)
        {
            var fg = new FormulaGenerator();
            foreach (var jsonFile in jsonFiles)
            {
                using (StreamReader file = File.OpenText(jsonPath + jsonFile + ".json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    foreach (var molecule in JToken.ReadFrom(reader))
                    {
                        try
                        {
                            var atomsMolecule = GetAtoms(molecule);
                            if (atomsMolecule != null && !FormulasQueue.Contains(atomsMolecule))
                                FormulasQueue.Enqueue(atomsMolecule);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private Dictionary<string, int> GetAtoms(JToken json)
        {
            Guard.JTokenValidation("atoms", json);
            var atoms = new Dictionary<string, int>();
            foreach (var atom in json["atoms"])
            {
                var symbol = GetCollectionByJToken(atom);
                if (Constants.ElementFilterList.Contains(symbol))
                {
                    if (!atoms.ContainsKey(symbol))
                    {
                        atoms.Add(symbol, 1);
                    }
                    else
                    {
                        var count = atoms[symbol];
                        atoms[symbol] = count + 1;
                    }
                }
                else
                    return null;
            }
            return atoms;
        }

        private string GetCollectionByJToken(JToken jtoken)
        {
            Guard.JTokenValidation("type", jtoken);
            return jtoken["type"].Value<string>();
        }

        public Dictionary<string, int> GetMolecule()
        {
            try
            {
                lock (queueLock)
                {
                    return FormulasQueue.Dequeue();
                }
            }
            catch (Exception)
            {
                return new Dictionary<string, int>();
            }
        }
    }
}
