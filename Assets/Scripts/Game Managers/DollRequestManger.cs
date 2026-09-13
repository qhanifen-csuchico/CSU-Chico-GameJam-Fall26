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
    public TMP_Text textField;

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

    void GenerateNewRequest()
    {
        int randomIndex = (int)UnityEngine.Random.Range(0, dollRequests.Count - 1);
        currRequestList = dollRequests[randomIndex];
    }

    public bool CompareDollToRequest(Doll doll)
    {
        DollPart.DollPartDescriptor dollDesc = doll.GetDescriptor();
        bool passing = true;
        int index = 0;
        while (passing && index < currRequestList.dollRequests.Length - 1)
        {
            foreach (DollRequest req in currRequestList.dollRequests)
            {
                if ((dollDesc & req.request) == req.request)
                {
                    passing = !req.exclusion;
                    break;
                }
                index++;
            }
        }

        if (!passing)
        {
            Debug.Log("Failed at {index}");
        }
        return passing;
    }
}
