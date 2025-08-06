using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBookSummary.Models;
public class FullBookAnalysis
{
    public List<ChapterAnalysis> Chapters { get; set; } = new();
    public Dictionary<string, CharacterEntry> Characters { get; set; } = new();
    public Dictionary<string, NamedLocationEntry> NamedLocations { get; set; } = new();
    public Dictionary<string, WorldVocabEntry> WorldVocab { get; set; } = new();
    public HashSet<string> Themes { get; set; } = new();
}

public class CharacterEntry
{
    public string Name { get; set; }
    public string Description { get; set; }

    public void Merge(CharacterEntry newInfo)
    {
        if (!Description.Contains(newInfo.Description))
            Description += $" {newInfo.Description}";
    }
}

