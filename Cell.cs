﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ProjektZtp
{
    public class Cell
    {
        public Position Position { get; }
        public bool IsHit { get; set; }
        public Button Button { get; set; }
        public Ship Ship { get; set; }

        private ICellState state;

        public void UpdateState()
        {
            // Determines the state of the cell based on whether it's hit and whether it contains a ship.
            if (IsHit)
            {
                if (Ship != null)
                {
                    state = Ship.isSunk() ? (ICellState)new SunkState() : new HitState();
                }
                else
                {
                    state = new MissedState();
                }
            }
            else
            {
                state = new NormalState();
            }

            UpdateAppearance();
        }

        public void UpdateAppearance()
        {
            state.UpdateAppearance(this);
        }

        public void MarkAsHit()
        {
            IsHit = true;
            UpdateState();
        }

        // Initializes a cell with a default state and associates it with a UI button for rendering.
        public Cell(Position position)
        {
            Position = position;
            IsHit = false;
            Button = new Button();
            state = new NormalState();
            UpdateState();
        }
    }

    public interface ICellState
    {
        void UpdateAppearance(Cell cell);
    }

    public class NormalState : ICellState
    {
        public void UpdateAppearance(Cell cell)
        {
            cell.Button.BackColor = cell.Ship != null ? Color.Green : Color.LightBlue;
            cell.Button.Text = string.Empty;
        }
    }

    public class HitState : ICellState
    {
        public void UpdateAppearance(Cell cell)
        {
            cell.Button.BackColor = Color.Red;
            cell.Button.Text = string.Empty;
        }
    }

    public class MissedState : ICellState
    {
        public void UpdateAppearance(Cell cell)
        {
            cell.Button.BackColor = Color.Black;
            cell.Button.Text = string.Empty;
        }
    }

    public class SunkState : ICellState
    {
        public void UpdateAppearance(Cell cell)
        {
            if (cell.Ship != null)
            {
                // Updates all cells of the sunk ship to clearly represent its destroyed state.
                foreach (var c in cell.Ship.Cells)
                {
                    c.Button.BackColor = Color.Red;
                    c.Button.Text = "X";
                    c.Button.Font = new Font(c.Button.Font.FontFamily, 14, FontStyle.Bold);
                }
            }
        }
    }

    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        // Represents the coordinates of a cell on the board.
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
