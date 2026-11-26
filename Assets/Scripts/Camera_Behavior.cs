using System.Xml.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    [SerializeField] private Player_Behavior m_player;

    [SerializeField] private Vector3 m_offset;//Currently not used
    private float m_zoom;
    [SerializeField] private float m_initialZoom;

    [Header("Raycast Fade Wall")]
    [SerializeField] private int m_distanceRaycastFadeWall;
    private void Start()
    {
        m_zoom = Camera.main.orthographicSize;
        m_initialZoom = m_zoom;
    }

    private void Update()
    {
        RaycastHit hit;

        Vector3 direction = (m_player.transform.position - transform.position).normalized;
        //LLM je ne savais pas
        //comment dire a mon raycast de pointer sur un point precis

        Debug.DrawRay(transform.position, direction * m_distanceRaycastFadeWall, Color.green);

        if (Physics.Raycast(transform.position, direction, out hit, m_distanceRaycastFadeWall))
        {
            Collider objectHit = hit.collider;
            FadableWalls fadableWallsScript = objectHit.GetComponent<FadableWalls>();

            if (fadableWallsScript != null)
            {
                Debug.Log("fadableWallsScript detected");
                fadableWallsScript.FadeOut();
            }

        }
        
    }

    void LateUpdate()
    {
        Camera.main.transform.position = m_player.transform.position + m_offset;
    }
}
