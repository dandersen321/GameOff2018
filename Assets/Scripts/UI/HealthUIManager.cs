using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour {

    public Image healthBar;
    public Text textHealth;

    // Use this for initialization
    void Start () {
        References.getBeacon().gameObject.GetComponent<Health>().OnHealthChange += UpdateHealthBar;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        textHealth.text = currentHealth + "/" + maxHealth;
    }
}
