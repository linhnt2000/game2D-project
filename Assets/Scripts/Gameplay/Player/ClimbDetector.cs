using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetector : MonoBehaviour
{
    [SerializeField] private ClimbStatTest test;
    [SerializeField] private Collider2D myCol;
    [SerializeField] private bool isClimbing;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enter(collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Exit(collision.collider);
    }

    private void Enter(Collider2D collider)
    {
        isClimbing = true;
        float dirToWall = Mathf.Sign(collider.transform.position.x - transform.position.x);
        PlayerMovement.instance.OnClimbingWall(dirToWall);
        StartCoroutine(VirtualForces(dirToWall));
    }

    private void Exit(Collider2D collider)
    {
        PlayerMovement.instance.moveDirectionLock = 0;
        if (isClimbing)
        {
            isClimbing = false;
            PlayerMovement.instance.Normalize();
            PlayerMovement.instance.anim.SetTrigger("Jump");          
            StopAllCoroutines();
        }      
    }  

    IEnumerator VirtualForces(float direction)
    {
        WaitForFixedUpdate delay = new WaitForFixedUpdate();

        while (true)
        {
            yield return delay;
            PlayerMovement.instance.AddForce(direction * Vector2.right * test.forceValue);
        }
    }

    internal void TemporarilyDisable(float waitTime = -1)
    {
        enabled = false;
        isClimbing = false; 
        StopAllCoroutines();
        Invoke("MakeActive", waitTime == -1 ? test.disableTime : waitTime);
    }

    private void MakeActive()
    {
        myCol.enabled = false;
        enabled = true;
        myCol.enabled = true;
    }

    private Collider2D[] m_hits = new Collider2D[3];
    internal void OnJumpCheck()
    {
        if (Physics2D.OverlapCircleNonAlloc(transform.position, 2, m_hits, LayerMask.GetMask("Climb Only")) >= 2 && PlayerMovement.instance.groundCheck.isGround)
        {
            Debug.Log("b");
            TemporarilyDisable(test.deltaDisableTime);
        }
    }
}
