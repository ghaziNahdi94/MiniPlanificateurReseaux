using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Planificateur_Reseaux
{

    [System.Serializable()]
    class GenerateColor
    {
        
        List<Color> couleurs = new List<Color>();
        List<Color> utilise = new List<Color>();
      

        public GenerateColor() {

            
            couleurs.Add(Color.FromArgb(255, 0, 0));
            couleurs.Add(Color.FromArgb(0, 255, 0));
            couleurs.Add(Color.FromArgb(0, 0, 255));
            couleurs.Add(Color.FromArgb(200, 200, 200));
            couleurs.Add(Color.FromArgb(250, 0, 100));
            couleurs.Add(Color.FromArgb(100, 0, 250));
            couleurs.Add(Color.FromArgb(250, 100, 0));
            couleurs.Add(Color.FromArgb(100, 250, 0));
            couleurs.Add(Color.FromArgb(0, 250, 100));
            couleurs.Add(Color.FromArgb(180, 0, 0));
            couleurs.Add(Color.FromArgb(0, 180, 0));
            couleurs.Add(Color.FromArgb(0, 0, 180));
            couleurs.Add(Color.FromArgb(100, 100, 100));
            couleurs.Add(Color.FromArgb(175, 0, 100));
            couleurs.Add(Color.FromArgb(100, 0, 175));
            couleurs.Add(Color.FromArgb(175, 100, 0));
            couleurs.Add(Color.FromArgb(100, 175, 0));
            couleurs.Add(Color.FromArgb(0, 175, 100));
            couleurs.Add(Color.FromArgb(105, 0, 0));
            couleurs.Add(Color.FromArgb(0, 105, 0));
            couleurs.Add(Color.FromArgb(0, 0, 105));
            couleurs.Add(Color.FromArgb(100, 0, 100));
            couleurs.Add(Color.FromArgb(100, 100, 0));
            couleurs.Add(Color.FromArgb(0, 100, 100));








        }


        public Color getColor() {

             if(couleurs.Count >= 1) { 
                Color couleur = couleurs.ElementAt(0);
                couleurs.RemoveAt(0);
                utilise.Add(couleur);
                return couleur;
            }
            else {
                return Color.Black;
            }

        }


        public void removeColor(Color couleur) {

            utilise.Remove(couleur);
            couleurs.Add(couleur);

        }




    }
}
