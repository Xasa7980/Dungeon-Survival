using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void InitializeSceneData ( )
    {
        currentSceneData = sceneDataArray.FirstOrDefault(s => s.sceneTag == "StartRoom");
        nextSceneData = GetNewSceneData();
    }
    public void LoadSceneAsync ( ) //LOAD SCENE FUNCTION
    {
        if (nextSceneData == null)
        {
            Debug.LogError("Next scene is empty.");
            nextSceneData = GetNewSceneData();
        }

        StartCoroutine(LoadSceneAdditiveAsync(nextSceneData));
        currentSceneIndex ++;
    }
    private IEnumerator LoadSceneAdditiveAsync ( SceneData sceneData ) //LOAD SCENE LOGIC
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(( ) => asyncLoadScene.isDone);

        Scene loadedScene = SceneManager.GetSceneByName(sceneData.sceneName);

        if (loadedScene.IsValid())
        {
            GameObject sceneRoomObject = loadedScene.GetRootGameObjects().FirstOrDefault(obj => obj.GetComponent<DungeonManager>() != null);
            sceneData.sceneRoom = sceneRoomObject;

            if (sceneRoomObject != null)
            {
                UpdateSceneData(sceneData,sceneRoomObject);

                currentSceneData.exit = SelectCurrentExit();
                nextSceneData.entrance = SelectNextEntrance();

                CalculateRoomTransform();

                OnSceneLoaded?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            Debug.LogError("No se encontró un GameObject con DungeonManager en la escena cargada.");
        }
    }

    public DungeonExits SelectCurrentExit ( ) // SELECT AN EXIT
    {
        if (currentSceneData == null)
        {
            Debug.Log("Not found any currentSceneData.exit");
            return null;
        }

        GameObject currentDungeonMainObject = currentSceneData.scene.GetRootGameObjects().Where(sD => sD.GetComponent<DungeonManager>()).First();

        currentSceneData.sceneRoomManager = currentDungeonMainObject.GetComponent<DungeonManager>();
        if (currentSceneData.sceneRoomManager.HasEntrance()) return null;

        DungeonExits exit = currentDungeonMainObject.GetComponent<DungeonManager>().SetExit();

        return exit;
    }
    public DungeonExits SelectNextEntrance ( ) //SELECT AN ENTRANCE
    {
        if (nextSceneData == null)
        {
            Debug.Log("Not found any nextSceneData.entrance");
            return null;
        }
        GameObject nextDungeonMainObject = nextSceneData.scene.GetRootGameObjects().Where(sD => sD.GetComponent<DungeonManager>().gameObject).First();

        nextSceneData.sceneRoomManager = nextDungeonMainObject.GetComponent<DungeonManager>();
        if (nextSceneData.sceneRoomManager.HasEntrance()) return null;

        DungeonExits entrance = nextDungeonMainObject.GetComponent<DungeonManager>().SetEntrance();

        return entrance;
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
        List<SceneData> initialFilter = sceneDataArray.Where(sD => randNum < sD.spawnRate && 
                                        currentSceneIndex > sD.spawnIndex && sD.sceneTag != roomTags.startRoom_Tag).ToList();
        foreach (SceneData sceneData in initialFilter)
        {
            Debug.Log(sceneData.sceneTag);
        }
        // Agregamos los que tengan diferente sceneTag que currentSceneData o el anterior y que no sea el tag "startRoom"
        List<SceneData> validSceneDatas = initialFilter
            .Where(sD => sD.sceneTag != currentSceneData.sceneTag/* && sD.sceneTag != prevSceneData.sceneTag*/).ToList(); 
        if (validSceneDatas.Count > 0)
        {
            Debug.Log(validSceneDatas.Count);

            //Coloco una al azar
            int randomIndex = UnityEngine.Random.Range(0, validSceneDatas.Count);
            SceneData selectedScene = validSceneDatas[randomIndex];

            return selectedScene;
        }
        SceneData selectedDefault = sceneDataArray.Where(sD => sD.sceneTag != roomTags.startRoom_Tag && sD.sceneTag != currentSceneData.sceneTag).FirstOrDefault();
        return selectedDefault;
    }
    private void UpdateSceneData ( SceneData sceneData, GameObject sceneRoomObject )
    {
        prevSceneData = currentSceneData;
        currentSceneData = sceneData;
        currentSceneData.sceneRoom = sceneRoomObject;
        nextSceneData = GetNewSceneData(); // Implement this method based on your logic
    }    
    private List<SceneData> loadedSceneDatas = new List<SceneData>();
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
    public void UpdatePrevSceneRoom( GameObject sceneRoomObject )//UPDATE THE GAMEOBJECT OF THE ROOM FROM A PREV SCENE DATA
    {
        if (prevSceneData != null)
        {
            prevSceneData.sceneRoom = sceneRoomObject;
        }
        else
        {
            Debug.LogError("currentSceneData es null. No se puede actualizar sceneRoom.");
        }
    }
    public void UpdateCurrentSceneRoom (GameObject sceneRoomObject ) //UPDATE THE GAMEOBJECT OF THE ROOM FROM A CURRENT SCENE DATA
    {
        if (currentSceneData != null)
        {
            currentSceneData.sceneRoom = sceneRoomObject;
        }
        else
        {
            Debug.LogError("currentSceneData es null. No se puede actualizar sceneRoom.");
        }
    }
    public void UpdateNextSceneRoom( GameObject sceneRoomObject )//UPDATE THE GAMEOBJECT OF THE ROOM FROM A NEXT SCENE DATA
    {
        if (nextSceneData != null)
        {
            nextSceneData.sceneRoom = sceneRoomObject;
        }
        else
        {
            Debug.LogError("currentSceneData es null. No se puede actualizar sceneRoom.");
        }
    }
    private void CalculateRoomTransform ( )
    {
        Debug.Log(currentSceneData.exit);
        Debug.Log(nextSceneData.entrance);

        if (currentSceneData.exit == null)
        {
            currentSceneData.exit = SelectCurrentExit();
        }
        if(nextSceneData.entrance == null)
        {
            currentSceneData.entrance = SelectNextEntrance();
        }
        Debug.Log(currentSceneData.exit);
        Debug.Log(nextSceneData.entrance);

        Vector3 exitOrientation = currentSceneData.exit.transform.forward; // O el vector que apunta hacia la salida
        Debug.DrawLine(currentSceneData.exit.transform.forward, currentSceneData.exit.transform.forward * 5, Color.red, 10);
        Vector3 entranceOrientation = nextSceneData.entrance.transform.forward; // O el vector que apunta hacia la entrada
        Debug.DrawLine(nextSceneData.exit.transform.forward, nextSceneData.exit.transform.forward * 5,Color.red,10);

        Vector3 crossProduct = Vector3.Cross(exitOrientation, entranceOrientation);
        float angle = Vector3.Angle(exitOrientation,entranceOrientation);
        if (crossProduct.y < 0) angle = -angle; //Determinamos la orientacion a la que rotamos

        nextSceneData.sceneRoom.transform.Rotate(Vector3.up, angle);
        
        Vector3 entranceOffset = nextSceneData.entrance.transform.localPosition;
        Quaternion rotation = Quaternion.Euler(0,angle,0);
        entranceOffset = rotation * entranceOffset;

        nextSceneData.sceneRoomManager.SetPosition(currentSceneData.exit.transform.position - entranceOffset);
        //nextSceneData.sceneRoom.transform.position = exit.transform.position - entranceOffset;
    }
    public SceneData GetPrevSceneData ( )
    {
        SceneData _prevSceneData;
        _prevSceneData = prevSceneData.sceneName != "" ? nextSceneData : null;
        return _prevSceneData;
    }
    public SceneData GetCurrentSceneData ( )
    {
        SceneData _currentSceneData;
        _currentSceneData = currentSceneData.sceneName != "" ? nextSceneData : null;

        return _currentSceneData;
    }
    public SceneData GetNextSceneData ( )
    {
        SceneData _nextSceneData;
        _nextSceneData = nextSceneData.sceneName != "" ? nextSceneData : null;

        return _nextSceneData;
    }
    public int GetCurrentSceneIndex ( )
    {
        return currentSceneIndex;
    }
    public void SetCurrentSceneIndex ( int i )
    {
        currentSceneIndex += i;
    }
}
[System.Serializable]
public class SceneData
{
    internal Scene scene => SceneManager.GetSceneByName(sceneName);
    internal GameObject sceneRoom;
    internal DungeonManager sceneRoomManager;
    internal DungeonExits exit;
    internal DungeonExits entrance;

    public string sceneName;
    public string sceneTag;

    public string environmentRule;

    public float spawnRate;
    public int spawnIndex;

    public SceneData ( GameObject _sceneRoom, string _environmentRule )
    {
        sceneRoom = _sceneRoom;
        environmentRule = _environmentRule;
    }
    public SceneData ( GameObject _sceneRoom )
    {
        sceneRoom = _sceneRoom;
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