using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        EnsureCameraAssigned();
    }

    void FixedUpdate()
    {
        if (cam == null)
        {
            EnsureCameraAssigned();
            if (cam == null) return;
        }

        float distance = cam.transform.position.x * parallaxEffect; //0 = move w cam | 1 = wont move | 0.5 = half

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }

    private void EnsureCameraAssigned()
    {
        if (cam == null && Camera.main != null)
        {
            cam = Camera.main.gameObject;
        }
    }
}