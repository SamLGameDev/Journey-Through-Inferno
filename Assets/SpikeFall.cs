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
    // Start is called before the first frame update
    void Start()
    {
        particles.gameObject.SetActive(false);
    }
    private void DetectUnderneath()
    {
        originalPosition = transform.position;
        if (notHit == false)
        {
            notHit = false;
        }


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
        }
        shadow.transform.localPosition = new Vector2(0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(Damage);
    }
    // Update is called once per frame
    void Update()
    {
        if (notHit)
        {
            DetectUnderneath();
            timeBeforeFall = Time.time;
            return;
        }
        else if(Time.time - TimeOfPreFall > timeBeforeFall)
        {
            StopAllCoroutines();
            fall();
            return;
        }
        PreFallEffects();
        StartCoroutine(Rumble());

    }
}
