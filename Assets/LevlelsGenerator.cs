using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevlelsGenerator : MonoBehaviour
{
    // The prefab to spawn
    public GameObject Levelprefab;

    // The parent object under which the prefabs will be spawned
    public Transform parent;

    // Number of prefabs to spawn
    public int numberOfPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPrefabs();
    }

    // Method to spawn prefabs
    void SpawnPrefabs()
    {
        if (Levelprefab == null || parent == null)
        {
            Debug.LogError("Prefab or parent is not assigned.");
            return;
        }

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Instantiate the prefab
            GameObject newPrefab = Instantiate(Levelprefab, parent);

            // Optionally, you can set the position and other properties of the spawned prefab here
            // Example: newPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 100, 0);

            // Set the name of the new prefab instance
            //newPrefab.name = prefab.name + "_" + i;
        }
    }
}
