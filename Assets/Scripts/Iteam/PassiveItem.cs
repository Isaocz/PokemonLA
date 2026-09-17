using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : Item
{
    public int PassiveItemIndex;

    // Ground display only: keep inventory sprites static and legible.
    private SpriteRenderer rewardSprite;
    private Color rewardBaseColor;
    private void OnEnable()
    {
        if (PassiveItemIndex != 56 && PassiveItemIndex != 57) return;
        rewardSprite = GetComponent<SpriteRenderer>();
        if (rewardSprite == null) return;
        rewardBaseColor = rewardSprite.color;
        StartCoroutine(RewardGlow());
    }
    private IEnumerator RewardGlow()
    {
        while (true)
        {
            float glow = (Mathf.Sin(Time.time * 3f) + 1f) * 0.5f;
            rewardSprite.color = Color.Lerp(rewardBaseColor,
                new Color(rewardBaseColor.r, rewardBaseColor.g * 0.78f, rewardBaseColor.b, rewardBaseColor.a), glow);
            yield return null;
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if (rewardSprite != null) rewardSprite.color = rewardBaseColor;
    }
}
