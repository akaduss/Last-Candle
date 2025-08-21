using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static event Action<float> OnCandleLit;

    [Header("Candle Settings")]
    public float flameTime;
    private float flameTimeLeft;

    [Header("UI")]
    public TextMeshProUGUI lightText;
    public Slider flameSlider;

    public float Light;
    public bool isLit;
    InputAction playAct;
    CandleAnimator candleAnimator;
    PlayerInput playerInput;

    void Start()
    {
        // Keep Start lean; initialization is handled in Awake/OnEnable to be
        // robust when Domain Reload is disabled.
        Light = 0;
        flameSlider.value = 0;
        flameSlider.maxValue = flameTime;
    }

    void Awake()
    {
        isLit = false;
        candleAnimator = GetComponent<CandleAnimator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        // Prefer the PlayerInput's action asset if present, otherwise fall back
        // to the global InputSystem.actions. This is safe when domain reload
        // is disabled because we resolve actions at enable time.
        var asset = playerInput != null ? playerInput.actions : InputSystem.actions;
        playAct = asset != null ? asset.FindAction("Interact") : null;

        if (playAct != null)
        {
            playAct.performed += PlayAct_performed;
            if (!playAct.enabled)
                playAct.Enable();
        }
        else
        {
            Debug.LogWarning("Interact action not found on InputActionAsset.");
        }
    }

    void OnDisable()
    {
        if (playAct != null)
        {
            playAct.performed -= PlayAct_performed;
            if (playAct.enabled)
                playAct.Disable();
            playAct = null;
        }
    }

    private void PlayAct_performed(InputAction.CallbackContext obj)
    {
        print("Candle lit");
        if (isLit)
            return;

        OnCandleLit?.Invoke(flameTime);
        flameTimeLeft = flameTime;
        candleAnimator.burnTime = flameTime;
        candleAnimator.fireParticle.Play();
        isLit = true;
    }

    void Update()
    {
        if (isLit)
        {
            flameTimeLeft -= Time.deltaTime;
            IncreaseLight();

            lightText.text = "Light: " + Light.ToString("F1");
            flameSlider.value = flameTimeLeft;

            if (flameTimeLeft <= 0)
            {
                isLit = false;
                candleAnimator.fireParticle.Stop();
                flameTimeLeft = flameTime;
            }
        }
    }

    void IncreaseLight()
    {
        Light++;
        print(Light);
    }

    void OnDestroy()
    {
        // OnDisable already unsubscribes; keep OnDestroy safe in case it is
        // called without OnDisable.
        if (playAct != null)
            playAct.performed -= PlayAct_performed;
    }
}
