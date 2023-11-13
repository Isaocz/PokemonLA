using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MewHpLeftUI : MonoBehaviour
{
    private Text healthText;
    private Empty ParentEmpty;
    private Mew mew;
    private Outline outline;
    void Start()
    {
        ParentEmpty = transform.parent.parent.GetComponent<Empty>();
        mew = transform.parent.parent.GetComponent<Mew>();
        healthText = GetComponent<Text>();
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (ParentEmpty != null)
        {
            switch (mew.SkillType)
            {
                case Type.TypeEnum.Normal: outline.effectColor = Type.TypeColor[0]; break;
                case Type.TypeEnum.Fighting: outline.effectColor = Type.TypeColor[1]; break;
                case Type.TypeEnum.Flying: outline.effectColor = Type.TypeColor[2]; break;
                case Type.TypeEnum.Poison: outline.effectColor = Type.TypeColor[3]; break;
                case Type.TypeEnum.Ground: outline.effectColor = Type.TypeColor[4]; break;
                case Type.TypeEnum.Rock: outline.effectColor = Type.TypeColor[5]; break;
                case Type.TypeEnum.Bug: outline.effectColor = Type.TypeColor[6]; break;
                case Type.TypeEnum.Ghost: outline.effectColor = Type.TypeColor[7]; break;
                case Type.TypeEnum.Steel: outline.effectColor = Type.TypeColor[8]; break;
                case Type.TypeEnum.Fire: outline.effectColor = Type.TypeColor[9]; break;
                case Type.TypeEnum.Water: outline.effectColor = Type.TypeColor[10]; break;
                case Type.TypeEnum.Grass: outline.effectColor = Type.TypeColor[11]; break;
                case Type.TypeEnum.Electric: outline.effectColor = Type.TypeColor[12]; break;
                case Type.TypeEnum.Psychic: outline.effectColor = Type.TypeColor[13]; break;
                case Type.TypeEnum.Ice: outline.effectColor = Type.TypeColor[14]; break;
                case Type.TypeEnum.Dragon: outline.effectColor = Type.TypeColor[15]; break;
                case Type.TypeEnum.Dark: outline.effectColor = Type.TypeColor[16]; break;
                case Type.TypeEnum.Fairy: outline.effectColor = Type.TypeColor[17]; break;
                default: outline.effectColor = Color.white; break;
            }

            if (mew.currentPhase != 3)
            {
                float healthPercentage = (float)ParentEmpty.EmptyHp / ParentEmpty.maxHP * 100f;
                if (ParentEmpty.Invincible)
                {
                    healthText.color = new Color(Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time + 0.5f, 1), Mathf.PingPong(Time.time + 1f, 1));
                    healthText.text = "(Invincible) " + healthPercentage.ToString("F1") + "%";

                }
                else
                {
                    healthText.color = Color.black;
                    healthText.text = healthPercentage.ToString("F1") + "%";
                }
            }
            else
            {
                float healthPercentage = ParentEmpty.uIHealth.Per * 100f;
                healthText.color = new Color(Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time + 0.5f, 1), Mathf.PingPong(Time.time + 1f, 1));
                healthText.text = "(Invincible) " + healthPercentage.ToString("F1") + "%";
            }
        }
    }
}
