using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planificateur_Reseaux
{

    [System.Serializable()]
    public class Couverture
    {


        public Point centre { get; set; }
        public float rayon { get; set; }
        public Color couleur { get; set; }

        public List<Obstacle> obstacles = new List<Obstacle>();


        [System.NonSerialized]
        public List<Zone> zones = null;

        [System.NonSerialized]
        public List<GraphicsPath> pathes = new List<GraphicsPath>();


        public List<Morceau> morceaux = new List<Morceau>();

        [System.NonSerialized]
        public Graphics gr = null;

        public Couverture(Point centre,float rayon,Color couleur)
        {
            
            this.centre = centre;
            this.rayon = rayon;
            this.couleur = couleur;
        }



        //Methodes


        public void drawCouverture(Graphics g) {
             zones =  new List<Zone>();


            gr = g;
            
            drawCercle(centre, rayon / 10, g, Color.FromArgb(255, couleur.R, couleur.G, couleur.B));
            drawRing(centre, (rayon / 10) * 2, rayon / 10, g, Color.FromArgb(229, couleur.R, couleur.G, couleur.B));
            drawRing(centre, (rayon / 10) * 3, (rayon / 10) * 2, g, Color.FromArgb(204, couleur.R, couleur.G, couleur.B));
            drawRing(centre, (rayon / 10) * 4, (rayon / 10) * 3, g, Color.FromArgb(178, couleur.R, couleur.G, couleur.B));
            drawRing(centre, (rayon / 10) * 5, (rayon / 10) *4, g, Color.FromArgb(153, couleur.R, couleur.G, couleur.B));
            drawRing(centre,(rayon / 10) * 6, (rayon / 10) * 5, g, Color.FromArgb(102, couleur.R, couleur.G, couleur.B));
            drawRing(centre,(rayon / 10) * 7, (rayon / 10) * 6, g, Color.FromArgb(76, couleur.R, couleur.G, couleur.B));
            drawRing(centre, (rayon / 10) * 8, (rayon / 10) * 7, g, Color.FromArgb(51, couleur.R, couleur.G, couleur.B));
            drawRing(centre,(rayon / 10) * 9, (rayon / 10) * 8, g, Color.FromArgb(25, couleur.R, couleur.G, couleur.B));
            drawRing(centre,(rayon / 10) * 10, (rayon / 10) * 9, g, Color.FromArgb(6, couleur.R, couleur.G, couleur.B));
            






            dessiner(g);





        }
  
                    
                    




        


    

            


       

        private void drawCercle(Point centre, float rayon, Graphics g, Color couleur)
        {

            float perimetre = 2 * rayon;
            SolidBrush brush = new SolidBrush(couleur);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(centre.X - rayon,centre.Y-rayon,perimetre,perimetre);
 

            zones.Add(new Zone(new Region(path),couleur,rayon));

        }

        private void drawRing(Point centre, float rayonRing, float rayonCercle, Graphics g, Color couleur)
        {

            float permietreRing = 2 * rayonRing;
            float perimetreCercle = 2 * rayonCercle;
         

            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();

            path1.AddEllipse(centre.X - rayonRing, centre.Y - rayonRing, permietreRing, permietreRing);
            path2.AddEllipse(centre.X - rayonCercle, centre.Y - rayonCercle, perimetreCercle, perimetreCercle);

            Region region = new Region(path1);

            region.Exclude(path2);



            zones.Add(new Zone(region,couleur,rayonRing));
       


        }

        private float distance(Point a,Point b)
        { return (float) (Math.Sqrt((a.X-b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y))); }



        private void dessiner(Graphics g)
        {



            foreach (Zone zone in zones)
            {

                foreach (GraphicsPath path in pathes)
                {

                   
                    zone.region.Exclude(path);
                   
                }

                g.FillRegion(new SolidBrush(zone.couleur), zone.region);


            }


            foreach (Morceau m in morceaux)
            {
                g.FillRegion(new SolidBrush(m.couleur),m.region);
            }



        }







        public void updateObstacle() {

            pathes = new List<GraphicsPath>();
            morceaux = new List<Morceau>();

            foreach(Obstacle obstacle in obstacles)
            {
               
                Point a = new Point(obstacle.picture.Location.X, obstacle.picture.Location.Y);
                Point b = new Point(obstacle.picture.Location.X + obstacle.picture.Width, obstacle.picture.Location.Y);
                Point c = new Point(obstacle.picture.Location.X + obstacle.picture.Width, obstacle.picture.Location.Y + obstacle.picture.Height);
                Point d = new Point(obstacle.picture.Location.X, obstacle.picture.Location.Y + obstacle.picture.Height);

                float dist = distance(a,centre);

                if (dist < distance(b, centre))
                    dist = distance(b, centre);

                if (dist < distance(c, centre))
                    dist = distance(c, centre);

                if (dist < distance(d, centre))
                    dist = distance(d, centre);


                GraphicsPath path = getPathObstacle(obstacle,gr);

                pathes.Add(path);

            }



        }


        private void drawMorceauDepart(float start,float end,float rayonMorceau,Color couleur, int repaire,float angle1,float angle2,int genre)
        {


            SolidBrush brush = new SolidBrush(couleur);

            GraphicsPath path = new GraphicsPath();
            path.AddPie(new Rectangle((int)(centre.X - rayonMorceau), (int)(centre.Y - rayonMorceau), (int)(2 * rayonMorceau), (int)(2 * rayonMorceau)),start,end);
            morceaux.Add(new Morceau(new Region(path), couleur, repaire, angle1,angle2,genre));
        }


        private void drawMorceauN(float start, float end, float rayonMorceau, float rayonCercle, Color couleur,GraphicsPath triangle, int repaire,float angle1,float angle2,int genre)
        {


            float perimetreCercle = 2 * rayonCercle;


            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();

            path1.AddPie(new Rectangle((int)(centre.X - rayonMorceau), (int)(centre.Y - rayonMorceau), (int)(2 * rayonMorceau), (int)(2 * rayonMorceau)), start, end);
            path2.AddPie(new Rectangle((int)(centre.X - rayonCercle), (int)(centre.Y - rayonCercle), (int)(2 * rayonCercle), (int)(2 * rayonCercle)), start, end);

            Region region = new Region(path1);

            region.Exclude(path2);
            region.Exclude(triangle);


            morceaux.Add(new Morceau(region, couleur,repaire,angle1,angle2,genre));




        }


        private void drawAllMorceaux(float start,float end, GraphicsPath polygonPath,int val,int repaire,float angle1,float angle2,int genre)
        {

            

            int[] opacitys = new int[] { 255,229,204,178,153,102,76,51,25,6};

            for (int i=0;i<opacitys.Length;i++)
            {

                if (opacitys[i] - val < 0)
                    opacitys[i] = 0;
                else
                    opacitys[i] -= val;
            }

           
            drawMorceauDepart(start,end, rayon / 10, Color.FromArgb(opacitys[0], couleur.R, couleur.G, couleur.B),repaire,angle1,angle2,genre);
            drawMorceauN(start, end, (rayon / 10) * 2, rayon / 10, Color.FromArgb(opacitys[1], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 3, (rayon / 10) * 2, Color.FromArgb(opacitys[2], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 4, (rayon / 10) * 3, Color.FromArgb(opacitys[3], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 5, (rayon / 10) * 4, Color.FromArgb(opacitys[4], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 6, (rayon / 10) * 5, Color.FromArgb(opacitys[5], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 7, (rayon / 10) * 6, Color.FromArgb(opacitys[6], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 8, (rayon / 10) * 7, Color.FromArgb(opacitys[7], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 9, (rayon / 10) * 8, Color.FromArgb(opacitys[8], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
            drawMorceauN(start, end, (rayon / 10) * 10, (rayon / 10) * 9, Color.FromArgb(opacitys[9], couleur.R, couleur.G, couleur.B), polygonPath, repaire, angle1, angle2, genre);
        }


        private int getPourcentage(int id)
        {
           
            switch (id)
            {
                case 1:
                    return 25;
                    break;

                case 2:
                    return 25;
                    break;

                case 3:
                    return 25;
                    break;

                case 4:
                    return 25;
                    break;

                case 5:
                    return 51;
                    break;

                case 6:
                    return 51;
                    break;

                case 7:
                    return 102;
                    break;

                case 8:
                    return 102;
                    break;

                case 9:
                    return 102;
                    break;

                case 10:
                    return 102;
                    break;

                case 11:
                    return 102;
                    break;

                case 12:
                    return 102;
                    break;

                case 13:
                    return 102;
                    break;

                case 14:
                    return 102;
                    break;

                case 15:
                    return 102;
                    break;

                case 16:
                    return 102;
                    break;

                case 17:
                    return 178;
                    break;

                case 18:
                    return 178;
                    break;

                case 19:
                    return 178;
                    break;

                case 20:
                    return 178;
                    break;

                case 21:
                    return 210;
                    break;

                case 22:
                    return 210;
                    break;

                case 23:
                    return 210;
                    break;

                case 24:
                    return 210;
                    break;

                case 25:
                    return 229;
                    break;

                case 26:
                    return 229;
                    break;

                default: return 1;



            }

        }

        private GraphicsPath getPathObstacle(Obstacle obstacle,Graphics g)
        {
            GraphicsPath path = new GraphicsPath(); // path obstacle
            GraphicsPath polygonPath = new GraphicsPath();


            Point a = new Point(obstacle.picture.Location.X, obstacle.picture.Location.Y);
            Point b = new Point(obstacle.picture.Location.X + obstacle.picture.Width, obstacle.picture.Location.Y);
            Point c = new Point(obstacle.picture.Location.X + obstacle.picture.Width, obstacle.picture.Location.Y + obstacle.picture.Height);
            Point d = new Point(obstacle.picture.Location.X, obstacle.picture.Location.Y + obstacle.picture.Height);
            Point axe1 = new Point(0, 0);
            Point axe2 = new Point(0, 0);


            float angle1 = 0;
            float angle2 = 0;
            Point haut = new Point(0, 0);
            Point bas = new Point(0, 0);



            //Coté droit
            if (a.X > centre.X && b.X > centre.X)
            {

                

                if (a.Y < centre.Y && d.Y > centre.Y)  //coupe l'axe des absice
                {
                    haut = a; bas = d;
                    axe1 = new Point(a.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));


                   
                    path.AddPie(new Rectangle( (int)(centre.X - rayon), (int)(centre.Y - rayon),(int)(2*rayon), (int)(2 * rayon)), angle2, -angle2 - angle1);
                    polygonPath.AddPolygon(new Point[] {centre,haut,bas});
                    path.AddPath(polygonPath, false);


                    drawAllMorceaux(angle2, -angle2 - angle1,polygonPath,getPourcentage(obstacle.id),1,angle1,angle2,getPourcentage(obstacle.id));


            

                }
                else if (a.Y < centre.Y && d.Y < centre.Y) // au dessus 
                {
                    haut = a; bas = c;
                    axe1 = new Point(a.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));

                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), -angle2, -angle1 + angle2);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);


                    drawAllMorceaux(-angle2, -angle1 + angle2, polygonPath, getPourcentage(obstacle.id),2, angle1, angle2, getPourcentage(obstacle.id));



                }
                else  //au dessous
                {
                    haut = b; bas = d;
                    axe1 = new Point(b.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), angle2, -angle2 + angle1);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);


                    drawAllMorceaux(angle2, -angle2 + angle1, polygonPath, getPourcentage(obstacle.id),3, angle1, angle2, getPourcentage(obstacle.id));

                }




                //Coté gauche
            }
            else if (a.X < centre.X && b.X < centre.X)
            {


                if (a.Y < centre.Y && d.Y > centre.Y)  //coupe l'axe des absice
                {
                    haut = b; bas = c;
                    axe1 = new Point(b.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), -angle2 - 180, angle1 + angle2);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);

                    drawAllMorceaux(-angle2 - 180, angle1 + angle2, polygonPath, getPourcentage(obstacle.id),4, angle1, angle2, getPourcentage(obstacle.id));

                }
                else if (a.Y < centre.Y && d.Y < centre.Y) // au dessus 
                {
                    haut = b; bas = d;
                    axe1 = new Point(b.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), angle2 + 180, angle1 - angle2);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);

                    drawAllMorceaux(angle2 + 180, angle1 - angle2, polygonPath, getPourcentage(obstacle.id),5, angle1, angle2, getPourcentage(obstacle.id));
                }
                else  //au dessous
                {
                    haut = a; bas = c;
                    axe1 = new Point(a.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), -angle2 - 180, -angle1 + angle2);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);

                    drawAllMorceaux(-angle2 - 180, -angle1 + angle2, polygonPath, getPourcentage(obstacle.id),6, angle1, angle2, getPourcentage(obstacle.id));
                }






            }
            else  // au milieu coupe l'axe des ordonnées
            {


                if (a.Y < centre.Y && d.Y < centre.Y) // en haut
                {
                    haut = d; bas = c;
                    axe1 = new Point(centre.X, d.Y); axe2 = new Point(centre.X, c.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), -(90 - angle2), -(angle1 + angle2));
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);

                    drawAllMorceaux(-(90 - angle2), -(angle1 + angle2), polygonPath, getPourcentage(obstacle.id),7, angle1, angle2, getPourcentage(obstacle.id));

                }
                else if (a.Y > centre.Y && d.Y > centre.Y) // en bas
                {
                    haut = a; bas = b;
                    axe1 = new Point(centre.X, a.Y); axe2 = new Point(centre.X, b.Y);
                    angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                    angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)), -(90 + angle2) - 180, angle1 + angle2);
                    polygonPath.AddPolygon(new Point[] { centre, haut, bas });
                    path.AddPath(polygonPath, false);

                    drawAllMorceaux(-(90 + angle2) - 180, angle1 + angle2, polygonPath, getPourcentage(obstacle.id),8, angle1, angle2, getPourcentage(obstacle.id));

                }
                else
                {

                    path.AddPie(new Rectangle((int)(centre.X - rayon), (int)(centre.Y - rayon), (int)(2 * rayon), (int)(2 * rayon)),0,360);
                      polygonPath.AddPolygon(new Point[] {centre,haut,bas});
                    path.AddPath(polygonPath, false);


                    drawAllMorceaux(0,360, polygonPath, getPourcentage(obstacle.id),9, angle1, angle2, getPourcentage(obstacle.id));
                }

            }


            return path;

            }















        public float calculateAngle(double opp, double hyp)
        {

            double sinAngle = opp / hyp;

            return (float)radianToDegree(Math.Asin(sinAngle)); ;
        }

        private double radianToDegree(double angle)
        {
            return 180.0 * angle / Math.PI;
        }

    }

}
