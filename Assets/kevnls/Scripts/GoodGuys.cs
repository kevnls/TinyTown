using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class GoodGuys : MonoBehaviour
    {

        public bool areSpawning;
        public float spawnRateSeconds = 10.0F;
        public int maxGoodGuys = 5;

        private float nextSpawn = 0.0F;
        private GameObject[] doors;

        private string[] characters = { "chr_bananaman", "chr_beardo1", "chr_beardo2", "chr_beardo3", "chr_beardo4", "chr_bridget", "chr_bro", "chr_brookie", "chr_butcher", "chr_chef", "chr_cop1", "chr_cop2", "chr_costume1", "chr_costume2", "chr_costume3", "chr_costume4", "chr_eskimo", "chr_fatkid", "chr_goth1", "chr_goth2", "chr_goth3", "chr_hazmat1", "chr_hazmat2", "chr_headphones", "chr_hunter1", "chr_hunter2", "chr_janitor", "chr_lady1", "chr_lady2", "chr_lady3", "chr_lady4", "chr_mailman", "chr_mayor", "chr_mechanic", "chr_mike", "chr_mission1", "chr_mission2", "chr_naked1", "chr_naked2", "chr_naked3", "chr_naked4", "chr_naked5", "chr_naked6", "chr_nun", "chr_nurse", "chr_paramedic1", "chr_paramedic2", "chr_ponytail1", "chr_ponytail2", "chr_ponytail3", "chr_priest", "chr_punk", "chr_raver1", "chr_raver2", "chr_raver3", "chr_riotcop", "chr_scientist", "chr_sign1", "chr_sign2", "chr_sports1", "chr_sports2", "chr_sports3", "chr_sports4", "chr_suit1", "chr_suit2", "chr_suit3", "chr_suit4", "chr_sumo1", "chr_sumo2", "chr_super4", "chr_super5", "chr_thief", "chr_worker1", "chr_worker2", "chr_worker3" };

        void Start()
        {
            doors = GameObject.FindGameObjectsWithTag("Door");   
        }

        void FixedUpdate()
        {
            if (areSpawning)
            {
                if (Time.fixedTime > nextSpawn)
                {
                    nextSpawn = Time.fixedTime + spawnRateSeconds;

                    int currentNumberOfGoodGuys = GameObject.FindGameObjectsWithTag("GoodGuy").Length;

                    if (currentNumberOfGoodGuys < maxGoodGuys)
                    {
                        SpawnCharacter();
                    }
                }
            }
        }

        private void SpawnCharacter()
        {
            Transform spawnLocation = GetRandomSpawnTransform(doors);
            Transform destinationLocation = GetRandomSpawnTransform(doors);

            GameObject spawnedCharacter = (GameObject)Instantiate(Resources.Load(GetRandomCharacter()), spawnLocation.position, spawnLocation.rotation);

            spawnedCharacter.GetComponent<GoodGuyAI>().destinationDoorLocation = destinationLocation;
        }

        private string GetRandomCharacter()
        {
            int index = Random.Range(0, characters.Length);
            return characters[index];
        }

        private Transform GetRandomSpawnTransform(GameObject[] gameObjectArray)
        {
            int index = Random.Range(0, gameObjectArray.Length);
            return gameObjectArray[index].transform;
        }
    }
}