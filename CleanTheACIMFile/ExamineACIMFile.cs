using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTheACIMFile
{

    public class DuplicateLineItem
    {
        public int Count { get; set; }
        public string Line { get; set; }
        public List<int> LocInFile { get; set; }
       
    }
    /// <summary>
    /// Store the ACIM File
    /// 
    /// </summary>
    public class RawDataContainer
    {
        public bool ThrowOut { get; set; }
        public string Line { get; set; }
        public int OriginalFileLoc { get; set; }
    }






    public class ExamineACIMFile
    {
        //hold entire ACIM File
        string[] _rawFile = null;

        public ExamineACIMFile(string[] rawFile)
        {
            _rawFile = rawFile;
        }

        public DuplicateLineItem[] DetermineDuplicateLines(out List<RawDataContainer> managedFile)
        {
            managedFile = new List<RawDataContainer>();
            //add the entire file to managedFile
            int c = 1;
            foreach (string str in _rawFile)
            {
                managedFile.Add(new RawDataContainer {Line = str, ThrowOut = false, OriginalFileLoc = c++ });
            }


            Dictionary<string, DuplicateLineItem> dubDic = new Dictionary<string, DuplicateLineItem>();

            // first try it without getting rid of white space


            // process the File
            //*********************************************************************
            int locInFile = 0;
            foreach (string s in _rawFile)
            {
                locInFile++;

                string str = s;



                if (!dubDic.ContainsKey(str))
                {
                    //new item
                    DuplicateLineItem item = new DuplicateLineItem
                    {
                        Count = 1,
                        Line = str,
                        LocInFile = new List<int>()
                    };
                    item.LocInFile.Add(locInFile);
                    dubDic.Add(item.Line, item);


                }
                else
                {
                    //existing
                    dubDic[str].Count += 1;
                    dubDic[str].LocInFile.Add(locInFile);
                }
            }
            //*************************************************************************

           

                    
            //Return the data
            //***************************************************************
            List<DuplicateLineItem> dubList = new List<DuplicateLineItem>();
            foreach(DuplicateLineItem d in dubDic.Values)
            {
                //only add 4 or more
                if(d.Count > 3)
                {
                    //set the throwout flag
                    if(d.Count > 699 & d.Count < 900)
                    {
                        foreach(int lineNum in d.LocInFile)
                        {
                            managedFile[lineNum - 1].ThrowOut = true;
                        }
                    }


                    //make the list
                    dubList.Add(d);
                }
                   
            }
            return dubList.ToArray();
            //***********************************************************************
        }
    }
}
