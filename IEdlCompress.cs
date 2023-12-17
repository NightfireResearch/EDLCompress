using System.Collections.Generic;
using System.IO;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace wdcossey;

public interface IEdlCompress
{
    Stream Decompress(string fileName);
    void Decompress(string fileName, Stream @out);
    Stream Decompress(Stream @in);
    void Decompress(Stream @in, Stream @out);

    public EdlCompress.EdlHeader ParseHeader(string fileName);
    public EdlCompress.EdlHeader ParseHeader(Stream @in);

    IEnumerable<EdlCompress.EdlHeader> Scan(string fileName);
    IEnumerable<EdlCompress.EdlHeader> Scan(Stream @in);
}
