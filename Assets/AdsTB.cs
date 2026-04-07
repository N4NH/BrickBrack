using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class AdsTB : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    public void ShowText(String str)
    {
        text.text = str;
        gameObject.SetActive(true);
        if (animator != null)
        {
            animator.SetTrigger("Start");
        }
    }
}
