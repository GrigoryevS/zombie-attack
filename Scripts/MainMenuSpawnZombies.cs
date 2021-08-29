using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSpawnZombies : MonoBehaviour
{
    public float timeSpown;
    public GameObject zombie;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spown());
    }



    IEnumerator Spown()
    {
        int r = Random.Range(0, spawnPoints.Length);

        GameObject zom = Instantiate(zombie, spawnPoints[r].position, Quaternion.identity);
        Destroy(zom, 30f);

        yield return new WaitForSecondsRealtime(timeSpown);

        StartCoroutine(Spown());
    }
}
