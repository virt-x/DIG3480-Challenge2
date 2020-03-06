using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public GameObject player, cameraLimitL, cameraLimitR;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x - offset.x, player.transform.position.y - offset.y, offset.z);
        if (transform.position.x < cameraLimitL.transform.position.x)
        {
            transform.position = new Vector3(cameraLimitL.transform.position.x, transform.position.y, offset.z);
        }
        if (transform.position.y < cameraLimitL.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, cameraLimitL.transform.position.y, offset.z);
        }
        if (transform.position.x > cameraLimitR.transform.position.x)
        {
            transform.position = new Vector3(cameraLimitR.transform.position.x, transform.position.y, offset.z);
        }
        if (transform.position.y > cameraLimitR.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, cameraLimitR.transform.position.y, offset.z);
        }
    }
}
