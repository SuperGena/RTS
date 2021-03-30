using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary selectedDictionary;
    RaycastHit hit;

    LayerMask ground = 1 << 8;

    bool dragSelect;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    Vector2[] corners;
    Vector3[] verts;
    Vector3[] vecs;

    void Start()
    {
        selectedDictionary = GetComponent<SelectedDictionary>();
        dragSelect = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!GameManager._instance.freeToPress && GameManager._instance.isFolowPath)
            {
                ResetData();
            }
            if(GameManager._instance.freeToPress && !GameManager._instance.isBuildingMenu)
            if (dragSelect == false)
            {
                GetPressedObject();
            }
            else
            {
                GenerateMesh();
            }

            dragSelect = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (GameManager._instance.isFolowPath)
            {
                ResetData();
            }
        }
    }

    void GetPressedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(p1);

        if (Physics.Raycast(ray, out hit, 50000f, ~ground))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (hit.transform.gameObject.GetComponent<Player>())
                {
                    GameManager._instance.freeToPress = false;
                    GameManager._instance.isFolowPath = true;
                    GameManager._instance.ShowStandartUI();

                    selectedDictionary.AddObjectToDictionary(hit.transform.gameObject);
                }
            }
            else
            {
                selectedDictionary.DeselectAll();
                //GameManager._instance.DisablePlayerUIInfo();


                if (hit.transform.gameObject.GetComponent<Player>())
                {
                    GameManager._instance.freeToPress = false;

                    GameManager._instance.isFolowPath = true;

                    GameManager.playerReferance = hit.transform.gameObject;
                    GameManager._instance.ShowPlayerUIInfo();

                    selectedDictionary.AddObjectToDictionary(hit.transform.gameObject);
                }
                else
                {
                    GameManager._instance.ShowStandartUI();
                    selectedDictionary.DeselectAll();

                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // chill
            }
            else
            {
                GameManager._instance.freeToPress = true;
                GameManager._instance.isFolowPath = false;
                GameManager._instance.ShowStandartUI();

                selectedDictionary.DeselectAll();
            }
        }
    }

    void GenerateMesh()
    {
        verts = new Vector3[4];
        vecs = new Vector3[4];
        int i = 0;
        p2 = Input.mousePosition;
        corners = GetBoundingBox(p1, p2);

        foreach (Vector2 corner in corners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);

            if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
            {
                verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                vecs[i] = ray.origin - hit.point;
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
            }
            i++;
        }

        //generate the mesh
        selectionMesh = GenerateSelectionMesh(verts, vecs);

        selectionBox = gameObject.AddComponent<MeshCollider>();
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.enabled = true;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            selectedDictionary.DeselectAll();
        }

        GameManager._instance.freeToPress = true;
        GameManager._instance.isFolowPath = true;

        Destroy(selectionBox, 0.02f);
    }

    private void ResetData()
    {
        GameManager._instance.freeToPress = true;
        GameManager._instance.isFolowPath = false;

        GameManager._instance.ShowStandartUI();
        selectedDictionary.DeselectAll();
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    Vector2[] GetBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; 

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            selectedDictionary.AddObjectToDictionary(other.gameObject);
        }
    }
}
