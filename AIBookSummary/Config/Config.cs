using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIBookSummary.Config;
internal class Config
{
    public required string ProjectPath { get; set; }

    public required string SchemaPath { get; set; }
    public required string EpubPath { get; set; }
    public required string SingleChapterPath { get; set; }
    public required string InstructionsPath { get; set; }
    public required string BookJsonPath { get; set; }

    public static Config Load()
    {
        var dir = Directory.GetParent(Environment.CurrentDirectory);
        dir = dir?.Parent;
        dir = dir?.Parent;
        if (dir == null)
            throw new InvalidOperationException("Unable to determine the project directory from the current directory.");
        string projectDirectory = dir.FullName;
        string configFilePath = Path.Combine(projectDirectory, "Config", "config.json");
        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configFilePath));
        if (config == null)
            throw new InvalidOperationException("Config file is missing required properties or is invalid.");

        var inputPath = Path.Combine(config.ProjectPath, "Input");
        var outputPath = Path.Combine(config.ProjectPath, "Output");
        var configPath = Path.Combine(config.ProjectPath, "Config");

        config.BookJsonPath = Path.Combine(outputPath, config.BookJsonPath);
        config.EpubPath = Path.Combine(inputPath, config.EpubPath);
        config.SingleChapterPath = Path.Combine(inputPath, config.SingleChapterPath);
        config.InstructionsPath = Path.Combine(inputPath, config.InstructionsPath);
        config.SchemaPath = Path.Combine(configPath, config.SchemaPath);

        return config;
    }
}
