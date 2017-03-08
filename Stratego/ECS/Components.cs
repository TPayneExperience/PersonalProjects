using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Component {
    namespace Utility {
        public class Position : ComponentAbs {
            public int x, y;
            public Position(int id, int posX, int posY) : base("Position", id) {
                x = posX;
                y = posY;
            }
        }
        public class Player : ComponentAbs {
            public int player;
            public Player(int id, int playerNum) : base("Player", id) { player = playerNum; }
        }
    }
    namespace Piece {
        public class Value : ComponentAbs {
            public int value;
            public Value(int id, int val) : base("Value", id) { value = val; }
        }
        public class Revealed : ComponentAbs {
            public bool revealed = false;
            public Revealed(int id) : base("Revealed", id) { }
        }
        public class Range : ComponentAbs {
            public int range;
            public Range(int id, int hasRange) : base("Range", id) { range = hasRange; }
        }
        public class Dead : ComponentAbs {
            public Dead(int id) : base("Dead", id) { }
        }
    }
    namespace Action {
        public class Winner : ComponentAbs {
            public int player, value;
            public Winner(int id, int playerNum, int val) : base("Winner", id) {
                player = playerNum;
                value = val;
            }
        }
        public class Loser : ComponentAbs {
            public int player, value;
            public Loser(int id, int playerNum, int val) : base("Loser", id) {
                player = playerNum;
                value = val;
            }
        }
    }
    namespace Board {
        public class Obstacles : ComponentAbs {
            public int[] obsticles = { 43, 44, 47, 48, 53, 54, 57, 58 };
            public Obstacles(int id) : base("Obstacles", id) { }
        }
        public class AllPieceValues : ComponentAbs {
            public List<int> values = new List<int>();
            public AllPieceValues(int id) : base("AllPieceValues", id) {
                int[] vals = {
                    // 2,3,10,11,12 // For Quick Demo
                    1,
                    2, 2, 2, 2, 2, 2, 2, 2,
                    3, 3, 3, 3, 3,
                    4, 4, 4, 4,
                    5, 5, 5, 5,
                    6, 6, 6, 6,
                    7, 7, 7,
                    8, 8,
                    9,
                    10,
                    11, 11, 11, 11, 11, 11,
                    12
                };
                foreach(var val in vals) {
                    values.Add(val);
                }
            }
        }

        public class GameState : ComponentAbs {
            public GameStates.GameState currentGameState;
            public Dictionary<string, GameStates.GameState> gameStates = new Dictionary<string, GameStates.GameState>();
            public GameState(int id) : base("GameState", id) {
                gameStates.Add("GameStart", new GameStates.GameStart(id));
                gameStates.Add("GameSetup", new GameStates.GameSetup(id));
                gameStates.Add("GameplayPause", new GameStates.GameplayPause(id));
                gameStates.Add("GameplayUnpause", new GameStates.GameplayUnpause(id));
                gameStates.Add("GameOver", new GameStates.GameOver(id));
                currentGameState = gameStates["GameStart"];
            }
        }
        public class StartingTag : ComponentAbs {
            public int startingTag = -1;
            public StartingTag(int id) : base("StartingTag", id) { }
        }
        public class CollisionTable : ComponentAbs {
            public Dictionary<int, Dictionary<int, int>> table;
            public CollisionTable(int id) : base("CollisionTable", id) {
                table = new Dictionary<int, Dictionary<int, int>>();
                for(int i = 1; i < 13; i++) table.Add(i, new Dictionary<int, int>());
                for (int i = 1; i < 13; i++) {
                    for (int j = 1; j < 12; j++) {
                        int z = i > j ? 1 : 0;
                        z = i == j ? 2 : z;
                        table[i][j] = z;
                    }
                }
                table[1][10] = 1;    // spy kills marshall
                table[3][11] = 1;    // miner kills bomb
                for (int i = 1; i < 13; i++)
                    table[i][12] = 1; // all kill flag
            }
        }
    }
    namespace Display {
        public class Buttons : ComponentAbs {
            public Dictionary<int, Button> buttons = new Dictionary<int, Button>();
            public Buttons(int id) : base("Buttons", id) { }
        }
        public class ListBoxes : ComponentAbs {
            public Dictionary<string, ListBox> listBoxes = new Dictionary<string, ListBox>();
            public ListBoxes(int id) : base("ListBoxes", id) { }
        }
        public class GroupBoxes : ComponentAbs {
            public Dictionary<string, GroupBox> groupBoxes = new Dictionary<string, GroupBox>();
            public GroupBoxes(int id) : base("GroupBoxes", id) { }
        }
        public class Images : ComponentAbs {
            public Dictionary<int, Dictionary<int, ImageBrush>> images;
            public Images(int id) : base("Images", id) {
                images = new Dictionary<int, Dictionary<int, ImageBrush>>();
                images.Add(0, new Dictionary<int, ImageBrush>());
                images.Add(1, new Dictionary<int, ImageBrush>());
                images.Add(2, new Dictionary<int, ImageBrush>());
                
                BitmapImage temp1 = new BitmapImage();
                temp1.BeginInit();
                temp1.UriSource = new Uri(@"../../Images/00_Null_Grey.png", UriKind.Relative);
                temp1.EndInit();
                BitmapImage temp2 = new BitmapImage();
                temp2.BeginInit();
                temp2.UriSource = new Uri(@"../../Images/13_Obstacle.png", UriKind.Relative);
                temp2.EndInit();

                images[0].Add(0, new ImageBrush(temp1));
                images[0].Add(1, new ImageBrush(temp2));

                string[] imageNames = {
                    "00_Null_Blue",
                    "01_Spy_Blue",
                    "02_Scout_Blue",
                    "03_Miner_Blue",
                    "04_Sergent_Blue",
                    "05_Lieutenant_Blue",
                    "06_Captain_Blue",
                    "07_Major_Blue",
                    "08_Colonel_Blue",
                    "09_General_Blue",
                    "10_Marshall_Blue",
                    "11_Bomb_Blue",
                    "12_Flag_Blue",
                    "00_Null_Red",
                    "01_Spy_Red",
                    "02_Scout_Red",
                    "03_Miner_Red",
                    "04_Sergent_Red",
                    "05_Lieutenant_Red",
                    "06_Captain_Red",
                    "07_Major_Red",
                    "08_Colonel_Red",
                    "09_General_Red",
                    "10_Marshall_Red",
                    "11_Bomb_Red",
                    "12_Flag_Red"
                };
                for (int i = 1; i < 3; i++) {
                    for (int j = 0; j < 13; j++) {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri(@"../../Images/" + imageNames[(i-1)*13 + (j)] +".png", UriKind.Relative);
                        bi.EndInit();
                        images[i].Add(j, new ImageBrush(bi));
                    }
                }
            }
        }
    }
}
