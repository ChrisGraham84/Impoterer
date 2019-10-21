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
using Importerer.Util;
using Importerer.Implementations;
using Importerer.Interfaces;

namespace Importerer
{
    public partial class Form1 : Form
    {
        Dictionary<int, List<string>> _SourceDictionary = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> _CompareDictionary = new Dictionary<int, List<string>>();

        public Form1()
        {
            InitializeComponent();
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
                    IDataFromFile CSVParser = new CSVFileImport();

                    _SourceDictionary = CSVParser.ImportFromFile(SourceFilePath);
                    _CompareDictionary = CSVParser.ImportFromFile(CompareFilePath);

                    //check to see if the column headers match
                    if(Util.Util.CompareColumnNameRow(_SourceDictionary[1], _CompareDictionary[1]))
                    {
                        string TypeError = string.Empty;
                        ITypeCompiler CodeCompiler = new EquatableTypeCompiler(_SourceDictionary[1]);
                        TypeError = CodeCompiler.CompileCode(ImportTypeName);

                        if(TypeError.ToLower() == "success")
                        {                        
                            txtErrorLog.ForeColor = Color.Blue;
                            txtErrorLog.Text = "Type Compiled Successfully!";

                            string AppError = string.Empty;
                            string AppName = "MatchCheckExportApp.exe";
                            IAppCompiler appCompiler = new MatchCheckExportAppCompiler(txtSourceFile.Text, txtCompareFile.Text, txtOutputPath.Text);
                            AppError = appCompiler.CompileCode(ImportTypeName, AppName);
                            if (AppError.ToLower() == "success")
                            {
                                txtErrorLog.ForeColor = Color.Blue;
                                txtErrorLog.Text = "App Compiled Successfully Attempting To Execute!";
                                txtErrorLog.Text += Environment.NewLine;
                                try
                                {
                                    Process.Start(AppName);
                                }
                                catch(Exception ex)
                                {
                                    txtErrorLog.ForeColor = Color.Red;
                                    txtErrorLog.Text = ex.ToString();
                                }
                            }
                            else
                            {
                                txtErrorLog.ForeColor = Color.Red;
                                txtErrorLog.Text = AppError;
                            }
                        }
                        else
                        {
                            txtErrorLog.ForeColor = Color.Red;
                            txtErrorLog.Text = TypeError;
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
    }
}
