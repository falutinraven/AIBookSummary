using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIBookSummary.Models;
public class Book
{
    public BookInfo BookInfo { get; set; }
    public List<ChapterInfo> Chapters { get; set; }
}

public class ChapterInfo
{
    public string Name { get; set; } = string.Empty;
    public string Contents { get; set; } = string.Empty;
    [JsonConstructor]
    public ChapterInfo(string Name, string Contents)
    {
        this.Name = Name;
        this.Contents = Contents;
    }
}

public class BookInfo
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    [JsonConstructor]
    public BookInfo(string Title, string Author)
    {
        this.Title = Title;
        this.Author = Author;
    }
}
