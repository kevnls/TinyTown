using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

namespace kevnls
{

    public class GameState : MonoBehaviour
    {

        private static int badGuysKilled = 0;
        private static int goodGuysKilled = 0;
        private static int playerHits = 0;
        private static int playerInfecteds = 0;
        private static GameObject HUD;
        private static GameObject score;
        private static Text kills;
        private static StringBuilder sb = new StringBuilder();

        void Start()
        {
            HUD = GameObject.Find("HUD");
            score = HUD.transform.Find("Score").gameObject;
            kills = score.GetComponentInChildren<Text>();
        }

        void Update()
        {
            sb.Append("Player Hits: ");
            sb.AppendLine(playerHits.ToString());
            sb.Append("Player Infection: ");
            sb.AppendLine(playerInfecteds.ToString());
            sb.Append("Bad Guys Killed: ");
            sb.AppendLine(badGuysKilled.ToString());
            sb.Append("Good Guys Killed: ");
            sb.Append(goodGuysKilled.ToString());

            kills.text = sb.ToString();

            sb.Remove(0, sb.Length);
        }

        public void BadGuyKilled()
        {
            badGuysKilled++;
        }

        public void GoodGuyKilled()
        {
            goodGuysKilled++;
        }

        public void PlayerHit()
        {
            playerHits++;
        }

        public void PlayerInfected()
        {
            playerInfecteds++;
        }
    }
}
