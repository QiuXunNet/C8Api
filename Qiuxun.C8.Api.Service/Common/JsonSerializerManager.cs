using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Qiuxun.C8.Api.Service.Common
{
    public static class JsonSerializerManager
    {
        public static T BsonDeSerialize<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                BsonReader reader = new BsonReader(stream);
                if (typeof(ICollection).IsAssignableFrom(typeof(T)))
                {
                    reader.ReadRootValueAsArray = true;
                }
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        public static T BsonDeSerialize<T>(Stream stream)
        {
            using (BsonReader reader = new BsonReader(stream))
            {
                if (typeof(ICollection).IsAssignableFrom(typeof(T)))
                {
                    reader.ReadRootValueAsArray = true;
                }
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        public static byte[] BsonSerialize(object data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BsonWriter jsonWriter = new BsonWriter(stream);
                new Newtonsoft.Json.JsonSerializer().Serialize(jsonWriter, data);
                return stream.ToArray();
            }
        }

        public static T JsonDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string JsonFormat(string json)
        {
            try
            {
                StringReader reader = new StringReader(json);
                JsonTextReader reader2 = new JsonTextReader(reader);
                StringBuilder builder = new StringBuilder();
                bool flag = false;
                bool flag2 = false;
                while (reader2.Read())
                {
                    switch (reader2.TokenType)
                    {
                        case JsonToken.None:
                        case JsonToken.StartConstructor:
                        case JsonToken.Comment:
                        case JsonToken.Raw:
                        case JsonToken.Undefined:
                        case JsonToken.EndConstructor:
                        case JsonToken.Bytes:
                            {
                                continue;
                            }
                        case JsonToken.StartObject:
                            {
                                flag2 = true;
                                builder.Append("{");
                                continue;
                            }
                        case JsonToken.StartArray:
                            {
                                flag = true;
                                builder.Append("[");
                                continue;
                            }
                        case JsonToken.PropertyName:
                            if (!flag2)
                            {
                                goto Label_01AE;
                            }
                            builder.AppendFormat("\"{0}\":", reader2.Value);
                            goto Label_01C0;

                        case JsonToken.Integer:
                            {
                                if (!flag)
                                {
                                    goto Label_0171;
                                }
                                builder.AppendFormat("{0},", reader2.Value);
                                continue;
                            }
                        case JsonToken.Float:
                            {
                                if (!flag)
                                {
                                    goto Label_0140;
                                }
                                builder.AppendFormat("{0},", reader2.Value);
                                continue;
                            }
                        case JsonToken.String:
                            {
                                if (!flag)
                                {
                                    goto Label_01FD;
                                }
                                builder.AppendFormat("\"{0}\",", reader2.Value);
                                continue;
                            }
                        case JsonToken.Boolean:
                            {
                                if (!flag)
                                {
                                    break;
                                }
                                builder.AppendFormat("{0},", ((bool)reader2.Value) ? "true" : "false");
                                continue;
                            }
                        case JsonToken.Null:
                            {
                                builder.Append("null");
                                continue;
                            }
                        case JsonToken.EndObject:
                            {
                                builder.Append("}");
                                continue;
                            }
                        case JsonToken.EndArray:
                            {
                                flag = false;
                                builder.Remove(builder.Length - 1, 1);
                                builder.Append("]");
                                continue;
                            }
                        case JsonToken.Date:
                            {
                                builder.AppendFormat("\"{0}\"", string.Format("{0:s}.{0:fffffff}{0:zzz}", reader2.Value));
                                continue;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                    builder.AppendFormat("{0}", ((bool)reader2.Value) ? "true" : "false");
                    continue;
                    Label_0140:
                    builder.AppendFormat("{0}", reader2.Value);
                    continue;
                    Label_0171:
                    builder.AppendFormat("{0}", reader2.Value);
                    continue;
                    Label_01AE:
                    builder.AppendFormat(",\"{0}\":", reader2.Value);
                    Label_01C0:
                    flag2 = false;
                    continue;
                    Label_01FD:
                    builder.AppendFormat("\"{0}\"", reader2.Value);
                }
                return builder.ToString();
            }
            catch
            {
            }
            return json;
        }

        public static string JsonSerializer<T>(T data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(data, settings);
        }
    }
}
