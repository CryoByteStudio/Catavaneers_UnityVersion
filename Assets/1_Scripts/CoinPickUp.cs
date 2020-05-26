using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CoinType { Gold, Silver, Copper}
public class CoinPickUp : MonoBehaviour
{
    [SerializeField] CoinType Coin;
    public SoundClipsInts soundCue = SoundClipsInts.GoldPickUp;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           
            if (Coin == CoinType.Gold) other.gameObject.GetComponent<PlayerInventory>().gold += 100;
            else if (Coin == CoinType.Silver) other.gameObject.GetComponent<PlayerInventory>().gold += 50;
            else if (Coin == CoinType.Copper) other.gameObject.GetComponent<PlayerInventory>().gold += 10;

            MusicManager.Instance.PlaySoundTrack(soundCue);
            Destroy(gameObject);
        }
    }

}
