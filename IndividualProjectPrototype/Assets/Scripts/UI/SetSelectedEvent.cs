using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SetSelectedEvent : MonoBehaviour
{
    public void setSelectedEvent(string name)
    {
        GameObject obj = GameObject.Find(name);
        if(obj.GetComponent<TMP_InputField>().interactable == false)
        {
            obj.GetComponent<TMP_InputField>().interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(obj);
    }
    
}
