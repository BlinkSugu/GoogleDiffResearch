namespace GoogleDiffResearch
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                string xml1 = @"d:\TSG\AiKira\Research\XML_Compare\Testing_Samples\FromRavi\522174_1_En_JobSheet_50.xml";
                string xml2 = @"d:\TSG\AiKira\Research\XML_Compare\Testing_Samples\FromRavi\522174_1_En_1_JobSheet_200.xml";
                string outPath = @"d:\TSG\AiKira\Research\XML_Compare\Testing_Samples\FromRavi";
                string clnt = "Springer";

                Speedtest speedtest = new Speedtest();

                speedtest.DiffMatch(xml1, xml2, outPath, clnt);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
