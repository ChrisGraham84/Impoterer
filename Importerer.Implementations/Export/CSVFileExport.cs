using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class CSVFileExport<T> : IDataToFile<T>
    {
        public void Export(List<T> Data, string OutputPath)
        {
            StringBuilder sbExceptions = new StringBuilder();
            var props = Data[0].GetType().GetProperties().ToList();
            string strArg = "";
            foreach (var prop in props)
            {
                if (props.IndexOf(prop) == props.Count - 1)
                {
                    sbExceptions.AppendFormat("{0}\n", prop.Name);
                    strArg += "{" + props.IndexOf(prop) + "}\n";
                }
                else
                {
                    sbExceptions.AppendFormat("{0},", prop.Name);
                    strArg += "{" + props.IndexOf(prop) + "},";
                }
            }
            foreach (var x in Data)
            {
                foreach (var p in props)
                {
                    if (props.IndexOf(p) == props.Count - 1)
                    {
                        sbExceptions.AppendFormat("{0}\n", x.GetType().GetProperty(p.Name).GetValue(x));
                    }
                    else
                    {
                        sbExceptions.AppendFormat("{0},", x.GetType().GetProperty(p.Name).GetValue(x));
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(OutputPath + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".csv"))
            {
                sw.Write(sbExceptions.ToString());
            }
        }
    }
}
