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
    public VisualElement CardContainer;
    public VisualElement AbsoluteContainer;


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

    }

    private void OnDisable()
    {
        BuildToggle.clicked -= ToggleBuildMode;
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
        if (_cards.ContainsKey(c.Id))
            return;

        VisualTreeAsset vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BuildPhase/UI/Card.uxml");
        VisualElement vE = vta.Instantiate();
        vE.name = c.Id;
        CardContainer.Add(vE);
        _cards.Add(c.Id, vE);

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
        target.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
        //Test.style.position = new StyleEnum<Position>(Position.Absolute); 
        target.CapturePointer(e.pointerId);
    }

    private void PointerMoveEvent(PointerMoveEvent evt)
    {
        if (isDragging && target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            target.transform.position = new Vector2(
                Mathf.Clamp(targetStartPosition.x + evt.position.x, 0, target.panel.visualTree.worldBound.width),
                Mathf.Clamp(targetStartPosition.y + evt.position.y, 0, target.panel.visualTree.worldBound.height));
        }
    }

    private void PointerUpEvent(PointerUpEvent e)
    {
        if (isDragging && target.HasPointerCapture(e.pointerId))
        {
            target.ReleasePointer(e.pointerId);

            if (DragDropManager.Instance.IsOverDropArea())
            {
                target.parent.Remove(target);
            }
            else
            {
                target.RemoveFromHierarchy();
                target.RemoveFromClassList("abs_container");
                CardContainer.Add(target);

                target.transform.position = targetStartPosition;
            }
            DragDropManager.Instance.IsDragging = false;
        }
    }


}

