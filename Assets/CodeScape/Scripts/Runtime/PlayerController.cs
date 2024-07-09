using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Scriptables;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private MAnimal animalController;
    private MalbersInput animalInput;
    void Initialize()
    {
        animalController = GetComponent<MAnimal>();
        animalInput = GetComponent<MalbersInput>();
        foreach (var item in animalInput.AllInputs)
        {
            if (item.input == "Jump")
                JumpInput = item;
        }
    }

    private int enemiesSleepSnippets = 0;
    private int SlowDownSnippetCount = 0;
    void SaveSnippets()
    {
        PlayerPrefs.SetInt("sleepSnippets", enemiesSleepSnippets);
        PlayerPrefs.SetInt("slowDownSnippets", SlowDownSnippetCount);
        PlayerPrefs.Save();
    }
    void FetchSavedSnippets()
    {
        enemiesSleepSnippets = PlayerPrefs.GetInt("sleepSnippets");
        SlowDownSnippetCount = PlayerPrefs.GetInt("slowDownSnippets");
    }

    public void CollectCodeSnippet(string snippetCode)
    {
        switch (snippetCode)
        {
            case "Enemies.Sleep()":
                enemiesSleepSnippets++;
                break;
            case "Entities.Speed/2":
                SlowDownSnippetCount++;
                break;
        }
        SaveSnippets();
    }

    public void ApplyNegativeEffect(string effect)
    {
        switch (effect)
        {
            case "Player.InvertControls()":
                InvertControl();
                break;
            case "Player.TimeMultiplier = 0.5f":
                SlowDown();
                break;
            case "Player.CanJump = false":
                StopJump();
                break;
        }
    }

    Coroutine currentInvertCor = null;

    [ContextMenu("Invert Control")]
    private void InvertControl()
    {
        animalInput.Horizontal.IsInverted = true;
        animalInput.Vertical.IsInverted = true;
        if (currentInvertCor != null)
            StopCoroutine(currentInvertCor);
        currentInvertCor = StartCoroutine(WaitAndExecuteAction(UndoInvertControl));
    }

    private void UndoInvertControl()
    {
        animalInput.Horizontal.IsInverted = false;
        animalInput.Vertical.IsInverted = false;
    }

    Coroutine currentJumpStopCor = null;
    private InputRow JumpInput;
    public void StopJump()
    {
        if (JumpInput != null)
            JumpInput.active = new BoolReference(false);
        if (currentJumpStopCor != null)
            StopCoroutine(currentJumpStopCor);
        currentJumpStopCor = StartCoroutine(WaitAndExecuteAction(UndoJumpStop));
    }
    private void UndoJumpStop()
    {
        if (JumpInput != null)
            JumpInput.active = new BoolReference(true);
    }

    Coroutine currentSlowDownStopCor = null;
    public void SlowDown()
    {
        animalController.TimeMultiplier = animalController.TimeMultiplier / 2;
        if (currentSlowDownStopCor != null)
            StopCoroutine(currentSlowDownStopCor);
        currentSlowDownStopCor = StartCoroutine(WaitAndExecuteAction(UndoSlowDown));
    }
    private void UndoSlowDown()
    {
        if (Time.timeScale == 1)
            animalController.TimeMultiplier = Time.timeScale * 2;
    }

    private IEnumerator WaitAndExecuteAction(Action action, float waitTime = 5f)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemySnippet"/* && other.GetComponent<PlayerController>() != null*/)
        {
            Debug.Log("Enemy snippet " + other.gameObject.name);
            var _enemySnippet = other.GetComponent<BugBomb>();
            ApplyNegativeEffect(_enemySnippet.snippet);
            Destroy(_enemySnippet.gameObject);
        }
    }

    Coroutine currentEntitiesSlowDownCor = null;
    private void EntitiesSlowDown()
    {
        Time.timeScale = 0.5f;
        animalController.TimeMultiplier = animalController.TimeMultiplier * 2;
        if (currentEntitiesSlowDownCor != null)
            StopCoroutine(currentEntitiesSlowDownCor);
        currentEntitiesSlowDownCor = StartCoroutine(WaitAndExecuteAction(UndoEntiesSlowDown, 10f));
    }

    private void UndoEntiesSlowDown()
    {
        Time.timeScale = 1f;
        animalController.TimeMultiplier = animalController.TimeMultiplier / 2;
    }
    Coroutine currentEnemySleepCor = null;
    private void EnemiesSleep()
    {
        throw new NotImplementedException();
        if (currentEnemySleepCor != null)
            StopCoroutine(currentEnemySleepCor);
        currentEnemySleepCor = StartCoroutine(WaitAndExecuteAction(UndoEnemiesSleep, 10f));
    }

    private void UndoEnemiesSleep()
    {
    }
}