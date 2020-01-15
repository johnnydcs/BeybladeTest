using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladeMovement : MonoBehaviour
{
    public GameObject sparksPrefab;
    public GameObject playerObject;
    public GameObject lightningPrefab;

    public float attackDmg = 5;
    public float bonusDmg = 10;
    public float currentPower = 15;
    public float health = 2000;

    public float rotationX, rotationY, rotationZ;

    public float xAngle = 0f;
    public float yAngle = 10.0f;
    public float zAngle = 0f;

    public float xConForce;
    public float yConForce = -5;
    public float zConForce;

    public float conForceMax = 225;
    public float conForceMin = -225;

    // Update is called once per frame
    void Update()
    {
        checkHealth();
        setAttackPower();
        checkTilt();
        randomMovement();
    }

    void checkHealth()
    {
        if (health <= 0)
        {
            // show lightning sfx before death
            GameObject lightningSfx = Instantiate(lightningPrefab, transform.position, transform.rotation);
            Destroy(lightningSfx, .5f);

            // destroy beyblade
            Destroy(playerObject, 1); // not: Destroy(this, 1); which deletes BeybladeMovement script
        }
    }

    void checkTilt()
    {
        rotationX = transform.localEulerAngles.x;
        rotationY = transform.localEulerAngles.y;
        rotationZ = transform.localEulerAngles.z;

        if (rotationX > 60.0f)
        {
            rotationX = 60.0f;
        }

        else if (rotationX < -60.0f)
        {
            rotationX = -60.0f;
        }

        transform.localEulerAngles = new Vector3(0, rotationY, rotationZ);
    }

    void randomMovement()
    {
        xConForce = Random.Range(conForceMin, conForceMax);
        zConForce = Random.Range(conForceMin, conForceMax);

        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.Rotate(xAngle, yAngle, xAngle, Space.Self);

        GetComponent<ConstantForce>().force = new Vector3(xConForce, yConForce, zConForce);
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
        if (collision.gameObject.name == "OutOfArena")
        {
            health = 0;
        }

        if (collision.gameObject.name == "TopOfArena")
        {
            attackDmg = 50.0f;
        }

        else if (collision.gameObject.name == "MiddleOfArena")
        {
            attackDmg = 25.0f;
        }

        else if (collision.gameObject.name == "BottomOfArena")
        {
            attackDmg = 5.0f;
        }
    }

    void setAttackPower()
    {
        if (rotationX > 0)
            bonusDmg = 60.0f + (rotationX / 30.0f);
        else if (rotationX < 0)
            bonusDmg = 60.0f - (rotationX / 30.0f);
        else
            bonusDmg = 10.0f;

        currentPower = (attackDmg + bonusDmg);
    }

    void takeDamage(float enemyDmg)
    {
        health -= enemyDmg;
    }
}