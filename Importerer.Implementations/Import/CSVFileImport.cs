using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class CSVFileImport : IDataFromFile
    {
        public Dictionary<int, List<string>> ImportFromFile(string Path)
        {
            string line;
            int count = 1;
            Dictionary<int, List<string>> dictRecords = new Dictionary<int, List<string>>();


            // Read the file and display it line by line.
            using (StreamReader file = new StreamReader(Path))
            {
                while ((line = file.ReadLine()) != null)
                {
                    List<string> sepList = new List<string>();
                    char[] delimiters = new char[] { ',' };
                    string[] parts = line.Replace(",", ", ").Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++)
                    {

                        //Console.WriteLine(parts[i]);
                        sepList.Add(parts[i].Trim());

                    }
                    dictRecords.Add(count, sepList);
                    count++;
                }

                file.Close();
            }
            return (dictRecords);
        }
    }
}
