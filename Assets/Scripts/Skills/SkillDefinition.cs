using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SkillSystem
{
    [CreateAssetMenu(menuName = "Gameplay/Skills/New Skill")]
    public class SkillDefinition : ScriptableObject
    {
        [Header("UI")]
        public string Name = string.Empty;

        [Header("Cast")]
        public GameObject Prefab;
        public SkillBehavior Behavior;
        //add any effects, cooldowns or costs here.

        [Header("Damage")]
        public int Damage;

        [Header("Projectile")]
        public GameObject ProjectilePrefab;
        public float Speed;
        public float DestroyAfterTime = 2f;
        public bool DestroyOnCollision = true;

        public void Cast(SkillDefinition _definition,int _playerId, Vector3 _position, Quaternion _rotation, out string uniqueKey, Transform _parent = null)
        {
            var newSkill = Instantiate(_definition.Prefab, _position, _rotation, _parent);
            string uniqueSkillKey = String.Format("{0}-{1}-{2}", _playerId.ToString(), _definition.Name, Guid.NewGuid());
            uniqueKey = uniqueSkillKey;
            newSkill.GetComponent<SkillBehavior>().CasterId = uniqueSkillKey;
        }
    }
}