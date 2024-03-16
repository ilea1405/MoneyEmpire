using UnityEngine;
using System.Collections.Generic;

public class NPCGenerator : MonoBehaviour
{
    public int numberOfNPCs;
    public List<GameObject> npcPrefabs = new List<GameObject>();
    public List<GameObject> sidewalkObjects = new List<GameObject>();

    private bool generationComplete = false;

    private void Start()
    {
        GenerateNPCs();
    }

    private void Update()
    {
        if (generationComplete)
        {
            enabled = false; // Disable the generator script
        }
    }

    private void GenerateNPCs()
    {
        if (npcPrefabs.Count == 0 || sidewalkObjects.Count == 0)
        {
            Debug.LogError("NPC prefabs or sidewalk objects are not assigned!");
            return;
        }

        for (int i = 0; i < numberOfNPCs; i++)
        {
            GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
            GameObject sidewalkObject = sidewalkObjects[Random.Range(0, sidewalkObjects.Count)];
            Vector3 spawnPosition = GetRandomSidewalkPosition(sidewalkObject);

            if (npcPrefab != null && sidewalkObject != null)
            {
                GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
                // You can add further customization to NPCs here if needed

                // Check if all NPCs are instantiated
                if (i == numberOfNPCs - 1)
                {
                    generationComplete = true;
                }
            }
        }
    }

    private Vector3 GetRandomSidewalkPosition(GameObject sidewalkObject)
    {
        // Modify this function to suit your game's sidewalk positioning logic
        Bounds sidewalkBounds = sidewalkObject.GetComponent<Collider>().bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(sidewalkBounds.min.x, sidewalkBounds.max.x),
            sidewalkObject.transform.position.y,
            Random.Range(sidewalkBounds.min.z, sidewalkBounds.max.z)
        );
        return randomPosition;
    }
}
