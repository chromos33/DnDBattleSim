﻿using System;
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
        public List<Point> MoveTo(Point _goal, bool goAdjacent = true)
        {
            double length = 0;
            List<List<Point>> ForbiddenPaths = new List<List<Point>>();
            List<Point> CurrentPath = new List<Point>();
            List<Point> ForbiddenPoints = new List<Point>();
            List<Point> FinalPath = new List<Point>();
            List<Point> TraveledPath = new List<Point>();
            CurrentPath.Add(position);
            Point TempStartingPoint = position;
            while ((!CurrentPath.Last().isAdjacent(_goal) && goAdjacent) || (!CurrentPath.Last().isOnField(_goal) && !goAdjacent))
            {
                Point currentmovepoint = Move(_goal, CurrentPath, ForbiddenPoints);
                IEnumerable<Point> TraveledContainedPoints1 = TraveledPath.Where(x => x.X == currentmovepoint.X && x.Y == currentmovepoint.Y);
                if (TraveledContainedPoints1.Count() == 0)
                {
                    TraveledPath.Add(currentmovepoint);
                }
                if(currentmovepoint == null)
                {
                    Point ShortestPathPoint = null;
                    foreach(Point point in TraveledPath)
                    {
                        if(ShortestPathPoint == null)
                        {
                            ShortestPathPoint = point;
                        }
                        else
                        {
                            if(ShortestPathPoint.Distance(_goal)>point.Distance(_goal))
                            {
                                ShortestPathPoint = point;
                            }
                        }
                    }
                    _goal = ShortestPathPoint;
                    CurrentPath = new List<Point>();
                    CurrentPath.Add(position);
                }
                else
                {
                    CurrentPath.Add(currentmovepoint);
                }
                
                if((!CurrentPath.Last().isAdjacent(_goal) && goAdjacent) || (!CurrentPath.Last().isOnField(_goal) && !goAdjacent))
                {
                    //if there actually can be a shorter path (more than 2 fields traversed
                    if (CurrentPath.Count() > 2)
                    {
                        List<Point> TempPath = new List<Point>();

                        TempPath.Add(TempStartingPoint);
                        Point tempgoalpoint = CurrentPath.Last();
                        // TODO: find out what to do when 1 Point in TempPath returns null
                        while (TempPath.Last().X != tempgoalpoint.X || TempPath.Last().Y != tempgoalpoint.Y)
                        {
                            Point tempmovepoint = Move(tempgoalpoint, TempPath);
                            TempPath.Add(tempmovepoint);
                            IEnumerable<Point> TraveledContainedPoints2 = TraveledPath.Where(x => x.X == tempmovepoint.X && x.Y == tempmovepoint.Y);
                            if (TraveledContainedPoints2.Count() == 0)
                            {
                                TraveledPath.Add(tempmovepoint);
                            }
                        }
                        if (isPathShorter(TempPath, CurrentPath))
                        {
                            foreach (Point cpoint in CurrentPath)
                            {
                                foreach (Point tpoint in TempPath)
                                {
                                    if (cpoint.isOnField(CurrentPath.First()) || cpoint.isOnField(CurrentPath.Last()))
                                    {
                                        // start end end point must stay allowed
                                    }
                                    else
                                    {
                                        if (NonOverLappingPoints(CurrentPath, TempPath).Count() > 0)
                                        {
                                            IEnumerable<Point> containedpoint = ForbiddenPoints.Where(x => x.X == cpoint.X && x.Y == cpoint.Y);
                                            if (containedpoint.Count() == 0)
                                            {
                                                ForbiddenPoints.Add(cpoint);
                                            }
                                        }
                                    }
                                }
                            }
                            if (CurrentPath.First().isOnField(TempPath.First()))
                            {
                                CurrentPath = TempPath;
                            }
                            else
                            {
                                CurrentPath = MergePaths(CurrentPath, TempPath);
                                Console.WriteLine("tst");
                            }


                        }
                        else
                        {
                            if (PathLength(TempPath) == PathLength(CurrentPath))
                            {
                                TempStartingPoint = TempPath.Last();
                            }
                        }
                    }
                }
                
            }
            return CurrentPath;
        }
        private Point Move(Point _goal, List<Point> CurrentPath, List<Point> ForbiddenPoints = null)
        {
            List<Point> possibleMovementPoints = GenerateMovementPoints(_goal, CurrentPath, ForbiddenPoints);


            if (possibleMovementPoints.Count() > 0)
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

        private bool isMoveable(List<Point> forbidden, Point targetPoint, Point _origin)
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
        private bool hasMoveableAdjacent(List<Point> forbidden, Point targetPoint, Point _origin)
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
            for (int i = 0; i + 1 < path.Count(); i++)
            {
                length += path[i].Distance(path[i + 1], true);
            }
            return length;
        }
        private bool ComparePaths(List<Point> basepath, List<Point> comparepath)
        {
            // returns if Paths are the same
            if (basepath.Count() != comparepath.Count())
            {
                return false;
            }
            for (int i = 0; i < basepath.Count(); i++)
            {
                if (basepath[i].isOnField(comparepath[i]))
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
                            if (CurrentPath.Count() >= 3)
                            {
                                if (!movepoint.isOnField(CurrentPath[CurrentPath.Count() - 2]))
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
                    else
                    {
                        possibleMovementPoints.Add(movepoint);
                    }

                }
            }
            return possibleMovementPoints;
        }
        // dont know if i ever gonna need this again.
        private List<Point> ReplaceSubPath(List<Point> ReplacementPath, List<Point> FullPath)
        {
            List<Point> ReturnPath = new List<Point>();

            int startreplacing = 0;
            int stopreplacing = 0;
            int i = 0;
            while (i < FullPath.Count())
            {
                if (ReplacementPath.First().isOnField(FullPath[i]))
                {
                    startreplacing = i;
                }
                if (ReplacementPath.Last().isOnField(FullPath[i]))
                {
                    stopreplacing = i;
                }
                i++;
            }
            for (int j = 0; j < FullPath.Count; j++)
            {
                if (j == startreplacing)
                {
                    ReturnPath.AddRange(ReplacementPath);
                }
                if (j >= startreplacing && j <= stopreplacing)
                {

                }
                else
                {
                    ReturnPath.Add(FullPath[j]);
                }
            }

            return ReturnPath;
        }
        public List<Point> NonOverLappingPoints(List<Point> CurrentPath,List<Point> TempPath)
        {
            IEnumerable<Point> nonoverlappingpoints = CurrentPath.Where(x => TempPath.All(a => !a.isOnField(x)));
            return nonoverlappingpoints.ToList();
        }
        public bool isPathShorter(List<Point> ShorterPath, List<Point> LongerPath)
        {
            // ShorterPath is not really the Shorter path it's just the path that is checked if it is shorter
            //does this 2 different ways if startpoints of both are the same and if they're not comparing length of where Temppath starts in CurrentPath
            if(SameStartingPoint(ShorterPath, LongerPath))
            {
                if(PathLength(ShorterPath) < PathLength(LongerPath))
                {
                    return true;
                }
            }
            else
            {
                List<Point> SubPath = new List<Point>();
                int substartingpoint = 0;
                for(int i = 0;i< LongerPath.Count();i++)
                {
                    if(LongerPath[i].isOnField(ShorterPath.First()))
                    {
                        substartingpoint = i;
                        break;
                    }
                    SubPath.Add(LongerPath[i]);
                }
                if (PathLength(ShorterPath) < PathLength(SubPath))
                {
                    return true;
                }

            }
            return false;
        }
        public bool SameStartingPoint(List<Point> ShorterPath, List<Point> LongerPath)
        {
            if(ShorterPath[0].isOnField(LongerPath[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Point> MergePaths(List<Point> FullPath,List<Point> TempPath)
        {
            List<Point> ReturnPaths = new List<Point>();
            List<Point> SubPath = new List<Point>();
            int substartingpoint = 0;
            bool substartingpointfound = false;
            for (int i = 0; i < FullPath.Count(); i++)
            {
                if (FullPath[i].isOnField(TempPath.First()))
                {
                    substartingpoint = i;
                    substartingpointfound = true;
                }
                if(i>=substartingpoint && substartingpointfound)
                {
                    SubPath.Add(FullPath[i]);
                }
                
            }
            if(NonOverLappingPoints(SubPath, TempPath).Count()==0)
            {
                return FullPath;
            }
            else
            {
                int x = 0;
                int startreplacing = 0;
                while (x < FullPath.Count())
                {
                    if (TempPath.First().isOnField(FullPath[x]))
                    {
                        startreplacing = x;
                    }
                    x++;
                }
                for (int j = 0; j < FullPath.Count(); j++)
                {
                    if (j == x)
                    {
                        break;
                    }
                    else
                    {
                        ReturnPaths.Add(FullPath[j]);
                    }
                }
                ReturnPaths.AddRange(TempPath);
            }
            return ReturnPaths;
        }
    }
}
