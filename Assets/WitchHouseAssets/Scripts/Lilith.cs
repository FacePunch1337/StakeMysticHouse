using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lilith : MonoBehaviour
{
    public static Lilith Instance { get; private set; }

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
    [SerializeField] private List<Sprite> emotes;
    [SerializeField] private GameObject lilith;
    [SerializeField] private TMP_Text lilithText;
    [SerializeField] private Button okButton;

    public UnityEvent OnDialogCompleted;

    private List<string> sentences;
    private int currentSentenceIndex;
    private int currentEmoteIndex;

    public bool IsDialogActive { get; private set; }

    private void Start()
    {
        if (emotes.Count > 0)
        {
            currentEmoteIndex = 0;
            lilith.GetComponent<Image>().sprite = emotes[currentEmoteIndex];
        }

        // okButton.onClick.AddListener(OnOkButtonClick);
    }

    public void Dialog(string text)
    {
        // Split the text into sentences and initialize indices
        sentences = new List<string>(Regex.Split(text, @"(?<=[.!?])\s+"));
        currentSentenceIndex = 0;
        currentEmoteIndex = 0;

        // Display the first sentence and emote
        if (sentences.Count > 0)
        {
            lilithText.text = sentences[currentSentenceIndex];
            lilith.GetComponent<Image>().sprite = emotes[currentEmoteIndex];
        }

        // Set dialog as active and open the dialog UI
        IsDialogActive = true;
        UIManager.Instance.OpenLilith();
        AudioManager.Instance.PlaySound(AudioManager.Sound.LilithVoice1);
    }

    private void OnOkButtonClick()
    {
        // Move to the next sentence and emote
        currentSentenceIndex++;
        currentEmoteIndex = (currentEmoteIndex + 1) % emotes.Count;

        // Check if there are more sentences to display
        if (currentSentenceIndex < sentences.Count)
        {
            lilithText.text = sentences[currentSentenceIndex];
            lilith.GetComponent<Image>().sprite = emotes[currentEmoteIndex];
            AudioManager.Instance.PlaySound(AudioManager.Sound.LilithVoice2);
        }
        else if (TutorialManager.Instance.GetTutorialActive() && TutorialManager.Instance.GetCurrentStep() == 12 && currentSentenceIndex == sentences.Count)
        {
            TutorialManager.Instance.EndTutorial();
        }
        else if (TutorialManager.Instance.GetTutorialActive() && TutorialManager.Instance.GetCurrentStep() == 2 && currentSentenceIndex == sentences.Count)
        {

            Invoke("GiveFirstRewardInTutorial", 1f);


        }
        else
        {
           
            UIManager.Instance.CloseLilith();
            IsDialogActive = false;
            OnDialogCompleted?.Invoke();
        }
    }

    public void GiveFirstRewardInTutorial()
    {
        RewardManager.Instance.GiveFirstReward();
        RewardManager.Instance.StartRewardCountdown(); 
    }
}
