using System;
using System.Collections.Generic;

namespace SistemKaryawan
{
    // Base class 
    public class Karyawan
    {
        public string Nama { get; set; }
        public decimal Gaji { get; set; }

        public Karyawan(string nama, decimal gaji)
        {
            Nama = nama;
            Gaji = gaji;
        }

        public virtual void Kerja()
        {
            Console.WriteLine($"{Nama} sedang bekerja.");
        }

        public virtual void InfoKaryawan()
        {
            Console.WriteLine($"Nama: {Nama} | Gaji: Rp{Gaji:N0}");
        }
    }

    //Tetap
    public class Tetap : Karyawan
    {
        public decimal Tunjangan { get; set; }

        public Tetap(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji)
        {
            Tunjangan = tunjangan;
        }

        public decimal HitungGajiTotal() => Gaji + Tunjangan;

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Karyawan Tetap) bekerja penuh waktu.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"   Tunjangan: Rp{Tunjangan:N0} | Gaji Total: Rp{HitungGajiTotal():N0}");
        }
    }

    // Kontrak
    public class Kontrak : Karyawan
    {
        public int Durasi { get; set; } // ini pakai bulan

        public Kontrak(string nama, decimal gaji, int durasi) : base(nama, gaji)
        {
            Durasi = durasi;
        }

        public void CekKontrak()
        {
            Console.WriteLine($"{Nama} memiliki kontrak selama {Durasi} bulan.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Karyawan Kontrak) bekerja sesuai masa kontrak.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"   Durasi Kontrak: {Durasi} bulan");
        }
    }

    // Turunan Tetap
    public class Manager : Tetap
    {
        public Manager(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji, tunjangan) { }

        public void Memimpin()
        {
            Console.WriteLine($"{Nama} memimpin rapat dan mengarahkan tim.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Manager) mengatur strategi dan mengawasi tim.");
        }
    }

    public class Staff : Tetap
    {
        public Staff(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji, tunjangan) { }

        public void KerjakanTugas()
        {
            Console.WriteLine($"{Nama} mengerjakan tugas harian dari atasan.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Staff) mengerjakan tugas operasional.");
        }
    }

    // Turunan Kontrak
    public class Magang : Kontrak
    {
        public Magang(string nama, decimal gaji, int durasi) : base(nama, gaji, durasi) { }

        public void Belajar()
        {
            Console.WriteLine($"{Nama} sedang belajar dari mentor.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Magang) membantu pekerjaan sambil belajar.");
        }
    }

    public class Freelancer : Kontrak
    {
        public Freelancer(string nama, decimal gaji, int durasi) : base(nama, gaji, durasi) { }

        public void AmbilProyek()
        {
            Console.WriteLine($"{Nama} mengambil proyek baru.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (Freelancer) mengerjakan proyek secara mandiri.");
        }
    }

    // Perusahaan
    public class Perusahaan
    {
        private List<Karyawan> daftar = new List<Karyawan>();

        public void TambahKaryawan(Karyawan karyawan)
        {
            daftar.Add(karyawan);
        }

        public void DaftarKaryawan()
        {
            foreach (Karyawan k in daftar)
            {
                k.InfoKaryawan();
            }
        }

        public List<Karyawan> Semua => daftar; // polymorphism disini kalau bingung
    }

    // Main
    class Program
    {
        static void Main(string[] args)
        {
            Perusahaan p = new Perusahaan();

            Manager manager = new Manager("Rismay", 15000000, 5000000);
            Staff staff = new Staff("Ulfa", 8000000, 1500000);
            Magang magang = new Magang("Reezo", 2000000, 6);
            Freelancer freelancer = new Freelancer("Fitri", 7000000, 3);

            p.TambahKaryawan(manager);
            p.TambahKaryawan(staff);
            p.TambahKaryawan(magang);
            p.TambahKaryawan(freelancer);

            Console.WriteLine("DAFTAR KARYAWAN");
            p.DaftarKaryawan();

            Console.WriteLine("\n 1: Kerja() Manager & Freelancer");
            manager.Kerja();
            freelancer.Kerja();

            Console.WriteLine("\n 2: Memimpin() Manager");
            manager.Memimpin();

            Console.WriteLine("\n 3: Info lengkap Manager");
            manager.InfoKaryawan();

            Console.WriteLine("\n 4: Belajar() Magang");
            magang.Belajar();

            Console.WriteLine("\n 5: Variabel Karyawan berisi Staff ");
            Karyawan k = new Staff("Andi", 8500000, 1000000);
            k.Kerja();
            // k.KerjakanTugas();  // <- Yang ini error kalau di-uncomment (jawaban soal 5)
            ((Staff)k).KerjakanTugas(); // yang ini butuh casting

            Console.WriteLine("\n POLYMORPHISM: semua karyawan Kerja() ");
            foreach (Karyawan kar in p.Semua)
            {
                kar.Kerja();
            }

            Console.WriteLine("\n METHOD KHUSUS (pakai pattern matching)");
            foreach (Karyawan kar in p.Semua)
            {
                if (kar is Manager m) m.Memimpin();
                else if (kar is Staff s) s.KerjakanTugas();
                else if (kar is Magang mg) mg.Belajar();
                else if (kar is Freelancer f) f.AmbilProyek();
            }

            Console.WriteLine("\n CekKontrak() ");
            magang.CekKontrak();
            freelancer.CekKontrak();

            Console.ReadKey();
        }
    }
}