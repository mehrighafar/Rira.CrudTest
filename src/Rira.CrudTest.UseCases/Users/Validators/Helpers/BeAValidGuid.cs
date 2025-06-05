namespace Rira.CrudTest.UseCases.Users.Validators.Helpers;

public static class BeAValidGuid
{
  public static bool IsValid(string guid)
  {
    if (string.IsNullOrEmpty(guid))
      return false;
    return Guid.TryParse(guid, out _);
  }
}
