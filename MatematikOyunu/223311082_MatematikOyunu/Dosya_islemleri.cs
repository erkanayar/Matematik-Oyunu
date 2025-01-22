using System;
using System.IO;

namespace _223311082_MatematikOyunu
{
    public class Dosya_islemleri
    {
        private const string filePath = @"C:\Users\Erkann\OneDrive\Masaüstü\223311082_MatematikOyunu\223311082_MatematikOyunu\kayit.txt";// Dosya yolu


        // Oyun verilerini kaydet
        public void SaveGameData(int dogru, int yanlis, int kalan, int seviye, int bolum)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(dogru);
                writer.WriteLine(yanlis);
                writer.WriteLine(kalan);
                writer.WriteLine(seviye);
                writer.WriteLine(bolum);
            }
        }

        // Oyun verilerini yükle
        public (int, int, int, int, int) LoadGameData()
        {
            if (!File.Exists(filePath))
            {
                // Dosya yoksa varsayılan değerleri döndür
                return (0, 0, 20, 1, 1);
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 5)
            {
                // Eksik veri varsa varsayılan değerleri döndür
                return (0, 0, 20, 1, 1);
            }

            int dogru = int.Parse(lines[0]);
            int yanlis = int.Parse(lines[1]);
            int kalan = int.Parse(lines[2]);
            int seviye = int.Parse(lines[3]);
            int bolum = int.Parse(lines[4]);

            return (dogru, yanlis, kalan, seviye, bolum);
        }
    }
}
