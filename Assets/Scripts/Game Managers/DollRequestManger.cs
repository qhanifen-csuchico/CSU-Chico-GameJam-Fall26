using UnityEngine;
using Unity.UI;
using UnityEditor.PackageManager.Requests;
using System;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;

public class DollRequestManger : MonoBehaviour
{
    public static DollRequestManger Instance;
    public TMP_Text[] textFields;
    public Color passColor, failColor;

    [Serializable]
    public class DollRequestList
    {
        public DollRequest[] dollRequests;
    }

    [Serializable]
    public class DollRequest
    {
        public string message;
        public bool exclusion;
        [SerializeField]
        public DollPart.DollPartDescriptor request;
    }

    public ScriptableObject dollRequestsPrefabs;
    public List<DollRequestList> dollRequests;

    public DollRequestList currRequestList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dollRequests = dollRequestsPrefabs.GetType().GetField("requestLists").GetValue(dollRequestsPrefabs) as List<DollRequestList>;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateNewRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNewRequest()
    {
        int randomIndex = (int)UnityEngine.Random.Range(0, dollRequests.Count);
        currRequestList = dollRequests[randomIndex];        
        for(int i=0; i < currRequestList.dollRequests.Length; i++)
        {            
            textFields[i].text = currRequestList.dollRequests[i].message;
            textFields[i].color = Color.white;
        }
    }

    public bool CompareDollToRequest(Doll doll)
    {
        DollPart.DollPartDescriptor dollDesc = doll.GetDescriptor();
        bool passing = true;
        Debug.Log($"Doll Requests: {currRequestList.dollRequests.Length}");
        for (int i = 0; i < currRequestList.dollRequests.Length; i++)
        {
            bool pass = true;
            DollRequest req = currRequestList.dollRequests[i];
            // If a the request matches a bitmask type, check if it was supposed to be an exclusion or not
            if ((dollDesc & req.request) == req.request)
            {
                pass = !req.exclusion;
            }
            else 
            {
                pass = req.exclusion;
            }
            if (!pass)
            {
                Debug.Log($"Failed at check {req.message}");
            }
            else
            {
                Debug.Log($"passed at check {req.message}, requirement was {(req.exclusion ? "exclusion" : "required")}");
            }

            textFields[i].color = pass ? passColor : failColor;
            passing = pass;
            
        }

        if (!passing)
        {
            
        }
        return passing;
    }
}
