using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qiuxun.C8.Api.Service.Common
{


    [Serializable, JsonConverter(typeof(ApiVersionConverter))]
    public sealed class ApiVersion : IComparable, IComparable<ApiVersion>
    {
        private const uint _buildMax = 0x7fffffff;
        private uint _buildVersion;
        private long _fullVersion;
        private uint _mainVersion;
        private const uint _majorMax = 0xff;
        private static uint _mask15 = 0x7fff;
        private static uint _mask23 = 0x7fffff;
        private const uint _minorMax = 0xff;
        private const uint _patchMax = 0x7fff;
        private uint[] _sections;
        public static readonly ApiVersion Empty = new ApiVersion(0L);

        public ApiVersion() : this((long)0L)
        {
        }

        public ApiVersion(long verNum)
        {
            this._sections = new uint[4];
            if (verNum != 0L)
            {
                this._mainVersion = (uint)(verNum >> 0x20);
                this._buildVersion = (uint)verNum;
                this._fullVersion = verNum;
                this._sections[0] = this._mainVersion >> 0x17;
                this._sections[1] = (this._mainVersion & _mask23) >> 15;
                this._sections[2] = this._mainVersion & _mask15;
                this._sections[3] = this._buildVersion;
            }
        }

        public ApiVersion(string verStr)
        {
            this._sections = new uint[4];
            try
            {
                string[] strArray = verStr.Split(new char[] { '.' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray.Length <= i)
                    {
                        break;
                    }
                    this._sections[i] = uint.Parse(strArray[i]);
                }
                this._mainVersion = ((this._sections[0] << 0x17) | (this._sections[1] << 15)) | this._sections[2];
                this._buildVersion = this._sections[3];
                this._fullVersion = (this._mainVersion << 0x20) | this._buildVersion;
            }
            catch (Exception)
            {
                this._mainVersion = 0;
                this._buildVersion = 0;
                this._fullVersion = 0L;
            }
        }

        public ApiVersion(uint mainVersion, uint buildVersion)
        {
            this._mainVersion = mainVersion;
            this._buildVersion = buildVersion;
            this._fullVersion = (this._mainVersion << 0x20) | this._buildVersion;
            this._sections = new uint[] { this._mainVersion >> 0x17, (this._mainVersion & _mask23) >> 15, this._mainVersion & _mask15, this._buildVersion };
        }

        public int CompareTo(ApiVersion other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (this > other)
            {
                return 1;
            }
            if (this == other)
            {
                return 0;
            }
            return -1;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            ApiVersion other = obj as ApiVersion;
            if (other == null)
            {
                throw new ArgumentException("不是有效的ApiVersion对象");
            }
            return this.CompareTo(other);
        }

        public bool Equals(ApiVersion ver2)
        {
            if (object.ReferenceEquals(ver2, null))
            {
                return false;
            }
            return (this._fullVersion == ver2._fullVersion);
        }

        public override bool Equals(object obj)
        {
            ApiVersion version = obj as ApiVersion;
            return this.Equals(version);
        }

        public override int GetHashCode()
        {
            return this._fullVersion.GetHashCode();
        }

        public void IncreaseBuild()
        {
            if (2147483647 <= this._sections[3])
            {
                throw new InvalidOperationException("BuildVersion已经达到最大值。");
            }
            this._sections[3]++;
            this._fullVersion += 1L;
        }

        public void IncreaseMain()
        {
            this._fullVersion += 0x100000000L;
            this._mainVersion++;
            if (0x7fff > this._sections[2])
            {
                this._sections[2]++;
            }
            else
            {
                this._sections[0] = this._mainVersion >> 0x17;
                this._sections[1] = (this._mainVersion & _mask23) >> 15;
                this._sections[2] = this._mainVersion & _mask15;
            }
        }

        public void IncreaseMajor()
        {
            if (0xff <= this._sections[0])
            {
                throw new InvalidOperationException("MajorVersion已经达到最大值。");
            }
            this._sections[0]++;
            this._fullVersion += 0x80000000000000L;
            this._mainVersion += 0x800000;
        }

        public void IncreaseMinor()
        {
            if (0xff <= this._sections[1])
            {
                throw new InvalidOperationException("MinorVersion已经达到最大值。");
            }
            this._sections[1]++;
            this._fullVersion += 0x800000000000L;
            this._mainVersion += 0x8000;
        }

        public void IncreasePatch()
        {
            if (0x7fff <= this._sections[2])
            {
                throw new InvalidOperationException("PatchVersion已经达到最大值。");
            }
            this._sections[2]++;
            this._fullVersion += 0x100000000L;
            this._mainVersion++;
        }

        public static bool operator ==(ApiVersion ver1, ApiVersion ver2)
        {
            if (object.ReferenceEquals(ver1, null))
            {
                return object.ReferenceEquals(ver2, null);
            }
            return ver1.Equals(ver2);
        }

        public static bool operator >(ApiVersion ver1, ApiVersion ver2)
        {
            return (ver2 < ver1);
        }

        public static bool operator >=(ApiVersion ver1, ApiVersion ver2)
        {
            return (ver2 <= ver1);
        }

        public static bool operator !=(ApiVersion ver1, ApiVersion ver2)
        {
            return !(ver1 == ver2);
        }

        public static bool operator <(ApiVersion ver1, ApiVersion ver2)
        {
            if (object.ReferenceEquals(ver1, null))
            {
                throw new ArgumentNullException("ver1");
            }
            if (object.ReferenceEquals(ver2, null))
            {
                throw new ArgumentNullException("ver2");
            }
            return (ver1._fullVersion < ver2._fullVersion);
        }

        public static bool operator <=(ApiVersion ver1, ApiVersion ver2)
        {
            if (object.ReferenceEquals(ver1, null))
            {
                throw new ArgumentNullException("ver1");
            }
            if (object.ReferenceEquals(ver2, null))
            {
                throw new ArgumentNullException("ver2");
            }
            return (ver1._fullVersion <= ver2._fullVersion);
        }

        public string ToMainVersionString()
        {
            return string.Format("{0}.{1}.{2}", this._sections[0], this._sections[1], this._sections[2]);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", new object[] { this._sections[0], this._sections[1], this._sections[2], this._sections[3] });
        }

        public uint BuildVersion
        {
            get
            {
                return this._buildVersion;
            }
        }

        public long FullVersion
        {
            get
            {
                return this._fullVersion;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this._fullVersion == 0L);
            }
        }

        public uint MainVersion
        {
            get
            {
                return this._mainVersion;
            }
        }

        public uint MajorVersion
        {
            get
            {
                return this._sections[0];
            }
        }

        public uint MinorVersion
        {
            get
            {
                return this._sections[1];
            }
        }

        public uint PatchVersion
        {
            get
            {
                return this._sections[2];
            }
        }

        public uint[] Sections
        {
            get
            {
                return this._sections;
            }
        }
    }
}
