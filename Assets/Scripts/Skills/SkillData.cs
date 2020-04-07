using SkillSystem;
using System;
using UnityEngine;

[Serializable]
public class SkillData
{
    [SerializeField]
    public string SkillName;
    [SerializeField]
    public SkillDefinition definition;
    [SerializeField]
    public int weight;
}
