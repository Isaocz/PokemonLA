using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardCastform : Projectile
{

    ParticleSystem BlizzardMist;
    ParticleSystem BlizzardSnow;
    ParticleSystem BlizzardSnowBall;

    float RangeTimer;

    WeatherBall b1;
    WeatherBall b2;
    WeatherBall b3;
    WeatherBall b4;

    bool isStop;
    public bool isDoubleTeam;

    // Start is called before the first frame update
    void Start()
    {
        BlizzardMist = GetComponent<ParticleSystem>();
        BlizzardSnow = transform.GetChild(0).GetComponent<ParticleSystem>();
        BlizzardSnowBall = transform.GetChild(1).GetComponent<ParticleSystem>();
        b1 = transform.GetChild(2).GetComponent<WeatherBall>();
        b2 = transform.GetChild(3).GetComponent<WeatherBall>();
        b3 = transform.GetChild(4).GetComponent<WeatherBall>();
        b4 = transform.GetChild(5).GetComponent<WeatherBall>();
        b1.empty = empty;
        b2.empty = empty;
        b3.empty = empty;
        b4.empty = empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (!empty.isEmptyFrozenDone && !empty.isCanNotMoveWhenParalysis)
        {
            RangeTimer += Time.deltaTime;
            if (BlizzardMist.isPaused) { BlizzardMist.Play(); }
            if (BlizzardSnow.isPaused) { BlizzardSnow.Play(); }
            if (BlizzardSnowBall.isPaused) { BlizzardSnowBall.Play(); }
        }
        else
        {
            if (!BlizzardMist.isPaused) { BlizzardMist.Pause(); }
            if (!BlizzardSnow.isPaused) { BlizzardSnow.Pause(); }
            if (!BlizzardSnowBall.isPaused) { BlizzardSnowBall.Pause(); }
        }
        if ((empty.isSleepDone || empty.isFearDone || empty.isDie) && !isStop)
        {
            isStop = true;
        }

        
        if (RangeTimer < (isDoubleTeam ? 12.0f : 10.0f) && !isStop)
        {
            var MistRange = BlizzardMist.shape;
            var SnowRange = BlizzardSnow.shape;
            var SnowBallRange = BlizzardSnowBall.shape;
            MistRange.radiusThickness = Mathf.Clamp(MistRange.radiusThickness + ( Time.deltaTime * 0.55f) / (isDoubleTeam ? 12.0f : 10.0f), 0.1f , 0.65f);
            SnowRange.radiusThickness = Mathf.Clamp(SnowRange.radiusThickness + ( Time.deltaTime * 0.55f) / (isDoubleTeam ? 12.0f : 10.0f), 0.1f , 0.65f);
            SnowBallRange.radiusThickness = Mathf.Clamp(SnowBallRange.radiusThickness + ( Time.deltaTime * 0.55f) / (isDoubleTeam ? 12.0f : 10.0f), 0.1f , 0.65f);
            MistRange.radius = Mathf.Clamp(MistRange.radius - (Time.deltaTime * 9 ) / (isDoubleTeam ? 12.0f : 10.0f), 9, 18);
            SnowRange.radius = Mathf.Clamp(SnowRange.radius - (Time.deltaTime * 9) / (isDoubleTeam ? 12.0f : 10.0f), 9, 18);
            SnowBallRange.radius = Mathf.Clamp(SnowBallRange.radius - (Time.deltaTime * 9) / (isDoubleTeam ? 12.0f : 10.0f), 9, 18);

            var MistSpeed = BlizzardMist.velocityOverLifetime;
            var SnowBallSpeed = BlizzardSnowBall.velocityOverLifetime;
            MistSpeed.orbitalZMultiplier = Mathf.Clamp(MistSpeed.orbitalZMultiplier + (Time.deltaTime * 0.7f) / (isDoubleTeam ? 12.0f : 10.0f), 0.9f, 1.6f);
            SnowBallSpeed.orbitalZMultiplier = Mathf.Clamp(SnowBallSpeed.orbitalZMultiplier + (Time.deltaTime * 1.0f) / (isDoubleTeam ? 12.0f : 10.0f), 1.7f, 2.7f);
        }
        if (isStop)
        {
            var MistEmission = BlizzardMist.emission;
            var SnowEmission = BlizzardSnow.emission;
            var SnowBallEmission = BlizzardSnowBall.emission;
            MistEmission.enabled = false;
            SnowEmission.enabled = false;
            SnowBallEmission.enabled = false;
        }


        if(RangeTimer > 12 || isStop)
        {
            if (b1 != null) { b1.Die(); }
            if (b2 != null) { b2.Die(); }
            if (b3 != null) { b3.Die(); }
            if (b4 != null) { b4.Die(); }
        } 
        else
        {
            if (!empty.isEmptyConfusionDone) {
                if (b1 != null) { b1.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 0, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b2 != null) { b2.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 90, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b3 != null) { b3.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 180, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b4 != null) { b4.gameObject.SetActive(true); b4.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 270, Vector3.forward) * Vector3.right * 4f + transform.position; }
            }
            else
            {
                if (b1 != null) { b1.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + -30, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b2 != null) { b2.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 90, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b3 != null) { b3.transform.position = Quaternion.AngleAxis(RangeTimer * (empty.isSilence ? 0.4f : 1) * (isDoubleTeam ? 80 : 110) + 210, Vector3.forward) * Vector3.right * 4f + transform.position; }
                if (b4 != null) { b4.gameObject.SetActive(false); }

            }
        }
        if (RangeTimer > 18) { Destroy(gameObject); }


    }

    public void BlizzardStop()
    {
        isStop = true;
    }

}
