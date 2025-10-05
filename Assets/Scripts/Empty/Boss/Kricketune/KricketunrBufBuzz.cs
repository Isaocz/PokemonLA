using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KricketunrBufBuzz : MonoBehaviour
{
    public Empty ParentEmpty;
    public KricketunrBufBuzzChild ChildProjectile;

    float TotalTimer;
    float BornTimer;
    int isTurn = 1;

    float TurnRotation = 19;
    float ChildSpeed = 6.9f;
    float Timer = 0.28f;


    // Update is called once per frame
    void Update()
    {
        
        if (!ParentEmpty.isEmptyConfusionDone)
        {
            TurnRotation = 19;
            ChildSpeed = 7.7f;
            Timer = 0.265f;
        }
        else
        {
            TurnRotation = 25;
            ChildSpeed = 5f;
            Timer = 0.5f;
        }

        if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis)
        {
            TotalTimer += Time.deltaTime;
        }
        if ((ParentEmpty.isSleepDone || ParentEmpty.isFearDone) && TotalTimer < 4.95f) 
        {
            TotalTimer = 4.95f;
        }

        if (TotalTimer <= 5) {
            if (!ParentEmpty.isEmptyFrozenDone && !ParentEmpty.isCanNotMoveWhenParalysis && !ParentEmpty.isSleepDone)
            {
                BornTimer += Time.deltaTime;
            }
            if (BornTimer > Timer)
            {
                BornTimer = 0;

                Vector2 v1 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * Vector2.up;
                KricketunrBufBuzzChild p1 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p1.LaunchNotForce(v1 , ChildSpeed);
                p1.empty = ParentEmpty;
                p1.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)Vector2.up, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v2 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * Vector2.down;
                KricketunrBufBuzzChild p2 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p2.LaunchNotForce(v2, ChildSpeed);
                p2.empty = ParentEmpty;
                p2.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v2, Vector3.up), Vector3.forward) * transform.rotation;


                if (!ParentEmpty.isEmptyConfusionDone)
                {
                    Vector2 v9 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, 1)).normalized;
                    KricketunrBufBuzzChild p9 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p9.LaunchNotForce(v9, ChildSpeed);
                    p9.empty = ParentEmpty;
                    p9.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v9, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v10 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, 1)).normalized;
                    KricketunrBufBuzzChild p10 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p10.LaunchNotForce(v10, ChildSpeed);
                    p10.empty = ParentEmpty;
                    p10.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v10, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v11 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, -1)).normalized;
                    KricketunrBufBuzzChild p11 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p11.LaunchNotForce(v11, ChildSpeed);
                    p11.empty = ParentEmpty;
                    p11.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v11, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v12 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, -1)).normalized;
                    KricketunrBufBuzzChild p12 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p12.LaunchNotForce(v12, ChildSpeed);
                    p12.empty = ParentEmpty;
                    p12.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v12, Vector3.up), Vector3.forward) * transform.rotation;
                }

                else
                {
                    Vector2 v9 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, 1)).normalized;
                    KricketunrBufBuzzChild p9 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p9.LaunchNotForce(v9, ChildSpeed);
                    p9.empty = ParentEmpty;
                    p9.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v9, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v10 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, 1)).normalized;
                    KricketunrBufBuzzChild p10 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p10.LaunchNotForce(v10, ChildSpeed);
                    p10.empty = ParentEmpty;
                    p10.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v10, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v11 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, -1)).normalized;
                    KricketunrBufBuzzChild p11 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p11.LaunchNotForce(v11, ChildSpeed);
                    p11.empty = ParentEmpty;
                    p11.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v11, Vector3.up), Vector3.forward) * transform.rotation;

                    Vector2 v12 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, -1)).normalized;
                    KricketunrBufBuzzChild p12 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                    p12.LaunchNotForce(v12, ChildSpeed);
                    p12.empty = ParentEmpty;
                    p12.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v12, Vector3.up), Vector3.forward) * transform.rotation;


                }


                /*
                Vector2 v5 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1, 1.732051f)).normalized;
                KricketunrBufBuzzChild p5 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p5.LaunchNotForce(v5, ChildSpeed);
                p5.empty = ParentEmpty;
                p5.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360(v5, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v6 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1, 1.732051f)).normalized;
                KricketunrBufBuzzChild p6 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p6.LaunchNotForce(v6, ChildSpeed);
                p6.empty = ParentEmpty;
                p6.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v6, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v7 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1, -1.732051f)).normalized;
                KricketunrBufBuzzChild p7 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p7.LaunchNotForce(v7, ChildSpeed);
                p7.empty = ParentEmpty;
                p7.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v7, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v8 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1, -1.732051f)).normalized;
                KricketunrBufBuzzChild p8 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p8.LaunchNotForce(v8, ChildSpeed);
                p8.empty = ParentEmpty;
                p8.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v8, Vector3.up), Vector3.forward) * transform.rotation;


                Vector2 v9 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, 1)).normalized;
                KricketunrBufBuzzChild p9 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p9.LaunchNotForce(v9, ChildSpeed);
                p9.empty = ParentEmpty;
                p9.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v9, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v10 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, 1)).normalized;
                KricketunrBufBuzzChild p10 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p10.LaunchNotForce(v10, ChildSpeed);
                p10.empty = ParentEmpty;
                p10.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v10, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v11 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(1.732051f, -1)).normalized;
                KricketunrBufBuzzChild p11 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p11.LaunchNotForce(v11, ChildSpeed);
                p11.empty = ParentEmpty;
                p11.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v11, Vector3.up), Vector3.forward) * transform.rotation;

                Vector2 v12 = Quaternion.AngleAxis(TurnRotation * isTurn, Vector3.forward) * (new Vector2(-1.732051f, -1)).normalized;
                KricketunrBufBuzzChild p12 = Instantiate(ChildProjectile, transform.position, Quaternion.identity, transform);
                p12.LaunchNotForce(v12, ChildSpeed);
                p12.empty = ParentEmpty;
                p12.transform.rotation = Quaternion.AngleAxis(_mTool.Angle_360((Vector3)v12, Vector3.up), Vector3.forward) * transform.rotation;
                
                */
                isTurn++;
            }
        }
        else if (TotalTimer >= 15)
        {
            Destroy(gameObject);
        }
    }
}
