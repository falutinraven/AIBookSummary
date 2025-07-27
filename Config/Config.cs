using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIBookSummary.Config;
internal class Config
{
    public string OutputPath { get; set; }
    public string InputPath { get; set; }
    public string ConfigPath { get; set; }

    public string SchemaPath { get; set; }
    public string EpubPath { get; set; }
    public string SingleChapterPath { get; set; }
    public string InstructionsPath { get; set; }
    public string BookJsonPath { get; set; }

    public static Config Load()
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string configFilePath = Path.Combine(projectDirectory, "Config", "config.json");
        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configFilePath)) ?? new Config();

        config.BookJsonPath = Path.Combine(config.OutputPath, config.BookJsonPath);
        config.EpubPath = Path.Combine(config.InputPath, config.EpubPath);
        config.SingleChapterPath = Path.Combine(config.InputPath, config.SingleChapterPath);
        config.InstructionsPath = Path.Combine(config.InputPath, config.InstructionsPath);
        config.SchemaPath = Path.Combine(config.ConfigPath, config.SchemaPath);

        return config;
    }
}
