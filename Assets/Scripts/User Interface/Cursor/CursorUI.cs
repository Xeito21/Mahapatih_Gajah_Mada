using UnityEngine;

public class CursorUI : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D cursorPoint;
    public Texture2D targetCursor;

    public static CursorUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetCustomCursor();
    }

    public void SetCustomCursor()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorPoint()
    {
        Cursor.SetCursor(cursorPoint, Vector2.zero, CursorMode.Auto);
    }

    public void SetTargetCursor()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.Auto);
    }
}
