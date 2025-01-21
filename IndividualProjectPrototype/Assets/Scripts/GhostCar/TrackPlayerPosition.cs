using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
struct DataPoint
{
    public Vector3 position;
    public Quaternion frontLeftWheelRotation; 
    public Quaternion frontRightWheelRotation; 
    public Quaternion rearLeftWheelRotation; 
    public Quaternion rearRightWheelRotation; 

    public bool horn;
    public Quaternion rotation;
    public float speed;
    public float time;

}
public class TrackPlayerPosition : MonoBehaviour
{
    [SerializeField]FinishLineTriggrer[] finishLineTriggrer;
    [SerializeField] private CarController carController;
    private List<DataPoint> lapData = new List<DataPoint>();
    private List<DataPoint> ghostData = new List<DataPoint>();

    private Transform flMeshGhostInstance;
    private Transform frMeshGhostInstance;
    private Transform rlMeshGhostInstance;
    private Transform rrMeshGhostInstance;
    public CarEngineSoundHandler carEngineSound { get; private set; }
    public HornSoundHandler GhostHornSound { get; private set; }

    public float getTime;


    LapCompleted ding;
    public float bestLapTime { get; private set; } = 0f;

    private int currentDataIndex;
    public float currentTime { get; private set; }

    private GameObject player;
    private Rigidbody rb;

    [SerializeField] private GameObject ghostCarMesh;

    private GameObject ghostCarInstance;

    public uint lapCount { get; private set; } = 0;

    public bool gameover;

    private float topSpeed;
    private PauseGame pauseGame;
    
    private void Start()
    {

        pauseGame = GameObject.Find("PauseMenuManager").GetComponent<PauseGame>();
        
        ding = new LapCompleted();
        gameover = false;
        finishLineTriggrer = FindObjectsOfType<FinishLineTriggrer>();
        bestLapTime = 0f;
        ghostData = lapData;
        currentDataIndex = 0;
        currentTime = Time.time;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            rb = player.GetComponent<Rigidbody>();
        }
        if (ghostCarMesh == null)
        {
            ghostCarMesh = GameObject.FindWithTag("Ghost");
        }
        topSpeed = carController.acura.engineBlock.maxRPM;
        carEngineSound = new CarEngineSoundHandler(topSpeed);
        GhostHornSound = new HornSoundHandler();
        ghostCarMesh.SetActive(false);
    }
    void Update(){
        
        
    }
    private void StartGhostCar()
    {
        if (ghostCarInstance == null)
        {
            ghostCarInstance = Instantiate(ghostCarMesh);
            if (!ghostCarInstance.gameObject.activeSelf)
            {
                ghostCarInstance.gameObject.SetActive(true);
            }
            rrMeshGhostInstance = FindChildByName(ghostCarInstance.transform, "RearRightWheel");
            rlMeshGhostInstance = FindChildByName(ghostCarInstance.transform, "RearLeftWheel");
            flMeshGhostInstance = FindChildByName(ghostCarInstance.transform, "FrontLeftWheel");
            frMeshGhostInstance = FindChildByName(ghostCarInstance.transform, "FrontRightWheel");
        }
    }
    private void UpdateGhost(float currentTime)
    {
        if (ghostData.Count == 0 || currentDataIndex >= ghostData.Count - 1)
        {
            return;
        }
        else
        {
            DataPoint currentData = ghostData[currentDataIndex];
            DataPoint nextData = ghostData[currentDataIndex + 1];

            float interpolationFactor = (currentTime - currentData.time) / (nextData.time - currentData.time);
            if (ghostCarInstance != null)
            {
                ghostCarInstance.transform.position = Vector3.Lerp(currentData.position, nextData.position, interpolationFactor);
                ghostCarInstance.transform.rotation = Quaternion.Lerp(currentData.rotation, nextData.rotation, interpolationFactor);
                rrMeshGhostInstance.transform.rotation = Quaternion.Lerp(currentData.rearRightWheelRotation, nextData.rearRightWheelRotation, interpolationFactor);
                rlMeshGhostInstance.transform.rotation = Quaternion.Lerp(currentData.rearLeftWheelRotation, nextData.rearLeftWheelRotation, interpolationFactor);
                flMeshGhostInstance.transform.rotation = Quaternion.Lerp(currentData.frontLeftWheelRotation, nextData.frontLeftWheelRotation, interpolationFactor);
                frMeshGhostInstance.transform.rotation = Quaternion.Lerp(currentData.frontRightWheelRotation, nextData.frontRightWheelRotation, interpolationFactor);
                UpdateEngineAudio(currentData.speed);
                UpdateHornSound(currentData.horn);

            }

            if (currentTime >= nextData.time)
            {
                currentDataIndex++;
            }
        }

    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            rb = player.GetComponent<Rigidbody>();
        }
        if(carEngineSound == null){
            carEngineSound = new CarEngineSoundHandler(topSpeed);
        }
        if(GhostHornSound == null){
            GhostHornSound = new HornSoundHandler();
        }
        if (!gameover && !carController.pauseGame.paused)
        {
            Vector3 position = player.transform.position;
            Quaternion rotation = player.transform.rotation;
            Quaternion rearRight = carController.wheelRR().transform.rotation;
            Quaternion rearLeft = carController.wheelRL().transform.rotation;
            Quaternion frontLeft = carController.wheelFL().transform.rotation;
            Quaternion frontRight = carController.wheelFR().transform.rotation;
            bool hornIsPlaying = carController.hornIsPlaying;
            float speed = carController.acura.engineBlock.currentRPM;
            if (lapData.Count > 0)
            {
            }
            else
            {
                currentTime = 0;
            }


            DataPoint newData = new DataPoint
            {
                position = position,
                rotation = rotation,
                rearRightWheelRotation = rearRight,
                rearLeftWheelRotation = rearLeft,
                frontLeftWheelRotation = frontLeft,
                frontRightWheelRotation = frontRight,
                speed = speed,
                time = currentTime,
                horn = hornIsPlaying
            };

            lapData.Add(newData);

            UpdateGhost(currentTime);
            currentTime += Time.fixedDeltaTime;
        }
        else{
            carEngineSound.GetFmodEngineObject().PauseEventSound();
            GhostHornSound.GetFmodHornSound().PauseEventSound();
        }
    }
    void UpdateEngineAudio(float currentSpeed)
    {
        float vel = 0;
        if (carEngineSound != null)
        {
            if (carEngineSound.GetFmodEngineObject() != null)
            {
                if (!pauseGame.paused)
                {
                    if (!carEngineSound.GetFmodEngineObject().IsEventPlaying())
                    {
                        
                        carEngineSound.GetFmodEngineObject().StartEventSound();
                    }
                    carEngineSound.GetFmodEngineObject().ResumeEventSound();
                }

                else
                {
                    carEngineSound.GetFmodEngineObject().PauseEventSound();
                }
                carEngineSound.GetFmodEngineObject().setSoundPlayPosition(ghostCarInstance.transform.position); 

            }
            
            carEngineSound.CurrentSpeed = Mathf.SmoothDamp(carEngineSound.CurrentSpeed, currentSpeed,ref vel,0.2f);

            carEngineSound.CurrentSpeed = currentSpeed;
            carEngineSound.UpdateCarEngineSound();
            
        }
        else
        {
            carEngineSound = new CarEngineSoundHandler(topSpeed);
        }

    }
    void UpdateHornSound(bool horn){
        if (GhostHornSound != null){
            if(GhostHornSound.GetFmodHornSound() != null){
                if (!pauseGame.paused)
                {

                    if(!GhostHornSound.GetFmodHornSound().IsEventPlaying() && horn){

                        GhostHornSound.GetFmodHornSound().StartEventSound();
                    }
                    GhostHornSound.GetFmodHornSound().ResumeEventSound();


                }
                else{
                    GhostHornSound.GetFmodHornSound().PauseEventSound();
                }
                GhostHornSound.GetFmodHornSound().setSoundPlayPosition(ghostCarInstance.transform.position);

            }
        }
        else{
            GhostHornSound = new HornSoundHandler();
        }
    }
    
    private void ProcessLapData(List<DataPoint> lapData)
    {
        float lapEndTime = lapData[lapData.Count - 1].time;
        lapEndTime = Mathf.FloorToInt(lapEndTime);

        
        if ((bestLapTime < lapEndTime && bestLapTime == 0f) || bestLapTime >= lapEndTime)
        {
            ding.GetFmodLapCompletedSound().StartEventSound();
            lapCount++;
            if (lapCount == 1)
            {
                StartGhostCar();
            }
            bestLapTime = lapEndTime;

            currentDataIndex = 0;
            ghostData = new List<DataPoint>(lapData);

        }
        else
        {
            gameover = true;
            ghostData.Clear();

        }

    }
    
    public void AddLap()
    {
        if(ding == null)
        {
            ding = new LapCompleted();
        }
        
        ProcessLapData(lapData);

        lapData.Clear();
    }

    Transform FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            // Recursive call to search in deeper levels
            Transform foundChild = FindChildByName(child, childName);
            if (foundChild != null)
                return foundChild;
        }
        return null; // Child not found
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Lap completed, process the recorded data

        }
    }
    private void OnDestroy(){
        carEngineSound.GetFmodEngineObject().EndSoundInstance();
    }
}


