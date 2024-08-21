using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float health;
    public float maxHealth = 100f;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void ChangeHealthBy(float healthDifference)
    {
        health -= healthDifference;   
    }

    public float GetHealth()
    {
        return health; 
    }
}
