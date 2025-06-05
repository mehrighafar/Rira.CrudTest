namespace Rira.CrudTest.UseCases.Users.Validators.Helpers;

public static class IranianNationalCodeHelper
{
  public static bool IsValid(string code)
  {
    if (string.IsNullOrWhiteSpace(code) || code.Length != 10 || !long.TryParse(code, out _))
      return false;

    if (new string(code[0], 10) == code)
      return false;

    var sum = 0;
    for (int i = 0; i < 9; i++)
      sum += (code[i] - '0') * (10 - i);

    var remainder = sum % 11;
    var checkDigit = code[9] - '0';

    return remainder < 2 ? checkDigit == remainder : checkDigit == (11 - remainder);
  }
}

