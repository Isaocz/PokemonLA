using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//һ����˵��ƶ���Ӱ
public class NormalEmptyShadow : MonoBehaviour
{
    /// <summary>
    /// ��Ӱ������ͼ������
    /// </summary>
    public List<SpriteRenderer> spriteRDList;
    /// <summary>
    /// ��Ӱ��ʧ��ʱ��
    /// </summary>
    public float DisappearingSpeed;
    /// <summary>
    /// ��Ӱ����Ⱦobj
    /// </summary>
    SpriteRenderer SpriteChildGOBJ;
    

    /// <summary>
    /// ���ò�Ӱ
    /// </summary>
    /// <param name="disappearingSpeed"></param>
    /// <param name="spriteList"></param>
    public void SetNormalEmptyShadow(float disappearingSpeed , List<SpriteRenderer> spriteList , Color color)
    {
        DisappearingSpeed = disappearingSpeed;

        //��ȡ��һ���Ӷ��� ��Ϊ��Ⱦ��game obj �����뵽spriteRDList����
        SpriteChildGOBJ = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteChildGOBJ.sprite = spriteList[0].sprite;
        SpriteChildGOBJ.sortingOrder = spriteList[0].sortingOrder - 1;
        SpriteChildGOBJ.color = color;
        spriteRDList.Add(SpriteChildGOBJ);

        //����ж����Ӱ ������Ҫ�Ĳ�Ӱ  �����뵽spriteRDList����
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
