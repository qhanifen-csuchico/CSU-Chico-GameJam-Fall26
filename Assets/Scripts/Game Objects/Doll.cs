using System;
using UnityEngine;
using static DollPart;

public class Doll : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public float socketRadius = 0.1f;

    public Transform headSocket;
    public Transform lArmSocket;
    public Transform rArmSocket;
    public Transform lLegSocket;
    public Transform rLegSocket;

    private int[] dollDescriptor = new int[10];

    private DollPart headPart;
    private DollPart l_ArmPart;
    private DollPart r_ArmPart;
    private DollPart l_LegPart;
    private DollPart r_LegPart;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(headSocket.position, socketRadius);
        Gizmos.DrawWireSphere(lArmSocket.position, socketRadius);
        Gizmos.DrawWireSphere(rArmSocket.position, socketRadius);
        Gizmos.DrawWireSphere(lLegSocket.position, socketRadius);
        Gizmos.DrawWireSphere(rLegSocket.position, socketRadius);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachPart(DollPart part)
    {
        if (part.partType == DollPartType.Head)
        {            
            headPart = part;
            part.transform.parent = headSocket;
            part.transform.localPosition = Vector3.zero;            
        }
        else if (part.partType == DollPartType.Arm)
        {
            if (part.isLeft)
            {                
                l_ArmPart = part;
                part.transform.parent = lArmSocket;
                part.transform.localPosition = Vector3.zero;
            }
            else
            {                
                r_ArmPart = part;
                part.transform.parent = rArmSocket;
                part.transform.localPosition = Vector3.zero;
            }
        }
        else if (part.partType == DollPartType.Leg)
        {
            if (part.isLeft)
            {                
                l_LegPart = part;
                part.transform.parent = lLegSocket;
                part.transform.localPosition = Vector3.zero;
            }
            else
            {                
                r_LegPart = part;
                part.transform.parent = rLegSocket;
                part.transform.localPosition = Vector3.zero;
            }
        }

        AddDollDescriptor(part.descriptor);
    }

    public void DetachPart(DollPart.DollPartType partType)
    {
        switch (partType)
        {
            case DollPartType.Head:
                if (headPart)
                {
                    SubtractDollDescriptor(headPart.descriptor);
                    Destroy(headPart.gameObject);
                }
                break;
            case DollPartType.Arm:
                if (l_ArmPart)
                {
                    SubtractDollDescriptor(l_ArmPart.descriptor);
                    Destroy(l_ArmPart.gameObject);
                    Destroy(r_ArmPart.gameObject);
                }
                break;
            case DollPartType.Leg:
                if (l_LegPart)
                {
                    SubtractDollDescriptor(l_LegPart.descriptor);
                    Destroy(l_LegPart.gameObject);
                    Destroy(r_LegPart.gameObject);
                }
                break;
            default:
                break;
        }
    }

    public Transform GetSpawnTransform(DollPart part)
    {
        switch (part.partType)
        {
            case DollPartType.Head:
                return headSocket;
            case DollPartType.Arm:
                return part.isLeft ? lArmSocket : rArmSocket;
            case DollPartType.Leg:
                return part.isLeft ? lLegSocket : rLegSocket;
            default:
                return null;
        }
    }

    public void AddDollDescriptor(DollPartDescriptor partDescriptor)
    {
        int index = 0;
        foreach(DollPartDescriptor flag in Enum.GetValues(typeof(DollPartDescriptor)))
        {
            if (flag == DollPartDescriptor.Normal) continue; // Skip Normal descriptor
            if (partDescriptor.HasFlag(flag))
            {
                dollDescriptor[index]++;
            }
            index++;
        }
    }
    public void SubtractDollDescriptor(DollPartDescriptor partDescriptor)
    {
        int index = 0;
        foreach (DollPartDescriptor flag in Enum.GetValues(typeof(DollPartDescriptor)))
        {
            if (flag == DollPartDescriptor.Normal) continue; // Skip Normal descriptor
            if (partDescriptor.HasFlag(flag))
            {
                dollDescriptor[index]--;
            }
            index++;
        }
    }

    // Create a bitmask by checking if any of the stored counts are greater than 1
    public DollPartDescriptor GetDescriptor()
    {
        DollPartDescriptor descriptor = 0;
        int index = 0;
        foreach(int val in dollDescriptor)
        {
            if(val > 0)
            {
                descriptor |=  (DollPartDescriptor)(1 << index);
            }
            index++;
        }
        return descriptor;
    }
}
