using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanTheACIMFile {
   public class ReadInTextFile {
      string[] _file;

      Regex _pageChecker;
      Regex _chapterChecker;
      Regex _lessonChecker;
      Regex _startParaChecker;


      enum LineInfo { Page_Number,Chapter_Number,Chater_Name,Chapter_section_Name,
      First_Line_in_paragraph, Continues_Line, Major_Section_Name,Not_Sure}

      public ReadInTextFile(string fullPath ) {

         try {
            //init regex expresions once
            //page
            _pageChecker = new Regex(@"^Page.\d+.of.\d+$");

            //chapter
            _chapterChecker = new Regex(@"^Chapter\s+\d+\.$");

            //lessons
            _lessonChecker = new Regex(@"^LESSON\s+\d+\.$");

            //start of paragraph
            _startParaChecker = new Regex(@"^[A-Z]-");










            //read in the file
            _file = System.IO.File.ReadAllLines(fullPath);

            object data = null;
            for(int index = 0; index < _file.Length; index++) {

               LineInfo lInfo = WhatLineIsThis(_file[index], index, out data);



            }




         }
         catch(Exception ex) {
            if(ex.InnerException != null) {
               StringBuilder sb = new StringBuilder();
               sb.Append(ex.Message);
               sb.Append("; ");
               sb.Append(ex.InnerException.Message);
               System.Diagnostics.Debug.WriteLine(sb.ToString());
            } else {
               System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
         }


      }
      private LineInfo WhatLineIsThis(string line, int index, out LineInfo next, out object data  ) {

         data = null;
         next = LineInfo.Not_Sure;

         if(_pageChecker.IsMatch(line)) {
            Regex pullNumber = new Regex(@"\d+");
            Match m = pullNumber.Match(line);
            data = Convert.ToInt32(m.Value);
            return LineInfo.Page_Number;
         }
         else if(_chapterChecker.IsMatch(line)) {
            Regex pullNumber = new Regex(@"\d+");
            Match m = pullNumber.Match(line);
            data = Convert.ToInt32(m.Value);
            next = LineInfo.Chater_Name;
            return LineInfo.Chapter_Number;
         }





         return LineInfo.Not_Sure;
      }
   }
}
