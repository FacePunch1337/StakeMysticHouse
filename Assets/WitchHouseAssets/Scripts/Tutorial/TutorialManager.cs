using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }


    private OrderManager orderManager;
    private Lilith lilith;
    public GameObject lilithPanel;
    public GameObject[] interactObjects; // Массив объектов, с которыми нужно взаимодействовать
    [SerializeField] private TutorialInstruction[] _tutorialInstructions; // Массив объектов, с которыми нужно взаимодействовать
    private GameObject currentTarget;

    private int currentStep = 0;
    public int GetCurrentStep() { return currentStep; }

    private bool tutorialActive = false;
    public bool GetTutorialActive() { return tutorialActive; }

   
    private Coroutine blinkingCoroutine;
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
                StartCoroutine(Instruction("Select the ingredients you need and click the buy button", currentTarget));
                break;

            case 4:
                Debug.Log("Next step 4");
                currentTarget = interactObjects[4];
                StartCoroutine(Instruction("Nice , you have all needed ingredients , close the store . Open the workbench and assemble the mortar.", currentTarget));
                break;
            case 5:
                Debug.Log("Next step 5");
                currentTarget = interactObjects[5];
                StartCoroutine(Instruction("Transfer all the ingredients in the order they should be and press the craft button . TIP : you can see the needed order anytime in book menu.", currentTarget));
                break;
            case 6:
                Debug.Log("Next step 6");
                currentTarget = interactObjects[6];
                StartCoroutine(Instruction("Wonderful! Now we can start brewing the potion . Close the workbench menu", currentTarget));
                break;
            case 7:
                Debug.Log("Next step 7");
                currentTarget = interactObjects[7];
                StartCoroutine(Instruction("Open the cauldron.", currentTarget));
                break;
            case 8:
                Debug.Log("Next step 8");
                currentTarget = interactObjects[8];
                StartCoroutine(Instruction("Move the mortar to the slot and press the 'Brew' button. After which you will need to mix the potion" , currentTarget));
                break;
            case 9:
                Debug.Log("Next step 9");
               
                StartCoroutine(Instruction("Nice! Now you can close cauldron menu"));
                break;
            case 10:
                Debug.Log("Next step 10");
                currentTarget = interactObjects[9];
                StartCoroutine(Instruction("Okay, now give the potion to the order menu.", currentTarget));
                break;
            case 11:
                Debug.Log("Next step 11");
                currentTarget = interactObjects[10];
                UIManager.Instance.HideAllPanels();
                StartCoroutine(Instruction("Transfer the potion to the slot and press the “Trade” button.", currentTarget));
                break;
            case 12:
                Debug.Log("Next step 12");             
                UIManager.Instance.HideAllPanels();
                StartCoroutine(Instruction("Great. Thank you very much for your help! You did well! Look at your recipes. They open at every level"));
                break;

                // Добавить больше шагов по мере необходимости
        }
    }

    private IEnumerator Instruction(string message, GameObject target)
    {
        yield return new WaitForSeconds(0.5f);
        ShowLilith(message);
        CheckForTutorialTrigger(target);
        HighlightObject(target);
    }
    private IEnumerator Instruction(string message)
    {
        yield return new WaitForSeconds(0.5f);
        ShowLilith(message);

    }
    
    private void ShowLilith(string text)
    {
        lilithPanel.SetActive(true);
        lilith.Dialog(text);
    }

    private void CheckForTutorialTrigger(GameObject target)
    {
        if (target.TryGetComponent<TutorialNextTrigger>(out TutorialNextTrigger trigger))
        {
            trigger.Init();
            Debug.Log($"Activated TutorialTrigger");
        }
    }

    private void HighlightObject(GameObject target)
    {
        Image imageComponent = target.GetComponent<Image>();
        if (imageComponent != null)
        {
            if (blinkingCoroutine == null)
            {
                blinkingCoroutine = StartCoroutine(BlinkObject(imageComponent));
            }
        }
        else
        {
            Debug.LogWarning("Target does not have an Image component");
        }
    }

    private IEnumerator BlinkObject(Image image)
    {
        Color originalColor = image.color;
        Color highlightColor;
        if (ColorUtility.TryParseHtmlString("#F0FF99", out highlightColor))
        {
            float blinkDuration = 1f; // Duration for a full blink cycle (fade in and out)
            float halfDuration = blinkDuration / 2f;

            while (true) // Бесконечный цикл для мигания
            {
                // Fade in
                for (float t = 0; t < halfDuration; t += Time.deltaTime)
                {
                    float alpha = Mathf.Lerp(0, 1, t / halfDuration);
                    image.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, alpha);
                    yield return null;
                }

                // Fade out
                for (float t = 0; t < halfDuration; t += Time.deltaTime)
                {
                    float alpha = Mathf.Lerp(1, 0, t / halfDuration);
                    image.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, alpha);
                    yield return null;
                }
            }
        }
        else
        {
            Debug.LogWarning("Failed to parse color string");
        }
    }

    public void Next()
    {
        if (tutorialActive == false)
            return;

        // Убираем подсветку
        RemoveHighlight(currentTarget);

        // Переходим к следующему шагу
        currentStep++;
        NextStep();
    }

    private void RemoveHighlight(GameObject target)
    {
       
        Image imageComponent = target.GetComponent<Image>();
        if (imageComponent != null)
        {
            // Останавливаем мигание и убираем подсветку
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
                blinkingCoroutine = null;
            }
            // Возвращаем изначальный цвет (можно задать через прозрачность или другой способ)
            imageComponent.color = Color.white; // Пример возврата к белому цвету
        }
    }

    public void EndTutorial()
    {
        tutorialActive = false;
        RemoveHighlight(currentTarget);
    }

    public void TryMoveNext(int triggeredStep)
    {
        if (currentStep != triggeredStep)
            return;
            
        Next();
    }


   
   

    [System.Serializable]
    private struct TutorialInstruction
    {
        public GameObject target;
        public UnityEvent targetAction;
    }
}
