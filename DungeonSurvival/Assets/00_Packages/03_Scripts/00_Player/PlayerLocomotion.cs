using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] float turnSpeed = 5;
    [SerializeField] float moveSpeed = 3;

    CharacterController controller;
    Vector3 velocity;

    [SerializeField] float characterGravity = 9;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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

        direction *= moveSpeed;
        direction.y = -characterGravity;
        velocity = direction * Time.deltaTime;

        controller.Move(velocity);
    }
}
