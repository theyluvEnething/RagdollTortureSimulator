using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyAfterEffect : MonoBehaviour
{
    private IEnumerator Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        float particleSystemDuration = particleSystem.main.duration;
        yield return new WaitForSeconds(particleSystemDuration * 2f);
        Destroy(gameObject);
    }
}
