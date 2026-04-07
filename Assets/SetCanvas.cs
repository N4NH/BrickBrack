using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvas : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.currentCanvas = this.transform;
    }
}
