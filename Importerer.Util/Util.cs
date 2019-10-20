using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Util
{
    public static class Util
    {

        public static string Talk()
        {
            return "Hello From Util";
        }
        public static List<T> ImportListFromCSVFile<T>(Dictionary<int, List<string>> Data)
        {
            List<T> data = new List<T>();

            var rawdata = Data.ToDictionary(x => x.Key, x => x.Value);

            foreach (var v in rawdata.Skip(1))
            {
                T obj = (T)Activator.CreateInstance(typeof(T), null);
                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        object value = new object();

                        if (prop.PropertyType.Name == "Int32")
                        {
                            value = int.Parse(v.Value[rawdata.Values.ToList()[0].IndexOf(prop.Name)]);
                        }
                        else
                        {
                            value = v.Value[rawdata.Values.ToList()[0].IndexOf(prop.Name)];
                        }
                        var propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, value, null);
                    }
                    catch
                    {
                        continue;
                    }
                }
                data.Add(obj);
            }
            return (data);

        }

        public static Dictionary<int, List<string>> ImportCSVFile(string filepath)
        {

            string line;
            int count = 1;
            Dictionary<int, List<string>> dictRecords = new Dictionary<int, List<string>>();


            // Read the file and display it line by line.
            using (StreamReader file = new StreamReader(filepath))
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

        public static List<T> ImportListFromCSVFile<T>(string filepath)
        {
            List<T> data = new List<T>();

            var rawdata = ImportCSVFile(filepath).ToDictionary(x => x.Key, x => x.Value);

            foreach (var v in rawdata.Skip(1))
            {
                T obj = (T)Activator.CreateInstance(typeof(T), null);
                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        object value = new object();

                        if (prop.PropertyType.Name == "Int32")
                        {
                            value = int.Parse(v.Value[rawdata.Values.ToList()[0].IndexOf(prop.Name)]);
                        }
                        else
                        {
                            value = v.Value[rawdata.Values.ToList()[0].IndexOf(prop.Name)];
                        }
                        var propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, value, null);
                    }
                    catch
                    {
                        continue;
                    }
                }
                data.Add(obj);
            }
            return (data);

        }
    }
}
