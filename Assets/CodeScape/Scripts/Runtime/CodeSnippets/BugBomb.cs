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
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Animal" && other.GetComponent<PlayerController>() != null)
    //    {
    //        PlayerController.Instance.ApplyNegativeEffect(type);
    //        StopCoroutine(AutoDestroy());
    //        Destroy(gameObject);
    //    }
    //}
    public void AutoDestroy()
    {
        StartCoroutine(AutoDestroyCoroutine());
    }
    IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(10f);//Destroy after 10 secs
        Destroy(this.gameObject);
    }
    //public void Destroy()
    //{
    //    body.useGravity = false;
    //    body.isKinematic = true;
    //    ObjectPool.Instance.ReleaseBugBomb(this);
    //}
}