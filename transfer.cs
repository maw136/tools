#!/usr/local/share/dotnet/dotnet run

using System.Text;

var inputFile = args[0];
var outputFile = args[0] + ".txt";

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

var content = File.ReadAllBytes(inputFile);
var base64arr = Convert.ToBase64String(content);
File.WriteAllText(outputFile, base64arr, Encoding.ASCII);

Console.WriteLine("Finished");
