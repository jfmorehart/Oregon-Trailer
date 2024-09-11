using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink;

public class triggerQuestTest : MonoBehaviour
{
    [SerializeField]
    TextAsset inkJson;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            centralEventHandler.StartEvent(inkJson);
        }
    }
}
