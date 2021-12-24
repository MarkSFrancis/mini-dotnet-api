// TODO Support for async

class ValidateRest<T>
{
  public Func<T, string> GetId { get; }

  public ValidateRest(Func<T, string> getId)
  {
    GetId = getId ?? throw new ArgumentNullException(nameof(getId));
  }

  public object GetAll(Func<object> handler) => handler();

  public object GetById(string id, Func<string, object?> handler)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return Results.NotFound();
    }

    id = id.Trim();
    var value = handler(id);

    return value is null ? Results.NotFound($"Item {id} was not found") : value;
  }

  public object Post(string getByIdRouteName, T value, Func<T, T> handler)
  {
    return Validation.Validate(value, (value) =>
    {
      var id = GetId(value);
      var result = handler(value);

      if (getByIdRouteName is not null)
      {
        return Results.CreatedAtRoute(getByIdRouteName, new { id }, value);
      }
      else
      {
        return Results.StatusCode(201);
      }
    });
  }

  public object Put(string id, T value, Func<string, T?> getCurrentValue, Func<T, T?, object> handler)
  {
    return Validation.Validate(value, (value) =>
    {
      var modelId = GetId(value);
      if (modelId != id)
      {
        return Results.BadRequest("The ID in the body does not match the ID in the route. Please make sure the ID is set correctly in the body");
      }

      var savedValue = getCurrentValue(id);

      if (savedValue is null)
      {
        return Results.NotFound($"Item {id} was not found");
      }

      return handler(value, savedValue);
    });
  }

  public object Delete(string id, Func<string, object> handler) => handler(id);
}
