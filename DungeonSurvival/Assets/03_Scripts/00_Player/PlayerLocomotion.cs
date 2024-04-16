using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    public static PlayerLocomotion current { get; private set; }

    [SerializeField] float SetSpeedSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float turnSpeed = 5;

    CharacterController controller;
    Animator anim;

    [SerializeField] float characterGravity = 9;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerComponents.instance.lockedAll) return;

        Vector3 frontDir = CameraController.current.transform.forward * Input.GetAxis("Vertical");
        Vector3 sideDir = CameraController.current.transform.right * Input.GetAxis("Horizontal");
        Vector3 direction = (frontDir + sideDir).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }

        float speed = direction.sqrMagnitude;
        anim.SetFloat("Speed", speed, 0.1f, Time.deltaTime * moveSpeed); //Personaje se mueve por animacion sin root, bueno para que cuando ataque se haga un override del layer del animator y se pare de caminar
                                                                         //y no pueda seguir caminando mientras ataca

        controller.Move(Vector3.down * characterGravity);
        //controller.Move(direction * moveSpeed * Time.deltaTime);
    }
}
