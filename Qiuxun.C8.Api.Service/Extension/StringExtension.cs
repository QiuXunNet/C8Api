using System.Text.RegularExpressions;

namespace System
{

    public static class StringExtension
    {
        public static T ParseTo<T>(this string str) where T : struct
        {
            return str.ParseTo<T>(default(T));
        }

        private static object ParseTo(string str, string type)
        {
            switch (type)
            {
                case "System.Boolean":
                    return ToBoolean(str);

                case "System.SByte":
                    return ToSByte(str);

                case "System.Byte":
                    return ToByte(str);

                case "System.UInt16":
                    return ToUInt16(str);

                case "System.Int16":
                    return ToInt16(str);

                case "System.uInt32":
                    return ToUInt32(str);

                case "System.Int32":
                    return str.ToInt32();

                case "System.UInt64":
                    return ToUInt64(str);

                case "System.Int64":
                    return ToInt64(str);

                case "System.Single":
                    return ToSingle(str);

                case "System.Double":
                    return ToDouble(str);

                case "System.Decimal":
                    return ToDecimal(str);

                case "System.DateTime":
                    return ToDateTime(str);

                case "System.Guid":
                    return ToGuid(str);
            }
            throw new NotSupportedException(string.Format("The string of \"{0}\" can not be parsed to {1}", str, type));
        }

        public static T ParseTo<T>(this string str, T defaultValue) where T : struct
        {
            T? nullable = str.ParseToNullable<T>();
            if (nullable.HasValue)
            {
                return nullable.Value;
            }
            return defaultValue;
        }

        public static T? ParseToNullable<T>(this string str) where T : struct
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return ToEnum<T>(str);
            }
            return (T?)ParseTo(str, type.FullName);
        }

        public static string ReplaceWhitespace(this string input, string replacement = "")
        {
            if (input == null)
            {
                return null;
            }
            return Regex.Replace(input, @"\s", replacement, RegexOptions.Compiled);
        }

        public static bool? ToBoolean(string value)
        {
            bool flag;
            if (bool.TryParse(value, out flag))
            {
                return new bool?(flag);
            }
            return null;
        }

        public static byte? ToByte(string value)
        {
            byte num;
            if (byte.TryParse(value, out num))
            {
                return new byte?(num);
            }
            return null;
        }

        public static DateTime? ToDateTime(string value)
        {
            DateTime time;
            if (DateTime.TryParse(value, out time))
            {
                return new DateTime?(time);
            }
            return null;
        }

        public static decimal? ToDecimal(string value)
        {
            decimal num;
            if (decimal.TryParse(value, out num))
            {
                return new decimal?(num);
            }
            return null;
        }

        public static double? ToDouble(string value)
        {
            double num;
            if (double.TryParse(value, out num))
            {
                return new double?(num);
            }
            return null;
        }

        public static T? ToEnum<T>(string str) where T : struct
        {
            T local;
            if (Enum.TryParse<T>(str, true, out local) && Enum.IsDefined(typeof(T), local))
            {
                return new T?(local);
            }
            return null;
        }

        public static Guid? ToGuid(string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid))
            {
                return new Guid?(guid);
            }
            return null;
        }

        public static short? ToInt16(string value)
        {
            short num;
            if (short.TryParse(value, out num))
            {
                return new short?(num);
            }
            return null;
        }

        public static int? ToInt32(this string input)
        {
            int num;
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out num))
            {
                return num;
            }
            return null;
        }

        public static int? ToInt32(this string input, int defaultValue)
        {
            int num;
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static long? ToInt64(string value)
        {
            long num;
            if (long.TryParse(value, out num))
            {
                return new long?(num);
            }
            return null;
        }

        public static sbyte? ToSByte(string value)
        {
            sbyte num;
            if (sbyte.TryParse(value, out num))
            {
                return new sbyte?(num);
            }
            return null;
        }

        public static float? ToSingle(string value)
        {
            float num;
            if (float.TryParse(value, out num))
            {
                return new float?(num);
            }
            return null;
        }

        public static ushort? ToUInt16(string value)
        {
            ushort num;
            if (ushort.TryParse(value, out num))
            {
                return new ushort?(num);
            }
            return null;
        }

        public static uint? ToUInt32(string value)
        {
            uint num;
            if (uint.TryParse(value, out num))
            {
                return new uint?(num);
            }
            return null;
        }

        public static ulong? ToUInt64(string value)
        {
            ulong num;
            if (ulong.TryParse(value, out num))
            {
                return new ulong?(num);
            }
            return null;
        }
    }
}
