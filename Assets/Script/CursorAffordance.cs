using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {


    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);


    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () {

        cameraRayCaster = GetComponent<CameraRaycaster>();
        cameraRayCaster.layerChange += OnLayerChanged;
	}
	
	// Update is called once per frame
	void OnLayerChanged () {
        
        switch (cameraRayCaster.currentLayerHit)
        {
            case (Layer.Walkable):
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case (Layer.Enemy):
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                break;
            default:
                Debug.LogError("no cursor to show");
                break;


        }

        
    }
}
