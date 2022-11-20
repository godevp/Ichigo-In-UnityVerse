using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
 
 Source file Name - DissppearPlatform.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: attached to each dissapear platform and on trigger enter from player will start couroutine from manager

 */
public class DissppearPlatform : MonoBehaviour
{
    public bool _active = true;
    private DissaperPlatformManager manager;

    private void Start()
    {
        manager = FindObjectOfType<DissaperPlatformManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(manager.DeactivAndActive(gameObject));
        }
    }
}
