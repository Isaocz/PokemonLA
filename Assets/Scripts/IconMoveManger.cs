using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMoveManger : MonoBehaviour
{
    List<Image> IconList = new List<Image> { };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image l = transform.GetChild(i).GetComponent<Image>();
            if (l != null)
            {
                IconList.Add(l);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < IconList.Count; i++)
        {
            IconList[i].rectTransform.position += Vector3.down * 5 * Time.deltaTime + Vector3.right * 7.5f * Time.deltaTime;
            if (IconList[i].rectTransform.anchoredPosition.x > 1650)
            {

                IconList[i].rectTransform.anchoredPosition = new Vector3( -1650 , IconList[i].rectTransform.anchoredPosition.y, 0);
                IconList[i].GetComponent<LittleIcon>().ChangeSprite();
            }
            if (IconList[i].rectTransform.anchoredPosition.y < -1500)
            {

                IconList[i].rectTransform.anchoredPosition = new Vector3(IconList[i].rectTransform.anchoredPosition.x, 1500, 0);
                IconList[i].GetComponent<LittleIcon>().ChangeSprite();
            }
        }
    }
}
