using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;
    public CanvasGroup menuCanvasGroup;
    public KeyCode toggleKey = KeyCode.Escape;

    [Header("Number Display")]
    public Image[] strengthDigits; 
    public Image[] vitalityDigits; 
    public Image[] staminaDigits; 
    public Image[] intelligenceDigits; 
    public Image[] defenseDigits; 
    public Sprite[] numberSprites; 

    private bool isMenuOpen = false;

    [Header("Player Stats Reference")]
    public PlayerStats playerStats; 

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Debug.LogError(" Multiple MenuUIManager instances detected! Destroying duplicate.");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetMenuVisibility(false, false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isMenuOpen = !isMenuOpen;
            SetMenuVisibility(isMenuOpen, true);

            if (isMenuOpen)
            {
                UpdateMenuStats();
            }
        }
    }

    void SetMenuVisibility(bool visible, bool fade)
    {
        Time.timeScale = visible ? 0f : 1f;

        if (fade)
        {
            StartCoroutine(FadeMenu(visible));
        }
        else
        {
            menuCanvasGroup.alpha = visible ? 1f : 0f;
            menuCanvasGroup.interactable = visible;
            menuCanvasGroup.blocksRaycasts = visible;
        }
    }

    IEnumerator FadeMenu(bool fadeIn)
    {
        float duration = 0.3f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        menuCanvasGroup.alpha = endAlpha;
        menuCanvasGroup.interactable = fadeIn;
        menuCanvasGroup.blocksRaycasts = fadeIn;
    }

    public void UpdateMenuStats()
    {
        if (playerStats != null) // Ensure playerStats is assigned
        {
            UpdateNumberDisplay(strengthDigits, playerStats.Strength);
            UpdateNumberDisplay(vitalityDigits, playerStats.Vitality);
            UpdateNumberDisplay(staminaDigits, playerStats.Stamina);
            UpdateNumberDisplay(intelligenceDigits, playerStats.Intelligence);
            UpdateNumberDisplay(defenseDigits, playerStats.Defense);
        }
        else
        {
            Debug.LogError("PlayerStats is not assigned in the MenuUIManager!");
        }
    }

    void UpdateNumberDisplay(Image[] digitImages, int value)
    {
        string valueStr = value.ToString();

        // Ensure the number display fits within the available digit slots
        for (int i = 0; i < digitImages.Length; i++)
        {
            if (i < valueStr.Length)
            {
                int digit = valueStr[i] - '0'; // Convert character to integer
                digitImages[i].sprite = numberSprites[digit]; // Replace sprite
                digitImages[i].enabled = true; // Ensure it's visible
            }
            else
            {
                digitImages[i].enabled = false; // Hide unused digits
            }
        }
    }
}
