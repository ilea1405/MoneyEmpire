using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNPC : MonoBehaviour
{
    public GameObject npc;
    public int npcCount;
    public float minDistance; // Minimum distance between NPCs
    private List<Vector3> characters = new List<Vector3>(); // Use lowercase for variable names
    
    void Start() 
    {
        StartCoroutine(NPCDrop());
    }

    IEnumerator NPCDrop()
    {
        int i = 0;
        while (i < npcCount)
        {
            Vector3 newPos = GenerateRandomPosition();
            // If newPos is zero, there are no available positions left, exit the loop
            if (newPos == Vector3.zero)
                break;
            
            GameObject newNPC = Instantiate(npc, newPos, Quaternion.Euler(0,Random.Range(1,360),0));
            characters.Add(newPos);
            yield return new WaitForSeconds(0.1f);
            i++;
            Debug.Log(i);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        float xPos;
        float zPos;
        Vector3 newPos;
        bool isValid = false;
        int attempts = 0; // Track the number of attempts

        do
        {
            xPos = Random.Range(1, 390);
            zPos = Random.Range(175, 200);
            newPos = new Vector3(xPos, 0.04f, zPos);
            isValid = true;

            foreach (Vector3 existingPos in characters)
            {
                if (Vector3.Distance(newPos, existingPos) < minDistance)
                {
                    isValid = false;
                    break;
                }
            }

            // If we've tried too many times, return Vector3.zero to indicate no available positions
            attempts++;
            if (attempts > 100)
                return Vector3.zero;
            
        } while (!isValid);

        Debug.Log(newPos);

        return newPos;
    }
}
