using UnityEngine;

public class CursorHover : MonoBehaviour
{
    public CursorSc customCursor; // Reference to the custom cursor script

    public Sprite defaultCursor;
    public Sprite hoverAttackCursor;
    public Sprite hoverQuestionCursor;
    public Sprite hoverSettingsCursor;

    private Sprite currentCursor; // Track current cursor to prevent redundant updates

    private void Start()
    {
        if (customCursor == null)
        {
            Debug.LogError("CursorHoverManager: No CursorSc script assigned!");
            return;
        }

        SetCursor(defaultCursor);
    }

    void Update()
    {
        if (customCursor == null) return; // Ensure customCursor is assigned

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Uses Physics.Raycast() for 3D BoxColliders
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

    private void SetCursor(Sprite newCursor)
    {
        if (newCursor != null && currentCursor != newCursor) // Prevent unnecessary updates
        {
            customCursor.SwapCursor(newCursor); // Call custom cursor script
            currentCursor = newCursor;
        }
    }
}
