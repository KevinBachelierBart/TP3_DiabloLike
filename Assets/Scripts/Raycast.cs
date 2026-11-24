using JetBrains.Annotations;
using NUnit.Framework;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [Header("Design")]
    public RaycastDirection m_raycastDirection;
    public enum RaycastDirection
    {
        Forward,
        Back,
        Left,
        Right,
        Up,
        Down

    };
    [SerializeField] private int m_distanceRaycast;

    private Vector3 m_directionRaycast;
    //[Header("-------------------------------DEBUG---------------------")]
    [SerializeField] private bool m_ShowRaycast;
    private void Start()
    {
        if(m_raycastDirection == RaycastDirection.Forward)
        {
            m_directionRaycast = new Vector3(0, 0, 1);
            //m_directionRaycast = transform.Vector3(0, 0, 1);

        }
        else if (m_raycastDirection == RaycastDirection.Back)
        {
            //m_directionRaycast = new Vector3(0, 0, -1);
            m_directionRaycast = transform.TransformDirection(new Vector3(0, 0, -1));

        }
        else if (m_raycastDirection == RaycastDirection.Left)
        {
            //m_directionRaycast = new Vector3(-1, 0, 0);
            m_directionRaycast = transform.TransformDirection(new Vector3(-1, 0, 0));

        }
        else if (m_raycastDirection == RaycastDirection.Right)
        {
            m_directionRaycast = transform.TransformDirection(new Vector3(1, 0, 0));

        }
        else if (m_raycastDirection == RaycastDirection.Up)
        {
            m_directionRaycast = transform.TransformDirection(new Vector3(0, 1, 0));
        }
        else if (m_raycastDirection == RaycastDirection.Down)
        {
            m_directionRaycast = transform.TransformDirection(new Vector3(0, -1, 0));
        }
    }

    void Update()
    {
        if (m_ShowRaycast)
        {
            Debug.DrawRay(transform.position, m_directionRaycast * 50f, Color.red);
        }

        if (Physics.Raycast(transform.position, m_directionRaycast, out RaycastHit hitInfo, m_distanceRaycast))
        {
            Collider objectHit = hitInfo.collider;
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, m_directionRaycast * 50f);
    }*/
}
