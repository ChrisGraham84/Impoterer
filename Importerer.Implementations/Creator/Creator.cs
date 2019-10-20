using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class Creator<T> : ICreator<T>
    {
        public List<T> CreateTypeList(Dictionary<int, List<string>> Data)
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
    }
}
