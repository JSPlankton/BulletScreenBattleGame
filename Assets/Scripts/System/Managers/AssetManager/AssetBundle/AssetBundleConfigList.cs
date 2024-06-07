using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class AssetBundleConfigList
{
    [XmlElement("ABList")]
    public List<BaseAssetBundle> ABList
    {
        get; set;
    }

}
