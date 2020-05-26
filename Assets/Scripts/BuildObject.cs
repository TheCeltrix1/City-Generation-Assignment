using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildObject", order = 100)]
public class BuildObject : ScriptableObject
{
    public Mesh[] baseObjects;
    public Mesh[] middleObjects;
    public Mesh[] topObjects;

    public Material[] colourScheme1;
    public Material[] colourScheme2;
    public Material[] colourScheme3;

    public Material accent;
    public Material neon;
    public Material[] interior;

    public int maxHeight;
}
