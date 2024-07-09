using MalbersAnimations.Controller;
using UnityEngine;

public class CollectibleCodeSnippet : MonoBehaviour
{
    CollectibleManager manager;
    Transform spawnPoint;

    bool isBugBomb = false;
    string snippet = "";

    public void Init(CollectibleManager manager, Transform spawnPoint)
    {
        this.manager = manager;
        this.spawnPoint = spawnPoint;
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1)
        {
            isBugBomb = false;
            snippet = CodeSnippets.GetRandomPositiveSnippet();
        }
        else
        {
            isBugBomb = true;
            snippet = CodeSnippets.GetRandomBugBombSnippet();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Animal" && other.GetComponent<PlayerController>() != null)
        {
            if (isBugBomb)
            {
                PlayerController.Instance.ApplyNegativeEffect(snippet);
            }
            else
            {
                PlayerController.Instance.CollectCodeSnippet(snippet);
            }
            manager.Collect(spawnPoint);
            Destroy(gameObject);
        }
    }
}