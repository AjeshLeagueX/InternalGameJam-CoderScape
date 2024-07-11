using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    public GameObject gameStartUI, gamePlayUI, finishGameUI;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        finishGameUI.SetActive(false);
        StartCoroutine(CloseGameStartUI());
    }
    public IEnumerator CloseGameStartUI()
    {
        yield return new WaitForSeconds(5f);
        gameStartUI.SetActive(false);
        gamePlayUI.SetActive(true);
    }
    public void FinishGameUI()
    {
        gamePlayUI.SetActive(false);
        finishGameUI.SetActive(true);
    }

    public TMP_Text enemySleepSnippetCount, entitiesSlowDownSnippet;
    public void UpdateSnippetUI(int enemySleepCount, int entitiesSlowCount)
    {
        enemySleepSnippetCount.text = "Enemies.Sleep() : " + enemySleepCount.ToString();
        entitiesSlowDownSnippet.text = "Entities.SlowDown() : " + entitiesSlowCount.ToString();
    }

    public TMP_Text statText; // Assign the TextMesh Pro UI component in the Inspector
    public Transform statsParent;
    public void ShowStatChange(string value, bool isPositive)
    {
        Color color = isPositive ? Color.green : Color.red;
        var _statsTxt = Instantiate(statText, statsParent);
        _statsTxt.gameObject.SetActive(true);
        _statsTxt.text = value;
        _statsTxt.color = color;
        _statsTxt.alpha = 1f; // Ensure text is fully visible

        StartCoroutine(FadeAfterWait(_statsTxt, color));
    }

    private IEnumerator FadeAfterWait(TMP_Text text, Color color)
    {
        yield return new WaitForSeconds(3f);

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.alpha = alpha;
            yield return null;
        }

        Destroy(text.gameObject);
    }
}