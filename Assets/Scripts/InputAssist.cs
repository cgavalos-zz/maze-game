namespace InputAssist
{
  public class Stopper
  {
    public bool lockVar;

    public Stopper()
    {
      lockVar = false;
    }

    public bool Update(bool signal)
    {
      if (signal)
      {
        if (lockVar)
        {
          return false;
        }
        else
        {
          lockVar = true;
          return true;
        }
      }
      else
      {
        lockVar = false;
        return false;
      }
    }
  }
}
