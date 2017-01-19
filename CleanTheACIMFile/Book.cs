using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTheACIMFile {
   /// <summary>
   /// Book Object
   /// </summary>
   public class Book {
      public List<BKMajorBookSection> ACIMMajorBookSection { get; set; }
      public string Title_Book { get; set; }
      public Book() { ACIMMajorBookSection = new List<BKMajorBookSection>(); Title_Book = string.Empty; }
      public BKMajorBookSection ReturnCurrentMajorBookSection() { return ACIMMajorBookSection[ACIMMajorBookSection.Count - 1]; }
      // ***********************************************************************************************
      //************* This section encapselates all functions from within the book class

      public void AddMajorBookSection( string title ) {

         var m = new BKMajorBookSection();
         m.Title_MajorBookSection = title;
         ACIMMajorBookSection.Add(m);
      }
      public void AddTextChapter(string chapterNumber ) {
         //Add it to the list
         ReturnCurrentMajorBookSection().AddChapter(chapterNumber);
      }
      public void AddTextChapterTitle(string title ) {
         ReturnCurrentMajorBookSection().ReturnCurrentChapter().ChapterTitle = title;
      }

      public void AddNewTextChapterSection( string section ) {
         ReturnCurrentMajorBookSection().ReturnCurrentChapter().AddSection(section);
      }
      public void AddParagraph(string line ) {
         //for the workbook we don't use the chapter section but we will create one
         if(ReturnCurrentMajorBookSection().ReturnCurrentChapter().ACIMChapterSection.Count == 0)
            AddNewTextChapterSection("Default");
         ReturnCurrentMajorBookSection().ReturnCurrentChapter().ReturnCurrentChapterSection().AddNewParagraph(line);
      }
      public void AddToParagraph(string line ) {
         ReturnCurrentMajorBookSection().ReturnCurrentChapter().ReturnCurrentChapterSection().ReturnCurrentParagraph().AddLineForNow(line);      }

      public void AddMajorSectionIntroductionTitle(string title) {
         ReturnCurrentMajorBookSection().IntroductionTitle = title;
      }
      public void AddMajorSectionIntroductionParagraph(string line ) {
         ReturnCurrentMajorBookSection().AddNewIntroductionParagraph(line);
      }
      public void AddToMajorSectionIntroductionParagraph(string line ) {
         ReturnCurrentMajorBookSection().ReturnCurrentIntroductionParagraph().AddLineForNow(line);
      }

      public void AddToChapterTitle(string line ) {
         ReturnCurrentMajorBookSection().ReturnCurrentChapter().ChapterTitle += line;
      }
      //***************************************************************************************************
   }

   /// <summary>
   /// Book Section Object
   /// </summary>
   public class BKMajorBookSection {
      public List<BKChapter> ACIMChapter { get; set; }
      public string Title_MajorBookSection { get; set; }
      public BKMajorBookSection( ) { ACIMChapter = new List<BKChapter>(); IntroductionParagraphs = new List<BKParagraph>(); }
      public void AddChapter(string chapterNumber ) {
         var b = new BKChapter();
         b.ChapterNumber = chapterNumber;
         ACIMChapter.Add(b);
      }
      public BKChapter ReturnCurrentChapter() { return ACIMChapter[ACIMChapter.Count - 1]; } 

      //Section for introduction
      //***********************************************************************************
      public string IntroductionTitle { get; set; }
      public List<BKParagraph> IntroductionParagraphs { get; set; }
      public void AddNewIntroductionParagraph( string line ) {
         var p = new BKParagraph();
         p.AddLineForNow(line);
         IntroductionParagraphs.Add(p);
      }
      public BKParagraph ReturnCurrentIntroductionParagraph() { return IntroductionParagraphs[IntroductionParagraphs.Count - 1]; }

      //*************************************************************************************

   }

   public class BKChapter {
      public List<BKChapterSection> ACIMChapterSection { get; set; }
      public string ChapterTitle { get; set; }
      public string ChapterNumber { get; set; }
      public BKChapter() { ACIMChapterSection = new List<BKChapterSection>();}
      public void AddSection( string sectionTitle ) {
         var s = new BKChapterSection();
         s.SectionTitle = sectionTitle;
         ACIMChapterSection.Add(s);
      }
      public BKChapterSection ReturnCurrentChapterSection() { return ACIMChapterSection[ACIMChapterSection.Count - 1]; }
   }
   public class BKChapterSection {
      public List<BKParagraph> ACIMParagraph { get; set; }
      public string SectionTitle { get; set; }
      public string ChapterNumber { get; set; }
      public BKChapterSection() { ACIMParagraph = new List<BKParagraph>();}
      public void AddNewParagraph( string line ) {
         var p = new BKParagraph();
         p.AddLineForNow(line);
         ACIMParagraph.Add(p);
      }
      public BKParagraph ReturnCurrentParagraph() { return ACIMParagraph[ACIMParagraph.Count - 1]; }
   }

   public class BKParagraph {
      public List<string> ACIMParagraph { get; set; }
      public BKParagraph() {
         ACIMParagraph = new List<string>();
      }
      public void AddLineForNow( string line) {
         ACIMParagraph.Add(line);
      }
      
      
   }
    public class ACIMLessons {
        public ACIMLessons() {
            Parts = new List<Part>();
        }
        public List<Part> Parts { get; set; }

        #region Access Methods
        public void AddPart(string partNumber ) {
            Part p = new Part();
            p.PartNumber = partNumber;
            Parts.Add(p);
        }
        public void AddPartIntroductionTitle(string introTitle ) {
            if(Parts == null)
                throw new Exception("Inside AddPartIntroductionTitle Error = The Parts list has not been initialized.");
            Parts[Parts.Count - 1].IntroductionTitle = introTitle;
        }
        public void StartNewIntroParagraph(string line ) {
            if(Parts == null)
                throw new Exception("Inside StartNewIntroParagraph Error = The Parts list has not been initialized.");
            BKParagraph p = new BKParagraph();
            p.AddLineForNow(line);
            Parts[Parts.Count - 1].IntroParagraphList.Add(p);
        }
        public void AddToIntroParagraph(string line ) {
            if(Parts == null)
                throw new Exception("Inside AddToIntroParagraph Error = The Parts list has not been initialized.");
            Parts[Parts.Count - 1].IntroParagraphList[Parts[Parts.Count - 1].IntroParagraphList.Count - 1].AddLineForNow(line);
        }
        public void AddLesson(string lessonNumber ) {
            Part.Lesson l = new Part.Lesson();
            l.LessonNumber = lessonNumber;
            Parts[Parts.Count - 1].Lessons.Add(l);
        }
        public void AddLessonTitle( string line ) {
            Parts[Parts.Count - 1].Lessons[Parts[Parts.Count - 1].Lessons.Count - 1].LessonTitleList.Add(line);
        }
        public void AddToLessonTitle( string line ) {
            Parts[Parts.Count - 1].Lessons[Parts[Parts.Count - 1].Lessons.Count - 1].LessonTitleList.Add(line);
        }
        public void AddLessonContentBeginningPara(string line ) {
            BKParagraph p = new CleanTheACIMFile.BKParagraph();
            p.AddLineForNow(line);
            Parts[Parts.Count - 1].Lessons[Parts[Parts.Count - 1].Lessons.Count - 1].LessonContentList.Add(p);
        }
        public void AddToLessonContentPara(string line ) {
            Parts[Parts.Count - 1].Lessons[Parts[Parts.Count - 1].Lessons.Count - 1].LessonContentList
                [Parts[Parts.Count - 1].Lessons[Parts[Parts.Count - 1].Lessons.Count - 1].LessonContentList.Count - 1].AddLineForNow(line);
        }
        #endregion


        public class Part {
            public Part() {
                IntroParagraphList = new List<BKParagraph>();
                Lessons = new List<Lesson>();
            }
            public string PartNumber { get; set; }
            public string IntroductionTitle { get; set; }
            public List<BKParagraph> IntroParagraphList { get; set; }

            public List<Lesson> Lessons { get; set; }


            public class Lesson {
                public string LessonNumber { get; set; }
                public List<string> LessonTitleList { get; set; }
                public List<BKParagraph> LessonContentList { get; set; }

            }
        }
       

         
    }


}
