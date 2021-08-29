using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RagdollControl : MonoBehaviour
{
    public Text txtMaxHP;//òåêñ îòîáğàæàşùèé ìàêñèìàëüíîå çäîğîâüå
    public Text txtCurHP;//òåêñò îòîáğàæàşùèé òåêóùåå çäîğîâüå

    public float defaultHP;
    public float maxHP;
    public float curHP;

    public Animator animator;
    public Rigidbody[] allRigidbodys;

    private ObjectWarehouse objectWarehouse;
    private WeaponÑontroller weaponÑontroller;

    void Awake() {

        if(GameObject.FindWithTag("Weapon Controller")) { weaponÑontroller = GameObject.FindWithTag("Weapon Controller").GetComponent<WeaponÑontroller>(); }
        if(GameObject.FindWithTag("Object Warehouse")) { objectWarehouse = GameObject.FindWithTag("Object Warehouse").GetComponent<ObjectWarehouse>(); };

        curHP = maxHP;

        if (gameObject.CompareTag("Player") & txtMaxHP & txtCurHP) {
            txtMaxHP.text = "/ " + maxHP;
            txtCurHP.text = "" + maxHP;
        }

        DisablingColliders();
    }


    // Update is called once per frame

    public void ObjectDamage(float damge)
    {
        curHP -= Mathf.Ceil(damge);
        if (gameObject.CompareTag("Player"))
        {
            txtCurHP.text = "" + curHP;
        }
        if (curHP <= 0)
        {
            MakePhysical();
            GameObject.FindWithTag("Spownes Zombi").GetComponent<SpawnZombies>().PopulationRegulation();
        }
    }

    public void BloodSplatter(Transform other)
    {

        GameObject effect = objectWarehouse.WeLoadBloodEffect();
        effect.transform.position = other.position;
        effect.transform.eulerAngles = new Vector3(other.eulerAngles.x, other.eulerAngles.y, other.eulerAngles.z);
    }

    public void DisablingColliders()
    {
        for (int i = 0; i < allRigidbodys.Length; i++)
        {
            allRigidbodys[i].isKinematic = true;
            allRigidbodys[i].gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void MakePhysical()
    {
        if (gameObject.CompareTag("Enemy")){
            GetComponent<WalkingControler>().enabled = false;
            float other = Mathf.Ceil(Random.Range(maxHP / 2, maxHP)/30);
            weaponÑontroller.DebitPluse(other);
        }
        if (gameObject.CompareTag("Player")){
            Camera.main.GetComponent<MoveCamera>().character = null;
            GetComponent<PlayerController>().enabled = false;
            Time.timeScale = 0.1f;
        }

        GetComponent<Rigidbody>().useGravity = false;
        animator.enabled = false;

        for (int i = 0; i < allRigidbodys.Length; i++)
        {
            GetComponent<Collider>().enabled = false;
            allRigidbodys[i].isKinematic = false;
            allRigidbodys[i].gameObject.GetComponent<Collider>().enabled = true;
        }
    }
}
