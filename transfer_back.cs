#!/usr/local/share/dotnet/dotnet run

using System.Text;

var inputFile = args[0];
var outputFile = args[0].EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ? args[0].Substring(0, args[0].LastIndexOf(".txt", StringComparison.OrdinalIgnoreCase)) : args[0] + ".org";

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

var content = File.ReadAllText(inputFile);
var binary = Convert.FromBase64String(content);
File.WriteAllBytes(outputFile, binary);

Console.WriteLine("Finished");
