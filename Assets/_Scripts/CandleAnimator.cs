using UnityEngine;

public class CandleAnimator : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public float burnTime;
    private float curTime;

    private void Start()
    {

        curTime = burnTime;
    }

    private void Update()
    {
        if (!fireParticle.isPlaying)
            return;
        
        if (curTime < 0)
            curTime = burnTime;
        curTime -= Time.deltaTime;

        var mainMod = fireParticle.main;
        mainMod.startSize = Mathf.Lerp(0.3f, 1, curTime / burnTime);
        mainMod.startLifetime = Mathf.Lerp(0.3f, 1, curTime / burnTime);
        //print(Mathf.Lerp(0.3f, 1, curTime / burnTime) +"::" + curTime / burnTime);
    }


}
