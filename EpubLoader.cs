using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VersOne.Epub;
namespace AIBookSummary;

internal class EpubLoader
{
    public static void Run(string filePath)
    {
        var allChaptersAndText = new List<(int level, List<string> chapter)>();
        using (EpubBookRef bookRef = EpubReader.OpenBook(filePath))
            foreach (EpubNavigationItemRef navigationItemRef in bookRef.GetNavigation())
                allChaptersAndText.AddRange(GetChapterAndText(navigationItemRef, 0));

        foreach (var (level, chapterAndText) in allChaptersAndText)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine(new string(' ', level * 2) + chapterAndText[0]);
            Console.WriteLine(new string('-', 50));
            foreach (string line in chapterAndText.Skip(1))
                Console.WriteLine(line);
        }
    }

    private static List<(int level, List<string> chapter)> GetChapterAndText(EpubNavigationItemRef navigationItemRef, int level)
    {
        var chapters = new List<(int, List<string>)>();

        var navAndText = new List<string> { navigationItemRef.Title };
        var htmlContent = navigationItemRef.HtmlContentFileRef?.ReadContent();
        navAndText.Add(GetText(htmlContent ?? string.Empty));
        chapters.Add((level, navAndText));

        foreach (EpubNavigationItemRef nestedNavigationItemRef in navigationItemRef.NestedItems)
            chapters.AddRange(GetChapterAndText(nestedNavigationItemRef, level + 1));

        return chapters;
    }

    private static string GetText(string text)
    {
        HtmlDocument htmlDocument = new();
        htmlDocument.LoadHtml(text);
        StringBuilder sb = new();

        var inlineElements = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "a", "b", "i", "span", "em", "strong", "u", "small", "abbr", "cite", "code", "mark", "q", "s", "sub", "sup"
        };

        foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()"))
        {
            var parentName = node.ParentNode?.Name ?? "";
            var textContent = node.InnerText.Trim();
            sb.Append(inlineElements.Contains(parentName) ? textContent : textContent + Environment.NewLine);
        }
        return sb.ToString();
    }
}
