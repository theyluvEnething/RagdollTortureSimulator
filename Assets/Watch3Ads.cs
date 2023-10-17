using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch3Ads : MonoBehaviour
{
    private int count = 0;

    public void WatchAd()
    {
        DataManager.Instance.AdsManager.ShowAd();
        count++;

        if (count >= 3)
        {
            RandomWeaponUnlock();
            count = 0;
        }
    }

    private void RandomWeaponUnlock()
    {
        // TO BE IMPLEMENTED
    }
}
