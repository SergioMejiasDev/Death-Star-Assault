using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that takes care of the main enemy functions in Alliance mode.
/// </summary>
public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    Vector3 destiny;
    [SerializeField] float speed = 150.0f;
    
    [Header("Shoot")]
    Transform player;
    [SerializeField] Transform shootPoint = null;
    float timeLastShoot;
    float cadency = 0.25f;

    [Header("Health")]
    public int maxHealth = 2;
    public int health;
    public Slider sliderHealth;
    public bool dead;
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        dead = false;
        ChooseDestiny();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if ((player != null) && (Vector3.Distance(transform.position, player.position) < 300))
            {
                transform.LookAt(player);
                shootPoint.LookAt(player);
                if (Time.time - timeLastShoot > cadency)
                {
                    Shoot();
                }
            }
            else
            {
                transform.LookAt(destiny);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                
                if (Vector3.Distance(destiny, transform.position) < 2)
                {
                    ChooseDestiny();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            health--;
            sliderHealth.value = health;

            collision.gameObject.SetActive(false);
            
            if (health <= 0 && !dead)
            {
                dead = true;
                GameManager.manager.UpdateScore(1, false);
                GameManager.manager.enemyNumber--;

                GameObject deadParticles = ObjectPooler.SharedInstance.GetPooledObject("Particles");
                if (deadParticles != null)
                {
                    deadParticles.SetActive(true);
                    deadParticles.transform.position = transform.position;
                    deadParticles.transform.rotation = transform.rotation;
                }

                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Function that is called for the enemy to shoot.
    /// </summary>
    void Shoot()
    {
        timeLastShoot = Time.time;

        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
    }

    /// <summary>
    /// Function that is responsible for assigning a new destination to the enemy.
    /// </summary>
    void ChooseDestiny()
    {
        if (player != null)
        {
            destiny = new Vector3(Random.Range(player.position.x - 200, player.position.x + 200), Random.Range(player.position.y - 200, player.position.y + 200), Random.Range(player.position.z - 200, player.position.z + 200));
        }
        else
        {
            destiny = new Vector3(Random.Range(transform.position.x - 200, transform.position.x + 200), Random.Range(transform.position.y - 200, transform.position.y + 200), Random.Range(transform.position.z - 200, transform.position.z + 200));
        }
    }
}

