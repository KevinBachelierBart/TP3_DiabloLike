using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Behavior : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_inputAction;
    private InputAction m_aim;
    private InputAction m_move;

    [Header("ANIMATION & VISUAL")]
    private Animator m_animator;
    [SerializeField] private ParticleSystem m_clickEffect;


    void Start()
    {
        m_aim = m_inputAction.FindAction("Aim");
        m_move= m_inputAction.FindAction("Move");

        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_aim.IsPressed())
        {
            m_animator.SetBool("isAiming", true);
            LookAtCursor();
        }
        else
        {
            m_animator.SetBool("isAiming", false);
        }

        if (m_move.WasPressedThisFrame())
        {
            ClickToMove();
        }

    }

    void ClickToMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 100f))
        {
            Debug.Log("Click hit");
            LayerMask layerHit = hit.collider.gameObject.layer;
            if(layerHit == LayerMask.NameToLayer("Ground"))
            {
                m_clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                m_clickEffect.transform.position = hit.point;
                m_clickEffect.Play();
            }
           
        }
    }

    void LookAtCursor()
    {
        Vector3 target = GetWorldMousePosition();
        target.y = transform.position.y;

        transform.LookAt(target);
    }


    private Vector3 GetWorldMousePosition()//LLM
    {
        Ray ray = Camera.main.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );//Je comprends je fais un ray qui a une originie et une direction dans une 3D et
              //je tranforme la position 2d de la souris EN un ray 3D
              //On peut voir le ray comme une droite infinie ? 
              //ray = rayon qui part de la camera vers la souris.

        Plane groundPlane = new Plane(Vector3.up, transform.position);//Creation dun plane horizontal

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);//Renvoie-moi sa position en 3D 
        }

        return transform.position;
    }
}
