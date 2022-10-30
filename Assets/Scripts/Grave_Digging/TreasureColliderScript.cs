using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureColliderScript : MonoBehaviour
{
    public MoneyExplosion Money;
    bool Inside = false;

    private float LiveTime; // Amount of time treasure has been active.

    private Rigidbody RB;

    private float GroundLevel;

    private void Start()
    {
        //Money = transform.parent.gameObject.GetComponent<MoneyExplosion>();
        RB = GetComponent<Rigidbody>();

        float RandAngle = Random.Range(0, 2 * Mathf.PI);
        float RandPower = Random.Range(1.75f, 2.5f);

        GroundLevel = transform.position.y;

        RB.velocity = new Vector3(RandPower * Mathf.Cos(RandAngle), 3.0f, RandPower * Mathf.Sin(RandAngle));
    }

    private void Update()
    {
        LiveTime += Time.deltaTime;

        if (transform.position.y < GroundLevel && RB.detectCollisions)
        {
            RB.velocity = Vector3.zero;
            //RB.detectCollisions = false;
            RB.useGravity = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(LiveTime);
        if (other.tag == "Player" && LiveTime >= 0.75f)
        {
            //Debug.Log("Pickup");
            MoneyPickUp();
        }
    }

    private void MoneyPickUp()
    {
        //Money.TreasurePickUp(this.gameObject);
        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.LOW)
        {
            GameManager.Instance.UpdateMoney(Random.Range(1, 3));
        }
        else if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.MEDIUM)
        {
            GameManager.Instance.UpdateMoney(Random.Range(2, 5));
        }
        else if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.HIGH)
        {
            GameManager.Instance.UpdateMoney(Random.Range(3, 7));
        }

        GameManager.Instance.CompleteTask(GoalType.Treasures, BodyQuality.Fresh);

        Destroy(this.gameObject);
    }
}
