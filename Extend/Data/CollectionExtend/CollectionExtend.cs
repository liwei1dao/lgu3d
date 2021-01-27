using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtend 
{
    /// <summary>
    /// 快速转换成数组
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static ArrayList ToArrayList(this ICollection keys)
    {
        return new ArrayList(keys);
    }

    /// <summary>
    /// 数组拷贝一份
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static List<T> ToListCopy<T>(this ICollection<T> keys)
    {
        return new List<T>(keys);
    }

    /// <summary>
    /// 列表模拟队列进入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RunQueue"></param>
    /// <returns></returns>
    public static void Enqueue<T>(this List<T> list, T t)
    {
        list.Insert(list.Count, t);
    }

    /// <summary>
    /// 列表模拟队列出队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Dequeue<T>(this List<T> list)
    {
        T t = list[0];
        list.RemoveAt(0);
        return t;
    }

    public static T Peek<T>(this List<T> list)
    {
        T t = list[0];
        return t;
    }

    /// <summary>
    /// 替换或者添加,或者修改一个值
    /// </summary>
    public static Dictionary<TKey, TValue> ForceAddValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
        return dict;
    }
}
