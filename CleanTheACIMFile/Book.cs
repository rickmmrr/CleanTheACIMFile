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
      public void AddMajorBookSection( MajorBookSection section ) { _bkMajorBookSection.Add(section); }
   }

   /// <summary>
   /// Book Section Object
   /// </summary>
   public class MajorBookSection {
      List<BkChapter> _bkChapter;
      public List<BkChapter> bkChapter { get { return _bkChapter; } }
      public string Title_MajorBookSection { get; set; }
      public MajorBookSection() { _bkChapter = new List<BkChapter>(); Title_MajorBookSection = string.Empty; }
      public void AddChapter(BkChapter chapter ) { _bkChapter.Add(chapter); }
   }

   public class BkChapter {
      List<BkParagraph> _bkParagraph;
      public List<BkParagraph> bkParagraph { get { return _bkParagraph; } }
      public string ChapterTitle { get; set; }
      public BkChapter() { _bkParagraph = new List<BkParagraph>(); ChapterTitle = string.Empty; }
      public void AddParagraph( BkParagraph paragraph ) { _bkParagraph.Add(paragraph); }
   }

   public class BkParagraph {
      List<BkSentence> _bkSentence;
      public List<BkSentence> bkSentence { get { return _bkSentence; }}
      public BkParagraph() { _bkSentence = new List<BkSentence>(); }
      public void AddSentence( BkSentence sentence) { _bkSentence.Add(sentence); }
      
   }
   public class BkSentence {
      public string bkSentence { get; set; }
   }


}
