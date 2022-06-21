using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{
    //public float health;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRadius;

    public float purchasePrice;
    public float sellingPrice;

    private GameObject targetTrack;

    [HideInInspector] public bool isFire = false;
    //[HideInInspector] public int turretTowerSideIndex;
    //[HideInInspector] public int turretExampleIndex;

    private Animator animator;
    private AudioSource audiosource;
    //private SoundEffector soundEffector;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        //soundEffector = GameObject.FindGameObjectWithTag("SoundEffector").GetComponent<SoundEffector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator.SetBool("isFire", true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isFire)
        {
            IEnumerable<GameObject> list = TrackController.instance.activeTracks;
            IEnumerable<GameObject> result = list.Reverse();

            foreach (var track in result)
            {
                if (Vector3.Distance(track.transform.position, transform.position) <= attackRadius)
                {
                    animator.SetBool("isFire", true);
                    isFire = true;
                    targetTrack = track;

                    audiosource.loop = true;
                    audiosource.Play();
                    //SoundEffector.isTurretSound[turretTowerSideIndex] = true;
                    //soundEffector.PlayTurretAttackSound(turretExampleIndex, turretTowerSideIndex);
                    //targetTrack.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    //Debug.Log(animator.GetBool("isFire"));
                }
            }
        }

        if (isFire && !TrackController.instance.activeTracks.Contains(targetTrack))
        {
            animator.SetBool("isFire", false);
            isFire = false;
            audiosource.loop = false;
            audiosource.Stop();
            //SoundEffector.isTurretSound[turretTowerSideIndex] = false;
            targetTrack = null;
        }

        if (isFire)
        {
            //targetTrack.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

            //Vector3 direction = targetTrack.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.LookAt(targetTrack.transform, new Vector3(0, 0, 1));
            Vector3 dir = targetTrack.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            targetTrack.GetComponent<Track>().GetDamage(damage * attackSpeed * Time.deltaTime);

            
            if (Vector3.Distance(targetTrack.transform.position, transform.position) > attackRadius)
            {
                animator.SetBool("isFire", false);
                isFire = false;
                audiosource.loop = false;
                audiosource.Stop();
                //SoundEffector.isTurretSound[turretTowerSideIndex] = false;
                //targetTrack.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                targetTrack = null;
            }
        }

        //if (animator.GetBool("isFire") && !GameController.instance.activeTracks.Contains(targetTrack))
        //{
        //    animator.SetBool("isFire", false);
        //    targetTrack = null;
        //}
    }
}
