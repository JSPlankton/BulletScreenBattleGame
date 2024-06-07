

using System;
using UnityEngine;


public static class ConvertUtil
{

    /// <summary>
    /// Base64 字符串转换为Texture
    /// </summary>
    /// <param name="baseString"></param>
    /// <returns></returns>
    public static Texture2D ConvertBase64StringToTexture(string baseString)
    {
        byte[] data = Convert.FromBase64String(baseString);
        Texture2D _target = ConvertByteToTexture(data);
        return _target;
    }
    /// <summary>
    /// 将byte[]转换为Texture 需修改相关-此处Texture数据已为初始数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Texture2D ConvertByteToTexture(byte[] data)
    {
        Texture2D _target = new Texture2D(2048, 2048);
        _target.LoadImage(data);
        return _target;
    }

    /// <summary>
    /// 将图片转换为Base64字符串 JPG格式
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="quality">JPG quality to encode with, 1..100 (default 75).</param>
    /// <returns></returns>
    public static string ConvertTextureToBase64String(Texture2D texture, int quality = 75)
    {
        if (texture == null)
            return string.Empty;

        byte[] bytes = texture.EncodeToJPG(quality);
        string _baseString = Convert.ToBase64String(bytes);
        return _baseString;
    }

}