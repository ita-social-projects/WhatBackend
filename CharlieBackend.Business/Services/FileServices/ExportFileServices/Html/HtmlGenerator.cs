using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public static class HtmlGenerator
    {
        public static StringBuilder HtmlTemplate { get; }

        static HtmlGenerator()
        {
            HtmlTemplate = new StringBuilder("<html><head><link rel = \"stylesheet\" " +
                    "href = \"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\" " +
                    "integrity = \"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm\" " +
                    "crossorigin = \"anonymous\" ></head><body><div class=\"container\">" +
                    "<h1>{0}</h1><table class=\"table table-bordered\" border=\"1\"><thead><tr>{1}</tr></thead>" +
                    "<tbody>{2}</tbody></table>" +
                    "</div></body></html>");
        }
        
        public static StringBuilder GenerateHtml(string header, string tableHead, string tableBody)
        {
            return new StringBuilder().AppendFormat(HtmlTemplate.ToString(), header, tableHead, tableBody);
        }

        public static string HeadTh(string str) => $"<th scope = \"col\">{str}</th>";

        public static string BodyTh(string str) => $"<th scope = \"row\">{str}</th>";
    }
}
