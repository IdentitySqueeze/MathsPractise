using System;
using System.Collections.Generic;
using System.Drawing;

namespace Fangorn {
    class WantMain { [STAThread] public static void Main(string[] args) { } }
    public interface INode<TNode>{
        List<TNode> columns{ get; set; } // LC
        List<TNode> rows{ get; set; } // RS (1 x num, 1 x den )
        //List<TElement> rowChunks{ get; set; } // --> columns
        string nodeValue { get; set; }
        //string Leaf { get; set; }
        bool IsColumn { get;  }//=> answerRows.Count==0;
        bool IsLeaf { get; }
        bool decorated { get; }
        char? op { get; set; }
        bool answered { get; set; }
        ansType ansType { get; set; }
        ColTyp colType { get; set; }
        int from { get; set; }
        int to { get; set; }
        bool Infinity { get; set; } 
    }
    public interface IRenderNode<TNode>{
        TNode parent { get; set; }
        bool showDiv { get; set; }
        // -- row enrichments ---
        //string wholeRow { get; set; }
        int rowLen { get; set; }
        int rowExpLen { get; set; }
        Rectangle rowRect { get; set; } //= Rectangle.Empty; //new Rectangle( 0,0,0,0 );
        int charCount { get; set; } //= 0;
        List<CharacterRange> toBlock { get; set; }// = new List<CharacterRange> { };
        Rectangle blockRect { get; set; } //= Rectangle.Empty;
        Rectangle boundsRect { get; set; } //= Rectangle.Empty;
        Region[] blockRegions { get; set; }
    }
    public interface IAnswerNode<TNode>{
        //bool answered{ get; }//{return answerRowChunks.All(a=>a.answered); } }
        List<CharacterRange> toBlock { get; set; }// = new List<CharacterRange> { };
        Rectangle blockRect { get; set; } //= Rectangle.Empty;
        Rectangle boundsRect { get; set; } //= Rectangle.Empty;
        Region[] blockRegions { get; set; }

    }
    public enum ansType : int {
        Col = 1,
        Num = 2,
        Den = 3,
    }
    public enum ColTyp : int {
        integer = 1,
        real,
        complex,
        fraction,
        product,
        sigma,
        integral,
        factorial,
        choose,
        brace,
        bracket,
        squareBracket,
        rooted,
        matrix,
        grouping,
        exponent   //Currently for rows. Put on columns too..? Exponents are added to both.
    }

}