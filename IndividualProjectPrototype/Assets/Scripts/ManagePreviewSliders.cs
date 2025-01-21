using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePreviewSliders : MonoBehaviour
{
    [SerializeField] private SliderPreviews[] sliderPreviews;
    void Start()
    {
        sliderPreviews = FindObjectsOfType<SliderPreviews>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
