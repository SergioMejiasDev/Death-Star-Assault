using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that is responsible for generating enemies in Empire mode.
/// </summary>
public class GeneratorEmpire : MonoBehaviour
{
    int maxEnemies = 1000000;
    [SerializeField] float timeBetweenEnemies = 4;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    /// <summary>
    /// Function that is responsible for generating enemies.
    /// </summary>
    public void GenerateEnemy()
    {
        Vector3 offset = new Vector3(Random.Range(-2000, 2000), 0, 0);
        GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");

        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyEmpire>().health = enemy.GetComponent<EnemyEmpire>().maxHealth;
            enemy.GetComponent<EnemyEmpire>().dead = false;
            enemy.GetComponent<EnemyEmpire>().sliderHealth.value = enemy.GetComponent<EnemyEmpire>().health;

            enemy.transform.position = transform.position + offset;
            enemy.transform.rotation = transform.rotation;
        }
        GameManager.manager.enemyNumber++;
        maxEnemies--;
    }

    /// <summary>
    /// Coroutine that calls the function to generate enemies after a while.
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
