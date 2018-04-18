using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Api.Securities
{
    internal static class WebSecuritySettings
    {
        internal static readonly MediaTypeHeaderValue _media_type_json = new MediaTypeHeaderValue("application/json");
        internal static readonly MediaTypeHeaderValue _media_type_json_encrypt = new MediaTypeHeaderValue("application/jsone");
        internal const string _media_type_json_encrypt_str = "application/jsone";
        internal static readonly MediaTypeHeaderValue _media_type_json_encrypt_text = new MediaTypeHeaderValue("application/jsonet");
        internal const string _media_type_json_encrypt_text_str = "application/jsonet";
        internal const string _media_type_json_str = "application/json";
        internal const string _privateKey = "<RSAKeyValue><Modulus>r064mYANMiiUeFUvePpIQchAxv8J/zJERUrNFQ2FV5Q3fSDp1nlxj3dHf8pIZeYl1ivoigY745iV16Nk/2afID3RDPOUNSGaBG5Euc0p6iyV/oiAkg0vH8m46F2tbBvAoGnT05Ia88BDNtCH0BbGHtiDaCFr6JaRX47zsiJGHpM=</Modulus><Exponent>AQAB</Exponent><P>vZ8Dfw8yQn1erY0kpOefwKm+S1YJVE0tLTuNWT5+iTjhfefLha2enWBJekfbPMP+wCX/8hHtDkA8ccrazvDE1w==</P><Q>7Kz71s7+E2/VpVIlGDQaEymGFqJ1MXoQgU3Ox7trzMdcydwmGnUx7gvJNUwjrI0KCctGjxWFLqFHYk8vuXrApQ==</Q><DP>WYgbPoMOWBaZ/ZgHFVXIOE/taeTVwtgt3I2hz+GSHXid/7TSg+vWWLh9+R60hZyFTHSkxMdyBqiN4azGY6+LQQ==</DP><DQ>dbcD+y8wx9IT3QoiUQt4/JbmjlN3HoirtORSOJ1LXKq7x9qrSPWJQ/CwvsWD6Mqtd3mXOotllm+45XilMAeR0Q==</DQ><InverseQ>L4nX3v2J9fl9mcN2Nb+prrYgMJWoLEgF6+w1tjDQKSa2DlgF6C/8esNTtnjLrOkn3DoqKEUonCfP+KTzkShJ4Q==</InverseQ><D>AJOIc5meuJi2qnVFO3WUgayh2sWd3SE81xtAn1c00OaOK0EGjm8pxS9NFlDKswacxeWUnHP954VNcpRc0aKQ1x+reGqLT2q5xQslkXGE9Raef+0MuT9EX9NbRowwiYvmGXOZNuEMM6s0d6GaxHnT5Cdy/09NPBezKfczZRWLLIk=</D></RSAKeyValue>";
        internal const string _publicKey = "<RSAKeyValue><Modulus>r064mYANMiiUeFUvePpIQchAxv8J/zJERUrNFQ2FV5Q3fSDp1nlxj3dHf8pIZeYl1ivoigY745iV16Nk/2afID3RDPOUNSGaBG5Euc0p6iyV/oiAkg0vH8m46F2tbBvAoGnT05Ia88BDNtCH0BbGHtiDaCFr6JaRX47zsiJGHpM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        internal const string _resp_aes_iv = "_resp_aes_iv";
        internal const string _resp_aes_key = "_resp_aes_key";
    }
}
