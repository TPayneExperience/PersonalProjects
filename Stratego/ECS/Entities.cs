
namespace Entity {
    public class MoveablePieceEntity : PieceEntity {
        public MoveablePieceEntity(int player, int value, int posX, int posY) : 
                                                    base(player, value, posX, posY) {
            int range = value == 2 ? 10 : 1; // SCOUT HAS 10 RANGE, OTHERS 1
            Add(new Component.Piece.Range(Id, range));
        }
    }
    public class PieceEntity : EntityAbs {
        public PieceEntity(int player, int value, int posX, int posY) {
            Add(new Component.Utility.Position(Id, posX, posY));
            Add(new Component.Utility.Player(Id, player));

            Add(new Component.Piece.Value(Id, value));
            Add(new Component.Piece.Revealed(Id));
        }
    }
    public class ActionEntity : EntityAbs {
        public ActionEntity(int winPlayer, int winVal, int losePlayer, int loseVal) {
            Add(new Component.Action.Winner(Id, winPlayer, winVal));
            Add(new Component.Action.Loser(Id, losePlayer, loseVal));
        }
    }
    public class BoardEntity : EntityAbs {
        static BoardEntity _instance;
        public static BoardEntity Instance {
            get {
                if (_instance == null) {
                    _instance = new BoardEntity();
                }
                return _instance;
            }
        }
        private BoardEntity() {
            Add(new Component.Utility.Player(Id, 1));

            Add(new Component.Board.StartingTag(Id));
            Add(new Component.Board.GameState(Id));
            Add(new Component.Board.Obstacles(Id));
            Add(new Component.Board.CollisionTable(Id));
            Add(new Component.Board.AllPieceValues(Id));
        }
    }
    public class DisplayEntity : EntityAbs {
        static DisplayEntity _instance;
        public static DisplayEntity Instance {
            get {
                if (_instance == null) {
                    _instance = new DisplayEntity();
                }
                return _instance;
            }
        }
        private DisplayEntity() {
            Add(new Component.Display.Images(Id));
            Add(new Component.Display.Buttons(Id));
            Add(new Component.Display.ListBoxes(Id));
            Add(new Component.Display.GroupBoxes(Id));
        }
    }
}
