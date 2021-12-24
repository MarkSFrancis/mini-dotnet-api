static class WebApplicationExtensions
{
  public static void MapEndpoints(this IApplicationBuilder app, string pathMatch, Action<IEndpointRouteBuilder> configure)
  {
    app.Map(pathMatch, appMapped => appMapped.UseRouting().UseEndpoints(configure));
  }
}
