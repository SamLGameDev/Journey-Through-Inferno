using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFall : MonoBehaviour
{
    [SerializeField]
    private LayerMask whatToFallOn;
    [SerializeField]
    private float speedOfFall;
    public bool notHit = true;
    private Vector2 originalPosition;
    [SerializeField]
    private float fallDistance;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private float TimeOfPreFall;
    [SerializeField]
    private int Damage;
    private float timeBeforeFall;
    [SerializeField]
    private GameObject shadow;
    [SerializeField]
    private float shadowGrowSpeed;

    private State _state = State.idle;
    public State SetState { set { _state = value; } }

    // Start is called before the first frame update
    void Start()
    {
        particles.gameObject.SetActive(false);
        Player_movement.pvP_Enabled = true;
    }
    private void DetectUnderneath()
    {
        originalPosition = transform.position;
        if (notHit == false)
        {
            notHit = false;
        }


    }
    public enum State
    {
        idle,
        prefall,
        falling,
        fallen
    }
    private void Awake()
    {
         shadow.transform.localPosition = new Vector2(0, (fallDistance/3) - 0.35f);
    }
    private IEnumerator Rumble()
    {
        while (true)
        {
            yield return null;
            transform.position = new Vector2(transform.position.x - 0.25f, transform.position.y);
            yield return null;
            transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
            yield return null;
            transform.position = new Vector2(transform.position.x - 0.25f, transform.position.y);
        }
    }
    private void PreFallEffects()
    {
        particles.gameObject.SetActive(true);

    }
    private void fall()
    {
        if (transform.position.y >= originalPosition.y - fallDistance)
        {
            transform.position += transform.up * Time.deltaTime * speedOfFall;
            shadow.transform.localScale = new Vector3(shadow.transform.localScale.x + shadowGrowSpeed, shadow.transform.localScale.y + shadowGrowSpeed, shadow.transform.localScale.z);
        }
        else
        {
            particles.gameObject.SetActive(false);
            SetState = State.fallen;
        }
        shadow.transform.localPosition = new Vector2(0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_state != State.fallen)
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(Damage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.idle:
                {
                    DetectUnderneath();
                    timeBeforeFall = Time.time;
                    break;
                }
            case State.falling:
                {
                    StopAllCoroutines();
                    fall();
                    break;
                }
            case State.prefall:
                {
                    PreFallEffects();
                    StartCoroutine(Rumble());
                    if (Time.time - TimeOfPreFall > timeBeforeFall)
                    {
                        SetState = State.falling; break;
                    }
                    break;
                }
        }

    }
}
