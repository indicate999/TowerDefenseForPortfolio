using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRadius;

    public float purchasePrice;
    public float sellingPrice;

    private GameObject targetTrack;

    [HideInInspector] public bool isFire = false;

    private Animator animator;
    private AudioSource audiosource;

    //This method displays the attack radius of the turret in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Here the beginning of the turret attack is programmed if the track is within its reach.
        if (!isFire)
        {
            IEnumerable<GameObject> list = TrackController.instance.activeTracks;
            IEnumerable<GameObject> result = list.Reverse();

            foreach (var track in result)
            {
                if (Vector3.Distance(track.transform.position, transform.position) <= attackRadius)
                    StartFire(track);
            }
        }

        //Here the end of the turret attack is programmed if the track has been destroyed
        if (isFire && !TrackController.instance.activeTracks.Contains(targetTrack))
            StopFire();


        if (isFire)
        {
            //Here, the turn of the turret towards the track that is under its attack is programmed.
            Vector3 dir = targetTrack.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            targetTrack.GetComponent<Track>().GetDamage(damage * attackSpeed * Time.deltaTime);

            //Here the attack of the turret stops if the track goes out of reach
            if (Vector3.Distance(targetTrack.transform.position, transform.position) > attackRadius)
                StopFire();
        }
    }

    //Everything related to the beginning of the attack is programmed here
    private void StartFire(GameObject track)
    {
        animator.SetBool("isFire", true);
        isFire = true;
        targetTrack = track;

        audiosource.loop = true;
        audiosource.Play();
    }

    //Everything related to the end of the attack is programmed here
    private void StopFire()
    {
        animator.SetBool("isFire", false);
        isFire = false;
        targetTrack = null;

        audiosource.loop = false;
        audiosource.Stop();
    }
}
