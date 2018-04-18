using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Common
{

    public class ServerRoles
    {
        private static ServerRoles _instance;
        private static object _lock = new object();
        private static string _serverRolesStr = ConfigurationManager.AppSettings["ServerRoles"];

        private ServerRoles()
        {
            this.All = (from d in _serverRolesStr.Split(new char[] { ',' }) select d.Trim().ParseTo<ServerRoleEnum>()).ToList<ServerRoleEnum>();
            this.All.RemoveAll(d => d == 0);
            if (this.All.Contains(ServerRoleEnum.LocalServer))
            {
                this.IsLocalServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.AliServer))
            {
                this.IsAliServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.ApiServer))
            {
                this.IsApiServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.BackEndServer))
            {
                this.IsBackEndServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.DevServer))
            {
                this.IsDevServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.TestServer))
            {
                this.IsTestServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.PrePublishServer))
            {
                this.IsPrePublishServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.FormalServer))
            {
                this.IsFormalServer = true;
            }
            if (this.All.Contains(ServerRoleEnum.StressTestServer))
            {
                this.IsStressTestServer = true;
            }
        }

        public List<ServerRoleEnum> All { get; set; }

        public static ServerRoles Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServerRoles();
                        }
                    }
                }
                return _instance;
            }
        }

        public bool IsAliServer { get; private set; }

        public bool IsApiServer { get; private set; }

        public bool IsBackEndServer { get; private set; }

        public bool IsDevServer { get; private set; }

        public bool IsFormalServer { get; private set; }

        public bool IsLocalServer { get; private set; }

        public bool IsPrePublishServer { get; private set; }

        public bool IsStressTestServer { get; private set; }

        public bool IsTestServer { get; private set; }
    }
}
