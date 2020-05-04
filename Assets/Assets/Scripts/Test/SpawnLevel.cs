using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnLevel : MonoBehaviour
{
    public GameObject Level;
    public static bool isGenerated = true;
    public static bool DoStuff = false;
    public static bool IsDone = false;

    private static SpawnLevel _instance;
    public static SpawnLevel Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public static void SpawnLevelGen()
    {
        IsDone = false;
        DoStuff = true;
    }

    private void Update()
    {
        if (DoStuff == true)
        {
            DoStuff = false;
            StartCoroutine(DelayLevelGen());
        }
    }

    IEnumerator DelayLevelGen()
    {
        yield return new WaitForSeconds(0f);
        IsDone = true;
        Instantiate(Level);
    }
}
