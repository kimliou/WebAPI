using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLLib.Services;

public interface ILockService
{
  void Start();
}
[Inject]
public class LockService : ILockService
{
  public static object executionObject = new object();
  public void Start()
  {
    lock (executionObject)
    {
      CreateSingleton();
    }
  }
  public void CreateSingleton()
  {

  }
}