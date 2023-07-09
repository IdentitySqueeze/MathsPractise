using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Fangorn;

namespace DustBlowerClient {
    public class wfPossibleAnswer : IPossibleAnswer<wfNode> {
        public List<wfNode> answer { get; set; } = new List<wfNode> { };
        public wfNode answerNode { get; set; }
        public bool IsSequence { get; set; } = false;
        public bool uniformSize { get; set; } = false;
    }
    public class wfNode : INode<wfNode>,
                          IRenderNode<wfNode>{
        // -- INode --
        public List<wfNode> columns { get; set; } = new List<wfNode>();
        public List<wfNode> rows { get; set; } = new List<wfNode>();      // Leaves.
        //public List<wfNodeElement> rowChunks { get; set; } = new List<wfNodeElement>();
        public string nodeValue { get; set; }
        //public string Leaf { get; set; }
        //public bool answered { get; set; }
        public ColTyp colType { get; set; } = ColTyp.real;

        #region -- sigma, product, integrals, choose ∞
        public int from { get; set; }
        public int to { get; set; }
        public bool Infinity { get; set; } = false;
        #endregion

        public bool IsColumn { get { return rows.Count==0; } }
        //public bool IsLeaf => (columns.Count==0);
        public bool IsLeaf => nodeValue != null;
        public bool decorated => (colType & (ColTyp.bracket | ColTyp.rooted))>0;
        public char? op { get; set; }
        // -- IRenderNode --
        public wfNode parent { get; set; }
        public bool showDiv { get; set; }
        //public string wholeRow { get; set; }
        public int rowLen { get; set; }
        public int rowHeight { get; set; }
        public int rowExpLen { get; set; }
        public Rectangle rowRect { get; set; } = Rectangle.Empty; //new Rectangle( 0,0,0,0 );
        public List<CharacterRange> toBlock { get; set; } = new List<CharacterRange> { };
        public int charCount { get; set; } = 0;
        public Rectangle blockRect { get; set; } = Rectangle.Empty;
        public Rectangle boundsRect { get; set; } = Rectangle.Empty;
        public Region[] blockRegions { get; set; }
        public int innerRootCount { get; set; }
        public int index { get; set; } = 1;
        public int colsMaxRowlen { get; set; }
        public int colsVerticalCentre { get; set; }
        public bool colsUniformlySpaced { get; set; }
        // -- IAnswerNode --
        public bool answered { get; set; } //sftt
        public ansType ansType { get; set; } = ansType.Num;
        //public bool InSequence { get; set; } = false;

    }
    public class wfNodeElement {
        public int qid;
        public int elid;
        public string elementChunk;
        public bool answered;
        public char? op { get; set; }
        public wfNodeElement() { }
        public wfNodeElement(int id, int ix, string chars, bool answer) {
            qid = id;
            elid = ix;
            elementChunk = chars;
            answered = answer;
        }
    }
}
