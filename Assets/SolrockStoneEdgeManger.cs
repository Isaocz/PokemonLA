using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolrockStoneEdgeManger : MonoBehaviour
{
    public Empty ParentEmpty;
    SolrockStoneEdge SE01;
    SolrockStoneEdge SE02;
    SolrockStoneEdge SE03;
    SolrockStoneEdge SE04;
    SolrockStoneEdge SE05;
    SolrockStoneEdge SE06;
    SolrockStoneEdge SE07;
    SolrockStoneEdge SE08;

    // Start is called before the first frame update
    void Start()
    {
        SE01 = transform.GetChild(0).GetComponent<SolrockStoneEdge>();
        SE02 = transform.GetChild(1).GetComponent<SolrockStoneEdge>();
        SE03 = transform.GetChild(2).GetComponent<SolrockStoneEdge>();
        SE04 = transform.GetChild(3).GetComponent<SolrockStoneEdge>();
        SE05 = transform.GetChild(4).GetComponent<SolrockStoneEdge>();
        SE06 = transform.GetChild(5).GetComponent<SolrockStoneEdge>();
        SE07 = transform.GetChild(6).GetComponent<SolrockStoneEdge>();
        SE08 = transform.GetChild(7).GetComponent<SolrockStoneEdge>();
        SE01.empty = ParentEmpty;
        SE02.empty = ParentEmpty;
        SE03.empty = ParentEmpty;
        SE04.empty = ParentEmpty;
        SE05.empty = ParentEmpty;
        SE06.empty = ParentEmpty;
        SE07.empty = ParentEmpty;
        SE08.empty = ParentEmpty;

        if (ParentEmpty.isEmptyConfusionDone)
        {
            SE01.gameObject.SetActive(false);
            SE05.gameObject.SetActive(false);
            SE02.LaunchNotForce(Quaternion.AngleAxis(30, Vector3.forward) * Vector3.right, 9);
            SE03.LaunchNotForce(Quaternion.AngleAxis(90, Vector3.forward) * Vector3.right, 9);
            SE04.LaunchNotForce(Quaternion.AngleAxis(150, Vector3.forward) * Vector3.right, 9);
            SE06.LaunchNotForce(Quaternion.AngleAxis(210, Vector3.forward) * Vector3.right, 9);
            SE07.LaunchNotForce(Quaternion.AngleAxis(270, Vector3.forward) * Vector3.right, 9);
            SE08.LaunchNotForce(Quaternion.AngleAxis(330, Vector3.forward) * Vector3.right, 9);
            SE02.transform.rotation = Quaternion.Euler(0, 0, 30);
            SE04.transform.rotation = Quaternion.Euler(0, 0, 150);
            SE06.transform.rotation = Quaternion.Euler(0, 0, 210);
            SE08.transform.rotation = Quaternion.Euler(0, 0, 330);
        }
        else
        {
            SE01.LaunchNotForce(Quaternion.AngleAxis(0, Vector3.forward) * Vector3.right, 9);
            SE02.LaunchNotForce(Quaternion.AngleAxis(45, Vector3.forward) * Vector3.right, 9);
            SE03.LaunchNotForce(Quaternion.AngleAxis(90, Vector3.forward) * Vector3.right, 9);
            SE04.LaunchNotForce(Quaternion.AngleAxis(135, Vector3.forward) * Vector3.right, 9);
            SE05.LaunchNotForce(Quaternion.AngleAxis(180, Vector3.forward) * Vector3.right, 9);
            SE06.LaunchNotForce(Quaternion.AngleAxis(225, Vector3.forward) * Vector3.right, 9);
            SE07.LaunchNotForce(Quaternion.AngleAxis(270, Vector3.forward) * Vector3.right, 9);
            SE08.LaunchNotForce(Quaternion.AngleAxis(315, Vector3.forward) * Vector3.right, 9);
        }

    }
}
