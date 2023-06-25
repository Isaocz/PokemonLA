using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceEffect : MonoBehaviour
{
    public float moveSpeed;//�ٶ�
    public float rotateSpeed;//��ת�ٶ�

    GameObject enemy;//���˶���
    float timer;//��ʱ��
    public float Waittime = 0.5f;//��ʼ׷��ʱ��
    RaycastHit hit;

    private float angle_fix = 1;//ÿ�������ĽǶ�
    private float angle_differ = 1;//����Ĳ���Ƕ�

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
            //����Ҫ׷������������ĽǶ��������
            Vector2 row = (enemy.transform.position - transform.position).normalized;
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
            //ָ������Ŀ��
            this.enemy = other.gameObject;
        }
    }
    //���߼�⣬�������Ŀ����������ڵ�
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
