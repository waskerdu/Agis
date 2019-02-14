using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class SceneManager : MonoBehaviour
    {
        // the scene manager runs the game itself. it is in charge of spawning and despawning chambers
        public GameObject[] roomPrefabs;
        public Sprite[] icons;
        GameObject miniMap;
        public GameObject minimapIcon;
        public int GridWidth = 8;
        public int GridHeight = 5;
        public int CellX = 3;
        public int CellY = 2;
        void Start()
        {
            miniMap = transform.GetChild(0).gameObject;
            LaunchMatch();
        }

        void Update()
        {
            if(Input.GetButtonDown("Jump")){MoveLeft();}
            if(Input.GetButtonDown("Fire")){MoveRight();}
        }

        void ChangeCell(int deltaX, int deltaY)
        {
            CellX+=deltaX;
            CellY+=deltaY;
            if(CellX == -1){CellX = 0;}
            if(CellX == GridWidth){CellX = GridWidth-1;}
            if(CellY == -1){CellY = 0;}
            if(CellY == GridHeight){CellY = GridHeight-1;}
            int cellIndex = CellX + CellY * GridWidth;
            for(int i = 0; i < miniMap.transform.childCount; i++)
            {
                miniMap.transform.GetChild(i).GetComponent<Image>().sprite=icons[0];
            }
            miniMap.transform.GetChild(cellIndex).GetComponent<Image>().sprite=icons[7];
        }

        void MoveUp(){ChangeCell(0,1);}
        void MoveDown(){ChangeCell(0,-1);}
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
            ChangeCell(0,0);
        }
    }
}
