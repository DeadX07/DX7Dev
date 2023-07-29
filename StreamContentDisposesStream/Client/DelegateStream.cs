// -----------------------------------------------------------------------------
// <copyright file="DelegateStream.cs" company="DCOM Engineering, LLC">
//     Copyright (c) David Anderson. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------

namespace Client;

using System.IO;

/// <summary>
/// Creates a stream that delegates for a base stream.
/// </summary>
public class DelegateStream : Stream
{
    /// <summary>
    /// Initializes a new instnace of the <see cref="DelegateStream"/> class.
    /// </summary>
    /// <param name="stream">The base stream to delegate.</param>
    public DelegateStream(Stream stream, bool leaveOpen = true)
    {
        BaseStream = stream;
        LeaveOpen  = leaveOpen;
    }

    /// <summary>
    /// Gets the stream this stream delegates for.
    /// </summary>
    public Stream BaseStream
    {
        get;
    }

    /// <summary>
    /// Gets or sets a value indicating if the base stream will be disposed.
    /// </summary>
    protected bool LeaveOpen
    {
        get;
        set;
    }

    /// <inheritdoc />
    public override void Flush()
    {
        BaseStream.Flush();
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        return BaseStream.Seek(offset, origin);
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        BaseStream.SetLength(value);
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        return BaseStream.Read(buffer, offset, count);
    }
    
    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        BaseStream.Write(buffer, offset, count);
    }
    
    /// <inheritdoc />
    public override bool CanRead
    {
        get => BaseStream.CanRead;
    }

    /// <inheritdoc />
    public override bool CanSeek
    {
        get => BaseStream.CanSeek;
    }
    
    /// <inheritdoc />
    public override bool CanWrite
    {
        get => BaseStream.CanWrite;
    }

    /// <inheritdoc />
    public override long Length
    {
        get => BaseStream.Length;
    }

    /// <inheritdoc />
    public override long Position
    {
        get => BaseStream.Position;
        set => BaseStream.Position = value;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (!LeaveOpen)
        {
            BaseStream?.Dispose();
        }

        base.Dispose(disposing);
    }
}