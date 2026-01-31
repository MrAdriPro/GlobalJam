using System.Collections;
using UnityEngine;

public class AlwaysBottomHierarchy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AlwaysAtBottom());
    }

    IEnumerator AlwaysAtBottom() 
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            transform.SetAsLastSibling();
        }
    }
}
