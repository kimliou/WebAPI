using DBLib.BlogDB;
using SharedSettingsLib.Attributes;
using SharedSettingsLib.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLLib.Services.DB
{
  public interface IBlogServices
  {
    public BlogResult Create(BlogResult blogResult);
  }
  [Inject]
  public class BlogServices : IBlogServices
  {
    public BlogServices(IBlogDB_Table blogDB_Table)
    {
      _blogDB = blogDB_Table;
    }
    private IBlogDB_Table _blogDB;
    public BlogResult Create(BlogResult blogResult)
    {
      return _blogDB.Create(blogResult);
    }
  }
}
