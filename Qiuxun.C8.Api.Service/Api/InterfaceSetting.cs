﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Api
{

    public class InterfaceSetting
    {
        private Dictionary<string, string> blacklistDictionary = new Dictionary<string, string>();
        private List<RequestLimit> otherRequestLimit = new List<RequestLimit>();
        private Dictionary<string, string> whitelistDictionary = new Dictionary<string, string>();

        public Dictionary<string, string> BlacklistDictionary
        {
            get
            {
                return this.blacklistDictionary;
            }
        }

        public bool Disable { get; set; }

        public bool DisableBlacklist { get; set; }

        public bool DisableLog { get; set; }

        public bool DisableWhitelist { get; set; }

        public bool IsRecordResponseData { get; set; }

        public List<RequestLimit> OtherRequestLimit
        {
            get
            {
                return this.otherRequestLimit;
            }
        }

        public RequestLimit RequestLimit { get; set; }

        public string Route { get; set; }

        public Dictionary<string, string> WhitelistDictionary
        {
            get
            {
                return this.whitelistDictionary;
            }
        }
    }
}