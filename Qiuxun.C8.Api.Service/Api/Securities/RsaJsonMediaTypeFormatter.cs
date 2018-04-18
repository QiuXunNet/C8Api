using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.Securities
{

    public class RsaJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public RsaJsonMediaTypeFormatter()
        {
            base.SupportedMediaTypes.Clear();
            base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/jsone"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        private RSAData Decrypt(byte[] data)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString("<RSAKeyValue><Modulus>r064mYANMiiUeFUvePpIQchAxv8J/zJERUrNFQ2FV5Q3fSDp1nlxj3dHf8pIZeYl1ivoigY745iV16Nk/2afID3RDPOUNSGaBG5Euc0p6iyV/oiAkg0vH8m46F2tbBvAoGnT05Ia88BDNtCH0BbGHtiDaCFr6JaRX47zsiJGHpM=</Modulus><Exponent>AQAB</Exponent><P>vZ8Dfw8yQn1erY0kpOefwKm+S1YJVE0tLTuNWT5+iTjhfefLha2enWBJekfbPMP+wCX/8hHtDkA8ccrazvDE1w==</P><Q>7Kz71s7+E2/VpVIlGDQaEymGFqJ1MXoQgU3Ox7trzMdcydwmGnUx7gvJNUwjrI0KCctGjxWFLqFHYk8vuXrApQ==</Q><DP>WYgbPoMOWBaZ/ZgHFVXIOE/taeTVwtgt3I2hz+GSHXid/7TSg+vWWLh9+R60hZyFTHSkxMdyBqiN4azGY6+LQQ==</DP><DQ>dbcD+y8wx9IT3QoiUQt4/JbmjlN3HoirtORSOJ1LXKq7x9qrSPWJQ/CwvsWD6Mqtd3mXOotllm+45XilMAeR0Q==</DQ><InverseQ>L4nX3v2J9fl9mcN2Nb+prrYgMJWoLEgF6+w1tjDQKSa2DlgF6C/8esNTtnjLrOkn3DoqKEUonCfP+KTzkShJ4Q==</InverseQ><D>AJOIc5meuJi2qnVFO3WUgayh2sWd3SE81xtAn1c00OaOK0EGjm8pxS9NFlDKswacxeWUnHP954VNcpRc0aKQ1x+reGqLT2q5xQslkXGE9Raef+0MuT9EX9NbRowwiYvmGXOZNuEMM6s0d6GaxHnT5Cdy/09NPBezKfczZRWLLIk=</D></RSAKeyValue>");
                int count = provider.KeySize / 8;
                string str = "";
                using (MemoryStream stream = new MemoryStream(data))
                {
                    using (MemoryStream stream2 = new MemoryStream())
                    {
                        while (true)
                        {
                            byte[] buffer2;
                            byte[] buffer = new byte[count];
                            int num2 = stream.Read(buffer, 0, count);
                            if (num2 <= 0)
                            {
                                break;
                            }
                            if (num2 == count)
                            {
                                buffer2 = buffer;
                            }
                            else
                            {
                                buffer2 = new byte[num2];
                                Buffer.BlockCopy(buffer, 0, buffer2, 0, num2);
                            }
                            byte[] buffer3 = provider.Decrypt(buffer2, false);
                            stream2.Write(buffer3, 0, buffer3.Length);
                        }
                        byte[] bytes = stream2.ToArray();
                        str = Encoding.UTF8.GetString(bytes);
                    }
                }
                return JsonConvert.DeserializeObject<RSAData>(str);
            }
        }

        protected virtual byte[] GetRequestData(Stream readStream)
        {
            int length = (int)readStream.Length;
            byte[] buffer = new byte[length];
            readStream.Read(buffer, 0, length);
            return buffer;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            Task<object> task;
            try
            {
                byte[] requestData = this.GetRequestData(readStream);
                RSAData data = this.Decrypt(requestData);
                content.Headers.Add("_resp_aes_key", data.k);
                content.Headers.Add("_resp_aes_iv", data.i);
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data.d));
                task = base.ReadFromStreamAsync(type, stream, content, formatterLogger);
            }
            catch (Exception exception)
            {
                throw new ApiException(exception);
            }
            return task;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            throw new NotImplementedException();
        }
    }
}
