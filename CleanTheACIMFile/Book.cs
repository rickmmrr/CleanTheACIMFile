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
      List<MajorBookSection> _bkMajorBookSection;
      public List<MajorBookSection> bkMajorBookSection { get { return _bkMajorBookSection; } }
      public string Title_Book { get; set; }
      public Book() { _bkMajorBookSection = new List<MajorBookSection>(); Title_Book = string.Empty; }
      // ***********************************************************************************************
      //************* This section encapsolates all functions from within the book class

      public void AddMajorBookSection( string title ) {

         MajorBookSection m = new MajorBookSection(title);
         _bkMajorBookSection.Add(m);
      }
      public void AddChapter(string chapterName ) {
         //Add it to the list
         _bkMajorBookSection[_bkMajorBookSection.Count - 1].AddChapter(chapterName);
      }

      public void AddNewParagraph(string para ) {
         BkChapter c = _bkMajorBookSection[_bkMajorBookSection.Count - 1].ReturnCurrentChapter;
         c.AddNewParagraph(para);
      }
      public void AddToParagraph(string line ) {
         BkChapter c = _bkMajorBookSection[_bkMajorBookSection.Count - 1].ReturnCurrentChapter;
         c.AddToParagraph(line);

      }

      //***************************************************************************************************
   }

   /// <summary>
   /// Book Section Object
   /// </summary>
   public class MajorBookSection {
      List<BkChapter> _bkChapter;
      public List<BkChapter> bkChapter { get { return _bkChapter; } }
      public string Title_MajorBookSection { get; set; }
      public MajorBookSection(string SectionName) { _bkChapter = new List<BkChapter>(); Title_MajorBookSection = SectionName; }
      public void AddChapter(string chapterName ) {
         BkChapter b = new BkChapter(chapterName);
         _bkChapter.Add(b);
      }
      public BkChapter ReturnCurrentChapter { get { return _bkChapter[_bkChapter.Count - 1]; } }
   }

   public class BkChapter {
      List<BkParagraph> _bkParagraph;
      public List<BkParagraph> bkParagraph { get { return _bkParagraph; } }
      public string ChapterTitle { get; set; }
      public BkChapter(string chapterTitle) { _bkParagraph = new List<BkParagraph>(); ChapterTitle = chapterTitle; }
      public void AddNewParagraph( string line ) {
         BkParagraph p = new BkParagraph(line);
         _bkParagraph.Add(p);
      }
      public void AddToParagraph(string line ) {
         _bkParagraph[_bkParagraph.Count - 1].AddLineForNow(line);
      }
      public BkParagraph ReturnCurrentChapter { get { return _bkParagraph[_bkParagraph.Count - 1]; }      }
   }

   public class BkParagraph {
      List<BkLine> _bkSentence;
      public List<BkLine> bkSentence { get { return _bkSentence; }}
      public BkParagraph(string line) {
         _bkSentence = new List<BkLine>();
         BkLine b = new BkLine(line);
         _bkSentence.Add(b);
      }
      public void AddLineForNow( string line) {
         BkLine b = new BkLine(line);
         _bkSentence.Add(b);
      }
      
      
   }
   public class BkLine {
      public string bkSentence { get; set; }
      public BkLine(string line ) {
         bkSentence = line;
      }
   }


}
