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
    public static void Run()
    {
        var config = Config.Config.Load();
        var instructions = JsonDocument.Parse(File.ReadAllText(config.InstructionsPath)).RootElement.GetProperty("instructions");
        var instructionText = string.Join("\n", instructions.EnumerateArray()
            .Select(e => e.GetProperty("instruction").GetString()));

        Models.Book book = JsonSerializer.Deserialize<Models.Book>(File.ReadAllText(config.BookJsonPath)) ?? new Models.Book();

        ChatClient client = new(model: "gpt-4.1-nano", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "chapter_analysis",
                jsonSchema: BinaryData.FromBytes(File.ReadAllBytes(config.SchemaPath)),
                jsonSchemaIsStrict: true)
        };

        var analysisHistory = new List<Models.ChapterAnalysis>();
        foreach (Models.ChapterInfo chapter in book.Chapters)
        {
            var lastAnalysis = analysisHistory.LastOrDefault();
            var JsonAnalysisHistory = lastAnalysis != null ? JsonSerializer.Serialize(lastAnalysis) : string.Empty;
            List<ChatMessage> messages = [
                    new SystemChatMessage(
                        [
                            ChatMessageContentPart.CreateTextPart(instructionText),
                            ChatMessageContentPart.CreateTextPart("The book you are summarizing is " + book.BookInfo.Title + " By: " + book.BookInfo.Author),
                            ChatMessageContentPart.CreateTextPart("Chapter Name: " + chapter.Name),
                            ChatMessageContentPart.CreateTextPart("Chapter Contents: " + chapter.Contents),
                    ])
            ];

            ChatCompletion completion = client.CompleteChat(messages, options);
            Models.ChapterAnalysis analysis = JsonSerializer.Deserialize<Models.ChapterAnalysis>(completion.Content[0].Text);
            analysisHistory.Add(analysis);
            Console.WriteLine("Summary for chapter: " + analysis.Summary);
        }
    }
}
