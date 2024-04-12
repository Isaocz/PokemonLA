using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextBlingBling : MonoBehaviour
{
    private Text UIText;
    private void Awake()
    {
        UIText = GetComponent<Text>();
    }

    void Start()
    {
        Sequence sq = DOTween.Sequence();
        //��һ��������ʾ͸����  ��Χ��[0,1] ,����һ�������Ǿ���ʱ��,�������е���һ������ֵ
        sq.Append(UIText.DOFade(0.2f, 1.5f));
        sq.Append(UIText.DOFade(0.9f, 1));
        sq.SetLoops(-1);
    }
}

