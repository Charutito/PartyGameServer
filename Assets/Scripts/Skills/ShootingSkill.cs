using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.Skills
{
    public class ShootingSkill : SkillBehavior
    {
        public float speed = 5f;

        private void Start()
        {
            Destroy(this.gameObject, Definition.DestroyAfterTime);
        }

        void FixedUpdate()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            ServerSend.SkillPosition(CasterId, transform.position);
        }
    }
}