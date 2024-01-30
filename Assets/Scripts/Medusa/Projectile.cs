using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class Projectile : MonoBehaviour
{
    public MedusaBehaviour mb;
    public Transform indicator;

    [Tooltip("Position we want to hit")]
    private Vector3 targetPos;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 5000;

    public float flightTime;

    Vector3 startPos;

    float passedTime;

    void Start()
    {
        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        startPos = transform.position;

        targetPos = indicator.position;
    }

    void Update()
    {
        // Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float nextX = Mathf.Lerp(startPos.x, x1, passedTime / flightTime);
        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.5f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.position = nextPos;

        // Do something when we reach the target
        if (nextPos == targetPos) Arrived();

        passedTime += Time.deltaTime;
    }

    void Arrived()
    {
        mb.TriggerDamage(indicator.transform.gameObject);
        Destroy(gameObject);
    }

    /// 
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// forward direction
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 90);
    }
}
