using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ListExtension;
public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;
    public event EventHandler OnSceneLoaded;

    
    public Vector3 sceneHierarchyPosition;
    public Vector3 sceneHierarchyRotation;

    public SceneData PrevSceneData
    {
        get { return prevSceneData; }
        set {  prevSceneData = value; }
    }
    private SceneData prevSceneData;
    public SceneData CurrentSceneData
    {
        get { return currentSceneData; }
        set { currentSceneData = value; }
    }
    private SceneData currentSceneData;
    public SceneData NextSceneData
    {
        get { return nextSceneData; }
        set { nextSceneData = value; }
    }
    private SceneData nextSceneData;
    
    private int currentSceneIndex;
    public SceneData[] sceneDataArray;

    private string environmentRule;
    private List<SceneData> loadedSceneDatas = new List<SceneData>();

    private SceneRoomTags roomTags => new SceneRoomTags();
    private void Awake ( )
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad( instance );
    }
    private void Start ( )
    {
        InitializeSceneData();
    }

    private void CheckForEnabledScenesRooms ( )
    {
        foreach (SceneData sceneData in loadedSceneDatas)
        {
            if(sceneData.sceneEnabled)
            {
                sceneData.sceneRoom = sceneData.scene.GetRootGameObjects().Where(s => s.GetComponent<DungeonManager>()).FirstOrDefault();
            }
        }
    }
    private void InitializeSceneData ( )
    {
        currentSceneData = sceneDataArray.FirstOrDefault(s => s.sceneTag == "StartRoom");

        if (currentSceneData != null)
        {
            currentSceneData.sceneEnabled = true;
            currentSceneData.InitializeSceneRoom();
            loadedSceneDatas.Add(currentSceneData);

            nextSceneData = GetNewSceneData();
        }
    }
    public void LoadSceneAsync ( ) //LOAD SCENE FUNCTION
    {
        if (String.IsNullOrEmpty(nextSceneData.sceneName))
        {
            Debug.LogError("Next scene is empty.");
            nextSceneData = GetNewSceneData();
        }

        StartCoroutine(LoadSceneAdditiveAsync(nextSceneData));
    }
    private IEnumerator LoadSceneAdditiveAsync ( SceneData sceneData ) //LOAD SCENE LOGIC
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(( ) => asyncLoadScene.isDone);

        Scene loadedScene = SceneManager.GetSceneByName(sceneData.sceneName);

        if (loadedScene.IsValid())
        {
            print("loadedScene");
            sceneData.InitializeSceneRoom();
            UpdateSceneDataReferences(sceneData);
            loadedSceneDatas.Add(sceneData);

            OnSceneLoaded?.Invoke(this, EventArgs.Empty);

            currentSceneIndex++;

            ReInitializeSceneDataValues();
        }
    }
    private void UpdateSceneDataReferences (SceneData sceneData ) // Update SceneData references

    {
        prevSceneData = currentSceneData;
        currentSceneData = sceneData;
        nextSceneData = GetNewSceneData();
    }
    private void ReInitializeSceneDataValues ( )
    {
        if(prevSceneData != null)
        {
            prevSceneData.InitializeSceneRoom();
        }
        if(currentSceneData != null)
        {
            currentSceneData.InitializeSceneRoom();
        }
        if(nextSceneData != null)
        {
            nextSceneData.InitializeSceneRoom();
        }
    }
    public SceneData GetSceneDataByName(string sceneName ) //GET SCENEDATA BY NAME
    {
        SceneData _currentSceneData = currentSceneData;
        if (!sceneName.IsNullOrWhitespace())
        {
            return sceneDataArray.Where(sD => sD.sceneName == sceneName).FirstOrDefault();
        }
        return _currentSceneData;
    }
    public SceneData GetSceneDataByIndex(int index ) //GET SCENE DATA BY INDEX
    {
        return sceneDataArray[index];
    }
    private SceneData GetNewSceneData ( ) //GET A SCENE DATA AVAILABLE TO SPAWN WITH FILTERING
    {
        //string _environmentRule = TryGetSceneRules();
        float randNum = UnityEngine.Random.Range(0, 100);
        //Filtro las escenas aptas para la colocación
        List<SceneData> initialFilter = sceneDataArray.Where(sD => randNum > sD.spawnRate && 
                                        currentSceneIndex > sD.spawnIndex && sD.sceneTag != roomTags.startRoom_Tag).ToList();

        // Agregamos los que tengan diferente sceneTag que currentSceneData o el anterior y que no sea el tag "startRoom"
        List<SceneData> validSceneDatas = initialFilter
            .Where(sD => sD.sceneTag != roomTags.startRoom_Tag && sD.sceneTag != currentSceneData.sceneTag).ToList();

        
        if (validSceneDatas.Count > 0)
        {
            Debug.Log("eaea");
            //Coloco una al azar
            int randomIndex = UnityEngine.Random.Range(0, validSceneDatas.Count);
            SceneData selectedScene = validSceneDatas[randomIndex];

            return selectedScene;
        }
        Debug.Log("eaea3");
        List<SceneData> validSceneDatasDefault = sceneDataArray.Where(sD => sD.sceneTag != roomTags.startRoom_Tag && currentSceneIndex + 3 >= sD.spawnIndex ).ToList();
        Debug.Log(validSceneDatasDefault.Count);

        return validSceneDatasDefault[UnityEngine.Random.Range(0, validSceneDatasDefault.Count)];
    }
    private string TryGetSceneRules ( SceneData sceneData ) //SET RULES FOR SCENES
    {
        if(currentSceneData.IsUnityNull() || nextSceneData.IsUnityNull())
        {
            Debug.Log("isUnityNull");
            environmentRule = "normal";
            return "normal";
        }
        string _environmentRule = sceneData.environmentRule;

        switch (_environmentRule)
        {
            case "normal":
                environmentRule = _environmentRule;
                return _environmentRule;
            case "frost":
                environmentRule = _environmentRule;
                return _environmentRule;
            case "lava":
                environmentRule = _environmentRule;
                return _environmentRule;
            default:
                return "normal";
        }
    }
    public void CalculateRoomTransform ( DungeonExits entrance, GameObject sceneRoom)
    {
        if (currentSceneData.exit == null || entrance == null)
        {
            return;
        }
        Transform exitParent = currentSceneData.exit.transform.parent;
        Transform entranceParent = entrance.transform.parent;
        if (exitParent == null || entranceParent == null)
        {
            Debug.LogError("Exit or Entrance does not have a parent object.");
            return;
        }
        Vector3 exitOrientation = exitParent.forward * 7.5f; // O el vector que apunta hacia la salida
        Vector3 entranceOrientation = entranceParent.forward * 7.5f; // O el vector que apunta hacia la entrada
                                                                     // Calcula la dirección deseada para la entrada en relación a la salida.
                                                                     // Esto es inverso porque queremos que la entrada de la nueva habitación mire hacia la salida.
        Vector3 desiredForward = -exitOrientation;

        // Calcula la rotación necesaria desde la orientación actual de la entrada hacia la dirección deseada.

        Quaternion currentRotation = Quaternion.LookRotation(entranceOrientation);
        Quaternion desiredRotation = Quaternion.LookRotation(desiredForward);
        Quaternion rotationDifference = desiredRotation * Quaternion.Inverse(currentRotation);

        // Aplica la rotación calculada al objeto sceneRoom en el espacio global.
        sceneRoom.transform.rotation = rotationDifference;

        // Ajusta la posición teniendo en cuenta el nuevo punto de orientación
        // Suponiendo que quieres mover sceneRoom para que su entrada quede justo en la posición de la salida
        // Este paso puede necesitar ajustes según la ubicación exacta de los puntos de entrada/salida y su relación espacial
        Vector3 entrancePosition = entrance.transform.position;
        Vector3 exitPosition = currentSceneData.exit.transform.position;
        Vector3 positionOffset = exitPosition - entrancePosition;
        sceneRoom.transform.position += positionOffset;
    }
    public int GetCurrentSceneIndex ( )
    {
        return currentSceneIndex;
    }
}
[System.Serializable]
public class SceneData
{
    public string sceneName;
    public string sceneTag;
    public string environmentRule;
    public bool sceneEnabled;
    public GameObject sceneRoom;
    public DungeonManager sceneRoomManager;
    public DungeonExits exit { get { return sceneRoomManager?.currentExit; } private set { exit = value; } }
    public DungeonExits entrance { get { return sceneRoomManager?.currentEntrance; } private set { exit = value; } }

    public float spawnRate;
    public int spawnIndex;

    public Scene scene => SceneManager.GetSceneByName(sceneName);

    public void InitializeSceneRoom ( )
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        sceneRoom = rootObjects.FirstOrDefault(obj => obj.GetComponent<DungeonManager>() != null);
        if (sceneRoom != null)
        {
            sceneEnabled = true;
            sceneRoomManager = sceneRoom.GetComponent<DungeonManager>();
        }
    }
    public void SetNewExit(DungeonExits newExit )
    {
        exit = newExit;
    }
    public void SetNewEntrance(DungeonExits newEntrance)
    {
        entrance = newEntrance;
    }
}
public class SceneRoomTags
{
    public string startRoom_Tag = "StartRoom";
    public string safeZone_Tag = "SafeZone";
    public string challenge_Tag = "Challenges";
    public string challengeBoss_Tag = "Challenge/Boss";
    public string treasures_Tag = "Treasures";
    public string puzzles_Tag = "Puzzles";
    public string exploration_Tag = "Exploration";
    public string boss_Tag = "Boss";

    //public string tag;
    //public override bool Equals ( object obj )
    //{
    //    return base.Equals(obj);
    //}
    //public override int GetHashCode ( )
    //{
    //    return tag.GetHashCode();
    //}
}