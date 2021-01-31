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
    [SerializeField] Toggle toggleMute = null;

    private void Awake()
    {
        manager = this;
        SetInitialScore();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (AudioListener.volume == 0)
            {
                toggleMute.SetIsOnWithoutNotify(true);
            }
        }
        Time.timeScale = 1;
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
        SceneManager.LoadScene(buildIndex);
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
    /// Call to manage the options menu toggle.
    /// </summary>
    public void MuteAllSounds ()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
        }
        else if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
        }
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
}
