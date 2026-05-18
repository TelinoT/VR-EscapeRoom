using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TuringRollSetup : Editor
{
    [MenuItem("Tools/Group Bomb into rolls")]
    public static void GroupRolls()
    {
        GameObject machineParent = Selection.activeGameObject;

        if (machineParent == null)
        {
            return;
        }

        Dictionary<string, List<GameObject>> groupedRolls = new Dictionary<string, List<GameObject>>();
        List<GameObject> childrenToProcess = new List<GameObject>();

        foreach (Transform child in machineParent.transform)
        {
            childrenToProcess.Add(child.gameObject);
        }

        foreach (GameObject obj in childrenToProcess)
        {
            string name = obj.name;
            int dotIndex = name.LastIndexOf('.');

            if (dotIndex == -1) continue;

            string suffix = name.Substring(dotIndex + 1);

            if (int.TryParse(suffix, out _))
            {
                if (!groupedRolls.ContainsKey(suffix))
                {
                    groupedRolls[suffix] = new List<GameObject>();
                }
                groupedRolls[suffix].Add(obj);
            }
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Group Rolls Around Mittelstange");
        int undoGroup = Undo.GetCurrentGroup();

        int groupsCreated = 0;

        foreach (KeyValuePair<string, List<GameObject>> pair in groupedRolls)
        {
            string suffix = pair.Key;
            List<GameObject> parts = pair.Value;

            GameObject pivotAnchor = null;
            foreach (GameObject part in parts)
            {
                if (part.name.ToLower().Contains("mittelstange"))
                {
                    pivotAnchor = part;
                    break;
                }
            }

            if (pivotAnchor == null)
            {
                foreach (GameObject part in parts)
                {
                    if (part.name.ToLower().Contains("torus") || part.name.ToLower().Contains("rolle"))
                    {
                        pivotAnchor = part;
                        break;
                    }
                }
            }
            if (pivotAnchor == null) pivotAnchor = parts[0];

            GameObject newParent = new GameObject("Roll_" + suffix);
            newParent.transform.position = pivotAnchor.transform.position;
            newParent.transform.rotation = pivotAnchor.transform.rotation;
            newParent.transform.SetParent(machineParent.transform, true);
            
            Undo.RegisterCreatedObjectUndo(newParent, "Create Roll Parent");

            foreach (GameObject part in parts)
            {
                Undo.SetTransformParent(part.transform, newParent.transform, "Move Part to Group");
            }

            groupsCreated++;
        }

        Undo.CollapseUndoOperations(undoGroup);
    }
}