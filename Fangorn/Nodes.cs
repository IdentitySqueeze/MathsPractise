using System;
using System.Collections.Generic;
using System.Drawing;

namespace Fangorn {
    class WantMain { [STAThread] public static void Main(string[] args) { } }
    public interface IPossibleAnswer<TNode> {
        List<TNode> answer { get; set; }
        TNode answerNode { get; set; }
        bool IsSequence { get; set; }
        bool uniformSize { get; set; }
    }
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
        int innerRootCount { get; set; }
    }
    public interface IRenderNode<TNode>{
        TNode parent { get; set; }
        bool showDiv { get; set; }
        int index { get; set; }
        int rowLen { get; set; }
        int rowHeight { get; set; }
        int colsMaxRowlen { get; set; }
        int colsVerticalCentre { get; set; }
        bool colsUniformlySpaced { get; set; }
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
