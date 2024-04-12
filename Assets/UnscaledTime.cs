using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTime : MonoBehaviour
{
    Material material;
    TrailRenderer t;

    private void Awake()
    {
        material = transform.GetComponent<Image>().material;
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
        Debug.Log(material.GetFloat("_UnscaledTime"));
    }

    private void Start()
    {
        
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
        Debug.Log(material.GetFloat("_UnscaledTime"));
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
        Shader.SetGlobalFloat("_UnScaleTime", Time.unscaledTime);
        Debug.Log("xxx");
    }
}
