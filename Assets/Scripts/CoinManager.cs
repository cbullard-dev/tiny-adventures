using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
  [SerializeField] private int coinValue = 1;
  private string coinName;

  private void Awake()
  {
    if (coinValue <= 0 || coinValue > 3) return;
    if (coinValue == 1) coinName = "BronzeCoin";
    if (coinValue == 2) coinName = "SilverCoin";
    if (coinValue == 3) coinName = "GoldCoin";
  }

  // Start is called before the first frame update

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag != "Player") return;
    AudioManager.Instance.Play(coinName);
    GameManager.Instance.PlayerScore += coinValue;
    Destroy(this.gameObject);
  }
}
