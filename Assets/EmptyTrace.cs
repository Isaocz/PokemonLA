using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTrace : MonoBehaviour
{
    public float moveSpeed;//速度
    GameObject Player;
    GameObject Subsititute;
    GameObject InfatuationEnemy;//敌人对象
    GameObject Target;
    float timer;//计时器
    public float Waittime = 0.5f;//开始追踪时间
    public float distance;//跟踪距离
    private float angle_fix = 80f;//每次修正的角度
    private float angle_differ = 10;//允许的差异角度


    Projectile ParentProjectel;
    Empty ParentEmpty;

    // Start is called before the first frame update
    void Start()
    {
        ParentProjectel = GetComponent<Projectile>();
        ParentEmpty = ParentProjectel.empty;
        Player = ParentEmpty.player.gameObject;
    }

    

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        
        if (timer > Waittime)
        {
            //开始对敌人进行检测
            if (ParentEmpty.isEmptyInfatuationDone)
            {
                OnArea();Target = InfatuationEnemy;
            }else if (ParentEmpty.isSubsititue && ParentEmpty.SubsititueTarget != null && (transform.position - ParentEmpty.SubsititueTarget.transform.position).magnitude < distance)
            {
                Target = ParentEmpty.SubsititueTarget;
            }
            else if((transform.position - Player.transform.position).magnitude < distance)
            {
                Target = Player;
            }
            else
            {
                Target = null;
            }


            //检测到敌人时则执行追踪
            if (Target != null)
            {
                //对需要追踪物体和自身间的角度求解运算
                Vector2 row = (Target.transform.position - transform.position).normalized;
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
                    Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z - Time.deltaTime * (angle_fix + timer * 100));
                    transform.rotation = reAngle;
                }
                else if (angle3 > 180 + angle_differ)
                {
                    Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z + Time.deltaTime * (angle_fix + timer * 100));
                    transform.rotation = reAngle;
                }

            }
        }

        

    }




    private void OnArea()
    {
        GameObject[] games = GameObject.FindGameObjectsWithTag("Empty");
        //设置当前最近需要追踪对象的距离，如果结束后这个值没有变说明范围内没有敌人
        float distance = this.distance;
        for(int i = 0; i < ParentEmpty.transform.parent.childCount; i++) { 
            //这句其实是多余的，因为目前测试场景中可被追踪的敌人都是这个标签，但是也可以保留
            //可以在此基础上发展追踪优先级，的比如同距离优先锁定BOSS，或者不能被追踪的敌人对象
            if (ParentEmpty.transform.GetChild(i).GetComponent<Empty>() != null)
            {
                //找到场景中指定为敌人的对象，进行距离求值运算
                float dis = Vector2.Distance(new Vector2(ParentEmpty.transform.GetChild(i).transform.position.x, ParentEmpty.transform.GetChild(i).transform.position.y),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
                if (distance > dis)
                {
                    //将最小距离的GameObject对象赋值给需要追踪的对象enemy，会不断的循环更替，最终结束循环的时候
                    //筛选出来的enemy就是距离这个弹幕最近的敌人
                    distance = dis;
                    InfatuationEnemy = ParentEmpty.transform.GetChild(i).gameObject;
                }
            }
        }
        if (distance == this.distance)
        {
            //如果没有找到或者飞行过程中脱离了最大追踪距离，删除追踪对象
            InfatuationEnemy = null;
        }
    }
}
