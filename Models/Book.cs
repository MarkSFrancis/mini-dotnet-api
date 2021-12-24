using System.ComponentModel.DataAnnotations;

record Book
{
  [Required]
  public string Title { get; set; } = "";

  [Required]
  public string Id { get; set; } = $"{Guid.NewGuid()}";

  ///
  /// <summary>Publication date. This could be null if the book is not published, or the publication date is not known (such as historical articles)</summary>
  ///
  public DateTime? PublicationDate { get; set; }

  [Required]
  public bool IsPublished { get; set; }
}
