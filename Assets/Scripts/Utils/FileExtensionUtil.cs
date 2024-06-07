/**
 *	name: 文件扩展名帮助类 
 *	author:zhaojinlun
 *	date:2021年4月13日
 *	copyright: fungather.net
 ***/
public class FileExtensionUtil
{

    public const string meta = ".meta";
    public const string cs = ".cs";
    public const string js = ".js";
    public const string mp3 = ".mp3";
    public const string ogg = ".ogg";
    public const string wav = ".wav";
    public const string prefab = ".prefab";
    public const string anim = ".anim";
    public const string png = ".png";
    public const string tga = ".tga";
    public const string jpg = ".jpg";
    public const string tif = ".tif";
    public const string tiff = ".tiff";
    public const string renderTexture = ".renderTexture";
    public const string unity = ".unity";
    public const string lua = ".lua";
    public const string txt = ".txt";
    public const string bytes = ".bytes";
    public const string bundle = ".ab";
    public const string manifest = ".manifest";
    public const string exr = ".exr";
    public const string controller = ".controller";
    public const string overridecontroller = ".overrideController";
    public const string shader = ".shader";
    public const string shadergraph = ".shadergraph";
    public const string ShaderGraph = ".ShaderGraph";
    public const string hlsl = ".hlsl";
    public const string shadervariants = ".shadervariants";
    public const string ttf = ".ttf";
    public const string dds = ".dds";
    public const string fbx = ".fbx";
    public const string obj = ".obj";
    public const string asset = ".asset";
    public const string mat = ".mat";
    public const string psd = ".psd";
    public const string bmp = ".bmp";
    public const string unitypackage = ".unitypackage";
    public const string svn = ".svn";
    public const string zip = ".zip";
    public const string log = ".log";
    public const string json = ".json";
    public const string xml = ".xml";
    public const string mov = ".mov";
    //public const string spriteatlas = ".spriteatlas";

    public const string fnt = ".fnt";
    public const string fontsettings = ".fontsettings";
    public const string otf = ".otf";
    public const string spriteatlas = ".spriteatlasv2";

    /// <summary>
    /// 所有的资源文件扩展名
    /// </summary>
    public static string[] AssetExtensisons =
    {
        anim, mp3, ogg, wav, png, tga, jpg, tif, tiff, renderTexture, prefab, unity, lua, exr, controller,
        overridecontroller, fbx, obj, asset, mat, shader, shadergraph, ShaderGraph, hlsl, shadervariants, psd, bmp, ttf, json  , fontsettings,otf, xml, txt, spriteatlas
    };

    /// <summary>
    /// 所有文本类文件扩展名
    /// </summary>
    public static string[] Files =
    {
        lua, txt, bytes, json
    };

    /// <summary>
    /// 所有音效类文件扩展名
    /// </summary>
    public static readonly string[] Sounds =
    {
        mp3, ogg, wav
    };

    /// <summary>
    /// 所有模型类文件扩展名
    /// </summary>
    public static readonly string[] Models =
    {
        fbx, obj
    };

    /// <summary>
    /// 所有贴图文件扩展名
    /// </summary>
    public static string[] Textures =
    {
        png, tga, jpg, tif, tiff, renderTexture, exr
    };

    /// <summary>
    /// 所有不应包含在依赖资源中的文件扩展名
    /// </summary>
    public static string[] NoIncludeDependExtensisons =
    {
        cs, js, dds
    };

    /// <summary>
    /// 导出资源文件
    /// </summary>
    public static string[] ExportExtensisons =
    {
        json, lua, bundle
    };

    public static string[] Shaders =
    {
        shader, shadergraph, ShaderGraph, hlsl, shadervariants
    };

    public static string[] Scenes =
    {
        unity
    };
}
