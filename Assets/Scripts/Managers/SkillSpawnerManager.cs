using ServerUtils;
using SkillSystem;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawnerManager : SingletonObject<SkillSpawnerManager>
{
    public static List<KeyValuePair<string, Vector3>> skillPositions = new List<KeyValuePair<string, Vector3>>();
    public SkillsSpawnDefinition skillsSpawnDef;

    [SerializeField]
    private PickupableSkill pickupableSkillPrefab;

    public float minSpawnPositionX;
    public float maxSpawnPositionX;

    public float minSpawnPositionZ;
    public float maxSpawnPositionZ;

    private float totalWeight;

    void Start()
    {
        foreach (SkillData skill in skillsSpawnDef.skills)
        {
            totalWeight += skill.weight;
        }

        //TODO: Change this for some kind of rubber banding plz
        InvokeRepeating("SpawnItem", 10f, 10f);
    }

    public void SpawnItem()
    {
        if(Server.clients.Values.Count > 0)
        {
            Vector3 skillSpawnPosition = new Vector3(Random.Range(minSpawnPositionX, maxSpawnPositionX), 0.78f, Random.Range(minSpawnPositionZ, maxSpawnPositionZ));

            float rValue = Random.Range(0, totalWeight);
            foreach (SkillData skill in skillsSpawnDef.skills)
            {
                rValue -= skill.weight;
                if (rValue < 0)
                {
                    PickupableSkill pickupableSkillInstance = Instantiate(pickupableSkillPrefab, skillSpawnPosition, Quaternion.identity);
                    pickupableSkillInstance.skillName = skill.SkillName;

                    //TODO: Delete this dictionary after the lobby implementation
                    skillPositions.Add(new KeyValuePair<string, Vector3>(pickupableSkillInstance.skillName, skillSpawnPosition));

                    ServerSend.SkillSpawned(skill.SkillName, skillSpawnPosition);
                    break;
                }
            }
        }
    }

    public static void SpawnExistingSkills(int clientId)
    {
        foreach (KeyValuePair<string, Vector3> item in skillPositions)
        {
            ServerSend.SkillSpawned(clientId, item.Key, item.Value);
        }
    }

    public SkillDefinition GetSkill(string skillId)
    {
        foreach (SkillData skill in skillsSpawnDef.skills)
        {
            return skill.definition;
        }

        return null;
    }
}
