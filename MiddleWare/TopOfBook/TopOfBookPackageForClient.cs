namespace Analyzer.MiddleWare.TopOfBook
{
    public class TopOfBookPackageForClient
    {
        public TopOfBookResponse[] topOfBookResponses { get; set; }
        public string packageType { get; set; }

        public TopOfBookPackageForClient(TopOfBookResponse[] topOfBookResponsesIn, string packageTypeIn)
        {
            topOfBookResponses = topOfBookResponsesIn;
            packageType = packageTypeIn;
        }
    }
}
