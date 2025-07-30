using AIBookSummary.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using VersOne.Epub;
namespace AIBookSummary;

internal class EpubLoader
{
    public static void Run()
    {
        var config = Config.Config.Load();
        var allChaptersAndText = new List<(int level, string chapterName, string chapter)>();
        var author = string.Empty;
        var title = string.Empty;
        using (EpubBookRef bookRef = EpubReader.OpenBook(config.EpubPath))
        {
            author = bookRef.Author;
            title = bookRef.Title;

            foreach (EpubNavigationItemRef navigationItemRef in bookRef?.GetNavigation() ?? new List<EpubNavigationItemRef>())
                allChaptersAndText.AddRange(GetChapterAndText(navigationItemRef, 0));
        }

        var book = new Book
        {
            BookInfo = new BookInfo(title, author),
            Chapters = allChaptersAndText
                .Where(c => WordCount(c.chapter) > 50)
                .Select(c => new ChapterInfo(c.chapterName, c.chapter))
                .ToList()
        };
        var jsonOutput = JsonSerializer.Serialize(book);
        File.WriteAllText(Path.Join(config.ProjectPath, "Output", $"{title}.json"), jsonOutput);
    }

    static List<(int level, string chapterName, string chapter)> GetChapterAndText(EpubNavigationItemRef navigationItemRef, int level)
    {
        var chapters = new List<(int, string, string)>();

        var htmlContent = navigationItemRef.HtmlContentFileRef?.ReadContent();
        chapters.Add((level, navigationItemRef.Title, GetText(htmlContent ?? string.Empty)));

        foreach (EpubNavigationItemRef nestedNavigationItemRef in navigationItemRef.NestedItems)
            chapters.AddRange(GetChapterAndText(nestedNavigationItemRef, level + 1));

        return chapters;
    }

    static string GetText(string text)
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
        return Regex.Replace(sb.ToString().Trim(), @"(\r\n){2,}", "\r\n");
    }

    static int WordCount(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

}


