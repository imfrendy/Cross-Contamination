// for stuff like: damage, death, health

// merged with PlayerBounds.cs and Health.cs

using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class PlayerAttributes : MonoBehaviour
{
	Vector3 origin;
	// step this distance from origin and you're dead, kiddo
	public float boundsRadius = 150f;

	public float startingHealth = 100f; // The amount of health the player starts the game with.
	private float currentHealth; // The current health the player has.
    public Slider healthSlider; // Reference to the UI's health bar.
	private bool invuln; // is the player immune to damage
    private bool healAbility;

	public Image Fill;
    public Image HealIndicator;
    public Image Jump;

    // won't work as public variables
    Color LowHealthColor;
	Color HighHealthColor;

	public float lowHealthPercent = 0.5f;

	public float timeToRegen = 3f;
	float timeSinceDamage;
	public float regenPerSecond = 2f;
    public GameObject gameOverText;
    private float timeSinceLastHeal; // time to next heal
    public float healCooldown = 10.0f;
    public Canvas HUD;

    // Use this for initialization
    void Start ()
    {
        // edit colors from here
        LowHealthColor = new Color(0.8f, 0.2f, 0);
		HighHealthColor = new Color (0, 0.5f, 0);

        currentHealth = startingHealth;
        healthSlider.wholeNumbers = false;

		origin = new Vector3 (0, 0, 0);

		timeSinceDamage = 0;
        timeSinceLastHeal = healCooldown;
        healAbility = false;
        HealIndicator.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentHealth > startingHealth) currentHealth = startingHealth;

        timeSinceLastHeal += Time.deltaTime;

        healthSlider.value = (currentHealth / startingHealth) * 100;

		if (currentHealth / startingHealth <= lowHealthPercent) {
			// set color
			Fill.color = Color.Lerp(LowHealthColor, HighHealthColor, (currentHealth / startingHealth) / lowHealthPercent);
		} else {
			Fill.color = HighHealthColor;
		}

		if (Vector3.Distance (transform.position, origin) >= boundsRadius) {
			// implement death timer

			print ("You are out of bounds at point " + transform.position);
		}

		if (invuln)
		{
			// add some graphical or UI effect
		}


		// regen after set time amount
		if (timeSinceDamage >= timeToRegen) {
			Regen (regenPerSecond * Time.deltaTime);
		}

		timeSinceDamage += Time.deltaTime;

        //Heal Ability
        Heal();
    }

    void Damage (float damage)
	{
		if (!invuln) {
			currentHealth -= damage;
			if (currentHealth <= 0) {
				currentHealth = 0;
				Die ();
			}

			timeSinceDamage = 0;
		}
	}

    void Heal()
    {
        if (healAbility && currentHealth < startingHealth && timeSinceLastHeal >= healCooldown && Input.GetAxis("Heal") == 1)
        {
            if (currentHealth+startingHealth/5 < startingHealth)
            {
                currentHealth += (startingHealth / 5);
            }
            else
            {
                currentHealth = startingHealth;
            }
            timeSinceLastHeal = 0;
        }
        HUD.SendMessage("FillHeal", timeSinceLastHeal / healCooldown);
    }

	void Regen (float health)
	{
		currentHealth += health;
		if (currentHealth >= startingHealth) {
			currentHealth = startingHealth;
		}
	}

	void Die () {
        Time.timeScale = 0;
        gameOverText.SetActive(true);
        // game over
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		// hitting the cube--in the future this should be changed to enemy's projectiles or something
		if (hit.gameObject.tag == "Enemy") {
			Rigidbody body = hit.collider.attachedRigidbody;
			//body.gameObject.SendMessage ("CubeExplosion", gameObject);

			// arbitrary damage
			//Damage (5f);
		}
	}

	void Invuln()
	{
		invuln = true;
	}

	void EndInvuln()
	{
		invuln = false;
	}

    void HealUnlock()
    {
        healAbility = true;
        HealIndicator.enabled = true;
        Jump.SendMessage("AsideJump");
    }
}
