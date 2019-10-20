using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Interfaces
{
    public interface IMatch
    {
        string originPath { get; set; }
        string comparePath { get; set; }
        string outputPath { get; set; }

    }
}
