using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int              numClouds = 40;
    public GameObject       cloudPrefab;
    public Vector3          cloudsPosMin = new Vector3(-50,-5,10);
    public Vector3          cloudsPosMax = new Vector3(150,100,10);
    public float            cloudScaleMin = 1;
    public float            cloudScaleMax = 3;
    public float            cloudSpeedMult = 0.5f;

    private GameObject[]    cloudInstances;


    void Awake() {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;

        for (int i = 0; i < numClouds; i++) {
            cloud  = Instantiate<GameObject>(cloudPrefab);

            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudsPosMin.x, cloudsPosMax.x);
            cPos.y = Random.Range(cloudsPosMin.y, cloudsPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            cPos.y = Mathf.Lerp(cloudsPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);
            
            cloudInstances[i] = cloud;            
        }

        
        
    }

    void Update() {
            foreach (GameObject cloud in cloudInstances) {
                float scaleVal = cloud.transform.localScale.x;
                Vector3 cPos = cloud.transform.position;
                cPos.x -= scaleVal * Time.deltaTime *  cloudSpeedMult;
                if (cPos.x <= cloudsPosMin.x) {
                    cPos.x = cloudsPosMax.x;
                }
                cloud.transform.position = cPos;
            }
        }
}
