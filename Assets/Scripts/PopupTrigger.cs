using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PopupTrigger : MonoBehaviour
{
    
    public flavor[] flavors = new flavor[3];
    bool activated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            if (collision.transform.CompareTag("Player"))
            {
                Debug.Log("Player hits this ");
                for (int i = 0; i < flavors.Length; i++)
                {
                    if (flavors[i].face != null)
                        PopupManager.instance.popupBehavior(flavors[i]);
                }
                activated = true;
            }
        }
    }

}
