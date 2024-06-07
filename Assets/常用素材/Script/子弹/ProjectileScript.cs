using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public int damageAmount = 10;
    public GameObject hitEffect;
       private Rigidbody rb;
    private string summonerName;//生成这个子弹的单位的名字
    private string factions;//单位的阵营
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
    public void NameUpdate(string summonerNames, string factionss)//更新，生成者的名字和生成者的阵营
    {
        summonerName = summonerNames;
        factions = factionss;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 检测到碰撞，如果碰撞的是敌人，则调用敌人的伤害函数
        Unit targetUnit = other.GetComponent<Unit>();
  
        if (targetUnit != null)
        {
            print("碰撞到了"+ other.name);
            targetUnit.TakeDamage(damageAmount,summonerName, factions);

            // 生成特效并销毁物体
            if (hitEffect != null)
            {
                if (hitEffect != null)
                {
                 GameObject obj=   Instantiate(hitEffect, transform.position, Quaternion.identity);
                    obj.GetComponent<ExplosiveDamage>().NameUpdate(summonerName, factions);
                }

            }
            print("子弹销毁");
            gameObject.SetActive(false);
        }
        else
        {
            Crystal crystals = other.GetComponent<Crystal>();
            if (crystals != null)
            {
                crystals.TakeDamage(damageAmount);

                // 生成特效并销毁物体
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
