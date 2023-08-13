using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTrace : MonoBehaviour
{
    public float moveSpeed;//�ٶ�
    GameObject Player;
    GameObject Subsititute;
    GameObject InfatuationEnemy;//���˶���
    GameObject Target;
    float timer;//��ʱ��
    public float Waittime = 0.5f;//��ʼ׷��ʱ��
    public float distance;//���پ���
    private float angle_fix = 80f;//ÿ�������ĽǶ�
    private float angle_differ = 10;//����Ĳ���Ƕ�


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
            //��ʼ�Ե��˽��м��
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


            //��⵽����ʱ��ִ��׷��
            if (Target != null)
            {
                //����Ҫ׷������������ĽǶ��������
                Vector2 row = (Target.transform.position - transform.position).normalized;
                //��ȡ�������н�
                float angle1 = Vector3.SignedAngle(Vector3.up, row, Vector3.forward);
                //���н���������������ȡֵ�ͷ�Χ����
                angle1 = (angle1 + 270) % 360;
                //��ȡ��Ļ������������
                float angle2 = transform.eulerAngles.z % 360;
                //��ȡ�����Ƕȼ�Ĳ���ֵ����׼��
                float angle3 = ((angle1 - angle2) + 360) % 360;


                //������ĽǶ���������ʹ������x��ָ����Ҫ׷�ٵ�Ŀ��
                //������Ҫ׷�ٶ���ķ�������Ƕȣ������趨��ֵ���е���
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
        //���õ�ǰ�����Ҫ׷�ٶ���ľ��룬������������ֵû�б�˵����Χ��û�е���
        float distance = this.distance;
        for(int i = 0; i < ParentEmpty.transform.parent.childCount; i++) { 
            //�����ʵ�Ƕ���ģ���ΪĿǰ���Գ����пɱ�׷�ٵĵ��˶��������ǩ������Ҳ���Ա���
            //�����ڴ˻����Ϸ�չ׷�����ȼ����ı���ͬ������������BOSS�����߲��ܱ�׷�ٵĵ��˶���
            if (ParentEmpty.transform.GetChild(i).GetComponent<Empty>() != null)
            {
                //�ҵ�������ָ��Ϊ���˵Ķ��󣬽��о�����ֵ����
                float dis = Vector2.Distance(new Vector2(ParentEmpty.transform.GetChild(i).transform.position.x, ParentEmpty.transform.GetChild(i).transform.position.y),
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
                if (distance > dis)
                {
                    //����С�����GameObject����ֵ����Ҫ׷�ٵĶ���enemy���᲻�ϵ�ѭ�����棬���ս���ѭ����ʱ��
                    //ɸѡ������enemy���Ǿ��������Ļ����ĵ���
                    distance = dis;
                    InfatuationEnemy = ParentEmpty.transform.GetChild(i).gameObject;
                }
            }
        }
        if (distance == this.distance)
        {
            //���û���ҵ����߷��й��������������׷�پ��룬ɾ��׷�ٶ���
            InfatuationEnemy = null;
        }
    }
}
