using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Planificateur_Reseaux
{


    public partial class Form1 : Form
    {

        private Point MouseDownLocation;
        private bool dragMode = false;
        private bool cursorMode = true;
        private SizeablePictureBox picture = null;


        private int nbrAccesPoint = 0;
        private int nbrStation = 0;

        private List<AccesPoint> accesPoints = new List<AccesPoint>();
        private GenerateColor colorGenerator = new GenerateColor();


        private List<Station> stations = new List<Station>();

        private List<Obstacle> obstacles = new List<Obstacle>();


      private Bitmap bitmap = null;


        //Paramétre
        private float defaultPuissance = 80;
        private Color backTableau = Color.White;


     

        //Constructeur 1
        public Form1()
        {
            InitializeComponent();
            this.Text = " Nouveau document  |  Planificateur Réseaux";


            bitmap = new Bitmap(tableau.Width,tableau.Height);
            

            //Curseurs
            handPictureBox.Cursor = Cursors.Default;
            deletePictureBox.Cursor = Cursors.Default;
            barreMenu.Cursor = Cursors.Default;
            this.Cursor = Cursors.Hand;


            toolBar.Renderer = new MyRender();

            accesPointTool.Click += ajouterPointDaccésToolStripMenuItem_Click;
            stationTool.Click += ajouterStationToolStripMenuItem_Click;
            paramTool.Click += paramétresToolStripMenuItem_Click;
            nouveauTool.Click += nouveauToolStripMenuItem_Click;
            ouvrirTool.Click += ouvrirToolStripMenuItem_Click;
            enrgTool.Click += enregistrerToolStripMenuItem_Click;
            couleurTool.BackColor = backTableau;


        }


        //Constructeur 2
        public Form1(String file) {

            InitializeComponent();
            String fileName = file.Substring(file.LastIndexOf('\\') + 1);
            String fileNameWithoutExtention = fileName.Substring(0, fileName.IndexOf('.'));
            this.Text = fileNameWithoutExtention+"   |  Planificateur Réseaux";
            

            //Curseurs
            handPictureBox.Cursor = Cursors.Default;
            deletePictureBox.Cursor = Cursors.Default;
            barreMenu.Cursor = Cursors.Default;
            this.Cursor = Cursors.Hand;


            toolBar.Renderer = new MyRender();
            accesPointTool.Click += ajouterPointDaccésToolStripMenuItem_Click;
            stationTool.Click += ajouterStationToolStripMenuItem_Click;
            paramTool.Click += paramétresToolStripMenuItem_Click;
            nouveauTool.Click += nouveauToolStripMenuItem_Click;
            ouvrirTool.Click += ouvrirToolStripMenuItem_Click;
            enrgTool.Click += enregistrerToolStripMenuItem_Click;
            couleurTool.BackColor = backTableau;



            Stream stream = File.Open(file, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            Saver saver = (Saver)bf.Deserialize(stream);

            accesPoints.Clear(); stations.Clear(); obstacles.Clear();

            accesPoints = saver.listeAccesPoints;
            stations = saver.listeStations;
            obstacles = saver.listeObstacles;
            colorGenerator = saver.generator;
            nbrAccesPoint = saver.nbrAccesPoint;
            nbrStation = saver.nbrStation;
            backTableau = saver.backTableau;
            defaultPuissance = saver.defaultPuissance;

            tableau.BackColor = backTableau;


            foreach (AccesPoint ac in accesPoints)
            {  
                reglerImage(ref ac.image);
                ac.image.Image = Properties.Resources.wireless_router;
                ac.image.grab = 2;
                ac.image.Location = new Point(ac.centre.X - ac.image.Size.Width / 2, ac.centre.Y - ac.image.Size.Height / 2);
                tableau.Controls.Add(ac.image);
            }

            foreach (Station st in stations)
            {
                reglerImage(ref st.image);
                st.image.Image = Properties.Resources.computer;
                st.image.Location = st.position;
                st.image.grab = 2;
                st.image.DoubleClick += doubleClickStations;
                tableau.Controls.Add(st.image);

            }

            foreach (Obstacle ob in obstacles)
            {

                reglerImage(ref ob.picture);
                ob.picture.Image = Obstacle.getImageFromId(ob.id);
                ob.picture.Location = ob.position;
                ob.picture.Size = ob.taille;
                tableau.Controls.Add(ob.picture);
            }

            
            foreach (AccesPoint ac in accesPoints) {


                foreach (Obstacle obCouverture in ac.couverture.obstacles) {


                    foreach (Obstacle ob in obstacles)
                    {
                        if (ob.id == obCouverture.id && ob.taille == obCouverture.taille && ob.position == obCouverture.position)
                        {
                            obCouverture.picture = ob.picture;
                            break;

                        }

                    }



                }


                ac.couverture.updateObstacle();

            }

           

            tableau.Refresh();



        }

        private void AccesPointPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }


        //Methode de Design
        private void fermerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    

        private void handEntred(object sender, EventArgs e)
        {
            handPictureBox.BackColor = Color.Black;

        }

        private void handLeaved(object sender, EventArgs e)
        {
            handPictureBox.BackColor = Color.Transparent;
        }

        private void deleteEntred(object sender, EventArgs e)
        {
            deletePictureBox.BackColor = Color.Black;
        }

        private void deleteLeaved(object sender, EventArgs e)
        {
            deletePictureBox.BackColor = Color.Transparent;
        }

        private void accesPointEntred(object sender, EventArgs e)
        {
            accesPointPictureBox.BackColor = Color.White;


            if (cursorMode == false)
                accesPointPictureBox.Cursor = Cursors.No;
            else
                accesPointPictureBox.Cursor = Cursors.Hand;
        }

        private void accesPointLeaved(object sender, EventArgs e)
        {
            accesPointPictureBox.BackColor = Color.Transparent;
        }

        private void stationEntred(object sender, EventArgs e)
        {
            stationPictureBox.BackColor = Color.Black;


            if (cursorMode == false)
               stationPictureBox.Cursor = Cursors.No;
            else
                stationPictureBox.Cursor = Cursors.Hand;
        }

        private void stationLeaved(object sender, EventArgs e)
        {
            stationPictureBox.BackColor = Color.Transparent;
        }

        private void o1Entred(object sender, EventArgs e)
        {
            o1PictureBox.BackColor = Color.Black;


            if (cursorMode == false)
                o1PictureBox.Cursor = Cursors.No;
            else
               o1PictureBox.Cursor = Cursors.Hand;
        }

        private void o1Leaved(object sender, EventArgs e)
        {
            o1PictureBox.BackColor = Color.Transparent;
        }

        private void o2Entred(object sender, EventArgs e)
        {
            o2PictureBox.BackColor = Color.Black;


            if (cursorMode == false)
                o2PictureBox.Cursor = Cursors.No;
            else
                o2PictureBox.Cursor = Cursors.Hand;
        }

        private void o2Leaved(object sender, EventArgs e)
        {
            o2PictureBox.BackColor = Color.Transparent;
        }

        private void listeObstaclesEntred(object sender, EventArgs e)
        {
            if (cursorMode == false)
                listeObstacles.Cursor = Cursors.No;
            else
                listeObstacles.Cursor = Cursors.Hand;
        }











        //Click Events

        private void pictureBox2_Click(object sender, EventArgs e)//hand click
        {
            this.Cursor = Cursors.Hand;
            cursorMode = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e) //delete click
        {
            this.Cursor = new Cursor(new Bitmap(Properties.Resources.delete_icon_54559, new Size(32, 32)).GetHicon());
            cursorMode = false;
        }


       


            //Mouse down Events
        private void elementMouseDown(PictureBox picture)   // acces point, station, obstacles
        {

                this.Cursor = new Cursor(new Bitmap(picture.Image, picture.Size).GetHicon());
                 dragMode = true;    
        }

        private void accesPointDown(object sender, MouseEventArgs e)
        {
            if (cursorMode) {
                elementMouseDown(accesPointPictureBox);
            }
        }





        private void stationDown(object sender, MouseEventArgs e)
        {
            if (cursorMode)
            {
                elementMouseDown(stationPictureBox);
            }
        }

        private void o1Down(object sender, MouseEventArgs e) //obstacle 1
        {
            if (cursorMode)
            {
                elementMouseDown(o1PictureBox);
            }
        }

        private void o2Down(object sender, MouseEventArgs e) //obstacle 2
        {
            if (cursorMode)
            {
                elementMouseDown(o2PictureBox);
            }
        }








        //Mouse UP events
        private void mouseUpElements(PictureBox picture) {

            if (cursorMode)
            {
                this.Cursor = Cursors.Hand;
                cursorMode = true;



                SizeablePictureBox p = new SizeablePictureBox();
                p.Image = picture.Image;
                p.SizeMode = picture.SizeMode;
                p.Size = picture.Size;


                //si p est un obstacle il faut taguer le nom
                if (picture.Image.Equals(o1PictureBox.Image))
                    p.Tag = Obstacle.getidFromDragAndDropObstacle(listeObstacles, true);
                else if (picture.Image.Equals(o2PictureBox.Image))
                    p.Tag = Obstacle.getidFromDragAndDropObstacle(listeObstacles, false);
                else
                    p.Tag = null;
                



                this.picture = p;

            }

            
        }

        private void accesPointUp(object sender, MouseEventArgs e)
        {
               mouseUpElements(accesPointPictureBox);
        }






        private void stationUp(object sender, MouseEventArgs e)
        {
            mouseUpElements(stationPictureBox);
        }

        private void o1Up(object sender, MouseEventArgs e)
        {
            mouseUpElements(o1PictureBox);
        }

        private void o2Up(object sender, MouseEventArgs e)
        {
            mouseUpElements(o2PictureBox);
        }




        //cancel drag Mode
        private void Panel1Enter(object sender, EventArgs e)
        {
            dragMode = false;
        }

        private void barreMenuEnter(object sender, EventArgs e)
        {
            dragMode = false;
        }





        //Drag and drop Elements dans le tableau
        private void picture_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                
                MouseDownLocation = e.Location;

                PictureBox p = (PictureBox)sender;
                AccesPoint ap = null;
                Station station = null;
               

                foreach (AccesPoint a in accesPoints)
                {
                    if (a.image.Equals(p)) { ap = a; break; }
                }

                foreach (Station s in stations) {
                    if (s.image.Equals(p)) { station = s;  break; }
                }

                if (ap != null) {
                    ap.dragMode = true;
                    tableau.Refresh();
                }

                if (station != null) {
                    station.dragMode = true;
                    tableau.Refresh();
                }


            }
        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {

            PictureBox p = (PictureBox)sender;
            
            if (e.Button == MouseButtons.Left)
            {
            
               p.Left = e.X + p.Left - MouseDownLocation.X;
                p.Top = e.Y + p.Top - MouseDownLocation.Y;
                
            }


        }

        private void connectStations()
        {


            //connect station to acces point
            foreach (Station st in stations)
            {

                Point a = new Point(st.image.Location.X, st.image.Location.Y);
                Point b = new Point(st.image.Location.X + st.image.Width, st.image.Location.Y);
                Point c = new Point(st.image.Location.X + st.image.Width, st.image.Location.Y + st.image.Height);
                Point d = new Point(st.image.Location.X, st.image.Location.Y + st.image.Height);
                Point axe1 = new Point(0, 0);
                Point axe2 = new Point(0, 0);


                float angle1 = 0;
                float angle2 = 0;
                Point haut = new Point(0, 0);
                Point bas = new Point(0, 0);


                foreach (AccesPoint ac in accesPoints)
                {

                    Point centre = ac.centre;

                    //Coté droit
                    if (a.X > centre.X && b.X > centre.X)
                    {

                        int cas = 1;

                        if (a.Y < centre.Y && d.Y > centre.Y)  //coupe l'axe des absice
                        {
                            haut = a; bas = d;
                            axe1 = new Point(a.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));







                        }
                        else if (a.Y < centre.Y && d.Y < centre.Y) // au dessus 
                        {

                            haut = a; bas = c;
                            axe1 = new Point(a.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));





                            cas = 2;




                        }
                        else  //au dessous
                        {
                            haut = b; bas = d;
                            axe1 = new Point(b.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));

                            cas = 3;

                        }






                        if (ac.couverture.morceaux != null && ac.couverture.morceaux.Count > 0) // il existe des morceaux
                        {
                            bool trouver = false;
                            bool optimal = false;
                            foreach (Morceau m in ac.couverture.morceaux)
                            {


                                if ((m.repaire == 2) && (((m.angle1 >= angle1) && (m.angle2 <= angle2) && (cas == 2))))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(d.X, d.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 1) && (((m.angle1 >= angle1) && (cas == 2 || cas == 1)) || ((m.angle2 >= angle2) && (cas == 3))))
                                {
                                    Point rep = new Point(0, 0);

                                    if (cas == 1)
                                        rep = new Point(a.X, a.Y + st.image.Height / 2);
                                    else if (cas == 2)
                                        rep = d;
                                    else
                                        rep = a;



                                    optimal = true;
                                    float dist = distance(new Point(rep.X, rep.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }

                                }
                                else if ((m.repaire == 3) && (((m.angle1 <= angle1) && (m.angle2 >= angle2)) && (cas == 3)))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(a.X, a.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 7) && ((90 - m.angle2 <= angle2) && (cas == 2)))
                                {
                                    optimal = true;
                                    float dist = distance(new Point(d.X, d.Y), centre);
                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 8) && ((90 - m.angle2 <= angle1) && (cas == 3)))
                                {
                                    optimal = true;
                                    float dist = distance(a, centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if (m.repaire == 9)
                                {
                                    optimal = true;
                                    float minDist = distance(a, centre);
                                    if (minDist > distance(b, centre))
                                        minDist = distance(b, centre);

                                    if (minDist > distance(c, centre))
                                        minDist = distance(c, centre);

                                    if (minDist > distance(d, centre))
                                        minDist = distance(d, centre);

                                    if (minDist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, minDist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }





                            }//fin for




                            if (!trouver && !optimal)
                            {

                                if (ac.rayon >= distance(a, ac.centre) || ac.rayon >= distance(d, ac.centre) || ac.rayon >= distance(new Point(a.X, (int)(a.Y + st.image.Height / 2)), ac.centre))
                                    st.connections.Add(new ConnectionAccesPointStation(ac, distance(centre, new Point(a.X, a.Y + st.image.Height / 2)), 0));
                            }



                        }
                        else // il n'existe pas des morceaux
                        {

                            if (ac.rayon >= distance(a, ac.centre) || ac.rayon >= distance(d, ac.centre) || ac.rayon >= distance(new Point(a.X, (int)(a.Y + st.image.Height / 2)), ac.centre))
                                st.connections.Add(new ConnectionAccesPointStation(ac, distance(centre, new Point(a.X, a.Y + st.image.Height / 2)), 0));
                        }





                        //Coté gauche
                    }
                    else if (a.X < centre.X && b.X < centre.X)
                    {

                        int cas = 1;

                        if (a.Y < centre.Y && d.Y > centre.Y)  //coupe l'axe des absice
                        {
                            haut = b; bas = c;
                            axe1 = new Point(b.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));



                        }
                        else if (a.Y < centre.Y && d.Y < centre.Y) // au dessus 
                        {
                            haut = b; bas = d;
                            axe1 = new Point(b.X, centre.Y); axe2 = new Point(d.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));

                            cas = 2;

                        }
                        else  //au dessous
                        {
                            haut = a; bas = c;
                            axe1 = new Point(a.X, centre.Y); axe2 = new Point(c.X, centre.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));
                            cas = 3;

                        }





                        if (ac.couverture.morceaux != null && ac.couverture.morceaux.Count > 0) // il existe des morceaux
                        {
                            bool trouver = false;
                            bool optimal = false;
                            foreach (Morceau m in ac.couverture.morceaux)
                            {




                                if ((m.repaire == 5) && (((m.angle1 >= angle1) && (m.angle2 <= angle2) && (cas == 2))))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(c.X, c.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 4) && (((m.angle1 >= angle1) && (cas == 2 || cas == 1)) || ((m.angle2 >= angle2) && (cas == 3))))
                                {


                                    Point rep = new Point(0, 0);

                                    if (cas == 1)
                                        rep = new Point(b.X, b.Y + st.image.Height / 2);
                                    else if (cas == 2)
                                        rep = c;
                                    else
                                        rep = new Point(b.X, b.Y);



                                    optimal = true;
                                    float dist = distance(new Point(rep.X, rep.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }

                                }
                                else if ((m.repaire == 6) && (((m.angle1 <= angle1) && (m.angle2 >= angle2)) && (cas == 3)))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(b.X, b.Y), centre);

                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }

                                }
                                else if ((m.repaire == 7) && ((90 - m.angle1 <= angle2) && (cas == 2)))
                                {
                                    optimal = true;
                                    float dist = distance(new Point(c.X, c.Y), centre);
                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 8) && ((90 - m.angle1 <= angle1) && (cas == 3)))
                                {
                                    optimal = true;
                                    float dist = distance(new Point(b.X, b.Y), centre);
                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if (m.repaire == 9)
                                {

                                    optimal = true;
                                    float minDist = distance(a, centre);
                                    if (minDist > distance(b, centre))
                                        minDist = distance(b, centre);

                                    if (minDist > distance(c, centre))
                                        minDist = distance(c, centre);

                                    if (minDist > distance(d, centre))
                                        minDist = distance(d, centre);

                                    if (minDist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, minDist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }






                            }


                            if (!trouver && !optimal)
                            {

                                if (ac.rayon >= distance(b, ac.centre) || ac.rayon >= distance(c, ac.centre) || ac.rayon >= distance(new Point(b.X, (int)(b.Y + st.image.Height / 2)), ac.centre))
                                    st.connections.Add(new ConnectionAccesPointStation(ac, distance(centre, new Point(a.X, a.Y + st.image.Height / 2)), 0));
                            }



                        }
                        else // il n'existe pas des morceaux
                        {

                            if (ac.rayon >= distance(b, ac.centre) || ac.rayon >= distance(c, ac.centre) || ac.rayon >= distance(new Point(b.X, (int)(b.Y + st.image.Height / 2)), ac.centre))
                                st.connections.Add(new ConnectionAccesPointStation(ac, distance(centre, new Point(b.X, b.Y + st.image.Height / 2)), 0));
                        }









                    }
                    else  // au milieu coupe l'axe des ordonnées
                    {

                        int cas = 1;
                        if (a.Y < centre.Y && d.Y < centre.Y) // en haut
                        {
                            haut = d; bas = c;
                            axe1 = new Point(centre.X, d.Y); axe2 = new Point(centre.X, c.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));








                        }
                        else if (a.Y > centre.Y && d.Y > centre.Y) // en bas
                        {
                            haut = a; bas = b;
                            axe1 = new Point(centre.X, a.Y); axe2 = new Point(centre.X, b.Y);
                            angle1 = calculateAngle(distance(haut, axe1), distance(haut, centre));
                            angle2 = calculateAngle(distance(bas, axe2), distance(bas, centre));


                            cas = 2;




                        }
                        else
                        {

                            //Ahsen cas
                            st.connections.Add(new ConnectionAccesPointStation(ac, 0, 255));

                        }




                        if (ac.couverture.morceaux != null && ac.couverture.morceaux.Count > 0) // il existe des morceaux
                        {
                            bool trouver = false;
                            bool optimal = false;
                            foreach (Morceau m in ac.couverture.morceaux)
                            {



                                if ((m.repaire == 7) && (m.angle1 >= angle1))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(d.X + st.image.Width / 2, d.Y), centre);
                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }

                                }
                                else if (m.repaire == 9)
                                {

                                    optimal = true;
                                    float minDist = distance(a, centre);
                                    if (minDist > distance(b, centre))
                                        minDist = distance(b, centre);

                                    if (minDist > distance(c, centre))
                                        minDist = distance(c, centre);

                                    if (minDist > distance(d, centre))
                                        minDist = distance(d, centre);

                                    if (minDist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, minDist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }
                                }
                                else if ((m.repaire == 8) && (m.angle1 >= angle1))
                                {

                                    optimal = true;
                                    float dist = distance(new Point(a.X + st.image.Width / 2, a.Y), centre);
                                    if (dist <= m.getDistance(ac.rayon))
                                    {

                                        st.connections.Add(new ConnectionAccesPointStation(ac, dist, m.getDistance(ac.rayon)));
                                        trouver = true;
                                        break;
                                    }

                                }






                            }


                            if (!trouver && !optimal)
                            {

                                if (ac.rayon >= distance(b, ac.centre) || ac.rayon >= distance(c, ac.centre) || ac.rayon >= distance(new Point(b.X, (int)(b.Y + st.image.Height / 2)), ac.centre))
                                    st.connections.Add(new ConnectionAccesPointStation(ac, distance(centre, new Point(a.X, a.Y + st.image.Height / 2)), 0));
                            }



                        }
                        else // il n'existe pas des morceaux
                        {

                            if (c.Y < centre.Y)
                            {
                                if (distance(new Point(d.X + st.image.Width / 2, d.Y), ac.centre) <= ac.rayon)
                                    st.connections.Add(new ConnectionAccesPointStation(ac, distance(new Point(d.X + st.image.Width / 2, d.Y), ac.centre), 0));
                            }
                            else
                            {

                                if (distance(new Point(a.X + st.image.Width / 2, a.Y), ac.centre) <= ac.rayon)
                                    st.connections.Add(new ConnectionAccesPointStation(ac, distance(new Point(a.X + st.image.Width / 2, a.Y), ac.centre), 0));
                            }
                        }

                    }

                }


            }

        }






        private void picture_MouseUp(object sender, MouseEventArgs e) {

            PictureBox p = (PictureBox)sender;
            AccesPoint ap = null;
            Station station = null;
            Obstacle obstacle = null;


            foreach (AccesPoint a in accesPoints)
            {
                if (a.image.Equals(p)) { ap = a; break; }
            }


            foreach (Station s in stations) {
                if (s.image.Equals(p)) { station = s; break; }
            }

            foreach(Obstacle ob in obstacles)
            {
                if (ob.picture.Equals(p)) { obstacle = ob; break; }
            }


            //si c'est une acces point
            if (ap != null) {
                ap.dragMode = false;
                ap.updateAccesPoint();
                tableau.Refresh();
            }

            //si c'est une station
            if (station != null) {
                station.dragMode = false;
                tableau.Refresh();
            }

            //si c'est un obstacle
            if(obstacle != null)
            {
                obstacle.updatePosition();
            }



            if (station == null)
            {
                //detection de l'intersection
                foreach (Obstacle ob in obstacles)
                {

                    Point a = new Point(ob.picture.Location.X, ob.picture.Location.Y);
                    Point b = new Point(a.X + ob.picture.Width, a.Y);
                    Point c = new Point(a.X + ob.picture.Width, a.Y + ob.picture.Height);
                    Point d = new Point(a.X, a.Y + ob.picture.Height);
                    Point f = new Point(a.X, a.Y + ob.picture.Height / 2);
                    Point h = new Point(b.X, ob.picture.Height / 2);
                    Point k = new Point(a.X + ob.picture.Width / 2, a.Y);
                    Point l = new Point(k.X, k.Y + ob.picture.Height);

                    foreach (AccesPoint ac in accesPoints)
                    {

                        // intersection
                        if (((distance(a, ac.centre) <= ac.rayon) || (distance(b, ac.centre) <= ac.rayon) || (distance(c, ac.centre) <= ac.rayon) || (distance(d, ac.centre) <= ac.rayon) || (distance(f, ac.centre) <= ac.rayon) || (distance(h, ac.centre) <= ac.rayon) || (distance(k, ac.centre) <= ac.rayon) || (distance(l, ac.centre) <= ac.rayon))) // intersection obstacle zone de couverture
                        {

                            if (!ac.couverture.obstacles.Contains(ob))
                                ac.couverture.obstacles.Add(ob);

                        }
                        else  // pas d'intersection
                        {
                            if (ac.couverture.obstacles.Contains(ob))
                                ac.couverture.obstacles.Remove(ob);

                        }

                        ac.couverture.updateObstacle();


                    }
                }

            }



            if (station != null)
                station.connections = new List<ConnectionAccesPointStation>();


            connectStations();


            

            tableau.Refresh();
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

        //drager un element dans le tableau
        private void tableauEnter(object sender, EventArgs e)
        {
            if (dragMode && picture != null)
            {
                
                //Add image after drag and drop
                Point position = this.PointToClient(Cursor.Position);
                picture.Location = new Point(position.X-138,position.Y-50);
                picture.BackColor = Color.Transparent;

                if (picture.Image.Equals(stationPictureBox.Image) || picture.Image.Equals(accesPointPictureBox.Image))
                    picture.grab = 2;

                tableau.Controls.Add(picture);

                //Add event for the image
                picture.Click += deleteElement;
                picture.MouseDown += picture_MouseDown;
                picture.MouseMove += picture_MouseMove;
                picture.MouseUp += picture_MouseUp;


                //if it is a Acces Point draw the couverture
                if (picture.Image.Equals(accesPointPictureBox.Image))
                {
                    AccesPoint accesPoint = new AccesPoint("Point d'acces " + (++nbrAccesPoint), defaultPuissance, picture, colorGenerator.getColor());

                    accesPoints.Add(accesPoint);
                    tableau.Refresh();
                }
                else if (picture.Image.Equals(stationPictureBox.Image))
                {
                    picture.DoubleClick += doubleClickStations;
                    Station station = new Station("Station "+ (++nbrStation), picture);
                    stations.Add(station);
                    tableau.Refresh();
                }
                else {

                    
                    Obstacle obstacle = new Obstacle((int) picture.Tag,picture);
                    obstacles.Add(obstacle);
               
                    

                }


                


                dragMode = false;
            }
        }











        //Les dessins dans le tableau
        private void tableauPaint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            
            
            foreach (AccesPoint ap in accesPoints)
            {
                if (!ap.dragMode)
                {
                    ap.couverture.centre = ap.centre;
                    ap.couverture.drawCouverture(g);
                    g.DrawString(ap.nom, new Font(FontFamily.Families[30], 15, FontStyle.Bold), new SolidBrush(Color.Black), new Point(ap.image.Location.X - (ap.nom.Length-ap.nom.Length/2)*3, ap.image.Location.Y + ap.image.Height));
                }

                  }



            foreach (Station s in stations) {

                if (!s.dragMode)
                {
                    s.drawConnection(g);
                    g.DrawString(s.name, new Font(FontFamily.Families[30], 15, FontStyle.Bold), new SolidBrush(Color.Black), new Point(s.image.Location.X - 20, s.image.Location.Y + s.image.Height));
                }
            }






           



        }



            //effacer event
        private void deleteElement(object sender, EventArgs e) {

            if (!cursorMode) {
                PictureBox p = (PictureBox)sender;
                int pictureIs = 0; //1 acces point ; 2 station ; 3 obstacle
                AccesPoint acces = null;
                Station station = null;
                Obstacle obstacle = null;

                foreach (AccesPoint ap in accesPoints) {
                    if (ap.image.Equals(p)) { acces = ap; pictureIs = 1; break; }
                }


               
              if(pictureIs != 1)
                    foreach (Station s in stations)
                    {
                        if (s.image.Equals(p)) { station = s; pictureIs = 2; break; }
                    }

                

               
                  if(pictureIs != 1 && pictureIs != 2)
                    foreach (Obstacle ob in obstacles) {
                        if (ob.picture.Equals(p)) { obstacle = ob; pictureIs = 3; break; }
                    }

                
                

               

                tableau.Controls.Remove(p);
                tableau.Refresh();


                if (pictureIs == 1) {
                    colorGenerator.removeColor(acces.couluer);
                    accesPoints.Remove(acces);
                   


                } else if (pictureIs == 2) {

                    if (station.dialogStat != null)
                    station.dialogStat.Dispose();
                   

                 
                    stations.Remove(station);
                    tableau.Refresh();


                } else if (pictureIs == 3) {
                    obstacles.Remove(obstacle);
                    foreach(AccesPoint ac in accesPoints)
                    {

                  
                        ac.couverture.obstacles.Remove(obstacle);
                        ac.couverture.updateObstacle();
                        tableau.Refresh();

                    }
                }


                foreach (Station s in stations)
                {
                    s.connections.Clear();
                    connectStations();
                }
              
            }
            }




        /**********************************La Barre de Menu*****************************************/



        //ajout Acces Point
        TextBox[] elements = new TextBox[4]; Form dialogSender = null;

        private void annuler_click(object sender, EventArgs e)
        {

            if (elements[0].Text.Equals("")&& elements[1].Text.Equals("") && elements[2].Text.Equals("") && elements[3].Text.Equals("") ) {

                dialogSender.Dispose();

            }
            else {

                foreach (TextBox t in elements)
                {
                    t.Text = "";
                }

            }

        }



        private void valider_click(object sender, EventArgs e) {
            float f = 0;int i = 0;
            if (elements[0].Text.Equals("") || elements[1].Text.Equals("") || elements[2].Text.Equals("") || elements[3].Text.Equals("") || !int.TryParse(elements[1].Text, out i) || !int.TryParse(elements[2].Text, out i) || !float.TryParse(elements[3].Text, out f))
            {

                MessageBox.Show("Le nom ne doit pas étre vide\nX est un entier qui appartient à [0," + tableau.Width + "]\nY est un entier qui appartient à [0," + tableau.Height + "]\nLa puissance est un nombre positive", "Vérifiez les champs Svp !", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else {


                SizeablePictureBox picture = new SizeablePictureBox();
                picture.Size = new Size(35,35);
                picture.Image = Properties.Resources.wireless_router;
                picture.Location = new Point(int.Parse(elements[1].Text), int.Parse(elements[2].Text));
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BackColor = Color.Transparent;


           
                    picture.grab = 2;

                

                tableau.Controls.Add(picture);

                picture.Click += deleteElement;
                picture.MouseDown += picture_MouseDown;
                picture.MouseMove += picture_MouseMove;
                picture.MouseUp += picture_MouseUp;


                AccesPoint ap = new AccesPoint(elements[0].Text, float.Parse(elements[3].Text), picture, colorGenerator.getColor());
                accesPoints.Add(ap);
                tableau.Refresh();


                dialogSender.Dispose();


            }



        }

        private void ajouterPointDaccésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dialog = new Form();
            dialog.Text = "Nouveau point d'accés";
            dialog.Icon = Icon.FromHandle(Properties.Resources.wireless_router.GetHicon());
            dialog.StartPosition = this.StartPosition;
            dialog.MaximizeBox = false;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            Bitmap background = new Bitmap(Properties.Resources.wireless_background);
            dialog.BackgroundImage = background;
            dialog.Size = new Size(400,300);


            Label nameLabel = new Label();
            nameLabel.Text = "Nom :";
            nameLabel.BackColor = Color.Transparent;
            nameLabel.Height = 12;
            nameLabel.Width = 50;
            nameLabel.Location = new Point(50, 43);

            TextBox name = new TextBox();
            name.Width = 150;
             name.Location = new Point(dialog.Width/2-name.Width/2,40);



            Label xLabel = new Label();
            xLabel.Text = "Position X : ";
            xLabel.BackColor = Color.Transparent;
            xLabel.Height = 12;
            xLabel.Width = 60;
            xLabel.Location = new Point(50,80);

            TextBox x = new TextBox();
            x.Width = 30;
            x.Location = new Point(dialog.Width / 2 - name.Width / 2, 76);



            Label yLabel = new Label();
            yLabel.Text = "Position Y : ";
            yLabel.BackColor = Color.Transparent;
            yLabel.Height = 12;
            yLabel.Width = 60;
            yLabel.Location = new Point(50, 120);

            TextBox y = new TextBox();
            y.Width = 30;
            y.Location = new Point(dialog.Width / 2 - name.Width / 2, 116);



            Label puissanceLabel = new Label();
            puissanceLabel.Text = "Puissance : ";
            puissanceLabel.BackColor = Color.Transparent;
            puissanceLabel.Height = 12;
            puissanceLabel.Width = 65;
            puissanceLabel.Location = new Point(50, 160);

            TextBox puissance = new TextBox();
            puissance.Width = 150;
            puissance.Location = new Point(dialog.Width / 2 - name.Width / 2,156);


            Button valider = new Button();
            valider.Text = "Valider";
            valider.Size = new Size(80, 30);
            valider.Location = new Point(110, dialog.Height - 85);
            


            Button annuler = new Button();
            annuler.Text = "Annuler";
            annuler.Size = new Size(80, 30);
            annuler.Location = new Point(210,dialog.Height - 85);


            elements[0] = name;
            elements[1] = x;
            elements[2] = y;
            elements[3] = puissance;
            dialogSender = dialog;



            annuler.Click += annuler_click;
            valider.Click += valider_click;
        




        dialog.Controls.Add(nameLabel);
            dialog.Controls.Add(name);
            dialog.Controls.Add(xLabel);
            dialog.Controls.Add(x);
            dialog.Controls.Add(yLabel);
            dialog.Controls.Add(y);
            dialog.Controls.Add(puissanceLabel);
            dialog.Controls.Add(puissance);
            dialog.Controls.Add(valider);
            dialog.Controls.Add(annuler);

            dialog.Show();
        }



        //Ajout Station
        private void stationAnnuler_click(object sender, EventArgs e)
        {

            if (elements[0].Text.Equals("") && elements[1].Text.Equals("") && elements[2].Text.Equals(""))
            {

                dialogSender.Dispose();

            }
            else
            {

                foreach (TextBox t in elements)
                {
                    if(t != null)
                    t.Text = "";
                }

            }

        }

        private void stationValider_click(object sender, EventArgs e)
        {
             int i = 0;
            if (elements[0].Text.Equals("") || elements[1].Text.Equals("") || elements[2].Text.Equals("") || !int.TryParse(elements[1].Text, out i) || !int.TryParse(elements[2].Text, out i))
            {

                MessageBox.Show("Le nom ne doit pas étre vide\nX est un entier qui appartient à [0," + tableau.Width + "]\nY est un entier qui appartient à [0," + tableau.Height+"]", "Vérifiez les champs Svp !", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {


                SizeablePictureBox picture = new SizeablePictureBox();
                picture.Size = new Size(35, 35);
                picture.Image = Properties.Resources.computer;
                picture.Location = new Point(int.Parse(elements[1].Text), int.Parse(elements[2].Text));
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BackColor = Color.Transparent;
                picture.grab = 2;
                tableau.Controls.Add(picture);

                picture.Click += deleteElement;
                picture.MouseDown += picture_MouseDown;
                picture.MouseMove += picture_MouseMove;
                picture.MouseUp += picture_MouseUp;
                picture.DoubleClick += doubleClickStations;

                Station station = new Station(elements[0].Text, picture);
                stations.Add(station);

                if (station != null)
                    station.connections = new List<ConnectionAccesPointStation>();


                connectStations();


                tableau.Refresh();


                dialogSender.Dispose();


            }



        }
        private void ajouterStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elements = new TextBox[4];
            Form dialog = new Form();
            dialog.Text = "Nouvelle station";
            dialog.Icon = Icon.FromHandle(Properties.Resources.computer.GetHicon());
            dialog.StartPosition = this.StartPosition;
            dialog.MaximizeBox = false;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            Bitmap background = new Bitmap(Properties.Resources.computer_background);
            dialog.BackgroundImage = background;
            dialog.Size = new Size(400, 300);


            Label nameLabel = new Label();
            nameLabel.Text = "Nom :";
            nameLabel.BackColor = Color.Transparent;
            nameLabel.Height = 12;
            nameLabel.Width = 50;
            nameLabel.Location = new Point(50, 43);

            TextBox name = new TextBox();
            name.Width = 150;
            name.Location = new Point(dialog.Width / 2 - name.Width / 2, 40);

            Label xLabel = new Label();
            xLabel.Text = "Position X : ";
            xLabel.BackColor = Color.Transparent;
            xLabel.Height = 12;
            xLabel.Width = 60;
            xLabel.Location = new Point(50, 80);

            TextBox x = new TextBox();
            x.Width = 30;
            x.Location = new Point(dialog.Width / 2 - name.Width / 2, 76);



            Label yLabel = new Label();
            yLabel.Text = "Position Y : ";
            yLabel.BackColor = Color.Transparent;
            yLabel.Height = 12;
            yLabel.Width = 60;
            yLabel.Location = new Point(50, 120);

            TextBox y = new TextBox();
            y.Width = 30;
            y.Location = new Point(dialog.Width / 2 - name.Width / 2, 116);


            Button valider = new Button();
            valider.Text = "Valider";
            valider.Size = new Size(80, 30);
            valider.Location = new Point(110, dialog.Height - 85);



            Button annuler = new Button();
            annuler.Text = "Annuler";
            annuler.Size = new Size(80, 30);
            annuler.Location = new Point(210, dialog.Height - 85);


            elements[0] = name;
            elements[1] = x;
            elements[2] = y;
            dialogSender = dialog;

            annuler.Click += stationAnnuler_click;
            valider.Click += stationValider_click;

            dialog.Controls.Add(nameLabel);
            dialog.Controls.Add(name);
            dialog.Controls.Add(xLabel);
            dialog.Controls.Add(x);
            dialog.Controls.Add(yLabel);
            dialog.Controls.Add(y);
            dialog.Controls.Add(valider);
            dialog.Controls.Add(annuler);
            dialog.Show();



        }


        //enregistrer Button
        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //mise a jour taille obstacles
            foreach (Obstacle o in obstacles)
                o.taille = o.picture.Size;

            Saver saver = new Saver(accesPoints,stations,obstacles,colorGenerator,nbrAccesPoint,nbrStation,defaultPuissance,backTableau);
            saver.save();
            String file = saver.file;
            String fileName = file.Substring(file.LastIndexOf('\\') + 1);
            if (fileName.Length>0) {
                String fileNameWithoutExtention = fileName.Substring(0, fileName.IndexOf('.'));
                this.Text = fileNameWithoutExtention + "   |  Planificateur Réseaux";
            }

        }


        //ouvrir Button
        private void reglerImage(ref SizeablePictureBox image) {
            image = new SizeablePictureBox();
            image.Size = new Size(35, 35);
            image.SizeMode = PictureBoxSizeMode.StretchImage;
            image.BackColor = Color.Transparent;
            image.Click += deleteElement;
            image.MouseDown += picture_MouseDown;
            image.MouseMove += picture_MouseMove;
            image.MouseUp += picture_MouseUp;
        }
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Saver saver = new Saver();
            saver.load();

            tableau.Controls.Clear();
            accesPoints.Clear();stations.Clear();obstacles.Clear();

            accesPoints = saver.listeAccesPoints;
            stations = saver.listeStations;
            obstacles = saver.listeObstacles;
            colorGenerator = saver.generator;
            nbrAccesPoint = saver.nbrAccesPoint;
            nbrStation = saver.nbrStation;
            backTableau = saver.backTableau;
            defaultPuissance = saver.defaultPuissance;

            tableau.BackColor = backTableau;

            foreach (AccesPoint ac in accesPoints)
            {
                reglerImage(ref ac.image);
                ac.image.Image = Properties.Resources.wireless_router;
                ac.image.Location = new Point(ac.centre.X - ac.image.Size.Width / 2, ac.centre.Y - ac.image.Size.Height / 2);
                ac.image.grab = 2;
                tableau.Controls.Add(ac.image);
            }

            foreach (Station st in stations) {
                reglerImage(ref st.image);
                st.image.Image = Properties.Resources.computer;
                st.image.Location = st.position;
                st.image.grab = 2;
                st.image.DoubleClick += doubleClickStations;
                tableau.Controls.Add(st.image);

            }

            foreach (Obstacle ob in obstacles) {
                reglerImage(ref ob.picture);
                ob.picture.Image = Obstacle.getImageFromId(ob.id);
                ob.picture.Location = ob.position;
                ob.picture.Size = ob.taille;
                tableau.Controls.Add(ob.picture);
            }

            foreach (AccesPoint ac in accesPoints)
            {


                foreach (Obstacle obCouverture in ac.couverture.obstacles)
                {


                    foreach (Obstacle ob in obstacles)
                    {
                        if (ob.id == obCouverture.id && ob.taille == obCouverture.taille && ob.position == obCouverture.position)
                        {
                            obCouverture.picture = ob.picture;
                            break;

                        }

                    }



                }


                ac.couverture.updateObstacle();

            }


            tableau.Refresh();

            if (!saver.file.Equals("")) {
                String file = saver.file;
                String fileName = file.Substring(file.LastIndexOf('\\') + 1);
                String fileNameWithoutExtention = fileName.Substring(0, fileName.IndexOf('.'));
                this.Text = fileNameWithoutExtention + "   |  Planificateur Réseaux";
            }

        }
        //Nouveau Document
      
        private void nouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form1().Show();
        }



        //Choix liste Obstacles
        private void listeObstacleSelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBox liste = (ComboBox) sender;
            switch (liste.SelectedIndex)
            {
                case 0:
                  o1PictureBox.Image = Properties.Resources.gimp_sap1;
                    o2PictureBox.Image = Properties.Resources.Door_smooth_3panels_red01__CC_BY_NC_SA_by_thedarkmod_com_;
                    break;

                case 1:
                    o1PictureBox.Image = Properties.Resources.goblet;
                 o2PictureBox.Image = Properties.Resources.sachet;
                    break;

                case 2:
                    o1PictureBox.Image = Properties.Resources.verre;
                    o2PictureBox.Image = Properties.Resources.fenetre;
                    break;

                case 3:
                    o1PictureBox.Image = Properties.Resources.verre_vert;
                    o2PictureBox.Image = Properties.Resources.vase;
                    break;

                case 4:
                    o1PictureBox.Image = Properties.Resources.bouteille_eau;
                    o2PictureBox.Image = Properties.Resources.piscine;
                    break;

                case 5:
                    o1PictureBox.Image = Properties.Resources.man;
                    o2PictureBox.Image = Properties.Resources.femme;
                    break;

                case 6:
                    o1PictureBox.Image = Properties.Resources.mur;
                    o2PictureBox.Image = Properties.Resources.cheminer;
                    break;
                case 7:
                    o1PictureBox.Image = Properties.Resources.mur_platre;
                    o2PictureBox.Image = Properties.Resources.statue;
                    break;

                case 8:
                    o1PictureBox.Image = Properties.Resources.verre_ceramique;
                    o2PictureBox.Image = Properties.Resources.vase_ceramique;
                    break;

                case 9:
                    o1PictureBox.Image = Properties.Resources.carnet;
                    o2PictureBox.Image = Properties.Resources.papier;
                    break;

                case 10:
                    o1PictureBox.Image = Properties.Resources.mur_beton;
                    o2PictureBox.Image = Properties.Resources.cheminer_beton;
                    break;

                case 11:
                    o1PictureBox.Image = Properties.Resources.plaque_verre_blinde;
                    o2PictureBox.Image = Properties.Resources.fenetre_verre_blinde;
                    break;

                case 12:
                    o1PictureBox.Image = Properties.Resources.porte_metal;
                    o2PictureBox.Image = Properties.Resources.robot;
                    break;

               
            }
        }





        //les obstacles Menu

        private int idObstacle = -1;
        private TextBox[] xy = new TextBox[2];
        private Form dialogObstacle = null;


        private void annulerObstacleDialogClick(Object sender,EventArgs e) {

            if (!xy[0].Text.Equals("") || !xy[1].Text.Equals("")) {
                xy[0].Text = ""; xy[1].Text = "";
            }
            else
            {
                dialogObstacle.Dispose();
            }

        }



        private void validerObstacleDialogClick(Object sender,EventArgs e) {

            int i = -1;
            if (!xy[0].Text.Equals("") && !xy[1].Text.Equals("") && int.TryParse(xy[0].Text,out i) && int.TryParse(xy[1].Text,out i)) {
                int x = int.Parse(xy[0].Text);
                int y = int.Parse(xy[1].Text);
                SizeablePictureBox picture = new SizeablePictureBox();
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BackColor = Color.Transparent;
                picture.Size = new Size(35, 35);
                picture.Image = (Image)dialogObstacle.Tag;
                picture.Location = new Point(x, y);

                picture.MouseMove += picture_MouseMove;
                picture.MouseDown += picture_MouseDown;
                picture.MouseUp += picture_MouseUp;
                picture.Click += deleteElement;

             
                Obstacle obstacle = new Obstacle(idObstacle,picture);
                obstacles.Add(obstacle);



                tableau.Controls.Add(picture);

                tableau.Refresh();
                dialogObstacle.Dispose();
            }else
            {
              
                MessageBox.Show("X est un entier qui appartient à[0, " + tableau.Width + "]\nY est un entier qui appartient à[0, " + tableau.Height+"]", "Vérifiez les champs Svp !",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        private void choixObstacleInfo(int idObstacle) {

            PictureBox picture = new PictureBox();

            picture.Image = Obstacle.getImageFromId(idObstacle);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.Size = new Size(80, 80);

            Point obstaclePosition = new Point(-1, -1);

            Form dialog = new Form();
            dialog.Text = " Nouveau obstacle";
            dialog.Icon = Icon.FromHandle(new Bitmap(picture.Image).GetHicon());
            dialog.StartPosition = this.StartPosition;
            dialog.MaximizeBox = false;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.Size = new Size(400,220);
            dialog.BackColor = Color.Gray;


            picture.Location = new Point(30,dialog.Height/2 - picture.Height +10);

            Label xLabel = new Label();
            xLabel.Text = "Position X  : ";
            xLabel.ForeColor = Color.White;
            xLabel.Location = new Point(170,55);
            xLabel.Width = 65;
            xLabel.Height = 12;


            TextBox x = new TextBox();
            x.Width = 40;
            x.Location = new Point(250,52);



            Label yLabel = new Label();
            yLabel.Text = "Position Y  : ";
            yLabel.ForeColor = Color.White;
            yLabel.Location = new Point(170, 85);
            yLabel.Width = 65;
            yLabel.Height = 12;


            TextBox y = new TextBox();
            y.Width = 40;
            y.Location = new Point(250, 82);


            xy[0] = x;
            xy[1] = y;
            dialogObstacle = dialog;
            dialogObstacle.Tag = picture.Image;


            Button valider = new Button();
            valider.Text = "Valider";
            valider.ForeColor = Color.White;
            valider.Size = new Size(80, 30);
            valider.Location = new Point(110, dialog.Height - 85);



            Button annuler = new Button();
            annuler.Text = "Annuler";
            annuler.ForeColor = Color.White;
            annuler.Size = new Size(80, 30);
            annuler.Location = new Point(210, dialog.Height - 85);

            valider.Click += validerObstacleDialogClick;
            annuler.Click += annulerObstacleDialogClick;

            dialog.Controls.Add(xLabel);
            dialog.Controls.Add(x);
            dialog.Controls.Add(yLabel);
            dialog.Controls.Add(y);
            dialog.Controls.Add(valider);
            dialog.Controls.Add(annuler);
            dialog.Controls.Add(picture);
            dialog.Show();
        }

        private void arbreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 1;
            choixObstacleInfo(idObstacle); 

             }

        private void porteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 2;
            choixObstacleInfo(idObstacle);
        }

        private void gobletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 3;
            choixObstacleInfo(idObstacle);
        }

        private void sachetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 4;
            choixObstacleInfo(idObstacle);
        }

        private void verreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 5;
            choixObstacleInfo(idObstacle);
        }

        private void fenetreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 6;
            choixObstacleInfo(idObstacle);
        }

        private void verreVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 7;
            choixObstacleInfo(idObstacle);
        }



        private void vaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 8;
            choixObstacleInfo(idObstacle);
        }

        private void bouteilleDeauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 9;
            choixObstacleInfo(idObstacle);
        }

        private void piscineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 10;
            choixObstacleInfo(idObstacle);
        }

        private void hommeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 11;
            choixObstacleInfo(idObstacle);
        }

        private void femmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 12;
            choixObstacleInfo(idObstacle);
        }

        private void murToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 13;
            choixObstacleInfo(idObstacle);
        }

        private void cheminerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 14;
            choixObstacleInfo(idObstacle);
        }

        private void murToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 15;
            choixObstacleInfo(idObstacle);
        }

        private void statueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 16;
            choixObstacleInfo(idObstacle);
        }

        private void verreToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            idObstacle = 17;
            choixObstacleInfo(idObstacle);
        }

        private void vaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 18;
            choixObstacleInfo(idObstacle);
        }

        private void carnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 19;
            choixObstacleInfo(idObstacle);
        }

        private void feuillePapierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 20;
            choixObstacleInfo(idObstacle);
        }

        private void murToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            idObstacle = 21;
            choixObstacleInfo(idObstacle);
        }

        private void cheminerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 22;
            choixObstacleInfo(idObstacle);
        }

        private void plaqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 23;
            choixObstacleInfo(idObstacle);
        }

        private void fenetreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 24;
            choixObstacleInfo(idObstacle);
        }

        private void porteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            idObstacle = 25;
            choixObstacleInfo(idObstacle);
        }

        private void robotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            idObstacle = 26;
            choixObstacleInfo(idObstacle);
        }


        //Paramétres

        /*------ Choix Couleur -------*/
        private TextBox puissanceParametre = null;
        private Color couleurParametre = Color.White;
        private Form dialogParametre = null;
        private void choisirCouleur(Object sender,EventArgs e) {
            
            ColorDialog dialogColor = new ColorDialog();
            dialogColor.AllowFullOpen = true;
            dialogColor.Color = Color.White;

            if (dialogColor.ShowDialog() == DialogResult.OK) {

                
                Button b = (Button)sender;

                b.BackColor = dialogColor.Color;
                couleurParametre = dialogColor.Color;

               

            }

        }


        private void annulerParametre(Object sender,EventArgs e) {

            if (!puissanceParametre.Text.Equals(""))
                puissanceParametre.Text = "";
            else
                dialogParametre.Dispose();

        }


        private void validerParametre(Object sender,EventArgs e) {
            float i = -1;
            if (!puissanceParametre.Text.Equals("") && float.TryParse(puissanceParametre.Text, out i))
            {

                defaultPuissance = float.Parse(puissanceParametre.Text);

                backTableau = couleurParametre;
                couleurTool.BackColor = couleurParametre;
                tableau.BackColor = backTableau;
                dialogParametre.Dispose();

            }
            else {

                MessageBox.Show("la puissance est un nombre positive", "Vérifiez les champs Svp !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void paramétresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dialog = new Form();
            dialog.Text = " Paramétres";
            dialog.Icon = Icon.FromHandle(new Bitmap(Properties.Resources._19674).GetHicon());
            dialog.StartPosition = this.StartPosition;
            dialog.MaximizeBox = false;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.Size = new Size(400,300);


            Label puissanceLabel = new Label();
            puissanceLabel.Text = "Puissance des points d'accés par défaut : ";
            puissanceLabel.Height = 12;
            puissanceLabel.Width = 220;
            puissanceLabel.Location = new Point(20,40);


            TextBox puissance = new TextBox();
            puissance.Width = 40;
            puissance.Location = new Point(260,38);
            puissance.Text = defaultPuissance + "";




            Label backLabel = new Label();
            backLabel.Text = "Arriére Plan par défaut : ";
            backLabel.Height = 12;
            backLabel.Width = 150;
            backLabel.Location = new Point(20, 120);


            Button couleur = new Button();
            couleur.Text = "";
            couleur.Location = new Point(200,110);
            couleur.BackColor = backTableau;
            couleur.Width = 120;couleur.Height = 40;




            Button valider = new Button();
            valider.Text = "Valider";
            valider.Size = new Size(80, 30);
            valider.Location = new Point(110, dialog.Height - 85);



            Button annuler = new Button();
            annuler.Text = "Annuler";
            annuler.Size = new Size(80, 30);
            annuler.Location = new Point(210, dialog.Height - 85);


            puissanceParametre = puissance;
            dialogParametre = dialog;
            couleur.Click += choisirCouleur;
            annuler.Click += annulerParametre;
            valider.Click += validerParametre;
            

            dialog.Controls.Add(puissanceLabel);
            dialog.Controls.Add(puissance);
            dialog.Controls.Add(backLabel);
            dialog.Controls.Add(couleur);
            dialog.Controls.Add(valider);
            dialog.Controls.Add(annuler);
            dialog.Show();

        }



        //calcul distance
        private float distance(Point a, Point b)
        { return (float)(Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y))); }









        ///////// affichage des cnx
        private TableLayoutPanel tableLayoutPanel = null;
        private Station StationSender = null; 

        private void doubleClickStations(object sender, EventArgs e)
        {

            PictureBox picture = sender as PictureBox;
            Station station = null;
            foreach (Station s in stations)
            {
                if (s.image.Equals(picture))
                {
                    station = s; break;
                }
            }

            Form dialog = new Form();
            dialog.Text = " "+station.name;
            dialog.Icon = Icon.FromHandle(new Bitmap(Properties.Resources.wifi).GetHicon());
            dialog.StartPosition = this.StartPosition;
            dialog.MaximizeBox = false;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.Size = new Size(370, 315);
            dialog.TopMost = true;


            station.dialogStat = dialog;


            Panel panelTb0 = new Panel();
            panelTb0.Size = new Size(dialog.Width - 40, 20);
            panelTb0.Location = new Point(10, 20);


            TableLayoutPanel tlp0 = new TableLayoutPanel();

            tlp0.Size = panelTb0.Size;
            tlp0.Location = new Point(0, 0);
            tlp0.AutoScroll = true;
            tlp0.AutoSize = true;
            tlp0.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlp0.RowCount = 1;
            tlp0.ColumnCount = 3;
            tlp0.Dock = DockStyle.Top;
            tlp0.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;


            tlp0.Controls.Add(new Label() { Text = "Point d'accés" , ForeColor = Color.White}, 0,0);
            tlp0.Controls.Add(new Label() { Text = "Signal", ForeColor = Color.White }, 1, 0);
            tlp0.Controls.Add(new Label() { Text = "Etat", ForeColor = Color.White }, 2, 0);
            tlp0.BackColor = Color.Black;

            Panel panelTb = new Panel();
            panelTb.Size = new Size(dialog.Width-28,180);
            panelTb.Location = new Point(10,50);
            panelTb.BackColor = Color.White;


            TableLayoutPanel tlp = new TableLayoutPanel();

            tlp.Size = panelTb.Size;
            tlp.Location = new Point(0,0);
            tlp.AutoScroll = true;
            tlp.AutoSize = false;
            tlp.RowCount = 0;
            tlp.ColumnCount = 3;
            tlp.Dock = DockStyle.Fill;
            tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;






            foreach (ConnectionAccesPointStation cnx in station.connections)
            {
               


                tlp.RowCount++;
                tlp.Controls.Add(new Label() { Text = cnx.accesPoint.nom},0,tlp.RowCount);
                tlp.Controls.Add(new Label() { Text = cnx.getQualityConnection().ToString("0.00")+" %" }, 1, tlp.RowCount);

                if(cnx.getQualityConnection() == station.maxQuality(station.connections))
                tlp.Controls.Add(new Label() { Text = "connécté",ForeColor = Color.Green}, 2, tlp.RowCount);
                else
                    tlp.Controls.Add(new Label() { Text = "non connécté", ForeColor = Color.Red }, 2, tlp.RowCount);


            }



            Button actualiser = new Button();
            actualiser.Location = new Point(157, dialog.Height - 75);
            actualiser.Size = new Size(50,30);
            actualiser.BackColor = Color.White;
            actualiser.BackgroundImage = Properties.Resources.act11;
            actualiser.BackgroundImageLayout = ImageLayout.Center;
           

            StationSender = station;
            tableLayoutPanel = tlp;

            actualiser.Click += actualiserCnxStationClick;

            panelTb0.Controls.Add(tlp0);
            panelTb.Controls.Add(tlp);

            dialog.Controls.Add(panelTb0);
            dialog.Controls.Add(panelTb);
            dialog.Controls.Add(actualiser);
            dialog.Show();

        }

     
        private void actualiserCnxStationClick(Object sender,EventArgs e)
        {

           

            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.RowCount = 0;



            List<String[]> l1 = new List<string[]>();
            List<String[]> l2 = new List<string[]>();
            int n = 0;

            foreach (ConnectionAccesPointStation cnx in StationSender.connections)
            {


               String s1 = cnx.accesPoint.nom;
                String s2 = cnx.getQualityConnection().ToString("0.00") + " %";

                String s3 = "";
                if (cnx.getQualityConnection() == StationSender.maxQuality(StationSender.connections))
                    s3 = "connécté";
                else
                    s3 = "non connécté";


                l1.Add(new string[] {s1,s2,s3 });

                  
            }


            

            foreach(String[] s in l1)
            {
                if (!existeInList(l2, s))
                    l2.Add(s);
            }


            foreach (String[] s in l1)
            {
                Debug.Write(s[0]+"/"+s[1]+"/"+s[2]+" | ");
            }
            Debug.WriteLine("");
            foreach (String[] s in l1)
            {
                Debug.Write(s[0] + "/" + s[1] + "/" + s[2] + " | ");
            }



            foreach (String[] s in l2) {


                tableLayoutPanel.RowCount++;
    
                tableLayoutPanel.Controls.Add(new Label() { Text = l2.ElementAt(tableLayoutPanel.RowCount-1)[0] }, 0, tableLayoutPanel.RowCount-1);
                tableLayoutPanel.Controls.Add(new Label() { Text = l2.ElementAt(tableLayoutPanel.RowCount-1)[1] }, 1, tableLayoutPanel.RowCount-1);

                if (l2.ElementAt(tableLayoutPanel.RowCount-1)[2].Equals("connécté"))
                    tableLayoutPanel.Controls.Add(new Label() { Text = "connécté", ForeColor = Color.Green }, 2, tableLayoutPanel.RowCount-1);
                else
                    tableLayoutPanel.Controls.Add(new Label() { Text = "non connécté", ForeColor = Color.Red }, 2, tableLayoutPanel.RowCount-1);
            }


           

           
           

        }

        private bool existeInList(List<String[]> liste,String[] tab)
        {
            bool ok = false;

            foreach (String[] s in liste)
            {

                if (tab[0].Equals(s[0]))
                {
                    ok = true;break;
                }

            }
            

            return ok;
        }



        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = backTableau;

            if(dialog.ShowDialog() == DialogResult.OK)
            {

                backTableau = dialog.Color;
                couleurTool.BackColor = dialog.Color;
                tableau.BackColor = backTableau;

            }


        }




    }


}


