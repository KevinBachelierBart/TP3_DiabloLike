using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    [Header("CLASS")]
    [SerializeField] private Player_Behavior m_player;

    [Header("Variables")]
    [SerializeField] private Vector3 m_offset;//Currently not used
    private float m_zoom;
    [SerializeField] private float m_initialZoom;

    [Header("Raycast Fade Wall")]
    [SerializeField] private int m_distanceReachRaycastFadeWall;
    [SerializeField] private float m_distanceSideRaycast;
    public FadableWalls m_currentWallTouched;
    public FadableWalls m_previousFadeableWall;
    public bool isBehindWall;
    private void Start()
    {
        m_zoom = Camera.main.orthographicSize;
        m_initialZoom = m_zoom;
    }

    private void Update()
    {
        CreateRaycasts();
    }

    void CreateRaycasts()
    {
        RaycastHit hit;

        Vector3 direction = (m_player.transform.position - transform.position).normalized;
        //LLM je ne savais pas
        //comment dire a mon raycast de pointer sur un point precis
        Vector3 LeftSidePlayer = ((new Vector3(0, 0, -m_distanceSideRaycast) + m_player.transform.position) - transform.position).normalized;
        //Par contre lui cest le mien et jen suis fier
        Vector3 RightSidePlayer = ((new Vector3(0, 0, +m_distanceSideRaycast) + m_player.transform.position) - transform.position).normalized;
        
        
        Color raycastColor = Color.green;

        isBehindWall = false;
        m_currentWallTouched = null;
        if (Physics.Raycast(transform.position, direction, out hit, m_distanceReachRaycastFadeWall))
        {
            Collider objectHit = hit.collider;
            m_currentWallTouched = objectHit.GetComponent<FadableWalls>();

            if (m_currentWallTouched != null) 
            {
                isBehindWall = true;
                if (isBehindWall)
                {
                    m_currentWallTouched.FadeOut();
                    isBehindWall = false;
                }
                
            }
        }

        // Gestion du changement de mur
        if (m_previousFadeableWall != m_currentWallTouched)
        {
            // Restaurer l'ancien mur
            if (m_previousFadeableWall != null)
            {
                m_previousFadeableWall.FadeIn();
            }

            // Mémoriser le nouveau mur
            m_previousFadeableWall = m_currentWallTouched;
        }


        if (Physics.Raycast(transform.position, LeftSidePlayer, out hit, m_distanceReachRaycastFadeWall))
        {
            Collider objectHit = hit.collider;
            m_currentWallTouched = objectHit.GetComponent<FadableWalls>();

            if (m_currentWallTouched != null)
            {
                isBehindWall = true;
                if (isBehindWall)
                {
                    m_currentWallTouched.FadeOut();
                    isBehindWall = false;
                }

            }
        }

        if (Physics.Raycast(transform.position, RightSidePlayer, out hit, m_distanceReachRaycastFadeWall))
        {
            Collider objectHit = hit.collider;
            m_currentWallTouched = objectHit.GetComponent<FadableWalls>();

            if (m_currentWallTouched != null)
            {
                isBehindWall = true;
                if (isBehindWall)
                {
                    m_currentWallTouched.FadeOut();
                    isBehindWall = false;
                }

            }
        }

        Debug.DrawRay(transform.position, direction * m_distanceReachRaycastFadeWall, raycastColor);
        Debug.DrawRay(transform.position, LeftSidePlayer * m_distanceReachRaycastFadeWall, raycastColor);
        Debug.DrawRay(transform.position, RightSidePlayer * m_distanceReachRaycastFadeWall, raycastColor);

    }
    void LateUpdate()
    {
        Camera.main.transform.position = m_player.transform.position + m_offset;
    }
}
