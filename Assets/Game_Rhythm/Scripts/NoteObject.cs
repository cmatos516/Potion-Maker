using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;

    private bool obtained = false;

    public KeyCode keyToPress;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
                obtained = true;
                //GameManager.instance.NoteHit();

                if(Mathf.Abs (transform.position.y) > 0.25)
                {
                    Debug.Log("Hit");
                  GameManager.instance.NormalHit();
                  Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
                
                else if (Mathf.Abs (transform.position.y) > 0.05)
                {
                    Debug.Log("Good");
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }

                else
                {
                    Debug.Log("Perfect");
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator" && gameObject.activeSelf)
        {
            canBePressed = false;
            if (!obtained)
            {
                GameManager.instance.NoteMissed();
                Instantiate(missEffect, transform.position, missEffect.transform.rotation);
            }
        }
    }
}
