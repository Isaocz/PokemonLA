using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TownMap : MonoBehaviour
{


    /// <summary>
    /// 树
    /// </summary>
    public Tree tree = new Tree();
    [System.Serializable]
    public class Tree
    {
        /// <summary>
        /// 东北树林
        /// </summary>
        public GameObject TreeRU;
        /// <summary>
        /// 西北树林
        /// </summary>
        public GameObject TreeLU;
        /// <summary>
        /// 东南树林
        /// </summary>
        public GameObject TreeRD;
        /// <summary>
        /// 西南树林
        /// </summary>
        public GameObject TreeLD;
    }



    /// <summary>
    /// 栅栏
    /// </summary>
    public Fence fence = new Fence();
    [System.Serializable]
    public class Fence
    {
        /// <summary>
        /// 东北栅栏
        /// </summary>
        public GameObject FenceRU;
        /// <summary>
        /// 西北栅栏
        /// </summary>
        public GameObject FenceLU;
        /// <summary>
        /// 东南栅栏
        /// </summary>
        public GameObject FenceRD;
        /// <summary>
        /// 西南栅栏
        /// </summary>
        public GameObject FenceLD;
        /// <summary>
        /// 技能商店和保育圆之间的栅栏
        /// </summary>
        public GameObject FenceBetwwen01;
        /// <summary>
        /// 奶馆和道具商店之间的栅栏
        /// </summary>
        public GameObject FenceBetwwen02;
        /// <summary>
        /// 道具商店和空地之间的栅栏
        /// </summary>
        public GameObject FenceBetwwen03;
    }



    /// <summary>
    /// 建筑工地
    /// </summary>
    public BuildNow buildnow = new BuildNow();
    [System.Serializable]
    public class BuildNow
    {
        /// <summary>
        /// 奶馆建筑工地
        /// </summary>
        public GameObject MilkBarBuildNow;
        /// <summary>
        ///  道具商店建筑工地
        /// </summary>
        public GameObject ItemShopBuildNow;
        /// <summary>
        ///  技能商店建筑工地
        /// </summary>
        public GameObject SkillMakerBuildNow;
        /// <summary>
        ///  保育圆建筑工地
        /// </summary>
        public GameObject DayCareBuildNow;
        /// <summary>
        ///  头目俱乐部建筑工地
        /// </summary>
        public GameObject BossClubBuildNow;
        /// <summary>
        ///  推石俱乐部建筑工地
        /// </summary>
        public GameObject RockClubBuildNow;
    }



    /// <summary>
    /// 建筑内在
    /// </summary>
    public BuildHouse buildhouse = new BuildHouse();
    [System.Serializable]
    public class BuildHouse
    {
        /// <summary>
        /// 冒险者工会
        /// </summary>
        public TownPoliceStation PoliceStation;
        /// <summary>
        /// 冒险者奶馆
        /// </summary>
        public TownMilkBar MilkBar;
        /// <summary>
        /// 建筑木屋
        /// </summary>
        public TownWoodenHouse WoodenHouse;
        /// <summary>
        /// 技能商店
        /// </summary>
        public TownSkillMaker SkillMaker;
        /// <summary>
        /// 保育圆
        /// </summary>
        public TownDayCare DayCare;
        /// <summary>
        /// 保育圆二楼
        /// </summary>
        public TownDayCareF2 DayCareF2;
        /// <summary>
        /// 道具商店
        /// </summary>
        public TownItemShop ItemShop;
        /// <summary>
        /// 头目俱乐部
        /// </summary>
        public TownBossClub BossClub;
        /// <summary>
        /// 推石俱乐部
        /// </summary>
        public TownRockClub RockClub;
    }




    /// <summary>
    /// 建筑外在
    /// </summary>
    public Build build = new Build();
    [System.Serializable]
    public class Build
    {
        /// <summary>
        /// 冒险者工会
        /// </summary>
        public GameObject PoliceStation;
        /// <summary>
        /// 冒险者奶馆
        /// </summary>
        public GameObject MilkBar;
        /// <summary>
        /// 建筑木屋
        /// </summary>
        public GameObject WoodenHouse;
        /// <summary>
        /// 技能商店
        /// </summary>
        public GameObject SkillMaker;
        /// <summary>
        /// 保育圆
        /// </summary>
        public GameObject DayCare;
        /// <summary>
        /// 道具商店
        /// </summary>
        public GameObject ItemShop;
        /// <summary>
        /// 头目俱乐部
        /// </summary>
        public GameObject BossClub;
        /// <summary>
        /// 推石俱乐部
        /// </summary>
        public GameObject RockClub;
        /// <summary>
        /// 公园
        /// </summary>
        public TownPark Park;
        /// <summary>
        /// 空地1
        /// </summary>
        public GameObject Empty1;
        /// <summary>
        /// 空地2
        /// </summary>
        public GameObject Empty2;
        /// <summary>
        /// 公共设施
        /// </summary>
        public TownPublicFacilities PublicFacilities;
    }


    /// <summary>
    /// 小镇中NPC的父对象
    /// </summary>
    public Transform TownNPCParent;


    /// <summary>
    /// 玩家
    /// </summary>
    public TownPlayer Player;
    /// <summary>
    /// 相机
    /// </summary>
    public Camera MainCamera;
    /// <summary>
    /// 静态地图
    /// </summary>
    public static TownMap townMap;


    //表示玩家在小镇中所处的位置
    public enum TownPlayerState
    {
        inMilkBar, //玩家在酒吧中
        inTown,    //玩家在镇上
        inWoodenHouse,    //玩家在铁骨建筑公司木屋内
        inSkillMaker,    //玩家在图图技能艺术廊内
        inDayCareF1,    //玩家在破壳宝育园一层
        inDayCareF2,    //玩家在破壳宝育园二层
        inItemShop,    //玩家在道具商店
        inBossClub,    //玩家在头目俱乐部
        inPoliceStation,    //玩家在冒险家俱乐部
        inRockClub,    //玩家在滚石俱乐部

    }
    public TownPlayerState State;
    public TownPlayerState StartState;

    private void Awake()
    {
        townMap = this;
        State = StartState;

        FindObjectOfType<CameraAdapt>().HideCameraMasks();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<TownPlayer>() != null) { Player = FindObjectOfType<TownPlayer>(); Player.transform.position = InstancePlayerPosition(); }
        if (FindObjectOfType<Camera>() != null) { MainCamera = FindObjectOfType<Camera>(); MainCamera.transform.position = InstanceCameraPosition(); }
    }

    public Vector3 InstancePlayerPosition()
    {
        Vector3 OutPut = Vector3.zero;
        switch (State)
        {
            case TownPlayerState.inTown:
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector3(1016.27f, 1.98f, 0);
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector3(202.45f, -1.51f, 0);
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector3(400.0f, -0.14f, 0);
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector3(606.96f, -5.41f, 0);
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector3(794.278f, 3.53f, 0);
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector3(200.0f, 201.6f, 0);
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector3(387.04f, 197.85f, 0);
                break;
            case TownPlayerState.inPoliceStation:
                OutPut = new Vector3(605.67f, 197.34f, 0);
                break;
            case TownPlayerState.inRockClub:
                OutPut = new Vector3(789.88f, 200.14f, 0);
                break;
        }
        return OutPut;
    }


    public Vector3 InstanceCameraPosition()
    {
        Vector3 OutPut = new Vector3(0, 0.7f, -11);
        switch (townMap.State)
        {
            case TownPlayerState.inTown:
                OutPut = new Vector3(0, 0.7f, -11);
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector3(1010.871f, 0.6414994f, -11);
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector3(200.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector3(400.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector3(600.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector3(800.0f, 0.6414994f, -11);
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector3(200.0f, 200.12f, -11);
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector3(396.44f, 199.98f, -11);
                break;
            case TownPlayerState.inPoliceStation:
                OutPut = new Vector3(596.44f, 199.98f, -11);
                break;
            case TownPlayerState.inRockClub:
                OutPut = new Vector3(789.48f, 199.98f, -11);
                break;
        }

        return OutPut;
    }

    public Vector2[] CameraBoard()
    {
        Vector2[] OutPut = new Vector2[] {
                    new Vector2(30000.0f, 30000.0f),
                    new Vector2(-30000.0f, 30000.0f),
                    new Vector2(-30000.0f, -30000.0f),
                    new Vector2(30000.0f, -30000.0f)
                };
        switch (townMap.State)
        {
            case TownPlayerState.inTown:
                OutPut = new Vector2[] {
                    new Vector2(36.0f, 32.2f),
                    new Vector2(-26.0f, 32.2f),
                    new Vector2(-26.0f, -2.0f),
                    new Vector2(36.0f, -2.0f)
                };
                break;
            case TownPlayerState.inMilkBar:
                OutPut = new Vector2[] {
                    new Vector2(1021.2f + 00, 11.4f + 00),
                    new Vector2(978.8f -00, 11.4f + 00),
                    new Vector2(978.8f -00, -10.1f - 00),
                    new Vector2(1021.2f + 00, -10.1f - 00)
                };
                break;
            case TownPlayerState.inWoodenHouse:
                OutPut = new Vector2[] {
                    new Vector2(218.6062f, 10.74151f),
                    new Vector2(181.3938f, 10.74151f),
                    new Vector2(181.3938f, -10.74151f),
                    new Vector2(218.6062f, -10.74151f)
                };
                break;
            case TownPlayerState.inSkillMaker:
                OutPut = new Vector2[] {
                    new Vector2(414.323f, 10.74151f),
                    new Vector2(385.677f, 10.74151f),
                    new Vector2(385.677f, -10.74151f),
                    new Vector2(414.323f, -10.74151f)
                };
                break;
            case TownPlayerState.inDayCareF1:
                OutPut = new Vector2[] {
                    new Vector2(618.6063f, 10.74151f),
                    new Vector2(581.3937f, 10.74151f),
                    new Vector2(581.3937f, -10.74151f),
                    new Vector2(618.6063f, -10.74151f)
                };
                break;
            case TownPlayerState.inDayCareF2:
                OutPut = new Vector2[] {
                    new Vector2(818.6063f, 10.74151f+4.598501f),
                    new Vector2(781.3937f, 10.74151f+4.598501f),
                    new Vector2(781.3937f, -10.74151f+4.698501f),
                    new Vector2(818.6063f, -10.74151f+4.698501f)
                };
                break;
            case TownPlayerState.inItemShop:
                OutPut = new Vector2[] {
                    new Vector2(211.6625f, 207.875f),
                    new Vector2(188.3375f, 207.875f),
                    new Vector2(188.3375f, 192.125f),
                    new Vector2(211.6625f, 192.125f)
                };
                break;
            case TownPlayerState.inBossClub:
                OutPut = new Vector2[] {
                    new Vector2(417.8835f, 210.723f),
                    new Vector2(382.1165f, 210.723f),
                    new Vector2(382.1165f, 189.2377f),
                    new Vector2(417.8835f, 189.2377f)
                };
                break;
            case TownPlayerState.inPoliceStation:
                OutPut = new Vector2[] {
                    new Vector2(617.8835f, 210.723f),
                    new Vector2(582.1165f, 210.723f),
                    new Vector2(582.1165f, 189.2377f),
                    new Vector2(617.8835f, 189.2377f)
                };
                break;
            case TownPlayerState.inRockClub:
                OutPut = new Vector2[] {
                    new Vector2(817.8835f, 210.723f),
                    new Vector2(782.1165f, 210.723f),
                    new Vector2(782.1165f, 189.2377f),
                    new Vector2(817.8835f, 189.2377f)
                };
                break;
        }
        return OutPut;
    }






    [CustomEditor(typeof(TownMap))]
    public class MyScriptEditor : Editor
    {
        private bool TreeFoldout = true;
        private bool FenceFoldout = true;
        private bool BuildHouseFoldout = true;
        private bool BuildNFoldout = true;
        private bool BuildNowFoldout = true;

        public override void OnInspectorGUI()
        {
            TownMap townmap = (TownMap)target;

            townmap.TownNPCParent = (Transform)EditorGUILayout.ObjectField("TownNPCParent", townmap.TownNPCParent, typeof(Transform), true);
            townmap.Player = (TownPlayer)EditorGUILayout.ObjectField("Player", townmap.Player, typeof(TownPlayer), true);
            townmap.MainCamera = (Camera)EditorGUILayout.ObjectField("MainCamera", townmap.MainCamera, typeof(Camera), true);

            TreeFoldout = EditorGUILayout.Foldout(TreeFoldout, "Tree");
            if (TreeFoldout)
            {
                EditorGUI.indentLevel++;
                townmap.tree.TreeRU = (GameObject)EditorGUILayout.ObjectField("TreeRU", townmap.tree.TreeRU, typeof(GameObject), true);
                townmap.tree.TreeLU = (GameObject)EditorGUILayout.ObjectField("TreeLU", townmap.tree.TreeLU, typeof(GameObject), true);
                townmap.tree.TreeRD = (GameObject)EditorGUILayout.ObjectField("TreeRD", townmap.tree.TreeRD, typeof(GameObject), true);
                townmap.tree.TreeLD = (GameObject)EditorGUILayout.ObjectField("TreeLD", townmap.tree.TreeLD, typeof(GameObject), true);
                EditorGUI.indentLevel--;
            }

            FenceFoldout = EditorGUILayout.Foldout(FenceFoldout, "Fence");
            if (FenceFoldout)
            {
                EditorGUI.indentLevel++;
                townmap.fence.FenceRU = (GameObject)EditorGUILayout.ObjectField("FenceRU", townmap.fence.FenceRU, typeof(GameObject), true);
                townmap.fence.FenceLU = (GameObject)EditorGUILayout.ObjectField("FenceLU", townmap.fence.FenceLU, typeof(GameObject), true);
                townmap.fence.FenceRD = (GameObject)EditorGUILayout.ObjectField("FenceRD", townmap.fence.FenceRD, typeof(GameObject), true);
                townmap.fence.FenceLD = (GameObject)EditorGUILayout.ObjectField("FenceLD", townmap.fence.FenceLD, typeof(GameObject), true);
                townmap.fence.FenceBetwwen01 = (GameObject)EditorGUILayout.ObjectField("FenceBetwwen01", townmap.fence.FenceBetwwen01, typeof(GameObject), true);
                townmap.fence.FenceBetwwen02 = (GameObject)EditorGUILayout.ObjectField("FenceBetwwen02", townmap.fence.FenceBetwwen02, typeof(GameObject), true);
                townmap.fence.FenceBetwwen03 = (GameObject)EditorGUILayout.ObjectField("FenceBetwwen03", townmap.fence.FenceBetwwen03, typeof(GameObject), true);
                EditorGUI.indentLevel--;
            }

            BuildNowFoldout = EditorGUILayout.Foldout(BuildNowFoldout, "BuildNow");
            if (BuildNowFoldout)
            {
                EditorGUI.indentLevel++;
                townmap.buildnow.MilkBarBuildNow = (GameObject)EditorGUILayout.ObjectField("MilkBarBuildNow", townmap.buildnow.MilkBarBuildNow, typeof(GameObject), true);
                townmap.buildnow.ItemShopBuildNow = (GameObject)EditorGUILayout.ObjectField("ItemShopBuildNow", townmap.buildnow.ItemShopBuildNow, typeof(GameObject), true);
                townmap.buildnow.SkillMakerBuildNow = (GameObject)EditorGUILayout.ObjectField("SkillMakerBuildNow", townmap.buildnow.SkillMakerBuildNow, typeof(GameObject), true);
                townmap.buildnow.DayCareBuildNow = (GameObject)EditorGUILayout.ObjectField("DayCareBuildNow", townmap.buildnow.DayCareBuildNow, typeof(GameObject), true);
                townmap.buildnow.BossClubBuildNow = (GameObject)EditorGUILayout.ObjectField("BossClubBuildNow", townmap.buildnow.BossClubBuildNow, typeof(GameObject), true);
                townmap.buildnow.RockClubBuildNow = (GameObject)EditorGUILayout.ObjectField("RockClubBuildNow", townmap.buildnow.RockClubBuildNow, typeof(GameObject), true);
                EditorGUI.indentLevel--;
            }

            BuildHouseFoldout = EditorGUILayout.Foldout(BuildHouseFoldout, "BuildHouse");
            if (BuildHouseFoldout)
            {
                EditorGUI.indentLevel++;
                townmap.buildhouse.PoliceStation = (TownPoliceStation)EditorGUILayout.ObjectField("PoliceStation", townmap.buildhouse.PoliceStation, typeof(TownPoliceStation), true);
                townmap.buildhouse.MilkBar = (TownMilkBar)EditorGUILayout.ObjectField("MilkBar", townmap.buildhouse.MilkBar, typeof(TownMilkBar), true);
                townmap.buildhouse.WoodenHouse = (TownWoodenHouse)EditorGUILayout.ObjectField("WoodenHouse", townmap.buildhouse.WoodenHouse, typeof(TownWoodenHouse), true);
                townmap.buildhouse.SkillMaker = (TownSkillMaker)EditorGUILayout.ObjectField("SkillMaker", townmap.buildhouse.SkillMaker, typeof(TownSkillMaker), true);
                townmap.buildhouse.DayCare = (TownDayCare)EditorGUILayout.ObjectField("DayCare", townmap.buildhouse.DayCare, typeof(TownDayCare), true);
                townmap.buildhouse.DayCareF2 = (TownDayCareF2)EditorGUILayout.ObjectField("DayCareF2", townmap.buildhouse.DayCareF2, typeof(TownDayCareF2), true);
                townmap.buildhouse.ItemShop = (TownItemShop)EditorGUILayout.ObjectField("ItemShop", townmap.buildhouse.ItemShop, typeof(TownItemShop), true);
                townmap.buildhouse.BossClub = (TownBossClub)EditorGUILayout.ObjectField("BossClub", townmap.buildhouse.BossClub, typeof(TownBossClub), true);
                townmap.buildhouse.RockClub = (TownRockClub)EditorGUILayout.ObjectField("RockClub", townmap.buildhouse.RockClub, typeof(TownRockClub), true);
                EditorGUI.indentLevel--;
            }

            BuildNFoldout = EditorGUILayout.Foldout(BuildNFoldout, "Build");
            if (BuildNFoldout)
            {
                EditorGUI.indentLevel++;
                townmap.build.PoliceStation = (GameObject)EditorGUILayout.ObjectField("PoliceStation", townmap.build.PoliceStation, typeof(GameObject), true);
                townmap.build.MilkBar = (GameObject)EditorGUILayout.ObjectField("MilkBar", townmap.build.MilkBar, typeof(GameObject), true);
                townmap.build.WoodenHouse = (GameObject)EditorGUILayout.ObjectField("WoodenHouse", townmap.build.WoodenHouse, typeof(GameObject), true);
                townmap.build.SkillMaker = (GameObject)EditorGUILayout.ObjectField("SkillMaker", townmap.build.SkillMaker, typeof(GameObject), true);
                townmap.build.DayCare = (GameObject)EditorGUILayout.ObjectField("DayCare", townmap.build.DayCare, typeof(GameObject), true);
                townmap.build.ItemShop = (GameObject)EditorGUILayout.ObjectField("ItemShop", townmap.build.ItemShop, typeof(GameObject), true);
                townmap.build.BossClub = (GameObject)EditorGUILayout.ObjectField("BossClub", townmap.build.BossClub, typeof(GameObject), true);
                townmap.build.RockClub = (GameObject)EditorGUILayout.ObjectField("RockClub", townmap.build.RockClub, typeof(GameObject), true);
                townmap.build.Park = (TownPark)EditorGUILayout.ObjectField("Park", townmap.build.Park, typeof(TownPark), true);
                townmap.build.Empty1 = (GameObject)EditorGUILayout.ObjectField("Empty1", townmap.build.Empty1, typeof(GameObject), true);
                townmap.build.Empty2 = (GameObject)EditorGUILayout.ObjectField("Empty2", townmap.build.Empty2, typeof(GameObject), true);
                townmap.build.PublicFacilities = (TownPublicFacilities)EditorGUILayout.ObjectField("PublicFacilities", townmap.build.PublicFacilities, typeof(TownPublicFacilities), true);
                EditorGUI.indentLevel--;
            }




            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
