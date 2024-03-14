using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialTextManger : MonoBehaviour
{
    [SerializeField] private string message;

    [SerializeField] private ListOfSpriteAssets assets;
    public static bool PS4;
    [SerializeField] private List<string> xboxButtonName;
    [SerializeField] private List<string> ps4ButtonName;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        if (PS4)
        {
            message.Replace("BUTTON", $"<sprite=\"{assets.assest.ElementAt(1).name}\" name=\"{ps4ButtonName[0]}\">");
            message.Replace("BUTTON2", $"<sprite=\"{assets.assest.ElementAt(1).name}\" name=\"{ps4ButtonName[1]}\">");
        }
        else
        {
           message = message.Replace("BUTTON", $"<sprite=\"{assets.assest.ElementAt(0).name}\" name=\"{xboxButtonName[0]}\">");
            if (xboxButtonName.Count > 1)
            {
             message = message.Replace("B2", $"<sprite=\"{assets.assest.ElementAt(0).name}\" name=\"{xboxButtonName[1]}\">");
            }
            if (xboxButtonName.Count > 2)
            {
                message = message.Replace("B3", $"<sprite=\"{assets.assest.ElementAt(0).name}\" name=\"{xboxButtonName[2]}\">");
            }
        }
        text.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
