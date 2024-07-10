using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;

    public float distance = 7f;
    [SerializeField]
    private LayerMask mask;

    private Interactable lastItem;
    private Interactable currentItem;

    private PlayerFieldOfView fov;
    private PlayerUI playerUI;

    public bool showPrompts;
    // TODO: consolidate player states like tutorial into a separate script
    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<PlayerFieldOfView>();
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {   
        // in decreasing priority, select from the interactables layer:
        // the object hit by the raycast
        // the closest object within a range of player view (w/in 3 ft of player, w/in 20 degrees of center of camera ray)
        // nothing
        Interactable selectedInteractable = null;

        // raycast
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction  * distance);  
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, distance, mask)) {
            Debug.Log("interactable raycast hit");
            selectedInteractable = hitInfo.collider.GetComponent<Interactable>();
        }
        

        // if nothing is hit, check for closest items within field of view
        if (selectedInteractable == null) {
            Debug.Log("raycast hit nothing, checking for closest objects in FOV");
            GameObject closestObjectWithInteractable = fov.GetClosestObjectInLayer();
            if (closestObjectWithInteractable != null) {
                selectedInteractable = closestObjectWithInteractable.GetComponent<Interactable>();
                Debug.Log("selecting closest object");
            }
            

        }


        UpdateSelectedItem(selectedInteractable);

    }

    // call UpdateHit EVERY UPDATE, even if no hit is found.
    // 
    // newHit can be null
    void UpdateSelectedItem(Interactable newItem) {
        // update lastHit and currentHit
        Debug.Log("UpdateSelectedItem called");
        if (lastItem == null && currentItem == null && newItem != null) {
            Debug.Log("last current new");
        }
        lastItem = currentItem;
        currentItem = newItem;
        
        // if we have changed from one object to another object or null    
        if (currentItem != lastItem && lastItem != null) { 
            UnHighlightInteractable(lastItem);
        }
        if (currentItem != lastItem && currentItem != null) { 
            HighlightInteractable(currentItem);
        }


    }

    void HighlightInteractable(Interactable obj) {
        Debug.Log("highlighted" + obj.gameObject.name);
        if (showPrompts) {
            playerUI.UpdatePromptText(obj.promptMessage);
        }
    }

    void UnHighlightInteractable(Interactable obj) {
        Debug.Log("unhighlighted" + obj.gameObject.name);
        if (showPrompts) {
            playerUI.UpdatePromptText("");
        }
    }

    // triggered when player presses interact button
    public void TriggerInteract() {
        if (currentItem != null) {
            currentItem.BaseInteract();
        }
        
    }
}
