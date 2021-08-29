using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnZombies : MonoBehaviour
{
    public Transform[] pointSpawn;//список всех спаунов
    public ObjectWarehouse objectWarehouse;//банк объектов
    public PlayerController playerController;
    public float spawnReloading;//перерыв между появлением зомби в пределеах одной волны
    public float maxZomie;//максимальное количество зомби в волне
    public float curZombie;//текущее количество зомби на волне
    public float currentWave;//текущая волна
    public float remainingZombies;//оставшиеся в живых зомби

    public GameObject shop;//магазин 
    public GameObject helps;//подсказки между волнами
    public Text txtCurrenWave;//текст отобржающий текущую волну
    public Text txtEnemy;//показывает сколько мобов осталось на карте
    public Image progressBar;//полоса остатков зомби

    public bool shootingPermit;//разрешение на стрельбу

    // Start is called before the first frame update
    void Start()
    {
        if (helps) { helps.SetActive(false); };

        shootingPermit = true;
        StartCoroutine(Spown());
        txtCurrenWave.text = "" + currentWave;
        
    }

    private void Update()
    {
        Vector3 minOther = progressBar.GetComponent<RectTransform>().localScale;
        Vector3 maxOther = new Vector3(curZombie / maxZomie, 1, 1);

        progressBar.GetComponent<RectTransform>().localScale = Vector3.Lerp(minOther, maxOther, spawnReloading * Time.deltaTime);

        if(curZombie >= maxZomie & remainingZombies <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TransitionNewWave();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                shop.SetActive(!shop.activeSelf);
                helps.SetActive(!helps.activeSelf);
                if(playerController.enabled == true)
                {
                    playerController.enabled = false;
                    shootingPermit = false;
                }
                else
                {
                    playerController.enabled = true;
                    shootingPermit = true;
                }
            }
        }
    }

    public void PopulationRegulation()
    {
        remainingZombies -= 1;
        txtEnemy.text = "" + remainingZombies;

        if(remainingZombies <= 0)
        {
            helps.SetActive(true);
        }
    }

    private void MakingEnemy(GameObject other, int rSpawn) {
        other.SetActive(true);
        other.GetComponent<CapsuleCollider>().enabled = true;
        other.GetComponent<Animator>().enabled = true;
        other.GetComponent<WalkingControler>().enabled = true;
        other.GetComponent<RagdollControl>().DisablingColliders();
        other.GetComponent<Rigidbody>().useGravity = true;
        other.transform.position = new Vector3(pointSpawn[rSpawn].position.x, pointSpawn[rSpawn].position.y, pointSpawn[rSpawn].position.z);
    }

    //обнуляем все параметры и переходим на новую волну
    private void TransitionNewWave()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }

        currentWave += 1;
        curZombie = 0;
        txtCurrenWave.text = "" + currentWave;

        maxZomie += currentWave;

        helps.SetActive(false);

        StartCoroutine(Spown());
    }

    IEnumerator Spown() {

        float rZombie = Mathf.Round(Random.Range(0f, 1f + currentWave / 12f));
        int rSpawn = Random.Range(0, pointSpawn.Length);

        if (rZombie == 0) rZombie = 1;

        GameObject go = objectWarehouse.WeLoadZombie(rZombie);
        MakingEnemy(go, rSpawn);

        WalkingControler walkingControler = go.GetComponent<WalkingControler>();
        RagdollControl ragdollControl = go.GetComponent<RagdollControl>();

        walkingControler.damage = walkingControler.defaultDamage + walkingControler.defaultDamage * currentWave / 100;
        ragdollControl.maxHP = ragdollControl.defaultHP + ragdollControl.defaultHP * currentWave / 100;

        ragdollControl.curHP = ragdollControl.maxHP;

        curZombie += 1;
        remainingZombies += 1;
        txtEnemy.text =  "" + remainingZombies;

        yield return new WaitForSecondsRealtime(spawnReloading);

        if (curZombie < maxZomie) {
            StartCoroutine(Spown());
        }
    }
}
