
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    public int damage;
    private string summonerName;//生成这个子弹的单位的名字
    private string factions;//单位的阵营
                           // Start is called before the first frame update
    public void NameUpdate(string summonerNames, string factionss)//更新，生成者的名字和生成者的阵营
    {
        summonerName = summonerNames;
        factions = factionss;
    }
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            unit.TakeDamage(damage, summonerName, factions);
        }
        Destroy(gameObject, 1f);
    }
}
