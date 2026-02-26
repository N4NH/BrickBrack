using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.Instance.ShowAds();
    }
    void OnDisable()
    {
        GameManager.Instance.resetAds();
    }
}
