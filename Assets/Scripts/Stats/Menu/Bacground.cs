using System.Collections;
using UnityEngine;

public class Bacground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        StartCoroutine(DisableAfterDelay(1f));
    }
    private IEnumerator DisableAfterDelay(float delay)
    {
        // Ждем 3 секунды
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
