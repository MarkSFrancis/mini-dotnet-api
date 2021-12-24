using System.ComponentModel.DataAnnotations;

record Person
{
  [Required]
  public string Name { get; set; } = "";

  [Required]
  public string Id { get; set; } = $"{Guid.NewGuid()}";
  
  [Required]
  public DateTime DateOfBirth { get; set; }
}
