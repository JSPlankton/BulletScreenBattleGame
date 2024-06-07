using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    public static Level instance;
    private void Awake()
    {
        instance = this;
    }
    public void Level0()
    {
        SceneManager.LoadScene("Level0");
    }
    public void Level1()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
