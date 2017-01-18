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

      Book _book;
      string[] _textFile;
      string[] _lessonFile;

      Regex _pageChecker;
      Regex _chapterChecker;
      Regex _lessonChecker;
      Regex _startParaChecker;
      Regex _paragraphSectionTitleChecker;
      Regex _skipPartIandPartII;

      // temparay
      Regex _stopAtLessonOne;


      enum LineInfo { Page_Number,Chapter_Number,Chapter_Title,Chapter_section_Name,
      First_Line_in_paragraph, Continues_Line, Major_Section_Name,Not_Sure,
         End_Of_File,Empty_Line,Lesson,part_One_or_Part_Two}



      public ReadInTextFile( string path) {

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
         _stopAtLessonOne = new Regex(@"^LESSON\s\d+\.");

         _skipPartIandPartII = new Regex(@"^PART\s[I]+$");


         _book = new Book();

         ReadACIMText(path, "rawDataACIMText.txt");
         ReadWorkbook(path, "rawDataWorkBook.txt");

         //Now serialize the book
         XmlSerializer xs = new XmlSerializer(typeof(Book));
         Stream s = File.OpenWrite(path + @"XmlCheck.xml");
         xs.Serialize(s, _book);
         s.Close();



      }
      void ReadWorkbook( string path, string file ) {

         try {

            _lessonFile = File.ReadAllLines(path + file);

            int index = 0;
            _book.AddMajorBookSection(_lessonFile[index++]);
            _book.AddMajorSectionIntroductionTitle(_lessonFile[index++]);

            object data = null;
            LineInfo nextFromRegex = LineInfo.Not_Sure;
            LineInfo nextFromText = LineInfo.Not_Sure;

            for(;;) {

               if(index >= _lessonFile.Length)
                  break;

               LineInfo lineInfo = WhatLineIsThis(_lessonFile[index], out nextFromRegex, out data);

               if(lineInfo == LineInfo.Lesson)
                  break;

               if(lineInfo == LineInfo.First_Line_in_paragraph) {
                  _book.AddMajorSectionIntroductionParagraph(_lessonFile[index++]);
               }
               else if(lineInfo == LineInfo.Continues_Line) {
                  _book.AddToMajorSectionIntroductionParagraph(_lessonFile[index++]);
               }
               else {
                  index++;
               }


            }

            //we should now be at lesson 1
            //read in all the lessons
            for(;;) {

               if(index >= _lessonFile.Length)
                  break;

               LineInfo lineInfo = WhatLineIsThis(_lessonFile[index], out nextFromRegex, out data);

               if(lineInfo == LineInfo.Lesson) {
                  _book.AddTextChapter(_lessonFile[index++]);
               }
               else if(lineInfo == LineInfo.Chapter_Title || nextFromRegex == LineInfo.Chapter_Title) {
                  _book.AddTextChapterTitle(_lessonFile[index++]);
               }
               else if(lineInfo == LineInfo.First_Line_in_paragraph) {
                  _book.AddParagraph(_lessonFile[index]);
               }
               else if(lineInfo == LineInfo.Continues_Line) {
                  _book.AddToParagraph(_lessonFile[index++]);
               }
               else {
                  index++;
               }


            }





         }
         catch(Exception ex) {
            var theException = ex;
            do {
               System.Diagnostics.Debug.WriteLine(theException.Message);
               theException = theException.InnerException;
            } while(theException != null);

         }
      }
      void ReadACIMText( string path, string file ) {

         try {

            //read in the file
            _textFile = System.IO.File.ReadAllLines(path + file);
            int index = 0;

            //first name in the file is the name of the book
            //and first major section name
            _book.Title_Book = _textFile[index];
            _book.AddMajorBookSection(_textFile[index++] + " Text");

            //Next is the major section introduction title
            _book.AddMajorSectionIntroductionTitle(_textFile[index++]);

            //read in the introduction paragraphs
            // break at chapter 1
            object data = null;
            LineInfo nextFromRegex = LineInfo.Not_Sure;
            LineInfo nextFromText = LineInfo.Not_Sure;

            for(;;) {
               LineInfo lineInfo = WhatLineIsThis(_textFile[index], out nextFromRegex, out data);

               if(lineInfo == LineInfo.Chapter_Number)
                  break;

               if(lineInfo == LineInfo.Page_Number || lineInfo == LineInfo.Empty_Line) {
                  index++;
                  continue;
               }


               if(lineInfo == LineInfo.First_Line_in_paragraph) {
                  _book.AddMajorSectionIntroductionParagraph(_textFile[index++]);
                  nextFromText = LineInfo.Not_Sure;

               } else if(lineInfo == LineInfo.Continues_Line) {
                  _book.AddToMajorSectionIntroductionParagraph(_textFile[index++]);
               }

            }



            BKMajorBookSection currentMajorBookSection = _book.ACIMMajorBookSection[_book.ACIMMajorBookSection.Count - 1];


            for(;;) {
               //Break at end of file
               if(index >= _textFile.Length)
                  break;

               //Temp
               if(_stopAtLessonOne.IsMatch(_textFile[index]))
                  break;


               LineInfo lineInfo = WhatLineIsThis(_textFile[index], out nextFromRegex, out data);

               if(lineInfo == LineInfo.Page_Number || lineInfo == LineInfo.Empty_Line) {
                  index++;
                  continue;
               }

               if(nextFromText == LineInfo.Chapter_Title) {
                  _book.AddTextChapterTitle(_textFile[index++]);
                  nextFromText = LineInfo.Chapter_section_Name;

               }
               else if(nextFromText == LineInfo.Chapter_section_Name) {
                  _book.AddNewTextChapterSection(_textFile[index++]);
                  nextFromText = LineInfo.First_Line_in_paragraph;

               }
               else if(nextFromText == LineInfo.First_Line_in_paragraph) {
                  _book.AddParagraph(_textFile[index++]);
                  nextFromText = LineInfo.Not_Sure;

               }
               else if(lineInfo == LineInfo.Chapter_Number) {

                  _book.AddTextChapter(_textFile[index++]);
                  nextFromText = LineInfo.Chapter_Title;

               }
               else if(lineInfo == LineInfo.Chapter_Title) {
                  _book.AddTextChapterTitle(_textFile[index++]);
                  nextFromText = LineInfo.Chapter_section_Name;

               }
               else if(lineInfo == LineInfo.Chapter_section_Name) {
                  _book.AddNewTextChapterSection(_textFile[index++]);
                  nextFromText = LineInfo.First_Line_in_paragraph;


               }
               else if(lineInfo == LineInfo.First_Line_in_paragraph) {

                  _book.AddParagraph(_textFile[index++]);
               }
               else if(lineInfo == LineInfo.Continues_Line) {
                  _book.AddToParagraph(_textFile[index++]);
               }

            }






         }
         catch(Exception ex) {

            var theException = ex;
            do {
               System.Diagnostics.Debug.WriteLine(theException.Message);
               theException = theException.InnerException;
            } while(theException != null);


         }
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



         } else if(_lessonChecker.IsMatch(line)) {
            next = LineInfo.Chapter_Title;
            return LineInfo.Lesson;

         }
         else {

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
