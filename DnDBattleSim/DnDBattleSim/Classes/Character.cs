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
            //NoPath Detection Missing TODO: Think of a way to detect this
            //Full Path Solution save every step and dissallow all steps that have been taken
            //if no more fields can be tread on then take the position with the lowest distance and the steps taken to get there as the new position (recalculate distance)
            // Bonus program a way to read all steps and remove the steps that have the higher distance between 2 points like if char moved from
            // (0,0) -> (0,1) ->
            //TODO:when character Class is fleshed out with at least movement don't move further than that distance
            
            double length = 0;
            start.Text = position.X + " / " + position.Y;
            goal.Text = _goal.X + " / " + _goal.Y;
            Tuple<Point, double> goalpoint = new Tuple<Point,double>(new Point(-10000, -10000),0);
            List<Point> TestedPath = new List<Point>();
            List<Point> FinalMovePath = new List<Point>();
            Point currentPosition = position;
            bool NoPathPossible = false;
            while (!NoPathPossible && !_goal.isAdjacent(goalpoint.Item1))
            {
                goalpoint = Move(_goal, currentPosition, TestedPath);
                if(goalpoint.Item1 != null)
                {
                    TestedPath.Add(goalpoint.Item1);
                    length += goalpoint.Item2;
                    currentPosition = goalpoint.Item1;
                }
                else
                {
                    List<Point> revertedList = TestedPath;
                    revertedList.Reverse();
                    Point temptarget = null;
                    foreach(Point lastposition in revertedList)
                    {
                        if(hasMoveableAdjacent(TestedPath,lastposition, currentPosition))
                        {
                            temptarget = lastposition;
                            break;
                        }
                    }
                    if(temptarget != null)
                    {

                    }
                    else
                    {
                        Point newMoveToPoint = hasShortestDistance(TestedPath,_goal);
                        NoPathPossible = true;
                        currentPosition = position;
                        steps.Items.Clear();
                        while (!(newMoveToPoint.X == currentPosition.X && newMoveToPoint.Y == currentPosition.Y))
                        {
                            goalpoint = Move(_goal, currentPosition, FinalMovePath);
                            length += goalpoint.Item2;
                            currentPosition = goalpoint.Item1;
                        }
                    }
                }
                
            }
            string steptest = position.X+"/"+position.Y;
            foreach(Point point in FinalMovePath)
            {
                steptest += "->"+ point.X + "/" + point.Y;
                steps.Items.Add(steptest);
                steptest = point.X + "/" + point.Y;
            }
            return new Tuple<Point,double>(goalpoint.Item1, length);
        }
        private Tuple<Point, double> Move(Point _goal, Point currentPos, List<Point> oldpoints = null)
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
                if (battlefield.isMovable(movepoint,currentPos))
                {
                    if (oldpoints != null)
                    {
                        IEnumerable<Point> movementpoints = oldpoints.Where(x => x.X == movepoint.X && x.Y == movepoint.Y);
                        if (movementpoints.Count() >0)
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
            if(possibleMovementPoints.Count()>0)
            {
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
                length = Math.Round(currentPos.Distance(nextmove)*2,MidpointRounding.AwayFromZero)/2;
                return new Tuple<Point, double>(nextmove, length);
            }
            else
            {
                return new Tuple<Point, double>(null,0);
            }
            
        }

        private bool isMoveable(List<Point> forbidden,Point targetPoint,Point _origin)
        {
            if (battlefield.isMovable(targetPoint, _origin))
            {
                if (forbidden != null)
                {
                    IEnumerable<Point> movementpoints = forbidden.Where(x => x.X == targetPoint.X && x.Y == targetPoint.Y);
                    if (movementpoints.Count() > 0)
                    {
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }
            return false;
        }
        private bool hasMoveableAdjacent(List<Point> forbidden, Point targetPoint,Point _origin)
        {
            List<Point> MovementPoints = new List<Point>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MovementPoints.Add(new Point(targetPoint.X - i, targetPoint.Y - j));
                }
            }
            foreach (Point movepoint in MovementPoints)
            {
                if (battlefield.isMovable(movepoint, _origin))
                {
                    if (forbidden != null)
                    {
                        IEnumerable<Point> movementpoints = forbidden.Where(x => x.X == movepoint.X && x.Y == movepoint.Y);
                        if (movementpoints.Count() > 0)
                        {

                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                }
            }

            return false;
        }
        private Point hasShortestDistance(List<Point> _Points,Point _goal)
        {
            return _Points.Aggregate((c, d) => c.Distance(_goal) < d.Distance(_goal) ? c : d);
        }
        private List<Point> GenerateShortestPath(List<Point> longpath)
        {
            List<Point> resultPath = new List<Point>();


            return resultPath;
        }
    }
}
