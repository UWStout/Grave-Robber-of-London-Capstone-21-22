using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Direction ExitDirection;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int MapEntrance = GameManager.Instance._RunManager.GetMapInfo().GetGraveyardEntrancePosition();
        if ((MapEntrance.x >= MapEntrance.y && ExitDirection != Direction.DOWN) || (MapEntrance.y > MapEntrance.x && ExitDirection != Direction.LEFT))
        {
            this.gameObject.SetActive(false);
        }
    }

    // Exits the graveyard on contact
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.EndRun();
        }
    }
}
