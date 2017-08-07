using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class BadGuys : MonoBehaviour
    {

        public bool areSpawning;
        public float spawnRateSeconds = 10.0F;
        public int maxBadGuys = 5;

        private byte spawnSwitcher = 0;
        private float nextSpawn = 0.0F;
        private GameObject[] doors;
        private GameObject[] ships;
        private GameObject player;

        private string[] regularAliens = { "alien_bot1", "alien_bot2", "alien_bot3", "alien_engi1a", "alien_engi1b", "alien_engi1c", "alien_engi2a", "alien_engi2b", "alien_engi2c", "alien_eye1a", "alien_eye1b", "alien_eye1c" };

        private string[] infectedGoodGuys = { "alien_infected1", "alien_infected2", "alien_infected3" };

        void Start()
        {
            doors = GameObject.FindGameObjectsWithTag("Door");
            ships = GameObject.FindGameObjectsWithTag("Ship");
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void FixedUpdate()
        {
            if (areSpawning)
            {
                if (Time.fixedTime > nextSpawn)
                {
                    nextSpawn = Time.fixedTime + spawnRateSeconds;

                    int currentNumberOfBadGuys = GameObject.FindGameObjectsWithTag("BadGuy").Length;

                    if (currentNumberOfBadGuys < maxBadGuys)
                    {
                        SpawnCharacter();
                    }
                }
            }
        }

        private void SpawnCharacter()
        {
            Transform spawnLocation;
            GameObject spawnedCharacter;
            bool infectedGoodGuy = false;

            //back and forth between regular aliens and infecteds
            if (spawnSwitcher == 0)
            {
                infectedGoodGuy = true;
                spawnSwitcher = 1;
            }
            else
            {
                spawnSwitcher = 0;
            }

            //regular aliens come from ships and infecteds come from doors
            if (infectedGoodGuy == true)
            {
                spawnLocation = GetRandomSpawnTransform(doors);
                spawnedCharacter = (GameObject)Instantiate(Resources.Load(GetRandomCharacter(infectedGoodGuys)), spawnLocation.position, spawnLocation.rotation);
            }
            else
            {
                spawnLocation = GetRandomSpawnTransform(ships);
                spawnedCharacter = (GameObject)Instantiate(Resources.Load(GetRandomCharacter(regularAliens)), spawnLocation.position, spawnLocation.rotation);
            }

            //inspect the 4 different types of bad guys and tell them where they're going
            if (spawnedCharacter.name.Contains("bot"))
            {
                //these guys will only target the player, they already have a victim
                spawnedCharacter.GetComponent<BadGuyAI>().victimLocation = player.transform;
            }
            else if (spawnedCharacter.name.Contains("infected"))
            {
                //these guys head to the player, but can be interrupted by other victims
                spawnedCharacter.GetComponent<BadGuyAI>().destinationLocation = player.transform;
            }
            else if (spawnedCharacter.name.Contains("engi"))
            {
                //these guys head to doors unless they're interrupted by other victims, including the player
                spawnedCharacter.GetComponent<BadGuyAI>().destinationLocation = GetRandomSpawnTransform(doors);
                spawnedCharacter.GetComponent<BadGuyAI>().isDoorChaser = true;
            }
            else if (spawnedCharacter.name.Contains("eye"))
            {
                //these guys head to doors unless they're interrupted by other victims, including the player
                spawnedCharacter.GetComponent<BadGuyAI>().destinationLocation = GetRandomSpawnTransform(doors);
                spawnedCharacter.GetComponent<BadGuyAI>().isDoorChaser = true;
            }
        }

        private string GetRandomCharacter(string[] typeOfAlien)
        {
                int index = Random.Range(0, typeOfAlien.Length);
                return typeOfAlien[index];
        }

        private Transform GetRandomSpawnTransform(GameObject[] gameObjectArray)
        {
            int index = Random.Range(0, gameObjectArray.Length);
            return gameObjectArray[index].transform;
        }
    }
}