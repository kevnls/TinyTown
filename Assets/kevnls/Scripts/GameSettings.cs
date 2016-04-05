using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class GameSettings : MonoBehaviour
    {

        public bool showCursor = false;
        public bool hasOccasionalRain = false;
        public float rainFrequency = 20.0F;
        public GameObject rain;
        public Color rainTint;
        public Color clearTint;

        private float nextRain;

        void Start()
        {
            Cursor.visible = showCursor;
            rain = GameObject.FindGameObjectWithTag("Rain");
        }

        void FixedUpdate()
        {
            if (hasOccasionalRain)
            {
                if (Time.fixedTime > nextRain)
                {
                    nextRain = Time.fixedTime + rainFrequency;

                    SwitchRain();
                }
            }
        }

        void SwitchRain()
        {
            if (rain.activeSelf)
            {
                rain.SetActive(false);
                RenderSettings.skybox.SetColor("_Tint", clearTint);
            }
            else
            {
                rain.SetActive(true);
                RenderSettings.skybox.SetColor("_Tint", rainTint);
            }
        }
    }
}
