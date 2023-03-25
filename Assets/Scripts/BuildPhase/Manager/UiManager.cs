using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class UiManager : Singleton<UiManager>
{


    public VisualElement root;
    public Button BuildToggle;
    public Button FoldOut_CardView;
    public VisualElement CardContainer;
    public VisualElement AbsoluteContainer;
    public VisualElement Card_View;

    List<VisualElement> _card_slots = new List<VisualElement>();


    private bool isDragging = false;
    private Vector3 pointerStartPosition;
    private VisualElement target;
    private Vector2 targetStartPosition { get; set; }
    private Dictionary<string, VisualElement> _cards = new Dictionary<string, VisualElement>();

    private void OnEnable()
    {
        root = this.GetComponent<UIDocument>().rootVisualElement;
        BuildToggle = root.Q<Button>("Button_BuildToggle");
        BuildToggle.clicked += ToggleBuildMode;
        CardContainer = root.Q<VisualElement>("VE_Right_Base");
        AbsoluteContainer = root.Q<VisualElement>("VE_Absolute");
        Card_View = root.Q<VisualElement>("Card_View");
        FoldOut_CardView = root.Q<Button>("Expand_Card_View");
        FoldOut_CardView.clicked += ToggleFold;

        InitCardSlots();
    }

    void InitCardSlots()
    {
        foreach (var cardslot in Card_View.Children())
        {
            _card_slots.Add(cardslot);
        }
    }


    private void OnDisable()
    {
        BuildToggle.clicked -= ToggleBuildMode;
        FoldOut_CardView.clicked -= ToggleFold;

    }


    public void ToggleFold()
    {
        Card_View.ToggleInClassList("fold_view");
    }



    public void ToggleBuildMode()
    {
        BuildManager buildManager = BuildManager.Instance;

        if (buildManager.BuildMode == BuildMode.Move)
        {
            buildManager.SetBuildMode(BuildMode.Detail);
            BuildToggle.text = "Toggle Detail Mode";
        }
        else
        {
            buildManager.SetBuildMode(BuildMode.Move);
            BuildToggle.text = "Toggle Move Mode";
        }
    }

    public void CreateCard(RoomCard c)
    {
        VisualElement cardslot = _card_slots.Where(x => x.childCount == 0).FirstOrDefault();

        if (cardslot == null)
            return;

        if (_cards.ContainsKey(c.Id))
            return;


     

        VisualTreeAsset vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BuildPhase/UI/Card.uxml");
        VisualElement vE = vta.Instantiate();

        CreateBitMap(RoomSaveData.LoadRoom(c.Room), vE);

        vE.name = c.Id;
        vE.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
        vE.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        cardslot.Add(vE);
        _cards.Add(c.Id, cardslot);

        vE.RegisterCallback<PointerDownEvent>(PointerDownEvent);
        vE.RegisterCallback<PointerMoveEvent>(PointerMoveEvent);
        vE.RegisterCallback<PointerUpEvent>(PointerUpEvent);
    }

    public void DeleteCard(RoomCard c)
    {
        VisualElement vE = _cards[c.Id];
        if (vE == null)
            return;

        vE.UnregisterCallback<PointerDownEvent>(PointerDownEvent);
        vE.UnregisterCallback<PointerMoveEvent>(PointerMoveEvent);
        vE.UnregisterCallback<PointerUpEvent>(PointerUpEvent);

        vE.parent.Remove(vE);
    }


    void CreateBitMap(Room r, VisualElement v)
    {

        Texture2D texture = new Texture2D(r.BoundingBox.Width, r.BoundingBox.Height);

        foreach (var tile in r.Tiles)
        {
            Color c = new Color();
            if (tile.TileType == TileType.edge)
                c = Color.grey;
            else if (tile.TileType == TileType.door)
                c = Color.black;
            else
                c = Color.green;

            texture.SetPixel((int)tile.X, (int)tile.Y, c);
            texture.Apply();
        }

        v.Q<VisualElement>("VE_Card").style.backgroundImage = new StyleBackground(texture);
        v.Q<VisualElement>("VE_Card").style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
        v.Q<VisualElement>("VE_Card").style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
        v.Q<VisualElement>("VE_Card").style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
        v.Q<VisualElement>("VE_Card").style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
    }


    private void PointerDownEvent(PointerDownEvent e)
    {
        DragDropManager.Instance.IsDragging = true;
        isDragging = true;
        pointerStartPosition = e.position;
        target = (VisualElement)e.currentTarget;
        targetStartPosition = target.transform.position;
        target.RemoveFromHierarchy();
        AbsoluteContainer.Add(target);
        target.AddToClassList("abs_container");
        //target.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
        //Test.style.position = new StyleEnum<Position>(Position.Absolute); 
        target.CapturePointer(e.pointerId);

        Debug.Log(Dev_Card_Builder.Instance.GetCardFromDeck(target.name).Room.Doors);
    }

    private void PointerMoveEvent(PointerMoveEvent evt)
    {
        if (isDragging && target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            target.transform.position = new Vector2(
                Mathf.Clamp(targetStartPosition.x + evt.position.x, 0, target.panel.visualTree.worldBound.width),
                Mathf.Clamp(targetStartPosition.y + evt.position.y, 0, target.panel.visualTree.worldBound.height));

            if (DragDropManager.Instance.IsOverDropArea())
            {
                if (DragDropManager.Instance.CanDropOnPlaceholder(DragDropManager.Instance.CurrentlyOver.gameObject, target.name))
                {
                    DragDropManager.Instance.CurrentlyOver.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {
                    DragDropManager.Instance.CurrentlyOver.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    private void PointerUpEvent(PointerUpEvent e)
    {
        if (isDragging && target.HasPointerCapture(e.pointerId))
        {
            target.ReleasePointer(e.pointerId);

            if (DragDropManager.Instance.IsOverDropArea() && DragDropManager.Instance.CanDropOnPlaceholder(DragDropManager.Instance.CurrentlyOver.gameObject, target.name))
            {
                _cards.Remove(_cards.Where(x => x.Key == target.name).FirstOrDefault().Key);
                target.parent.Remove(target);
                BuildManager.Instance.RegisterRoom(BuildManager.Instance.GetPlaceholder(DragDropManager.Instance.CurrentlyOver.gameObject), Dev_Card_Builder.Instance.GetCardFromDeck(target.name).Room);
            }
            else
            {
                target.RemoveFromHierarchy();
                target.RemoveFromClassList("abs_container");
                _cards.Where(x => x.Key == target.name).FirstOrDefault().Value.Add(target);
                target.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
                target.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
                target.transform.position = targetStartPosition;
            }
            DragDropManager.Instance.IsDragging = false;
        }
    }


}

