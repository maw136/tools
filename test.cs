#!/usr/local/share/dotnet/dotnet run

File.AppendAllBytes("test.b.bin", [(byte)65, (byte)66]);
File.AppendAllBytes("test.b.bin", [(byte)67, (byte)68]);