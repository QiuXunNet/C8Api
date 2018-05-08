using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.ResourceManagement
{
    public interface IPictureProcess
    {
        IPictureProcess Format(string mimeType);
        string GetResult();
        IPictureProcess Height(int height);
        IPictureProcess LongSide();
        IPictureProcess MidCut();
        IPictureProcess ShortSide();
        IPictureProcess Stretch();
        IPictureProcess Width(int width);
    }
}
