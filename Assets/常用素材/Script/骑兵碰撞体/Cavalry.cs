using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Cavalry : MonoBehaviour
{
    public float attackDistance=5;
    // Start is called before the first frame update
    private string camp;
    private Unit units;

    void Start()
    {
        units = GetComponent<Unit>();
        camp = units.factions();
        //Invoke("End", 15f);
    }
    private void Update()
    {
        
    }
    private IEnumerator Cd()
    {
        yield return new WaitForSeconds(15f);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = 6;
        agent.stoppingDistance = attackDistance;
        //units.FindNearestEnemy();
        units.attackDistance = attackDistance;
        units.addattackDistance = 0;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Cavalry>());
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null)
        {
            if (camp== "Blue"&& unit.factions() == "Red")
            {
                StartCoroutine(Cd());
                unit.TakeDamage(100, units.summonerName, camp);
            }
            else if (camp == "Red" && unit.factions() == "Blue")
            {
                StartCoroutine(Cd());
                unit.TakeDamage(100, units.summonerName, camp);
            }
        }
        else if (unit == null)
        {
            Crystal crystal = collision.gameObject.GetComponent<Crystal>();
            if (crystal != null)
            {
                StartCoroutine(Cd());
                crystal.TakeDamage(100);
            }

        }
    }

}
