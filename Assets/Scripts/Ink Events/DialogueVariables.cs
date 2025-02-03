using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink;
using Ink.Runtime;
using System.IO;
public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        //compile the story
        Story globalVariableStory = new Story(loadGlobalsJSON.text);

        //initialize the story
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariableStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariableStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized local variables" + name + " = " + value);
        }
    }

    public void StartListening(Story story)
    {

        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChange;
    }
    
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChange;
    }

    private void VariableChange(string name, Ink.Runtime.Object value)
    {
        //Debug.Log("Variable changed: " + name + " = " + value);
        if (name == "has_grenade")
        {
            Debug.Log("has_grenade has been set to " + value);
        }
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name,value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
            //Debug.Log(variable.Key);
        }
    }


}
