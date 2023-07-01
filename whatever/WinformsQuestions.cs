using System.Drawing;
using Polish;
using Polish.OLevel.Numbers;

namespace DustBlowerClient {
    // -- Winforms decorators --
    public abstract class wfQuestion : Question {
        wfQuestion(int id, qParameters qParams, Graphics g) : base(id, qParams) { }
    }

    [NaturalName("Arithmetic Expression")]
    public class wfBasicArithmetic : qBasicArithmetic    {
        public wfBasicArithmetic(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Basic Add/Subtract")]
    public class wfBasicAddSubtract: qBasicAddSubtract   {
        public wfBasicAddSubtract(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Basic Multiply/Divide")]
    public class wfBasicMultiplyDivide: qBasicMultiplyDivide    {
        public wfBasicMultiplyDivide(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("To Standard Form")]
    public class wfStandardFormX : qStandardFormX {
        public wfStandardFormX(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("From Standard Form")]
    public class wfStandardFormSF : qStandardFormSF {
        public wfStandardFormSF(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Mixed Standard Form")]
    public class wfStandardFormMixed  : qStandardFormMixed {
        public wfStandardFormMixed(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Standard Form Arithmetic")]
    public class wfStandardFormArithmetic : qStandardFormArithmetic{
        public wfStandardFormArithmetic(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Factors")]
    public class wfFactors : qFactors    {
        public wfFactors(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Prime Factor Products")]
    public class wfPrimeFactorProducts : qPrimeFactorProducts    {
        public wfPrimeFactorProducts(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Highest Common Factor")]
    public class wfHighestCommonFactor : qHighestCommonFactor    {
        public wfHighestCommonFactor(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Lowest Common Multiple")]
    public class wfLowestCommonMultiple : qLowestCommonMultiple    {
        public wfLowestCommonMultiple(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Mean")]
    public class wfAveragesMean : qAveragesMean{
        public wfAveragesMean(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Mean How Many")]
    public class wfAveragesMeanHowMany : qAveragesMeanHowMany{
        public wfAveragesMeanHowMany(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Mean Compose")]
    public class wfAveragesMeanCompose : qAveragesMeanCompose{
        public wfAveragesMeanCompose(int id, qParameters qParams) : base(id, qParams) { }
    }

    //TODO: n-subsets
    [NaturalName("Mean Subset")]
    public class wfAveragesMeanSubset : qAveragesMeanSubset{
        public wfAveragesMeanSubset(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Mean Subtract")]
    public class wfAveragesMeanSubtract : qAveragesMeanSubtract{
        public wfAveragesMeanSubtract(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Multiply Up")]
    public class wfFractionMultiplyUp : qFractionMultiplyUp    {
        public wfFractionMultiplyUp(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Divide Down")]
    public class wfFractionDivideDown : qFractionDivideDown    {
        public wfFractionDivideDown(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Reduce")]
    public class wfFractionReduce : qFractionReduce    {
        public wfFractionReduce(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("To Mixed")]
    public class wfFractionToMixed : qFractionToMixed    {
        public wfFractionToMixed(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Top Heavy")]
    public class wfFractionTopHeavy : qFractionTopHeavy    {
        public wfFractionTopHeavy(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Lowest Common Denominator")]
    public class wfFractionLowestCommonDenominator : qFractionLowestCommonDenominator    {
        public wfFractionLowestCommonDenominator(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Fraction of")]
    public class wfFractionOf: qFractionOf{
        public wfFractionOf(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Cancel")]
    public class wfFractionCancel : qFractionCancel    {
        public wfFractionCancel(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Addition")]
    public class wfFractionAddition : qFractionAddition    {
        public wfFractionAddition(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Subtraction")]
    public class wfFractionSubtract : qFractionSubtract    {
        public wfFractionSubtract(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Multiplication")]
    public class wfFractionMultiply : qFractionMultiply    {
        public wfFractionMultiply(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Division")]
    public class wfFractionDivision : qFractionDivision    {
        public wfFractionDivision(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Decimal Places")]
    public class wfDecimalPlaces : qDecimalPlaces    {
        public wfDecimalPlaces(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Significant Figures")]
    public class wfSignificantFigures : qSignificantFigures    {
        public wfSignificantFigures(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Arithmetic Expression")]
    public class wfFractionBasicArithmetic : qFractionBasicArithmetic    {
        public wfFractionBasicArithmetic(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Addition")]
    public class wfDecimalAdd : qDecimalAdd    {
        public wfDecimalAdd(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Subtraction")]
    public class wfDecimalSubtract : qDecimalSubtract    {
        public wfDecimalSubtract(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Multiplicatin")]
    public class wfDecimalMultiply : qDecimalMultiply    {
        public wfDecimalMultiply(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Division")]
    public class wfDecimalDivide : qDecimalDivide    {
        public wfDecimalDivide(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Long Multiplication")]
    public class wfDecimalLongMultiply : qDecimalLongMultiply    {
        public wfDecimalLongMultiply(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Long Division")]
    public class wfDecimalLongDivide : qDecimalLongDivide    {
        public wfDecimalLongDivide(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Fraction to Decimal")]
    public class wfFraction2Decimal : qFraction2Decimal    {
        public wfFraction2Decimal(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Decimal to Fraction")]
    public class wfDecimal2Fraction : qDecimal2Fraction    {
        public wfDecimal2Fraction(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Ration to Fraction")]
    public class wfRatio2Fraction : qRatio2Fraction    {
        public wfRatio2Fraction(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Fraction to Ratio")]
    public class wfFraction2Ratio : qFraction2Ratio    {
        public wfFraction2Ratio(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Ratio Money")]
    public class wfRatioPencePound : qRatioPencePound    {
        public wfRatioPencePound(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Ratio Mixed")]
    public class wfRatioMixed : qRatioMixed{
        public wfRatioMixed(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Ratio Find")]
    public class wfRatioFind : qRatioFind{
        public wfRatioFind(int id, qParameters qParams) : base(id, qParams) { }

    }
    [NaturalName("Ratio Distribute")]
    public class wfRatioDistribute : qRatioDistribute{
        public wfRatioDistribute(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Ratio Divide")]
    public class wfRatioDivide : qRatioDivide{
        public wfRatioDivide(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Ratio - Decimals")]
    public class wfRatioDecimals : qRatioDecimals {
        public wfRatioDecimals(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Ratio - Fractions")]
    public class wfRatioFractions : qRatioFractions {
        public wfRatioFractions(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Proportion Cost")]
    public class wfProportionBuysCosts : qProportionBuysCosts{

        public wfProportionBuysCosts(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Proportion Amounts")]
    public class wfProportionThingMoves : qProportionThingMoves{
        public wfProportionThingMoves(int id, qParameters qParams) : base(id, qParams) { }
    }

    //[NaturalName("Proportion Recipe")]
    public class wfProportionRecipe : qProportionRecipe{
        public wfProportionRecipe(int id, qParameters qParams) : base(id, qParams) { }
    }

    
    [NaturalName("Fraction to Percent")]
    public class wfFraction2Percent : qFraction2Percent{
        public wfFraction2Percent(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent to Fraction")]
    public class wfPercent2Fraction : qPercent2Fraction{
        public wfPercent2Fraction(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Decimal to Percent")]
    public class wfDecimal2Percent : qDecimal2Percent{
        public wfDecimal2Percent(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent to Decimal")]
    public class wfPercent2Decimal : qPercent2Decimal{
        public wfPercent2Decimal(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Result")]
    public class wfPercentNoResult : qPercentNoResult{
        public wfPercentNoResult(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Number")]
    public class wfPercentNoNumber : qPercentNoNumber{
        public wfPercentNoNumber(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Percentage")]
    public class wfPercentNoPercentage : qPercentNoPercentage{
        public wfPercentNoPercentage(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName(@"Percent P&L")]
    public class wfPercentProfitLoss : qPercentProfitLoss{
        public wfPercentProfitLoss(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Discount")]
    public class wfPercentDiscount : qPercentDiscount{
        public wfPercentDiscount(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Change Cost")]
    public class wfPercentChangeNoCost : qPercentChangeNoCost{
        public wfPercentChangeNoCost(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Percent Change Sale")]
    public class wfPercentChangeNoSell : qPercentChangeNoSell{
        public wfPercentChangeNoSell(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName(@"Percent Change P&L")]
    public class wfPercentChangeNoPL : qPercentChangeNoPL{
        public wfPercentChangeNoPL(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName(@"Simple Interest")]
    public class wfInterest : qInterest {
        public wfInterest(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName(@"Compounding")]
    public class wfCompounding : qCompounding {
        public wfCompounding(int id, qParameters qParams) : base(id, qParams) { }
    }



    [NaturalName("x², x³")] 
    public class wfPowersSquaresCubes : qPowersSquaresCubes{
        public wfPowersSquaresCubes (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("xⁿ")] 
    public class wfPowersNthPowers : qPowersNthPowers{
        public wfPowersNthPowers (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("xᵐ + xⁿ")] 
    public class wfPowersPlus : qPowersPlus{
        public wfPowersPlus (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("xᵐ - xⁿ")] 
    public class wfPowersMinus : qPowersMinus{
        public wfPowersMinus (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("xᵐ * xⁿ")] 
    public class wfPowersMultiply : qPowersMultiply{
        public wfPowersMultiply (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("xᵐ / xⁿ")] 
    public class wfPowersDivide : qPowersDivide{
        public wfPowersDivide (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("x⁻ʸ")] 
    public class wfPowersNegative: qPowersNegative{
        public wfPowersNegative (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Fraction ˣ")] 
    public class wfPowersFractionˣ : qPowersFractionˣ{
        public wfPowersFractionˣ (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("x ᶠʳᵃᶜᵗⁱᵒⁿ")] 
    public class wfPowersXᶠʳᵃᶜᵗⁱᵒⁿ : qPowersXᶠʳᵃᶜᵗⁱᵒⁿ{
        public wfPowersXᶠʳᵃᶜᵗⁱᵒⁿ (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("(xˣ)ʸ")] 
    public class wfPowersBrackets:qPowersBrackets{
        public wfPowersBrackets (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("ⁿ√x")] 
    public class wfPowersⁿᵗʰRoot:qPowersⁿᵗʰRoot{
        public wfPowersⁿᵗʰRoot (int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("√x, ³√x")] 
    public class wfRootsSquaresCubes :qRootsSquaresCubes{
        public wfRootsSquaresCubes (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("ⁿ√x")] 
    public class wfRootsNthRoots :qRootsNthRoots{
        public wfRootsNthRoots (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("√xʸ")] 
    public class wfRootsRootXʸ :qRootsRootXʸ{
        public wfRootsRootXʸ (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("ⁿ√xʸ")] 
    public class wfRootsⁿᵗʰRootXʸ :qRootsⁿᵗʰRootXʸ{
        public wfRootsⁿᵗʰRootXʸ (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("√x/y")] 
    public class wfRootsOfFraction :qRootsOfFraction{
        public wfRootsOfFraction (int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("√xy")] 
    public class wfRootsProduct:qRootsProduct{
        public wfRootsProduct (int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Simplify Surd √x")] 
    public class wfSurdsSimplify:qSurdsSimplify{
        public wfSurdsSimplify(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Multiply Surd")] 
    public class wfSurdsMultiply:qSurdsMultiply{
        public wfSurdsMultiply(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Fractional surds")]
    public class wfSurdsFractional : qSurdsFractional {
        public wfSurdsFractional(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Expand & simplify 1")]
    public class wfSurdsExpandSimplify1 : qSurdsExpandSimplify1 {
        public wfSurdsExpandSimplify1(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Expand & simplify 2")]
    public class wfSurdsExpandSimplify2 : qSurdsExpandSimplify2 {
        public wfSurdsExpandSimplify2(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Expand & simplify 3")]
    public class wfSurdsExpandSimplify3 : qSurdsExpandSimplify3 {
        public wfSurdsExpandSimplify3(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Expand & simplify 4")]
    public class wfSurdsExpandSimplify4 : qSurdsExpandSimplify4 {
        public wfSurdsExpandSimplify4(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Surds brackets")]
    public class wfSurdsBracketSquared : qSurdsBracketSquared{
        public wfSurdsBracketSquared(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Surd diff 2 squares")]
    public class wfSurdsDiff2Squares : qSurdsDiff2Squares{
        public wfSurdsDiff2Squares(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Surds rationalise denom")]
    public class wfSurdsRationaliseDenom : qSurdsRationaliseDenom{
        public wfSurdsRationaliseDenom(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Convert to Log")] 
    public class wfLogsIndex2Log:qLogsIndex2Log{
        public wfLogsIndex2Log(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Convert to Index")] 
    public class wfLogsLog2Index:qLogsLog2Index{
        public wfLogsLog2Index(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Log addition")]
    public class wfLogsAddition : qLogsAddition {
        public wfLogsAddition(int id, qParameters qParams) : base(id, qParams) { }
    }
    
    [NaturalName("Log subtraction")]
    public class wfLogsSubtraction : qLogsSubtraction {
        public wfLogsSubtraction(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Evaluate")] 
    public class wfLogsEvaluate:qLogsEvaluate{
        public wfLogsEvaluate(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Express as Terms")] 
    public class wfLogsExpressTerms:qLogsExpressTerms{
        public wfLogsExpressTerms(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("Simplify")] 
    public class wfLogsSimplify:qLogsSimplify{
        public wfLogsSimplify(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("AP: Find the nth term 1")]
    public class wfSeqAPFindnthTerm1 : qSeqAPFindnthTerm1 {
        public wfSeqAPFindnthTerm1(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Find the nth term 2")]
    public class wfSeqAPFindnthTerm2 : qSeqAPFindnthTerm2 {
        public wfSeqAPFindnthTerm2(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("AP: Find nth, mth terms")]
    public class wfSeqAPFindnmthTerms : qSeqAPFindnmthTerms {
        public wfSeqAPFindnmthTerms(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Find 3rd term")]
    public class wfSeqAPFind3rdTerm : qSeqAPFind3rdTerm {
        public wfSeqAPFind3rdTerm(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Find which term")]
    public class wfSeqAPWhichTerm : qSeqAPWhichTerm {
        public wfSeqAPWhichTerm(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Arithmetic means")]
    public class wfSeqAPArithmeticMeans : qSeqAPArithmeticMeans {
        public wfSeqAPArithmeticMeans(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("AP: Sum 1")]
    public class wfSeqAPSum1 : qSeqAPSum1 {
        public wfSeqAPSum1(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Sum 2")]
    public class wfSeqAPSum2 : qSeqAPSum2 {
        public wfSeqAPSum2(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Sum 3")]
    public class wfSeqAPSum3 : qSeqAPSum3 {
        public wfSeqAPSum3(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("AP: Sum range")]
    public class wfSeqAPSumRange : qSeqAPSumRange {
        public wfSeqAPSumRange(int id, qParameters qParams) : base(id, qParams) { }
    }



    [NaturalName("HP: nth term")]
    public class wfSeqHPFindnthTerm : qSeqHPFindnthTerm {
        public wfSeqHPFindnthTerm(int id, qParameters qParams) : base(id, qParams) { }
    }




    [NaturalName("GP: nth term")]
    public class wfSeqGPFindnthTerm1 : qSeqGPFindnthTerm1 {
        public wfSeqGPFindnthTerm1(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("GP: nth, mth terms")]
    public class wfSeqGPFindnmthTerms : qSeqGPFindnmthTerms {
        public wfSeqGPFindnmthTerms(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("GP: Find 3rd term 1")]
    public class wfSeqGPFind3rdTerm1 : qSeqGPFind3rdTerm1 {
        public wfSeqGPFind3rdTerm1(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("GP: Find 3rd term 2")]
    public class wfSeqGPFind3rdTerm2 : qSeqGPFind3rdTerm2 {
        public wfSeqGPFind3rdTerm2(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("GP: Arithmetic means")]
    public class wfSeqGPGeometicMeans : qSeqGPGeometicMeans {
        public wfSeqGPGeometicMeans(int id, qParameters qParams) : base(id, qParams) { }
    }
    [NaturalName("GP: Sum 1")]
    public class wfSeqGPSum1 : qSeqGPSum1 {
        public wfSeqGPSum1(int id, qParameters qParams) : base(id, qParams) { }
    }




    [NaturalName("Sigma")]
    public class wfSigma1: qSigma1{
        public wfSigma1(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Test")]
    public class wfTestQuestion : qTestQuestion {
        public wfTestQuestion(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Test new")]
    public class wfTestQuestionNew : qTestQuestionNew {
        public wfTestQuestionNew(int id, qParameters qParams) : base(id, qParams) { }
    }


    [NaturalName("Traingles")]
    public class wfTriangleTest : qTriangleTest{
        public wfTriangleTest(int id, qParameters qParams) : base(id, qParams) { }
    }

    [NaturalName("Algebra test")]
    public class wfAlgebraTest : qAlgebraTest {
        public wfAlgebraTest(int id, qParameters qParams) : base(id, qParams) { }
    }

}