using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    public enum Faction { Red, Blue }
    public Faction faction;

    public int maxHealth = 100;
    private int currentHealth;

    public GameObject projectilePrefab;
    public float attackDistance = 10f;
    public float attackCooldown = 0.7f;

    public Transform launch;
    public float cd=2f;

    public Slider crystalSlider;

    public GameObject victoryImage;
    public TextMeshProUGUI hpText;

    // 范围内的敌人列表
    private List<Transform> enemiesInRange = new List<Transform>();
    public Animator anim;


    private void Awake()
    {
        // maxHealth = DataConfiguration.instance.hpMax;
        maxHealth = Int32.MaxValue;
    }
    void Start()
    {
        currentHealth = maxHealth;
        crystalSlider.maxValue = maxHealth;
        crystalSlider.value = maxHealth;
        hpText.text = currentHealth+"";
        // 开始检测范围内的敌人
        // StartCoroutine(CheckEnemiesInRange());
    }
    Transform nearestEnemy;
    // 协程，定期检测范围内的敌人
     void Update()
    {
        cd -= Time.deltaTime;
        if (nearestEnemy != null && nearestEnemy.gameObject.activeSelf == true && cd <= 0f)
        {
            cd = 2f;
            StartCoroutine(AttackCooldownCoroutine(nearestEnemy));
            //continue;
        }
        // 如果目标死亡，则重新检索范围内的敌人
        if (nearestEnemy == null || nearestEnemy.gameObject.activeSelf == false)
        {
            // 清空敌人列表
            enemiesInRange.Clear();

            // 获取范围内所有敌人
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance);
            foreach (Collider collider in colliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    if (faction == Faction.Red && unit.factions() == "Blue")
                    {
                        // 如果进入攻击范围的单位是敌方，则进行攻击
                        enemiesInRange.Add(collider.transform);
                    }
                    else if (faction == Faction.Blue && unit.factions() == "Red")
                    {
                        enemiesInRange.Add(collider.transform);
                    }
                }
            
            }
            nearestEnemy = FindNearestEnemy();
        }
    }


    Transform FindNearestEnemy()
    {

        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }



    void OnDrawGizmosSelected()
    {
        // 绘制攻击范围的图示
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
    IEnumerator AttackCooldownCoroutine(Transform target)
    {
        if (target != null)
        {
            //yield return new WaitForSeconds(attackCooldown); // CD

            // 生成 Projectile 物体并获取 ProjectileScript 组件
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.7f);

            if (target != null)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, target.transform.position, Quaternion.identity);
                // 设置 Projectile 的伤害量

                    target.GetComponent<Unit>().TakeDamage(1000,null,null);

           

                // 销毁物体的延迟时间，避免没有击中敌人时无法销毁物体
                Destroy(projectileObject, 5f);
            }

         
   
    
        }
        else
        {
            nearestEnemy = null; // 重置最近的敌人，以便在下次检测中重新找到
        }
    }



    public void TakeDamage(int damage)
    {
        if (currentHealth < 0)
        {
            return;
        }
        currentHealth -= damage;
        //print(currentHealth);
        crystalSlider.value = currentHealth;
        hpText.text =  currentHealth+"";
        if (currentHealth <= 0)
        {
           
            // 如果血量小于等于0，则水晶死亡
            Die();
        }
    }

    void Die()
    {
        victoryImage.SetActive(true);
        UIManager.instance.UpdateKillRankingDisplays(GameManagers.Instance.redPlayers, GameManagers.Instance.bluePlayers); // 传递红队和蓝队的玩家列表
        // 水晶死亡的处理逻辑
        Destroy(gameObject);
    }
}
