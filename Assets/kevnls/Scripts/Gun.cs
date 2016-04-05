using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class Gun : MonoBehaviour
    {

        GameObject flarePoint;
        ParticleSystem flare;

        void Start ()
        {
            flarePoint = transform.Find("FlareSpawn").gameObject;
            flare = flarePoint.GetComponent<ParticleSystem>();
        }

        public void Fire()
        {
            flare.Play();
        }
    }
}
