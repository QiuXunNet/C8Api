using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public class AESHelper
    {
        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if ((cipherText == null) || (cipherText.Length <= 0))
            {
                throw new ArgumentNullException("cipherText");
            }
            if ((Key == null) || (Key.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateDecryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream(cipherText))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static byte[] DecryptBytes(byte[] bytes, byte[] Key, byte[] IV)
        {
            if ((bytes == null) || (bytes.Length <= 0))
            {
                throw new ArgumentNullException("bytes");
            }
            if ((Key == null) || (Key.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length <= 0))
            {
                throw new ArgumentNullException("IV");
            }
            byte[] buffer = new byte[0x400];
            List<byte> list = new List<byte>();
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateDecryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                    {
                        int count = stream2.Read(buffer, 0, 0x400);
                        list.AddRange(buffer.Take<byte>(count));
                        while (count == 0x400)
                        {
                            buffer = new byte[0x400];
                            count = stream2.Read(buffer, 0, 0x400);
                            if (count <= 0)
                            {
                                goto Label_0106;
                            }
                            list.AddRange(buffer.Take<byte>(count));
                        }
                    }
                }
            }
            Label_0106:
            return list.ToArray();
        }

        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            if ((plainText == null) || (plainText.Length <= 0))
            {
                throw new ArgumentNullException("plainText");
            }
            if ((Key == null) || (Key.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateEncryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream2))
                        {
                            writer.Write(plainText);
                        }
                        return stream.ToArray();
                    }
                }
            }
        }

        public static byte[] EncryptBytes(byte[] bytes, byte[] Key, byte[] IV)
        {
            byte[] buffer;
            if ((bytes == null) || (bytes.Length <= 0))
            {
                throw new ArgumentNullException("bytes");
            }
            if ((Key == null) || (Key.Length <= 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null) || (IV.Length <= 0))
            {
                throw new ArgumentNullException("IV");
            }
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Key;
                managed.IV = IV;
                ICryptoTransform transform = managed.CreateEncryptor(managed.Key, managed.IV);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        stream2.Write(bytes, 0, bytes.Length);
                    }
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        public static Dictionary<string, byte[]> GenerateKeyAndIV()
        {
            Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.GenerateKey();
                managed.GenerateIV();
                dictionary.Add("key", managed.Key);
                dictionary.Add("iv", managed.IV);
            }
            return dictionary;
        }
    }
}
