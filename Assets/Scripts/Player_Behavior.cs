using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player_Behavior : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_inputAction;
    private InputAction m_aim;
    private InputAction m_leftClick;

    [Header("NAVMESH")]
    private NavMeshAgent m_agent;

    [Header("DESIGN")]
    [SerializeField] private m_enumStateMachine m_stateMachine;
    public enum m_enumStateMachine 
    
    {
        Idle,
        Walking,
        Aiming,
        Shooting
    };

    private bool m_isMoving = false;
    public Vector3 velocit;
    public float distanceToHit;
    [Space(10)]
    [Header("ANIMATION & VISUAL")]
    private Animator m_animator;
    [SerializeField] private ParticleSystem m_clickEffect;

    [Header("SOUNDS")]
    [SerializeField] private AudioSource m_sound1911;
    void Start()
    {
        m_aim = m_inputAction.FindAction("Aim");
        m_leftClick= m_inputAction.FindAction("Move");

        m_animator = GetComponent<Animator>();

        m_agent = GetComponent<NavMeshAgent>();

        SwitchEnumTo(m_enumStateMachine.Idle);

        m_isMoving = false;
    }

    void Update()
    {
        if (m_aim.IsPressed())
        {
            m_animator.SetBool("isAiming", true);
            LookAtCursor();

            SwitchEnumTo(m_enumStateMachine.Aiming);

        }
        else
        {
            m_animator.SetBool("isAiming", false);
            SwitchEnumTo(m_enumStateMachine.Idle);//Es tu idle forcement a false ? 
        }
        

        if (m_leftClick.WasPressedThisFrame())
        {
            if(m_stateMachine == m_enumStateMachine.Idle)
            {
                ClickToMove();

            }
            else if (m_stateMachine == m_enumStateMachine.Aiming)
            {
                SwitchEnumTo(m_enumStateMachine.Shooting);
                m_sound1911.Play();

                m_animator.SetBool("isShooting", true);
            }
            
        }

        Debug.Log("isMoving = " + m_isMoving);
        velocit = m_agent.velocity;
        if (m_isMoving)
        {
            distanceToHit = m_agent.remainingDistance;
            if (distanceToHit >= 0.01)
            {
                m_stateMachine = m_enumStateMachine.Walking;
                m_animator.SetBool("isWalking", true);
            }
            else
            {
                m_animator.SetBool("isWalking", false);
                m_isMoving = false;
            }
        }
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 100f))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                m_clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                m_clickEffect.transform.position = hit.point;
                m_clickEffect.Play();
                m_agent.destination = hit.point;

                m_isMoving = true;
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

    public void EndAnimationShooting()
    {
        SwitchEnumTo(m_enumStateMachine.Aiming);
        m_animator.SetBool("isShooting", false);
        Debug.Log("EndAnimationShooting");
       
    }

    public void SwitchEnumTo(m_enumStateMachine newState)
    {
        m_stateMachine = newState;
    }
}
