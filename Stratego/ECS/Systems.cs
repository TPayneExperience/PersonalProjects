using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Systems {
    namespace Display {
        public static class Board {
            static Board() { }
            public static void UpdateBoardImages() {
                ClearBoard();
                int currentPlayer = Gameplay.Board.GetCurrentPlayer();
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                foreach (int id in Component.ComponentManager.Instance.Get("Position")) {
                    Entity.PieceEntity pe = (Entity.PieceEntity) Entity.EntityManager.Instance.Get(id);
                    Component.Utility.Player pc = (Component.Utility.Player)pe.components["Player"];
                    Component.Utility.Position pos = (Component.Utility.Position)pe.components["Position"];
                    Component.Piece.Value value = (Component.Piece.Value)pe.components["Value"];
                    Component.Piece.Revealed r = (Component.Piece.Revealed)pe.components["Revealed"];
                    
                    if (currentPlayer == pc.player || r.revealed)
                        SetImage(PosToTag(pos.x, pos.y), pc.player, value.value);
                    else
                        SetImage(PosToTag(pos.x, pos.y), pc.player, 0);
                }
            }
            public static void ClearBoard() {
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                Component.Board.Obstacles o = (Component.Board.Obstacles)Entity.BoardEntity.Instance.components["Obstacles"];
                for (int i = 1; i <= 100; i++) SetImage(i);
                foreach (int i in o.obsticles) SetImage(i, 0, 1);
            }
            public static void SetImage(int tag, int playerNum = 0, int pieceValue = 0) {
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                Component.Display.Images i = (Component.Display.Images)Entity.DisplayEntity.Instance.components["Images"];
                bc.buttons[tag].Background = i.images[playerNum][pieceValue];
            }
            public static void SetButtonMessage(string message) {
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                bc.buttons[0].Content = message;
            }
            
            public static void SetButtonLock(int tag, bool isLocked) {
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                bc.buttons[tag].IsEnabled = !isLocked;
            }
            public static void SetAllButtonLocks(bool isLocked) {
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                foreach (int tag in bc.buttons.Keys)
                    SetButtonLock(tag, isLocked);
            } 
            public static void SetLockListBox(string listbox, bool isLocked) {
                Component.Display.ListBoxes lbc = (Component.Display.ListBoxes)Entity.DisplayEntity.Instance.components["ListBoxes"];
                lbc.listBoxes[listbox].IsEnabled = !isLocked;
            }
            
            public static int[] TagToPos(int tag) {
                return new int[] { tag % 10, (int)(tag / 10) };
            }
            public static int PosToTag(int x, int y) {
                return (x + y * 10);
            }
        }
        public static class Graveyard {
            static Graveyard() { }
            public static void UpdateGraveyards() {
                Component.Display.ListBoxes l = (Component.Display.ListBoxes)Entity.DisplayEntity.Instance.components["ListBoxes"];
                l.listBoxes["Graveyard1"].Items.Clear();
                l.listBoxes["Graveyard2"].Items.Clear();

                foreach (var piece in GetDeadPieces()) {
                    Component.Piece.Value vc = (Component.Piece.Value)piece.components["Value"];
                    Component.Utility.Player player = (Component.Utility.Player)piece.components["Player"];
                    ListBoxItem item = CreateGraveyardItem(player.player, vc.value);
                    l.listBoxes["Graveyard" + player.player.ToString()].Items.Insert(0, item);
                }
            }
            private static List<Entity.PieceEntity> GetDeadPieces() {
                List<Entity.PieceEntity> pieces = new List<Entity.PieceEntity>();
                foreach(int id in Component.ComponentManager.Instance.Get("Dead")) 
                    pieces.Add((Entity.PieceEntity)Entity.EntityManager.Instance.Get(id));
                return pieces;
            }
            public static ListBoxItem CreateGraveyardItem(int player, int value) {
                ListBoxItem i = new ListBoxItem();
                i.Content = ValueToPieceName(value);
                i.Background = GetColor(player);
                i.Foreground = System.Windows.Media.Brushes.White;
                i.Tag = value;
                return i;
            }
            public static System.Windows.Media.Brush GetColor(int playerNum) {
                if (playerNum == 1)
                    return System.Windows.Media.Brushes.Blue;
                else if (playerNum == 2)
                    return System.Windows.Media.Brushes.DarkRed;
                return System.Windows.Media.Brushes.DarkSlateGray;
            }
            public static string ValueToPieceName(int value) {
                switch(value) {
                    case 1:
                        return "01_Spy";
                    case 2:
                        return "02_Scout";
                    case 3:
                        return "03_Miner";
                    case 4:
                        return "04_Sergent";
                    case 5:
                        return "05_Lieutenant";
                    case 6:
                        return "06_Captain";
                    case 7:
                        return "07_Major";
                    case 8:
                        return "08_Colonel";
                    case 9:
                        return "09_General";
                    case 10:
                        return "10_Marshall";
                    case 11:
                        return "B_Bomb";
                    case 12:
                        return "F_Flag";
                    case 13:
                        return "Obstacal";
                    default:
                        return "";
                }
            }
            public static void SetGroupBoxTitle(string groupbox, string desc) {
                Component.Display.GroupBoxes gbc = (Component.Display.GroupBoxes)Entity.DisplayEntity.Instance.components["GroupBoxes"];
                gbc.groupBoxes[groupbox].Header = desc;
            }
        }
        public static class History {
            static History() { }
            public static void UpdateHistory() {
                Component.Display.ListBoxes lbc = (Component.Display.ListBoxes) Entity.DisplayEntity.Instance.components["ListBoxes"];
                lbc.listBoxes["History"].Items.Clear();

                foreach (int id in Component.ComponentManager.Instance.Get("Winner")) {
                    Entity.ActionEntity action = (Entity.ActionEntity)Entity.EntityManager.Instance.Get(id);
                    Component.Action.Winner w = (Component.Action.Winner)action.components["Winner"];
                    Component.Action.Loser l = (Component.Action.Loser)action.components["Loser"];
                    lbc.listBoxes["History"].Items.Insert(0, CreateHistoryItem(w.player, w.value, l.value));
                }
            }
            private static ListBoxItem CreateHistoryItem(int player, int winValue, int loseValue) {
                ListBoxItem i = new ListBoxItem();
                i.Content = Graveyard.ValueToPieceName(winValue) + " defeated " + Graveyard.ValueToPieceName(loseValue);
                i.Background = Graveyard.GetColor(player);
                i.Foreground = System.Windows.Media.Brushes.White;
                return i;
            }
        }
    }
    namespace GameSetup {
        public static class Board {
            static Board() { }
            private static Entity.PieceEntity CreateBoardPiece(int playerNum, int value, int x, int y) {
                Entity.PieceEntity p;
                if (value <= 10)
                    p = new Entity.MoveablePieceEntity(playerNum, value, x, y);
                else
                    p = new Entity.PieceEntity(playerNum, value, x, y);
                return p;
            }
            private static void SwapPositions(int tag1, int tag2) {
                int[] pos1 = Display.Board.TagToPos(tag1);
                int[] pos2 = Display.Board.TagToPos(tag2);
                List<Entity.PieceEntity> pieceList1 = Gameplay.Board.GetPiecesAtButton(tag1);
                List<Entity.PieceEntity> pieceList2 = Gameplay.Board.GetPiecesAtButton(tag2);
                foreach (var piece in pieceList1) Gameplay.Board.MovePiece(piece, pos2[0], pos2[1]);
                foreach (var piece in pieceList2) Gameplay.Board.MovePiece(piece, pos1[0], pos1[1]);
            }

            public static void LockInvalidPlacements() {
                int currentPlayer = Gameplay.Board.GetCurrentPlayer();
                Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                foreach (int tag in bc.buttons.Keys) {
                    if (tag > 60)
                        Display.Board.SetButtonLock(tag, currentPlayer == 2);
                    else if (tag > 40)
                        Display.Board.SetButtonLock(tag, true);
                    else if (tag > 0) {
                        Display.Board.SetButtonLock(tag, currentPlayer == 1);
                    }
                }
            }
            public static void SetPiece(int tag) {
                Component.Display.ListBoxes lbc = (Component.Display.ListBoxes)Entity.DisplayEntity.Instance.components["ListBoxes"];
                Component.Utility.Player pc = (Component.Utility.Player)Entity.BoardEntity.Instance.components["Player"];
                ListBox graveyard = lbc.listBoxes["Graveyard" + pc.player.ToString()];

                if (graveyard.SelectedItem == null) {
                    if (Gameplay.Board.GetStartingTag() == -1)
                        Gameplay.Board.SetStartingTag(tag);
                    else {
                        SwapPositions(tag, Gameplay.Board.GetStartingTag());
                        Gameplay.Board.SetStartingTag(-1);
                    }
                } else if (Gameplay.Board.GetPiecesAtButton(tag).Count == 0) {
                    ListBoxItem item = (ListBoxItem)graveyard.SelectedItem;
                    int[] pos = Display.Board.TagToPos(tag);
                    CreateBoardPiece(pc.player, Int32.Parse(item.Tag.ToString()), pos[0], pos[1]);
                    graveyard.Items.Remove(item);
                    graveyard.SelectedItem = null;
                }
            }
            public static bool IsGraveyardEmpty() {
                Component.Display.ListBoxes lbc = (Component.Display.ListBoxes)Entity.DisplayEntity.Instance.components["ListBoxes"];
                return !lbc.listBoxes["Graveyard" + Gameplay.Board.GetCurrentPlayer().ToString()].HasItems;
            }
        }
        public static class Graveyard {
            static Graveyard() { }
            public static void PopulateGraveyard() {
                Component.Utility.Player pc = (Component.Utility.Player)Entity.BoardEntity.Instance.components["Player"];
                Component.Board.AllPieceValues apvc = (Component.Board.AllPieceValues)Entity.BoardEntity.Instance.components["AllPieceValues"];
                Component.Display.ListBoxes lbc = (Component.Display.ListBoxes)Entity.DisplayEntity.Instance.components["ListBoxes"];
                foreach (var value in apvc.values) {
                    ListBoxItem i = Display.Graveyard.CreateGraveyardItem(pc.player, value);
                    lbc.listBoxes["Graveyard" + pc.player.ToString()].Items.Insert(0, i);
                }
            }
        }
    }
    namespace Gameplay {
        public static class Board {
            static Board() { }
            public static List<Entity.PieceEntity> GetPiecesAtButton(int tag) {
                List<Entity.PieceEntity> pieces = new List<Entity.PieceEntity>();
                int[] buttonPos = Display.Board.TagToPos(tag);
                foreach (int id in Component.ComponentManager.Instance.Get("Position")) {
                    Entity.PieceEntity pe = (Entity.PieceEntity)Entity.EntityManager.Instance.Get(id);
                    Component.Utility.Position entityPos = (Component.Utility.Position)pe.components["Position"];

                    if (buttonPos[0] == entityPos.x && buttonPos[1] == entityPos.y)
                        pieces.Add(pe);
                }
                return pieces;
            }
            public static void SetStartingTag(int tag) {
                Component.Board.StartingTag stc = (Component.Board.StartingTag)Entity.BoardEntity.Instance.components["StartingTag"];
                stc.startingTag = tag;
            }
            public static int GetStartingTag() {
                Component.Board.StartingTag stc = (Component.Board.StartingTag)Entity.BoardEntity.Instance.components["StartingTag"];
                return stc.startingTag;
            }

            public static bool IsOutOfMoveablePieces() {
                List<Entity.PieceEntity> pieces = new List<Entity.PieceEntity>();
                foreach (int id in Component.ComponentManager.Instance.Get("Range")) {
                    Entity.PieceEntity pe = (Entity.PieceEntity)Entity.EntityManager.Instance.Get(id);
                    Component.Utility.Player pc = (Component.Utility.Player)pe.components["Player"];
                    if (pc.player == GetCurrentPlayer())
                        pieces.Add(pe);
                }
                return pieces.Count == 0;
            }
            public static void Update(int tag) {
                int startingTag = GetStartingTag();
                if (startingTag == -1) {
                    SetStartingTag(tag);
                    LockInvalidMoves();
                } else if (startingTag == tag) {
                    Display.Board.SetAllButtonLocks(true);
                    UnlockValidSelections();
                    SetStartingTag(-1);
                } else {
                    int[] pos = Display.Board.TagToPos(tag);
                    Entity.PieceEntity mover = GetPiecesAtButton(startingTag)[0];
                    List<Entity.PieceEntity> possibleTargets = GetPiecesAtButton(tag);
                    MovePiece(mover, pos[0], pos[1]);
                    foreach (var target in possibleTargets)
                        EvalCollision(mover, target);
                    Display.History.UpdateHistory();
                    Display.Graveyard.UpdateGraveyards();
                    SwitchPlayers();
                    SetStartingTag(-1);
                }
            }
            public static int IsGameOver() {
                if (Component.ComponentManager.Instance.Get("Range").Count == 0)
                    return -1;
                foreach (int id in Component.ComponentManager.Instance.Get("Dead")){
                    Component.Piece.Value lc = (Component.Piece.Value) Entity.EntityManager.Instance.Get(id).components["Value"];
                    if (lc.value == 12) {
                        SwitchPlayers();
                        return GetCurrentPlayer();
                    }
                }
                return 0;
            }
            public static void EvalCollision(Entity.PieceEntity mover, Entity.PieceEntity target) {
                Component.Board.CollisionTable c = (Component.Board.CollisionTable)Entity.BoardEntity.Instance.components["CollisionTable"];
                Component.Piece.Value vc0 = (Component.Piece.Value)mover.components["Value"];
                Component.Piece.Value vc1 = (Component.Piece.Value)target.components["Value"];
                Component.Utility.Player p0 = (Component.Utility.Player)mover.components["Player"];
                Component.Utility.Player p1 = (Component.Utility.Player)target.components["Player"];
                    
                if (c.table[vc0.value][vc1.value] == 1) {
                    RevealPiece(mover);
                    History.CreateAction(p0.player, vc0.value, p1.player, vc1.value);
                    SetPieceToDead(target);
                } else if (c.table[vc0.value][vc1.value] == 0) {
                    RevealPiece(target);
                    History.CreateAction(p1.player, vc1.value, p0.player, vc0.value);
                    SetPieceToDead(mover);
                } else {
                    History.CreateAction(p0.player, vc0.value, p1.player, vc1.value);
                    History.CreateAction(p1.player, vc1.value, p0.player, vc0.value);
                    SetPieceToDead(target);
                    SetPieceToDead(mover);
                }
                
            }
            public static int GetCurrentPlayer() {
                Component.Utility.Player pc = (Component.Utility.Player) Entity.BoardEntity.Instance.components["Player"];
                return pc.player;
            }
            private static void RevealPiece(Entity.PieceEntity piece) {
                Component.Piece.Revealed rc = (Component.Piece.Revealed)piece.components["Revealed"];
                rc.revealed = true;
            }

            public static void UnlockValidSelections() {
                foreach (int id in Component.ComponentManager.Instance.Get("Range")) {
                    Entity.PieceEntity p = (Entity.PieceEntity) Entity.EntityManager.Instance.Get(id);
                    if (GetValidMoves(p).Count > 0) {
                        Component.Utility.Player player = (Component.Utility.Player)p.components["Player"];
                        Component.Utility.Position pc = (Component.Utility.Position)p.components["Position"];
                        Display.Board.SetButtonLock(Display.Board.PosToTag(pc.x, pc.y), player.player != GetCurrentPlayer());
                    }
                }
            }
            public static void LockInvalidMoves() {
                List<Entity.PieceEntity> pieces = GetPiecesAtButton(GetStartingTag());
                Display.Board.SetAllButtonLocks(true);
                Display.Board.SetButtonLock(GetStartingTag(), false);
                if (pieces.Count > 0) {
                    Component.Display.Buttons bc = (Component.Display.Buttons)Entity.DisplayEntity.Instance.components["Buttons"];
                    foreach (int tag in GetValidMoves(pieces[0]))
                        Display.Board.SetButtonLock(tag, false);
                } 
            }
            private static List<int> GetValidMoves(Entity.PieceEntity piece) {
                Component.Board.Obstacles o = (Component.Board.Obstacles)Entity.BoardEntity.Instance.components["Obstacles"];
                List<int> validPositions = new List<int>();
                int currentPlayer = GetCurrentPlayer();
                foreach (List<int> direction in GetAllPossibleMoves(piece)) {
                    foreach(int tag in direction) {
                        if (o.obsticles.Contains(tag))
                            break;
                        List<Entity.PieceEntity> pieces = GetPiecesAtButton(tag);
                        if (pieces.Count == 0)
                            validPositions.Add(tag);
                        else {
                            Component.Utility.Player pc = (Component.Utility.Player)pieces[0].components["Player"];
                            if (pc.player != currentPlayer)
                                validPositions.Add(tag);
                            break;
                        }
                    }
                }
                return validPositions;
            }
            private static List<List<int>> GetAllPossibleMoves(Entity.PieceEntity piece) {
                List<List<int>> positions = new List<List<int>>();

                Component.Piece.Range rc = (Component.Piece.Range)piece.components["Range"];
                Component.Utility.Position pc = (Component.Utility.Position)piece.components["Position"];
                
                for (int i = 0; i < 4; i++) positions.Add(new List<int>());
                for (int i = 1; i <= rc.range; i++) {
                    if (pc.x + i <= 10)
                        positions[0].Add(Display.Board.PosToTag(pc.x + i, pc.y));
                    if (pc.x - i >= 1)
                        positions[1].Add(Display.Board.PosToTag(pc.x - i, pc.y));
                    if (pc.y + i <= 9)
                        positions[2].Add(Display.Board.PosToTag(pc.x, pc.y + i));
                    if (pc.y - i >= 0)
                        positions[3].Add(Display.Board.PosToTag(pc.x, pc.y - i));
                }
                return positions;
            }
            public static void SetPieceToDead(Entity.PieceEntity p) {
                if (p.components.ContainsKey("Range")) {
                    p.Remove("Range");
                }
                p.Remove("Position");
                p.Remove("Alive");
                p.Remove("Revealed");
                p.Add(new Component.Piece.Dead(p.Id));
            }

            public static Entity.PieceEntity MovePiece(Entity.PieceEntity p, int x, int y) {
                Component.Utility.Position pc = (Component.Utility.Position)p.components["Position"];
                pc.x = x;
                pc.y = y;
                return p;
            }
            public static void SwitchPlayers() {
                Component.Utility.Player pc = (Component.Utility.Player)Entity.BoardEntity.Instance.components["Player"];
                pc.player = pc.player == 2? 1: 2;
            }
        }
        public static class Graveyard {
            static Graveyard() { }
            public static void TitleGroupBox(GroupBox gb, int player) {
                gb.Header = "GRAVEYARD";
            }
        }
        public static class History {
            static History() { }
            public static void CreateAction(int winPlayer, int winValue, int losePlayer, int loseValue) {
                new Entity.ActionEntity(winPlayer, winValue, losePlayer, loseValue);
            }
        }
    }
    namespace GameOver {
        public static class Board {
            static Board() { }
        }
        public static class Graveyard {
            static Graveyard() { }
        }
        public static class History {
            static History() { }
        }
    }
}
