using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public abstract class SkillBehavior : MonoBehaviour
    {
        [SerializeField]
        protected SkillDefinition Definition;
        [SerializeField]
        public string CasterId;
    }
}