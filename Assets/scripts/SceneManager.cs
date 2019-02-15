using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class RoomData
    {
        public enum RoomType
        {
            passible,
            impassible,
        }

        public RoomType type = RoomType.passible;
        public Vector2 smokeDirection = Vector2.right;
        public GameObject prefab;

    }
    public class SceneManager : MonoBehaviour
    {
        // the scene manager runs the game itself. it is in charge of spawning and despawning chambers
        public GameObject[] roomPrefabs;
        public Sprite[] icons;
        GameObject miniMap;
        GameObject currentRoom;
        public GameObject minimapIcon;
        public List<RoomData> Grid;
        public int GridWidth = 8;
        public int GridHeight = 5;
        public int CellX = 3;
        public int CellY = 2;
        public bool CanMoveUp = true;
        public bool CanMoveDown = true;
        public bool CanMoveLeft = true;
        public bool CanMoveRight = true;
        void Start()
        {
            miniMap = transform.GetChild(0).gameObject;
            LaunchMatch();
        }

        void Update()
        {
            //
            if(Input.GetButtonDown("MenuUp")){MoveUp();}
            if(Input.GetButtonDown("MenuDown")){MoveDown();}
            if(Input.GetButtonDown("MenuLeft")){MoveLeft();}
            if(Input.GetButtonDown("MenuRight")){MoveRight();}
        }

        bool IsCellOpen(int x, int y)
        {
            int cellIndex = x + y * GridWidth;
            if(cellIndex<0 || cellIndex > Grid.Count-1){return false;}
            if(Grid[cellIndex].type == RoomData.RoomType.impassible){return false;}
            return true;
        }

        RoomData GetCell(int x, int y)
        {
            int cellIndex = x + y * GridWidth;
            return Grid[cellIndex];
        }

        void ChangeCell(int deltaX, int deltaY)
        {
            if(IsCellOpen(CellX+deltaX,CellY+deltaY) == false){return;}
            CellX+=deltaX;
            CellY+=deltaY;
            if(CellX == -1){CellX = 0;}
            if(CellX == GridWidth){CellX = GridWidth-1;}
            if(CellY == -1){CellY = 0;}
            if(CellY == GridHeight){CellY = GridHeight-1;}
            int cellIndex = CellX + CellY * GridWidth;
            for(int i = 0; i < miniMap.transform.childCount; i++)
            {
                Sprite spr;
                if(Grid[i].type == RoomData.RoomType.impassible){spr = icons[8];}
                else{ spr = icons[0]; }
                miniMap.transform.GetChild(i).GetComponent<Image>().sprite=spr;
            }
            miniMap.transform.GetChild(cellIndex).GetComponent<Image>().sprite=icons[7];
            CanMoveUp = IsCellOpen(CellX,CellY-1);
            CanMoveDown = IsCellOpen(CellX,CellY+1);
            CanMoveLeft = IsCellOpen(CellX-1,CellY);
            CanMoveRight = IsCellOpen(CellX+1,CellY);
            if(currentRoom!=null)
            {
                Destroy(currentRoom);
            }
            currentRoom=Instantiate(GetCell(CellX,CellY).prefab);
        }

        void MoveUp(){ChangeCell(0,-1);}
        void MoveDown(){ChangeCell(0,1);}
        void MoveLeft(){ChangeCell(-1,0);}
        void MoveRight(){ChangeCell(1,0);}

        void LaunchMatch()
        {
            // make sure minimap is right dimensions
            while(miniMap.transform.childCount < GridWidth*GridHeight)
            {
                GameObject icon = Instantiate(minimapIcon);
                icon.transform.SetParent(miniMap.transform);
                icon.transform.localScale = Vector3.one;
            }
            for(int i =  GridWidth * GridHeight; i < miniMap.transform.childCount; i++)
            {
                Destroy(miniMap.transform.GetChild(i));
            }
            

            // load grid
            Grid = new List<RoomData>(GridWidth*GridHeight);
            for(int i = 0; i < GridWidth * GridHeight; i++)
            { 
                //Grid.Add(new RoomData());
                RoomData tempRoom = new RoomData();
                int prefabIndex = Random.Range(0,roomPrefabs.Length);
                tempRoom.prefab=roomPrefabs[prefabIndex];
                Grid.Add(tempRoom);
            }
            Grid[0].type=RoomData.RoomType.impassible;
            Grid[3].type=RoomData.RoomType.impassible;

            ChangeCell(0,0);
        }
    }
}
