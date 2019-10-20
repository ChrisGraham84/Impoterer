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
            
            //Namespace
            ClassBuilder.Append("namespace Importerer.Implementations {\n");
            
            //Class declaration
            ClassBuilder.Append("public class MatchCheckExport{\n");

            //Main declaration
            ClassBuilder.Append("static void Main(string[] args){\n");

            //Business Logic Start
            ClassBuilder.Append(" IDataFromFile CSVFileImporter = new CSVFileImport();");
            ClassBuilder.AppendFormat("var OriginRawData = CSVFileImporter.ImportFromFile(@\"{0}\");", originPath);
            ClassBuilder.AppendFormat("var CompareRawData = CSVFileImporter.ImportFromFile(@\"{ 0}\");", comparePath);
            ClassBuilder.Append("");
            ClassBuilder.Append("ICreator<Test> TestCreator = new Creator<Test>();");
            ClassBuilder.Append("var OriginData = TestCreator.CreateTypeList(OriginRawData);");
            ClassBuilder.Append("var CompareData = TestCreator.CreateTypeList(CompareRawData);");
            ClassBuilder.Append("");
            ClassBuilder.Append("IComparerer<Test> TestMatcher = new MatchSets<Test>();");
            ClassBuilder.Append("var NonMatching = TestMatcher.Compare(OriginData, CompareData);");
            ClassBuilder.Append("");
            ClassBuilder.Append("IDataToFile<Test> CSVFileExpoerter = new CSVFileExport<Test>();");
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
