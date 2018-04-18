using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 十六进制扩展类
    /// </summary>
    public static class HexExtension
    {
        
        public static string BinaryToHex(this byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            char[] chArray = new char[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                byte num2 = data[i];
                chArray[2 * i] = NibbleToHex((byte)(num2 >> 4));
                chArray[(2 * i) + 1] = NibbleToHex((byte)(num2 & 15));
            }
            return new string(chArray);
        }
        
        public static byte[] HexToBinary(this string data)
        {
            if ((data == null) || ((data.Length % 2) != 0))
            {
                return null;
            }
            byte[] buffer = new byte[data.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                int num2 = HexToInt(data[2 * i]);
                int num3 = HexToInt(data[(2 * i) + 1]);
                if ((num2 == -1) || (num3 == -1))
                {
                    return null;
                }
                buffer[i] = (byte)((num2 << 4) | num3);
            }
            return buffer;
        }

        public static byte[] HexToBinary(this char[] data)
        {
            if ((data == null) || ((data.Length % 2) != 0))
            {
                return null;
            }
            byte[] buffer = new byte[data.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                int num2 = HexToInt(data[2 * i]);
                int num3 = HexToInt(data[(2 * i) + 1]);
                if ((num2 == -1) || (num3 == -1))
                {
                    return null;
                }
                buffer[i] = (byte)((num2 << 4) | num3);
            }
            return buffer;
        }

        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        private static char NibbleToHex(byte nibble)
        {
            return ((nibble < 10) ? ((char)(nibble + 48)) : ((char)((nibble - 10) + 65)));
        }
    }
}
