using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Collider[] overlappedColliders;//������ ���� ���������� � ���� ������
   
    public Vector3 weaponPoint;//����� ������� ������ � �����

    public float damage;//���� ��������� ��������
    public float radius;//������ ������ �������
    public float force;//���� ������
    public int grenadeType;//0 - �������; 1 - ����; 2 - �����; 

    public GameObject bigExplosionEffect;//����� ������
    public float grenadeSpeed;//�������� ������ �������
    public float posY;//������� �� ��� Y 
    public Vector3 grenadePos;

    // Update is called once per frame
    void FixedUpdate()
    {
        FlightGrants();

        if (transform.position.y < -30f) {
            gameObject.transform.position = weaponPoint;
            gameObject.SetActive(false);
            bigExplosionEffect.SetActive(false);

            if (gameObject.GetComponent<MeshCollider>())
            {
                GetComponent<MeshCollider>().enabled = true;
            }
        }
    }

    //������ �������� ����� ������ �������
    public void SettingSpeed(float pos)
    {
        if(grenadeType == 0 || grenadeType == 2)
        {
            posY = pos;
        }
        if(grenadeType == 1)
        {
            posY = 0.1f;
        }
    }

    private void FlightGrants() {

        posY -= 0.1f * Time.deltaTime * grenadeSpeed;
        grenadePos = new Vector3(0, posY, 1);

        transform.Translate(grenadePos * Time.deltaTime * grenadeSpeed);
    }

    public void Expplode() {
        if(grenadeType == 1 || grenadeType == 2)
        {
            GetComponent<MeshCollider>().enabled = false;
        }

        overlappedColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < overlappedColliders.Length; i++) {
            RagdollControl ragdollControl = overlappedColliders[i].gameObject.GetComponent<RagdollControl>();
            if (ragdollControl) {
                float dis = Vector3.Distance(transform.position, ragdollControl.gameObject.transform.position);
                ragdollControl.ObjectDamage(damage / dis);
            }
        }
        overlappedColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force, transform.position, radius, 2f);
            }
        }

        bigExplosionEffect.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if(grenadeType == 0)
            {
                Expplode();   
            }
            if(grenadeType == 1)
            { 
                grenadePos = transform.position;
                grenadeSpeed = 0;
            }
            if (grenadeType == 2)
            {
                grenadePos = transform.position;
                grenadeSpeed = 0;
            }
        }
        if (other.CompareTag("Enemy") & grenadeType == 1)
        {
            Expplode();
        }
    }
}
