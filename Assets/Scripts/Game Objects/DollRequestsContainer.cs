using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static DollRequestManger;

[CreateAssetMenu(fileName = "DollRequestsContainer", menuName = "Scriptable Objects/DollRequestsContainer")]
public class DollRequestsContainer : ScriptableObject
{
    [Header("Container for Doll Request List")]
    public List<DollRequestList> requestLists;
}
