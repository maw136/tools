#!/usr/local/share/dotnet/dotnet run

var inputFile = args[0];
var outputFile = args[0];

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

if (inputFile.EndsWith(".part", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Reading parts...");

    var justName = Path.GetFileName(inputFile);
    var dir = Path.GetDirectoryName(inputFile);

    var nameParts = justName.Split('.');
    var originalName = string.Join('.', nameParts[0..^2]);

    outputFile = Path.Join(dir, originalName).EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ? TrimLast(outputFile, ".txt") : outputFile + ".org";
    File.Delete(outputFile);

    for (int i = 0; ; i++)
    {
        var partPath = Path.Join(dir, $"{originalName}.{i}.part");
        if (File.Exists(partPath))
        {
            var content = File.ReadAllText(inputFile);
            var binary = Convert.FromBase64String(content);
            File.AppendAllBytes(outputFile, binary);
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

    outputFile = outputFile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ? TrimLast(outputFile, ".txt") : outputFile + ".org";
    File.Delete(outputFile);

    var content = File.ReadAllText(inputFile);
    var binary = Convert.FromBase64String(content);
    File.WriteAllBytes(outputFile, binary);
}

Console.WriteLine("Finished");

static string TrimLast(string source, string input)
{
    return source.Substring(0, source.LastIndexOf(input, StringComparison.OrdinalIgnoreCase));
}
