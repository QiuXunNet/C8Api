using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Api
{
    public class AesKeyIv
    {
        public AesKeyIv(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.Iv = iv;
        }

        public byte[] Iv { get; set; }

        public byte[] Key { get; set; }
    }
}
