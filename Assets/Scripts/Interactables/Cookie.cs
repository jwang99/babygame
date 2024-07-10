using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        promptMessage = "[e] to interact";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact() {
        base.Interact();
        Debug.Log("interacted with" + gameObject.name);
        // interacting triggers the player into eating the cookie
        // TODO: have player play animation to eat the cookie, then make the cookie disappear. For now just have the cookie disappear.
        gameObject.SetActive(false);
    }


}
