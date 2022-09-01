using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
public class LineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer lineRenderer;
    public ARPlacementInteractable placementInteractable;
    public TextMeshPro mText;
    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);
    }
    void DrawLine(ARObjectPlacementEventArgs args)
    {
        //increase point count
        lineRenderer.positionCount++;
        //add point locn in point renderer
        lineRenderer.SetPosition(index:lineRenderer.positionCount-1,args.placementObject.transform.position);
        if(lineRenderer.positionCount>1){
            Vector3 pointA= lineRenderer.GetPosition(index:lineRenderer.positionCount-1);
            Vector3 pointB= lineRenderer.GetPosition(index:lineRenderer.positionCount-2);
            float dist = Vector3.Distance(pointA,pointB);
            TextMeshPro distText = Instantiate(mText);
            distText.text=""+dist;

            Vector3 dirn= (pointB-pointA);
            Vector3 normal = args.placementObject.transform.up;

            Vector3 upd= Vector3.Cross(lhs: dirn, rhs: normal).normalized;
            Quaternion rotation= Quaternion.LookRotation(forward:-normal,upwards:upd);
            distText.transform.rotation= rotation;
            distText.transform.position= (pointA+dirn*0.5f) + upd*0.05f;
        }
    }
}
