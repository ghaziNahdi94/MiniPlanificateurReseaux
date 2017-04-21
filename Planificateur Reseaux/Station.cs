


using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace Planificateur_Reseaux
{
    [System.Serializable()]
    class Station
    {

        public string name { get; set; }

        [System.NonSerialized]
        public SizeablePictureBox image;


        public List<ConnectionAccesPointStation> connections = new List<ConnectionAccesPointStation>();
        public bool dragMode { get; set; }
        public Point position { get; set; }

        [System.NonSerialized]
        public Form dialogStat = null;

        public Station() { }

        public Station(string name, SizeablePictureBox image) {

            this.name = name;
            this.image = image;
            this.position = new Point(image.Location.X,image.Location.Y);
            dragMode = false;


        }


        public void drawConnection(Graphics g)
        {

            foreach (ConnectionAccesPointStation cnx in connections)
            {
                if (maxQuality(connections) == cnx.getQualityConnection())
                {
                    g.DrawLine(new Pen(Color.Black, 3), cnx.accesPoint.centre, new Point(this.image.Location.X + this.image.Width / 2, this.image.Location.Y + this.image.Height / 2));
                    break;
                }
            }


        }



        public float maxQuality(List<ConnectionAccesPointStation> liste)
        {
            float max = liste.ElementAt(0).getQualityConnection();

            foreach (ConnectionAccesPointStation cnx in liste)
            {
                if (max < cnx.getQualityConnection())
                    max = cnx.getQualityConnection();
            }


            return max;
        }



    }
}
