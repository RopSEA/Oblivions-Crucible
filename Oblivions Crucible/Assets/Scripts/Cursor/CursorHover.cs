using UnityEngine;

public class CursorHoverManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D hoverAttackCursor;
    public Texture2D hoverQuestionCursor;
    public Texture2D hoverSettingsCursor;

    public Vector2 cursorHotspot = Vector2.zero;
    private Texture2D currentCursor;

    private void Start()
    {
        SetCursor(defaultCursor);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) //  Uses Physics.Raycast() for 3D BoxColliders
        {

            GameObject hoveredObject = hit.collider.gameObject;
            UICategory categoryComponent = hoveredObject.GetComponent<UICategory>();

            if (categoryComponent != null)
            {
                switch (categoryComponent.category)
                {
                    case UICategory.CategoryType.Attack:
                        SetCursor(hoverAttackCursor);
                        break;
                    case UICategory.CategoryType.Question:
                        SetCursor(hoverQuestionCursor);
                        break;
                    case UICategory.CategoryType.Settings:
                        SetCursor(hoverSettingsCursor);
                        break;
                    default:
                        SetCursor(defaultCursor);
                        break;
                }
            }
            else
            {
                SetCursor(defaultCursor);
            }
        }
        else
        {
            SetCursor(defaultCursor);
        }
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        if (cursorTexture != null && currentCursor != cursorTexture) // Prevents unnecessary updates
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
            currentCursor = cursorTexture;
            Debug.Log($"Cursor changed to: {cursorTexture.name}");
        }
    }
}
