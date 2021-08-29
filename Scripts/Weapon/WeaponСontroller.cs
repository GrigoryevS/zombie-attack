using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponСontroller : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnZombies spawnZombies;

    public Transform[] handPosition;//две бустыши отвечающие за положение рук
    public Vector3[] leftHand;//позиции левой руки
    public Vector3[] rightHand;//позиции правой руки

    public GameObject radialMenu;//радиальное меню с выбором оружия

    public Text txtCurAmmunition;//текс показывающий количество патронов в обойме
    public Text txtGeneralAmmunition;//текс показывающий общее количество патронов
    public GameObject[] weapon;//список всего оружия

    public float coins;//общее количество монет у игрока
    public Text txtCoins;//текст отоброжающий количество монет

    public GameObject curWeapon;//оружие которое у игрока в руках

    // Start is called before the first frame update
    void Awake()
    {
        curWeapon = weapon[1];
        DebitMinuse(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            radialMenu.SetActive(true);
            spawnZombies.shootingPermit = false;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            radialMenu.SetActive(false);
            spawnZombies.shootingPermit = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponChange(1);
            ChangingPositionHands(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChange(2);
            ChangingPositionHands(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponChange(3);
            ChangingPositionHands(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WeaponChange(4);
            ChangingPositionHands(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            WeaponChange(5);
            ChangingPositionHands(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            WeaponChange(6);
            ChangingPositionHands(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            WeaponChange(7);
            ChangingPositionHands(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            WeaponChange(8);
            ChangingPositionHands(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            WeaponChange(9);
            ChangingPositionHands(1);
        }
    }

    //Выбор гранаты через круговое меню
    public void GrenadeSelection(int type)
    {
        playerController.typeGrenate = type;
    }

    //Выбираем оружие через круговое меню
    public void WeaponSelection(int type)
    {
        WeaponChange(type);
        if(type == 1 || type == 2 || type == 3)
        {
            ChangingPositionHands(0);
        }
        else
        {
            ChangingPositionHands(1);
        }
        
    }

    //Активируем выбранное оружие и скрываем старое
    private void WeaponChange(int other) {

        if (weapon[other].GetComponent<WeaponScript>().access == false) {
            return;
        }

        curWeapon.SetActive(false);
        curWeapon.GetComponent<WeaponScript>().StopAllCoroutines();
        curWeapon.GetComponent<WeaponScript>().recharge = false;
        curWeapon = weapon[other];
        txtCurAmmunition.text = "" + curWeapon.GetComponent<WeaponScript>().curAmmunition;
        txtGeneralAmmunition.text = "" + curWeapon.GetComponent<WeaponScript>().generalAmmunition;
        weapon[other].SetActive(true);
    }

    //Отвечает за смену позиции рук
    private void ChangingPositionHands(int other) {
        handPosition[0].localPosition = leftHand[other];
        handPosition[1].localPosition = rightHand[other];
    }

    public void DebitMinuse(float other)
    {
        coins -= other;
        txtCoins.text = "" + coins;
    }

    public void DebitPluse(float other)
    {
        coins += other;
        txtCoins.text = "" + coins;
    }
}
