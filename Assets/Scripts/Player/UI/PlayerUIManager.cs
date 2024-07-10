using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField] private TMP_Text healthText;

    [Header("Time Properties")]
    [SerializeField] private TMP_Text timeText;

    [Header("Kills Properties")]
    [SerializeField] private TMP_Text killsText;

    [Header("Damage Properties")]
    [SerializeField] private TMP_Text damageText;

    [Header("Speed Properties")]
    [SerializeField] private TMP_Text speedText;

    [Header("Rocks Properties")]
    [SerializeField] private TMP_Text rocksText;

    [Space]

    [Header("Referencias")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerThrowing playerThrowing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerCombat.health.ToString();
        timeText.text = $"{(int)GameManager.instance.timeTimer}";
        killsText.text = GameManager.instance.killCounter.ToString();
        damageText.text = String.Format("{0:0.0}", playerCombat.damage);
        speedText.text = String.Format("{0:0.0}", playerMovement.movementSpeed);
        rocksText.text = playerThrowing.rocksCount.ToString();
    }
}
