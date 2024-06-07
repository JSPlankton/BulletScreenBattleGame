using System.Xml.Serialization;

[System.Serializable]
public class BaseAssetBundle
{
    [XmlAttribute("AssetName")]
    public string AssetName
    {
        get; set;
    }

    [XmlAttribute("BundleName")]
    public string BundleName
    {
        get; set;
    }
}
