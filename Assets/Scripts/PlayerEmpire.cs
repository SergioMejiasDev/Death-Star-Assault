using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that controls the main functions of the player in Empire mode.
/// </summary>
public class PlayerEmpire : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    float speed;
    float speedRot = 1;

    [Header("Shoot")]
    [SerializeField] GameObject bullet = null;
    [SerializeField] Transform[] shootPoint = null;
    int cannonNumber = 0;
    bool shootAll;
    float timeLastShoot;
    float cadency = 0.25f;
    
    [Header("Panels")]
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;

    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    int health;
    [SerializeField] Slider sliderHealth = null;
    [SerializeField] Text textHealth = null;
    #endregion

    private void Start()
    {
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = ("Player Ship: " + (health * 100 / maxHealth) + " %");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Vertical"));
            transform.Translate(Vector3.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"));
            transform.Translate(Vector3.up * speed * Time.deltaTime * Input.GetAxis("UpDown"));

            transform.Rotate(Vector3.up * speedRot * Input.GetAxis("Mouse X"));
            transform.Rotate(Vector3.left * speedRot * Input.GetAxis("Mouse Y"));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

            if (Input.GetKey(KeyCode.LeftShift))
                speed = 250.0f;
            else
                speed = 150.0f;

            if (Input.GetButton("Fire1") && Time.time - timeLastShoot > cadency)
                Shoot();
            if (Input.GetButtonDown("Fire2"))
                shootAll = !shootAll;
        }
       
        if (Input.GetButtonDown("Cancel"))
        {
            panelPause.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy"))
        {
            health--;
            sliderHealth.value = health;
            textHealth.text = ("Player Ship: " + (health * 100 / maxHealth) + " %");

            Destroy(other.gameObject);
            if (health <= 0)
            {
                Death();
            }
        }
        
        if (other.gameObject.CompareTag("DeathStar"))
        {
            Death();
        }
    }

    /// <summary>
    /// Function in charge of the player being able to shoot.
    /// </summary>
    void Shoot()
    {
        timeLastShoot = Time.time;
        if (shootAll)
        {
            for (int i = 0; i < shootPoint.Length; i++)
            {
                Destroy(Instantiate(bullet, shootPoint[i].position, shootPoint[i].rotation), 2);
            }
        }
        else
        {
            Destroy(Instantiate(bullet, shootPoint[cannonNumber].position, shootPoint[cannonNumber].rotation), 2);
            cannonNumber++;
            if (cannonNumber > 1)
            {
                cannonNumber = 0;
            }
        }
    }

    /// <summary>
    /// Function in charge of resuming the game after a pause.
    /// </summary>
    public void ResumeGame()
    {
        panelPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Function that is called when the player dies.
    /// </summary>
    public void Death()
    {
        panelGameOver.SetActive(true);
        health = 0;
        sliderHealth.value = 0;
        textHealth.text = ("Player Ship: 0 %");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Camera.main.transform.SetParent(null);
        GameObject deadParticles = ObjectPooler.SharedInstance.GetPooledObject("Particles");
        if (deadParticles != null)
        {
            deadParticles.SetActive(true);
            deadParticles.transform.position = transform.position;
            deadParticles.transform.rotation = transform.rotation;
        }
        Destroy(gameObject);
    }
}
