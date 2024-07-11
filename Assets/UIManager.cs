using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject workbanchPanel;
    [SerializeField] private GameObject cauldronPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject orderPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject lilithPanel;
    [SerializeField] private GameObject rewardPanel;

    // Добавьте ссылки на UI-элементы для уровня и прогресс-бара
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Image experienceBar;

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
                workbanchPanel.SetActive(true);
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

    public void HideAllPanels()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        cauldronPanel.SetActive(false);
        bookshelfPanel.SetActive(false);
        workbanchPanel.SetActive(false);
        shopPanel.SetActive(false);
        orderPanel.SetActive(false);
       
        CloseInventry();
       

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
    private void CloseInventry()
    {
        inventoryPanel.SetActive(false);
    }

    // Метод для обновления UI уровня и прогресса
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
