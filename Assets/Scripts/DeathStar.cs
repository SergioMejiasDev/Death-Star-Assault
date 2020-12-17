﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used by the Death Star in Alliance mode.
/// </summary>
public class DeathStar : MonoBehaviour
{
    [SerializeField] Transform generationPoint = null;
    [SerializeField] int maxEnemies = 100;
    [SerializeField] float timeBetweenEnemies = 2;
    [SerializeField] GameObject panelVictory = null;

    [SerializeField] int maxHealth = 200;
    int health;
    [SerializeField] Slider sliderHealth = null;
    [SerializeField] Text textHealth = null;
    [SerializeField] GameObject deadParticles = null;

    private void Start()
    {
        StartCoroutine(Spawner());
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = ("Death Star: " + (health * 100 / maxHealth) + " %");
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
            textHealth.text = ("Death Star: " + (health * 100 / maxHealth) + " %");

            Destroy(other.gameObject);
            
            if (health <= 0)
            {
                textHealth.text = ("Death Star: 0 %");
                Instantiate(deadParticles, transform.position, transform.rotation);
                panelVictory.SetActive(true);
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
                GenerateEnemy();
            }
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
}