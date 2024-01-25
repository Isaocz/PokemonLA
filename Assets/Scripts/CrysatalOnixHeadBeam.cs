using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrysatalOnixHeadBeam : MonoBehaviour
{
    public int LookX;
    Animator animator;

    public CrystalOnix ParentCrystalOnix;
    float HeadBeamTimer;
    ParticleSystem PS;
    bool isPSDestroy;

    public GameObject StartPS;
    bool isStartPSBorn;

    bool isSPUP;

    public CrysatalOnixMeteorBeam Beam;
    bool isBeamBorn;
    CrysatalOnixMeteorBeam BeamObj;

    // Start is called before the first frame update
    void Start()
    {
        PS = transform.GetChild(4).GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        if (LookX == -1)
        {
            transform.position = ParentCrystalOnix.transform.parent.position + Vector3.right * 8.0f;
            animator.SetFloat("LookX", -1.0f);
        }
        else if (LookX == 1)
        {
            transform.position = ParentCrystalOnix.transform.parent.position - Vector3.right * 8.0f;
            animator.SetFloat("LookX", 1.0f);
        }
        GetComponent<OnixTailSubEmpty>().ParentEmpty = ParentCrystalOnix;
    }

    // Update is called once per frame
    void Update()
    {
        HeadBeamTimer += Time.deltaTime;
        if (HeadBeamTimer >= 0.9f && !isStartPSBorn)
        {
            isStartPSBorn = true;
            Instantiate(StartPS , transform.position + Vector3.up * 2.0f + Vector3.right * (float)LookX , Quaternion.identity,transform);
        }
        if (HeadBeamTimer >= 2.2f && !isSPUP)
        {
            isSPUP = true;
            ParentCrystalOnix.SpAChange(3 , 10.0f);
        }
        if (HeadBeamTimer >= 3.0f && !isBeamBorn)
        {
            isBeamBorn = true;
            BeamObj = Instantiate(Beam , transform.position + Vector3.up * 2.0f + Vector3.right * (float)LookX , Quaternion.Euler(0,0, ((LookX == 1)? -60 : 240 ) ));
            BeamObj.empty = ParentCrystalOnix;
        }
        if (HeadBeamTimer >= 3.3f && HeadBeamTimer <= 4.5f)
        {
            if (LookX == 1)
            {
                BeamObj.transform.rotation = Quaternion.Euler(0, 0, BeamObj.transform.rotation.eulerAngles.z + Time.deltaTime *100.0f);
            }
            else
            {
                BeamObj.transform.rotation = Quaternion.Euler(0, 0, BeamObj.transform.rotation.eulerAngles.z - Time.deltaTime * 100.0f);
            }
        }
        if (HeadBeamTimer >= 7.5f && !isPSDestroy)
        {
            isPSDestroy = true; Destroy(PS.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EmptyTouchHit(other.gameObject);
        }
    }

    public void EmptyTouchHit(GameObject player)
    {
        //如果触碰到的是玩家，使玩家扣除一点血量
        PlayerControler playerControler = player.gameObject.GetComponent<PlayerControler>();
        Pokemon.PokemonHpChange(ParentCrystalOnix.gameObject, player, 10, 0, 0, Type.TypeEnum.No);
        if (playerControler != null)
        {
            playerControler.KnockOutPoint = 5;
            playerControler.KnockOutDirection = (playerControler.transform.position - transform.position).normalized;
        }
    }


    public void DestroyFelf()
    {
        Destroy(gameObject);
    }

}
