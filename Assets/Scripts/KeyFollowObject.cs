 using UnityEngine;
 using System.Collections;
 
 public class KeyFollowObject : MonoBehaviour {
    public Vector3 offset;
    public Transform lookAt;
    private Camera cam;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
    }
    
    // Update is called once per frame
    public void Update() {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);
        if (transform.position != pos) {
            transform.position = pos;
        }
    }
 }