using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//一般敌人的移动残影
public class NormalEmptyShadow : MonoBehaviour
{
    /// <summary>
    /// 残影的所有图像名单
    /// </summary>
    public List<SpriteRenderer> spriteRDList;
    /// <summary>
    /// 残影消失的时间
    /// </summary>
    public float DisappearingSpeed;
    /// <summary>
    /// 残影的渲染obj
    /// </summary>
    SpriteRenderer SpriteChildGOBJ;
    

    /// <summary>
    /// 设置残影
    /// </summary>
    /// <param name="disappearingSpeed"></param>
    /// <param name="spriteList"></param>
    public void SetNormalEmptyShadow(float disappearingSpeed , List<SpriteRenderer> spriteList , Color color)
    {
        DisappearingSpeed = disappearingSpeed;

        //获取第一个子对象 作为渲染器game obj 并放入到spriteRDList当中
        SpriteChildGOBJ = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteChildGOBJ.sprite = spriteList[0].sprite;
        SpriteChildGOBJ.sortingOrder = spriteList[0].sortingOrder - 1;
        SpriteChildGOBJ.color = color;
        spriteRDList.Add(SpriteChildGOBJ);

        //如果有多个残影 生成需要的残影  并放入到spriteRDList当中
        if (spriteList.Count != 1) {
            for (int i = 1; i < spriteList.Count; i++)
            {
                SpriteRenderer s = Instantiate(SpriteChildGOBJ.gameObject, transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
                s.sprite = spriteList[0].sprite;
                s.sortingOrder = spriteList[0].sortingOrder - 1;
                s.color = color;
                spriteRDList.Add(s.GetComponent<SpriteRenderer>());
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (spriteRDList.Count != 0)
        {
            for (int i = 0; i < spriteRDList.Count; i++)
            {
                spriteRDList[i].color -= new Color(0, 0, 0, Time.deltaTime * 1.7f);
            }
            if (spriteRDList[0].color.a < 0.05f)
            {
                Destroy(gameObject);
            }
        }


    }
}
