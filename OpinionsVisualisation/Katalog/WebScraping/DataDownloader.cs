using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Katalog.WebScraping
{
    class OpinionsDownloader
    {
        public static List<string> DownloadOpinionsGoldenLine(string webPageGoldenLine)
        {
            HtmlWeb webGoldenLine = new HtmlWeb();
            HtmlDocument htmlDocGoldenLine = webGoldenLine.Load(webPageGoldenLine);
            IEnumerable<HtmlNode> htmlNodesGoldenLine = GetElementsByClasses(htmlDocGoldenLine, "span", new List<string>() { "summary" });

            var opinions = htmlNodesGoldenLine.Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim());

            //IEnumerable<HtmlNode> htmlNodesGoldenLineNextPage = GetElementsByClasses(htmlDocGoldenLine, "li", new List<string>() { "next" });
            //while (htmlNodesGoldenLineNextPage.Count() != 0)
            //{
            //    string nextPage = @"https://www.goldenline.pl" + htmlNodesGoldenLineNextPage.ElementAt(0).InnerHtml.Split('\"')[1];
            //    htmlDocGoldenLine = webGoldenLine.Load(webPageGoldenLine);
            //    htmlNodesGoldenLine = GetElementsByClasses(htmlDocGoldenLine, "span", new List<string>() { "summary" });

            //    opinions.Concat(htmlNodesGoldenLine.Select(node => HttpUtility.HtmlDecode(node.InnerText)));

            //    htmlNodesGoldenLineNextPage = GetElementsByClasses(htmlDocGoldenLine, "li", new List<string>() { "next" });
            //}

            return opinions.ToList();
        }

        public static List<string> DownloadOpinionsGowork(string webPageGowork)
        {
            HtmlWeb webGowork = new HtmlWeb();
            HtmlDocument htmlDocGowork = webGowork.Load(webPageGowork);
            IEnumerable<HtmlNode> htmlNodesGowork = htmlDocGowork.DocumentNode.SelectNodes("//p");

            var opinions = htmlNodesGowork.Where(node => node.Attributes["property"] != null)
                                          .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim())
                                          .Where(opinion => opinion != "(usunięte przez administratora)");

            var htmlNodesGoworkNextPage = GetElementsByClasses(htmlDocGowork, "li", new List<string>() { "page-item" });
            var www = htmlNodesGoworkNextPage.SingleOrDefault(text => text.InnerText.Trim() == "Next");
            int pagesCount = Convert.ToInt32(htmlNodesGoworkNextPage.ElementAt(4).InnerText.Trim());

            for (int i = 2; i <= pagesCount; i++)
            {
                var html = webPageGowork + "," + i.ToString();
                htmlDocGowork = webGowork.Load(html);
                htmlNodesGowork = htmlDocGowork.DocumentNode.SelectNodes("//p");

                opinions = opinions.Concat(htmlNodesGowork.Where(node => node.Attributes["property"] != null)
                                                          .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim())
                                                          .Where(opinion => opinion != "(usunięte przez administratora)"));
            }

            return opinions.ToList();
        }

        public static List<string> DownloadOpinionsAbsolvent(string webPageAbsolvent)
        {
            HtmlWeb webAbsolvent = new HtmlWeb();
            HtmlDocument htmlDocAbsolvent = webAbsolvent.Load(webPageAbsolvent);
            IEnumerable<HtmlNode> htmlNodesAbsolvent = GetElementsByClasses(htmlDocAbsolvent, "div", new List<string>() { "inner-details" });

            var opinions = htmlNodesAbsolvent.Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim());
            return opinions.ToList();

            //IEnumerable<HtmlNode> htmlNodesAbsolventNextPage = GetElementsByClasses(htmlDocAbsolvent, "a", new List<string>() { "next-prev" });
            //while (htmlNodesAbsolventNextPage.Count() != 0)
            //{
            //    var str = htmlNodesAbsolventNextPage.ElementAt(0).OuterHtml.Split('\"')[3].Replace("amp;", "").Split('?')[1];
            //    htmlAbsolvent = @"https://www.absolvent.pl/pracodawcy/profil-accenture/opinie" + "?" + str;
            //    htmlDocAbsolvent = webAbsolvent.Load(htmlAbsolvent);
            //    htmlNodesAbsolvent = GetElementsByClasses(htmlDocAbsolvent, "div", new List<string>() { "inner-details" });
            //    PrintOpinions(htmlNodesAbsolvent);
            //    htmlNodesAbsolventNextPage = GetElementsByClasses(htmlDocAbsolvent, "a", new List<string>() { "next-prev" });
            //}
        }

        private static IEnumerable<HtmlNode> GetElementsByClasses(HtmlDocument doc, string tag, IEnumerable<String> classNames)
        {
            String selector = tag + "." + String.Join(".", classNames);
            return doc.QuerySelectorAll(selector);
        }
    }
}
