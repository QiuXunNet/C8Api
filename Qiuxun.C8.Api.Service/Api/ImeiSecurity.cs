using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.ClintSecurities
{
    internal class ImeiSecurity
    {
        private static readonly byte[] _aes_iv = "29341F85E6CD29FA619E9DFB3CD1CFEB".HexToBinary();
        private const string _aes_iv_str = "29341F85E6CD29FA619E9DFB3CD1CFEB";
        private static readonly byte[] _aes_key = "4BBFCE5DCC90A2C34068AC4157C4BA90C02076F15F538CB442DCD41EECB9D0C3".HexToBinary();
        private const string _aes_key_str = "4BBFCE5DCC90A2C34068AC4157C4BA90C02076F15F538CB442DCD41EECB9D0C3";
        internal const int _float_precision = 10000000;
        private static readonly DateTime _init_time = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        private static long ByteArrayToLong(byte[] bytes, bool isLittleEndian = false)
        {
            long num = 0L;
            if (isLittleEndian)
            {
                for (int j = 0; j < 8; j++)
                {
                    int num3 = 8 * j;
                    num += (bytes[j] & 0xff) << num3;
                }
                return num;
            }
            for (int i = 0; i < 8; i++)
            {
                int num5 = (7 - i) * 8;
                num += (bytes[i] & 255) << num5;
            }
            return num;
        }

        internal static RequestImeiDto Decrypt(string encryptedImei, string url, string userAgent, double lng, double lat)
        {
            RequestImeiDto dto = new RequestImeiDto(encryptedImei);
            try
            {
                if (string.IsNullOrEmpty(encryptedImei))
                {
                    dto.IsFake = true;
                    return dto;
                }
                byte[] src = null;
                src = AESHelper.DecryptBytes(encryptedImei.HexToBinary(), _aes_key, _aes_iv);
                url = (url == null) ? "" : url.ToLower().Trim();
                userAgent = (userAgent == null) ? "" : userAgent.ToLower().Trim();
                int num = (int)(lng * 10000000.0);
                int num2 = (int)(lat * 10000000.0);
                string str = string.Format("{0}{1}{2}", url, num, num2);
                string str2 = string.Format("{0}{1}{2}", userAgent, num, num2);
                int num3 = Math.Abs(GetHashCode2(str));
                int num4 = Math.Abs(GetHashCode2(str2));
                DateTime time = _init_time.AddSeconds((double)-num3);
                int length = src.Length;
                int num6 = length - 6;
                int key = num3 % num6;
                int num8 = num4 % num6;
                int num9 = 2;
                byte[] dst = new byte[8];
                byte[] buffer3 = new byte[num6];
                KeyValuePair<int, int>[] pairArray = new KeyValuePair<int, int>[2];
                if (key <= num8)
                {
                    pairArray[0] = new KeyValuePair<int, int>(key, 0);
                    pairArray[1] = new KeyValuePair<int, int>(num8, 1);
                }
                else
                {
                    pairArray[0] = new KeyValuePair<int, int>(num8, 1);
                    pairArray[1] = new KeyValuePair<int, int>(key, 0);
                }
                int srcOffset = 0;
                int dstOffset = 0;
                for (int i = 0; i < num9; i++)
                {
                    int num13 = pairArray[i].Key + (i * 3);
                    int num14 = pairArray[i].Value;
                    int num15 = num13 - srcOffset;
                    if (num15 != 0)
                    {
                        Buffer.BlockCopy(src, srcOffset, buffer3, dstOffset, num15);
                        dstOffset += num15;
                        srcOffset += num15;
                    }
                    Buffer.BlockCopy(src, num13, dst, num14 * 3, 3);
                    srcOffset += 3;
                }
                int count = length - srcOffset;
                if (count != 0)
                {
                    Buffer.BlockCopy(src, srcOffset, buffer3, dstOffset, count);
                }
                long num17 = BitConverter.ToInt64(dst, 0);
                dto.GenerateTime = new DateTime?(time.AddMilliseconds((double)num17).ToLocalTime());
                dto.RealImei = buffer3.BinaryToHex();
                dto.IsFake = false;
                return dto;
            }
            catch
            {
                dto.IsFake = true;
                return dto;
            }
        }

        internal static string Encrypt(string imei, string url, string userAgent, double lng, double lat)
        {
            url = url.ToLower().Trim();
            userAgent = userAgent.ToLower().Trim();
            int num = (int)(lng * 10000000.0);
            int num2 = (int)(lat * 10000000.0);
            string str = string.Format("{0}{1}{2}", url, num, num2);
            string str2 = string.Format("{0}{1}{2}", userAgent, num, num2);
            int num3 = Math.Abs(GetHashCode2(str));
            int num4 = Math.Abs(GetHashCode2(str2));
            DateTime time = _init_time.AddSeconds((double)-num3);
            TimeSpan span = (TimeSpan)(DateTime.Now - time);
            long totalMilliseconds = (long)span.TotalMilliseconds;
            byte[] buffer = LongToByteArray(totalMilliseconds, true);
            byte[] buffer2 = imei.HexToBinary();
            int length = buffer2.Length;
            int key = num3 % length;
            byte[] buffer3 = new byte[] { buffer[0], buffer[1], buffer[2] };
            KeyValuePair<int, byte[]> pair = new KeyValuePair<int, byte[]>(key, buffer3);
            int num8 = num4 % length;
            byte[] buffer4 = new byte[] { buffer[3], buffer[4], buffer[5] };
            KeyValuePair<int, byte[]> pair2 = new KeyValuePair<int, byte[]>(num8, buffer4);
            int num9 = 2;
            byte[] bytes = new byte[length + 6];
            KeyValuePair<int, byte[]>[] pairArray = new KeyValuePair<int, byte[]>[num9];
            if (key <= num8)
            {
                pairArray[0] = pair;
                pairArray[1] = pair2;
            }
            else
            {
                pairArray[0] = pair2;
                pairArray[1] = pair;
            }
            int index = 0;
            int num11 = 0;
            for (int i = 0; i < num9; i++)
            {
                int num13 = pairArray[i].Key;
                byte[] buffer6 = pairArray[i].Value;
                int num14 = num13 - num11;
                if (num14 != 0)
                {
                    for (int m = 0; m < num14; m++)
                    {
                        bytes[index] = buffer2[num11];
                        num11++;
                        index++;
                    }
                }
                for (int k = 0; k < buffer6.Length; k++)
                {
                    bytes[index] = buffer6[k];
                    index++;
                }
            }
            int num17 = length - num11;
            for (int j = 0; j < num17; j++)
            {
                bytes[index] = buffer2[num11];
                num11++;
                index++;
            }
            return AESHelper.EncryptBytes(bytes, _aes_key, _aes_iv).BinaryToHex();
        }

        private static int GetHashCode2(string str)
        {
            int num = 0;
            string str2 = str;
            for (int i = 0; i < str2.Length; i++)
            {
                num = (0x1f * num) + str2[i];
            }
            return num;
        }

        private static byte[] LongToByteArray(long i, bool isLittleEndian = false)
        {
            byte[] buffer = new byte[8];
            if (isLittleEndian)
            {
                buffer[0] = (byte)(i & 0xffL);
                buffer[1] = (byte)((i >> 8) & 0xffL);
                buffer[2] = (byte)((i >> 0x10) & 0xffL);
                buffer[3] = (byte)((i >> 0x18) & 0xffL);
                buffer[4] = (byte)((i >> 0x20) & 0xffL);
                buffer[5] = (byte)((i >> 40) & 0xffL);
                buffer[6] = (byte)((i >> 0x30) & 0xffL);
                buffer[7] = (byte)((i >> 0x38) & 0xffL);
                return buffer;
            }
            buffer[0] = (byte)((i >> 0x38) & 0xffL);
            buffer[1] = (byte)((i >> 0x30) & 0xffL);
            buffer[2] = (byte)((i >> 40) & 0xffL);
            buffer[3] = (byte)((i >> 0x20) & 0xffL);
            buffer[4] = (byte)((i >> 0x18) & 0xffL);
            buffer[5] = (byte)((i >> 0x10) & 0xffL);
            buffer[6] = (byte)((i >> 8) & 0xffL);
            buffer[7] = (byte)(i & 0xffL);
            return buffer;
        }

    }
}
