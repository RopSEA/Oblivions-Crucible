using UnityEngine;

public class UICategory : MonoBehaviour
{
    public enum CategoryType { Default, Attack, Question, Settings }
    public CategoryType category = CategoryType.Default; // Assign this in the Inspector
}
