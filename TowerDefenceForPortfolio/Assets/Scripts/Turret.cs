using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{
    //public float health;
    public float damage;
    public float attackSpeed;
    public float attackRadius;

    private GameObject targetTrack;

    private Animator animator;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator.SetBool("isFire", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("isFire"))
        {
            IEnumerable<GameObject> list = GameController.instance.activeTracks;
            IEnumerable<GameObject> result = list.Reverse();

            foreach (var track in result)
            {
                if (Vector3.Distance(track.transform.position, transform.position) <= attackRadius)
                {
                    animator.SetBool("isFire", true);
                    targetTrack = track;
                    //Debug.Log(animator.GetBool("isFire"));
                }
            }
        }

        if (animator.GetBool("isFire"))
        {
            //Vector3 direction = targetTrack.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.LookAt(targetTrack.transform, new Vector3(0, 0, 1));
            Vector3 dir = targetTrack.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Vector3.Distance(targetTrack.transform.position, transform.position) > attackRadius)
            {
                animator.SetBool("isFire", false);
                targetTrack = null;
            }
        }
    }
}
