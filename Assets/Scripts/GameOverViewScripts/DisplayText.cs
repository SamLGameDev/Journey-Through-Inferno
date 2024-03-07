using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    private GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).gameObject;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
