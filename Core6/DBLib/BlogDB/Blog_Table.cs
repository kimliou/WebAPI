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
    public BlogResult Create(BlogResult input);
    public List<BlogResult> Read(BlogQuery input);
  }
  public class Blog_Table : IBlogDB_Table
  {
    public Blog_Table(IApplicationDbContext dbContext)
    {
      SqlConnection = dbContext.SqlConnection;
    }
    public SqlConnection? SqlConnection { get; }

    public BlogResult Create(BlogResult input)
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
      return SqlConnection.QueryFirstOrDefault<BlogResult>(sqlStr,input);
    }
    public List<BlogResult> Read(BlogQuery input)
    {
      //建議把所有欄位打出來，原因是效能問題
      var sqlStr = @"SELECT [BlogId]
      ,[Url]
      ,[Rating]
  FROM [dbo].[Blog] ";
      sqlStr += ObjectExtension.GenerateSqlWhere(input);
      return SqlConnection.Query<BlogResult>(sqlStr,input).ToList();
    }
  }
}
