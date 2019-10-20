using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Interfaces
{
    public interface IComparerer<T>
    {
        List<T> Compare (List<T> Source, List<T> Compare);
    }
}
