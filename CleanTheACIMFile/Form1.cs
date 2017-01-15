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

      string _rootPath;

        public Form1()
        {
            InitializeComponent();

         var appSettings = ConfigurationManager.AppSettings;
         _rootPath = appSettings["TextPath"] ?? "Not Found";
         if(_rootPath == "Not Found")
            throw new Exception("Root text path is not set");

      }

      private void button1_Click(object sender, EventArgs e)
        {
            try
            {




                string[] rawACIMFile = null;



                try
                {


                    rawACIMFile = File.ReadAllLines(_rootPath + "acim-3.txt");
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
                    File.WriteAllLines(_rootPath + "dubFile.txt", retList.ToArray());


                    StringBuilder sb = new StringBuilder();
                    foreach (RawDataContainer r in rawDataCon)
                    {
                        if (!r.ThrowOut)
                            sb.AppendLine(r.Line);
                    }


                    File.WriteAllLines(_rootPath + "rawDataStepOne.txt", new string[] { sb.ToString() });

                }
                catch (Exception exe)
                {
                    Debug.WriteLine($"Error = {exe.Message}");
                }



            string[] check = ExamineACIMFile.SaveAndRemovePageNums(_rootPath + "rawDataStepOne.txt");

            File.WriteAllLines(_rootPath + "Line-Headers.txt", check);


            MessageBox.Show("Done!");





            }
            catch (Exception re)
            {
                Debug.WriteLine($"Error = {re.Message}");
            }


        }

      private void button2_Click( object sender, EventArgs e ) {



         ReadInTextFile t = new ReadInTextFile(_rootPath, "rawDataACIMText.txt");

        

         MessageBox.Show("Done!");

         this.Close();

      }
   }
}
