// -----------------------------------------------------------------------------
// <copyright file="FileEditor.cs" company="DCOM Engineering, LLC">
//     Copyright (c) David Anderson. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------

namespace Client;

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A basic example of a file editor to demonstrate basic file editing capabilities.
/// </summary>
public class FileEditor
{
    /// <summary>
    /// Gets the file context for accessing the temporary working file and output file.
    /// </summary>
    public FileContext FileContext
    {
        get;
        private set;
    }

    /// <summary>
    /// Closes the file currently open in the editor, if any.
    /// </summary>
    public void Close()
    {
        if (FileContext != null)
        {
            FileContext.SaveStream.Close();
            FileContext.WorkStream.Close();
            FileContext = null;
        }
    }

    /// <summary>
    /// Opens the specified file in the editor.
    /// </summary>
    /// <param name="fileName">The full directory and file name to the file to open.</param>
    /// <exception cref="InvalidOperationException">if a file is already open in the editor.</exception>
    public void Open(string fileName)
    {
        if (FileContext != null)
        {
            throw new InvalidOperationException($"{FileContext.SaveStream.Name} is already open.");
        }

        var thisAssembly = Assembly.GetExecutingAssembly();

        string appDirectoryName = Path.GetDirectoryName(thisAssembly.Location);
        string saveFileName     = fileName;
        string workFileName     = Path.Combine(appDirectoryName, $"~${Path.GetFileName(saveFileName)}");

        FileStream workStream = new(workFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
        FileStream saveStream = new(saveFileName, FileMode.Open, FileAccess.ReadWrite);

        FileContext = new(saveStream, workStream);
    }

    /// <summary>
    /// Saves the contents of the temporary working file to the output file.
    /// </summary>
    /// <returns>Returns the <see cref="Task"/> for the asynchronous operation.</returns>
    public async Task SaveAsync()
    {
        FileContext.SaveStream.SetLength(0);
            
        FileContext.WorkStream.Seek(0, SeekOrigin.Begin);

        await FileContext.WorkStream.CopyToAsync(FileContext.SaveStream);

        await FileContext.SaveStream.FlushAsync();
    }

    /// <summary>
    /// Writes the specified value to a new line in the file.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <returns>Returns the <see cref="Task"/> for the asynchronous operation.</returns>
    public async Task WriteLineAsync(string value)
    {
        using var writer = new StreamWriter(FileContext.WorkStream, Encoding.Default, 4096, true);

        await writer.WriteLineAsync(value);
    }
}