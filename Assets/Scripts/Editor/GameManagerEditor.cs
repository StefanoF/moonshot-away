using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector(); 

        if (GUILayout.Button("Change Universe")) {
            GameManager.Instance.playerState.changeUniverse.Raise();
        }

        if (GUILayout.Button("Launch Rocket")) {
            GameManager.Instance.playerState.launchRocket.Raise();
        }
    }
}