using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ObjectWarehouse objectWarehouse;//банк объектов

    public Transform weaponPoint;//пустышка на каторой висит оружие
    public int typeGrenate;//тип гранаты который используетс€
    public Text txtMaxGrenate;
    public Text txtCurGrenate;

    public float speedPlayer;//скорость передвижени€ игрока
    private float gradatioon = 1;

    private Transform cam;//трансформ камены игрока
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;
    private Animator animator;

    private void Awake()
    {
        if (GameObject.FindWithTag("Object Warehouse")) { objectWarehouse = GameObject.FindWithTag("Object Warehouse").GetComponent<ObjectWarehouse>(); }
        
        animator = gameObject.GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {   

        Ray ray = cam.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.CompareTag("Ground"))
            {
                Vector3 weaponAngle = new Vector3(hit.point.x, 1.2f, hit.point.z);
                weaponPoint.LookAt(weaponAngle);
            }

            else
            {
                weaponPoint.LookAt(hit.point);
            }

            if (hit.transform & hit.transform != gameObject.transform)
            {
                Vector3 target = new Vector3(hit.point.x, 0f, hit.point.z);

                transform.LookAt(target);

                if (transform.eulerAngles.x != 0 || transform.eulerAngles.z != 0)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.G)) {
                Vector3 target = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                ThrowingGrenade(target);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            objectWarehouse.DetonatingBomb();
        }

        GlobalMovement();
    }

    private void GlobalMovement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (cam)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = z * camForward + x * cam.right;
        }
        else
        {
            move = z * Vector3.forward + x * Vector3.right;
        }
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        Move(move);
    }

    private void Move(Vector3 move){
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        this.moveInput = move;
        ConvertMoveInput();
        UpdateAnimator();
    }

    private void ConvertMoveInput() {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;

        if (forwardAmount <= -0.7f & gradatioon > 0.7f)
        {
            gradatioon -= 0.5f * Time.deltaTime;
        }
        if (forwardAmount >= -0.5f & gradatioon < 1f) 
        {
            gradatioon += 0.5f * Time.deltaTime;
        }

        transform.Translate(turnAmount * Time.deltaTime * speedPlayer * gradatioon, 0, forwardAmount * Time.deltaTime * speedPlayer * gradatioon);
    }

    private void UpdateAnimator() {
        animator.SetFloat("Z", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("X", turnAmount, 0.1f, Time.deltaTime);
    }

    //ћетод отвечающий за бросок гранаты гратаны
    private void ThrowingGrenade(Vector3 target) {

        float dis = Vector3.Distance(gameObject.transform.position, target);
        GameObject grenade = objectWarehouse.WeloadGrenade(typeGrenate);
        grenade.GetComponent<Grenade>().weaponPoint = weaponPoint.position;
        grenade.GetComponent<Grenade>().SettingSpeed(dis / 20f);
        grenade.transform.position = new Vector3(weaponPoint.position.x, weaponPoint.position.y, weaponPoint.position.z);
        grenade.transform.LookAt(target);

    }
}
