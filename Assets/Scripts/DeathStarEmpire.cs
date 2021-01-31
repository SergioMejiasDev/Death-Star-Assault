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

    private void Start()
    {
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = ("Death Star: " + (health * 100 / maxHealth) + " %");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy") || other.gameObject.CompareTag("BulletPlayer"))
        {
            health--;
            sliderHealth.value = health;
            textHealth.text = ("Death Star: " + (health * 100 / maxHealth) + " %");

            Destroy(other.gameObject);

            if (health <= 0)
            {
                textHealth.text = ("Death Star: 0 %");
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
