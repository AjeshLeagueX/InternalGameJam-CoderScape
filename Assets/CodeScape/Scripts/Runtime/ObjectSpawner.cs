using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;
    private void Awake()
    {
        Instance = this;

        //for (int i = 0; i < 10; i++)
        //{
        //    var _snippet = Instantiate(bugBombPrefab);
        //    bugBombQueue.Enqueue(_snippet);
        //}
    }
    [SerializeField] private BugBomb bugBombPrefab;
    private Queue<BugBomb> bugBombQueue = new Queue<BugBomb>();
    public BugBomb GetBugBomb()
    {
        //if (bugBombQueue.Count > 0)
        //{
        //    var _bugBomb = bugBombQueue.Dequeue();
        //    _bugBomb.gameObject.SetActive(true);
        //    _bugBomb.InitForSpawn();
        //    return _bugBomb;
        //}
        //else
        //{
            var _bugBomb = Instantiate(bugBombPrefab);
            _bugBomb.gameObject.SetActive(true);
            _bugBomb.InitForSpawn();
            return _bugBomb;
        //}
    }

    //public void ReleaseBugBomb(BugBomb bugBomb)
    //{
    //    if (bugBomb != null)
    //    {
    //        bugBomb.transform.parent = this.transform;
    //        bugBomb.transform.localPosition = Vector3.zero;
    //        bugBomb.transform.localRotation = Quaternion.identity;
    //        bugBombQueue.Enqueue(bugBomb);
    //    }
    //}
}