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
        string typeName { get; set ; }
        List<string> properties = new List<string>();
        
        public EquatableTypeCompiler(string TypeName, List<string> Properties)
        {
            typeName = TypeName;
            properties = Properties;

        }

        public string CompileCode(string TypeName)
        {
            string Error = string.Empty;

            ITypeCodeGenerator GenerateMatchExportCode = new TypeEquatableCodeGenerator();
            var typeCode = GenerateMatchExportCode.GenerateTypeCode(properties, typeName);

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
                return "Success!";   
            }

            return Error;
        }
    }
}
