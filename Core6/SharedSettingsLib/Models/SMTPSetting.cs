using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Models
{
  public class SMTPSetting
  {
    /// <summary>
    /// SMTP路徑位址
    /// </summary>
    public string? SMTPServer { get; set; }
    /// <summary>
    /// SMTP Port號
    /// </summary>
    public string? SMTPPort { get; set; }
    /// <summary>
    /// 寄件人帳號
    /// </summary>
    public string? SMTP_ID { get; set; }
    /// <summary>
    /// 寄件人密碼
    /// </summary>
    public string? SMTP_Pass { get; set; }

    public string? SMTPEmailFrom { get; set; }
    /// <summary>
    /// 收件人
    /// </summary>
    public string? SMTPEmailTo { get; set; }
    /// <summary>
    /// Mail 標題
    /// </summary>
    public string? SMTPSubject { get; set; }
    /// <summary>
    /// Mail 內容
    /// </summary>
    public string? SMTPBody { get; set; }

    public bool EnableSsl { get; set; } = false;
  }
}
