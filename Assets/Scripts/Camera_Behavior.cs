using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Behavior : MonoBehaviour
{
    
    [SerializeField] private Player_Behavior m_player;
    //[SerializeField] private int m_currentZoom;
    [SerializeField] private float m_cameraAxeX;
    [SerializeField] private float m_cameraAxeY;
    [SerializeField] private float m_zoom;

    public Vector3 DistanceToPlayer;

    private void Start()
    {
        //DistanceToPlayer = (transform.position - m_player.transform.position).normalized;
        m_cameraAxeX = m_player.transform.position.x;
        m_cameraAxeY = m_player.transform.position.y;
    }
    void LateUpdate()
    {
        Camera.main.transform.LookAt(m_player.transform);
        //Camera.main

        //DistanceToPlayer = (transform.position -  m_player.transform.position).normalized;
        //transform.position = m_player.transform.position + DistanceToPlayer * m_zoom;
    }
}
