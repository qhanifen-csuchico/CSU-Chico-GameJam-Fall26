using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DollSpriteContainer", menuName = "Scriptable Objects/DollSpriteContainer")]
public class DollSpriteContainer : ScriptableObject
{
    [Header("Doll Sprite array")]
    public List<Sprite> headSprites;
    public List<Sprite> armSprites;
    public List<Sprite> legSprites;
}
