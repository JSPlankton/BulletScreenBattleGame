using UnityEngine;

// 定义用于表示礼物消息的类
[System.Serializable]
public class GiftMessage
{
    public string type;        // 消息类型
    public string count;       //单次点赞数
    public string total;       //总点赞数
    public string name;        // 发送者名称
    public string uid;         // 发送者唯一标识
    public string head_img;    // 发送者头像链接
    public string giftId;      // 礼物ID
    public string giftCount;   // 礼物数量
    public string giftName;    // 礼物名称
    public string content;     // 消息内容
}


