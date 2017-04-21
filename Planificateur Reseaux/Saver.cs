using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Planificateur_Reseaux
{

    [Serializable()]
    class Saver
    {
        public List<AccesPoint> listeAccesPoints = new List<AccesPoint>();
        public List<Station> listeStations = new List<Station>();
        public List<Obstacle> listeObstacles = new List<Obstacle>();
        public GenerateColor generator = null;
        public int nbrAccesPoint = 0;
        public int nbrStation = 0;
        public float defaultPuissance = -1;
        public Color backTableau = Color.White;
        public String file = "";

        public Saver() { }


        public Saver(List<AccesPoint> listeAccesPoints, List<Station> listeStations, List<Obstacle> listeObstacles,GenerateColor generator,int nbrAccesPoint,int nbrStation,float defaultPuissance,Color backTableau)
        {

            this.listeAccesPoints = listeAccesPoints;
            this.listeObstacles = listeObstacles;
            this.listeStations = listeStations;
            this.generator = generator;
            this.nbrAccesPoint = nbrAccesPoint;
            this.nbrStation = nbrStation;
            this.defaultPuissance = defaultPuissance;
            this.backTableau = backTableau;

        }






        public  void save()
        {

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "RT Files | *.rt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                BinaryFormatter bf = new BinaryFormatter();
                String fileName = dialog.FileName;

                this.file = fileName;
                File.Delete(dialog.FileName);
                Stream stream = File.Open(dialog.FileName, FileMode.CreateNew);
                bf.Serialize(stream,this);
                stream.Close();
            }



        }



        public void load()
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RT Files | *.rt";

            List<Object> loadObjects = new List<object>();

            if (ofd.ShowDialog() == DialogResult.OK)
            {


                Stream stream = File.Open(ofd.FileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Saver saver = (Saver)bf.Deserialize(stream);

                this.listeAccesPoints = saver.listeAccesPoints;
                this.listeStations = saver.listeStations;
                this.listeObstacles = saver.listeObstacles;
                this.generator = saver.generator;
                this.nbrAccesPoint = saver.nbrAccesPoint;
                this.nbrStation = saver.nbrStation;
                this.backTableau = saver.backTableau;
                this.defaultPuissance = saver.defaultPuissance;


                this.file = ofd.FileName;
                stream.Close();

            }



        }










    }
}
