using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public IMovable MyMovable { get; set; }

    private Image icon;

    [SerializeField]
    private Vector3 offset;
    

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;
        DeleteItem();
    }

    public void TakeMovable(IMovable movable)
    {
        this.MyMovable = movable;
        icon.sprite = movable.MyIcon;
        icon.color = Color.white;
    }

    public IMovable Put()
    {
        IMovable tmp = MyMovable;
        MyMovable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    public void Drop()
    {
        MyMovable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMovable != null)
        {
            if (MyMovable is Item && InventoryScript.MyInstance.FromSlot != null)
            {
                (MyMovable as Item).MySlot.Clear();
            }

            Drop();
            InventoryScript.MyInstance.FromSlot = null;
        }
    }
}
