using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class Player : MonoBehaviour
    {
        private GameState gameState;
        private Gun gun;
        private int gunRange = 1000;
        private bool isRaging = false;
        private float rageDuration;
        private float rageTimer = 0.0F;

        // Use this for initialization
        void Start() 
        {
            gameState = GameObject.Find("GameController").GetComponent<GameState>();
            //this might cause problems if I add the ability to switch guns, if so write a SwitchGun() function
            gun = GetComponentInChildren<Gun>();
	    }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }

            if (isRaging)
            {
                //countdown timer for rage duration
                if (Time.fixedTime > rageTimer)
                {
                    rageTimer = Time.fixedTime + rageDuration;
                    //do rage (updates)
                    
                }
                else
                {
                    EndInfectedRage();
                    
                }
            }
        }

        private void GotHit()
        {
            gameState.PlayerHit();
        }

        private void GotInfected()
        {
            gameState.PlayerInfected();
        }

        private void StartInfectedRage(float duration)
        {
            isRaging = true;
            rageDuration = duration;
            //do rage (settings, one-time)
        }

        private void EndInfectedRage()
        {
            rageTimer = 0.0F;
            isRaging = false;
            //end rage (settings, one-time)
        }

        private void Fire()
        {
            gun.Fire();

            RaycastHit hit;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out hit, gunRange))
            {
                if (hit.transform.gameObject.tag == "BadGuy" || hit.transform.gameObject.tag == "GoodGuy")
                {
                    //tell the gameObject that it got hit
                    hit.transform.gameObject.SendMessage("GotHit");
                }
            }
        }
    }
}
