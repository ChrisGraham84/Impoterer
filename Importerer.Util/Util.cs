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

        public static bool CompareColumnNameRow(List<string> SourceNames, List<string> CompareNames)
        {
            return SourceNames.TrueForAll(clmn => CompareNames.Contains(clmn));
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

        public static string GenerateImportType(List<string> ColumnNames, string ImportTypeName)
        {
            StringBuilder ClassBuilder = new StringBuilder();

            //Add all the using statements
            ClassBuilder.Append("using System;");
            ClassBuilder.Append("using System.Collections.Generic;");
            ClassBuilder.Append("using System.Diagnostics.CodeAnalysis;");
            ClassBuilder.Append("using System.IO;");
            ClassBuilder.Append("using System.Linq;");
            ClassBuilder.Append("using System.Reflection;");
            ClassBuilder.Append("using System.Text;");

            ClassBuilder.Append("namespace ImporterType {");
            //Declare the class and make sure it inherits from IEquatable
            ClassBuilder.AppendFormat("public class {0} : IEquatable<{0}>", ImportTypeName);
            ClassBuilder.Append(" {");

            //properties
            foreach (var p in ColumnNames)
            {
                ClassBuilder.Append("	public string " + p + " { get; set; }");
            }
            ClassBuilder.Append(" ");

            //Equatable Method
            ClassBuilder.AppendFormat("	public bool Equals({0} other)", ImportTypeName);
            ClassBuilder.Append("	{");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return false;");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return true;");
            foreach (var p in ColumnNames)
            {
                //first line has the return statemen
                if (ColumnNames.IndexOf(p) == 0)
                {
                    ClassBuilder.AppendFormat("			return {0}.Equals(other.{0})", p);
                }
                //last line needds to have a semicolon
                else if (ColumnNames.IndexOf(p) == ColumnNames.Count - 1)
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0});", p);
                }
                else
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0})", p);
                }

            }
            ClassBuilder.Append("	}");
            ClassBuilder.Append("}");
            ClassBuilder.Append("}");

            return ClassBuilder.ToString();
        }

        public static string GenerateComparerApp(string ImportTypeName, string SourceFilePath, string CompareFilePath, string OutputPath)
        {
            string executer = string.Empty;
            using (StreamReader file = new StreamReader("exectuter.txt"))
            {
                executer = file.ReadToEnd();
            }
            executer = executer.Replace("[ImportTypeName]", ImportTypeName);
            executer = executer.Replace("[SourceFilePath]", SourceFilePath);
            executer = executer.Replace("[CompareFilePath]", CompareFilePath);
            executer = executer.Replace("[OutputPath]", OutputPath);


            return executer;
        }


    }
}
