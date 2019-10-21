using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class EquatableTypeCompiler : ITypeCompiler
    {
        List<string> properties = new List<string>();
        
        public EquatableTypeCompiler(List<string> Properties)
        {
            properties = Properties;
        }

        public string CompileCode(string TypeName)
        {
            string Error = string.Empty;

            ITypeCodeGenerator GenerateMatchExportCode = new TypeEquatableCodeGenerator();
            var typeCode = GenerateMatchExportCode.GenerateTypeCode(properties, TypeName);

            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            string Output = $"{TypeName}.dll";

            String[] referenceAssemblies = { "System.Linq.dll", "System.Reflection.dll" };
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters(referenceAssemblies, Output, false);
            //Generate a DLL
            parameters.GenerateExecutable = false;

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, typeCode);

            if (results.Errors.Count > 0)
            {
               
                foreach (CompilerError CompErr in results.Errors)
                {
                    Error += "Line number" + CompErr.Line + ", Error Number:"
                        + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";"
                        + Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                return "success";   
            }

            return Error;
        }
    }
}
