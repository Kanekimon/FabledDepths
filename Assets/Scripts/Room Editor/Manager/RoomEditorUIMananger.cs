using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace Assets.Scripts.Room_Editor.Manager
{
    public class RoomEditorUIMananger : Singleton<RoomEditorUIMananger>
    {
        Color orignalTabColor;
        Color selectedTabColor;

        public Placeable ActiveTile;

        public VisualElement root;
        public VisualElement AbsoluteContainer;

        public VisualElement Tile_Tab;
        public VisualElement Tile_Container;

        public VisualElement Obstacle_Tab;
        public VisualElement Obstacle_Container;

        public VisualElement Active_Tab;

        public Dictionary<VisualElement, Placeable> Tile_Items = new Dictionary<VisualElement, Placeable>();
        public Dictionary<VisualElement, Placeable> Obstacle_Items = new Dictionary<VisualElement, Placeable>();

        private void Start()
        {
            root = this.GetComponent<UIDocument>().rootVisualElement;

            Tile_Tab = root.Q<VisualElement>("Tile_Tab");
            Obstacle_Tab = root.Q<VisualElement>("Obstacle_Tab");

            Tile_Container = root.Q<VisualElement>("Tile_Container");
            Obstacle_Container = root.Q<VisualElement>("Obstacle_Container");

            orignalTabColor = new Color32(135, 140, 143,255);
            selectedTabColor = new Color32(252, 247, 255,255);

            Active_Tab = Tile_Tab;
            Tile_Tab.style.backgroundColor = selectedTabColor;
            Obstacle_Tab.style.backgroundColor = orignalTabColor;

            Tile_Tab.AddManipulator(new Clickable(() => ClickTab("Tile")));
            Obstacle_Tab.AddManipulator(new Clickable(() => ClickTab("Obstacle")));


            LoadPlaceables();
        }


        public void ClickTab(string tab)
        {
            if (!Active_Tab.name.Contains(tab))
            {
                if (tab.Equals("Tile"))
                {
                    Active_Tab = Tile_Container;
                    Obstacle_Container.style.display = DisplayStyle.None;
                    Tile_Container.style.display = DisplayStyle.Flex;
                    Tile_Tab.style.backgroundColor = selectedTabColor;
                    Obstacle_Tab.style.backgroundColor = orignalTabColor;
                }
                else
                {
                    Active_Tab = Obstacle_Container;
                    Tile_Container.style.display = DisplayStyle.None;
                    Obstacle_Container.style.display = DisplayStyle.Flex;
                    Tile_Tab.style.backgroundColor = orignalTabColor;
                    Obstacle_Tab.style.backgroundColor = selectedTabColor;
                }
            }
        }

        void LoadPlaceables()
        {
            foreach(Placeable pl in RoomEditorManager.Instance.Placeables)
            {
                if(pl != null)
                {
                    if(pl.PlaceableType == PlaceableType.Tile)
                    {
                        AddTileItem(pl, Tile_Container);
                    }
                    else
                    {
                        AddTileItem(pl, Obstacle_Container);

                    }
                }
            }
            VisualElement first = Active_Tab.Children().Where(x => x.name.Contains("[")).FirstOrDefault();
            SetActiveTile(Tile_Items.First().Key, Tile_Items.Values.First());
        }

        public void AddTileItem(Placeable pl, VisualElement container)
        {
            VisualElement slot = container.Children().Where(x => !x.name.Contains("[")).FirstOrDefault();
            Tile_Items[slot] = pl;
            slot.name += $"[{pl.PlaceablePrefab.name}]";
            slot.style.backgroundImage = pl.PlaceablePrefab.GetComponent<SpriteRenderer>().sprite.texture;

            slot.AddManipulator(new Clickable(() => {
                SetActiveTile(slot, pl);
                }));

        }

        void SetActiveTile(VisualElement slot, Placeable pl)
        {
            VisualElement old = Tile_Items.Where(x => x.Value == ActiveTile).FirstOrDefault().Key;
            RemoveBorder(old);
            ActiveTile = pl;
            AddBorder(slot);
        }

        void RemoveBorder(VisualElement o)
        {
            if (o == null)
                return; 

            o.style.borderTopWidth = 0;
            o.style.borderBottomWidth = 0;
            o.style.borderLeftWidth = 0;
            o.style.borderRightWidth = 0;
        }

        void AddBorder(VisualElement o)
        {
            o.style.borderTopWidth = 2;
            o.style.borderBottomWidth = 2;
            o.style.borderLeftWidth = 2;
            o.style.borderRightWidth = 2;

            o.style.borderBottomColor = Color.black;
            o.style.borderTopColor = Color.black;
            o.style.borderLeftColor = Color.black;
            o.style.borderRightColor = Color.black;

            o.style.borderBottomLeftRadius = 5;
            o.style.borderBottomRightRadius = 5;
            o.style.borderTopLeftRadius = 5;
            o.style.borderTopRightRadius = 5;
        }
    }
}
