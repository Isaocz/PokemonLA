using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HardworkShow : MonoBehaviour
{

    public Sprite[] HardworkTypeList = new Sprite[] { };
    public SpriteRenderer HardworkTypeMark;


    /// <summary>
    /// 设置奋斗力增减的文字
    /// </summary>
    /// <param name="Value"> 增减值 </param>
    /// <param name="HardworkType">增加种类 0生命 1攻击 2物防 3特攻 4特防 5攻速 6移速 7幸运 </param>
    public void SetText(float Value, int HardworkType)
    {

        bool isPlus; //增加还是减少
        if (Value >= 0) { isPlus = true; }
        else { isPlus = false; }

        float HardworkValue = Mathf.Abs(Value);
        string floatString = HardworkValue.ToString();
        char[] charArray = floatString.ToCharArray();
        int[] intArray = System.Array.ConvertAll(charArray, c => (int)char.GetNumericValue(c));

        string[] HPHardworkFont = new string[]
        {
            "<sprite=285>",  //0
            "<sprite=286>",  //1
            "<sprite=287>",  //2
            "<sprite=288>",  //3
            "<sprite=289>",  //4
            "<sprite=290>",  //5
            "<sprite=291>",  //6
            "<sprite=292>",  //7
            "<sprite=293>",  //8
            "<sprite=294>",  //9
            "<sprite=295>",  //.
            "<sprite=373>",  //+
            "<sprite=374>"   //-
        };
        string[] AtkHardworkFont = new string[]
        {
            "<sprite=296>",  //0
            "<sprite=297>",  //1
            "<sprite=298>",  //2
            "<sprite=299>",  //3
            "<sprite=300>",  //4
            "<sprite=301>",  //5
            "<sprite=302>",  //6
            "<sprite=303>",  //7
            "<sprite=304>",  //8
            "<sprite=305>",  //9
            "<sprite=306>",  //.
            "<sprite=375>",  //+
            "<sprite=376>"   //-
        };
        string[] DefHardworkFont = new string[]
        {
            "<sprite=307>",  //0
            "<sprite=308>",  //1
            "<sprite=309>",  //2
            "<sprite=310>",  //3
            "<sprite=311>",  //4
            "<sprite=312>",  //5
            "<sprite=313>",  //6
            "<sprite=314>",  //7
            "<sprite=315>",  //8
            "<sprite=316>",  //9
            "<sprite=317>",  //.
            "<sprite=377>",  //+
            "<sprite=378>"   //-
        };
        string[] SpAHardworkFont = new string[]
        {
            "<sprite=318>",  //0
            "<sprite=319>",  //1
            "<sprite=320>",  //2
            "<sprite=321>",  //3
            "<sprite=322>",  //4
            "<sprite=323>",  //5
            "<sprite=324>",  //6
            "<sprite=325>",  //7
            "<sprite=326>",  //8
            "<sprite=327>",  //9
            "<sprite=328>",  //.
            "<sprite=379>",  //+
            "<sprite=380>"   //-
        };
        string[] SpDHardworkFont = new string[]
        {
            "<sprite=329>",  //0
            "<sprite=330>",  //1
            "<sprite=331>",  //2
            "<sprite=332>",  //3
            "<sprite=333>",  //4
            "<sprite=334>",  //5
            "<sprite=335>",  //6
            "<sprite=336>",  //7
            "<sprite=337>",  //8
            "<sprite=338>",  //9
            "<sprite=339>",  //.
            "<sprite=381>",  //+
            "<sprite=382>"   //-
        };
        string[] SpeHardworkFont = new string[]
        {
            "<sprite=340>",  //0
            "<sprite=341>",  //1
            "<sprite=342>",  //2
            "<sprite=343>",  //3
            "<sprite=344>",  //4
            "<sprite=345>",  //5
            "<sprite=346>",  //6
            "<sprite=347>",  //7
            "<sprite=348>",  //8
            "<sprite=349>",  //9
            "<sprite=350>",  //.
            "<sprite=383>",  //+
            "<sprite=384>"   //-
        };
        string[] MoveSpeHardworkFont = new string[]
        {
            "<sprite=351>",  //0
            "<sprite=352>",  //1
            "<sprite=353>",  //2
            "<sprite=354>",  //3
            "<sprite=355>",  //4
            "<sprite=356>",  //5
            "<sprite=357>",  //6
            "<sprite=358>",  //7
            "<sprite=359>",  //8
            "<sprite=360>",  //9
            "<sprite=361>",  //.
            "<sprite=385>",  //+
            "<sprite=386>"   //-
        };
        string[] LuckHardworkFont = new string[]
        {
            "<sprite=362>",  //0
            "<sprite=363>",  //1
            "<sprite=364>",  //2
            "<sprite=365>",  //3
            "<sprite=366>",  //4
            "<sprite=367>",  //5
            "<sprite=368>",  //6
            "<sprite=369>",  //7
            "<sprite=370>",  //8
            "<sprite=371>",  //9
            "<sprite=372>",  //.
            "<sprite=387>",  //+
            "<sprite=388>"   //-
        };

        string debug = null;
        for (int i = 0; i < intArray.Length; i++)
        {
            if (intArray[i] == -1) { intArray[i] = 10; }
            debug += intArray[i].ToString() + " ";
        }
        Debug.Log(debug);

        
        string result = null;
        string[] s = HPHardworkFont;
        switch (HardworkType)
        {
            case 0:
                s = HPHardworkFont;
                break;
            case 1:
                s = AtkHardworkFont;
                break;
            case 2:
                s = DefHardworkFont;
                break;
            case 3:
                s = SpAHardworkFont;
                break;
            case 4:
                s = SpDHardworkFont;
                break;
            case 5:
                s = SpeHardworkFont;
                break;
            case 6:
                s = MoveSpeHardworkFont;
                break;
            case 7:
                s = LuckHardworkFont;
                break;
        }

        if (isPlus) { result += s[11]; }
        else { result += s[12]; }
        for (int i = 0; i < charArray.Length; i++)
        {
            if (intArray[i] >= 0 && intArray[i] <= 11) {
                result += s[intArray[i]];
            }
        }

        HardworkTypeMark.sprite = HardworkTypeList[HardworkType];
        transform.GetChild(0).GetComponent<TextMeshPro>().text = result;
        Destroy(gameObject,1.5f);
        
    }

    float Timer;
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
    }
}
