using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyButterfly : MonoBehaviour
{

    public bool isAttack;
    public bool isCanNotAttack;
    public PlayerControler player;
    public Empty Target;
    public FairyButterfly.ButterflyType BFType;
    SpriteRenderer BFSprite;


    public bool isInfatuation
    {
        get { return isinfatuation; }
        set { isinfatuation = value; }
    }
    bool isinfatuation = false;


    public enum ButterflyType
    {
        ǳ��ɫ��ͨ��,
        ǳ��ɫ��Ѫ��,
        ��ɫ���ٹ�����,
        ��ɫ�����ع���,
        ��ɫ���ӹ�����,
    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<PlayerButterflyManger>().player;
        BFSprite = GetComponent<SpriteRenderer>();
        switch (BFType)
        {
            case ButterflyType.ǳ��ɫ��ͨ��:
                BFSprite.color = new Color(0.9150943f , 0.6517889f , 0.7923888f , 1);
                break;
            case ButterflyType.ǳ��ɫ��Ѫ��:
                BFSprite.color = new Color(0.8998755f, 0.9137255f, 0.6509804f, 1);
                break;
            case ButterflyType.��ɫ���ٹ�����:
                BFSprite.color = new Color(0.745283f, 0.3848812f, 0.3128782f, 1);
                break;
            case ButterflyType.��ɫ�����ع���:
                BFSprite.color = new Color(0.4316927f, 0.6190022f, 0.8396226f, 1);
                break;
            case ButterflyType.��ɫ���ӹ�����:
                BFSprite.color = new Color(0.8396226f, 0.5381053f, 0.2495105f, 1);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) { player = transform.parent.GetComponent<PlayerButterflyManger>().player; }

        if (isAttack)
        {
            if (Target != null) {
                transform.position += (Target.transform.position - transform.position).normalized * ((BFType == ButterflyType.��ɫ���ٹ�����)? 1.8f : 4f ) * Time.deltaTime;
            }
            else
            {
                transform.position += (player.transform.position - transform.position).normalized * ((BFType == ButterflyType.��ɫ���ٹ�����) ? 1.8f : 4f) * Time.deltaTime;
                if ((player.transform.position - transform.position).magnitude < 1.5f)
                {
                    isAttack = false;
                    Target = null;
                    transform.parent = player.transform.GetChild(5).GetChild(2);
                }
            }
        }

    }

    public void ResetType(FairyButterfly.ButterflyType RType)
    {
        BFType = RType;
        switch (BFType)
        {
            case ButterflyType.ǳ��ɫ��ͨ��:
                BFSprite.color = new Color(0.9150943f, 0.6517889f, 0.7923888f, 1);
                break;
            case ButterflyType.ǳ��ɫ��Ѫ��:
                BFSprite.color = new Color(0.8998755f, 0.9137255f, 0.6509804f, 1);
                break;
            case ButterflyType.��ɫ���ٹ�����:
                BFSprite.color = new Color(0.745283f, 0.3848812f, 0.3128782f, 1);
                break;
            case ButterflyType.��ɫ�����ع���:
                BFSprite.color = new Color(0.4316927f, 0.6190022f, 0.8396226f, 1);
                break;
            case ButterflyType.��ɫ���ӹ�����:
                BFSprite.color = new Color(0.8396226f, 0.5381053f, 0.2495105f, 1);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null && !e.Invincible)
            {
                if (isinfatuation) { e.EmptyInfatuation(15,0.4f); }
                if (player.SpAAbilityPoint >= player.AtkAbilityPoint)
                {
                    Pokemon.PokemonHpChange(player.gameObject , e.gameObject , 0 , ((BFType == ButterflyType.��ɫ���ٹ�����) ? 30 : 15), 0 , Type.TypeEnum.Fairy);
                }
                else
                {
                    Pokemon.PokemonHpChange(player.gameObject, e.gameObject, ((BFType == ButterflyType.��ɫ���ٹ�����) ? 30 : 15), 0, 0, Type.TypeEnum.Fairy);
                }
                if (BFType == ButterflyType.ǳ��ɫ��Ѫ��)
                {
                    Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, Mathf.Clamp(player.maxHp/16 , 1 , 20) , Type.TypeEnum.IgnoreType);
                }
                if (BFType == ButterflyType.��ɫ�����ع���)
                {
                    player.ChangeHPW(new Vector2Int(4,1));
                }
                if (BFType == ButterflyType.��ɫ���ӹ�����)
                {
                    player.ChangeHPW(new Vector2Int(2, 1));
                }
                Destroy(gameObject);
            }
            
        }
    }
}
