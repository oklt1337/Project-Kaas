using Collection.NetworkPlayer.Scripts;
using UnityEngine;
using static Collection.GameManager.Scripts.GameManager;

namespace Collection.Items.Scripts
{
    public class ItemBoxBehaviour : MonoBehaviour
    {
        
        /// <summary>
        /// Gets a new Item for the player based on the position.
        /// </summary>
        /// <param name="position"> The position of the car. </param>
        /// <returns> The new item. </returns>
        private ItemBehaviour GetNewItem(byte position)
        {
            ItemBehaviour item = null;
            var rng = Random.Range(0, 101);
            
            switch (position)
            {
                case 1:
                    if (rng > 10)
                    {
                        item = Gm.AllItems[0];
                    }
                    else if(rng > 5)
                    {
                        item = Gm.AllItems[1];
                    }
                    else
                    {
                        item = Gm.AllItems[3];
                    }
                    break;
                case 2:
                    if (rng > 60)
                    {
                        item = Gm.AllItems[0];
                    }
                    else if(rng > 25)
                    {
                        item = Gm.AllItems[5];
                    }
                    else
                    {
                        item = Gm.AllItems[3];
                    }
                    break;
                case 3:
                    if (rng > 90)
                    {
                        item = Gm.AllItems[0];
                    }
                    else if(rng > 5)
                    {
                        item = Gm.AllItems[1];
                    }
                    else
                    {
                        item = Gm.AllItems[5];
                    }
                    break;
                case 4:
                    if (rng > 50)
                    {
                        item = Gm.AllItems[3];
                    }
                    else if(rng > 35)
                    {
                        item = Gm.AllItems[1];
                    }
                    else
                    {
                        item = Gm.AllItems[5];
                    }
                    break;
                case 5:
                    if (rng > 65)
                    {
                        item = Gm.AllItems[4];
                    }
                    else if(rng > 5)
                    {
                        item = Gm.AllItems[2];
                    }
                    else
                    {
                        item = Gm.AllItems[6];
                    }
                    break;
                case 6:
                    if (rng > 50)
                    {
                        item = Gm.AllItems[4];
                    }
                    else if(rng > 15)
                    {
                        item = Gm.AllItems[5];
                    }
                    else
                    {
                        item = Gm.AllItems[1];
                    }
                    break;
                case 7:
                    if (rng > 50)
                    {
                        item = Gm.AllItems[4];
                    }
                    else if(rng > 25)
                    {
                        item = Gm.AllItems[2];
                    }
                    else
                    {
                        item = Gm.AllItems[6];
                    }
                    break;
                case 8:
                    if (rng > 20)
                    {
                        item = Gm.AllItems[6];
                    }
                    else if(rng > 5)
                    {
                        item = Gm.AllItems[2];
                    }
                    else
                    {
                        item = Gm.AllItems[4];
                    }
                    break;
            }
            return item;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // 'Destroys' the item box after collision and gives player Item. 
            if(!other.CompareTag("Player") && !other.isTrigger)
                return;
            
            print("penis");
            gameObject.SetActive(false);
            var player = other.gameObject.GetComponent<PlayerHandler>();
            player.SetItem(GetNewItem(player.Position));
        }
    }
}
