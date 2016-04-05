using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class Projectile : MonoBehaviour
    {

        public float lifetime = 0.0F;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "GoodGuy" || other.gameObject.tag == "BadGuy")
            {
                other.gameObject.SendMessage("GotHit");
                Destroy(gameObject);
            }
        }
    }
}
