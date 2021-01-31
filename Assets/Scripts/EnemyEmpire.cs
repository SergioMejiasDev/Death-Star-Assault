using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the main functions of the enemies in Empire mode.
/// </summary>
public class EnemyEmpire : MonoBehaviour
{
    [Header("Movement")]
    Vector3 destiny;
    float speed = 150.0f;
    Transform player;
    Transform deathStar;

    [Header("Shoot")]
    [SerializeField] Transform shootPoint = null;
    [SerializeField] GameObject bullet = null;
    float timeLastShoot;
    float cadency = 0.25f;

    [Header("Health")]
    public int maxHealth = 2;
    public int health;
    public Slider sliderHealth = null;
    public bool dead;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        deathStar = GameObject.FindGameObjectWithTag("DeathStar").transform;
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
            else if ((deathStar != null) && (Vector3.Distance(transform.position, deathStar.position) < 500))
            {
                transform.LookAt(deathStar);
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

    private void OnTriggerEnter(Collider x)
    {
        if (x.gameObject.CompareTag("BulletPlayer"))
        {
            health--;
            sliderHealth.value = health;

            Destroy(x.gameObject);
            if (health <= 0 && !dead)
            {
                dead = true;
                GameManager.manager.UpdateScore(1, true);
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
    /// Function that allows the enemy to shoot.
    /// </summary>
    void Shoot()
    {
        timeLastShoot = Time.time;
        Destroy(Instantiate(bullet, shootPoint.position, shootPoint.rotation), 2);
    }

    /// <summary>
    /// Function that makes the enemy look for a new destination.
    /// </summary>
    void ChooseDestiny()
    {
        if (deathStar != null)
        {
            destiny = new Vector3(Random.Range(deathStar.position.x - 500, deathStar.position.x + 500), Random.Range(deathStar.position.y - 500, deathStar.position.y + 500), Random.Range(deathStar.position.z - 500, deathStar.position.z + 500));
        }
    }

}

