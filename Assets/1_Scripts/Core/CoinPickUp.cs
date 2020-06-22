using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CoinType { Gold = 3, Silver = 2, Copper = 1}
public class CoinPickUp : MonoBehaviour
{
    [SerializeField] CoinType Coin;
    public SoundClipsInts soundCue = SoundClipsInts.GoldPickUp;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           if (other.gameObject.GetComponent<PlayerInventory>() != null)
            other.gameObject.GetComponent<PlayerInventory>().gold += (int)Coin;
            MusicManager.Instance.PlaySoundTrack(soundCue);
            Destroy(gameObject);
            
        }
    }

}

