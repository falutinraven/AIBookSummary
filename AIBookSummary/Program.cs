//using OpenAI.Chat;

//ChatClient client = new(
//  model: "gpt-4.1",
//  apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
//);

//ChatCompletion completion = client.CompleteChat("Say 'this is a test.'");

//Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

using AIBookSummary;
using System;
using System.Text;
using System.Xml;
using VersOne.Epub;

//EpubLoader.Run("E:\\Coding\\Projects\\AIBookSummary\\input\\The_Way_of_Kings.epub");

var book = await OpenAIProcessing.GenerateChapterSummaries();
Console.WriteLine($"Book Title: {book.Title}");

