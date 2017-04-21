using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



namespace Planificateur_Reseaux
{

    [System.Serializable()]
    public class AccesPoint
    {

        
        public bool dragMode { get; set; }
        public string nom { get; set; }

        [System.NonSerialized]
        public SizeablePictureBox image ;

       

       

        public AccesPoint()
        {

        }


        public Point position { get; set; }
        public float puissance { get; set; }
        public Couverture couverture { get; set; }
        public Color couluer { get; set; }
 

        public float rayon { get; set; }
        public Point centre { get; set; }



     


        public AccesPoint(string nom,float puissance, SizeablePictureBox image,Color couleur)
        {
            dragMode = false;
            this.nom = nom;
            this.puissance = puissance;
            this.image = image;
            this.couluer = couluer;
            rayon = (float)(1.666 * puissance + 83.334);
            centre = new Point(this.image.Location.X + this.image.Width / 2, this.image.Location.Y + this.image.Height / 2);
            this.couverture = new Couverture(centre,rayon, couleur);

           

        }




        //methode
        public void updateAccesPoint() {

            this.centre = new Point(this.image.Location.X + this.image.Width / 2, this.image.Location.Y + this.image.Height / 2);
            couverture.rayon = (float)(1.666 * puissance + 83.334);

        }



    }
}
