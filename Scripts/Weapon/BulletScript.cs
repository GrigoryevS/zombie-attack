using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;
    public float speed;
    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward.normalized * speed);

        if (lifetime >= 0) {
            lifetime -= 1 * Time.deltaTime;
        }
        if (lifetime <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<RagdollControl>())
        {
            other.gameObject.GetComponent<RagdollControl>().ObjectDamage(damage);
            other.gameObject.GetComponent<RagdollControl>().BloodSplatter(gameObject.transform);
        }
        gameObject.SetActive(false);
    }
}
