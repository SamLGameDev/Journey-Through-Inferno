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
    private static bool _createdCard = false;
    void Start()
    {
        

    }
    public void Display()
    {
        if (_createdCard)
        {
            return;
        }
        _createdCard = true;
        text = Instantiate(TextBox, GameObject.Find("CardSelectionCanvas").transform);
        text.GetComponent<RectTransform>().position =(Vector2)GameManager.instance.topTextBox.transform.position;
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(525, 200);
        text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
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
        bottomtext.GetComponent<RectTransform>().position = new Vector2(GameManager.instance.bottomTextBox.transform.position.x,
            GameManager.instance.bottomTextBox.transform.position.y + 2);
        bottomtext.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 200);
        bottomtext.GetComponent<TMPro.TextMeshProUGUI>().text = card.description.Substring(i+1);
        bottomtext.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        text.SetActive(true);
        bottomtext.SetActive(true);


    }
    public void hide()
    {
        Destroy(text);
        Destroy(bottomtext);
        _createdCard = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
