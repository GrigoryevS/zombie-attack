using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInformationUI : MonoBehaviour
{
    public Color[] weaponsColor;//цвета оружия
    public Material[] weaponMaterials;//материалы оружия
    public WeaponСontroller weaponContrller;
    public WeaponScript weaponScript;//привязанный скрипт оружия

    public GameObject icone;//иконка оружия
    public GameObject buyWeapon;//кнопка стоимости покупки оружия
    public GameObject improveWeapons;//кнопка для улучшение оружия
    public GameObject buyCartridges;//кнопка стоимости покупки патронов
    public Text cartridges;//количество патронов доступное игроку

    public float flBuyWeapon;//стоимость покупки оружия
    public float flImproveWeapons;//стоимость улучшения оружия
    public float flBuyCartridges;//стоимость патронов
    public int improvementRate;//на сколько процентов будет улучшино оружие
    public int numberСartridges;//количество купленных за раз патронов

    private int weaponLevel;//уровень оружия

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

    //Купить оружия
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

    //Улучшить оружие
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

    //Купить патроны
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
        weaponScript.generalAmmunition += numberСartridges;
        UpdatingAmmoAmount();

        if (weaponContrller.curWeapon == weaponScript.gameObject)
        {
            weaponScript.txtGeneralAmmunition.text = "/ " + weaponScript.generalAmmunition;
        }
    }

    //Обновляем количество патронов в магазине
    public void UpdatingAmmoAmount()
    {
        cartridges.text = "" + weaponScript.generalAmmunition;
    }
}
