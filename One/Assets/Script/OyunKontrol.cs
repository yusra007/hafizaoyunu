using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OyunKontrol : MonoBehaviour
{

    // GENEL AYARLAR
    public int hedefbasari;
    int ilkSecimDegeri;
    int anlikbasari;
    //-----------------------
    GameObject secilenButon;
    GameObject butonunKendisi;
    //-----------------------
    public Sprite defaultSprite;
    public AudioSource[] sesler;
    public GameObject[] Butonlar;
    public TextMeshProUGUI Sayac;
    public GameObject[] OyunSonuPaneller;
    public Slider ZamanSlider;
    // SAYAÇ
    public float ToplamZaman;
    float dakika;
    float saniye;
    bool zamanlayici;
    float gecenZaman;
    //-----------------------  

    public GameObject Grid;
    public GameObject Havuz;


    bool olusturmaDurumu;
    int olusturmaSayisi;
    int toplamElemanSayisi;
    void Start()
    {
        ilkSecimDegeri = 0;
        zamanlayici = true;
        gecenZaman = 0;
        olusturmaDurumu = true;
        olusturmaSayisi = 0;
        toplamElemanSayisi = Havuz.transform.childCount;
        ZamanSlider.value = gecenZaman;
        ZamanSlider.maxValue = ToplamZaman;

        StartCoroutine(Olustur());


    }
    private void Update()
    {

        if (zamanlayici && gecenZaman != ToplamZaman)
        {

            gecenZaman += Time.deltaTime;

            ZamanSlider.value = gecenZaman;

            if (ZamanSlider.maxValue == ZamanSlider.value)
            {
                zamanlayici = false;
                GameOver();
            }

            /* dakika = Mathf.FloorToInt(ToplamZaman / 60); //  119 / 2 = 1       
             saniye = Mathf.FloorToInt(ToplamZaman % 60); // 119 / 2 = 1 *****  => 59     

             // Sayac.text = Mathf.FloorToInt(ToplamZaman).ToString();
             Sayac.text = string.Format("{0:00}:{1:00}", dakika, saniye);*/

        }

    }



    IEnumerator Olustur()
    {


        yield return new WaitForSeconds(.1f);

        while (olusturmaDurumu)
        {
            int rastgeleSayi = Random.Range(0, Havuz.transform.childCount - 1);

            if (Havuz.transform.GetChild(rastgeleSayi).gameObject != null)
            {
                Havuz.transform.GetChild(rastgeleSayi).transform.SetParent(Grid.transform);
                olusturmaSayisi++;

                if (olusturmaSayisi == toplamElemanSayisi)
                {
                    olusturmaDurumu = false;
                }
            
            }

        }
    }
    public void Oyunudurdur()
    {
        OyunSonuPaneller[2].SetActive(true);
        Time.timeScale = 0;

    }
    public void OyunaDevamEt()
    {
        OyunSonuPaneller[2].SetActive(false);
        Time.timeScale = 1;

    }
    void GameOver()
    {
        OyunSonuPaneller[0].SetActive(true);

    }
    void Win()
    {
        OyunSonuPaneller[1].SetActive(true);

    }
    public void AnaMenu()
    {
        SceneManager.LoadScene("AnaMenu");

    }

    public void TekrarOyna()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void ObjeVer(GameObject objem)
    {
        butonunKendisi = objem;

        butonunKendisi.GetComponent<Image>().sprite = butonunKendisi.GetComponentInChildren<SpriteRenderer>().sprite;
        butonunKendisi.GetComponent<Image>().raycastTarget = false;
        sesler[0].Play();
        butonunKendisi.GetComponent<AudioSource>().Play();
        
    }

    void Butonlarindurumu(bool durum)
    {
        foreach (var item in Butonlar)
        {
            if (item != null)
            {
                item.GetComponent<Image>().raycastTarget = durum;

            }
        }

    }

    public void ButonTikladi(int deger)
    {

        Kontrol(deger);

    }

    void Kontrol(int gelendeger)
    {

        if (ilkSecimDegeri == 0)
        {
            ilkSecimDegeri = gelendeger;
            secilenButon = butonunKendisi;
        }
        else
        {
            StartCoroutine(kontroletbakalim(gelendeger));
        }

    }
    IEnumerator kontroletbakalim(int gelendeger)
    {
        Butonlarindurumu(false);
        yield return new WaitForSeconds(1);

        if (ilkSecimDegeri == gelendeger)
        {
            Debug.Log("deneme");
            anlikbasari++;
            secilenButon.GetComponent<Image>().enabled = false;
            butonunKendisi.GetComponent<Image>().enabled = false;
            // secilenButon.GetComponent<Button>().enabled = false;
            // butonunKendisi.GetComponent<Button>().enabled = false;
            ilkSecimDegeri = 0;
            secilenButon = null;
            Butonlarindurumu(true);

            if (hedefbasari == anlikbasari)
            {
                Win();

            }
        }
        else
        {

            sesler[1].Play();
            secilenButon.GetComponent<Image>().sprite = defaultSprite;
            butonunKendisi.GetComponent<Image>().sprite = defaultSprite;
            ilkSecimDegeri = 0;
            secilenButon = null;
            Butonlarindurumu(true);


        }

    }
}
