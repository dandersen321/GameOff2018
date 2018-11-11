using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    /// <summary>
    /// Event that can be registered against and is called anytime health is changed
    /// </summary>
    public delegate void OnHealthChangeAction(int currentHealth, int maxHealth);
    public event OnHealthChangeAction OnHealthChange;

    /// <summary>
    /// The maximum health of the entity
    /// </summary>
    [SerializeField]
    private int maxHealth;

    /// <summary>
    /// Convinence property that allows for pulblic get but private set
    /// </summary>
    public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

    /// <summary>
    /// The current health of the entity
    /// </summary>
    public int currentHealth { get; private set; }

    private bool isAlive = true;

    void Start()
    {
        currentHealth = MaxHealth;

        // Make sure everything is displayed correctly at the start
        if (OnHealthChange != null)
        {
            OnHealthChange(currentHealth, maxHealth);
        }

    }

    /// <summary>
    /// Used to deal damage to the entity
    /// </summary>
    /// <param name="damageType">Type of damage to deal</param>
    /// <param name="amount">Amount of damage to deal</param>
    public void TakeDamage(int amount)
    {
        if (isAlive)
        {
            currentHealth -= amount;

            if (OnHealthChange != null)
            {
                OnHealthChange(currentHealth, maxHealth);
            }

            //Debug.Log(gameObject.name + " took " + damageToDeal + " " + damageType + " damage to ");

            if (currentHealth == 0)
            {
                isAlive = false;
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
