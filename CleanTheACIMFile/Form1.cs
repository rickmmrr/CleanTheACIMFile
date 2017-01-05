using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace CleanTheACIMFile
{
    public partial class Form1 : Form
    {
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {




                string[] rawACIMFile = null;

                var appSettings = ConfigurationManager.AppSettings;
                string rootPath = appSettings["TextPath"] ?? "Not Found";
                if (rootPath == "Not Found")
                    throw new Exception("Root text path is not set");



                try
                {


                    rawACIMFile = File.ReadAllLines(rootPath + "acim-3.txt");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error = {ex.Message}");
                }

                //start working on the file.

                ExamineACIMFile examine = new ExamineACIMFile(rawACIMFile);

                List<RawDataContainer> rawDataCon = null;
                DuplicateLineItem[] dub = examine.DetermineDuplicateLines(out rawDataCon);

                try
                {

                    List<string> retList = new List<string>();

                    foreach (DuplicateLineItem d in dub)
                    {
                        retList.Add($"{d.Count}   {d.Line}");
                    }


                    //save the file
                    File.WriteAllLines(rootPath + "dubFile.txt", retList.ToArray());


                    StringBuilder sb = new StringBuilder();
                    foreach (RawDataContainer r in rawDataCon)
                    {
                        if (!r.ThrowOut)
                            sb.AppendLine(r.Line);
                    }


                    File.WriteAllLines(rootPath + "rawDataStepOne.txt", new string[] { sb.ToString() });

                }
                catch (Exception exe)
                {
                    Debug.WriteLine($"Error = {exe.Message}");
                }







                MessageBox.Show("Done!");





            }
            catch (Exception re)
            {
                Debug.WriteLine($"Error = {re.Message}");
            }


        }
    }
}
