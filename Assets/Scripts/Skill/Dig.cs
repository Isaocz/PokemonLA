using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : Skill
{

    GameObject PlayerSpriteParent;
    GameObject PlayerShadow01;
    GameObject PlayerShadow02;
    public GameObject SoftMud;
    public GameObject BigSoftMud;
    bool isOverMudBorn;

    GameObject OverPS;
    GameObject MovePS;
    Vector3 StartScale;
    Vector3 StartPosition;
    Vector3 MoveDirector;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.rotation.eulerAngles == Vector3.zero) { MoveDirector = Vector3.right; }
        else if (transform.rotation.eulerAngles == new Vector3(0,0,90)) { MoveDirector = Vector3.up; }
        else if (transform.rotation.eulerAngles == new Vector3(0,0,180)) { MoveDirector = Vector3.left; }
        else if (transform.rotation.eulerAngles == new Vector3(0,0,270)) { MoveDirector = Vector3.down; }

        if (SkillFrom == 2)
        {
            Instantiate(BigSoftMud, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(SoftMud, transform.position, Quaternion.identity);
        }
        OverPS = transform.GetChild(2).gameObject;
        MovePS = transform.GetChild(3).gameObject;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        UseSpaceItem.RemoveFly(player);
        PlayerShadow01 = player.transform.GetChild(0).gameObject;
        PlayerShadow02 = player.transform.GetChild(1).gameObject;
        PlayerSpriteParent = player.transform.GetChild(3).gameObject;
        StartScale = PlayerSpriteParent.transform.localScale;
        StartPosition = PlayerSpriteParent.transform.localPosition;
        player.isCanNotMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        ResetPlayer();

        if (ExistenceTime > 2)
        {
            PlayerSpriteParent.transform.localScale -= new Vector3(0, Time.deltaTime, 0);
            PlayerShadow01.SetActive(false);
            PlayerShadow02.SetActive(false);
        }else if (ExistenceTime <= 1.5f)
        {
            if (!isOverMudBorn && SkillFrom == 2)
            {
                isOverMudBorn = true;
                Instantiate(SoftMud, transform.position, Quaternion.identity);
            }
            if (ExistenceTime >= 1f)
            {
                PlayerSpriteParent.transform.localScale += new Vector3(0, 2 * Time.deltaTime, 0);
            }
            OverPS.SetActive(true);
            MovePS.SetActive(false);
            PlayerShadow01.SetActive(true);
            PlayerShadow02.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (ExistenceTime <= 2 && ExistenceTime > 1.5f)
        {
            player.isInvincibleAlways = true;
            MovePS.SetActive(true);
            PlayerSpriteParent.transform.localScale = new Vector3(1,0,1);
            Vector2 position = player.transform.position;
            position.x += MoveDirector.x * Time.deltaTime * 10;
            position.y += MoveDirector.y * Time.deltaTime * 10;
            player.transform.position = position;
        }
    }

    void ResetPlayer()
    {
        if ((player == null && baby == null) || PlayerShadow01 == null)
        {
            player = GameObject.FindObjectOfType<PlayerControler>();
            UseSpaceItem.RemoveFly(player);
            PlayerShadow01 = player.transform.GetChild(0).gameObject;
            PlayerShadow02 = player.transform.GetChild(1).gameObject;
            PlayerSpriteParent = player.transform.GetChild(3).gameObject;
            StartScale = PlayerSpriteParent.transform.localScale;
            StartPosition = PlayerSpriteParent.transform.localPosition;
            player.isCanNotMove = true;
        }
    }


    private void OnDestroy()
    {
        ResetPlayer();

        PlayerSpriteParent.transform.localScale = player.PlayerLocalScal;
        PlayerSpriteParent.transform.localPosition = player.PlayerLocalPosition;
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
        if (!player.isEvolution) {
            PlayerShadow01.SetActive(true);
            PlayerShadow02.SetActive(true);
        }
    }

    


}
