using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;
    public CanvasGroup menuCanvasGroup;
    public CanvasGroup SettingsCanvas;
    public KeyCode toggleKey = KeyCode.Escape;

    [Header("Number Display")]
    public Image[] strengthDigits; 
    public Image[] vitalityDigits; 
    public Image[] staminaDigits; 
    public Image[] intelligenceDigits; 
    public Image[] defenseDigits; 
    public Sprite[] numberSprites; 

    private bool isMenuOpen = false;
    private GameObject playerz;
    public GameObject cursor;


    [Header("Player Stats Reference")]
    public PlayerStats playerStats; 
    TutorialManager tutorial;

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
        tutorial = FindObjectOfType<TutorialManager>();
       // FindPlayer();
        SetMenuVisibility(false, false);
        FindPlayer();
        setNewPlayerStats();
    }

    void Update()
    {
        if (tutorial == null || tutorial.allowStats)
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (isMenuOpen == false && Time.timeScale == 0)
                {
                    return;
                }
                isMenuOpen = !isMenuOpen;
                SetMenuVisibility(isMenuOpen, true);

                if (isMenuOpen)
                {
                    UpdateMenuStats();
                }
            }
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerz = playerObj;
        }
        else
        {
            Debug.LogWarning("No player found!");
        }
    }

    public void setNewPlayerStats()
    {
        Classes player = playerz.GetComponent<Classes>();
        playerStats.Strength = player.attack;
        playerStats.Vitality = player.vit;
        playerStats.Stamina = player.movementSpeed;
        playerStats.Intelligence = player.intelligence;
        playerStats.Defense = player.defense;

        UpdateMenuStats();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        // New Map
    }

    public void newRun()
    {
        SceneManager.LoadScene("Selection");
    }

    public void unPause()
    {
        isMenuOpen = !isMenuOpen;
        SetMenuVisibility(false, false);
    }

    void SetMenuVisibility(bool visible, bool fade)
    {
        Time.timeScale = visible ? 0f : 1f;

        if (visible == true)
        {
            cursor.SetActive(true);
        }
        else
        {
            cursor.SetActive(false);
        }

        if (fade)
        {
            StartCoroutine(FadeMenu(visible));
        }
        else
        {
            menuCanvasGroup.alpha = visible ? 1f : 0f;
            menuCanvasGroup.interactable = visible;
            menuCanvasGroup.blocksRaycasts = visible;

            SettingsCanvas.alpha = visible ? 1f : 0f;
            SettingsCanvas.interactable = visible;
            SettingsCanvas.blocksRaycasts = visible;
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
            SettingsCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        menuCanvasGroup.alpha = endAlpha;
        menuCanvasGroup.interactable = fadeIn;
        menuCanvasGroup.blocksRaycasts = fadeIn;

        SettingsCanvas.alpha = endAlpha;
        SettingsCanvas.interactable = fadeIn;
        SettingsCanvas.blocksRaycasts = fadeIn;
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
