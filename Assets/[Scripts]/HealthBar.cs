using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 
 Source file Name - HealthBar.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: health bar update

 */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
   [SerializeField] private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SetMaxHealth(player._maxHealth);
        SetHealth(player._health);
    }


    private void Update()
    {
        SetHealth(player._health);
    }
    public void SetMaxHealth(float mHealth)
    {
        slider.maxValue = mHealth;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health; 
    }
}
