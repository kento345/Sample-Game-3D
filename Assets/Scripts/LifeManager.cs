using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image curentLife;
    [SerializeField] private TextMeshProUGUI curentNum;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curentLife.fillAmount = player.life.Value;
        curentNum.text = player.life.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        curentLife.fillAmount = player.life.Value / 100;
        curentNum.text = player.life.Value.ToString();
    }
}
