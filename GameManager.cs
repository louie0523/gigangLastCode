using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Title,
    Enemy,
    Boss,
    Store,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public State state;

    public float GameTime = 1f;
    public float ProjectileTime = 1f;

    public List<Item> Inven = new List<Item>();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }


    public List<Enemy> AllEnemy()
    {
        Enemy[] list = FindObjectsOfType<Enemy>();

        List<Enemy> enemies = new List<Enemy>();
        foreach(Enemy enemy in list)
        {
            if(enemy.isAlive)
                enemies.Add(enemy);
        }

        return enemies;
    }

    public List<Projectile> AllProjectile()
    {
        Projectile[] list = FindObjectsOfType<Projectile>();

        List<Projectile> enemies = new List<Projectile>();
        foreach (Projectile enemy in list)
        {
   
                enemies.Add(enemy);
        }

        return enemies;
    }
}
