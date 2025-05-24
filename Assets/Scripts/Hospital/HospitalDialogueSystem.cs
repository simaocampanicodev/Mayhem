using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class HospitalDialogueSystem : MonoBehaviour
{
    public GameObject doctorPanel;
    public GameObject playerPanel;
    public TMP_Text doctorText;
    public TMP_Text playerText;
    public Image doctorImage;
    public Image playerImage;

    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 0.5f;

    private Languages language = Languages.English;
    private JArray dialogueData;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private bool canProceed = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            string lan = PlayerPrefs.GetString("Language");
            System.Enum.TryParse(lan, out Languages parsedLanguage);
            language = parsedLanguage;
        }

        LoadDialogueData();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput();
        }
    }

    void LoadDialogueData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("HospitalDialogue_" + language.ToString());
        dialogueData = JArray.Parse(jsonFile.text);
    }

    void StartDialogue()
    {
        currentDialogueIndex = 0;
        ShowCurrentDialogue();
    }

    void HandleInput()
    {
        if (isTyping)
        {
            CompleteText();
        }
        else if (canProceed)
        {
            NextDialogue();
        }
    }

    void ShowCurrentDialogue()
    {
        if (currentDialogueIndex >= dialogueData.Count)
        {
            EndDialogue();
            return;
        }

        JObject currentLine = (JObject)dialogueData[currentDialogueIndex];
        string speaker = currentLine["speaker"].ToString();
        string text = currentLine["text"].ToString();

        canProceed = false;

        if (speaker == "doctor")
        {
            StartCoroutine(ShowDoctorDialogue(text));
        }
        else if (speaker == "player")
        {
            StartCoroutine(ShowPlayerDialogue(text));
        }
    }

    IEnumerator ShowDoctorDialogue(string text)
    {
        if (playerPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeOutPanel(playerPanel, playerImage));
        }

        yield return StartCoroutine(FadeInPanel(doctorPanel, doctorImage));
        yield return StartCoroutine(TypeText(doctorText, text));
    }

    IEnumerator ShowPlayerDialogue(string text)
    {
        if (doctorPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeOutPanel(doctorPanel, doctorImage));
        }

        yield return StartCoroutine(FadeInPanel(playerPanel, playerImage));
        yield return StartCoroutine(TypeText(playerText, text));
    }

    IEnumerator FadeOutPanel(GameObject panel, Image characterImage)
    {
        float timer = 0f;
        Color imageColor = characterImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            imageColor.a = alpha;
            characterImage.color = imageColor;
            yield return null;
        }

        imageColor.a = 0f;
        characterImage.color = imageColor;
        panel.SetActive(false);
    }

    IEnumerator FadeInPanel(GameObject panel, Image characterImage)
    {
        panel.SetActive(true);

        float timer = 0f;
        Color imageColor = characterImage.color;
        imageColor.a = 0f;
        characterImage.color = imageColor;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            imageColor.a = alpha;
            characterImage.color = imageColor;
            yield return null;
        }

        imageColor.a = 1f;
        characterImage.color = imageColor;
    }

    IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        isTyping = true;
        textComponent.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        canProceed = true;
    }

    void CompleteText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        JObject currentLine = (JObject)dialogueData[currentDialogueIndex];
        string text = currentLine["text"].ToString();
        string speaker = currentLine["speaker"].ToString();

        if (speaker == "doctor")
        {
            doctorText.text = text;
        }
        else if (speaker == "player")
        {
            playerText.text = text;
        }

        isTyping = false;
        canProceed = true;
    }

    void NextDialogue()
    {
        currentDialogueIndex++;
        ShowCurrentDialogue();
    }

    void EndDialogue()
    {
        // Usar depois para chamar a conta do hospital
    }
}