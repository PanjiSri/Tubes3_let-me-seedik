using System;
using System.Collections.Generic;
using System.IO;

namespace Tubes3_let_me_seedik
{
    class BackEnd
    {
        public float persentase { get; private set; }
        public string[] biodata { get; private set; }
        public string pathGambar { get; private set; }

        public BackEnd(bool algoritma) 
        {
            pathGambar = "";
            // string folderPath = "./tes";
            string targetPath = "./src/citra";
            // List<string> filePaths = FileManager.GetFilePaths(folderPath);
            List<string> targetPaths = FileManager.GetFilePaths(targetPath);
            string input = targetPaths[0];

            DatabaseManager dbManager = new DatabaseManager("Server=127.0.0.1; User=root; Password=; Database=sidik");
            string[] allPath = dbManager.GetAllPath();
            // string[] allName = dbManager.GetAllName();
            // pathGambar = Path.Combine(Path.GetFullPath("../../test"), "100__M_Left_index_finger.bmp");
            int position;
            float maxPersen = 0;
            string real_name = "";
            GrayscaleToASCIIConverter asciiConverter = new GrayscaleToASCIIConverter();        

            foreach(var path in allPath){
                string wholeImage = asciiConverter.ConvertToASCII(path);
                string bestSegment = asciiConverter.GetBestSegment(input);
                string wholeImageInput = asciiConverter.ConvertToASCII(input);

                if (algoritma)
                {
                    position = KMP.KmpMatch(wholeImage, bestSegment);
                }
                else
                {
                    position = BM.BmMatch(wholeImage, bestSegment);
                }

                if (position != -1)
                {
                    // Console.WriteLine($"Pattern starts at position {position + 1}");

                    //PATH ITU ISINYA PATH KE GAMBARNYA
                    real_name = dbManager.GetName(path);
                    pathGambar = path;
                    maxPersen = 100;
                    break;
                }
                else
                {
                    int distance = Levenshtein.LevenshteinDistance(wholeImage, wholeImageInput);

                    float persent = Levenshtein.PersentaseKemiripan(distance, wholeImage, wholeImageInput);

                    if (maxPersen < persent)
                    {
                        real_name = dbManager.GetName(path);
                        pathGambar = path;
                        maxPersen = persent;
                    }
                }
            }  
            persentase = maxPersen;
            string[] allName = dbManager.GetAllName();

            Regexp r = new Regexp();
            string name_regex = r.createRegex(real_name);
            string alay_name = "";
            foreach (var name in allName) {
                if (r.isMatch(name, name_regex)) {
                    alay_name = name;
                    break;
                }
            }
        
            if (alay_name == "") {
                Console.WriteLine("nama tidak ada");
            }

            //INI REAL_BIODATA BUAT BIODATANYA
            biodata = dbManager.GetBiodata(alay_name);
            biodata[1] = real_name;
            Console.WriteLine(biodata[1]);
            // string[] real_biodata = dbManager.GetBiodata(alay_name);
            // String pattern = "Ini siapa";
            // String text = "Ini siapa sih";
            // if (algoritma == true)
            // {
            //     start = BM.BmMatch(text, pattern);
            // }
            // else
            // {
            //     start = KMP.KmpMatch(text, pattern);
            // }

            // if (start != -1)
            // {
            //     kemiripan = Levenshtein.LevenshteinDistance(text, pattern);
            // }
            // // Console.WriteLine(Levenshtein.PersentaseKemiripan(kemiripan, text, pattern));
            // persentase = Levenshtein.PersentaseKemiripan(kemiripan, text, pattern);
        }

        public void Penyelesaian(String gambarDatabase, String gambarUji, bool algoritma)
        {
            int start;
            int kemiripan = 100;
            String pattern = "Ini siapa";
            String text = "Ini siapa sih";
            if (algoritma == true)
            {
                start = BM.BmMatch(text, pattern);
            }
            else
            {
                start = KMP.KmpMatch(text, pattern);
            }

            if (start != -1)
            {
                kemiripan = Levenshtein.LevenshteinDistance(text, pattern);
            }
            // Console.WriteLine(Levenshtein.PersentaseKemiripan(kemiripan, text, pattern));
            persentase = Levenshtein.PersentaseKemiripan(kemiripan, text, pattern);

        }
        public static int Prosedur(){
            int pilihan = 0;
            int start;
            int kemiripan = 100;
            String pattern = "Ini siapa";
            String text = "Ini siapa sih";
            if (pilihan == 0) {
                start = BM.BmMatch(text, pattern);
            } else {
                start = KMP.KmpMatch(text, pattern);
            }

            if (start != -1)
            {
                kemiripan = Levenshtein.LevenshteinDistance(text, pattern);
            }
            // Console.WriteLine(Levenshtein.PersentaseKemiripan(kemiripan, text, pattern));
            return (int) Levenshtein.PersentaseKemiripan(kemiripan, text, pattern);
        }
    }

}