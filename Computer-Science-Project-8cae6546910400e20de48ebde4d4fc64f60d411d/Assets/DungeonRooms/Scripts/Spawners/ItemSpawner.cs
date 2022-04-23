using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight; // Drop rate of an item
    }
    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;

    private void Awake()
    {
        totalWeight = 0;
        foreach (var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }
    }

    private void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativewieght = items[0].weight;

        while (pick > cumulativewieght && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativewieght += items[chosenIndex].weight;
        }

        GameObject i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
    }
}
