using SharedSettingsLib.Extensions;
using SharedSettingsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Network
{
  public interface ISMTPMailService
  {
  }
  public class SMTPMailService : ISMTPMailService
  {
    public SMTPMailService()
    {
      this.Log = Serilog.Log.Logger;
    }

    private Serilog.ILogger Log { get; }
    public void SendMail(SMTPSetting sMTPSetting,
      string Email_From, string Email_To, string Email_CC, string Email_Subject, string Email_Body,
      bool UseDefaultCredentials = false, bool EnableSsl = false, bool throwException = false
    )
    {
      var methodInfo = MethodBase.GetCurrentMethod().GetMethodInfo();
      try
      {
        string SMTP_Server = sMTPSetting.SMTPServer!;
        int SMTP_Port = sMTPSetting.SMTPPort?.ToInt16() ?? 587;
        string SMTP_ID = sMTPSetting.SMTP_ID!;
        string SMTP_Pass = sMTPSetting.SMTP_Pass!;      
        using (SmtpClient SmtpServer = new SmtpClient(SMTP_Server))
        {
          using (MailMessage mail = new MailMessage())
          {
            if (Email_To.Contains(';')) //  如果有 ; 分號分隔多筆 Eamil 收件人 _
            {
              foreach (var email in Email_To.Split(';'))
              {
                if (!string.IsNullOrEmpty(email))
                {
                  mail.To.Add(email); // Email 收件人 
                  Log.Information($"收件人 {nameof(email)}: {email}");
                }
              }
            }
            else
            {
              mail.To.Add(Email_To); //Email 收件人 
              Log.Information($"收件人 {nameof(Email_To)}: {Email_To}");
            }
            if (!string.IsNullOrEmpty(Email_CC)) //Email_CC 新增 CC 副本功能 _
            {
              if (Email_CC.Contains(';'))
              {
                foreach (var email in Email_CC.Split(';'))
                {
                  if (!string.IsNullOrEmpty(email))
                  {
                    mail.CC.Add(email); // Email 收件人 
                    Log.Information($"CC 收件人 {nameof(email)}: {email}");
                  }
                }
              }
              else
              {
                mail.CC.Add(Email_CC);
                Log.Information($"CC 收件人 {nameof(Email_CC)}: {Email_CC}");
              }
            }
            mail.From = new MailAddress(Email_From);
            mail.Subject = Email_Subject;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Body = Email_Body;  // 寄件內容
            SmtpServer.Port = SMTP_Port; // 587;
            SmtpServer.UseDefaultCredentials = UseDefaultCredentials; 
            {
              using (var psSecureString = new System.Security.SecureString())
              {
                SmtpServer.Credentials = new System.Net.NetworkCredential(SMTP_ID, SMTP_Pass);
                SmtpServer.EnableSsl = EnableSsl;
                SmtpServer.Send(mail);
              }
            }
          } 
        }
      }
      catch (Exception ex)
      {
        Log.Error($"寄信發生錯誤: ");
        ex.LogError(methodInfo);
        if (throwException) throw; // 是否要拋出例外 _
      }
    }
  }
}
