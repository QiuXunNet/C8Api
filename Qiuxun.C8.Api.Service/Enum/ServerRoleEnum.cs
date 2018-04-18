using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Enum
{
    [Flags]
    public enum ServerRoleEnum
    {
        AliServer = 2,
        ApiServer = 8,
        BackEndServer = 9,
        DevServer = 16,
        FormalServer = 128,
        LocalServer = 1,
        PrePublishServer = 64,
        StressTestServer = 256,
        TestServer = 32
    }
}
