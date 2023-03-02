using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Change : MonoBehaviour
{
    public Cursor_Change instance = null;
    [SerializeField] Texture2D cursorImg;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }

}
