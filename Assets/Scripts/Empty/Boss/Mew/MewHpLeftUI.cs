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
                case PokemonType.TypeEnum.Normal: outline.effectColor = PokemonType.TypeColor[0]; break;
                case PokemonType.TypeEnum.Fighting: outline.effectColor = PokemonType.TypeColor[1]; break;
                case PokemonType.TypeEnum.Flying: outline.effectColor = PokemonType.TypeColor[2]; break;
                case PokemonType.TypeEnum.Poison: outline.effectColor = PokemonType.TypeColor[3]; break;
                case PokemonType.TypeEnum.Ground: outline.effectColor = PokemonType.TypeColor[4]; break;
                case PokemonType.TypeEnum.Rock: outline.effectColor = PokemonType.TypeColor[5]; break;
                case PokemonType.TypeEnum.Bug: outline.effectColor = PokemonType.TypeColor[6]; break;
                case PokemonType.TypeEnum.Ghost: outline.effectColor = PokemonType.TypeColor[7]; break;
                case PokemonType.TypeEnum.Steel: outline.effectColor = PokemonType.TypeColor[8]; break;
                case PokemonType.TypeEnum.Fire: outline.effectColor = PokemonType.TypeColor[9]; break;
                case PokemonType.TypeEnum.Water: outline.effectColor = PokemonType.TypeColor[10]; break;
                case PokemonType.TypeEnum.Grass: outline.effectColor = PokemonType.TypeColor[11]; break;
                case PokemonType.TypeEnum.Electric: outline.effectColor = PokemonType.TypeColor[12]; break;
                case PokemonType.TypeEnum.Psychic: outline.effectColor = PokemonType.TypeColor[13]; break;
                case PokemonType.TypeEnum.Ice: outline.effectColor = PokemonType.TypeColor[14]; break;
                case PokemonType.TypeEnum.Dragon: outline.effectColor = PokemonType.TypeColor[15]; break;
                case PokemonType.TypeEnum.Dark: outline.effectColor = PokemonType.TypeColor[16]; break;
                case PokemonType.TypeEnum.Fairy: outline.effectColor = PokemonType.TypeColor[17]; break;
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
