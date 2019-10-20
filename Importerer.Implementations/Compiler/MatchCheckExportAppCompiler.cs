using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    public class MatchCheckExportAppCompiler : IAppCompiler, IMatch
    {
        public string originPath { get; set; }
        public string comparePath { get; set; }
        public string outputPath { get; set; }

        public MatchCheckExportAppCompiler(string OriginPath, string ComparePath, string OutputPath)
        {
            originPath = OriginPath;
            comparePath = ComparePath;
            outputPath = OutputPath;
        }
        public string CompileCode(string TypeName, string AppName)
        {
            string Errors = string.Empty;

            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
           // string AppName = "MatchCheckExport.exe";
            //System.Core is needed to access System.Linq for some reason
            //Add Impoterer.Util and THe ImportType dll
            String[] referenceAssembliesExe = { "System.Core.dll", "System.Linq.dll", "System.Reflection.dll", 
                "Importerer.Util.dll", "Importerer.Interfaces.dll","Importerer.Implementations.dll", TypeName + ".dll" };
            CompilerParameters parameters = new CompilerParameters(referenceAssembliesExe, AppName, false);
            //Make sure we generate an EXE not a DLL
            parameters.GenerateExecutable = true;

            IAppCodeGenerator CodeGenerator = new MatchCheckExportAppCodeGenerator(originPath, comparePath, outputPath);
            string appCode = CodeGenerator.GenerateAppCode(TypeName);
            Console.WriteLine(appCode);
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, appCode);

            if (results.Errors.Count > 0)
            {
               
                foreach (CompilerError CompErr in results.Errors)
                {
                   Errors += "Line number" + CompErr.Line + ", Error Number:"
                        + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";"
                        + Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                Errors = "Succes";
            }

            return Errors;
        }
    }
}
