using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DollPrefabCollection", menuName = "Scriptable Objects/DollPrefabCollection")]
public class DollPrefabCollection : ScriptableObject
{
    [Header("Prefab Collections")]
    [Tooltip("Base prefab")]
    [SerializeField]
    public GameObject basePrefab;

    [Tooltip("List of all head prefabs")]
    [SerializeField]
    public List<GameObject> headPrefabs;

    [Tooltip("List of all arm prefabs")]
    [SerializeField]
    public List<GameObject> armPrefabs;
    [Tooltip("List of all leg prefabs")]
    [SerializeField]
    public List<GameObject> legPrefabs;
}
