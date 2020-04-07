using UnityEngine;

public class PickupableSkill : MonoBehaviour
{
    public string skillName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            if (_player.AttemptPickupItem())
            {
                SkillPickedUp(_player);
            }
        }
    }

    private void SkillPickedUp(Player player)
    {
        ServerSend.SkillPickedUp(skillName, player.id);
        player.currentSkill = SkillSpawnerManager.Instance.GetSkill(skillName);
        Destroy(this.gameObject);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
#endif
}
