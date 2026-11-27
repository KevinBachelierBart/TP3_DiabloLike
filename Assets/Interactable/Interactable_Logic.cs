using UnityEngine;

public class Interactable_Logic : MonoBehaviour
{
    [SerializeField] private Vector3 m_interactablePoint;
    [SerializeField]private int m_distancePoint = 2;
    void Start()
    {
        m_interactablePoint = transform.position + Vector3.forward * m_distancePoint;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = m_interactablePoint;
        sphere.transform.localScale = sphere.transform.localScale / 2;
        sphere.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
