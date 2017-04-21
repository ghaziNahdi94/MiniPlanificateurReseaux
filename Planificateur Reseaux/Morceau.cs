using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planificateur_Reseaux
{

    [System.Serializable()]
    public class Morceau
    {
        [System.NonSerialized]
        public Region region;
        public Color couleur;
        public int repaire;
        public float angle1;
        public float angle2;
        public int genre;


        public Morceau() { }


        public Morceau(Region region, Color couleur,int repaire,float angle1,float angle2,int genre) {

            this.region = region;
            this.couleur = couleur;
            this.repaire = repaire;
            this.angle1 = angle1;
            this.angle2 = angle2;
            this.genre = genre;

        }



        public float getDistance(float rayon)
        {

          
            switch (genre)
            {


              
                case 229:
                    return rayon / 10;
                    break;
                case 204:
                    return (rayon / 10) * 2;
                    break;
                case 178:
                    return (rayon / 10) * 3;
                    break;
                case 153:
                    return (rayon / 10) * 4;
                    break;
                case 102:
                    return (rayon / 10) * 5;
                    break;
                case 76:
                    return (rayon / 10) * 6;
                    break;
                case 51:
            
                    return (rayon / 10) * 7;
                    break;
                case 25:
                    return (rayon / 10) * 8;
                    break;
                case 6: return (rayon/10)*9;
                    break;

                default: return rayon/10;
            

            }

        }



    }
}
