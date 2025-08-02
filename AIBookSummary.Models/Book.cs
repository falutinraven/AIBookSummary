using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIBookSummary.Models;
public class Book
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public required List<ChapterInfo> Chapters { get; set; } = new List<ChapterInfo>();
}

public class ChapterInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Contents { get; set; } = string.Empty;
    public ChapterAnalysis? Analysis { get; set; }
    [JsonConstructor]
    public ChapterInfo(string Name, string Contents)
    {
        this.Name = Name;
        this.Contents = Contents;
    }
}

