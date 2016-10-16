using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private ParticleSystem m_HitParticles;

    private float fireDamage;
    private float fireStrikeArmor;
    private float fireStunTime;
    private int slowDownLevel;
    Vector3 m_HitPoint;
    private Vector3 m_AttackerPotision;
    private BasicEnemy m_BasicEnemy;
    public int speed=15;

    private Vector3 m_TargetPos;
    private Vector3 m_Direction;

    public void Start()
    {
        m_HitParticles = GetComponent<ParticleSystem>();
    }

    public void Init(float fireDamage, float fireStrikeArmor, float fireStunTime, int slowDownLevel,Vector3 m_AttackerPotision,BasicEnemy m_BasicEnemy)
    {
        this.fireDamage = fireDamage;
        this.fireStrikeArmor = fireStrikeArmor;
        this.fireStunTime = fireStunTime;
        this.slowDownLevel = slowDownLevel;
        this.m_AttackerPotision = m_AttackerPotision;
        this.m_BasicEnemy = m_BasicEnemy;
    }

    private void FixedUpdate()
    {
        if (m_BasicEnemy!=null)
        {
            m_Direction = m_TargetPos - transform.position;
            m_TargetPos = m_BasicEnemy.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BasicEnemy>()==m_BasicEnemy)
        {
            m_HitPoint = transform.position;
            m_BasicEnemy.TakeDamage(fireDamage, fireStrikeArmor, fireStunTime, slowDownLevel, m_HitPoint, m_AttackerPotision);
            Destroy(gameObject);
        }
    }
}
