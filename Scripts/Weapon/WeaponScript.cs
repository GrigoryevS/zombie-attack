using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
    public Text txtCurAmmunition;//текс показывающий количество патронов в обойме
    public Text txtGeneralAmmunition;//текс показывающий общее количество патронов
    public Image rechargeBar;//полоса прогресса перезарядки

    public float rechargeTime;//время на перезарядку обоймы
    public float shotTime;//задержа между выстрелами

    public int maxAmmunition;//максимальное количество патронов в обойме
    public int curAmmunition;//текщее количество патронов в обойме
    public int generalAmmunition;//общее количество боеприпасов
    public int damage;//урок оружия
    public int numberBullets;//количество выпускаемых пуль
    public int typeBullet;//тип которым стреляет оружие

    public GameObject shotFlash;//эффект выстрела
    public Transform bulletPoint;//точка создания патрона

    public bool recharge;//перезаряжается ли оружие в даннай момент

    private SpawnZombies spawnZombies;
    private ObjectWarehouse objWarehouse;//банк объектов
    private Camera cam;//камера игрока
    private Vector3 targetPoint;//место куда направлен курсор
    private float disTarget;//дистанция до курсора

    private float scaleBar;//переменная отвечающая за длинну полосы перезарядки

    private bool triggerPulled;//зажат ли курок в данный момент
    public bool access;//куплено ли оружие или нет

    // Start is called before the first frame update
    void Awake()
    {
        spawnZombies = GameObject.FindWithTag("Spownes Zombi").GetComponent<SpawnZombies>();
        objWarehouse = GameObject.FindWithTag("Object Warehouse").GetComponent<ObjectWarehouse>();
        cam = Camera.main;
        curAmmunition = maxAmmunition;
        txtCurAmmunition.text = "" + curAmmunition;
        txtGeneralAmmunition.text = "/ " + generalAmmunition; 
    }

    private void FixedUpdate()
    {
        if (recharge){
            scaleBar += 1f / rechargeTime * Time.deltaTime;
            rechargeBar.GetComponent<RectTransform>().localScale = new Vector3(scaleBar, 1, 1);
        }

        if (!recharge) {
            rechargeBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }


    }

    // Update is called once per frame
    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        Ray ray = cam.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                targetPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                bulletPoint.LookAt(targetPoint);
            }
            else
            {
                targetPoint = new Vector3(hit.point.x, 1f, hit.point.z);
                bulletPoint.LookAt(targetPoint);
            }
            if (Input.GetMouseButtonDown(0))
            {
                disTarget = Vector3.Distance(bulletPoint.transform.position, hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0) & !recharge)
        {
            StopAllCoroutines();
            triggerPulled = true;
            WeaponController();
        }

        if (Input.GetMouseButtonUp(0))
        {
            triggerPulled = false;
        }

        if (Input.GetKeyDown(KeyCode.R) & !recharge)
        {
            StartCoroutine(Recharge());
        }
    }

    //Перезарядка
    private IEnumerator Recharge()
    {
        scaleBar = 0;
        recharge = true;
        
        yield return new WaitForSecondsRealtime(rechargeTime);
        if (generalAmmunition >= maxAmmunition)
        {
            curAmmunition = maxAmmunition;
            generalAmmunition -= maxAmmunition;
        }
        if (generalAmmunition < maxAmmunition & generalAmmunition > 0)
        {
            curAmmunition = generalAmmunition;
            generalAmmunition = 0;
        }
        txtCurAmmunition.text = "" + curAmmunition;
        txtGeneralAmmunition.text = "/ " + generalAmmunition;
        recharge = false;
    }

    //Выстрел из оружия
    private IEnumerator TakeShot() {
        curAmmunition -= 1;

        for (int i = 0; i < numberBullets; i++)
        {
            GameObject bul;

            if (i == 0)
            {
                if (typeBullet == 0)
                {
                    bul = objWarehouse.TakeTheGunBullets();

                    bul.transform.position = new Vector3(bulletPoint.position.x, bulletPoint.position.y, bulletPoint.position.z);
                    bul.transform.eulerAngles = new Vector3(bulletPoint.eulerAngles.x, bulletPoint.eulerAngles.y, bulletPoint.eulerAngles.z);
                    bul.GetComponent<BulletScript>().damage = damage;
                }
                if(typeBullet == 1)
                {
                    bul = objWarehouse.WeLoadVog();
                    bul.transform.position = new Vector3(bulletPoint.position.x, bulletPoint.position.y, bulletPoint.position.z);
                    bul.transform.eulerAngles = new Vector3(bulletPoint.eulerAngles.x, bulletPoint.eulerAngles.y, bulletPoint.eulerAngles.z);
                    bul.GetComponent<Grenade>().damage = damage;
                    bul.GetComponent<Grenade>().weaponPoint = bulletPoint.position;
                    bul.GetComponent<Grenade>().posY = disTarget / 20f;
                }
            }
            else
            {
                bul = objWarehouse.TakeTheGunBullets();

                float angleX = Random.Range(-8, 8);
                float angleY = Random.Range(-8, 8);
                bul.transform.position = new Vector3(bulletPoint.position.x, bulletPoint.position.y, bulletPoint.position.z);
                bul.transform.eulerAngles = new Vector3(bulletPoint.eulerAngles.x + angleX, bulletPoint.eulerAngles.y + angleY, bulletPoint.eulerAngles.z);
                bul.GetComponent<BulletScript>().damage = damage;
            }
        }

        GameObject ef = Instantiate(shotFlash);
        ef.transform.position = new Vector3(bulletPoint.position.x, bulletPoint.position.y, bulletPoint.position.z);
        ef.transform.eulerAngles = new Vector3(bulletPoint.eulerAngles.x, bulletPoint.eulerAngles.y, bulletPoint.eulerAngles.z);
        Destroy(ef, 0.5f);

        txtCurAmmunition.text = "" + curAmmunition;

        yield return new WaitForSecondsRealtime(shotTime);
        if (triggerPulled & curAmmunition > 0) StartCoroutine(TakeShot());
    }

    //Отвечает за стрельбу и перезарядку
    private void WeaponController() {
        if(!spawnZombies.shootingPermit)
        {
            return;
        }

        if (curAmmunition > 0) {
            StartCoroutine(TakeShot());
            return;
        }
        if (generalAmmunition != 0) {
            StartCoroutine(Recharge());
        }   
    }
}
