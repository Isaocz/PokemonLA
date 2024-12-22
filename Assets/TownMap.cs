using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TownMap : MonoBehaviour
{


    /// <summary>
    /// ��
    /// </summary>
    public Tree tree = new Tree();
    [System.Serializable]
    public class Tree
    {
        /// <summary>
        /// ��������
        /// </summary>
        public GameObject TreeRU;
        /// <summary>
        /// ��������
        /// </summary>
        public GameObject TreeLU;
        /// <summary>
        /// ��������
        /// </summary>
        public GameObject TreeRD;
        /// <summary>
        /// ��������
        /// </summary>
        public GameObject TreeLD;
    }



    /// <summary>
    /// դ��
    /// </summary>
    public Fence fence = new Fence();
    [System.Serializable]
    public class Fence
    {
        /// <summary>
        /// ����դ��
        /// </summary>
        public GameObject FenceRU;
        /// <summary>
        /// ����դ��
        /// </summary>
        public GameObject FenceLU;
        /// <summary>
        /// ����դ��
        /// </summary>
        public GameObject FenceRD;
        /// <summary>
        /// ����դ��
        /// </summary>
        public GameObject FenceLD;
        /// <summary>
        /// �����̵�ͱ���Բ֮���դ��
        /// </summary>
        public GameObject FenceBetwwen01;
        /// <summary>
        /// �̹ݺ͵����̵�֮���դ��
        /// </summary>
        public GameObject FenceBetwwen02;
        /// <summary>
        /// �����̵�Ϳյ�֮���դ��
        /// </summary>
        public GameObject FenceBetwwen03;
    }



    /// <summary>
    /// ��������
    /// </summary>
    public BuildNow buildnow = new BuildNow();
    [System.Serializable]
    public class BuildNow
    {
        /// <summary>
        /// �̹ݽ�������
        /// </summary>
        public GameObject MilkBarBuildNow;
        /// <summary>
        ///  �����̵꽨������
        /// </summary>
        public GameObject ItemShopBuildNow;
        /// <summary>
        ///  �����̵꽨������
        /// </summary>
        public GameObject SkillMakerBuildNow;
        /// <summary>
        ///  ����Բ��������
        /// </summary>
        public GameObject DayCareBuildNow;
        /// <summary>
        ///  ͷĿ���ֲ���������
        /// </summary>
        public GameObject BossClubBuildNow;
        /// <summary>
        ///  ��ʯ���ֲ���������
        /// </summary>
        public GameObject RockClubBuildNow;
    }



    /// <summary>
    /// ��������
    /// </summary>
    public BuildHouse buildhouse = new BuildHouse();
    [System.Serializable]
    public class BuildHouse
    {
        /// <summary>
        /// ð���߹���
        /// </summary>
        public TownPoliceStation PoliceStation;
        /// <summary>
        /// ð�����̹�
        /// </summary>
        public TownMilkBar MilkBar;
        /// <summary>
        /// ����ľ��
        /// </summary>
        public TownWoodenHouse WoodenHouse;
        /// <summary>
        /// �����̵�
        /// </summary>
        public TownSkillMaker SkillMaker;
        /// <summary>
        /// ����Բ
        /// </summary>
        public TownDayCare DayCare;
        /// <summary>
        /// ����Բ��¥
        /// </summary>
        public TownDayCareF2 DayCareF2;
        /// <summary>
        /// �����̵�
        /// </summary>
        public TownItemShop ItemShop;
        /// <summary>
        /// ͷĿ���ֲ�
        /// </summary>
        public TownBossClub BossClub;
        /// <summary>
        /// ��ʯ���ֲ�
        /// </summary>
        public TownRockClub RockClub;
    }




    /// <summary>
    /// ��������
    /// </summary>
    public Build build = new Build();
    [System.Serializable]
    public class Build
    {
        /// <summary>
        /// ð���߹���
        /// </summary>
        public GameObject PoliceStation;
        /// <summary>
        /// ð�����̹�
        /// </summary>
        public GameObject MilkBar;
        /// <summary>
        /// ����ľ��
        /// </summary>
        public GameObject WoodenHouse;
        /// <summary>
        /// �����̵�
        /// </summary>
        public GameObject SkillMaker;
        /// <summary>
        /// ����Բ
        /// </summary>
        public GameObject DayCare;
        /// <summary>
        /// �����̵�
        /// </summary>
        public GameObject ItemShop;
        /// <summary>
        /// ͷĿ���ֲ�
        /// </summary>
        public GameObject BossClub;
        /// <summary>
        /// ��ʯ���ֲ�
        /// </summary>
        public GameObject RockClub;
        /// <summary>
        /// ��԰
        /// </summary>
        public TownPark Park;
        /// <summary>
        /// �յ�1
        /// </summary>
        public GameObject Empty1;
        /// <summary>
        /// �յ�2
        /// </summary>
        public GameObject Empty2;
        /// <summary>
        /// ������ʩ
        /// </summary>
        public TownPublicFacilities PublicFacilities;
    }


    /// <summary>
    /// С����NPC�ĸ�����
    /// </summary>
    public Transform TownNPCParent;


    /// <summary>
    /// ���
    /// </summary>
    public TownPlayer Player;
    /// <summary>
    /// ���
    /// </summary>
    public Camera MainCamera;
    /// <summary>
    /// ��̬��ͼ
    /// </summary>
    public static TownMap townMap;


    //��ʾ�����С����������λ��
    public enum TownPlayerState
    {
        inMilkBar, //����ھư���
        inTown,    //���������
        inWoodenHouse,    //��������ǽ�����˾ľ����
        inSkillMaker,    //�����ͼͼ������������
        inDayCareF1,    //������ƿǱ���԰һ��
        inDayCareF2,    //������ƿǱ���԰����
        inItemShop,    //����ڵ����̵�
        inBossClub,    //�����ͷĿ���ֲ�
        inPoliceStation,    //�����ð�ռҾ��ֲ�
        inRockClub,    //����ڹ�ʯ���ֲ�

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
