using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewTail : MonoBehaviour
{
    public bool isBorn;
    public GameObject MewNPC;

    Vector2Int Director;
    Rigidbody2D MewRigidbody;
    Animator MewAnimator;
    Vector2 NowPostion;

    float TurnTimer;
    float TurmTime;

    bool isUpEmpty;
    bool isDownEmpty;
    bool isRightEmpty;
    bool isLeftEmpty;

    // Start is called before the first frame update
    void Start()
    {
        switch (Random.Range(0,4))
        {
            case 0: Director = Vector2Int.up; break;
            case 1: Director = Vector2Int.down; break;
            case 2: Director = Vector2Int.left; break;
            case 3: Director = Vector2Int.right; break;
        }
        MewRigidbody = GetComponent<Rigidbody2D>();
        MewAnimator = GetComponent<Animator>();
        TurmTime = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBorn) {
            TurnTimer += Time.deltaTime;
            if (TurnTimer >= TurmTime)
            {
                Director = TurnDirector();
                TurnTimer = 0;
                TurmTime = Random.Range(0.5f, 1.2f);
            }
            if(NowPostion == (Vector2)transform.position)
            {
                Director = TurnDirector();
                TurnTimer = 0;
            }
            RaycastHit2D SearchPlayer = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Director, 8f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly"));
            if (SearchPlayer.collider != null && SearchPlayer.transform.tag == "Player") {
                Director = TurnDirector();
                TurnTimer = 0;
            }


            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2Int.up, 1f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly")).collider != null) { isUpEmpty = false; }
            else { isUpEmpty = true; }
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2Int.down, 1f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly")).collider != null) { isDownEmpty = false; }
            else { isDownEmpty = true; }
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2Int.right, 1f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly")).collider != null) { isRightEmpty = false; }
            else { isRightEmpty = true; }
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2Int.left, 1f, LayerMask.GetMask("Player", "Enviroment", "Room", "PlayerFly")).collider != null) { isLeftEmpty = false; }
            else { isLeftEmpty = true; }



            MewAnimator.SetFloat("LookX", Director.x);
        }
    }

    private void FixedUpdate()
    {
        if (!isBorn)
        {
            MewRigidbody.MovePosition(new Vector2(Mathf.Clamp((MewRigidbody.position.x + (float)Director.x * Time.deltaTime * 11.5f) , -7.8f , 7.8f) , Mathf.Clamp(( MewRigidbody.position.y + (float)Director.y * Time.deltaTime * 11.5f) , -3.5f , 4.4f )) );
            //MewRigidbody.MovePosition(new Vector2(((MewRigidbody.position.x + (float)Director.x * Time.deltaTime * 11.5f)) , (( MewRigidbody.position.y + (float)Director.y * Time.deltaTime * 11.5f))) );
            NowPostion = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
            if (player != null)
            {
                Instantiate(MewNPC, transform.position + Vector3.up * 0.8f, Quaternion.identity, transform.parent);
                Destroy(gameObject);
                
            }
        }
    }

    Vector2Int TurnDirector()
    {
        Vector2Int Output = Director;
        switch (Random.Range(0, 3))
        {
            case 0: 
                Output = -Director;
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = new Vector2Int(Director.y, Director.x);
                }
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = -new Vector2Int(Director.y, Director.x);
                }
                break;
            case 1:
                Output = new Vector2Int(Director.y , Director.x);
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = -new Vector2Int(Director.y, Director.x);
                }
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = -Director;
                }
                break;
            case 2:
                Output = -new Vector2Int(Director.y, Director.x);
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = -Director;
                }
                if ((Output == Vector2Int.up && !isUpEmpty) || (Output == Vector2Int.down && !isDownEmpty) || (Output == Vector2Int.right && !isRightEmpty) || (Output == Vector2Int.left && !isLeftEmpty))
                {
                    Output = new Vector2Int(Director.y, Director.x);
                }
                break;
        }

        return Output;
    }

}
