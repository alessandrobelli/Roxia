using System;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializedField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;


    RaycastHit playerHit;
    Ray ray;
    GameObject player;
    Material previousFrontObject;

    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer); // declare new delegate type
    public event OnLayerChange layerChange; // instantiate observers

    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        //layerChangeObserver += setOpacityForObjectInTheMiddle;



    }

    void Update()
    {


        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                if (layerHit != layer)
                {
                    layerHit = layer;
                    layerChange(layerHit);

                }
                 
                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;


        if (currentLayerHit != Layer.RaycastEndStop) // layer did not change and is is not equal to RayCastEndStop
        {
            layerHit = Layer.RaycastEndStop;
        }

    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
      
        RaycastHit hit; // used as an out parameter
      
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }

    private void LateUpdate()
    {
        setOpacityForObjectInTheMiddle();
    }

    void setOpacityForObjectInTheMiddle()
    {
        Vector3 viewPos = viewCamera.WorldToViewportPoint(player.transform.position);

        ray = viewCamera.ViewportPointToRay(viewPos);

       
        if (Physics.Raycast(ray, out playerHit, distanceToBackground))
        {
                if (!playerHit.transform.name.Contains("Enemy") && !playerHit.transform.name.Contains("Player"))
            {


                if (previousFrontObject)
                {
                    StandardShaderUtils.ChangeRenderMode(previousFrontObject, StandardShaderUtils.BlendMode.Opaque);
                }

                StandardShaderUtils.ChangeRenderMode(playerHit.transform.GetComponent<MeshRenderer>().material, StandardShaderUtils.BlendMode.Transparent);

                previousFrontObject = playerHit.transform.GetComponent<MeshRenderer>().material;

            }
            else
            {
                if (previousFrontObject)
                {
                    StandardShaderUtils.ChangeRenderMode(previousFrontObject, StandardShaderUtils.BlendMode.Opaque);
                }

            }
        }
    }
}

internal class SerializedFieldAttribute : Attribute
{
}