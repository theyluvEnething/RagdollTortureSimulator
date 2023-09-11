using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private ParticleSystem fog;

    [Header("ATTRIBUTES")]
    [SerializeField] private float WaitToReset = 0.5f;

    void Start()
    {
        fog.Stop();
    }

    public void resetLevel()
    {
        GameManager.Instance.keepItemState = true;
        StartCoroutine(ResetAnimation());
    }
    
    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(WaitToReset);
        fog.Simulate(1.35f, true, true);
        fog.Play();

        // Wait for 1 second
        yield return new WaitForSeconds(2.0f);

        fog.Stop();

        if (GameManager.Instance.GetDeathCount() % 5 == 0)
        {
            DataManager.Instance.AdsManager.ShowAd();
        }

        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
