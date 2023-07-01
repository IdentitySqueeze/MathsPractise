using System.Collections.Generic;
using System.Drawing;

namespace Fangorn {
    public interface IWalkerArgsRtns{
        // -- Nav control --
        bool Exit{ get; } // Early exit
        int depth { get; set; }
    }

    public class KeysArgsRtns : IWalkerArgsRtns {
        public bool Exit { get; set;}
        public bool hit { get; set; }
        public bool keepIt { get; set; }
        public string InputSoFar{ get; set; }
        public bool IsSequence { get; set; }
        public int depth { get; set; }
        public bool exponent { get; set; }
        public Color inKeysColour { get; set; }
    }
    public class ConvertArgsRtns<TNode> : IWalkerArgsRtns{ // one per answer
        public bool Exit { get => false; }
        //public List<wfNode> ToTree { get; set; } = new List<wfNode>( ); // Top-level columns
        public TNode currentNode { get; set; }
        public int width { get; set; }
        public int depth { get; set; }
        public bool exponent { get; set; }
    }
    public class MeasureArgsRtns : IWalkerArgsRtns {
        public bool Exit { get => false; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int depth { get; set; }
        public int currentMax { get; set; }
        public int maxRootDepth { get; set; }
        public Stack<rootStacker> rootStack { get; set; } = new Stack<rootStacker> { };
        public bool exponent { get; set; }
        public Rectangle exponentPoint { get; set; }
    }
    public class SizeArgsRtns: IWalkerArgsRtns
    {
        public bool Exit { get => false; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool uniformSize { get; set; }
        public int uniformedWidth { get; set; }
        public int depth { get; set; }
        public int maxRootDepth { get; set; }
        public bool exponent { get; set; }
        public Rectangle exponentPoint { get; set; }
        public Point TopLeft { get; set; }

    }
    public class DrawArgsRtns : IWalkerArgsRtns{
        public bool Exit { get => hit; }
        public bool hit { get; set; }
        public bool Selected { get; set; }
        public bool ShowAnswers { get; set; }
        public int Width { get; set; }
        public int Height{ get; set; }
        public Graphics g{ get; set; }
        public Point TopLeft{ get; set; }
        public Point tmpTopLeft{ get; set; }
        public Bitmap Bitmap{ get; set; }

        //public List<Bitmap> ansBitmaps { get; set; }
        public Graphics ansBackBuffer { get; set; }
        public int depth { get; set; }
        public int maxRootDepth { get; set; }
        public bool exponent { get; set; }
        public Rectangle exponentPoint { get; set; }
    }


    public struct rootStacker {
        public int innersCount { get; set; }
        public int consecs { get; set; }
    }

}
