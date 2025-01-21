using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineTriggrer : MonoBehaviour
{

    [SerializeField] GameObject FinishLineEnter;
    [SerializeField] GameObject FinishLineExit;
    [SerializeField] GameObject FinishLineBlock;

    [SerializeField] public bool canCrossFinishLine;
    bool crossed;
    bool startCrossing;
    [SerializeField] private FinishLineTriggrer[] otherInstances;
    [SerializeField] private List<FinishLineTriggrer> otherInstancesList;
    [SerializeField] private Collider[] objectColliders;
    private Collider finishLineBlockCollider;


    TrackPlayerPosition trackPlayerPosition;



    void getOtherInstances()
    {
       

    }
    
    private void Start()
    {
        trackPlayerPosition = FindObjectOfType<TrackPlayerPosition>();
        getOtherInstances();
        objectColliders = FindAnyComponentInChildren<Collider>(this.gameObject);

    }
    private void Update()
    {

        if (canCrossFinishLine == true)
        {
            if (FinishLineBlock.GetComponent<Collider>().enabled == true)
                FinishLineBlock.GetComponent<Collider>().enabled = false;
        }
        else
        {
            if (FinishLineBlock.GetComponent<Collider>().enabled == false)
                FinishLineBlock.GetComponent<Collider>().enabled = true;

        }

        if (crossed && gameObject.tag == "Player")
        {
            if (trackPlayerPosition != null)
            {
                trackPlayerPosition.AddLap();
            }
            crossed = false;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == FinishLineEnter)
        {
            canCrossFinishLine = true;
            startCrossing = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == FinishLineExit)
        {
            canCrossFinishLine = false;
            if (startCrossing == true)
            {
                crossed = true;
                startCrossing = false;
            }

        }
    }

    private T[] FindAnyComponentInChildren<T>(GameObject Parrent) where T : Component
    {
        // Get all components of type T in this GameObject and its children
        T[] components = Parrent.GetComponentsInChildren<T>(true);


        return components;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);

    }
}
