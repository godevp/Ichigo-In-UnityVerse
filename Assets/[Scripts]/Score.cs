using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
 
 Source file Name - Score.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: update for the score
 */
public class Score : MonoBehaviour
{

    public TMP_Text _text;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(DataSaver.Instance != null)
        _text.text = "Score: " + DataSaver.Instance.score;
    }
}
