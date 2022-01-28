using System.Text;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices.Html
{
    public static class HtmlGenerator
    {
        #region [Private variables]
        private const string _htmlPattern = "<html><head><link rel = \"stylesheet\" " +
                    "href = \"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\" " +
                    "integrity = \"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm\" " +
                    "crossorigin = \"anonymous\" ></head><body><div class=\"col-12\">" +
                    "<h1>{0}</h1>{1}</div></body></html>";
        #endregion

        public const string NonBreakingSpace = "&nbsp;-&nbsp;";

        public static StringBuilder GenerateHtml(string header, StringBuilder table)
        {
            return new StringBuilder().AppendFormat(_htmlPattern, header, table);
        }

        /// <summary>
        /// Accepts an array of headers and an array of arrays of rows
        /// </summary>
        /// <param name="headers">Сolumn headers</param>
        /// <param name="rows">Each element of the array must contain an array with data cells for headers</param>
        /// <returns>Html table</returns>
        public static StringBuilder GenerateTable(string[] headers, string[][] rows)
        {
            StringBuilder table = new StringBuilder("<table class=\"table table-bordered\" border=\"1\">");

            StringBuilder tableHead = new StringBuilder("<thead><tr>");

            tableHead.Append(HeadTh("#"));
            foreach (var header in headers)
            {
                tableHead.Append(HeadTh(header));
            }
            tableHead.Append("</tr></thead>");

            StringBuilder tableBody = new StringBuilder();

            tableBody.Append("<tbody>");
            int counter = 1;
            foreach (var row in rows)
            {
                tableBody.Append("<tr>");
                tableBody.Append(BodyTh($"{counter}"));
                foreach (var data in row)
                {
                    tableBody.Append(BodyTd(data));
                }
                tableBody.Append("</tr>");
                counter++;
            }
            tableBody.Append("</tbody>");

            table.Append(tableHead);
            table.Append(tableBody);
            table.Append("</table>");

            return table;
        }

        public static string HeadTh(string str) => $"<th scope = \"col\"><center>{str}</center></th>";

        public static string BodyTh(string str) => $"<th scope = \"row\"><center>{str}</center></th>";

        public static string BodyTd(string str) => $"<td><center>{str}</center></td>";

        public static string MarkText(string str) => $"<mark>{str}</mark>";
    }
}
