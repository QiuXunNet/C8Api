using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.ResourceManagement
{

    public class AliOssPictureProcess : IPictureProcess
    {
        private StringBuilder _processBuilder = new StringBuilder();

        public AliOssPictureProcess()
        {
            this._processBuilder.Append("@");
        }

        public IPictureProcess Format(string mimeType)
        {
            string str = "";
            string str2 = mimeType;
            if (str2 != null)
            {
                if (!(str2 == "image/gif"))
                {
                    if (str2 == "image/jpeg")
                    {
                        str = ".jpg";
                        goto Label_0072;
                    }
                    if (str2 == "image/png")
                    {
                        str = ".png";
                        goto Label_0072;
                    }
                    if (str2 == "image/bmp")
                    {
                        str = ".bmp";
                        goto Label_0072;
                    }
                }
                else
                {
                    str = ".gif";
                    goto Label_0072;
                }
            }
            throw new NotSupportedException(string.Format("不支持的图片类型：{0}", mimeType));
            Label_0072:
            this._processBuilder.Append(str);
            return this;
        }

        public string GetResult()
        {
            string str = this._processBuilder.ToString();
            if (str == "@")
            {
                return string.Empty;
            }
            return str;
        }

        public IPictureProcess Height(int height)
        {
            this._processBuilder.Append(height);
            this._processBuilder.Append("h_");
            return this;
        }

        public IPictureProcess LongSide()
        {
            this._processBuilder.Append("0e_");
            return this;
        }

        public IPictureProcess MidCut()
        {
            this._processBuilder.Append("1c_");
            return this;
        }

        public IPictureProcess ShortSide()
        {
            this._processBuilder.Append("1e_");
            return this;
        }

        public IPictureProcess Stretch()
        {
            this._processBuilder.Append("2e_");
            return this;
        }

        public IPictureProcess Width(int width)
        {
            this._processBuilder.Append(width);
            this._processBuilder.Append("w_");
            return this;
        }
    }
}
