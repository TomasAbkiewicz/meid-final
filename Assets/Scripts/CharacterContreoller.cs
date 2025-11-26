using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContreoller : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 6f;
    public float sprintMultiplier = 1.8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    Vector3 velocity;

    void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // inputs
        float x = Input.GetAxis("Horizontal"); // A/D, izquierda/derecha
        float z = Input.GetAxis("Vertical");   // W/S, adelante/atrás

        // calcular velocidad (sprint con Left Shift)
        bool sprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = walkSpeed * (sprinting ? sprintMultiplier : 1f);

        // movimiento relativo a la orientación del jugador
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1f) move.Normalize();

        controller.Move(move * currentSpeed * Time.deltaTime);

        // salto y gravedad (usamos CharacterController.isGrounded)
        if (controller.isGrounded && velocity.y < 0)
        {
            // pequeño empujón hacia abajo para mantener contacto con suelo
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // v = sqrt(2 * h * -g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
