using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
{

    public IUsable MyUsable { get; set; }

    [SerializeField]
    private TextMeshProUGUI stackSize;

    private Stack<IUsable> usables = new Stack<IUsable>();
    private int count;

    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount { get => count; }

    public TextMeshProUGUI MyStackText { get => stackSize; }

    // Start is called before the first frame update

    [SerializeField]
    private Image icon;

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (HandScript.MyInstance.MyMovable == null) //Solamente le puedo picar si no tengo nada en la mano
        {
            if (MyUsable != null)
            {
                MyUsable.Use();
            }
            if (usables != null && usables.Count > 0)
            {
                usables.Peek().Use();
            }
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMovable != null && HandScript.MyInstance.MyMovable is IUsable)
            {
                SetUsable(HandScript.MyInstance.MyMovable as IUsable);
            }
        }
    }

    public void SetUsable(IUsable usable)
    {
        if (usable is Item)
        {
            usables = InventoryScript.MyInstance.GetUsables(usable);
            count = usables.Count;
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {
            this.MyUsable = usable;
        }
        
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        //Si el item es el mismo que tenemos en este boton
        if (item is IUsable && usables.Count > 0)
        {
            if (usables.Peek().GetType() == item.GetType())
            {
                usables = InventoryScript.MyInstance.GetUsables(item as IUsable);
                count = usables.Count;
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

}
