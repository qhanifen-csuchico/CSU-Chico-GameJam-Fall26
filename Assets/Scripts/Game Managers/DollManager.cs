using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

public class DollManager : MonoBehaviour
{
    private bool lockState = false;

    public ScriptableObject dollPrefabs;
    private GameObject dollBasePrefab;
    private List<GameObject> headParts;
    private List<GameObject> armParts;
    private List<GameObject> legParts;
    

    public Transform dollStagingPosition;
    public Transform dollSpawnPosition;
    public Transform dollEndPosition;
    public Transform dollDumpPosition;
        
    public Doll currDoll;
    public Doll prevDoll;
    public float moveTime = 2.0f;

    public int dollHeadIndex = 0;
    public int dollArmIndex = 0;
    public int dollLegIndex = 0;

    
    void Awake()
    {
        dollBasePrefab = dollPrefabs.GetType().GetField("basePrefab").GetValue(dollPrefabs) as GameObject;
        headParts = dollPrefabs.GetType().GetField("headPrefabs").GetValue(dollPrefabs) as List<GameObject>;
        armParts = dollPrefabs.GetType().GetField("armPrefabs").GetValue(dollPrefabs) as List<GameObject>;
        legParts = dollPrefabs.GetType().GetField("legPrefabs").GetValue(dollPrefabs) as List<GameObject>;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(NewDoll());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator NewDoll()
    {
        if(currDoll)
        {
            prevDoll = currDoll;
        }
        GameObject newDoll = Instantiate(dollBasePrefab, dollSpawnPosition.position, Quaternion.identity, null);
        currDoll = newDoll.GetComponent<Doll>();
        lockState = true;
        yield return StartCoroutine(MoveDollToPosition(currDoll, dollStagingPosition, moveTime));
        lockState = false;
    }

    IEnumerator MoveDollToPosition(Doll doll, Transform pos, float time)
    {
        Vector3 startPos = doll.transform.position;
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            doll.transform.position = Vector3.Lerp(startPos, pos.position, elapsedTime/time);
            yield return null;
        }
    }

    void InstantiateParts(GameObject part)
    {
        if (part.GetComponent<DollPart>() == null)
        {
            Debug.LogError("The provided part does not have a DollPart component.");
            return;
        }
        DollPart dollPart = part.GetComponent<DollPart>();
        if(dollPart.partType == DollPart.DollPartType.Head)
        {
            GameObject head = Instantiate(part, currDoll.transform.position, Quaternion.identity, null);
            currDoll.AttachPart(head.GetComponent<DollPart>());

        }
        else
        {
            GameObject lPart = Instantiate(part, currDoll.transform.position, Quaternion.identity, null);
            GameObject rPart = Instantiate(part, currDoll.transform.position, Quaternion.identity, null);

            DollPart lPartDP = lPart.GetComponent<DollPart>();
            DollPart rPartDP = rPart.GetComponent<DollPart>();

            lPartDP.isLeft = true;
            rPartDP.isLeft = false;

            currDoll.AttachPart(lPartDP);
            currDoll.AttachPart(rPartDP);
        }
    }

    public void NextHead()
    {
        if (!lockState)
        {
            dollHeadIndex++;
            if (dollHeadIndex >= headParts.Count)
            {
                dollHeadIndex = 0;
            }
        }
    }

    public void PrevHead()
    {
        if (!lockState)
        {
            dollHeadIndex--;
            if (dollHeadIndex < 0)
            {
                dollHeadIndex = headParts.Count - 1;
            }
        }
    }

    public void SpawnHead()
    {
        if(!lockState)
        {
            currDoll.DetachPart(DollPart.DollPartType.Head);
            InstantiateParts(headParts[dollHeadIndex]);
        }
    }

    public void NextArm()
    {
        if (!lockState)
        {
            dollArmIndex++;
            if (dollArmIndex >= armParts.Count)
            {
                dollArmIndex = 0;
            }
        }
    }

    public void PrevArm()
    {
        if (!lockState)
        {
            dollArmIndex--;
            if (dollArmIndex < 0)
            {
                dollArmIndex = armParts.Count - 1;
            }
        }
    }

    public void SpawnArms()
    {
        if (!lockState)
        {
            currDoll.DetachPart(DollPart.DollPartType.Arm);
            InstantiateParts(armParts[dollArmIndex]);
        }
    }

    public void NextLeg()
    {
        if (!lockState)
        {
            dollLegIndex++;
            if (dollLegIndex >= legParts.Count)
            {
                dollLegIndex = 0;
            }
        }
    }

    public void PrevLeg()
    {
        if (!lockState)
        {
            dollLegIndex--;
            if (dollLegIndex < 0)
            {
                dollLegIndex = legParts.Count - 1;
            }
        }
    }

    public void SpawnLegs()
    {
        if (!lockState)
        {
            currDoll.DetachPart(DollPart.DollPartType.Leg);
            InstantiateParts(legParts[dollLegIndex]);
        }
    }

    public void SubmitDoll()
    {
        if (DollRequestManger.Instance.CompareDollToRequest(currDoll))
        {
            StartCoroutine(MoveDollToPosition(currDoll, dollEndPosition, 2.0f));
        }
        else
        {
            StartCoroutine(MoveDollToPosition(currDoll, dollDumpPosition, 2.0f));
        }
        
    }
}
