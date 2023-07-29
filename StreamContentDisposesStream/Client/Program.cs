// -----------------------------------------------------------------------------
// <copyright file="Program.cs" company="DCOM Engineering, LLC">
//     Copyright (c) David Anderson. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------

namespace Client;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/// <remarks>
/// To demonstrate the problem, add text to the sample file using the console editor and execute
/// a upload command and subsequent upload or save commands to try and access the file stream
/// after it has already been uploaded to the web api.
///
/// There are two solution configurations: Debug (Problem), Debug (Solution) you can switch
/// between to excerise the demonstrated problem, or the code that provides the solution.
/// </remarks>
internal class Program
{
    private static async Task Main()
    {
        FileEditor fileEditor = new();

        fileEditor.Open("Sample.txt");

        while (true)
        {
            Console.Write($"{Path.GetFileName(fileEditor.FileContext.SaveStream.Name)} > ");

            string s = Console.ReadLine();

            if (ExitRequested(s))
            {
                break;
            }

            switch (s)
            {
                case "upload":
                {
#if PROBLEM
                    FileStream tempStream = fileEditor.FileContext.SaveStream;
#elif SOLUTION
                    using DelegateStream tempStream = new(fileEditor.FileContext.SaveStream, leaveOpen: true);
#endif
                    string fileName = Path.GetFileName(fileEditor.FileContext.SaveStream.Name);

                    await UploadFileAsync(tempStream, fileName);

                    break;
                }
                case "save":
                {
                    await fileEditor.SaveAsync();

                    await DisplayPreview(fileEditor);

                    break;
                }
                default:
                {
                    await fileEditor.WriteLineAsync(s);

                    break;
                }
            }
        }

        fileEditor.Close();
    }

    private static async Task DisplayPreview(FileEditor fileEditor)
    {
        fileEditor.FileContext.SaveStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(fileEditor.FileContext.SaveStream, Encoding.Default, true, 4096, true);

        string preview = await reader.ReadToEndAsync();
            
        Console.WriteLine();
        Console.WriteLine("-- Sample.txt (Preview) -- ");
        Console.WriteLine();
        Console.WriteLine(preview);
        Console.WriteLine("--");
        Console.WriteLine();
    }

    private static bool ExitRequested(string value)
    {
        var exitCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                           {
                               "quit",
                               "q",
                               "exit"
                           };

        return exitCommands.Contains(value);
    }

    private static async Task UploadFileAsync(Stream stream, string fileName)
    {
        using var httpClient = new HttpClient();

        var multipartFormContent = new MultipartFormDataContent("FileUpload");
        var streamContent        = new StreamContent(stream);

        streamContent.Headers.ContentType        = new MediaTypeHeaderValue("application/octet-stream");
        streamContent.Headers.ContentLength      = stream.Length;
        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                                   {
                                                       FileName = fileName
                                                   };

        multipartFormContent.Add(streamContent);

        using var message = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7021/api/upload")
                            {
                                Content = multipartFormContent
                            };

        using HttpResponseMessage response = await httpClient.SendAsync(message);
    }
}