using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkill : MonoBehaviour
{
    public Color originalColor;
    public Color targetColor;
    public float transitionDuration = 1f;

    private float transitionTimer = 0f;
    private Renderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.material.color = originalColor;
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        if (transitionTimer <= transitionDuration)
        {
            float t = transitionTimer / transitionDuration;
            meshRenderer.material.color = Color.Lerp(originalColor, targetColor, t);
            transitionTimer += Time.deltaTime;
        }
    }
}
