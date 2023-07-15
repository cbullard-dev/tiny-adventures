using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
  [SerializeField] private int coinValue = 1;
  private string _coinName;

  private void Awake()
  {
    if (coinValue <= 0 || coinValue > 3) return;
    if (coinValue == 1) _coinName = "BronzeCoin";
    if (coinValue == 2) _coinName = "SilverCoin";
    if (coinValue == 3) _coinName = "GoldCoin";
  }

  // Start is called before the first frame update

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag != "Player" || !other.GetComponent<PlayerController>().PlayerAlive()) return;
    AudioManager.Instance.Play(_coinName);
    GameManager.Instance.PlayerScore += coinValue;
    Destroy(this.gameObject);
  }
}
