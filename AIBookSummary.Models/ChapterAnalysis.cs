using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIBookSummary.Models;
public class ChapterAnalysis
{
    [JsonIgnore]
    [Key]
    public long? Id { get; set; }

    [JsonPropertyName("chapterName")]
    public string ChapterName { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("themes")]
    public List<string>? Themes { get; set; } = new();

    [JsonPropertyName("worldVocab")]
    public List<WorldVocabItem>? WorldVocab { get; set; }

    [JsonPropertyName("characters")]
    public List<CharacterInfo>? Characters { get; set; } = new();

    [JsonPropertyName("namedlocations")]
    public List<NamedLocation>? NamedLocations { get; set; }
}

public class WorldVocabItem
{
    [JsonIgnore]
    [Key]
    public long? Id { get; set; }

    [JsonPropertyName("word")]
    public string Word { get; set; } = string.Empty;

    [JsonPropertyName("bestGuessAtMeaning")]
    public string BestGuessAtMeaning { get; set; } = string.Empty;
}

public class CharacterInfo
{
    [JsonIgnore]
    [Key]
    public long? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}


public class NamedLocation
{
    [JsonIgnore]
    [Key]
    public long? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
