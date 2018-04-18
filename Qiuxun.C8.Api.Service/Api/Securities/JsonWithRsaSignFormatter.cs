using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.Securities
{

    public class JsonWithRsaSignFormatter : JsonMediaTypeFormatter
    {
        public override object ReadFromStream(Type type, Stream readStream, Encoding effectiveEncoding, IFormatterLogger formatterLogger)
        {
            throw new NotImplementedException();
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            if (!(value is ApiResult))
            {
                ApiException exception = new ApiException(9001,"程序错误");
                base.WriteToStream(type, new ApiResult(exception.Code, exception.Desc), writeStream, effectiveEncoding);
            }
            else
            {
                string s = JsonConvert.SerializeObject(value);
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
                {
                    Type type3;
                    Type type2 = value.GetType();
                    provider.FromXmlString("<RSAKeyValue><Modulus>r064mYANMiiUeFUvePpIQchAxv8J/zJERUrNFQ2FV5Q3fSDp1nlxj3dHf8pIZeYl1ivoigY745iV16Nk/2afID3RDPOUNSGaBG5Euc0p6iyV/oiAkg0vH8m46F2tbBvAoGnT05Ia88BDNtCH0BbGHtiDaCFr6JaRX47zsiJGHpM=</Modulus><Exponent>AQAB</Exponent><P>vZ8Dfw8yQn1erY0kpOefwKm+S1YJVE0tLTuNWT5+iTjhfefLha2enWBJekfbPMP+wCX/8hHtDkA8ccrazvDE1w==</P><Q>7Kz71s7+E2/VpVIlGDQaEymGFqJ1MXoQgU3Ox7trzMdcydwmGnUx7gvJNUwjrI0KCctGjxWFLqFHYk8vuXrApQ==</Q><DP>WYgbPoMOWBaZ/ZgHFVXIOE/taeTVwtgt3I2hz+GSHXid/7TSg+vWWLh9+R60hZyFTHSkxMdyBqiN4azGY6+LQQ==</DP><DQ>dbcD+y8wx9IT3QoiUQt4/JbmjlN3HoirtORSOJ1LXKq7x9qrSPWJQ/CwvsWD6Mqtd3mXOotllm+45XilMAeR0Q==</DQ><InverseQ>L4nX3v2J9fl9mcN2Nb+prrYgMJWoLEgF6+w1tjDQKSa2DlgF6C/8esNTtnjLrOkn3DoqKEUonCfP+KTzkShJ4Q==</InverseQ><D>AJOIc5meuJi2qnVFO3WUgayh2sWd3SE81xtAn1c00OaOK0EGjm8pxS9NFlDKswacxeWUnHP954VNcpRc0aKQ1x+reGqLT2q5xQslkXGE9Raef+0MuT9EX9NbRowwiYvmGXOZNuEMM6s0d6GaxHnT5Cdy/09NPBezKfczZRWLLIk=</D></RSAKeyValue>");
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    HashAlgorithm halg = HashAlgorithm.Create("SHA1");
                    string str2 = Convert.ToBase64String(provider.SignData(bytes, halg));
                    if (type2.IsGenericType)
                    {
                        type3 = typeof(RsaLoResult<>).MakeGenericType(new Type[] { type2.GenericTypeArguments[0] });
                    }
                    else
                    {
                        type3 = typeof(RsaLoResult);
                    }
                    object obj2 = type3.GetMethod("CopyFrom").Invoke(null, new object[] { value, str2 });
                    base.WriteToStream(type, obj2, writeStream, effectiveEncoding);
                }
            }
        }
    }
}
