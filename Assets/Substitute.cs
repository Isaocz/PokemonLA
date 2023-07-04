using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Substitute : MonoBehaviour
{
    public PlayerControler ParentPlayer;
    public int MaxHP;
    public bool isDieBoom;
    public Skill DieBoom;
    [Tooltip("ui��Ѫ��")]
    public SubstituteHpBar uIHealth;
    int NowHp;
    bool isSubstituteInvincible;
    float InvincileTimer = 0.8f;
    Animator animator;

    public void SetSubstitute(int maxHP,PlayerControler player)
    {
        MaxHP = maxHP;
        NowHp = maxHP;
        ParentPlayer = player;
        animator = GetComponent<Animator>();
        animator.SetFloat("LookX" , (Random.Range(0.0f,1.0f)>0.5f?-1:1));
        animator.SetFloat("LookY" , (Random.Range(0.0f,1.0f)>0.5f?-1:1));
    }

    public void SubStituteChangeHp(float ChangePoint, float ChangePointSp, int SkillType)
    {
        if (NowHp > 0) {
            if (ChangePoint < 0 || ChangePointSp < 0)
            {
                Type.TypeEnum enumVaue = (Type.TypeEnum)SkillType;
                ChangePoint = ChangePoint * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                ChangePointSp = ChangePointSp * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Water) ? (Weather.GlobalWeather.isRainPlus ? 1.8f : 1.3f) : 1)
                    * ((Weather.GlobalWeather.isRain && enumVaue == Type.TypeEnum.Fire) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Water) ? 0.5f : 1)
                    * ((Weather.GlobalWeather.isSunny && enumVaue == Type.TypeEnum.Fire) ? (Weather.GlobalWeather.isSunnyPlus ? 1.8f : 1.3f) : 1);
                ChangePoint = ChangePoint * (ParentPlayer.isReflect ? 0.75f : 1);
                ChangePointSp = ChangePointSp * (ParentPlayer.isLightScreen ? 0.75f : 1);
                //����޵н��������޵еĻ���Ϊ���޵�״̬���޵�ʱ���ʱ��ʱ������Ϊ�޵�ʱ��
                if (isSubstituteInvincible || ParentPlayer.isInvincibleAlways)
                {
                    return;
                }
                else
                {

                    if ((int)SkillType != 19)
                    {
                        NowHp = Mathf.Clamp(NowHp + (int)((ChangePoint / ParentPlayer.DefAbilityPoint + ChangePointSp / ParentPlayer.SpdAbilityPoint - 2) * (Type.TYPE[(int)SkillType][ParentPlayer.PlayerType01] * Type.TYPE[(int)SkillType][ParentPlayer.PlayerType02] * (ParentPlayer.PlayerTeraTypeJOR == 0 ? Type.TYPE[(int)SkillType][ParentPlayer.PlayerTeraType] : Type.TYPE[(int)SkillType][ParentPlayer.PlayerTeraTypeJOR])) * ((ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])))), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                        Debug.Log("xxxxx" + ((int)((ChangePoint / ParentPlayer.DefAbilityPoint + ChangePointSp / ParentPlayer.SpdAbilityPoint - 2) * (Type.TYPE[(int)SkillType][ParentPlayer.PlayerType01] * Type.TYPE[(int)SkillType][ParentPlayer.PlayerType02] * (ParentPlayer.PlayerTeraTypeJOR == 0 ? Type.TYPE[(int)SkillType][ParentPlayer.PlayerTeraType] : Type.TYPE[(int)SkillType][ParentPlayer.PlayerTeraTypeJOR])) * ((ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]) > 0 ? Mathf.Pow(1.2f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType])) : Mathf.Pow(0.8f, (ParentPlayer.playerData.TypeDefAlways[(int)SkillType] + ParentPlayer.playerData.TypeDefJustOneRoom[(int)SkillType]))))));
                    }
                    else
                    {
                        NowHp = Mathf.Clamp(NowHp + Mathf.Clamp((int)ChangePoint, -100000, -1), (NowHp > 1) ? (ParentPlayer.playerData.isEndure ? 1 : 0) : 0, MaxHP);
                    }
                    uIHealth.Per = (float)NowHp / (float)MaxHP;
                    uIHealth.ChangeHpDown();
                    isSubstituteInvincible = true;

                    if (NowHp <= 0) {
                        animator.SetTrigger("Die");
                        if (isDieBoom)
                        {
                            Instantiate(DieBoom, transform.position, Quaternion.identity).player = ParentPlayer;
                        }
                    }
                    //Ѫ������ʱ��Ѫ��UI�����ǰѪ����������Ѫ�������ĺ���

                    //���������Ķ�������������
                    animator.SetTrigger("Hit");
                }
            }
        }
    }

    private void Update()
    {
        if (isSubstituteInvincible)
        {
            InvincileTimer -= Time.deltaTime;
            /*
            //���޵�ʱ���ʱ�����е�ǰ0��15���ڱ�����
            if (InvincileTimer > 0.8f - 0.15f)
            {

                Vector2 position = rigidbody2D.position;
                position.x = Mathf.Clamp(position.x + koDirection.x * 2.2f * konckout * Time.deltaTime, NowRoom.x * 30 - 12, NowRoom.x * 30 + 12);
                position.y = Mathf.Clamp(position.y + koDirection.y * 2.2f * konckout * Time.deltaTime, NowRoom.y * 24 - 7.3f, NowRoom.y * 24 + 7.3f);
                rigidbody2D.position = position;
            }
            */
            if (InvincileTimer <= 0)
            {
                isSubstituteInvincible = false;
                InvincileTimer = 0.8f;
            }
        }
    }

    // Start is called before the first frame update
    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if(e != null && !e.isSubsititue)
            {
                e.isSubsititue = true;
                e.SubsititueTarget = this.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null && e.isSubsititue)
            {
                e.isSubsititue = false;
                e.SubsititueTarget = null;
                Snorlax snorlax = e.GetComponent<Snorlax>();
                if (snorlax != null && snorlax.isSlam)
                {
                    e.isSubsititue = true;
                    e.SubsititueTarget = this.gameObject;
                }
            }
        }
    }
}
