using Microsoft.Extensions.Caching.Memory;

class TypedCache<T>
{
  protected const string _cacheDelimiter = "/";

  public TypedCache(IMemoryCache cache)
  {
    Cache = cache;

    _typeName = typeof(T).FullName ?? throw new ArgumentException($"{nameof(cache)} cannot be a generic type", nameof(cache));
  }

  private readonly string _typeName;

  protected IMemoryCache Cache { get; }

  protected virtual string GetKey(string id) =>
    $"{_typeName}{_cacheDelimiter}{id}";

  public Dictionary<string, T> Get()
  {
    Cache.TryGetValue<Dictionary<string, T>>(_typeName, out var value);

    return value ?? new();
  }

  public T? Get(string id)
  {
    Cache.TryGetValue<T>(GetKey(id), out var value);

    return value;
  }

  public T Set(string id, T value)
  {
    Cache.Set<T>(GetKey(id), value);

    var allItems = Get();
    if (allItems.ContainsKey(id))
    {
      allItems[id] = value;
    }
    else
    {
      allItems.Add(id, value);
    }

    Cache.Set(_typeName, allItems);

    return value;
  }

  public void Remove(string id)
  {
    Cache.Remove(GetKey(id));

    var items = Get();
    items.Remove(id);
    if (items.Count == 0)
    {
      Cache.Remove(_typeName);
    }
  }
}

static class WebApplicationTypedCacheExtensions
{
  public static IServiceCollection AddTypedCache(this IServiceCollection services) =>
    services.AddMemoryCache().AddTransient(typeof(TypedCache<>));
}
