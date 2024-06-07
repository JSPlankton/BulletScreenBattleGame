/**
 *	name: 类 
 *	author:zhaojinlun
 *	date:2021年4月13日
 *	copyright: fungather.net
 ***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryEx
{
    public static TValue[] ValueToArray<TKey, TValue>(this Dictionary<TKey, TValue> dic)
    {
        TValue[] arrays = new TValue[dic.Values.Count];
        dic.Values.CopyTo(arrays, 0);
        return arrays;
    }
}
