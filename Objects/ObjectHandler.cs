using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Objects
{
    public static class ObjectHandler
    {

        private static Dictionary<string,Figure> _figures = [];

        public static Dictionary<string, Figure> GetFigures { get => _figures; }

        private static int _objectNameCounter = 0;
        public static int ObjectnameCounter { get => _objectNameCounter++; }

        public static void RemoveAllObjects()
        {
            foreach (Figure fig in _figures.Values)
            {
                RemoveFigureFromScene(fig);
            }
        }
        public static void AddFigureToScene (Figure figure)
        {
            _figures.Add (figure.Name, figure);
        }

        public static void RemoveFigureFromScene (Figure figure)
        {
            _figures.Remove (figure.Name);
        }


        public static void DrawAllFiguresInScene ()
        {
            foreach (Figure figure in _figures.Values)
            {
                
                
                figure.Draw();
            }
        }

    }
}
