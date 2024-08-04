using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject settingsPanel;
   
    public void OpenSettings()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        settingsPanel.SetActive(true);
        Debug.Log("Settings");
    }
    public void CloseSettings()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        settingsPanel.SetActive(false);
        Debug.Log("Settings");
    }
    // Пример метода для загрузки игровой сцены
    public void StartGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        SceneManager.LoadScene(1);
    }

    // Пример метода для выхода в главное меню
    public void ExitGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        Application.Quit();
    }
}
