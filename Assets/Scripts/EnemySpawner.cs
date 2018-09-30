// not using this as a guide anymore: http://riskofrain.wikia.com/wiki/Enemies_%26_Bosses#The_Director

// in addition to spawns, used to handle terrain switching

using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject CubeEnemy;
	public GameObject Donut;
    public GameObject GummyBear;
	public GameObject Bread;
	public GameObject Oreo;
	public GameObject Burger;

	public GameObject player;		// point to reference when spawning baddies
	public int numberOfEnemies = 2;

	private float elapsedTime;


	public float boundsRadius = 150.0f; // radius of area player can go to
	public float maxDist = 100.0f; // max distance of spawn
	public float minDist = 20.0f; // minimum distance to player;

    public string[] enemies = {"Donut", "Bread", "GummyBear"};
	public float[] costs = {}; // set costs in editor; length must match enemies
    int currentEnemy;

	Vector3 origin;

    public float points = 500f;
    float pointRate;

	public float spawnTime = 20f;
	float spawnTimer;

	public GameObject[] terrains;

    void Awake()
    {
        // choose a terrain at random
        int i = Random.Range(0, terrains.Length);
        terrains[i].SetActive(true);
        for (int j = 0; j < terrains.Length; j++)
        {
            if (j != i)
            {
                terrains[j].SetActive(false);
            }
        }
    }

	void Start (){
		origin = new Vector3 (0, 0, 0);
		elapsedTime = 0;

		// increase over time?
        pointRate = .3f;

		currentEnemy = Random.Range (0, enemies.Length);

		// spawn enemies
		while (costs [currentEnemy] <= points) {

			// later development: modify to include elite clones
			if (costs [currentEnemy] <= points) {
				points -= costs [currentEnemy];
				Spawn (enemies [currentEnemy]);
				currentEnemy = Random.Range (0, enemies.Length); // indicate needs new enemy
			}
		}
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;

        points += Time.deltaTime * pointRate;

		if (spawnTimer >= spawnTime) {
			if (currentEnemy == -1) {
				currentEnemy = Random.Range (0, enemies.Length);
			}

			// later development: modify to include elite clones
			if (costs [currentEnemy] <= points) {
				print ("I spawn");
				points -= costs [currentEnemy];
				Spawn (enemies [currentEnemy]);
				currentEnemy = -1; // indicate needs new enemy

				spawnTimer = 0;
			}
		}

		spawnTimer += Time.deltaTime;
	}


	void Spawn (string EnemyName){
           // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        if (EnemyName == "Donut")
        {
            // spawn up to a half dozen
			for (int i = 0; i < Random.Range(1,7); i++)
            {
				Quaternion spawnRotation = InitSpawnRotation ();
				Vector3 spawnPosition = InitSpawnPosition ();
                Donut.SetActive(true);
                Instantiate(Donut, spawnPosition, spawnRotation);
                Donut.SetActive(false);
            }
        }
        else if (EnemyName == "GummyBear")
        {
			Quaternion spawnRotation = InitSpawnRotation ();
			Vector3 spawnPosition = InitSpawnPosition ();

            GummyBear.SetActive(true);
			Instantiate(GummyBear, spawnPosition, spawnRotation);
            GummyBear.SetActive(false);
        }

		else if (EnemyName == "Bread")
		{
            // spawn 2-4 loaves at a time
            for (int i = 0; i < Random.Range(2,5); i++)
            {
				Quaternion spawnRotation = InitSpawnRotation ();
				Vector3 spawnPosition = InitSpawnPosition ();

                Bread.SetActive(true);
                Instantiate(Bread, spawnPosition, spawnRotation);
                Bread.SetActive(false);
            }
		}

		else if (EnemyName == "Oreo")
		{
			for (int i = 0; i < Random.Range (2, 10); i++) {
				Quaternion spawnRotation = InitSpawnRotation ();
				Vector3 spawnPosition = InitSpawnPosition ();

				Oreo.SetActive (true);
				Instantiate (Oreo, spawnPosition, spawnRotation);
				Oreo.SetActive (false);
			}
		}
	}

	Vector3 GetSpawnPos (float min, float max)
	{
		float angle = Random.Range(0.0f, Mathf.PI*2);
		float spawnDist = Random.Range (min, max);
		return new Vector3(spawnDist * Mathf.Sin(angle), 0 , spawnDist * Mathf.Cos(angle));
	}

	Quaternion InitSpawnRotation()
	{
		return Quaternion.Euler (0.0f, Random.Range (0, 180), 0.0f);
	}


	Vector3 InitSpawnPosition()
	{
		Vector3 spawnPosition = GetSpawnPos (minDist, boundsRadius);

		// spawn with y level with the height map
		spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition) + .5f; // spawn everything .5 meter off the ground

		return spawnPosition;
	}

	// unused, for now
	Vector3 InitSpawnPositionNearPlayer()
	{
		Vector3 spawnPosition = new Vector3 (0, 0, boundsRadius + 1f); // quick hack to make it out of bounds
		int attempts = 0;

		// give up after 5 tries -- quick fix for hanging issues
		while ((Vector3.Distance (spawnPosition, origin) > boundsRadius) && attempts < 5) {
			Vector3 relativePos = GetSpawnPos (minDist, maxDist);
			spawnPosition = relativePos + player.transform.position;
			attempts++;
		}


		// spawn with y level with the height map
		spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition) + .5f; // spawn everything .5 meter off the ground

		return spawnPosition;
	}

	void EndSpawn ()
	{
		pointRate = 0;

		// spawn the boss
		print ("boss spawn");
		Quaternion spawnRotation = InitSpawnRotation ();
		Vector3 spawnPosition = InitSpawnPositionNearPlayer ();

		Burger.SetActive(true);
		Instantiate(Burger, spawnPosition, spawnRotation);
		Burger.SetActive(false);
	}
}
