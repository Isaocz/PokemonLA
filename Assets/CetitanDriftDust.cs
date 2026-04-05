using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanDriftDust : MonoBehaviour
{
    public ParticleSystem PSL;
    public ParticleSystem PSL2;
    public ParticleSystem PSL3;
    public ParticleSystem PSL4;
    public ParticleSystem PSR;
    public ParticleSystem PSR2;
    public ParticleSystem PSR3;
    public ParticleSystem PSR4;


    bool isStartL = false;
    bool isStartR = false;


    public void StartPSL()
    {
        if (!isStartL) {
            PSL.Play();
            PSL2.Play();
            PSL3.Play();
            PSL4.Play();
            var e = PSL.emission;
            e.enabled = true;
            var e2 = PSL2.emission;
            e2.enabled = true;
            var e3 = PSL3.emission;
            e3.enabled = true;
            var e4 = PSL4.emission;
            e4.enabled = true;
            isStartL = true;
        }
    }

    public void StartPSR()
    {
        if (!isStartR) {
            PSR.Play();
            PSR2.Play();
            PSR3.Play();
            PSR4.Play();
            var e = PSR.emission;
            e.enabled = true;
            var e2 = PSR2.emission;
            e2.enabled = true;
            var e3 = PSR3.emission;
            e3.enabled = true;
            var e4 = PSR4.emission;
            e4.enabled = true;
            isStartR = true;
        }
    }

    public void StopPSL()
    {
        if (isStartL) {
            var e = PSL.emission;
            e.enabled = false;
            var e2 = PSL2.emission;
            e2.enabled = false;
            var e3 = PSL3.emission;
            e3.enabled = false;
            var e4 = PSL4.emission;
            e4.enabled = false;
            isStartL = false;
        }
    }

    public void StopPSR()
    {
        if (isStartR) {
            var e = PSR.emission;
            e.enabled = false;
            var e2 = PSR2.emission;
            e2.enabled = false;
            var e3 = PSR3.emission;
            e3.enabled = false;
            var e4 = PSR4.emission;
            e4.enabled = false;
            isStartR = false;
        }
    }


    public void SetRotation(Vector2 d)
    {
        //transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(d,Vector2.right));
        var s1 = PSL.shape;
        var s2 = PSL2.shape;
        var s3 = PSR.shape;
        var s4 = PSR2.shape;
        var s5 = PSL3.shape;
        var s6 = PSR3.shape;
        var s7 = PSL4.shape;
        var s8 = PSR4.shape;
        s1.rotation = new Vector3(0,0, _mTool.Angle_360Y(d, Vector2.right)+75.0f);
        s2.rotation = new Vector3(0,0, _mTool.Angle_360Y(d, Vector2.right)+75.0f);
        s3.rotation = new Vector3(0,0, _mTool.Angle_360Y(d, Vector2.right)+255.0f);
        s4.rotation = new Vector3(0,0, _mTool.Angle_360Y(d, Vector2.right)+255.0f);
        s5.rotation = new Vector3(0, 0, _mTool.Angle_360Y(d, Vector2.right) + 255.0f);
        s6.rotation = new Vector3(0,0, _mTool.Angle_360Y(d, Vector2.right)+ 75.0f);
        s7.rotation = new Vector3(0, 0, _mTool.Angle_360Y(d, Vector2.right) + 255.0f);
        s8.rotation = new Vector3(0, 0, _mTool.Angle_360Y(d, Vector2.right) + 75.0f);

    }

}
