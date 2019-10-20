using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Interfaces
{
    public interface ICreator<T>
    {
        List<T> CreateTypeList(Dictionary<int, List<string>> Data);
    }
}
