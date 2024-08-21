using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthLow : MonoBehaviour
{
    public Player player;
    private TMP_Text text;
    private HealthBar healthBar;
    [Range(0.01f, 0.50f)]
    public float colorChangeSpeed;
    private float colorChangeTimer;
    public float timeUntilFadeout;
    private float fadeoutTimer;
    public float fadeoutSpeed;
    [Range(0.01f, 0.50f)]
    public float shakeSpeed;
    public float shakeStrength;
    private float shakeTimer = 0f;
    private Vector2 originalPos;
    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }
    private void Awake()
    {
        healthBar = FindFirstObjectByType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar == null) return;
        if (player.GetHealth() < healthBar.healthUntilShake)
        {
            text.enabled = true;
            if (colorChangeTimer >= colorChangeSpeed)
            {
                colorChangeTimer = 0;
                Color color = new Color(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), text.color.a);
                text.color = color;
            }
            else colorChangeTimer += Time.deltaTime;
            if (fadeoutTimer >= timeUntilFadeout)
            {
                Color color = text.color;
                color.a -= fadeoutSpeed * Time.deltaTime;
                text.color = color;
            }
            else fadeoutTimer += Time.deltaTime;
            if (shakeTimer < shakeSpeed)
            {
                shakeTimer += Time.deltaTime;
            }
            else
            {
                shakeTimer = 0f;
                rect.anchoredPosition = new Vector2(originalPos.x + Random.Range(-shakeStrength, shakeStrength), originalPos.y + Random.Range(-shakeStrength, shakeStrength));
            }
        }
        else
        {
            text.enabled = false;
            Color color = text.color;
            color.a = 1;
            text.color = color;
            fadeoutTimer = 0;
            colorChangeTimer = 0;
        }
    }
}
