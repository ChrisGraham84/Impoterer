﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importerer.Interfaces
{
    public interface IAppCompiler
    {
        string CompileCode(string TypeName, string AppName);
    }
}
