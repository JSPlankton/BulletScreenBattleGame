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

    // ��Χ�ڵĵ����б�
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
        // ��ʼ��ⷶΧ�ڵĵ���
        // StartCoroutine(CheckEnemiesInRange());
    }
    Transform nearestEnemy;
    // Э�̣����ڼ�ⷶΧ�ڵĵ���
     void Update()
    {
        cd -= Time.deltaTime;
        if (nearestEnemy != null && nearestEnemy.gameObject.activeSelf == true && cd <= 0f)
        {
            cd = 2f;
            StartCoroutine(AttackCooldownCoroutine(nearestEnemy));
            //continue;
        }
        // ���Ŀ�������������¼�����Χ�ڵĵ���
        if (nearestEnemy == null || nearestEnemy.gameObject.activeSelf == false)
        {
            // ��յ����б�
            enemiesInRange.Clear();

            // ��ȡ��Χ�����е���
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance);
            foreach (Collider collider in colliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    if (faction == Faction.Red && unit.factions() == "Blue")
                    {
                        // ������빥����Χ�ĵ�λ�ǵз�������й���
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
        // ���ƹ�����Χ��ͼʾ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
    IEnumerator AttackCooldownCoroutine(Transform target)
    {
        if (target != null)
        {
            //yield return new WaitForSeconds(attackCooldown); // CD

            // ���� Projectile ���岢��ȡ ProjectileScript ���
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.7f);

            if (target != null)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, target.transform.position, Quaternion.identity);
                // ���� Projectile ���˺���

                    target.GetComponent<Unit>().TakeDamage(1000,null,null);

           

                // ����������ӳ�ʱ�䣬����û�л��е���ʱ�޷���������
                Destroy(projectileObject, 5f);
            }

         
   
    
        }
        else
        {
            nearestEnemy = null; // ��������ĵ��ˣ��Ա����´μ���������ҵ�
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
           
            // ���Ѫ��С�ڵ���0����ˮ������
            Die();
        }
    }

    void Die()
    {
        victoryImage.SetActive(true);
        UIManager.instance.UpdateKillRankingDisplays(GameManagers.Instance.redPlayers, GameManagers.Instance.bluePlayers); // ���ݺ�Ӻ����ӵ�����б�
        // ˮ�������Ĵ����߼�
        Destroy(gameObject);
    }
}
