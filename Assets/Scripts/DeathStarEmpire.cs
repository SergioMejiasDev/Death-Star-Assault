using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that takes care of the functions of the Death Star in Empire mode.
/// </summary>
public class DeathStarEmpire : MonoBehaviour
{
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] Timer timer = null;
    [SerializeField] int maxHealth = 3000;
    int health;
    [SerializeField] Slider sliderHealth = null;
    [SerializeField] Text textHealth = null;
    [SerializeField] GameObject deadParticles = null;
    [SerializeField] MultilanguageText healthStringObject = null;
    string healthString;

    private void Start()
    {
        switch (MultilanguageManager.multilanguageManager.activeLanguage)
        {
            case "EN":
                healthString = healthStringObject.english;
                break;
            case "ES":
                healthString = healthStringObject.spanish;
                break;
        }

        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = healthString + "100 %";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy") || other.gameObject.CompareTag("BulletPlayer"))
        {
            health--;
            sliderHealth.value = health;
            textHealth.text = (healthString + (health * 100 / maxHealth) + " %");

            Destroy(other.gameObject);

            if (health <= 0)
            {
                textHealth.text = (healthString + "0 %");
                Instantiate(deadParticles, transform.position, transform.rotation);
                panelGameOver.SetActive(true);
                timer.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(gameObject);
            }
        }
    }
}
