using System;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;

namespace ProtoBuf
{
    public static class ProtobufHelper
    {
        #region Protobuf

        /// <summary>
        /// 序列化并返回二进制
        /// Serialize an Object and return byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ProtoSerialize<T>(T obj) where T : class
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (IOException ex)
            {
                Debug.LogError($"[StringifyHelper] 错误：{ex.Message}, {ex.Data["StackTrace"]}");
                return null;
            }
        }

        /// <summary>
        /// 通过二进制反序列化
        /// Deserialize with byte[]
        /// </summary>
        /// <param name="msg"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ProtoDeSerialize<T>(byte[] msg) where T : class
        {
            using (var ms = new MemoryStream(msg))
            {
                var data = ProtoBuf.Serializer.Deserialize<T>(ms);
                return data;
            }
        }
        public static object ProtoDeSerialize(Type type, byte[] msg)
        {
            using (var ms = new MemoryStream(msg))
            {
                var data = ProtoBuf.Serializer.Deserialize(type, ms);
                return data;
            }
        }

        /// <summary>
        /// 通过二进制反序列化
        /// Deserialize with byte[]
        /// </summary>
        /// <param name="msg"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object ProtoDeSerialize(Google.Protobuf.WellKnownTypes.Any any)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string className = any.type_url.Substring(any.type_url.IndexOf("/") + 1);
            Type type = assembly.GetType(className); //程序集名称.类名
            var ms = new MemoryStream(any.value);
            var data = ProtoBuf.Serializer.Deserialize(type, ms);
            return data;
        }



        public static T FromBytes<T>(byte[] bytes, int index, int count)
        {
            using (MemoryStream stream = new(bytes, index, count))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        #endregion Protobuf

        #region JSON

        /// <summary>
        /// 将类转换至JSON字符串
        /// Convert object to JSON string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JSONSerliaze(object value)
        {
            try
            {
                var json = JsonMapper.ToJson(value);
                return json;
            }
            catch (IOException ex)
            {
                Debug.LogError($"[StringifyHelper] 错误：{ex.Message}, {ex.Data["StackTrace"]}");
                return null;
            }
        }

        /// <summary>
        /// 将JSON字符串转类
        /// Convert JSON string to Class
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JSONDeSerliaze<T>(string value)
        {
            try
            {
                var jsonObj = JsonMapper.ToObject<T>(value);
                return jsonObj;
            }
            catch (IOException ex)
            {
                Debug.LogError($"[StringifyHelper] 错误：{ex.Message}, {ex.Data["StackTrace"]}");
                return default(T);
            }

        }
        /// <summary>
        /// 协议对象深拷贝
        /// </summary>
        public static T DeepClone<T>(T instance)
        {
            return ProtoBuf.Serializer.DeepClone(instance);
        }

        #endregion JSON
    }
}