﻿namespace Rira.CrudTest.Core.Exceptions;
public static class ExceptionFilterUtility
{
  public static bool True(Action action)
  {
    action();
    return true;
  }

  public static bool False(Action action)
  {
    action();
    return false;
  }
}
