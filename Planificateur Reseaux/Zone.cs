using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planificateur_Reseaux
{

    [System.Serializable()]
    public class Zone
    {


        public Color couleur { get; set; }
        public float rayon { get; set; }
        public Region region;


        public Zone() { }

        public Zone(Region region,Color couleur,float rayon) {
            this.region = region;
            this.couleur = couleur;
            this.rayon = rayon;
        }



    }
}
