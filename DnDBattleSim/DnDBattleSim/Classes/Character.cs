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
        public List<Point> MoveTo(Point _goal,bool goAdjacent = true)
        {
            
            double length = 0;
            Point goalpoint = new Point(-10000, -10000);
            List<List<Point>> ForbiddenPaths = new List<List<Point>>();
            List<Point> CurrentPath = new List<Point>();
            List<Point> ForbiddenPoints = new List<Point>();
            CurrentPath.Add(position);
            while((!goalpoint.isAdjacent(_goal) && goAdjacent) || (!goalpoint.isOnField(_goal) && !goAdjacent))
            {
                goalpoint = Move(_goal, CurrentPath, ForbiddenPoints);
                CurrentPath.Add(goalpoint);
                //if there actually can be a shorter path (more than 2 fields traversed
                if(CurrentPath.Count() > 2)
                {
                    List<Point> TempPath = new List<Point>();
                    
                    TempPath.Add(position);
                    List<Point> TempforbiddenList = new List<Point>();
                    TempforbiddenList.AddRange(ForbiddenPoints);
                    //TODO: prevent Points to be added to tempforbiddenList that are already in it.
                    for(int i = 1;i< CurrentPath.Count();i++)
                    {
                        //Don't need to filter First Point since we start the loop at the second
                        if(i != CurrentPath.Count()-1)
                        {
                            IEnumerable<Point> containedpoint = TempforbiddenList.Where(x => x.X == CurrentPath[i].X && x.Y == CurrentPath[i].Y);
                            if (containedpoint.Count() == 0)
                            {
                                TempforbiddenList.Add(CurrentPath[i]);
                            }
                        }
                    }
                    Point tempgoalpoint = CurrentPath.Last();
                    // TODO: find out what to do when 1 Point in TempPath returns null
                    while(TempPath.Last().X != tempgoalpoint.X || TempPath.Last().Y != tempgoalpoint.Y)
                    {
                        TempPath.Add(Move(tempgoalpoint, TempPath, TempforbiddenList));
                    }
                    if(PathLength(TempPath)<PathLength(CurrentPath))
                    {
                        foreach(Point cpoint in CurrentPath)
                        {
                            foreach(Point tpoint in TempPath)
                            {
                                if((tpoint.isOnField(TempPath.First()) || tpoint.isOnField(TempPath.Last()) || cpoint.isOnField(CurrentPath.First()) || cpoint.isOnField(CurrentPath.Last())) && TempPath.Count() > 2)
                                {
                                    // start end end point must stay allowed
                                }
                                else
                                {
                                    List<Point> TempCheckList = new List<Point>();
                                    TempCheckList.AddRange(CurrentPath);
                                    if(TempCheckList.Count > 2)
                                    {
                                        TempCheckList.Remove(CurrentPath.Last());
                                        TempCheckList.Remove(CurrentPath.First());
                                    }
                                    IEnumerable<Point> overlapping_points = TempCheckList.Where(x => x.X == tpoint.X && x.Y == tpoint.Y);
                                    if (overlapping_points.Count() == 0 || ForbiddenPoints.Count() == 0)
                                    {
                                        IEnumerable<Point> containedpoint = ForbiddenPoints.Where(x => x.X == cpoint.X && x.Y == cpoint.Y);
                                        if(containedpoint.Count() == 0)
                                        {
                                            ForbiddenPoints.Add(cpoint);
                                        }
                                        
                                    }
                                }
                            }
                        }
                        CurrentPath = TempPath;
                    }
                }
            }                     
            return CurrentPath;
        }
        private Point Move(Point _goal, List<Point> CurrentPath,List<Point> ForbiddenPoints = null)
        {
            List<Point> possibleMovementPoints = GenerateMovementPoints(_goal, CurrentPath, ForbiddenPoints);

                        
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
        private bool ComparePaths(List<Point> basepath, List<Point> comparepath)
        {
            // returns if Paths are the same
            if(basepath.Count() != comparepath.Count())
            {
                return false;
            }
            for(int i = 0;i<basepath.Count();i++)
            {
                if(basepath[i].isOnField(comparepath[i]))
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private List<Point> GenerateMovementPoints(Point _goal, List<Point> CurrentPath, List<Point> ForbiddenPoints = null)
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
                    if (ForbiddenPoints != null)
                    {
                        if (isMoveable(ForbiddenPoints, movepoint, CurrentPath.Last()))
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
            return possibleMovementPoints;
        }
    }
}
