// REUSED COMPONENT

/*
 * CloudCrafter.cs - made by Jason Illick in a game progamming class from last semester.
 * 
 * This script instantiates and moves Cloud objects.
 * 
 * This script is attached to the cameras in the level scenes (although it doesn't control the
 * cameras in any way, we just needed somewhere to put it).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    // \/\/ ADDED \/\/
    [SerializeField]
    private float _cloudPosZ = 50;
    // /\/\ ADDED /\/\

    private GameObject[] cloudInstances;

    void Awake()
    {
        // Make an array large enough to hold all the Cloud instances
        cloudInstances = new GameObject[numClouds];

        // Find the CloudAnchor parent GameObject
        GameObject anchor = GameObject.Find("CloudAnchor");

        // Iterate through and make the clouds
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            // Make an instance of cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);

            // Position Cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            // \/\/ REMOVED \/\/
            /*
            // Scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            // Smaller clouds (with smaller scaleU) should be nearer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            // Smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;
            */
            // /\/\ REMOVED /\/\

            // \/\/ ADDED \/\/
            cPos.z = _cloudPosZ;
            // /\/\ ADDED /\/\

            // Apply these transforms to the cloud
            cloud.transform.position = cPos;
            // \/\/ REMOVED \/\/
            //cloud.transform.localScale = Vector3.one * scaleVal;
            // /\/\ REMOVED /\/\

            // Make cloud a child of the anchor
            cloud.transform.SetParent(anchor.transform);

            // Add the cloud to cloudInstances
            cloudInstances[i] = cloud;
        }
    }

    void Update()
    {
        // Iterate over each cloud that was created
        foreach (GameObject cloud in cloudInstances)
        {
            // Get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            // Move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            // If a cloud has moved too far to the left.
            if (cPos.x <= cloudPosMin.x)
            {
                // Move it to the far right
                cPos.x = cloudPosMax.x;
            }

            // Apply the new position to cloud
            cloud.transform.position = cPos;
        }
    }
}
