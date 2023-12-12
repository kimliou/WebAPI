using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models.DB
{
  public class UsersResult
  {
    /// <summary>
    /// 用戶編號
    /// </summary>
    public string UserID { get; set; } = string.Empty;
    /// <summary>
    /// 用戶名稱
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// 手機號碼
    /// </summary>
    public string? UserMobile { get; set; }
    /// <summary>
    /// 郵件
    /// </summary>
    public string? UserEmail { get; set; }
    /// 是否登入系統(Y/N)
    /// </summary>
    public string? IsLogon { get; set; }
    /// <summary>
    /// 登入系統時間
    /// </summary>
    public DateTime? LogonTime { get; set; }
    /// <summary>
    /// 更新日期時間
    /// </summary>
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 最後更新人員
    /// </summary>
    public string UpdateUser { get; set; } = string.Empty;
  }
}
