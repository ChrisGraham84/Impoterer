using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Importerer
{
    public partial class Form1 : Form
    {
        Dictionary<int, List<string>> _SourceDictionary = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> _CompareDictionary = new Dictionary<int, List<string>>();

        public Form1()
        {
            InitializeComponent();
            //Initialize some values for test
            txtImportTypeName.Text = "SetCat";
            txtOutputPath.Text = @"C:\Repos\CSVComparer\tests\";
            txtSourceFile.Text = @"C:\Repos\CSVComparer\tests\setcat_full.csv";
            txtCompareFile.Text = @"C:\Repos\CSVComparer\tests\setcat.csv";

        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            //Make sure that we don't have an empty fields
            if(txtImportTypeName.Text != string.Empty 
                && txtOutputPath.Text != string.Empty 
                && txtSourceFile.Text != string.Empty
                && txtCompareFile.Text != string.Empty)
            {
                //a little sanitization
                string ImportTypeName = txtImportTypeName.Text.Replace(" ", "").Replace("'", "").Replace("-", "").Replace("*", "");
                
                //Set up some paths
                string SourceFilePath = txtSourceFile.Text;
                string CompareFilePath = txtCompareFile.Text;
                string OutputPath = txtOutputPath.Text;


                if(File.Exists(SourceFilePath) && File.Exists(CompareFilePath))
                {
                    _SourceDictionary = ImportCSVFile(SourceFilePath);
                    _CompareDictionary = ImportCSVFile(CompareFilePath);

                    //check to see if the column headers match
                    if(_SourceDictionary[1].TrueForAll(clmn => _CompareDictionary[1].Contains(clmn)))
                    {
                        List<string> ColumnNames = _SourceDictionary[1];
                        txtErrorLog.Text = "Column Names: " + Environment.NewLine;
                        ColumnNames.ForEach(clmn => txtErrorLog.Text += "   " + clmn + Environment.NewLine);
                        
                        
                        var ClassCode = GenerateImportType(ColumnNames, ImportTypeName);
                        txtErrorLog.Text += Environment.NewLine;
                        txtErrorLog.Text += Environment.NewLine;
                        txtErrorLog.Text += ClassCode;


                        //Begin to build the class dll
                        CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
                        string Output = $"{ImportTypeName}.dll";

                        txtErrorLog.Text = "";
                        String[] referenceAssemblies = { "System.Linq.dll","System.Reflection.dll"};
                        System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters(referenceAssemblies,Output,false);
                        //Make sure we generate an EXE not a DLL
                        parameters.GenerateExecutable = false;
                      
                        CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, ClassCode);

                        if(results.Errors.Count > 0)
                        {
                            txtErrorLog.ForeColor = Color.Red;
                            foreach (CompilerError CompErr in results.Errors)
                            {
                                txtErrorLog.Text = txtErrorLog.Text + "Line number" + CompErr.Line + ", Error Number:"
                                    + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";"
                                    + Environment.NewLine + Environment.NewLine;
                            }
                        }
                        else
                        {
                            //Successful Compile
                            txtErrorLog.ForeColor = Color.Blue;
                            txtErrorLog.Text = "Success!";

                            string app_base = AppDomain.CurrentDomain.BaseDirectory;
                            var dll = Assembly.LoadFile(app_base + Output);

                            Type ty = dll.GetExportedTypes()[0];
                            var c = Activator.CreateInstance(ty);

                            foreach (var prop in c.GetType().GetProperties())
                            {
                                txtErrorLog.Text += prop.Name + Environment.NewLine;
                            }


                            //run an exe for comparer
                            string Executer = "Compare.exe";
                            //System.Core is needed to access System.Linq for some reason
                            //Add Impoterer.Util and THe ImportType dll
                            String[] referenceAssembliesExe = { "System.Core.dll", "System.Linq.dll", "System.Reflection.dll", "Importerer.Util.dll", Output };
                            parameters = new CompilerParameters(referenceAssembliesExe, Executer, false);
                            //Make sure we generate an EXE not a DLL
                            parameters.GenerateExecutable = true;
                            string appCode = GenerateComparerApp(ImportTypeName);
                            txtErrorLog.Text = appCode;
                            results = codeProvider.CompileAssemblyFromSource(parameters, appCode);

                            if (results.Errors.Count > 0)
                            {
                                txtErrorLog.ForeColor = Color.Red;
                                foreach (CompilerError CompErr in results.Errors)
                                {
                                    txtErrorLog.Text = txtErrorLog.Text + "Line number" + CompErr.Line + ", Error Number:"
                                        + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";"
                                        + Environment.NewLine + Environment.NewLine;
                                }
                            }
                            else
                            {
                                txtErrorLog.ForeColor = Color.Blue;
                                txtErrorLog.Text = "Success!";
                                txtErrorLog.Text += Environment.NewLine;
                                Process.Start(Executer);
                            }

                            /*foreach (Type type in dll.GetExportedTypes())
                            {
                                dynamic c = Activator.CreateInstance(type);
                                txtErrorLog.Text = c.Woof();
                            }*/
                        }
                    }
                }
                else
                {
                    txtErrorLog.Text = "One or More selected files do not exist";
                }
               
            }
            else 
            {
                txtErrorLog.Text = "Not all required fields are filled out";
            }
        }

        public static Dictionary<int, List<string>> ImportCSVFile(string filepath)
        {

            string line;
            int count = 1;
            Dictionary<int, List<string>> dictRecords = new Dictionary<int, List<string>>();


            // Read the file and display it line by line.
            using (StreamReader file = new StreamReader(filepath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    List<string> sepList = new List<string>();
                    char[] delimiters = new char[] { ',' };
                    string[] parts = line.Replace(",", ", ").Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < parts.Length; i++)
                    {

                        //Console.WriteLine(parts[i]);
                        sepList.Add(parts[i].Trim());

                    }
                    dictRecords.Add(count, sepList);
                    count++;
                }

                file.Close();
            }
            return (dictRecords);
        }

       
        public string GenerateComparerApp(string ImportTypeName)
        {
            string executer = string.Empty;
            using (StreamReader file = new StreamReader("exectuter.txt"))
            {
                executer = file.ReadToEnd();
            }
            executer = executer.Replace("[ImportTypeName]", ImportTypeName);
            executer = executer.Replace("[SourceFilePath]", txtSourceFile.Text);
            executer = executer.Replace("[CompareFilePath]", txtCompareFile.Text);
            executer = executer.Replace("[OutputPath]", txtOutputPath.Text);


            return executer; 
        }

        public string GenerateImportType(List<string> ColumnNames, string ImportTypeName)
        {
            StringBuilder ClassBuilder = new StringBuilder();

            //Add all the using statements
            ClassBuilder.Append("using System;");
            ClassBuilder.Append("using System.Collections.Generic;");
            ClassBuilder.Append("using System.Diagnostics.CodeAnalysis;");
            ClassBuilder.Append("using System.IO;");
            ClassBuilder.Append("using System.Linq;");
            ClassBuilder.Append("using System.Reflection;");
            ClassBuilder.Append("using System.Text;");

            ClassBuilder.Append("namespace ImporterType {");
            //Declare the class and make sure it inherits from IEquatable
            ClassBuilder.AppendFormat("public class {0} : IEquatable<{0}>", ImportTypeName);
            ClassBuilder.Append(" {");

            //properties
            foreach (var p in ColumnNames)
            {
                ClassBuilder.Append("	public string " + p + " { get; set; }");
            }
            ClassBuilder.Append(" ");

            //Equatable Method
            ClassBuilder.AppendFormat("	public bool Equals({0} other)", ImportTypeName);
            ClassBuilder.Append("	{");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return false;");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return true;");
            foreach (var p in ColumnNames)
            {
                //first line has the return statemen
                if (ColumnNames.IndexOf(p) == 0)
                {
                    ClassBuilder.AppendFormat("			return {0}.Equals(other.{0})", p);
                }
                //last line needds to have a semicolon
                else if (ColumnNames.IndexOf(p) == ColumnNames.Count - 1)
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0});", p);
                }
                else
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0})", p);
                }

            }
            ClassBuilder.Append("	}");
            ClassBuilder.Append("}");
            ClassBuilder.Append("}");

            return ClassBuilder.ToString();
        }
    }
}
