using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class TextLanguage : MonoBehaviour
{
    [SerializeField] private Languages language = Languages.English;
    private TMP_Text uiText;
    [SerializeField] int line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            string lan = PlayerPrefs.GetString("Language");
            System.Enum.TryParse(lan, out Languages parsedLanguage);
            language = parsedLanguage;
        }
        else {
            PlayerPrefs.SetString("Language", language.ToString());
        }
        TextAsset jsonFile = Resources.Load<TextAsset>(language.ToString()); // vai buscar json das mensagens
        JArray text = JArray.Parse(jsonFile.text); // dá parse
        uiText = GetComponent<TMP_Text>();
        uiText.text = text[line].ToString();
    }
}
