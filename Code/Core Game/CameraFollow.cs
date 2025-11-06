using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    public float smoothSpeed = .125f;
    public Vector3 offset;
    float cameraPosZ = -10;

    private void Start()
    {
        player = FindObjectOfType<Boss>().gameObject.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 posToFollow = new Vector3(player.transform.position.x, player.transform.position.y, cameraPosZ) + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, posToFollow, smoothSpeed);

            transform.position = smoothedPos;
        }
        else
        {
            Camera.main.transform.position = Vector3.zero;
        }

    }
}
