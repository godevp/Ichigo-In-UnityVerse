using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 
 Source file Name - Bars.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: health bar update

 */
public class Bars : MonoBehaviour
{
    public Slider slider;
    public Slider manaSlider;
   [SerializeField] private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SetMaxHealth(player._maxHealth);
        SetHealth(player._health);
        manaSlider.value = player._maxMana;
        manaSlider.maxValue = player._maxMana;
       
    }


    private void Update()
    {
        SetHealth(player._health);
        manaSlider.value = player._mana;
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
