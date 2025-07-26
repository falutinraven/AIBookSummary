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
    public static void Run(string filePath)
    {
        var allChaptersAndText = new List<(int level, string chapterName, string chapter)>();
        var author = string.Empty;
        var title = string.Empty;
        using (EpubBookRef bookRef = EpubReader.OpenBook(filePath))
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
        var json = JsonSerializer.Serialize(book);
        File.WriteAllText("chapters.json", json);
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

internal class ChapterInfo
{
    public string ChapterName { get; set; }
    public string Chapter { get; set; }
    public ChapterInfo(string chapterName, string chapter)
    {
        ChapterName = chapterName;
        Chapter = chapter;
    }
}

internal class BookInfo
{
    public string Title { get; set; }
    public string Author { get; set; }
    public BookInfo(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

internal class Book
{
    public BookInfo BookInfo { get; set; }
    public List<ChapterInfo> Chapters { get; set; }
}
