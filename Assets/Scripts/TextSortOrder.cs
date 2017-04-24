using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSortOrder : MonoBehaviour {

    // Use this for initialization
    public int sortOrder = 10;
    void Start() {
        GetComponent<MeshRenderer>().sortingLayerName = "Debug2";
        GetComponent<MeshRenderer>().sortingOrder = sortOrder;
    }

    // Update is called once per frame
    void Update() {

    }
}
