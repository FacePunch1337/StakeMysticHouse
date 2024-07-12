using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    // Массив булей для отслеживания изученных рецептов
    public bool[] learnedRecipes = new bool[3] { true, false, false }; // Первый рецепт открыт по умолчанию

    // Массив объектов RecipeUI
    public RecipeUI[] recipeUIs;

    // Индекс текущего рецепта
    private int currentRecipeIndex = 0;

    // Цвета для закрытых и открытых рецептов
    public Color closedColor = new Color(0.26f, 0.25f, 0.35f); // #424159
    public Color openedColor = Color.white;

    private int money = 0;
    private int currentLevel = 1;
    private int currentExperience = 0;
    private int experienceToNextLevel = 100;

    private void Start()
    {
        LoadProgress();

        // Обновление интерфейса после загрузки прогресса
        for (int i = 0; i < recipeUIs.Length; i++)
        {
            UpdateRecipeUI(i);
            SetRecipeActive(i, i == currentRecipeIndex);
        }

        UIManager.Instance.UpdateUI(); // Обновление UI уровня после загрузки

    }


    public void AddExperience(int amount)
    {
        currentExperience += amount;
        CheckLevelUp();
        UIManager.Instance.UpdateUI();
        SaveProgress(); // Сохранение прогресса при добавлении опыта
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UIManager.Instance.UpdateUI();
        SaveProgress(); // Сохранение прогресса при добавлении денег
    }

    public void Pay(int amount)
    {
        money -= amount;
        UIManager.Instance.UpdateUI();
        SaveProgress(); // Сохранение прогресса при трате денег
    }

    public int GetMoney()
    {
        return money;
    }

    private void CheckLevelUp()
    {
        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            currentLevel++;
            experienceToNextLevel = CalculateNextLevelExperience(currentLevel);
            UnlockRecipe(currentLevel);
            UIManager.Instance.UpdateUI();

            if (currentLevel > 1)
            {
                AudioManager.Instance.PlaySound(AudioManager.Sound.RewardSound);
                UIManager.Instance.ShowLevelUpView();
                ShopManager.Instance.UnlockItems(2); // На каждом новом уровне разблокируем 2 новых предмета
            }

            SaveProgress(); // Сохранение прогресса при повышении уровня
        }
    }

    private int CalculateNextLevelExperience(int level)
    {
        return 100 * 2 * (level - 1);
    }

    // Метод для открытия нового рецепта
    private void UnlockRecipe(int level)
    {
        if (level - 1 < learnedRecipes.Length)
        {
            learnedRecipes[level - 1] = true;
            Debug.Log("Recipe " + (level - 1) + " unlocked!");
            UpdateRecipeUI(level - 1);
            SwitchToRecipe(level - 1);
            SaveProgress(); // Сохранение прогресса при разблокировке рецепта
        }
    }

    // Метод для обновления UI рецепта
    private void UpdateRecipeUI(int recipeIndex)
    {
        if (recipeUIs[recipeIndex] != null)
        {
            recipeUIs[recipeIndex].recipeImage.color = learnedRecipes[recipeIndex] ? openedColor : closedColor;
            recipeUIs[recipeIndex].nameText.SetActive(learnedRecipes[recipeIndex]);
            recipeUIs[recipeIndex].ingredients.SetActive(learnedRecipes[recipeIndex]);
        }
    }

    // Метод для переключения на указанный рецепт
    private void SwitchToRecipe(int recipeIndex)
    {
        if (recipeIndex >= 0 && recipeIndex < recipeUIs.Length)
        {
            SetRecipeActive(currentRecipeIndex, false);
            currentRecipeIndex = recipeIndex;
            SetRecipeActive(currentRecipeIndex, true);
            SaveProgress(); // Сохранение прогресса при переключении рецепта
        }
    }

    public void NextRecipe()
    {
        if (currentRecipeIndex < recipeUIs.Length - 1)
        {
            SetRecipeActive(currentRecipeIndex, false);
            currentRecipeIndex++;
            SetRecipeActive(currentRecipeIndex, true);
            SaveProgress(); // Сохранение прогресса при переключении на следующий рецепт
        }
    }

    public void PreviousRecipe()
    {
        if (currentRecipeIndex > 0)
        {
            SetRecipeActive(currentRecipeIndex, false);
            currentRecipeIndex--;
            SetRecipeActive(currentRecipeIndex, true);
            SaveProgress(); // Сохранение прогресса при переключении на предыдущий рецепт
        }
    }

    private void SetRecipeActive(int recipeIndex, bool isActive)
    {
        if (recipeUIs[recipeIndex] != null)
        {
            recipeUIs[recipeIndex].recipeObject.SetActive(isActive);
        }
    }

    public List<int> GetLearnedRecipes()
    {
        List<int> learnedRecipeIndices = new List<int>();
        for (int i = 0; i < learnedRecipes.Length; i++)
        {
            if (learnedRecipes[i])
            {
                learnedRecipeIndices.Add(i);
            }
        }
        return learnedRecipeIndices;
    }

    public string GetRecipeName(int recipeIndex)
    {
        if (recipeUIs[recipeIndex] != null)
        {
            return recipeUIs[recipeIndex].nameText.GetComponent<TMP_Text>().text;

        }
        return null;
    }
    public string GetMortarName(int recipeIndex)
    {
        if (recipeUIs[recipeIndex] != null)
        {
            return recipeUIs[recipeIndex].mortarName;

        }
        return null;
    }

    public Sprite GetRecipeSprite(int recipeIndex)
    {
        if (recipeUIs[recipeIndex] != null)
        {
            return recipeUIs[recipeIndex].recipeImage.sprite;
        }
        return null;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentExperience()
    {
        return currentExperience;
    }

    public int GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }

    private void SaveProgress()
    {
        Debug.Log("Saving progress...");

        // Сохранение текущих значений
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.SetInt("Experience", currentExperience);
        PlayerPrefs.SetInt("CurrentRecipeIndex", currentRecipeIndex);

        for (int i = 0; i < learnedRecipes.Length; i++)
        {
            PlayerPrefs.SetInt("Recipe_" + i, learnedRecipes[i] ? 1 : 0);
            Debug.Log($"Saved Recipe_{i}: {(learnedRecipes[i] ? 1 : 0)}");
        }

        // Принудительное сохранение данных
        PlayerPrefs.Save();

        Debug.Log("Progress saved.");
    }

    private void LoadProgress()
    {
        Debug.Log("Loading progress...");

        // Загрузка сохраненных значений
        money = PlayerPrefs.GetInt("Money", 0);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        currentExperience = PlayerPrefs.GetInt("Experience", 0);
        currentRecipeIndex = PlayerPrefs.GetInt("CurrentRecipeIndex", 0);

        // Логирование загруженных значений
        Debug.Log($"Loaded Money: {money}");
        Debug.Log($"Loaded Level: {currentLevel}");
        Debug.Log($"Loaded Experience: {currentExperience}");
        Debug.Log($"Loaded CurrentRecipeIndex: {currentRecipeIndex}");

        for (int i = 0; i < learnedRecipes.Length; i++)
        {
            learnedRecipes[i] = PlayerPrefs.GetInt("Recipe_" + i, i == 0 ? 1 : 0) == 1;
            Debug.Log($"Loaded Recipe_{i}: {(learnedRecipes[i] ? 1 : 0)}");
        }

        Debug.Log("Progress loaded.");
    }

    // Метод для загрузки новой игры или уровня
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Метод для выхода в главное меню
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
