
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && unit.gameObject.activeSelf == true)
        {
            unit.TakeDamage(damage,null,null);
        }
        Destroy(gameObject, 1f);
    }

}
