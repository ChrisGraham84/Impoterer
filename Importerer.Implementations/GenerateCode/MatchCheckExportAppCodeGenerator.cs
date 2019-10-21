using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    /// <summary>
    /// This is a class that just a template for the text generator until i make a better solution
    /// </summary>
    class Test
    {
        public void DoThing()
        {
            IDataFromFile CSVFileImporter = new CSVFileImport();
            var OriginRawData = CSVFileImporter.ImportFromFile("SourcePath");
            var CompareRawData = CSVFileImporter.ImportFromFile("ComparePath");

            ICreator<Test> TestCreator = new Creator<Test>();
            var OriginData = TestCreator.CreateTypeList(OriginRawData);
            var CompareData = TestCreator.CreateTypeList(CompareRawData);

            IComparerer<Test> TestMatcher = new MatchSets<Test>();
            var NonMatching = TestMatcher.Compare(OriginData, CompareData);

            IDataToFile<Test> CSVFileExpoerter = new CSVFileExport<Test>();
            CSVFileExpoerter.Export(NonMatching, "OutputPath");
            
        }
    }
    public class MatchCheckExportAppCodeGenerator : IAppCodeGenerator, IMatch
    {
        public string originPath { get; set; }
        public string comparePath { get; set; }
        public string outputPath { get; set; }

        public MatchCheckExportAppCodeGenerator(string OriginPath, string ComparePath, string OutputPath)
        {
            originPath = OriginPath;
            comparePath = ComparePath;
            outputPath = OutputPath;
        }

        public string GenerateAppCode(string TypeName)
        {
            StringBuilder ClassBuilder = new StringBuilder();
            //Using Statements
            ClassBuilder.Append("using System;\n");
            ClassBuilder.Append("using System.Collections.Generic;\n");
            ClassBuilder.Append("using System.Diagnostics.CodeAnalysis;\n");
            ClassBuilder.Append("using System.IO;\n");
            ClassBuilder.Append("using System.Linq;\n");
            ClassBuilder.Append("using System.Reflection;\n");
            ClassBuilder.Append("using System.Text;\n");
            ClassBuilder.Append("using Importerer.Interfaces;\n");
            ClassBuilder.Append("using Importerer.Implementations;\n");
            ClassBuilder.Append("using ImporterType;\n");

            //Namespace
            ClassBuilder.Append("namespace Importerer.Implementations {\n");
            
            //Class declaration
            ClassBuilder.Append("public class MatchCheckExport{\n");

            //Main declaration
            ClassBuilder.Append("static void Main(string[] args){\n");

            //Business Logic Start
            ClassBuilder.Append(" IDataFromFile CSVFileImporter = new CSVFileImport();");
            ClassBuilder.AppendFormat("var OriginRawData = CSVFileImporter.ImportFromFile(@\"{0}\");", originPath);
            ClassBuilder.AppendFormat("var CompareRawData = CSVFileImporter.ImportFromFile(@\"{0}\");", comparePath);
            ClassBuilder.Append("\n");
            ClassBuilder.AppendFormat("ICreator<{0}> {0}Creator = new Creator<{0}>();\n",TypeName);
            ClassBuilder.AppendFormat("var OriginData = {0}Creator.CreateTypeList(OriginRawData);\n", TypeName);
            ClassBuilder.AppendFormat("var CompareData = {0}Creator.CreateTypeList(CompareRawData);\n", TypeName);
            ClassBuilder.Append("");
            ClassBuilder.AppendFormat("IComparerer<{0}> {0}Matcher = new MatchSets<{0}>();\n",TypeName);
            ClassBuilder.AppendFormat("var NonMatching = {0}Matcher.Compare(OriginData, CompareData);\n", TypeName);
            ClassBuilder.Append("");
            ClassBuilder.AppendFormat("IDataToFile<{0}> CSVFileExpoerter = new CSVFileExport<{0}>();\n",TypeName);
            ClassBuilder.AppendFormat("CSVFileExpoerter.Export(NonMatching,@\"{0}\");", outputPath);
            //BUsiness Logic End

            //end Main declaration
            ClassBuilder.Append(" }\n");

            //end class declaration
            ClassBuilder.Append(" }\n");
            
            //end namespace
            ClassBuilder.Append(" }");


            return ClassBuilder.ToString();
        }

    }
}
