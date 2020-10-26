using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjects : MonoBehaviour
{
    void Update()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(Disable());
        }       
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
