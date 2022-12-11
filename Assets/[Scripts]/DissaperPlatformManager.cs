using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - DissaperPlatformManager.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: manager for dissapear platform which will respawn and deactivate by the enumerator

 */
public class DissaperPlatformManager : MonoBehaviour
{
    [SerializeField] float deactivateTime = 2.0f;
    [SerializeField] float respawnTime = 5.5f;

    public IEnumerator DeactivAndActive(GameObject go)
    {
        
        if (go.GetComponent<DissppearPlatform>()._active)
        {
            yield return new WaitForSeconds(deactivateTime);
            go.GetComponent<DissppearPlatform>()._active = false;
            StartCoroutine(DeactivAndActive(go));
        }
        else
        {
            yield return new WaitForSeconds(respawnTime);
            go.GetComponent<DissppearPlatform>()._active = true;
        }
        go.SetActive(go.GetComponent<DissppearPlatform>()._active);
        

    }
}
