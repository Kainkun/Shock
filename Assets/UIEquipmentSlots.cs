using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentSlots : MonoBehaviour
{
    List<(Image ImageUI, Equipment Equipment)> slots;
    GameObject slotImagePrefab;

    float nextSlotXPosition;
    public float widthBetweenSlots = 40;
    int maxSlots = 5;

    public (Image ImageUI, Equipment Equipment) CurrentSlot
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
            int v;

            if (SlotCount == 0)
                v = 0;
            else
                v = Manager.mod(value, SlotCount);

            CurrentSlot.ImageUI.color = Color.grey;
            currentSlotIndex = v;
            CurrentSlot.ImageUI.color = Color.white;
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
        slots = new List<(Image ImageUI, Equipment Equipment)>();
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

    public (Image ImageUI, Equipment Equipment) AddSlot(Equipment equipment = null)
    {
        if (SlotCount >= maxSlots)
            return (null, null);

        GameObject addedSlot = Instantiate(slotImagePrefab, transform);
        Image image = addedSlot.GetComponent<Image>();

        if (equipment != null && equipment.Icon != null)
            image.sprite = equipment.Icon;

        slots.Add((image, equipment));
        image.color = Color.grey;

        addedSlot.transform.localPosition = new Vector3(nextSlotXPosition, 0, 0);
        nextSlotXPosition += widthBetweenSlots;

        Vector3 v = transform.position;
        v.x = Screen.width/2 - nextSlotXPosition / 3;
        transform.position = v;

        return slots[slots.Count - 1];
    }
    public void RemoveSlot()
    {
        if (SlotCount <= 0)
            return;

        GameObject removedSlot = slots[slots.Count - 1].ImageUI.gameObject;
        slots.RemoveAt(slots.Count - 1);
        Destroy(removedSlot);

        nextSlotXPosition -= widthBetweenSlots;

        Vector3 v = transform.position;
        v.x = Screen.width / 2 - nextSlotXPosition / 3;
        transform.position = v;
    }
}
