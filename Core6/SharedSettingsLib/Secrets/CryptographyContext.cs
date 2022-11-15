using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedSettingsLib.Secrets
{
  public class CryptographyContext
  {
    /// <summary>
    /// 預設的加密金鑰
    /// </summary>
    public readonly static byte[] defaultAesKeyByteArray = { 56, 48, 56, 57, 98, 52, 100, 57, 51, 99, 54, 98, 52, 53, 57, 52, 57, 102, 101, 99, 50, 99, 52, 48, 101, 99, 50, 57, 99, 49, 100, 52 };


    /// <summary>
    /// AES 加密字串
    /// </summary>
    /// <param name="text"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Encrypt(string text, string key = "")
    {
      byte[] iv = new byte[16];
      byte[] array;
      try
      {
        using (Aes aes = Aes.Create())
        {
          aes.Key = defaultAesKeyByteArray; // Encoding.UTF8.GetBytes(defaultAesKey);
          aes.IV = iv;
          ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
          using (MemoryStream ms = new MemoryStream())
          {
            using (CryptoStream cryptostream = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
            {
              using (StreamWriter streamWriter = new StreamWriter((Stream)cryptostream))
              {
                streamWriter.Write(text);
              }
              array = ms.ToArray();
            }
          }
        }
        return Convert.ToBase64String(array);
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// AES 解密字串
    /// </summary>
    /// <param name="text"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Decrypt(string text, string key = "")
    {
      try
      {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(text);
        using (Aes aes = Aes.Create())
        {
          aes.Key = defaultAesKeyByteArray;
          aes.IV = iv;
          ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
          using (MemoryStream ms = new MemoryStream(buffer))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
            {
              using (StreamReader sr = new StreamReader(cryptoStream))
              {
                return sr.ReadToEnd();
              }
            }
          }
        }
      }
      catch (Exception)
      {
        return String.Empty;
      }



    }
  }
}
