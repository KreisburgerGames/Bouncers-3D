using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEnergyBarFill : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    private float barWidth;
    public float snappiness;
    void Start()
    {
        barWidth = GetComponent<RectTransform>().sizeDelta.x;   
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = Mathf.Lerp(GetComponent<Image>().fillAmount, player.energy / player.maxEnergy, snappiness * Time.deltaTime);
    }
}
