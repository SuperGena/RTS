using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{

    public Dictionary<int, GameObject> selectedObjects = new Dictionary<int, GameObject>();

    public void AddObjectToDictionary(GameObject Object)
    {
        int id = Object.GetInstanceID();

        if (!(selectedObjects.ContainsKey(id)))
        {
            selectedObjects.Add(id, Object);
            Object.AddComponent<SelectedObject>();
        }
    }

    public void Deselect(int id)
    {
        Destroy(selectedObjects[id].GetComponent<SelectedObject>());
        selectedObjects.Remove(id);
    }

    public void DeselectAll()
    {
        foreach (KeyValuePair<int,GameObject> pair in selectedObjects)
        {
            if (pair.Value != null)
            {
                Destroy(selectedObjects[pair.Key].GetComponent<SelectedObject>());
            }
        }
        selectedObjects.Clear();
    }
}
