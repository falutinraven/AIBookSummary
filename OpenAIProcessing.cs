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
    public static void Run(string filePath)
    {
        ChatClient client = new(model: "gpt-4.1-nano", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        List<ChatMessage> messages = [
                new UserChatMessage("You are an expert book chapter summarizer. I will provide you with a chapter in a book, and you will analyze and provide a summary, key themes, universe specific vocabulary (best guess at meaning given context) characters, and locations. Here is the json version of the chapter including name of book and chapter contents: "
                                    + System.IO.File.ReadAllText(filePath) ),
        ];

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "chapter_analysis",
                jsonSchema: BinaryData.FromBytes("""
                    {
                      "type": "object",
                      "additionalProperties": false,
                      "properties": {
                        "chapterName": {
                          "type": "string"
                        },
                        "summary": {
                          "type": "string"
                        },
                        "themes": {
                          "type": "array",
                          "items": {
                            "type": "string"
                          }
                        },
                        "worldVocab": {
                          "type": "array",
                          "items": {
                            "type": "object",
                            "additionalProperties": false,
                            "properties": {
                              "word": { "type": "string" },
                              "best_guess_at_meaning": { "type": "string" }
                            },
                            "required": ["word", "best_guess_at_meaning"]
                          }
                        },
                        "characters": {
                          "type": "array",
                          "items": {
                            "type": "object",
                            "additionalProperties": false,
                            "properties": {
                              "name": { "type": "string" },
                              "description": { "type": "string" }
                            },
                            "required": ["name", "description"]
                          }
                        },
                        "locations": {
                          "type": "array",
                          "items": {
                            "type": "object",
                            "additionalProperties": false,
                            "properties": {
                              "name": { "type": "string" },
                              "description": { "type": "string" }
                            },
                            "required": ["name", "description"]
                          }
                        }
                      },
                      "required": ["chapterName", "summary", "themes", "worldVocab", "characters", "locations"]
                    }
                    """u8.ToArray()),
                jsonSchemaIsStrict: true)
        };

        ChatCompletion completion = client.CompleteChat(messages, options);
        using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

        Console.WriteLine($"Chapter Name: {structuredJson.RootElement.GetProperty("chapterName")}");

        foreach (JsonElement stepElement in structuredJson.RootElement.GetProperty("characters").EnumerateArray())
        {
            Console.WriteLine($"  - name: {stepElement.GetProperty("name")}");
            Console.WriteLine($"    Description: {stepElement.GetProperty("description")}");
        }
    }

}
