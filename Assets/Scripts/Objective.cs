using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
    public float boundsRadius = 150f;

    public float maxHealth = 1000f;
    float health;

    float animTimer;
    public float animLength = 4f;

	public GameObject spawner;

	void Start () {		


        transform.position = InitSpawnPosition();
        animTimer = 0;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (animTimer >= animLength && health > 0)
        {
            BroadcastMessage("Floating");
            animTimer -= animLength;
        }
        animTimer += Time.deltaTime;
	}

	void AddPineappleExplosion(Vector3 sourcePoint)
	{
		float damage = Mathf.SmoothStep(0, 100, (20.0f - Vector3.Distance(sourcePoint, transform.position)) / 20.0f);
		ApplyDamage(damage);
	}

    void ApplyDamage(float damage)
    {
		if (health > 0) {
			this.health -= damage;

			if (health <= 0) {
				health = 0;
				BroadcastMessage("PleaseDie");
				spawner.SendMessage ("EndSpawn");
			}
			BroadcastMessage ("Indicate", health / maxHealth);
		}
    }

	void AddEggExplosion(Vector3 sourcePoint)
	{
		float damage = Mathf.SmoothStep(0, 100, (10.0f - Vector3.Distance(sourcePoint, transform.position)) / 10.0f);
		ApplyDamage(damage);
	}

	void Push (Vector3 forceVector)
	{
	}

    // spawwn anywhere in bounds
    Vector3 GetSpawnPos(float bounds)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2);

		//spawn far away
		float spawnDist = Random.Range(bounds * (2/3), bounds);
        return new Vector3(spawnDist * Mathf.Sin(angle), 0, spawnDist * Mathf.Cos(angle));
    }
    Vector3 InitSpawnPosition()
    {
        Vector3 spawnPosition = GetSpawnPos(boundsRadius);


        //TODO: spawn with y level with the height map
        spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);

        return spawnPosition;
    }
}
