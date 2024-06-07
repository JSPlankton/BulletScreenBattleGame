using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 导入NavMeshAgent所在的命名空间
using UnityEngine.UI;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshAgent))]

public class Unit : MonoBehaviour
{
    // 定义单位的阵营
    public enum Faction { Red, Blue }
    /// <summary>
    /// 单位类型
    /// </summary>
    public enum UnitType
    {
        /// <summary>
        /// 近战
        /// </summary>
        MeleeWarfare,


        /// <summary>
        /// 远程
        /// </summary>
        Remote,

    }

    public enum UnitStatus
    {

        /// <summary>
        /// 移动
        /// </summary>
        Move,


        /// <summary>
        /// 攻击
        /// </summary>
        Attack,


        /// <summary>
        /// 攻击水晶
        /// </summary>
        AttackCrystal,
    }
    public Faction faction;
    public UnitType unitType;
    public UnitStatus unitStatus;

    // 单位的最大血量和当前血量
    public int maxHealth = 100;
    private int currentHealth;

    // 单位的攻击力
    public int attackDamage = 20;

    //攻击CD
    public float attackCooldown = 2f;
    //public float movementSpeed = 3f;

    // 当前攻击的目标和是否正在攻击的标志
    public GameObject target;
    private bool isAttacking;

    public GameObject projectilePrefab; // Projectile预制体

    //物体生成的位置
    public Transform attackPoint;

    private Animator anim;
    private bool isMove;

    private NavMeshAgent navAgent; // 声明NavMeshAgent变量用于导航

    public int score;

    //攻击水晶时检索敌人间隔
    public float cd = 0;
    public float cdMax = 5f;

    private float lastDistanceCheckTime;
    private float distanceCheckInterval = 3f;


    private GameObject currentProjectile; // 初始加载的弹药

    public string summonerName; // 用于存储召唤者的用户名
    public float attackDistance;//攻击距离
    private Crystal crystal;//水晶目标单位
    void Start()
    {
        anim = GetComponent<Animator>();
        // 初始化血量和攻击冷却时间
        currentHealth = maxHealth;
        //lastAttackTime = Time.time;
        isAttacking = false;

        navAgent = GetComponent<NavMeshAgent>(); // 获取NavMeshAgent组件
        attackDistance = navAgent.stoppingDistance;
        //if (unitType == UnitType.Remote)
        //{
        //    currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        //    currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//设置生成的子弹的名字和
        //    currentProjectile.SetActive(false);
        //}

        //FindNearestEnemy();
        if (faction == Faction.Red)
        {
            if (GameManagers.Instance.crystalBlue != null)
            {
                navAgent.SetDestination(GameManagers.Instance.crystalBlue.transform.position);
                target = GameManagers.Instance.crystalBlue;
                crystal = target.GetComponent<Crystal>();
            }

        }
        else
        {
            if (GameManagers.Instance.crystalRed != null)
            {
                navAgent.SetDestination(GameManagers.Instance.crystalRed.transform.position);
                target = GameManagers.Instance.crystalRed;
                crystal = target.GetComponent<Crystal>();
            }
    
        }
  


    }

    private void Update()
    {
        return;
        //if (attackCrystal == true)
        //{
        //    print("水晶");
        //    cd -= Time.deltaTime;
        //    if (cd <= 0)
        //    {
        //        print("从新查找");
        //        FindNearestEnemy();

        //        cd = 5f;
        //    }
        //}

        switch (unitStatus)
        {
            case UnitStatus.Move:
                Moves();
                break;
            case UnitStatus.Attack:
                Attack();
                break;
            case UnitStatus.AttackCrystal:
                AttackCrystals();
                break;
        }
    }

    // 设置召唤者用户名的方法
    public void SetSummoner(string name)
    {
        summonerName = name;


    }
    //private float attackCrystalCd;
    private float attackCrystalCdMax = 3f;
    private bool isCooldownCoroutineRunning = false; // 用于检查协程是否在运行中

    // 在Start方法中调用，启动协程

    // 攻击水晶状态协程
    public void AttackCrystals()
    {
        if (target != null && (target == GameManagers.Instance.crystalBlue || target == GameManagers.Instance.crystalRed))
        {
            if (!isCooldownCoroutineRunning) // 避免重复启动协程
            {
                StartCoroutine(AttackCrystalCooldown());
                float distances = Vector3.Distance(transform.position, target.transform.position);
                if (distances <= attackDistance)
                {
                    print("攻击水晶，当前攻击距离" + distances + "朝向水晶移动");
                    anim.SetBool("Move", false);
                    AttackCrystal();
                }
                else if (distances > attackDistance)
                {
                    print("朝向水晶移动");
                    navAgent.isStopped = false;
                    anim.SetBool("Move", true);
                    navAgent.SetDestination(target.transform.position);
                    print("当前目标为水晶,尝试切换目标");
                    FindNearestEnemy();
                    return;
                }
               
            }

        }
    }

    // 攻击水晶冷却协程
    private IEnumerator AttackCrystalCooldown()
    {
        isCooldownCoroutineRunning = true; // 标记协程正在运行
        yield return new WaitForSeconds(attackCrystalCdMax);


        //attackCrystalCd = Random.Range(0, 0.5f);
        isCooldownCoroutineRunning = false; // 协程运行结束
    }

    public void Moves()
    {
        cd += Time.deltaTime;

        //cd2 += Time.deltaTime;
        if (target != null)
        {
         
            if (cd > cdMax)
            {
 
                print("Time.time已到，执行移动距离判断");

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > attackDistance * 2)
                {
                    anim.SetBool("Move", true);
                    cd = Random.Range(0, cdMax);
                    FindNearestEnemy();
                    return;
                }
                if (distance > attackDistance)
                {
                    // 当目标在攻击范围之外时，移动向目标、
                    navAgent.isStopped = false;
                    anim.SetBool("Move", true);
                    navAgent.SetDestination(target.transform.position);
                    cd = Random.Range(0, cdMax);
                    return;
                }
                else if (distance <= attackDistance)
                {
                    navAgent.isStopped = true;
                    anim.SetBool("Move", false);
                    navAgent.SetDestination(target.transform.position);
                    if (target == GameManagers.Instance.crystalBlue || target == GameManagers.Instance.crystalRed)
                    {
                        unitStatus = UnitStatus.AttackCrystal;
                        isAttacking = false;
                    }
                    else
                    {
                        unitStatus = UnitStatus.Attack;
                        isAttacking = false;
                    }
   
                    return;
                }
       

            }

        }
        else
        {
            print("目标丢失，从新寻找");
            FindNearestEnemy();
            return;
        }
    }
    public LayerMask enemyLayerMask; // 设置为只包含敌人单位的Layer
    private Collider[] colliders = new Collider[10]; // 预先创建的碰撞体数组
    //private Collider[] colliders; // 预先创建的碰撞体数组
    public float addattackDistance;

    public void FindNearestEnemy()
    {
        //colliders = new Collider[20];
        float nearestDistance = attackDistance+ addattackDistance;
       // 用于存储碰撞体的数组，根据需要调整大小
        int count = Physics.OverlapSphereNonAlloc(transform.position, nearestDistance, colliders, enemyLayerMask);

        print("开始检索敌人");
        //float nearestDistance = attackDistance;
        GameObject nearestEnemy = null;

        for (int i = 0; i < count; i++)
        {
            Unit enemy = colliders[i].GetComponent<Unit>();

            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy.gameObject;
                }
            }
        }
        // 设置当前攻击目标为最近的敌人
        if (nearestEnemy != null)
        {
            target = nearestEnemy;

            navAgent.SetDestination(target.transform.position);
            unitStatus = UnitStatus.Move;
            return;
        }



        // 如果没有找到敌人，攻击水晶
        if (target == null && GameManagers.Instance.crystalBlue != null && GameManagers.Instance.crystalRed != null)
        {
            target = faction == Faction.Red ? GameManagers.Instance.crystalBlue : GameManagers.Instance.crystalRed;
            print("设置敌人为水晶" + target.name);

            navAgent.SetDestination(target.transform.position);
            anim.SetBool("Move", true);
            unitStatus = UnitStatus.AttackCrystal;
        }


    }


    private void LookAtTarget()
    {

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);


    }

    void Attack()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > attackDistance * 2)
            {
                FindNearestEnemy();
                return;
            }
            else if (distance > attackDistance)
            {
                unitStatus = UnitStatus.Move;
                navAgent.SetDestination(target.transform.position);
                return;
            }
            else if (distance <= attackDistance&& !isAttacking)
            {

                    // 标志为正在攻击，开始攻击冷却计时
                    isAttacking = true;
                    StartCoroutine(AttackCooldown());

                    // 播放攻击动画或生成攻击特效
                    anim.SetTrigger("Attack");
                    StartCoroutine(SpawnProjectileWithDelay(0.5f)); // 改为调用新的函数

            }
        }
        else
        {
            FindNearestEnemy();
            return;
        }
    }

    // 攻击单位的函数
    private void AttackUnit()
    {
        LookAtTarget();

        if (unitType == UnitType.Remote)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y += 1.3f; // 增加y坐标
            attackPoint.transform.LookAt(targetPosition);
            currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
            currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//设置生成的子弹的名字和
        }
        else if (unitType == UnitType.MeleeWarfare)
        {
            Unit unit = target.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeDamage(attackDamage, summonerName, factions());
            }
        }
    }

    // 延迟生成攻击特效的函数
    private IEnumerator SpawnProjectileWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            AttackUnit();
            FindNearestEnemy(); // 在生成攻击特效后寻找下一个目标
        }
    }

    void AttackCrystal()
    {
        if (target != null)
        {
            if (!isAttacking)
            {
                // 标志为正在攻击，开始攻击冷却计时
                isAttacking = true;
                StartCoroutine(AttackCooldown());

                // 播放攻击动画或生成攻击特效

                anim.SetTrigger("Attack");
                StartCoroutine(SpawnProjectileAfterDelayCrystal(0.5f));
            }
        }

        else
        {
            FindNearestEnemy();
            return;
        }

    }
    //单位攻击水晶
    private IEnumerator SpawnProjectileAfterDelayCrystal(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            // 生成Projectile物体并获取ProjectileScript组件
            LookAtTarget();

            if (unitType == UnitType.Remote)
            {


                //  Instantiate(projectilePrefab, attackPoint.position, attackPoint.transform.rotation);

                Vector3 targetPosition = target.transform.position;
                targetPosition.y += 1.3f; // 增加y坐标
                attackPoint.transform.LookAt(targetPosition);
                currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
                currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//设置生成的子弹的名字和


            }
            else if (unitType == UnitType.MeleeWarfare)
            {

                print("对水晶造成伤害");
                crystal.TakeDamage(attackDamage);


            }

        }

    }

    //传入伤害和击杀者
    public void TakeDamage(int damage, string attackerName, string factions)
    {
        if (currentHealth <= 0)
        {
            return;
        }
            currentHealth -= damage;

        // 如果血量小于等于0，则单位死亡，并且传入击杀者
        if (currentHealth <= 0)
        {
            Die(attackerName, factions);
        }
    }

    void Die(string attackerName, string factions)
    {
        navAgent.isStopped = true;
        // 根据阵营选择播放死亡特效或动画
        //if (faction == Faction.Red)
        //{
        //    GameManagers.Instance.redUnits.Remove(gameObject);
        //    GameManagers.Instance.redUnits.RemoveAll(item => item == null);
        //}
        //else
        //{
        //    GameManagers.Instance.blueUnits.Remove(gameObject);
        //    GameManagers.Instance.blueUnits.RemoveAll(item => item == null);
        //}

        anim.SetTrigger("Die");
        //if (currentProjectile != null)
        //{
        //    Destroy(currentProjectile, 1f);

        //}
        // 增加击杀计数
        GameManagers.Instance.HandleKillEvent(attackerName, factions, score);
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 1f); // 触发死亡特效或动画，可以使用Particle System或Animator
    }

    IEnumerator AttackCooldown()
    {
        print("等待");
        // 攻击冷却等待时间
        yield return new WaitForSeconds(attackCooldown);
        // 攻击冷却结束，标志为未攻击状态
        isAttacking = false;
    }

    public string factions()
    {
        return faction == Faction.Red ? "Red" : "Blue";
    }

    public void SetUnitName(string name)
    {
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        lastChild.transform.GetChild(1).GetComponent<Text>().text = name;
        lastChild.GetComponent<LookAts>().enabled = true;
    }

}
