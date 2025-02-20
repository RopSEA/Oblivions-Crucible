using UnityEngine;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject titlePart1; // First part of the title
    public GameObject titlePart2; // Second part of the title
    public bool isTitleFinished = false;
    private CanvasGroup canvasGroup1, canvasGroup2;

    void Start()
    {
        // Get or add CanvasGroup for both parts
        canvasGroup1 = titlePart1.GetComponent<CanvasGroup>();
        if (canvasGroup1 == null)
            canvasGroup1 = titlePart1.AddComponent<CanvasGroup>();

        canvasGroup2 = titlePart2.GetComponent<CanvasGroup>();
        if (canvasGroup2 == null)
            canvasGroup2 = titlePart2.AddComponent<CanvasGroup>();

        // Start hidden
        canvasGroup1.alpha = 0f;
        canvasGroup2.alpha = 0f;
        titlePart1.transform.localScale = Vector3.zero;
        titlePart2.transform.localScale = Vector3.zero;

        titlePart1.SetActive(false);
        titlePart2.SetActive(false);
    }

    public void ShowTitleScreen()
    {
        StartCoroutine(ShowTitleSequence());
    }

    IEnumerator ShowTitleSequence()
    {
        float delayBetweenParts = .2f; // Time between first and second title part

        // Show first title part
        titlePart1.SetActive(true);
        yield return StartCoroutine(FadeAndScaleIn(titlePart1, canvasGroup1));

        // Wait before showing the second part
        yield return new WaitForSeconds(delayBetweenParts);

        // Show second title part
        titlePart2.SetActive(true);
        yield return StartCoroutine(FadeAndScaleIn(titlePart2, canvasGroup2));
        isTitleFinished = true;
    }

    IEnumerator FadeAndScaleIn(GameObject obj, CanvasGroup canvasGroup)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(0.1846536f, 0.1846536f, 0.1846536f);

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
