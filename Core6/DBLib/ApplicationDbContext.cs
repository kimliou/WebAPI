using Microsoft.EntityFrameworkCore;
using SharedSettingsLib;
using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLib
{
  public interface IApplicationDbContext
  {
    public SqlConnection? SqlConnection { get; }
  }
  public class ApplicationDbContext :IApplicationDbContext
  {
    public ApplicationDbContext(
     SqlConnection SqlConnection,
     AppSettings appSettings
    )
    {
      this.SqlConnection = SqlConnection;
      SqlConnection.ConnectionString = appSettings.GetDefaultConnectionString();
    }
    public SqlConnection? SqlConnection { get; }
  }
}
