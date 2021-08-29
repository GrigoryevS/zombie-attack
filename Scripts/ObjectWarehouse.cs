using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWarehouse : MonoBehaviour
{
    public GameObject[] zombieDrinker;
    public int currenZombieDrinker;

    public GameObject[] zombieMedic;
    public int currenZombieMedic;

    public GameObject[] zombieSoldier;
    public int currenZombieSoldier;

    public GameObject[] zombieMonster;
    public int currenMonster;

    public GameObject[] zombieMask;
    public int currenMask;

    public GameObject[] zombieFat;
    public int currenFat;

    public GameObject[] zombieGunner;
    public int currenGunner;

    public GameObject[] gunBullets;
    public int currenGunBullets;

    public GameObject[] grenade;
    public int currenGrenade;
    public int quantityGrenade;
    public int maxGrenade;

    public GameObject[] mine;
    public int currenMine;
    public int quantityMine;
    public int maxMine;

    public GameObject[] bomb;
    public int currenBomb;
    public int quantityBomb;
    public int maxBomb;
    public int blownUpBomb;

    public GameObject[] vog;
    public int currenVog;

    public GameObject bloodEffect;

    private GameObject curZombie;

    public GameObject WeLoadZombie(float other)
    {
        switch (other)
        {

            case 1:
                if (currenZombieDrinker < zombieDrinker.Length - 1)
                {
                    currenZombieDrinker += 1;
                }
                else
                {
                    currenZombieDrinker = 0;
                }
                curZombie = zombieDrinker[currenZombieDrinker];
                break;

            case 2:
                if (currenZombieMedic < zombieMedic.Length - 1)
                {
                    currenZombieMedic += 1;
                }
                else
                {
                    currenZombieMedic = 0;
                }
                curZombie = zombieMedic[currenZombieMedic];
                break;

            case 3:
                if (currenZombieSoldier < zombieSoldier.Length - 1)
                {
                    currenZombieSoldier += 1;
                }
                else
                {
                    currenZombieSoldier = 0;
                }
                curZombie = zombieSoldier[currenZombieSoldier];
                break;

            case 4:
                if (currenMonster < zombieMonster.Length - 1)
                {
                    currenMonster += 1;
                }
                else
                {
                    currenMonster = 0;
                }
                curZombie = zombieMonster[currenMonster];
                break;

            case 5:
                if (currenMask < zombieMask.Length - 1)
                {
                    currenMask += 1;
                }
                else
                {
                    currenMask = 0;
                }
                curZombie = zombieMask[currenMask];
                break;

            case 6:
                if (currenFat < zombieFat.Length - 1)
                {
                    currenFat += 1;
                }
                else
                {
                    currenFat = 0;
                }
                curZombie = zombieFat[currenFat];
                break;

            case 7:
                if (currenGunner < zombieGunner.Length - 1)
                {
                    currenGunner += 1;
                }
                else
                {
                    currenGunner = 0;
                }
                curZombie = zombieGunner[currenGunner];
                break;
        }

        return curZombie;
    }

    //Заряжаем обычную пулю
    public GameObject TakeTheGunBullets() {
        GameObject bul = gunBullets[currenGunBullets];
        bul.SetActive(true);
        bul.GetComponent<BulletScript>().lifetime = 3;

        if (currenGunBullets < gunBullets.Length) currenGunBullets += 1;
        if (currenGunBullets >= gunBullets.Length) currenGunBullets = 0;
        return bul;
    }

    //Загружаем гранату
    private GameObject gren;

    public GameObject WeloadGrenade(int type) {

        switch (type)
        {
            case 0:
                gren = grenade[currenGrenade];

                if (quantityGrenade > 0)
                {
                    gren.SetActive(true);
                    quantityGrenade -= 1;

                    if (currenGrenade < grenade.Length) currenGrenade += 1;
                    if (currenGrenade >= grenade.Length) currenGrenade = 0;
                }

                break;

            case 1:
                gren = mine[currenMine];

                if (quantityMine > 0)
                {
                    gren.SetActive(true);
                    quantityMine -= 1;

                    if (currenMine < mine.Length) currenMine += 1;
                    if (currenMine >= mine.Length) currenMine = 0;
                } 

                break;

            case 2:
                gren = bomb[currenBomb];

                if (quantityBomb > 0)
                {
                    gren.SetActive(true);
                    quantityBomb -= 1;

                    if (currenBomb < bomb.Length) currenBomb += 1;
                    if (currenBomb >= bomb.Length) currenBomb = 0;
                } 

                break;
        }

        return gren;
    }

    public GameObject WeLoadVog()
    {
        GameObject v = vog[currenVog];
        v.SetActive(true);

        if (currenVog < vog.Length) currenVog += 1;
        if (currenVog >= vog.Length) currenVog = 0;
        return v;
    }

    public GameObject WeLoadBloodEffect()
    {
        GameObject effect = Instantiate(bloodEffect);
        Destroy(effect, 1f);

        return effect;
    }

    //Подрыв бомбы
    public void DetonatingBomb()
    {
        if (bomb[blownUpBomb].activeSelf == true){
            bomb[blownUpBomb].GetComponent<Grenade>().Expplode();

            if (blownUpBomb < bomb.Length) blownUpBomb += 1;
            if (blownUpBomb >= bomb.Length) blownUpBomb = 1;
        }
    }
}
