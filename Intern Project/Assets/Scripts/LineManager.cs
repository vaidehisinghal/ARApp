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
    private int pointCnt=0;
    LineRenderer line;
    public bool continuous;
    public TextMeshProUGUI buttonText;
    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);
    }

    public void ToggleBetween()
    {
        continuous=!continuous;
        if(!continuous){
            buttonText.text="Continuous";
        }
        else{
            buttonText.text="Discrete";
        }
    }

    void DrawLine(ARObjectPlacementEventArgs args)
    {
        pointCnt++;
        if(pointCnt<2){
            line= Instantiate(lineRenderer);
            line.positionCount=1;
        }
        else{
            line.positionCount=pointCnt;
            if(!continuous){
                pointCnt=0;
            }
        }
        //add point locn in point renderer
        line.SetPosition(index:line.positionCount-1,args.placementObject.transform.position);
        if(line.positionCount>1){
            Vector3 pointA= line.GetPosition(index:line.positionCount-1);
            Vector3 pointB= line.GetPosition(index:line.positionCount-2);
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
