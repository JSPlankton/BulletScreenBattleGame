using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationEnd()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
