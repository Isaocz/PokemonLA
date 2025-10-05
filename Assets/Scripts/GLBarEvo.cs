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
        // 闪光效果
        // 这里可以使用UIEffect插件的相关代码

        // 渐隐旧的Sprite
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

        // 替换Sprite
        icon.sprite = newSprite;

        // 渐显新的Sprite
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }
    }
}
