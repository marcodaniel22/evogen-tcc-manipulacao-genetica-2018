using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.DataGen
{
    public class ExcelToJson
    {
        private string excelPath;
        private string excelFileName;
        private string jsonPath;
        private string jsonFileName;

        public ExcelToJson()
        {
            this.excelPath = ConfigurationManager.AppSettings["ScriptPath"];
            this.excelFileName = "Elements.xlsx";
            this.jsonFileName = "Elements.json";
            this.jsonPath = ConfigurationManager.AppSettings["ScriptPath"];
        }

        public string ConnectAndConvert()
        {
            try
            {
                using (var stream = File.Open(excelPath + excelFileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dict = new List<Dictionary<string, dynamic>>();
                        reader.Read();
                        var headers = new List<string>();
                        for (int i = 0; i < reader.RowCount; i++)
                        {
                            dict.Add(new Dictionary<string, dynamic>());
                            for (int j = 0; j < reader.FieldCount - 1; j++)
                            {
                                var header = reader.GetString(j);
                                dict[i].Add(header, String.Empty);
                                if (headers.Find(x => x == header) == null)
                                    headers.Add(header);
                            }
                        }
                        var lineCount = 0;
                        do
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < headers.Count; i++)
                                    dict[lineCount][headers[i]] = reader.GetValue(i);
                                lineCount++;
                            }
                        } while (reader.NextResult());

                        var result = JsonConvert.SerializeObject(dict);
                        File.WriteAllText(jsonPath + jsonFileName, result);
                        return "Success!";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
