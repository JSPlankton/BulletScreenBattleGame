using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public int damageAmount = 10;
    public GameObject hitEffect;
       private Rigidbody rb;
    private string summonerName;//��������ӵ��ĵ�λ������
    private string factions;//��λ����Ӫ
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
       
    }
    private void Start()
    {
      
    }
    public void Ebd()
    {

        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        Vector3 forwardForce = transform.forward * speed;
        rb.velocity = Vector3.zero;
        rb.AddForce(forwardForce);
        Invoke("Ebd", 5f);
    }
    public void NameUpdate(string summonerNames, string factionss)//���£������ߵ����ֺ������ߵ���Ӫ
    {
        summonerName = summonerNames;
        factions = factionss;
    }
    private void OnTriggerEnter(Collider other)
    {
        // ��⵽��ײ�������ײ���ǵ��ˣ�����õ��˵��˺�����
        Unit targetUnit = other.GetComponent<Unit>();
  
        if (targetUnit != null)
        {
            print("��ײ����"+ other.name);
            targetUnit.TakeDamage(damageAmount,summonerName, factions);

            // ������Ч����������
            if (hitEffect != null)
            {
                if (hitEffect != null)
                {
                 GameObject obj=   Instantiate(hitEffect, transform.position, Quaternion.identity);
                    obj.GetComponent<ExplosiveDamage>().NameUpdate(summonerName, factions);
                }

            }
            print("�ӵ�����");
            gameObject.SetActive(false);
        }
        else
        {
            Crystal crystals = other.GetComponent<Crystal>();
            if (crystals != null)
            {
                crystals.TakeDamage(damageAmount);

                // ������Ч����������
                if (hitEffect != null)
                {
                    if (hitEffect != null)
                    {
                        Instantiate(hitEffect, transform.position, Quaternion.identity);
                    }

                }
                gameObject.SetActive(false);
            }
        }
    }
}
