using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyEchoedVoice : MonoBehaviour
{
    //�ͷŻ����ĵ���
    public Empty ParentEmpty;
    //��������
    int SpDmage = 40;
    //������ײ��
    CircleCollider2D EchoedVoiceCircleCollider;
    //���ܷ�ΧָʾȦ
    SkillRangeCircle EchoedVoiceRangeCircle;
    //������Ч1
    ParticleSystem ps1;
    //������Ч2
    ParticleSystem ps2;


    private void Start()
    {
        Destroy(this.gameObject, 6.5f);
    }


    //���ݻ����ȼ������û����ķ�Χ������
    public void SetEchoedVoiceLevel(int EchoedVoiceLevel)
    {
        GetChangeableChild();
        var main1 = ps1.main;
        var emmision1 = ps1.emission;
        var main2 = ps2.main;
        var emmision2 = ps2.emission;
        switch (EchoedVoiceLevel)
        {
            case 0:
                SpDmage = 40;
                EchoedVoiceCircleCollider.radius = 2.0f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(1, 1, 0);

                main1.startSize = 1.0f;
                emmision1.rateOverTimeMultiplier = 2.0f;
                Debug.Log(emmision1.rateOverTimeMultiplier);

                main2.startSize = 0.5f;
                main2.startSpeed = 2.666667f;
                emmision2.rateOverTimeMultiplier = 7;
                break;
            case 1:
                SpDmage = 80;
                EchoedVoiceCircleCollider.radius = 2.8f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(1.4f, 1.4f, 0);

                main1.startSize = 1.4f;
                emmision1.rateOverTime = 2.4f;

                main2.startSize = 0.6058f;
                main2.startSpeed = 3.728f;
                emmision2.rateOverTime = 8;
                break;
            case 2:
                SpDmage = 120;
                EchoedVoiceCircleCollider.radius = 3.7f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(1.85f, 1.85f, 0);

                main1.startSize = 1.85f;
                emmision1.rateOverTime = 2.9f;

                main2.startSize = 0.802f;
                main2.startSpeed = 4.93f;
                emmision2.rateOverTime = 10;
                break;
            case 3:
                SpDmage = 160;
                EchoedVoiceCircleCollider.radius = 4.7f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(2.35f, 2.35f, 0);

                main1.startSize = 2.35f;
                emmision1.rateOverTime = 3.2f;

                main2.startSize = 1.02f;
                main2.startSpeed = 6.26f;
                emmision2.rateOverTime = 11;
                break;
            case 4:
                SpDmage = 200;
                EchoedVoiceCircleCollider.radius = 6.0f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(3, 3, 0);

                main1.startSize = 3.0f;
                emmision1.rateOverTime = 3.5f;

                main2.startSize = 1.3f;
                main2.startSpeed = 8.0f;
                emmision2.rateOverTime = 12;
                break;
            default:
                SpDmage = 40;
                EchoedVoiceCircleCollider.radius = 2.0f;
                EchoedVoiceRangeCircle.transform.localScale = new Vector3(1, 1, 0);

                main1.startSize = 1.0f;
                emmision1.rateOverTime = 2.0f;

                main2.startSize = 0.5f;
                main2.startSpeed = 2.666667f;
                emmision2.rateOverTime = 7;
                break;
        }
    }

    //��ȡ��������ȼ����ı�����
    void GetChangeableChild()
    {
        EchoedVoiceCircleCollider = transform.GetComponent<CircleCollider2D>();
        EchoedVoiceRangeCircle = transform.GetChild(0).GetComponent<SkillRangeCircle>();
        ps1 = transform.GetChild(1).GetComponent<ParticleSystem>();
        ps2 = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentEmpty != null)
        {
            //δ���Ȼ�ʱ
            if (!ParentEmpty.isEmptyInfatuationDone && other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Normal);
                if (p != null)
                {
                    p.KnockOutPoint = 5f;
                    p.KnockOutDirection = (p.transform.position - transform.position).normalized;
                }
            }
            //���Ȼ�ʱ
            else if (ParentEmpty.isEmptyInfatuationDone && other.tag == "Empty" && other.gameObject != ParentEmpty.gameObject)
            {
                Empty e = other.GetComponent<Empty>();
                Pokemon.PokemonHpChange(ParentEmpty.gameObject, other.gameObject, 0, SpDmage, 0, PokemonType.TypeEnum.Normal);
            }
        }
    }
}
