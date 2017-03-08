using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStates {
    public class GameState {
        protected int _id;
        protected GameState(int id) { _id = id; }
        virtual public void Enter() { }
        virtual public void Execute(int tag) { }
        virtual public void Exit() { }
    }

    public class GameStart : GameState {
        public GameStart(int id) : base(id) { }
        override public void Enter() {
            Systems.Display.Board.UpdateBoardImages();
            Systems.Display.Board.SetAllButtonLocks(true);
            Systems.Display.Board.SetButtonLock(0, false);
            Systems.Display.Board.SetButtonMessage("Start Game!");
        }
        override public void Execute(int tag) {
            Exit();
        }
        override public void Exit() {
            Component.Board.GameState g = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            g.currentGameState = g.gameStates["GameSetup"];
            g.currentGameState.Enter();
        }
    }
    public class GameSetup : GameState {
        public GameSetup(int id) : base(id) { }
        override public void Enter() {
            Systems.GameSetup.Graveyard.PopulateGraveyard();
            Systems.Display.Board.UpdateBoardImages();
            Systems.GameSetup.Board.LockInvalidPlacements();
            Systems.Display.Board.SetButtonLock(0, true);
            Systems.Display.Board.SetButtonMessage("Set your board...");
            Systems.Display.Graveyard.SetGroupBoxTitle("Graveyard", "Your Pieces");
        }
        override public void Execute(int tag) {
            if (tag != 0) {
                Systems.GameSetup.Board.SetPiece(tag);
                Systems.Display.Board.UpdateBoardImages();
                Systems.GameSetup.Board.LockInvalidPlacements();
                if (Systems.GameSetup.Board.IsGraveyardEmpty()) {
                    Systems.Display.Board.SetButtonLock(0, false);
                    Systems.Display.Board.SetButtonMessage("Setup Complete!");
                }
            } else
                Exit();
        }
        override public void Exit() {
            if (Systems.Gameplay.Board.GetCurrentPlayer() == 1) {
                Systems.Gameplay.Board.SwitchPlayers();
                Enter();
            } else {
                Systems.Gameplay.Board.SwitchPlayers();
                Systems.Display.Graveyard.SetGroupBoxTitle("Graveyard", "Graveyard");
                Component.Board.GameState g = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
                g.currentGameState = g.gameStates["GameplayPause"];
                g.currentGameState.Enter();
            }
        }
    }
    public class GameplayPause : GameState {
        public GameplayPause(int id) : base(id) { }
        override public void Enter() {
            Systems.Display.Board.ClearBoard();
            Systems.Display.Board.SetAllButtonLocks(true);
            Systems.Display.Board.SetButtonLock(0, false);
            Systems.Display.Board.SetButtonMessage("Ready Player " + Systems.Gameplay.Board.GetCurrentPlayer().ToString());
        }
        override public void Execute(int id) {
            Exit();
        }
        override public void Exit() {
            Component.Board.GameState g = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            g.currentGameState = g.gameStates["GameplayUnpause"];
            g.currentGameState.Enter();
        }
    }
    public class GameplayUnpause : GameState {
        public GameplayUnpause(int id) : base(id) { }
        override public void Enter() {
            Systems.Display.Board.UpdateBoardImages();
            Systems.Display.Board.SetButtonMessage("Inside Gameplay");
            Systems.Display.Board.SetButtonLock(0, true);
            Systems.Display.Board.SetAllButtonLocks(true);
            Systems.Gameplay.Board.UnlockValidSelections();
            Systems.Gameplay.Board.SetStartingTag(-1);
        }
        override public void Execute(int tag) {
            int currentPlayer = Systems.Gameplay.Board.GetCurrentPlayer();
            Systems.Gameplay.Board.Update(tag);
            if (currentPlayer != Systems.Gameplay.Board.GetCurrentPlayer())
                Exit();
        }
        override public void Exit() {
            Component.Board.GameState gc = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            if (Systems.Gameplay.Board.IsGameOver() != 0)
                gc.currentGameState = gc.gameStates["GameOver"];
            else {
                gc.currentGameState = gc.gameStates["GameplayPause"];
                if (Systems.Gameplay.Board.IsOutOfMoveablePieces())
                    Systems.Gameplay.Board.SwitchPlayers();
            }
            gc.currentGameState.Enter();
        }
    }
    public class GameOver : GameState {
        public GameOver(int id) : base(id) { }
        override public void Enter() {
            Systems.Gameplay.Board.SwitchPlayers();
            Systems.Display.Board.UpdateBoardImages();
            Systems.Display.Board.SetAllButtonLocks(true);
            Systems.Display.Board.SetButtonLock(0, false);
            Systems.Display.Board.SetButtonMessage("New Game");
            if (Systems.Gameplay.Board.IsGameOver() == -1)
                System.Windows.MessageBox.Show("Draw");
            else
                System.Windows.MessageBox.Show("Player " + Systems.Gameplay.Board.GetCurrentPlayer().ToString() + " Wins!");
           }
        override public void Execute(int tag) {
            Exit();
        }
        override public void Exit() {
            // MISSING: Clear out all old data for game
            Component.Board.GameState g = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            g.currentGameState = g.gameStates["GameStart"];
            g.currentGameState.Enter();
        }
    }
    

}
