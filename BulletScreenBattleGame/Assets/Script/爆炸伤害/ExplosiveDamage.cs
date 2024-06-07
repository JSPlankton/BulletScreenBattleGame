
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    public int damage;
    private string summonerName;//��������ӵ��ĵ�λ������
    private string factions;//��λ����Ӫ
                           // Start is called before the first frame update
    public void NameUpdate(string summonerNames, string factionss)//���£������ߵ����ֺ������ߵ���Ӫ
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
