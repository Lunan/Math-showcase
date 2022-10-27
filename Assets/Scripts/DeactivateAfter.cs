using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfter : MonoBehaviour
{
    public float timeToDeactivate;

    private float startedAt;

    private void OnEnable()
    {
        startedAt = Time.realtimeSinceStartup;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > startedAt + timeToDeactivate)
        {
            gameObject.SetActive(false);
        }
    }
}
