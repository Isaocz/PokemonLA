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
        //第一个参数表示透明度  范围是[0,1] ,后面一个参数是经历时间,可以自行调节一个合适值
        sq.Append(UIText.DOFade(0.2f, 1.5f));
        sq.Append(UIText.DOFade(0.9f, 1));
        sq.SetLoops(-1);
    }
}

