using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZtp
{
    public abstract class Player
    {
        public string Username { get; set; }
        public Fleet PlayerFleet { get; private set; }
        public Board PlayerBoard { get; private set; }
        public Invoker Invoker { get; private set; }

        // Teraz konstruktor Player nie wymaga argumentu username
        public Player(string username = "AI")  // Domyślna nazwa to "AI"
        {
            Username = username;  // Można ustawić domyślną nazwę lub przypisać inną w konstruktorze
            Invoker = new Invoker();    // Inicjalizowanie Invokera
        }

        public void SetPlayerFleet(Fleet fleet)
        {
            PlayerFleet = fleet;
        }

        public void SetPlayerBoard(int boardSize)
        {
            PlayerBoard = new Board(boardSize);
        }

        public void ResetBoard()
        {
            // Zakładając, że masz metodę Clear w Board, która resetuje stan planszy.
        }

        public Board GetBoard()
        {
            return PlayerBoard;
        }

        public abstract Position MakeShot();  // Do zaimplementowania w klasach dziedziczących
        public abstract bool PlaceShips();    // Do zaimplementowania w klasach dziedziczących
        public abstract bool AddShipToFleet(Ship ship);  // Do zaimplementowania w klasach dziedziczących
    }

    public class PlayerHuman : Player
    {
        public PlayerHuman(string username) : base(username)
        {
            // Dodatkowa logika dla gracza człowieka, jeśli jest potrzebna.
        }

        public override bool AddShipToFleet(Ship ship)
        {
            // Implementacja logiki dodawania statku do floty dla gracza człowieka
            // np. sprawdzanie, czy statek nie koliduje z innymi itp.
            return true;
        }

        public override Position MakeShot()
        {
            // Implementacja wykonywania strzału przez gracza człowieka
            // Np. otrzymanie pozycji od użytkownika.
            return new Position(0, 0);  // Przyklad zwrócenia pozycji
        }

        public override bool PlaceShips()
        {
            // Implementacja logiki ustawiania statków przez gracza człowieka
            // Np. przy pomocy interfejsu użytkownika.
            return true;
        }
    }

    public class PlayerAi : Player
    {
        // Teraz konstruktor PlayerAi nie wymaga parametru username
        public PlayerAi() : base("AI")  // Domyślnie ustawiamy nazwę na "AI"
        {
            // Dodatkowa logika dla gracza AI, jeśli jest potrzebna.
        }

        public override bool AddShipToFleet(Ship ship)
        {
            // Implementacja logiki dodawania statku do floty dla gracza AI
            // AI może wybierać losowo miejsca na statki
            return true;
        }

        public override Position MakeShot()
        {
            // Implementacja wykonywania strzału przez AI
            // AI może losowo wybierać pozycję, czy też stosować jakąś strategię
            return new Position(1, 1);  // Przykład losowej pozycji
        }

        public override bool PlaceShips()
        {
            // AI może ustawiać statki w sposób losowy lub zgodnie z jakąś strategią
            return true;
        }
    }
}
