using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace kevnls
{

    public class BadGuyAI : MonoBehaviour
    {

        public float distanceFromDestinationToDestroyCharacter;
        public Transform destinationLocation;
        public Transform victimLocation;
        public bool isTalking = true;
        public bool doorChaser = false;
        public bool isShooter = false;
        public float projectileForce = 75.0F;
        public float fireRate = 1.0F;
        public float shooterMinDistance = 10.0F;

        private string isSaying = "Bzzzt Bazzzook!!";
        private NavMeshAgent navAgent;
        private GameObject deathParticles;
        private Text speechText;
        private Image speechBubble;
        private float blabberRate = 11.4F;
        private float speechBubbleTime = 1.0F;
        private float nextBlab = 0.0F;
        private float nextBubblePop = 0.0F;
        private int fireRange = 1000;
        private float nextFire = 0.0F;
        private bool isDying = false;

        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            deathParticles = transform.Find("DeathParticles").gameObject;
            speechBubble = transform.Find("BadGuyCanvas").gameObject.GetComponentInChildren<Image>();
            speechText = speechBubble.GetComponentInChildren<Text>();
            GetComponentInChildren<LookAtCamera>().SourceCamera = Camera.main;
            speechBubble.enabled = false;
            speechText.enabled = false;
        }

        void Update()
        {
            if (!isDying)
            {
                MoveCharacterTowardDestination();

                if (isTalking)
                {
                    //bad guys just talk on a set timer at any distance from the player
                    Talk();

                    if (Time.fixedTime > nextBubblePop)
                    {
                        //pops the speech bubble
                        speechBubble.enabled = false;
                        speechText.enabled = false;
                    }
                }

                if (victimLocation == null)
                {
                    WatchForVictims();
                }

                if (isShooter)
                {
                    if (Time.fixedTime > nextFire)
                    {
                        nextFire = Time.fixedTime + fireRate;

                        // Create a vector from the enemy to the victim and store the angle between it and forward.
                        Vector3 direction = victimLocation.position - transform.position;
                        float angle = Vector3.Angle(direction, transform.forward);

                        //If the angle between forward and the victim is less than x degrees
                        if (angle < 0.5F)
                        {
                            RaycastHit hit;

                            //and if a raycast towards the victim hits something
                            if (Physics.Raycast(transform.position, direction.normalized, out hit, fireRange))
                            {
                                //and if the raycast hits the victim
                                if (hit.collider.gameObject.transform == victimLocation)
                                {
                                    Fire();
                                }
                            }
                        }
                    }
                }

                if (doorChaser)
                {
                    if (CheckIfInsideDestroyRange())
                    {
                        DestroyGameCharacter();
                    }
                }
            }
            else
            {
                Die();
            }
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag == "GoodGuy")
            {
                //bad guys (including robots) will kill good guys if they run into them
                col.gameObject.SendMessage("GotHit");
            }
            else if (col.gameObject.tag == "Player")
            {
                //bad guys (excluding robots) will infect the player and die if they run into them
                if (!isShooter)
                {
                    col.gameObject.SendMessage("GotInfected");
                    isDying = true;
                }
            }
        }

        private void Fire()
        {
            Transform projectileSpawnLeft = transform.FindChild("ProjectileSpawnLeft");
            Transform projectileSpawnRight = transform.FindChild("ProjectileSpawnRight");

            GameObject projectile1 = (GameObject)Instantiate(Resources.Load("Projectile"), projectileSpawnLeft.position, Quaternion.identity);
            GameObject projectile2 = (GameObject)Instantiate(Resources.Load("Projectile"), projectileSpawnRight.position, Quaternion.identity);

            projectile1.GetComponent<Rigidbody>().AddForce((victimLocation.position - projectileSpawnLeft.position).normalized * projectileForce);
            projectile2.GetComponent<Rigidbody>().AddForce((victimLocation.position - projectileSpawnRight.position).normalized * projectileForce);
        }

        private void MoveCharacterTowardDestination()
        {
            //head to the original destination unless you have a victim
            if (victimLocation != null)
            {
                if (isShooter)
                {
                    //stay a short distance away from the victim
                    navAgent.destination = (transform.position - victimLocation.transform.position).normalized * shooterMinDistance + victimLocation.transform.position;

                    //rotate to face the victim
                    Quaternion rotate = Quaternion.LookRotation(victimLocation.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 35.0F);
                }
                else
                {
                    navAgent.destination = victimLocation.position;
                }
            }
            else
            {
                navAgent.destination = destinationLocation.position;
            }
        }

        private void WatchForVictims()
        {
            //keep an eye out for possible victims only if you don't have one
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10.0F);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "GoodGuyCollider")
                {
                    victimLocation = hitColliders[i].transform;

                    //send a message to the victim to tell them you're after them
                    hitColliders[i].gameObject.SendMessageUpwards("BeingChased");
                    break;
                }
                else if (hitColliders[i].tag == "Player")
                {
                    victimLocation = hitColliders[i].transform;
                    break;
                }
                i++;
            }
        }

        private void Talk()
        {
            if (Time.fixedTime > nextBlab)
            {
                nextBlab = Time.fixedTime + blabberRate;

                isSaying = Story.GetPhrase("BadGuy");

                speechBubble.enabled = true;
                speechText.enabled = true;
                speechText.text = isSaying;
                nextBubblePop = Time.fixedTime + speechBubbleTime;
            }
        }

        //this is only used for aliens who's original destination is a door
        private bool CheckIfInsideDestroyRange()
        {
            float distance = Vector3.Distance(transform.position, destinationLocation.position);

            if (distance < distanceFromDestinationToDestroyCharacter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GotHit()
        {
            isDying = true;
            GameObject.Find("GameController").GetComponent<GameState>().BadGuyKilled();
        }

        private void Die()
        {
            //stops update from trying to move the character on the nav mesh
            gameObject.GetComponent<NavMeshAgent>().enabled = false;

            //turns off the colliders
            gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;

            //hides the character
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

            //stops the rocket particle emitter if the character has one
            Transform hoverRocket = gameObject.transform.FindChild("HoverRocket");
            if (hoverRocket != null)
            {
                hoverRocket.GetComponent<ParticleSystem>().Stop();
            }

            //hides speech bubble
            speechBubble.enabled = false;
            speechText.enabled = false;

            //shows death particles
            deathParticles.GetComponent<ParticleSystem>().Play();

            //this should match the length of the particle emission
            Invoke("DestroyGameCharacter", .75f);
        }

        private void DestroyGameCharacter()
        {
            Destroy(gameObject);
        }

    }
}
