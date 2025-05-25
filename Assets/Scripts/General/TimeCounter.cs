using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Image minuteDigit;
    public Image secondTens;
    public Image secondUnits;

    public Sprite[] digitSprites;
    public int startMinutes = 1;
    public int startSeconds = 30;

    private float remainingTime;

    void Start()
    {
        remainingTime = startMinutes * 60 + startSeconds;
    }

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0f) remainingTime = 0f;
        }

        if (remainingTime == 1f)
        {
            KeepGameData data = GameObject.Find("KeepCoffeeData").GetComponent<KeepGameData>();
            data.KeepCoffeeDataAfterLoad();
        }

        int totalSeconds = Mathf.FloorToInt(remainingTime);
        int minutes = Mathf.Clamp(totalSeconds / 60, 0, 9);
        int seconds = totalSeconds % 60;

        int sTens = seconds / 10;
        int sUnits = seconds % 10;

        if (digitSprites.Length >= 10)
        {
            minuteDigit.sprite = digitSprites[minutes];
            secondTens.sprite = digitSprites[sTens];
            secondUnits.sprite = digitSprites[sUnits];
        }
    }
}
