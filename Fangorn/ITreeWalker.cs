using System;
using System.Collections.Generic;

namespace Fangorn {
    public interface ITreeWalker<TNode, TRtn>
        where TNode : INode<TNode>
        where TRtn : IWalkerArgsRtns {
        Func<TNode, TRtn, TRtn> PostColumnOp { get; set; }
        Func<List<TNode>, TRtn, TRtn> PostTraversalOp { get; set; }
        Func<TNode, TRtn, TRtn> PreColumnOp { get; set; }
        Func<TNode, TRtn, TRtn> PreRowOp { get; set; }
        Func<TNode, TRtn, TRtn> PostRowOp { get; set; }
        Func<TNode, TRtn, TRtn> PreTraversalOp { get; set; }
        Func<TNode, TRtn, TRtn> RowOp { get; set; }
        TRtn Traverse(Genus genus, IPossibleAnswer<TNode> possibleAnswer, TRtn rtn);
        TRtn Traverse(List<TNode> arcs, TRtn rtn, int depth = 0);
        TRtn Traverse(TNode arc, TRtn rtn, int depth = 0);
    }
}