using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 _cameraOffset;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float smoothFactor = 0.5f;

    [SerializeField]
    [Range(0.01f, 5.0f)]
    private float rotatingSpeed = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = playerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        LookAtTarget();
    }

    void LookAtTarget(){
        Vector3 lookPos = playerTransform.position - transform.position;
        Quaternion qt = Quaternion.LookRotation(lookPos, playerTransform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, qt, rotatingSpeed * Time.deltaTime);
    }
}
