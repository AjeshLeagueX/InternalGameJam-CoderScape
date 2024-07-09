using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDir;
    [SerializeField] private GameObject UICanvas;
    public void StartGame()
    {
        UICanvas.SetActive(false);
        LoadGame();
    }

    public void ContinueGame()
    {
        UICanvas.SetActive(false);
        LoadGame();
    }

    void LoadGame()
    {
        playableDir.Play();
    }

    public void OnPlayableFinished()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}