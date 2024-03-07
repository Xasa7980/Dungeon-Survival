using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ListExtension
{
    public static class ListExtensions
    {
        public static void AddToList ( this List<SceneData> listOfDatas, SceneData sceneData, GameObject gameObject )
        {
            listOfDatas.Add(sceneData);
            sceneData.sceneRoom = gameObject;
        }
    }
}
