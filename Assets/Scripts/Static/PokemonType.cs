using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PokemonType : MonoBehaviour
{

    // Start is called before the first frame update
    public enum TypeEnum
    {
        No = 0,
        Normal = 1,
        Fighting = 2,
        Flying = 3,
        Poison = 4,
        Ground = 5,
        Rock = 6,
        Bug = 7,
        Ghost = 8,
        Steel = 9,
        Fire = 10,
        Water = 11,
        Grass = 12,
        Electric = 13,
        Psychic = 14,
        Ice = 15,
        Dragon = 16,
        Dark = 17,
        Fairy = 18,
        IgnoreType = 19,
    }


    //type[行][列] 用行属性攻击列属性时的补正
    public static float[][] TYPE =new float[][]{ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },  //  无0

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 0.64f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },  //  普1
          
                                          new float[] { 1.0f, 1.2f, 1.0f, 0.8f, 0.8f, 1.0f, 1.2f, 0.8f, 0.64f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.2f, 1.0f, 1.2f, 0.8f },  //  斗2

                                          new float[] { 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 0.8f, 1.2f, 1.0f, 0.8f, 1.0f, 1.0f, 1.2f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },  //  飞3

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 0.8f, 0.8f, 1.0f, 0.8f, 0.64f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f },  //  毒4

                                          new float[] { 1.0f, 1.0f, 1.0f, 0.64f, 1.2f, 1.0f, 1.2f, 0.8f, 1.0f, 1.2f, 1.2f, 1.0f, 0.8f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },  //  地5

                                          new float[] { 1.0f, 1.0f, 0.8f, 1.2f, 1.0f, 0.8f, 1.0f, 1.2f, 1.0f, 0.8f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f },  //  岩6

                                          new float[] { 1.0f, 1.0f, 0.8f, 0.8f, 0.8f, 1.0f, 1.0f, 1.0f, 0.8f, 0.8f, 0.8f, 1.0f, 1.2f, 1.0f, 1.2f, 1.0f, 1.0f, 1.2f, 0.8f },  //  虫7

                                          new float[] { 1.0f, 0.64f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 0.8f, 1.0f },  //  鬼8
     
                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 0.8f, 0.8f, 0.8f, 1.0f, 0.8f, 1.0f, 1.2f, 1.0f, 1.0f, 1.2f },  //  钢9

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.2f, 1.0f, 1.2f, 0.8f, 0.8f, 1.2f, 1.0f, 1.0f, 1.2f, 0.8f, 1.0f, 1.0f },  //  火10

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.2f, 1.0f, 1.0f, 1.0f, 1.2f, 0.8f, 0.8f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f },  //  水11

                                          new float[] { 1.0f, 1.0f, 1.0f, 0.8f, 0.8f, 1.2f, 1.2f, 0.8f, 1.0f, 0.8f, 0.8f, 1.2f, 0.8f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f },  //  草12

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 0.64f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 0.8f, 0.8f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f },  //  电13
     
                                          new float[] { 1.0f, 1.0f, 1.2f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f, 0.64f, 1.0f },  //  超14

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 0.8f, 0.8f, 0.8f, 1.2f, 1.0f, 1.0f, 0.8f, 1.2f, 1.0f, 1.0f },  //  冰15

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 0.64f },  //  龙16

                                          new float[] { 1.0f, 1.0f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.0f, 1.0f, 0.8f, 0.8f },  //  恶17

                                          new float[] { 1.0f, 1.0f, 1.2f, 1.0f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 0.8f, 0.8f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.2f, 1.2f, 1.0f },  //  仙18

                                          new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }}; //  属性穿透19


    public static Vector4[] TypeColor = {
        new Vector4(0,0,0,0.5882353f),                                    //无  00
        new Vector4(0.7176471f, 0.6901961f, 0.6666667f, 0.5882353f),      //普通01
        new Vector4(0.4433962f, 0.2580442f, 0.08993414f, 0.7803922f),     //格斗02
        new Vector4(0.24644f,0.4380967f,0.735849f,0.5882353f),            //飞行03
        new Vector4(0.5660378f,0.08810966f,0.4045756f,0.5882353f),        //毒  04
        new Vector4(0.764151f,0.6169877f,0.2703364f,0.7058824f),          //地面05
        new Vector4(0.7075472f, 0.5511783f,0.2636614f,0.7372549f ),       //岩石06
        new Vector4(0.7019608f,0.7372549f,0.2705882f,0.5882353f),         //虫  07
        new Vector4(0.2357066f,0.1169455f,0.3396226f,0.627451f ),         //幽灵08
        new Vector4(0.5f,0.5f,0.5f,0.5882353f),                           //钢  09
        new Vector4(1,0.2308041f,0.08962262f,0.6862745f),                 //火  10
        new Vector4(0.2705882f,0.6196079f,0.9803922f,0.5882353f),         //水  11
        new Vector4(0.4823529f,0.7137255f,0.1960784f,0.5882353f),         //草  12
        new Vector4(0.9921569f,0.9372549f,0.1529412f,0.5882353f),         //电  13
        new Vector4(0.8301887f,0.1683873f,0.4182985f,0.5882353f),         //超能14
        new Vector4(0.509804f,0.7294118f,1,0.5882353f),                   //冰  15
        new Vector4(0,0.04001015f,0.5137255f,0.682353f),                  //龙  16
        new Vector4(0.4056604f,0.3041446f,0.2851104f,0.8313726f),         //恶  17
        new Vector4(0.764151f,0.4649787f,0.6818696f,0.5882353f) };        //妖精18     


    public static Vector4[] TypeColor2 = {
        new Vector4(0,0,0,0.5882353f),                                    //无  00
        new Vector4(0.7176471f, 0.6901961f, 0.6666667f, 0.5882353f),      //普通01
        new Vector4(1, 0.75f, 0.175f, 0.7803922f),     //格斗02
        new Vector4(0.678f,0.643f,0.91f,0.5882353f),            //飞行03
        new Vector4(0.875f,0.385f,0.733f,0.5882353f),        //毒  04
        new Vector4(0.81f,0.702f,0.51f,0.7058824f),          //地面05
        new Vector4(0.68f, 0.54f,0.2636614f,0.702f ),       //岩石06
        new Vector4(0.9f,0.945f,0.506f,0.5882353f),         //虫  07
        new Vector4(0.92f,0.31f,0.43f,0.627451f ),         //幽灵08
        new Vector4(0.765f,0.64f,0.5f,0.53f),                           //钢  09
        new Vector4(1,0.95f,0,0.6862745f),                 //火  10
        new Vector4(0.54f,0.75f,0.8903922f,0.5882353f),         //水  11
        new Vector4(0.69f,0.87f,0.54f,0.5882353f),         //草  12
        new Vector4(0.96f,0.93f,0.63f,0.5882353f),         //电  13
        new Vector4(0.86f,0.50f,0.655f,0.5882353f),         //超能14
        new Vector4(0.495f,0.765f,0.895f,0.5882353f),                   //冰  15
        new Vector4(0.845f,0.553f,0.5137255f,0.682353f),                  //龙  16
        new Vector4(1,0.498f,0.53104f,0.8313726f),         //恶  17
        new Vector4(0.823f,0.87f,0.95f,0.5882353f) };        //妖精18     



    public static Color[] TeraTypeColor = {
            new Vector4(0,0,0,1.0f),                                    //无  00
            new Vector4(1,1, 1, 1.0f),      //普通01
            new Vector4(0.8490566f,0.3308623f, 0.09211465f, 1.0f),     //格斗02
            new Vector4(0.2669989f,0.3516708f,0.754717f,1.0f),            //飞行03
            new Vector4(0.5773619f,0.2232111f,0.8301887f,1.0f),        //毒  04
            new Vector4(0.6226415f,0.4680493f,0.08517268f,1.0f),          //地面05
            new Vector4(0.4528302f,0.4463697f,0.1901032f,1.0f ),       //岩石06
            new Vector4(0.5148496f,0.7735849f,0.4561232f,1.0f),         //虫  07
            new Vector4(0.3214015f,0.1674528f,0.6698113f,1.0f ),         //幽灵08
            new Vector4(0.378471f,0.5036707f,0.5943396f,1.0f),                           //钢  09
            new Vector4(0.7830189f,0.2277878f,0.08495018f,1.0f),                 //火  10
            new Vector4(0.2079032f,0.2384928f,0.6037736f,1.0f),         //水  11
            new Vector4(0.2308206f,0.8584906f,0.3941831f,1.0f),         //草  12
            new Vector4(0.9339623f,0.9234161f,0.4537647f,1.0f),         //电  13
            new Vector4(0.7488047f,0.1703008f,0.8396226f,1.0f),         //超能14
            new Vector4(0,0.7069082f,1,1.0f),                   //冰  15
            new Vector4(0.3939791f,0.1981132f,0.7924528f,1.0f),                  //龙  16
            new Vector4(0.9528302f,0.2651744f,0.2719075f,1.0f),         //恶  17
            new Vector4(0.9433962f,0.1913492f,0.5875216f,1.0f) };        //妖精18     


    public static string[] TypeChineseName = {
        "无属性","普通","格斗","飞行","毒","地面","岩石","虫","幽灵","钢","火","水","草","电","超能","冰","龙","恶","妖精","真实伤害"};



    /// <summary>
    /// 特性的构造体
    /// </summary>
    public struct Ability
    {
        public int AbilityIndex;
        public string AbilityChineseName;
        public string AbilityEngName;
        public string AbilityDescribe01;
        public string AbilityDescribe02;
        public Color AbilityToggleColor;
        public Color AbilityToggleTextColor;
        public Color AbilityToggleTextColorHL;

        public Ability(int abilityIndex, string abilityChineseName, string abilityEngName, string abilityDescribe01, string abilityDescribe02, Color abilityToggleColor, Color abilityToggleTextColor, Color abilityToggleTextColorHL)
        {
            AbilityIndex = abilityIndex;
            AbilityChineseName = abilityChineseName;
            AbilityEngName = abilityEngName;
            AbilityDescribe01 = abilityDescribe01;
            AbilityDescribe02 = abilityDescribe02;
            AbilityToggleColor = abilityToggleColor;
            AbilityToggleTextColor = abilityToggleTextColor;
            AbilityToggleTextColorHL = abilityToggleTextColorHL;
        }
    }

    public static Ability[] AbilityList = new Ability[]
    {
        new Ability( 0 , "无特性"     , "None"            , "无特性"                                                             , "无特性"                                                     , Color.white                                             , Color.black                                               , new Color( 0.6320754f, 0.6320754f, 0.6320754f, 1.0f )    ),
        new Ability( 1 , "迟钝"       , "Oblivious"       , "当异常状态的进度被连续累积时，两次累积之间的时间间隔巨幅变长。"     , "并且当使用了接触类招式后，一段时间异常状态的进度不会累积。" , new Color( 0.7843137f, 0.6039216f, 0.4823529f, 1.0f )   , new Color( 0.509434f, 0.252314f, 0.252314f, 1.0f )        , Color.white   ),
        new Ability( 2 , "雪隐"       , "Snow Cloak"      , "使用了冰属性技能后，一段时间内大幅提升移动速度。"                   , ""                                                           , new Color( 0.681f, 0.809f, 1.0f, 1.0f )                 , new Color( 0.1137255f, 0.3137255f, 0.4745098f, 1.0f )     , Color.white ),
        new Ability( 3 , "厚脂肪"     , "Thick Fat"       , ""                                                                   , ""                                                           , new Color( 0.9490196f, 1.0f, 0.8078431f, 1.0f )         , new Color( 0.4339623f, 0.4221423f, 0.3459416f, 1.0f )     , new Color( 0.7697138f, 0.7735849f, 0.5509968f, 1.0f )  ),
        new Ability( 4 , "叶子防守"   , "Leaf Guard"      , "晴天或者处于草丛中时异常状态的进度不会累积。"                       , ""                                                           , new Color( 0.6235294f, 0.7490196f, 0.4705882f, 1.0f )   , new Color( 0.1658064f, 0.4339623f, 0.3206199f, 1.0f )     , Color.white  ),
        new Ability( 5 , "甜幕"       , "Sweet Veil"      , ""                                                                   , ""                                                           , new Color( 0.9529412f, 0.5529412f, 0.8313726f, 1.0f )   , new Color( 0.5188679f, 0.2031417f, 0.4229119f, 1.0f )     , Color.white   ),
        new Ability( 6 , "女王的威严" , "Queenly Majesty" , "向对手施加威慑力，还未受伤的敌人无法对你施加伤害！"                 , ""                                                           , new Color( 0.6235294f, 0.7490196f, 0.4705882f, 1.0f )   , new Color( 0.1658064f, 0.4339623f, 0.3206199f, 1.0f )     , Color.white   ),
        new Ability( 7 , "逃跑"       , "Run Away"        , "移动速度变得更快。"                                                 , ""                                                           , new Color( 0.9490196f, 1.0f, 0.8078431f, 1.0f )         , new Color( 0.4339623f, 0.4221423f, 0.3459416f, 1.0f )     , new Color( 0.7697138f, 0.7735849f, 0.5509968f, 1.0f )    ),
        new Ability( 8 , "适应力"     , "Adaptability"    , "与自身同属性的招式威力会变得更高。"                                 , ""                                                           , new Color( 0.9490196f, 1.0f, 0.8078431f, 1.0f )         , new Color( 0.4339623f, 0.4221423f, 0.3459416f, 1.0f )     , new Color( 0.7697138f, 0.7735849f, 0.5509968f, 1.0f )    ),
        new Ability( 9 , "危险预知"   , "Anticipation"    , ""                                                                   , ""                                                           , new Color( 0.735849f, 0.3297437f, 0.3840066f, 1.0f )    , new Color( 0.4339623f, 0.1535244f, 0.1915003f, 1.0f )     , Color.white ),
        new Ability(10 , "迷人之躯"   , "Cute Charm"      , "当接触类招式命中后，有概率累计目标的着迷进度。"                     , ""                                                           , new Color( 0.9529412f, 0.5529412f, 0.8313726f, 1.0f )   , new Color( 0.5188679f, 0.2031417f, 0.4229119f, 1.0f )     , Color.white  ),
        new Ability(11 , "妖精皮肤"   , "Pixilate"        , ""                                                                   , ""                                                           , new Color( 0.9529412f, 0.5529412f, 0.8313726f, 1.0f )   , new Color( 0.5188679f, 0.2031417f, 0.4229119f, 1.0f )     , Color.white  ),
        new Ability(12 , "激流"       , "Torrent"         , "HP减少的时候，水属性的招式威力会提高。"                             , ""                                                           , new Color( 0.3019608f, 0.5764706f, 0.8588235f, 1.0f )   , new Color( 0.119927f, 0.2871757f, 0.4622642f, 1.0f )      , Color.white  ),
        new Ability(13 , "好胜"       , "Competitive"     , ""                                                                   , ""                                                           , new Color( 0.9490196f, 1.0f, 0.8078431f, 1.0f )         , new Color( 0.4339623f, 0.4221423f, 0.3459416f, 1.0f )     , new Color( 0.7697138f, 0.7735849f, 0.5509968f, 1.0f )     ),
        new Ability(14 , "恒净之躯"   , "Clear Body"      , "能力降低时，能力的降低值大幅变少。"                                 , ""                                                           , new Color( 0.7058824f, 0.7058824f, 0.7058824f, 1.0f )   , new Color( 0.3584906f, 0.3584906f, 0.3584906f, 1.0f )     , Color.white  ),
        new Ability(15 , "轻金属"     , "Light Metal"     , ""                                                                   , ""                                                           , new Color( 0.7058824f, 0.7058824f, 0.7058824f, 1.0f )   , new Color( 0.3584906f, 0.3584906f, 0.3584906f, 1.0f )     , Color.white ),
        new Ability(16 , "同步"       , "Synchronize"     , "当处于麻痹，烧伤，中毒状态时，攻击敌人有概率把异常状态传染给敌人。" , ""                                                           , new Color( 1.0f, 0.3529412f, 0.6862745f, 1.0f )         , new Color( 0.509434f, 0.07929869f, 0.300026f, 1.0f )      , Color.white  ),
        new Ability(17 , "精神力"     , "Inner Focus"     , ""                                                                   , ""                                                           , new Color( 1.0f, 0.3529412f, 0.6862745f, 1.0f )         , new Color( 0.1960784f, 0.1960784f, 0.1960784f, 1.0f )     , Color.white ),
        new Ability(18 , "魔法镜"     , "Magic Bounce"    , ""                                                                   , ""                                                           , new Color( 1.0f, 0.3529412f, 0.6862745f, 1.0f )         , new Color( 0.1960784f, 0.1960784f, 0.1960784f, 1.0f )     , Color.white  ),
    };

}


