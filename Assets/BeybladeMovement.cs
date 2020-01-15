using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladeMovement : MonoBehaviour
{
    public GameObject sparksPrefab;
    public GameObject playerObject;
    public GameObject lightningPrefab;

    public float moveSpeed = 1f;
    public float attackDmg = 5;
    public float currentPower = 5;
    public float health = 2000;

    public float xAngle = 0f;
    public float yAngle = 10.0f;
    public float zAngle = 0f;

    public float xConForce;
    public float yConForce = -5;
    public float zConForce;

    public float conForceMax = 125;
    public float conForceMin = -125;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // show lightning sfx before death
            GameObject lightningSfx = Instantiate(lightningPrefab, transform.position, transform.rotation);
            Destroy(lightningSfx, .5f);

            // destroy beyblade
            Destroy(playerObject, 1); // not: Destroy(this, 1); which deletes BeybladeMovement script
        }

        setAttackPower();

        xConForce = Random.Range(conForceMin, conForceMax);
        zConForce = Random.Range(conForceMin, conForceMax);

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        transform.Rotate(xAngle, yAngle, xAngle, Space.Self);

        GetComponent<ConstantForce>().force = new Vector3(xConForce, yConForce, zConForce);

        if (conForceMax < 0)
            conForceMax = 0;

        if (conForceMin > 0)
            conForceMin = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            takeDamage(collision.gameObject.GetComponent<BeybladeMovement>().currentPower);
            GameObject sparksOnContact = Instantiate(sparksPrefab, transform.position, transform.rotation);
            Destroy(sparksOnContact, .5f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "TopOfArena")
        {
            attackDmg = 15.0f;
        }

        else if (collision.gameObject.name == "MiddleOfArena")
        {
            attackDmg = 10.0f;
        }

        else if (collision.gameObject.name == "BottomOfArena")
        {
            attackDmg = 5.0f;
        }
    }

    void setAttackPower()
    {
        currentPower = (attackDmg + xConForce + zConForce);
    }

    void takeDamage(float enemyDmg)
    {
        health -= enemyDmg;
    }
}