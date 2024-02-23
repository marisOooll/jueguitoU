using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRB;

    [Header("Movimiento")]
    private float mH = 0f;
    [SerializeField] private float velocidadMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadorDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool isGrounded;
    private bool salto = false;

    private EntradasMovimiento entradasMovimiento;

    [Header("Animacion")]
    private Animator animator;

    private void Awake()
    {
        entradasMovimiento = new EntradasMovimiento();
    }

    private void OnEnable()
    {
        entradasMovimiento.Enable();
    }

    private void OnDisable()
    {
        entradasMovimiento.Disable();
    }

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        entradasMovimiento.Movimiento.Salto.performed += contexto => Jump(contexto);
    }

    private void Update()
    {
        mH = entradasMovimiento.Movimiento.Horizontal.ReadValue<float>() * velocidadMovimiento;

        animator.SetFloat("Horizontal", Mathf.Abs(mH));

        animator.SetFloat("VelocidadY", playerRB.velocity.y);

    }

    private void Jump(InputAction.CallbackContext contexto)
    {
        salto = true;
    }

    private void FixedUpdate() //Cambios en las f�sicas, mojorar rendimineto
    {
        isGrounded = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo", isGrounded);
        //Mover
        Move(mH * Time.fixedDeltaTime, salto);

        salto = false;
    }

    private void Move(float mover, bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, playerRB.velocity.y);
        playerRB.velocity = Vector3.SmoothDamp(playerRB.velocity, velocidadObjetivo, ref velocidad, suavizadorDeMovimiento); //suavizado a la hora de frenar

        if (mover > 0 && !mirandoDerecha)
        {
            Flip();

        }
        else if (mover < 0 && mirandoDerecha)
        {
            Flip();
        }

        if (isGrounded && saltar)
        {
            isGrounded = false;
            playerRB.AddForce(new Vector2(0f, fuerzaSalto));
        }
    }

    private void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}