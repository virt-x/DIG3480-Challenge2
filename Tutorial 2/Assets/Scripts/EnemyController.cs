using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform start, end;
    public float speed, startOffset;
    private float startTime, length;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time + startOffset;
        length = Vector2.Distance(start.position, end.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Time.time - startTime) * (speed);
        float fraction = distance / length;
        transform.position = Vector2.Lerp(start.position, end.position, Mathf.PingPong(fraction, 1));
    }
}
