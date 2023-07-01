using Polish;
namespace DustBlowerClient {
    // -- Winforms decorators --

    [NaturalName("Basic Arithmetic")]
    public class wfBasicArithmeticFactory : qBasicArithmeticFactory {}

    [NaturalName("Factors")]
    public class wfFactorsFactory : qFactorsFactory {}

    [NaturalName("Fractions")]
    public class wfFractionsFactory : qFractionsFactory {}

    [NaturalName("Decimals")]
    public class wfDecimalsFactory : qDecimalsFactory {}

    [NaturalName("Ratio/Proportion")]
    public class wfRatioProportionFactory : qRatioProportionFactory {}
    
    [NaturalName("Percentages")]
    public class wfPercentagesFactory : qPercentagesFactory {}

    //[NaturalName("Averages")]
    //public class wfAveragesFactory : qAveragesFactory {}

    [NaturalName("Powers")]
    public class wfPowersFactory : qPowersFactory { }

    [NaturalName("Roots & Surds")]
    public class wfRootsSurds : qRootsSurds{ }

    [NaturalName("Logs")]
    public class wfLogs: qLogs{ }

    [NaturalName("Sequences")]
    public class wfSequences : qSequences { }

    [NaturalName("Test")]
    public class wfTest:qTest{ }


    [NaturalName("Test new")]
    public class wfTestNew : qTestNew { }

    //[NaturalName("Pictures")]
    //public class wfPictures:qPictures{ }

    //[NaturalName("Algebra")]
    //public class wfAlgebraFactory: qAlgebraFactory { }

}
