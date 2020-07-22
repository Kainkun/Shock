using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentSlots : MonoBehaviour
{
    List<Image> slots;
    GameObject slotImagePrefab;

    float nextSlotXPosition;
    public float widthBetweenSlots = 40;
    int maxSlots = 5;

    Image CurrentSlot
    {
        get { return slots[CurrentSlotIndex]; }
        set { slots[CurrentSlotIndex] = value; }
    }

    int currentSlotIndex;
    public int CurrentSlotIndex
    {
        get
        {
            return currentSlotIndex;
        }
        set
        {
            CurrentSlot.color = Color.grey;
            currentSlotIndex = value;
            CurrentSlot.color = Color.white;
        }
    }

    public int SlotCount
    {
        get { return slots.Count; }
        set
        {
            int difference = value - SlotCount;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    AddSlot();
                }
            }
            else if(difference < 0)
            {
                for (int i = 0; i < -difference; i++)
                {
                    RemoveSlot();
                }
            }

        }
    }

    private void Awake()
    {
        slotImagePrefab = Resources.Load("Prefabs/EquipmentSlot") as GameObject;
        slots = new List<Image>();
        //slots = new List<Image>(GetComponentsInChildren<Image>());
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SlotCount++;
        if (Input.GetKeyDown(KeyCode.O))
            SlotCount--;
    }

    GameObject AddSlot()
    {
        if (SlotCount >= maxSlots)
            return null;

        GameObject addedSlot = Instantiate(slotImagePrefab, transform);
        Image image = addedSlot.GetComponent<Image>();
        slots.Add(image);
        image.color = Color.grey;

        addedSlot.transform.localPosition = new Vector3(nextSlotXPosition, 0, 0);
        nextSlotXPosition += widthBetweenSlots;

        Vector3 v = transform.position;
        v.x = Screen.width/2 - nextSlotXPosition / 3;
        transform.position = v;

        return addedSlot;
    }
    void RemoveSlot()
    {
        if (SlotCount <= 0)
            return;

        GameObject removedSlot = slots[slots.Count - 1].gameObject;
        slots.RemoveAt(slots.Count - 1);
        Destroy(removedSlot);

        nextSlotXPosition -= widthBetweenSlots;

        Vector3 v = transform.position;
        v.x = Screen.width / 2 - nextSlotXPosition / 3;
        transform.position = v;
    }
}
