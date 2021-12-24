using System.ComponentModel.DataAnnotations;

static class Validation
{
  public static object Validate<T>(T value, Func<T, object> inner)
  {
    var result = new List<ValidationResult>();

    if (value is null)
    {
      return Results.BadRequest("A value is required");
    }

    Validator.TryValidateObject(value, new ValidationContext(value), result);

    if (result.Count > 0)
    {
      return Results.BadRequest(result);
    }

    return inner(value);
  }
}
