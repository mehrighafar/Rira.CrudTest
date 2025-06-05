using System.Text.Json.Serialization;

namespace Rira.CrudTest.BDD.Entities;

internal class TestUserEntity
{
  [JsonIgnore]
  public Guid Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string NationalCode { get; set; } = string.Empty;
  public DateOnly DateOfBirth { get; set; } = new DateOnly();

}
