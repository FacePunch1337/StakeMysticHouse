using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    [SerializeField] private int rewardIntervalMinutes = 1; // �������� � ������� ����� ���������
    [SerializeField] private int value;
    [SerializeField] private TMP_Text countdownText; // UI-������� ��� ����������� ��������� �������
    [SerializeField] private TMP_Text valueText; // UI-������� ��� ����������� ��������� �������

    private DateTime lastRewardTime;
    private DateTime nextRewardTime;
    private bool isFirstRewardGiven = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadLastRewardTime();

        if (lastRewardTime != DateTime.MinValue)
        {
            isFirstRewardGiven = true;
            nextRewardTime = lastRewardTime.AddMinutes(rewardIntervalMinutes);
            StartRewardCountdown();
        }
    }

    public void StartRewardCountdown()
    {
        StartCoroutine(CheckForReward());
        StartCoroutine(UpdateCountdown());
    }

    private IEnumerator StartCountdownCoroutine(TimeSpan initialDelay)
    {
        yield return new WaitForSeconds((float)initialDelay.TotalSeconds);
        GiveReward();
        lastRewardTime = DateTime.Now;
        nextRewardTime = lastRewardTime.AddMinutes(rewardIntervalMinutes);
        SaveLastRewardTime();
    }

    public void GiveFirstReward()
    {
        if (!isFirstRewardGiven)
        {
            GiveReward();
            lastRewardTime = DateTime.Now;
            nextRewardTime = lastRewardTime.AddMinutes(rewardIntervalMinutes);
            SaveLastRewardTime();
            isFirstRewardGiven = true;
            StartRewardCountdown();
        }
    }

    private void LoadLastRewardTime()
    {
        if (PlayerPrefs.HasKey("LastRewardTime"))
        {
            long ticks = Convert.ToInt64(PlayerPrefs.GetString("LastRewardTime"));
            lastRewardTime = new DateTime(ticks);
        }
        else
        {
            lastRewardTime = DateTime.MinValue;
        }
    }

    private void SaveLastRewardTime()
    {
        PlayerPrefs.SetString("LastRewardTime", lastRewardTime.Ticks.ToString());
    }

    private IEnumerator CheckForReward()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // ��������� ������ �������

            DateTime now = DateTime.Now;

            if (now >= nextRewardTime)
            {
                GiveReward();
                lastRewardTime = now;
                nextRewardTime = lastRewardTime.AddMinutes(rewardIntervalMinutes);
                SaveLastRewardTime();
            }
        }
    }

    private IEnumerator UpdateCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // ��������� ������ �������

            TimeSpan timeRemaining = nextRewardTime - DateTime.Now;

            if (timeRemaining.TotalSeconds > 0)
            {
                countdownText.text = string.Format("{0:D2}:{1:D2}",
                    timeRemaining.Minutes,
                    timeRemaining.Seconds);
            }
            else
            {
                countdownText.text = "00:00";
            }
        }
    }

    private void GiveReward()
    {
        // �������� ����� ��� ��� ������ ������� ������
        AudioManager.Instance.PlaySound(AudioManager.Sound.RewardSound);
        valueText.text = value.ToString();
        Debug.Log("Player has received a reward!");
        UIManager.Instance.OpenReward();
    }

    public void ApplyReward()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.BuySound);
        GameManager.Instance.AddMoney(value);
        UIManager.Instance.CloseReward();
    }
}
