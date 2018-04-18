using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Auth
{
    internal class PermissionHelper
    {
        internal static TokenInfo DecryptToken(string token)
        {
            byte[] buffer = AESHelper.DecryptBytes(token.HexToBinary(), AuthSettings._aes_key_token, AuthSettings._aes_iv_token);
            if (buffer.Length == 12)
            {
                return new TokenInfo { UserCode = BitConverter.ToInt32(buffer, 0), AuthTime = DateTime.FromBinary(BitConverter.ToInt64(buffer, 4)) };
            }
            return new TokenInfo { UserCode = BitConverter.ToInt64(buffer, 0), AuthTime = DateTime.FromBinary(BitConverter.ToInt64(buffer, 8)) };
        }

        internal static string EncryptToken(long userCode, DateTime authTime)
        {
            return AESHelper.EncryptBytes(BitConverter.GetBytes(userCode).Concat<byte>(BitConverter.GetBytes(authTime.ToBinary())).ToArray<byte>(), AuthSettings._aes_key_token, AuthSettings._aes_iv_token).BinaryToHex();
        }
    }
}
