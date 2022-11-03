using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiquidParticle : MonoBehaviour
{
    [SerializeField] LiquidColor liquidColor;
    Animator anim;
    Rigidbody2D rb;

    public LiquidColor _LiquidColor => liquidColor;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Invoke(nameof(CheckIfOrphan), 6f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cork"))
        {
            DestroyAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteriorColl"))
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            Flask targetFlask = collision.GetComponentInParent<Flask>();
            bool isHalfFull = targetFlask.IsHalfFullOfIndicatedLiquid();

            if (targetFlask.contentList.Count>=targetFlask.Limit || targetFlask.CurrentColorClass.liquidColor != this.liquidColor && !isHalfFull)
            {
                DestroyAnimation();
            }
            else
            {
                StartCoroutine(DisableTrailsCoroutine());
                this.transform.SetParent(targetFlask.transform);
                targetFlask.contentList.Add(this.gameObject);
                targetFlask.LiquidParticlesCount[liquidColor]++;
                rb.velocity = new Vector2(0, -2);
                rb.gravityScale = .4f;
                targetFlask.CheckIfCompleted();
                //SoundsManager.instance.Play_LiquidSound();
            }
        }
    }

    IEnumerator DisableTrailsCoroutine()
    {
        yield return new WaitForSeconds(1.25f);
        DisableTrail();
    }

    public void DestroyAnimation()
    {
        DisableTrail();
        float rnd = Random.Range(0.2f, 0.5f);
        transform.DOScale(0, rnd);
        Destroy(gameObject, rnd + .1f);
    }

    public void CheckIfOrphan()
    {
        if (this.transform.parent == null)
        {
            DestroyAnimation();
        }
    }

    public void DisableTrail()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }
}
