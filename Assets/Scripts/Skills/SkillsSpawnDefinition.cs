using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay /Skills/SkillsSpawnDefinition")]
public sealed class SkillsSpawnDefinition : ScriptableObject
{
    [Header("Skills Name and Weight")]
    [SerializeField]
    public List<SkillData> skills;
}
