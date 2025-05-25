using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class HospitalDialogueSystem : MonoBehaviour
{
    public GameObject hospitalBillPanel;
    public TMP_Text billText;
    public Button yesButton;
    public Button noButton;
    public int baseBillCost = 80;

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
        LoadLanguage();
        LoadDialogueData();
        StartCoroutine(DelayedStartDialogue());
    }

    void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            string lan = PlayerPrefs.GetString("Language");

            if (System.Enum.TryParse(lan, out Languages parsedLanguage))
            {
                language = parsedLanguage;
            }
            else
            {
                language = Languages.English;
            }
        }
        else
        {
            language = Languages.English;
        }
    }

    void LoadDialogueData()
    {
        string fileName = "HospitalDialogue_" + language.ToString();

        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            dialogueData = JArray.Parse(jsonFile.text);
        }
    }

    IEnumerator DelayedStartDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput();
        }
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

        doctorText.text = "";

        yield return StartCoroutine(FadeInPanel(doctorPanel, doctorImage));

        typingCoroutine = StartCoroutine(TypeText(doctorText, text));
        yield return typingCoroutine;
    }

    IEnumerator ShowPlayerDialogue(string text)
    {
        if (doctorPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeOutPanel(doctorPanel, doctorImage));
        }

        playerText.text = "";

        yield return StartCoroutine(FadeInPanel(playerPanel, playerImage));

        typingCoroutine = StartCoroutine(TypeText(playerText, text));
        yield return typingCoroutine;
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
            typingCoroutine = null;
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
        StartCoroutine(EndDialogueSequence());
    }

    IEnumerator EndDialogueSequence()
    {
        // Fazer fade out dos painéis do diálogo
        if (doctorPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeOutPanel(doctorPanel, doctorImage));
        }

        if (playerPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeOutPanel(playerPanel, playerImage));
        }

        // Pequena pausa antes de mostrar a conta
        yield return new WaitForSeconds(0.5f);

        // Agora mostrar a conta do hospital
        ShowHospitalBill();
    }

    void ShowHospitalBill()
    {
        // Calcular o custo baseado na vida perdida
        int playerLife = GetPlayerLife();
        int lifeLost = 100 - playerLife;
        int billCost = baseBillCost + (lifeLost * 2); // 2 dinheiro por ponto de vida perdido

        // Mostrar a UI da conta
        hospitalBillPanel.SetActive(true);
        billText.text = $"Hospital Bill: ${billCost}\nDo you want to pay?";

        // Configurar os botões
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => OnYesClicked(billCost));
        noButton.onClick.AddListener(OnNoClicked);
    }

    int GetPlayerLife()
    {
        GameObject dataObj = GameObject.Find("KeepCoffeeData");
        if (dataObj != null)
        {
            KeepGameData data = dataObj.GetComponent<KeepGameData>();
            return data.playerLife;
        }
        return 100; // Valor padrão se não encontrar os dados
    }

    int GetPlayerMoney()
    {
        GameObject dataObj = GameObject.Find("KeepCoffeeData");
        if (dataObj != null)
        {
            KeepGameData data = dataObj.GetComponent<KeepGameData>();
            return data.money; // Assumindo que você tem uma variável money no KeepGameData
        }
        return 0;
    }

    void OnYesClicked(int billCost)
    {
        int playerMoney = GetPlayerMoney();

        if (playerMoney >= billCost)
        {
            // Pagar a conta
            GameObject dataObj = GameObject.Find("KeepCoffeeData");
            if (dataObj != null)
            {
                KeepGameData data = dataObj.GetComponent<KeepGameData>();
                data.money -= billCost;
            }

            // Voltar para o CoffeeShop
            StartCoroutine(FadeAndLoadScene("CoffeeShop"));
        }
        else
        {
            // Não tem dinheiro suficiente - vai para o menu
            StartCoroutine(FadeAndLoadScene("TitleScreen"));
        }
    }

    void OnNoClicked()
    {
        // Não quer pagar - vai para o menu
        StartCoroutine(FadeAndLoadScene("TitleScreen"));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Aqui você pode adicionar um fade out se tiver um sistema de fade
        // Por exemplo, se tiver um FadeManager:
        // yield return StartCoroutine(FadeManager.Instance.FadeOut());

        yield return new WaitForSeconds(0.5f); // Pequena pausa
        SceneManager.LoadScene(sceneName);
    }
}