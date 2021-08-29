using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingControler : MonoBehaviour
{
    public float speedWolk;
    public float damage;
    public float defaultDamage;

    private bool attack;
    private float distans;
    private GameObject player;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Wolk();

        if (Input.GetKeyDown(KeyCode.Space) & gameObject.tag != "Player")
        {
            gameObject.isStatic = true;
        }
    }

    private void Wolk() {
        distans = Vector3.Distance(transform.position, player.transform.position);

        transform.LookAt(player.transform);

        if (distans > 1 & attack == false) {
            transform.Translate(Vector3.forward * Time.deltaTime * speedWolk);
        }

        if (distans < 1.5f & attack == false) {
            animator.SetBool("Attack", true);
            StartCoroutine(Attack());
        }

        if (distans > 2f) {
            animator.SetBool("Attack", false);
        }
    }

    IEnumerator Attack() {
        attack = true;
        yield return new WaitForSecondsRealtime(0.5f);
        if (distans < 2f) {
            player.GetComponent<RagdollControl>().ObjectDamage(damage);
        }
        yield return new WaitForSecondsRealtime(1.2f);
        attack = false;
    }
}
