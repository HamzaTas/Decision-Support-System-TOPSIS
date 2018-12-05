using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decision_Support_System___Topsis.App_Classes
{
    public class Topsis
    {
        //standart karar matrisi
        public double[,] StandartKararMatris;

        public void StandartKararMatrisiHazirla(double[,] matris)
        {
            StandartKararMatris = new double[4, 7];
            //her sütun için
            double r = 0;
            for (int j = 0; j < 7; j++) // her sütun için normalize hesabı yapılır
            {
                r = 0;
                for (int i = 0; i < 4; i++) // sıradaki sütun için satırlarda gezip R bulduk
                {
                    r += Math.Pow(matris[i, j], 2);
                }
                r = Math.Sqrt(r);
                for (int i = 0; i < 4; i++) // sıradaki sütun için satırlarda gezip Aij / R yaptık
                {
                    StandartKararMatris[i, j] = matris[i, j] / r;
                }
            }
        }

        public double[,] StandartKararMatrisiGoster()
        {
            return StandartKararMatris;
        }

        //Agirlikli standart karar matrisi
        public double[,] agirlikliStandartKararMatrisi;

        public void agirlikliStandartKararMatrisiHazirla(double[] agirliklar)
        {
            agirlikliStandartKararMatrisi = new double[4, 7];
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    agirlikliStandartKararMatrisi[j, i] = StandartKararMatris[j, i] * agirliklar[i];
                }
            }

        }

        public double[,] agirlikliStandartKararMatrisiGoster()
        {
            return agirlikliStandartKararMatrisi;
        }

        //ideal çözüm ve negatif ideal çözüm değerleri(min ve max değerlerin bulunması)
        public double[] IdealcozumDegerleri;
        public double[] negatifİdealcozumDegerleri;

        public void idealVeNegatifİdealCozumDegerleriHesapla()
        {
            IdealcozumDegerleri = new double[7];
            negatifİdealcozumDegerleri = new double[7];
            double max, min;
            for (int j = 0; j < 7; j++) // sırayla tüm sütunlar döner
            {
                max = min = agirlikliStandartKararMatrisi[0, j];
                if (j == 3 || j == 4) // Kira ve Çevredeki Market Sayıları SÜTUNLARI için ters değerleri alacağız. çünkü bunların az olmasını istiyoruz
                {
                    for (int i = 0; i < 4; i++) // her sütun için satırlar arasında , min ve max bulma
                    {
                        if (agirlikliStandartKararMatrisi[i, j] > max)
                            max = agirlikliStandartKararMatrisi[i, j];
                        if (agirlikliStandartKararMatrisi[i, j] < min)
                            min = agirlikliStandartKararMatrisi[i, j];
                        IdealcozumDegerleri[j] = min;
                        negatifİdealcozumDegerleri[j] = max;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++) // her sütun için satırlar arasında , min ve max bulma
                    {
                        if (agirlikliStandartKararMatrisi[i, j] > max)
                            max = agirlikliStandartKararMatrisi[i, j];
                        if (agirlikliStandartKararMatrisi[i, j] < min)
                            min = agirlikliStandartKararMatrisi[i, j];
                        IdealcozumDegerleri[j] = max;
                        negatifİdealcozumDegerleri[j] = min;
                    }
                }
            }


        }

        public double[] idealCozumDegerleriGoster()
        {
            return IdealcozumDegerleri;
        }

        public double[] negatifİdealcozumDegerleriGoster()
        {
            return negatifİdealcozumDegerleri;
        }

        // ideal ve negatif ideal noktalara olan uzaklık - ideal ayrım
        public double[] idealUzaklik;
        public double[] negatifİdealUzaklik;

        public void uzakliklarinHesabi()
        {
            idealUzaklik = new double[4]; // her satır için yaptığımız için satır kadar uzunlukta olacak
            negatifİdealUzaklik = new double[4];
            double sayi = 0;

            for (int j = 0; j < 4; j++)
            {
                //pozitif
                sayi = 0;
                for (int i = 0; i < 7; i++)
                {
                    sayi += Math.Pow(agirlikliStandartKararMatrisi[j, i] - IdealcozumDegerleri[i], 2);
                }
                sayi = Math.Sqrt(sayi);
                idealUzaklik[j] = sayi;
                //negatif
                sayi = 0;
                for (int i = 0; i < 5; i++)
                {
                    sayi += Math.Pow(agirlikliStandartKararMatrisi[j, i] - negatifİdealcozumDegerleri[i], 2);
                }
                sayi = Math.Sqrt(sayi);
                negatifİdealUzaklik[j] = sayi;
            }

        }

        public double[] idealUzaklikGoster()
        {
            return idealUzaklik;
        }

        public double[] negatifİdealUzaklikGoster()
        {
            return negatifİdealUzaklik;

        }

        // İdeal çözüme gerekli yakınlık değeri
        public double[] idealCozumMatrisi = { 0, 0, 0, 0 };
        public double[] idealCozumSirali;
        public double idealCozum;

        public void idealCozumHesabi()
        {
            for (int i = 0; i < 4; i++)
            {
                idealCozumMatrisi[i] = negatifİdealUzaklik[i] / (negatifİdealUzaklik[i] + idealUzaklik[i]);
             }
        }

        public double s1;
        public double s2;
        public double s3;
        public double s4;
        public double[] idealCozumSiralaVeGoster()
        {
            s1 = idealCozumMatrisi[0];
            s2 = idealCozumMatrisi[1];
            s3 = idealCozumMatrisi[2];
            s4 = idealCozumMatrisi[3];
            double gecici;
            for (int i = 0; i < idealCozumMatrisi.Length - 1; i++)
            {
                for (int x = i + 1; x < idealCozumMatrisi.Length; x++)
                {
                    if (idealCozumMatrisi[i] > idealCozumMatrisi[x])
                    {
                        gecici = idealCozumMatrisi[x];
                        idealCozumMatrisi[x] = idealCozumMatrisi[i];
                        idealCozumMatrisi[i] = gecici;
                    }
                }
            }
            idealCozumSirali = idealCozumMatrisi;
            idealCozum = idealCozumSirali[3];
            return idealCozumSirali;
        }



    }
}