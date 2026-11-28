using UnityEngine;
using UnityEngine.Events;

public class Interactable_Logic : MonoBehaviour
{
    public bool m_isInteractable;
    [HideInInspector] public Vector3 m_interactablePoint;
    [SerializeField] private float m_distancePoint = 0.6f;
    public UnityEvent m_InteractableEvent;
    

    [SerializeField] private AudioSource m_soundInteract;

    [SerializeField] private bool m_isDebugActive = false;
    void Start()
    {
        m_interactablePoint = transform.position + Vector3.forward * m_distancePoint;
        m_interactablePoint += new Vector3(0, 0.1f, 0);

        if (m_isDebugActive)
        {
            //Creation d une sphere
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = sphere.transform.localScale / 3;
            sphere.transform.SetParent(transform);
            Collider SphereCollider = sphere.GetComponent<Collider>();
            SphereCollider.enabled = false;
            sphere.transform.position = m_interactablePoint;
        }
        
    }

    public void PlayInteractableSound()
    {
        m_soundInteract.Play();
    }
}
