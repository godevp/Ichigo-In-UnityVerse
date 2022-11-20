using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        _text.text = "Score: " + player._score.ToString();
    }
}
