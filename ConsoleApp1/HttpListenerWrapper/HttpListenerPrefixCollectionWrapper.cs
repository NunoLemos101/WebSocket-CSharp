using System.Collections;
using System.Net;

namespace ConsoleApp1.HttpListenerWrapper;

public class HttpListenerPrefixCollectionWrapper : IHttpListenerPrefixCollection
{
    private readonly HttpListenerPrefixCollection _prefixes;

    public HttpListenerPrefixCollectionWrapper(HttpListenerPrefixCollection prefixes)
    {
        _prefixes = prefixes;
    }

    public void Add(string uriPrefix) => _prefixes.Add(uriPrefix);

    public void Clear() => _prefixes.Clear();

    public bool Contains(string uriPrefix) => _prefixes.Contains(uriPrefix);

    public void CopyTo(string[] array, int offset) => _prefixes.CopyTo(array, offset);

    public bool Remove(string uriPrefix) => _prefixes.Remove(uriPrefix);

    public int Count => _prefixes.Count;

    public IEnumerator GetEnumerator() => _prefixes.GetEnumerator();
}