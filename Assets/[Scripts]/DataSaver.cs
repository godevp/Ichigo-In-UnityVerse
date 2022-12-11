using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    /*
 
 Source file Name - DataSaver.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 12/11/2022
 Program description: Used for score between levels

 */
    private static DataSaver instance = null;
    private PlayerController player;

    public int score = 0;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    public static DataSaver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataSaver>();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        DataSaver original = Instance;
        DataSaver[] saverClones = FindObjectsOfType<DataSaver>();
        foreach (DataSaver saver in saverClones)
        {
            if (saver != original)
            {
                Destroy(saver.gameObject);
            }
        }
    }

    private void Update()
    {
        if(player != null)
        {
            score = player._score;
        }
    }
}
