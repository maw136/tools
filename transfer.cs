#!/usr/local/share/dotnet/dotnet run

using System.Diagnostics;
using System.Text;

var inputFile = args[0];
var outputFile = args[0] + ".txt";
var partSize = 1335466393;

Console.WriteLine($"Input: {inputFile}, Output: {outputFile}");

using FileStream inputFs = File.OpenRead(inputFile);
Console.WriteLine($"Size: {inputFs.Length}");

if (inputFs.Length > partSize)
{
    Console.WriteLine("Writing parts...");
    byte[] contentBs = new byte[partSize];
    for (int i = 1; ; i++)
    {
        // multipart
        var read = inputFs.Read(contentBs, 0, partSize);
        if (read < 1)
        {
            break;
        }

        var base64arr = Convert.ToBase64String(contentBs, 0, read);
        var justName = $"{Path.GetFileName(outputFile)}.{i}.part";
        File.WriteAllText($"{outputFile}.{i}.part", base64arr);
        //using var gitAdd = Process.Start("git", $"add \"{justName}\"");
        //gitAdd.WaitForExit();
        //using var gitCommit = Process.Start("git", $"commit -m \"{justName}\"");
        //gitCommit.WaitForExit();
    }
}
else
{
    Console.WriteLine("Writing single file...");
    byte[] contentBs = new byte[inputFs.Length];
    var read = inputFs.Read(contentBs, 0, contentBs.Length);
    var base64arr = Convert.ToBase64String(contentBs, 0, read);
    File.WriteAllText(outputFile, base64arr, Encoding.ASCII);

    //var justName = Path.GetFileName(outputFile);
    //using var gitAdd = Process.Start("git", $"add \"{justName}\"");
    //gitAdd.WaitForExit();
    //using var gitCommit = Process.Start("git", $"commit -m \"{justName}\"");
    //gitCommit.WaitForExit();
}

//using var gitPush = Process.Start("git", "push");
//gitPush.WaitForExit();

Console.WriteLine("Finished");
