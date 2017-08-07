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
        public int infectedTriggerCount = 5;
        public float infectedRageDuration = 10.0F;

        private float nextRain;

        void Start()
        {
            GameState.infectedRageDuration = infectedRageDuration;
            GameState.infectedTriggerCount = infectedTriggerCount;
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
                RenderSettings.fogDensity = 8.0F;
                RenderSettings.skybox.SetColor("_Tint", clearTint);
            }
            else
            {
                rain.SetActive(true);
                RenderSettings.fogDensity = 2.0F;
                RenderSettings.skybox.SetColor("_Tint", rainTint);
            }
        }
    }
}
