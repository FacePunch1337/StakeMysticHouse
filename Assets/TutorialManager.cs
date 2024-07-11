using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }


    private OrderManager orderManager;
    private Lilith lilith;
    public GameObject lilithPanel;
    public GameObject[] interactObjects; // Массив объектов, с которыми нужно взаимодействовать
    private GameObject currentTarget;

    private int currentStep = 0;
    public int GetCurrentStep() { return currentStep; }

    private bool tutorialActive = false;
    public bool GetTutorialActive() { return tutorialActive; }
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
        orderManager = GameManager.Instance.GetComponent<OrderManager>();
        lilith = GameManager.Instance.GetComponent<Lilith>();
    }

    public void StartTutorial()
    {
        tutorialActive = true;
        currentStep = 0;
        NextStep();
    }

    private void NextStep()
    {
        switch (currentStep)
        {
            case 0:
                Debug.Log("Start");
                currentTarget = interactObjects[0];
                StartCoroutine(Instruction("Welcome! Let's learn how to complete your first order. Open book and look recipe.", currentTarget));
                break;
            case 1:
                Debug.Log("Next step 1");
                StartCoroutine(Instruction("This is your current order. You need to make this potion with this igredients. "));
                currentTarget = interactObjects[1];
                StartCoroutine(Instruction("Remember the sequence? Let's move on. Let's go back to the room!", currentTarget));
                break;
            case 2:
                Debug.Log("Next step 2");
                currentTarget = interactObjects[2];
                StartCoroutine(Instruction("We need to buy ingredients. Open a store.", currentTarget));
                break;
            case 3:
                Debug.Log("Next step 3");
                currentTarget = interactObjects[3];
                StartCoroutine(Instruction("Select the ingredients you need and click the buy button. After purchasing, close the store", currentTarget));
                break;

            case 4:
                Debug.Log("Next step 4");
                currentTarget = interactObjects[4];
                StartCoroutine(Instruction("Open the workbench and assemble the mortar.", currentTarget));
                break;
            case 5:
                Debug.Log("Next step 5");
                currentTarget = interactObjects[5];
                StartCoroutine(Instruction("Transfer all the ingredients in the order they should be and press the craft button.", currentTarget));
                break;
            case 6:
                Debug.Log("Next step 6");
                currentTarget = interactObjects[6];
                StartCoroutine(Instruction("Wonderful! Now we can start brewing the potion.", currentTarget));
                break;
            case 7:
                Debug.Log("Next step 7");
                currentTarget = interactObjects[7];
                StartCoroutine(Instruction("Open the cauldron.", currentTarget));
                break;
            case 8:
                Debug.Log("Next step 8");
                StartCoroutine(Instruction("Move the mortar to the slot and press the 'Brew' button. After which you will need to mix the potion"));
                break;
            case 9:
                Debug.Log("Next step 9");
                currentTarget = interactObjects[8];
                StartCoroutine(Instruction("Nice! Good job!", currentTarget));
                break;
            case 10:
                Debug.Log("Next step 10");
                currentTarget = interactObjects[9];
                StartCoroutine(Instruction("Okay, now give the potion to the order menu.", currentTarget));
                break;
            case 11:
                Debug.Log("Next step 11");
                UIManager.Instance.HideAllPanels();
                StartCoroutine(Instruction("Transfer the potion to the slot and press the “Trade” button.", currentTarget));
                break;
            case 12:
                Debug.Log("Next step 12");
                UIManager.Instance.HideAllPanels();
                StartCoroutine(Instruction("Great. Thank you very much for your help! You did well! Look at your recipes. They open at every level.", currentTarget));
                break;

                // Добавить больше шагов по мере необходимости
        }
    }

    private IEnumerator Instruction(string message, GameObject target)
    {
        yield return new WaitForSeconds(1f);
        ShowLilith(message);
        HighlightObject(target);
    }
    private IEnumerator Instruction(string message)
    {
        yield return new WaitForSeconds(1f);
        ShowLilith(message);

    }


    private void ShowLilith(string text)
    {
        lilithPanel.SetActive(true);
        lilith.Dialog(text);
    }

    private void HighlightObject(GameObject target)
    {
        Image imageComponent = target.GetComponent<Image>();
        if (imageComponent != null)
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#F0FF99", out newColor))
            {
                imageComponent.color = newColor;
            }
            else
            {
                Debug.LogWarning("Failed to parse color string");
            }
        }
        else
        {
            Debug.LogWarning("Target does not have an Image component");
        }
    }

    public void Next()
    {


        if (tutorialActive)
        {
            // Убираем подсветку
            RemoveHighlight(currentTarget);

            // Переходим к следующему шагу
            currentStep++;
            NextStep();
        }
    }


    private void RemoveHighlight(GameObject target)
    {
        Image imageComponent = target.GetComponent<Image>();
        if (imageComponent != null)
        {
            // Возвращаем изначальный цвет (можно задать через прозрачность или другой способ)
            imageComponent.color = Color.white; // Пример возврата к белому цвету
        }
    }

    public void EndTutorial()
    {
        tutorialActive = false;
    }

}
