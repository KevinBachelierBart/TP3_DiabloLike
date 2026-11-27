using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAndCursor_Manager : MonoBehaviour
{
    public Texture2D m_idleCursor;
    private Vector2 m_idleCursorPos;
    public Texture2D m_investigateCursor;
    private Vector2 m_investigateCursorPos;
    public Texture2D m_attackCursor;

    void Start()
    {
        m_idleCursorPos = new Vector2(8, 21);
        m_investigateCursorPos = new Vector2(8, 21);


        Cursor.SetCursor(m_idleCursor, m_idleCursorPos, CursorMode.Auto);
    }
    void Update()
    {

        Ray rayFromCamera = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(rayFromCamera, out hit, 100f))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Cursor.SetCursor(m_idleCursor, m_idleCursorPos, CursorMode.Auto);

            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                Cursor.SetCursor(m_investigateCursor, m_investigateCursorPos, CursorMode.Auto);

            }
        }

    }
}