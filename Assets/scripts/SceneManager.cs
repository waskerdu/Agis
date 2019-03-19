using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelDataSpace;
using System.IO;

namespace GameScene
{
    
    public class SceneManager : MonoBehaviour
    {
        // the scene manager runs the game itself. it is in charge of spawning and despawning chambers
        public GameObject[] roomPrefabs;
        public GameObject player;
        public int numPlayers = 2;
        public GameObject killPlane;
        public Sprite[] icons;
        public GameObject miniMap;
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
        List<GameObject> doors;
        List<GameObject> players;
        string dataPath;
        void Start()
        {
            dataPath = Path.Combine(Application.persistentDataPath, "LevelData.txt");
            //Debug.Log(dataPath);
            doors = new List<GameObject>();
            players = new List<GameObject>();
            for(int i = 0; i<4;i++)
            {
                GameObject currentPlayer = Instantiate(player);
                currentPlayer.SetActive(false);
                players.Add(currentPlayer);
            }
            LaunchMatch();
        }

        void Update()
        {
            //
            /* *if(Input.GetButtonDown("MenuUp")){MoveUp();}
            if(Input.GetButtonDown("MenuDown")){MoveDown();}
            if(Input.GetButtonDown("MenuLeft")){MoveLeft();}
            if(Input.GetButtonDown("MenuRight")){MoveRight();}/* */
        }

        void SaveLevel()
        {
            LevelData levelData = new LevelData();
            levelData.cells = Grid;
            string jsonString = JsonUtility.ToJson (levelData);

            using (StreamWriter streamWriter = File.CreateText (dataPath))
            {
                streamWriter.Write (jsonString);
            }
        }

        void LoadLevel()
        {
            using (StreamReader streamReader = File.OpenText (dataPath))
            {
                string jsonString = streamReader.ReadToEnd ();
                LevelData data = JsonUtility.FromJson<LevelData> (jsonString);
                Grid = data.cells;
            }
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
            killPlane.transform.position = transform.position;
            killPlane.GetComponent<Rigidbody2D>().velocity = Grid[cellIndex].smokeDirection;
            killPlane.transform.GetChild(0).gameObject.SetActive(false);
            killPlane.transform.GetChild(1).gameObject.SetActive(false);
            if(Grid[cellIndex].smokeDirection.x > 0){killPlane.transform.GetChild(0).gameObject.SetActive(true);}
            else if(Grid[cellIndex].smokeDirection.x < 0){killPlane.transform.GetChild(1).gameObject.SetActive(true);}
            for(int i = 0; i < Grid.Count; i++)
            {
                Sprite spr = icons[0];
                //if(Grid[i].type == RoomData.RoomType.impassible){spr = icons[7];}
                //else if(Grid[i].type == RoomData.RoomType.leftWin){spr.getc}
                if(Grid[i].smokeDirection.x > 0) {spr = icons[4];}
                else if(Grid[i].smokeDirection.x < 0) {spr = icons[1];}
                miniMap.transform.GetChild(i).GetComponent<Image>().sprite=spr;
                Color currentColor = Color.white;
                if(Grid[i].type == RoomData.RoomType.impassible){currentColor = Color.black;}
                else if(Grid[i].type == RoomData.RoomType.leftWin){currentColor = LevelDataSpace.Colors.colors[0];}
                else if(Grid[i].type == RoomData.RoomType.rightWin){currentColor = LevelDataSpace.Colors.colors[1];}
                else if(Grid[i].type == RoomData.RoomType.upWin){currentColor = LevelDataSpace.Colors.colors[2];}
                else if(Grid[i].type == RoomData.RoomType.downWin){currentColor = LevelDataSpace.Colors.colors[3];}
                miniMap.transform.GetChild(i).GetComponent<Image>().color = currentColor;
            }
            //miniMap.transform.GetChild(cellIndex).GetComponent<Image>().sprite=icons[8];
            miniMap.transform.GetChild(cellIndex).GetComponent<Image>().color = Color.yellow;
            CanMoveUp = IsCellOpen(CellX,CellY-1);
            CanMoveDown = IsCellOpen(CellX,CellY+1);
            CanMoveLeft = IsCellOpen(CellX-1,CellY);
            CanMoveRight = IsCellOpen(CellX+1,CellY);
            if(currentRoom!=null)
            {
                Destroy(currentRoom);
            }
            currentRoom=Instantiate(roomPrefabs[GetCell(CellX,CellY).prefabIndex]);
            currentRoom.transform.SetParent(transform);
            doors.Clear();
            foreach (Transform child in currentRoom.GetComponentsInChildren<Transform>())
            {
                if(child.gameObject.tag == "Respawn"){doors.Add(child.gameObject);}
            }

            for(int i = 0; i<numPlayers; i++)
            {
                GameObject currentPlayer = players[i];
                currentPlayer.SetActive(true);
                currentPlayer.transform.SetParent(gameObject.transform);
                currentPlayer.transform.position = doors[i].transform.position - Vector3.forward;
                doors[i].SetActive(false);
            }
        }

        void MoveUp(){ChangeCell(0,-1);}
        void MoveDown(){ChangeCell(0,1);}
        void MoveLeft(){ChangeCell(-1,0);}
        void MoveRight(){ChangeCell(1,0);}

        void LaunchMatch()
        {
            // assign default controllers
            string[] hitMessages = {"MoveLeft","MoveRight","MoveUp","MoveDown"};
            if (numPlayers != 0)
            {
                for( int i = 0; i < numPlayers; i++)
                {
                    players[i].SetActive(true);
                    players[i].SendMessage("SetPlayerNum",i);
                    players[i].SendMessage("SetInputs",true);
                    players[i].SendMessage("SetHitMessage",hitMessages[i]);
                    players[i].SetActive(false);
                }
            }
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
            //LoadLevel();
            miniMap.GetComponent<GridLayoutGroup>().constraintCount = GridWidth;
            Grid = new List<RoomData>(GridWidth*GridHeight);
            for(int i = 0; i < GridWidth * GridHeight; i++)
            {
                RoomData tempRoom = new RoomData();
                tempRoom.prefabIndex = Random.Range(0,roomPrefabs.Length);
                Grid.Add(tempRoom);
            }
            Grid[0 + GridWidth * 2].type=RoomData.RoomType.leftWin;
            Grid[0 + GridWidth * 1].type=RoomData.RoomType.impassible;
            Grid[0 + GridWidth * 3].type=RoomData.RoomType.impassible;
            Grid[-1 + GridWidth * 3].type=RoomData.RoomType.rightWin;
            Grid[-1 + GridWidth * 2].type=RoomData.RoomType.impassible;
            Grid[-1 + GridWidth * 4].type=RoomData.RoomType.impassible;
            Grid[-3 + GridWidth * 3].smokeDirection = Vector3.right;
            Grid[3].type=RoomData.RoomType.upWin;
            Grid[2].type=RoomData.RoomType.impassible;
            Grid[4].type=RoomData.RoomType.impassible;
            Grid[3 + GridWidth * 4].type=RoomData.RoomType.downWin;
            Grid[2 + GridWidth * 4].type=RoomData.RoomType.impassible;
            Grid[4 + GridWidth * 4].type=RoomData.RoomType.impassible;/* */
            ChangeCell(0,0);
            SaveLevel();
        }
    }
}
