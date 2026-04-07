using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfKill : MonoBehaviour
{
    public void Kill()
    {
        Destroy(gameObject);
    }
}
