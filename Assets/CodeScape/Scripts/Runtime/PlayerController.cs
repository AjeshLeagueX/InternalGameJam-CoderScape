using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Scriptables;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private void Start()
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
        FetchSavedSnippets();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && enemiesSleepSnippets > 1)
        {
            if (EnemiesSleep())
            {
                enemiesSleepSnippets--;
                SaveSnippets();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && SlowDownSnippetCount > 1)
        {
            if (EntitiesSlowDown())
            {
                SlowDownSnippetCount--;
                SaveSnippets();
            }
        }
    }

    private int enemiesSleepSnippets = 0;
    private int SlowDownSnippetCount = 0;
    void SaveSnippets()
    {
        PlayerPrefs.SetInt("sleepSnippets", enemiesSleepSnippets);
        PlayerPrefs.SetInt("slowDownSnippets", SlowDownSnippetCount);
        PlayerPrefs.Save();
        UI_Manager.Instance.UpdateSnippetUI(enemiesSleepSnippets, SlowDownSnippetCount);
    }
    void FetchSavedSnippets()
    {
        enemiesSleepSnippets = PlayerPrefs.GetInt("sleepSnippets");
        SlowDownSnippetCount = PlayerPrefs.GetInt("slowDownSnippets");
        UI_Manager.Instance.UpdateSnippetUI(enemiesSleepSnippets, SlowDownSnippetCount);
    }

    public void CollectCodeSnippet(string snippetCode)
    {
        switch (snippetCode)
        {
            case "Entities.Sleep()":
                {
                    UI_Manager.Instance.ShowStatChange("Collected Enemies.Sleep()", true);
                    enemiesSleepSnippets++;
                }
                break;
            case "Entities.Speed/2":
                {
                    UI_Manager.Instance.ShowStatChange("Collected Entities.Speed/2", true);
                    SlowDownSnippetCount++;
                }
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
            default:
                throw new NotImplementedException($"Not Implemented for effect {effect}");
        }
    }

    Coroutine currentInvertCor = null;

    [ContextMenu("Invert Control")]
    private void InvertControl()
    {
        UI_Manager.Instance.ShowStatChange("Attacked by Player.InvertControls()", false);
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
        UI_Manager.Instance.ShowStatChange("Attacked by Player.TimeMultiplier = 0.5f", false);

        animalController.TimeMultiplier = 0.5f;
        if (currentSlowDownStopCor != null)
            StopCoroutine(currentSlowDownStopCor);
        currentSlowDownStopCor = StartCoroutine(WaitAndExecuteAction(UndoSlowDown));
    }
    private void UndoSlowDown()
    {
        if (Time.timeScale == 1)
            animalController.TimeMultiplier = 1f;
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
        else if (other.tag == "EndGame")
        {
            UI_Manager.Instance.FinishGameUI();
        }
    }

    Coroutine currentEntitiesSlowDownCor = null;
    private bool EntitiesSlowDown()
    {
        if (currentEntitiesSlowDownCor != null)
            return false;
        EntitiesManager.Instance.SlowDownAllTransformers(2f);
        currentEntitiesSlowDownCor = StartCoroutine(WaitAndExecuteAction(UndoEntiesSlowDown, 10f));
        return true;
    }

    private void UndoEntiesSlowDown()
    {
        EntitiesManager.Instance.UndoSlowDown();
        currentEntitiesSlowDownCor = null;
    }
    Coroutine currentEnemySleepCor = null;
    private bool EnemiesSleep()
    {
        if (currentEnemySleepCor != null)
            return false;
        EntitiesManager.Instance.FreezeEnemyFuntions();
        currentEnemySleepCor = StartCoroutine(WaitAndExecuteAction(UndoEnemiesSleep, 10f));
        return true;
    }

    private void UndoEnemiesSleep()
    {
        EntitiesManager.Instance.UnFreezeEnemyFunctions();
        currentEnemySleepCor = null;
    }

    //[ContextMenu("Test")]
    //private void Check()
    //{
    //    var gos = GameObject.FindGameObjectsWithTag("EnemySnippet");

    //    foreach (var item in gos)
    //        Debug.Log(item.name);
    //}
}