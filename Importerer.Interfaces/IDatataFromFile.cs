using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Interfaces
{
    public interface IDataFromFile
    {
        Dictionary<int, List<string>> ImportFromFile(string Path);
    }
}
