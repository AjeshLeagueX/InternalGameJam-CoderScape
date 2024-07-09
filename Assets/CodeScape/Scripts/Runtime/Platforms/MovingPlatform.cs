using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Vector3> points;
    public float speed = 2f;
    public float waitTime = 1f;
    public bool useLocalPosition = false;
    public bool isPingPong = false;

    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private bool forward = true;

    void Start()
    {
        if (points.Count > 0)
        {
            if (useLocalPosition)
            {
                transform.localPosition = points[0];
            }
            else
            {
                transform.position = points[0];
            }
        }
    }

    void Update()
    {
        if (points.Count > 1 && !isWaiting)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        Vector3 targetPosition = useLocalPosition ? points[currentPointIndex] : points[currentPointIndex];
        float step = speed * Time.deltaTime;
        if (useLocalPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }

        if (Vector3.Distance(useLocalPosition ? transform.localPosition : transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(WaitAndMoveToNextPoint());
        }
    }

    private IEnumerator WaitAndMoveToNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        if (isPingPong)
        {
            if (forward)
            {
                currentPointIndex++;
                if (currentPointIndex >= points.Count)
                {
                    currentPointIndex = points.Count - 2;
                    forward = false;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    forward = true;
                }
            }
        }
        else
        {
            currentPointIndex = (currentPointIndex + 1) % points.Count;
        }

        isWaiting = false;
    }

    private void OnDrawGizmos()
    {
        if (points != null && points.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (Vector3 point in points)
            {
                Vector3 drawPoint = useLocalPosition ? transform.TransformPoint(point) : point;
                Gizmos.DrawSphere(drawPoint, 0.2f);
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 startPoint = useLocalPosition ? transform.TransformPoint(points[i]) : points[i];
                Vector3 endPoint = useLocalPosition ? transform.TransformPoint(points[(i + 1) % points.Count]) : points[(i + 1) % points.Count];
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}
