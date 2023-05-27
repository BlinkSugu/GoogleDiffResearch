// Copyright 2010 Google Inc.
// All Right Reserved.

using DiffMatchPatch;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;

namespace GoogleDiffResearch
{
    public enum OpType
    {
        INSERT,
        DELETE
    }
    public class ReportModel
    {
        public string sourceValue { get; set; } = "";
        public string destValue { get; set; } = "";
        public string resultValue { get; set; } = "";
        public OpType Type { get; set; }
    }
    public class Speedtest
    {
        public static int addLineCntr = 0;
        public static void Main(string[] args)
        {
            try
            {
                string xml1 = "d:\\TSG\\AiKira\\XML_Compare\\Testing_Samples\\Set17\\105786_4_En_JobSheet_5.xml";
                string xml2 = "d:\\TSG\\AiKira\\XML_Compare\\Testing_Samples\\Set17\\105786_4_En_JobSheet_50.xml";

                string text1 = File.ReadAllText(xml1);
                string text2 = File.ReadAllText(xml2);

                List<Diff> listD = new List<Diff>();

                diff_match_patch dmp = new diff_match_patch();
                dmp.Diff_Timeout = 0;

                text1 = FormatXml(text1);
                text2 = FormatXml(text2);

                text1 = FormatTab(text1);
                text2 = FormatTab(text2);

                File.WriteAllText("Speedtest1_up.xml", text1);
                File.WriteAllText("Speedtest2_up.xml", text2);

                // Execute one reverse diff as a warmup.
                dmp.diff_main(text2, text1);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                DateTime ms_start = DateTime.Now;
                listD = dmp.diff_main(text1, text2);
                dmp.diff_cleanupSemanticLossless(listD);
                dmp.diff_cleanupSemantic(listD);
                dmp.diff_cleanupEfficiency(listD);

                string html = dmp.diff_prettyHtml(listD);

                Speedtest sptest = new Speedtest();

                string HtmlRpt = sptest.CleanedOutHtml(html, text2, text1);

                string outputxml = Path.GetFileNameWithoutExtension(xml1);
                outputxml += ".html";
                File.WriteAllText(outputxml, HtmlRpt);

                DateTime ms_end = DateTime.Now;

                Console.WriteLine("Elapsed time: " + (ms_end - ms_start));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
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
        private string CleanedOutHtml(string htmlCont, string xmlSource, string xmlDest)
        {
            try
            {
                string diffHtml = htmlCont;
                List<ReportModel> report;
                string[] prismAssets = { "<link rel=\"stylesheet\" href=\"prism.css\" data-noprefix />", "<script src=\"prism.js\"></script>" };
                string tableRow = "";
                string tableBody = "";
                string cssStyle = "<style>*{box-sizing: border-box;}p{margin: 0.5em 0em;}div.meta{margin-left: 2em;margin-bottom: 2em;font-family: sans-serif;}p.notation{float: right;margin-right: 2.5em;}table{width: 96%;margin: 0em auto 5em auto;border-spacing: 0px;border: 0px;box-shadow: 20px 40px 30px grey;}th{font-family: sans-serif;padding: 4px;background-color: rgb(57, 173, 57);color: #FFFFFF;position: sticky;top: 0;width: 33%;}td{vertical-align:top;padding: 4px 3px; background-color: #2d2d2d;}th:nth-of-type(1){border-top-left-radius: 15px;width: 33%;}th:nth-of-type(2){border-right: 3px solid #bdc6bd;border-left: 3px solid #bdc6bd;}th:nth-of-type(3){border-top-right-radius: 15px;}tbody tr:last-child td{border-bottom: 4px solid rgb(57, 173, 57);}tbody tr:first-child td{border-top: 0px solid #bdc6bd;}tbody tr td{border-top: 3px solid #bdc6bd;}tbody tr td:nth-of-type(2){border-right: 3px solid #bdc6bd;border-left: 3px solid #bdc6bd;}tbody tr:hover td, tr:hover pre{background-color: #1d1d1d;}span.diff{background: #AAFFAA;color: red;font-weight: bold;}del{background-color: red !important;color: white !important;font-weight: bold !important;text-decoration: none;}ins{background-color: rgb(5, 152, 5) !important;color: white !important;font-weight: bold !important;text-decoration: none;}span.ins{display: inline-block; background-color: rgb(5, 152, 5); width: 13px; height: 13px; border-radius: 15px; transition: 0.4s background-color;}span.del{display: inline-block; background-color: red; width: 13px; height: 13px; border-radius: 15px;}span.mod span.ins{position: relative; left: -6px;}button.btnIns{border: 2px solid rgb(5, 152, 5); border-radius: 7px; padding: 6px 4px 3px 4px; background-color: white; font-weight: bold; transition: 0.4s border, 0.4s background-color, 0.4s color;}button.btnDel{border: 2px solid red; border-radius: 5px; padding: 6px 4px 3px 4px; background-color: white; font-weight: bold; transition: 0.4s border, 0.4s background-color, 0.4s color;}button.btnMod{border-top: 2px solid red; border-right: 2px solid red; border-bottom: 2px solid rgb(5, 152, 5); border-left: 2px solid rgb(5, 152, 5); border-radius: 5px; padding: 6px 4px 3px 4px; background-color: white; font-weight: bold; transition: 0.4s border, 0.4s background-color, 0.4s color;}button.btnIns:hover{cursor:pointer;border: 2px solid rgb(58, 16, 225); background-color: rgb(19, 184, 19); color: white;}button.btnDel:hover{cursor:pointer;border: 2px solid rgb(58, 16, 225); background-color: rgb(233, 74, 74); color: white;}button.btnIns:hover span.ins{background-color: white;}button.btnDel:hover span.del{background-color: white;}button.btnMod:hover{cursor:pointer;background-color:#EEE;}table.xmlTable{display:none;} pre, code{word-break: break-all !important;}table.xmlTable tr td{width: 50%;}table.xmlTable tr td:first-child{padding-left:10px;} p.rptView{float:left;}.xmlTable{display:none;}.summary {clear: both;}.summary table td,.summary table th {background-color: white;width: 50%;border: 2px solid black !important;}.summary table {border-collapse: collapse;box-shadow: none;width: 20%;margin: 0.5em 0 0.5em 0;}.summary table tr:hover td {background-color: #AAFFAA;}th.thdRpt {color: #1d1d1d;}span#errTxt{color: red;display: none;}</style>";

                diffHtml = HtmlCleanup(diffHtml);
                
                string SbySide = SidebySide(diffHtml);

                report = HtmlReport(diffHtml);
                Dictionary<string, int> summaryRpt = new Dictionary<string, int>() {
                    { "Modification", 0},
                    { "Deletion", 0},
                    { "Insertion", 0}
                };
                if (report.Count > 0)
                {
                    foreach (var line in report)
                    {
                        if (line.resultValue.Contains("<del") & line.resultValue.Contains("<ins"))
                        {
                            tableRow += $"<tr class=\"mod-row\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Modification"] += 1;
                        }
                        else if (line.resultValue.Contains("<del"))
                        {
                            tableRow += $"<tr class=\"del-row\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Deletion"] += 1;
                        }
                        else
                        {
                            tableRow += $"<tr class=\"ins-row\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Insertion"] += 1;
                        }
                    }
                    tableBody = $"<table id=\"resultTable\"><thead><tr><th>PREVIOUS VERSION XML</th><th>NEW VERSION XML</th><th>COMPARISON RESULT</th></tr></thead><tbody>{tableRow}</tbody></table>";
                }
                else
                {

                    tableRow += $"<tr class=\"idn-head-row\"><td colspan=\"2\" style=\"text-align: center\"><span style=\"color: white; font-size: 120%;\"><b>Both the XML files are identical!</b></span></td></tr>";
                    tableRow += $"<tr class=\"idn-row\"><td><script type=\"text/plain\" class=\"language-xml\">{xmlSource}</script></td><td><script type=\"text/plain\" class=\"language-xml\">{xmlDest}</script></td></tr>";
                    tableBody = $"<table id=\"resultTable\"><thead><tr><th>PREVIOUS VERSION XML</th><th>NEW VERSION XML</th></tr></thead><tbody>{tableRow}</tbody></table>";
                }

                string htmltemplate = $"<html><head><title>Metadata Compare</title>{prismAssets[0]}{cssStyle}</head><body><div class=\"meta\"><h2>Metadata Compare</h2><p><b>Client:</b> Springer</p><p><b>Comparison:</b> Old XML vs New XML</p><p><b>Date Time:</b> {DateTime.Now}</p><div class=\"summary\"><table><thead><tr><th colspan=\"2\" class=\"thdRpt\">Report Summary</th></tr></thead><tbody><tr><td>Insertions:</td><td>{summaryRpt["Insertion"]}</td></tr><tr><td>Deletions:</td><td>{summaryRpt["Deletion"]}</td></tr><tr><td>Modifications:</td><td>{summaryRpt["Modification"]}</td></tr><tr><td>Total difference:</td><td>{report.Count}</td></tr></tbody></table></div><p class=\"rptView\"><b>REPORT VIEW:</b> <input type=\"radio\" name=\"View\" onchange=\"ShowView('Side')\"><label>Side-by-Side View</label> <input type=\"radio\" name=\"View\" onchange=\"ShowView('Smart')\" checked><label>Smart View</label><span id=\"notation1\">&#x2003; Quick Search: <input type=\"text\" id=\"srchTxt\" placeholder=\"Search...\"/> <span id=\"errTxt\">Match not found!</span></span></p><p class=\"notation\" id=\"notation\"><b>NOTATION & FILTER:</b> <button id=\"btnIns\" class=\"btnIns\"><span class=\"ins\"></span> INSERTION</button> <button id=\"btnDel\" class=\"btnDel\"><span class=\"del\"></span> DELETION</button> <button id=\"btnMod\" class=\"btnMod\"><span class=\"mod\"><span class=\"del\"></span><span class=\"ins\"></span></span> MODIFICATION</button> <button id=\"btnClr\" class=\"btnClr\">CLEAR</button></p></div>{tableBody}{SbySide}{prismAssets[1]}<script>function filterRows(className) {{ var table = document.getElementById(\"resultTable\"); var rows = table.getElementsByTagName(\"tr\"); for (var i = 1; i < rows.length; i++) {{ var row = rows[i]; if (className != \"clr\") {{ if (row.classList.contains(className)) {{ row.style.display = \"\"; }} else {{ row.style.display = \"none\"; }} }} else {{ row.style.display = \"\"; }} }} }} document.getElementById(\"btnIns\").addEventListener(\"click\", function () {{ filterRows(\"ins-row\"); }}); document.getElementById(\"btnDel\").addEventListener(\"click\", function () {{ filterRows(\"del-row\"); }}); document.getElementById(\"btnMod\").addEventListener(\"click\", function () {{ filterRows(\"mod-row\"); }}); document.getElementById(\"btnClr\").addEventListener(\"click\", function () {{ filterRows(\"clr\"); }}); function ShowView(View) {{ if (View == \"Side\") {{ document.getElementById(\"resultTable\").style.display = \"none\"; document.getElementById(\"xmlTable\").style.display = \"table\"; document.getElementById(\"notation\").style.display = \"none\"; document.getElementById(\"notation1\").style.display = \"none\";}} else {{ document.getElementById(\"xmlTable\").style.display = \"none\"; document.getElementById(\"resultTable\").style.display = \"\"; document.getElementById(\"notation\").style.display = \"block\"; document.getElementById(\"notation1\").style.display = \"inline\";}} }}const searchInput = document.getElementById('srchTxt');const dataTable = document.getElementById('resultTable');searchInput.addEventListener('input', function() {{filterRows(\"clr\");const searchText = searchInput.value.toLowerCase();const rows = dataTable.getElementsByTagName('tr');let count = 0;for (let i = 1; i < rows.length; i++) {{const oldxml = rows[i].getElementsByTagName('td')[0].innerText.toLowerCase();const newxml = rows[i].getElementsByTagName('td')[1].innerText.toLowerCase();if (oldxml.includes(searchText) || newxml.includes(searchText)) {{rows[i].style.display = '';count++;}} else {{rows[i].style.display = 'none';}}if(count == 0){{document.getElementById(\"errTxt\").style.display = \"inline\";}}else{{document.getElementById(\"errTxt\").style.display = \"none\";}}}}}});</script></body></html>";
                return htmltemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static string HtmlCleanup(string rcdHtml)
        {
            try
            {
                string clnHtml = rcdHtml;
                clnHtml = clnHtml.Replace("&para;", "");
                clnHtml = Regex.Replace(clnHtml, "<br>(</(?:ins|del)>)", "$1<br>");
                clnHtml = Regex.Replace(clnHtml, "<br></span>", "</span><br>");
                clnHtml = Regex.Replace(clnHtml, "<br>([^><]+)</span>(<(?:del|ins))", "</span><br><span>$1</span>$2");
                clnHtml = Regex.Replace(clnHtml, "<[/]?span>", "");
                return clnHtml;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static List<ReportModel> HtmlReport(string html)
        {
            try
            {
                List<ReportModel> tmpReport = new();
                string[] lines = html.Split("<br>");
                int i = 0;
                while (i < lines.Length)
                {
                    ReportModel tmpRModel = new();
                    int[] lnRange = new int[2];
                    int[] lnDiffRange = new int[2];

                    if (lines[i].Contains("<del") || lines[i].Contains("<ins"))
                    {
                        if (Regex.IsMatch(lines[i], @"</(del|ins)>([^<>]+)?$"))
                        {
                            lnRange = LineRangeSelection(i, lines);
                            tmpReport.Add(CleanSelection(tmpRModel, lines, lnRange));
                            i = lnRange[1];
                            continue;
                        }
                        else
                        {
                            lnDiffRange[0] = i;
                            lnDiffRange[1] = LineSelectionSuffixDiff(i + 1, lines);
                            lnRange = LineRangeSelectionMulti(lnDiffRange, lines);
                            tmpReport.Add(CleanSelection(tmpRModel, lines, lnRange));
                            i = lnRange[1];
                            continue;
                        }
                    }
                    i++;
                }
                return tmpReport;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static int[] LineRangeSelection(int i, string[] lines)
        {
            int[] lineRange = new int[2];

            if(i == 0)
            {
                lineRange[0] = 0;
                lineRange[1] = LineSelectionSuffix(i + 1, lines);
            }
            else if (i == lines.Length)
            {
                lineRange[0] = LineSelectionPrefix(i - 1, lines);
                lineRange[1] = i;
            }
            else
            {
                lineRange[0] = LineSelectionPrefix(i - 1, lines);
                lineRange[1] = LineSelectionSuffix(i + 1, lines);
            }           

            return lineRange;
        }
        private static int[] LineRangeSelectionMulti(int[] i, string[] lines)
        {
            int[] lineRange = new int[2];
            if (i[0] == 0)
            {
                lineRange[0] = 0;
                lineRange[1] = LineSelectionSuffix(i[1] + 1, lines);
            }
            else if (i[1] == lines.Length)
            {
                lineRange[0] = LineSelectionPrefix(i[0] - 1, lines);
                lineRange[1] = i[1];
            }
            else
            {
                lineRange[0] = LineSelectionPrefix(i[0] - 1, lines);
                lineRange[1] = LineSelectionSuffix(i[1] + 1, lines);
            }
            return lineRange;
        }
        private static int LineSelectionSuffixDiff(int i, string[] lines)
        {
            if (i < lines.Length)
            {
                if (Regex.IsMatch(lines[i], @"</(del|ins)>([^<>]+)?$"))
                {
                    return i - 1;
                }
                else
                {
                    return LineSelectionSuffixDiff(i + 1, lines);
                }
            }
            else
            {
                return i;
            }
        }
        private static int LineSelectionSuffix(int i, string[] lines, int? counter = 0)
        {
            if (counter < 2 && i < lines.Length)
            {
                if (lines[i].Contains("<del") || lines[i].Contains("<ins") || lines[i].Contains("</del") || lines[i].Contains("</ins"))
                {
                    int tmpLnend = 0;
                    addLineCntr = 0;
                    if (Regex.IsMatch(lines[i], @"<(del|ins)([^<>]+)>([^<>]+)?$"))
                    {
                        tmpLnend = LineSelectionSuffixDiff(i + 1, lines);
                        return LineSelectionSuffix(tmpLnend + 1, lines, addLineCntr);
                    }
                    else
                    {
                        return LineSelectionSuffix(i + 1, lines, addLineCntr);
                    }
                    
                }
                else
                {
                    addLineCntr += 1;
                    return LineSelectionSuffix(i + 1, lines, addLineCntr);
                }
            }
            else
            {
                addLineCntr = 0;
                return i-1;
            }            
        }
        private static int LineSelectionPrefix(int i, string[] lines, int? counter = 0)
        {
            if (counter < 1 && i >= 0)
            {
                if (lines[i].Contains("<del") || lines[i].Contains("<ins") || lines[i].Contains("</del") || lines[i].Contains("</ins"))
                {
                    addLineCntr = 0;
                    return LineSelectionPrefix(i - 1, lines, addLineCntr);
                }
                else
                {
                    addLineCntr += 1;
                    return LineSelectionPrefix(i - 1, lines, addLineCntr);
                }
            }
            else
            {
                addLineCntr = 0;
                if (i < 0)
                {
                    return 0;
                }
                else
                {
                    return i;
                }
            }
        }
        private static ReportModel CleanSelection(ReportModel rpt, string[] str, int[] lnRnge)
        {
            for (var i = lnRnge[0]; i <= lnRnge[1]; i++)
            {
                rpt.sourceValue += str[i] + "&templt;br/&tempgt;";
                rpt.destValue += str[i] + "&templt;br/&tempgt;";
                rpt.resultValue += str[i] + "<br/>";
            }
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "<ins([^><]+)>([^><]+)</ins>", "");
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "<del([^><]+)>([^><]+)</del>", "<span class=\"diff\">$2</span>", RegexOptions.Multiline);
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "(\r\n|\n|\r)", "");
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "&templt;br/&tempgt;", "<br/>");

            rpt.destValue = Regex.Replace(rpt.destValue, "<del([^><]+)>([^><]+)</del>", "");
            rpt.destValue = Regex.Replace(rpt.destValue, "<ins([^><]+)>([^><]+)</ins>", "<span class=\"diff\">$2</span>");
            rpt.destValue = Regex.Replace(rpt.destValue, "(\r\n|\n|\r)", "");
            rpt.destValue = Regex.Replace(rpt.destValue, "&templt;br/&tempgt;", "<br/>");

            rpt.resultValue = Regex.Replace(rpt.resultValue, "(\r\n|\n|\r)", "");

            return rpt;
        }
        private string SidebySide(string rawHtml)
        {
            string tableRow = "";
            string tableBody;
            string prevXml = Regex.Replace(rawHtml, @"<br>", "&tmplt;br&tmpgt;");
            var matchs = Regex.Matches(prevXml, @"<ins([^><]+)>([^><]+)</ins>");
            const string pattern = "&tmplt;br&tmpgt;";
            foreach (Match match in matchs)
            {
                string breakStr = "";
                int count = Regex.Matches(match.Groups[2].Value, pattern, RegexOptions.IgnoreCase).Count;
                if (count > 0)
                {
                    breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn = new Regex("<ins([^><]+)>([^><]+)</ins>");
                prevXml = ptrn.Replace(prevXml, breakStr, 1);
            }
            prevXml = Regex.Replace(prevXml, @"&tmplt;br&tmpgt;", "<br/>");

            string destXml = Regex.Replace(rawHtml, @"<br>", "&tmplt;br&tmpgt;");
            matchs = Regex.Matches(destXml, @"<del([^><]+)>([^><]+)</del>");
            foreach (Match match in matchs)
            {
                string breakStr = "";
                int count = Regex.Matches(match.Groups[2].Value, pattern, RegexOptions.IgnoreCase).Count;
                if (count > 0)
                {
                    breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn = new Regex("<del([^><]+)>([^><]+)</del>");
                destXml = ptrn.Replace(destXml, breakStr, 1);
            }
            destXml = Regex.Replace(destXml, @"&tmplt;br&tmpgt;", "<br/>");


            tableRow += $"<tr class=\"idn-row\"><td><pre><code class=\"language-xml\">{prevXml}</code><pre></td><td><pre><code class=\"language-xml\">{destXml}</code></pre></td></tr>";
            tableBody = $"<table id=\"xmlTable\" class=\"xmlTable\"><thead><tr><th>PREVIOUS VERSION XML</th><th>NEW VERSION XML</th></tr></thead><tbody>{tableRow}</tbody></table>";

            return tableBody;
        }

    }
}
