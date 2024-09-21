using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMembersPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public InfoPanleRoleInfoMark RoleMark;

    public GameObject MarkParent;

    private void OnEnable()
    {
        SaveData save = SaveLoader.saveLoader.saveData;
        if (MarkParent.transform.childCount != 0)
        {
            foreach (Transform child in MarkParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < save.RoleList.Count; i++)
        {
            if (save.RoleList[i].isUnlock) 
            {
                Instantiate(RoleMark, MarkParent.transform).SetRoleInfo(save.RoleList[i]);
            }
        }

    }
}
