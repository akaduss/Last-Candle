using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    public float Light;
    public bool isLit;
    InputAction playAct;
    CandleAnimator candleAnimator;

    void Start()
    {
        isLit = false;
        candleAnimator = GetComponent<CandleAnimator>();
        playAct = InputSystem.actions.FindAction("Interact");
        playAct.performed += PlayAct_performed; ;
    }

    private void PlayAct_performed(InputAction.CallbackContext obj)
    {
        if (isLit)
            return;

        candleAnimator.fireParticle.Play();
        isLit = true;
    }

    void Update()
    {
        if (isLit)
        {
            IncreaseLight();
        }
    }

    void IncreaseLight()
    {
        Light++;
        print(Light);
    }
}
