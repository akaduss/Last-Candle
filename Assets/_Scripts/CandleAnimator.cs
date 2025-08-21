using System;
using UnityEngine;

public class CandleAnimator : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public Light pointLight;
    public float lightIntensity = 1f;
    public float lightRange = 1f;
    public float burnTime;
    private float curTime;

    private void Start()
    {
        pointLight.intensity = 0;
        curTime = burnTime;
    }

    void OnEnable()
    {
        Game.OnCandleLit += Game_OnCandleLit;
    }

    void OnDisable()
    {
        Game.OnCandleLit -= Game_OnCandleLit;
    }

    private void Game_OnCandleLit(float flameTime)
    {
        fireParticle.Play();
        curTime = flameTime;
    }


    private void Update()
    {
        if (!fireParticle.isPlaying)
            return;


        curTime -= Time.deltaTime;

        var mainMod = fireParticle.main;
        mainMod.startSize = Mathf.Lerp(0.3f, 1, curTime / burnTime);
        mainMod.startLifetime = Mathf.Lerp(0.3f, 1, curTime / burnTime);

        //if normalized time < 0.5 lower range of light to %50 of original and intensity
        if (curTime / burnTime < 0.5f)
        {
            pointLight.intensity = Mathf.Lerp(lightIntensity * 0.5f, lightIntensity, curTime / (burnTime * 0.5f));
            pointLight.range = Mathf.Lerp(lightRange * 0.5f, lightRange, curTime / (burnTime * 0.5f));
        }
        else if (curTime / burnTime >= 0.5f)
        {
            pointLight.intensity = lightIntensity;
            pointLight.range = lightRange;
        }

        if (curTime <= 0)
        {
            fireParticle.Stop();
            pointLight.intensity = 0;
            pointLight.range = 0;
        }
        print("CandleAnimator Update: " + curTime);
    }


}
