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
        public Point MoveTo(Point _goal,bool goAdjacent = true)
        {
            
            double length = 0;
            Point goalpoint = new Point(-10000, -10000);
            List<List<Point>> ForbiddenPaths = new List<List<Point>>();
            List<Point> CurrentPath = new List<Point>();
            CurrentPath.Add(position);
            while((goalpoint.isAdjacent(_goal) && goAdjacent) || (goalpoint.isOnField(_goal) && !goAdjacent))
            {
                goalpoint = Move(_goal, CurrentPath, ForbiddenPaths);
                CurrentPath.Add(goalpoint);
                //if there actually can be a shorter path (more than 2 fields traversed
                if(CurrentPath.Count() > 2)
                {
                    List<Point> TempPath = new List<Point>();
                    TempPath.Add(position);
                    Point tempgoalpoint = CurrentPath.Last();
                    while(TempPath.Last().X != tempgoalpoint.X && TempPath.Last().Y != tempgoalpoint.Y)
                    {
                        TempPath.Add(Move(tempgoalpoint, TempPath));
                    }
                    if(PathLength(TempPath)<PathLength(CurrentPath))
                    {
                        ForbiddenPaths.Add(CurrentPath);
                        CurrentPath = TempPath;
                    }
                }
            }


                                    
            return goalpoint;
        }
        private Point Move(Point _goal, List<Point> CurrentPath, List<List<Point>> ForbiddenPaths = null)
        {
            List<Point> MovementPoints = new List<Point>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MovementPoints.Add(new Point(CurrentPath.Last().X - i, CurrentPath.Last().Y - j));
                }
            }
            List<Point> possibleMovementPoints = new List<Point>();
            foreach (Point movepoint in MovementPoints)
            {
                if (battlefield.isMovable(movepoint, CurrentPath.Last()))
                {
                    if(ForbiddenPaths.Count() > 0)
                    {
                        List<List<Point>> ApplicableRules = new List<List<Point>>();
                        foreach(List<Point> ForbiddenPath in ForbiddenPaths)
                        {
                            if(ForbiddenPath.Count() >= CurrentPath.Count())
                            {
                                ApplicableRules.Add(ForbiddenPath);
                            }
                        }
                        foreach(List<Point> Rule in ApplicableRules)
                        {
                            for (int i = 0; i < CurrentPath.Count(); i++)
                            {
                                if (CurrentPath[i].X != Rule[i].X && CurrentPath[i].Y != Rule[i].Y)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        possibleMovementPoints.Add(movepoint);
                    }

                    /*
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
                    }*/

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
                return nextmove;
            }
            else
            {
                return null;
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
        private double PathLength(List<Point> path)
        {
            double length = 0;
            for (int i = 0; i+1 < path.Count(); i++)
            {
                length += path[i].Distance(path[i + 1],true);
            }
            return length;
        }
    }
}
