using _223311082_MatematikOyunu;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _223311082_MatematikOyunu
{
    public partial class _223311082_MatematikOyunu : Form
    {
        int dogru = 0;
        int yanlis = 0;
        int kalan = 20; // Toplam sorular
        int zaman;
        int seviye = 1; // Başlangıç seviyesi
        int bolum = 1;
        int blokSorusayisi = 5; // Her blokta 5 soru var
        int pashakki = 1; // Her soru için bir pas hakkı var
        Timer kalanzaman = new Timer();
        Dictionary<int, bool> PasSorular = new Dictionary<int, bool>(); // Pas geçilen soruları takip eden dictionary

        Dosya_islemleri d_Islemleri = new Dosya_islemleri(); // DataProcessor sınıfı örneği

        public _223311082_MatematikOyunu(string[] args = null) // string[] türünde bir argüman alacak şekilde güncelliyoruz
        {
            InitializeComponent();
            kalanzaman.Tick += KalanZaman_Tick;
            kalanzaman.Interval = 1000;
            kalanzaman.Start();
            zaman = 15 + (bolum * 5); // Bölüme göre zaman ayarlaması



            // Eğer argümanlar varsa, onları kullanıyoruz
            if (args != null && args.Length > 1 && args[0].ToLower() == "open")
            {
                string cheatCode = args[1].ToLower(); // İkinci argüman hile kodu olarak kullanılıyor
                ApplyCheatCode(cheatCode); // Hile kodunu uygula
            }
            else
            {
                LoadGameData(); // Normal oyun verilerini yükle
            }
            random(); // İlk rastgele sorular oluşturuluyor
        }
        private void ApplyCheatCode(string cheatCode)
        {
            switch (cheatCode)
            {
                case "2":
                    seviye = 2;
                    bolum = 1;
                    MessageBox.Show("2. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "3":
                    seviye = 3;
                    bolum = 1;
                    MessageBox.Show("3. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "4":
                    seviye = 4;
                    bolum = 1;
                    MessageBox.Show("4. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "5":
                    seviye = 5;
                    bolum = 1;
                    MessageBox.Show("5. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "all":
                    seviye = 5;
                    bolum = 4;

                    MessageBox.Show("Tüm seviyelerin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                default:
                    MessageBox.Show("Geçersiz hile kodu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            // Güncel bilgileri kullanıcı arayüzüne yansıt
            lblSeviye.Text = $"Seviye= {seviye}";
            lblBolum.Text = $"Bölüm= {bolum}";
        }

        private void DisableTimer()
        {
            kalanzaman.Stop(); // Timer'ı durdur
            zaman = 0; // Zamanı sıfırla veya istediğin bir süreyi ayarla
            Zaman.Text = $"Kalan Süre: {zaman} Saniye"; // Zamanı güncelle
            kalanzaman.Interval = int.MaxValue; // Zamanlayıcıyı işlevsiz hale getir

        }

        private void LoadGameData()
        {
            var (dogru, yanlis, kalan, seviye, bolum) = d_Islemleri.LoadGameData();
            this.dogru = dogru;
            this.yanlis = yanlis;
            this.kalan = kalan;
            this.seviye = seviye;
            this.bolum = bolum;

            Dogru.Text = this.dogru.ToString();
            Yanlis.Text = this.yanlis.ToString();
            Kalan.Text = this.kalan.ToString();
            lblSeviye.Text = $"Seviye= {this.seviye}";
            lblBolum.Text = $"Bölüm= {this.bolum}";

            // Zamanı güncelle
            zaman = 20 + (seviye - 1) * 10; // Başlangıç süresi + (seviye - 1) * 10
            Zaman.Text = $"Kalan Süre: {zaman} Saniye";

            Yildiz();
        }

        private void KalanZaman_Tick(object sender, EventArgs e)
        {
            // Zamanlayıcı durdurulmuşsa, hiçbir işlem yapma
            if (zaman <= 0)
            {
                return;
            }

            zaman--;
            Zaman.Text = $"Kalan Süre: {zaman} Saniye";
            if (zaman <= 0)
            {
                kalanzaman.Stop();
                MessageBox.Show($"Süre doldu!\nDoğru sayısı: {dogru}\nYanlış sayısı: {yanlis}", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GameOver();
            }

            // Her saniye verileri kaydet
            d_Islemleri.SaveGameData(dogru, yanlis, kalan, seviye, bolum);
        }

        private void GameOver()
        {
            DialogResult result = MessageBox.Show("Tekrar oynamak ister misiniz?", "Oyun Bitti", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                Application.Exit();
            }
        }
        private void ResetGame()
        {
            seviye = 1;
            dogru = 0;
            yanlis = 0;
            PasSorular.Clear();
            kalan = 20; // Toplam soru sayısı
            zaman = 20; // Başlangıç zamanı
            kalanzaman.Start();
            random(); // Yeni sorular oluştur
        }
        public void random()
        {
            // Her yeni sayfa açıldığında PasSorular sıfırlanır.
            PasSorular.Clear();
            Random rnd = new Random();
            char[] sembol = { '+', '-', '*', '/' };

            int maxSayi = 10; // Başlangıçta maksimum sayı 100
            int minSayi = 0;
            // Seviyeye göre maksimum sayıyı ayarlıyoruz
            if (seviye == 2)
            {
                minSayi = 10;
                maxSayi = 30; 
            }
            else if (seviye == 3)
            {
                minSayi = 30;
                maxSayi = 50; 
            }
            else if (seviye == 4)
            {
                minSayi = 50;
                maxSayi = 100; 
            }
            else if (seviye == 5)
            {
                minSayi = 100;
                maxSayi = 200;
            }

            for (int i = 1; i <= blokSorusayisi; i++)
            {
                TextBox Sonuclar = (TextBox)this.Controls[$"Sonuc{i}"];
                Sonuclar.Clear();
                Sonuclar.Enabled = true; // Yeni sorular aktif hale getirilir

                int s1 = rnd.Next(minSayi, maxSayi); // Seviye bazlı maksimum sayı aralığı
                int s2 = rnd.Next(minSayi + 1, maxSayi); // 0'dan büyük olması için 1 ile başlıyoruz

                int islemIndex = rnd.Next(0, sembol.Length);
                char islemS = sembol[islemIndex];

                // Bölme işlemi için, sıfır bölünmemesi için s2'yi 1 ile maxSayi arasında alıyoruz.
                if (islemS == '/')
                {
                    // s1'in s2'ye tam bölünebilmesi için, s1'i s2 ile çarpıyoruz.
                    s1 = s2 * rnd.Next(1, maxSayi / s2);
                }

                Label degisken1 = (Label)this.Controls[$"degisken{i}"];
                Label degisken1_1 = (Label)this.Controls[$"degisken{i}_{i}"];
                Label islemLabel = (Label)this.Controls[$"islem{i}"];

                degisken1.Text = s1.ToString();
                degisken1_1.Text = s2.ToString();
                islemLabel.Text = islemS.ToString();
            }

        }

        public int Hesapla(string s1Str, string s2Str, string islemStr)
        {
            int s1 = int.Parse(s1Str);
            int s2 = int.Parse(s2Str);
            char islem = islemStr[0];

            int sonuc = 0;
            switch (islem)
            {
                case '+':
                    sonuc = s1 + s2;
                    break;
                case '-':
                    sonuc = s1 - s2;
                    break;
                case '*':
                    sonuc = s1 * s2;
                    break;
                case '/':
                    sonuc = s2 != 0 ? s1 / s2 : 0;
                    break;
            }
            return sonuc;
        }
        public void Pasİslemleri(int i, TextBox cevapTextBox)
        {
            if (!PasSorular.ContainsKey(i))
            {
                PasSorular[i] = true; // Soruyu pas olarak işaretle
                MessageBox.Show($"{i}. soruyu pas geçtiniz. Bu soruya tekrar döneceksiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cevapTextBox.Enabled = true; // Pas geçilen sorunun aktif kalması için
            }
            else
            {
                if (PasSorular[i]) // Pas hakkı bitmişse
                {
                    yanlis++; // Pas hakkı bittiğinde yanlış kabul edilir
                    MessageBox.Show($"{i}. soruyu pas geçtiniz ve yanlış kabul edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PasSorular[i] = false; // Yanlış olarak kabul edilen pas sorusu çözülmüş sayılır
                    cevapTextBox.Enabled = false; // Yanlış olarak işaretlenen soru artık pasif olur
                }
                else
                {
                    MessageBox.Show($"{i}. soruyu zaten pas geçtiniz ve yanlış kabul edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void SonrakiSeviye()
        {
            if (PasSorular.ContainsValue(true))
            {
                TekrarSor(); // Pas geçilen sorular yeniden soruluyor
            }
            else
            {
                // Seviye arttırılıyor
                if (seviye <= 5)
                {
                    if (bolum < 4)
                    {
                        bolum++;
                        zaman += 10; // Yeni seviyede 10 saniye ekleniyor
                        lblSeviye.Text = $"Seviye= {seviye}";
                        lblBolum.Text = $"Bölüm= {bolum}";
                        MessageBox.Show($"Tebrikler! {seviye}.Seviye {bolum}.Bölüm'e geçtiniz!", "Seviye Atladı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        random(); // Yeni seviyedeki sorular oluşturuluyor
                        zaman = 20 + (seviye - 1) * 10; // Bölüme göre zaman ayarlaması
                    }
                    else
                    {
                        if (dogru >= 11)
                        {
                            zaman += 10; // Yeni seviyede 10 saniye ekleniyor
                            kalan = 20;
                            dogru = 0;
                            yanlis = 0;
                            seviye++;
                            bolum = 0;
                        }
                        else
                        {
                            MessageBox.Show($"Yeterli Sayıda Doğrunuz Yok", "Seviye Tekrarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            kalan = 20;
                            dogru = 0;
                            yanlis = 0;
                            bolum = 0;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Oyun bitti! Tebrikler tüm seviyeleri tamamladınız!", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GameOver(); // Oyun bittiğinde
                }
            }
        }

            private void TekrarSor()
        {
            foreach (var soruNo in PasSorular.Keys)
            {
                if (PasSorular[soruNo] && pashakki > 0) // Sadece pas geçilen sorular ve pas hakkı bitmemişse tekrar sorulur
                {
                    TextBox cevapTextBox = (TextBox)this.Controls[$"Sonuc{soruNo}"];
                    cevapTextBox.Clear(); // Cevapları temizle
                    cevapTextBox.Enabled = true; // Pas geçilen sorular aktif hale getiriliyor
                }
            }
        }
        public void Yildiz()
        {
            if (dogru >= 11 && dogru <= 15)
            {
                YildizGoster.Text = "Yıldız= *";
            }
            else if (dogru >= 16 && dogru <= 18)
            {
                YildizGoster.Text = "Yıldız= **";
            }
            else if (dogru >= 19 && dogru <= 20)
            {
                YildizGoster.Text = "Yıldız= ***";
            }
        }


        private void Text_Et_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= blokSorusayisi; i++)
            {
                Label degisken1 = (Label)this.Controls[$"degisken{i}"];
                Label degisken1_1 = (Label)this.Controls[$"degisken{i}_{i}"];
                Label islemLabel = (Label)this.Controls[$"islem{i}"];
                TextBox cevapTextBox = (TextBox)this.Controls[$"Sonuc{i}"];

                if (cevapTextBox == null)
                {
                    MessageBox.Show($"Cevap için TextBox {i} bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                // Eğer soru daha önce cevaplandıysa, bu soruyu atla
                if (PasSorular.ContainsKey(i) && !PasSorular[i])
                {
                    continue; // Zaten cevaplanmış bir soru, tekrar kontrol edilmez
                }

                if (int.TryParse(cevapTextBox.Text, out int kullaniciCevabi))
                {
                    int sonuc = Hesapla(degisken1.Text, degisken1_1.Text, islemLabel.Text);

                    if (kullaniciCevabi == sonuc)
                    {
                        dogru++;
                        kalan--;
                        PasSorular[i] = false; // Soru doğru çözülmüş, pas değil
                    }
                    else
                    {
                        yanlis++;
                        kalan--;
                        PasSorular[i] = false; // Soru yanlış çözülmüş, pas değil
                    }

                    // Doğru veya yanlış olsa bile bu TextBox'ı pasif yapıyoruz
                    cevapTextBox.Enabled = false;
                }
                else
                {
                    // Cevaplanmayan sorular varsa pas işlemi kontrolü
                    Pasİslemleri(i, cevapTextBox);
                }
            }

            // Doğru ve yanlış sayısını güncelle
            Dogru.Text = dogru.ToString();
            Yanlis.Text = yanlis.ToString();
            Kalan.Text = kalan.ToString();

            // Yıldız sayısını güncelle
            Yildiz();

            // Seviye kontrolü
            SonrakiSeviye();
        }


        private void _223311082_MatematikOyunu_Load(object sender, EventArgs e)
        {

        }
    }
}
