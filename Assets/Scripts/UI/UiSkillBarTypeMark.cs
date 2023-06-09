using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSkillBarTypeMark : MonoBehaviour
{
    public Image TypeMark;
    public Sprite None;
    public Sprite Nurmal;
    public Sprite Fight;
    public Sprite Fly;
    public Sprite Poison;
    public Sprite Ground;
    public Sprite Rock;
    public Sprite Bug;
    public Sprite Ghost;
    public Sprite Steel;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Grass;
    public Sprite Electric;
    public Sprite Psychic;
    public Sprite Ice;
    public Sprite Dragon;
    public Sprite Dark;
    public Sprite Fairy;


    // Start is called before the first frame update
    public void ChangeTypeMark(int type)
    {
        switch (type)
        {
            case 0:
                TypeMark.sprite = None;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 1:
                TypeMark.sprite = Nurmal;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 2:
                TypeMark.sprite = Fight;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 3:
                TypeMark.sprite = Fly;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 4:
                TypeMark.sprite = Poison;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 5:
                TypeMark.sprite = Ground;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 6:
                TypeMark.sprite = Rock;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 7:
                TypeMark.sprite = Bug;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 8:
                TypeMark.sprite = Ghost;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 9:
                TypeMark.sprite = Steel;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 10:
                TypeMark.sprite = Fire;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 11:
                TypeMark.sprite = Water;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 12:
                TypeMark.sprite = Grass;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 13:
                TypeMark.sprite = Electric;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 14:
                TypeMark.sprite = Psychic;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 15:
                TypeMark.sprite = Ice;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 16:
                TypeMark.sprite = Dragon;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 17:
                TypeMark.sprite = Dark;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
            case 18:
                TypeMark.sprite = Fairy;
                TypeMark.color = Type.TypeColor[type] - new Vector4(0.3f, 0.3f, 0.3f, -1);
                break;
        }
            

    }
}
