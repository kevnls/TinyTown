using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace kevnls
{

    public class GoodGuyAI : MonoBehaviour
    {

        public float distanceFromDestinationToDestroyCharacter;
        public Transform destinationDoorLocation;
        
        private Transform player;
        private bool iAmStopped = false;
        private string isSaying;
        private NavMeshAgent navAgent;
        private GameObject deathParticles;
        private Text speechText;
        private Image speechBubble;
        private float blabberRate = 8.7F;
        private float speechBubbleTime = 3.0F;
        private float nextBlab = 0.0F;
        private float nextBubblePop = 0.0F;
        private bool isDying = false;

        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            deathParticles = transform.Find("DeathParticles").gameObject;
            speechBubble = transform.Find("GoodGuyCanvas").gameObject.GetComponentInChildren<Image>();
            speechText = speechBubble.GetComponentInChildren<Text>();
            player = GameObject.Find("Player").transform;
            GetComponentInChildren<LookAtCamera>().SourceCamera = Camera.main;
            speechBubble.enabled = false;
            speechText.enabled = false;
        }

        void Update()
        {
            if (!isDying)
            {
                if (Vector3.Distance(transform.position, player.position) < 5.0F)
                {
                    //good guys only talk when the player is near them
                    Talk();

                    iAmStopped = true;

                    //stops the character
                    navAgent.Stop();

                    //rotates the character
                    Quaternion rotate = Quaternion.LookRotation(player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 4.0F);

                    //stops the animation
                    gameObject.GetComponentInChildren<Animator>().enabled = false;
                }
                else
                {
                    if (iAmStopped)
                    {
                        navAgent.Resume();
                        gameObject.GetComponentInChildren<Animator>().enabled = true;
                        iAmStopped = false;
                    }

                    MoveCharacterTowardDestinationDoor();
                }

                if (Time.fixedTime > nextBubblePop)
                {
                    //pops the speech bubble
                    speechBubble.enabled = false;
                    speechText.enabled = false;
                }

                if (CheckIfInsideDestroyRange())
                {
                    DestroyGameCharacter();
                }
            }
            else
            {
                Die();
            }
        }

        private void MoveCharacterTowardDestinationDoor()
        {
            navAgent.destination = destinationDoorLocation.position;
        }

        private void Talk()
        {
            if (Time.fixedTime > nextBlab)
            {
                nextBlab = Time.fixedTime + blabberRate;

                isSaying = Story.GetPhrase("GoodGuy");

                speechBubble.enabled = true;
                speechText.enabled = true;
                speechText.text = isSaying;
                nextBubblePop = Time.fixedTime + speechBubbleTime;
            }
        }

        private bool CheckIfInsideDestroyRange()
        {
            float distance = Vector3.Distance(transform.position, destinationDoorLocation.position);

            if (distance < distanceFromDestinationToDestroyCharacter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void BeingChased()
        {
            //double time it!
            GetComponent<NavMeshAgent>().speed = 3.0F;
            GetComponentInChildren<Animator>().speed = 1.6F;
        }

        public void GotHit()
        {
            isDying = true;
            GameObject.Find("GameController").GetComponent<GameState>().GoodGuyKilled();
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

            //hides speech bubble
            speechBubble.enabled = false;
            speechText.enabled = false;

            //shows death particles
            deathParticles.GetComponent<ParticleSystem>().Play();

            //this should match the length of the particle emission
            Invoke("DestroyGameCharacter", .85f);
        }

        private void DestroyGameCharacter()
        {
            Destroy(gameObject);
        }

    }
}
