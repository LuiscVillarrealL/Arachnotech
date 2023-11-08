using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(DetectScript))]
public class DetectEditorScript : Editor
{
    void OnSceneGUI()
    {
        DetectScript detectScript = (DetectScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(detectScript.transform.position, Vector3.up, Vector3.forward, 360, detectScript.detectRadius);
        Vector3 viewAngleA = detectScript.DirFromAngle(-detectScript.viewAngle / 2, false);
        Vector3 viewAngleB = detectScript.DirFromAngle(detectScript.viewAngle / 2, false);

        Handles.DrawLine(detectScript.transform.position, detectScript.transform.position + viewAngleA * detectScript.viewAngle);
        Handles.DrawLine(detectScript.transform.position, detectScript.transform.position + viewAngleB * detectScript.viewAngle);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in detectScript.visibleTargets)
        {
            Handles.DrawLine(detectScript.transform.position, visibleTarget.position);
        }
    }
}
