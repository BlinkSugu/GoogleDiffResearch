using DiffMatchPatch;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace GoogleDiffResearch
{
    /// <summary>
    /// This class contains raws functions to compare couple of text format files or xml files
    /// </summary>
    public class DiffCompare
    {
        /// <summary>
        /// This function will compare the text files and provide the resulted output.
        /// If you want to compare XML files, then both input XML files should be parser error-free.
        /// </summary>
        /// <param name="req">This is a content model for raising a request on DiffCompare should include the previous version file, the latest version file, and a type such as 'Text' or 'XML'.</param>
        /// <returns></returns>
        public static GoogleCompareResponse Compare(GoogleCompareRequest req)
        {
            GoogleCompareResponse response = new GoogleCompareResponse();
            response.isServiceSuccessfull = true;
            try
            {
                string text1 = req.OldVersionFile;
                string text2 = req.NewVersionFile;
                InputType fileType = req.FileType;

                if (File.Exists(text1))
                {
                    text1 = File.ReadAllText(text1);
                }
                if (File.Exists(text2))
                {
                    text2 = File.ReadAllText(text2);
                }

                if (fileType == InputType.XML)
                {
                    text1 = FormatXml(text1);
                    text2 = FormatXml(text2);
                    text1 = FormatTab(text1);
                    text2 = FormatTab(text2);
                }

                diff_match_patch dmp = new();
                dmp.Diff_Timeout = 0;

                // Execute one reverse diff as a warmup.
                dmp.diff_main(text2, text1);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                List<Diff> listD = new();
                listD = dmp.diff_main(text1, text2);
                dmp.diff_cleanupSemanticLossless(listD);
                dmp.diff_cleanupSemantic(listD);
                dmp.diff_cleanupEfficiency(listD);

                //Returns the HTML output here
                string html = dmp.diff_prettyHtml(listD);

                response.Result = true;
                response.ResultDescription = "Comparison between both the files executed successfully!";
                response.OutHtml = html;

                return response;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.ResultDescription = "Unknown error occurred please check with aiKira team!";
                response.OutHtml = string.Empty;
                Console.WriteLine(ex);
                return response;
            }
        }
        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (XmlException)
            {
                throw; // or any other error handling mechanism
            }
        }
        private static string FormatTab(string xml)
        {
            try
            {
                bool IsMatch = true;
                while (IsMatch)
                {
                    if (xml.Contains('\t') || Regex.IsMatch(xml, "  "))
                    {
                        xml = Regex.Replace(xml, @"\t", "");
                        xml = Regex.Replace(xml, "(  +)", " ");
                    }
                    else
                    {
                        IsMatch = false;
                    }
                }
                IsMatch = true;
                while (IsMatch)
                {
                    if (Regex.IsMatch(xml, "\r\n |\n |\r ") || Regex.IsMatch(xml, " \r\n| \n| \r"))
                    {
                        xml = Regex.Replace(xml, "\r\n |\n |\r ", "\n");
                        xml = Regex.Replace(xml, " \r\n| \n| \r", "\n");
                    }
                    else
                    {
                        IsMatch = false;
                    }
                }
                xml = Regex.Replace(xml, "\r\n|\n|\r", "");
                xml = Regex.Replace(xml, "><", ">\n<");

                return xml;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
