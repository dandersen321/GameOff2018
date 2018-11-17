using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public delegate void OnHealthChangeAction(int currentHealth, int maxHealth);
    public event OnHealthChangeAction OnHealthChange;

    public delegate void OnDeathAction();
    public event OnDeathAction onDeathAction;


    [SerializeField]
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }
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

    public void TakeDamage(int amount)
    {
        if (isAlive)
        {
            currentHealth -= amount;

            if (OnHealthChange != null)
            {
                OnHealthChange(currentHealth, maxHealth);
            }

            //Debug.Log(gameObject.name + " took " + amount + " damage to ");

            if (currentHealth <= 0)
            {
                isAlive = false;
                if(onDeathAction != null)
                {
                    onDeathAction();
                }
            }
        }
    }

    private void Die()
    {
        if (this.gameObject.name == "Beacon")
        {
            Debug.Log("Game Lost!!!");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
