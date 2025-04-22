using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public RPGTalk rpgTalk;
    public MovementTracker movementTracker;  // Reference to your MovementTracker
    
    private int tutorialStep = 0;

    private bool hasMoved = false;
    private bool hasAttacked = false;
    private bool hasDodged = false;
    private bool usedPrimarySkill = false;
    private bool usedSecondarySkill = false;

    private bool hasOpenedStats = false;
    private bool shopOpened = false;
    private bool finalFightTriggered = false;
    private bool statsMenuOpened = false;  

    public bool allowDodge = false;
    public bool allowSkills = false;
    public bool allowStats = false;

    public ShopDisplay shopDisplay;
    private bool waitingForShopClose = false;

    public TutorialEnemySpawner enemySpawner; 

        public bool HasMoved()
    {
        return hasMoved;
    }

    public void OnTutorialWaveComplete()
    {
        Debug.Log("Tutorial enemies defeated — end the tutorial.");
        EndTutorial();
    }

    void Start()
    {
        if (shopDisplay != null)
        {
            shopDisplay.OnShopClosed.AddListener(OnShopClosed);
        }
        movementTracker.OnAllDirectionsMoved.AddListener(OnMovementComplete);
        rpgTalk.OnEndTalk += OnDialogFinished;

        StartIntro();
    }

    void OnMovementComplete()
    {
        if (tutorialStep == 1 && !hasMoved)
        {
            hasMoved = true;
            GoToBasicAttack();
        }
    }
    void OnShopClosed()
{
    if (tutorialStep == 6 && !shopOpened)
    {
        shopOpened = true;
        GoToFinalFight();
    }
}

    void Update()
    {
        if (rpgTalk.isPlaying) return;
        switch (tutorialStep)
        {
            case 2:
                // Waiting for dummy kill to call OnDummyDestroyed()
                break;
            case 3:
                if (!hasDodged && Input.GetKeyDown(KeyCode.Space))
                {
                    hasDodged = true;
                    GoToAbilitiesIntro();
                }
                break;
            case 4:
                if (!usedPrimarySkill && Input.GetKeyDown(KeyCode.Q))
                {
                    usedPrimarySkill = true;
                    Debug.Log("Primary skill (Q) used");
                }

                if (!usedSecondarySkill && Input.GetKeyDown(KeyCode.E))
                {
                    usedSecondarySkill = true;
                    Debug.Log("Secondary skill (E) used");
                }

                if (usedPrimarySkill && usedSecondarySkill)
                {
                    GoToStats();
                }
                break;

            case 5:
                if (!statsMenuOpened && Input.GetKeyDown(KeyCode.Escape))
                {
                    statsMenuOpened = true; // First press: open stats menu
                }
                else if (statsMenuOpened && Input.GetKeyDown(KeyCode.Escape))
                {
                    hasOpenedStats = true;  // Second press: close it
                    GoToShop();
                }
                break;
            case 6:
                // Shop function handles
                break;
            case 7:
                if (!finalFightTriggered)
                {
                    GoToFinalFight();
                }
                break;
            case 8:

                break;
        }
    }

    void StartIntro()
    {
        tutorialStep = 1;
        rpgTalk.NewTalk("overseer_intro", "basic_attack");
    }

    public void OnDummyDestroyed()
    {
        if (tutorialStep == 2)
        {
            GoToDodge();
        }
    }

    void GoToBasicAttack()
    {
        tutorialStep = 2;
        rpgTalk.NewTalk("basic_attack", "defensive_mechanics");
    }

    void GoToDodge()
    {
        tutorialStep = 3;
        rpgTalk.NewTalk("defensive_mechanics", "abilities_intro");
    }

    void GoToAbilitiesIntro()
    {
        tutorialStep = 4;
        rpgTalk.NewTalk("abilities_intro", "stats_menu");
    }

    void GoToStats()
    {
        tutorialStep = 5;
        rpgTalk.NewTalk("stats_menu", "shop_intro");
    }

    void GoToShop()
    {
        tutorialStep = 6;
        rpgTalk.NewTalk("shop_intro", "final_duel");

        Invoke(nameof(OpenShopUI), 1.5f);
    }

    void OpenShopUI()
    {
        Debug.Log("Opening Shop UI from TutorialManager"); // ✅ Add this
        if (shopDisplay != null)
        {
            shopDisplay.ShowShop();
            waitingForShopClose = true;
        }
        else
        {
            Debug.LogWarning("ShopDisplay not assigned in TutorialManager!");
        }
    }

    void GoToFinalFight()
    {
        tutorialStep = 7;
        finalFightTriggered = true;
        
        rpgTalk.NewTalk("final_duel", "final_farewell");

        if (enemySpawner != null)
        {
            enemySpawner.SpawnWave(); // Spawns the enemies
        }
        else
        {
            Debug.LogWarning("No enemy spawner assigned to TutorialManager.");
        }
    }

    public void EndTutorial()
    {
         if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
        {
            DataPersistenceManager.instance.GameData.tutorialDone = true;
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("Tutorial completed and saved.");
        }
        rpgTalk.NewTalk("final_farewell", "end");
        SceneManager.LoadScene("Selection");
    }

    void OnDialogFinished()
    {
        switch (tutorialStep)
        {
            case 3:
                allowDodge = true;
                break;
            case 4:
                allowSkills = true;
                break;
            case 5:
                allowStats = true;
                break;
        }
    }
}
