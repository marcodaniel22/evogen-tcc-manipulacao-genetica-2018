using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.DataGen
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 0;
            while (input >= 0)
            {
                try
                {
                    Console.WriteLine("\n1 - Convert the Excel file in a Json file");
                    Console.WriteLine("2 - Load element dataset and save it in MongoDb");
                    Console.WriteLine("3 - Load molecule dataset and save it in MongoDb");
                    Console.WriteLine("4 - Get all molecules from MongoDb");
                    Console.Write("Option: ");
                    var key = Console.ReadKey().KeyChar.ToString();
                    input = Convert.ToInt32(key);
                    var dataConverter = new DataSetToMongo();
                    var dataAccess = new DataAccess();
                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("\nConverting...");
                            var excelConverter = new ExcelToJson();
                            Console.WriteLine(excelConverter.ConnectAndConvert());
                            break;
                        case 2:
                            Console.WriteLine("\nProcessing...");
                            Console.WriteLine(dataConverter.ConsumeJsonAndSave("Element"));
                            break;
                        case 3:
                            Console.WriteLine("\nProcessing...");
                            Console.WriteLine(dataConverter.ConsumeJsonAndSave("Molecule"));
                            break;
                        case 4:
                            Console.WriteLine("\nProcessing...");
                            //dataAccess.GetMolecules();
                            Console.WriteLine("Success!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    input = -1;
                }
            }
        }
    }
}
