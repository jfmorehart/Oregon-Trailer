using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink;
using Ink.Runtime;
using System.IO;
public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    public DialogueVariables(string globalFilesPath)
    {
        //compile the story
        string filecontents = File.ReadAllText(globalFilesPath);
        Ink.Compiler compiler = new Ink.Compiler(filecontents);
        Story globalVariableStory = compiler.Compile();

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
        Debug.Log("Variable changed: " + name + " = " + value);
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name,value);
        }
    }

    //this copies the variables from Ink to Unity -- You do not need to touch this
    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
            //Debug.Log(variable.Key);
        }
    }


}