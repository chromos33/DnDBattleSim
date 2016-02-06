using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDBattleSim.Classes.SubClasses;

namespace DnDBattleSim.Classes
{
    class BattleField
    {
        List<Character> Characters;
        List<Point> MapConfig;
        Random randomenerator;
        public BattleField(Random randomizer, int configurement = 0)
        {
            // 0 = simple room 10x10, 1 = 2 wide Pathway,2 = 1 wide Pathway,3 = crossintersection
            randomenerator = randomizer;
            Characters = new List<Character>();
            MapConfig = new List<Point>();
            switch (configurement)
            {
                default:
                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            if (!CheckIfAlreadyExistant(new Point(x, y)))
                            {
                                MapConfig.Add(new Point(x, y));
                            }
                        }
                    }
                    break;
                case 1:
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            if (!CheckIfAlreadyExistant(new Point(x, y)))
                            {
                                MapConfig.Add(new Point(x, y));
                            }
                        }
                    }
                    break;
                case 2:
                    for (int x = 0; x < 1; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            if (!CheckIfAlreadyExistant(new Point(x, y)))
                            {
                                MapConfig.Add(new Point(x, y));
                            }
                        }
                    }
                    break;
                case 3:
                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            if (x == 4 || x == 5)
                            {
                                if (!CheckIfAlreadyExistant(new Point(x, y)))
                                {
                                    MapConfig.Add(new Point(x, y));
                                }
                            }
                            if (y == 4 || y == 5)
                            {
                                if (!CheckIfAlreadyExistant(new Point(x, y)))
                                {
                                    MapConfig.Add(new Point(x, y));
                                }
                            }
                        }
                    }
                    break;
                case 4:
                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            if(x > 5)
                            {
                                if (!CheckIfAlreadyExistant(new Point(x, y)))
                                {
                                    MapConfig.Add(new Point(x, y));
                                }
                            }
                            else
                            {
                                if (y > 2 && x < 3)
                                {
                                    if (!CheckIfAlreadyExistant(new Point(x, y)))
                                    {
                                        MapConfig.Add(new Point(x, y));
                                    }
                                }
                                if (x > 2 && x < 6 && (y < 3 || y > 5))
                                {
                                    if (!CheckIfAlreadyExistant(new Point(x, y)))
                                    {
                                        MapConfig.Add(new Point(x, y));
                                    }
                                }
                            } 
                        }
                    }
                    break;

            }
        }
        public bool CheckIfAlreadyExistant(Point point)
        {
            IEnumerable<Point> movementpoints = MapConfig.Where(x => x.X == point.X && x.Y == point.Y);
            return false;
        }
        public void addCharacter(Character character,int _x = -1, int _y = -1)
        {
            if(_x ==-1 && _y==-1)
            {
                if (Characters.Count > 0)
                {
                    bool positionoccupied = false;
                    Point position = null;
                    while (true)
                    {
                        position = MapConfig.ElementAt(randomenerator.Next(MapConfig.Count));
                        foreach (Character Character in Characters)
                        {
                            if (Character.isOccupied(position))
                            {
                                positionoccupied = true;
                            }
                        }
                        if (!positionoccupied)
                        {
                            break;
                        }
                    }
                    character.setPosition(position);
                    character.setBattlefield(this);
                    Characters.Add(character);
                }
                else
                {
                    character.setPosition(MapConfig.ElementAt(randomenerator.Next(MapConfig.Count)));
                    character.setBattlefield(this);
                    Characters.Add(character);
                }
            }
            else
            {
                Point pos = new Point(_x, _y);
                character.setPosition(pos);
                character.setBattlefield(this);
                Characters.Add(character);
            }
            
        }
        public List<Character> getCharacters()
        {
            return Characters;
        }
        public bool isMovable(Point _point, Point _origin)
        {
            //TODO isDiagonal truly possible (check adjacend fields for existance)
            IEnumerable<Point> mapconfigqueryresult = MapConfig.Where(x => x.X == _point.X && x.Y == _point.Y);
            bool result = false;
            if (mapconfigqueryresult.Count() > 0)
            {
                result = true;
                if (isDiagonal(_point,_origin))
                {
                    IEnumerable<Point> mapconfigquerydiagonalresult = MapConfig.Where(x => x.X == _point.X && x.Y == _origin.Y || x.X == _origin.X && x.Y == _point.Y);
                    if(mapconfigquerydiagonalresult.Count() == 0)
                    {
                        result = false;
                    }
                }
                if(result)
                {
                    IEnumerable<Character> characterqueryresult = Characters.Where(x => x.getPoint().X == _point.X && x.getPoint().Y == _point.Y);
                    if (characterqueryresult.Count() > 0)
                    {
                        result = false;
                    }
                }
                

            }
            return result;
        }
        public bool isDiagonal(Point p1,Point p2)
        {
            if(p1.Distance(p2)>1)
            {
                return true;
            }
            return false;
        }


    }
}
