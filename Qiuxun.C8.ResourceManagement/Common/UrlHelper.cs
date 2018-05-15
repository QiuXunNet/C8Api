using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.ResourceManagement.Common
{

    public class UrlHelper
    {
        private static readonly string[] _allowed_second_domain = new string[] { "com", "net", "org", "gov" };

        public static string BuildUrl(string src1, string src2, params string[] targets)
        {
            List<string> source = new List<string> {
                src1,
                src2
            };
            source.AddRange(targets);
            source.RemoveAll(d => string.IsNullOrWhiteSpace(d));
            if (source.Count <= 1)
            {
                return source.FirstOrDefault<string>();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(src1);
            for (int i = 1; i < source.Count; i++)
            {
                bool flag = source[i - 1].EndsWith("/");
                bool flag2 = source[i].StartsWith("/");
                if (flag && flag2)
                {
                    builder.Append(source[i].TrimStart(new char[] { '/' }));
                }
                else if (!flag && !flag2)
                {
                    builder.Append('/');
                    builder.Append(source[i]);
                }
                else
                {
                    builder.Append(source[i]);
                }
            }
            return builder.ToString();
        }

        public static string GetContentType(string fileName)
        {
            string str = "application/octet-stream";
            if (Path.HasExtension(fileName))
            {
                string str2 = Path.GetExtension(fileName).ToUpper();
                if (string.IsNullOrWhiteSpace(str2))
                {
                    return str;
                }
                if (str2 == ".323")
                {
                    return "text/h323";
                }
                if (str2 == ".ACX")
                {
                    return "application/internet-property-stream";
                }
                if (str2 == ".AI")
                {
                    return "application/postscript";
                }
                if (str2 == ".AIF")
                {
                    return "audio/x-aiff";
                }
                if (str2 == ".AIFC")
                {
                    return "audio/x-aiff";
                }
                if (str2 == ".AIFF")
                {
                    return "audio/x-aiff";
                }
                if (str2 == ".ASF")
                {
                    return "video/x-ms-asf";
                }
                if (str2 == ".SR")
                {
                    return "video/x-ms-asf";
                }
                if (str2 == ".SX")
                {
                    return "video/x-ms-asf";
                }
                if (str2 == ".AU")
                {
                    return "audio/basic";
                }
                if (str2 == ".AVI")
                {
                    return "video/x-msvideo";
                }
                if (str2 == ".AXS")
                {
                    return "application/olescript";
                }
                if (str2 == ".BAS")
                {
                    return "text/plain";
                }
                if (str2 == ".BCPIO")
                {
                    return "application/x-bcpio";
                }
                if (str2 == ".BIN")
                {
                    return "application/octet-stream";
                }
                if (str2 == ".BMP")
                {
                    return "image/bmp";
                }
                if (str2 == ".C")
                {
                    return "text/plain";
                }
                if (str2 == ".CAT")
                {
                    return "application/vnd.ms-pkiseccat";
                }
                if (str2 == ".CDF")
                {
                    return "application/x-cdf";
                }
                if (str2 == ".CER")
                {
                    return "application/x-x509-ca-cert";
                }
                if (str2 == ".CLASS")
                {
                    return "application/octet-stream";
                }
                if (str2 == ".CLP")
                {
                    return "application/x-msclip";
                }
                if (str2 == ".CMX")
                {
                    return "image/x-cmx";
                }
                if (str2 == ".COD")
                {
                    return "image/cis-cod";
                }
                if (str2 == ".CPIO")
                {
                    return "application/x-cpio";
                }
                if (str2 == ".CRD")
                {
                    return "application/x-mscardfile";
                }
                if (str2 == ".CRL")
                {
                    return "application/pkix-crl";
                }
                if (str2 == ".CRT")
                {
                    return "application/x-x509-ca-cert";
                }
                if (str2 == ".CSH")
                {
                    return "application/x-csh";
                }
                if (str2 == ".CSS")
                {
                    return "text/css";
                }
                if (str2 == ".DCR")
                {
                    return "application/x-director";
                }
                if (str2 == ".DER")
                {
                    return "application/x-x509-ca-cert";
                }
                if (str2 == ".DIR")
                {
                    return "application/x-director";
                }
                if (str2 == ".DLL")
                {
                    return "application/x-msdownload";
                }
                if (str2 == ".DMS")
                {
                    return "application/octet-stream";
                }
                if (str2 == ".DOC")
                {
                    return "application/msword";
                }
                if (str2 == ".DOT")
                {
                    return "application/msword";
                }
                if (str2 == ".DVI")
                {
                    return "application/x-dvi";
                }
                if (str2 == ".DXR")
                {
                    return "application/x-director";
                }
                if (str2 == ".EPS")
                {
                    return "application/postscript";
                }
                if (str2 == ".ETX")
                {
                    return "text/x-setext";
                }
                if (str2 == ".EVY")
                {
                    return "application/envoy";
                }
                if (str2 == ".EXE")
                {
                    return "application/octet-stream";
                }
                if (str2 == ".FIF")
                {
                    return "application/fractals";
                }
                if (str2 == ".FLR")
                {
                    return "x-world/x-vrml";
                }
                if (str2 == ".GIF")
                {
                    return "image/gif";
                }
                if (str2 == ".GTAR")
                {
                    return "application/x-gtar";
                }
                if (str2 == ".GZ")
                {
                    return "application/x-gzip";
                }
                if (str2 == ".H")
                {
                    return "text/plain";
                }
                if (str2 == ".HDF")
                {
                    return "application/x-hdf";
                }
                if (str2 == ".HLP")
                {
                    return "application/winhlp";
                }
                if (str2 == ".HQX")
                {
                    return "application/mac-binhex40";
                }
                if (str2 == ".HTA")
                {
                    return "application/hta";
                }
                if (str2 == ".HTC")
                {
                    return "text/x-component";
                }
                if (str2 == ".HTM")
                {
                    return "text/html";
                }
                if (str2 == ".HTML")
                {
                    return "text/html";
                }
                if (str2 == ".HTT")
                {
                    return "text/webviewhtml";
                }
                if (str2 == ".ICO")
                {
                    return "image/x-icon";
                }
                if (str2 == ".IEF")
                {
                    return "image/ief";
                }
                if (str2 == ".III")
                {
                    return "application/x-iphone";
                }
                if (str2 == ".INS")
                {
                    return "application/x-internet-signup";
                }
                if (str2 == ".ISP")
                {
                    return "application/x-internet-signup";
                }
                if (str2 == ".JFIF")
                {
                    return "image/pipeg";
                }
                if (str2 == ".JPG")
                {
                    return "image/jpeg";
                }
                if (str2 == ".JPEG")
                {
                    return "image/jpeg";
                }
                if (str2 == ".PNG")
                {
                    str = "image/png";
                }
            }
            return str;
        }

        public static string GetImageSurfix(string contentType)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                contentType = contentType.ToLower();
                switch (contentType)
                {
                    case "image/bmp":
                        return ".bmp";

                    case "image/gif":
                        return ".gif";

                    case "image/jpeg":
                        return ".jpg";

                    case "image/jpg":
                        return ".jpg";

                    case "image/png":
                        return ".png";
                }
            }
            return string.Empty;
        }

        public static string GetRootDomain(Uri uri)
        {
            if (uri.HostNameType == UriHostNameType.Dns)
            {
                if (uri.IsLoopback)
                {
                    return uri.Host;
                }
                string[] strArray = uri.Host.ToLower().Split(new char[] { '.' });
                int length = strArray.Length;
                string section2 = strArray[length - 2];
                if (_allowed_second_domain.Any<string>(d => d == section2))
                {
                    return string.Format("{0}.{1}.{2}", strArray[length - 3], strArray[length - 2], strArray[length - 1]);
                }
                return string.Format("{0}.{1}", strArray[length - 2], strArray[length - 1]);
            }
            return uri.Host;
        }
    }
}
