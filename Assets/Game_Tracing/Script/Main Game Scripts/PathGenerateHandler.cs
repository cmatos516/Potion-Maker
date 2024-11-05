using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PathGenerateHandler : MonoBehaviour
{
    public static PathGenerateHandler Instance;
    public List<GameObject> myListOfPaths = new List<GameObject>();
    public GameObject LinePathPrefab;
    public Transform Spawnpoint;
    public int theCurrentNumber;

    public bool hasBeenInstantiated;

    private void Awake()
    {
        if (Instance == null)
        {
        Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(PathGenerateHandler))]
    public class LineEditor : Editor
    {
        string thisField;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PathGenerateHandler thisItem = (PathGenerateHandler)target;

            if (GUILayout.Button("Create Line"))
            {
                thisItem.GenerateLine();
            }
            GUILayout.Space(15);

            if (GUILayout.Button("Remove All Path"))
            {
                for (int i = 0; i < thisItem.myListOfPaths.Count; i++) // Changed from Counts to Count
                {
                    DestroyImmediate(thisItem.myListOfPaths[i]);
                }
                thisItem.myListOfPaths.Clear();
            }

            GUILayout.Space(15);
            thisField = GUILayout.TextField(thisField);
            if (GUILayout.Button("Remove Specific Path"))
            {
                thisItem.RemovePath(int.Parse(thisField) - 1);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    #endif
    #endregion

    public void GenerateLine()
    {
        GameObject thisGO = Instantiate(LinePathPrefab, Spawnpoint);
        theCurrentNumber = myListOfPaths.Count;
        myListOfPaths.Add(thisGO);
        theCurrentNumber += 1;
        thisGO.GetComponent<PathDrawer>().MyCurrentNumber = theCurrentNumber;
    }

    public void RemovePath(int itemNum)
    {
        DestroyImmediate(myListOfPaths[itemNum]);
        myListOfPaths.RemoveAt(itemNum);
        theCurrentNumber = myListOfPaths.Count;
        for (int i = 1; i <= myListOfPaths.Count; i++)
        {
            myListOfPaths[i - 1].GetComponent<PathDrawer>().MyCurrentNumber = i;
        }
    }
}