using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planificateur_Reseaux
{

    [System.Serializable()]
    public class ConnectionAccesPointStation
    {

        public AccesPoint accesPoint;
        public float distance;
        public float pourcentage;

        public ConnectionAccesPointStation(AccesPoint accesPoint, float distance,float pourcentage)
        {
            this.accesPoint = accesPoint;
            this.distance = distance;
            this.pourcentage = pourcentage;
        }




        public float getQualityConnection() {

            float quality = 100;
            float rayon = accesPoint.rayon;
            int attenuation = attenuationSignal();
          

            if (distance <= rayon/10) {

                //qualité 100%

                float min = 0;
                float max = rayon / 10;

                quality = (10 - ((distance - min) / (max - min)) * 10)+90;



            }
            else if (distance <= (rayon/10)*2)
            {
                //qualité 90%

                float min = rayon/10;
                float max = (rayon / 10)*2;

                quality = (10 - ((distance - min) / (max - min)) * 10)+80;

            }
            else if (distance <= (rayon / 10) * 3)
            {

                //qualité 80%

                float min = (rayon / 10) * 2;
                float max = (rayon / 10) * 3;

                quality = (10 - ((distance - min) / (max - min)) * 10)+70;


            }
            else if (distance <= (rayon / 10) * 4)
            {

                //qualité 70%

                float min = (rayon / 10) * 3;
                float max = (rayon / 10) * 4;
                quality = (10 - ((distance - min) / (max - min)) * 10)+60;


            }
            else if (distance <= (rayon / 10) * 5)
            {

                //qualité 60%

                float min = (rayon / 10) * 4;
                float max = (rayon / 10) * 5;

                quality = (10 - ((distance - min) / (max - min)) * 10)+50;


            }
            else if (distance <= (rayon / 10) * 6)
            {

                //qualité 50%

                float min = (rayon / 10) * 5;
                float max = (rayon / 10) * 6;

                quality = (10 - ((distance - min) / (max - min)) * 10)+40;


            }
            else if (distance <= (rayon / 10) * 7)
            {

                //qualité 40%

                float min = (rayon / 10) * 6;
                float max = (rayon / 10) * 7;

                quality = (10 - ((distance - min) / (max - min)) * 10)+30;


            }
            else if (distance <= (rayon / 10) * 8)
            {

                //qualité 30%

                float min = (rayon / 10) * 7;
                float max = (rayon / 10) * 8;

                quality = (10 - ((distance - min) / (max - min)) * 10)+20;



            }
            else if (distance <= (rayon / 10) * 9)
            {

                //qualité 20%

                float min = (rayon / 10) * 8;
                float max = (rayon / 10) * 9;

                quality = (10 - ((distance - min) / (max - min)) * 10)+10;


            }
            else
            {
                //qualité 10%

                float min = (rayon / 10) * 9;
                float max = rayon;

                quality = 10- ((distance-min) / (max-min))*10;
                
            }

           
            return quality-attenuation;


        }





        private int attenuationSignal()
        {
            int attenuation = 0;

           

            if (pourcentage == (accesPoint.rayon / 10) * 9)
            {
                attenuation = 10;
            }else if (pourcentage == (accesPoint.rayon / 10) * 8)
            {
                
                attenuation = 20;
            }

            else if (pourcentage.ToString("0.00").Equals(((accesPoint.rayon / 10) * 7).ToString("0.00")))
            {
               
                attenuation = 30;
            }
            else if (pourcentage == (accesPoint.rayon / 10) * 6)
            {
                attenuation = 40;
            }
            else if (pourcentage == (accesPoint.rayon / 10) * 5)
            {
                attenuation = 50;
            }
            else if (pourcentage == (accesPoint.rayon / 10) * 4)
            {
                attenuation = 60;
            }
            else if (pourcentage == (accesPoint.rayon / 10) * 3)
            {
                attenuation = 70;
            }
            else if (pourcentage == (accesPoint.rayon / 10) * 2)
            {
                attenuation = 80;

            }
            else if (pourcentage == accesPoint.rayon/10)
            {
                attenuation = 90;
            }
            else
            {
         
                attenuation = 0;
            }


            return attenuation;
        }









    }
}
