using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ����NavMeshAgent���ڵ������ռ�
using UnityEngine.UI;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshAgent))]

public class Unit : MonoBehaviour
{
    // ���嵥λ����Ӫ
    public enum Faction { Red, Blue }
    /// <summary>
    /// ��λ����
    /// </summary>
    public enum UnitType
    {
        /// <summary>
        /// ��ս
        /// </summary>
        MeleeWarfare,


        /// <summary>
        /// Զ��
        /// </summary>
        Remote,

    }

    public enum UnitStatus
    {

        /// <summary>
        /// �ƶ�
        /// </summary>
        Move,


        /// <summary>
        /// ����
        /// </summary>
        Attack,


        /// <summary>
        /// ����ˮ��
        /// </summary>
        AttackCrystal,
    }
    public Faction faction;
    public UnitType unitType;
    public UnitStatus unitStatus;

    // ��λ�����Ѫ���͵�ǰѪ��
    public int maxHealth = 100;
    private int currentHealth;

    // ��λ�Ĺ�����
    public int attackDamage = 20;

    //����CD
    public float attackCooldown = 2f;
    //public float movementSpeed = 3f;

    // ��ǰ������Ŀ����Ƿ����ڹ����ı�־
    public GameObject target;
    private bool isAttacking;

    public GameObject projectilePrefab; // ProjectileԤ����

    //�������ɵ�λ��
    public Transform attackPoint;

    private Animator anim;
    private bool isMove;

    private NavMeshAgent navAgent; // ����NavMeshAgent�������ڵ���

    public int score;

    //����ˮ��ʱ�������˼��
    public float cd = 0;
    public float cdMax = 5f;

    private float lastDistanceCheckTime;
    private float distanceCheckInterval = 3f;


    private GameObject currentProjectile; // ��ʼ���صĵ�ҩ

    public string summonerName; // ���ڴ洢�ٻ��ߵ��û���
    public float attackDistance;//��������
    private Crystal crystal;//ˮ��Ŀ�굥λ
    void Start()
    {
        anim = GetComponent<Animator>();
        // ��ʼ��Ѫ���͹�����ȴʱ��
        currentHealth = maxHealth;
        //lastAttackTime = Time.time;
        isAttacking = false;

        navAgent = GetComponent<NavMeshAgent>(); // ��ȡNavMeshAgent���
        attackDistance = navAgent.stoppingDistance;
        //if (unitType == UnitType.Remote)
        //{
        //    currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        //    currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//�������ɵ��ӵ������ֺ�
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
        //    print("ˮ��");
        //    cd -= Time.deltaTime;
        //    if (cd <= 0)
        //    {
        //        print("���²���");
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

    // �����ٻ����û����ķ���
    public void SetSummoner(string name)
    {
        summonerName = name;


    }
    //private float attackCrystalCd;
    private float attackCrystalCdMax = 3f;
    private bool isCooldownCoroutineRunning = false; // ���ڼ��Э���Ƿ���������

    // ��Start�����е��ã�����Э��

    // ����ˮ��״̬Э��
    public void AttackCrystals()
    {
        if (target != null && (target == GameManagers.Instance.crystalBlue || target == GameManagers.Instance.crystalRed))
        {
            if (!isCooldownCoroutineRunning) // �����ظ�����Э��
            {
                StartCoroutine(AttackCrystalCooldown());
                float distances = Vector3.Distance(transform.position, target.transform.position);
                if (distances <= attackDistance)
                {
                    print("����ˮ������ǰ��������" + distances + "����ˮ���ƶ�");
                    anim.SetBool("Move", false);
                    AttackCrystal();
                }
                else if (distances > attackDistance)
                {
                    print("����ˮ���ƶ�");
                    navAgent.isStopped = false;
                    anim.SetBool("Move", true);
                    navAgent.SetDestination(target.transform.position);
                    print("��ǰĿ��Ϊˮ��,�����л�Ŀ��");
                    FindNearestEnemy();
                    return;
                }
               
            }

        }
    }

    // ����ˮ����ȴЭ��
    private IEnumerator AttackCrystalCooldown()
    {
        isCooldownCoroutineRunning = true; // ���Э����������
        yield return new WaitForSeconds(attackCrystalCdMax);


        //attackCrystalCd = Random.Range(0, 0.5f);
        isCooldownCoroutineRunning = false; // Э�����н���
    }

    public void Moves()
    {
        cd += Time.deltaTime;

        //cd2 += Time.deltaTime;
        if (target != null)
        {
         
            if (cd > cdMax)
            {
 
                print("Time.time�ѵ���ִ���ƶ������ж�");

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
                    // ��Ŀ���ڹ�����Χ֮��ʱ���ƶ���Ŀ�ꡢ
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
            print("Ŀ�궪ʧ������Ѱ��");
            FindNearestEnemy();
            return;
        }
    }
    public LayerMask enemyLayerMask; // ����Ϊֻ�������˵�λ��Layer
    private Collider[] colliders = new Collider[10]; // Ԥ�ȴ�������ײ������
    //private Collider[] colliders; // Ԥ�ȴ�������ײ������
    public float addattackDistance;

    public void FindNearestEnemy()
    {
        //colliders = new Collider[20];
        float nearestDistance = attackDistance+ addattackDistance;
       // ���ڴ洢��ײ������飬������Ҫ������С
        int count = Physics.OverlapSphereNonAlloc(transform.position, nearestDistance, colliders, enemyLayerMask);

        print("��ʼ��������");
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
        // ���õ�ǰ����Ŀ��Ϊ����ĵ���
        if (nearestEnemy != null)
        {
            target = nearestEnemy;

            navAgent.SetDestination(target.transform.position);
            unitStatus = UnitStatus.Move;
            return;
        }



        // ���û���ҵ����ˣ�����ˮ��
        if (target == null && GameManagers.Instance.crystalBlue != null && GameManagers.Instance.crystalRed != null)
        {
            target = faction == Faction.Red ? GameManagers.Instance.crystalBlue : GameManagers.Instance.crystalRed;
            print("���õ���Ϊˮ��" + target.name);

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

                    // ��־Ϊ���ڹ�������ʼ������ȴ��ʱ
                    isAttacking = true;
                    StartCoroutine(AttackCooldown());

                    // ���Ź������������ɹ�����Ч
                    anim.SetTrigger("Attack");
                    StartCoroutine(SpawnProjectileWithDelay(0.5f)); // ��Ϊ�����µĺ���

            }
        }
        else
        {
            FindNearestEnemy();
            return;
        }
    }

    // ������λ�ĺ���
    private void AttackUnit()
    {
        LookAtTarget();

        if (unitType == UnitType.Remote)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y += 1.3f; // ����y����
            attackPoint.transform.LookAt(targetPosition);
            currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
            currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//�������ɵ��ӵ������ֺ�
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

    // �ӳ����ɹ�����Ч�ĺ���
    private IEnumerator SpawnProjectileWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            AttackUnit();
            FindNearestEnemy(); // �����ɹ�����Ч��Ѱ����һ��Ŀ��
        }
    }

    void AttackCrystal()
    {
        if (target != null)
        {
            if (!isAttacking)
            {
                // ��־Ϊ���ڹ�������ʼ������ȴ��ʱ
                isAttacking = true;
                StartCoroutine(AttackCooldown());

                // ���Ź������������ɹ�����Ч

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
    //��λ����ˮ��
    private IEnumerator SpawnProjectileAfterDelayCrystal(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            // ����Projectile���岢��ȡProjectileScript���
            LookAtTarget();

            if (unitType == UnitType.Remote)
            {


                //  Instantiate(projectilePrefab, attackPoint.position, attackPoint.transform.rotation);

                Vector3 targetPosition = target.transform.position;
                targetPosition.y += 1.3f; // ����y����
                attackPoint.transform.LookAt(targetPosition);
                currentProjectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
                currentProjectile.GetComponent<ProjectileScript>().NameUpdate(summonerName, factions());//�������ɵ��ӵ������ֺ�


            }
            else if (unitType == UnitType.MeleeWarfare)
            {

                print("��ˮ������˺�");
                crystal.TakeDamage(attackDamage);


            }

        }

    }

    //�����˺��ͻ�ɱ��
    public void TakeDamage(int damage, string attackerName, string factions)
    {
        if (currentHealth <= 0)
        {
            return;
        }
            currentHealth -= damage;

        // ���Ѫ��С�ڵ���0����λ���������Ҵ����ɱ��
        if (currentHealth <= 0)
        {
            Die(attackerName, factions);
        }
    }

    void Die(string attackerName, string factions)
    {
        navAgent.isStopped = true;
        // ������Ӫѡ�񲥷�������Ч�򶯻�
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
        // ���ӻ�ɱ����
        GameManagers.Instance.HandleKillEvent(attackerName, factions, score);
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 1f); // ����������Ч�򶯻�������ʹ��Particle System��Animator
    }

    IEnumerator AttackCooldown()
    {
        print("�ȴ�");
        // ������ȴ�ȴ�ʱ��
        yield return new WaitForSeconds(attackCooldown);
        // ������ȴ��������־Ϊδ����״̬
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
