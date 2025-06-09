using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab objek yang akan di-spawn
    public Transform[] spawnPoints; // Lokasi-lokasi spawn
    public int maxObjects = 50; // Jumlah maksimal objek yang dapat di-spawn
    public float spawnInterval = 10f; // Waktu interval spawn dalam detik

    private int currentObjectCount = 0; // Jumlah objek yang ada di scene

    void Start()
    {
        // Memulai coroutine untuk spawn objek secara berkala
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (currentObjectCount < maxObjects)
            {
                // Spawn 2-3 objek secara acak
                int spawnCount = Random.Range(2, 4); 

                for (int i = 0; i < spawnCount; i++)
                {
                    if (currentObjectCount >= maxObjects)
                        break; // Berhenti jika sudah mencapai jumlah maksimal

                    // Pilih spawn point secara acak
                    int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                    Transform spawnPoint = spawnPoints[randomSpawnIndex];

                    // Spawn objek di lokasi yang dipilih
                    Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);

                    // Tambahkan jumlah objek yang ada
                    currentObjectCount++;
                }
            }

            // Tunggu selama 'spawnInterval' detik sebelum spawn lagi
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Fungsi ini bisa dipanggil untuk mengurangi jumlah objek jika ada yang dihancurkan atau dihapus
    public void RemoveObject()
    {
        if (currentObjectCount > 0)
            currentObjectCount--;
    }
}
