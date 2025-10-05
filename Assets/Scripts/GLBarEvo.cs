using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GLBarEvo : MonoBehaviour
{
    public Image icon;
    public Sprite newSprite;
    public float flashDuration = 0.5f;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(PlayUpgradeEffect());
    }

    private IEnumerator PlayUpgradeEffect()
    {
        // ����Ч��
        // �������ʹ��UIEffect�������ش���

        // �����ɵ�Sprite
        CanvasGroup canvasGroup = icon.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = icon.gameObject.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsedTime / fadeDuration);
            yield return null;
        }

        // �滻Sprite
        icon.sprite = newSprite;

        // �����µ�Sprite
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }
    }
}
