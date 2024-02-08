using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDescription : MonoBehaviour
{
    // Start is called before the first frame update
    public TarotCards card;
    public GameObject TextBox;
    private GameObject text;
    private GameObject bottomtext;
    void Start()
    {
        
    }
    public void Display()
    {
        text = Instantiate(TextBox, GameObject.Find("CardSelectionCanvas").transform);
        text.GetComponent<RectTransform>().position = new Vector2(425, 250);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(525, 200);
        int i = -1;
        foreach (char letter in card.description)
        {
            i++;
            if (letter == '_')
            {
                break;
            }
        }
        text.GetComponent<TMPro.TextMeshProUGUI>().text = card.description.Substring(0, i);
        bottomtext = Instantiate(TextBox, GameObject.Find("CardSelectionCanvas").transform);
        bottomtext.GetComponent<RectTransform>().position = new Vector2(425, 100);
        bottomtext.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 200);
        bottomtext.GetComponent<TMPro.TextMeshProUGUI>().text = card.description.Substring(i+1);
        text.SetActive(true);
        bottomtext.SetActive(true) ;

    }
    public void hide()
    {
        Destroy(text);
        Destroy(bottomtext);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
