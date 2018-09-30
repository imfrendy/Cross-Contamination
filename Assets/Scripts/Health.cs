// DONT USE THIS!!! PLEASE!!!!!
// IT HAS BEEN REPLACED BY PlayerAttributes.cs
//


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
	public float startingHealth = 100f;                            // The amount of health the player starts the game with.
	public float currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.

	public Image Fill;
	public Color LowHealthColor;
	public Color HighHealthColor;

	public float lowHealthPercent = 0.25f;

    // Use this for initialization
    void Start ()
    {
        currentHealth = startingHealth;
        healthSlider.wholeNumbers = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthSlider.value = currentHealth;
        if (Input.GetKeyDown("space"))
        {
            currentHealth -= 1;
        }

		if (currentHealth / startingHealth <= 0.25) {
			// set color
			Fill.color = Color.red;
		}
    }
}
