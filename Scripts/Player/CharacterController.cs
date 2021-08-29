using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Camera _camera;
    private Animator animator;
    private float ver;
    private float hor;
    private Vector3 intersectionPoint;

    public GameObject bullet;
    public Transform weaponPoint;
    public Transform bulletPoint;

    public float walkSpeed;
    public float runSpeed;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            intersectionPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            if (hit.transform != gameObject.transform) {
                Vector3 target = new Vector3(hit.point.x, 0f, hit.point.z);
                transform.LookAt(target);

                bulletPoint.transform.LookAt(hit.point);
            }
            if (hit.transform.CompareTag("Enemy"))
            {
                bulletPoint.transform.LookAt(hit.point);
            }
            else 
            {
                bulletPoint.localEulerAngles = new Vector3(0, 90, 0);    
            }
        }

        AnimationsController();
        Shooting();
    }

    private void AnimationsController() {

        ver = Input.GetAxis("Vertical");
        hor = Input.GetAxis("Horizontal");

        animator.SetFloat("Direction", hor);
        animator.SetFloat("Speed", ver);

        transform.Translate(new Vector3(hor * walkSpeed * Time.deltaTime, 0, ver * walkSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (ver == 0) {
                animator.SetFloat("Speed", ver + 0.2f);
            }
        }
        else {
            animator.SetFloat("Speed", ver);
        }

        if (Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.W))
        {
            animator.SetFloat("Speed", ver * 2f);
            transform.Translate(new Vector3(hor * runSpeed * Time.deltaTime, 0, ver * runSpeed * Time.deltaTime));
        }
    }

    private void Shooting() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            GameObject bul = Instantiate(bullet, bulletPoint.transform.position, Quaternion.identity);
            bul.transform.localEulerAngles = new Vector3(bulletPoint.transform.eulerAngles.x, bulletPoint.transform.eulerAngles.y, bulletPoint.transform.eulerAngles.z);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bulletPoint.position, bulletPoint.GetChild(0).position);
    }
}
