#!/usr/local/share/dotnet/dotnet run

using System.Text;

var inputFile = args[0];
var outputFile = args[0] + ".txt";
var partSize = 49 * 1024 * 1024;

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

var content = File.ReadAllBytes(inputFile);
var base64arr = Convert.ToBase64String(content);
if (base64arr.Length > partSize) // assume 1B/1char
{
    Console.WriteLine("Writing parts...");
    var leftover = base64arr.Length;
    var parts = (int)Math.Ceiling(base64arr.Length / (double)partSize);
    for (int i = 0; i < parts; i++)
    {
        var spanSize = leftover >= partSize ? partSize : leftover;
        File.WriteAllText($"{outputFile}.{i}.part", base64arr.AsSpan(partSize * i, spanSize));
        leftover -= partSize;
        //File.WriteAllText(outputFile, base64arr, Encoding.ASCII);
    }
    // split
}
else
{
    Console.WriteLine("Writing single file...");
    File.WriteAllText(outputFile, base64arr, Encoding.ASCII);
}

Console.WriteLine("Finished");
