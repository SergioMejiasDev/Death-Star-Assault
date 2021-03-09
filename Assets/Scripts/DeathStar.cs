using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used by the Death Star in Alliance mode.
/// </summary>
public class DeathStar : MonoBehaviour
{
    [SerializeField] Transform generationPoint = null;
    [SerializeField] int maxEnemies = 100;
    [SerializeField] float timeBetweenEnemies = 2;
    [SerializeField] GameObject panelVictory = null;
    [SerializeField] Timer timer = null;
    [SerializeField] GameObject player = null;

    [SerializeField] int maxHealth = 200;
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

        StartCoroutine(Spawner());
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = (healthString + "100 %");
    }

    /// <summary>
    /// Function called to generate a new enemy.
    /// </summary>
    public void GenerateEnemy()
    {
        Vector3 offset = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0);
        GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");

        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().health = enemy.GetComponent<Enemy>().maxHealth;
            enemy.GetComponent<Enemy>().dead = false;
            enemy.GetComponent<Enemy>().sliderHealth.value = enemy.GetComponent<Enemy>().health;

            enemy.transform.position = generationPoint.position + offset;
            enemy.transform.rotation = generationPoint.rotation;
        }

        GameManager.manager.enemyNumber++;
        maxEnemies--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletPlayer") && (GameManager.manager.enemyNumber <= 0 && maxEnemies <= 0))
        {
            health--;
            sliderHealth.value = health;
            textHealth.text = (healthString + (health * 100 / maxHealth) + " %");

            other.gameObject.SetActive(false);
            
            if (health <= 0)
            {
                textHealth.text = (healthString + "0 %");
                Instantiate(deadParticles, transform.position, transform.rotation);
                panelVictory.SetActive(true);
                timer.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Coroutine that is in charge of calling the enemy creation function after a while.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (true)
        {
            if (maxEnemies > 0 && GameManager.manager.enemyNumber <= 30)
            {
                if (player != null)
                {
                    GenerateEnemy();
                }
            }
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
}
