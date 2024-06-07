
public static class TypeConversion
{
    public static int ToInt(this string str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }
    public static int ToInt(this object str)
    {
        int i = 0;
        int.TryParse(str.ToString(), out i);
        return i;
    }
    public static double ToDouble(this string str)
    {
        //兼容异常
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        
        //转换字符串只允许数字和小数点
        //double.TryParse(str, out i);
        double.TryParse(str,
            System.Globalization.NumberStyles.AllowDecimalPoint
            | System.Globalization.NumberStyles.AllowLeadingSign

            , System.Globalization.CultureInfo.InvariantCulture, out double i);

        return i;
    }
    public static float ToFloat(this string str)
    {
        //兼容异常
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        //转换字符串只允许数字和小数点
        //float.TryParse(str, out i);
        float.TryParse(str,
            System.Globalization.NumberStyles.AllowDecimalPoint
            | System.Globalization.NumberStyles.AllowLeadingSign

            , System.Globalization.CultureInfo.InvariantCulture, out float i);

        return i;
    }
    public static long ToLong(this string str)
    {
        long i = 0;
        if (str.Contains("."))
        {
            string[] array = str.Split('.');
            long.TryParse(array[0], out i);
        }
        else
        {
            long.TryParse(str, out i);
        }

        return i;
    }
}
