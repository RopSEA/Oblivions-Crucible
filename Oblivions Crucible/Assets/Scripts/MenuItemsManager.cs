using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuItemManager : MonoBehaviour
{
    public TitleScreenManager titleScreenManager; // Reference to Title Manager
    public List<GameObject> menuItems = new List<GameObject>(); // List of menu items

    private List<CanvasGroup> menuCanvasGroups = new List<CanvasGroup>();

    void Start()
    {
        // Initialize menu items
        foreach (GameObject menuItem in menuItems)
        {
            if (menuItem != null)
            {
                CanvasGroup cg = menuItem.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = menuItem.AddComponent<CanvasGroup>();

                cg.alpha = 0f; // Start hidden
                menuItem.transform.localScale = Vector3.zero; // Start at zero scale
                menuItem.SetActive(false);

                menuCanvasGroups.Add(cg);
            }
        }


        StartCoroutine(WaitForTitleScreen());
    }

    IEnumerator WaitForTitleScreen()
    {

        if (titleScreenManager == null)
        {
            Debug.LogError(" TitleScreenManager reference is missing in MenuItemManager! Assign it in the Inspector.");
            yield break;
        }

        while (!titleScreenManager.isTitleFinished)
        {
            yield return null; // Check again on the next frame
        }


        StartCoroutine(ShowMenuItems());
    }

    IEnumerator ShowMenuItems()
    {
        float delayBetweenItems = 0.2f; // Delay between menu items appearing

        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i] != null)
            {
                menuItems[i].SetActive(true); // Make item visible
                StartCoroutine(FadeAndScaleIn(menuItems[i], menuCanvasGroups[i]));
            }
            yield return new WaitForSeconds(delayBetweenItems); // Small delay between each item
        }
    }

    IEnumerator FadeAndScaleIn(GameObject obj, CanvasGroup canvasGroup)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(0.1846536f, 0.1846536f, 0.1846536f); // Final scale

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        obj.transform.localScale = endScale;
    }
}
