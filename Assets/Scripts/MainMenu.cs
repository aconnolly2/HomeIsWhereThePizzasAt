using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    List<GameObject> UIPanels = new List<GameObject>();

    public GameObject MainMenuScreen;
    public GameObject BackStoryScreen;
    public GameObject CreditsScreen;
    public GameObject Instructions1Screen;
    public GameObject Instructions2Screen;

    private void Start()
    {
        UIPanels.Add(MainMenuScreen);
        UIPanels.Add(BackStoryScreen);
        UIPanels.Add(CreditsScreen);
        UIPanels.Add(Instructions1Screen);
        UIPanels.Add(Instructions2Screen);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Backstory()
    {
        closeScreens();
        BackStoryScreen.SetActive(true);
    }

    public void Instructions1()
    {
        closeScreens();
        Instructions1Screen.SetActive(true);
    }

    public void Instructions2()
    {
        closeScreens();
        Instructions2Screen.SetActive(true);
    }

    public void Credits()
    {
        closeScreens();
        CreditsScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        closeScreens();
        MainMenuScreen.SetActive(true);
    }

    void closeScreens()
    {
        foreach(GameObject go in UIPanels)
        {
            go.SetActive(false);
        }
    }
}
