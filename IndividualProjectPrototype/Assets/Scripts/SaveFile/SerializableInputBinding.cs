using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class SerializableInputBinding
{
    public string name;
    public string path;
    public string interactions; // Include any other properties you need
    public string groups; // The action groups associated with the binding
    public string action; // The action name this binding belongs to

    // Constructor to create from InputBinding

    public SerializableInputBinding()
    {
        name = "";
        path = "";
        interactions = "";
        groups = "";
        action = ""; 
    }
    public SerializableInputBinding(InputBinding binding)
    {
        name = binding.name;
        path = binding.path;
        interactions = binding.interactions;
        groups = binding.groups;
        action = binding.action; // This assumes you can access the action name
    }
    

    // Convert back to InputBinding
    public InputBinding ToInputBinding()
    {
        return new InputBinding
        {
            name = name,
            path = path,
            interactions = interactions,
            groups = groups,
            action = action
        };
    }
    ~SerializableInputBinding()
    {
        name = "";
        path = "";
        interactions = "";
        groups = "";
        action = "";
    }
}
