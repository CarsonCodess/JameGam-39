using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SpriteSet
{
    [FormerlySerializedAs("animationId")] public string name;
    [FormerlySerializedAs("animationFrameRate")] public float speed;
    public string nextAnimation;
    public bool loop;
    [Space(4.5f)] public List<Sprite> animationFrames;
}

