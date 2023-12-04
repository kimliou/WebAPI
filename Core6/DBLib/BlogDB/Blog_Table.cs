using SharedSettingsLib.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SharedSettingsLib.Extensions;

namespace DBLib.BlogDB
{
  public interface IBlogDB_Table
  {
    public BlogResult Create(BlogResult input, bool throwException = false);
    public List<BlogResult> Read(BlogQuery input);
  }
  public class Blog_Table : IBlogDB_Table
  {
    public Blog_Table(IApplicationDbContext dbContext)
    {
      SqlConnection = dbContext.SqlConnection;
    }
    public SqlConnection? SqlConnection { get; }

    public BlogResult Create(BlogResult input , bool throwException = false)
    {
      try
      {
        var sqlStr = @"
INSERT INTO [dbo].[Blog]
           ([BlogId]
           ,[Url]
           ,[Rating])
     VALUES
           (@BlogId
           ,@Url
           ,@Rating)
";
        return SqlConnection.QueryFirstOrDefault<BlogResult>(sqlStr, input);
      }
      catch (Exception)
      {
        throw;
      }
      
    }
    public List<BlogResult> Read(BlogQuery input)
    {
      try
      {
        var sqlStr = @"SELECT [BlogId]
      ,[Url]
      ,[Rating]
  FROM [dbo].[Blog] ";
      sqlStr += ObjectExtension.GenerateSqlWhere(input);
      return SqlConnection.Query<BlogResult>(sqlStr,input).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
