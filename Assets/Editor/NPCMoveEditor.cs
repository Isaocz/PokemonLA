using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCMoveEditor : MonoBehaviour
{

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
