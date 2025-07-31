using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIBookSummary.Models;
public class ChapterAnalysis
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("chapterName")]
    public string ChapterName { get; set; } = string.Empty;

    [JsonPropertyName("chapterIndex")]
    public int ChapterIndex { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("themes")]
    public List<string> Themes { get; set; } = new();

    [JsonPropertyName("worldVocab")]
    public List<WorldVocabItem>? WorldVocab { get; set; }

    [JsonPropertyName("characters")]
    public List<CharacterInfo> Characters { get; set; } = new();

    [JsonPropertyName("namedlocations")]
    public List<NamedLocation>? NamedLocations { get; set; }
}

public class WorldVocabItem
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("word")]
    public string Word { get; set; } = string.Empty;

    [JsonPropertyName("best_guess_at_meaning")]
    public string BestGuessAtMeaning { get; set; } = string.Empty;
}

public class CharacterInfo
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("chaptersIntroduced")]
    public List<int> ChaptersIntroduced { get; set; } = new();

    [JsonPropertyName("progression")]
    public List<CharacterProgression>? Progression { get; set; }
}

public class CharacterProgression
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("chapter")]
    public int Chapter { get; set; }

    [JsonPropertyName("change")]
    public string Change { get; set; } = string.Empty;
}

public class NamedLocation
{
    [JsonIgnore]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("chaptersIntroduced")]
    public List<int> ChaptersIntroduced { get; set; } = new();
}
