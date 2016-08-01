using DbToEntity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbToEntity
{
    class Program
    {
        public static string DbName { get; set; }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=====================");
                Console.WriteLine("DbToEntity Start...");
                Console.WriteLine("=====================");

                var tbs = DbHelper.GetTablesName();
                DbName = tbs.Item1;
                foreach (var table in tbs.Item2)
                {
                    Console.WriteLine("Processing:"+ table.Name);
                    var cols = DbHelper.GetColumns(table.Name);
                    FileHelper.CreateFile(table.Name,cols,tbs.Item1);
                }

                Console.WriteLine("=====================");
                Console.WriteLine("DbToEntity Success!");
                Console.WriteLine("Path:"+ AppDomain.CurrentDomain.BaseDirectory + "\\Results\\");
                Console.WriteLine("=====================");
                Console.WriteLine("Press Any Key To Exit!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DbToEntity Error:" + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }
    }
}
