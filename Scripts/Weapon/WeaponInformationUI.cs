using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInformationUI : MonoBehaviour
{
    public Color[] weaponsColor;//����� ������
    public Material[] weaponMaterials;//��������� ������
    public Weapon�ontroller weaponContrller;
    public WeaponScript weaponScript;//����������� ������ ������

    public GameObject icone;//������ ������
    public GameObject buyWeapon;//������ ��������� ������� ������
    public GameObject improveWeapons;//������ ��� ��������� ������
    public GameObject buyCartridges;//������ ��������� ������� ��������
    public Text cartridges;//���������� �������� ��������� ������

    public float flBuyWeapon;//��������� ������� ������
    public float flImproveWeapons;//��������� ��������� ������
    public float flBuyCartridges;//��������� ��������
    public int improvementRate;//�� ������� ��������� ����� �������� ������
    public int number�artridges;//���������� ��������� �� ��� ��������

    private int weaponLevel;//������� ������

    void Awake()
    {
        UpdatingAmmoAmount();
        weaponMaterials[0].color = weaponsColor[0];
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UpdatingAmmoAmount();
        }
    }

    //������ ������
    public void BuyWeapon() {
        if (weaponScript.access == true)
        {
            return;
        }
        if (flBuyWeapon > weaponContrller.coins)
        {
            return;
        }
        weaponContrller.DebitMinuse(flBuyWeapon);
        weaponScript.access = true;
    }

    //�������� ������
    public void UpgradeWeapons() {
        if (flImproveWeapons > weaponContrller.coins)
        {
            return;
        }
        if(weaponLevel >= weaponsColor.Length - 1)
        {
            return;
        }
        if (weaponScript.access == false)
        {
            return;
        }
        weaponContrller.DebitMinuse(flImproveWeapons);
        weaponLevel += 1;
        weaponScript.damage += weaponScript.damage * improvementRate / 100;
        weaponMaterials[0].color = weaponsColor[weaponLevel];
    }

    //������ �������
    public void BuyCartridges() { 
        if(flBuyCartridges > weaponContrller.coins)
        {
            return;
        }
        if (weaponScript.access == false)
        {
            return;
        }
        weaponContrller.DebitMinuse(flBuyCartridges);
        weaponScript.generalAmmunition += number�artridges;
        UpdatingAmmoAmount();

        if (weaponContrller.curWeapon == weaponScript.gameObject)
        {
            weaponScript.txtGeneralAmmunition.text = "/ " + weaponScript.generalAmmunition;
        }
    }

    //��������� ���������� �������� � ��������
    public void UpdatingAmmoAmount()
    {
        cartridges.text = "" + weaponScript.generalAmmunition;
    }
}
