static class CachedRest
{
  public static void MapCachedRest<T>(this IEndpointRouteBuilder routes, Func<T, string> getId, string resourceTypeId)
  {
    if (getId is null)
    {
      throw new ArgumentNullException(nameof(getId));
    }

    var rest = new ValidateRest<T>(getId);
    var getSingleRouteName = $"{resourceTypeId}-GET";

    MapCachedGetAll<T>(routes);

    MapCachedGetById(routes, rest)
      .WithName(getSingleRouteName);

    MapCachedPost(routes, rest, getSingleRouteName);

    MapCachedPut(routes, rest);

    MapCachedDelete<T>(routes);
  }

  public static RouteHandlerBuilder MapCachedGetAll<T>(this IEndpointRouteBuilder routes) =>
    routes.MapGet("", (TypedCache<T> cache) => cache.Get());

  public static RouteHandlerBuilder MapCachedGetById<T>(this IEndpointRouteBuilder routes, ValidateRest<T> rest) =>
    routes
      .MapGet("{id}", (TypedCache<T> cache, string id) =>
        rest.GetById(id, (id) => cache.Get(id)));

  public static RouteHandlerBuilder MapCachedPost<T>(this IEndpointRouteBuilder routes, ValidateRest<T> rest, string getByIdRouteName) =>
    routes
      .MapPost("", (TypedCache<T> cache, T value) =>
        rest.Post(getByIdRouteName, value, (value) =>
        {
          var id = rest.GetId(value);
          cache.Set(id, value);
          return value;
        })
    );

  public static RouteHandlerBuilder MapCachedPut<T>(this IEndpointRouteBuilder routes, ValidateRest<T> rest) =>
    routes
      .MapPut("{id}", (TypedCache<T> cache, string id, T value) =>
        rest.Put(id, value, (id) => cache.Get(id), (value, saved) =>
        {
          cache.Set(id, value);
          return Results.Ok(value);
        }));

  public static RouteHandlerBuilder MapCachedDelete<T>(this IEndpointRouteBuilder routes) =>
    routes.MapDelete("{id}", (TypedCache<T> cache, string id) => cache.Remove(id));
}
