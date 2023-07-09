﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathUtils;

namespace Fangorn {

    /* Notes
     * The full set:
     *      So must include types FROM and TO both for converts
     */
    public class Treebeard<TNode, TNodeTo> where TNode : INode<TNode>, IRenderNode<TNode>, new()
                                          where TNodeTo : INode<TNodeTo>, IRenderNode<TNodeTo>, new() {
        public class OriginalTreeWalker<TNode, TRtn> : ITreeWalker<TNode, TRtn>
                     where TRtn : IWalkerArgsRtns
                     where TNode : INode<TNode> {
            public TRtn Traverse(Genus genus, IPossibleAnswer<TNode> possibleAnswer, TRtn rtn) {
                switch (genus) {
                    case Genus.OriginalClient:
                        rtn=  Traverse(possibleAnswer.answer, rtn);
                        break;
                    case Genus.OriginalService:
                        rtn= Traverse(possibleAnswer.answer, rtn);
                        break;
                    case Genus.NewStructure:
                        rtn=Traverse(possibleAnswer.answerNode, rtn);
                        break;
                    default:
                        break;
                }
                return rtn;
            }
            public TRtn Traverse(TNode nodeIn, TRtn rtn, int depth = 0) => rtn;
            public TRtn Traverse(List<TNode> arcs, TRtn rtn, int depth = 0) {
                //----------------------------------
                //Convert, measure, size, draw, keys
                //----------------------------------
                rtn.depth = depth;
                if (depth==0) {
                    rtn= PreTraversalOp(arcs[0], rtn); 
                    //arcs[0] is a hack.
                    //The original client genus will be discouraged going forward
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
            public Func<TNode, TRtn, TRtn> PreTraversalOp { get; set; } = (a, b) => b;
            public Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> RowOp { get; set; } = (a, b) => b;
            #endregion
        }
        public class WalkNodeInEverythingRow<TNode, TRtn> : ITreeWalker<TNode, TRtn>
             where TRtn : IWalkerArgsRtns
             where TNode : INode<TNode> {
            public TRtn Traverse(Genus genus, IPossibleAnswer<TNode> possibleAnswer, TRtn rtn) {
                switch (genus) {
                    case Genus.OriginalClient:
                        rtn=  Traverse(possibleAnswer.answer, rtn);

                        break;
                    case Genus.OriginalService:
                        rtn= Traverse(possibleAnswer.answer, rtn);

                        break;
                    case Genus.NewStructure:
                        rtn=Traverse(possibleAnswer.answerNode, rtn);

                        break;
                    default:
                        break;
                }
                return rtn;
            }

            public TRtn Traverse(TNode nodeIn, TRtn rtn, int depth = 0) {
                //----------------------------------------------
                //Convert, measure, layout, draw, keys, evaluate
                //----------------------------------------------
                rtn.depth = depth;
                if (depth==0) {
                    // Conv: Create a root holder in rtn - set nodeIn parent to dummy node
                    rtn= PreTraversalOp(nodeIn, rtn);
                }

                // Conv: Copy this node
                // Meas: If I'm rooted increment my parent row rootCount
                // Layt: Adjust width & height for root
                rtn = PreColumnOp(nodeIn, rtn);

                
                for (int j = 0; j<nodeIn.rows.Count; j++) {  // Every node has at least one row
                    TNode row = nodeIn.rows[j];

                    // Conv: Copy this row
                    rtn = PreRowOp(row, rtn);

                    // Layt: for won't enter if row has a nodeValue
                    for (int i = 0; i<row.columns.Count; i++) {
                        TNode node = row.columns[i];
                        depth--;
                        rtn = Traverse(node, rtn, depth);
                        depth++;
                    }

                    // Conv: Reset currentNode for rows
                    rtn = PostRowOp(row, rtn);
                }
                if (rtn.Exit) return rtn;

                // Conv: Reset currentNode for columns
                // Meas: Increase my parent row rootCount by my ancestor rootCount
                // Layt: Adjust width & height for roots
                rtn = PostColumnOp(nodeIn, rtn);
                return rtn;
            }
            #region blank ops
            public TRtn Traverse(List<TNode> arcs, TRtn rtn, int depth = 0) => rtn;
            public Func<TNode, TRtn, TRtn> PreTraversalOp { get; set; } = (a, b) => b;
            public Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostColumnOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PreRowOp { get; set; } = (a, b) => b;
            public Func<TNode, TRtn, TRtn> PostRowOp { get; set; } = (a, b) => b;
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
                        convertWalker = (ITreeWalker<TNode, TRtns>)new WalkNodeInEverythingRow<TNode, ConvertArgsRtns<TNodeTo>>();
                        break;
                    default:
                        break;
                }
                return convertWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, ConvertArgsRtns<TNodeTo>> convertWalker) {
                switch (version) {
                    case Genus.OriginalClient:
                        convertWalker.PreTraversalOp = ClientPreTraversalOp;
                        convertWalker.PreColumnOp = ConvertPreColumnOp;
                        convertWalker.RowOp = ClientConvertRowOp;
                        convertWalker.PostColumnOp = ConvertPostColumnOp;
                        break;
                    case Genus.OriginalService:
                        throw new NotImplementedException();
                    case Genus.NewStructure:
                        convertWalker.PreTraversalOp = NewPreTraversalOp;
                        convertWalker.PreColumnOp = NewConvertPreColumnOp;
                        convertWalker.PreRowOp = NewConvertPreRowOp;
                        convertWalker.RowOp = NewConvertRowOp;
                        convertWalker.PostRowOp = NewConvertPostRowOp;
                        convertWalker.PostColumnOp = ConvertPostColumnOp;
                        break;
                    default:
                        break;
                }
            }
            public ConvertArgsRtns<TNodeTo> ClientPreTraversalOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                rtn.currentNode = new TNodeTo(); // dummy root
                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> NewPreTraversalOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                TNodeTo node = new TNodeTo();    // dummy root
                TNodeTo nodeRow = new TNodeTo(); // dummy row
                node.rows.Add(nodeRow);
                nodeRow.parent = node;
                rtn.currentNode = nodeRow;
                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> ConvertPreColumnOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                // Copy incoming node to new node

                TNodeTo node = new TNodeTo();
                node.colType = answerCol.colType;
                node.from= answerCol.from;
                node.to= answerCol.to;
                node.Infinity= answerCol.Infinity;

                // Add new node to (rtn) CurrentNode's columns (its parent)

                rtn.currentNode.columns.Add(node);

                // Explicitly set (rtn) CurrentNode as new node's parent

                node.parent = rtn.currentNode;

                // Make new node the (rtn) CurrentNode

                rtn.currentNode = node;

                // Between here and postColumn will be a traverse on incomingNodes' columns

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
            public ConvertArgsRtns<TNodeTo> NewConvertPreColumnOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                // Convert incoming column
                TNodeTo node = new TNodeTo() {
                    colType = answerCol.colType,
                    from= answerCol.from,
                    to= answerCol.to,
                    Infinity= answerCol.Infinity
                };

                // Add to parent
                // All columns' parents are now rows
                rtn.currentNode.columns.Add(node); //CurrentNode is currently a row

                // Preserve the structure
                node.parent = rtn.currentNode;

                // Make this column the CurrentNode
                rtn.currentNode = node;

                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> ConvertPostColumnOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                // Make (PreColumn) new node's parent the (rtn) CurrentNode again
                rtn.currentNode = rtn.currentNode.parent;
                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> NewConvertPostRowOp(TNode answerCol, ConvertArgsRtns<TNodeTo> rtn) {
                rtn.currentNode = rtn.currentNode.parent;
                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> NewConvertPreRowOp(TNode rowNode, ConvertArgsRtns<TNodeTo> rtn) {
                // Convert incoming row
                TNodeTo node = new TNodeTo() {
                    showDiv = rowNode.showDiv,
                    colType = rowNode.colType,
                    nodeValue = rowNode.nodeValue, // Only rows have values
                    index = rowNode.index
                };
                // Add to parent
                // All row's parents are columns
                rtn.currentNode.rows.Add(node); //CurrentNode is currently a column

                // Preserve the structure
                node.parent = rtn.currentNode;

                // Make this row the CurrentNode
                rtn.currentNode = node;

                return rtn;
            }
            public ConvertArgsRtns<TNodeTo> ClientConvertRowOp(TNode answerColumn, ConvertArgsRtns<TNodeTo> rtn) {

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
            public ConvertArgsRtns<TNodeTo> NewConvertRowOp(TNode rowNode, ConvertArgsRtns<TNodeTo> rtn) {
                // Now a single row operation
                TNodeTo node = new TNodeTo();
                node.showDiv = rowNode.showDiv; // needs to be a row-based 'underlined' thing now
                node.colType = rowNode.colType; // TODO
                node.nodeValue= rowNode.nodeValue;
                node.answered = rowNode.answered;

                rtn.currentNode.rows.Add(node);
                return rtn;
            }
            public void assignConversionResult(Genus version, IPossibleAnswer<TNodeTo> paTo, ConvertArgsRtns<TNodeTo> rtn) {
                switch (version) {
                    case Genus.OriginalClient:
                        paTo.answer.AddRange(rtn.currentNode.columns);
                        break;
                    case Genus.OriginalService:
                        paTo.answer.AddRange(rtn.currentNode.columns);
                        break;
                    case Genus.NewStructure:
                        paTo.answerNode = rtn.currentNode.columns[0];
                        break;
                    default:
                        break;
                }
            }
        }
        public class MeasureFactory<TNode, TRtns> where TNode : INode<TNode>, IRenderNode<TNode>, new()
            where TRtns : IWalkerArgsRtns, new() {
            public ITreeWalker<TNode, MeasureArgsRtns> measureWalker = new OriginalTreeWalker<TNode, MeasureArgsRtns>();
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
                        measureWalker = (ITreeWalker<TNode, TRtns>)new WalkNodeInEverythingRow<TNode, MeasureArgsRtns>();
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
                        measureWalker.PreRowOp = NewMeasurePreRowOp;
                        measureWalker.RowOp = NewMeasureRowOp;
                        measureWalker.PostRowOp = NewMeasurePostRowOp;
                        //measureWalker.PreColumnOp= NewMeasurePreColumnOp;
                        measureWalker.PostColumnOp= NewMeasurePostColumnOp;
                        break;
                    default:
                        break;
                }
            }
            public MeasureArgsRtns MeasureProcessRows(TNode answerCol, MeasureArgsRtns rtn) {
                var letterHeight = 19;
                foreach (var row in answerCol.rows) {
                    using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                        row.rowLen = drawstuff.GetStringLength(row.nodeValue, g, new Font("Arial", 15), new Rectangle(0, 0, 500, 500), null, null);
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
            public MeasureArgsRtns NewMeasurePreRowOp(TNode rowNode, MeasureArgsRtns rtn) {
                if (rowNode.nodeValue != null) { //Not all rows have nodeValues
                    var letterHeight = 15;
                    using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                        rowNode.rowLen = drawstuff.GetStringLength(rowNode.nodeValue, g, new Font("Consolas", 15), new Rectangle(0, 0, 500, 500), null, null);
                    }
                    rowNode.rowHeight= letterHeight;
                    rowNode.rowRect = new Rectangle(0, 0, rowNode.rowLen, letterHeight);
                    rtn.Height += letterHeight + 10;
                }
                return rtn;
            }
            public MeasureArgsRtns NewMeasureRowOp(TNode rowNode, MeasureArgsRtns rtn) {
                // Now a single row operation
                var letterHeight = 15;
                using (Graphics g = Graphics.FromImage(new Bitmap(1, 1))) {
                    rowNode.rowLen = drawstuff.GetStringLength(rowNode.nodeValue, g, new Font("Consolas", 15), new Rectangle(0, 0, 500, 500), null, null);
                }
                rowNode.rowRect = new Rectangle(0, 0, rowNode.rowLen, letterHeight);
                rtn.Height += letterHeight + 10;
                return rtn;
            }
            public MeasureArgsRtns NewMeasurePostRowOp(TNode rowNode, MeasureArgsRtns rtn) {
                if (rowNode.nodeValue == null) { // Container
                    rowNode.colsMaxRowlen = rowNode.columns.Max(c=>c.rows.Max(r => r.rowLen));
                    var rowsHeight = rowNode.columns.Max(c => c.rows.Sum(r => r.rowHeight));
                    var rowsCount = rowNode.columns.Count;
                    rowNode.colsVerticalCentre = (rowsHeight + (rowsCount-1)*10)/2;
                }
                if (rowNode.colType == ColTyp.rooted) {
                    rowNode.parent.innerRootCount++; // My root
                }
                rowNode.parent.innerRootCount += rowNode.innerRootCount; // My ancestors' roots 
                return rtn;
            }
            public MeasureArgsRtns NewMeasurePreColumnOp(TNode columnNode, MeasureArgsRtns rtn) {
                if (columnNode.colType == ColTyp.rooted) {
                    columnNode.parent.innerRootCount++; //My root
                }
                return rtn;
            }
            public MeasureArgsRtns NewMeasurePostColumnOp(TNode columnNode, MeasureArgsRtns rtn) {
                if (columnNode.parent != null) {
                    columnNode.parent.innerRootCount += columnNode.innerRootCount; //My ancestors' roots
                }
                return rtn;
            }
        }
        public class LayoutFactory<TNode, TRtns> where TNode : INode<TNode>, IRenderNode<TNode>, new()
                                                 where TRtns : IWalkerArgsRtns, new() {
            public ITreeWalker<TNode, SizeArgsRtns> layoutWalker = new OriginalTreeWalker<TNode, SizeArgsRtns>();
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
                        layoutWalker = (ITreeWalker<TNode, TRtns>)new WalkNodeInEverythingRow<TNode, SizeArgsRtns>();
                        break;
                    default:
                        break;
                }
                return layoutWalker;
            }
            public void GetWalkerOps(Genus version, ITreeWalker<TNode, SizeArgsRtns> layoutWalker) {
                switch (version) {
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
                        layoutWalker.PreRowOp = NewLayoutPreRowOp;
                        layoutWalker.PreColumnOp = NewLayoutPreColumnOp;
                        layoutWalker.PostColumnOp = NewLayoutPostColumnOp;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            public SizeArgsRtns NewLayoutPreRowOp(TNode rowNode, SizeArgsRtns rtn) {
                // Now a single row operation
                var letterHeight = 15;
                if (rowNode.nodeValue !=null) {
                    Point offset = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);

                    if (rowNode.colType==ColTyp.bracket) {
                        rtn.Width=10;
                    }
                    if (rowNode.nodeValue.Trim() != ",") {
                        rowNode.rowLen+=5;
                    }

                    int stackWidth, x, y, length;

                    // Calculate x
                    stackWidth = rowNode.parent.rows.Max(n => n.rowLen);
                    x = offset.X + rtn.xIncrement + stackWidth/2 - rowNode.rowLen/2;
                    y = offset.Y + (rowNode.index * (letterHeight + 10))-10;
                    if (rtn.uniformSize) {
                        length = rowNode.parent.parent.colsMaxRowlen + 5;
                    } else {
                        length = rowNode.rowLen;
                    }

                    // Calculate y
                    if (rowNode.parent.rows.Count==1) {
                        // Single row.
                        // Centre vertically wrt sibling columns' rows.
                        // Parent column's parent row has what I need - See #NOTE G0914
                        y = (offset.Y + rowNode.parent.parent.colsVerticalCentre) - 5;
                    } else {
                        y = offset.Y + rtn.yIncrement + rowNode.rowHeight;
                    }

                    rowNode.rowRect = new Rectangle(x, y, length, letterHeight);
                    rtn.yIncrement += rowNode.rowHeight + 10;

                } else {
                    // # NOTE G0914
                    // Non-leaf rows are column containers.
                    // I only have columns. Store their collective vertical centre. Use my rowRect.
                    int stackHeight = rowNode.columns.Max(c=>c.rows.Max(n => n.index)* (letterHeight + 10));
                    rowNode.rowRect = new Rectangle(rowNode.rowRect.X, rowNode.rowRect.Y, rowNode.rowRect.Width, stackHeight);
                    // REMEMBER: Valueless/non-leaf row now has a rowRect..
                }
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
                rtn.xIncrement=rtn.Width;
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
            public SizeArgsRtns NewLayoutPreColumnOp(TNode columnNode, SizeArgsRtns rtn) {
                rtn.Height += 5 * columnNode.innerRootCount;
                rtn.Width += 5 * columnNode.innerRootCount;
                rtn.yIncrement = 0; // Reset on each new column
                return rtn;
            }
            public SizeArgsRtns NewLayoutPostColumnOp(TNode ac, SizeArgsRtns rtn) {
                // Update the cumulative x from my stack of rows
                rtn.xIncrement += ac.rows.Max(r => r.rowLen);
                return rtn;
            }
            //public SizeArgsRtns LayoutPostColumnOp(TNode ac, SizeArgsRtns rtn) {
            //    // Update the cumulative x from my stack of rows
            //    rtn.xIncrement += ac.rows.Max(r => r.rowLen);
            //    return rtn;
            //}


        }
        public class DrawFactory<TNode, TRtns> : IDrawFactory where TNode : INode<TNode>,
                                                             IRenderNode<TNode>,
                                                             new()
                                               where TRtns : IWalkerArgsRtns,
                                                             new() {
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
                        drawWalker = (ITreeWalker<TNode, TRtns>)new OriginalTreeWalker<TNode, DrawArgsRtns>();
                        break;
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
                        //I'm going to have to count roots.

                        //drawWalker.RowOp = NewDrawProcessRows;
                        drawWalker.PreRowOp=NewDrawProcessRows;
                        //drawWalker.PostRowOp=;
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
                if ((answerColumn.colType & (ColTyp.fraction))>0) {

                    // Draw_ProcessIndividualFractionStyledExpressionColumns

                    // Brackets
                    if ((answerColumn.colType & (ColTyp.bracket))>0) {
                        var minX = answerColumn.rows.Min(row => row.rowRect.X);
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
            public DrawArgsRtns NewDrawProcessRows(TNode rowNode, DrawArgsRtns rtn) {
                // Now a single row operation
                if (rowNode.nodeValue!=null) {
                    var letterHeight = 15;
                    var font = new Font("Arial", 10);

                    //if (answerColumn.colType==ColTyp.real) { }    // sorted with fractions
                    //if (answerColumn.colType==ColTyp.integer) { } // sorted with fractions
                    if (rowNode.colType==ColTyp.product) { }   // draw prodct
                    if (rowNode.colType==ColTyp.sigma) { }     // draw sigma
                    if (rowNode.colType==ColTyp.integral) { }  // draw integral
                    if (rowNode.colType==ColTyp.complex) { }   // draw complex
                    if (rowNode.colType==ColTyp.factorial) { } // draw factorial
                    if (rowNode.colType==ColTyp.choose) { }    // draw choose
                    if ((rowNode.colType & (ColTyp.fraction))>0) {

                        // Prelim leaf (row) brackets
                        //if ((rowNode.colType & (ColTyp.bracket))>0) {
                        //    var minX = rowNode.rows.Min(row => row.rowRect.X);
                        //    var minY = rowNode.rows.Min(row => row.rowRect.Y);
                        //    var height = rtn.Height;
                        //    rtn.ansBackBuffer.DrawLine(divisionPen,
                        //        new Point(rowNode.rowRect.X-8, rowNode.rowRect.Y),
                        //        new Point(rowNode.rowRect.X-8, rowNode.rowRect.Y + height));

                        //    rtn.ansBackBuffer.DrawLine(divisionPen,
                        //        new Point(rowNode.rowRect.X+rtn.Width-8, rowNode.rowRect.Y),
                        //        new Point(rowNode.rowRect.X+rtn.Width-8, rowNode.rowRect.Y + height));
                        //}

                        // Actual node value
                        rtn.ansBackBuffer.DrawString(rowNode.nodeValue, font, Brushes.Blue, 
                            new Rectangle(rowNode.rowRect.X, rowNode.rowRect.Y, rowNode.rowLen, letterHeight));

                        // Block over the top
                        //rowNode.charCount=0;
                        //rowNode.toBlock.Clear();

                        //if (!rowNode.answered && !rtn.ShowAnswers)
                        //    rowNode.toBlock.Add(new CharacterRange(0, rowNode.nodeValue.Length));

                        //rowNode.blockRegions = drawstuff.GetRegionArray(rowNode.nodeValue, font, new Rectangle(0, 0, 500, 500), rowNode.toBlock.ToArray(), rtn.ansBackBuffer);
                        //foreach (var reg in rowNode.blockRegions) {
                        //    rowNode.boundsRect = Rectangle.Round(reg.GetBounds(rtn.ansBackBuffer));
                        //    rowNode.blockRect = new Rectangle(rowNode.rowRect.X + rowNode.boundsRect.X,
                        //                                      rowNode.rowRect.Y + rowNode.boundsRect.Y,
                        //                                      rowNode.boundsRect.Width,
                        //                                      rowNode.boundsRect.Height);
                        //    rtn.ansBackBuffer.FillRectangle(Brushes.Orange, rowNode.blockRect);
                        //}

                        // division line
                        if (rowNode.showDiv) {
                            int xStart = rowNode.parent.rows.Min(r => r.rowRect.X);
                            int xEnd = xStart + rowNode.parent.rows.Max(r => r.rowLen)-5;
                            int midY = (rowNode.rowRect.Y + rowNode.rowRect.Height) +5;

                            rtn.ansBackBuffer.DrawLine(divisionPen,
                                new Point(xStart, midY),
                                new Point(xEnd, midY));
                        }
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
                        keysWalker = (ITreeWalker<TNode, TRtns>)new WalkNodeInEverythingRow<TNode, KeysArgsRtns>();
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
    public enum Genus : int {
        OriginalClient = 1,
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
