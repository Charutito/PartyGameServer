using System.Collections;
using UnityEngine;
using SkillSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Transform shootOrigin;
    public float health;
    public float maxHealth = 100f;
    public int itemAmount = 0;
    public int maxItemAmount = 3;
    public float respawnTime = 5f;
    public float MovementSpeed = 6f;

    [SerializeField]
    private Rigidbody rigidBody;
    private Vector3 Velocity;
    private Vector3 VelocityLastFrame;
    private Vector3 AccelerationVector;
    private float Acceleration { get; set; }
    private float Deceleration { get; set; }
    private Vector3 normalizedInput;
    private Vector3 lerpedInput;
    private float acceleration;
    private float movementSpeed;

    public SkillDefinition currentSkill;

    private float[] inputs;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new float[2];

        Acceleration = 10f;
        Deceleration = 10f;
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        if (health <= 0f)
        {
            return;
        }

        Velocity = rigidBody.velocity;
        AccelerationVector = (rigidBody.velocity - VelocityLastFrame) / Time.fixedDeltaTime;

        MoveTransform(inputs[0], inputs[1]);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void MoveTransform(float x, float z)
    {
        Vector3 _movementVector = Vector3.zero;
        Vector3 _currentInput = Vector3.zero;

        _currentInput.x = x;
        _currentInput.z = z;

        normalizedInput = _currentInput.normalized;

        if ((Acceleration == 0) || (Deceleration == 0))
        {
            lerpedInput = _currentInput;
        }
        else
        {
            if (normalizedInput.magnitude == 0)
            {
                acceleration = Mathf.Lerp(acceleration, 0f, Deceleration * Time.deltaTime);
                lerpedInput = Vector3.Lerp(lerpedInput, lerpedInput * acceleration, Time.deltaTime * Deceleration);
            }
            else
            {
                acceleration = Mathf.Lerp(acceleration, 1f, Acceleration * Time.deltaTime);
                lerpedInput = Vector3.ClampMagnitude(normalizedInput, acceleration);
            }
        }

        _movementVector.x = lerpedInput.x;
        _movementVector.y = 0f;
        _movementVector.z = lerpedInput.z;

        movementSpeed = MovementSpeed * 1f;

        _movementVector *= movementSpeed;

        if (_movementVector.magnitude > MovementSpeed)
        {
            _movementVector = Vector3.ClampMagnitude(_movementVector, MovementSpeed);
        }

        Vector3 newMovement = rigidBody.position + _movementVector * Time.fixedDeltaTime;
        rigidBody.MovePosition(newMovement);

        ServerSend.PlayerPosition(this);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    public void SetInput(float[] _inputs)
    {
        inputs = _inputs;
    }

    public void Shoot(Vector3 _facingDirection, string _skillId)
    {
        //TODO USE FACING DIRECTION TO GET ROTATION FOR SKILL
        string uniqueKey = string.Empty;

        currentSkill.Cast(currentSkill, id, shootOrigin.position, Quaternion.identity, out uniqueKey);
        ServerSend.SkillCasted(id, uniqueKey);
    }

    public void TakeDamage(float _damage)
    {
        if (health <= 0f)
        {
            return;
        }

        health -= _damage;
        if (health <= 0f)
        {
            Die();
        }

        ServerSend.PlayerHealth(this);
    }

    private void Die()
    {
        health = 0f;
        //TODO dead animation
        int randomIndex = Random.Range(0, GameManager.Instance.respawnPoints.Length);
        transform.position = GameManager.Instance.respawnPoints[randomIndex].transform.position;
        ServerSend.PlayerPosition(this);
        StartCoroutine(Respawn(respawnTime));
    }

    private IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("Player " + id + " is respawning...");
        health = maxHealth;
        ServerSend.PlayerRespawned(this);
    }

    public bool AttemptPickupItem()
    {
        if (itemAmount >= maxItemAmount)
        {
            return false;
        }

        itemAmount++;
        return true;
    }
}
