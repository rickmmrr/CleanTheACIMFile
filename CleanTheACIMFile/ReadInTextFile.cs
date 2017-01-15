using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CleanTheACIMFile {
   public class ReadInTextFile {
      string[] _file;

      Regex _pageChecker;
      Regex _chapterChecker;
      Regex _lessonChecker;
      Regex _startParaChecker;
      Regex _paragraphSectionTitleChecker;

      // temparay
      Regex _stopAtLessonOne;


      enum LineInfo { Page_Number,Chapter_Number,Chapter_Title,Chapter_section_Name,
      First_Line_in_paragraph, Continues_Line, Major_Section_Name,Not_Sure,End_Of_File,Empty_Line}


      void ReadWorkbook( path, string file ) {

      }

      void ReadACIMText( string path, string file ) {
         try {




            Book book = new Book();






            //read in the file
            _file = System.IO.File.ReadAllLines(path + file);
            int index = 0;

            //first name in the file is the name of the book
            //and first major section name
            book.Title_Book = _file[index];
            book.AddMajorBookSection(_file[index++] + " Text");



            BKMajorBookSection currentMajorBookSection = book.ACIMMajorBookSection[book.ACIMMajorBookSection.Count - 1];
            object data = null;
            LineInfo nextFromRegex = LineInfo.Not_Sure;
            LineInfo nextFromText = LineInfo.Not_Sure;


            for(;;) {
               //Break at end of file
               if(index >= _file.Length)
                  break;

               //Temp
               if(_stopAtLessonOne.IsMatch(_file[index]))
                  break;


               LineInfo lineInfo = WhatLineIsThis(_file[index], out nextFromRegex, out data);

               if(lineInfo == LineInfo.Page_Number || lineInfo == LineInfo.Empty_Line) {
                  index++;
                  continue;
               }

               if(nextFromText == LineInfo.Chapter_Title) {
                  book.AddChapterTitle(_file[index++]);
                  nextFromText = LineInfo.Chapter_section_Name;

               }
               else if(nextFromText == LineInfo.Chapter_section_Name) {
                  book.AddNewChapterSection(_file[index++]);
                  nextFromText = LineInfo.First_Line_in_paragraph;

               }
               else if(nextFromText == LineInfo.First_Line_in_paragraph) {
                  book.AddParagraph(_file[index++]);
                  nextFromText = LineInfo.Not_Sure;

               }
               else if(lineInfo == LineInfo.Chapter_Number) {

                  book.AddChapter(_file[index++]);
                  nextFromText = LineInfo.Chapter_Title;

               }
               else if(lineInfo == LineInfo.Chapter_Title) {
                  book.AddChapterTitle(_file[index++]);
                  nextFromText = LineInfo.Chapter_section_Name;

               }
               else if(lineInfo == LineInfo.Chapter_section_Name) {
                  book.AddNewChapterSection(_file[index++]);
                  nextFromText = LineInfo.First_Line_in_paragraph;


               }
               else if(lineInfo == LineInfo.First_Line_in_paragraph) {

                  book.AddParagraph(_file[index++]);
               }
               else if(lineInfo == LineInfo.Continues_Line) {
                  book.AddToParagraph(_file[index++]);
               }

            }

            //Now serialize the book
            XmlSerializer xs = new XmlSerializer(typeof(Book));
            Stream s = File.OpenWrite(path + @"XmlCheck.xml");
            xs.Serialize(s, book);
            s.Close();


            //Deserialize

            XmlSerializer deSel = new XmlSerializer(typeof(Book));
            Stream ds = File.OpenRead(path + @"XmlCheck.xml");
            Book book2 = (Book)deSel.Deserialize(ds);
            ds.Close();

            List<string> chapters = new List<string>();
            foreach(BKChapter ch in book2.ACIMMajorBookSection[0].ACIMChapter) {
               chapters.Add(ch.ChapterTitle);
            }

            string chapter1 = chapters[0];



         }
         catch(Exception ex) {

            var theException = ex;
            do {
               System.Diagnostics.Debug.WriteLine(theException.Message);
               theException = theException.InnerException;
            } while(theException != null);


         }
      }

      public ReadInTextFile(string path, string file ) {

         //init regex expresions once
         //page
         _pageChecker = new Regex(@"^Page.\d+.of.\d+$");

         //chapter
         _chapterChecker = new Regex(@"^Chapter\s+\d+\.$");

         //lessons
         _lessonChecker = new Regex(@"^LESSON\s+\d+\.$");

         //start of paragraph
         _startParaChecker = new Regex(@"^[A-Z]-");

         //start of paragraph section title
         _paragraphSectionTitleChecker = new Regex(@"^[IVX]+\.\s");

         //temp stop at lesson 1
         _stopAtLessonOne = new Regex(@"^LESSON\s1\\.");




      }
      private LineInfo WhatLineIsThis(string line, out LineInfo next, out object data  ) {

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
            next = LineInfo.Chapter_section_Name;



            return LineInfo.Chapter_Number;


         } else if(_startParaChecker.IsMatch(line)) {

            //get the paragraph heading
            Regex pullHeading = new Regex(@"^.\s");
            Match m = pullHeading.Match(line);
            data = m.Value.ToString();
            next = LineInfo.Not_Sure;
            return LineInfo.First_Line_in_paragraph;
            

         } else if(_paragraphSectionTitleChecker.IsMatch(line)) {

            next = LineInfo.First_Line_in_paragraph;
            return LineInfo.Chapter_section_Name;



         } else {

            // it has to be an empty line or part of a paragraph
            // or new paragraph section name


            next = LineInfo.Not_Sure;
            if(string.IsNullOrWhiteSpace(line)) {
               return LineInfo.Empty_Line;
            }
            else {
               return LineInfo.Continues_Line;

            }

         }
      }
   }
}
