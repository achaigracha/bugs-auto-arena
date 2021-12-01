using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public Team player;
    public Team enemy;
    public float fightDelay = 5f;
    List<CardController> playerCards;
    List<CardController> enemyCards;
    public Transform playerSlotsParent;


    private float timeSinceLastFight;
    private int currentSlot = 0;
    private bool fightOver = true;
    private CardController playerCard;
    private CardController enemyCard;
    private Transform[] playerSlots;

    void Start()
    {
        playerSlots = playerSlotsParent.GetComponentsInChildren<Transform>();
        playerCards = player.GetCards();
        enemyCards = enemy.GetCards();
      
    }


    void Update()
    {
        if (!fightOver && timeSinceLastFight > fightDelay)
        {
            FightAlgorithm();
            timeSinceLastFight = 0;
            
        }

        timeSinceLastFight += Time.deltaTime;
    }

    public void FightAlgorithm()
    {
        
        if (currentSlot < playerCards.Count)
        {
        playerCard = playerCards[currentSlot];

        }

        if (currentSlot < enemyCards.Count)
        {
         enemyCard = enemyCards[currentSlot];
        }

        if (playerCard.CurrentPower <= 0)
        {
             playerCard = FindAliveMember(playerCards);
        }
        if (enemyCard.CurrentPower <= 0)
        {
             enemyCard = FindAliveMember(enemyCards);
        }


        if (playerCard != null)
        {
            if (enemyCard != null)
            {
                fightOver = false;
            }
            else
            {
                print("player won game");
                fightOver = true;
                return;
            }
        }
        else
        {
            print("player lost game");
            fightOver = true;
            return;
        }

        print("Fight : " + playerCard.BugName + " vs " + enemyCard.BugName);

        int playerPower = playerCard.CurrentPower;
        int enemyPower = enemyCard.CurrentPower;

        enemyCard.TakeDamage(playerPower);
        playerCard.TakeDamage(enemyPower);

        currentSlot++;
        if (currentSlot >= Mathf.Max(playerCards.Count, enemyCards.Count))
        {
            currentSlot = 0;
        }
        
    }


    private CardController FindAliveMember(List<CardController> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].CurrentPower > 0)
            {
                return members[i];
            }
        }
        return null;
    }

    public void BeginCombat()
    {

        playerCards = player.GetCards();
        enemyCards = enemy.GetCards();

        PlacePlayerCards();
    }
    private void PlacePlayerCards()
    {
        for (int i = 1; i < playerSlots.Length; i++)
        {

            if (playerCards[i - 1] != null)
            {

                GameObject newCard = Instantiate(playerCards[i - 1].gameObject, playerSlots[i].position, Quaternion.identity, playerSlots[i]);
                GameObject oldCard = playerCards[i - 1].gameObject;
                playerCards[i - 1] = newCard.GetComponent<CardController>();
                Destroy(oldCard.gameObject);
            }

        }
    }
}
