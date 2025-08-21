#!/usr/local/share/dotnet/dotnet run

var inputFile = args[0];
var outputFile = args[0];

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

var content = "";
if (inputFile.EndsWith(".part", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Reading parts...");

    outputFile = TrimLast(outputFile, ".part");

    var justName = Path.GetFileName(inputFile);
    var dir = Path.GetDirectoryName(inputFile);

    var nameParts = justName.Split('.');

    var number = nameParts[^2];
    var originalName = string.Join('.', nameParts[0..^2]);

    for (int i = 0; ; i++)
    {
        var partPath = Path.Join(dir, $"{originalName}.{i}.part");
        if (File.Exists(partPath))
        {
            content += File.ReadAllText(partPath);
        }
        else
        {
            break;
        }
    }
}
else
{
    Console.WriteLine("Reading single file...");
    content = File.ReadAllText(inputFile);
}

outputFile = outputFile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ? TrimLast(outputFile, ".txt") : outputFile + ".org";

var binary = Convert.FromBase64String(content);
File.WriteAllBytes(outputFile, binary);

Console.WriteLine("Finished");

static string TrimLast(string source, string input)
{
    return source.Substring(0, source.LastIndexOf(input, StringComparison.OrdinalIgnoreCase));
}
