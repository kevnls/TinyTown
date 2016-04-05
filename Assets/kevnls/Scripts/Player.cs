using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class Player : MonoBehaviour
    {
        private GameState gameState;
        private Gun gun;
        private int gunRange = 1000;

        // Use this for initialization
        void Start() 
        {
            gameState = GameObject.Find("GameController").GetComponent<GameState>();
            //this might cause problems if I add the ability to switch guns, if so move to Fire()
            //leaving for performance reasons
            gun = GetComponentInChildren<Gun>();
	    }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
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
