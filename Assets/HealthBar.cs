using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    private float barWidth;
    public float snappiness;
    private RectTransform rect;
    private Vector2 originalPos;
    [Range(0.01f, 0.50f)]
    public float shakeSpeed;
    public float shakeStrength;
    public float healthUntilShake;
    private float shakeTimer = 0f;
    private float currentShakeStrength = 0f;
    public float transitionSpeed = 2f;

    void Start()
    {
        barWidth = GetComponent<RectTransform>().sizeDelta.x;
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = Mathf.Lerp(GetComponent<Image>().fillAmount, player.GetHealth() / player.maxHealth, snappiness * Time.deltaTime);
        if (shakeTimer < shakeSpeed)
        {
            shakeTimer += Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            rect.anchoredPosition = new Vector2(originalPos.x + Random.Range(-currentShakeStrength, currentShakeStrength), originalPos.y + Random.Range(-currentShakeStrength, currentShakeStrength));
        }
        if (player.GetHealth() <= healthUntilShake)
        {
            currentShakeStrength = shakeStrength;
        }
        else currentShakeStrength = Mathf.Lerp(currentShakeStrength, 0, transitionSpeed * Time.deltaTime);
    }
}
