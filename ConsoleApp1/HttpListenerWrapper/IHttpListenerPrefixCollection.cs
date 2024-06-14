using System.Collections;

namespace ConsoleApp1.HttpListenerWrapper;

public interface IHttpListenerPrefixCollection : IEnumerable
{
    void Add(string uriPrefix);
    void Clear();
    bool Contains(string uriPrefix);
    void CopyTo(string[] array, int offset);
    bool Remove(string uriPrefix);
    int Count { get; }
}