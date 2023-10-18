using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomItem : MonoBehaviour
{

    [SerializeField] private List<GameObject> Items = new List<GameObject>();
    [SerializeField] private ParticleSystem DestroyParticles;

    IEnumerator Start()
    {
        int randomIndex = Random.Range(0, Items.Count);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(0.26f);
        Instantiate(DestroyParticles, transform.position, transform.rotation);
        Instantiate(Items[randomIndex], transform.position, Items[randomIndex].transform.rotation);

    }
}
