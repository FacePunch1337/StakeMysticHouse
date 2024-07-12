using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public enum PanelType
    {
        Cauldron,
        Bookshelf,
        Workbench,
        Shop,
        Order,
        Lilith,
        Reward,
        Settings
    }

    [SerializeField] private GameObject bookshelfPanel;
    [SerializeField] private GameObject workbenchPanel;
    [SerializeField] private GameObject cauldronPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject orderPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject lilithPanel;
    [SerializeField] private GameObject rewardPanel;

    [SerializeField] private GameObject levelUpView;
    [SerializeField] private TMP_Text levelUpViewText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Image experienceBar;

    private CanvasGroup levelUpCanvasGroup1;
    private CanvasGroup levelUpCanvasGroup2;

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

        levelUpCanvasGroup1 = levelUpView.GetComponent<CanvasGroup>();
        levelUpCanvasGroup2 = levelUpViewText.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        UpdateUI();
    }

    public void ShowPanel(PanelType panelType)
    {
        StartCoroutine(ShowPanelWithDelay(panelType, 0.3f));
    }

    private IEnumerator ShowPanelWithDelay(PanelType panelType, float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (panelType)
        {
            case PanelType.Cauldron:
                cauldronPanel.SetActive(true);
                OpenInventory("mortars");
                break;
            case PanelType.Bookshelf:
                bookshelfPanel.SetActive(true);
                break;
            case PanelType.Workbench:
                workbenchPanel.SetActive(true);
                OpenInventory("ingredients");
                break;
            case PanelType.Shop:
                shopPanel.SetActive(true);
                break;
            case PanelType.Order:
                orderPanel.SetActive(true);
                OpenInventory("potions");
                break;
            case PanelType.Lilith:
                lilithPanel.SetActive(true);
                break;
            case PanelType.Reward:
                rewardPanel.SetActive(true);
                break;
        }
    }

    public void ShowLevelUpView()
    {
        StartCoroutine(ShowLevelUpViewCoroutine());
    }

    private IEnumerator ShowLevelUpViewCoroutine()
    {
        float duration = 0.5f; // Длительность анимации

        // Появление
        levelUpView.SetActive(true);
        float startTime = Time.time;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (Time.time < startTime + duration)
        {
            float progress = (Time.time - startTime) / duration;
            levelUpCanvasGroup1.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            levelUpCanvasGroup2.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }
        levelUpCanvasGroup1.alpha = endAlpha;
        levelUpCanvasGroup2.alpha = endAlpha;

        // Задержка перед исчезновением
        yield return new WaitForSeconds(1f);

        // Исчезновение
        startTime = Time.time;
        startAlpha = 1f;
        endAlpha = 0f;

        while (Time.time < startTime + duration)
        {
            float progress = (Time.time - startTime) / duration;
            levelUpCanvasGroup1.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            levelUpCanvasGroup2.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }
        levelUpCanvasGroup1.alpha = endAlpha;
        levelUpCanvasGroup2.alpha = endAlpha;

        levelUpView.SetActive(false);
    }

    public void HideAllPanels()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        cauldronPanel.SetActive(false);
        bookshelfPanel.SetActive(false);
        workbenchPanel.SetActive(false);
        shopPanel.SetActive(false);
        orderPanel.SetActive(false);
        CloseInventory();
    }

    private void OpenInventory(string name)
    {
        inventoryPanel.SetActive(true);
        Inventory.Instance.ShowGrid(name);
    }

    public void OpenShop()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        ShowPanel(PanelType.Shop);
    }

    public void OpenOrder()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        ShowPanel(PanelType.Order);
    }

    public void OpenLilith()
    {
        ShowPanel(PanelType.Lilith);
    }

    public void OpenReward()
    {
        ShowPanel(PanelType.Reward);
    }

    public void OpenSettings()
    {
        ShowPanel(PanelType.Settings);
    }

    public void CloseLilith()
    {
        lilithPanel.SetActive(false);
    }

    public void CloseReward()
    {
        rewardPanel.SetActive(false);
    }

    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        int currentLevel = GameManager.Instance.GetCurrentLevel();
        int currentExperience = GameManager.Instance.GetCurrentExperience();
        int experienceToNextLevel = GameManager.Instance.GetExperienceToNextLevel();

        int money = GameManager.Instance.GetMoney();
        levelText.text = currentLevel.ToString();
        moneyText.text = money.ToString();
        experienceBar.fillAmount = (float)currentExperience / experienceToNextLevel;
    }
}
