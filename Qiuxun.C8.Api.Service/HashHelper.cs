using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service
{

    public class HashHelper
    {
        public static string Encrypt(HashCryptoType format, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("The input can not be none.");
            }
            if (format == HashCryptoType.None)
            {
                return input;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Encrypt(format, bytes, null).BinaryToHex();
        }

        public static byte[] Encrypt(HashCryptoType format, byte[] input, byte[] salt)
        {
            if ((input == null) || (input.Length == 0))
            {
                throw new ArgumentException("The input can not be none.");
            }
            if (format == HashCryptoType.None)
            {
                return input;
            }
            return HashEncrypt(format, input, salt);
        }

        public static string Encrypt(HashCryptoType format, string input, string salt, StrToBytesType saltToBytesType = StrToBytesType.Hex)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("The input can not be none.");
            }
            if (format == HashCryptoType.None)
            {
                return input;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] buffer2 = null;
            if (!string.IsNullOrEmpty(salt))
            {
                switch (saltToBytesType)
                {
                    case StrToBytesType.UTF8:
                        buffer2 = Encoding.UTF8.GetBytes(salt);
                        goto Label_0074;

                    case StrToBytesType.Hex:
                        buffer2 = salt.HexToBinary();
                        goto Label_0074;

                    case StrToBytesType.Base64:
                        buffer2 = Convert.FromBase64String(salt);
                        goto Label_0074;
                }
                throw new ArgumentException("Can't resolve saltToBytesType.", "saltToBytesType");
            }
            Label_0074:
            return HashEncrypt(format, bytes, buffer2).BinaryToHex();
        }

        public static byte[] GenerateCryptKey(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("length must greater than zero");
            }
            byte[] data = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(data);
            return data;
        }

        private static byte[] HashEncrypt(HashCryptoType format, byte[] input, byte[] salt)
        {
            byte[] buffer2;
            HashAlgorithm algorithm = HashAlgorithm.Create(format.ToString());
            if (algorithm is KeyedHashAlgorithm)
            {
                KeyedHashAlgorithm algorithm2 = (KeyedHashAlgorithm)algorithm;
                if ((salt != null) && (salt.Length > 0))
                {
                    algorithm2.Key = salt;
                }
                return algorithm2.ComputeHash(input);
            }
            if ((salt != null) && (salt.Length > 0))
            {
                buffer2 = new byte[input.Length + salt.Length];
                Buffer.BlockCopy(input, 0, buffer2, 0, input.Length);
                Buffer.BlockCopy(salt, 0, buffer2, input.Length, salt.Length);
            }
            else
            {
                buffer2 = input;
            }
            return algorithm.ComputeHash(buffer2);
        }
    }
}
