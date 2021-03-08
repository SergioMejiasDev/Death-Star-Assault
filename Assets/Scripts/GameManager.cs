using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the main functions of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    int score;
    int scoreEmpire;
    [SerializeField] Text scoreText = null;
    [SerializeField] Text scoreEmpireText = null;
    public int enemyNumber;
    [SerializeField] GameObject[] panels = null;

    [SerializeField] GameObject loadingPanel = null;
    [SerializeField] Image fadeImage = null;
    [SerializeField] Text loadingText = null;

    private void Awake()
    {
        manager = this;
        SetInitialScore();
        Time.timeScale = 1;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (PlayerPrefs.GetInt("FirstTime") == 0)
            {
                ManagePanels(panels[3]);
            }
        }
    }

    /// <summary>
    /// Function to establish the initial values of the score at the beginning of the game.
    /// </summary>
    void SetInitialScore()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            score = 100;
            scoreText.text = score.ToString();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            scoreEmpire = 0;
            scoreEmpireText.text = scoreEmpire.ToString();
        }
    }

    /// <summary>
    /// Function we call to update our score after eliminating an enemy.
    /// </summary>
    /// <param name="num">How much the score increases.</param>
    /// <param name="empireMode">True if we are playing in Empire mode, false if it is Alliance mode.</param>
    public void UpdateScore(int num, bool empireMode)
    {
        if (!empireMode)
        {
            score -= num;
            scoreText.text = score.ToString();
        }
        else
        {
            scoreEmpire += num;
            scoreEmpireText.text = scoreEmpire.ToString();
        }
    }

    /// <summary>
    /// Function called to load a new scene.
    /// </summary>
    /// <param name="buildIndex">Number of the scene we want to load.</param>
    public void LoadScene(int buildIndex)
    {
        StartCoroutine(Loading(buildIndex));
    }

    /// <summary>
    /// Call to close the game.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Function we call to open and close the panels in the menu.
    /// </summary>
    /// <param name="panel">Panel that we want to open.</param>
    public void ManagePanels(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panel.SetActive(true);
    }

    /// <summary>
    /// Coroutine started every time we change scene.
    /// </summary>
    /// <param name="sceneNumber">Scene we want to load.</param>
    /// <returns></returns>
    IEnumerator Loading(int sceneNumber)
    {
        Time.timeScale = 1;
        loadingPanel.SetActive(true);

        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a < 1)
        {
            alphaValue = imageColor.a + (2 * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        loadingText.enabled = true;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneNumber);
    }
}
