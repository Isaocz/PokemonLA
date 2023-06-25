using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceEffect : MonoBehaviour
{
    public float moveSpeed;//速度
    public float rotateSpeed;//旋转速度

    GameObject enemy;//敌人对象
    float timer;//计时器
    public float Waittime = 0.5f;//开始追踪时间
    RaycastHit hit;

    private float angle_fix = 1;//每次修正的角度
    private float angle_differ = 1;//允许的差异角度

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        if (enemy != null)
        {
            //对需要追踪物体和自身间的角度求解运算
            Vector2 row = (enemy.transform.position - transform.position).normalized;
            //获取两物体间夹角
            float angle1 = Vector3.SignedAngle(Vector3.up, row, Vector3.forward);
            //将夹角坐标和世界坐标的取值和范围对齐
            angle1 = (angle1 + 270) % 360;
            //获取弹幕自身世界坐标
            float angle2 = transform.eulerAngles.z % 360;
            //获取两个角度间的差异值并标准化
            float angle3 = ((angle1 - angle2) + 360) % 360;
            //对物体的角度做修正，使得物体x轴指向需要追踪的目标
            //朝向需要追踪对象的方向调整角度，按照设定的值进行调整
            if (angle3 < 180 - angle_differ)
            {
                Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z - angle_fix);
                transform.rotation = reAngle;
            }
            else if (angle3 > 180 + angle_differ)
            {
                Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle_fix);
                transform.rotation = reAngle;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Empty enemy = other.gameObject.GetComponent<Empty>();
        if (enemy != null)
        {
            //指定敌人目标
            this.enemy = other.gameObject;
        }
    }
    //射线检测，如果击中目标点则销毁炮弹
    void CheckHint()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform == enemy && hit.distance < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
