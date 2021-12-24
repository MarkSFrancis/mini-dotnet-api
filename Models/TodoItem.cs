using System.ComponentModel.DataAnnotations;

record Todo
{
  [Required]
  public string Name { get; set; } = "";

  [Required]
  public string Id { get; set; } = $"{Guid.NewGuid()}";

  public DateTime? CompletedOn { get; set; }
}
