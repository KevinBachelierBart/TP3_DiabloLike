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

    //private Vector3 m_origin;
    //private Vector3 m_destination;
    //[SerializeField] private float m_movementTime;

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
        #region Aiming (HOLD)
        if (m_aim.IsPressed())
        {
            if(m_stateMachine == m_enumStateMachine.Idle)
            {
                m_animator.SetBool("isAiming", true);
                LookAtCursor();

                SwitchEnumTo(m_enumStateMachine.Aiming);
            }
            else
            {
                Debug.LogWarning("Can't aim while doing something else than Idle");
            }
        }
        else
        {
            //Il faudrait dire a lanimator d arreter lanimation
            m_animator.SetBool("isAiming", false);
            SwitchEnumTo(m_enumStateMachine.Idle);//Es tu idle forcement a false ? A mediter
        }
        #endregion

        #region When Moving
        if (m_isMoving)
        {
            m_stateMachine = m_enumStateMachine.Walking;
            m_animator.SetBool("isWalking", true);

            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_isMoving = false;
                m_stateMachine = m_enumStateMachine.Idle;
                m_animator.SetBool("isWalking", false);
            }


        }
        #endregion

        #region LeftClick
        if (m_leftClick.WasPressedThisFrame())
        {
            if (m_stateMachine == m_enumStateMachine.Idle || m_stateMachine == m_enumStateMachine.Walking)
            {
                ClickToMove();

            }
            else if (m_stateMachine == m_enumStateMachine.Aiming || m_stateMachine == m_enumStateMachine.Shooting)
            {
                SwitchEnumTo(m_enumStateMachine.Shooting);
                m_sound1911.Play();

                m_animator.SetBool("isShooting", true);
                m_animator.Play("CharacterArmature_Idle_Gun_Shoot");
            }
            
        }
        #endregion
    }

    void ClickToMove()
    {
        Ray rayFromCamera = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        int mouseRaycastMask = ~LayerMask.GetMask("IgnoreMouseRaycast");//Exclu ce layer lors du raycasting
        //1h à chercher cette logique pour qu'un LLM me la donne en 1min...

        if (Physics.Raycast(rayFromCamera, out hit, 100f, mouseRaycastMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //Particle effect
                m_clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                m_clickEffect.transform.position = hit.point;
                m_clickEffect.Play();

                //Agent moving
                m_agent.destination = hit.point;
                m_agent.transform.LookAt(hit.point);

                m_isMoving = true;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                Debug.Log("Tu as clicke sur un interactable Object");
            }


        }
    }

    void LookAtCursor()//Pour le aiming - PETE
    {
        Vector3 target = GetWorldMousePosition();
        target.y = transform.position.y;

        transform.LookAt(target);
    }

    private Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Je comprends que je fais un ray qui a une originie et une direction dans une 3D et
        //je tranforme la position 2d de la souris EN un ray 3D
        //On peut voir le ray comme une droite infinie ? 
        //ray = rayon qui part de la camera vers la souris.

        //LLM CI DESSOUS
        Plane groundPlane = new Plane(Vector3.up, transform.position);//Creation dun plane horizontal

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);//Renvoie-moi sa position en 3D 
        }

        return transform.position;
        //LLM CI DESSUS
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

    void CalculateCollider()
    {

    }
}
