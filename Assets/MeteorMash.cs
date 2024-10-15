using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMash : Skill
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null)
            {
                HitAndKo(e);
                if (SkillFrom == 2)
                {
                    Debug.Log((float)(PokemonType.TYPE[9][(int)e.EmptyType01]));
                    Debug.Log((float)(PokemonType.TYPE[9][(int)e.EmptyType02]));
                    Debug.Log(Mathf.Pow(1.2f, -e.TypeDef[9]));
                    if ((float)(PokemonType.TYPE[9][(int)e.EmptyType01]) * (float)(PokemonType.TYPE[9][(int)e.EmptyType02] * Mathf.Pow(1.2f , -e.TypeDef[9])) > 1.0f)
                    {
                        if (player.playerData.DefBounsJustOneRoom <= 8) {
                            player.playerData.DefBounsJustOneRoom++;
                        }
                        if (player.playerData.SpDBounsJustOneRoom <= 8)
                        {
                            player.playerData.SpDBounsJustOneRoom++;
                        }
                        player.ReFreshAbllityPoint();
                    }
                    //if (e.EmptyType01) { }
                }
                if (Random.Range(0.0f, 1.0f) + (float)(player.LuckPoint / 20) >= 0.8f)
                {
                    if (player.playerData.AtkBounsJustOneRoom <= 8) {
                        player.playerData.AtkBounsJustOneRoom++;
                    }
                    player.ReFreshAbllityPoint();

                }
            }
        }
    }
}
