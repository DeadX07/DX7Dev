// -----------------------------------------------------------------------------
// <copyright file="FileContext.cs" company="DCOM Engineering, LLC">
//     Copyright (c) David Anderson. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------

namespace Client;

using System.IO;

/// <summary>
/// Manages a temporary working stream and a output stream for accessing a file.
/// </summary>
public class FileContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileContext"/> class.
    /// </summary>
    /// <param name="saveStream">The output stream that will be used to save the file contents.</param>
    /// <param name="workStream">The temporary working stream that will be used to access the file.</param>
    public FileContext(FileStream saveStream, FileStream workStream)
    {
        SaveStream = saveStream;
        WorkStream = workStream;
    }

    /// <summary>
    /// Gets the output stream that will be used to save the file contents.
    /// </summary>
    public FileStream SaveStream
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets the temporary working stream that will be used to access the file.
    /// </summary>
    public FileStream WorkStream
    {
        get; 
        set;
    }
}