using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathUtils;
//using static Fangorn.Original<TNode, TNodeTo>;

namespace Fangorn {

    /* Notes
     * The full set:
     *      So must include types FROM and TO both for converts
     */
    public class Treebeard<TNode, TNodeTo> where TNode   : INode<TNode>,   IRenderNode<TNode>,   new()
                                          where TNodeTo : INode<TNodeTo>, IRenderNode<TNodeTo>, new() {
        public class OriginalTreeWalker<TNode, TRtn> : ITreeWalker<TNode, TRtn>
                     where TRtn : IWalkerArgsRtns
                     where TNode : INode<TNode> {
            public TRtn Traverse(TNode nodeIn, TRtn rtn, int depth = 0) => rtn;
            public TRtn Traverse(List<TNode> arcs, TRtn rtn, int depth = 0) {
                //----------------------------------
                //Convert, measure, size, draw, keys
                //----------------------------------
                rtn.depth = depth;
                if (depth==0) {
                    rtn= PreTraversalOp(arcs, rtn);
                }
                for (int i = 0; i<arcs.Count; i++) { // This is across...
                    TNode arc = arcs[i];
                    //if ( !arc.HasRows) { 
                    if (arc.IsColumn) { // There's no Rows/leaves  
                        rtn = PreColumnOp(arc, rtn);                                                   //Convert: bracket row handled
                        depth++;
                        rtn = Traverse(arc.columns, rtn, depth);   // This is Down...
                        depth--;
                        rtn = PostColumnOp(arc, rtn);                                                  //Measure: bracket wholerow handled.  
                    } else {
                        rtn = PreRowOp(arc, rtn);
                        rtn=RowOp(arc, rtn); // This can have 'downs' in it too.
                    }
                    if (i+1==arcs.Count && depth==0) { // Final top-level column...?
                        rtn = PostTraversalOp(arcs, rtn);
                    }
                    if (rtn.Exit) return rtn;
                }
                return rtn;
            }
            #region decs
            public Func<List<TNode>, TRtn, TRtn> PreTraversalOp { get; set; } = (a, b) => b;
            public Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> RowOp { get; set; } = (a, b) => b;
            #endregion
        }
        public class WalkNodeInEverythingRow<TNode, TRtn> : ITreeWalker<TNode, TRtn>
             where TRtn : IWalkerArgsRtns
             where TNode : INode<TNode> {
            public TRtn Traverse(TNode nodeIn, TRtn rtn, int depth = 0) {
                //----------------------------------------------
                //Convert, measure, layout, draw, keys, evaluate
                //----------------------------------------------
                rtn.depth = depth;
                //if (depth==0) {
                //rtn= PreTraversalOp(nodeIn, rtn);
                //}

                for (int i = 0; i<nodeIn.columns.Count; i++) { // This is across...
                    TNode node = nodeIn.columns[i];
                    // Every column has minimum one row
                    for (int j = 0; j<node.rows.Count; j++) {
                        //if leaf
                        //    // Down to the nodeValue
                        //    // Leaf means no columns (leaves/nodeValues can still be bracketted, rooted etc)
                        //    rtn = RowOp(node, rtn);
                        //else
                        //   // This row has columns (an expression, not a nodeValue)
                        //   // Columns (individuals, groups) can be bracketted, rooted etc.
                        //
                        //         // Is the column bracketted/rooted etc?   // <- put this in an op
                        //         if( bracketted / rooted )                 // <- put this in an op
                        //         Process decoration( )                     // <- put this in an op
                        //
                        //   rtn = columnOp(node, rtn);
                        //
                        //   // Recurse
                        //   rtn = Traverse(node, rtn, depth);   
                        //
                        //   // maybe post ColumnOp for bracket ends
                        //
                    }
                    if (rtn.Exit) return rtn;
                }
                return rtn;
            }
            public TRtn Traverse(List<TNode> arcs, TRtn rtn, int depth = 0) => rtn;
            #region decs
            public Func<List<TNode>, TRtn, TRtn> PreTraversalOp { get; set; } = (a, b) => b;
            public Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> RowOp { get; set; } = (a, b) => b;
            #endregion
        }

        public class NewTreeWalkerNotHasRows<TNode, TRtn> : ITreeWalker<TNode, TRtn>
             where TRtn : IWalkerArgsRtns
             where TNode : INode<TNode> {
            public TRtn Traverse(List<TNode> nodes, TRtn rtn, int depth = 0) {
                //----------------------------------------------
                //Convert, measure, layout, draw, keys, evaluate
                //----------------------------------------------
                rtn.depth = depth;
                if (depth==0) {
                    rtn= PreTraversalOp(nodes, rtn);
                }
                for (int i = 0; i<nodes.Count; i++) { // This is across...
                    TNode node = nodes[i];
                    //if (node.decorated ) {
                    //    rtn = PreColumnOp(node, rtn);
                    //}
                    if (node.IsColumn) {
                        //rtn = PreColumnOp(node, rtn)              // Convert: bracket row handled
                        depth++;
                        rtn = Traverse(node.columns, rtn, depth);   // This is Down...
                        depth--;
                        rtn=PostColumnOp(node, rtn);                // Measure: bracket wholerow handled
                    } else {
                        //if (node.decorated) {
                        //    rtn = PreRowOp(node, rtn);
                        //}
                        rtn=RowOp(node, rtn); // This can have 'downs' in it too by starting a new traverse
                    }
                    if (rtn.Exit) return rtn;
                }
                return rtn;
            }
            public TRtn Traverse(TNode arc, TRtn rtn, int depth = 0) => rtn;
            #region decs
            public Func<List<TNode>, TRtn, TRtn> PreTraversalOp { get; set; } = (a, b) => b;
            public Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> RowOp { get; set; } = (a, b) => b;
            #endregion
        }

        public class ConvertFactory<TNode, TRtns> where TNode : INode<TNode>, IRenderNode<TNode>, new()
            where TRtns : IWalkerArgsRtns, new() {
            public ITreeWalker<TNode, TRtns> GetWalker(Genus version) {
                ITreeWalker<TNode, TRtns> convertWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, ConvertArgsRtns<TNodeTo>>();
                switch (version) {
                    case Genus.OriginalClient:
                        convertWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, ConvertArgsRtns<TNodeTo>>();
                        break;
                    case Genus.OriginalService:
                        throw new NotImplementedException();
                    case Genus.NewStructure:
                        convertWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, ConvertArgsRtns<TNodeTo>>();
                        break;
                    default:
                        break;
                }
                return convertWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, ConvertArgsRtns<TNodeTo>> convertWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        convertWalker.RowOp = ClientConvertRowOp;
                        convertWalker.PreColumnOp = ConvertPreColumnOp;
                        convertWalker.PostColumnOp = ConvertPostColumnOp;
                        break;
                    case Genus.OriginalService:
                        throw new NotImplementedException();
                    case Genus.NewStructure:
                        convertWalker.RowOp = ClientConvertRowOp;
                        convertWalker.PreColumnOp = ConvertPreColumnOp;
                        convertWalker.PostColumnOp = ConvertPostColumnOp;
                        break;
                    default:
                        break;
                }
            }
            public ConvertArgsRtns<TNodeTo> ConvertPreColumnOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                TNodeTo node = new TNodeTo();
                node.colType = answerCol.colType;
                node.from= answerCol.from;
                node.to= answerCol.to;
                node.Infinity= answerCol.Infinity;
                rtn.currentNode.columns.Add(node);
                node.parent = rtn.currentNode;
                rtn.currentNode = node;

                //if (answerCol.colType==ColTyp.bracket && answerCol.rows.Count>0) { // old powered bracket fix
                //    TNodeTo leaf = new TNodeTo();
                //    foreach (var Row in answerCol.rows) {
                //        leaf = new TNodeTo();
                //        leaf.nodeValue = Row.nodeValue;
                //        leaf.answered = Row.answered;
                //        node.rows.Add(leaf);
                //    }
                //}

                return rtn;
            }
            public  ConvertArgsRtns<TNodeTo> ConvertPostColumnOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                rtn.currentNode = rtn.currentNode.parent;
                return rtn;
            }
            public  ConvertArgsRtns<TNodeTo> ClientConvertRowOp(TNode answerColumn, ConvertArgsRtns<TNodeTo> rtn) {

                TNodeTo node = new TNodeTo();
                node.showDiv = answerColumn.showDiv;
                node.colType = answerColumn.colType;
                TNodeTo leaf = new TNodeTo();

                foreach (var Row in answerColumn.rows) {
                    leaf = new TNodeTo();
                    leaf.nodeValue= Row.nodeValue;
                    leaf.answered = Row.answered;
                    node.rows.Add(leaf);
                }
                rtn.currentNode.columns.Add(node);
                return rtn;
            }
        }
        public class MeasureFactory<TNode, TRtns> where TNode: INode<TNode>, IRenderNode<TNode>, new()
            where TRtns:IWalkerArgsRtns, new(){

            //ON THE BENCH
            public ITreeWalker<TNode, MeasureArgsRtns> measureWalker = new OriginalTreeWalker<TNode, MeasureArgsRtns>();
            //public ITreeWalker<TNode, TRtns> GetWalker() => (ITreeWalker<TNode, TRtns>)measureWalker;
            public ITreeWalker<TNode, TRtns> GetWalker(Genus version) {
                ITreeWalker<TNode, TRtns> measureWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, MeasureArgsRtns>();
                switch (version) {
                    case Genus.OriginalClient:
                        measureWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, MeasureArgsRtns>();
                        break;
                    case Genus.OriginalService:
                        measureWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, MeasureArgsRtns>();
                        break;
                    case Genus.NewStructure:
                        measureWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, MeasureArgsRtns>();
                        break;
                    default:
                        break;
                }
                return measureWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, MeasureArgsRtns> measureWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        measureWalker.RowOp = MeasureProcessRowsService;
                        break;
                    case Genus.OriginalService:
                        measureWalker.RowOp = MeasureProcessRowsService;
                        break;
                    case Genus.NewStructure:
                        measureWalker.RowOp = MeasureNewRowOp;
                        //measureWalker.PreColumnOp= MeasureNewPreColumn;
                        //measureWalker.PostColumnOp= MeasureNewPostColumn;
                        break;
                    default:
                        break;
                }
            }
            public MeasureArgsRtns MeasureProcessRows(TNode answerCol, MeasureArgsRtns rtn) {
                var letterHeight = 19;
                foreach (var row in answerCol.rows) {
                    using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                        row.rowLen = drawstuff.GetStringLength(row.nodeValue, g, new Font("Arial", 15), new Rectangle(0,0,500,500), null, null);
                    }
                }
                if (answerCol.rows.Count==1) { //Single row
                } else if (answerCol.rows.Count==2) { //Double row
                    rtn.Height = letterHeight*2+10;
                }
                //if integer column
                //  there'll be a denominator somewhere...
                return rtn;
            }
            public MeasureArgsRtns MeasureProcessRowsService(TNode qc, MeasureArgsRtns rtn) {
                var letterHeight = 15;
                var expLetterHeight = 8;
                var ourHeight = 0;
                Font font = new Font("Consolas", 15);
                using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {

                    foreach (var row in qc.rows) { // nums, denoms
                        if (rtn.exponent) {
                            //---------------------------------
                            // Am I recursing into an exponent now?
                            //---------------------------------
                            row.rowLen = drawstuff.GetStringLength(row.nodeValue, g, font, new Rectangle(0, 0, 200, 500), null, null);
                            row.rowRect = new Rectangle(0, 0, row.rowLen, expLetterHeight);
                            ourHeight += expLetterHeight * qc.rows.Count;
                        //} else if (row.colType== ColTyp.bracket) {
                        //    rtn = measureWalker.Traverse(row.columns, rtn, rtn.depth);

                        } else {
                            rtn.currentMax=Math.Max(rtn.currentMax, qc.rows.Count); // bracket verticals
                            row.rowLen = drawstuff.GetStringLength(row.nodeValue, g, font, new Rectangle(0, 0, 200, 500), null, null);
                            row.rowRect = new Rectangle(0, 0, row.rowLen, letterHeight);
                            //-------------------------------
                            //Does this row have an exponent?
                            //-------------------------------
                            if (row.columns.Count == 1 && row.columns[0].colType == ColTyp.exponent && row.columns[0].columns.Count > 0) {
                                var exponentGroup = row.columns[0]; //Grouping column, no drawn parts. It's never processed.
                                rtn.exponent = true;
                                //ON THE BENCH
                                rtn = measureWalker.Traverse(exponentGroup.columns, rtn, rtn.depth);
                                rtn.exponent = false;
                                ourHeight += expLetterHeight+2;
                            }
                            //}
                        }
                    }
                }
                if (!rtn.exponent) {
                    switch (qc.colType) {

                        case ColTyp.brace:
                            break;
                        case ColTyp.bracket:
                            break;
                        case ColTyp.squareBracket:
                            break;

                        case ColTyp.fraction: //ASSUMPTION ABOUT ROW HEIGHTS
                            ourHeight += letterHeight* qc.rows.Count;
                            break;
                        case ColTyp.integer:
                            ourHeight += letterHeight;
                            break;
                        case ColTyp.exponent:
                            break;

                        case ColTyp.rooted:
                            ourHeight += 8;
                            break;

                        case ColTyp.sigma:
                            break;
                        case ColTyp.product:
                            break;
                        case ColTyp.integral:
                            break;

                        case ColTyp.matrix:
                            break;

                        case ColTyp.factorial:
                            break;
                        case ColTyp.choose:
                            break;
                        case ColTyp.complex:
                            break;
                        case ColTyp.real:
                            ourHeight += letterHeight;
                            break;
                        default:
                            break;
                    }
                }
                rtn.Height = Math.Max(rtn.Height, ourHeight);
                return rtn;
            }
            public MeasureArgsRtns MeasureNewRowOp(TNode node, MeasureArgsRtns rtn) {
                var letterHeight = 15; // Integers
                foreach (var row in node.rows) {
                    using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                        row.rowLen = drawstuff.GetStringLength(row.nodeValue, g, new Font("Arial", 15), new Rectangle(0, 0, 500, 500), null, null);
                    }
                    row.rowRect = new Rectangle(0, 0, row.rowLen, letterHeight);
                }
                //if (node.rows.Count>1) { //Fractions, matrices
                    rtn.Height = letterHeight*node.rows.Count+(10*node.rows.Count);
                //}
                return rtn;
            }
        }
        public class LayoutFactory<TNode, TRtns> where TNode : INode<TNode>, 
                                                               IRenderNode<TNode>,
                                                               new()
                                                 where  TRtns: IWalkerArgsRtns,
                                                               new()
            {
            //ON THE BENCH
            public ITreeWalker<TNode, SizeArgsRtns> layoutWalker = new OriginalTreeWalker<TNode, SizeArgsRtns>();
            //public ITreeWalker<TNode, TRtns> GetWalker() => (ITreeWalker<TNode, TRtns>)layoutWalker;
            public ITreeWalker<TNode, TRtns> GetWalker(Genus version) {
                ITreeWalker<TNode, TRtns> layoutWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, SizeArgsRtns>();
                switch (version) {
                    case Genus.OriginalClient:
                        layoutWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, SizeArgsRtns>();
                        break;
                    case Genus.OriginalService:
                        layoutWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, SizeArgsRtns>();
                        break;
                    case Genus.NewStructure:
                        layoutWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, SizeArgsRtns>();
                        break;
                    default:
                        break;
                }
                return layoutWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, SizeArgsRtns> layoutWalker) {
                switch(version){
                    case Genus.OriginalClient:
                        //layoutWalker.RowOp = LayoutProcessRows;
                        layoutWalker.RowOp = SizeProcessRows;
                        //layoutWalker.PreColumnOp = LayoutPreColumnOp;
                        //layoutWalker.PostColumnOp = LayoutPostColumnOp;
                        break;
                    case Genus.OriginalService:
                        layoutWalker.RowOp = SizeProcessRowsService;
                        break;
                    case Genus.NewStructure:
                        layoutWalker.RowOp = LayoutNewRowOp;
                        //layoutWalker.PreColumnOp = LayoutPreColumnOp;
                        //layoutWalker.PostColumnOp = LayoutNewPostColumnOp;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            public SizeArgsRtns LayoutNewRowOp(TNode node, SizeArgsRtns rtn) {
                Point offset = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);
                var letterHeight = 15;
                foreach (var rowNode in node.rows) {
                    if (rowNode.nodeValue.Trim() != ",") {
                        rowNode.rowLen+=5;
                    }
                    if (rtn.uniformSize) {
                        rowNode.rowLen= rtn.uniformedWidth+5;
                    }
                }
                if (node.colType==ColTyp.bracket) {
                    rtn.Width=10;
                }
                int maxWidth = node.rows.Max(r => r.rowLen);

                node.rows[0].rowRect = new Rectangle(rtn.Width, 0, node.rows[0].rowLen, letterHeight);
                if (node.rows.Count > 1){ //Fractions, matrices
                    rtn.Height =Math.Max(rtn.Height, letterHeight * node.rows.Count + (10* node.rows.Count));
                    for (int i = 0; i<node.rows.Count; i++) {
                        //node.rows[i].rowRect = new Rectangle(rtn.Width, letterHeight + 10, node.rows[i].rowLen, letterHeight);
                        node.rows[i].rowRect = new Rectangle(offset.X+rtn.Width, offset.Y+((letterHeight * i)+ (10*i)), node.rows[i].rowRect.Width, node.rows[i].rowRect.Height);
                    }
                }
                rtn.Width += maxWidth + (node.colType==ColTyp.bracket ? 10:0);
                return rtn;
            }
            public SizeArgsRtns LayoutProcessRows(TNode answerCol, SizeArgsRtns rtn) {
                var letterHeight = 19;
                foreach (var ansRow in answerCol.rows) {
                    if (ansRow.nodeValue.Trim() != ",") {
                        ansRow.rowLen+=10;
                    }
                    if (rtn.uniformSize) {
                        ansRow.rowLen= rtn.uniformedWidth+10;
                    }
                }
                //Sizing a single column/fraction
                int maxWidth = answerCol.rows.Max(r => r.rowLen);
                if (answerCol.rows.Count == 1) { //Single row
                    answerCol.rows[0].rowRect = new Rectangle(rtn.Width, 0, maxWidth, letterHeight);
                } else if (answerCol.rows.Count == 2) { //Double row
                    rtn.Height =Math.Max(rtn.Height, letterHeight * 2 + 10);
                    answerCol.rows[0].rowRect
                    = new Rectangle(rtn.Width, 0, answerCol.rows[0].rowLen, letterHeight);
                    answerCol.rows[1].rowRect
                    = new Rectangle(rtn.Width, letterHeight + 10, answerCol.rows[1].rowLen, letterHeight);
                }
                rtn.Width += maxWidth;
                return rtn;
            }
            public SizeArgsRtns SizeProcessRows(TNode answerCol, SizeArgsRtns rtn) {
                var letterHeight = 15;
                foreach (var ansRow in answerCol.rows) {
                    if (ansRow.nodeValue.Trim() != ",") {
                        ansRow.rowLen+=10;
                    }
                    if (rtn.uniformSize) {
                        ansRow.rowLen= rtn.uniformedWidth+10;
                    }
                }
                //Sizing a single column/fraction
                int maxWidth = answerCol.rows.Max(r => r.rowLen);
                if (answerCol.rows.Count == 1) { //Single row
                    answerCol.rows[0].rowRect = new Rectangle(rtn.Width, 0, maxWidth, letterHeight);
                } else if (answerCol.rows.Count == 2) { //Double row
                    rtn.Height =Math.Max(rtn.Height, letterHeight * 2 + 10);
                    answerCol.rows[0].rowRect
                    = new Rectangle(rtn.Width, 0, answerCol.rows[0].rowLen, letterHeight);
                    answerCol.rows[1].rowRect
                    = new Rectangle(rtn.Width, letterHeight + 10, answerCol.rows[1].rowLen, letterHeight);
                }
                rtn.Width += maxWidth;
                return rtn;
            }
            public SizeArgsRtns SizeProcessRowsService(TNode qc, SizeArgsRtns rtn) {
                var letterHeight = 15;
                var expLetterHeight = 8;
                // X's and Y's (rowRects)
                float mid = (float)rtn.Height/2f; // PROVISIONAL
                float dynY = 0;
                Point offset = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);

                if (!rtn.exponent) {
                    foreach (var ansRow in qc.rows) {
                        //if (ansRow.rowChunks[0].elementChunk.Trim() != ",") { // Standard spacing
                        ansRow.rowLen+=5;
                        //}
                        if (rtn.uniformSize) {
                            ansRow.rowLen= rtn.uniformedWidth+5;
                        }
                    } //Widths only
                }
                int maxRowLen = qc.rows.Max(r => r.rowLen);

                if (rtn.exponent) {
                    //---------------------------------
                    // Am I processing an exponent now?
                    //---------------------------------
                    var maxRowWidth = 0; var myWidth = 0;
                    // -- centering num/denom --
                    maxRowWidth = qc.rows.Max(r => r.rowLen); //qc.rows.Max(r => r.rowRect.Width);
                    var midX = maxRowWidth/2;
                    // -------------------------
                    for (int i = 0; i<qc.rows.Count; i++) {  // 1..n rows    (singles/fractionals)
                        var row = qc.rows[i];
                        dynY = (rtn.exponentPoint.Y - (((float)qc.rows.Count/2f)*(expLetterHeight-5)))+((expLetterHeight-5)*i); //Hug the division line closer
                        dynY += expLetterHeight/3;
                        myWidth = row.rowLen;                 //row.rowRect.Width;                    
                        row.rowRect = new Rectangle(offset.X + rtn.exponentPoint.X+midX-myWidth/2, offset.Y + (int)dynY,
                                                     row.rowRect.Width, row.rowRect.Height);
                    }
                    rtn.exponentPoint = new Rectangle(rtn.exponentPoint.X+maxRowWidth+1, rtn.exponentPoint.Y,
                                                      maxRowWidth+5, qc.rows.Sum(r => r.rowRect.Height));
                } else {
                    //------------
                    // Normal size
                    //------------
                    var exGWidth = 0; var exGStart = 0; var exGEnd = 0; var exponents = false;
                    for (int i = 0; i<qc.rows.Count; i++) {  // 1..n rows    (singles/fractionals)
                        var row = qc.rows[i];
                        dynY = (mid- (((float)qc.rows.Count/2f)*letterHeight))+(letterHeight*i);
                        row.rowRect = new Rectangle(offset.X + rtn.Width + maxRowLen/2-row.rowLen/2, offset.Y + (int)dynY,
                                                    row.rowRect.Width+2, row.rowRect.Height);
                        //-------------------------------
                        //Does this row have an exponent?
                        //-------------------------------
                        if (row.columns.Count == 1 && row.columns[0].colType == ColTyp.exponent && row.columns[0].columns.Count > 0) {
                            exponents=true;
                            rtn.exponent = true;
                            var exponentGroup = row.columns[0]; //Grouping column, no drawn parts. Stores exponent group width.
                            rtn.exponentPoint = new Rectangle(row.rowRect.X+row.rowRect.Width, row.rowRect.Y,
                                                              row.rowRect.Width+2, row.rowRect.Height);
                            exGStart = rtn.exponentPoint.X;
                            //ON THE BENCH
                            rtn = layoutWalker.Traverse(exponentGroup.columns, rtn, rtn.depth);
                            exGEnd = rtn.exponentPoint.X;
                            exGWidth=exGEnd-exGStart;
                            row.rowExpLen = exGWidth;
                            rtn.exponent = false;
                        }
                    }
                    int colStart = Utils.min(qc.rows.Select(r => r.rowRect.X).ToArray())+2;
                    int colEnd = Utils.max(qc.rows.Select(r => r.rowRect.Width).ToArray())-1 + colStart;
                    var colBasesMidX = colStart+(colEnd-colStart)/2;
                    //If there's multiple rows & one+ has an exponent, growth of overall width (rtn.Width) won't be straightforward (see draw Division line)
                    var calculatedWidths = new List<int> { };
                    qc.rows.ForEach(row => calculatedWidths.Add(colBasesMidX + row.rowRect.Width/2 + row.rowExpLen)); // allBasesMid + (halfMyBaseWidth + myExpWidth)
                    colEnd = Utils.max(calculatedWidths.ToArray());
                    rtn.Width += (colEnd-colStart)+10;
                }
                return rtn;
            }
            public SizeArgsRtns LayoutPreColumnOp(TNode ac, SizeArgsRtns rtn) {
                var font = new Font("Arial", 15);
                if (ac.colType==ColTyp.bracket)
                    rtn.Width +=10;
                if (ac.colType==ColTyp.sigma) {
                    int len = drawstuff.GetStringLength(ac.from+"__", font);
                    ac.rowRect=new Rectangle(rtn.Width, 0, len, 1);
                    rtn.Width+=len;
                }

                return rtn;
            }
            public SizeArgsRtns LayoutPostColumnOp(TNode ac, SizeArgsRtns rtn) {
                var letterHeight = 19;
                float mid = (float)rtn.Height/2f;
                float dynY = (mid- ((float)ac.rows.Count/2f*letterHeight));// TODO: sync

                if (ac.colType==ColTyp.bracket)
                    rtn.Width +=10;
                if (ac.colType==ColTyp.sigma) {
                    ac.rowRect=new Rectangle(ac.rowRect.X, (int)dynY, rtn.Width, 3*letterHeight);
                    rtn.Height=Math.Max(rtn.Height, 5*letterHeight);
                }

                return rtn;
            }


        }
        public class DrawFactory<TNode, TRtns> : IDrawFactory where TNode : INode<TNode>, 
                                                             IRenderNode<TNode>, 
                                                             new()
                                               where TRtns : IWalkerArgsRtns,
                                                             new(){
            private Pen divisionPen { get; set; } = new Pen(Color.Black, 2);
            //public ITreeWalker<TNode, DrawArgsRtns> drawWalker = new TreeWalker<TNode, DrawArgsRtns>();
            //public ITreeWalker<TNode, TRtns> GetWalker() => (ITreeWalker<TNode, TRtns>)drawWalker;
            public ITreeWalker<TNode, TRtns> GetWalker(Genus version) {
                ITreeWalker<TNode, TRtns> drawWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, DrawArgsRtns>();
                switch (version) {
                    case Genus.OriginalClient:
                        drawWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, DrawArgsRtns>();
                        break;
                    case Genus.OriginalService:
                        throw new NotImplementedException();
                    case Genus.NewStructure:
                        //drawWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, DrawArgsRtns>();
                        drawWalker = (ITreeWalker<TNode, TRtns>)new WalkNodeInEverythingRow<TNode, DrawArgsRtns>();
                        break;
                    default:
                        break;
                }
                return drawWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, DrawArgsRtns> drawWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        drawWalker.RowOp = DrawProcessRows;
                        drawWalker.PreColumnOp = DrawPreProcessColumns;
                        drawWalker.PostColumnOp = DrawPostProcessColumns;
                        //drawWalker.PostTraversalOp = DrawPostTraversal;
                        break;
                    case Genus.NewStructure:
                        drawWalker.RowOp = DrawProcessRows;
                        //drawWalker.PreColumnOp = DrawPreProcessColumns;
                        //drawWalker.PostColumnOp = DrawPostProcessColumns;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            public DrawArgsRtns DrawProcessRows(TNode answerColumn, DrawArgsRtns rtn) {
                var letterHeight = 15;
                var font = new Font("Arial", 10);

                //if (answerColumn.colType==ColTyp.real) { }    // sorted with fractions
                //if (answerColumn.colType==ColTyp.integer) { } // sorted with fractions
                if (answerColumn.colType==ColTyp.product) { }   // draw prodct
                if (answerColumn.colType==ColTyp.sigma) { }     // draw sigma
                if (answerColumn.colType==ColTyp.integral) { }  // draw integral
                if (answerColumn.colType==ColTyp.complex) { }   // draw complex
                if (answerColumn.colType==ColTyp.factorial) { } // draw factorial
                if (answerColumn.colType==ColTyp.choose) { }    // draw choose
                if ((answerColumn.colType & (ColTyp.fraction ))>0) {

                    // Draw_ProcessIndividualFractionStyledExpressionColumns

                    // Brackets
                    if ((answerColumn.colType & (ColTyp.bracket))>0) {
                        var minX = answerColumn.rows.Min(row=>row.rowRect.X);
                        var minY = answerColumn.rows.Min(row => row.rowRect.Y);
                        var height = rtn.Height;
                        rtn.ansBackBuffer.DrawLine(divisionPen,
                            new Point(minX-8, minY),
                            new Point(minX-8, minY + height));

                        rtn.ansBackBuffer.DrawLine(divisionPen,
                            new Point(minX + rtn.Width+8, minY),
                            new Point(minX + rtn.Width+8, minY + height));
                    }
                    foreach (var row in answerColumn.rows) { // One or two rows...
                                                             // Draw -------------------------------------------------
                                                             // 
                        if ((answerColumn.rows.Count == 1) && rtn.Height > letterHeight) { // double expression, single term
                            var y = rtn.Height/2-letterHeight/2;
                            rtn.ansBackBuffer.DrawString(row.nodeValue, font, Brushes.Blue, row.rowRect.X, y);
                        } else {                              // single expression
                            rtn.ansBackBuffer.DrawString(row.nodeValue, font, Brushes.Blue, row.rowRect);
                        }

                        // Block over the top ------------------------------------
                        row.charCount=0;
                        row.toBlock.Clear();

                        if (!row.answered && !rtn.ShowAnswers)
                            row.toBlock.Add(new CharacterRange(0, row.nodeValue.Length));

                        row.blockRegions = drawstuff.GetRegionArray(row.nodeValue, font, new Rectangle(0, 0, 500, 500), row.toBlock.ToArray(), rtn.ansBackBuffer); 
                        foreach (var reg in row.blockRegions) {
                            row.boundsRect = Rectangle.Round(reg.GetBounds(rtn.ansBackBuffer));
                            row.blockRect = new Rectangle(row.rowRect.X + row.boundsRect.X,
                                                           row.rowRect.Y + row.boundsRect.Y,
                                                                           row.boundsRect.Width,
                                                                           row.boundsRect.Height);
                            if ((answerColumn.rows.Count == 1) && rtn.Height > letterHeight) { // double expression, single term
                                var y = rtn.Height/2-letterHeight/2;
                                rtn.ansBackBuffer.FillRectangle(Brushes.Orange, row.blockRect.X, y, row.blockRect.Width, row.blockRect.Height);
                            } else {
                                rtn.ansBackBuffer.FillRectangle(Brushes.Orange, row.blockRect);
                            }
                        }
                    }
                    // -- division line --
                    if (answerColumn.rows.Count > 1 && answerColumn.showDiv) {
                        int x = Utils.min(answerColumn.rows.Select(r => r.rowRect.X).ToArray());
                        rtn.ansBackBuffer.DrawLine(divisionPen,
                            new Point(x, letterHeight+6),
                            new Point(x + Utils.max(answerColumn.rows.Select(ar => ar.rowLen).ToArray()) -5, letterHeight+6));
                    }
                }
                return rtn;
            }
            public DrawArgsRtns DrawPreProcessColumns(TNode answerCol, DrawArgsRtns rtn) {
                foreach (TNode ac in answerCol.rows) {
                    if (ac.colType == ColTyp.bracket) {
                        rtn.Width += 30; // both ends
                                         // draw left bracket (graphics, X,Y coordinates),,,
                    } else if (ac.colType == ColTyp.brace) {
                        rtn.Width += 30; // both ends
                                         // draw left brace
                    } else if (ac.colType == ColTyp.squareBracket) {
                        rtn.Width += 30; // both ends
                                         // draw left square bracket
                    } else if (ac.colType == ColTyp.rooted) {
                        rtn.Width += 30; // both ends
                                         //rtn.YPadding += 10;
                                         // draw root side
                    }
                }
                return rtn;
            }
            public DrawArgsRtns DrawPostProcessColumns(TNode ac, DrawArgsRtns rtn) {
                var letterHeight = 15;
                var font = new Font("Arial", 15);
                //foreach (wfNode ac in answerCol.answerRows) {
                if (ac.colType == ColTyp.bracket) {
                    // draw right bracket
                } else if (ac.colType == ColTyp.brace) {
                    // draw right brace
                } else if (ac.colType == ColTyp.squareBracket) {
                    // draw right square bracket
                } else if (ac.colType == ColTyp.rooted) {
                    // draw root ceiling
                }
                if (ac.colType == ColTyp.sigma) {
                    #region sigma
                    int row = letterHeight;
                    Point[] points = new Point[]{
                        new Point(ac.rowRect.Width/2+17,row+2),
                        new Point(ac.rowRect.Width/2+15,row),
                        new Point(ac.rowRect.Width/2-15,row),
                        new Point(ac.rowRect.Width/2,   row/2+ac.rowRect.Height/2-1),
                        new Point(ac.rowRect.Width/2-15,ac.rowRect.Height-1),
                        new Point(ac.rowRect.Width/2+15,ac.rowRect.Height-1),
                        new Point(ac.rowRect.Width/2+17,ac.rowRect.Height-3),
                    };
                    Pen p = new Pen(Color.Black, 2);
                    //PathDraws.Add(new PathDraw(points, p));
                    var gp = new GraphicsPath();
                    for (int i = 1; i<points.Count(); i++) {
                        gp.AddLine(points[i-1], points[i]);
                    }
                    rtn.ansBackBuffer.DrawPath(p, gp);

                    //AddTextDraw($@"n={ac.from}", font,
                    //    new Point(2+ac.rowRect.Width/2-GetStringLength("n="+ac.from, font)/2, ac.rowRect.Height));
                    rtn.ansBackBuffer.DrawString($@"n={ac.from}", font, Brushes.Black,
                        new Rectangle(2+ac.rowRect.Width/2-drawstuff.GetStringLength("n="+ac.from, font)/2, ac.rowRect.Height,
                        drawstuff.GetStringLength("n="+ac.from, font)+10, letterHeight));
                    if (ac.Infinity) {
                        //AddTextDraw($@"∞", font, new Point(ac.rowRect.Width/2-3, 0));
                        rtn.ansBackBuffer.DrawString($@"∞", font, Brushes.Black,
                            new Rectangle(ac.rowRect.Width/2-3, 0,
                            10, letterHeight));
                    } else {
                        //AddTextDraw($@"{ac.to}", font,
                        //    new Point(2+ac.rowRect.Width/2-GetStringLength(""+ac.to, font)/2, 0));
                        rtn.ansBackBuffer.DrawString($@"{ac.to}", font, Brushes.Black,
                            new Rectangle(ac.rowRect.Width/2-3, 0,
                            drawstuff.GetStringLength(""+ac.to, font)+10, letterHeight));

                    }
                    #endregion

                }
                //}
                return rtn;
            }
        }
        public class DrawFactoryUnanswerable<TNode, TRtns> : IDrawFactory where TNode : INode<TNode>,
                                                             IRenderNode<TNode>,
                                                             new()
                                               where TRtns : IWalkerArgsRtns,
                                                             new() {
            private Pen divisionPen { get; set; } = new Pen(Color.Black, 2);
            public ITreeWalker<TNode, DrawArgsRtns> drawWalker = new OriginalTreeWalker<TNode, DrawArgsRtns>();
            public ITreeWalker<TNode, TRtns> GetWalker() => (ITreeWalker<TNode, TRtns>)drawWalker;
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, DrawArgsRtns> drawWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        drawWalker.RowOp = DrawProcessRows;
                        //drawWalker.PreColumnOp = DrawPreProcessColumns;
                        //drawWalker.PostColumnOp = DrawPostProcessColumns;
                        //drawWalker.PostTraversalOp = DrawPostTraversal;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            public DrawArgsRtns DrawProcessRows(TNode answerColumn, DrawArgsRtns rtn) {
                var letterHeight = 19;
                var font = new Font("Arial", 15);

                //if (answerColumn.colType==ColTyp.real) { }    // sorted with fractions
                //if (answerColumn.colType==ColTyp.integer) { } // sorted with fractions
                if (answerColumn.colType==ColTyp.product) { }   // draw prodct
                if (answerColumn.colType==ColTyp.sigma) { }     // draw sigma
                if (answerColumn.colType==ColTyp.integral) { }  // draw integral
                if (answerColumn.colType==ColTyp.complex) { }   // draw complex
                if (answerColumn.colType==ColTyp.factorial) { } // draw factorial
                if (answerColumn.colType==ColTyp.choose) { }    // draw choose
                if (answerColumn.colType==ColTyp.fraction) {

                    // Draw_ProcessIndividualFractionStyledExpressionColumns
                    foreach (var row in answerColumn.rows) { // One or two rows...
                                                             // Draw -------------------------------------------------
                                                             // 
                        if ((answerColumn.rows.Count == 1) && rtn.Height > letterHeight) { // double expression, single term
                            var y = rtn.Height/2-letterHeight/2;
                            rtn.ansBackBuffer.DrawString(row.nodeValue, font, Brushes.Blue, row.rowRect.X, y);
                        } else {                              // single expression
                            rtn.ansBackBuffer.DrawString(row.nodeValue, font, Brushes.Blue, row.rowRect);
                        }

                    }
                    // -- division line --
                    if (answerColumn.rows.Count > 1 && answerColumn.showDiv) {
                        int x = Utils.min(answerColumn.rows.Select(r => r.rowRect.X).ToArray());
                        rtn.ansBackBuffer.DrawLine(divisionPen,
                            new Point(x, letterHeight+6),
                            new Point(x + Utils.max(answerColumn.rows.Select(ar => ar.rowLen).ToArray()) -5, letterHeight+6));
                    }
                }
                return rtn;
            }
            public DrawArgsRtns DrawPreProcessColumns(TNode answerCol, DrawArgsRtns rtn) {
                foreach (TNode ac in answerCol.rows) {
                    if (ac.colType == ColTyp.bracket) {
                        rtn.Width += 30; // both ends
                                         // draw left bracket (graphics, X,Y coordinates),,,
                    } else if (ac.colType == ColTyp.brace) {
                        rtn.Width += 30; // both ends
                                         // draw left brace
                    } else if (ac.colType == ColTyp.squareBracket) {
                        rtn.Width += 30; // both ends
                                         // draw left square bracket
                    } else if (ac.colType == ColTyp.rooted) {
                        rtn.Width += 30; // both ends
                                         //rtn.YPadding += 10;
                                         // draw root side
                    }
                }
                return rtn;
            }
            public DrawArgsRtns DrawPostProcessColumns(TNode ac, DrawArgsRtns rtn) {
                var letterHeight = 19;
                var font = new Font("Arial", 15);
                //foreach (wfNode ac in answerCol.answerRows) {
                if (ac.colType == ColTyp.bracket) {
                    // draw right bracket
                } else if (ac.colType == ColTyp.brace) {
                    // draw right brace
                } else if (ac.colType == ColTyp.squareBracket) {
                    // draw right square bracket
                } else if (ac.colType == ColTyp.rooted) {
                    // draw root ceiling
                }
                if (ac.colType == ColTyp.sigma) {
                    #region sigma
                    int row = letterHeight;
                    Point[] points = new Point[]{
                        new Point(ac.rowRect.Width/2+17,row+2),
                        new Point(ac.rowRect.Width/2+15,row),
                        new Point(ac.rowRect.Width/2-15,row),
                        new Point(ac.rowRect.Width/2,   row/2+ac.rowRect.Height/2-1),
                        new Point(ac.rowRect.Width/2-15,ac.rowRect.Height-1),
                        new Point(ac.rowRect.Width/2+15,ac.rowRect.Height-1),
                        new Point(ac.rowRect.Width/2+17,ac.rowRect.Height-3),
                    };
                    Pen p = new Pen(Color.Black, 2);
                    //PathDraws.Add(new PathDraw(points, p));
                    var gp = new GraphicsPath();
                    for (int i = 1; i<points.Count(); i++) {
                        gp.AddLine(points[i-1], points[i]);
                    }
                    rtn.ansBackBuffer.DrawPath(p, gp);

                    //AddTextDraw($@"n={ac.from}", font,
                    //    new Point(2+ac.rowRect.Width/2-GetStringLength("n="+ac.from, font)/2, ac.rowRect.Height));
                    rtn.ansBackBuffer.DrawString($@"n={ac.from}", font, Brushes.Black,
                        new Rectangle(2+ac.rowRect.Width/2-drawstuff.GetStringLength("n="+ac.from, font)/2, ac.rowRect.Height,
                        drawstuff.GetStringLength("n="+ac.from, font)+10, letterHeight));
                    if (ac.Infinity) {
                        //AddTextDraw($@"∞", font, new Point(ac.rowRect.Width/2-3, 0));
                        rtn.ansBackBuffer.DrawString($@"∞", font, Brushes.Black,
                            new Rectangle(ac.rowRect.Width/2-3, 0,
                            10, letterHeight));
                    } else {
                        //AddTextDraw($@"{ac.to}", font,
                        //    new Point(2+ac.rowRect.Width/2-GetStringLength(""+ac.to, font)/2, 0));
                        rtn.ansBackBuffer.DrawString($@"{ac.to}", font, Brushes.Black,
                            new Rectangle(ac.rowRect.Width/2-3, 0,
                            drawstuff.GetStringLength(""+ac.to, font)+10, letterHeight));

                    }
                    #endregion

                }
                //}
                return rtn;
            }

        }
        public class KeysFactory<TNode, TRtns> where TNode : INode<TNode>, IRenderNode<TNode>, new() where TRtns : IWalkerArgsRtns, new() {
            //public ITreeWalker<TNode, KeysArgsRtns> keysWalker = new TreeWalker<TNode, KeysArgsRtns>();
            //public ITreeWalker<TNode, TRtns> GetWalker() => (ITreeWalker<TNode, TRtns>)keysWalker;
            public ITreeWalker<TNode, TRtns> GetWalker(Genus version) {
                ITreeWalker<TNode, TRtns> keysWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, KeysArgsRtns>();
                switch (version) {
                    case Genus.OriginalClient:
                        keysWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, KeysArgsRtns>();
                        break;
                    case Genus.OriginalService:
                        throw new NotImplementedException();
                    case Genus.NewStructure:
                        keysWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, KeysArgsRtns>();
                        break;
                    default:
                        break;
                }
                return keysWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, KeysArgsRtns> keysWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        keysWalker.RowOp=KeysProcessRows;
                        break;
                    case Genus.NewStructure:
                        keysWalker.RowOp=KeysProcessRows;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            public KeysArgsRtns KeysProcessRows(TNode answerNode, KeysArgsRtns rtn) {
                // One single keypress...
                // Every question queried...

                // InputSoFar emptied after every succesful chunk-match...
                // Numerators mandatory ...

                // ------------------------------------------------- \\
                // -- KeysWalker needs to reach values
                // --     Housing, structure, layout   - no
                // --     Ops, separators, punctuation - no
                // --
                // -- Structure irrelevant - the appropriate sequence is all that's required
                //
                TNode nextRow = new TNode();

                if (answerNode.rows.Any(a => a.ansType == ansType.Num))
                    nextRow = answerNode.rows.First(a => a.ansType == ansType.Num);
                if (nextRow.IsColumn) //IsEmpty
                    nextRow = answerNode.rows[0];
                if (!nextRow.answered) { // Skip over answered nums/denoms
                                         //for (int i = 0; i<nextRow.rowChunks.Count; i++) {
                                         //wfNodeElement ac = nextRow.rowChunks[i]; // each chunk...
                    if (nextRow.nodeValue== rtn.InputSoFar) { // Match !!
                        rtn.hit = true;
                        rtn.Exit = true;
                        nextRow.answered=true;

                        // Row now answered if all answerChunks answered
                        // possibleAnswer now answered if all Rows in all Columns answered
                        return rtn; // Any chunk can match, but only one...
                                    //} else if (ac.elementChunk.StartsWith(rtn.InputSoFar, StringComparison.InvariantCulture)) { // Partial Match !!
                                    // element/chunk partially matched
                    } else if (nextRow.nodeValue.StartsWith(rtn.InputSoFar, StringComparison.InvariantCulture)) { // Partial Match !!
                                                                                                                  // element/chunk partially matched

                        rtn.keepIt = true;
                        rtn.inKeysColour = Color.Black; //Alright so far..., so keep looping chunks...
                    } else if (rtn.IsSequence) {
                        rtn.Exit = true;
                        return rtn;
                    }
                } // nextRow.answered

                // Denominator if present..
                nextRow = default;
                
                if (answerNode.rows.Count == 2) { // Not that good at this
                    if (answerNode.rows.Any(a => a.ansType == ansType.Den))
                        nextRow = answerNode.rows.First(a => a.ansType == ansType.Den);
                    if (nextRow == null)
                        nextRow = answerNode.rows[1];
                    if (!nextRow.answered) {  // Skip over answered nums/denoms
                                              //for (int i = 0; i<nextRow.rowChunks.Count; i++) { wfNodeElement ac = nextRow.rowChunks[i]; // each chunk...
                        // Any chunk in the row can match, but only one of them...
                        //if ((!ac.answered) && ac.elementChunk == rtn.InputSoFar) { // Match !!
                        if ((!nextRow.answered) && nextRow.nodeValue== rtn.InputSoFar) { // Match !!
                            rtn.hit = true;
                            rtn.Exit = true;

                            nextRow.answered=true;
                            // Row now answered if all answerChunks answered
                            // possibleAnswer now answered if all Rows in all Columns answered
                            return rtn;
                        } else if (nextRow.nodeValue.StartsWith(rtn.InputSoFar, StringComparison.InvariantCulture)) { // Partial Match !!
                                                                                                                      // element/chunk partially matched
                            rtn.keepIt = true;
                            rtn.inKeysColour = Color.Black; //Alright so far..., so keep looping chunks...
                        } // Matched...?
                    } // nexttRow.answered
                }
                return rtn;
            }
        }
        public class EvaluateFactory { }
    }
    public interface IDrawFactory { }
    public enum Genus: int {
            OriginalClient  = 1,
            OriginalService = 2,
            NewStructure = 4
    }

    public static class drawstuff {
        public static int GetStringLength(string txt, Font font) {
            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                return GetStringLength(txt, g, font, new Rectangle(0, 0, 0, 0), null, null)+10;
            }
        }
        public static StringFormat GetStringFormat(string str) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, str.Length) });
            return strFmt;
        }
        public static StringFormat GetStringFormat(CharacterRange[] crArray) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(crArray);
            return strFmt;
        }
        public static Region[] GetRegionArray(string str, Font font, Rectangle rect, StringFormat strFmt, Graphics g) {
            if (strFmt is null)
                strFmt = GetStringFormat(str);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }
        public static int GetStringLength(string str, Graphics g, Font font, Rectangle rect, StringFormat strFmt, Region[] region) {
            if (region is null) // Get whole string
                region = GetRegionArray(str, font, rect, strFmt, g);
            return (int)region.Select(r => r.GetBounds(g).Width).Sum();
        }
        public static Region[] GetRegionArray(string str, Font font, Rectangle rect, CharacterRange[] crArray, Graphics g) {
            StringFormat strFmt = GetStringFormat(crArray);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }
   }
}
