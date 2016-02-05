using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DnDBattleSim.Classes.SubClasses;
using System.Windows.Forms;

namespace DnDBattleSim.Classes
{
    class Character
    {
        Point position;
        BattleField battlefield;
        public string Name;
        public Character()
        {

        }
        public void setBattlefield(BattleField _battlefield)
        {
            battlefield = _battlefield;
        }
        public void setPosition(Point _position)
        {
            position = _position;
        }
        public bool isOccupied(Point _position)
        {
            bool isoccupied = false;
            if (position.X == _position.X && position.Y == _position.Y)
            {
                isoccupied = true;
            }
            return isoccupied;
        }
        public Point getPoint()
        {
            return position;
        }
        public Tuple<Point,double> MoveTo(Point _goal,TextBox start,TextBox goal,ListBox steps)
        {
            double length = 0;
            start.Text = position.X + " / " + position.Y;
            goal.Text = _goal.X + " / " + _goal.Y;
            Tuple<Point, double> goalpoint = new Tuple<Point,double>(new Point(-10000, -10000),0);
            Point currentPosition = position;
            while (!_goal.isAdjacent(goalpoint.Item1))
            {
                string step = currentPosition.X + " / " + currentPosition.Y + " -> ";
                goalpoint = Move(_goal, currentPosition, goalpoint.Item1);
                step += goalpoint.Item1.X + " / " + goalpoint.Item1.Y;
                steps.Items.Add(step);
                length += goalpoint.Item2;
                currentPosition = goalpoint.Item1;
            }
            return new Tuple<Point,double>(goalpoint.Item1, length);
        }
        private Tuple<Point, double> Move(Point _goal, Point currentPos, Point oldpoint = null)
        {
            double length = 0;
            List<Point> MovementPoints = new List<Point>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MovementPoints.Add(new Point(currentPos.X - i, currentPos.Y - j));
                }
            }
            List<Point> possibleMovementPoints = new List<Point>();
            foreach (Point movepoint in MovementPoints)
            {
                if (battlefield.isMovable(movepoint))
                {
                    if (oldpoint != null)
                    {
                        if (oldpoint.X == movepoint.X && oldpoint.Y == movepoint.Y)
                        {

                        }
                        else
                        {
                            possibleMovementPoints.Add(movepoint);
                        }
                    }
                    else
                    {
                        possibleMovementPoints.Add(movepoint);
                    }

                }
            }
            Point nextmove = null;
            foreach (Point movepoint in possibleMovementPoints)
            {
                if (nextmove != null)
                {
                    if (movepoint.Distance(_goal) < nextmove.Distance(_goal))
                    {
                        nextmove = movepoint;
                    }
                }
                else
                {
                    nextmove = movepoint;
                }
            }
            length = currentPos.Distance(nextmove);
            return new Tuple<Point, double>(nextmove, length);
        }

    }
}
