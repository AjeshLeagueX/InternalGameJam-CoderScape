using System.Collections;
using UnityEngine;

public class BugBomb : MonoBehaviour
{
    public string snippet;
    string[] effects = { "Player.InvertControls()", "Player.TimeMultiplier = 0.5f", "Player.CanJump = false" };
    public string GetRandomBugBombSnippet()
    {
        return effects[Random.Range(0, effects.Length)];
    }
    public Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    public void InitForSpawn()
    {
        snippet = GetRandomBugBombSnippet();
        body.useGravity = false;
        body.isKinematic = true;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    public void AutoDestroy()
    {
        StartCoroutine(AutoDestroyCoroutine());
    }
    IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(10f);//Destroy after 10 secs
        Destroy(this.gameObject);
    }
    public void Destroy()
    {
        // Reset forces and clear all velocities to ensure it starts clean
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.useGravity = false;
        body.isKinematic = true;
        ObjectSpawner.Instance.ReleaseBugBomb(this);
    }
}