using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryTree : MonoBehaviour
{
    Animator animator;
    public RandomBerryTypeDef Berry;
    public HealthUpCCg CCG;
    public SpaceItem WY;
    public PokemonBall PB;
    public PlayerControler player;


    bool BanUp;
    bool BanDown;
    bool BanRight;
    bool BanLeft;

    int r;
    int BanCount;


    private void Start()
    {
        animator = GetComponent<Animator>();
        CheckBan();
    }


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerControler>();
            if (player != null)
            {
                animator.SetTrigger("Drop");
            }
        }
    }

    public void DropABerry()
    {
        Debug.Log(BanUp + "+" + BanDown + "+" + BanRight + "+" + BanLeft);
        float PandomPoint = (player.playerData.IsPassiveGetList[0] ? (Random.Range(0.0f, 1.0f)) : (Random.Range(0.0f, 1.025f)));
        if (PandomPoint <= 0.35f)
        {
            RandomBerryTypeDef b = Instantiate(Berry, transform.position + ((Quaternion.AngleAxis(r, Vector3.forward) * Vector3.right).normalized), Quaternion.identity, transform.parent.parent);
            b.isLunch = true;
            b.BanLunchUp = BanUp;
            b.BanLunchDown = BanDown;
            b.BanLunchRight = BanRight;
            b.BanLunchLeft = BanLeft;
        }
        else if(PandomPoint > 0.35f && PandomPoint <= 0.90f)
        {
            HealthUpCCg b = Instantiate(CCG, transform.position + ((Quaternion.AngleAxis(r, Vector3.forward) * Vector3.right).normalized), Quaternion.identity, transform.parent.parent);
            b.isLunch = true;
            b.BanLunchUp = BanUp;
            b.BanLunchDown = BanDown;
            b.BanLunchRight = BanRight;
            b.BanLunchLeft = BanLeft;
        }
        else if (PandomPoint > 0.90f && PandomPoint <= 1.0f)
        {
            SpaceItem b = Instantiate(WY, transform.position + ((Quaternion.AngleAxis(r, Vector3.forward) * Vector3.right).normalized), Quaternion.identity, transform.parent.parent);
            b.isLunch = true;
            b.BanLunchUp = BanUp;
            b.BanLunchDown = BanDown;
            b.BanLunchRight = BanRight;
            b.BanLunchLeft = BanLeft;
        }
        else
        {
            PokemonBall b = Instantiate(PB, transform.position + ((Quaternion.AngleAxis(r, Vector3.forward) * Vector3.right).normalized), Quaternion.identity, transform.parent.parent);
            b.isLunch = true;
            b.BanLunchUp = BanUp;
            b.BanLunchDown = BanDown;
            b.BanLunchRight = BanRight;
            b.BanLunchLeft = BanLeft;
        }
        Debug.Log(BanRight);
        Debug.Log(BanLeft);
        Debug.Log(BanUp);
        Debug.Log(BanDown);
    }


    void CheckBan()
    {
        RaycastHit2D CheckRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.right , 0.7f, LayerMask.GetMask("Enviroment", "Room" , "Water"));
        RaycastHit2D CheckLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left , 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        RaycastHit2D CheckUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up , 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        RaycastHit2D CheckDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.down , 0.7f, LayerMask.GetMask("Enviroment", "Room", "Water"));
        if (CheckRight.collider != null && CheckRight.collider.transform != transform.parent) { BanRight = true; }
        if (CheckLeft.collider != null && CheckLeft.collider.transform != transform.parent)    { BanLeft = true; }
        if (CheckUp.collider != null && CheckUp.collider.transform != transform.parent)          { BanUp = true;  }
        if (CheckDown.collider != null && CheckDown.collider.transform != transform.parent)    { BanDown = true;  }
        r = Random.Range(0, 360);
        while ((BanUp && r > 45 && r <= 135) || (BanLeft && r > 135 && r <= 225) || (BanDown && r > 225 && r <= 315) || (BanRight && (r > 315 || r <= 45)))
        {
            r = Random.Range(0, 360);
            BanCount++;
            if (BanCount >= 20)
            {
                break;
            }
        }
    }

}
