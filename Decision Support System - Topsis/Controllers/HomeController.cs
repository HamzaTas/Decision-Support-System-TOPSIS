using Decision_Support_System___Topsis.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Decision_Support_System___Topsis.App_Classes;

namespace Decision_Support_System___Topsis.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(S1 s1,S2 s2 , S3 s3, S4 s4 , Agirlik a)
        {
            //Karar Matrisi
            double[,] KararMatris = new double[,] {
                { s1.S1_K1, s1.S1_K2, s1.S1_K3, s1.S1_K4, s1.S1_K5, s1.S1_K6, s1.S1_K7},
                { s2.S2_K1, s2.S2_K2, s2.S2_K3, s2.S2_K4, s2.S2_K5, s2.S2_K6, s2.S2_K7},
                { s3.S3_K1, s3.S3_K2, s3.S3_K3, s3.S3_K4, s3.S3_K5, s3.S3_K6, s3.S3_K7},
                { s4.S4_K1, s4.S4_K2, s4.S4_K3, s4.S4_K4, s4.S4_K5, s4.S4_K6, s4.S4_K7 },};

            // Seçenekler
            List<string> secenekler = new List<string>();
            secenekler.Add(s1.Sec1);
            secenekler.Add(s2.Sec2);
            secenekler.Add(s3.Sec3);
            secenekler.Add(s4.Sec4);
            TempData["Secenekler"] = secenekler;

            // Ağırlıklar
            double[] Agirliklar = new double[] {
                Convert.ToDouble(a.K1),
                Convert.ToDouble(a.K2),
                Convert.ToDouble(a.K3),
                Convert.ToDouble(a.K4),
                Convert.ToDouble(a.K5),
                Convert.ToDouble(a.K6),
                Convert.ToDouble(a.K7) };

            Topsis topsis = new Topsis();

            // Karar Matrisi
            TempData["KararMatrisi"] = KararMatris;

            //1.Adım : Standart Karar Matrisi
            topsis.StandartKararMatrisiHazirla(KararMatris);
            double[,] StandartKararMatris = topsis.StandartKararMatrisiGoster();
            TempData["StandartKararMatrisi"] = StandartKararMatris;

            //2.Adım : Ağırlıklı standart Karar Matrisi
            topsis.agirlikliStandartKararMatrisiHazirla(Agirliklar);
            double[,] agirlikliStandartKararMatrisi = topsis.agirlikliStandartKararMatrisiGoster();
            TempData["agirlikliStandartKararMatrisi"] = agirlikliStandartKararMatrisi;

            //3.Adım : ideal ve negatif ideal çözüm setleri
            topsis.idealVeNegatifİdealCozumDegerleriHesapla();
            double[] IdealcozumDegerler = topsis.idealCozumDegerleriGoster();
            double[] negatifİdealcozumDegerleri = topsis.negatifİdealcozumDegerleriGoster();
            TempData["IdealcozumDegerler"] = IdealcozumDegerler;
            TempData["negatifİdealcozumDegerleri"] = negatifİdealcozumDegerleri;

            //4.Adım : ideal ve negatif ideal noktalara olan uzaklık - ideal ayrım
            topsis.uzakliklarinHesabi();
            double[] idealUzaklik = topsis.idealUzaklik;
            double[] negatifİdealUzaklik = topsis.negatifİdealUzaklik;
            TempData["idealUzaklik"] = idealUzaklik;
            TempData["negatifİdealUzaklik"] = negatifİdealUzaklik;

            //5.Adım : İdeal çözüme gerekli yakınlık değeri
            topsis.idealCozumHesabi();
            double[] idealCozumSirali = topsis.idealCozumSiralaVeGoster();
            double idealCozum = topsis.idealCozum;
            TempData["idealCozumSirali"] = idealCozumSirali;
            TempData["idealCozum"] = idealCozum;

            // Secenek degerleri
            double S1 = topsis.s1;
            double S2 = topsis.s2;
            double S3 = topsis.s3;
            double S4 = topsis.s4;
            TempData["Ss1"] = S1;
            TempData["Ss2"] = S2;
            TempData["Ss3"] = S3;
            TempData["Ss4"] = S4;

            return RedirectToAction("Result");
        }

        public ActionResult Result( )
        {
            return View();
        }
    }
}