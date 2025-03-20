// Copyright 2010 Google Inc.
// All Right Reserved

using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GoogleDiffResearch
{
    public class ReportModel
    {
        public string sourceValue { get; set; } = "";
        public string destValue { get; set; } = "";
        public string resultValue { get; set; } = "";
    }

    /// <summary>
    /// This class contains the functions to compare couple of XML files.
    /// </summary>
    public class Speedtest
    {
        public static int addLineCntr = 0;
        public static string Client = "", OldXML = "", NewXML = "";
        public static List<string> trRow = new List<string>();

        /// <summary>
        /// This function will take to 2 xml files as input and provide the html as output in given output path.
        /// </summary>
        /// <param name="xmlPrev">This parameter represents the previous version of XML</param>
        /// <param name="xmlNew">This parameter represents the new version of XML</param>
        /// <param name="outpath">This parameter represents the output path for the HTML Report</param>
        /// <param name="clnt">This parameter represents the client name to specify in HTML Report</param>
        /// <returns>Return the boolean value respective to the process success or failure</returns>
        public bool DiffMatch(string xmlPrev, string xmlNew, string outpath, string clnt)
        {
            try
            {
                GoogleCompareRequest req = new GoogleCompareRequest();
                Client = clnt;

                req.OldVersionFile = xmlPrev;
                req.NewVersionFile = xmlNew;
                req.FileType = InputType.XML;

                GoogleCompareResponse res = DiffCompare.Compare(req);

                if (res.isServiceSuccessfull == true && res.Result == true)
                {
                    string HtmlRpt = CleanedOutHtml(res.OutHtml);
                    if (Regex.IsMatch(outpath, @"\\$"))
                    {
                        File.WriteAllText(outpath + "CompareReport.html", HtmlRpt);
                    }
                    else
                    {
                        File.WriteAllText(outpath + "\\CompareReport.html", HtmlRpt);
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        private static string CleanedOutHtml(string htmlCont)
        {
            try
            {
                string diffHtml = htmlCont;
                diffHtml = Regex.Replace(diffHtml, "<ins style=\"background:#e6ffe6;\"></ins>", "");
                diffHtml = Regex.Replace(diffHtml, "<del style=\"background:#ffe6e6;\"></del>", "");

                List<ReportModel> report;
                List<string> prismAssets = new() { "<link rel=\"stylesheet\" href=\"prism.css\" data-noprefix />", "<script src=\"prism.js\"></script>" };
                string tableRow = "";
                string tableBody = "";
                string cssCommon = "<style>body {background: #F6F7FB;height: 100%;}.bg-blue {background-color: #185ABD;}::-webkit-scrollbar {width: 6px;height: 6px;cursor: default;/* background-color: rgba(0,0,0,.1);  #c8c6c4;*/}::-webkit-scrollbar-thumb {background-color: #8c8b8a;border-radius: 8px;}.navbar-light .navbar-brand {color: #fff;font-size: 14px;display: flex;vertical-align: middle;align-items: center;}/*XML Comparsion*/.filterxml .btn-group {border: 1px solid #185ABD;background-color: #185ABD;border-radius: 4px;}.filterxml .btn-group .dropdown-menu li label {padding-left: 10px;font-weight: 400 !important;font-size: 12px;}.filterxml .btn-group .dropdown-menu li a {color: #333;font-weight: 500 !important;}.filterxml .btn-group span.multiselect-selected-text {color: #fff;font-size: 12px;}.filterxml .btn-group .dropdown-toggle::after {border-top-color: #fff;position: absolute;top: 13px;right: 10px;}.filterxml .btn-group button.multiselect.dropdown-toggle {padding: 2px;text-align: left;padding-left: 27px;position: relative;top: -1px;}.xmlHeadrColr {background-color: #fff;padding: 9px 0;margin: 0;}/***** RADIO BUTTON STYLES *****/.rdio {position: relative;display: flex;vertical-align: middle;align-items: center;margin-right: 22px;}.rdio input[type=radio] {opacity: 0;}.rdio label {padding-left: 6px;cursor: pointer;margin-bottom: 0;font-size: 12px;}.rdio label:before {width: 15px;height: 15px;position: absolute;top: 7px;left: 0;content: \"\";display: inline-block;-moz-border-radius: 50px;-webkit-border-radius: 50px;border-radius: 50px;border: 1px solid #000000;background: #fff;}.rdio input[type=radio] {margin: 0px;}.rdio input[type=radio]:disabled+label {color: #999;}.rdio input[type=radio]:disabled+label:before {background-color: #000000;}pre,code {word-break: break-all !important;}.rdio input[type=radio]:checked+label::after {content: \"\";position: absolute;top: 11px;left: 4px;display: inline-block;font-size: 11px;width: 7px;height: 7px;background-color: #000000;-moz-border-radius: 50px;-webkit-border-radius: 50px;border-radius: 50px;}.rdio-default input[type=radio]:checked+label:before {border-color: #000000;}.rdio-primary input[type=radio]:checked+label:before {border-color: #000000;}.rdio-primary input[type=radio]:checked+label::after {background-color: #185abd;}.filterxml {display: flex;position: relative;height: 30px;}.radiobuttons {display: flex;margin-left: 7px;}.radiobuttons .rdio:first-child:after {content: '';background-color: #333333;position: absolute;width: 1px;height: 14px;right: -12px;}.rightXmlCom {font-size: 12px;margin-bottom: 0;padding-top: 7px;}.xmlHeaderComp h5 {background-color: #d1def2;font-size: 12px;padding: 10px 8px;}.xmlbodyComp {overflow-y: auto;height: calc(100vh - 180px);background-color: #fff;}.filterxml .btn-group .dropdown-menu li label input[type=\"checkbox\"] {position: relative;top: 2px;}.xmlCompareTable thead th {background-color: #d1def2;font-size: 13px;padding: 10px 8px;font-weight: 500;border: 0;z-index: 9;}.xmlCompareTable tbody td {position: relative;background-color: #ffffff;font-size: 14px;border: 0;padding: 0;}.xmlCompareTable .table {border-collapse: separate;border-spacing:26px 0px;margin-bottom: 0px;}.sideBySide thead th {width: 50%;}.filterSvgIcon {position: absolute;z-index: 999;left: 7px;top: 4px;}.filterSvgIcon svg {width: 20px;height: 20px;}.radiobuttons .searchoptions .input-group {display: flex;width: 150px;margin: 0px 11px;}.iconsLis {margin-bottom: 0;padding-left: 0;padding-top: 3px;}.iconsLis li {float: left;width: calc(50% - 10px);margin-right: 10px;position: relative;display: inline-block;cursor: pointer;}.radiobuttons .searchoptions .input-group input.form-control {height: 30px;font-size: 12px;box-shadow: none;}.radiobuttons .searchoptions svg.searchSvg.whiteSvg {width: 14px;position: relative;}.radiobuttons .searchoptions .input-group .input-group-append .input-group-text {background-color: #185abd;height: 30px;}.iconsLis li:first-child:after {content: '';background-color: #c9c9c9;position: absolute;width: 1px;height: 20px;right: -3px;top: 2px;}.xmlCompareTable {height: calc(100vh - 150px);}.xmlCompareTable thead th {position: sticky;top: 0;}.footerParts {position: fixed;bottom: 0;width: 100%;left: 0;display: flex;justify-content: space-between;font-size: 12px;background-color: #f2f1f1;padding: 6px;vertical-align: middle;margin: 0;}.iconsLis {display: none;}.footerParts p {margin-bottom: 0;position: absolute;top: 0;right: 0;}.footerParts h6 {font-size: 12px;margin-bottom: 0;text-align: center;}.footerParts .grn {color: green;}.footerParts .dels {color: #f00;}.footerParts .mod {color: #185abd;}div.hideDiv div.hideButton.collapsed::before {content: \"+\";}div.hideDiv div.hideButton::before {content: '-';}.hideDiv {position: relative;}.lnNum {position: absolute;left: 7px;font-size: 12px;}.sideBySide pre.language-xml {padding: 0px 10px 0px 45px;outline:none;margin: 0;}.hideButton {display: inline-block;position: absolute;left: -40px;color: black;background-color: white;padding: 0px 8px;font-size: 18px;font-weight: 800;margin-top: -3px;cursor: pointer;}del{text-decoration-color: #333;}.smartViews.xmlCompareTable thead th {width: 33%;}span.diff { background-color: #c4e0c4; }span.lnBtn {position: absolute;top: 3px;left: -20px;border: 1px solid #969696;width: 13px;text-align: center;height: 13px;line-height: 11px;font-weight: 700;font-size: 12px;color: #000;cursor:pointer;} .grayFilter {filter: grayscale(1);font-weight: bold;}.grayFilter:after {content: \"...\";}</style>";
                string cssPrism = "<style>.command-line-prompt,.line-numbers .line-numbers-rows{border-right:1px solid #999;font-size:100%;letter-spacing:-1px;pointer-events:none}.token.treeview-part .entry-line,.token.treeview-part .entry-name{position:relative;vertical-align:top;display:inline-block}code[class*=language-],pre[class*=language-]{color: #333;background:0 0;font-family:Consolas,Monaco,'Andale Mono','Ubuntu Mono',monospace;font-size:1em;text-align:left;white-space:pre-wrap;word-spacing:normal;word-break:normal;word-wrap:normal;line-height:1.5;-moz-tab-size:4;-o-tab-size:4;tab-size:4;-webkit-hyphens:none;-moz-hyphens:none;-ms-hyphens:none;hyphens:none}pre[class*=language-]{padding:1em;margin:.5em 0;overflow:auto}:not(pre)>code[class*=language-],pre[class*=language-]{background: #ffffff;}:not(pre)>code[class*=language-]{padding:.1em;border-radius:.3em;white-space:normal}.token.block-comment,.token.cdata,.token.comment,.token.doctype,.token.prolog{color: #333}.token.punctuation{color: #333}.token.attr-name,.token.deleted,.token.namespace,.token.tag{color: #d12b2f}.token.function-name{color:#6196cc}.token.boolean,.token.function,.token.number{color:#f08d49}.token.class-name,.token.constant,.token.property,.token.symbol{color:#f8c555}.token.atrule,.token.builtin,.token.important,.token.keyword,.token.selector{color:#cc99cd}.token.attr-value,.token.char,.token.regex,.token.string,.token.variable{color: green}.token.entity,.token.operator,.token.url{color:#333}.token.bold,.token.important{font-weight:700}.token.italic{font-style:italic}.token.entity{cursor:help}.token.inserted{color:green}pre[class*=language-].line-numbers{position:relative;padding-left:3.8em;counter-reset:linenumber}pre[class*=language-].line-numbers>code{position:relative;white-space:inherit}.line-numbers .line-numbers-rows{position:absolute;top:0;left:-3.8em;width:3em;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.line-numbers-rows>span{display:block;counter-increment:linenumber}.line-numbers-rows>span:before{content:counter(linenumber);color:#999;display:block;padding-right:.8em;text-align:right}.command-line-prompt{display:block;float:left;margin-right:1em;text-align:right;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.command-line-prompt>span:before{opacity:.7;content:' ';display:block;padding-right:.8em}.command-line-prompt>span[data-user]:before{content:\"[\" attr(data-user) \"@\" attr(data-host) \"] $\"}.command-line-prompt>span[data-user=root]:before{content:\"[\" attr(data-user) \"@\" attr(data-host) \"] #\"}.command-line-prompt>span[data-prompt]:before{content:attr(data-prompt)}.command-line-prompt>span[data-continuation-prompt]:before{content:attr(data-continuation-prompt)}.command-line span.token.output{opacity:.7}[class*=lang-] script[type='text/plain'],[class*=language-] script[type='text/plain'],script[type='text/plain'][class*=lang-],script[type='text/plain'][class*=language-]{display:block;font:100% Consolas,Monaco,monospace;white-space:pre;overflow:auto}.token.punctuation.brace-hover,.token.punctuation.brace-selected{outline:solid 1px}.rainbow-braces .token.punctuation.brace-level-1,.rainbow-braces .token.punctuation.brace-level-5,.rainbow-braces .token.punctuation.brace-level-9{color:#e50;opacity:1}.rainbow-braces .token.punctuation.brace-level-10,.rainbow-braces .token.punctuation.brace-level-2,.rainbow-braces .token.punctuation.brace-level-6{color:#0b3;opacity:1}.rainbow-braces .token.punctuation.brace-level-11,.rainbow-braces .token.punctuation.brace-level-3,.rainbow-braces .token.punctuation.brace-level-7{color:#26f;opacity:1}.rainbow-braces .token.punctuation.brace-level-12,.rainbow-braces .token.punctuation.brace-level-4,.rainbow-braces .token.punctuation.brace-level-8{color:#e0e;opacity:1}.token.treeview-part .entry-line{text-indent:-99em;width:1.2em}.token.treeview-part .entry-line:before,.token.treeview-part .line-h:after{content:\"\";position:absolute;top:0;left:50%;width:50%;height:100%}.token.treeview-part .line-h:before,.token.treeview-part .line-v:before{border-left:1px solid #ccc}.token.treeview-part .line-v-last:before{height:50%;border-left:1px solid #ccc;border-bottom:1px solid #ccc}.token.treeview-part .line-h:after{height:50%;border-bottom:1px solid #ccc}.token.treeview-part .entry-name.dotfile{opacity:.5}@font-face{font-family:PrismTreeview;src:url(\"data:application/font-woff;base64,d09GRgABAAAAAAgYAAsAAAAAEGAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAABHU1VCAAABCAAAADsAAABUIIslek9TLzIAAAFEAAAAPwAAAFY1UkH9Y21hcAAAAYQAAAB/AAACCtvO7yxnbHlmAAACBAAAA+MAAAlACm1VqmhlYWQAAAXoAAAAKgAAADZfxj5jaGhlYQAABhQAAAAYAAAAJAFbAMFobXR4AAAGLAAAAA4AAAA0CGQAAGxvY2EAAAY8AAAAHAAAABwM9A9CbWF4cAAABlgAAAAfAAAAIAEgAHZuYW1lAAAGeAAAATcAAAJSfUrk+HBvc3QAAAewAAAAZgAAAIka0DSfeJxjYGRgYOBiMGCwY2BycfMJYeDLSSzJY5BiYGGAAJA8MpsxJzM9kYEDxgPKsYBpDiBmg4gCACY7BUgAeJxjYGRYyjiBgZWBgaGQoRZISkLpUAYOBj0GBiYGVmYGrCAgzTWFweEV4ysehs1ArgDDFgZGIA3CDAB2tQjAAHic7ZHLEcMwCESfLCz/VEoKSEE5parURxMOC4c0Ec283WGFdABgBXrwCAzam4bOK9KWeefM3Hhmjyn3ed+hTRq1pS7Ra/HjYGPniHcXMy4G/zNTP7/KW5HTXArkvdBW3ArN19dCG/NRIN8K5HuB/CiQn4U26VeBfBbML9NEH78AeJyVVc1u20YQ3pn905JcSgr/YsuSDTEg3cR1bFEkYyS1HQcQ2jQF2hot6vYSoECKnnPLA/SWUy9NTr31Bfp+6azsNI0SGiolzu7ODnfn+2Z2lnHG3rxhr9nfLGKbLGesncAYYnUHpsVnMG/uwyzNdFIVd6HI6twp8+R3LpT4TSglLoTHwwJgG2/dFvKrl9yI507/p5CCq4LTxB/PlPjkFaMHnWB/0S9je7RTPS+utnGtom1T2q5pk/e3H0M1S18rsXAL7wgpxQuhAmteGGvNjmcfGXuwnFNOPCXxeOGmnjrBLWNyBeNtVq2Hs03yus1aPS3mzSyNVSfu588iW1Q93x/4fjcHn+5EkS2tMxr4xIRa8ese+4L9uKZnxEqs8+ldyN9atU02a5t5uQ8hZGms1QTKpaKYqnipiNNOAIeIADC0JNEOYY+jtSgFoOchiAjRGFACpUTRje8bwIYWGCDEgENY8MEu9bnCYCdAxftoNg0KiSpUtPaHcanYwzXRu6T4r40b5npal3V7UHWCPJW9niyl1vIHgoujEXZjudBkeWkOeMQBRmbEPhKzij1i52t6/TadL+3q7H0U1eq4E8cG4gIIwQLx8VX7ToPXgPrehVc5QXHR7gMSmwjKfaYAP4KvZV+yn9bE18y2IY37LvtyrSg3i7ZK++B603ndlg/gBJpZRsfpBI6hyiaQ6FjlnThz8lAC3LgBIMnXDOAXxBQ4SIgiEhx2AcGCAwAhwjXRpCQms42bwAUt75BvAwgONzdgOfWEwzk4Ylzj4mz+5YEzzXzWX9aNlk7ot65y5QnBHsNlm6zDTu7sspRqG4V+fgJ1lVBZ07Nm7s5nemo3Lf3PO7iwtnroQ5/YDGwPRUip6fV6L+27p+wCHwSvPs85UnHqId8NAn5IBsKdv95KrL9m31Gsf2a/rluDslk1y1J9GE+LUmmVT/OyOHaFKGnapt2H5XeJTmKd6qYNoVVZOy+pWzr7rMip3ndG/4mQSoUcMbAqG/YNIAdXhkAqTVruXhocSKN0iS4Rwj7vSS4fcF/La07BfeQSuRAcFeW+9igjwPhhYPpGCBCBHhxiKMyFMFT7ziRH7RtfIWdiha+TdW+Rqs7bLHdN2ZJIKl0um0x3op9saYr0REeRdj09pl43pMzz4tjztrY8L4o8bzT+oLY27PR/eFtXs/YY5vtwB5Iqad14eYN0ujveMaGWqkdU3TKbQSC5Uvxaf4fA7SAQ3r2tEfIhd4duld91bwMisjqBw22orthNcroXl7KqO1329HBgAexgoCfGAwiDPoBnriki3lmNojrzvD0tjo6E3vPYP6E2BMIAeJxjYGRgYADiY8t3FsTz23xl4GbYzIAB/v9nWM6wBcjgYGAC8QH+QQhZAAB4nGNgZGBg2MzAACeXMzAyoAJeADPyAh14nGNgAILNpGEA0fgIZQAAAAAAAAA2AHIAvgE+AZgCCAKMAv4DlgPsBEYEoHicY2BkYGDgZchi4GQAASYg5gJCBob/YD4DABTSAZcAeJx9kU1uwjAQhV/4qwpqhdSqi67cTTeVEmBXDgBbhBD7AHYISuLUMSD2PUdP0HNwjp6i676k3qQS9Ujjb968mYUNoI8zPJTHw02Vy9PAFatfbpLuHbfIT47b6MF33KH+6riLF0wc93CHN27wWtdUHvHuuIFbfDhuUv903CKfHbfxgC/HHerfjrtYen3HPTx7ambiIl0YKQ+xPM5ltE9CU9NqxVKaItaZGPqDmj6VmTShlRuxOoniEI2sVUIZnYqJzqxMEi1yo3dybf2ttfk4CJTT/bVOMYNBjAIpFiTJOLCWOGLOHGGPBCE7l32XO0tmw04MjQwCQ7774B//lDmrZkJY3hvOrHBiLuiJMKJqoVgrejQ3CP5Yubt0JwxNJa96Oypr6j621VSOMQKG+uP36eKmHylcb0MAeJxtwdEOgjAMBdBeWEFR/Mdl7bTJtMsygc/nwVfPoYF+QP+tGDAigDFhxgVXLLjhjhUPCtmKTtmLaGN7x6dy/Io5bybqoevRQ3LRObb0sk3HKpn1SFqW6ru26vbpYfcmRCccJhqsAAA=\") format(\"woff\")}.token.treeview-part .entry-name:before{content:\"\\ea01\";font-family:PrismTreeview;font-size:inherit;font-style:normal;-webkit-font-smoothing:antialiased;-moz-osx-font-smoothing:grayscale;width:2.5ex;display:inline-block}.token.treeview-part .entry-name.dir:before{content:\"\\ea02\"}.token.treeview-part .entry-name.ext-bmp:before,.token.treeview-part .entry-name.ext-eps:before,.token.treeview-part .entry-name.ext-gif:before,.token.treeview-part .entry-name.ext-jpe:before,.token.treeview-part .entry-name.ext-jpeg:before,.token.treeview-part .entry-name.ext-jpg:before,.token.treeview-part .entry-name.ext-png:before,.token.treeview-part .entry-name.ext-svg:before,.token.treeview-part .entry-name.ext-tiff:before{content:\"\\ea03\"}.token.treeview-part .entry-name.ext-cfg:before,.token.treeview-part .entry-name.ext-conf:before,.token.treeview-part .entry-name.ext-config:before,.token.treeview-part .entry-name.ext-csv:before,.token.treeview-part .entry-name.ext-ini:before,.token.treeview-part .entry-name.ext-log:before,.token.treeview-part .entry-name.ext-md:before,.token.treeview-part .entry-name.ext-nfo:before,.token.treeview-part .entry-name.ext-txt:before{content:\"\\ea06\"}.token.treeview-part .entry-name.ext-asp:before,.token.treeview-part .entry-name.ext-aspx:before,.token.treeview-part .entry-name.ext-c:before,.token.treeview-part .entry-name.ext-cc:before,.token.treeview-part .entry-name.ext-cpp:before,.token.treeview-part .entry-name.ext-cs:before,.token.treeview-part .entry-name.ext-css:before,.token.treeview-part .entry-name.ext-h:before,.token.treeview-part .entry-name.ext-hh:before,.token.treeview-part .entry-name.ext-htm:before,.token.treeview-part .entry-name.ext-html:before,.token.treeview-part .entry-name.ext-jav:before,.token.treeview-part .entry-name.ext-java:before,.token.treeview-part .entry-name.ext-js:before,.token.treeview-part .entry-name.ext-php:before,.token.treeview-part .entry-name.ext-rb:before,.token.treeview-part .entry-name.ext-xml:before{content:\"\\ea07\"}.token.treeview-part .entry-name.ext-7z:before,.token.treeview-part .entry-name.ext-bz2:before,.token.treeview-part .entry-name.ext-bz:before,.token.treeview-part .entry-name.ext-gz:before,.token.treeview-part .entry-name.ext-rar:before,.token.treeview-part .entry-name.ext-tar:before,.token.treeview-part .entry-name.ext-tgz:before,.token.treeview-part .entry-name.ext-zip:before{content:\"\\ea08\"}.token.treeview-part .entry-name.ext-aac:before,.token.treeview-part .entry-name.ext-au:before,.token.treeview-part .entry-name.ext-cda:before,.token.treeview-part .entry-name.ext-flac:before,.token.treeview-part .entry-name.ext-mp3:before,.token.treeview-part .entry-name.ext-oga:before,.token.treeview-part .entry-name.ext-ogg:before,.token.treeview-part .entry-name.ext-wav:before,.token.treeview-part .entry-name.ext-wma:before{content:\"\\ea04\"}.token.treeview-part .entry-name.ext-avi:before,.token.treeview-part .entry-name.ext-flv:before,.token.treeview-part .entry-name.ext-mkv:before,.token.treeview-part .entry-name.ext-mov:before,.token.treeview-part .entry-name.ext-mp4:before,.token.treeview-part .entry-name.ext-mpeg:before,.token.treeview-part .entry-name.ext-mpg:before,.token.treeview-part .entry-name.ext-ogv:before,.token.treeview-part .entry-name.ext-webm:before{content:\"\\ea05\"}.token.treeview-part .entry-name.ext-pdf:before{content:\"\\ea09\"}.token.treeview-part .entry-name.ext-xls:before,.token.treeview-part .entry-name.ext-xlsx:before{content:\"\\ea0a\"}.token.treeview-part .entry-name.ext-doc:before,.token.treeview-part .entry-name.ext-docm:before,.token.treeview-part .entry-name.ext-docx:before{content:\"\\ea0c\"}.token.treeview-part .entry-name.ext-pps:before,.token.treeview-part .entry-name.ext-ppt:before,.token.treeview-part .entry-name.ext-pptx:before{content:\"\\ea0b\"}</style>";
                string jsCommon = "<script>function EditorAccordion() {const btns = document.querySelectorAll(\".lnBtn\");if (btns) {btns.forEach(x => {x.addEventListener('click', function() {if (x.innerHTML == \"+\") {x.innerHTML = \"-\";} else {x.innerHTML = \"+\";}const startId = parseInt(x.getAttribute(\"data-start-td\").replace(\"lineNoIns\", \"\").replace(\"lineNoDel\", \"\"));const endId = parseInt(x.getAttribute(\"data-end-td\").replace(\"lineNoIns\", \"\").replace(\"lineNoDel\", \"\"));const type = x.getAttribute(\"data-start-td\").includes(\"Ins\");const elementTd = document.querySelector(\"#lineNo\" + (type ? \"Ins\" : \"Del\") + (startId - 1)).querySelector(\"code\");if(elementTd){if(elementTd.className.includes(\"grayFilter\")){elementTd.classList.remove(\"grayFilter\");}else{elementTd.classList.add(\"grayFilter\");}}for (var i = startId; i <= endId; i++) {const element = document.querySelector(\"#lineNo\" + (type ? \"Ins\" : \"Del\") + i).closest(\"tr\");if (element) {if (element.style.display == \"none\") {element.style.display = \"\";} else {element.style.display = \"none\";}}}});});}} $(document).ready(function(){EditorAccordion(); $('#framework').multiselect({nonSelectedText: 'Filter',enableFiltering: false,buttonWidth: '140px',selectAllText: 'All',includeSelectAllOption: true,selectAllNumber: true});$('#framework').on('change', function(){var selectedOptions = $('#framework option:selected');var selectedCount = selectedOptions.length;if (selectedCount <= 1) {$('.multiselect-selected-text').text('1 selected');var className = selectedOptions.attr('value');filterRows(className);} else {$('.multiselect-selected-text').text(selectedCount + ' selected');var classNames = [];selectedOptions.each(function(){classNames.push($(this).attr('value'));});filterRows(classNames.join(' '));}});function filterRows(className) {$(\"#resultTable tr\").each(function (index, row) {if (index !== 0) {if (className !== \"clr\" && className !== undefined) {if (className.includes($(row).attr(\"class\"))) {$(row).show();} else {$(row).hide();}} else {$(row).show();}}});}const searchInput = $('#srchTxt');const dataTable = $('#resultTable');searchInput.on('input', function(){filterRows(\"clr\");const searchText = searchInput.val().toLowerCase();const rows = dataTable.find('tr');let count = 0;rows.each(function (index, row) {if (index !== 0) {const oldxml = $(row).find('td:eq(0)').text().toLowerCase();const newxml = $(row).find('td:eq(1)').text().toLowerCase();if (oldxml.includes(searchText) || newxml.includes(searchText)) {$(row).show();count++;} else {$(row).hide();}}});if (count === 0) {$('#errTxt').show();} else {$('#errTxt').hide();}});$('.hideButton').on('click', function(){var ele = $(this).next();if (ele.css('display') === 'block' || ele.css('display') === '') {ele.hide();$(this).removeClass('collapsed');} else {ele.show();$(this).addClass('collapsed');}});$('#expandRadio').on('click', function(){var elems = $(\".hideCont\");var elebtn = $(\".hideButton\");elems.hide();elebtn.removeClass(\"collapsed\");});$('#collapseRadio').on('click', function(){var elems = $(\".hideCont\");var elebtn = $(\".hideButton\");elems.show();elebtn.addClass(\"collapsed\");});$('.sideBySide').hide();$('.radiobuttons input[type=\"radio\"]').click(function(){if ($(this).attr('id') == 'radio1') {$('.sideBySide, .iconsLis').show();$('.smartViews, .searchoptions, .filterPart').hide();} else {$('.smartViews, .searchoptions, .filterPart').show();$('.sideBySide, .iconsLis').hide();}});});</script>";
                string jsPrism = "<script>var _self = (typeof window !== 'undefined') ? window : ((typeof WorkerGlobalScope !== 'undefined' && self instanceof WorkerGlobalScope)? self: {});/*** Prism: Lightweight, robust, elegant syntax highlighting** @license MIT <https://opensource.org/licenses/MIT>* @author Lea Verou <https://lea.verou.me>* @namespace* @public*/var Prism = (function (_self) {var lang = /(?:^|\\s)lang(?:uage)?-([\\w-]+)(?=\\s|$)/i;var uniqueId = 0;var plainTextGrammar = {};var _ = {/*** By default, Prism will attempt to highlight all code elements (by calling {@link Prism.highlightAll}) on the* current page after the page finished loading. This might be a problem if e.g. you wanted to asynchronously load* additional languages or plugins yourself.** By setting this value to `true`, Prism will not automatically highlight all code elements on the page.** You obviously have to change this value before the automatic highlighting started. To do this, you can add an* empty Prism object into the global scope before loading the Prism script like this:** ```js* window.Prism = window.Prism || {};* Prism.manual = true;* * ```** @default false* @type {boolean}* @memberof Prism* @public*/manual: _self.Prism && _self.Prism.manual,/*** By default, if Prism is in a web worker, it assumes that it is in a worker it created itself, so it uses* `addEventListener` to communicate with its parent instance. However, if you're using Prism manually in your* own worker, you don't want it to do this.** By setting this value to `true`, Prism will not add its own listeners to the worker.** You obviously have to change this value before Prism executes. To do this, you can add an* empty Prism object into the global scope before loading the Prism script like this:** ```js* window.Prism = window.Prism || {};* Prism.disableWorkerMessageHandler = true;* * ```** @default false* @type {boolean}* @memberof Prism* @public*/disableWorkerMessageHandler: _self.Prism && _self.Prism.disableWorkerMessageHandler,/*** A namespace for utility methods.** All function in this namespace that are not explicitly marked as _public_ are for __internal use only__ and may* change or disappear at any time.** @namespace* @memberof Prism*/util: {encode: function encode(tokens) {if (tokens instanceof Token) {return new Token(tokens.type, encode(tokens.content), tokens.alias);} else if (Array.isArray(tokens)) {return tokens.map(encode);} else {return tokens.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/\\u00a0/g, ' ');}},/*** Returns the name of the type of the given value.** @param {any} o* @returns {string}* @example* type(null)      === 'Null'* type(undefined) === 'Undefined'* type(123)       === 'Number'* type('foo')     === 'String'* type(true)      === 'Boolean'* type([1, 2])    === 'Array'* type({})        === 'Object'* type(String)    === 'Function'* type(/abc+/)    === 'RegExp'*/type: function (o) {return Object.prototype.toString.call(o).slice(8, -1);},/*** Returns a unique number for the given object. Later calls will still return the same number.** @param {Object} obj* @returns {number}*/objId: function (obj) {if (!obj['__id']) {Object.defineProperty(obj, '__id', { value: ++uniqueId });}return obj['__id'];},/*** Creates a deep clone of the given object.** The main intended use of this function is to clone language definitions.** @param {T} o* @param {Record<number, any>} [visited]* @returns {T}* @template T*/clone: function deepClone(o, visited) {visited = visited || {};var clone; var id;switch (_.util.type(o)) {case 'Object':id = _.util.objId(o);if (visited[id]) {return visited[id];}clone = /** @type {Record<string, any>} */ ({});visited[id] = clone;for (var key in o) {if (o.hasOwnProperty(key)) {clone[key] = deepClone(o[key], visited);}}return /** @type {any} */ (clone);case 'Array':id = _.util.objId(o);if (visited[id]) {return visited[id];}clone = [];visited[id] = clone;(/** @type {Array} */(/** @type {any} */(o))).forEach(function (v, i) {clone[i] = deepClone(v, visited);});return /** @type {any} */ (clone);default:return o;}},/*** Returns the Prism language of the given element set by a `language-xxxx` or `lang-xxxx` class.** If no language is set for the element or the element is `null` or `undefined`, `none` will be returned.** @param {Element} element* @returns {string}*/getLanguage: function (element) {while (element) {var m = lang.exec(element.className);if (m) {return m[1].toLowerCase();}element = element.parentElement;}return 'none';},/*** Sets the Prism `language-xxxx` class of the given element.** @param {Element} element* @param {string} language* @returns {void}*/setLanguage: function (element, language) {element.className = element.className.replace(RegExp(lang, 'gi'), '');element.classList.add('language-' + language);},/*** Returns the script element that is currently executing.** This does __not__ work for line script element.** @returns {HTMLScriptElement | null}*/currentScript: function () {if (typeof document === 'undefined') {return null;}if ('currentScript' in document && 1 < 2 /* hack to trip TS' flow analysis */) {return /** @type {any} */ (document.currentScript);}try {throw new Error();} catch (err) {var src = (/at [^(\\r\\n]*\\((.*):[^:]+:[^:]+\\)$/i.exec(err.stack) || [])[1];if (src) {var scripts = document.getElementsByTagName('script');for (var i in scripts) {if (scripts[i].src == src) {return scripts[i];}}}return null;}},/*** Returns whether a given class is active for `element`.** The class can be activated if `element` or one of its ancestors has the given class and it can be deactivated* if `element` or one of its ancestors has the negated version of the given class. The _negated version_ of the* given class is just the given class with a `no-` prefix.** Whether the class is active is determined by the closest ancestor of `element` (where `element` itself is* closest ancestor) that has the given class or the negated version of it. If neither `element` nor any of its* ancestors have the given class or the negated version of it, then the default activation will be returned.** In the paradoxical situation where the closest ancestor contains __both__ the given class and the negated* version of it, the class is considered active.** @param {Element} element* @param {string} className* @param {boolean} [defaultActivation=false]* @returns {boolean}*/isActive: function (element, className, defaultActivation) {var no = 'no-' + className;while (element) {var classList = element.classList;if (classList.contains(className)) {return true;}if (classList.contains(no)) {return false;}element = element.parentElement;}return !!defaultActivation;}},/*** This namespace contains all currently loaded languages and the some helper functions to create and modify languages.** @namespace* @memberof Prism* @public*/languages: {/*** The grammar for plain, unformatted text.*/plain: plainTextGrammar,plaintext: plainTextGrammar,text: plainTextGrammar,txt: plainTextGrammar,/*** Creates a deep copy of the language with the given id and appends the given tokens.** If a token in `redef` also appears in the copied language, then the existing token in the copied language* will be overwritten at its original position.** ## Best practices** Since the position of overwriting tokens (token in `redef` that overwrite tokens in the copied language)* doesn't matter, they can technically be in any order. However, this can be confusing to others that trying to* understand the language definition because, normally, the order of tokens matters in Prism grammars.** Therefore, it is encouraged to order overwriting tokens according to the positions of the overwritten tokens.* Furthermore, all non-overwriting tokens should be placed after the overwriting ones.** @param {string} id The id of the language to extend. This has to be a key in `Prism.languages`.* @param {Grammar} redef The new tokens to append.* @returns {Grammar} The new language created.* @public* @example* Prism.languages['css-with-colors'] = Prism.languages.extend('css', {*     *     *     'comment': { ... },*     *     'color': /\\b(?:red|green|blue)\\b/* });*/extend: function (id, redef) {var lang = _.util.clone(_.languages[id]);for (var key in redef) {lang[key] = redef[key];}return lang;},/*** Inserts tokens _before_ another token in a language definition or any other grammar.** ## Usage** This helper method makes it easy to modify existing languages. For example, the CSS language definition* not only defines CSS highlighting for CSS documents, but also needs to define highlighting for CSS embedded* in HTML through `<style>` elements. To do this, it needs to modify `Prism.languages.markup` and add the* appropriate tokens. However, `Prism.languages.markup` is a regular JavaScript object literal, so if you do* this:** ```js* Prism.languages.markup.style = {*     * };* ```** then the `style` token will be added (and processed) at the end. `insertBefore` allows you to insert tokens* before existing tokens. For the CSS example above, you would use it like this:** ```js* Prism.languages.insertBefore('markup', 'cdata', {*     'style': {*         *     }* });* ```** ## Special cases** If the grammars of `inside` and `insert` have tokens with the same name, the tokens in `inside`'s grammar* will be ignored.** This behavior can be used to insert tokens after `before`:** ```js* Prism.languages.insertBefore('markup', 'comment', {*     'comment': Prism.languages.markup.comment,*     * });* ```** ## Limitations** The main problem `insertBefore` has to solve is iteration order. Since ES2015, the iteration order for object* properties is guaranteed to be the insertion order (except for integer keys) but some browsers behave* differently when keys are deleted and re-inserted. So `insertBefore` can't be implemented by temporarily* deleting properties which is necessary to insert at arbitrary positions.** To solve this problem, `insertBefore` doesn't actually insert the given tokens into the target object.* Instead, it will create a new object and replace all references to the target object with the new one. This* can be done without temporarily deleting properties, so the iteration order is well-defined.** However, only references that can be reached from `Prism.languages` or `insert` will be replaced. I.e. if* you hold the target object in a variable, then the value of the variable will not change.** ```js* var oldMarkup = Prism.languages.markup;* var newMarkup = Prism.languages.insertBefore('markup', 'comment', { ... });** assert(oldMarkup !== Prism.languages.markup);* assert(newMarkup === Prism.languages.markup);* ```** @param {string} inside The property of `root` (e.g. a language id in `Prism.languages`) that contains the* object to be modified.* @param {string} before The key to insert before.* @param {Grammar} insert An object containing the key-value pairs to be inserted.* @param {Object<string, any>} [root] The object containing `inside`, i.e. the object that contains the* object to be modified.** Defaults to `Prism.languages`.* @returns {Grammar} The new grammar object.* @public*/insertBefore: function (inside, before, insert, root) {root = root || /** @type {any} */ (_.languages);var grammar = root[inside];/** @type {Grammar} */var ret = {};for (var token in grammar) {if (grammar.hasOwnProperty(token)) {if (token == before) {for (var newToken in insert) {if (insert.hasOwnProperty(newToken)) {ret[newToken] = insert[newToken];}}}if (!insert.hasOwnProperty(token)) {ret[token] = grammar[token];}}}var old = root[inside];root[inside] = ret;_.languages.DFS(_.languages, function (key, value) {if (value === old && key != inside) {this[key] = ret;}});return ret;},DFS: function DFS(o, callback, type, visited) {visited = visited || {};var objId = _.util.objId;for (var i in o) {if (o.hasOwnProperty(i)) {callback.call(o, i, o[i], type || i);var property = o[i];var propertyType = _.util.type(property);if (propertyType === 'Object' && !visited[objId(property)]) {visited[objId(property)] = true;DFS(property, callback, null, visited);} else if (propertyType === 'Array' && !visited[objId(property)]) {visited[objId(property)] = true;DFS(property, callback, i, visited);}}}}},plugins: {},/*** This is the most high-level function in Prism’s API.* It fetches all the elements that have a `.language-xxxx` class and then calls {@link Prism.highlightElement} on* each one of them.** This is equivalent to `Prism.highlightAllUnder(document, async, callback)`.** @param {boolean} [async=false] Same as in {@link Prism.highlightAllUnder}.* @param {HighlightCallback} [callback] Same as in {@link Prism.highlightAllUnder}.* @memberof Prism* @public*/highlightAll: function (async, callback) {_.highlightAllUnder(document, async, callback);},/*** Fetches all the descendants of `container` that have a `.language-xxxx` class and then calls* {@link Prism.highlightElement} on each one of them.** The following hooks will be run:* 1. `before-highlightall`* 2. `before-all-elements-highlight`* 3. All hooks of {@link Prism.highlightElement} for each element.** @param {ParentNode} container The root element, whose descendants that have a `.language-xxxx` class will be highlighted.* @param {boolean} [async=false] Whether each element is to be highlighted asynchronously using Web Workers.* @param {HighlightCallback} [callback] An optional callback to be invoked on each element after its highlighting is done.* @memberof Prism* @public*/highlightAllUnder: function (container, async, callback) {var env = {callback: callback,container: container,selector: 'code[class*=\"language-\"], [class*=\"language-\"] code, code[class*=\"lang-\"], [class*=\"lang-\"] code'};_.hooks.run('before-highlightall', env);env.elements = Array.prototype.slice.apply(env.container.querySelectorAll(env.selector));_.hooks.run('before-all-elements-highlight', env);for (var i = 0, element; (element = env.elements[i++]);) {_.highlightElement(element, async === true, env.callback);}},/*** Highlights the code inside a single element.** The following hooks will be run:* 1. `before-sanity-check`* 2. `before-highlight`* 3. All hooks of {@link Prism.highlight}. These hooks will be run by an asynchronous worker if `async` is `true`.* 4. `before-insert`* 5. `after-highlight`* 6. `complete`** Some the above hooks will be skipped if the element doesn't contain any text or there is no grammar loaded for* the element's language.** @param {Element} element The element containing the code.* It must have a class of `language-xxxx` to be processed, where `xxxx` is a valid language identifier.* @param {boolean} [async=false] Whether the element is to be highlighted asynchronously using Web Workers* to improve performance and avoid blocking the UI when highlighting very large chunks of code. This option is* [disabled by default](https://prismjs.com/faq.html#why-is-asynchronous-highlighting-disabled-by-default).** Note: All language definitions required to highlight the code must be included in the main `prism.js` file for* asynchronous highlighting to work. You can build your own bundle on the* [Download page](https://prismjs.com/download.html).* @param {HighlightCallback} [callback] An optional callback to be invoked after the highlighting is done.* Mostly useful when `async` is `true`, since in that case, the highlighting is done asynchronously.* @memberof Prism* @public*/highlightElement: function (element, async, callback) {var language = _.util.getLanguage(element);var grammar = _.languages[language];_.util.setLanguage(element, language);var parent = element.parentElement;if (parent && parent.nodeName.toLowerCase() === 'pre') {_.util.setLanguage(parent, language);}var code = element.textContent;var env = {element: element,language: language,grammar: grammar,code: code};function insertHighlightedCode(highlightedCode) {env.highlightedCode = highlightedCode;_.hooks.run('before-insert', env);env.element.innerHTML = env.highlightedCode;_.hooks.run('after-highlight', env);_.hooks.run('complete', env);callback && callback.call(env.element);}_.hooks.run('before-sanity-check', env);parent = env.element.parentElement;if (parent && parent.nodeName.toLowerCase() === 'pre' && !parent.hasAttribute('tabindex')) {parent.setAttribute('tabindex', '0');}if (!env.code) {_.hooks.run('complete', env);callback && callback.call(env.element);return;}_.hooks.run('before-highlight', env);if (!env.grammar) {insertHighlightedCode(_.util.encode(env.code));return;}if (async && _self.Worker) {var worker = new Worker(_.filename);worker.onmessage = function (evt) {insertHighlightedCode(evt.data);};worker.postMessage(JSON.stringify({language: env.language,code: env.code,immediateClose: true}));} else {insertHighlightedCode(_.highlight(env.code, env.grammar, env.language));}},/*** Low-level function, only use if you know what you’re doing. It accepts a string of text as input* and the language definitions to use, and returns a string with the HTML produced.** The following hooks will be run:* 1. `before-tokenize`* 2. `after-tokenize`* 3. `wrap`: On each {@link Token}.** @param {string} text A string with the code to be highlighted.* @param {Grammar} grammar An object containing the tokens to use.** Usually a language definition like `Prism.languages.markup`.* @param {string} language The name of the language definition passed to `grammar`.* @returns {string} The highlighted HTML.* @memberof Prism* @public* @example* Prism.highlight('var foo = true;', Prism.languages.javascript, 'javascript');*/highlight: function (text, grammar, language) {var env = {code: text,grammar: grammar,language: language};_.hooks.run('before-tokenize', env);if (!env.grammar) {throw new Error('The language \"' + env.language + '\" has no grammar.');}env.tokens = _.tokenize(env.code, env.grammar);_.hooks.run('after-tokenize', env);return Token.stringify(_.util.encode(env.tokens), env.language);},/*** This is the heart of Prism, and the most low-level function you can use. It accepts a string of text as input* and the language definitions to use, and returns an array with the tokenized code.** When the language definition includes nested tokens, the function is called recursively on each of these tokens.** This method could be useful in other contexts as well, as a very crude parser.** @param {string} text A string with the code to be highlighted.* @param {Grammar} grammar An object containing the tokens to use.** Usually a language definition like `Prism.languages.markup`.* @returns {TokenStream} An array of strings and tokens, a token stream.* @memberof Prism* @public* @example* let code = `var foo = 0;`;* let tokens = Prism.tokenize(code, Prism.languages.javascript);* tokens.forEach(token => {*     if (token instanceof Prism.Token && token.type === 'number') {*         console.log(`Found numeric literal: ${token.content}`);*     }* });*/tokenize: function (text, grammar) {var rest = grammar.rest;if (rest) {for (var token in rest) {grammar[token] = rest[token];}delete grammar.rest;}var tokenList = new LinkedList();addAfter(tokenList, tokenList.head, text);matchGrammar(text, tokenList, grammar, tokenList.head, 0);return toArray(tokenList);},/*** @namespace* @memberof Prism* @public*/hooks: {all: {},/*** Adds the given callback to the list of callbacks for the given hook.** The callback will be invoked when the hook it is registered for is run.* Hooks are usually directly run by a highlight function but you can also run hooks yourself.** One callback function can be registered to multiple hooks and the same hook multiple times.** @param {string} name The name of the hook.* @param {HookCallback} callback The callback function which is given environment variables.* @public*/add: function (name, callback) {var hooks = _.hooks.all;hooks[name] = hooks[name] || [];hooks[name].push(callback);},/*** Runs a hook invoking all registered callbacks with the given environment variables.** Callbacks will be invoked synchronously and in the order in which they were registered.** @param {string} name The name of the hook.* @param {Object<string, any>} env The environment variables of the hook passed to all callbacks registered.* @public*/run: function (name, env) {var callbacks = _.hooks.all[name];if (!callbacks || !callbacks.length) {return;}for (var i = 0, callback; (callback = callbacks[i++]);) {callback(env);}}},Token: Token};_self.Prism = _;/*** Creates a new token.** @param {string} type See {@link Token#type type}* @param {string | TokenStream} content See {@link Token#content content}* @param {string|string[]} [alias] The alias(es) of the token.* @param {string} [matchedStr=\"\"] A copy of the full string this token was created from.* @class* @global* @public*/function Token(type, content, alias, matchedStr) {/*** The type of the token.** This is usually the key of a pattern in a {@link Grammar}.** @type {string}* @see GrammarToken* @public*/this.type = type;/*** The strings or tokens contained by this token.** This will be a token stream if the pattern matched also defined an `inside` grammar.** @type {string | TokenStream}* @public*/this.content = content;/*** The alias(es) of the token.** @type {string|string[]}* @see GrammarToken* @public*/this.alias = alias;this.length = (matchedStr || '').length | 0;}/*** A token stream is an array of strings and {@link Token Token} objects.** Token streams have to fulfill a few properties that are assumed by most functions (mostly internal ones) that process* them.** 1. No adjacent strings.* 2. No empty strings.**    The only exception here is the token stream that only contains the empty string and nothing else.** @typedef {Array<string | Token>} TokenStream* @global* @public*//*** Converts the given token or token stream to an HTML representation.** The following hooks will be run:* 1. `wrap`: On each {@link Token}.** @param {string | Token | TokenStream} o The token or token stream to be converted.* @param {string} language The name of current language.* @returns {string} The HTML representation of the token or token stream.* @memberof Token* @static*/Token.stringify = function stringify(o, language) {if (typeof o == 'string') {return o;}if (Array.isArray(o)) {var s = '';o.forEach(function (e) {s += stringify(e, language);});return s;}var env = {type: o.type,content: stringify(o.content, language),tag: 'span',classes: ['token', o.type],attributes: {},language: language};var aliases = o.alias;if (aliases) {if (Array.isArray(aliases)) {Array.prototype.push.apply(env.classes, aliases);} else {env.classes.push(aliases);}}_.hooks.run('wrap', env);var attributes = '';for (var name in env.attributes) {attributes += ' ' + name + '=\"' + (env.attributes[name] || '').replace(/\"/g, '&quot;') + '\"';}return '<' + env.tag + ' class=\"' + env.classes.join(' ') + '\"' + attributes + '>' + env.content + '</' + env.tag + '>';};/*** @param {RegExp} pattern* @param {number} pos* @param {string} text* @param {boolean} lookbehind* @returns {RegExpExecArray | null}*/function matchPattern(pattern, pos, text, lookbehind) {pattern.lastIndex = pos;var match = pattern.exec(text);if (match && lookbehind && match[1]) {var lookbehindLength = match[1].length;match.index += lookbehindLength;match[0] = match[0].slice(lookbehindLength);}return match;}/*** @param {string} text* @param {LinkedList<string | Token>} tokenList* @param {any} grammar* @param {LinkedListNode<string | Token>} startNode* @param {number} startPos* @param {RematchOptions} [rematch]* @returns {void}* @private** @typedef RematchOptions* @property {string} cause* @property {number} reach*/function matchGrammar(text, tokenList, grammar, startNode, startPos, rematch) {for (var token in grammar) {if (!grammar.hasOwnProperty(token) || !grammar[token]) {continue;}var patterns = grammar[token];patterns = Array.isArray(patterns) ? patterns : [patterns];for (var j = 0; j < patterns.length; ++j) {if (rematch && rematch.cause == token + ',' + j) {return;}var patternObj = patterns[j];var inside = patternObj.inside;var lookbehind = !!patternObj.lookbehind;var greedy = !!patternObj.greedy;var alias = patternObj.alias;if (greedy && !patternObj.pattern.global) {var flags = patternObj.pattern.toString().match(/[imsuy]*$/)[0];patternObj.pattern = RegExp(patternObj.pattern.source, flags + 'g');}/** @type {RegExp} */var pattern = patternObj.pattern || patternObj;for ( var currentNode = startNode.next, pos = startPos;currentNode !== tokenList.tail;pos += currentNode.value.length, currentNode = currentNode.next) {if (rematch && pos >= rematch.reach) {break;}var str = currentNode.value;if (tokenList.length > text.length) {return;}if (str instanceof Token) {continue;}var removeCount = 1; var match;if (greedy) {match = matchPattern(pattern, pos, text, lookbehind);if (!match || match.index >= text.length) {break;}var from = match.index;var to = match.index + match[0].length;var p = pos;p += currentNode.value.length;while (from >= p) {currentNode = currentNode.next;p += currentNode.value.length;}p -= currentNode.value.length;pos = p;if (currentNode.value instanceof Token) {continue;}for (var k = currentNode;k !== tokenList.tail && (p < to || typeof k.value === 'string');k = k.next) {removeCount++;p += k.value.length;}removeCount--;str = text.slice(pos, p);match.index -= pos;} else {match = matchPattern(pattern, 0, str, lookbehind);if (!match) {continue;}}var from = match.index;var matchStr = match[0];var before = str.slice(0, from);var after = str.slice(from + matchStr.length);var reach = pos + str.length;if (rematch && reach > rematch.reach) {rematch.reach = reach;}var removeFrom = currentNode.prev;if (before) {removeFrom = addAfter(tokenList, removeFrom, before);pos += before.length;}removeRange(tokenList, removeFrom, removeCount);var wrapped = new Token(token, inside ? _.tokenize(matchStr, inside) : matchStr, alias, matchStr);currentNode = addAfter(tokenList, removeFrom, wrapped);if (after) {addAfter(tokenList, currentNode, after);}if (removeCount > 1) {/** @type {RematchOptions} */var nestedRematch = {cause: token + ',' + j,reach: reach};matchGrammar(text, tokenList, grammar, currentNode.prev, pos, nestedRematch);if (rematch && nestedRematch.reach > rematch.reach) {rematch.reach = nestedRematch.reach;}}}}}}/*** @typedef LinkedListNode* @property {T} value* @property {LinkedListNode<T> | null} prev The previous node.* @property {LinkedListNode<T> | null} next The next node.* @template T* @private*//*** @template T* @private*/function LinkedList() {/** @type {LinkedListNode<T>} */var head = { value: null, prev: null, next: null };/** @type {LinkedListNode<T>} */var tail = { value: null, prev: head, next: null };head.next = tail;/** @type {LinkedListNode<T>} */this.head = head;/** @type {LinkedListNode<T>} */this.tail = tail;this.length = 0;}/*** Adds a new node with the given value to the list.** @param {LinkedList<T>} list* @param {LinkedListNode<T>} node* @param {T} value* @returns {LinkedListNode<T>} The added node.* @template T*/function addAfter(list, node, value) {var next = node.next;var newNode = { value: value, prev: node, next: next };node.next = newNode;next.prev = newNode;list.length++;return newNode;}/*** Removes `count` nodes after the given node. The given node will not be removed.** @param {LinkedList<T>} list* @param {LinkedListNode<T>} node* @param {number} count* @template T*/function removeRange(list, node, count) {var next = node.next;for (var i = 0; i < count && next !== list.tail; i++) {next = next.next;}node.next = next;next.prev = node;list.length -= i;}/*** @param {LinkedList<T>} list* @returns {T[]}* @template T*/function toArray(list) {var array = [];var node = list.head.next;while (node !== list.tail) {array.push(node.value);node = node.next;}return array;}if (!_self.document) {if (!_self.addEventListener) {return _;}if (!_.disableWorkerMessageHandler) {_self.addEventListener('message', function (evt) {var message = JSON.parse(evt.data);var lang = message.language;var code = message.code;var immediateClose = message.immediateClose;_self.postMessage(_.highlight(code, _.languages[lang], lang));if (immediateClose) {_self.close();}}, false);}return _;}var script = _.util.currentScript();if (script) {_.filename = script.src;if (script.hasAttribute('data-manual')) {_.manual = true;}}function highlightAutomaticallyCallback() {if (!_.manual) {_.highlightAll();}}if (!_.manual) {var readyState = document.readyState;if (readyState === 'loading' || readyState === 'interactive' && script && script.defer) {document.addEventListener('DOMContentLoaded', highlightAutomaticallyCallback);} else {if (window.requestAnimationFrame) {window.requestAnimationFrame(highlightAutomaticallyCallback);} else {window.setTimeout(highlightAutomaticallyCallback, 16);}}}return _;}(_self));if (typeof module !== 'undefined' && module.exports) {module.exports = Prism;}if (typeof global !== 'undefined') {global.Prism = Prism;}/*** The expansion of a simple `RegExp` literal to support additional properties.** @typedef GrammarToken* @property {RegExp} pattern The regular expression of the token.* @property {boolean} [lookbehind=false] If `true`, then the first capturing group of `pattern` will (effectively)* behave as a lookbehind group meaning that the captured text will not be part of the matched text of the new token.* @property {boolean} [greedy=false] Whether the token is greedy.* @property {string|string[]} [alias] An optional alias or list of aliases.* @property {Grammar} [inside] The nested grammar of this token.** The `inside` grammar will be used to tokenize the text value of each token of this kind.** This can be used to make nested and even recursive language definitions.** Note: This can cause infinite recursion. Be careful when you embed different languages or even the same language into* each another.* @global* @public*//*** @typedef Grammar* @type {Object<string, RegExp | GrammarToken | Array<RegExp | GrammarToken>>}* @property {Grammar} [rest] An optional grammar object that will be appended to this grammar.* @global* @public*//*** A function which will invoked after an element was successfully highlighted.** @callback HighlightCallback* @param {Element} element The element successfully highlighted.* @returns {void}* @global* @public*//*** @callback HookCallback* @param {Object<string, any>} env The environment variables of the hook.* @returns {void}* @global* @public*/;Prism.languages.markup = {'comment': {pattern: /<!--(?:(?!<!--)[\\s\\S])*?-->/,greedy: true},'prolog': {pattern: /<\\?[\\s\\S]+?\\?>/,greedy: true},'doctype': {pattern: /<!DOCTYPE(?:[^>\"'[\\]]|\"[^\"]*\"|'[^']*')+(?:\\[(?:[^<\"'\\]]|\"[^\"]*\"|'[^']*'|<(?!!--)|<!--(?:[^-]|-(?!->))*-->)*\\]\\s*)?>/i,greedy: true,inside: {'internal-subset': {pattern: /(^[^\\[]*\\[)[\\s\\S]+(?=\\]>$)/,lookbehind: true,greedy: true,inside: null },'string': {pattern: /\"[^\"]*\"|'[^']*'/,greedy: true},'punctuation': /^<!|>$|[[\\]]/,'doctype-tag': /^DOCTYPE/i,'name': /[^\\s<>'\"]+/}},'cdata': {pattern: /<!\\[CDATA\\[[\\s\\S]*?\\]\\]>/i,greedy: true},'tag': {pattern: /<\\/?(?!\\d)[^\\s>\\/=$<%]+(?:\\s(?:\\s*[^\\s>\\/=]+(?:\\s*=\\s*(?:\"[^\"]*\"|'[^']*'|[^\\s'\">=]+(?=[\\s>]))|(?=[\\s/>])))+)?\\s*\\/?>/,greedy: true,inside: {'tag': {pattern: /^<\\/?[^\\s>\\/]+/,inside: {'punctuation': /^<\\/?/,'namespace': /^[^\\s>\\/:]+:/}},'special-attr': [],'attr-value': {pattern: /=\\s*(?:\"[^\"]*\"|'[^']*'|[^\\s'\">=]+)/,inside: {'punctuation': [{pattern: /^=/,alias: 'attr-equals'},{pattern: /^(\\s*)[\"']|[\"']$/,lookbehind: true}]}},'punctuation': /\\/?>/,'attr-name': {pattern: /[^\\s>\\/]+/,inside: {'namespace': /^[^\\s>\\/:]+:/}}}},'entity': [{pattern: /&[\\da-z]{1,8};/i,alias: 'named-entity'},/&#x?[\\da-f]{1,8};/i]};Prism.languages.markup['tag'].inside['attr-value'].inside['entity'] =Prism.languages.markup['entity'];Prism.languages.markup['doctype'].inside['internal-subset'].inside = Prism.languages.markup;Prism.hooks.add('wrap', function (env) {if (env.type === 'entity') {env.attributes['title'] = env.content.replace(/&amp;/, '&');}});Object.defineProperty(Prism.languages.markup.tag, 'addInlined', {/*** Adds an inlined language to markup.** An example of an inlined language is CSS with `<style>` tags.** @param {string} tagName The name of the tag that contains the inlined language. This name will be treated as* case insensitive.* @param {string} lang The language key.* @example* addInlined('style', 'css');*/value: function addInlined(tagName, lang) {var includedCdataInside = {};includedCdataInside['language-' + lang] = {pattern: /(^<!\\[CDATA\\[)[\\s\\S]+?(?=\\]\\]>$)/i,lookbehind: true,inside: Prism.languages[lang]};includedCdataInside['cdata'] = /^<!\\[CDATA\\[|\\]\\]>$/i;var inside = {'included-cdata': {pattern: /<!\\[CDATA\\[[\\s\\S]*?\\]\\]>/i,inside: includedCdataInside}};inside['language-' + lang] = {pattern: /[\\s\\S]+/,inside: Prism.languages[lang]};var def = {};def[tagName] = {pattern: RegExp(/(<__[^>]*>)(?:<!\\[CDATA\\[(?:[^\\]]|\\](?!\\]>))*\\]\\]>|(?!<!\\[CDATA\\[)[\\s\\S])*?(?=<\\/__>)/.source.replace(/__/g, function () { return tagName; }), 'i'),lookbehind: true,greedy: true,inside: inside};Prism.languages.insertBefore('markup', 'cdata', def);}});Object.defineProperty(Prism.languages.markup.tag, 'addAttribute', {/*** Adds an pattern to highlight languages embedded in HTML attributes.** An example of an inlined language is CSS with `style` attributes.** @param {string} attrName The name of the tag that contains the inlined language. This name will be treated as* case insensitive.* @param {string} lang The language key.* @example* addAttribute('style', 'css');*/value: function (attrName, lang) {Prism.languages.markup.tag.inside['special-attr'].push({pattern: RegExp(/(^|[\"'\\s])/.source + '(?:' + attrName + ')' + /\\s*=\\s*(?:\"[^\"]*\"|'[^']*'|[^\\s'\">=]+(?=[\\s>]))/.source,'i'),lookbehind: true,inside: {'attr-name': /^[^\\s=]+/,'attr-value': {pattern: /=[\\s\\S]+/,inside: {'value': {pattern: /(^=\\s*([\"']|(?![\"'])))\\S[\\s\\S]*(?=\\2$)/,lookbehind: true,alias: [lang, 'language-' + lang],inside: Prism.languages[lang]},'punctuation': [{pattern: /^=/,alias: 'attr-equals'},/\"|'/]}}}});}});Prism.languages.html = Prism.languages.markup;Prism.languages.mathml = Prism.languages.markup;Prism.languages.svg = Prism.languages.markup;Prism.languages.xml = Prism.languages.extend('markup', {});Prism.languages.ssml = Prism.languages.xml;Prism.languages.atom = Prism.languages.xml;Prism.languages.rss = Prism.languages.xml;(function (Prism) {var string = /(?:\"(?:\\\\(?:\\r\\n|[\\s\\S])|[^\"\\\\\\r\\n])*\"|'(?:\\\\(?:\\r\\n|[\\s\\S])|[^'\\\\\\r\\n])*')/;Prism.languages.css = {'comment': /\\/\\*[\\s\\S]*?\\*\\//,'atrule': {pattern: RegExp('@[\\\\w-](?:' + /[^;{\\s\"']|\\s+(?!\\s)/.source + '|' + string.source + ')*?' + /(?:;|(?=\\s*\\{))/.source),inside: {'rule': /^@[\\w-]+/,'selector-function-argument': {pattern: /(\\bselector\\s*\\(\\s*(?![\\s)]))(?:[^()\\s]|\\s+(?![\\s)])|\\((?:[^()]|\\([^()]*\\))*\\))+(?=\\s*\\))/,lookbehind: true,alias: 'selector'},'keyword': {pattern: /(^|[^\\w-])(?:and|not|only|or)(?![\\w-])/,lookbehind: true}}},'url': {pattern: RegExp('\\\\burl\\\\((?:' + string.source + '|' + /(?:[^\\\\\\r\\n()\"']|\\\\[\\s\\S])*/.source + ')\\\\)', 'i'),greedy: true,inside: {'function': /^url/i,'punctuation': /^\\(|\\)$/,'string': {pattern: RegExp('^' + string.source + '$'),alias: 'url'}}},'selector': {pattern: RegExp('(^|[{}\\\\s])[^{}\\\\s](?:[^{};\"\\'\\\\s]|\\\\s+(?![\\\\s{])|' + string.source + ')*(?=\\\\s*\\\\{)'),lookbehind: true},'string': {pattern: string,greedy: true},'property': {pattern: /(^|[^-\\w\\xA0-\\uFFFF])(?!\\s)[-_a-z\\xA0-\\uFFFF](?:(?!\\s)[-\\w\\xA0-\\uFFFF])*(?=\\s*:)/i,lookbehind: true},'important': /!important\\b/i,'function': {pattern: /(^|[^-a-z0-9])[-a-z0-9]+(?=\\()/i,lookbehind: true},'punctuation': /[(){};:,]/};Prism.languages.css['atrule'].inside.rest = Prism.languages.css;var markup = Prism.languages.markup;if (markup) {markup.tag.addInlined('style', 'css');markup.tag.addAttribute('style', 'css');}}(Prism));Prism.languages.clike = {'comment': [{pattern: /(^|[^\\\\])\\/\\*[\\s\\S]*?(?:\\*\\/|$)/,lookbehind: true,greedy: true},{pattern: /(^|[^\\\\:])\\/\\/.*/,lookbehind: true,greedy: true}],'string': {pattern: /([\"'])(?:\\\\(?:\\r\\n|[\\s\\S])|(?!\\1)[^\\\\\\r\\n])*\\1/,greedy: true},'class-name': {pattern: /(\\b(?:class|extends|implements|instanceof|interface|new|trait)\\s+|\\bcatch\\s+\\()[\\w.\\\\]+/i,lookbehind: true,inside: {'punctuation': /[.\\\\]/}},'keyword': /\\b(?:break|catch|continue|do|else|finally|for|function|if|in|instanceof|new|null|return|throw|try|while)\\b/,'boolean': /\\b(?:false|true)\\b/,'function': /\\b\\w+(?=\\()/,'number': /\\b0x[\\da-f]+\\b|(?:\\b\\d+(?:\\.\\d*)?|\\B\\.\\d+)(?:e[+-]?\\d+)?/i,'operator': /[<>]=?|[!=]=?=?|--?|\\+\\+?|&&?|\\|\\|?|[?*/~^%]/,'punctuation': /[{}[\\];(),.:]/};Prism.languages.javascript = Prism.languages.extend('clike', {'class-name': [Prism.languages.clike['class-name'],{pattern: /(^|[^$\\w\\xA0-\\uFFFF])(?!\\s)[_$A-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*(?=\\.(?:constructor|prototype))/,lookbehind: true}],'keyword': [{pattern: /((?:^|\\})\\s*)catch\\b/,lookbehind: true},{pattern: /(^|[^.]|\\.\\.\\.\\s*)\\b(?:as|assert(?=\\s*\\{)|async(?=\\s*(?:function\\b|\\(|[$\\w\\xA0-\\uFFFF]|$))|await|break|case|class|const|continue|debugger|default|delete|do|else|enum|export|extends|finally(?=\\s*(?:\\{|$))|for|from(?=\\s*(?:['\"]|$))|function|(?:get|set)(?=\\s*(?:[#\\[$\\w\\xA0-\\uFFFF]|$))|if|implements|import|in|instanceof|interface|let|new|null|of|package|private|protected|public|return|static|super|switch|this|throw|try|typeof|undefined|var|void|while|with|yield)\\b/,lookbehind: true},],'function': /#?(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*(?=\\s*(?:\\.\\s*(?:apply|bind|call)\\s*)?\\()/,'number': {pattern: RegExp(/(^|[^\\w$])/.source +'(?:' +(/NaN|Infinity/.source +'|' +/0[bB][01]+(?:_[01]+)*n?/.source +'|' +/0[oO][0-7]+(?:_[0-7]+)*n?/.source +'|' +/0[xX][\\dA-Fa-f]+(?:_[\\dA-Fa-f]+)*n?/.source +'|' +/\\d+(?:_\\d+)*n/.source +'|' +/(?:\\d+(?:_\\d+)*(?:\\.(?:\\d+(?:_\\d+)*)?)?|\\.\\d+(?:_\\d+)*)(?:[Ee][+-]?\\d+(?:_\\d+)*)?/.source) +')' +/(?![\\w$])/.source),lookbehind: true},'operator': /--|\\+\\+|\\*\\*=?|=>|&&=?|\\|\\|=?|[!=]==|<<=?|>>>?=?|[-+*/%&|^!=<>]=?|\\.{3}|\\?\\?=?|\\?\\.?|[~:]/});Prism.languages.javascript['class-name'][0].pattern = /(\\b(?:class|extends|implements|instanceof|interface|new)\\s+)[\\w.\\\\]+/;Prism.languages.insertBefore('javascript', 'keyword', {'regex': {pattern: RegExp(/((?:^|[^$\\w\\xA0-\\uFFFF.\"'\\])\\s]|\\b(?:return|yield))\\s*)/.source +/\\//.source +'(?:' +/(?:\\[(?:[^\\]\\\\\\r\\n]|\\\\.)*\\]|\\\\.|[^/\\\\\\[\\r\\n])+\\/[dgimyus]{0,7}/.source +'|' +/(?:\\[(?:[^[\\]\\\\\\r\\n]|\\\\.|\\[(?:[^[\\]\\\\\\r\\n]|\\\\.|\\[(?:[^[\\]\\\\\\r\\n]|\\\\.)*\\])*\\])*\\]|\\\\.|[^/\\\\\\[\\r\\n])+\\/[dgimyus]{0,7}v[dgimyus]{0,7}/.source +')' +/(?=(?:\\s|\\/\\*(?:[^*]|\\*(?!\\/))*\\*\\/)*(?:$|[\\r\\n,.;:})\\]]|\\/\\/))/.source),lookbehind: true,greedy: true,inside: {'regex-source': {pattern: /^(\\/)[\\s\\S]+(?=\\/[a-z]*$)/,lookbehind: true,alias: 'language-regex',inside: Prism.languages.regex},'regex-delimiter': /^\\/|\\/$/,'regex-flags': /^[a-z]+$/,}},'function-variable': {pattern: /#?(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*(?=\\s*[=:]\\s*(?:async\\s*)?(?:\\bfunction\\b|(?:\\((?:[^()]|\\([^()]*\\))*\\)|(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*)\\s*=>))/,alias: 'function'},'parameter': [{pattern: /(function(?:\\s+(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*)?\\s*\\(\\s*)(?!\\s)(?:[^()\\s]|\\s+(?![\\s)])|\\([^()]*\\))+(?=\\s*\\))/,lookbehind: true,inside: Prism.languages.javascript},{pattern: /(^|[^$\\w\\xA0-\\uFFFF])(?!\\s)[_$a-z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*(?=\\s*=>)/i,lookbehind: true,inside: Prism.languages.javascript},{pattern: /(\\(\\s*)(?!\\s)(?:[^()\\s]|\\s+(?![\\s)])|\\([^()]*\\))+(?=\\s*\\)\\s*=>)/,lookbehind: true,inside: Prism.languages.javascript},{pattern: /((?:\\b|\\s|^)(?!(?:as|async|await|break|case|catch|class|const|continue|debugger|default|delete|do|else|enum|export|extends|finally|for|from|function|get|if|implements|import|in|instanceof|interface|let|new|null|of|package|private|protected|public|return|set|static|super|switch|this|throw|try|typeof|undefined|var|void|while|with|yield)(?![$\\w\\xA0-\\uFFFF]))(?:(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*\\s*)\\(\\s*|\\]\\s*\\(\\s*)(?!\\s)(?:[^()\\s]|\\s+(?![\\s)])|\\([^()]*\\))+(?=\\s*\\)\\s*\\{)/,lookbehind: true,inside: Prism.languages.javascript}],'constant': /\\b[A-Z](?:[A-Z_]|\\dx?)*\\b/});Prism.languages.insertBefore('javascript', 'string', {'hashbang': {pattern: /^#!.*/,greedy: true,alias: 'comment'},'template-string': {pattern: /`(?:\\\\[\\s\\S]|\\$\\{(?:[^{}]|\\{(?:[^{}]|\\{[^}]*\\})*\\})+\\}|(?!\\$\\{)[^\\\\`])*`/,greedy: true,inside: {'template-punctuation': {pattern: /^`|`$/,alias: 'string'},'interpolation': {pattern: /((?:^|[^\\\\])(?:\\\\{2})*)\\$\\{(?:[^{}]|\\{(?:[^{}]|\\{[^}]*\\})*\\})+\\}/,lookbehind: true,inside: {'interpolation-punctuation': {pattern: /^\\$\\{|\\}$/,alias: 'punctuation'},rest: Prism.languages.javascript}},'string': /[\\s\\S]+/}},'string-property': {pattern: /((?:^|[,{])[ \\t]*)([\"'])(?:\\\\(?:\\r\\n|[\\s\\S])|(?!\\2)[^\\\\\\r\\n])*\\2(?=\\s*:)/m,lookbehind: true,greedy: true,alias: 'property'}});Prism.languages.insertBefore('javascript', 'operator', {'literal-property': {pattern: /((?:^|[,{])[ \\t]*)(?!\\s)[_$a-zA-Z\\xA0-\\uFFFF](?:(?!\\s)[$\\w\\xA0-\\uFFFF])*(?=\\s*:)/m,lookbehind: true,alias: 'property'},});if (Prism.languages.markup) {Prism.languages.markup.tag.addInlined('script', 'javascript');Prism.languages.markup.tag.addAttribute(/on(?:abort|blur|change|click|composition(?:end|start|update)|dblclick|error|focus(?:in|out)?|key(?:down|up)|load|mouse(?:down|enter|leave|move|out|over|up)|reset|resize|scroll|select|slotchange|submit|unload|wheel)/.source,'javascript');}Prism.languages.js = Prism.languages.javascript;(function (Prism) {var envVars = '\\\\b(?:BASH|BASHOPTS|BASH_ALIASES|BASH_ARGC|BASH_ARGV|BASH_CMDS|BASH_COMPLETION_COMPAT_DIR|BASH_LINENO|BASH_REMATCH|BASH_SOURCE|BASH_VERSINFO|BASH_VERSION|COLORTERM|COLUMNS|COMP_WORDBREAKS|DBUS_SESSION_BUS_ADDRESS|DEFAULTS_PATH|DESKTOP_SESSION|DIRSTACK|DISPLAY|EUID|GDMSESSION|GDM_LANG|GNOME_KEYRING_CONTROL|GNOME_KEYRING_PID|GPG_AGENT_INFO|GROUPS|HISTCONTROL|HISTFILE|HISTFILESIZE|HISTSIZE|HOME|HOSTNAME|HOSTTYPE|IFS|INSTANCE|JOB|LANG|LANGUAGE|LC_ADDRESS|LC_ALL|LC_IDENTIFICATION|LC_MEASUREMENT|LC_MONETARY|LC_NAME|LC_NUMERIC|LC_PAPER|LC_TELEPHONE|LC_TIME|LESSCLOSE|LESSOPEN|LINES|LOGNAME|LS_COLORS|MACHTYPE|MAILCHECK|MANDATORY_PATH|NO_AT_BRIDGE|OLDPWD|OPTERR|OPTIND|ORBIT_SOCKETDIR|OSTYPE|PAPERSIZE|PATH|PIPESTATUS|PPID|PS1|PS2|PS3|PS4|PWD|RANDOM|REPLY|SECONDS|SELINUX_INIT|SESSION|SESSIONTYPE|SESSION_MANAGER|SHELL|SHELLOPTS|SHLVL|SSH_AUTH_SOCK|TERM|UID|UPSTART_EVENTS|UPSTART_INSTANCE|UPSTART_JOB|UPSTART_SESSION|USER|WINDOWID|XAUTHORITY|XDG_CONFIG_DIRS|XDG_CURRENT_DESKTOP|XDG_DATA_DIRS|XDG_GREETER_DATA_DIR|XDG_MENU_PREFIX|XDG_RUNTIME_DIR|XDG_SEAT|XDG_SEAT_PATH|XDG_SESSION_DESKTOP|XDG_SESSION_ID|XDG_SESSION_PATH|XDG_SESSION_TYPE|XDG_VTNR|XMODIFIERS)\\\\b';var commandAfterHeredoc = {pattern: /(^([\"']?)\\w+\\2)[ \\t]+\\S.*/,lookbehind: true,alias: 'punctuation', inside: null };var insideString = {'bash': commandAfterHeredoc,'environment': {pattern: RegExp('\\\\$' + envVars),alias: 'constant'},'variable': [{pattern: /\\$?\\(\\([\\s\\S]+?\\)\\)/,greedy: true,inside: {'variable': [{pattern: /(^\\$\\(\\([\\s\\S]+)\\)\\)/,lookbehind: true},/^\\$\\(\\(/],'number': /\\b0x[\\dA-Fa-f]+\\b|(?:\\b\\d+(?:\\.\\d*)?|\\B\\.\\d+)(?:[Ee]-?\\d+)?/,'operator': /--|\\+\\+|\\*\\*=?|<<=?|>>=?|&&|\\|\\||[=!+\\-*/%<>^&|]=?|[?~:]/,'punctuation': /\\(\\(?|\\)\\)?|,|;/}},{pattern: /\\$\\((?:\\([^)]+\\)|[^()])+\\)|`[^`]+`/,greedy: true,inside: {'variable': /^\\$\\(|^`|\\)$|`$/}},{pattern: /\\$\\{[^}]+\\}/,greedy: true,inside: {'operator': /:[-=?+]?|[!\\/]|##?|%%?|\\^\\^?|,,?/,'punctuation': /[\\[\\]]/,'environment': {pattern: RegExp('(\\\\{)' + envVars),lookbehind: true,alias: 'constant'}}},/\\$(?:\\w+|[#?*!@$])/],'entity': /\\\\(?:[abceEfnrtv\\\\\"]|O?[0-7]{1,3}|U[0-9a-fA-F]{8}|u[0-9a-fA-F]{4}|x[0-9a-fA-F]{1,2})/};Prism.languages.bash = {'shebang': {pattern: /^#!\\s*\\/.*/,alias: 'important'},'comment': {pattern: /(^|[^\"{\\\\$])#.*/,lookbehind: true},'function-name': [{pattern: /(\\bfunction\\s+)[\\w-]+(?=(?:\\s*\\(?:\\s*\\))?\\s*\\{)/,lookbehind: true,alias: 'function'},{pattern: /\\b[\\w-]+(?=\\s*\\(\\s*\\)\\s*\\{)/,alias: 'function'}],'for-or-select': {pattern: /(\\b(?:for|select)\\s+)\\w+(?=\\s+in\\s)/,alias: 'variable',lookbehind: true},'assign-left': {pattern: /(^|[\\s;|&]|[<>]\\()\\w+(?:\\.\\w+)*(?=\\+?=)/,inside: {'environment': {pattern: RegExp('(^|[\\\\s;|&]|[<>]\\\\()' + envVars),lookbehind: true,alias: 'constant'}},alias: 'variable',lookbehind: true},'parameter': {pattern: /(^|\\s)-{1,2}(?:\\w+:[+-]?)?\\w+(?:\\.\\w+)*(?=[=\\s]|$)/,alias: 'variable',lookbehind: true},'string': [{pattern: /((?:^|[^<])<<-?\\s*)(\\w+)\\s[\\s\\S]*?(?:\\r?\\n|\\r)\\2/,lookbehind: true,greedy: true,inside: insideString},{pattern: /((?:^|[^<])<<-?\\s*)([\"'])(\\w+)\\2\\s[\\s\\S]*?(?:\\r?\\n|\\r)\\3/,lookbehind: true,greedy: true,inside: {'bash': commandAfterHeredoc}},{pattern: /(^|[^\\\\](?:\\\\\\\\)*)\"(?:\\\\[\\s\\S]|\\$\\([^)]+\\)|\\$(?!\\()|`[^`]+`|[^\"\\\\`$])*\"/,lookbehind: true,greedy: true,inside: insideString},{pattern: /(^|[^$\\\\])'[^']*'/,lookbehind: true,greedy: true},{pattern: /\\$'(?:[^'\\\\]|\\\\[\\s\\S])*'/,greedy: true,inside: {'entity': insideString.entity}}],'environment': {pattern: RegExp('\\\\$?' + envVars),alias: 'constant'},'variable': insideString.variable,'function': {pattern: /(^|[\\s;|&]|[<>]\\()(?:add|apropos|apt|apt-cache|apt-get|aptitude|aspell|automysqlbackup|awk|basename|bash|bc|bconsole|bg|bzip2|cal|cargo|cat|cfdisk|chgrp|chkconfig|chmod|chown|chroot|cksum|clear|cmp|column|comm|composer|cp|cron|crontab|csplit|curl|cut|date|dc|dd|ddrescue|debootstrap|df|diff|diff3|dig|dir|dircolors|dirname|dirs|dmesg|docker|docker-compose|du|egrep|eject|env|ethtool|expand|expect|expr|fdformat|fdisk|fg|fgrep|file|find|fmt|fold|format|free|fsck|ftp|fuser|gawk|git|gparted|grep|groupadd|groupdel|groupmod|groups|grub-mkconfig|gzip|halt|head|hg|history|host|hostname|htop|iconv|id|ifconfig|ifdown|ifup|import|install|ip|java|jobs|join|kill|killall|less|link|ln|locate|logname|logrotate|look|lpc|lpr|lprint|lprintd|lprintq|lprm|ls|lsof|lynx|make|man|mc|mdadm|mkconfig|mkdir|mke2fs|mkfifo|mkfs|mkisofs|mknod|mkswap|mmv|more|most|mount|mtools|mtr|mutt|mv|nano|nc|netstat|nice|nl|node|nohup|notify-send|npm|nslookup|op|open|parted|passwd|paste|pathchk|ping|pkill|pnpm|podman|podman-compose|popd|pr|printcap|printenv|ps|pushd|pv|quota|quotacheck|quotactl|ram|rar|rcp|reboot|remsync|rename|renice|rev|rm|rmdir|rpm|rsync|scp|screen|sdiff|sed|sendmail|seq|service|sftp|sh|shellcheck|shuf|shutdown|sleep|slocate|sort|split|ssh|stat|strace|su|sudo|sum|suspend|swapon|sync|sysctl|tac|tail|tar|tee|time|timeout|top|touch|tr|traceroute|tsort|tty|umount|uname|unexpand|uniq|units|unrar|unshar|unzip|update-grub|uptime|useradd|userdel|usermod|users|uudecode|uuencode|v|vcpkg|vdir|vi|vim|virsh|vmstat|wait|watch|wc|wget|whereis|which|who|whoami|write|xargs|xdg-open|yarn|yes|zenity|zip|zsh|zypper)(?=$|[)\\s;|&])/,lookbehind: true},'keyword': {pattern: /(^|[\\s;|&]|[<>]\\()(?:case|do|done|elif|else|esac|fi|for|function|if|in|select|then|until|while)(?=$|[)\\s;|&])/,lookbehind: true},'builtin': {pattern: /(^|[\\s;|&]|[<>]\\()(?:\\.|:|alias|bind|break|builtin|caller|cd|command|continue|declare|echo|enable|eval|exec|exit|export|getopts|hash|help|let|local|logout|mapfile|printf|pwd|read|readarray|readonly|return|set|shift|shopt|source|test|times|trap|type|typeset|ulimit|umask|unalias|unset)(?=$|[)\\s;|&])/,lookbehind: true,alias: 'class-name'},'boolean': {pattern: /(^|[\\s;|&]|[<>]\\()(?:false|true)(?=$|[)\\s;|&])/,lookbehind: true},'file-descriptor': {pattern: /\\B&\\d\\b/,alias: 'important'},'operator': {pattern: /\\d?<>|>\\||\\+=|=[=~]?|!=?|<<[<-]?|[&\\d]?>>|\\d[<>]&?|[<>][&=]?|&[>&]?|\\|[&|]?/,inside: {'file-descriptor': {pattern: /^\\d/,alias: 'important'}}},'punctuation': /\\$?\\(\\(?|\\)\\)?|\\.\\.|[{}[\\];\\\\]/,'number': {pattern: /(^|\\s)(?:[1-9]\\d*|0)(?:[.,]\\d+)?\\b/,lookbehind: true}};commandAfterHeredoc.inside = Prism.languages.bash;/* Patterns in command substitution. */var toBeCopied = ['comment','function-name','for-or-select','assign-left','parameter','string','environment','function','keyword','builtin','boolean','file-descriptor','operator','punctuation','number'];var inside = insideString.variable[1].inside;for (var i = 0; i < toBeCopied.length; i++) {inside[toBeCopied[i]] = Prism.languages.bash[toBeCopied[i]];}Prism.languages.sh = Prism.languages.bash;Prism.languages.shell = Prism.languages.bash;}(Prism));(function () {if (typeof Prism === 'undefined' || typeof document === 'undefined') {return;}/*** Plugin name which is used as a class name for <pre> which is activating the plugin** @type {string}*/var PLUGIN_NAME = 'line-numbers';/*** Regular expression used for determining line breaks** @type {RegExp}*/var NEW_LINE_EXP = /\\n(?!$)/g;/*** Global exports*/var config = Prism.plugins.lineNumbers = {/*** Get node for provided line number** @param {Element} element pre element* @param {number} number line number* @returns {Element|undefined}*/getLine: function (element, number) {if (element.tagName !== 'PRE' || !element.classList.contains(PLUGIN_NAME)) {return;}var lineNumberRows = element.querySelector('.line-numbers-rows');if (!lineNumberRows) {return;}var lineNumberStart = parseInt(element.getAttribute('data-start'), 10) || 1;var lineNumberEnd = lineNumberStart + (lineNumberRows.children.length - 1);if (number < lineNumberStart) {number = lineNumberStart;}if (number > lineNumberEnd) {number = lineNumberEnd;}var lineIndex = number - lineNumberStart;return lineNumberRows.children[lineIndex];},/*** Resizes the line numbers of the given element.** This function will not add line numbers. It will only resize existing ones.** @param {HTMLElement} element A `<pre>` element with line numbers.* @returns {void}*/resize: function (element) {resizeElements([element]);},/*** Whether the plugin can assume that the units font sizes and margins are not depended on the size of* the current viewport.** Setting this to `true` will allow the plugin to do certain optimizations for better performance.** Set this to `false` if you use any of the following CSS units: `vh`, `vw`, `vmin`, `vmax`.** @type {boolean}*/assumeViewportIndependence: true};/*** Resizes the given elements.** @param {HTMLElement[]} elements*/function resizeElements(elements) {elements = elements.filter(function (e) {var codeStyles = getStyles(e);var whiteSpace = codeStyles['white-space'];return whiteSpace === 'pre-wrap' || whiteSpace === 'pre-line';});if (elements.length == 0) {return;}var infos = elements.map(function (element) {var codeElement = element.querySelector('code');var lineNumbersWrapper = element.querySelector('.line-numbers-rows');if (!codeElement || !lineNumbersWrapper) {return undefined;}/** @type {HTMLElement} */var lineNumberSizer = element.querySelector('.line-numbers-sizer');var codeLines = codeElement.textContent.split(NEW_LINE_EXP);if (!lineNumberSizer) {lineNumberSizer = document.createElement('span');lineNumberSizer.className = 'line-numbers-sizer';codeElement.appendChild(lineNumberSizer);}lineNumberSizer.innerHTML = '0';lineNumberSizer.style.display = 'block';var oneLinerHeight = lineNumberSizer.getBoundingClientRect().height;lineNumberSizer.innerHTML = '';return {element: element,lines: codeLines,lineHeights: [],oneLinerHeight: oneLinerHeight,sizer: lineNumberSizer,};}).filter(Boolean);infos.forEach(function (info) {var lineNumberSizer = info.sizer;var lines = info.lines;var lineHeights = info.lineHeights;var oneLinerHeight = info.oneLinerHeight;lineHeights[lines.length - 1] = undefined;lines.forEach(function (line, index) {if (line && line.length > 1) {var e = lineNumberSizer.appendChild(document.createElement('span'));e.style.display = 'block';e.textContent = line;} else {lineHeights[index] = oneLinerHeight;}});});infos.forEach(function (info) {var lineNumberSizer = info.sizer;var lineHeights = info.lineHeights;var childIndex = 0;for (var i = 0; i < lineHeights.length; i++) {if (lineHeights[i] === undefined) {lineHeights[i] = lineNumberSizer.children[childIndex++].getBoundingClientRect().height;}}});infos.forEach(function (info) {var lineNumberSizer = info.sizer;var wrapper = info.element.querySelector('.line-numbers-rows');lineNumberSizer.style.display = 'none';lineNumberSizer.innerHTML = '';info.lineHeights.forEach(function (height, lineNumber) {wrapper.children[lineNumber].style.height = height + 'px';});});}/*** Returns style declarations for the element** @param {Element} element*/function getStyles(element) {if (!element) {return null;}return window.getComputedStyle ? getComputedStyle(element) : (element.currentStyle || null);}var lastWidth = undefined;window.addEventListener('resize', function () {if (config.assumeViewportIndependence && lastWidth === window.innerWidth) {return;}lastWidth = window.innerWidth;resizeElements(Array.prototype.slice.call(document.querySelectorAll('pre.' + PLUGIN_NAME)));});Prism.hooks.add('complete', function (env) {if (!env.code) {return;}var code = /** @type {Element} */ (env.element);var pre = /** @type {HTMLElement} */ (code.parentNode);if (!pre || !/pre/i.test(pre.nodeName)) {return;}if (code.querySelector('.line-numbers-rows')) {return;}if (!Prism.util.isActive(code, PLUGIN_NAME)) {return;}code.classList.remove(PLUGIN_NAME);pre.classList.add(PLUGIN_NAME);var match = env.code.match(NEW_LINE_EXP);var linesNum = match ? match.length + 1 : 1;var lineNumbersWrapper;var lines = new Array(linesNum + 1).join('<span></span>');lineNumbersWrapper = document.createElement('span');lineNumbersWrapper.setAttribute('aria-hidden', 'true');lineNumbersWrapper.className = 'line-numbers-rows';lineNumbersWrapper.innerHTML = lines;if (pre.hasAttribute('data-start')) {pre.style.counterReset = 'linenumber ' + (parseInt(pre.getAttribute('data-start'), 10) - 1);}env.element.appendChild(lineNumbersWrapper);resizeElements([pre]);Prism.hooks.run('line-numbers', env);});Prism.hooks.add('line-numbers', function (env) {env.plugins = env.plugins || {};env.plugins.lineNumbers = true;});}());(function () {if (typeof Prism === 'undefined' || typeof document === 'undefined' || !document.createRange) {return;}Prism.plugins.KeepMarkup = true;Prism.hooks.add('before-highlight', function (env) {if (!env.element.children.length) {return;}if (!Prism.util.isActive(env.element, 'keep-markup', true)) {return;}var dropTokens = Prism.util.isActive(env.element, 'drop-tokens', false);/*** Returns whether the given element should be kept.** @param {HTMLElement} element* @returns {boolean}*/function shouldKeep(element) {if (dropTokens && element.nodeName.toLowerCase() === 'span' && element.classList.contains('token')) {return false;}return true;}var pos = 0;var data = [];function processElement(element) {if (!shouldKeep(element)) {processChildren(element);return;}var o = {element: element,posOpen: pos};data.push(o);processChildren(element);o.posClose = pos;}function processChildren(element) {for (var i = 0, l = element.childNodes.length; i < l; i++) {var child = element.childNodes[i];if (child.nodeType === 1) { processElement(child);} else if (child.nodeType === 3) { pos += child.data.length;}}}processChildren(env.element);if (data.length) {env.keepMarkup = data;}});Prism.hooks.add('after-highlight', function (env) {if (env.keepMarkup && env.keepMarkup.length) {var walk = function (elt, nodeState) {for (var i = 0, l = elt.childNodes.length; i < l; i++) {var child = elt.childNodes[i];if (child.nodeType === 1) { if (!walk(child, nodeState)) {return false;}} else if (child.nodeType === 3) { if (!nodeState.nodeStart && nodeState.pos + child.data.length > nodeState.node.posOpen) {nodeState.nodeStart = child;nodeState.nodeStartPos = nodeState.node.posOpen - nodeState.pos;}if (nodeState.nodeStart && nodeState.pos + child.data.length >= nodeState.node.posClose) {nodeState.nodeEnd = child;nodeState.nodeEndPos = nodeState.node.posClose - nodeState.pos;}nodeState.pos += child.data.length;}if (nodeState.nodeStart && nodeState.nodeEnd) {var range = document.createRange();range.setStart(nodeState.nodeStart, nodeState.nodeStartPos);range.setEnd(nodeState.nodeEnd, nodeState.nodeEndPos);nodeState.node.element.innerHTML = '';nodeState.node.element.appendChild(range.extractContents());range.insertNode(nodeState.node.element);range.detach();return false;}}return true;};env.keepMarkup.forEach(function (node) {walk(env.element, {node: node,pos: 0});});env.highlightedCode = env.element.innerHTML;}});}());(function () {if (typeof Prism === 'undefined' || typeof document === 'undefined') {return;}var CLASS_PATTERN = /(?:^|\\s)command-line(?:\\s|$)/;var PROMPT_CLASS = 'command-line-prompt';/** @type {(str: string, prefix: string) => boolean} */var startsWith = ''.startsWith? function (s, p) { return s.startsWith(p); }: function (s, p) { return s.indexOf(p) === 0; };/** @type {(str: string, suffix: string) => boolean} */var endsWith = ''.endsWith? function (str, suffix) {return str.endsWith(suffix);}: function (str, suffix) {var len = str.length;return str.substring(len - suffix.length, len) === suffix;};/*** Returns whether the given hook environment has a command line info object.** @param {any} env* @returns {boolean}*/function hasCommandLineInfo(env) {var vars = env.vars = env.vars || {};return 'command-line' in vars;}/*** Returns the command line info object from the given hook environment.** @param {any} env* @returns {CommandLineInfo}** @typedef CommandLineInfo* @property {boolean} [complete]* @property {number} [numberOfLines]* @property {string[]} [outputLines]*/function getCommandLineInfo(env) {var vars = env.vars = env.vars || {};return vars['command-line'] = vars['command-line'] || {};}Prism.hooks.add('before-highlight', function (env) {var commandLine = getCommandLineInfo(env);if (commandLine.complete || !env.code) {commandLine.complete = true;return;}var pre = env.element.parentElement;if (!pre || !/pre/i.test(pre.nodeName) || (!CLASS_PATTERN.test(pre.className) && !CLASS_PATTERN.test(env.element.className))) {commandLine.complete = true;return;}var existingPrompt = env.element.querySelector('.' + PROMPT_CLASS);if (existingPrompt) {existingPrompt.remove();}var codeLines = env.code.split('\\n');commandLine.numberOfLines = codeLines.length;/** @type {string[]} */var outputLines = commandLine.outputLines = [];var outputSections = pre.getAttribute('data-output');var outputFilter = pre.getAttribute('data-filter-output');if (outputSections !== null) { outputSections.split(',').forEach(function (section) {var range = section.split('-');var outputStart = parseInt(range[0], 10);var outputEnd = range.length === 2 ? parseInt(range[1], 10) : outputStart;if (!isNaN(outputStart) && !isNaN(outputEnd)) {if (outputStart < 1) {outputStart = 1;}if (outputEnd > codeLines.length) {outputEnd = codeLines.length;}outputStart--;outputEnd--;for (var j = outputStart; j <= outputEnd; j++) {outputLines[j] = codeLines[j];codeLines[j] = '';}}});} else if (outputFilter) { for (var i = 0; i < codeLines.length; i++) {if (startsWith(codeLines[i], outputFilter)) { outputLines[i] = codeLines[i].slice(outputFilter.length);codeLines[i] = '';}}}var continuationLineIndicies = commandLine.continuationLineIndicies = new Set();var lineContinuationStr = pre.getAttribute('data-continuation-str');var continuationFilter = pre.getAttribute('data-filter-continuation');for (var j = 0; j < codeLines.length; j++) {var line = codeLines[j];if (!line) {continue;}if (lineContinuationStr && endsWith(line, lineContinuationStr)) {continuationLineIndicies.add(j + 1);}if (j > 0 && continuationFilter && startsWith(line, continuationFilter)) {codeLines[j] = line.slice(continuationFilter.length);continuationLineIndicies.add(j);}}env.code = codeLines.join('\\n');});Prism.hooks.add('before-insert', function (env) {var commandLine = getCommandLineInfo(env);if (commandLine.complete) {return;}var codeLines = env.highlightedCode.split('\\n');var outputLines = commandLine.outputLines || [];for (var i = 0, l = codeLines.length; i < l; i++) {if (outputLines.hasOwnProperty(i)) {codeLines[i] = '<span class=\"token output\">'+ Prism.util.encode(outputLines[i]) + '</span>';} else {codeLines[i] = '<span class=\"token command\">'+ codeLines[i] + '</span>';}}env.highlightedCode = codeLines.join('\\n');});Prism.hooks.add('complete', function (env) {if (!hasCommandLineInfo(env)) {return;}var commandLine = getCommandLineInfo(env);if (commandLine.complete) {return;}var pre = env.element.parentElement;if (CLASS_PATTERN.test(env.element.className)) { env.element.className = env.element.className.replace(CLASS_PATTERN, ' ');}if (!CLASS_PATTERN.test(pre.className)) { pre.className += ' command-line';}function getAttribute(key, defaultValue) {return (pre.getAttribute(key) || defaultValue).replace(/\"/g, '&quot');}var promptLines = '';var rowCount = commandLine.numberOfLines || 0;var promptText = getAttribute('data-prompt', '');var promptLine;if (promptText !== '') {promptLine = '<span data-prompt=\"' + promptText + '\"></span>';} else {var user = getAttribute('data-user', 'user');var host = getAttribute('data-host', 'localhost');promptLine = '<span data-user=\"' + user + '\" data-host=\"' + host + '\"></span>';}var continuationLineIndicies = commandLine.continuationLineIndicies || new Set();var continuationPromptText = getAttribute('data-continuation-prompt', '>');var continuationPromptLine = '<span data-continuation-prompt=\"' + continuationPromptText + '\"></span>';for (var j = 0; j < rowCount; j++) {if (continuationLineIndicies.has(j)) {promptLines += continuationPromptLine;} else {promptLines += promptLine;}}var prompt = document.createElement('span');prompt.className = PROMPT_CLASS;prompt.innerHTML = promptLines;var outputLines = commandLine.outputLines || [];for (var i = 0, l = outputLines.length; i < l; i++) {if (outputLines.hasOwnProperty(i)) {var node = prompt.children[i];node.removeAttribute('data-user');node.removeAttribute('data-host');node.removeAttribute('data-prompt');}}env.element.insertBefore(prompt, env.element.firstChild);commandLine.complete = true;});}());(function () {if (typeof Prism === 'undefined' || typeof document === 'undefined') {return;}if (!Element.prototype.matches) {Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;}Prism.plugins.UnescapedMarkup = true;Prism.hooks.add('before-highlightall', function (env) {env.selector += ', [class*=\"lang-\"] script[type=\"text/plain\"]'+ ', [class*=\"language-\"] script[type=\"text/plain\"]'+ ', script[type=\"text/plain\"][class*=\"lang-\"]'+ ', script[type=\"text/plain\"][class*=\"language-\"]';});Prism.hooks.add('before-sanity-check', function (env) {/** @type {HTMLElement} */var element = env.element;if (element.matches('script[type=\"text/plain\"]')) {var code = document.createElement('code');var pre = document.createElement('pre');pre.className = code.className = element.className;var dataset = element.dataset;Object.keys(dataset || {}).forEach(function (key) {if (Object.prototype.hasOwnProperty.call(dataset, key)) {pre.dataset[key] = dataset[key];}});code.textContent = env.code = env.code.replace(/&lt;\\/script(?:>|&gt;)/gi, '</scri' + 'pt>');pre.appendChild(code);element.parentNode.replaceChild(pre, element);env.element = code;return;}if (!env.code) {var childNodes = element.childNodes;if (childNodes.length === 1 && childNodes[0].nodeName == '#comment') {element.textContent = env.code = childNodes[0].textContent;}}});}());(function () {if (typeof Prism === 'undefined') {return;}var assign = Object.assign || function (obj1, obj2) {for (var name in obj2) {if (obj2.hasOwnProperty(name)) {obj1[name] = obj2[name];}}return obj1;};function NormalizeWhitespace(defaults) {this.defaults = assign({}, defaults);}function toCamelCase(value) {return value.replace(/-(\\w)/g, function (match, firstChar) {return firstChar.toUpperCase();});}function tabLen(str) {var res = 0;for (var i = 0; i < str.length; ++i) {if (str.charCodeAt(i) == '\\t'.charCodeAt(0)) {res += 3;}}return str.length + res;}var settingsConfig = {'remove-trailing': 'boolean','remove-indent': 'boolean','left-trim': 'boolean','right-trim': 'boolean','break-lines': 'number','indent': 'number','remove-initial-line-feed': 'boolean','tabs-to-spaces': 'number','spaces-to-tabs': 'number',};NormalizeWhitespace.prototype = {setDefaults: function (defaults) {this.defaults = assign(this.defaults, defaults);},normalize: function (input, settings) {settings = assign(this.defaults, settings);for (var name in settings) {var methodName = toCamelCase(name);if (name !== 'normalize' && methodName !== 'setDefaults' &&settings[name] && this[methodName]) {input = this[methodName].call(this, input, settings[name]);}}return input;},/** Normalization methods*/leftTrim: function (input) {return input.replace(/^\\s+/, '');},rightTrim: function (input) {return input.replace(/\\s+$/, '');},tabsToSpaces: function (input, spaces) {spaces = spaces|0 || 4;return input.replace(/\\t/g, new Array(++spaces).join(' '));},spacesToTabs: function (input, spaces) {spaces = spaces|0 || 4;return input.replace(RegExp(' {' + spaces + '}', 'g'), '\\t');},removeTrailing: function (input) {return input.replace(/\\s*?$/gm, '');},removeInitialLineFeed: function (input) {return input.replace(/^(?:\\r?\\n|\\r)/, '');},removeIndent: function (input) {var indents = input.match(/^[^\\S\\n\\r]*(?=\\S)/gm);if (!indents || !indents[0].length) {return input;}indents.sort(function (a, b) { return a.length - b.length; });if (!indents[0].length) {return input;}return input.replace(RegExp('^' + indents[0], 'gm'), '');},indent: function (input, tabs) {return input.replace(/^[^\\S\\n\\r]*(?=\\S)/gm, new Array(++tabs).join('\\t') + '$&');},breakLines: function (input, characters) {characters = (characters === true) ? 80 : characters|0 || 80;var lines = input.split('\\n');for (var i = 0; i < lines.length; ++i) {if (tabLen(lines[i]) <= characters) {continue;}var line = lines[i].split(/(\\s+)/g);var len = 0;for (var j = 0; j < line.length; ++j) {var tl = tabLen(line[j]);len += tl;if (len > characters) {line[j] = '\\n' + line[j];len = tl;}}lines[i] = line.join('');}return lines.join('\\n');}};if (typeof module !== 'undefined' && module.exports) {module.exports = NormalizeWhitespace;}Prism.plugins.NormalizeWhitespace = new NormalizeWhitespace({'remove-trailing': true,'remove-indent': true,'left-trim': true,'right-trim': true,/*'break-lines': 80,'indent': 2,'remove-initial-line-feed': false,'tabs-to-spaces': 4,'spaces-to-tabs': 4*/});Prism.hooks.add('before-sanity-check', function (env) {var Normalizer = Prism.plugins.NormalizeWhitespace;if (env.settings && env.settings['whitespace-normalization'] === false) {return;}if (!Prism.util.isActive(env.element, 'whitespace-normalization', true)) {return;}if ((!env.element || !env.element.parentNode) && env.code) {env.code = Normalizer.normalize(env.code, env.settings);return;}var pre = env.element.parentNode;if (!env.code || !pre || pre.nodeName.toLowerCase() !== 'pre') {return;}if (env.settings == null) { env.settings = {}; }for (var key in settingsConfig) {if (Object.hasOwnProperty.call(settingsConfig, key)) {var settingType = settingsConfig[key];if (pre.hasAttribute('data-' + key)) {try {var value = JSON.parse(pre.getAttribute('data-' + key) || 'true');if (typeof value === settingType) {env.settings[key] = value;}} catch (_error) {}}}}var children = pre.childNodes;var before = '';var after = '';var codeFound = false;for (var i = 0; i < children.length; ++i) {var node = children[i];if (node == env.element) {codeFound = true;} else if (node.nodeName === '#text') {if (codeFound) {after += node.nodeValue;} else {before += node.nodeValue;}pre.removeChild(node);--i;}}if (!env.element.children.length || !Prism.plugins.KeepMarkup) {env.code = before + env.code + after;env.code = Normalizer.normalize(env.code, env.settings);} else {var html = before + env.element.innerHTML + after;env.element.innerHTML = Normalizer.normalize(html, env.settings);env.code = env.element.textContent;}});}());(function () {if (typeof Prism === 'undefined' || typeof document === 'undefined') {return;}function mapClassName(name) {var customClass = Prism.plugins.customClass;if (customClass) {return customClass.apply(name, 'none');} else {return name;}}var PARTNER = {'(': ')','[': ']','{': '}',};var NAMES = {'(': 'brace-round','[': 'brace-square','{': 'brace-curly',};var BRACE_ALIAS_MAP = {'${': '{', };var LEVEL_WARP = 12;var pairIdCounter = 0;var BRACE_ID_PATTERN = /^(pair-\\d+-)(close|open)$/;/*** Returns the brace partner given one brace of a brace pair.** @param {HTMLElement} brace* @returns {HTMLElement}*/function getPartnerBrace(brace) {var match = BRACE_ID_PATTERN.exec(brace.id);return document.querySelector('#' + match[1] + (match[2] == 'open' ? 'close' : 'open'));}/*** @this {HTMLElement}*/function hoverBrace() {if (!Prism.util.isActive(this, 'brace-hover', true)) {return;}[this, getPartnerBrace(this)].forEach(function (e) {e.classList.add(mapClassName('brace-hover'));});}/*** @this {HTMLElement}*/function leaveBrace() {[this, getPartnerBrace(this)].forEach(function (e) {e.classList.remove(mapClassName('brace-hover'));});}/*** @this {HTMLElement}*/function clickBrace() {if (!Prism.util.isActive(this, 'brace-select', true)) {return;}[this, getPartnerBrace(this)].forEach(function (e) {e.classList.add(mapClassName('brace-selected'));});}Prism.hooks.add('complete', function (env) {/** @type {HTMLElement} */var code = env.element;var pre = code.parentElement;if (!pre || pre.tagName != 'PRE') {return;}/** @type {string[]} */var toMatch = [];if (Prism.util.isActive(code, 'match-braces')) {toMatch.push('(', '[', '{');}if (toMatch.length == 0) {return;}if (!pre.__listenerAdded) {pre.addEventListener('mousedown', function removeBraceSelected() {var code = pre.querySelector('code');var className = mapClassName('brace-selected');Array.prototype.slice.call(code.querySelectorAll('.' + className)).forEach(function (e) {e.classList.remove(className);});});Object.defineProperty(pre, '__listenerAdded', { value: true });}/** @type {HTMLSpanElement[]} */var punctuation = Array.prototype.slice.call(code.querySelectorAll('span.' + mapClassName('token') + '.' + mapClassName('punctuation')));/** @type {{ index: number, open: boolean, element: HTMLElement }[]} */var allBraces = [];toMatch.forEach(function (open) {var close = PARTNER[open];var name = mapClassName(NAMES[open]);/** @type {[number, number][]} */var pairs = [];/** @type {number[]} */var openStack = [];for (var i = 0; i < punctuation.length; i++) {var element = punctuation[i];if (element.childElementCount == 0) {var text = element.textContent;text = BRACE_ALIAS_MAP[text] || text;if (text === open) {allBraces.push({ index: i, open: true, element: element });element.classList.add(name);element.classList.add(mapClassName('brace-open'));openStack.push(i);} else if (text === close) {allBraces.push({ index: i, open: false, element: element });element.classList.add(name);element.classList.add(mapClassName('brace-close'));if (openStack.length) {pairs.push([i, openStack.pop()]);}}}}pairs.forEach(function (pair) {var pairId = 'pair-' + (pairIdCounter++) + '-';var opening = punctuation[pair[0]];var closing = punctuation[pair[1]];opening.id = pairId + 'open';closing.id = pairId + 'close';[opening, closing].forEach(function (e) {e.addEventListener('mouseenter', hoverBrace);e.addEventListener('mouseleave', leaveBrace);e.addEventListener('click', clickBrace);});});});var level = 0;allBraces.sort(function (a, b) { return a.index - b.index; });allBraces.forEach(function (brace) {if (brace.open) {brace.element.classList.add(mapClassName('brace-level-' + (level % LEVEL_WARP + 1)));level++;} else {level = Math.max(0, level - 1);brace.element.classList.add(mapClassName('brace-level-' + (level % LEVEL_WARP + 1)));}});});}());(function () {if (typeof Prism === 'undefined') {return;}Prism.languages.treeview = {'treeview-part': {pattern: /^.+/m,inside: {'entry-line': [{pattern: /\\|-- |├── /,alias: 'line-h'},{pattern: /\\| {3}|│ {3}/,alias: 'line-v'},{pattern: /`-- |└── /,alias: 'line-v-last'},{pattern: / {4}/,alias: 'line-v-gap'}],'entry-name': {pattern: /.*\\S.*/,inside: {'operator': / -> /,}}}}};Prism.hooks.add('wrap', function (env) {if (env.language === 'treeview' && env.type === 'entry-name') {var classes = env.classes;var folderPattern = /(^|[^\\\\])\\/\\s*$/;if (folderPattern.test(env.content)) {env.content = env.content.replace(folderPattern, '$1');classes.push('dir');} else {env.content = env.content.replace(/(^|[^\\\\])[=*|]\\s*$/, '$1');var parts = env.content.toLowerCase().replace(/\\s+/g, '').split('.');while (parts.length > 1) {parts.shift();classes.push('ext-' + parts.join('-'));}}if (env.content[0] === '.') {classes.push('dotfile');}}});}());</script>";

                diffHtml = HtmlCleanup(diffHtml);

                string SbySide = SidebySide(diffHtml);

                report = HtmlReport(diffHtml);
                Dictionary<string, int> summaryRpt = new() {
                    { "Modification", 0},
                    { "Deletion", 0},
                    { "Insertion", 0}
                };
                if (report.Count > 0)
                {
                    int modR = 0, delR = 0, insR = 0;
                    foreach (var line in report)
                    {
                        if (line.resultValue.Contains("<del") & line.resultValue.Contains("<ins"))
                        {
                            modR++;
                            tableRow += $"<tr class=\"mod-row\" id=\"mod-row{modR}\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Modification"] += 1;
                        }
                        else if (line.resultValue.Contains("<del"))
                        {
                            delR++;
                            tableRow += $"<tr class=\"del-row\" id=\"del-row{delR}\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Deletion"] += 1;
                        }
                        else
                        {
                            insR++;
                            tableRow += $"<tr class=\"ins-row\" id=\"ins-row{insR}\"><td><pre><code class=\"language-xml\">{line.sourceValue}</code></td><td><pre><code class=\"language-xml\">{line.destValue}</code></pre></td><td><pre><code class=\"language-xml\">{line.resultValue}</code></pre></td></tr>";
                            summaryRpt["Insertion"] += 1;
                        }
                    }
                    tableBody = $"<table class=\"table\" id=\"resultTable\"><thead><tr><th>Previous Version XML</th><th>New Version XML</th><th>Comparison Result</th></tr></thead><tbody>{tableRow}</tbody></table>";
                }
                else
                {
                    tableRow = $"<tr class=\"idn-head-row\"><td colspan=\"3\" style=\"text-align: center\"><b>Both the XML files are identical!</b></td></tr>";
                    tableBody = $"<table class=\"table\" id=\"resultTable\"><thead><tr><th>Previous Version XML</th><th>New Version XML</th><th>Comparison Result</th></tr></thead><tbody>{tableRow}</tbody></table>";
                }

                int maxR = Math.Max(Math.Max(summaryRpt["Insertion"], summaryRpt["Deletion"]), summaryRpt["Modification"]);

                string htmltemplate = $"<!DOCTYPE html><html lang=\"en\"><head><title>XML Comparison</title><meta charset=\"utf-8\"/><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" /><link rel=\"stylesheet\" href=\"https://pro.fontawesome.com/releases/v5.12.1/css/all.css\" /><link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css\" /><link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/css/bootstrap-multiselect.css\" />{cssCommon}{cssPrism}</head><body><nav class=\"navbar customNav navbar-light bg-blue py-1\"><a class=\"navbar-brand py-0\" href=\"#\"><svg class=\"logoSvg mr-2\" id=\"Component_20_1\" data-name=\"Component 20 – 1\" xmlns=\"http://www.w3.org/2000/svg\" width=\"28\" height=\"28\" viewBox=\"0 0 32.141 32\"><path id=\"Union_8\" data-name=\"Union 8\"d=\"M35.12,52.828l-.047-.049a4.254,4.254,0,0,1,.106-6.037l4.267-4.1H27.461a4.3,4.3,0,0,1-3.789-1.184l-.048-.048a4.254,4.254,0,0,1,0-6.038L35.437,23.619a4.308,4.308,0,0,1,6.07,0l.048.048a4.254,4.254,0,0,1,0,6.038L37.2,34.034H49.614a4.3,4.3,0,0,1,3.65,1.267l.047.049a4.318,4.318,0,0,1-.483,6.4L41.188,52.933a4.308,4.308,0,0,1-6.069-.105Z\"transform=\"translate(-22.367 -22.249)\" fill=\"#fa6e2e\" opacity=\"0.8\" /><path id=\"Union_9\" data-name=\"Union 9\"d=\"M15.522,32a4.286,4.286,0,1,1,.076,0ZM27.417,20.471,17.24,20.3l-.129,0L24.6,12.988a4.154,4.154,0,0,1,7.138,3.043l0,.229a4.273,4.273,0,0,1-4.214,4.217Zm-25.584-.69A4.3,4.3,0,0,1,0,16.251v-.228a4.274,4.274,0,0,1,4.14-4.29l.1,0,10.178-.005h.129L7.189,19.168a4.292,4.292,0,0,1-5.356.613ZM11.645,4.326a4.288,4.288,0,1,1,4.288,4.326,4.307,4.307,0,0,1-4.288-4.326Z\"transform=\"translate(0.221 0)\" fill=\"#ff5000\" /></svg><span>Meta Data Compare</span></a></nav><div class=\"container-fluid mt-2\"><div class=\"row xmlHeadrColr\"><div class=\"col-md-6\"><p class=\"rightXmlCom\">Client: <b>{Client}</b> | Comparison: <b>{OldXML + "</b> vs <b>" + NewXML}</b> | Date Time: <b>{DateTime.Now}</b> </p></div><div class=\"col-md-6\"><div class=\"filterxml\"><div class=\"radiobuttons\"><div class=\"rdio rdio-primary radio-inline\"><input name=\"radio\" value=\"1\" id=\"radio1\" type=\"radio\"><label for=\"radio1\">Side-by-Side View</label></div><div class=\"rdio rdio-primary radio-\"><input name=\"radio\" value=\"2\" id=\"radio2\" type=\"radio\" checked><label for=\"radio2\">Smart View</label></div><div class=\"position-relative filterPart\"><div class=\"filterSvgIcon\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"21.5\" height=\"21.194\" viewBox=\"0 0 28 28\"><path data-name=\"filter\" d=\"M1.861.75h17.778a1.111 1.111 0 0 1 1.111 1.111v1.762a1.111 1.111 0 0 1-.325.785l-7.128 7.127a1.111 1.111 0 0 0-.324.786v7.006a1.111 1.111 0 0 1-1.381 1.078l-2.223-.555a1.111 1.111 0 0 1-.841-1.078v-6.45a1.111 1.111 0 0 0-.325-.785L1.074 4.409a1.111 1.111 0 0 1-.324-.786V1.861A1.111 1.111 0 0 1 1.861.75Z\" fill=\"none\" stroke=\"#fff\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"1.5\"></path></svg></div><select id=\"framework\" name=\"framework[]\" multiple class=\"form-control\"><option value=\"ins-row\">Inserted</option><option value=\"del-row\">Deleted</option><option value=\"mod-row\">Modified</option></select></div><div class=\"searchoptions\"><div class=\"input-group\"><input id=\"srchTxt\" type=\"text\" class=\"form-control\" placeholder=\"Search...\" aria-describedby=\"basic-addon2\"><div class=\"input-group-append\"><span class=\"input-group-text\"><svg xmlns=\"http://www.w3.org/2000/svg\" class=\"searchSvg whiteSvg\" width=\"18\" height=\"18\" viewBox=\"0 0 28 28\"><g id=\"search\" transform=\"translate(0.75 0.75)\"><path id=\"search-2\" data-name=\"search\" d=\"M20,20l5,5M5,13.571A8.571,8.571,0,1,0,13.571,5,8.571,8.571,0,0,0,5,13.571Z\" transform=\"translate(-5 -5)\" fill=\"none\" stroke=\"#fff\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"1.5\"></path></g></svg></span></div></div></div><ul class=\"iconsLis\"><li id=\"expandRadio\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"18\" height=\"18\" class=\"commonSvg\" viewBox=\"0 0 28 28\"><g data-name=\"expand all\" fill=\"none\" stroke=\"#0078d4\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"1.5\"><g data-name=\"Group 26059\"><path d=\"M.75 7.161V.75h6.411\"></path><path data-name=\"border-tl\" d=\"M19.161 7.161V.75H12.75\"></path></g><g data-name=\"Group 26060\"><path data-name=\"border-tl\" d=\"M.75 12.75v6.411h6.411M19.161 12.75v6.411H12.75\"></path></g></g></svg></li><li id=\"collapseRadio\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"18\" height=\"18\" class=\"commonSvg\" viewBox=\"0 0 28 28\"><g data-name=\"collaps all\" fill=\"none\" stroke=\"#0078d4\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"1.5\"><path d=\"M7.161.75v6.411H.75\"></path><path data-name=\"border-tl\" d=\"M12.75.75v6.411h6.411M7.161 19.161V12.75H.75M12.75 19.161V12.75h6.411\"></path></g></svg></li></ul></div></div></div></div><div class=\"row mt-3\"><div class=\"col-md-12\"><div class=\"xmlCompareTable smartViews table-responsive\">{tableBody}</div><div class=\"xmlCompareTable sideBySide table-responsive\">{SbySide}</div></div></div><div class=\"row footers\"><div class=\"col-md-12 \"><div class=\"row footerParts\"><div class=\"col-md-12 text-right\"><h6>© 2023 | www.straive.com. All rights reserved.</h6><p><b class=\"grn\">Insertion:</b> {summaryRpt["Insertion"]} | <b class=\"dels\">Deletion:</b> {summaryRpt["Deletion"]} | <b class=\"mod\">Modified:</b> {summaryRpt["Modification"]} | <b>Total:</b> {report.Count}</p></div></div></div></div></div><script src=\"https://cdn.jsdelivr.net/npm/jquery@3.6.4/dist/jquery.slim.min.js\"></script><script src=\"https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js\"></script><script src=\"https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js\"></script><script src=\"https://cdnjs.cloudflare.com/ajax/libs/highcharts/9.3.1/highcharts.js\"></script><script src=\"https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/js/bootstrap-multiselect.js\"></script>{jsPrism}{jsCommon}</body></html>";

                //{prismAssets[1]}

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

            if (i == 0)
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
        private static int LineSelectionSuffix(int i, string[] lines, int counter = 0)
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
                return i - 1;
            }
        }
        private static int LineSelectionPrefix(int i, string[] lines, int counter = 0)
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
            //rpt.sourceValue = Regex.Replace(rpt.sourceValue, "<ins([^><]+)>([^><]+)</ins>", "");

            var matchs = Regex.Matches(rpt.sourceValue, @"<ins([^><]+)>([^><]+)</ins>");
            const string pattern = "&templt;br/&tempgt;";
            foreach (Match match in matchs)
            {
                string breakStr = "";
                int count = Regex.Matches(match.Groups[2].Value, pattern, RegexOptions.IgnoreCase).Count;
                if (count > 0)
                {
                    //breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn = new Regex("<ins([^><]+)>([^><]+)</ins>");
                rpt.sourceValue = ptrn.Replace(rpt.sourceValue, breakStr, 1);
            }

            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "<del([^><]+)>([^><]+)</del>", "<span class=\"diff\">$2</span>", RegexOptions.Multiline);
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "(\r\n|\n|\r)", "");
            rpt.sourceValue = Regex.Replace(rpt.sourceValue, "&templt;br/&tempgt;", "<br/>");

            //rpt.destValue = Regex.Replace(rpt.destValue, "<del([^><]+)>([^><]+)</del>", "");

            matchs = Regex.Matches(rpt.destValue, @"<del([^><]+)>([^><]+)</del>");
            foreach (Match match in matchs)
            {
                string breakStr = "";
                int count = Regex.Matches(match.Groups[2].Value, pattern, RegexOptions.IgnoreCase).Count;
                if (count > 0)
                {
                    //breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn = new Regex("<del([^><]+)>([^><]+)</del>");
                rpt.destValue = ptrn.Replace(rpt.destValue, breakStr, 1);
            }

            rpt.destValue = Regex.Replace(rpt.destValue, "<ins([^><]+)>([^><]+)</ins>", "<span class=\"diff\">$2</span>");
            rpt.destValue = Regex.Replace(rpt.destValue, "(\r\n|\n|\r)", "");
            rpt.destValue = Regex.Replace(rpt.destValue, "&templt;br/&tempgt;", "<br/>");

            rpt.resultValue = Regex.Replace(rpt.resultValue, "(\r\n|\n|\r)", "");

            return rpt;
        }
        private static string SidebySide(string rawHtml)
        {
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
                    //breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn1 = new Regex("<ins([^><]+)>([^><]+)</ins>");
                prevXml = ptrn1.Replace(prevXml, breakStr, 1);
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
                    //breakStr = pattern;
                    for (var i = 0; i < count; i++)
                    {
                        breakStr += pattern;
                    }
                }
                Regex ptrn1 = new Regex("<del([^><]+)>([^><]+)</del>");
                destXml = ptrn1.Replace(destXml, breakStr, 1);
            }
            destXml = Regex.Replace(destXml, @"&tmplt;br&tmpgt;", "<br/>");

            prevXml = IndentCleanup(prevXml, "del");
            destXml = IndentCleanup(destXml, "ins");

            HideSimilarities2(prevXml, "del");
            HideSimilarities2(destXml, "ins");

            tableBody = $"<table class=\"table\"><thead><tr><th>Previous Version XML</th><th>New Version XML</th></tr></thead><tbody>{string.Join("\n", trRow)}</tbody></table>";

            return tableBody;
        }
        private static string HideSimilarities(string xCont, string opt)
        {
            Dictionary<int, int> spanDiv = new();
            List<string> inputLines;
            inputLines = xCont.Split("\n").ToList();
            int countr = 0, lncntr, cnt = 0, stLn = 0, edLn = 0;
            lncntr = inputLines.Count;
            while (cnt < lncntr)
            {
                if (inputLines[cnt] != null && inputLines[cnt] != string.Empty)
                {
                    if (Regex.IsMatch(inputLines[cnt], $"<{opt} "))
                    {
                        edLn = cnt - 3;

                        countr = 0;
                        if (!Regex.IsMatch(inputLines[cnt], $"</{opt}>"))
                        {
                            while (true)
                            {
                                cnt++;
                                if (Regex.IsMatch(inputLines[cnt], $"</{opt}>"))
                                {
                                    break;
                                }
                                if (cnt == lncntr - 1)
                                {
                                    edLn = cnt;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        countr++;
                        if (countr == 1)
                        {
                            stLn = cnt;
                        }
                        if (cnt == lncntr - 1)
                        {
                            edLn = cnt;
                        }
                    }
                    if (edLn - stLn > 6)
                    {
                        spanDiv.Add(stLn + 2, edLn);
                        stLn = edLn;
                    }
                }
                cnt++;
            }

            bool extensionOn = false;
            for (int x = 0; x < inputLines.Count - 1; x++)
            {
                if (inputLines[x].Contains($"<{opt} ") && !inputLines[x].Contains($"</{opt}>"))
                {
                    extensionOn = true;
                }
                if (extensionOn)
                {
                    inputLines[x] = inputLines[x] + $"</{opt}>";
                    if (opt == "ins")
                        inputLines[x + 1] = $"<ins style=\"background:#e6ffe6;\">" + inputLines[x + 1];
                    else
                        inputLines[x + 1] = $"<del style=\"background:#ffe6e6;\">" + inputLines[x + 1];
                }
                if (inputLines[x + 1].Contains($"</{opt}>") && !inputLines[x + 1].Contains($"<{opt} >"))
                {
                    extensionOn = false;
                }
            }

            inputLines = inputLines.Select((x, i) => $"<span class=\"lnNum\">{i + 1}</span>{x}").ToList();

            foreach (var div in spanDiv.Reverse())
            {
                inputLines.Insert(div.Value, "</div></div>");
                inputLines.Insert(div.Key, "<div class=\"hideDiv\"><div class=\"hideButton collapsed\"></div><div class=\"hideCont\">");
            }
            xCont = string.Join("\n", inputLines);
            xCont = Regex.Replace(xCont, @"\r\n</div>", "</div>");
            xCont = Regex.Replace(xCont, @"</div></div>\n", "</div></div>");
            xCont = xCont.Replace("<div class=\"hideCont\">\n", "<div class=\"hideCont\">");

            return xCont;
        }
        private static void HideSimilarities2(string xCont, string opt)
        {
            bool extensionOn = false;
            List<string> inputLines = xCont.Split("\n").ToList();
            Dictionary<int, int> spanDiv = new Dictionary<int, int>();

            //Extracting the position to place collapse and expand button
            int startAssertValue = 0;
            for (int x = 0; x < inputLines.Count; x++)
            {
                if (startAssertValue == 0 && inputLines[x].Contains($"<{opt} ") && !inputLines[x].Contains($"</{opt}>"))
                {
                    startAssertValue = x;
                }
                if (startAssertValue > 0 && inputLines[x].Contains($"</{opt}>") && !inputLines[x].Contains($"<{opt} >"))
                {
                    if (x > startAssertValue + 5)
                    {
                        spanDiv.Add(startAssertValue, x + 1);
                    }
                    startAssertValue = 0;
                }
            }

            //Replacing the stretched tags to line independent tags
            for (int x = 0; x < inputLines.Count - 1; x++)
            {
                if (inputLines[x].Contains($"<{opt} ") && !inputLines[x].Contains($"</{opt}>"))
                {
                    extensionOn = true;
                }
                if (extensionOn)
                {
                    inputLines[x] = inputLines[x] + $"</{opt}>";
                    if (opt == "ins")
                        inputLines[x + 1] = $"<ins style=\"background:#e6ffe6;\">" + inputLines[x + 1];
                    else
                        inputLines[x + 1] = $"<del style=\"background:#ffe6e6;\">" + inputLines[x + 1];
                }
                if (inputLines[x + 1].Contains($"</{opt}>") && !inputLines[x + 1].Contains($"<{opt} >"))
                {
                    extensionOn = false;
                }
            }
            //Adding line no's
            if (opt == "del")
            {
                trRow = inputLines.Select((x, i) => $"<td id=\"lineNoDel{i + 1}\">{(spanDiv.ContainsKey(i) ? $"<span class=\"lnBtn\" data-start-td=\"lineNoDel{i + 2}\" data-end-td=\"lineNoDel{spanDiv[i]}\">-</span>" : "")}<span class=\"lnNum\">{i + 1}</span><pre><code class=\"language-xml\">" + x + "</code></pre></td>").ToList();
            }
            else
            {
                inputLines = inputLines.Select((x, i) => $"<td id=\"lineNoIns{i + 1}\">{(spanDiv.ContainsKey(i) ? $"<span class=\"lnBtn\" data-start-td=\"lineNoIns{i + 2}\" data-end-td=\"lineNoIns{spanDiv[i]}\">-</span>" : "")}<span class=\"lnNum\">{i + 1}</span><pre><code class=\"language-xml\">" + x + "</code></pre></td>").ToList();
                for (int s = 0; s < inputLines.Count; s++)
                {
                    trRow[s] = "<tr>" + trRow[s] + inputLines[s] + "</tr>";
                }
            }
        }


        private static string IndentCleanup(string xCont, string opt)
        {
            string orgCont;
            orgCont = xCont;
            xCont = xCont.Replace("<br/>", "\n");
            try
            {
                List<string> inputLines;
                Dictionary<string, string> spaces = new();
                Dictionary<string, int> changesOpen = new();
                Dictionary<string, int> changesClose = new();
                List<string> crctnsBefore = new();
                List<string> crctnsAfter = new();

                foreach (Match match in Regex.Matches(orgCont, $"<{opt}[^>]+>([^><]+)</{opt}>").Cast<Match>())
                {
                    crctnsBefore.Add(match.Groups[1].Value);
                }

                orgCont = Regex.Replace(orgCont, "&lt;", "<");
                orgCont = Regex.Replace(orgCont, "&gt;", ">");

                int br = 0;
                while (Regex.IsMatch(orgCont, "(<br/><br/>)+"))
                {
                    br++;
                    spaces.Add("<br" + br + "/>", Regex.Match(orgCont, "(<br/>)((?:<br/>)+)").Groups[0].Value.Substring(10));
                    Regex ptrn = new("(<br/><br/>)+");
                    orgCont = ptrn.Replace(orgCont, "<br" + br + "/>", 1);
                }

                orgCont = Regex.Replace(orgCont, "<br/>", "\n");
                orgCont = Regex.Replace(orgCont, "<br([0-9]+)/>", "\n<br$1/>\n");
                orgCont = Regex.Replace(orgCont, $"<((?:/)?{opt}(?: [^><]+)?)>", "&lt;$1&gt;");
                orgCont = Regex.Replace(orgCont, "<([^><]+)?<(br[0-9]+/)>\n([^><]+)?>", "<$1zzxx$2xxzz$3>");
                orgCont = Regex.Replace(orgCont, "\n(zzxxbr[0-9]+/xxzz)", "$1");
                orgCont = Regex.Replace(orgCont, "&lt;", "<");
                orgCont = Regex.Replace(orgCont, "&gt;", ">");
                orgCont = Regex.Replace(orgCont, $"([^>])\n<{opt}", $"$1<{opt}");

                inputLines = orgCont.Split("\n").ToList();

                int lineCntBefore = inputLines.Count;

                int i = 0;
                while (i < lineCntBefore)
                {
                    if (inputLines[i].IndexOf($"<{opt} ") >= 0)
                    {
                        changesOpen.Add(i + ": " + inputLines[i].IndexOf($"<{opt} "), inputLines[i].IndexOf($"<{opt} "));
                        Regex rgx = new(@$"<{opt}[^>]+>");
                        inputLines[i] = rgx.Replace(inputLines[i], "", 1);
                        if (inputLines[i].IndexOf($"<{opt} ") > 0)
                        {
                            continue;
                        }
                    }
                    i++;
                }

                i = 0;
                while (i < lineCntBefore)
                {
                    if (inputLines[i].IndexOf($"</{opt}>") > 0)
                    {
                        changesClose.Add(i + ": " + inputLines[i].IndexOf($"</{opt}>"), inputLines[i].IndexOf($"</{opt}>"));
                        Regex rgx1 = new(@$"</{opt}>");
                        inputLines[i] = rgx1.Replace(inputLines[i], "", 1);
                        if (inputLines[i].IndexOf($"</{opt}>") > 0)
                        {
                            continue;
                        }
                    }
                    i++;
                }

                orgCont = string.Join("", inputLines);
                orgCont = XDocument.Parse(orgCont).ToString();

                inputLines = orgCont.Split("\n").ToList();


                int sp;
                int lineCntAfter = inputLines.Count;

                if (lineCntBefore != lineCntAfter)
                {
                    Console.WriteLine("Indentation process not supported for this compare report!");
                    return xCont;
                }

                foreach (var ln in changesClose.Reverse())
                {

                    if (inputLines.Count >= int.Parse(Regex.Replace(ln.Key, ": [0-9]+", "")))
                    {
                        sp = inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))].IndexOf("<");
                        sp = sp == -1 ? 0 : sp;
                        inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))] = inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))].Insert(ln.Value + sp, $"</{opt}>");
                    }
                }

                foreach (var ln in changesOpen.Reverse())
                {
                    string styleVal = opt == "del" ? "ffe6e6" : "e6ffe6";
                    if (inputLines.Count >= int.Parse(Regex.Replace(ln.Key, ": [0-9]+", "")))
                    {
                        sp = inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))].IndexOf("<");
                        sp = sp == -1 ? 0 : sp;
                        inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))] = inputLines[int.Parse(Regex.Replace(ln.Key, ": [0-9]+", ""))].Insert(ln.Value + sp, $"<{opt} style=\"background:#{styleVal};\">");
                    }
                }

                foreach (Match match in Regex.Matches(orgCont, $"<{opt}[^>]+>([^><]+)</{opt}>").Cast<Match>())
                {
                    crctnsAfter.Add(match.Groups[1].Value);
                }

                if (crctnsBefore != crctnsAfter)
                {
                    Console.WriteLine("Indentation process not supported for this compare report!");
                    return xCont;
                }

                orgCont = string.Join("\n", inputLines);
                orgCont = Regex.Replace(orgCont, "<", "&lt;");
                orgCont = Regex.Replace(orgCont, ">", "&gt;");
                orgCont = Regex.Replace(orgCont, "zzxx(br[0-9]+/)xxzz", "<$1/>");
                orgCont = Regex.Replace(orgCont, "&lt;(br[0-9]+[ ]?/)&gt;", "<$1>");
                orgCont = Regex.Replace(orgCont, "<(br[0-9]+)[ ]?/>", "<$1/>");
                orgCont = Regex.Replace(orgCont, "<(br[0-9]+)//>", "<$1/><br/>");

                //insert the retained linebreaks to match line number
                foreach (var space in spaces)
                {
                    orgCont = orgCont.Replace(space.Key, space.Value);
                }
                orgCont = orgCont.Replace("<br/>", "\n");
                orgCont = Regex.Replace(orgCont, $"&lt;{opt} style=\"([^>\"]+)\"&gt;", $"<{opt} style=\"$1\">");
                orgCont = orgCont.Replace($"&lt;/{opt}&gt;", $"</{opt}>");

                return orgCont;
            }
            catch (Exception)
            {
                Console.WriteLine("Indentation process not supported for this compare report!");
                return xCont;
            }
        }
    }
}
