using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// 对话字符串的类 包含字符和表情
/// </summary>
public class DialogString
{
    //表情
    public enum Face
    {
        Angry,         //生气
        Crying,        //哭泣
        Determined,    //坚定
        Dizzy,         //晕倒
        Happy,         //开心
        Inspired,      //受到启发
        Joyous,        //欢庆
        Normal,        //普通
        Pain,          //痛苦
        Sad,           //伤心
        Shouting,      //震惊
        Sigh,          //叹气
        Special1,      //特殊1
        Special2,      //特殊2
        Stunned,       //目瞪口呆
        Surprised,     //惊讶
        TearyEyed,     //泪眼汪汪
        Worried,       //担心
        Sleep,         //睡觉
    }

    public string DialogueString;
    public Face DialogueFace;
    public string Value;//事件节点的值

    public DialogString()
    {
        DialogueString = "";
        DialogueFace = Face.Normal;
    }

    public DialogString( string s , Face f )
    {
        DialogueString = s;
        DialogueFace = f;
    }
}

/// <summary>
/// NPC对话分歧节点
/// </summary>
public class DialogNode
{
    public string dialogueText;
    public List<DialogOption> options;
    public DialogNode nextNode;

    public DialogNode(string dialogueText, List<DialogOption> options, DialogNode nextNode)
    {
        this.dialogueText = dialogueText;
        this.options = options;
        this.nextNode = nextNode;
    }

    public bool CheckConditions(NPCDialogState gameState)
    {
        // 根据游戏状态检查条件
        return true; // 示例：总是返回true
    }
}

/// <summary>
/// NPC对话的选项
/// </summary>
public class DialogOption
{
    //该选项会改变什么NPC对话情况

    public string optionText;
    public DialogNode nextNode;
    /// <summary>
    /// 选项会修改的对话分支情况，第一位为情况，第二位为值
    /// </summary>
    public Vector2Int OptionBool;


    public DialogOption(string optionText, DialogNode nextNode , Vector2Int OptionBool)
    {
        this.optionText = optionText;
        this.nextNode = nextNode;
        this.OptionBool = OptionBool;
        
    }

    public bool CheckConditions(NPCDialogState gameState)
    {
        // 根据游戏状态检查条件
        return true; // 示例：总是返回true
    }


}


public static class DialogInfo
{

    //分支选项不可以超过10个字，每个节点不可超过6个分支选项

    //木屋修建老爹的对话列表
    public static List<DialogNode> WHDialog = new List<DialogNode>
    {
        /*0*/new DialogNode("欢迎光临铁骨建筑公司！", null, null),
        /*1*/new DialogNode("本公司承包了冒险小镇的开发项目，\n唔，冒险团可以支付冒险点数来委托我们开发小镇！\n请问有什么事情吗？", null ,null ),
        /*2*/new DialogNode("原来你就是新来的冒险团的负责人啊，唔，欢迎欢迎！\n有什么想要开发的项目吗？", null,null),
        /*3*/new DialogNode("唔，只是路过啊，\n唔，那看看吧，可不要乱动啊。", null,null ),
        /*4*/new DialogNode("_打开面板", null,null ),
        /*5*/new DialogNode("暂时还没想好啊，唔，可以去问问镇民们的开发意愿哦。", null,null ),
        /*6*/new DialogNode("收到委托！本公司会尽快动工！\n唔，不过工期可能要一段时间，请耐心等待！", null,null ),

        /*7*/new DialogNode("唔，有正在忙的委托哦，\n等当前项目完成后再开始新项目吧。", null,null ),

        /*8*/new DialogNode("唔，你又来了，有什么事情吗？", null,null ),

        /*9*/new DialogNode("之前的委托已经完成了！\n快去看看吧！", null,null ),
        /*10*/new DialogNode("唔，冒险辛苦了。\n需要委托开发项目吗？", null,null ),



    };

    //木屋修建老爹的选项列表
    public static List<DialogOption> WHOption = new List<DialogOption>
    {
        /*0*/new DialogOption("我是冒险团负责人", null , new Vector2Int(1,1)),
        /*1*/new DialogOption("我只是看看", null , new Vector2Int(1,0))
    };





    public static List<DialogNode> LucarioDialog = new List<DialogNode>
    {
        /*0*/new DialogNode("你好，这里是冒险公会的冒险镇分会。", null, null),
        /*1*/new DialogNode("我是本分会的分会长，请问有什么事情吗？", null, null),
        /*2*/new DialogNode("不要用问题回答问题！", null, null),
        /*3*/new DialogNode("路过的话你可以在这里休息一会，\n不过你也看到了，本镇还在开拓阶段。", null, null),
        /*4*/new DialogNode("抱歉没有可供休息的场所，不介意的话就在这里休息一会吧。", null, null),
        /*5*/new DialogNode("原来你是上午说的会来报道的新冒险团负责人啊，\n一路走来辛苦了吧，", null, null),
        /*6*/new DialogNode("本镇十分欢迎你的入住，\n期待你在本镇能够快乐的进行冒险活动。", null, null),
        /*7*/new DialogNode("看起来你还是个新手，\n有什么问题吗？", null, null),


        /*8*/new DialogNode("额，你连这个都不知道吗。。。\n看来有必要向冒险学校反映一下了。", null, null),
        /*9*/new DialogNode("简单来说呢，总的来说呢，\n冒险就是。。。嗯。。。", null, null),
        /*10*/new DialogNode("冒险就是冒险！", null, null),
        /*11*/new DialogNode("具体来说，从小镇的北部出发，可以开始冒险，\n然后可以派和冒险团缔结契约的冒险家出去探险，", null, null),
        /*12*/new DialogNode("不过说起来你的冒险团目前只有你一个人，\n所以你只能自己上了，", null, null),
        /*13*/new DialogNode("冒险开始后，冒险家会遇到各种各样的敌人，\n说是敌人其实只是野生的宝可梦，", null, null),
        /*14*/new DialogNode("擅自去别人家冒险别人会生气也很理所当然吧。", null, null),
        /*15*/new DialogNode("不断地前往新的地区，探明本地区更多的未知区域，\n就是冒险的主要内容。", null, null),
        /*16*/new DialogNode("冒险的路上，不止有敌人，\n还会不断邂逅伙伴或者宝物。", null, null),
        /*17*/new DialogNode("完成冒险后可以得到工会奖励的冒险点数，\n冒险点数可以用来支付冒险镇的各种消费哦。", null, null),
        /*18*/new DialogNode("额，还真是废了一番口舌。\n还有什么问题吗？", null, null),

        /*19*/new DialogNode("冒险镇是这个小镇名字，\n真是个简明扼要的名字。", null, null),
        /*20*/new DialogNode("前一段时间，总公会展开了冒险大开发活动，\n在大量冒险家的探索下，发现了这个神秘的地区。", null, null),
        /*21*/new DialogNode("从地图上来说，该地区占地并不是很大，\n但是只要进入，内部就好像无边无尽一样，永远也探索不完。", null, null),
        /*22*/new DialogNode("总公会认为这里存在传说级特异情况，\n于是决定在该区域的外围建立一座前哨站，", null, null),
        /*23*/new DialogNode("也就是本镇，冒险镇了，\n因此按理来说冒险镇可说是总工会一次非常重要的特殊行动。", null, null),
        /*24*/new DialogNode("不过。。。\n<color=red>这个行动的负责人</color>。。。\n。。。", null, null),
        /*25*/new DialogNode("唉。。。可以说是非常非常不靠谱了。。。", null, null),


        /*26*/new DialogNode("不是不靠谱，而是非常非常不靠谱", null, null),
        /*27*/new DialogNode("他自说自话的把一堆资料扔给我们三个人，\n然后抛下了【加油！我还有非常绝密的任务要进行!】\n这种烂话就呼呼大睡了。", null, null),
        /*28*/new DialogNode("看资料的意思应该是让我负责探险和管理，\n老爹负责建设，\n爱管侍小姐负责照顾大家并且经营冒险者奶馆。", null, null),


        /*29*/new DialogNode("老爹现在就在隔壁，是一个经验丰富的修建老匠，\n不过最近很喜欢玩那种开公司的过家家游戏。。。\n他会负责帮助我们建设冒险镇的，不过也要支付给他冒险点数。", null, null),
        /*30*/new DialogNode("爱管侍小姐啊，她是总工会下属的后勤部的天才爱管侍，\n据说曾经得到过料理大赛和饮料大赛的双重金奖\n不过人似乎有点天真，来的路上总是很好奇。", null, null),
        /*31*/new DialogNode("她应该在小镇上思考冒险者奶馆的建造地址吧,\n如果你有什么想法可以跟她谈谈。", null, null),
        /*32*/new DialogNode("我上午看了下资料，因为有很多事情要处理，\n我很难分心于冒险任务了。\n所以我和<color=red>那家伙</color>请求了额外的帮手", null, null),
        /*33*/new DialogNode("。。。嗯。。。虽然有点不礼貌，\n不过看你的样子，<color=red>那家伙</color>还是一如既往的不靠谱啊。。。", null, null),
        /*34*/new DialogNode("看来你还只是萌芽级冒险团，希望你能尽快适应各种危险的任务。", null, null),


        /*35*/new DialogNode("冒险分会主要负责管理冒险者，保障冒险者的后勤，汇总冒险者的信息，\n以及发布冒险任务。", null, null),
        /*36*/new DialogNode("有各种各样的冒险任务，有公会自身发布的，也有悬赏者发布的。\n完成后会收到各种各样的奖励哦。", null, null),


        /*37*/new DialogNode("你作为新来的冒险团负责人，可以镇上的其他人打个招呼。\n也可以马上前往小镇北部开始冒险了，\n这样你可以大概试验下自己的能力，我会根据情况给你分配不同的任务。", null, null),


        /*38*/new DialogNode("我在整理之前冒险家对于本区域的冒险报告，\n看来有必要向冒险学校反映一下宝可梦文的教育问题了。", null, null),

       
        /*39*/new DialogNode("还有什么问题吗？", null, null),
        /*40*/new DialogNode("确定没有问题了吗？", null, null),
        /*41*/new DialogNode("好的，那么祝你冒险愉快。", null, null),

        /*42*/new DialogNode("有什么事情吗？", null, null),


        /*43*/new DialogNode("额。。。找不到吗？", null, null),
        /*44*/new DialogNode("她之前和我说看到了很棒的奶馆选址地，于是就跑到森林里去了。\n会不会是在森林里迷路了。。。", null, null),
        /*45*/new DialogNode("之前嘱咐过她不要跑太远的。。。", null, null),
        /*46*/new DialogNode("。。。。。。", null, null),
        /*47*/new DialogNode("这样吧，我以分会长的身份向你委托第一份任务，\n去找到迷路的爱管侍小姐吧。", null, null),
        /*48*/new DialogNode("虽然有点不像话，\n不过希望你能顺利的完成任务。", null, null),

        /*49*/new DialogNode("那么一路顺风。", null, null),
        
    };


    public static List<DialogOption> LucarioOption = new List<DialogOption>
    {
        /*0*/new DialogOption("我是冒险团负责人", null , new Vector2Int(1,1)),
        /*1*/new DialogOption("只是路过看看", null , new Vector2Int(1,0)),
        /*2*/new DialogOption("你在干什么？", null , new Vector2Int(1,0)),


        /*3*/new DialogOption("什么是冒险？", null , new Vector2Int(1,0)),
        /*4*/new DialogOption("冒险镇是什么？", null , new Vector2Int(1,0)),
        /*5*/new DialogOption("冒险分会是什么？", null , new Vector2Int(1,0)),
        /*6*/new DialogOption("我该做什么？", null , new Vector2Int(1,0)),
        /*7*/new DialogOption("你在干什么？", null , new Vector2Int(1,0)),
        /*8*/new DialogOption("没有问题了", null , new Vector2Int(1,0)),



        /*9*/new DialogOption("怎么了？", null , new Vector2Int(1,0)),
        /*10*/new DialogOption("不靠谱？", null , new Vector2Int(1,0)),

        /*11*/new DialogOption("。。。", null , new Vector2Int(1,0)),
        /*12*/new DialogOption("三个人?", null , new Vector2Int(1,0)),


        /*13*/new DialogOption("老爹？", null , new Vector2Int(1,0)),
        /*14*/new DialogOption("爱管侍小姐？", null , new Vector2Int(1,0)),
        /*15*/new DialogOption("那我呢？", null , new Vector2Int(1,0)),

        /*16*/new DialogOption("冒险任务？", null , new Vector2Int(1,0)),


        /*17*/new DialogOption("真的没有了", null , new Vector2Int(1,0)),
        /*18*/new DialogOption("还是有个问题", null , new Vector2Int(1,0)),

        /*19*/new DialogOption("找不到爱管侍小姐", null , new Vector2Int(1,0)),
        /*20*/new DialogOption("没什么事情", null , new Vector2Int(1,0)),

        /*21*/new DialogOption("遵命！", null , new Vector2Int(1,0)),
        /*22*/new DialogOption("好吧。。。", null , new Vector2Int(1,0)),


    };



    //初始化列表
    static DialogInfo()
    {

        Debug.Log("xxx");
        //修建老匠部分
        {
            WHOption[0].nextNode = WHDialog[2];
            WHOption[1].nextNode = WHDialog[3];

            WHDialog[0].nextNode = WHDialog[1];
            WHDialog[1].options = new List<DialogOption> { WHOption[0], WHOption[1] };
            WHDialog[2].nextNode = WHDialog[4];
            WHDialog[8].options = new List<DialogOption> { WHOption[0], WHOption[1] };
            WHDialog[9].nextNode = WHDialog[10];
            WHDialog[10].nextNode = WHDialog[4];
        }

        //路卡利欧部分
        {
            LucarioOption[0].nextNode = LucarioDialog[5];
            LucarioOption[1].nextNode = LucarioDialog[3];
            LucarioOption[2].nextNode = LucarioDialog[2];
            LucarioOption[3].nextNode = LucarioDialog[8];
            LucarioOption[4].nextNode = LucarioDialog[19];
            LucarioOption[5].nextNode = LucarioDialog[35];
            LucarioOption[6].nextNode = LucarioDialog[37];
            LucarioOption[7].nextNode = LucarioDialog[38];
            LucarioOption[8].nextNode = LucarioDialog[40];
            LucarioOption[9].nextNode = LucarioDialog[27];
            LucarioOption[10].nextNode = LucarioDialog[26];
            LucarioOption[11].nextNode = LucarioDialog[28];
            LucarioOption[12].nextNode = LucarioDialog[28];
            LucarioOption[13].nextNode = LucarioDialog[29];
            LucarioOption[14].nextNode = LucarioDialog[30];
            LucarioOption[15].nextNode = LucarioDialog[32];
            LucarioOption[16].nextNode = LucarioDialog[36];
            LucarioOption[17].nextNode = LucarioDialog[41];
            LucarioOption[18].nextNode = LucarioDialog[39];
            LucarioOption[19].nextNode = LucarioDialog[43];
            LucarioOption[20].nextNode = LucarioDialog[41];
            LucarioOption[21].nextNode = LucarioDialog[49];
            LucarioOption[22].nextNode = LucarioDialog[49];


            LucarioDialog[0].nextNode = LucarioDialog[1];
            LucarioDialog[1].options = new List<DialogOption> { LucarioOption[0], LucarioOption[1], LucarioOption[2] };
            LucarioDialog[3].nextNode = LucarioDialog[4];
            LucarioDialog[5].nextNode = LucarioDialog[6];
            LucarioDialog[6].nextNode = LucarioDialog[7];
            LucarioDialog[7].options = new List<DialogOption> { LucarioOption[3], LucarioOption[4], LucarioOption[5], LucarioOption[6], LucarioOption[7], LucarioOption[8] };
            LucarioDialog[8].nextNode = LucarioDialog[9];
            LucarioDialog[9].nextNode = LucarioDialog[10];
            LucarioDialog[10].nextNode = LucarioDialog[11];
            LucarioDialog[11].nextNode = LucarioDialog[12];
            LucarioDialog[12].nextNode = LucarioDialog[13];
            LucarioDialog[13].nextNode = LucarioDialog[14];
            LucarioDialog[14].nextNode = LucarioDialog[15];
            LucarioDialog[15].nextNode = LucarioDialog[16];
            LucarioDialog[16].nextNode = LucarioDialog[17];
            LucarioDialog[17].nextNode = LucarioDialog[18];
            LucarioDialog[18].options = new List<DialogOption> { LucarioOption[3], LucarioOption[4], LucarioOption[5], LucarioOption[6], LucarioOption[7], LucarioOption[8] };

            LucarioDialog[19].nextNode = LucarioDialog[20];
            LucarioDialog[20].nextNode = LucarioDialog[21];
            LucarioDialog[21].nextNode = LucarioDialog[22];
            LucarioDialog[22].nextNode = LucarioDialog[23];
            LucarioDialog[23].nextNode = LucarioDialog[24];
            LucarioDialog[24].nextNode = LucarioDialog[25];
            LucarioDialog[25].options = new List<DialogOption> { LucarioOption[9], LucarioOption[10] };

            LucarioDialog[26].nextNode = LucarioDialog[27];
            LucarioDialog[27].options = new List<DialogOption> { LucarioOption[11], LucarioOption[12] };
            LucarioDialog[28].options = new List<DialogOption> { LucarioOption[13], LucarioOption[14], LucarioOption[15] };

            LucarioDialog[29].options = new List<DialogOption> {  LucarioOption[14], LucarioOption[15] };
            LucarioDialog[30].nextNode = LucarioDialog[31];
            LucarioDialog[31].options = new List<DialogOption> { LucarioOption[13], LucarioOption[15] };
            LucarioDialog[32].nextNode = LucarioDialog[33];
            LucarioDialog[33].nextNode = LucarioDialog[34];
            LucarioDialog[34].nextNode = LucarioDialog[18];
            LucarioDialog[35].options = new List<DialogOption> { LucarioOption[16]};
            LucarioDialog[36].nextNode = LucarioDialog[39];
            LucarioDialog[37].nextNode = LucarioDialog[39];
            LucarioDialog[38].nextNode = LucarioDialog[39];
            LucarioDialog[39].options = new List<DialogOption> { LucarioOption[3], LucarioOption[4], LucarioOption[5], LucarioOption[6], LucarioOption[7], LucarioOption[8] };
            LucarioDialog[40].options = new List<DialogOption> { LucarioOption[17], LucarioOption[18] };

            LucarioDialog[42].options = new List<DialogOption> { LucarioOption[19], LucarioOption[20] };
            LucarioDialog[43].nextNode = LucarioDialog[44];
            LucarioDialog[44].nextNode = LucarioDialog[45];
            LucarioDialog[45].nextNode = LucarioDialog[46];
            LucarioDialog[46].nextNode = LucarioDialog[47];
            LucarioDialog[47].nextNode = LucarioDialog[48];
            LucarioDialog[48].options = new List<DialogOption> { LucarioOption[21], LucarioOption[22] };
        }
    }
}
