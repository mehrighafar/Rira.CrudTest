using System.ComponentModel.DataAnnotations;

namespace Rira.CrudTest.Core.UserAggregate.Entities;

public class User
{
  [Required]
  public Guid Id { get; set; }

  [Required]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  public string LastName { get; set; } = string.Empty;

  [Required]
  public string NationalCode { get; set; } = string.Empty;

  [Required]
  public DateOnly DateOfBirth { get; set; } = new DateOnly();
}
