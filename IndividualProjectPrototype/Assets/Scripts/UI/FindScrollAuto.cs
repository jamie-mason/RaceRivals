using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindScrollAuto : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ScrollRectAutoScroll> autoScrolls;
    void Start()
    {
        var scrolls = FindObjectsOfType<ScrollRectAutoScroll>();
        foreach (var scroll in scrolls){
            autoScrolls.Add(scroll);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
