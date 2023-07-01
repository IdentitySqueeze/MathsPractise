using System;
using System.Collections.Generic;

namespace Fangorn {

    // Thoughts: One concrete Walker with attached methods can support multiple simultaneous traversals on 
    // entirely different, completely isolated data structures.
    // Because the data + rtns are passed in, and run themselves on themselves, unknown to each other.
    // The methods used are the same, but that's all.
    // Sequential version of simultaneous.
    // One WalkerReturns per traversal. Separate WalkerReturns + data = separate traversal, separate universe.
    //
    // Thoguhts continued:
    //  Above, each Traversal is called from within another Traversal.
    // As Traverse is nested already, a new from-the-top Traverse is only called because it's necessary for some reason.
    // Normal, nested Traverse is always called on columns
    //
    // The reason is that once Row processing you can't 'jump' to column processing.
    // But if you start a new traverse you can.
    //
    // MeasureRow op calls traverse on finding exponents
    // MeasurePostColumn op too.
    // Always when exponents found.
    //
    // MeasureRow        - looping rows, switch to columns        rows.g_column.columns
    // MeasurePostColumn - single row,   switch to columns        row.columns            (no grouping column)
    // SizeProcessRows   - looping rows, switch to columns        rows.g_column.columns
    // SizePostColumn    - single row,   switch to rows ?    *    g_row                  (      ? ? ?       )
    // DrawProcessRows   - looping rows, switch to columns        rows.g_column.columns
    // DrawPostColumn    - single row,   switch to columns        row.columns            (no grouping column)
    //
    // *  Might be a bug in here, might not.
    //
    // Even more thoughts:
    //  Grouping columns are empty columns used to store exponent group width.
    // For some reason, bracket exponents are stored in rows not columns.
    // Exponents are added to an expression's .columns
    // Exponents are added to a bracket's .rows
    // Columns almost mean inside, e.g., inside the brackets; rows almost mean outside, e.g., exponents. When talking bracket-atively.
    //
    // So far, Pre & Post column ops are defined only for brackets etc, never for mathematical expressions.
    //

}
