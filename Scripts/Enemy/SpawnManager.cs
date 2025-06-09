using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject objtype1;

	public Transform spawnPos;


    public int maxSpawns,curSpawns;
	public float spawnRate;

	public float xDisplacement;

	void Start () 
	{

		if (objtype1 != null) {
			InvokeRepeating("SpawnObj", spawnRate, spawnRate);
		}
			
	}

	void SpawnObj()
	{
		if (objtype1 != null) {


			if(curSpawns < maxSpawns)
			{
				Vector3 spawnPosition = new Vector3(spawnPos.position.x-1+curSpawns*xDisplacement, spawnPos.position.y, spawnPos.position.z);

				Quaternion spawnRotation = Quaternion.Euler(0f, 0f, -40f) * Quaternion.LookRotation(-Vector3.right);

				GameObject.Instantiate(objtype1, spawnPosition, spawnRotation);
				curSpawns++;
			}
			else
			{

			}

		}
	}


}