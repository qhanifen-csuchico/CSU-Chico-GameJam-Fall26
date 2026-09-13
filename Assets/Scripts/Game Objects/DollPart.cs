using System;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DollPart : MonoBehaviour, IDollPart
{
    public enum DollPartType
    {
        Head,
        Body,
        Arm,
        Leg
    }

    [Flags]
    public enum DollPartDescriptor
    {
        Normal                  = 0,
        Broken                  = 1 << 0,
        Robotic                 = 1 << 1,
        Animalistic             = 1 << 2,
        Aquatic                 = 1 << 3,
        Avian                   = 1 << 4,
        CanGrab                 = 1 << 5,
        Mythical                = 1 << 6,
        Strong                  = 1 << 7,
        AllTerrain              = 1 << 8,
        OneEye                  = 1 << 9,
        BreatheUnderwater       = 1 << 10,
        Fast                    = 1 << 11
    }

    [SerializeField]
    private bool _isLeft;
    private bool _updateSprite;
    public bool isLeft
    {
        get => _isLeft;
        set
        {
            _isLeft = value;
            _updateSprite = true;
        }
    }

    public DollPartType partType;
    public DollPartDescriptor descriptor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_updateSprite)
        {
            _updateSprite = false;
            FlipSprite(_isLeft);
        }
    }

    private void OnValidate()
    {
        isLeft = _isLeft;
    }

    public void AttachToDoll(Doll doll)
    {
        switch (partType)
        {
            case DollPartType.Head:
                transform.SetParent(doll.headSocket);
                break;
            case DollPartType.Body:
                transform.SetParent(doll.transform);
                break;
            case DollPartType.Arm:
                if (isLeft)
                {
                    transform.SetParent(doll.lArmSocket);
                }
                else
                {
                    transform.SetParent(doll.rArmSocket);
                }
                break;
            case DollPartType.Leg:
                if (isLeft)
                {
                    transform.SetParent(doll.lLegSocket);
                }
                else
                {
                    transform.SetParent(doll.rLegSocket);
                }
                break;
        }
    }

    public void DetachFromDoll(Doll doll)
    {
        transform.SetParent(null);
    }

    public void FlipSprite(bool flip)
    {
        if (flip)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
