using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace AIBookSummary;


internal class OpenAIProcessing
{
    public static async Task<Models.ChapterAnalysis> GenerateChapterSummary(string chapterName, string chapterContents)
    {
        var config = Config.Config.Load();
        var instructions = JsonDocument.Parse(File.ReadAllText(config.InstructionsPath)).RootElement.GetProperty("chapterAnalysisInstructions");
        var instructionText = string.Join("\n", instructions.EnumerateArray()
            .Select(e => e.GetProperty("instruction").GetString()));

        ChatClient client = new(model: "gpt-4.1-nano", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "chapter_analysis",
                jsonSchema: BinaryData.FromBytes(File.ReadAllBytes(config.SchemaPath)),
                jsonSchemaIsStrict: true)
        };

        List<ChatMessage> messages = [
                new SystemChatMessage(
                    [
                        ChatMessageContentPart.CreateTextPart(instructionText),
                        ChatMessageContentPart.CreateTextPart($"Chapter Name:  {chapterName}"),
                        ChatMessageContentPart.CreateTextPart($"Chapter Contents: {chapterContents}"),
                ])
        ];

        ChatCompletion completion = await client.CompleteChatAsync(messages, options);
        var analysisJson = completion.Content.Count > 0 ? completion.Content[0].Text : null;
        if (string.IsNullOrWhiteSpace(analysisJson))
            throw new InvalidOperationException("Chat completion did not return any content to deserialize.");
        var analysis = JsonSerializer.Deserialize<Models.ChapterAnalysis>(analysisJson) ?? throw new InvalidOperationException("Deserialized ChapterAnalysis is null.");
        Console.WriteLine($"Chapter: {chapterName} - Analysis: {analysisJson}");
        return analysis;
    }

    //public static async Task<Models.Book> GenerateChapterSummaries()
    //{
    //    var config = Config.Config.Load();
    //    var instructions = JsonDocument.Parse(File.ReadAllText(config.InstructionsPath)).RootElement.GetProperty("instructions");
    //    var instructionText = string.Join("\n", instructions.EnumerateArray()
    //        .Select(e => e.GetProperty("instruction").GetString()));

    //    Models.Book book = JsonSerializer.Deserialize<Models.Book>(File.ReadAllText(config.BookJsonPath)) ?? new Models.Book { Chapters = new List<Models.ChapterInfo>() };

    //    ChatClient client = new(model: "gpt-4.1-nano", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    //    ChatCompletionOptions options = new()
    //    {
    //        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
    //            jsonSchemaFormatName: "chapter_analysis",
    //            jsonSchema: BinaryData.FromBytes(File.ReadAllBytes(config.SchemaPath)),
    //            jsonSchemaIsStrict: true)
    //    };

    //    Models.ChapterAnalysis? prevAnalysis = null;
    //    foreach (Models.ChapterInfo chapter in book.Chapters)
    //    {
    //        var jsonAnalysisHistory = prevAnalysis != null ? JsonSerializer.Serialize(prevAnalysis) : string.Empty;
    //        List<ChatMessage> messages = [
    //                new SystemChatMessage(
    //                    [
    //                        ChatMessageContentPart.CreateTextPart(instructionText),
    //                        ChatMessageContentPart.CreateTextPart($"The book you are summarizing is {book.Title} By: {book.Author}"),
    //                        ChatMessageContentPart.CreateTextPart($"Previous Analysis:  {jsonAnalysisHistory}"),
    //                        ChatMessageContentPart.CreateTextPart($"Chapter Name:  {chapter.Name}"),
    //                        ChatMessageContentPart.CreateTextPart($"Chapter Contents: {chapter.Contents}"),
    //                ])
    //        ];

    //        ChatCompletion completion = await client.CompleteChatAsync(messages, options);
    //        var analysisJson = completion.Content.Count > 0 ? completion.Content[0].Text : null;
    //        if (string.IsNullOrWhiteSpace(analysisJson))
    //            throw new InvalidOperationException("Chat completion did not return any content to deserialize.");
    //        prevAnalysis = JsonSerializer.Deserialize<Models.ChapterAnalysis>(analysisJson) ?? throw new InvalidOperationException("Deserialized ChapterAnalysis is null.");
    //        chapter.Analysis = prevAnalysis;
    //        Console.WriteLine($"Chapter: {chapter.Name} - Analysis: {prevAnalysis.Summary}");
    //    }
    //    File.WriteAllText(Path.Combine(config.ProjectPath, "Output", $"{book.Title.Replace(" ", "_")}_contents_and_analysis.json"), JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true }));
    //    return book;
    //}
}
