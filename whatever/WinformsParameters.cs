using System;
using System.Collections.Generic;
using Polish;

namespace DustBlowerClient {
    public class wfParametersFactory
    {
        string setting {get;set; }="Basic";
        Dictionary<Type,Func<qParameters>> switchBasic {get; set; }
        public wfParametersFactory(){ 
            string ops = "/*-+";
            int[,] numbers = { {2,10} };
            bool brackets = true;
            int terms = 3;
            bool fractions = false;
            bool decimals = false;
            bool integers = true;
            bool negatives=true;
            int limit = 200;
            bool includeFailure = true;
            bool unique = true;
            bool mixed = true;
            int mantissa = 1;
            int characteristic = 1;
            bool brevity = true;
            int decimalPoints =2;
            int numerator=2;
            int denominator=2;

            switchBasic = new Dictionary<Type,Func<qParameters>>{ 
            #region Basic arithmetic
              { typeof(wfBasicArithmetic), ()=>new qParameters("*/-+", new int[,]{ {2,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},  
              { typeof(wfBasicAddSubtract), ()=>new qParameters("+-", new int[,]{ {2,100},{2,100} }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},  
              //{ typeof(wfBasicMultiplyDivide), ()=>new qParameters("*/", new int[,]{ {2,10 },{2,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},  
              { typeof(wfBasicMultiplyDivide), ()=>new qParameters("*/", new int[,]{ {2,10 },{11,99 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},  
              { typeof(wfLowestCommonMultiple), ()=>new qParameters(ops, new int[,]{ { 2,9} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalPlaces), ()=>new qParameters(ops, new int[,]{ { 1,10} }, brackets, 3, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfSignificantFigures), ()=>new qParameters(ops, new int[,]{ { 1,10} }, brackets, terms, fractions, true, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, 3, numerator, denominator)},
              { typeof(wfStandardFormX), ()=>new qParameters(ops, numbers, brackets, terms, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfStandardFormSF), ()=>new qParameters(ops, numbers, brackets, terms, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfStandardFormMixed), ()=>new qParameters("*/", numbers, brackets, terms, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfStandardFormArithmetic), ()=>new qParameters("*/-+", numbers, brackets, terms, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfAveragesMean), ()=>new qParameters(ops, numbers, brackets, 5, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfAveragesMeanHowMany), ()=>new qParameters(ops, numbers, brackets, 5, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfAveragesMeanCompose), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfAveragesMeanSubset), ()=>new qParameters(ops, new int[,]{{1,10}}, brackets, 10, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfAveragesMeanSubtract), ()=>new qParameters(ops, numbers, brackets, 5, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            #endregion
            #region Factors
              { typeof(wfFactors), ()=>new qParameters(ops, new int[,]{ {2,5} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfPrimeFactorProducts), ()=>new qParameters(ops, new int[,]{ {2,5} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfHighestCommonFactor), ()=>new qParameters(ops, new int[,]{ {2,5} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
            #endregion
            #region fractions
              { typeof(wfFractionMultiplyUp), ()=>new qParameters(ops, new int[,]{ {1,10} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionDivideDown), ()=>new qParameters(ops, new int[,]{ {2,10} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionReduce), ()=>new qParameters(ops, new int[,]{ {1,10} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionToMixed), ()=>new qParameters(ops, new int[,]{ {1,10} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionTopHeavy), ()=>new qParameters(ops, new int[,]{ {1,10} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionLowestCommonDenominator), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionOf), ()=>new qParameters(ops, new int[,]{ { 1, 25 }, {2,50} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionCancel), ()=>new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionAddition), ()=>new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionSubtract), ()=>new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionMultiply), ()=>new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionDivision), ()=>new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFractionBasicArithmetic), ()=>new qParameters("/-+*", new int[,]{ {1,15 } }, brackets, 6, fractions, decimals, integers, true, limit, includeFailure, false, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
            #endregion
            #region decimals
              { typeof(wfDecimalAdd), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalSubtract), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalMultiply), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalDivide), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalLongMultiply), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimalLongDivide), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfFraction2Decimal), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfDecimal2Fraction), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
            #endregion
            #region ratioProportion
              { typeof(wfFraction2Ratio), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatio2Fraction), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioPencePound), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioMixed), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioFind), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioDistribute), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioDivide), ()=>new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfProportionBuysCosts), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfProportionThingMoves), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioDecimals), ()=>new qParameters(ops, new int[,]{ {2,9} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
              { typeof(wfRatioFractions), ()=>new qParameters(ops, new int[,]{ {2,9},{2,20} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator)},
            #endregion
            #region percentages
              { typeof(wfFraction2Percent), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercent2Fraction), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfDecimal2Percent), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercent2Decimal), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentNoResult), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentNoNumber), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentNoPercentage), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentProfitLoss), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentDiscount), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentChangeNoCost), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentChangeNoSell), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfPercentChangeNoPL), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfInterest), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
              { typeof(wfCompounding), ()=>new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator)},
            #endregion
            #region powers
              { typeof(wfPowersSquaresCubes), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersNthPowers), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersPlus),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersMinus),        ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersMultiply),  ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersDivide),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersNegative),     ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersFractionˣ), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersXᶠʳᵃᶜᵗⁱᵒⁿ),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersⁿᵗʰRoot),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfPowersBrackets), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            #endregion      
            #region roots & surds
              { typeof(wfRootsSquaresCubes), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfRootsNthRoots), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfRootsRootXʸ), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfRootsⁿᵗʰRootXʸ), ()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfRootsOfFraction),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfRootsProduct),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsSimplify),()=>new qParameters(ops, new int[,]{ { 2,20 } }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsMultiply),()=>new qParameters("-+", new int[,]{ { 2,20 } }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsFractional),()=>new qParameters("-+", new int[,]{ { 2,20 } }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsExpandSimplify1),()=>new qParameters("-+", numbers, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsExpandSimplify2),()=>new qParameters("-+", numbers, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsExpandSimplify3),()=>new qParameters("-+", numbers, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsExpandSimplify4),()=>new qParameters("-+", numbers, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsBracketSquared),()=>new qParameters("-+", numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsDiff2Squares),()=>new qParameters("-+", numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSurdsRationaliseDenom),()=>new qParameters(ops, new int[,]{ { 2,20 } }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            #endregion      
            #region logs
              { typeof(wfLogsIndex2Log),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsLog2Index),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsAddition),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsSubtraction),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsEvaluate),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsExpressTerms),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfLogsSimplify),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            #endregion
            #region sequences              
              { typeof(wfSigma1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPFindnthTerm1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPFindnthTerm2),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPFindnmthTerms),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPFind3rdTerm),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPWhichTerm),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPArithmeticMeans),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPSum1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPSum2),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPSum3),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqAPSumRange),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqHPFindnthTerm),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPFindnthTerm1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPFindnmthTerms),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPFind3rdTerm1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPFind3rdTerm2),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPGeometicMeans),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
              { typeof(wfSeqGPSum1),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},

              #endregion
            { typeof(wfTestQuestion),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            { typeof(wfTestQuestionNew),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},


            #region pictures
            { typeof(wfTriangleTest),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            #endregion
            { typeof(wfAlgebraTest),()=>new qParameters(ops, numbers, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator)},
            };
        }


        public qParameters getParams(Type qt, string qSettings="Basic") { 
            qParameters rtn=new qParameters();
            switch(setting){
                case "Basic":
                    rtn=switchBasic[qt]();
                    break;
                case "Intermediate":
                    break;
                case "Hard":
                    break;
                default:
                    break;
            }
            return rtn;
        }
    }
}
