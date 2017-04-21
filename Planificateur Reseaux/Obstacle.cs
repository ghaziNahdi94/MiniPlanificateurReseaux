using System.Drawing;
using System.Windows.Forms;

namespace Planificateur_Reseaux
{


    [System.Serializable()]
    public class Obstacle
    {
        [System.NonSerialized]
        public SizeablePictureBox picture;
        public int id { get; set; }
        public Point position { get; set; }

        public Size taille; 

     

        public Obstacle() { }

        public Obstacle(int id, SizeablePictureBox picture) {
            this.id = id;
            this.picture = picture;
            taille = picture.Size;
            this.position = new Point(picture.Location.X, picture.Location.Y);


          
        }


        public void updatePosition() {
            this.position = new Point(picture.Location.X - 2, picture.Location.Y);
            this.taille = picture.Size;
        }


        public static Image getImageFromId(int id) {

            switch (id) {

                case 1: return Properties.Resources.gimp_sap1;
                    break;

                case 2:
                    return Properties.Resources.Door_smooth_3panels_red01__CC_BY_NC_SA_by_thedarkmod_com_;
                    break;

                case 3:
                    return Properties.Resources.goblet;
                    break;

                case 4:
                    return Properties.Resources.sachet;
                    break;

                case 5:
                    return Properties.Resources.verre;
                    break;

                case 6:
                    return Properties.Resources.fenetre;
                    break;

                case 7:
                    return Properties.Resources.verre_vert;
                    break;

                case 8:
                    return Properties.Resources.vase;
                    break;

                case 9:
                    return Properties.Resources.bouteille_eau;
                    break;

                case 10:
                    return Properties.Resources.piscine;
                    break;

                case 11:
                    return Properties.Resources.man;
                    break;

                case 12:
                    return Properties.Resources.femme;
                    break;

                case 13:
                    return Properties.Resources.mur;
                    break;

                case 14:
                    return Properties.Resources.cheminer;
                    break;

                case 15:
                    return Properties.Resources.mur_platre;
                    break;

                case 16:
                    return Properties.Resources.statue;
                    break;

                case 17:
                    return Properties.Resources.verre_ceramique;
                    break;

                case 18:
                    return Properties.Resources.vase_ceramique;
                    break;

                case 19:
                    return Properties.Resources.carnet;
                    break;

                case 20:
                    return Properties.Resources.papier;
                    break;

                case 21:
                    return Properties.Resources.mur_beton;
                    break;

                case 22:
                    return Properties.Resources.cheminer_beton;
                    break;

                case 23:
                    return Properties.Resources.plaque_verre_blinde;
                    break;

                case 24:
                    return Properties.Resources.fenetre_verre_blinde;
                    break;

                case 25:
                    return Properties.Resources.porte_metal;
                    break;

                case 26:
                    return Properties.Resources.robot;
                    break;

                default: return null;
                    

            }

        }





        public static int getidFromDragAndDropObstacle(ComboBox listeObstacles,bool isFisrtObstacle)
        {

            switch (listeObstacles.SelectedIndex)
            {
                case 0:
                    if (isFisrtObstacle)
                        return 1;
                    else
                        return 2;
                    break;

                case 1:
                    if (isFisrtObstacle)
                        return 3;
                    else
                        return 4;
                    break;

                case 2:
                    if (isFisrtObstacle)
                        return 5;
                    else
                        return 6;
                    break;

                case 3:
                    if (isFisrtObstacle)
                        return 7;
                    else
                        return 8;
                    break;

                case 4:
                    if (isFisrtObstacle)
                        return 9;
                    else
                        return 10;
                    break;

                case 5:
                    if (isFisrtObstacle)
                        return 11;
                    else
                        return 12;
                    break;

                case 6:
                    if (isFisrtObstacle)
                        return 13;
                    else
                        return 14;
                    break;
                case 7:
                    if (isFisrtObstacle)
                        return 15;
                    else
                        return 16;
                    break;

                case 8:
                    if (isFisrtObstacle)
                        return 17;
                    else
                        return 18;
                    break;

                case 9:
                    if (isFisrtObstacle)
                        return 19;
                    else
                        return 20;
                    break;

                case 10:
                    if (isFisrtObstacle)
                        return 21;
                    else
                        return 22;
                    break;

                case 11:
                    if (isFisrtObstacle)
                        return 23;
                    else
                        return 24;
                    break;

                case 12:
                    if (isFisrtObstacle)
                        return 25;
                    else
                        return 26;
                    break;

                default: return -1;
            }

        


            }

        










    }
}
